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
using System.Linq;
using System.Runtime.Serialization;

namespace Corex.Library.ComponentModel.Model.Svc;

[DataContract, Serializable]
public class ServiceBathResponse<TEntityID> : ServiceResponse
{
    private readonly Result[] _results;

    public ServiceBathResponse(IEnumerable<Result> results)
    {
        _results = results.ToArrayEfficient();
    }

    public int CountFailed => _results.Count(r => r.Status == EServiceResponse.Failed);

    public int CountSuccess => _results.Count(r => r.Status == EServiceResponse.Succeeded);

    [DataMember]
    public IEnumerable<Result> Results => _results;

    public class Result
    {
        public Result(TEntityID entityID, string detail, EServiceResponse status)
        {
            EntityID = entityID;
            Detail = detail;
            Status = status;
        }

        public string Detail { get; }

        public TEntityID EntityID { get; }

        public EServiceResponse Status { get; }
    }
}