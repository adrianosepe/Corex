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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Corex.Library.ComponentModel.Model.Validation.Annotations;
using Corex.Library.ComponentModel.Model.Validation.Annotations.Key;

namespace Corex.Library.ComponentModel.Model.Validation.Behaviors;

public static class BehaviorIdentifier
{
    public static bool IsPropertyRequired(PropertyDescriptor propertyDescriptor)
    {
        var attributes = propertyDescriptor.Attributes;
        var required = attributes.OfType<RequiredAttribute>().Any()
                       || attributes.OfType<XRequiredAttribute>().Any();

        if (!required)
        {
            if (XHelper.Types.IsNullable(propertyDescriptor.PropertyType))
            {
                return false;
            }

            required = attributes.OfType<XForeignKeyAttribute>().Any(att => !att.UpdatedProgrammatically);
        }

        return required;
    }

    public static class Fk
    {
        public static bool Is(PropertyInfo propertyInfo) => Is(propertyInfo, out _);

        public static bool Is(PropertyInfo propertyInfo, out string navPropertyName)
        {
            var att = XHelper.Reflections.Attributes.Get<XForeignKeyAttribute>(propertyInfo);

            if (att == null)
            {
                navPropertyName = null;
                return false;
            }

            navPropertyName = att.NavigationPropertyName ?? Entity.Metadata.GetNavigationPropertyName(propertyInfo.Name);

            return true;
        }

        public static bool Is(PropertyDescriptor propertyDescriptor, out string navPropertyName)
        {
            var att = XHelper.Reflections.Attributes.Get<XForeignKeyAttribute>(propertyDescriptor);

            if (att == null)
            {
                navPropertyName = null;
                return false;
            }

            navPropertyName = att.NavigationPropertyName ?? Entity.Metadata.GetNavigationPropertyName(propertyDescriptor.Name);

            return true;
        }

        public static bool TryGetNavigationProperty(Type modelType, string fkPropertyName, out PropertyInfo navPropertyInfo, Type navPropertyType = null)
        {
            var navPropertyName = Entity.Metadata.GetNavigationPropertyName(fkPropertyName);
            navPropertyInfo = XHelper.Reflections.Properties.Get(modelType, navPropertyName, false);

            if (navPropertyInfo != null && navPropertyType != null)
            {
                if (!XHelper.Types.Is(navPropertyInfo.PropertyType, navPropertyType))
                {
                    navPropertyInfo = null;
                }
            }

            return navPropertyInfo != null;
        }

        public static bool TryGetNavigationProperty(Type modelType, PropertyInfo fkPropertyInfo, out PropertyInfo navPropertyInfo, Type navPropertyType = null)
        {
            if (!Is(fkPropertyInfo, out var navPropertyName))
            {
                navPropertyInfo = null;

                return false;
            }

            navPropertyInfo = XHelper.Reflections.Properties.Get(modelType, navPropertyName, false);

            if (navPropertyInfo != null && navPropertyType != null)
            {
                if (!XHelper.Types.Is(navPropertyInfo.PropertyType, navPropertyType))
                {
                    navPropertyInfo = null;
                }
            }

            return navPropertyInfo != null;
        }

        public static bool TryGetNavigationProperty(Type modelType, PropertyDescriptor fkPropertyDescriptor, out PropertyInfo navPropertyInfo, Type navPropertyType = null)
        {
            if (!Is(fkPropertyDescriptor, out var navPropertyName))
            {
                navPropertyInfo = null;

                return false;
            }

            navPropertyInfo = XHelper.Reflections.Properties.Get(modelType, navPropertyName, false);

            if (navPropertyInfo != null && navPropertyType != null)
            {
                if (!XHelper.Types.Is(navPropertyInfo.PropertyType, navPropertyType))
                {
                    navPropertyInfo = null;
                }
            }

            return navPropertyInfo != null;
        }
    }
}