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
using System.Text;
using Corex.Exceptions.Base;

namespace Corex.Db;

public class DbBathProcess
{
    private readonly StringBuilder _buffer;
    private readonly int _maxLengthParameter;
    private readonly Action<string> _methodExecute;

    private readonly object _objSync = new object();
    private readonly char _parameterSeparator;

    public DbBathProcess(int maxLengthParameter, char parameterSeparator, Action<string> methodExecute)
    {
        _maxLengthParameter = maxLengthParameter;
        _parameterSeparator = parameterSeparator;
        _methodExecute = methodExecute;

        _buffer = new StringBuilder();
    }

    public void AddParameterPart(string parameterPart)
    {
        if (string.IsNullOrEmpty(parameterPart))
        {
            throw new XException("N�o � poss�vel adicionar um par�metro parcial nulo ou vazio.");
        }

        lock (_objSync)
        {
            if (_buffer.Length + parameterPart.Length > _maxLengthParameter)
            {
                _methodExecute(_buffer.ToString());
                _buffer.Length = 0;
            }

            _buffer.Append(parameterPart + _parameterSeparator);
        }
    }

    public void Flush()
    {
        lock (_objSync)
        {
            if (_buffer.Length <= 0)
            {
                return;
            }

            _methodExecute(_buffer.ToString());
            _buffer.Length = 0;
        }
    }
}