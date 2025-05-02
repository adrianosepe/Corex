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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Corex.Library.Formater;
using JetBrains.Annotations;

namespace Corex.Library;

public partial class XHelper
{
    public static partial class Strings
    {
        public const string Empty = "";
        public const char SpaceChar = ' ';

        public static string Alignment(ETextAlignment alignment, string data, int length, char alignWith = SpaceChar)
        {
            switch (alignment)
            {
                case ETextAlignment.Left:
                    return Left(data, length, alignWith);

                case ETextAlignment.Center:
                    return Center(data, length, alignWith);

                case ETextAlignment.Right:
                    return Right(data, length, alignWith);

                default:
                    throw new ArgumentOutOfRangeException(nameof(alignment));
            }
        }

        public static bool AllHaveData(params string[] data) => data.All(HasData);

        public static int IndexOfNth(string str, char value, int nth)
        {
            nth--;

            if (nth < 0)
            {
                throw new ArgumentException("Can not find a negative index of substring in string. Must start with 0");
            }

            var offset = str.IndexOf(value);
            for (var i = 0; i < nth; i++)
            {
                if (offset == -1)
                {
                    return -1;
                }

                offset = str.IndexOf(value, offset + 1);
            }

            return offset;
        }

        public static bool HasData(string data) => !string.IsNullOrWhiteSpace(data) && data != Empty;

        [ContractAnnotation("data:null => true")]
        public static bool IsEmptyOrWhiteSpace(string data) => !HasData(data);

        public static bool IsLetterWithDiacritics(char eUnicodeChar)
        {
            var s = eUnicodeChar.ToString().Normalize(NormalizationForm.FormD);

            return s.Length > 1
                   && char.IsLetter(s[0])
                   && s.Skip(1)
                       .All(c2 => CharUnicodeInfo.GetUnicodeCategory(c2) == UnicodeCategory.NonSpacingMark);
        }

        [ContractAnnotation("data:null => false")]
        public static bool IsNotEmptyOrWhiteSpace(string data) => HasData(data);

        public static string RemoveDiacritics(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return data;
            }

            data = data.Normalize(NormalizationForm.FormD);
            var chars = data.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public static string RemoveSpecialCharacters(string input)
        {
            const string pattern = @"[^\w\.@-]";

            return Regex.Replace(input, pattern, Empty);
        }

        public static string Repeat(string value, int count) => new StringBuilder().Insert(0, value, count).ToString();

        public static string Truncate(string data, int length)
        {
            if (string.IsNullOrEmpty(data))
            {
                return data;
            }

            return data.Length <= length ? data : data.Substring(0, length);
        }

        /// <summary>
        ///     Adiciona espaços antes dos caracteres capitais de uma string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UnconcatString(string input) => Regex.Replace(input, "[A-Z]", " $0").Trim();

        private static string Center(string data, int length, char alignWith = SpaceChar)
        {
            data = Truncate(data, length);

            var spaces = length - data.Length;
            var padLeft = spaces / 2 + data.Length;

            return data.PadLeft(padLeft, alignWith).PadRight(length, alignWith);
        }

        private static string Left(string data, int length, char alignWith = SpaceChar) => Truncate(data, length).PadRight(length, alignWith);

        private static string Right(string data, int length, char alignWith = SpaceChar) => Truncate(data, length).PadLeft(length, alignWith);
    }
}