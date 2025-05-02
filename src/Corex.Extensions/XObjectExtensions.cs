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
using System.Linq.Expressions;
using System.Reflection;
using Corex.Exceptions.Assert;
using Corex.Library;

// ReSharper disable CheckNamespace
public static partial class XObjectExtensions
// ReSharper restore CheckNamespace
{
    public static T As<T>(this object obj) where T : class => obj as T;
    // #region Casts

    public static object Box<T>(this T obj) where T : struct => obj;

    public static T CastOrDefault<T>(this object value, T defaultValue)
    {
        if (Equals(value, null))
        {
            return defaultValue;
        }

        return (T) value;
    }

    public static T CastTo<T>(this object value) => (T) value;

    // #region Converts

    public static T ConvertTo<T>(this object value, T defaultValue = default)
    {
        if (value == null)
        {
            return defaultValue;
        }

        var targetType = typeof(T);

        if (value.GetType() == targetType)
        {
            return (T) value;
        }

        var converter = TypeDescriptor.GetConverter(value);
        if (converter.CanConvertTo(targetType))
        {
            return (T) converter.ConvertTo(value, targetType);
        }

        converter = TypeDescriptor.GetConverter(targetType);
        if (converter.CanConvertFrom(value.GetType()))
        {
            return (T) converter.ConvertFrom(value);
        }

        return defaultValue;
    }

    // #region Expression

    public static Expression<Func<T, TResult>> ExpressionFor<T, TResult>(this T obj, Expression<Func<T, TResult>> expression) => expression;

    public static TAttribute GetAttribute<TAttribute>(this object obj) where TAttribute : Attribute
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        return XHelper.Reflections.Attributes.Get<TAttribute>(obj.GetType());
    }

    public static bool Is<T>(this object obj) where T : class => obj is T;

    public static bool Is<T>(this object obj, out T @as) where T : class
    {
        @as = obj as T;
        return @as != null;
    }

    public static bool IsDbNull(this object obj) => obj == DBNull.Value;

    public static bool IsEqual<T>(this T objA, T objB) => XHelper.Obj.AreEquals(objA, objB);

    public static bool IsNotEqual<T>(this T value, T valueToCompare) => !IsEqual(value, valueToCompare);

    public static bool IsNotNull(this object target) => !IsNull(target);

    // #region Check Is Null Or Not Is Null

    public static bool IsNull(this object obj) => XHelper.Obj.IsNull(obj);

    // #region Method

    public static object MethodInvoke(this object obj, string methodName, params object[] arguments) => XHelper.Reflections.Methods.Invoke(obj, methodName, arguments);

    public static object MethodInvokeGeneric(this object obj, string methodName, Type[] genericTypes, params object[] arguments)
        => XHelper.Reflections.Methods.InvokeGeneric(obj, methodName, genericTypes, arguments);

    // #region Property

    public static PropertyInfo PropertyInfo<T>(this T obj, Expression<Func<T, object>> expression)
    {
        XContract.ArgIsNotNull(obj, nameof(obj));
        XContract.ArgIsNotNull(expression, nameof(expression));

        return XHelper.Reflections.Properties.Get(obj, XHelper.Expressions.GetMemberName(expression));
    }

    public static object PropertyReadValue(this object obj, string propertyName) => XHelper.Reflections.Properties.ReadValue<object>(obj, propertyName);

    public static object PropertyReadValue(this object obj, PropertyInfo propertyInfo) => XHelper.Reflections.Properties.ReadValue<object>(obj, propertyInfo);

    public static object PropertyReadValueIfExists(this object obj,  string propertyName) => XHelper.Reflections.Properties.ReadValueIfExists<object>(obj, propertyName);

    public static void PropertyWriteValue(this object obj, string propertyName, object newValue) => XHelper.Reflections.Properties.WriteValue(obj, propertyName, newValue);

    public static void PropertyWriteValueIfExists(this object obj, string propertyName, object newValue) => XHelper.Reflections.Properties.WriteValueIfExists(obj, propertyName, newValue);
}