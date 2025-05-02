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
    public static class UnitOfMeasure
    {
        public enum EUnitType
        {
            Meter,
            Kilometer,
            Degree
        }

        public const double DegreeOnMeters = 111111.11;
        public const double DegreeOnKilometers = 111.11111;

        public const long Kilobyte = 1024;
        public const long Megabyte = 1024 * Kilobyte;
        public const long Gigabyte = 1024 * Megabyte;
        public const long Terabyte = 1024 * Gigabyte;
        public const long Petabyte = 1024 * Gigabyte;

        public static double ConvertDegreeTo(double rate, EUnitType futureUnit)
        {
            return futureUnit switch
                {
                    EUnitType.Kilometer => (rate * DegreeOnKilometers),
                    EUnitType.Meter => (rate * DegreeOnMeters),
                    _ => -1
                };
        }
    }
}