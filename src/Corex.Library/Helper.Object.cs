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
using System.Linq;
using Corex.Exceptions.Assert;

namespace Corex.Library;

public partial class XHelper
{
    public static class Obj
    {
        public static bool AnyNull(params object[] args) => args.Any(o => o == null);

        public static bool AreEquals(object objA, object objB) => Equals(objA, objB);

        public static bool AreNotEquals(object objA, object objB) => !AreEquals(objA, objB);

        public static object[] AsArray(params object[] args) => args;

        public static void DoNothing(object data) { }

        public static TResult GetOrDefault<T, TResult>(T obj, Func<T, TResult> func, TResult defaultValue) where T : class
        {
            XContract.ArgIsNotNull(func, nameof(func));

            if (obj == null)
            {
                return defaultValue;
            }

            return func(obj);
        }

        public static TResult GetOrDefault<T, TResult>(T obj, Func<T, TResult> func) where T : class => GetOrDefault(obj, func, default);

        public static bool IsDbNull(object @object) => @object == DBNull.Value;

        public static bool IsNotNull(object @object) => !IsNull(@object);

        public static bool IsNull(object @object) => @object is null or DBNull;
    }
}