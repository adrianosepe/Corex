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
using System.Linq;
using System.Text;
using Corex.Library;

// ReSharper disable CheckNamespace
public static class XStringExtensions
// ReSharper restore CheckNamespace
{
    public static readonly Encoding DefaultEncoding = Encoding.UTF8;

    public static string AddOnLeft(this string value, string data) => data + value;

    public static string AddOnRight(this string value, string data) => value + data;

    // #region Convert
    public static byte AsByte(this string value) => Convert.ToByte(value);

    public static byte[] AsByteArray(this string value, Encoding encoding = null) => (encoding ?? DefaultEncoding).GetBytes(value);

    public static double AsDouble(this string value) => Convert.ToDouble(value);

    public static float AsFloat(this string value) => Convert.ToSingle(value);

    public static int AsInt32(this string value) => Convert.ToInt32(value);

    public static long AsInt64(this string value) => Convert.ToInt64(value);

    public static short AsShort(this string value) => Convert.ToInt16(value);

    public static string AsString(this byte[] data, Encoding encoding = null) => (encoding ?? DefaultEncoding).GetString(data);

    public static string BeginWith(this string value, string begin) => value + begin;

    public static string Concat(this IEnumerable<string> data, string separator = ", ")
    {
        var asArray = data.ToArrayEfficient();

        return asArray.Length == 0 ? string.Empty : asArray.Aggregate((s, s1) => s + separator + s1);
    }

    public static string Delete(this string content, int countFromIndexZero, int countFromLastIndex = 0) => content.Substring(countFromIndexZero, content.Length - (countFromLastIndex + 1));

    public static int IndexOfNth(this string str, char value, int nth = 0) => XHelper.Strings.IndexOfNth(str, value, nth);

    public static string Insert(this string original, string insertValue, int offset)
    {
        var ivLength = insertValue.Length;

        var index = offset;
        while (index < original.Length)
        {
            original = original.Insert(index, insertValue);
            index += offset + ivLength;
        }

        return original;
    }

    public static bool IsEmptyOrWhiteSpace(this string value) => value.IsNullOrEmpty() || value.All(char.IsWhiteSpace);

    public static bool IsNotEmpty(this string value) => value.IsNullOrEmpty() == false;

    public static bool IsNull(this string value, bool considereEmptyAsNull = true) => considereEmptyAsNull ? string.IsNullOrEmpty(value) : XHelper.Obj.IsNull(value);

    public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

    /// <summary>
    ///     Remove any instance of the given character from the current string.
    /// </summary>
    /// <param name="value">
    ///     The input.
    /// </param>
    /// <param name="removeCharc">
    ///     The remove char.
    /// </param>
    /// <remarks>
    ///     Contributed by Michael T, http://about.me/MichaelTran
    /// </remarks>
    public static string Remove(this string value, params char[] removeCharc)
    {
        var result = value;
        if (!string.IsNullOrEmpty(result) && removeCharc != null)
        {
            Array.ForEach(removeCharc, c => result = result.Remove(c.ToString()));
        }

        return result;
    }

    /// <summary>
    ///     Remove any instance of the given string pattern from the current string.
    /// </summary>
    /// <param name="value">The input.</param>
    /// <param name="strings">The strings.</param>
    /// <returns></returns>
    /// <remarks>
    ///     Contributed by Michael T, http://about.me/MichaelTran
    /// </remarks>
    public static string Remove(this string value, params string[] strings)
    {
        return strings.Aggregate(value, (current, c) => current.Replace(c, string.Empty));
    }

    public static string ReplaceMany(this string value, params (string Parte, string NewValue)[] args) => args.Aggregate(value, (current, replace) => current.Replace(replace.Parte, replace.NewValue));

    public static int SafeLength(this string value) => value?.Length ?? 0;

    public static string TrimSafe(this string value) => string.IsNullOrEmpty(value) ? string.Empty : value.Trim();

    public static string Truncate(this string value, int length) => XHelper.Strings.Truncate(value, length);
}