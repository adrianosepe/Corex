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
using System.Data;

// ReSharper disable CheckNamespace
public static class XDataRowViewViewExtensions
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Gets the record value casted to the specified data type or the data types default value.
    /// </summary>
    /// <typeparam name="T">The return data type</typeparam>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static T Get<T>(this DataRowView row, string field) => row.Get(field, default(T));

    /// <summary>
    ///     Gets the record value casted to the specified data type or the specified default value.
    /// </summary>
    /// <typeparam name="T">The return data type</typeparam>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static T Get<T>(this DataRowView row, string field, T defaultValue)
    {
        var value = row[field];
        return value == DBNull.Value ? defaultValue : value.ConvertTo(defaultValue);
    }

    /// <summary>
    ///     Gets the record value casted as bool or false.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static bool GetBoolean(this DataRowView row, string field) => row.GetBoolean(field, false);

    /// <summary>
    ///     Gets the record value casted as bool or the specified default value.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static bool GetBoolean(this DataRowView row, string field, bool defaultValue)
    {
        var value = row[field];
        return value as bool? ?? defaultValue;
    }

    /// <summary>
    ///     Gets the record value casted as byte array.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static byte[] GetBytes(this DataRowView row, string field) => row[field] as byte[];

    /// <summary>
    ///     Gets the record value casted as DateTime or DateTime.MinValue.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static DateTime GetDateTime(this DataRowView row, string field) => row.GetDateTime(field, DateTime.MinValue);

    /// <summary>
    ///     Gets the record value casted as DateTime or the specified default value.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static DateTime GetDateTime(this DataRowView row, string field, DateTime defaultValue)
    {
        var value = row[field];
        return value as DateTime? ?? defaultValue;
    }

    /// <summary>
    ///     Gets the record value casted as DateTimeOffset (UTC) or DateTime.MinValue.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static DateTimeOffset GetDateTimeOffset(this DataRowView row, string field) => new DateTimeOffset(row.GetDateTime(field), TimeSpan.Zero);

    /// <summary>
    ///     Gets the record value casted as DateTimeOffset (UTC) or the specified default value.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static DateTimeOffset GetDateTimeOffset(this DataRowView row, string field, DateTimeOffset defaultValue)
    {
        var dt = row.GetDateTime(field);
        return dt != DateTime.MinValue ? new DateTimeOffset(dt, TimeSpan.Zero) : defaultValue;
    }

    /// <summary>
    ///     Gets the record value casted as decimal or 0.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static decimal GetDecimal(this DataRowView row, string field) => row.GetDecimal(field, 0);

    /// <summary>
    ///     Gets the record value casted as decimal or the specified default value.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static decimal GetDecimal(this DataRowView row, string field, long defaultValue)
    {
        var value = row[field];
        return value as decimal? ?? defaultValue;
    }

    /// <summary>
    ///     Gets the record value casted as Guid or Guid.Empty.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static Guid GetGuid(this DataRowView row, string field)
    {
        var value = row[field];
        return value as Guid? ?? Guid.Empty;
    }

    /// <summary>
    ///     Gets the record value casted as int or 0.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static int GetInt32(this DataRowView row, string field) => row.GetInt32(field, 0);

    /// <summary>
    ///     Gets the record value casted as int or the specified default value.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static int GetInt32(this DataRowView row, string field, int defaultValue)
    {
        var value = row[field];
        return value as int? ?? defaultValue;
    }

    /// <summary>
    ///     Gets the record value casted as long or 0.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static long GetInt64(this DataRowView row, string field) => row.GetInt64(field, 0);

    /// <summary>
    ///     Gets the record value casted as long or the specified default value.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static long GetInt64(this DataRowView row, string field, int defaultValue)
    {
        var value = row[field];
        return value as long? ?? defaultValue;
    }

    /// <summary>
    ///     Gets the record value casted as string or null.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static string GetString(this DataRowView row, string field) => row.GetString(field, null);

    /// <summary>
    ///     Gets the record value casted as string or the specified default value.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static string GetString(this DataRowView row, string field, string defaultValue)
    {
        var value = row[field];
        var s = value as string;
        return s ?? defaultValue;
    }

    /// <summary>
    ///     Gets the record value as Type class instance or null.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static Type GetType(this DataRowView row, string field) => row.GetType(field, null);

    /// <summary>
    ///     Gets the record value as Type class instance or the specified default value.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static Type GetType(this DataRowView row, string field, Type defaultValue)
    {
        var classType = row.GetString(field);
        if (!classType.IsNotEmpty())
        {
            return defaultValue;
        }

        var type = Type.GetType(classType);
        return type != null ? type : defaultValue;
    }

    /// <summary>
    ///     Gets the record value as class instance from a type name or null.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static object GetTypeInstance(this DataRowView row, string field) => row.GetTypeInstance(field, null);

    /// <summary>
    ///     Gets the record value as class instance from a type name or the specified default type.
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static object GetTypeInstance(this DataRowView row, string field, Type defaultValue)
    {
        var type = row.GetType(field, defaultValue);
        return type != null ? Activator.CreateInstance(type) : null;
    }

    /// <summary>
    ///     Gets the record value as class instance from a type name or null.
    /// </summary>
    /// <typeparam name="T">The type to be casted to</typeparam>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static T GetTypeInstance<T>(this DataRowView row, string field) where T : class => row.GetTypeInstance(field, null) as T;

    /// <summary>
    ///     Gets the record value as class instance from a type name or the specified default type.
    /// </summary>
    /// <typeparam name="T">The type to be casted to</typeparam>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="type">The type.</param>
    /// <returns>The record value</returns>
    public static T GetTypeInstanceSafe<T>(this DataRowView row, string field, Type type) where T : class
    {
        var instance = row.GetTypeInstance(field, null) as T;
        return instance ?? Activator.CreateInstance(type) as T;
    }

    /// <summary>
    ///     Gets the record value as class instance from a type name or an instance from the specified type.
    /// </summary>
    /// <typeparam name="T">The type to be casted to</typeparam>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static T GetTypeInstanceSafe<T>(this DataRowView row, string field) where T : class, new()
    {
        var instance = row.GetTypeInstance(field, null) as T;
        return instance ?? new T();
    }

    /// <summary>
    ///     Determines whether the record value is DBNull.Value
    /// </summary>
    /// <param name="row">The data row.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>
    ///     <c>true</c> if the value is DBNull.Value; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsDbNull(this DataRowView row, string field)
    {
        var value = row[field];
        return value == DBNull.Value;
    }
}