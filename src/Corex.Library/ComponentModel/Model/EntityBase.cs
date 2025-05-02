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

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Corex.Library.ComponentModel.Model.Business;
using Corex.Library.ComponentModel.Model.Present.Attributes;
using Corex.Library.ComponentModel.Model.Utilities;
using Corex.Library.ComponentModel.Model.Validation;
using Corex.Library.ComponentModel.Model.Validation.Annotations.Key;

namespace Corex.Library.ComponentModel.Model;

[DebuggerDisplay("{" + nameof(DisplaySmall) + "}")]
public abstract class EntityBase : Entity, IEntity, IDataErrorInfo
{
    public virtual string DisplayFull => $"{ID}";

    public virtual string DisplaySmall => $"{ID}";

    public virtual string Display => DisplaySmall;

    string IDataErrorInfo.Error
    {
        get
        {
            var validationResult = EntityValidator.Validate(this);

            return !validationResult.HasViolations ? null : validationResult.ToString();
        }
    }

    string IDataErrorInfo.this[string columnName]
    {
        get
        {
            var validationResult = EntityValidator.ValidateProperty(this, columnName);

            return !validationResult.HasViolations ? null : validationResult.ToString();
        }
    }

    [Display(Name = EntityBaseDisplayPattern.DefaultDisplayNameID), XPrimaryKey]
    public abstract int ID { get; set; }

    public new class Metadata
    {
        [Display(Name = EntityBaseDisplayPattern.DefaultDisplayNameID), XPrimaryKey]
        public int ID { get; set; }
    }

    public class MetadataWithLookup
    {
        [Display(Name = EntityBaseDisplayPattern.DefaultDisplayNameID), XPrimaryKey, LookupDisplay(0)]
        public int ID { get; set; }
    }
}