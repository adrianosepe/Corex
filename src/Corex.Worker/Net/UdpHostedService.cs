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

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Corex.Worker.Base;
using Microsoft.Extensions.Configuration;

namespace Corex.Worker.Net
{
    public abstract class UdpHostedService : HostedServiceBase
    {
        public IPAddress Address { get; set; }

        public int Port { get; private set; }

        protected abstract void DoWork(IPEndPoint endPoint, byte[] buffer);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var client = new UdpClient(new IPEndPoint(Address, Port));

            while (!cancellationToken.IsCancellationRequested)
            {
                if (client.Available == 0)
                {
                    await Task.Delay(100, cancellationToken);

                    continue;
                }

                var endPoint = new IPEndPoint(IPAddress.Any, 0);
                var buffer = client.Receive(ref endPoint);

                if (buffer.Length == 0)
                {
                    continue;
                }

                DoWork(endPoint, buffer);
            }
        }

        protected override void InternalInitializeFromSettingsSection(IConfigurationSection settingSection)
        {
            base.InternalInitializeFromSettingsSection(settingSection);

            Address = IPAddress.Parse(settingSection.GetSection("EndPoint:Address").Value);
            Port = settingSection.GetSection("EndPoint:Port").Get<int>();
        }
    }
}