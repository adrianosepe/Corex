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
using System.Text;

// ReSharper disable UnusedMember.Global

namespace Corex.Library;

public partial class XHelper
{
    public static class Streams
    {
        public static void CreateMemory(Action<MemoryStream> read)
        {
            using var stream = new MemoryStream();

            read(stream);
        }

        public static void CreateMemory(byte[] buffer, Action<MemoryStream> read)
        {
            CreateMemory(
                stream =>
                    {
                        stream.Write(buffer, 0, buffer.Length);
                        stream.Position = 0;

                        read(stream);
                    });
        }

        public static void CreateMemory(Action<MemoryStream> write, Action<MemoryStream> read)
        {
            CreateMemory(
                stream =>
                    {
                        write(stream);
                        stream.Position = 0;
                        read(stream);
                    });
        }

        public static Stream GetStream(string value) => new MemoryStream(Encoding.UTF8.GetBytes(value ?? string.Empty));

        public static byte[] StreamToByteArray(Action<MemoryStream> write)
        {
            byte[] buffer = null;

            CreateMemory(
                stream =>
                    {
                        write(stream);

                        stream.Position = 0;
                        buffer = stream.ToArray();
                    });

            return buffer;
        }
    }
}