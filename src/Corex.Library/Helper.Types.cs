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
using System.Reflection;
using System.Runtime.CompilerServices;
using Corex.Exceptions.Assert;
using Corex.Exceptions.Base;
using Corex.Library.Attributes;

namespace Corex.Library;

public partial class XHelper
{
    public static class Types
    {
        public static IEnumerable<ExtensionItem> ExtractGenericTypesFromImplementations(Type contractType, params Type[] types)
        {
            if (contractType == null)
            {
                throw new ArgumentNullException(nameof(contractType));
            }

            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            var countGenericArguments = contractType.GetGenericArguments().Length;

            if (countGenericArguments == 0)
            {
                throw new XException("Não é possível extrair tipos genéricos a partir de um tipo de contrato sem argumentos genéricos");
            }

// ReSharper disable LoopCanBeConvertedToQuery
            foreach (var type in types)
// ReSharper restore LoopCanBeConvertedToQuery
            {
                var genericArgs = GetGenericArguments(type, countGenericArguments);
                if (genericArgs.Length == 0)
                {
                    continue;
                }

                var genericContractType = MakeGenericType(contractType, genericArgs);

                yield return new ExtensionItem(type, contractType, genericContractType);
            }
        }

        public static Type ExtractType(Type type) => Nullable.GetUnderlyingType(type) ?? type;

        public static string FormatName(Type type)
        {
            if (type.IsGenericType)
            {
                var name = type.GetGenericTypeDefinition().Name;
                name = name.Substring(0, name.IndexOf('`'));
                return $"{name}<{type.GetGenericArguments().Select(FormatName).Aggregate((s, s1) => s + ", " + s1)}>";
            }

            return type.Name;
        }

        public static IEnumerable<Type> GetAllInAssembly(Assembly assembly, bool allowHideTypes = false)
        {
            XContract.ArgIsNotNull(assembly, nameof(assembly));

            return assembly
                .GetTypes()
                .Where(type => allowHideTypes || !Reflections.Attributes.HasAttribute<HideTypeAttribute>(type, false));
        }

        public static IEnumerable<Type> GetAllInAssembly(Assembly assembly, Type contractType, bool allowHideTypes = false)
        {
            XContract.ArgIsNotNull(assembly, nameof(assembly));
            XContract.ArgIsNotNull(contractType, nameof(contractType));

            return assembly
                .GetTypes()
                .Where(type => Is(contractType, type) && (allowHideTypes || !Reflections.Attributes.HasAttribute<HideTypeAttribute>(type, false)));
        }

        public static Type GetElementType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            return type;
        }

        public static Type GetEnumType(Type type)
        {
            if (type.IsEnum)
            {
                return type;
            }

            type = Nullable.GetUnderlyingType(type);

            return type == null || !type.IsEnum ? null : type;
        }

        public static Type GetGenericArgument(Type type) => GetGenericArguments(type).FirstOrDefault();

        public static Type GetGenericArgumentOnTop(Type type) => type.GetGenericArguments().FirstOrDefault();

        public static Type[] GetGenericArguments(Type type, int countGenericArguments = 1)
        {
            var result = type.GetGenericArguments();
            if (result.Length != countGenericArguments)
            {
                return type.BaseType != null && type.BaseType != typeof(object)
                    ? GetGenericArguments(type.BaseType, countGenericArguments)
                    : new Type[0];
            }

            return result;
        }

        public static bool IfNullable(Type type, out Type underlyingType)
        {
            underlyingType = Nullable.GetUnderlyingType(type);

            return underlyingType != null;
        }

        public static bool Is(Type contract, Type implementation)
        {
            if (contract.IsInterface)
            {
                return IsInterface(contract, implementation);
            }

            return IsClass(contract, implementation);
        }

        public static bool Is<TContract>(object obj, out TContract converted)
        {
            if (obj is TContract)
            {
                converted = (TContract) obj;
                return true;
            }

            converted = default;
            return false;
        }

        public static bool IsAnonymous(Type type)
        {
            if (type.IsGenericType)
            {
                var d = type.GetGenericTypeDefinition();
                if (d.IsClass && d.IsSealed && d.Attributes.HasFlag(TypeAttributes.NotPublic))
                {
                    var attributes = d.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false);
                    if (attributes.Length > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;

        public static bool IsSubType(Type baseType, Type implementation) => implementation.IsSubclassOf(baseType);

        public static Type MakeGenericType(Type type, params Type[] typeArguments) => type.MakeGenericType(typeArguments);

        public static Type MakeGenericTypeFromGenericType(Type type, Type concreteType)
        {
            var args = concreteType.GetGenericArguments();
            if (args.Length == 0)
            {
                return concreteType;
            }

            return MakeGenericType(type, args);
        }

        public static IEnumerable<Type> NavigateFromInheritance(Type type)
        {
            while (type != null)
            {
                yield return type;

                type = type.BaseType == typeof(object) ? null : type.BaseType;
            }
        }

        public static bool SupportNull(Type type) => type.IsClass || Nullable.GetUnderlyingType(type) != null;

        public static IEnumerable<Type> WhereIs(IEnumerable<Type> types, Type contractType)
        {
            XContract.ArgIsNotNull(types, nameof(types));
            XContract.ArgIsNotNull(contractType, nameof(contractType));

            return types.Where(type => Is(contractType, type));
        }

        private static bool IsClass(Type contract, Type implementation)
        {
            if (contract.IsAssignableFrom(implementation))
            {
                return true;
            }

            return NavigateFromInheritance(implementation)
                .Any(type =>
                    {
                        var local = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                        return contract == local;
                    });
        }

        private static bool IsInterface(Type contract, Type implementation)
        {
            if (contract.IsAssignableFrom(implementation))
            {
                return true;
            }

            if (implementation.IsInterface && implementation.IsGenericType && implementation.GetGenericTypeDefinition() == contract)
            {
                return true;
            }

            return implementation.GetInterfaces()
                .Where(i => i.IsGenericType)
                .Any(i => i.GetGenericTypeDefinition() == contract);
        }

        public struct ExtensionItem
        {
            public ExtensionItem(Type type, Type contractType, Type genericContractType)
            {
                Type = type;
                ContractType = contractType;
                GenericContractType = genericContractType;
            }

            public Type Type { get; }

            public Type ContractType { get; }

            public Type GenericContractType { get; }
        }
    }
}