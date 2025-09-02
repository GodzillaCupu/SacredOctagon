using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DGE.Utils
{
    public static class Utils_StringExtensions
    {
        public const string NUM_SEPARATOR = ".";

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }

        public static string
            ReformatErrorText(string text) // Fix text yang numpuk di Market bagian dekorasi dan lootbox.
        {
            text = Regex.Replace(text, "[^0-9a-zA-Z]+", " ");
            return text;
        }
        public static string
            RemoveInvalidCharsForFileName(this string text) // Fix text yang numpuk di Market bagian dekorasi dan lootbox.
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return Regex.Replace(text, "[^a-zA-Z0-9_-]", "");
        }

        public static string ReplaceAt(this string str, int index, int length, string replace)
        {
            return str.Remove(index, Math.Min(length, str.Length - index))
                .Insert(index, replace);
        }

        public static string FormatNumber(float num, string separator = NUM_SEPARATOR)
        {
            if (num == 0) return "0";
            bool isNegative = num < 0;
            num = MathF.Abs(num);
            bool hasDecimals = HasDecimalNumber(num);
            string decimals = string.Empty;
            if (hasDecimals) decimals = GetDecimalNumber(num);
            //Debug.Log($"[Format] Num to string : {decimals}");
            string formattedNumber = hasDecimals ? Mathf.FloorToInt(num).ToString("#,#").Replace(",", separator) : num.ToString("#,#").Replace(",", separator);
            //Debug.Log($"[Format] Formatted : {formattedNumber}");
            if (string.IsNullOrEmpty(formattedNumber)) formattedNumber = "0";
            if (hasDecimals) formattedNumber += $",{decimals}";
            if (isNegative) formattedNumber = $"-{formattedNumber}";
            //Debug.Log($"[Format] Formatted : {formattedNumber}");
            return formattedNumber;
        }
        public static string FormatNumber(string oldNum, string separator = NUM_SEPARATOR)
        {
            if (float.TryParse(oldNum, out float newNum))
                return FormatNumber(newNum, separator);
            return oldNum;
        }

        public static string FormatNumber(long num, string separator = NUM_SEPARATOR)
        {
            if (num == 0) return "0";
            string formattedNumber = num.ToString("#,#").Replace(",", separator);
            return formattedNumber;
        }

        public static string FormatNumberToLatter(int num)
        {
            int satuan = num % 10;
            int puluhan = num % 100 / 10;
            int ratusan = num % 1000 / 100;
            string format = num switch
            {
                <= 11 => Parse(num),
                > 11 and < 20 => Parse(satuan) + " belas",
                >= 20 and < 100 => $"{Parse(puluhan)} puluh {Parse(satuan).ToLower()}".Trim(),
                >= 100 and <= 111 => $"{Parse(100)} {Parse(num - 100)}",
                _ => ""
            };

            string Parse(int num)
            {
                return num switch
                {
                    0 => "",
                    1 => NumberLatter.Satu.ToString(),
                    2 => NumberLatter.Dua.ToString(),
                    3 => NumberLatter.Tiga.ToString(),
                    4 => NumberLatter.Empat.ToString(),
                    5 => NumberLatter.Lima.ToString(),
                    6 => NumberLatter.Enam.ToString(),
                    7 => NumberLatter.Tujuh.ToString(),
                    8 => NumberLatter.Delapan.ToString(),
                    9 => NumberLatter.Sembilan.ToString(),
                    10 => NumberLatter.Sepuluh.ToString(),
                    11 => NumberLatter.Sebelas.ToString(),
                    100 => NumberLatter.Seratus.ToString(),
                    _ => num.ToString()
                };
            }
            return format.ToLower();
        }
        public static bool HasDecimalNumber(float num)
        {
            string decimals = num.ToString();
            bool hasDecimals = decimals.Contains(".");
            return hasDecimals;
        }

        public static string GetDecimalNumber(float num)
        {
            string decimals = num.ToString();
            decimals = decimals.Remove(0, decimals.IndexOf('.') + 1);
            return decimals;
        }

        /// <summary>
        /// Capitalize the first character of a string and after a point
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CapitalizeAfterPeriod(string text)
        {
            return Regex.Replace(text, @"(^\w)|(\.\s*\w)", m => m.Value.ToUpper());
        }

        enum NumberLatter
        {
            Nol, Satu, Dua, Tiga, Empat, Lima, Enam, Tujuh, Delapan, Sembilan, Sepuluh, Sebelas, Seratus
        }
    }
}