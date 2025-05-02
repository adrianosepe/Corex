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
using System.Collections.Generic;

namespace Corex.Library.Map;

public static class Table
{
    public static IEnumerable<Row<Tuple<T, T, T>>> DistributeThreeColumns<T>(IEnumerable<T> data)
    {
        var arr = XHelper.Enumerable.ToArrayEfficient(data);

        var range = new Range(0, arr.Length);

        var rowIndex = 0;
        for (var i = 0; range.In(i);)
        {
            yield return new Row<Tuple<T, T, T>>
                {
                    Index = rowIndex++,
                    Data = new Tuple<T, T, T>(arr[i++], range.In(i) ? arr[i++] : default, range.In(i) ? arr[i++] : default)
                };
        }
    }

    public static IEnumerable<Row<Tuple<T, T>>> DistributeTwoColumns<T>(IEnumerable<T> data)
    {
        var arr = XHelper.Enumerable.ToArrayEfficient(data);

        var range = new Range(0, arr.Length - 1);

        var rowIndex = 0;
        for (var i = 0; range.In(i);)
        {
            yield return new Row<Tuple<T, T>>
                {
                    Index = rowIndex++,
                    Data = new Tuple<T, T>(arr[i++], range.In(i) ? arr[i++] : default)
                };
        }
    }
}