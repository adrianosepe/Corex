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
using System.Data;
using System.Linq;
using Corex.Exceptions.Base;

namespace Corex.Db.Converters;

public static class TypeConvertor
{
    private static readonly List<DbTypeMapEntry> DbTypeList = new List<DbTypeMapEntry>();

    static TypeConvertor()
    {
        DbTypeList.Add(new DbTypeMapEntry(typeof(bool), DbType.Boolean, SqlDbType.Bit));
        DbTypeList.Add(new DbTypeMapEntry(typeof(byte), DbType.Double, SqlDbType.TinyInt));
        DbTypeList.Add(new DbTypeMapEntry(typeof(byte[]), DbType.Binary, SqlDbType.Image));
        DbTypeList.Add(new DbTypeMapEntry(typeof(DateTime), DbType.DateTime, SqlDbType.DateTime));
        DbTypeList.Add(new DbTypeMapEntry(typeof(decimal), DbType.Decimal, SqlDbType.Decimal));
        DbTypeList.Add(new DbTypeMapEntry(typeof(double), DbType.Double, SqlDbType.Float));
        DbTypeList.Add(new DbTypeMapEntry(typeof(Guid), DbType.Guid, SqlDbType.UniqueIdentifier));
        DbTypeList.Add(new DbTypeMapEntry(typeof(short), DbType.Int16, SqlDbType.SmallInt));
        DbTypeList.Add(new DbTypeMapEntry(typeof(int), DbType.Int32, SqlDbType.Int));
        DbTypeList.Add(new DbTypeMapEntry(typeof(long), DbType.Int64, SqlDbType.BigInt));
        DbTypeList.Add(new DbTypeMapEntry(typeof(object), DbType.Object, SqlDbType.Variant));
        DbTypeList.Add(new DbTypeMapEntry(typeof(string), DbType.String, SqlDbType.VarChar));
    }

    public static DbType ToDbType(Type type)
    {
        var entry = Find(type);
        return entry.DbType;
    }

    public static DbType ToDbType(SqlDbType sqlDbType)
    {
        var entry = Find(sqlDbType);
        return entry.DbType;
    }

    public static Type ToNetType(DbType dbType)
    {
        var entry = Find(dbType);

        return entry.Type;
    }

    public static Type ToNetType(SqlDbType sqlDbType)
    {
        var entry = Find(sqlDbType);

        return entry.Type;
    }

    public static SqlDbType ToSqlDbType(Type type)
    {
        var entry = Find(type);

        return entry.SqlDbType;
    }

    public static SqlDbType ToSqlDbType(DbType dbType)
    {
        var entry = Find(dbType);

        return entry.SqlDbType;
    }

    private static DbTypeMapEntry Find(Type type)
    {
        foreach (var t in DbTypeList.Where(t => t.Type == type || t.Type == Nullable.GetUnderlyingType(type)))
        {
            return t;
        }

        throw new XException("Referenced an unsupported Type");
    }

    private static DbTypeMapEntry Find(DbType dbType)
    {
        foreach (var t in DbTypeList.Where(t => t.DbType == dbType))
        {
            return t;
        }

        throw new XException("Referenced an unsupported DbType");
    }

    private static DbTypeMapEntry Find(SqlDbType sqlDbType)
    {
        foreach (var t in DbTypeList.Where(t => t.SqlDbType == sqlDbType))
        {
            return t;
        }

        throw new XException("Referenced an unsupported SqlDbType");
    }

    private struct DbTypeMapEntry
    {
        public readonly Type Type;
        public readonly DbType DbType;
        public readonly SqlDbType SqlDbType;

        public DbTypeMapEntry(Type type, DbType dbType, SqlDbType sqlDbType)
        {
            Type = type;
            DbType = dbType;
            SqlDbType = sqlDbType;
        }
    }
}