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

// ReSharper disable CheckNamespace
public static class XLazyExtensions
// ReSharper restore CheckNamespace
{
    public static Lazy<TReturn> AsLazy<TReturn>(this Func<TReturn> func)
    {
        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        return new Lazy<TReturn>(func);
    }

    public static Lazy<TReturn> AsLazy<TParam, TReturn>(this Func<TParam, TReturn> func, TParam value)
    {
        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
        }

        return new Lazy<TReturn>(() => func(value));
    }

    public static TReturn ValueIfCreated<TReturn>(this Lazy<TReturn> lazy) where TReturn : class => lazy.IsValueCreated ? lazy.Value : null;
}