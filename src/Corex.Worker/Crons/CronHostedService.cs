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
using Cronos;
using Corex.Worker.Base;
using Microsoft.Extensions.Logging;
using Timer = System.Timers.Timer;

namespace Corex.Worker.Crons
{
    public abstract class CronHostedService : HostedServiceBase
    {
        private readonly string _cronExpression;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;

        private Timer _timer;

        protected CronHostedService(string cronExpression, TimeZoneInfo timeZoneInfo, ILoggerFactory loggerFactory)
        {
            _cronExpression = cronExpression;

            _expression = CronExpression.Parse(cronExpression);
            _timeZoneInfo = timeZoneInfo;

            Logger = loggerFactory.CreateLogger($"{GetType().Name}#{Name}");
        }

        protected override void InternalDisposing(in bool disposing) => _timer?.Dispose();

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Service starting (Cron: {_cronExpression})");

            await ScheduleJob(cancellationToken);
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();

            await Task.CompletedTask;
        }

        public abstract Task<Result> DoWork(CancellationToken cancellationToken);

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;

                _timer = new Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                    {
                        _timer.Dispose();
                        _timer = null;

                        while (!cancellationToken.IsCancellationRequested)
                        {
                            var result = await DoWork(cancellationToken);
                            if (result.Success || result.RetryTimeSpan == null)
                            {
                                break;
                            }
                        }

                        if (!cancellationToken.IsCancellationRequested)
                        {
                            await ScheduleJob(cancellationToken);
                        }
                    };

                _timer.Start();

                Logger.LogWarning($"Next occurrence at {next.Value:dd/MM/yyyy hh:mm:ss}");
            }

            await Task.CompletedTask;
        }

        public class Result
        {
            public TimeSpan? RetryTimeSpan { get; set; }
            public bool Success { get; set; }
        }
    }
}