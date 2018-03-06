using Common.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public static class CommonExtensions
{

    #region Defaults
    public static bool IsNotDefault(this bool value)
    {
        return value != default(bool);
    }
    public static bool IsNotDefault(this bool? value)
    {
        return value != default(bool?);
    }
    public static bool IsNotDefault(this decimal value)
    {
        return value != default(decimal);
    }
    public static bool IsNotDefault(this decimal? value)
    {
        return value != default(decimal?);
    }
    public static bool IsNotDefault(this DateTime value)
    {
        return value != default(DateTime);
    }
    public static bool IsNotDefault(this DateTime? value)
    {
        return value.IsNotNull() && value.Value != default(DateTime);
    }
    public static bool IsNotDefault(this byte value)
    {
        return !IsDefault(value);
    }
    public static bool IsNotDefault(this int value)
    {
        return !IsDefault(value);
    }
    public static bool IsNotDefault(this byte[] values)
    {
        return !IsDefault(values);
    }
    public static bool IsNotDefault(this int[] values)
    {
        return !IsDefault(values);
    }
    public static bool IsDefault(this byte[] values)
    {
        return values == default(byte[]);
    }
    public static bool IsDefault(this int[] values)
    {
        return values == default(int[]);
    }
    public static bool IsNotDefault(this byte? value)
    {
        return !IsDefault(value);
    }
    public static bool IsNotDefault(this int? value)
    {
        return !IsDefault(value);
    }
    public static bool IsDefault(this byte value)
    {
        return value == default(byte);
    }
    public static bool IsDefault(this int value)
    {
        return value == default(int);
    }
    public static bool IsDefault(this byte? value)
    {
        return value == default(byte?);
    }
    public static bool IsDefault(this int? value)
    {
        return value == default(int?);
    }
    public static bool IsNotNull(this object obj)
    {
        return obj != null;
    }
    #endregion

    #region Nulls

    public static bool IsNullOrEmpaty<T>(this IEnumerable<T> obj)
    {
        return obj.IsNull() || obj.Count() == 0;

    }
    public static bool IsNull(this object obj)
    {
        return obj == null;
    }
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }
    public static bool IsNotNullOrEmpty(this string value)
    {
        return !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);
    }

    #endregion

    #region Ready

    public static bool IsReady(this int value)
    {
        return IsSent(value) && value > 0;
    }
    public static bool IsReady(this short value)
    {
        return IsSent(value) && value > 0;
    }

    public static bool IsReady(this Int64 value)
    {
        return IsSent(value) && value > 0;
    }

    public static bool IsReady(this Int64? value)
    {
        return IsSent(value) && value > 0;
    }

    #endregion

    #region Sents
    public static bool IsSent(this Guid value)
    {
        return value != null ;
    }
    public static bool IsSent(this short value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this Int64 value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this Int64? value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this Int16? value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this byte[] value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this byte? value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this byte value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this int value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this int[] values)
    {
        return IsNotDefault(values);
    }
    public static bool IsSent(this decimal value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this decimal? value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this string value)
    {
        return IsNotNullOrEmpty(value);
    }
    public static bool IsSent(this float? value)
    {
        return value.IsNotNull();
    }
    public static bool IsSent(this float value)
    {
        return value.IsNotNull();
    }
    public static bool IsSent(this int? value)
    {
        return value.IsNotNull();
    }
    public static bool IsSent(this bool value)
    {
        return value.IsNotDefault();
    }
    public static bool IsSent(this bool? value)
    {
        return value.IsNotDefault();
    }
    public static bool IsSent(this DateTime value)
    {
        return IsNotDefault(value);
    }
    public static bool IsSent(this DateTime? value)
    {
        return value.IsNotNull();
    }
    #endregion

    #region IsNotSent
    public static bool IsNotSent(this string value)
    {
        return !value.IsSent() || value == " ";
    }
    #endregion

    #region Anys
    public static bool IsAny<T>(this ICollection<T> obj)
    {
        return obj.IsNotNull() && obj.Count() > 0;
    }
    public static bool IsAny<T>(this IEnumerable<T> obj)
    {
        return obj.IsNotNull() && obj.Count() > 0;
    }
    public static bool NotIsAny<T>(this ICollection<T> obj)
    {
        return !obj.IsAny();
    }
    public static bool NotIsAny<T>(this IEnumerable<T> obj)
    {
        return !obj.IsAny();
    }
    #endregion

    #region Comparative
    public static bool IsEqualZero(this int value)
    {
        return value == 0;
    }
    public static bool IsEqualZero(this decimal value)
    {
        return value == 0;
    }
    public static bool IsMoreThanZero(this decimal value)
    {
        return value > 0;
    }
    public static bool IsMoreThanZero(this int value)
    {
        return value > 0;
    }
    public static bool IsNegative(this decimal value)
    {
        return value < 0;
    }
    public static bool IsNegative(this int value)
    {
        return value < 0;
    }
    public static bool IsPositive(this decimal value)
    {
        return value >= 0;
    }
    public static bool IsPositive(this int value)
    {
        return value >= 0;
    }

    #endregion

    #region Convert
    public static string ToDecimalenUS(this decimal value)
    {
        return value.ToString(CultureInfo.GetCultureInfo("en-US").NumberFormat);
    }
    public static string ToDecimalenUS(this decimal? value)
    {
        return value.IsNotNull() ? value.Value.ToString(CultureInfo.GetCultureInfo("en-US").NumberFormat) : "0";
    }
    public static string ToCurrency(this decimal value)
    {
        return string.Format("{0:N2}", value);
    }
    public static string ToCurrency(this decimal? value)
    {
        return string.Format("{0:N2}", value);
    }
    public static string ToPercentage(this decimal instance)
    {
        return (instance / 100).ToString("p");
    }
    public static string ToPercentage(this decimal? instance)
    {
        if (instance.IsNull())
            return "";
        else
            return ToPercentage(instance.Value);
    }
    public static DateTime ToDate(this string value)
    {
        try
        {
            DateTime result;
            DateTime.TryParse(value, out result);
            return result;
        }
        catch
        {
            return new DateTime();
        }
    }
    public static int ToInt(this string zipCode)
    {
        try
        {
            return Convert.ToInt32(zipCode);
        }
        catch
        {
            return 0;
        }
    }
    public static int ToInt(this decimal valor)
    {
        try
        {
            return Convert.ToInt32(valor);
        }
        catch
        {
            return 0;
        }
    }
    public static int ToInt(this decimal? valor)
    {
        try
        {
            return Convert.ToInt32(valor.Value);
        }
        catch
        {
            return 0;
        }
    }

    public static string RemoveAccents(this string text)
    {
        StringBuilder sbReturn = new StringBuilder();
        var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
        foreach (char letter in arrayText)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                sbReturn.Append(letter);
        }
        return sbReturn.ToString();
    }

    public static string ToUpperCase(this string str, bool removeAccents = false)
    {
        return str.ToUpperCase(string.Empty, removeAccents);
    }

    public static string ToUpperCase(this string str, string caseNull, bool removeAccents = false)
    {
        if (str == null)
            return caseNull;

        if (str == string.Empty)
            return caseNull;

        if (removeAccents)
            return str.ToUpper().RemoveAccents();

        return str.ToUpper();
    }

    public static string ToLowerCase(this string str, bool removeAccents = false)
    {
        return str.ToLowerCase(string.Empty, removeAccents);
    }

    public static string ToLowerCase(this string str, string caseNull, bool removeAccents = false)
    {
        if (str == null)
            return caseNull;

        if (str == string.Empty)
            return caseNull;

        if (removeAccents)
            return str.ToLower().RemoveAccents();

        return str.ToLower();
    }

    public static string CapitalizarNome(this string str)
    {
        string[] excecoes = new string[] { "e", "de", "da", "das", "do", "dos" };
        var palavras = new Queue<string>();
        foreach (var palavra in str.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (!excecoes.Contains(palavra))
                palavras.Enqueue(char.ToUpper(palavra[0]) + palavra.Substring(1));
            else
                palavras.Enqueue(palavra);

        }
        return string.Join(" ", palavras);
    }

    public static string FisrtCharToLower(this string str)
    {
        if (str.IsNullOrEmpaty())
            return str;

        var firstLower = str.FirstOrDefault().ToString().ToLower();
        var result = string.Format("{0}{1}", firstLower, str.Substring(1, str.Length - 1));

        return result;
    }

    public static string CleanUnnecessarySpaces(this string str)
    {
        if (!str.IsSent())
            return str;

        var newStr = string.Empty;

        newStr = Regex.Replace(str, @"^\s+", ""); // First caracter as space
        newStr = Regex.Replace(str, @"\s+$", ""); // Last caracter as space
        newStr = Regex.Replace(str, @"[ ]{2,}", " "); // Double or more spaces in middle

        return newStr;
    }



    #endregion

    #region Types
    public static bool IsNumber(this string value)
    {
        return _IsNumber(value);
    }
    public static bool IsNumber(this object value)
    {
        return _IsNumber(value);
    }
    private static bool _IsNumber(object value)
    {
        if (value.IsNotNull() && value.ToString() == "-") return false;
        var valueString = Convert.ToString(value);
        var regex = new Regex(@"^-?(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$");

        if (valueString.IsNullOrEmpaty())
            return false;

        if (regex.IsMatch(valueString.Replace(",", ".")))
            return !(valueString.Length > 29);

        return false;
    }

    public static bool IsDecimal(this object value)
    {
        if (value.IsNull())
            return false;

        var valueString = value.ToString();
        return valueString.ToString().Contains(".") || valueString.Contains(",");
    }
    public static bool IsEmail(this string value)
    {
        var regex = new Regex(@"[\w-]+@([\w-]+\.)+[\w-]+");
        return regex.IsMatch(value);
    }
    public static bool IsDigit(this string value)
    {
        foreach (var item in value)
        {
            if (!char.IsDigit(item))
                return false;
        }

        return true;

    }
    public static bool IsDate(this string value)
    {
        DateTime date;
        return DateTime.TryParse(value, out date);
    }
    #endregion

    #region Others


    public static T SetNullToEmptyInstance<T>(this object obj) where T : DomainBase
    {

        var props = typeof(T).GetProperties();

        var instanceIsNull = true;
        foreach (var item in props)
        {

            instanceIsNull = item.IsNullOrDefault(obj);
            if (instanceIsNull == false)
                break;

        }

        return instanceIsNull ? null : (T)obj;
    }
    public static T returnNotNull<T>(this T model)
    {
        var newInstance = Activator.CreateInstance(typeof(T));
        return model.IsNotNull() ? model : (T)newInstance;

    }


    #endregion

}

