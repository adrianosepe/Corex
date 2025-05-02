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

using System.Xml.Linq;
using System.Linq;
using System.Xml;

// ReSharper disable CheckNamespace
public static class XXmlDocumentExtensions
    // ReSharper restore CheckNamespace
{
    public static XmlAttribute AttributeCreate(this XmlDocument document, string prefix, string name, string value)
    {
        var attribute = document.CreateAttribute(prefix, name, "http://www.w3.org/2000/xmlns/");
        attribute.Value = value;

        return attribute;
    }

    public static XmlDocument RemoveXmlns(this XmlDocument doc)
    {
        XDocument d;
        using (var nodeReader = new XmlNodeReader(doc))
        {
            d = XDocument.Load(nodeReader);
        }

        d.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

        foreach (var elem in d.Descendants())
        {
            elem.Name = elem.Name.LocalName;
        }

        var xmlDocument = new XmlDocument();
        using (var xmlReader = d.CreateReader())
        {
            xmlDocument.Load(xmlReader);
        }

        return xmlDocument;
    }
}