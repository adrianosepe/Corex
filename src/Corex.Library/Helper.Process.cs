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
using System.Diagnostics;
using System.Linq;

namespace Corex.Library;

public partial class XHelper
{
    public static class WinProcess
    {
        private static readonly Lazy<Process> LazyCurrent;

        static WinProcess()
        {
            LazyCurrent = new Lazy<Process>(Process.GetCurrentProcess);
        }

        public static Process Current => LazyCurrent.Value;

        public static string GetAppAssembliesRepository() => AppDomain.CurrentDomain.BaseDirectory;

        public static string GetAppBaseDirectory() => AppDomain.CurrentDomain.BaseDirectory;

        public static int GetProcessID() => Process.GetCurrentProcess().Id;

        public static Process GetRunningInstance()
        {
            return Process
                .GetProcessesByName(Current.ProcessName)
                .FirstOrDefault(t => t.Id != Current.Id);
        }

        public static void KillAllSame()
        {
            Process
                .GetProcessesByName(Current.ProcessName)
                .Where(t => t.Id != Current.Id)
                .ToList()
                .ForEach(t => t.Kill());
        }

        public static void RunWithoutWindows(string fileName, string arguments)
        {
            Process.Start(
                new ProcessStartInfo
                    {
                        FileName = fileName,
                        Arguments = arguments,
                        UseShellExecute = true,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    });
        }
    }
}