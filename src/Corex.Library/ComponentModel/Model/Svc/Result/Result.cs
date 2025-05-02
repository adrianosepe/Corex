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
using System.Linq;
using System.Threading.Tasks;
using Corex.Library.ComponentModel.Model.Svc.Enums;

namespace Corex.Library.ComponentModel.Model.Svc;

public class Result : TypedResult<object>
{
    public new static Result Error(string error) =>
        new Result
            {
                Message = error,
                Status = EResultStatus.Error,
            };

    public new static Result Error(ServiceResponse response)
    {
        return new Result
            {
                Message = "Ocorreu um erro no processo, verifique o(s) detalhe(s),",
                Status = response.Status == EServiceResponse.Succeeded ? EResultStatus.Ok : EResultStatus.Error,
                Details = response.ValidationResult.Validations.Select(
                    validationResult =>
                        {
                            var key = validationResult.MemberNames.Any()
                                ? validationResult.MemberNames.Aggregate((s, s1) => s + ";" + s1)
                                : string.Empty;

                            return new ResultInfo
                                {
                                    Key = key,
                                    Message = validationResult.ErrorMessage
                                };
                        }).ToArray()
            };
    }

    public new static Result Exception(Exception ex) => Error(ex.Message);

    public static Result Success(object data) => new Result
        {
            Data = data,
            Status = EResultStatus.Ok,
        };

    public new static async Task<Result> Success(Func<Task<object>> funcTask)
    {
        var data = await funcTask();

        return Success(data);
    }

    public new static Result Success(string success) =>
        new Result
            {
                Message = success,
                Status = EResultStatus.Ok,
            };

    public new static Result Warning(string alert) =>
        new Result
            {
                Message = alert,
                Status = EResultStatus.Warning,
            };
}