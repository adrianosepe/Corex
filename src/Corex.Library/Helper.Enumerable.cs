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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Corex.Exceptions.Assert;
using Corex.Exceptions.Base;

namespace Corex.Library;

public partial class XHelper
{
    public static class Enumerable
    {
        public static bool FirstOrDefault<T>(IEnumerable<T> values, out T value)
        {
            value = values.FirstOrDefault();

            return !EqualityComparer<T>.Default.Equals(value, default);
        }

        public static void ForEach<T>(IEnumerable<T> enumerable, Action<T> action)
        {
            XContract.ArgIsNotNull(enumerable, nameof(enumerable));
            XContract.ArgIsNotNull(action, nameof(action));

            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        public static bool IsIn<T>(T value, params T[] args)
        {
            if (Obj.IsNull(value))
            {
                return false;
            }

            return args.Any(arg => arg?.Equals(value) ?? false);
        }

        public static bool IsNullOrEmpty<T>(IEnumerable<T> enumerable)
        {
            switch (enumerable) {
                case null:
                    return true;

                case T[] asArray:
                    return asArray.Length == 0;

                default:
                    return !enumerable.Any();
            }
        }

        public static TSource MinBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            using var sourceIterator = source.GetEnumerator();
            if (!sourceIterator.MoveNext())
            {
                throw new XInvalidOperationException("Sequence was empty");
            }

            var min = sourceIterator.Current;
            var minKey = selector(min);
            while (sourceIterator.MoveNext())
            {
                var candidate = sourceIterator.Current;
                var candidateProjected = selector(candidate);
                if (comparer.Compare(candidateProjected, minKey) < 0)
                {
                    min = candidate;
                    minKey = candidateProjected;
                }
            }

            return min;
        }

        public static IEnumerable<T> Subtraction<T>(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2, Comparison<T> compare = null)
        {
            XContract.ArgIsNotNull(enumerable1, nameof(enumerable1));
            XContract.ArgIsNotNull(enumerable2, nameof(enumerable2));

            if (compare == null)
            {
                compare = (x, y) => Obj.AreEquals(x, y) ? 0 : 1;
            }

            enumerable1 = enumerable1.ToList();

            if (!enumerable1.Any())
            {
                return Array.Empty<T>();
            }

            enumerable2 = enumerable2.ToList();

            var result = new List<T>();

            ForEach(
                enumerable1,
                c =>
                    {
                        // ReSharper disable SimplifyLinqExpression
                        if (!enumerable2.Any(x => compare(c, x) == 0))
                            // ReSharper restore SimplifyLinqExpression
                        {
                            result.Add(c);
                        }
                    });

            return result.ToArray();
        }

        public static T[] ToArrayEfficient<T>(IEnumerable<T> values)
        {
            var array = values as T[];
            return array ?? values.ToArray();
        }

        public static BindingList<T> ToBindingList<T>(IEnumerable<T> data) where T : class
        {
            var bl = data as BindingList<T>;

            return bl ?? new BindingList<T>(ToListEfficient(data));
        }

        public static List<T> ToListEfficient<T>(IEnumerable<T> values)
        {
            var array = values as List<T>;

            return array ?? values.ToList();
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(IEnumerable<T> values, int nSize)
        {
            var data = values.ToArray();

            for (var i = 0; i < data.Length; i += nSize)
            {
                yield return data.Skip(i).Take(nSize);
            }
        }

        public static IEnumerable<IEnumerable<T>> GroupWhile<T>(IEnumerable<T> seq, Func<T, T, bool> condition)
        {
            var array = seq as T[] ?? seq.ToArray();

            if (array.Length == 0)
            {
                yield return Array.Empty<T>();
            }

            var prev = array.First();
            var list = new List<T>
                {
                    prev
                };

            foreach (var item in array.Skip(1))
            {
                if (condition(prev, item) == false)
                {
                    yield return list;

                    list = new List<T>();
                }

                list.Add(item);
                prev = item;
            }

            yield return list;
        }
    }
}