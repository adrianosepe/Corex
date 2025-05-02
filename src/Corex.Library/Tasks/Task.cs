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
using Corex.Library.Tasks.Threading;

namespace Corex.Library.Tasks;

public abstract class XTask : ITask
{
    private readonly object _syncRunning = new object();

    private readonly ManualResetEvent _waitStop;

    private bool _active;

    private bool _disposed;
    private bool _waitComplete;

    protected XTask()
    {
        _waitStop = new ManualResetEvent(false);
    }

    public TimeSpan Frequency => InternalGetFrequency() ?? TimeSpan.Zero;

    public bool IsContinuousTask => InternalGetFrequency() == null;

    // ReSharper disable UnusedMember.Global
    public bool IsStopped => !Enabled;
    // ReSharper restore UnusedMember.Global

    public bool IsStopping { get; private set; }

    public string Name { get; private set; }

    public bool AutoCatchException { get; set; } = true;

    public bool Enabled
    {
        get
        {
            lock (_syncRunning)
            {
                return _active;
            }
        }
    }

    public bool Start()
    {
        InternalCheckIfCanRunTask();

        lock (_syncRunning)
        {
            if (!_active)
            {
                _active = true;
                _waitStop.Reset();

                InternalRunTask();

                return true;
            }

            return false;
        }
    }

    public async Task<bool> Stop(TimeSpan? timeout = null, bool waitComplete = false)
    {
        var tOut = new XTimeout(timeout ?? TimeSpan.Zero);

        lock (_syncRunning)
        {
            if (!_active || IsStopping)
            {
                return false;
            }

            _waitComplete = waitComplete;
            IsStopping = true;
        }

        return await Task.Run(() =>
            {
                InternalWaitComplete(ref _waitComplete, tOut);

                return tOut.WaitOne(_waitStop);
            });
    }

    public void Configure(string workerName)
    {
        if (Enabled)
        {
            throw new Exception("Can't configure this task when it is running.");
        }

        Name = workerName ?? throw new ArgumentNullException(nameof(workerName));
    }

    protected virtual void InternalCheckIfCanRunTask() { }

    protected abstract void InternalExecute();

    protected virtual TimeSpan? InternalGetFrequency() => null;

    protected void InternalRunTask()
    {
        Task.Run(() =>
            {
                Thread.CurrentThread.Name = Name;

                bool keepExecuting;
                do
                {
                    keepExecuting = true;
                    try
                    {
                        InternalExecute();

                        if (IsStopping && !_waitComplete)
                        {
                            StopThisExecution();
                        }
                    }
                    catch (TaskStopExecutionException)
                    {
                        keepExecuting = false;
                    }
                    catch (OperationCanceledException)
                    {
                        keepExecuting = false;
                    }
                    catch (Exception)
                    {
                        if (!AutoCatchException)
                        {
                            throw;
                        }
                    }

                    var frequency = InternalGetFrequency();
                    if (frequency == null)
                    {
                        continue;
                    }

                    var respectFrequency = keepExecuting && !_waitComplete;
                    if (!respectFrequency)
                    {
                        continue;
                    }

                    var waitForSeconds = Math.Max((int) frequency.Value.TotalSeconds, 1);
                    for (var i = 0; i < waitForSeconds; i++)
                    {
                        Thread.Sleep(1000);

                        if (IsStopping && !_waitComplete)
                        {
                            break;
                        }
                    }
                } while (keepExecuting);

                lock (_syncRunning)
                {
                    _active = false;
                    IsStopping = false;
                    _waitComplete = false;

                    _waitStop.Set();
                }
            });
    }

    protected virtual void InternalWaitComplete(ref bool waitComplete, XTimeout tOut) { }

    protected void StopThisExecution()
    {
        throw new TaskStopExecutionException();
    }

    private class TaskStopExecutionException : Exception { }

    #region IDisposable

    ~XTask()
    {
        Dispose(false);
    }

    private async void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        if (disposing)
        {
            GC.SuppressFinalize(this);

            _waitStop.Dispose();
        }

        await Stop();
    }

    public void Dispose()
    {
        Dispose(true);
    }

    #endregion
}