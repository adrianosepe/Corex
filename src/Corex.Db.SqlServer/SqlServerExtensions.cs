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

using System.Data;
using Corex.Db.Contract;
using Microsoft.Data.SqlClient;

namespace Corex.Db.SqlServer;

// ReSharper disable CheckNamespace
public static class SqlServerExtensions
// ReSharper restore CheckNamespace
{
    public static TResult ExecuteFunc<TResult>(this IDbAccess access, string funcName, params SqlParameter[] parameters) => access.ExecFunc<TResult, SqlParameter>(funcName, parameters);

    public static T ExecuteProc<T>(this IDbAccess access, string procName, params SqlParameter[] parameters) => access.ExecProc<T, SqlParameter>(procName, parameters);

    public static int ExecuteProc(this IDbAccess access, string procName, params SqlParameter[] parameters) => access.ExecProc(procName, parameters);

    public static void ExecuteProcAndFill(this IDbAccess access, DataTable dataTable, string procName, params SqlParameter[] parameters)
    {
        access.ExecProcAndFill(dataTable, procName, parameters);
    }

    public static TResult ExecuteText<TResult>(this IDbAccess access, string sql, params SqlParameter[] parameters) => access.ExecText<TResult, SqlParameter>(sql, parameters);

    public static TResult ExecuteText<TResult>(this IDbAccess access, string sql, CommandBehavior behavior, params SqlParameter[] parameters) => access.ExecText<TResult, SqlParameter>(sql, behavior, parameters);

    public static int ExecuteText(this IDbAccess access, string sql, params SqlParameter[] parameters) => access.ExecText(sql, parameters);

    public static string RenameObjectToRunAsAutonomousTransaction(this IDbAccess access, string schema, object objectName)
    {
        var builder = new SqlConnectionStringBuilder(access.Session.Database.ConnectionString);
        var initialCatalog = builder.InitialCatalog;

        return $"loopback.[{initialCatalog}].[{schema}].[{objectName}]";
    }
}