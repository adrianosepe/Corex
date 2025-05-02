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
using System.Linq;
using System.Reflection;
using Corex.Exceptions.Assert;
using Corex.Exceptions.Contract;

namespace Corex.Library;

public partial class XHelper
{
    public partial class Reflections
    {
        public static class Methods
        {
            public const BindingFlags InstanceNonPublicBindingFlags = BindingFlags.NonPublic | BindingFlags.NonPublic | BindingFlags.Instance;
            public const BindingFlags InstanceBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            public const BindingFlags StaticBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            public static MethodInfo Get(Type type, string methodName, bool ifNotFoundThrowAnException = true)
            {
                XContract.ArgIsNotNull(type, nameof(type));
                XContract.ArgIsNotNull(methodName, nameof(methodName));

                return Get(type, methodName, InstanceBindingFlags, ifNotFoundThrowAnException: ifNotFoundThrowAnException);
            }

            public static MethodInfo Get(object obj, string methodName, bool ifNotFoundThrowAnException = true)
            {
                XContract.ArgIsNotNull(obj, nameof(obj));
                XContract.ArgIsNotNull(methodName, nameof(methodName));

                return Get(obj.GetType(), methodName, InstanceBindingFlags, ifNotFoundThrowAnException: ifNotFoundThrowAnException);
            }

            public static MethodInfo GetStatic(Type type, string methodName, bool ifNotFoundThrowAnException = true)
            {
                XContract.ArgIsNotNull(type, nameof(type));
                XContract.ArgIsNotNull(methodName, nameof(methodName));

                return Get(type, methodName, StaticBindingFlags, ifNotFoundThrowAnException: ifNotFoundThrowAnException);
            }

            public static object Invoke(object obj, string methodName, params object[] arguments)
            {
                XContract.ArgIsNotNull(obj, nameof(obj));
                XContract.ArgIsNotNull(methodName, nameof(methodName));

                return Invoke(obj, Get(obj, methodName), arguments);
            }

            public static object Invoke(object obj, MethodInfo method, params object[] arguments)
            {
                XContract.ArgIsNotNull(obj, nameof(obj));
                XContract.ArgIsNotNull(method, nameof(method));

                return method.Invoke(obj, arguments);
            }

            public static object InvokeGeneric(object obj, string methodName, Type[] genericTypes, params object[] arguments)
            {
                return InvokeGeneric(
                    obj,
                    Get(obj.GetType(), methodName, InstanceBindingFlags, methodInfo => InternalCheckIfEquals(methodInfo, genericTypes, arguments)),
                    genericTypes, arguments);
            }

            public static object InvokeGeneric(object obj, string methodName, Type[] genericTypes, BindingFlags flags, params object[] arguments)
            {
                return InvokeGeneric(
                    obj,
                    Get(obj.GetType(), methodName, flags, methodInfo => InternalCheckIfEquals(methodInfo, genericTypes, arguments)),
                    genericTypes, arguments);
            }

            public static object InvokeGeneric(object obj, MethodInfo method, Type[] genericTypes, params object[] arguments)
            {
                XContract.ArgIsNotNull(obj, nameof(obj));
                XContract.ArgIsNotNull(method, nameof(method));
                XContract.ArgIsNotNull(genericTypes, nameof(genericTypes));

                if (genericTypes.Length == 0)
                {
                    throw XArgumentException.Create(nameof(genericTypes), "A quantidade de parâmetros genéricos não pode ser 0 (zero).");
                }

                var generic = method.MakeGenericMethod(genericTypes);
                return generic.Invoke(obj, arguments);
            }

            public static object InvokeStatic(Type objectType, string methodName, Type[] genericTypes, params object[] arguments)
            {
                XContract.ArgIsNotNull(objectType, nameof(objectType));
                XContract.ArgIsNotNull(methodName, nameof(methodName));

                var generics = objectType
                    .GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Where(info => info.Name == methodName
                                   && info.IsGenericMethod
                                   && info.GetGenericArguments().Length == genericTypes.Length)
                    .Select(
                        info =>
                            {
                                try
                                {
                                    return info.MakeGenericMethod(genericTypes);
                                }
                                catch (Exception)
                                {
                                    return null;
                                }
                            })
                    .Where(info => info != null)
                    .ToArray();

                return generics.First().Invoke(null, arguments);
            }

            public static bool MathParameters(MethodInfo method, object[] args)
            {
                XContract.ArgIsNotNull(method, nameof(method));
                XContract.ArgIsNotNull(args, nameof(args));

                var typesOfArgs = args.Select(o => o.GetType()).ToArray();

                return MathParameters(method, typesOfArgs);
            }

            public static bool MathParameters(MethodInfo method, Type[] typesOfArgs)
            {
                XContract.ArgIsNotNull(method, nameof(method));
                XContract.ArgIsNotNull(typesOfArgs, nameof(typesOfArgs));

                var parameters = method.GetParameters();

                if (parameters.Length != typesOfArgs.Length)
                {
                    return false;
                }

                if (typesOfArgs.Length == 0)
                {
                    return true;
                }

                for (var i = 0; i < parameters.Length; i++)
                {
                    var argType = typesOfArgs[i];
                    var parameterInfo = parameters[i];

                    if (IsParameterTypeOf(parameterInfo, argType))
                    {
                        continue;
                    }

                    return false;
                }

                return true;
            }

            private static MethodInfo Get(
                Type type, string methodName, BindingFlags bindingFlags,
                Func<MethodInfo, bool> checkIfEquals = null, bool ifNotFoundThrowAnException = true)
            {
                XContract.ArgIsNotNull(type, nameof(type));
                XContract.ArgIsNotNull(methodName, nameof(methodName));

                try
                {
                    var methodInfo = type.GetMethod(methodName, bindingFlags);

                    if (methodInfo == null && ifNotFoundThrowAnException)
                    {
                        throw XArgumentException.Create(nameof(methodName), $"Method '{methodName}' not found on type {type.Name}.");
                    }

                    return methodInfo;
                }
                catch (AmbiguousMatchException)
                {
                    if (checkIfEquals == null)
                    {
                        throw;
                    }

                    return type
                        .GetMethods(bindingFlags)
                        .FirstOrDefault(info => info.Name == methodName && checkIfEquals(info));
                }
            }

            private static bool InternalCheckIfEquals(MethodInfo methodInfo, Type[] genericTypes, object[] arguments) =>
                MathParameters(methodInfo, arguments) &&
                (!methodInfo.IsGenericMethod || MathGenericParameters(methodInfo, genericTypes));

            private static bool IsParameterTypeOf(ParameterInfo info, Type argType) =>
                Types.Is(info.ParameterType, argType)
                || Types.IsNullable(info.ParameterType) && argType.IsAssignableFrom(info.ParameterType.GetGenericArguments()[0]);

            private static bool MathGenericParameters(MethodInfo methodInfo, Type[] genericTypes)
            {
                XContract.ArgIsNotNull(methodInfo, nameof(methodInfo));
                XContract.ArgIsNotNull(genericTypes, nameof(genericTypes));

                if (!methodInfo.IsGenericMethod)
                {
                    return false;
                }

                var genericArguments = methodInfo.GetGenericArguments();
                if (genericArguments.Length != genericTypes.Length)
                {
                    return false;
                }

                try
                {
                    methodInfo.MakeGenericMethod(genericTypes);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}