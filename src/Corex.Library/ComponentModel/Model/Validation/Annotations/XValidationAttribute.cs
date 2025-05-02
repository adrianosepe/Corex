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
using System.Linq;
using Corex.Library.ComponentModel.Model.Enums;

// ReSharper disable UnusedMember.Global

namespace Corex.Library.ComponentModel.Model.Validation.Annotations;

public abstract class XValidationAttribute : XDataTypeAttribute
{
    public enum EValidationMode
    {
        Custom,

        MaskDataTime,
        MaskDateTimeAdvancingCaret,
        MaskNumeric,
        MaskRegEx,
        MaskRegular,
        MaskSimple
    }

    private string _maskForEditor;

    protected XValidationAttribute(EDataType dataType, EValidationMode mode, bool isComputed = false) : base(dataType, isComputed)
    {
        Mode = mode;
    }

    protected XValidationAttribute(EDataType dataType, EValidationMode mode, string errorMessage, bool isComputed = false) : base(dataType, isComputed)
    {
        Mode = mode;
        ErrorMessage = errorMessage;
    }

    public virtual bool Enabled => RestrictFor == EModoOperacao.None || RestrictFor == ValidationSettings.RestrictionFor;

    public abstract string Mask { get; }

    public string MaskForEditor
    {
        get => _maskForEditor ?? MaskWithoutStartAndEnd;
        set => _maskForEditor = value;
    }

    public string MaskForEditorWithoutStartAndEnd => MaskForEditor.Replace("^", "").Replace("$", "");
    
    public string MaskWithoutStartAndEnd => Mask.Replace("^", "").Replace("$", "");

    public EValidationMode Mode { get; protected set; }

    public EModoOperacao RestrictFor { get; set; }

    protected override bool InternalIsEnabled() => Enabled;
}