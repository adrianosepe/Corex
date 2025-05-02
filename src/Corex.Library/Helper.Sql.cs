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
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

// ReSharper disable BuiltInTypeReferenceStyle
namespace Corex.Library;

public partial class XHelper
{
    public static class Sql
    {
        private static readonly Dictionary<Type, SqlDbType> MapClrToDbType = new Dictionary<Type, SqlDbType>
            {
                {typeof(bool), SqlDbType.Bit},
                {typeof(string), SqlDbType.NVarChar},
                {typeof(DateTime), SqlDbType.DateTime},
                {typeof(short), SqlDbType.SmallInt},
                {typeof(int), SqlDbType.Int},
                {typeof(long), SqlDbType.BigInt},
                {typeof(decimal), SqlDbType.Float},
                {typeof(double), SqlDbType.Decimal},
                {typeof(byte[]), SqlDbType.Binary}
            };

        private static readonly Dictionary<SqlDbType, Type> MapDbTypeToClr = new Dictionary<SqlDbType, Type>
            {
                {SqlDbType.BigInt, typeof(long)},
                {SqlDbType.Binary, typeof(byte[])},
                {SqlDbType.Bit, typeof(bool)},
                {SqlDbType.Char, typeof(string)},
                {SqlDbType.Date, typeof(DateTime)},
                {SqlDbType.DateTime, typeof(DateTime)},
                {SqlDbType.DateTime2, typeof(DateTime)},
                {SqlDbType.DateTimeOffset, typeof(DateTimeOffset)},
                {SqlDbType.Decimal, typeof(decimal)},
                {SqlDbType.Float, typeof(double)},
                {SqlDbType.Image, typeof(byte[])},
                {SqlDbType.Int, typeof(int)},
                {SqlDbType.Money, typeof(decimal)},
                {SqlDbType.NChar, typeof(string)},
                {SqlDbType.NText, typeof(string)},
                {SqlDbType.NVarChar, typeof(string)},
                {SqlDbType.Real, typeof(float)},
                {SqlDbType.SmallDateTime, typeof(DateTime)},
                {SqlDbType.SmallInt, typeof(short)},
                {SqlDbType.SmallMoney, typeof(decimal)},
                {SqlDbType.Text, typeof(string)},
                {SqlDbType.Time, typeof(TimeSpan)},
                {SqlDbType.Timestamp, typeof(byte[])},
                {SqlDbType.TinyInt, typeof(byte)},
                {SqlDbType.UniqueIdentifier, typeof(Guid)},
                {SqlDbType.VarBinary, typeof(byte[])},
                {SqlDbType.VarChar, typeof(string)}
            };

        public static string GetAssemblyHexString(string assemblyPath)
        {
            if (!Path.IsPathRooted(assemblyPath))
            {
                assemblyPath = Path.Combine(Environment.CurrentDirectory, assemblyPath);
            }

            var builder = new StringBuilder();
            builder.Append("0x");

            using (var stream = new FileStream(assemblyPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var currentByte = stream.ReadByte();
                while (currentByte > -1)
                {
                    builder.Append(currentByte.ToString("X2", CultureInfo.InvariantCulture));
                    currentByte = stream.ReadByte();
                }
            }

            return builder.ToString();
        }

        public static Type GetClrType(SqlDbType dbType, bool asNullable = false)
        {
            MapDbTypeToClr.TryGetValue(dbType, out var clrType);

            if (asNullable && clrType != null && clrType.IsValueType)
            {
                clrType = Types.MakeGenericType(typeof(Nullable<>), clrType);
            }

            return clrType;
        }

        public static SqlDbType GetDbType(Type clrType)
        {
            if (Types.IfNullable(clrType, out var underlyingType))
            {
                clrType = underlyingType;
            }

            MapClrToDbType.TryGetValue(clrType, out var dbType);

            return dbType;
        }

        public static TResult GetFromDbValue<TResult>(object dbValue) => Converts.ConvertTo<TResult>(dbValue);

        public static IEnumerable<string> SplitScriptByLines(string script, int countLines)
        {
            var count = 0;
            var builder = new StringBuilder();
            using (var reader = new StringReader(script))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    builder.AppendLine(line);
                    if (++count == countLines)
                    {
                        yield return builder.ToString();

                        count = 0;
                        builder.Clear();
                    }
                }
            }

            if (builder.Length > 0)
            {
                yield return builder.ToString();
            }
        }

        public static IEnumerable<string> SplitScriptByMarker(string script, string marker)
        {
            var pattern = marker;

            var matcher = new Regex(pattern, RegexOptions.Compiled);
            var start = 0;
            var batch = matcher.Match(script);

            while (batch.Success)
            {
                var end = batch.Index;

                var partialScript = script.Substring(start, end - start).Trim();

                if (Strings.HasData(partialScript))
                {
                    yield return partialScript;
                }

                start = end + batch.Length;
                batch = matcher.Match(script, start);
            }

            if (script.Length > start)
            {
                var partialScript = script.Substring(start, script.Length - start).Trim();
                if (Strings.HasData(partialScript))
                {
                    yield return partialScript;
                }
            }
        }

        public static object ToDbValue(object @object) => @object ?? DBNull.Value;
    }
}