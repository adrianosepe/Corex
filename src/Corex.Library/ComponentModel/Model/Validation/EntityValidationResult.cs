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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace Corex.Library.ComponentModel.Model.Validation;

[DataContract, Serializable]
public class EntityValidationResult
{
    public static readonly EntityValidationResult Empty = new EntityValidationResult();

    internal readonly List<ValidationResult> InternalValidations;

    public EntityValidationResult(params string[] errors) : this(errors.Select(error => new ValidationResult(error))) { }

    public EntityValidationResult(params IEnumerable<ValidationResult>[] validations)
    {
        InternalValidations = validations
            .SelectMany(
                result =>
                    {
                        var validationResults = result as ValidationResult[] ?? result.ToArray();

                        return validationResults.ToList();
                    })
            .ToList();
    }

    public EntityValidationResult(IEnumerable<ValidationResult> validations = null)
    {
        InternalValidations = validations?.ToList() ?? new List<ValidationResult>();
    }

    [DataMember]
    public bool AnyViolationOrWarning => HasViolations || HasWarning;

    [DataMember]
    public bool HasViolations => Validations.Any(result => !(result is WarningValidationResult));

    [DataMember]
    public bool HasWarning => Validations.Any(result => result is WarningValidationResult);

    [DataMember]
    public bool IsEmpty => InternalValidations.Count == 0;

    [DataMember]
    public IEnumerable<ValidationResult> Validations => InternalValidations;

    // ReSharper disable LocalizableElement
    public override string ToString()
    {
        if (InternalValidations.Count == 0)
        {
            return string.Empty;
        }

        return InternalValidations.Select(result => result.ErrorMessage).Aggregate((s, s1) => s + "\n" + s1);
    }
    // ReSharper restore LocalizableElement
}