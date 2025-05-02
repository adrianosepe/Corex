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

using System.Text;

namespace Corex.Library;

public partial class XHelper
{
    public partial class Encodes
    {
        public static class Base32
        {
            private const string ValidChars = "QAZ2WSX3" + "EDC4RFV5" + "TGB6YHN7" + "UJM8K9LP";

            public static byte[] FromBase32String(string str)
            {
                var numBytes = str.Length * 5 / 8;
                var bytes = new byte[numBytes];

                // all UPPERCASE chars
                str = str.ToUpper();

                if (str.Length < 3)
                {
                    bytes[0] = (byte) (ValidChars.IndexOf(str[0]) | (ValidChars.IndexOf(str[1]) << 5));
                    return bytes;
                }

                var bitBuffer = ValidChars.IndexOf(str[0]) | (ValidChars.IndexOf(str[1]) << 5);
                var bitsInBuffer = 10;
                var currentCharIndex = 2;
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = (byte) bitBuffer;
                    bitBuffer >>= 8;
                    bitsInBuffer -= 8;
                    while (bitsInBuffer < 8 && currentCharIndex < str.Length)
                    {
                        bitBuffer |= ValidChars.IndexOf(str[currentCharIndex++]) << bitsInBuffer;
                        bitsInBuffer += 5;
                    }
                }

                return bytes;
            }

            public static string ToBase32String(byte[] bytes)
            {
                var sb = new StringBuilder(); // holds the base32 chars
                var hi = 5;
                var currentByte = 0;

                while (currentByte < bytes.Length)
                {
                    // do we need to use the next byte?
                    byte index;
                    if (hi > 8)
                    {
                        // get the last piece from the current byte, shift it to the right
                        // and increment the byte counter
                        index = (byte) (bytes[currentByte++] >> (hi - 5));
                        if (currentByte != bytes.Length)
                        {
                            // if we are not at the end, get the first piece from
                            // the next byte, clear it and shift it to the left
                            index = (byte) (((byte) (bytes[currentByte] << (16 - hi)) >> 3) | index);
                        }

                        hi -= 3;
                    }
                    else if (hi == 8)
                    {
                        index = (byte) (bytes[currentByte++] >> 3);
                        hi -= 3;
                    }
                    else
                    {
                        // simply get the stuff from the current byte
                        index = (byte) ((byte) (bytes[currentByte] << (8 - hi)) >> 3);
                        hi += 5;
                    }

                    sb.Append(ValidChars[index]);
                }

                return sb.ToString();
            }
        }
    }
}