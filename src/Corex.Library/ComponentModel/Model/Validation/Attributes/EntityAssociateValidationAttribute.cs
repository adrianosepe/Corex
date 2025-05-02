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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Corex.Library.ComponentModel.Model.Validation.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property)]
public class EntityAssociateValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IEnumerable enumerable)
        {
            var violations = new List<ValidationResult>();
            enumerable.Cast<object>()
                .ToArray()
                .ForEach(
                    model =>
                        {
                            var validation = EntityValidator.Validate(model);

                            if (!validation.HasViolations)
                            {
                                return;
                            }

                            violations.AddRange(validation.Validations);
                        });

            if (violations.Count == 0)
            {
                return ValidationResult.Success;
            }

            return new EntityAssociateValidationResult(
                $"Existem dados incorretos associados a '{validationContext.DisplayName}'",
                violations);
        }

        var validationResult = EntityValidator.Validate(value);

        if (!validationResult.HasViolations)
        {
            return ValidationResult.Success;
        }

        return new EntityAssociateValidationResult(
            $"Existens dados incorretos associados a '{validationContext.DisplayName}'",
            validationResult.Validations);
    }
}