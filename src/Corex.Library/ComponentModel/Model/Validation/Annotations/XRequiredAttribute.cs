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
using System.Collections;
using System.ComponentModel.DataAnnotations;
using Corex.Library.ComponentModel.Model.Enums;
using Corex.Library.Properties;

namespace Corex.Library.ComponentModel.Model.Validation.Annotations;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property)]
public class XRequiredAttribute : ValidationAttribute
{
    public enum EDataType
    {
        Common,
        ZeroIsInvalid
    }

    public XRequiredAttribute()
    {
        ErrorMessage = DbMessages.XRequiredAttribute_XRequiredAttribute_O_campo__0__deve_ser_informado_;
    }

    public EDataType DataType { get; set; }

    public virtual bool Enabled => RestrictFor == EModoOperacao.None || RestrictFor == ValidationSettings.RestrictionFor;

    public EModoOperacao RestrictFor { get; set; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (!Enabled)
        {
            return ValidationResult.Success;
        }

        if (value == null)
        {
            return GetErrorResult(validationContext);
        }

        if (value.GetType().IsArray && ((Array) value).Length == 0)
        {
            return GetErrorResult(validationContext);
        }

        if (value.GetType().IsEnum && !XHelper.Enums.IsDefined((Enum) value))
        {
            return GetErrorResult(validationContext);
        }

        if (value is IEnumerable valueenum && !valueenum.GetEnumerator().MoveNext())
        {
            return GetErrorResult(validationContext);
        }

        switch (Type.GetTypeCode(value.GetType()))
        {
            case TypeCode.Int32:
                if (DataType == EDataType.ZeroIsInvalid)
                {
                    if (Convert.ToInt32(value) == 0)
                    {
                        return GetErrorResult(validationContext);
                    }
                }

                break;

            case TypeCode.String:
                if (string.IsNullOrEmpty(value as string))
                {
                    return GetErrorResult(validationContext);
                }

                break;
        }

        return ValidationResult.Success;
    }

    private ValidationResult GetErrorResult(ValidationContext validationContext) => new ValidationResult(string.Format(ErrorMessage, validationContext?.DisplayName));
}