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
using System.Text.RegularExpressions;

namespace Corex.Library.ComponentModel.Model.Validation.Annotations.Contact;

public sealed class XUrlAttribute : XRegexAttribute
{
    private const string Message = "O campo {0} não possui uma URL válida.";

    private static readonly Regex RegexReqProtocol =
        new Regex(
            ValidationDataFormats.ContactUrlReqProtocol, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    private static readonly Regex RegexOpProtocol =
        new Regex(
            ValidationDataFormats.ContactUrlOpProtocol, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    private static readonly Regex RegexWithoutProtocol =
        new Regex(
            ValidationDataFormats.ContactUrlWithoutProtocol, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    public XUrlAttribute(EUrlOption option = EUrlOption.RequireProtocol) : base(Translate(option), Message, EDataType.Url)
    {
        Mode = EValidationMode.MaskRegEx;

        MaskForEditor = ValidationDataFormats.ContactUrlMask;
    }

    private static Regex Translate(EUrlOption option)
    {
        return option switch
            {
                EUrlOption.RequireProtocol => RegexReqProtocol,
                EUrlOption.OptionalProtocol => RegexOpProtocol,
                EUrlOption.WithoutProtocol => RegexWithoutProtocol,
                _ => throw new ArgumentOutOfRangeException(nameof(option), option, null)
            };
    }
}