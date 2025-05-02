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
using Corex.Library.ComponentModel.Model.Primitives;

// ReSharper disable CheckNamespace
public static class XTimeSpanExtensions
// ReSharper restore CheckNamespace
{
    /// <summary>
    ///     Returns <see cref="TimeSpan" /> for given number of Days.
    /// </summary>
    public static XTimeSpan Days(this int days) => new XTimeSpan {TimeSpan = TimeSpan.FromDays(days)};

    /// <summary>
    ///     Returns <see cref="TimeSpan" /> for given number of Days.
    /// </summary>
    public static XTimeSpan Days(this double days) => new XTimeSpan {TimeSpan = TimeSpan.FromDays(days)};

    /// <summary>
    ///     Returns <see cref="TimeSpan" /> for given number of Hours.
    /// </summary>
    public static XTimeSpan Hours(this int hours) => new XTimeSpan {TimeSpan = TimeSpan.FromHours(hours)};

    /// <summary>
    ///     Returns <see cref="TimeSpan" /> for given number of Hours.
    /// </summary>
    public static XTimeSpan Hours(this double hours) => new XTimeSpan {TimeSpan = TimeSpan.FromHours(hours)};

    /// <summary>
    ///     Returns <see cref="TimeSpan" /> for given number of Milliseconds.
    /// </summary>
    public static XTimeSpan Milliseconds(this int milliseconds) => new XTimeSpan {TimeSpan = TimeSpan.FromMilliseconds(milliseconds)};

    /// <summary>
    ///     Returns <see cref="TimeSpan" /> for given number of Milliseconds.
    /// </summary>
    public static XTimeSpan Milliseconds(this double milliseconds) => new XTimeSpan {TimeSpan = TimeSpan.FromMilliseconds(milliseconds)};

    /// <summary>
    ///     Returns <see cref="TimeSpan" /> for given number of Minutes.
    /// </summary>
    public static XTimeSpan Minutes(this int minutes) => new XTimeSpan {TimeSpan = TimeSpan.FromMinutes(minutes)};

    /// <summary>
    ///     Returns <see cref="TimeSpan" /> for given number of Minutes.
    /// </summary>
    public static XTimeSpan Minutes(this double minutes) => new XTimeSpan {TimeSpan = TimeSpan.FromMinutes(minutes)};

    /// <summary>
    ///     Returns <see cref="TimeSpan" /> value for given number of Months.
    /// </summary>
    public static XTimeSpan Months(this int months) => new XTimeSpan {Months = months};

    /// <summary>
    ///     Returns <see cref="TimeSpan" /> for given number of Seconds.
    /// </summary>
    public static XTimeSpan Seconds(this int seconds) => new XTimeSpan {TimeSpan = TimeSpan.FromSeconds(seconds)};

    /// <summary>
    ///     Returns <see cref="TimeSpan" /> for given number of Seconds.
    /// </summary>
    public static XTimeSpan Seconds(this double seconds) => new XTimeSpan {TimeSpan = TimeSpan.FromSeconds(seconds)};

    /// <summary>
    ///     Returns <see cref="TimeSpan" /> for given number of ticks.
    /// </summary>
    public static XTimeSpan Ticks(this int ticks) => new XTimeSpan {TimeSpan = TimeSpan.FromTicks(ticks)};

    /// <summary>
    ///     Returns <see cref="TimeSpan" /> for given number of ticks.
    /// </summary>
    public static XTimeSpan Ticks(this long ticks) => new XTimeSpan {TimeSpan = TimeSpan.FromTicks(ticks)};

    /// <summary>
    ///     Returns <see cref="TimeSpan" /> for given number of Weeks (number of weeks * 7).
    /// </summary>
    public static XTimeSpan Weeks(this int weeks) => new XTimeSpan {TimeSpan = TimeSpan.FromDays(weeks * 7)};

    /// <summary>
    ///     Returns <see cref="TimeSpan" /> for given number of Weeks (number of weeks * 7).
    /// </summary>
    public static XTimeSpan Weeks(this double weeks) => new XTimeSpan {TimeSpan = TimeSpan.FromDays(weeks * 7)};

    /// <summary>
    ///     Generates <see cref="TimeSpan" /> value for given number of Years.
    /// </summary>
    public static XTimeSpan Years(this int years) => new XTimeSpan {Years = years};
}