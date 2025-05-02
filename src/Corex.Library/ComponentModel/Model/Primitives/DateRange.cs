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
using Corex.Library.ComponentModel.Model.Abstraction;
using Corex.Library.Properties;

namespace Corex.Library.ComponentModel.Model.Primitives;

public struct DateRange : IDateRange
{
    public DateRange(DateTime? min, DateTime? max)
    {
        Min = min;
        Max = max;
    }

    public DateTime? Min { get; }

    public DateTime? Max { get; }

    public DateTime? MinAsBeginDay => Min?.Date;

    public DateTime? MaxAsEndDay => Max?.Date.AddDays(1).AddSeconds(-1);

    public TimeSpan Range
    {
        get
        {
            if (!IsHasRangeValues)
            {
                return TimeSpan.Zero;
            }

            // ReSharper disable PossibleInvalidOperationException
            return Max.Value - Min.Value;
            // ReSharper restore PossibleInvalidOperationException
        }
    }

    public bool IsNullMin => Min == null;

    public bool IsNullMax => Max == null;

    public bool IsNull => IsNullMin && IsNullMax;

    public bool IsNullPartial => IsNullMin || IsNullMax;

    public bool IsHasRangeValues => !IsNullMin && !IsNullMax;

    public override bool Equals(object obj) => obj is DateRange range && this == range;

    public override int GetHashCode() => Min.GetHashCode() ^ Max.GetHashCode();

    public static bool operator ==(DateRange dr1, DateRange dr2) => dr1.Min == dr2.Min && dr1.Max == dr2.Max;

    public static bool operator !=(DateRange dr1, DateRange dr2) => !(dr1 == dr2);

    public DateRange UpdateMin(DateTime? min) => new DateRange(min, Max);

    public DateRange UpdateMax(DateTime? max) => new DateRange(Min, max);

    public override string ToString()
    {
        if (IsNull)
        {
            return DbMessages.DateRange_ToString__Não_Definido_;
        }

        if (!IsNullPartial)
        {
            return string.Format(DbMessages.DateRange_ToString_maior_ou_igual__0__e_menor_ou_gual__1_, Min, Max);
        }

        return IsNullMin
            ? string.Format(DbMessages.DateRange_ToString_maior_ou_igual_a__0_, Max)
            : string.Format(DbMessages.DateRange_ToString_menor_ou_igual_a__0_, Min);
    }
}