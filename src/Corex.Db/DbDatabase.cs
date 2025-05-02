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
using Corex.Db.Contract;
using Corex.Db.Enums;
using Corex.Library;

namespace Corex.Db;

public abstract class DbDatabase : IDbDatabase
{
    private IDbSession _defaultSession;

    protected DbDatabase(string name, int commandTimeout = 60)
    {
        Name = name;

        CommandTimeout = commandTimeout;
    }

    public IDbDateTimeProvider DbDateTimeProvider { get; set; }

    public abstract int ConnectionTimeout { get; set; }

    public int CommandTimeout { get; set; }

    public string ConnectionString => InternalGetConnectionString();

    public IDbSession DefaultSession
    {
        get => _defaultSession ??= InternalCreateSqlSession();
        internal set => _defaultSession = value;
    }

    public string Name { get; }

    public EDatabaseOption Options { get; set; }

    public bool CheckConnection(out string error)
    {
        try
        {
            InternalGetDateTime();

            error = null;

            return true;
        }
        catch (DbException ex)
        {
            error = XHelper.Exceptions.GetMessageOnTopOfStack(ex);

            return false;
        }
    }

    public DbAccess GetAccess() => DefaultSession.GetAccess().CastTo<DbAccess>();

    public DateTime GetDateTime() => InternalGetDateTime();

    public DbConnection NewConnection() => InternalCreateConnection(ConnectionString);

    public IDbSession NewSession() => InternalCreateSqlSession();

    protected abstract DbConnection InternalCreateConnection(string connectionString);

    protected abstract IDbSession InternalCreateSqlSession();

    protected abstract string InternalGetConnectionString();

    protected abstract DateTime InternalGetDateTime(int connectionTimeout = 5);
}