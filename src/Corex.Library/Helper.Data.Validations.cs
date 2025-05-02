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

using Corex.Library.ComponentModel.Model.Validation;

namespace Corex.Library;

public partial class XHelper
{
    public static partial class Data
    {
        public static class Validations
        {
            public static bool AreIntegerNumbers(string input) => ValidationDataFormats.AreIntegerNumbers(input);

            public static bool AreNumbers(string input) => ValidationDataFormats.AreNumbers(input);

            public static bool IsCep(string input) => ValidationDataFormats.IsCep(input);

            public static bool IsCnpj(string input) => ValidationDataFormats.IsCnpj(input);

            public static bool IsCpf(string input) => ValidationDataFormats.IsCpf(input);

            public static bool IsEmail(string input) => ValidationDataFormats.IsEmail(input);

            public static bool IsGuid(string input) => ValidationDataFormats.IsGuid(input);

            public static bool IsHora(string input) => ValidationDataFormats.IsHora(input);

            public static bool IsIccID(string input) => ValidationDataFormats.IsIccID(input);

            public static bool IsIPv4(string input) => ValidationDataFormats.IsIPv4(input);

            public static bool IsLatitude(string input) => ValidationDataFormats.IsLatitude(input);

            public static bool IsLoginPatternLength6To60(string input) => ValidationDataFormats.IsLoginPatternLength6To60(input);

            public static bool IsLongitude(string input) => ValidationDataFormats.IsLongitude(input);

            public static bool IsPlaca(string input) => ValidationDataFormats.IsPlaca(input);

            public static bool IsRuc(string input) => ValidationDataFormats.IsRuc(input);

            public static bool IsTelefone(string input, bool accept9Digits = true) => ValidationDataFormats.IsTelefone(input, accept9Digits);

            public static bool IsUrl(string input) => ValidationDataFormats.IsUrl(input);
        }
    }
}