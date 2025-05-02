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
using System.Collections.Generic;
using System.ComponentModel;
using Corex.Exceptions.Assert;

namespace Corex.Library.ComponentModel.Types.Wrapper;

public class VirtualPropertyDescriptor : PropertyDescriptor
{
    private readonly VirtualTypeDescriptor _parent;

    public VirtualPropertyDescriptor(
        string propertyName, object value, Type dataType,
        string displayName, string category, VirtualTypeDescriptor parent)
        : base(propertyName, null)
    {
        XContract.ArgIsNotNull(dataType, nameof(dataType));

        Value = value;
        PropertyType = dataType;
        DisplayName = displayName;
        Category = category;
        _parent = parent;
    }

    public override string Category { get; }

    public override Type ComponentType => typeof(VirtualTypeDescriptor);

    public override string DisplayName { get; }

    public override bool IsReadOnly => false;
    public override Type PropertyType { get; }

    public object Value { get; set; }

    public void AddNewAttributes(Attribute[] attributes)
    {
        if (attributes.IsNullOrEmpty())
        {
            return;
        }

        var newAttrs = new List<Attribute>();

        newAttrs.AddRange(AttributeArray);
        newAttrs.AddRange(attributes);

        AttributeArray = newAttrs.ToArray();
    }

    public override bool CanResetValue(object component) => true;

    public override object GetValue(object component) => Value;

    public override void ResetValue(object component) { }

    public override void SetValue(object component, object value)
    {
        Value = value;
    }

    public override bool ShouldSerializeValue(object component) => false;
}