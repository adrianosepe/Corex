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
using Corex.Exceptions.Contract;

namespace Corex.Library.ComponentModel.Model.Primitives;

[StructLayout(LayoutKind.Sequential)]
public struct XTimeSpan : IEquatable<XTimeSpan>, IComparable<TimeSpan>, IComparable<XTimeSpan>
{
    public const int DaysPerYear = 365;

    public int Months { get; set; }

    public int Years { get; set; }

    public TimeSpan TimeSpan { get; set; }

    public long Ticks => ((TimeSpan) this).Ticks;

    public int Days => ((TimeSpan) this).Days;

    public int Hours => ((TimeSpan) this).Hours;

    public int Milliseconds => ((TimeSpan) this).Milliseconds;

    public int Minutes => ((TimeSpan) this).Minutes;

    public int Seconds => ((TimeSpan) this).Seconds;

    public double TotalDays => ((TimeSpan) this).TotalDays;

    public double TotalHours => ((TimeSpan) this).TotalHours;

    public double TotalMilliseconds => ((TimeSpan) this).TotalMilliseconds;

    public double TotalMinutes => ((TimeSpan) this).TotalMinutes;

    public double TotalSeconds => ((TimeSpan) this).TotalSeconds;

    public XTimeSpan Add(XTimeSpan number) => AddInternal(this, number);

    public XTimeSpan Subtract(XTimeSpan timeSpan) => SubtractInternal(this, timeSpan);

    public static XTimeSpan operator +(XTimeSpan left, XTimeSpan right) => AddInternal(left, right);

    public static XTimeSpan operator +(XTimeSpan left, TimeSpan right) => AddInternal(left, right);

    public static XTimeSpan operator +(TimeSpan left, XTimeSpan right) => AddInternal(left, right);

    public static XTimeSpan operator -(XTimeSpan value) => value.Negate();

    public static XTimeSpan operator -(XTimeSpan left, XTimeSpan right) => SubtractInternal(left, right);

    public static XTimeSpan operator -(TimeSpan left, XTimeSpan right) => SubtractInternal(left, right);

    public static XTimeSpan operator -(XTimeSpan left, TimeSpan right) => SubtractInternal(left, right);

    public static bool operator ==(XTimeSpan left, XTimeSpan right) => left.Years == right.Years && left.Months == right.Months && left.TimeSpan == right.TimeSpan;

    public static bool operator ==(TimeSpan left, XTimeSpan right) => (XTimeSpan) left == right;

    public static bool operator ==(XTimeSpan left, TimeSpan right) => left == (XTimeSpan) right;

    public static bool operator !=(XTimeSpan left, XTimeSpan right) => !(left == right);

    public static bool operator !=(TimeSpan left, XTimeSpan right) => !(left == right);

    public static bool operator !=(XTimeSpan left, TimeSpan right) => !(left == right);

    public static bool operator <(XTimeSpan left, XTimeSpan right) => (TimeSpan) left < (TimeSpan) right;

    public static bool operator <(XTimeSpan left, TimeSpan right) => (TimeSpan) left < right;

    public static bool operator <(TimeSpan left, XTimeSpan right) => left < (TimeSpan) right;

    public static bool operator <=(XTimeSpan left, XTimeSpan right) => (TimeSpan) left <= (TimeSpan) right;

    public static bool operator <=(XTimeSpan left, TimeSpan right) => (TimeSpan) left <= right;

    public static bool operator <=(TimeSpan left, XTimeSpan right) => left <= (TimeSpan) right;

    public static bool operator >(XTimeSpan left, XTimeSpan right) => (TimeSpan) left > (TimeSpan) right;

    public static bool operator >(XTimeSpan left, TimeSpan right) => (TimeSpan) left > right;

    public static bool operator >(TimeSpan left, XTimeSpan right) => left > (TimeSpan) right;

    public static bool operator >=(XTimeSpan left, XTimeSpan right) => (TimeSpan) left >= (TimeSpan) right;

    public static bool operator >=(XTimeSpan left, TimeSpan right) => (TimeSpan) left >= right;

    public static bool operator >=(TimeSpan left, XTimeSpan right) => left >= (TimeSpan) right;

    public static implicit operator TimeSpan(XTimeSpan timeSpan)
    {
        var daysFromYears = DaysPerYear * timeSpan.Years;
        var daysFromMonths = 30 * timeSpan.Months;
        var days = daysFromMonths + daysFromYears;
        return new TimeSpan(days, 0, 0, 0) + timeSpan.TimeSpan;
    }

    public static implicit operator XTimeSpan(TimeSpan timeSpan) => new XTimeSpan {TimeSpan = timeSpan};

    public object Clone() =>
        new XTimeSpan
            {
                TimeSpan = TimeSpan,
                Months = Months,
                Years = Years
            };

    bool IEquatable<XTimeSpan>.Equals(XTimeSpan other) => Equals(other);

    public override string ToString() => ((TimeSpan) this).ToString();

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        var type = obj.GetType();
        if (type == typeof(XTimeSpan))
        {
            return this == (XTimeSpan) obj;
        }

        if (type == typeof(TimeSpan))
        {
            return this == (TimeSpan) obj;
        }

        return false;
    }

    public override int GetHashCode() => Months.GetHashCode() ^ Years.GetHashCode() ^ TimeSpan.GetHashCode();

    private static XTimeSpan AddInternal(XTimeSpan left, XTimeSpan right) =>
        new XTimeSpan
            {
                Years = left.Years + right.Years,
                Months = left.Months + right.Months,
                TimeSpan = left.TimeSpan + right.TimeSpan
            };

    private static XTimeSpan SubtractInternal(XTimeSpan left, XTimeSpan right) =>
        new XTimeSpan
            {
                Years = left.Years - right.Years,
                Months = left.Months - right.Months,
                TimeSpan = left.TimeSpan - right.TimeSpan
            };

    public int CompareTo(TimeSpan other) => ((TimeSpan) this).CompareTo(other);

    public int CompareTo(object value)
    {
        if (value is TimeSpan span)
        {
            return ((TimeSpan) this).CompareTo(span);
        }

        throw XArgumentException.Create(nameof(value), "Value must be a TimeSpan");
    }

    public int CompareTo(XTimeSpan value) => ((TimeSpan) this).CompareTo(value);

    public TimeSpan Negate() =>
        new XTimeSpan
            {
                TimeSpan = -TimeSpan,
                Months = -Months,
                Years = -Years
            };
}