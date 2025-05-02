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
using Corex.Library.ComponentModel.Model.Abstraction;
using Corex.Library.Properties;

namespace Corex.Library.ComponentModel.Model.Validation.Annotations.TypedDate;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property)]
public class XDateRangeAttribute : XValidationAttribute
{
    public XDateRangeAttribute() : base(EDataType.Custom, EValidationMode.Custom) { }

    public bool AllowIncompleteRange { get; set; }

    public override string Mask => null;

    public int MaxRange { get; set; }

    public int MinRange { get; set; }

    protected override ValidationResult InternalIsValid(object value, ValidationContext validationContext)
    {
        var dateRange = value.As<IDateRange>();

        if (dateRange == null)
        {
            return ValidationResult.Success;
        }

        if (!dateRange.IsNull)
        {
            return ValidationResult.Success;
        }

        if (dateRange.IsNullPartial)
        {
            return AllowIncompleteRange
                ? ValidationResult.Success
                : GetErrorResult(DbMessages.XDateRangeAttribute_IsValid_O_período_informado_está_incompleto_, validationContext);
        }

        var range = dateRange.Range;

        if (range.TotalDays < 0)
        {
            return GetErrorResult(
                DbMessages.XDateRangeAttribute_IsValid_Período_informado_é_inválido_, validationContext);
        }

        if (MinRange != 0)
        {
            if (range.TotalDays < MinRange)
            {
                return GetErrorResult(
                    string.Format(DbMessages.XDateRangeAttribute_IsValid_O_período_mínimo_deve_ser_de__0__dia_s__, MinRange), validationContext);
            }
        }

        if (MaxRange != 0)
        {
            if (range.TotalDays > MaxRange)
            {
                return GetErrorResult(
                    string.Format(DbMessages.XDateRangeAttribute_IsValid_O_período_máximo_deve_ser_de__0__dia_s__, MinRange), validationContext);
            }
        }

        return ValidationResult.Success;
    }

    protected override bool InternalIsValid(object value) => true;

    private static ValidationResult GetErrorResult(string errorMessage, ValidationContext validationContext)
    {
        return new ValidationResult(errorMessage, new[] {validationContext.MemberName});
    }
}