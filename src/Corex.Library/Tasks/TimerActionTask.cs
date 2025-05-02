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
using System.Threading.Tasks;
using System.Timers;
using Corex.Exceptions.Assert;

// ReSharper disable UnusedMember.Global

namespace Corex.Library.Tasks;

public class TimerActionTask : ITask
{
    private readonly Timer _timer;

    private bool _disposed;

    public TimerActionTask(TimeSpan normalFrequence) : this(normalFrequence, normalFrequence, normalFrequence) { }

    public TimerActionTask(TimeSpan start, TimeSpan normalFrequence, TimeSpan errorFrequence)
    {
        NormalFrequence = normalFrequence;
        ErrorFrequence = errorFrequence;

        _timer = new Timer(start.TotalMilliseconds) {AutoReset = false};
    }

    public TimeSpan ErrorFrequence { get; set; }

    public TimeSpan NormalFrequence { get; set; }

    public bool AutoCatchException { get; set; }

    public bool Enabled => _timer.Enabled;

    public bool Start()
    {
        if (!_timer.Enabled)
        {
            _timer.Start();

            return true;
        }

        return false;
    }

    public async Task<bool> Stop(TimeSpan? timeout = null, bool waitComplete = false)
    {
        if (_timer.Enabled)
        {
            _timer.Stop();

            return await Task.Run(() => true);
        }

        return await Task.Run(() => false);
    }

    public event EventHandler<Exception> ExceptionHandled;

    public TimerActionTask Binding(Action action)
    {
        XContract.ArgIsNotNull(action, nameof(action));

        var localAction = action;

        _timer.Elapsed +=
            (sender, args) =>
                {
                    var restart = true;
                    try
                    {
                        localAction();

                        _timer.Interval = NormalFrequence.TotalMilliseconds;
                    }
                    catch (Exception ex)
                    {
                        XDebug.OnException(ex);

                        if (!AutoCatchException)
                        {
                            restart = false;

                            throw;
                        }

                        ExceptionHandled?.Invoke(this, ex);

                        _timer.Interval = ErrorFrequence.TotalMilliseconds;
                    }
                    finally
                    {
                        if (restart)
                        {
                            _timer.Start();
                        }
                    }
                };

        return this;
    }

    public static TimerActionTask New(TimeSpan start, TimeSpan normalFrequence, TimeSpan errorFrequence)
    {
        var newAutoSync = new TimerActionTask(start, normalFrequence, errorFrequence);

        return newAutoSync;
    }

    public void Reset()
    {
        if (!Enabled)
        {
            return;
        }

        _timer.Stop();
        _timer.Interval = NormalFrequence.TotalMilliseconds;
        _timer.Start();
    }

    #region IDisposable

    ~TimerActionTask()
    {
        Dispose(false);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        if (disposing)
        {
            GC.SuppressFinalize(this);
        }

        _timer.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
    }

    #endregion
}