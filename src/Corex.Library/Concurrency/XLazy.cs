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

// ReSharper disable UnusedMember.Global

namespace Corex.Library.Concurrency;

public class XLazy<T>
{
    private readonly Action<T> _onCreated;
    private readonly Action<T> _releaseAction;
    private readonly object _sync = new object();

    private readonly Func<T> _valueFactory;

    private T _value;

    public XLazy(Func<T> valueFactory, Action<T> releaseAction = null, Action<T> onCreated = null)
    {
        _valueFactory = valueFactory;
        _releaseAction = releaseAction;
        _onCreated = onCreated;
    }

    public bool IsValueCreated { get; private set; }

    public virtual T Value
    {
        get
        {
            lock (_sync)
            {
                CheckToInvalidate();

                if (!IsValueCreated)
                {
                    IsValueCreated = true;
                    _value = _valueFactory();

                    _onCreated?.Invoke(_value);
                }

                return _value;
            }
        }

        protected set
        {
            lock (_sync)
            {
                if (!IsValueCreated)
                {
                    IsValueCreated = true;
                    _value = value;

                    _onCreated?.Invoke(_value);
                }
            }
        }
    }

    public T ValueIfCreated
    {
        get
        {
            lock (_sync)
            {
                return _value;
            }
        }
    }

    public void Execute()
    {
        // ReSharper disable UnusedVariable
        var obj = Value;
        // ReSharper restore UnusedVariable
    }

    public void ReCreate()
    {
        Reset();

        Execute();
    }

    public void Reset()
    {
        T lValue;
        lock (_sync)
        {
            if (!IsValueCreated)
            {
                return;
            }

            lValue = _value;

            IsValueCreated = false;
            _value = default;
        }

        _releaseAction?.Invoke(lValue);
    }

    protected virtual void CheckToInvalidate() { }
}