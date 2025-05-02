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
using System.IO;
using Corex.Exceptions.Assert;

// ReSharper disable UnusedMember.Global

namespace Corex.Library.Files;

public class FileExtension
{
    public static readonly FileExtension Excel = new FileExtension("*.xlsx", EFileExtension.Xlsx);

    public static readonly FileExtension ExcelLegace = new FileExtension("*.xls", EFileExtension.Xls);

    public static readonly FileExtension ExcelAll = new FileExtensions("Planilhas do Excel", Excel, ExcelLegace);

    public static readonly FileExtension Xml = new FileExtension("*.xml", EFileExtension.Xml);

    public static readonly FileExtension Doc = new FileExtension("*.docx", EFileExtension.Docx);

    public static readonly FileExtension DocLegace = new FileExtension("*.doc", EFileExtension.Doc);

    public static readonly FileExtension DocsAll = new FileExtensions("Documentos do Word", Doc, DocLegace);

    public static readonly FileExtension Pdf = new FileExtension("*.pdf", EFileExtension.Pdf);

    public static readonly FileExtension Txt = new FileExtension("*.txt", EFileExtension.Txt);

    public static readonly FileExtension Png = new FileExtension("*.png", EFileExtension.Png);

    public static readonly FileExtension Any = new FileExtension("*.*", EFileExtension.Docx);

    public static readonly FileExtension Eml = new FileExtension("*.eml", EFileExtension.Eml);

    public static readonly FileExtension Csv = new FileExtension("*.csv", EFileExtension.Csv);

    public FileExtension(string value, EFileExtension type, string description = null)
    {
        XContract.ArgIsNotNull(value, nameof(value));

        if (XHelper.Enums.IsIn(type, EFileExtension.Unknown, EFileExtension.MultiExtensions) && string.IsNullOrEmpty(description))
        {
            throw new ArgumentNullException(nameof(description));
        }

        Type = type;
        Value = value;
        Description = description ?? XHelper.Enums.GetDisplay(type);
    }

    public string Description { get; }

    public EFileExtension Type { get; }

    public virtual string Value { get; }

    // ReSharper disable LocalizableElement
    public string AsFilter() => $"{Description}|{Value}";
    // ReSharper restore LocalizableElement

    public virtual string ChangeExtension(string fileName) => Path.ChangeExtension(fileName, Value.Replace("*.", string.Empty));
}