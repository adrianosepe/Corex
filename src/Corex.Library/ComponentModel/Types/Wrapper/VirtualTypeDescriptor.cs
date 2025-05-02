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
using System.Linq;

namespace Corex.Library.ComponentModel.Types.Wrapper;

public class VirtualTypeDescriptor : object, ICustomTypeDescriptor
{
    private readonly Dictionary<string, VirtualPropertyDescriptor> _data;

    public VirtualTypeDescriptor()
    {
        _data = new Dictionary<string, VirtualPropertyDescriptor>();
    }

    public IEnumerable<string> PropertiesNames => _data.Keys;

    public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(this, true);

    public string GetClassName() => TypeDescriptor.GetClassName(this, true);

    public string GetComponentName() => TypeDescriptor.GetComponentName(this, true);

    public TypeConverter GetConverter() => TypeDescriptor.GetConverter(this, true);

    public EventDescriptor GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(this, true);

    public PropertyDescriptor GetDefaultProperty() => TypeDescriptor.GetDefaultProperty(this, true);

    public object GetEditor(Type editorBaseType) => TypeDescriptor.GetEditor(this, editorBaseType, true);

    public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents(this, true);

    public EventDescriptorCollection GetEvents(Attribute[] attributes) => TypeDescriptor.GetEvents(attributes, true);

    public PropertyDescriptorCollection GetProperties() => new PropertyDescriptorCollection(_data.Values.Cast<PropertyDescriptor>().ToArray());

    public PropertyDescriptorCollection GetProperties(Attribute[] attributes) => GetProperties();

    public object GetPropertyOwner(PropertyDescriptor pd) => this;

    public void Add(
        string propertyName, string displayName, Type dataType, object value,
        string category, Attribute[] attributes = null)
    {
        var newPropertyDescriptor = new VirtualPropertyDescriptor(
            propertyName, value, dataType, displayName, category, this);

        newPropertyDescriptor.AddNewAttributes(attributes);

        _data.Add(propertyName, newPropertyDescriptor);
    }

    public object GetValue(string propertyName) => _data[propertyName].Value;
}