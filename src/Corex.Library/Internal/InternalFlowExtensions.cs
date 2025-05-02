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
using Corex.Library;

// ReSharper disable CheckNamespace
internal static class InternalFlowExtensions
// ReSharper restore CheckNamespace
{
    public static T FlowMustBeNotNull<T>(this T obj) => XHelper.Flow.FlowMustBeNotNull(obj);

    public static Context IfIs<T>(this object value, Action<T> isAction) where T : class => new Context(XHelper.Flow.IfIs(value, isAction), value);

    public static TResult IfIsGet<T, TResult>(this object value, Func<T, TResult> getAction, TResult defaultValue) where T : class => XHelper.Flow.IfIsGet(value, getAction, defaultValue);

    public static Context IfIsNot<T>(this object value, Action isAction) where T : class => new Context(XHelper.Flow.IfIsNot<T>(value, isAction), value);

    public static Context OrIfIs<T>(this Context ctx, Action<T> isAction) where T : class
    {
        if (ctx.Stop)
        {
            return ctx;
        }

        ctx.Stop = XHelper.Flow.IfIs(ctx.Value, isAction);

        return ctx;
    }

    public static Context OrIfIsNot<T>(this Context ctx, Action isAction) where T : class
    {
        if (ctx.Stop)
        {
            return ctx;
        }

        ctx.Stop = XHelper.Flow.IfIsNot<T>(ctx.Value, isAction);

        return ctx;
    }

    public struct Context
    {
        public Context(bool stop, object value)
        {
            Stop = stop;
            Value = value;
        }

        public bool Stop { get; internal set; }

        public object Value { get; internal set; }
    }
}