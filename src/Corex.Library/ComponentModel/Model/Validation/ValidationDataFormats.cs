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
using System.Text.RegularExpressions;

// ReSharper disable UnusedMember.Global

namespace Corex.Library.ComponentModel.Model.Validation;

public static class ValidationDataFormats
{
    public const string RegexPlacaVeiculoPattern = "^[a-zA-Z]{3}[0-9]{4}$";
    public const string RegexLongitudePattern = @"^-?([1]?[1-7][1-9]|[1]?[1-8][0]|[1-9]?[0-9])\\,{1}\\d{1,10}$";
    public const string RegexLatitudePattern = @"^-?([1-8]?[1-9]|[1-9]0)\\,{1}\\d{1,10}$";

    public const string GeoEnderecoNumeroRegex = @"\d{1,9}";

    public const string GeoCepMask = @"\d\d\d\d\d-\d\d\d";
    public const string GeoCepRegex = @"^\d{2}[\.]?\d{3}-?\d{3}$"; //86.026-200 ou  86026200

    public const string GeoEstadoIDRegex = @"^\d{1,2}$";

    public const string GeoMunicipioIDMask = "0000000";
    public const string GeoMunicipioIDRegex = @"^\d{0|7}$";

    public const string ContactFoneMask9Digits = "(00) 00000-0000";
    public const string RegexTelefonePattern = @"^\([1-9]{2}\)[0-9]{4}\-[0-9]{4}$";
    public const string RegexTelefonePatternFull9Digits = @"^((\+?\d{2,3})?\(\d{2}\)\d{4,5}-\d{4}|(\+?\d{2,3})?\d{10,11})$"; // +55(43)99156-2225 ou +5543991562225

    public const string ContactEmailMask = @"^(\w|[\.\-])+@(\w|[\-]+\.)*(\w|[\-]){2,63}\.[a-zA-Z]{2,4}$";
    public const string ContactEmailRegex = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";

    public const string ContactFoneMask = "(00) 0000-0000";
    public const string ContactFoneRegex = @"^((\+?\d{2,3})?\s*\(\d{2}\)\s*\d{4}-\d{4}|(\+?\d{2,3})?\d{10})$"; // +55(43)9156-2225 ou +554391562225

    public const string ContactUrlMask = "((http|https)://)?([a-zA-Z0-9.]|%[0-9A-Za-z]|/|:[0-9]?)*";

    public const string ContactUrlProtocol = @"(ftp|http|https):\/\/";

    public const string ContactUrlBody = @"(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?";

    public const string ContactUrlReqProtocol = "^" + ContactUrlProtocol + ContactUrlBody + "$";
    public const string ContactUrlOpProtocol = "^(" + ContactUrlProtocol + ")?" + ContactUrlBody + "$";
    public const string ContactUrlWithoutProtocol = "^" + ContactUrlBody + "$";

    public const string DocInscEstadualMask = "9999999999";
    public const string DocInscEstadualRegex = "^[0-9]{1,20}$";

    public const string DocInscMunicipalMask = "9999999999";
    public const string DocInscMunicipalRegex = "^[0-9]{1,10}$";

    public const string DocRgMask = "000000009";
    public const string DocRgRegex = @"^[0-9]{1,2}[\.]?[0-9]{3}[\.]?[0-9]{3}[-]?[0-9]{1}$";

    public const string DocCpfMask = "000.000.000-00";
    public const string DocCpfRegex = @"^[0-9]{3}[\.]?[0-9]{3}[\.]?[0-9]{3}[-]?[0-9]{2}$";

    public const string DocCnpjMask = "00.000.000/0000-00";
    public const string DocCnpjRegex = @"^[0-9]{2}[\.]?[0-9]{3}[\.]?[0-9]{3}[\/]?[0-9]{4}[-]?[0-9]{2}$";

    public const string DocRucMask = "990.000.000-0";
    public const string DocRucRegex = "^[0-9]{3,15}[-]?[0-9]{1}$";

    public const string BussinesNcmMask = "0000.00.00";
    public const string BussinesNcmRegex = @"^\d{8}$";

    public const string KeyBarcodeMask = "0000000099999";
    public const string KeyBarcodeRegex = @"^\d{8,13}$";

    public const string PasswordRegex = @"^\w{4,20}$";

    public const string HexNumberRegex = "^(?:0[xX])?[0-9a-fA-F]+$";

    public const string RegexGuidPattern = @"^[{|\(]?[0-9a-fA-F]{8}[-]?([0-9a-fA-F]{4}[-]?){3}[0-9a-fA-F]{12}[\)|}]?$";
    public const string RegexHoraPattern = "^([0-1][0-9]|[2][0-3]):([0-5][0-9])$";

    public const string RegexLoginPatternLength6To60 = "[0-9A-Za-z_.]{6,60}";

    public const string RegexNumbersPattern = @" ^ -*[0-9,\.]+$";
    public const string RegexIntegerNumbersPattern = "^-*[0-9]+$";

    public const string IccIDRegex = @"^\w{15,40}$";

    public static bool AllowNullOrEmpty(string input, Func<string, bool> validation) => string.IsNullOrEmpty(input?.Trim()) || validation(input);

    public static bool AreIntegerNumbers(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, RegexIntegerNumbersPattern);
    }

    public static bool AreNumbers(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, RegexNumbersPattern);
    }

    public static bool IsCep(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, GeoCepRegex);
    }

    public static bool IsCnpj(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, DocCnpjRegex) && VerificaValidadeCnpj(input);
    }

    public static bool IsCpf(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, DocCpfRegex) && VeirifcaValidadeCpf(input);
    }

    public static bool IsEmail(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, ContactEmailRegex);
    }

    public static bool IsGuid(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, RegexGuidPattern);
    }

    public static bool IsHora(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, RegexHoraPattern);
    }

    public static bool IsIccID(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, IccIDRegex);
    }

    public static bool IsIPv4(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        var splitValues = input.Split('.');
        if (splitValues.Length != 4)
        {
            return false;
        }

        return splitValues.All(r => byte.TryParse(r, out _));
    }

    public static bool IsLatitude(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, RegexLatitudePattern);
    }

    /// <summary>
    ///     Verifica se o login está no formato válido,
    ///     aceitá login de tamanho entre 6-60 caracteres
    /// </summary>
    /// <param name="input">Login</param>
    /// <returns>
    ///     <c>true</c> if [is login pattern (6-60 chars) valid format] [the specified input]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsLoginPatternLength6To60(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, RegexLoginPatternLength6To60);
    }

    public static bool IsLongitude(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, RegexLongitudePattern);
    }

    public static bool IsPlaca(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, RegexPlacaVeiculoPattern);
    }

    public static bool IsRg(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, DocRgRegex);
    }

    public static bool IsRuc(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, DocRucRegex) && VerificaValidadeRuc(input);
    }

    public static bool IsTelefone(string input, bool accept9Digits = true)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, accept9Digits ? RegexTelefonePatternFull9Digits : ContactFoneRegex);
    }

    public static bool IsTelefone9Digits(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, RegexTelefonePatternFull9Digits);
    }

    public static bool IsUrl(string input)
    {
        if (!XHelper.Strings.HasData(input))
        {
            return false;
        }

        return Regex.IsMatch(input, ContactUrlReqProtocol);
    }

    public static bool VeirifcaValidadePis(string pis)
    {
        try
        {
            var multiplicador = new[] {3, 2, 9, 8, 7, 6, 5, 4, 3, 2};

            if (pis.Trim().Length != 11)
            {
                return false;
            }

            pis = pis.Trim();
            pis = pis.Replace("-", string.Empty).Replace(".", string.Empty).PadLeft(11, '0');

            var sum = 0;
            for (var i = 0; i < 10; i++)
            {
                sum += int.Parse(pis[i].ToString()) * multiplicador[i];
            }

            var value = sum % 11;
            value = value < 2 ? 0 : 11 - value;

            return pis.EndsWith(value.ToString());
        }
        catch
        {
            return false;
        }
    }

    public static bool VerificaValidadeCnpj(string cnpj)
    {
        try
        {
            var multiplicador1 = new[] {5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2};
            var multiplicador2 = new[] {6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2};

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);

            if (cnpj.Length != 14)
            {
                return false;
            }

            var value = cnpj.Substring(0, 12);

            var soma = 0;
            for (var i = 0; i < 12; i++)
            {
                soma += int.Parse(value[i].ToString()) * multiplicador1[i];
            }

            var resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            var digito = resto.ToString();

            soma = 0;
            value += digito;
            for (var i = 0; i < 13; i++)
            {
                soma += int.Parse(value[i].ToString()) * multiplicador2[i];
            }

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto;

            return cnpj.EndsWith(digito);
        }
        catch
        {
            return false;
        }
    }

    private static bool VeirifcaValidadeCpf(string cpf)
    {
        try
        {
            var multiplicador1 = new[] {10, 9, 8, 7, 6, 5, 4, 3, 2};
            var multiplicador2 = new[] {11, 10, 9, 8, 7, 6, 5, 4, 3, 2};

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", string.Empty).Replace("-", string.Empty);

            if (cpf.Length != 11)
            {
                return false;
            }

            var sum = 0;
            var value = cpf.Substring(0, 9);
            for (var i = 0; i < 9; i++)
            {
                sum += int.Parse(value[i].ToString()) * multiplicador1[i];
            }

            var resto = sum % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            var digito = resto.ToString();

            sum = 0;
            value += digito;
            for (var i = 0; i < 10; i++)
            {
                sum += int.Parse(value[i].ToString()) * multiplicador2[i];
            }

            resto = sum % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto;

            return cpf.EndsWith(digito);
        }
        catch
        {
            return false;
        }
    }

    private static bool VerificaValidadeRuc(string input)
    {
        try
        {
            input = input.Replace("-", string.Empty);

            const int baseMax = 11;

            var dig = Convert.ToInt32(input[input.Length - 1].ToString());
            var ruc = input.Remove(input.Length - 1, 1);

            var fator = 2;
            var soma = 0;

            for (var i = ruc.Length - 1; i >= 0; i--)
            {
                soma += Convert.ToInt32(ruc[i].ToString()) * fator;

                fator++;
                if (fator > baseMax)
                {
                    fator = 2;
                }
            }

            var resto = soma % baseMax;
            var verificador = resto > 1 ? baseMax - resto : 0;

            return verificador == dig;
        }
        catch
        {
            return false;
        }
    }
}