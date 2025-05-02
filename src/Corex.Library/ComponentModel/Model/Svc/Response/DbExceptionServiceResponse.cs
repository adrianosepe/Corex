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
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Runtime.Serialization;
using Corex.Library.ComponentModel.Model.Validation;

namespace Corex.Library.ComponentModel.Model.Svc;

[DataContract, Serializable]
public class DbExceptionServiceResponse<T> : ServiceResponse<T> where T : class
{
    // ReSharper disable SuggestBaseTypeForParameter
    public DbExceptionServiceResponse(DbException exception) : this(null, exception) { }
    // ReSharper restore SuggestBaseTypeForParameter

    public DbExceptionServiceResponse(T entity, Exception exception) : base(entity, CreateResult(exception)) { }

    private static EntityValidationResult CreateResult(Exception exception) => new EntityValidationResult(new[] {new ValidationResult(exception.Message)});
}