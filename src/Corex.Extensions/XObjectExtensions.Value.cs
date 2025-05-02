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

using System.Xml.Serialization;
using Corex.Library;

// ReSharper disable CheckNamespace
public static partial class XObjectExtensions
// ReSharper restore CheckNamespace
{
    public static object NotNull(this object obj)
    {
        if (Equals(obj, null))
        {
            return null;
        }

        var type = obj.GetType();
        if (!type.IsEnum)
        {
            return obj.ToString();
        }

        var memberInfo = XHelper.Reflections.Fields.GetStatic(type, obj.ToString());
        var xmlEnumMember = XHelper.Reflections.Attributes.Get<XmlEnumAttribute>(memberInfo);

        return xmlEnumMember != null ? xmlEnumMember.Name : obj.ToString();
    }
}