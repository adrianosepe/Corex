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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Corex.Exceptions.Assert;
using Corex.Exceptions.Contract;
using Corex.Library.Attributes;

namespace Corex.Library;

public partial class XHelper
{
    public partial class Reflections
    {
        public static class Properties
        {
            public const BindingFlags InstanceBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            public const BindingFlags StaticBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            public static PropertyInfo Get(Type type, string propertyName, bool ifNotFoundThrowAnException = true) => Get(type, propertyName, InstanceBindingFlags, ifNotFoundThrowAnException);

            public static PropertyInfo Get(object obj, string propertyName, bool ifNotFoundThrowAnException = true)
            {
                XContract.ArgIsNotNull(obj, nameof(obj));

                return Get(obj.GetType(), propertyName, ifNotFoundThrowAnException);
            }

            public static PropertyInfo GetStatic(Type type, string propertyName, bool ifNotFoundThrowAnException = true) => Get(type, propertyName, StaticBindingFlags, ifNotFoundThrowAnException);

            public static IEnumerable<(string PropertyName, string DisplayName, object Value, PropertyInfo PropInfo)> Pivot(object obj)
            {
                foreach (var prop in obj.GetType().GetProperties(InstanceBindingFlags))
                {
                    if (Attributes.HasAttribute<HideMemberAttribute>(obj.GetType()))
                    {
                        continue;
                    }

                    var displayName = Models.GetDisplayName(prop) ?? prop.Name;

                    yield return (prop.Name, displayName, prop.GetValue(obj), prop);
                }
            }

            public static IEnumerable<KeyValuePair<TKey, TValue>> PivotKeyValue<TKey, TValue>(
                object obj,
                Expression<Func<PropertyInfo, TKey>> keyField,
                Expression<Func<PropertyInfo, TValue>> valueField)
            {
                foreach (var prop in obj.GetType().GetProperties(InstanceBindingFlags))
                {
                    if (Attributes.HasAttribute<HideMemberAttribute>(prop))
                    {
                        continue;
                    }

                    yield return new KeyValuePair<TKey, TValue>(
                        keyField.Compile().Invoke(prop),
                        valueField.Compile().Invoke(prop)
                    );
                }
            }

            public static IEnumerable<KeyValuePair<TKey, object>> PivotPropertyAndValue<TKey>(
                object obj,
                Expression<Func<PropertyInfo, TKey>> keyField)
            {
                return PivotKeyValue(
                    obj,
                    keyField,
                    info => info.GetValue(obj));
            }

            public static T ReadValue<T>(object obj, string propertyName) => ReadValue<T>(obj, Get(obj, propertyName));

            public static T ReadValue<T>(object obj, PropertyInfo propertyInfo) => (T) propertyInfo.GetValue(obj);

            public static T ReadValueIfExists<T>(object obj, string propertyName)
            {
                var property = Get(obj, propertyName, false);
                return (T) property?.GetValue(obj);
            }

            public static void WriteValue(object obj, string propertyName, object newValue)
            {
                var property = Get(obj, propertyName);
                WriteValue(obj, property, newValue);
            }

            public static void WriteValue(object obj, PropertyInfo property, object newValue)
            {
                property.SetValue(obj, newValue);
            }

            public static void WriteValueIfExists(object obj, string propertyName, object newValue)
            {
                var property = Get(obj, propertyName, false);
                WriteValueIfExists(obj, property, newValue);
            }

            public static void WriteValueIfExists(object obj, PropertyInfo property, object newValue)
            {
                property?.SetValue(obj, newValue);
            }

            private static PropertyInfo Get(Type type, string propertyName, BindingFlags bindingFlags, bool ifNotFoundThrowAnException = true)
            {
                XContract.ArgIsNotNull(type, nameof(type));
                XContract.ArgIsNotNull(propertyName, nameof(propertyName));

                try
                {
                    var propertyInfo = type.GetProperty(propertyName, bindingFlags);
                    if (propertyInfo == null && ifNotFoundThrowAnException)
                    {
                        throw XArgumentException.Create(nameof(propertyName), $"Property '{propertyName}' not found on type {type.Name}.");
                    }

                    return propertyInfo;
                }
                catch (AmbiguousMatchException)
                {
                    return type
                        .GetProperties(InstanceBindingFlags)
                        .FirstOrDefault(info => info.Name == propertyName);
                }
            }
        }
    }
}