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

namespace Corex.Library.ComponentModel.Model.Validation.Annotations;

public abstract class XDataTypeAttribute : DataTypeAttribute
{
    protected XDataTypeAttribute(EDataType dataType, bool isComputed = false) : base(Translate(dataType))
    {
        DataType = dataType;

        IsComputed = isComputed;
    }

    public new EDataType DataType { get; }

    public bool IsComputed { get; }

    public override string GetDataTypeName()
    {
        if (base.DataType == System.ComponentModel.DataAnnotations.DataType.Custom)
        {
            return "Custom";
        }

        return base.GetDataTypeName();
    }

    public sealed override bool IsValid(object value) => IsComputed || !InternalIsEnabled() || InternalIsValid(value);

    protected virtual bool InternalIsEnabled() => true;

    protected virtual bool InternalIsValid(object value) => base.IsValid(value);

    protected virtual ValidationResult InternalIsValid(object value, ValidationContext validationContext) => base.IsValid(value, validationContext);

    protected sealed override ValidationResult IsValid(object value, ValidationContext validationContext) => IsComputed || !InternalIsEnabled() ? ValidationResult.Success : InternalIsValid(value, validationContext);

    private static DataType Translate(EDataType dataType)
    {
        var translated = XHelper.Enums.TryGetValue<DataType>(dataType.ToString());

        return translated ?? System.ComponentModel.DataAnnotations.DataType.Custom;
    }
}