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
using System.Data.Common;
using Corex.Db.Contract;
using Corex.Db.Enums;
using Corex.Exceptions.Base;

namespace Corex.Db;

[Serializable]
public abstract class DbSession : IDbSession
{
    private readonly Dictionary<IDbAccess, DbContext> _accessInTransaction;

    private DbDatabase _database;

    private IDbTransactionManager _transactionManager;

    protected DbSession()
    {
        if (DbStorage.Instance.DefaultDatabase == null)
        {
            throw new XException("N�o foi poss�vel criar uma sess�o apartir da base de dados Default.");
        }

        _database = DbStorage.Instance.DefaultDatabase as DbDatabase;
        if (_database == null)
        {
            throw new XException($"A classe {GetType().Name} n�o tem suporte a Database do tipo {DbStorage.Instance.DefaultDatabase.GetType().Name}");
        }

        _accessInTransaction = new Dictionary<IDbAccess, DbContext>();
    }

    protected DbSession(IDbDatabase database)
    {
        _database = database as DbDatabase;
        if (_database == null)
        {
            throw new XException($"A classe {GetType().Name} n�o tem suporte a Database do tipo {database.GetType().Name}");
        }

        _accessInTransaction = new Dictionary<IDbAccess, DbContext>();
    }

    public IDbDatabase Database
    {
        get => _database;
        protected set => _database = value as DbDatabase;
    }

    public bool InTransaction => TransactionManager.InTransaction;

    public IDbTransactionManager TransactionManager
    {
        get => _transactionManager ??= new DbTransactionManager(this);
        protected set => _transactionManager = value;
    }

    public void BeginTransaction()
    {
        TransactionManager.StartTransaction();
    }

    public IDbContext BuildContext() => InternalBuildContext();

    public void Commit()
    {
        TransactionManager.Commit();

        ChangeContextsState(EDbContextState.Confirmed);
    }

    public abstract DbDataAdapter CreateDataAtapter(DbCommand cmd);

    public abstract DbParameter CreateReturnParameter();

    public abstract bool RetryInteractionOnDbExcepion(DbException exception);

    public void Rollback()
    {
        TransactionManager.Rollback();

        ChangeContextsState(EDbContextState.Cancelled);
    }

    IDbAccess IDbAccessProvider.GetAccess() => GetAccess();

    public DbAccess GetAccess()
    {
        var context = BuildContext() as DbContext;
        var access = new DbAccess(this, context);

        if (context != null && context.InTransaction)
        {
            _accessInTransaction.Add(access, context);
        }

        return access;
    }

    public static IDbSession NewFromDefault() => DbStorage.Instance.DefaultDatabase.NewSession();

    protected virtual IDbContext InternalBuildContext()
    {
        var context = TransactionManager.InTransaction
            ? new DbContext(TransactionManager.Connection, TransactionManager.Transaction, false)
            : new DbContext(_database.NewConnection(), true);

        return context;
    }

    internal void AddAccess(DbAccess access)
    {
        access.Session = this;
        if (access.Context.InTransaction)
        {
            _accessInTransaction.Add(access, (DbContext) access.Context);
        }
    }

    internal void RemoveAccess(IDbAccess dbAccess)
    {
        if (_accessInTransaction.ContainsKey(dbAccess))
        {
            _accessInTransaction.Remove(dbAccess);
        }
    }

    private void ChangeContextsState(EDbContextState state)
    {
        foreach (var context in _accessInTransaction.Values)
        {
            context.State = state;
        }
    }
}