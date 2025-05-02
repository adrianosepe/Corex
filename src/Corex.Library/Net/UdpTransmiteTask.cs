#region License

// Copyright(c) 2023 GrappTec
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Corex.Library.Tasks;
using Corex.Library.Tasks.Threading;

namespace Corex.Library.Net;

public class UdpTransmiteTask : XTask
{
    private readonly BlockingCollection<Item> _data;
    private readonly IDestinationProvider _destinationProvider;

    private readonly Socket _socket;

    private CancellationTokenSource _cancellationTokenSource;

    public UdpTransmiteTask(IDestinationProvider destinationProvider)
    {
        _destinationProvider = destinationProvider;

        _cancellationTokenSource = new CancellationTokenSource();

        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        _data = new BlockingCollection<Item>();

        AutoCatchException = false;
    }

    public void Add(byte[] datagrama)
    {
        if (IsStopping)
        {
            return;
        }

        _data.Add(new Item {Datagrama = datagrama});
    }

    protected override void InternalExecute()
    {
        var item = _data.Take(_cancellationTokenSource.Token);

        foreach (var ep in _destinationProvider.Get())
        {
            try
            {
                _socket.SendTo(item.Datagrama, ep);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }

    protected override TimeSpan? InternalGetFrequency() => null;

    protected override void InternalWaitComplete(ref bool waitComplete, XTimeout tOut)
    {
        base.InternalWaitComplete(ref waitComplete, tOut);

        if (waitComplete && tOut.Keep())
        {
            while (_data.Count > 0 && tOut.Keep())
            {
                Thread.Sleep(100);
            }
        }

        waitComplete = false;

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    private class Item
    {
        public byte[] Datagrama { get; set; }
    }

    public interface IDestinationProvider
    {
        IEnumerable<EndPoint> Get();
    }
}