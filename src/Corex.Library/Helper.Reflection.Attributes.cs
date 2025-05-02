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
using System.Reflection;

namespace Corex.Library;

public partial class XHelper
{
    public partial class Reflections
    {
        public static class Attributes
        {
            public static TAttribute Get<TAttribute>(Type objType) where TAttribute : class => TypeDescriptor.GetAttributes(objType).OfType<TAttribute>().FirstOrDefault();

            public static TAttribute Get<TAttribute>(MemberInfo memberInfo, bool inherit = true) where TAttribute : class
            {
                var customs = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit);
                if (customs.Length > 0)
                {
                    return (TAttribute) customs[0];
                }

                return null;
            }

            public static TAttribute Get<TAttribute>(PropertyDescriptor descriptor) where TAttribute : class => GetMany<TAttribute>(descriptor).FirstOrDefault();

            public static TValue GetData<TAttribute, TValue>(MemberInfo memberInfo, TValue defaultValue, Func<TAttribute, TValue> extractData, bool inherit = true) where TAttribute : class
            {
                var attribute = Get<TAttribute>(memberInfo, inherit);

                return attribute == null ? defaultValue : extractData(attribute);
            }

            public static TValue GetData<TAttribute, TValue>(PropertyDescriptor descriptor, TValue defaultValue, Func<TAttribute, TValue> extractData, bool inherit = true) where TAttribute : class
            {
                var attribute = Get<TAttribute>(descriptor);

                return attribute == null ? defaultValue : extractData(attribute);
            }

            public static TValue GetData<TAttribute, TValue>(Type type, TValue defaultValue, Func<TAttribute, TValue> extractData, bool inherit = true) where TAttribute : class
            {
                var attribute = Get<TAttribute>(type, inherit);

                return attribute == null ? defaultValue : extractData(attribute);
            }

            public static TAttribute GetFromEnum<TAttribute>(object enumValue, bool inherit = true) where TAttribute : class
            {
                var fieldInfo = Enums.GetFieldInfo(enumValue);

                return Get<TAttribute>(fieldInfo);
            }

            public static IEnumerable<TAttribute> GetMany<TAttribute>(Type objType) where TAttribute : class => TypeDescriptor.GetAttributes(objType).OfType<TAttribute>();

            public static IEnumerable<TAttribute> GetMany<TAttribute>(MemberInfo memberInfo, bool inherit = true) where TAttribute : class => memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>();

            public static IEnumerable<TAttribute> GetMany<TAttribute>(PropertyDescriptor descriptor) where TAttribute : class => descriptor.Attributes.OfType<TAttribute>();

            public static bool HasAttribute<TAttribute>(Type type) where TAttribute : class => GetMany<TAttribute>(type).Any();

            public static bool HasAttribute<TAttribute>(MemberInfo memberInfo, bool inherit = true) where TAttribute : class => GetMany<TAttribute>(memberInfo, inherit).Any();

            public static bool HasAttribute<TAttribute>(PropertyDescriptor descriptor) where TAttribute : class => GetMany<TAttribute>(descriptor).Any();
        }
    }
}