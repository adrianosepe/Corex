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
using System.Threading;
using System.Threading.Tasks;
using Corex.Worker.Base;

namespace Corex.Worker.Timed
{
    public abstract class TimedHostedService : HostedServiceBase
    {
        private Timer _timer;

        public TimeSpan BeginOn { get; } = TimeSpan.Zero;

        public abstract TimeSpan Period { get; }

        protected abstract void DoWork(object state);

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, BeginOn, Period);

            return Task.CompletedTask;
        }

        protected override Task InternalCleanup(Task obj)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return base.InternalCleanup(obj);
        }

        protected override void InternalDisposing(in bool disposing)
        {
            base.InternalDisposing(in disposing);

            _timer?.Dispose();
        }
    }
}