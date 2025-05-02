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

using System.Text.RegularExpressions;
using Corex.Library.Properties;

namespace Corex.Library.ComponentModel.Model.Validation.Annotations.Docs;

public class XRucOrCiAttribute : XRegexAttribute
{
    private static readonly Regex Regex =
        new Regex(
            ValidationDataFormats.DocRucRegex, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

    public XRucOrCiAttribute() : base(Regex, DbMessages.XRucOrCiAttribute_XRucOrCiAttribute_O_campo__0__não_possui_um_RUC_válido_)
    {
        Mode = EValidationMode.MaskRegEx;
        MaskForEditor = ValidationDataFormats.DocRucRegex;
    }

    public override bool Enabled => ValidationSettings.InParaguayMode;

    protected override bool InternalIsValid(object value)
    {
        if (XHelper.Strings.IsEmptyOrWhiteSpace(value?.ToString()))
        {
            return false;
        }

        var documentoID = value.ToString();

        // ReSharper disable LocalizableElement
        var isRuc = documentoID.Contains("-");
        // ReSharper restore LocalizableElement

        return !isRuc || XHelper.Data.Validations.IsRuc(value.ToString());
    }
}