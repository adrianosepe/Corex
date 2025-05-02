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
using System.ComponentModel;
using System.Globalization;

namespace Corex.Library;

public partial class XHelper
{
    public static partial class DisplayFormat
    {
        public static class Mask
        {
            public static string Format(string mask, string text, CultureInfo culture = null) => Format(mask, text, out _, out _, culture);

            public static string Format(string mask, string text, out MaskedTextResultHint hint, out int hintPosition, CultureInfo culture = null)
            {
                if (text == null)
                {
                    text = string.Empty;
                }

                if (culture == null)
                {
                    culture = CultureInfo.InvariantCulture;
                }

                var provider = new MaskedTextProvider(mask, culture, true);

                // Format and return the string
                provider.Set(text, out hintPosition, out hint);

                // Positive hint results are successful
                if (hint > 0)
                {
                    return provider.ToString();
                }

                // Return the text as-is if it didn't fit the mask
                return text;
            }
        }
    }
}