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
using System.Threading;
using Corex.Exceptions.Assert;

namespace Corex.Library;

public partial class XHelper
{
    // ReSharper disable InconsistentNaming
    public static class I18n
        // ReSharper restore InconsistentNaming
    {
        public enum ELanguage
        {
            Invariant = 0,
            Brazil = 1,
            Paraguay = 2
        }

        private static ELanguage _currentLanguage;
        private static EventHandler _currentLanguageChanged;

        static I18n()
        {
            _currentLanguage = ELanguage.Invariant;

            InternalCurrentLanguageChanged();
        }

        public static CultureInfo CurrentCulture { get; private set; }

        public static ELanguage CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    XContract.IsEnumValid(nameof(CurrentLanguage), value);

                    _currentLanguage = value;

                    InternalCurrentLanguageChanged();

                    _currentLanguageChanged?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        public static string NumberDecimalSeparator => CurrentCulture.NumberFormat.NumberDecimalSeparator;

        public static event EventHandler CurrentLanguageChanged
        {
            add
            {
                _currentLanguageChanged += value;

                value(typeof(I18n), EventArgs.Empty);
            }
            remove => _currentLanguageChanged -= value;
        }

        private static CultureInfo IdentifyCulture(ELanguage currentLanguage)
        {
            switch (currentLanguage)
            {
                case ELanguage.Brazil:
                    return new CultureInfo("pt-BR");

                case ELanguage.Paraguay:
                    return new CultureInfo("es-PY");

                default:
                    return CultureInfo.InvariantCulture;
            }
        }

        private static void InternalCurrentLanguageChanged()
        {
            CurrentCulture = IdentifyCulture(_currentLanguage);

            Thread.CurrentThread.CurrentCulture = CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = CurrentCulture;

            CultureInfo.DefaultThreadCurrentCulture = CurrentCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CurrentCulture;
        }
    }
}