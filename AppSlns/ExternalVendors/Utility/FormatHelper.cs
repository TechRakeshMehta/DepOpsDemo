using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExternalVendors.Utility
{
    public class FormatHelper
    {
        #region FormatAddress
        public static string FormatAddress(string name, string address1, string address2, string city, string state, string zip)
        {
            StringBuilder residence = new StringBuilder();
            residence.AppendLine("<Table width=\"600\">");
            residence.AppendLine("  <tr>");
            residence.AppendLine("    <th colspan=\"2\">Address " + name + "</th>");
            residence.AppendLine("  </tr>");
            residence.AppendLine("  <tr>");
            residence.AppendLine("    <td class=\"Key\">Address 1:</td>");
            residence.AppendLine("    <td>" + address1 + "</td>");
            residence.AppendLine("  </tr>");
            residence.AppendLine("  <tr>");
            residence.AppendLine("    <td class=\"Key\">Address 2:</td>");
            residence.AppendLine("    <td>" + address2 + "</td>");
            residence.AppendLine("  </tr>");
            residence.AppendLine("  <tr>");
            residence.AppendLine("    <td class=\"Key\">City:</td>");
            residence.AppendLine("    <td>" + city + "</td>");
            residence.AppendLine("  </tr>");
            residence.AppendLine("  <tr>");
            residence.AppendLine("    <td class=\"Key\">State:</td>");
            residence.AppendLine("    <td>" + state + "</td>");
            residence.AppendLine("  </tr>");
            residence.AppendLine("  <tr>");
            residence.AppendLine("    <td class=\"Key\">Zip Code:</td>");
            residence.AppendLine("    <td>" + zip + "</td>");
            residence.AppendLine("  </tr>");
            residence.AppendLine("</table");
            return residence.ToString();
        }
        #endregion

        #region SsnInjectDashes
        public static string SsnInjectDashes(string ssn)
        {
            if (ssn == null)
            {
                return String.Empty;
            }
            string value = ssn;
            Regex re = new Regex(@"(\d\d\d)(\d\d)(\d\d\d\d)");
            if (re.IsMatch(ssn))
            {
                Match match = re.Match(ssn);
                value = match.Groups[1] + "-" + match.Groups[2] + "-" + match.Groups[3];
            }
            return value;
        }
        #endregion

        #region SsnInjectDashesMaskAllButLastFour
        public static string SsnInjectDashesMaskAllButLastFour(string ssn)
        {
            if (ssn == null)
            {
                return String.Empty;
            }
            string value = "xxx-xxx-xxxx";
            if (ssn != null)
            {
                Regex re = new Regex(@"(\d\d\d-\d\d-)(\d\d\d\d)");
                if (re.IsMatch(ssn))
                {
                    Match match = re.Match(ssn);
                    value = "xxx-xx-" + match.Groups[2];
                }
            }
            return value;
        }
        #endregion

        #region SsnGetLastFour
        /// <summary>
        /// Returns the last four of an SSN
        /// </summary>
        /// <param name="ssn">The SSN to parse; MUST BE IN xxx-xx-xxxx</param>
        /// <returns></returns>
        public static string SsnGetLastFour(string ssn)
        {
            if (ssn == null)
            {
                return String.Empty;
            }
            string value = "xxxx";
            if (ssn != null)
            {
                Regex re = new Regex(@"(\d\d\d-\d\d-)(\d\d\d\d)");
                if (re.IsMatch(ssn))
                {
                    Match match = re.Match(ssn);
                    value = match.Groups[2] + "";
                }
            }
            return value;
        }
        #endregion

        #region SsnRemoveDashes
        public static string SsnRemoveDashes(string ssn)
        {
            if (ssn == null)
            {
                return String.Empty;
            }
            Regex r = new Regex("(?:[^0-9 ])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            string firstpass = r.Replace(ssn, String.Empty);

            r = new Regex(@"\s", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            string secondpass = r.Replace(firstpass, String.Empty);

            return secondpass;
        }
        #endregion

        #region DOBInjectDashesMaskYear
        public static string DOBInjectDashesMaskYear(string dob)
        {
            string value = "xx/xx/xxxx";
            if (dob != null)
            {
                Regex re = new Regex(@"(\d?\d/\d?\d/)(\d\d\d\d)");
                if (re.IsMatch(dob))
                {
                    Match match = re.Match(dob);
                    value = match.Groups[1] + "xxxx";
                }
            }
            return value;
        }
        #endregion

        #region CreditCardMask
        public static string CreditCardMask(string cardNumber)
        {
            string value = "xxxxxxxxxxxxxxxxxxxx";
            if (cardNumber != null)
            {
                Regex re = new Regex(@"^\d+(\d\d\d\d)$");
                if (re.IsMatch(cardNumber))
                {
                    Match match = re.Match(cardNumber);
                    value = "xxxxxxxxxxxx" + match.Groups[1];
                }
            }
            return value;
        }
        #endregion

        #region ConvertPhoneFormat
        /// <summary>
        /// This is used to convert one phone format to another.
        /// EX.   xxx-xxx-xxxx to (xxx) xxx-xxxx
        /// 
        /// Codes
        /// ------
        /// Dashes2Parenthesis = xxx-xxx-xxxx to (xxx) xxx-xxxx
        /// Parenrenthesis2Dashes = (xxx) xxx-xxxx to xxx-xxx-xxxx
        /// NumberOnly2Parenthesis = xxxxxxxxxx to (xxx) xxx-xxxx
        /// NumberOnly2Dashes = xxxxxxxxxx to xxx-xxx-xxxx
        /// RemovesEverythingButNumbers = removes formatting from any string and leaves only numbers xxxxxxxxxx
        /// </summary>
        /// <param name="number">phone number in formatted string </param>
        /// <param name="conversiontype">the indicator for which conversion should be attempted.</param>
        /// <returns></returns>
        public static string ConvertPhoneFormat(string number, conversiontype conversiontype)
        {
            if (number == null)
            {
                return String.Empty;
            }
            string converted = string.Empty;

            if (conversiontype == conversiontype.Dashes2Parenthesis)
            {
                Regex re = new Regex(@"^(\d\d\d)-(\d\d\d)-(\d\d\d\d)$");
                if (re.IsMatch(number))
                {
                    Match match = re.Match(number);
                    converted = "(" + match.Groups[1] + ") " + match.Groups[2] + "-" + match.Groups[3];
                }
            }
            if (conversiontype == conversiontype.Parenrenthesis2Dashes)
            {
                Regex re = new Regex(@"^\((\d\d\d)\)\s*(\d\d\d)-(\d\d\d\d)$");
                if (re.IsMatch(number))
                {
                    Match match = re.Match(number);
                    converted = match.Groups[1] + "-" + match.Groups[2] + "-" + match.Groups[3];
                }
            }
            if (conversiontype == conversiontype.NumberOnly2Parenthesis)
            {
                Regex re = new Regex(@"^(\d\d\d)(\d\d\d)(\d\d\d\d)$");
                if (re.IsMatch(number))
                {
                    Match match = re.Match(number);
                    converted = "(" + match.Groups[1] + ") " + match.Groups[2] + "-" + match.Groups[3];
                }
            }
            if (conversiontype == conversiontype.NumberOnly2Dashes)
            {
                Regex re = new Regex(@"^(\d\d\d)(\d\d\d)(\d\d\d\d)$");
                if (re.IsMatch(number))
                {
                    Match match = re.Match(number);
                    converted = match.Groups[1] + "-" + match.Groups[2] + "-" + match.Groups[3];
                }
            }
            if (conversiontype == conversiontype.RemovesEverythingButNumbers)
            {
                Regex r = new Regex("(?:[^0-9 ])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                string firstpass = r.Replace(number, String.Empty);

                r = new Regex(@"\s", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                string secondpass = r.Replace(firstpass, String.Empty);

                return secondpass;
            }
            if (conversiontype == conversiontype.ReconsituteParenDashFromNum)
            {
                Regex r = new Regex("(?:[^0-9 ])", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
                if (r.IsMatch(number))
                {
                    string secondpass = "(" + number.Substring(0, 3) + ") " + number.Substring(3, 3) + "-" + number.Substring(6, number.Length - 6);
                    return secondpass;
                }
            }

            return converted;
        }
        #endregion

        #region EmailValid
        public static bool EmailValid(string email)
        {
            bool status = false;
            Regex re = new Regex(@"^([a-zA-Z0-9_\-\.]*)([a-zA-Z0-9]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            if (re.IsMatch(email))
            {
                status = true;
            }
            return status;
        }
        #endregion

    }

    public enum conversiontype
    {
        Dashes2Parenthesis = 1,
        Parenrenthesis2Dashes = 2,
        NumberOnly2Parenthesis = 3,
        NumberOnly2Dashes = 4,
        RemovesEverythingButNumbers = 5,
        ReconsituteParenDashFromNum = 6
    }
}

