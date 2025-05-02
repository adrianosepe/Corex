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

using Corex.Db.Contract;
using Corex.Db.Enums;
using Microsoft.Data.SqlClient;

namespace Corex.Db.SqlServer;

public static class SqlServerExceptionHandler
{
    public const int TimeoutErrorCode = 1222;
    public const int DeadlockErrorCode = 1205;

    public const int TransportLevelErrorCode = 121;
    public const int TransportLevelSeverityClass = 20;

    public static bool RetryInteraction(IDbDatabase database, SqlException exception)
    {
        switch (exception.Number)
        {
            case TimeoutErrorCode:
            case -2:
                return (database.Options & EDatabaseOption.RetryTimeout) == EDatabaseOption.RetryTimeout;

            case DeadlockErrorCode:
                return (database.Options & EDatabaseOption.RetryDeadlock) == EDatabaseOption.RetryDeadlock;

            case TransportLevelErrorCode when exception.Class == TransportLevelSeverityClass:
                SqlConnection.ClearAllPools();

                return true;

            default:
                return false;
        }
    }
}