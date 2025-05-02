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

using System.Text;
using Corex.Exceptions.Base;
using Corex.Exceptions.Bussines;
using Corex.Library.ComponentModel.Model.Validation;

namespace Corex.Library.ComponentModel.Model.Svc;

public static class ServiceResponseExtensions
{
    public static void Join<TEntity>(this ServiceResponse<TEntity> @this, ServiceResponse<TEntity> response) => @this.ValidationResult.Join(response.ValidationResult);

    public static void Join<TEntity>(this ServiceResponse<TEntity> @this, EntityValidationResult validationResult) => @this.ValidationResult.Join(validationResult);

    public static XException TranslateToException<TEntity>(this ServiceResponse<TEntity> @this, string message)
    {
        if (@this.Success)
        {
            return null;
        }

        var builder = new StringBuilder(message);

        foreach (var validation in @this.ValidationResult.Validations)
        {
            // ReSharper disable LocalizableElement
            builder.Append($"\n\t- {validation.ErrorMessage}");
            // ReSharper restore LocalizableElement
        }

        return XFlowException.Create(builder.ToString());
    }
}