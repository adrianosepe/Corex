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
using Corex.Exceptions.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Corex.Worker.Base
{
    /// <summary>
    ///     Base class for implementing a long running <see cref="IHostedService" />.
    /// </summary>
    public abstract class HostedServiceBase : IHostedService, IDisposable
    {
        private bool _disposed;

        private Task _executingTask;
        private CancellationTokenSource _stoppingCts;

        // ReSharper disable EmptyConstructor
        protected HostedServiceBase() { }
        // ReSharper restore EmptyConstructor

        public bool Enabled { get; private set; }

        public bool IsInitialized { get; private set; }

        public string Name { get; private set; }

        protected ILogger Logger { get; set; }

        /// <summary>
        ///     Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            if (Enabled)
            {
                return Task.CompletedTask;
            }

            Enabled = true;

            // Create linked token to allow cancelling executing task from provided token
            _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Logger.LogInformation("Service is started.");

            // Store the task we're executing
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            // If the task is completed then return it, this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            if (!Enabled)
            {
                return;
            }

            Enabled = false;

            Logger.LogInformation("Service is stoping.");

            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(
                    _executingTask.ContinueWith(InternalCleanup, cancellationToken), Task.Delay(Timeout.Infinite, cancellationToken));
            }

            Logger.LogInformation("Service is stoped.");
        }

        /// <summary>
        ///     This method is called when the <see cref="IHostedService" /> starts. The implementation should return a task that
        ///     represents
        ///     the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="cancellationToken">Triggered when <see cref="IHostedService.StopAsync(CancellationToken)" /> is called.</param>
        /// <returns>A <see cref="Task" /> that represents the long running operations.</returns>
        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);

        protected virtual Task InternalCleanup(Task obj) => Task.CompletedTask;

        protected virtual void InternalInitializeFromSettingsSection(IConfigurationSection settingSection) { }

        internal void Initialize(string name, ILoggerFactory loggerFactory, IConfigurationSection settingSection)
        {
            if (IsInitialized)
            {
                XInitializeException.Reinitialize(this);
            }

            IsInitialized = true;

            Name = name;
            Logger = loggerFactory.CreateLogger($"#{Name}");

            if (settingSection != null)
            {
                InternalInitializeFromSettingsSection(settingSection);
            }
        }

        #region Dispose Pattern

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            _stoppingCts?.Cancel();

            if (disposing)
            {
                GC.SuppressFinalize(this);
            }

            InternalDisposing(disposing);
        }

        protected virtual void InternalDisposing(in bool disposing) { }

        ~HostedServiceBase()
        {
            Dispose(false);
        }

        #endregion
    }
}