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

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Corex.Library;

public partial class XHelper
{
    public static class RegEx
    {
        public static IEnumerable<FindInfo> Matches(string openToken, string closeToken, string origin)
        {
            var e = new Regex($@"{openToken}(.+){closeToken}");

            var matchCollection = e.Matches(origin);

            return
                matchCollection
                    .Cast<Match>()
                    .Select(
                        match =>
                            new FindInfo
                                {
                                    Expression = match.Value,
                                    Content = match.Groups.Cast<Capture>().LastOrDefault()?.Value,
                                    Index = match.Index,
                                    Length = match.Length
                                });
        }

        public class FindInfo
        {
            public string Content { get; set; }

            public string Expression { get; set; }

            public int Index { get; set; }

            public int Length { get; set; }
        }
    }
}