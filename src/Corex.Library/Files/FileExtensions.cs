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
using Corex.Exceptions.Base;

namespace Corex.Library.Files;

internal class FileExtensions : FileExtension
{
    private readonly Lazy<string> _lazyCreateValue;

    public FileExtensions(string description, params FileExtension[] extensions) : base(string.Empty, EFileExtension.MultiExtensions, description)
    {
        Extensions = extensions;

        _lazyCreateValue = new Lazy<string>(
            () =>
                {
                    var extensionsAsString = Extensions.Select(extension => extension.Value).Aggregate((filter1, filter2) => filter1 + ";" + filter2);

                    return $"{Description}|{extensionsAsString}";
                });
    }

    public FileExtension[] Extensions { get; }

    public override string Value => _lazyCreateValue.Value;

    public override string ChangeExtension(string fileName) => throw new XException($"Não é possível alterar a extensão de um arquivo a partir de um {nameof(FileExtensions)}");
}