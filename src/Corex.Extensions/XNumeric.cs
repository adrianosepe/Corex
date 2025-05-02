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

using Corex.Library;

// ReSharper disable CheckNamespace
public static class XNumeric
// ReSharper restore CheckNamespace
{
    public static string CurrencySymbol => XHelper.DisplayFormat.Financial.CurrencySymbol;

    public static decimal AsMoney(this decimal value) => XHelper.Data.Numeric.Round(value, 2);

    public static string DisplayAsCurrency(this decimal value) => XHelper.DisplayFormat.Financial.AsCurrency(value);

    public static string DisplayAsCurrency(this double value) => XHelper.DisplayFormat.Financial.AsCurrency((decimal) value);

    public static string FormatAsCurrency(this decimal value) => XHelper.DisplayFormat.Financial.Format(value);

    public static string FormatAsCurrency(this double value) => XHelper.DisplayFormat.Financial.Format((decimal) value);

    public static decimal XRound(this decimal value, int decimalPlaces) => XHelper.Data.Numeric.Round(value, decimalPlaces);

    public static decimal? XRound(this decimal? value, int decimalPlaces) => XHelper.Data.Numeric.Round(value, decimalPlaces);
}