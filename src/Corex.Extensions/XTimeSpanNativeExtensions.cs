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
using System.Text;
using Corex.Extensions.Properties;

// ReSharper disable CheckNamespace
public static class XTimeSpanNativeExtensions
// ReSharper restore CheckNamespace
{
    public static TimeSpan InDays(this int value) => TimeSpan.FromDays(value);

    public static TimeSpan InDays(this double value) => TimeSpan.FromDays(value);

    public static TimeSpan InHours(this int value) => TimeSpan.FromHours(value);

    public static TimeSpan InHours(this double value) => TimeSpan.FromHours(value);

    public static TimeSpan InMinutes(this int value) => TimeSpan.FromMinutes(value);

    public static TimeSpan InMinutes(this double value) => TimeSpan.FromMinutes(value);

    public static TimeSpan InSeconds(this int value) => TimeSpan.FromSeconds(value);

    public static TimeSpan InSeconds(this double value) => TimeSpan.FromSeconds(value);

    public static string ToDefaultDisplay(this TimeSpan? span) => span == null ? DbMessages.XTimeSpanNativeExtensions_FailOnCalculate : ToDefaultDisplay(span.Value);

    public static string ToDefaultDisplay(this TimeSpan span)
    {
        var builder = new StringBuilder();

        double fraction;
        if (span.Days > 0)
        {
            builder.Append(
                $"{span.Days} dia{(span.Days > 1 ? "s" : string.Empty)} ");

            fraction = span.TotalHours - span.Days * 24;

            builder.Append(
                $"{fraction:f2} hora{(fraction > 1 ? "s" : string.Empty)}");
        }
        else
        {
            if (span.Hours > 0)
            {
                builder.Append(
                    $"{span.Hours} hora{(span.Hours > 1 ? "s" : string.Empty)} ");
            }

            if (span.TotalMinutes > 1)
            {
                fraction = span.TotalMinutes - span.Hours * 60;

                builder.Append(
                    $"{fraction:f1} minuto{(fraction > 1 ? "s" : string.Empty)}");
            }
            else
            {
                builder.Append("Menos de 1 minuto.");
            }
        }

        return builder.ToString();
    }
}