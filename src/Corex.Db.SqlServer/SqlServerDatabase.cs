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
using System.Data.Common;
using System.Security;
using Corex.Db.Contract;
using Microsoft.Data.SqlClient;

namespace Corex.Db.SqlServer;

public class SqlServerDatabase : DbDatabase
{
    private readonly SqlCredential _credential;
    private readonly Lazy<SqlConnectionStringBuilder> _lazyConnStrBuilder;

    private string _connectionString;

    public SqlServerDatabase(string name, string connectionString, string user, SecureString pwd) : base(name)
    {
        _connectionString = connectionString;
        _credential = new SqlCredential(user, pwd);

        _lazyConnStrBuilder = new Lazy<SqlConnectionStringBuilder>(
            () => new SqlConnectionStringBuilder(_connectionString));
    }

    public SqlServerDatabase(string name, string connectionString) : base(name)
    {
        _connectionString = connectionString;

        _lazyConnStrBuilder = new Lazy<SqlConnectionStringBuilder>(
            () => new SqlConnectionStringBuilder(_connectionString));
    }

    public override int ConnectionTimeout
    {
        get => Builder.ConnectTimeout;
        set
        {
            if (ConnectionTimeout == value)
            {
                return;
            }

            Builder.ConnectTimeout = value;

            _connectionString = Builder.ToString();
        }
    }

    private SqlConnectionStringBuilder Builder => _lazyConnStrBuilder.Value;

    protected override DbConnection InternalCreateConnection(string connectionString) => new SqlConnection(connectionString, _credential);

    protected override IDbSession InternalCreateSqlSession() => new SqlServerSession(this);

    protected override string InternalGetConnectionString() => _connectionString;

    protected override DateTime InternalGetDateTime(int connectionTimeout = 5)
    {
        var local = new SqlConnectionStringBuilder(_connectionString)
            {
                ConnectTimeout = connectionTimeout
            };

        using var conn = InternalCreateConnection(local.ToString());
        using var comm = conn.CreateCommand();

        conn.Open();

        comm.CommandText = "SELECT CONVERT(datetimeoffset, GETDATE()) AT TIME ZONE 'E. South America Standard Time'";
        var result = comm.ExecuteScalar();
        var dt = (DateTimeOffset)result;

        return dt.DateTime;
    }
}