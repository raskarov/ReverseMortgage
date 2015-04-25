using System;

namespace LoanStar.Common
{
    /// <summary>
    /// Converts currency from numbers to words
    /// Example: NumberToWord.WordFromNumber(123645.23)
    /// </summary>
    public class NumberToWord
    {
        private static readonly string[] onesMapping =
            new string[] {
            "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine",
            "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
        };
        private static readonly string[] tensMapping =
            new string[] {
            "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
        };
        private static readonly string[] groupMapping =
            new string[] {
            "Hundred", "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion", "Sextillian",
            "Septillion", "Octillion", "Nonillion", "Decillion", "Undecillion", "Duodecillion", "Tredecillion",
            "Quattuordecillion", "Quindecillion", "Sexdecillion", "Septendecillion", "Octodecillion", "Novemdecillion",
            "Vigintillion", "Unvigintillion", "Duovigintillion", "10^72", "10^75", "10^78", "10^81", "10^84", "10^87",
            "Vigintinonillion", "10^93", "10^96", "Duotrigintillion", "Trestrigintillion"
        };

        // NOTE: 10^303 is approaching the limits of double, as ~1.7e308 is where we are going
        // 10^303 is a centillion and a 10^309 is a duocentillion


        public static string WordFromNumber(int number)
        {
            return WordFromNumber((long)number);
        }

        public static string WordFromNumber(long number)
        {
            return WordFromNumber((double)number);
        }

        public static string WordFromNumber(string number)
        {
            return WordFromNumber(Convert.ToDouble(number));
        }

        public static string WordFromNumber(decimal number)
        {
            if (number == 0)
            {
//                return "0 dollars and 0 cents";
                  return "";
            }
            return WordFromNumber(Convert.ToDouble(number));
        }

        public static string WordFromNumber(double number)
        {
            if (number == 0)
                return "";
            string sign = null;
            if (number < 0)
            {
                sign = "Negative";
                number = Math.Abs(number);
            }

            //int decimalDigits = 0;
            //double num = number;
            //while (num < 1 || (num - Math.Floor(num) > 1e-10))
            //{
            //    num *= 10;
            //    decimalDigits++;
            //}
            //string decimalString = null;
            //if (decimalDigits > 0)
            //{
            //    string[] str = number.ToString().Split(new string[] { "." }, StringSplitOptions.None);
            //    if (str.Length > 1)
            //    {
            //        decimalString = str[1];
            //    }
            //    decimalString += " cents";
            //}
            //else
            //{
            //    decimalString += "00 cents";
            //}
            string retVal = null;
            int group = 0;
            if (number < 1)
            {
                retVal = onesMapping[0];
            }
            else
            {
                while (number >= 1)
                {
                    int numberToProcess = (number >= 1e16) ? 0 : (int)(number % 1000);
                    number = number / 1000;

                    string groupDescription = ProcessGroup(numberToProcess);
                    if (groupDescription != null)
                    {
                        if (group > 0)
                        {
                            retVal = groupMapping[group] + " " + retVal;
                        }
                        retVal = groupDescription + " " + retVal;
                    }

                    group++;
                }
            }
            /*
            return String.Format("{0}{4}{1}{3}{2}",
                sign,
                retVal,
                decimalString,
                (decimalString != null) ? "dollars and " : " dollars",
                (sign != null) ? " " : "");
             */
            return String.Format("{0}{2}{1}",
                sign,
                retVal,
                (sign != null) ? " " : "");
        }

        private static string ProcessGroup(int number)
        {
            int tens = number % 100;
            int hundreds = number / 100;

            string retVal = null;
            if (hundreds > 0)
            {
                retVal = onesMapping[hundreds] + " " + groupMapping[0];
                //if (hundreds == 1) retVal = onesMapping[hundreds] + " " + groupMapping[0];
                //else retVal = onesMapping[hundreds] + " " + groupMapping[0] + "s";
            }
            if (tens > 0)
            {
                if (tens < 20)
                {
                    retVal += ((retVal != null) ? " " : "") + onesMapping[tens];
                }
                else
                {
                    int ones = tens % 10;
                    tens = (tens / 10) - 2; // 20's offset

                    retVal += ((retVal != null) ? " " : "") + tensMapping[tens];

                    if (ones > 0)
                    {
                        retVal += ((retVal != null) ? " " : "") + onesMapping[ones];
                    }
                }
            }

            return retVal;
        }
    }
}
