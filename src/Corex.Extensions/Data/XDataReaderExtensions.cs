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
using System.Linq;

// ReSharper disable CheckNamespace
public static class XDataReaderExtensions
// ReSharper restore CheckNamespace
{
    public static DateTime AsDateTime(this IDataReader dr, string columnName) => (DateTime) dr[columnName];

    /// <summary>
    ///     Gets the record value casted to the specified data type or the data types default value.
    /// </summary>
    /// <typeparam name="T">The return data type</typeparam>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static T Get<T>(this IDataReader reader, string field) => reader.Get(field, default(T));

    /// <summary>
    ///     Gets the record value casted to the specified data type or the specified default value.
    /// </summary>
    /// <typeparam name="T">The return data type</typeparam>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static T Get<T>(this IDataReader reader, string field, T defaultValue)
    {
        var value = reader[field];
        if (value == DBNull.Value)
        {
            return defaultValue;
        }

        if (value is T variable)
        {
            return variable;
        }

        return value.ConvertTo(defaultValue);
    }

    /// <summary>
    ///     Gets the record value casted as bool or false.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static bool GetBoolean(this IDataReader reader, string field) => reader.GetBoolean(field, false);

    /// <summary>
    ///     Gets the record value casted as bool or the specified default value.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static bool GetBoolean(this IDataReader reader, string field, bool defaultValue)
    {
        var value = reader[field];
        return value as bool? ?? defaultValue;
    }

    public static byte GetByte(this IDataReader dr, string columnName) => (byte) dr[columnName];

    /// <summary>
    ///     Gets the record value casted as byte array.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static byte[] GetBytes(this IDataReader reader, string field) => reader[field] as byte[];

    public static char GetChar(this IDataReader dr, string columnName) => GetString(dr, columnName).First();

    /// <summary>
    ///     Gets the record value casted as DateTime or DateTime.MinValue.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static DateTime GetDateTime(this IDataReader reader, string field) => reader.GetDateTime(field, DateTime.MinValue);

    /// <summary>
    ///     Gets the record value casted as DateTime or the specified default value.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static DateTime GetDateTime(this IDataReader reader, string field, DateTime defaultValue)
    {
        var value = reader[field];
        return value as DateTime? ?? defaultValue;
    }

    /// <summary>
    ///     Gets the record value casted as DateTimeOffset (UTC) or DateTime.MinValue.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static DateTimeOffset GetDateTimeOffset(this IDataReader reader, string field) => new DateTimeOffset(reader.GetDateTime(field), TimeSpan.Zero);

    /// <summary>
    ///     Gets the record value casted as DateTimeOffset (UTC) or the specified default value.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static DateTimeOffset GetDateTimeOffset(this IDataReader reader, string field, DateTimeOffset defaultValue)
    {
        var dt = reader.GetDateTime(field);
        return dt != DateTime.MinValue ? new DateTimeOffset(dt, TimeSpan.Zero) : defaultValue;
    }

    /// <summary>
    ///     Gets the record value casted as decimal or 0.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static decimal GetDecimal(this IDataReader reader, string field) => reader.GetDecimal(field, 0);

    /// <summary>
    ///     Gets the record value casted as decimal or the specified default value.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static decimal GetDecimal(this IDataReader reader, string field, long defaultValue)
    {
        var value = reader[field];
        return value as decimal? ?? defaultValue;
    }

    public static double GetDouble(this IDataReader dr, string columnName) => (double) dr[columnName];

    public static float GetFloat(this IDataReader dr, string columnName) => (float) dr[columnName];

    /// <summary>
    ///     Gets the record value casted as Guid or Guid.Empty.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static Guid GetGuid(this IDataReader reader, string field)
    {
        var value = reader[field];
        return value as Guid? ?? Guid.Empty;
    }

    public static int GetInt(this IDataReader dr, string columnName) => (int) dr[columnName];

    /// <summary>
    ///     Gets the record value casted as int or 0.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static int GetInt32(this IDataReader reader, string field) => reader.GetInt32(field, 0);

    /// <summary>
    ///     Gets the record value casted as int or the specified default value.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static int GetInt32(this IDataReader reader, string field, int defaultValue)
    {
        var value = reader[field];
        return value as int? ?? defaultValue;
    }

    /// <summary>
    ///     Gets the record value casted as long or 0.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static long GetInt64(this IDataReader reader, string field) => reader.GetInt64(field, 0);

    /// <summary>
    ///     Gets the record value casted as long or the specified default value.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static long GetInt64(this IDataReader reader, string field, int defaultValue)
    {
        var value = reader[field];
        return value as long? ?? defaultValue;
    }

    /// <summary>
    ///     Gets the record value casted as bool or null.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static bool? GetNullableBoolean(this IDataReader reader, string field)
    {
        var value = reader[field];
        return value as bool?;
    }

    /// <summary>
    ///     Gets the record value casted as DateTime or null.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static DateTime? GetNullableDateTime(this IDataReader reader, string field)
    {
        var value = reader[field];
        return value as DateTime?;
    }

    /// <summary>
    ///     Gets the record value casted as DateTimeOffset (UTC) or null.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static DateTimeOffset? GetNullableDateTimeOffset(this IDataReader reader, string field)
    {
        var dt = reader.GetNullableDateTime(field);
        return dt != null ? (DateTimeOffset?) new DateTimeOffset(dt.Value, TimeSpan.Zero) : null;
    }

    /// <summary>
    ///     Gets the record value casted as decimal or null.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static decimal? GetNullableDecimal(this IDataReader reader, string field)
    {
        var value = reader[field];
        return value as decimal?;
    }

    /// <summary>
    ///     Gets the record value casted as Guid? or null.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static Guid? GetNullableGuid(this IDataReader reader, string field)
    {
        var value = reader[field];

        return value as Guid?;
    }

    /// <summary>
    ///     Gets the record value casted as int or null.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static int? GetNullableInt32(this IDataReader reader, string field)
    {
        var value = reader[field];
        return value as int?;
    }

    /// <summary>
    ///     Gets the record value casted as long or null.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static long? GetNullableInt64(this IDataReader reader, string field)
    {
        var value = reader[field];
        return value as long?;
    }

    public static short GetShort(this IDataReader dr, string columnName) => (short) dr[columnName];

    /// <summary>
    ///     Gets the record value casted as string or null.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static string GetString(this IDataReader reader, string field) => reader.GetString(field, null);

    /// <summary>
    ///     Gets the record value casted as string or the specified default value.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static string GetString(this IDataReader reader, string field, string defaultValue)
    {
        var value = reader[field];
        var s = value as string;

        return s ?? defaultValue;
    }

    /// <summary>
    ///     Gets the record value as Type class instance or null.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static Type GetType(this IDataReader reader, string field) => reader.GetType(field, null);

    /// <summary>
    ///     Gets the record value as Type class instance or the specified default value.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static Type GetType(this IDataReader reader, string field, Type defaultValue)
    {
        var classType = reader.GetString(field);
        if (classType.IsNotEmpty())
        {
            var type = Type.GetType(classType);
            if (type != null)
            {
                return type;
            }
        }

        return defaultValue;
    }

    /// <summary>
    ///     Gets the record value as class instance from a type name or null.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static object GetTypeInstance(this IDataReader reader, string field) => reader.GetTypeInstance(field, null);

    /// <summary>
    ///     Gets the record value as class instance from a type name or the specified default type.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns>The record value</returns>
    public static object GetTypeInstance(this IDataReader reader, string field, Type defaultValue)
    {
        var type = reader.GetType(field, defaultValue);
        return type != null ? Activator.CreateInstance(type) : null;
    }

    /// <summary>
    ///     Gets the record value as class instance from a type name or null.
    /// </summary>
    /// <typeparam name="T">The type to be casted to</typeparam>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static T GetTypeInstance<T>(this IDataReader reader, string field) where T : class => reader.GetTypeInstance(field, null) as T;

    /// <summary>
    ///     Gets the record value as class instance from a type name or the specified default type.
    /// </summary>
    /// <typeparam name="T">The type to be casted to</typeparam>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <param name="type">The type.</param>
    /// <returns>The record value</returns>
    public static T GetTypeInstanceSafe<T>(this IDataReader reader, string field, Type type) where T : class
    {
        var instance = reader.GetTypeInstance(field, null) as T;
        return instance ?? Activator.CreateInstance(type) as T;
    }

    /// <summary>
    ///     Gets the record value as class instance from a type name or an instance from the specified type.
    /// </summary>
    /// <typeparam name="T">The type to be casted to</typeparam>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>The record value</returns>
    public static T GetTypeInstanceSafe<T>(this IDataReader reader, string field) where T : class, new()
    {
        var instance = reader.GetTypeInstance(field, null) as T;
        return instance ?? new T();
    }

    /// <summary>
    ///     Returns the index of a column by name (case insensitive) or -1
    /// </summary>
    /// <param name="this"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static int IndexOf(this IDataRecord @this, string name)
    {
        for (var i = 0; i < @this.FieldCount; i++)
        {
            if (string.Compare(@this.GetName(i), name, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    ///     Determines whether the record value is DBNull.Value
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="field">The name of the record field.</param>
    /// <returns>
    ///     <c>true</c> if the value is DBNull.Value; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsDbNull(this IDataReader reader, string field)
    {
        var value = reader[field];
        return value == DBNull.Value;
    }

    /// <summary>
    ///     Reads all all records from a data reader and performs an action for each.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <param name="action">The action to be performed.</param>
    /// <returns>
    ///     The count of actions that were performed.
    /// </returns>
    public static int ReadAll(this IDataReader reader, Action<IDataReader> action)
    {
        var count = 0;
        while (reader.Read())
        {
            action(reader);
            count++;
        }

        return count;
    }
}