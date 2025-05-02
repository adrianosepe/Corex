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

namespace Corex.Exceptions.Base;

public class XDbException : XException
{
    protected const string Prefix = "XDbException:";

    public XDbException(Exception ex) : base(ex.Message.Replace(Prefix, string.Empty), ex)
    {
        if (!ex.Message.StartsWith(Prefix))
        {
            throw new XException(
                "A exce��o XDbException requer como base uma exce��o disparada pelo " +
                "banco de dados, e esta exce��o ser� assumida como par�metro.");
        }
    }

    protected XDbException(Exception ex, string message) : base(message.Replace(Prefix, string.Empty), ex) { }

    protected XDbException(string exception, Exception innerException = null) : base($"{Prefix}{exception}", innerException) { }

    public static string FormatMessage(Exception exception)
    {
        if (exception is DbException && exception.Message.StartsWith(Prefix))
        {
            return exception.Message.Replace(Prefix, string.Empty);
        }

        return exception.Message;
    }

    public static bool IsOne(Exception exception, out XDbException xException)
    {
        xException = null;

        if (exception is not DbException sqlEx)
        {
            return xException != null;
        }

        if (exception.Message.StartsWith(Prefix))
        {
            xException = new XDbException(sqlEx);
        }
        else
        {
            var result = exception.Message.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            if (result.Length > 1 && result[1].StartsWith("XDbException:"))
            {
                xException = new XDbException(sqlEx, result[1].Replace("XDbException:", string.Empty));
            }
        }

        return xException != null;
    }
}