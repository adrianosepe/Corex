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
using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using Corex.Library.Data.Udt;
using Microsoft.Data.SqlClient;

// ReSharper disable CheckNamespace
public static class XSqlParameterExtensions
// ReSharper restore CheckNamespace
{
    public static SqlCommand AddParameters(this SqlCommand cmd, params SqlParameter[] args)
    {
        cmd.Parameters.AddRange(args);

        return cmd;
    }

    public static SqlParameterCollection AddParameters(this SqlParameterCollection parameters, params SqlParameter[] args)
    {
        parameters.AddRange(args);

        return parameters;
    }

    public static SqlParameter AsOutput(this SqlParameter parameter)
    {
        parameter.Direction = ParameterDirection.Output;

        return parameter;
    }

    public static SqlParameter AsReturnValue(this SqlParameter parameter)
    {
        parameter.Direction = ParameterDirection.ReturnValue;

        return parameter;
    }

    public static SqlXml AsSqlXml(this string content) => new SqlXml(XmlReader.Create(new StringReader(content)));

    public static SqlParameter Set(this SqlParameter parameter, string name, SqlDbType type, object value, bool setDbNullOnNullValue = true)
    {
        parameter.ParameterName = name;
        parameter.SqlDbType = type;
        parameter.Value = setDbNullOnNullValue && Equals(value, null) ? DBNull.Value : value;

        return parameter;
    }

    public static SqlParameter SetAsUdt(this SqlParameter parameter, string name, object value)
    {
        switch (value)
        {
            case DataTable table:
                parameter.Set(name, SqlDbType.Structured, table);
                parameter.TypeName = table.TableName;
                break;

            case UdtBase udtBase:
                parameter.Set(name, SqlDbType.Structured, udtBase.Table);
                parameter.TypeName = udtBase.Table.TableName;
                break;

            default:
                parameter.Set(name, SqlDbType.Udt, value);
                parameter.TypeName = $"dbo.{value.GetType().Name}";
                break;
        }

        return parameter;
    }

    public static SqlParameter SetAsXml(this SqlParameter parameter, string name, string value)
    {
        parameter.Set(name, SqlDbType.Xml, value.AsSqlXml());

        return parameter;
    }
}