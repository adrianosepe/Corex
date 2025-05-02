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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Corex.Library;

public partial class XHelper
{
    public static partial class Strings
    {
        public static class Formater
        {
            public static string Interpolation(string original, object obj)
            {
                const string openToken = "{";
                const string closeToken = "}";

                var properties = TypeDescriptor
                    .GetProperties(obj)
                    .Cast<PropertyDescriptor>()
                    .ToArray();

                var builder = new StringBuilder(original);

                Enumerable.ForEach(
                    RegEx.Matches(openToken, closeToken, original),
                    info =>
                        {
                            var prop = properties.FirstOrDefault(descriptor => descriptor.Name == info.Content);
                            if (prop == null)
                            {
                                return;
                            }

                            builder.Replace(info.Expression, prop.GetValue(obj)?.ToString() ?? string.Empty);
                        });

                return builder.ToString();
            }

            public static string SeparatedStringToCamelCase(string str) => I18n.CurrentCulture.TextInfo.ToTitleCase(str);

            public static string SepareteCamelCase(string str) => Regex.Replace(str, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }
    }
}