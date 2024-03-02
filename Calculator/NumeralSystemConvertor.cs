using System;
using System.Globalization;

namespace Сalculator
{
    public static class NumeralSystemConvertor
    {
        public static string ConvertToDecimal(string number, int fromBase, char fractionalSeparator = ',')
        {
            if (fromBase < 2 || fromBase > 10)
                throw new ArgumentException("The fromBase must be >= 2 and <= 10");

            double decimalNumber = 0;
            int separatorIndex = number.IndexOf(fractionalSeparator);
            if (separatorIndex == -1)
                separatorIndex = number.Length;

            for (int i = 0; i < separatorIndex; i++)
            {
                decimalNumber += int.Parse(number[i].ToString()) * (int)Math.Pow(fromBase, separatorIndex - i - 1);
            }

            for (int i = separatorIndex + 1; i < number.Length; i++)
            {
                decimalNumber += int.Parse(number[i].ToString()) * Math.Pow(fromBase, separatorIndex - i);
            }

            return decimalNumber.ToString(CultureInfo.CurrentCulture);
        }

        public static string ConvertFromDecimal(double number, int toBase, int precision = 5,
            char fractionalSeparator = ',')
        {
            if (toBase < 2 || toBase > 10)
                throw new ArgumentException("The toBase must be >= 2 and <= 10");

            if (number == 0)
                return "0";

            int intNumber = (int)number;
            double fractionalNumber = number - intNumber;
            string result = (intNumber % toBase).ToString();


            while ((intNumber /= toBase) > 0)
            {
                result = intNumber % toBase + result;
            }

            if (fractionalNumber > 0)
            {
                result += fractionalSeparator;
                for (int i = 0; i < precision && fractionalNumber > 0; i++)
                {
                    double basePow = Math.Pow(toBase, -i - 1);
                    result += ((int)(fractionalNumber / basePow)).ToString();
                    fractionalNumber -= (int)(fractionalNumber / basePow) * basePow;
                }
            }

            return result;
        }
    }
}