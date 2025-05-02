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
using Corex.Exceptions.Base;
using NativeTypeDesc = System.ComponentModel.TypeDescriptor;

namespace Corex.Library.ComponentModel.Types.Wrapper;

public class XCustomTypeDescriptor : ICustomTypeDescriptor
{
    private readonly object _entity;

    private readonly Type _entityType;

    private readonly Lazy<PropertyDescriptorCollection> _lazyProperties;

    public XCustomTypeDescriptor(object entity)
    {
        _entity = entity;
        _entityType = _entity.GetType();
        TypeDescriptor = XTypeDescriptor.Get(_entityType);

        _lazyProperties = new Lazy<PropertyDescriptorCollection>(
            () =>
                {
                    var properties = new List<XPropertyDescriptor>();

                    properties.AddRange(
                        TypeDescriptor.PropertiesOfMetadataPropertyDescriptor
                            .Select(prop => new XPropertyDescriptor(prop, null, this)));

                    return new PropertyDescriptorCollection(properties.Cast<PropertyDescriptor>().ToArray());
                });
    }

    public XTypeDescriptor TypeDescriptor { get; }

    public AttributeCollection GetAttributes() => NativeTypeDesc.GetAttributes(_entityType, true);

    public string GetClassName() => NativeTypeDesc.GetClassName(_entityType, true);

    public string GetComponentName() => NativeTypeDesc.GetComponentName(_entityType, true);

    public TypeConverter GetConverter() => NativeTypeDesc.GetConverter(_entityType, true);

    public EventDescriptor GetDefaultEvent() => NativeTypeDesc.GetDefaultEvent(_entityType, true);

    public PropertyDescriptor GetDefaultProperty() => NativeTypeDesc.GetDefaultProperty(_entityType, true);

    public object GetEditor(Type editorBaseType) => NativeTypeDesc.GetEditor(_entityType, editorBaseType, true);

    public EventDescriptorCollection GetEvents() => NativeTypeDesc.GetEvents(_entityType, true);

    public EventDescriptorCollection GetEvents(Attribute[] attributes) => NativeTypeDesc.GetEvents(attributes, true);

    public PropertyDescriptorCollection GetProperties() => _lazyProperties.Value;

    public PropertyDescriptorCollection GetProperties(Attribute[] attributes) => GetProperties();

    public object GetPropertyOwner(PropertyDescriptor propertyDescriptor) => _entity;

    public void AddAttributesToProperty(string propertyName, Attribute[] customAttributes)
    {
        var propertyDescriptor = GetProperties()
            .Find(propertyName, true)
            .FlowMustBeNotNull()
            .As<XPropertyDescriptor>();

        if (propertyDescriptor == null)
        {
            throw new XException($"The property description must be of type {nameof(XPropertyDescriptor)}");
        }

        propertyDescriptor.AddNewAttributes(customAttributes);
    }
}