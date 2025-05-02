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

namespace Corex.Exceptions.Contract;

[Serializable]
public class XArgumentException : ArgumentException
{
    private const string DefaultMessageObjectTypeInvalid = "Argumento {0} é inválido, necessáriamente ele deveria ser do tipo : {1}";
    private const string DefaultMessageNumericValueInvalid = "Argumento numérico {0} é inválido, restrição: {1}";
    private const string DefaultMessageEnumerableIsEmpty = "Argumento {0} não possui elementos";
    private const string DefaultMessageEnumInvalidValid = "Argumento {0} não possui um valor válido para o enumerador {1}";

    protected XArgumentException() { }

    protected XArgumentException(string message) : base(message) { }

    protected XArgumentException(string paramName, string message) : base(message, paramName) { }

    public static XArgumentException Create(string paramName, string message) => new XArgumentException(paramName, message);

    public static XArgumentException CreateInvalidEnumerableIsEmpty(string paramName)
        => new XArgumentException(paramName, string.Format(DefaultMessageEnumerableIsEmpty, paramName));

    public static XArgumentException CreateInvalidEnumValue(string paramName, Enum value)
        => new XArgumentException(paramName, string.Format(DefaultMessageEnumInvalidValid, paramName, value.GetType().Name));

    public static XArgumentException CreateInvalidNumericValue(string paramName, string numericRestriction)
        => new XArgumentException(paramName, string.Format(DefaultMessageNumericValueInvalid, paramName, numericRestriction));

    public static XArgumentException CreateInvalidObjectType(string paramName, Type expectedType)
        => new XArgumentException(paramName, string.Format(DefaultMessageObjectTypeInvalid, paramName, expectedType.Name));
}