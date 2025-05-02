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
using System.Reflection;
using Corex.Exceptions.Assert;
using Corex.Exceptions.Contract;

namespace Corex.Library;

public partial class XHelper
{
    public partial class Reflections
    {
        public static class Fields
        {
            public const BindingFlags InstanceBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            public const BindingFlags StaticBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            public static FieldInfo Get(object obj, string fieldName, bool ifNotFoundThrowAnException = true) => Get(obj.GetType(), fieldName, InstanceBindingFlags, ifNotFoundThrowAnException);

            public static FieldInfo GetStatic(Type type, string fieldName, bool ifNotFoundThrowAnException = true) => Get(type, fieldName, StaticBindingFlags, ifNotFoundThrowAnException);

            public static T ReadValue<T>(object obj, string fieldName) => (T) Get(obj, fieldName).GetValue(obj);

            public static void WriteValue(object obj, string fieldName, object newValue)
            {
                var field = Get(obj, fieldName);
                field.SetValue(obj, newValue);
            }

            public static void WriteValueIfPropertyExists(object obj, string fieldName, object newValue)
            {
                var field = Get(obj, fieldName, false);
                field?.SetValue(obj, newValue);
            }

            private static FieldInfo Get(Type type, string fieldName, BindingFlags bindingFlags, bool ifNotFoundThrowAnException = true)
            {
                XContract.ArgIsNotNull(fieldName, nameof(fieldName));
                XContract.ArgIsNotNull(type, nameof(type));

                var fieldInfo = type.GetField(fieldName, bindingFlags);

                if (fieldInfo == null && ifNotFoundThrowAnException)
                {
                    throw XArgumentException.Create(nameof(fieldName), $"Field '{fieldName}' not found on type {type.Name}.");
                }

                return fieldInfo;
            }
        }
    }
}