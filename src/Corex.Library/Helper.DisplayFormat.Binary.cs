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

namespace Corex.Library;

public partial class XHelper
{
    public static partial class DisplayFormat
    {
        public static class Binary
        {
            public static string ByteToGigabyte(long length)
            {
                var mb = length / UnitOfMeasure.Gigabyte;

                return $"{mb:F} GB";
            }

            public static string ByteToKilobyte(long length)
            {
                var mb = length / UnitOfMeasure.Kilobyte;

                return $"{mb:F} KB";
            }

            public static string ByteToMegabyte(long length)
            {
                var mb = length / UnitOfMeasure.Megabyte;

                return $"{mb:F} MB";
            }

            public static string ByteToPetabyte(long length)
            {
                var mb = length / UnitOfMeasure.Petabyte;

                return $"{mb:F} PB";
            }

            public static string ByteToTerabyte(long length)
            {
                var mb = length / UnitOfMeasure.Terabyte;

                return $"{mb:F} TB";
            }
        }
    }
}