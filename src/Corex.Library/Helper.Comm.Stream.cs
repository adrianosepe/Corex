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
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Corex.Library;

public partial class XHelper
{
    public static partial class Comm
    {
        public static class Stream
        {
            private static readonly uint[] Lookup32Unsafe = CreateLookup32Unsafe();

            private static readonly unsafe uint* Lookup32UnsafeP = (uint*) GCHandle.Alloc(Lookup32Unsafe, GCHandleType.Pinned).AddrOfPinnedObject();

            public static unsafe string ByteArrayToHexString(byte[] bytes)
            {
                var lookupP = Lookup32UnsafeP;
                var result = new char[bytes.Length * 2];

                fixed (byte* bytesP = bytes)
                fixed (char* resultP = result)
                {
                    var resultP2 = (uint*) resultP;
                    for (var i = 0; i < bytes.Length; i++)
                    {
                        resultP2[i] = lookupP[bytesP[i]];
                    }
                }

                return new string(result);
            }

            public static string ComputeMd5Hash(string s, Encoding encoding = null)
            {
                encoding ??= Encoding.UTF8;

                var bytes = encoding.GetBytes(s);

                var sec = new MD5CryptoServiceProvider();
                var hashBytes = sec.ComputeHash(bytes);

                return ByteArrayToHexString(hashBytes);
            }

            public static string ComputeSha1Hash(string s, Encoding encoding = null)
            {
                encoding ??= Encoding.UTF8;

                var bytes = encoding.GetBytes(s);

                var sha1 = SHA1.Create();
                var hashBytes = sha1.ComputeHash(bytes);

                return ByteArrayToHexString(hashBytes);
            }

            public static byte[] HexStringToByteArray(string hex)
            {
                var numberChars = hex.Length;
                var bytes = new byte[numberChars / 2];
                for (var i = 0; i < numberChars; i += 2)
                {
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                }

                return bytes;
            }

            public static string StringToHexString(string s, Encoding encoding)
            {
                var data = encoding.GetBytes(s);

                return ByteArrayToHexString(data);
            }

            private static uint[] CreateLookup32Unsafe()
            {
                var result = new uint[256];
                for (var i = 0; i < 256; i++)
                {
                    // ReSharper disable LocalizableElement
                    var s = i.ToString("X2");
                    // ReSharper restore LocalizableElement

                    if (BitConverter.IsLittleEndian)
                    {
                        result[i] = s[0] + ((uint) s[1] << 16);
                    }
                    else
                    {
                        result[i] = s[1] + ((uint) s[0] << 16);
                    }
                }

                return result;
            }
        }
    }
}