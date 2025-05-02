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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Corex.Worker.Base;
using Microsoft.Extensions.Logging;

namespace Corex.Worker.Files
{
    public class FileSystemWatcherService : HostedServiceBase
    {
        public FileSystemWatcherService(string watchPath, string watchFilter)
        {
            WatchPath = watchPath;
            WatchFilter = watchFilter;
        }

        public string WatchFilter { get; }

        public string WatchPath { get; }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Whatching {WatchPath}");

            using var watcher = new FileSystemWatcher
                {
                    Path = WatchPath,
                    Filter = WatchFilter,
                    IncludeSubdirectories = false
                };

            watcher.Created +=
                (sender, args) => Task.Run(() => InternalHandleCreated(args, cancellationToken), cancellationToken);

            watcher.Changed +=
                (sender, args) => Task.Run(() => InternalHandleChanged(args, cancellationToken), cancellationToken);

            watcher.Deleted +=
                (sender, args) => Task.Run(() => InternalHandleDeleted(args, cancellationToken), cancellationToken);

            watcher.Renamed +=
                (sender, args) => Task.Run(() => InternalHandleRenamed(args, cancellationToken), cancellationToken);

            watcher.Error +=
                (sender, args) => Task.Run(() => InternalHandleError(args, cancellationToken), cancellationToken);

            await InternalBeforeBeginWatch(cancellationToken);

            watcher.EnableRaisingEvents = true;

            await Task.Delay(-1, cancellationToken);
        }

        protected virtual async Task InternalBeforeBeginWatch(CancellationToken cancellationToken) => await Task.CompletedTask;

        protected virtual void InternalHandleChanged(FileSystemEventArgs args, CancellationToken cancellationToken) { }

        protected virtual void InternalHandleCreated(FileSystemEventArgs args, CancellationToken cancellationToken) { }

        protected virtual void InternalHandleDeleted(FileSystemEventArgs args, CancellationToken cancellationToken) { }

        protected virtual void InternalHandleError(ErrorEventArgs args, CancellationToken cancellationToken) { }

        protected virtual void InternalHandleRenamed(RenamedEventArgs args, CancellationToken cancellationToken) { }
    }
}