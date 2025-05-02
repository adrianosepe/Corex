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

namespace Corex.Library.Protocol;

public class Crc16Ccitt
{
    private const ushort Poly = 4129;
    private readonly ushort _initialValue;
    private readonly ushort[] _table = new ushort[256];

    public Crc16Ccitt(ECrcType type)
    {
        _initialValue = (ushort) type;
        for (var i = 0; i < _table.Length; ++i)
        {
            ushort temp = 0;
            var a = (ushort) (i << 8);
            for (var j = 0; j < 8; ++j)
            {
                if (((temp ^ a) & 0x8000) != 0)
                {
                    temp = (ushort) ((temp << 1) ^ Poly);
                }
                else
                {
                    temp <<= 1;
                }

                a <<= 1;
            }

            _table[i] = temp;
        }
    }

    public ushort ComputeChecksum(byte[] bytes)
    {
        return bytes.Aggregate(_initialValue, (current, t) => (ushort) ((current << 8) ^ _table[(current >> 8) ^ (0xff & t)]));
    }

    public byte[] ComputeChecksumBytes(byte[] bytes)
    {
        var crc = ComputeChecksum(bytes);

        return BitConverter.GetBytes(crc);
    }
}