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

using System.Collections.Concurrent;
using System.IO.Ports;

namespace Corex.Library.Serial;

public class XSerialProviderPool
{
    public static readonly XSerialProviderPool Instance = new XSerialProviderPool();

    private readonly ConcurrentDictionary<string, SerialProvider> _data;

    protected XSerialProviderPool()
    {
        _data = new ConcurrentDictionary<string, SerialProvider>();
    }

    public SerialProvider Get(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits)
    {
        return _data.GetOrAdd(
            CreateKey(portName, baudRate, dataBits, parity, stopBits),
            lKey =>
                {
                    var provider = new SerialProvider();

                    provider.Configure(portName, baudRate, dataBits, parity, stopBits);
                    provider.Disposed += (sender, args) => _data.TryRemove(lKey, out _);

                    return provider;
                });
    }

    private static string CreateKey(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits) => $"{portName}.{baudRate}.{dataBits}.{parity}.{stopBits}";
}