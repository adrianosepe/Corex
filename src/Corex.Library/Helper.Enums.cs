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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Corex.Library.ComponentModel.Model.Present;

// ReSharper disable UnusedMember.Global

namespace Corex.Library;

public partial class XHelper
{
    public static class Enums
    {
        public static IEnumerable<TEnum> ForEach<TEnum>(TEnum flags) where TEnum : Enum
        {
            return Enum.GetValues(flags.GetType())
                .Cast<TEnum>()
                .Where(value => IsInFlags(value, flags));
        }

        public static IEnumerable<TEnum> GetAll<TEnum>() where TEnum : Enum => Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

        public static IEnumerable GetAll(Type enumType) => Enum.GetValues(enumType);

        public static IEnumerable<TEnum> GetBrowsable<TEnum>() where TEnum : Enum
        {
            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            return values.Where(e => Reflections.Attributes.GetFromEnum<BrowsableAttribute>(e)?.Browsable ?? true);
        }

        public static string GetDescription<TEnum>(TEnum enumValue) => GetDescription(enumValue, enumValue.ToString());

        public static string GetDescription(object enumValue) => GetDescription(enumValue, enumValue.ToString());

        public static string GetDescription(object enumValue, string defaultValue)
        {
            var type = enumValue.GetType();
            if (!type.IsEnum)
            {
                return enumValue.ToString();
            }

            var fieldInfo = type.GetField(enumValue.ToString());

            string description = null;

            if (fieldInfo != null)
            {
                description = Reflections.Attributes.GetData<DescriptionAttribute, string>(fieldInfo, null, attribute => attribute.Description)
                              ?? Reflections.Attributes.GetData<DisplayAttribute, string>(fieldInfo, null, attribute => attribute.Description)
                              ?? Reflections.Attributes.GetData<IPresentDisplay, string>(fieldInfo, null, attribute => attribute.Description);
            }

            return description ?? defaultValue;
        }

        public static IEnumerable<(int id, string description)> GetDescriptions<TEnum>() where TEnum : Enum => GetDescriptions(typeof(TEnum));

        public static IEnumerable<(int id, string description)> GetDescriptions(Type type)
        {
            foreach (var @enum in GetAll(type))
            {
                yield return ((int) Convert.ChangeType(@enum, typeof(int)), GetDescription(@enum));
            }
        }

        public static string GetDisplay<TEnum>(TEnum enumValue) => GetDisplay(enumValue, enumValue.ToString());

        public static string GetDisplay(object enumValue) => GetDisplay(enumValue, enumValue.ToString());

        public static string GetDisplay(object enumValue, string defaultValue)
        {
            var type = enumValue.GetType();
            if (!type.IsEnum)
            {
                return enumValue.ToString();
            }

            var fieldInfo = type.GetField(enumValue.ToString());

            string display = null;

            if (fieldInfo != null)
            {
                display = Reflections.Attributes.GetData<DisplayAttribute, string>(fieldInfo, null, attribute => attribute.GetName())
                          ?? Reflections.Attributes.GetData<IPresentDisplay, string>(fieldInfo, null, attribute => attribute.Name);
            }

            return display ?? enumValue.ToString();
        }

        public static IEnumerable<(int id, string display)> GetDisplays(Type type)
        {
            foreach (var @enum in GetAll(type))
            {
                yield return ((int) Convert.ChangeType(@enum, typeof(int)), GetDisplay(@enum));
            }
        }

        public static FieldInfo GetFieldInfo(object enumValue)
        {
            var type = enumValue.GetType();
            if (!type.IsEnum)
            {
                return null;
            }

            return type.GetField(enumValue.ToString());
        }

        public static int GetIndex<TEnum>(object value) where TEnum : Enum
        {
            var index = -1;
            foreach (var item in GetAll<TEnum>())
            {
                if (item.Equals(value))
                {
                    return ++index;
                }

                index++;
            }

            return -1;
        }

        public static IEnumerable<TEnum> GetNotBrowsable<TEnum>() where TEnum : Enum
        {
            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            return values.Where(e => !(Reflections.Attributes.GetFromEnum<BrowsableAttribute>(e)?.Browsable ?? true));
        }

        public static TEnum GetValue<TEnum>(string name, TEnum defaultValue = default) where TEnum : struct
        {
            if (Enum.TryParse(name, true, out TEnum value))
            {
                return value;
            }

            return defaultValue;
        }

        public static bool IsDefined<TEnum>(TEnum enumValue) where TEnum : Enum
        {
            var type = typeof(TEnum);

            return type.IsEnum && Enum.IsDefined(type, enumValue);
        }

        public static bool IsDefined(object enumValue)
        {
            if (!enumValue.GetType().IsEnum)
            {
                return false;
            }

            return Enum.IsDefined(enumValue.GetType(), enumValue);
        }

        public static bool IsDefined(Enum enumValue) => Enum.IsDefined(enumValue.GetType(), enumValue);

        public static bool IsIn<TEnum>(TEnum element, params TEnum[] elements) where TEnum : Enum
        {
            if (!Enum.IsDefined(typeof(TEnum), element))
            {
                return false;
            }

            return elements.Contains(element);
        }

        public static bool IsInFlags<TEnum>(TEnum element, TEnum set) where TEnum : Enum
        {
            if (!Enum.IsDefined(typeof(TEnum), element))
            {
                return false;
            }

            var intSetValue = Convert.ToInt32(set);
            var intElement = Convert.ToInt32(element);

            return (intElement & intSetValue) == intElement;
        }

        public static bool IsSameValue<T1, T2>(T1 firstValue, T2 secondValue)
        {
            var intFirstValue = Convert.ToInt32(firstValue);
            var intSecondValue = Convert.ToInt32(secondValue);

            return intFirstValue == intSecondValue;
        }

        public static IDictionary<object, string> ToDictionary(Type enumType, bool onlyBrowsable = false)
        {
            var items = Enum.GetValues(enumType);

            var query = items.Cast<object>();
            if (onlyBrowsable)
            {
                query = query.Where(o => Reflections.Attributes.GetFromEnum<BrowsableAttribute>(o)?.Browsable ?? true);
            }

            return query.ToDictionary(item => item, GetDisplay);
        }

        public static TEnum? TryGetValue<TEnum>(string name) where TEnum : struct
        {
            if (Enum.TryParse(name, true, out TEnum value))
            {
                return value;
            }

            return null;
        }
    }
}