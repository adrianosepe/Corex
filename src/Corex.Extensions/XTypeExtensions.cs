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
using Corex.Exceptions.Assert;
using Corex.Library;
using Corex.Library.Attributes;

// ReSharper disable CheckNamespace
public static class XTypeExtensions
// ReSharper restore CheckNamespace
{
    public static TAttribute GetAttribute<TAttribute>(this Type type) where TAttribute : Attribute
    {
        XContract.ArgIsNotNull(type, nameof(type));

        return XHelper.Reflections.Attributes.Get<TAttribute>(type);
    }

    public static IEnumerable<Type> HasAttribute<TAttribute>(this IEnumerable<Type> types) where TAttribute : Attribute
    {
        return types.Where(type => XHelper.Reflections.Attributes.HasAttribute<TAttribute>(type, false));
    }

    public static bool Is(this Type implementation, params Type[] contractsTypes)
    {
        return contractsTypes.All(contract => XHelper.Types.Is(contract, implementation));
    }

    public static IEnumerable<Type> IsClass(this IEnumerable<Type> types)
    {
        return types.Where(type => type.IsClass);
    }

    public static IEnumerable<Type> IsVisible(this IEnumerable<Type> types)
    {
        return types.Where(type => !XHelper.Reflections.Attributes.HasAttribute<HideTypeAttribute>(type, false));
    }
}