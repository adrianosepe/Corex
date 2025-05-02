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
using System.Reflection;

namespace Corex.Library.ComponentModel.Model.Validation;

public static class EntityValidator
{
    public static bool IsValidate(object entity) => !Validate(entity, out _).IsEmpty;

    public static EntityValidationResult Validate(object entity) => Validate(entity, out _);

    public static EntityValidationResult Validate(object entity, out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();
        var vc = new ValidationContext(entity, null, null);

        Validator.TryValidateObject(entity, vc, validationResults, true);

        return new EntityValidationResult(validationResults);
    }

    public static bool Validate(object entity, List<ValidationResult> validationResults)
    {
        if (entity == null)
        {
            return true;
        }

        var vc = new ValidationContext(entity, null, null);

        return Validator.TryValidateObject(entity, vc, validationResults, true);
    }

    public static EntityValidationResult ValidateProperty(object entity, string propertyName) => ValidateProperty(entity, entity.GetType().GetProperty(propertyName));

    public static EntityValidationResult ValidateProperty(object entity, PropertyInfo propertyInfo)
    {
        if (propertyInfo == null)
        {
            return new EntityValidationResult();
        }

        var validationResults = new List<ValidationResult>();
        var vc = new ValidationContext(entity, null, null) {MemberName = propertyInfo.Name};

        Validator.TryValidateProperty(propertyInfo.GetValue(entity), vc, validationResults);

        return new EntityValidationResult(validationResults);
    }

    public static EntityValidationResult ValidateProperty(object entity, string propertyName, object value)
    {
        if (propertyName == null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        var validationResults = new List<ValidationResult>();
        var vc = new ValidationContext(entity, null, null) {MemberName = propertyName};

        Validator.TryValidateProperty(value, vc, validationResults);

        return new EntityValidationResult(validationResults);
    }

    public static EntityValidationResult ValidateRecursive(object entity) => ValidateRecursive(entity, out _);

    public static EntityValidationResult ValidateRecursive(object entity, out List<ValidationResult> validationResults)
    {
        validationResults = new List<ValidationResult>();

        ValidateRecursive(entity, validationResults, 0, 4);

        return new EntityValidationResult(validationResults);
    }

    public static void ValidateRecursive(object obj, List<ValidationResult> results, int level, int maxDeep)
    {
        if (obj == null)
        {
            return;
        }

        Validate(obj, results);

        if (++level >= maxDeep)
        {
            return;
        }

        var properties = obj
            .GetType()
            .GetProperties()
            .Where(prop => prop.CanRead
                           && !prop.GetCustomAttributes(typeof(SkipRecursiveValidationAttribute), false).Any()
                           && prop.GetIndexParameters().Length == 0)
            .ToArray();

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(string) || property.PropertyType.IsValueType)
            {
                continue;
            }

            var value = XHelper.Reflections.Properties.ReadValue<object>(obj, property.Name);

            switch (value)
            {
                case null:
                    continue;

                case IEnumerable asEnumerable:
                {
                    foreach (var enumObj in asEnumerable)
                    {
                        ValidateRecursive(enumObj, results, level, maxDeep);
                    }

                    break;
                }

                default:
                    ValidateRecursive(value, results, level, maxDeep);
                    break;
            }
        }
    }
}