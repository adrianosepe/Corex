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
using System.Linq;

namespace Corex.Library.ComponentModel.Model.Validation.Annotations.Bussines;

public sealed class XCreditCardAttribute : XDataTypeAttribute
{
    private const string Message = "O campo {0} não possui um número de cartão de crédito válido.";

    public XCreditCardAttribute() : base(EDataType.Custom)
    {
        ErrorMessage = Message;
    }

    protected override bool InternalIsValid(object value)
    {
        if (value == null)
        {
            return true;
        }

        if (!(value is string str1))
        {
            return false;
        }

        var str2 = str1.Replace("-", string.Empty).Replace(" ", string.Empty);
        var num1 = 0;
        var flag = false;

        foreach (var ch in str2.Reverse())
        {
            if (ch < 48 || ch > 57)
            {
                return false;
            }

            var num2 = (ch - 48) * (flag ? 2 : 1);
            flag = !flag;
            while (num2 > 0)
            {
                num1 += num2 % 10;
                num2 /= 10;
            }
        }

        return num1 % 10 == 0;
    }
}