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
using Corex.Exceptions.Contract;
using LinqExpressions = System.Linq.Expressions;

namespace Corex.Library;

public partial class XHelper
{
    public static class Expressions
    {
        public static LinqExpressions.Expression<Func<TModel, bool>> BuildExpression<TModel>(LinqExpressions.Expression<Func<TModel, bool>> value) => value;

        public static LinqExpressions.Expression<Func<TModel, T>> GenerateMemberExpression<TModel, T>(PropertyInfo propertyInfo)
        {
            var entityParam = LinqExpressions.Expression.Parameter(typeof(TModel), "e");
            LinqExpressions.Expression columnExpr = LinqExpressions.Expression.Property(entityParam, propertyInfo);

            if (propertyInfo.PropertyType != typeof(T))
            {
                columnExpr = LinqExpressions.Expression.Convert(columnExpr, typeof(T));
            }

            return LinqExpressions.Expression.Lambda<Func<TModel, T>>(columnExpr, entityParam);
        }

        public static LinqExpressions.Expression<Func<TModel, object>> GenerateMemberExpression<TModel>(PropertyInfo propertyInfo)
        {
            var entityParam = LinqExpressions.Expression.Parameter(typeof(TModel), "e");
            LinqExpressions.Expression columnExpr = LinqExpressions.Expression.Property(entityParam, propertyInfo);

            if (!propertyInfo.PropertyType.IsClass)
            {
                columnExpr = LinqExpressions.Expression.Convert(columnExpr, typeof(object));
            }

            return LinqExpressions.Expression.Lambda<Func<TModel, object>>(columnExpr, entityParam);
        }

        public static LinqExpressions.LambdaExpression GenerateMemberExpression(Type modelType, PropertyInfo propertyInfo)
        {
            var entityParam = LinqExpressions.Expression.Parameter(modelType, "e");
            LinqExpressions.Expression columnExpr = LinqExpressions.Expression.Property(entityParam, propertyInfo);

            if (!propertyInfo.PropertyType.IsClass)
            {
                columnExpr = LinqExpressions.Expression.Convert(columnExpr, typeof(object));
            }

            return LinqExpressions.Expression.Lambda(modelType, columnExpr, entityParam);
        }

        public static LinqExpressions.LambdaExpression GenerateMemberExpression(PropertyInfo propertyInfo)
        {
// ReSharper disable AssignNullToNotNullAttribute
            var entityParam = LinqExpressions.Expression.Parameter(propertyInfo.DeclaringType, "e");
// ReSharper restore AssignNullToNotNullAttribute
            LinqExpressions.Expression columnExpr = LinqExpressions.Expression.Property(entityParam, propertyInfo);

            if (!propertyInfo.PropertyType.IsClass)
            {
                columnExpr = LinqExpressions.Expression.Convert(columnExpr, typeof(object));
            }

            return LinqExpressions.Expression.Lambda(columnExpr, entityParam);
        }

        public static LinqExpressions.ExpressionType GetExpressionType(LinqExpressions.LambdaExpression memberSelector) => memberSelector.Body.NodeType;

        public static string GetMemberName(LinqExpressions.LambdaExpression expression)
        {
            static string NameSelector(LinqExpressions.Expression e)
            {
                switch (e.NodeType)
                {
                    case LinqExpressions.ExpressionType.Parameter:
                        return e.CastTo<LinqExpressions.ParameterExpression>().Name;

                    case LinqExpressions.ExpressionType.MemberAccess:
                        return e.CastTo<LinqExpressions.MemberExpression>().Member.Name;

                    case LinqExpressions.ExpressionType.Call:
                        return e.CastTo<LinqExpressions.MethodCallExpression>().Method.Name;

                    case LinqExpressions.ExpressionType.Convert:
                    case LinqExpressions.ExpressionType.ConvertChecked:
                        return NameSelector(e.CastTo<LinqExpressions.UnaryExpression>().Operand);

                    case LinqExpressions.ExpressionType.Invoke:
                        return NameSelector(e.CastTo<LinqExpressions.InvocationExpression>().Expression);

                    case LinqExpressions.ExpressionType.ArrayLength:
                        return "Length";

                    default:
                        throw new Exception("Not a proper member selector");
                }
            }

            return NameSelector(expression.Body);
        }

        public static string GetMemberName<T>(LinqExpressions.Expression<Func<T, object>> expression) => GetMemberName(expression.CastTo<LinqExpressions.LambdaExpression>());

        public static Type GetMemberType<T>(LinqExpressions.Expression<Func<T, object>> expression)
        {
            Func<LinqExpressions.Expression, Type> typeSelector = null;

            typeSelector = e =>
                {
                    switch (e.NodeType)
                    {
                        case LinqExpressions.ExpressionType.Convert:
                        case LinqExpressions.ExpressionType.ConvertChecked:
                            return typeSelector(e.CastTo<LinqExpressions.UnaryExpression>().Operand);

                        case LinqExpressions.ExpressionType.MemberAccess:
                            return e.Type;

                        default:
                            throw new Exception("Not a proper member selector");
                    }
                };

            return typeSelector(expression.Body);
        }

        public static object GetMemberValue<T>(LinqExpressions.Expression<Func<T, object>> memberSelector, T obj) => memberSelector.Compile()(obj);

        public static TValue GetMemberValue<T, TValue>(LinqExpressions.Expression<Func<T, TValue>> memberSelector, T obj) => memberSelector.Compile()(obj);

        public static TItem GetMemberValueOfType<T, TItem>(LinqExpressions.Expression<Func<T, TItem>> memberSelector, T obj) => memberSelector.Compile()(obj);

        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(LinqExpressions.Expression<Func<TSource, TProperty>> expression)
        {
            var type = typeof(TSource);
            PropertyInfo propInfo;
            var member = expression.Body as LinqExpressions.MemberExpression;
            if (member == null)
            {
                propInfo = type.GetProperty(GetMemberName(expression));

                if (propInfo == null)
                {
                    throw new ArgumentException($"Expression '{expression}' refers to a method, not a property.");
                }
            }
            else
            {
                propInfo = member.Member as PropertyInfo;
            }

            if (propInfo == null)
            {
                throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");
            }

            if (type != propInfo.ReflectedType && (propInfo.ReflectedType == null || !type.IsSubclassOf(propInfo.ReflectedType)))
            {
                throw new ArgumentException($"Expresion '{expression}' refers to a property that is not from type {type}.");
            }

            return propInfo;
        }

        public static bool IsConstantOf<T>(LinqExpressions.LambdaExpression expression) =>
            GetExpressionType(expression) == LinqExpressions.ExpressionType.Convert &&
            expression.Body is LinqExpressions.UnaryExpression &&
            expression.Body.CastTo<LinqExpressions.UnaryExpression>().Operand.NodeType == LinqExpressions.ExpressionType.Constant &&
            expression.Body.CastTo<LinqExpressions.UnaryExpression>().Operand.Type == typeof(T);

        public static bool IsEquals(LinqExpressions.LambdaExpression exp1, LinqExpressions.LambdaExpression exp2) =>
            exp1.Type == exp2.Type &&
            exp1.NodeType == exp2.NodeType &&
            GetMemberName(exp1) == GetMemberName(exp2);

        public static bool IsEquals(string exp1AsString, LinqExpressions.LambdaExpression exp2) => exp1AsString == GetMemberName(exp2);

        public static TValue ReadValue<T, TValue>(T target, LinqExpressions.Expression<Func<T, TValue>> expression)
        {
            LinqExpressions.MemberExpression memberExpression;

            var body = expression.Body;
            if (body is LinqExpressions.UnaryExpression)
            {
                memberExpression = body.CastTo<LinqExpressions.UnaryExpression>().Operand.CastTo<LinqExpressions.MemberExpression>();
            }
            else if (body is LinqExpressions.MemberExpression)
            {
                memberExpression = body.As<LinqExpressions.MemberExpression>();
            }
            else
            {
                throw XArgumentException.Create(nameof(expression), "Expression not supported.");
            }

            var property = memberExpression?.Member.As<PropertyInfo>();
            if (property == null)
            {
                throw XArgumentException.Create(nameof(expression), "Expression not supported.");
            }

            return (TValue) property?.GetValue(target);
        }

        public static void WriteValue<T, TValue>(T target, LinqExpressions.Expression<Func<T, TValue>> memberLamda, TValue value)
        {
            var memberSelectorExpression = memberLamda.Body as LinqExpressions.MemberExpression;
            var property = memberSelectorExpression?.Member as PropertyInfo;
            property?.SetValue(target, value, null);
        }
    }
}