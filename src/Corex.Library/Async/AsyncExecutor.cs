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

namespace Corex.Library.Async;

public class AsyncExecutor<TData>
{
    private readonly Func<TData> _execute;

    private readonly object _sync = new object();

    private Task _task;
    private TData _value;

    public AsyncExecutor(Func<TData> execute, bool autoBeginInvoke = true)
    {
        _execute = execute;

        if (autoBeginInvoke)
        {
            BeginInvoke();
        }
    }

    public bool BeginInvoke()
    {
        lock (_sync)
        {
            if (_task != null)
            {
                return false;
            }

            _task = Task.Run(
                () =>
                    {
                        _value = _execute();

                        _task = null;
                    });

            return true;
        }
    }

    public TData ReadValue(int milisecondsTimeout = int.MaxValue)
    {
        lock (_sync)
        {
            _task?.Wait(milisecondsTimeout);

            return _value;
        }
    }
}