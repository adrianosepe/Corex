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
using System.Threading;
using System.Threading.Tasks;
using Corex.Exceptions.Assert;

namespace Corex.Library;

public partial class XHelper
{
    public static partial class Async
    {
        public static class Tasks
        {
            public static string CreateTheadName(object context, string alias)
            {
                XContract.ArgIsNotNull(context, nameof(context));
                XContract.ArgIsNotNull(alias, nameof(alias));

                return $"#Task::{context.GetType().Name}->{alias}";
            }

            public static async Task ForEachAsync<T>(IEnumerable<T> source, int maxParallelCount, Func<T, Task> action)
            {
                source = source.ToArray();

                using var completeSemaphoreSlim = new SemaphoreSlim(1);
                using var taskCountLimitSemaphoreSlim = new SemaphoreSlim(maxParallelCount);

                await completeSemaphoreSlim.WaitAsync();
                var runningTaskCount = source.Count();

                foreach (var item in source)
                {
                    await taskCountLimitSemaphoreSlim.WaitAsync();

#pragma warning disable 4014
                    Task.Run(async () =>
#pragma warning restore 4014
                        {
                            try
                            {
                                await action(item).ContinueWith(
                                    task =>
                                        {
                                            Interlocked.Decrement(ref runningTaskCount);
                                            if (runningTaskCount == 0)
                                            {
                                                completeSemaphoreSlim.Release();
                                            }
                                        });
                            }
                            finally
                            {
                                taskCountLimitSemaphoreSlim.Release();
                            }
                        });
                }

                await completeSemaphoreSlim.WaitAsync();
            }

            public static Task Run(object context, string alias, Action actionToRun)
            {
                XContract.ArgIsNotNull(context, nameof(context));
                XContract.ArgIsNotNull(alias, nameof(alias));
                XContract.ArgIsNotNull(actionToRun, nameof(actionToRun));

                return Task.Run(
                    () =>
                        {
                            Thread.CurrentThread.Name = CreateTheadName(context, alias);

                            try
                            {
                                actionToRun();
                            }
                            catch (Exception ex)
                            {
                                XDebug.OnException(ex);

                                throw;
                            }
                        });
            }

            public static Task<T> Run<T>(object context, string alias, Func<T> actionToRun)
            {
                XContract.ArgIsNotNull(context, nameof(context));
                XContract.ArgIsNotNull(alias, nameof(alias));
                XContract.ArgIsNotNull(actionToRun, nameof(actionToRun));

                return Task.Run(
                    () =>
                        {
                            Thread.CurrentThread.Name = CreateTheadName(context, alias);

                            try
                            {
                                return actionToRun();
                            }
                            catch (Exception ex)
                            {
                                XDebug.OnException(ex);

                                throw;
                            }
                        });
            }

            public static Task Wait(TimeSpan wait) => Task.Delay(wait);
        }
    }
}