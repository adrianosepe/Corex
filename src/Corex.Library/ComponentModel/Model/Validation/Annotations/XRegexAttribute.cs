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

namespace Corex.Library.ComponentModel.Model.Validation.Annotations;

public class XRegexAttribute : XValidationAttribute
{
    private const string Message = "O campo {0} não possui um valor válido.";

    protected const RegexOptions DefaultRegexOptions = RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled;

    private readonly Regex _regex;

    public XRegexAttribute(string regex, string errorMessage = null, EDataType dataType = EDataType.Custom)
        : this(new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled), errorMessage ?? Message, dataType) { }

    public XRegexAttribute(Regex regex, string errorMessage = null, EDataType dataType = EDataType.Custom) : base(dataType, EValidationMode.MaskRegEx, errorMessage)
    {
        _regex = regex;
    }

    public virtual bool DisconsiderEmptyValue => true;

    public override string Mask => _regex.ToString();

    protected override bool InternalIsValid(object value)
    {
        if (value == null)
        {
            return true;
        }

        var input = value.As<string>();

        if (DisconsiderEmptyValue && input.IsEmptyOrWhiteSpace())
        {
            return true;
        }

        if (input != null)
        {
            return _regex.Match(input).Length > 0;
        }

        return false;
    }
}