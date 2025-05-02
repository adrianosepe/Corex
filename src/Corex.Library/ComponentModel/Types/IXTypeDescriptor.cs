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
using System.Reflection;

namespace Corex.Library.ComponentModel.Types;

public interface IXTypeDescriptor
{
    string ModelDisplayName { get; }
    Type ModelType { get; }
    IEnumerable<PropertyDescriptor> Properties { get; }
    PropertyDescriptor GetDefaultLookupProperty();
    object GetDefaultLookupPropertyValue(object obj);

    string GetModelDisplayName(object obj);

    PropertyDescriptor GetPrimaryKeyProperty();
    object GetPrimaryKeyPropertyValue(object obj);
    PropertyDescriptor GetPropertyDescriptor(string propertyName);

    string GetPropertyDisplayName(PropertyDescriptor propertyDescription, bool formatRequiredAsHtml = true);

    PropertyInfo GetPropertyInfo(string propertyName);
    bool IsPropertyRequired(PropertyDescriptor propertyDescriptor);
}

// ReSharper disable UnusedTypeParameter
public interface IXTypeDescriptor<TModel> : IXTypeDescriptor { }
// ReSharper restore UnusedTypeParameter

public interface IXVMTypeDescriptor<TModel> : IXTypeDescriptor<TModel> { }