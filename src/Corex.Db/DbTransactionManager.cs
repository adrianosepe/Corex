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
using Corex.Db.Contract;

namespace Corex.Db;

public class DbTransactionManager : IDbTransactionManager
{
    public DbTransactionManager(IDbSession session)
    {
        Session = session;
    }

    public DbConnection Connection { get; private set; }

    public bool InTransaction => Transaction != null;

    public IDbSession Session { get; }

    public DbTransaction Transaction { get; private set; }

    public void Commit()
    {
        if (!InTransaction)
        {
            return;
        }

        Transaction.Commit();

        Clean();
    }

    public void Rollback()
    {
        if (!InTransaction)
        {
            return;
        }

        Transaction.Rollback();

        Clean();
    }

    public void StartTransaction()
    {
        if (InTransaction)
        {
            return;
        }

        Connection = Session.Database.NewConnection();

        if (Connection.State == ConnectionState.Closed)
        {
            Connection.Open();
        }

        Transaction = Connection.BeginTransaction();
    }

    private void Clean()
    {
        Connection.Dispose();
        Transaction.Dispose();

        Connection = null;
        Transaction = null;
    }
}