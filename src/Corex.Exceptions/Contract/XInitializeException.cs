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

using Corex.Exceptions.Base;

namespace Corex.Exceptions.Contract;

public class XInitializeException : XException
{
    protected XInitializeException(string message) : base(message) { }

    public static XInitializeException Create(string message) => new XInitializeException(message);

    public static XInitializeException CreateForNullReference(object initializingOject, string requireMemberName)
        => new XInitializeException(
            $"The type {initializingOject.GetType().Name} is initialized, but the member '{requireMemberName}' is required and NULL!");

    public static XInitializeException InvalidInheritance(object initializingOject, string inheritanceMethodName)
        => new XInitializeException(
            $"The type {initializingOject.GetType().Name} don't supports the initialization from this member '{inheritanceMethodName}', try use the overloaded!");

    public static XInitializeException PropertyButRequireMemberIsNull(object initializingPropertyOf, string propertyName, string requireMemberName)
        => new XInitializeException(
            $"The type {initializingPropertyOf.GetType().Name} had a property {propertyName} initialized, but the member '{requireMemberName}' is required and NULL!");

    public static XInitializeException Reinitialize(string message) => new XInitializeException(message);

    public static XInitializeException Reinitialize(object obj) => new XInitializeException($"The type {obj.GetType().Name} is initialized, you can't do this again!");
}