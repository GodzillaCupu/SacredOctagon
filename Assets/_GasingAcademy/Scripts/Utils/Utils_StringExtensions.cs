namespace DGE.Utils
{
    public class Utils_StringExtensions
    {
        public const string NUM_SEPARATOR = ".";

        public static string FormatNumber(long num, string separator = NUM_SEPARATOR)
        {
            if (num == 0) return "0";
            string formattedNumber = num.ToString("#,#").Replace(",", separator);
            return formattedNumber;
        }
    }
}