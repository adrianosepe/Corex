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
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Corex.Library.ComponentModel.Model.Validation;

namespace Corex.Library.ComponentModel.Model.Svc;

[DataContract, Serializable]
public class ServiceResponse<TEntity> : ServiceResponse
{
    public ServiceResponse(TEntity entity, EntityValidationResult validationResult = null)
    {
        Entity = entity;

        ValidationResult = validationResult ?? new EntityValidationResult();
    }

    public ServiceResponse(TEntity entity, params string[] errors)
    {
        Entity = entity;

        ValidationResult = new EntityValidationResult(errors.Select(error => new ValidationResult(error)));
    }

    [DataMember]
    public TEntity Entity { get; }

    public override string ToString()
    {
        var bd = new StringBuilder();

        // ReSharper disable LocalizableElement
        ValidationResult.Validations.ForEach(result => bd.Append($"- {result.ErrorMessage} \n"));
        // ReSharper restore LocalizableElement

        return bd.ToString();
    }
}