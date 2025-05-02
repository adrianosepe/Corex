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
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Corex.Exceptions.Contract;
using Corex.Library.Properties;

namespace Corex.Library.ComponentModel.Model.Validation.Annotations.TypedNumber;

public class XRangeAttribute : RangeAttribute
{
    public enum EPreSetBehavior
    {
        /// <summary>
        ///     Valores 4dig maior que zero ate maximo decimal valido (restrito em 9999999999.9999M)
        /// </summary>
        CurrencyGreaterThanZeroAndMaxDecimal4Dig,

        /// <summary>
        ///     Valores 2dig maior que zero ate maximo decimal valido (restrito em 9999999999.99M)
        /// </summary>
        CurrencyGreaterThanZeroAndMaxDecimal2Dig,

        /// <summary>
        ///     Percentual int entre 1 e 99%
        /// </summary>
        PercentGreaterThanZeroAnd99Int,

        /// <summary>
        ///     Percentual 2Dig margens de descontos entre 0.00 e 99.99%
        /// </summary>
        PercentMax1002Dig,

        /// <summary>
        ///     Percentual 2Dig margens de descontos entre 0.00 e 99.99%
        /// </summary>
        PercentMax10002Dig,

        /// <summary>
        ///     Percentual 2Dig margens de lucro entre -99.00 e 999.99%
        /// </summary>
        PercentMinus99Until999Decimal2Dig,

        /// <summary>
        ///     Inteiro maior que zero
        /// </summary>
        IntegerGreatThanZero,

        /// <summary>
        ///     Inteiro maior ou igual a zero
        /// </summary>
        IntegerGreatOrEqualZero
    }

    public enum ERangeBehavior
    {
        ValueInRange,
        ValueOutRange
    }

    private static readonly Dictionary<EPreSetBehavior, object[]> PreSetBehaviors;

    private readonly double? _maximum;
    private readonly double? _minimum;

    static XRangeAttribute()
    {
        PreSetBehaviors = new Dictionary<EPreSetBehavior, object[]>
            {
                {
                    EPreSetBehavior.CurrencyGreaterThanZeroAndMaxDecimal4Dig, new object[]
                        {
                            typeof(decimal), 0.0001M.ToString(CultureInfo.CurrentCulture), 9999999999.9999M.ToString(CultureInfo.CurrentCulture)
                        }
                },
                {
                    EPreSetBehavior.CurrencyGreaterThanZeroAndMaxDecimal2Dig, new object[]
                        {
                            typeof(decimal), 0.0001M.ToString(CultureInfo.CurrentCulture), 9999999999.99M.ToString(CultureInfo.CurrentCulture)
                        }
                },
                {
                    EPreSetBehavior.PercentGreaterThanZeroAnd99Int, new object[]
                        {
                            typeof(int), 1.ToString(), 99.ToString()
                        }
                },
                {
                    EPreSetBehavior.PercentMax1002Dig, new object[]
                        {
                            typeof(decimal), 0.00M.ToString(CultureInfo.CurrentCulture), 99.99M.ToString(CultureInfo.CurrentCulture)
                        }
                },
                {
                    EPreSetBehavior.PercentMax10002Dig, new object[]
                        {
                            typeof(decimal), 0.00M.ToString(CultureInfo.CurrentCulture), 999.99M.ToString(CultureInfo.CurrentCulture)
                        }
                },
                {
                    EPreSetBehavior.PercentMinus99Until999Decimal2Dig, new object[]
                        {
                            typeof(decimal), (-99.00M).ToString(CultureInfo.CurrentCulture), 999.99M.ToString(CultureInfo.CurrentCulture)
                        }
                },
                {
                    EPreSetBehavior.IntegerGreatThanZero, new object[]
                        {
                            typeof(int), 1.ToString(), int.MaxValue.ToString()
                        }
                },
                {
                    EPreSetBehavior.IntegerGreatOrEqualZero, new object[]
                        {
                            typeof(int), 0.ToString(), int.MaxValue.ToString()
                        }
                }
            };
    }

    public XRangeAttribute(EPreSetBehavior preSetBehavior) :
        base((Type) PreSetBehaviors[preSetBehavior][0], (string) PreSetBehaviors[preSetBehavior][1], (string) PreSetBehaviors[preSetBehavior][2])
    {
        _minimum = 1;
        _maximum = 2;

        ErrorMessage = CreateErrorMessage();
    }

    public XRangeAttribute(
        int minimum, int maximum, ERangeBehavior minValueRangeBehavior = ERangeBehavior.ValueInRange, ERangeBehavior maxValueraRangeBehavior = ERangeBehavior.ValueInRange)
        : base(UpdateValue(minimum, minValueRangeBehavior, () => minimum + 1), UpdateValue(maximum, maxValueraRangeBehavior, () => maximum - 1))
    {
        var min = (int) Minimum;
        var max = (int) Maximum;

        _minimum = min == int.MinValue ? (double?) null : min;
        _maximum = max == int.MaxValue ? (double?) null : max;

        ThrowInitializationExceptionWhenMinAndMaxIsNull();

        ErrorMessage = CreateErrorMessage();
    }

    public XRangeAttribute(
        double minimum, double maximum, ERangeBehavior minValueRangeBehavior = ERangeBehavior.ValueInRange,
        ERangeBehavior maxValueraRangeBehavior = ERangeBehavior.ValueInRange)
        : base(UpdateValue(minimum, minValueRangeBehavior, () => minimum + double.Epsilon), UpdateValue(maximum, maxValueraRangeBehavior, () => maximum - double.Epsilon))
    {
        var min = (double) Minimum;
        var max = (double) Maximum;

        // ReSharper disable CompareOfFloatsByEqualityOperator
        _minimum = min == double.MinValue ? (double?) null : min;
        _maximum = max == double.MaxValue ? (double?) null : max;
        // ReSharper restore CompareOfFloatsByEqualityOperator

        ThrowInitializationExceptionWhenMinAndMaxIsNull();

        ErrorMessage = CreateErrorMessage();
    }

    private string CreateErrorMessage()
    {
        if (_minimum == null)
        {
            return DbMessages.XRangeAttribute_CreateErrorMessage_O_campo__0__permite_um_valor_máximo__2_;
        }

        if (_maximum == null)
        {
            return DbMessages.XRangeAttribute_CreateErrorMessage_O_campo__0__permite_um_valor_mínimo__1_;
        }

        return DbMessages.XRangeAttribute_CreateErrorMessage_O_campo__0__permite_um_valor_entre__1__e__2_;
    }

    private void ThrowInitializationExceptionWhenMinAndMaxIsNull()
    {
        if (_minimum == null && _maximum == null)
        {
            throw XInitializeException.Create($"Não é possível definir um {nameof(XRangeAttribute)} sem informar ao menos um limite.");
        }
    }

    private static T UpdateValue<T>(T value, ERangeBehavior valueRangeBehavior, Func<T> funcValueWhenOutRange) where T : struct =>
        valueRangeBehavior == ERangeBehavior.ValueOutRange
            ? funcValueWhenOutRange()
            : value;
}