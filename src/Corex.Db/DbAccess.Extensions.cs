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
using System.Data.Common;

namespace Corex.Db;

public partial class DbAccess
{
    public static class Actions
    {
        public static T ExecuteProc<T>(string procName, params DbParameter[] parameters)
        {
            using var access = new DbAccess();
            return access.ExecProc<T, DbParameter>(procName, parameters);
        }

        public static int ExecuteProc(string procName, params DbParameter[] parameters)
        {
            using var access = new DbAccess();
            return access.ExecProc(procName, parameters);
        }

        public static T ExecuteProc<T>(DbSession session, string procName, params DbParameter[] parameters)
        {
            using var access = new DbAccess {Session = session};
            return access.ExecProc<T, DbParameter>(procName, parameters);
        }

        public static void ExecuteProcAndFill(DataTable dataTable, string procName, params DbParameter[] parameters)
        {
            using var access = new DbAccess();
            access.ExecProcAndFill(dataTable, procName, parameters);
        }

        public static void ExecuteProcAndFill(DbSession session, DataTable dataTable, string procName, params DbParameter[] parameters)
        {
            using var access = new DbAccess {Session = session};
            access.ExecProcAndFill(dataTable, procName, parameters);
        }

        public static int ExecuteText(string sql, params DbParameter[] parameters)
        {
            using var access = new DbAccess();
            return access.ExecText(sql, parameters);
        }

        public static T ExecuteText<T>(string sql, params DbParameter[] parameters) => ExecuteText<T>(sql, CommandBehavior.Default, parameters);

        public static T ExecuteText<T>(string sql, CommandBehavior behavior, params DbParameter[] parameters)
        {
            using var access = new DbAccess();
            return access.ExecText<T, DbParameter>(sql, behavior, parameters);
        }

        public static T ExecuteText<T>(DbSession session, string sql, params DbParameter[] parameters) => ExecuteText<T>(session, sql, CommandBehavior.Default, parameters);

        public static T ExecuteText<T>(DbSession session, string sql, CommandBehavior behavior, params DbParameter[] parameters)
        {
            using var access = new DbAccess {Session = session};
            return access.ExecText<T, DbParameter>(sql, behavior, parameters);
        }
    }
}