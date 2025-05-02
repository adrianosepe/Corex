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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Corex.Library;

// ReSharper disable CheckNamespace
public static class XEnumerableExtensions
// ReSharper restore CheckNamespace
{
    public static IEnumerable<TTarget> ConvertTo<TSource, TTarget>(this IEnumerable<TSource> source, Func<TSource, TTarget> funcConvert) => source.Select(funcConvert);

    public static bool FirstOrDefault<T>(this IEnumerable<T> values, out T value) => XHelper.Enumerable.FirstOrDefault(values, out value);

    public static void ForEach<T>(this IEnumerable<T> values, Action<T> action) => XHelper.Enumerable.ForEach(values, action);

    public static void ForEachObject(this IEnumerable values, Action<object> action) => XHelper.Enumerable.ForEach(values.Cast<object>(), action);

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) => XHelper.Enumerable.IsNullOrEmpty(enumerable);

    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector) => source.MinBy(selector, Comparer<TKey>.Default);

    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer) => XHelper.Enumerable.MinBy(source, selector, comparer);

    public static IEnumerable<T> Subtraction<T>(this IEnumerable<T> enumerable1, IEnumerable<T> enumerable2, Comparison<T> compare = null) => XHelper.Enumerable.Subtraction(enumerable1, enumerable2, compare);

    public static BindingList<T> ToBindingList<T>(this IEnumerable<T> data) where T : class => XHelper.Enumerable.ToBindingList(data);

    public static T[] ToArrayEfficient<T>(this IEnumerable<T> values) => XHelper.Enumerable.ToArrayEfficient(values);

    public static List<T> ToListEfficient<T>(this IEnumerable<T> values) => XHelper.Enumerable.ToListEfficient(values);

    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> values, int nSize) => XHelper.Enumerable.Split(values, nSize);
}