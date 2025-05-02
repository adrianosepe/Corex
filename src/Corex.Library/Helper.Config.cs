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
using System.Configuration;
using Corex.Library.Settings;

namespace Corex.Library;

public static partial class XHelper
{
    public sealed class ConfigWrapper<TKey, TValue> where TValue : class
    {
        private readonly Func<TKey, TValue> _translate;

        internal ConfigWrapper(Func<TKey, TValue> translate)
        {
            _translate = translate;
        }

        public TValue this[TKey name] => _translate(name);

        public TValue Get(TKey key, TValue defaultValue) => this[key] ?? defaultValue;
    }

    public static class Configs
    {
        public static SettingsBuilder XAppSettings(string sectionID) => new SettingsBuilder(sectionID, "AppSettings");
#if !NETSTANDARD
        public static readonly ConfigWrapper<string, string> AppSettings = new ConfigWrapper<string, string>(s => ConfigurationManager.AppSettings[s]);

        public static readonly ConfigWrapper<string, string> ConnStrings = new ConfigWrapper<string, string>(s => ConfigurationManager.ConnectionStrings[s].ConnectionString);

        public static readonly ConfigWrapper<string, ConnectionStringSettings> ConnStringsSettings = new ConfigWrapper<string, ConnectionStringSettings>(s => ConfigurationManager.ConnectionStrings[s]);
#endif
    }
}