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

// ReSharper disable UnusedMember.Global

// ReSharper disable CheckNamespace
public static class XEnumExtensions
// ReSharper restore CheckNamespace
{
    public static string DisplayEnumAsDescription<TEnum>(this TEnum enumValue) => XHelper.Enums.GetDisplay(enumValue, enumValue.ToString());

    public static bool IsDefined<TEnum>(this TEnum enumValue) => Enum.IsDefined(typeof(TEnum), enumValue);

    public static bool IsIn<TEnum>(this TEnum enumValue, params TEnum[] enums) where TEnum : Enum => XHelper.Enums.IsIn(enumValue, enums);

    public static bool IsLast<TEnum>(this TEnum enumValue)
    {
        if (!Enum.IsDefined(typeof(TEnum), enumValue))
        {
            return false;
        }

        var values = Enum.GetValues(typeof(TEnum));

        return Array.IndexOf(values, enumValue) + 1 == values.Length;
    }

    public static TEnum Next<TEnum>(this TEnum enumValue)
    {
        if (!Enum.IsDefined(typeof(TEnum), enumValue))
        {
            return enumValue;
        }

        var values = Enum.GetValues(typeof(TEnum));
        var index = Array.IndexOf(values, enumValue);

        if (index + 1 == values.Length)
        {
            return enumValue;
        }

        return (TEnum) values.GetValue(index + 1);
    }
}