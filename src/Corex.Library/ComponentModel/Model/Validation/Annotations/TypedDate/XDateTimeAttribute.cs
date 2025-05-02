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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Corex.Library.ComponentModel.Model.Validation.Behaviors;
using Corex.Library.ComponentModel.Model.Validation.Enums;
using Corex.Library.Properties;

namespace Corex.Library.ComponentModel.Model.Validation.Annotations.TypedDate;

public class XDateTimeAttribute : XValidationAttribute, IDateTimeConstraint
{
    public XDateTimeAttribute(bool useAdvancingCaret = true, bool isComputed = false)
        : base(
            EDataType.DateTime,
            useAdvancingCaret ? EValidationMode.MaskDateTimeAdvancingCaret : EValidationMode.MaskDataTime,
            isComputed)
    {
        ErrorMessage = DbMessages.XDateTimeAttribute_XDateTimeAttribute_O_campo__0__deve_ser_informado;
    }

    // ReSharper disable LocalizableElement
    public override string Mask => "G";
    // ReSharper restore LocalizableElement

    public EDateTimeFormat Format => EDateTimeFormat.DateTime;

    protected override bool InternalIsValid(object value) => true;

    protected override ValidationResult InternalIsValid(object value, ValidationContext validationContext)
    {
        if (validationContext == null)
        {
            return ValidationResult.Success;
        }

        var propertyInfo = XHelper.Reflections.Properties.Get(validationContext.ObjectType, validationContext.MemberName);

        if (XHelper.Types.IsNullable(propertyInfo.PropertyType) && value == null)
        {
            return ValidationResult.Success;
        }

        if (DateTime.TryParse(value.ToString(), out var dateTime) && !XHelper.Models.DateTimeIsValid(dateTime))
        {
            return GetErrorResult(validationContext);
        }

        return ValidationResult.Success;
    }

    private ValidationResult GetErrorResult(ValidationContext validationContext) => new ValidationResult(string.Format(ErrorMessage, validationContext?.DisplayName));
}