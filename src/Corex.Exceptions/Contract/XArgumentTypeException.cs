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
using Corex.Exceptions.Assert;
using Corex.Exceptions.Base;

namespace Corex.Exceptions.Contract;

public class XArgumentTypeException : XException
{
    private const string DefaultMessage = "The argument {0} is incompatible with expected type!\n\nWait.Type: {1}\nReceive.Type: {2}";

    protected XArgumentTypeException(string argumentName, Type wait, Type received = null)
        : base(string.Format(DefaultMessage, argumentName, wait.FullName, GetTypeName(received)))
    {
        XContract.ArgIsNotNull(argumentName, nameof(argumentName));
        XContract.ArgIsNotNull(wait, nameof(wait));
        XContract.ArgIsNotNull(received, nameof(received));

        ArgumentName = argumentName;
        Wait = wait;
        Received = received;
    }

    public string ArgumentName { get; }

    public Type Received { get; }

    public Type Wait { get; }

    public static XArgumentTypeException Create(string argumentName, Type wait)
    {
        if (argumentName == null)
        {
            throw new ArgumentNullException(nameof(argumentName));
        }

        if (wait == null)
        {
            throw new ArgumentNullException(nameof(wait));
        }

        return new XArgumentTypeException(argumentName, wait, typeof(NullReference));
    }

    public static XArgumentTypeException CreateFromObject(string argumentName, Type wait, object received)
    {
        if (argumentName == null)
        {
            throw new ArgumentNullException(nameof(argumentName));
        }

        if (wait == null)
        {
            throw new ArgumentNullException(nameof(wait));
        }

        return new XArgumentTypeException(argumentName, wait, received?.GetType() ?? typeof(NullReference));
    }

    public static XArgumentTypeException CreateFromType(string argumentName, Type wait, Type received)
    {
        if (argumentName == null)
        {
            throw new ArgumentNullException(nameof(argumentName));
        }

        if (wait == null)
        {
            throw new ArgumentNullException(nameof(wait));
        }

        if (received == null)
        {
            throw new ArgumentNullException(nameof(received));
        }

        return new XArgumentTypeException(argumentName, wait, received);
    }

    // ReSharper disable LocalizableElement
    private static string GetTypeName(Type type) => type == typeof(NullReference) ? "[Object is NULL]" : type.FullName;
    // ReSharper restore LocalizableElement

    private static class NullReference { }
}