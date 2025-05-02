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
using System.Globalization;
using Corex.Library.Properties;

namespace Corex.Library;

public partial class XHelper
{
    public static partial class DisplayFormat
    {
        public static class Date
        {
            public static string Format(TimeSpan span)
            {
                var hours = (int) span.TotalHours;
                var minutes = (int) (span.TotalMinutes - hours * 60);
                var seconds = (int) (span.TotalSeconds - (int) span.TotalMinutes * 60);

                if (hours > 0)
                {
                    if (minutes > 0 && seconds > 0)
                    {
                        return string.Format(DbMessages.Date_Format1, hours, minutes, seconds);
                    }

                    if (minutes > 0)
                    {
                        return string.Format(DbMessages.Date_Format2, hours, minutes);
                    }

                    if (seconds > 0)
                    {
                        return string.Format(DbMessages.Date_Format3, hours, seconds);
                    }

                    return string.Format(DbMessages.Date_Format4, hours);
                }

                if (minutes > 0 && seconds > 0)
                {
                    return string.Format(DbMessages.Date_Format5, minutes, seconds);
                }

                if (minutes > 0)
                {
                    return string.Format(DbMessages.Date_Format6, minutes);
                }

                return string.Format(DbMessages.Date_Format7, seconds);
            }

            public static string FormatAsLongDate(DateTime dateTime) => dateTime.ToString("G", CultureInfo.CurrentCulture);

            public static string FormatAsShortDate(DateTime dateTime) => dateTime.ToString("d", CultureInfo.CurrentCulture);
            
            public static string FormatSmall(double seconds, bool showSeconds = true)
            {
                var span = TimeSpan.FromSeconds(seconds);

                return
                    showSeconds
                        ? $"{span.Days * 24 + span.Hours}:{span.Minutes:D2}:{span.Seconds:D2}"
                        : $"{span.Days * 24 + span.Hours}:{span.Minutes:D2}";
            }

            public static string FormatSmall(TimeSpan span, bool showSeconds = true) => FormatSmall(span.TotalSeconds, showSeconds);
        }
    }
}