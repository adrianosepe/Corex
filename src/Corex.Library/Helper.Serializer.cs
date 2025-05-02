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
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Corex.Library;

public partial class XHelper
{
    public static class Serializers
    {
        public static class Xml
        {
            public static T Deserialize<T>(string data, params Type[] extraTypes)
            {
                var serializer = new XmlSerializer(typeof(T), extraTypes);
                using var reader = new StringReader(data);
                return (T) serializer.Deserialize(reader);
            }

            public static string Serialize(object obj, params Type[] extraTypes)
            {
                var serializer = new XmlSerializer(obj.GetType(), extraTypes);
                using var writer = new StringWriter();
                serializer.Serialize(writer, obj);

                return writer.ToString();
            }

            public static void Serialize(object obj, string fileName)
            {
                var serializer = new XmlSerializer(obj.GetType());
                using var sw = File.CreateText(fileName);
                serializer.Serialize(sw, obj);
                sw.Flush();
            }

            public static string SerializeDataContract(object obj)
            {
                var serializer = new DataContractSerializer(obj.GetType());
                using var sw = new StringWriter();
                using var writer = XmlWriter.Create(sw);
                serializer.WriteObject(writer, obj);

                return sw.ToString();
            }

            public static XmlWriter SerializeToXmlWriter(object obj, params Type[] extraTypes)
            {
                var serializer = new XmlSerializer(obj.GetType(), extraTypes);
                var writer = XmlWriter.Create(new MemoryStream());
                serializer.Serialize(writer, obj);

                return writer;
            }
        }

        public static class DataContract
        {
            public static T Deserialize<T>(XmlReader reader, params Type[] knowTypes)
            {
                var serializer = new DataContractSerializer(typeof(T), knowTypes);
                var obj = (T) serializer.ReadObject(reader);
                return obj;
            }

            public static T Deserialize<T>(string data, params Type[] knowTypes) where T : class
            {
                if (string.IsNullOrEmpty(data))
                {
                    return null;
                }

                using var reader = XmlReader.Create(new StringReader(data));
                var obj = Deserialize<T>(reader, knowTypes);
                return obj;
            }

            public static void Serialize<T>(T obj, XmlWriter writter, params Type[] knowTypes)
            {
                var serializer = new DataContractSerializer(typeof(T), knowTypes);
                serializer.WriteObject(writter, obj);
            }

            public static string Serialize<T>(T obj, params Type[] knowTypes)
            {
                using var textWriter = new StringWriter();
                using var writter = XmlWriter.Create(textWriter, new XmlWriterSettings
                    {
                        Encoding = Encoding.Unicode,
                        Indent = true,
                        IndentChars = "\t"
                    });
                Serialize(obj, writter, knowTypes);
                writter.Flush();
                writter.Close();
                return textWriter.ToString();
            }
        }

        public static class Json
        {
            private static readonly Lazy<JsonSerializerOptions> LazyDefaultSettings = new(
                () => new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        IncludeFields = false,
                    });

            static Json() { }

            public static JsonSerializerOptions DefaultSettings => LazyDefaultSettings.Value;

            public static T Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value, DefaultSettings);

            public static string Serialize(object obj) => JsonSerializer.Serialize(obj, DefaultSettings);
        }
    }
}