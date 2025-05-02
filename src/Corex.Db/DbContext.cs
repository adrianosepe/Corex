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
using System.Data.Common;
using Corex.Db.Contract;
using Corex.Db.Enums;
using Corex.Exceptions.Base;

namespace Corex.Db;

public class DbContext : IDbContext
{
    private readonly bool _allowConnectionDispose;

    private bool _disposed;

    public DbContext(DbConnection connection, bool allowConnectionDispose)
    {
        Connection = connection;
        State = EDbContextState.OutTransaction;

        _allowConnectionDispose = allowConnectionDispose;
    }

    public DbContext(DbConnection connection, DbTransaction transaction, bool allowConnectionDispose) : this(connection, allowConnectionDispose)
    {
        Transaction = transaction;
        State = EDbContextState.InTransaction;

        _allowConnectionDispose = allowConnectionDispose;
    }

    public DbConnection Connection { get; private set; }

    public bool InTransaction => Transaction != null;

    public bool IsAvailable => State != EDbContextState.Disposed && (State == EDbContextState.OutTransaction || State == EDbContextState.InTransaction);

    public EDbContextState State { get; internal set; }

    public DbTransaction Transaction { get; }

    public void Close()
    {
        if (!InTransaction)
        {
            Connection.Close();
        }
    }

    public DbCommand CreateCommand()
    {
        ThrowExceptionIsNotAvailable();

        var command = Connection.CreateCommand();
        command.Transaction = Transaction;

        return command;
    }

    public void Dispose()
    {
        Dispose(true);
    }

    public void Open()
    {
        ThrowExceptionIsNotAvailable();

        if (Connection.State == ConnectionState.Closed)
        {
            Connection.Open();
        }
    }

    ~DbContext()
    {
        Dispose(false);
    }

    internal void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        if (!InTransaction)
        {
            if (disposing && _allowConnectionDispose)
            {
                Connection.Dispose();
            }

            Connection = null;
        }

        State = EDbContextState.Disposed;

        if (disposing)
        {
            GC.SuppressFinalize(this);
        }
    }

    private void ThrowExceptionIsNotAvailable()
    {
        if (!IsAvailable)
        {
            throw new XException("A contexto de transa��o foi finalizado tornando-se indispon�vel para essa opera��o.");
        }
    }
}