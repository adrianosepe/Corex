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
public static class XArrayExtensions
// ReSharper restore CheckNamespace
{
    public static void ForEach<T>(this T[] array, Action<T, int> action)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        for (var i = 0; i < array.Length; i++)
        {
            action(array[i], i);
        }
    }

    public static bool IsEmpty<T>(this T[] array) => XHelper.Arrays.IsEmpty(array);

    public static bool IsNullOrEmpty<T>(this T[] array) => XHelper.Arrays.IsNullOrEmpty(array);

    public static bool IsThere<T>(this T[] array, int index) => array != null && index >= 0 && index <= array.Length - 1;

    public static bool TryGet<T>(this T[] array, int index, out T item)
    {
        if (!array.IsThere(index))
        {
            item = default;
            return false;
        }

        item = array[index];
        return true;
    }
}