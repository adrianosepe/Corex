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

namespace Corex.Library.ComponentModel.Types.Wrapper;

public class XPropertyDescriptor : PropertyDescriptor
{
    private readonly XCustomTypeDescriptor _customTypeDescriptor;

    public XPropertyDescriptor(PropertyDescriptor descriptor, Attribute[] attributes, XCustomTypeDescriptor customTypeDescriptor) : base(descriptor, attributes)
    {
        Descriptor = descriptor;
        _customTypeDescriptor = customTypeDescriptor;
    }

    public override Type ComponentType => Descriptor.ComponentType;

    public PropertyDescriptor Descriptor { get; }

    public override string DisplayName => _customTypeDescriptor.TypeDescriptor.GetPropertyDisplayName(Descriptor, false);

    public override bool IsReadOnly
    {
        get
        {
            var readOnlyAttribute = (ReadOnlyAttribute) Attributes[typeof(ReadOnlyAttribute)];

            return readOnlyAttribute?.IsReadOnly ?? Descriptor.IsReadOnly;
        }
    }

    public bool IsRequired => _customTypeDescriptor.TypeDescriptor.IsPropertyRequired(Descriptor);

    public override Type PropertyType => Descriptor.PropertyType;

    public void AddNewAttributes(Attribute[] attributes)
    {
        var newAttrs = new List<Attribute>();

        newAttrs.AddRange(AttributeArray);
        newAttrs.AddRange(attributes);

        AttributeArray = newAttrs.ToArray();
    }

    public override bool CanResetValue(object component) => Descriptor.CanResetValue(component);

    public override object GetValue(object component) => Descriptor.GetValue(component);

    public override void ResetValue(object component)
    {
        Descriptor.ResetValue(component);
    }

    public override void SetValue(object component, object value)
    {
        Descriptor.SetValue(component, value);
    }

    public override bool ShouldSerializeValue(object component) => Descriptor.ShouldSerializeValue(component);
}