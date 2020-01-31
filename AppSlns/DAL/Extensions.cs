#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  Extensions.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.Security;

#endregion

#region Application Specific

using INTSOF.Utils;
using System.Diagnostics;

#endregion

#endregion

namespace DAL
{
    /// <summary>
    /// This class handles the operations related to Extension method.
    /// </summary>
    public static class Extensions
    {
        #region Variables

        #endregion

        #region Properties

        #endregion

        #region Events

        #endregion

        #region Methods

        /// <summary>
        /// Finds a control nested within another control or possibly further down in the hierarchy.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Control FindControlRecursive(this Control root, String id)
        {
            if (!root.IsNull())
            {
                Control controlFound = root.FindControl(id);
               
                if (!controlFound.IsNull())
                {
                    return controlFound;
                }
                
                foreach (Control control in root.Controls)
                {
                    controlFound = control.FindControlRecursive(id);
                    
                    if (!controlFound.IsNull())
                    {
                        return controlFound;
                    }
                }
            } return null;
        }

        /// <summary>
        /// Get decimal value from String.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Decimal GetMoney(this String str)
        {
            decimal value;
            decimal.TryParse(str, out value);
            return value;
        }

        /// <summary>
        /// Get integer value from String.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Int32? GetIntValue(this String str)
        {
            Int32? outValue = null;
            Int32 value;

            if (Int32.TryParse(str, out value))
            {
                outValue = value;
            }

            return outValue;
        }

        /// <summary>
        /// Get integer value from nullable integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Int32 GetInt(this Int32? value)
        {
            Int32 outvalue = AppConsts.NONE;

            if (value.HasValue)
            {
                outvalue = value.Value;
            }

            return outvalue;
        }

        /// <summary>
        /// Get formated phone number from String.
        /// </summary>
        /// <param name="num">The num.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String GetPhoneFormat(this String num)
        {
            if (String.IsNullOrEmpty(num))
            {
                return String.Empty;
            }

            num = num.Replace("(", "").Replace(")", "").Replace("-", "");
            String results = String.Empty;
            String formatPattern = @"(\d{3})(\d{3})(\d{4})";
            results = Regex.Replace(num, formatPattern, "($1) $2-$3");
            return results;
        }

        /// <summary>
        /// Get message based on membership created status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String GetMessage(this MembershipCreateStatus status)
        {
            String message = String.Empty;

            switch (status)
            {
                case MembershipCreateStatus.DuplicateEmail:
                    message = "Duplicate Email";
                    break;
                case MembershipCreateStatus.DuplicateProviderUserKey:
                    message = "Duplicate User Id";
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                    message = "User name already exists.";
                    break;
                case MembershipCreateStatus.InvalidAnswer:
                    message = "Answer does not match the database value.";
                    break;
                case MembershipCreateStatus.InvalidEmail:
                    message = "Email address is not valid.";
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    message = "Password criteria does not match, must be between 8 and 20 characters in length and should have at least one uppercase letter and one number/special character.";
                    break;
                case MembershipCreateStatus.InvalidProviderUserKey:
                    break;
                case MembershipCreateStatus.InvalidQuestion:
                    message = "Question is invalid";
                    break;
                case MembershipCreateStatus.InvalidUserName:
                    message = "User name criteria does not match. Please try again.";
                    break;
                case MembershipCreateStatus.ProviderError:
                    message = "Provider unavailable.";
                    break;
                case MembershipCreateStatus.Success:
                    break;
                case MembershipCreateStatus.UserRejected:
                    message = "User name was rejected.";
                    break;
                default:
                    break;

            }
            return message;
        }

        /// <summary>
        /// Get currency String based on money.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <param name="largValue">if set to <c>true</c> [larg value].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String GetCurrencyString(this decimal? money, Boolean largValue)
        {
            String value = String.Empty;
            if (money.HasValue)
            {
                value = money.Value.ToString(largValue ? "C0" : "C2");
            }

            return value;
        }

        /// <summary>
        /// Get short date String.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String GetDateString(this DateTime? dt)
        {
            String value = String.Empty;

            if (dt.HasValue)
            {
                value = dt.Value.ToShortDateString();
            }

            return value;
        }

        /// <summary>
        /// Get date String in format MM/dd/yyyy hh:mm:ss tt ET
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="showTime">if set to <c>true</c> [show time].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String GetDateString(this DateTime? dt, Boolean showTime)
        {
            String value = String.Empty;

            if (!dt.HasValue)
            {
                return value;
            }

            value = showTime ? dt.Value.ToString("MM/dd/yyyy hh:mm:ss tt ET") : dt.Value.ToString("MM/dd/yyyy");
            return value;
        }

        /// <summary>
        /// Return integer value into String format.
        /// </summary>
        /// <param name="inputData">The i.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String GetString(this Int32? inputData)
        {
            String value = String.Empty;
            
            if (inputData.HasValue)
            {
                value = inputData.ToString();
            }

            return value;
        }

        /// <summary>
        /// Return String boolean value into boolean type e.g "True" into true.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool GetBool(this String str)
        {
            Boolean value = false;
            Boolean.TryParse(str, out value);
            return value;
        }

        /// <summary>
        /// Return decimal into String.
        /// </summary>
        /// <param name="money">The money.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String GetDecimalString(this Decimal? money)
        {
            String value = String.Empty;

            if (money.HasValue)
            {
                value = money.Value.ToString("#.00");
            }

            return value;
        }

        /// <summary>
        /// Clones the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T CloneObject<T>(this T source)
        {
            var dcs = new System.Runtime.Serialization
                .DataContractSerializer(typeof(T));

            using (var ms = new System.IO.MemoryStream())
            {
                dcs.WriteObject(ms, source);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                return (T)dcs.ReadObject(ms);
            }
        }

        /// <summary>
        /// Return datetime into 12 hours format.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String GetTimeString(DateTime? dt)
        {
            String strReturn = "12:00:00 AM";

            if (!dt.IsNull())
            {
                strReturn = Convert.ToString((((DateTime)dt).Hour > 12 ? (((DateTime)dt).Hour - 12) : ((((DateTime)dt).Hour == 0 ? 12 : ((DateTime)dt).Hour)))
                    + ":" + (((DateTime)dt).Minute < 10 ? "0" + ((DateTime)dt).Minute.ToString() : ((DateTime)dt).Minute.ToString())
                    + ":" + (((DateTime)dt).Second < 10 ? "0" + ((DateTime)dt).Second.ToString() : ((DateTime)dt).Second.ToString())
                                                                                             + " " + (((DateTime)dt).Hour >= 12 ? "PM" : "AM"));
            }

            return strReturn;
        }

        /// <summary>
        /// Return datetime into 12 hours format with/without show seconds.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="showSeconds">if set to <c>true</c> [show seconds].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String GetTimeString(DateTime? dt, Boolean showSeconds)
        {
            String strReturn = String.Empty; // "12:00:00 AM";

            if (!dt.IsNull())
            {
                if (showSeconds)
                {
                    strReturn = "12:00:00 AM";

                    strReturn = Convert.ToString((((DateTime)dt).Hour > 12 ? (((DateTime)dt).Hour - 12) : ((((DateTime)dt).Hour == 0 ? 12 : ((DateTime)dt).Hour)))
                    + ":" + (((DateTime)dt).Minute < 10 ? "0" + ((DateTime)dt).Minute.ToString() : ((DateTime)dt).Minute.ToString())
                    + ":" + (((DateTime)dt).Second < 10 ? "0" + ((DateTime)dt).Second.ToString() : ((DateTime)dt).Second.ToString())
                                                                                             + " " + (((DateTime)dt).Hour >= 12 ? "PM" : "AM"));
                }
                else
                {
                    strReturn = "12:00 AM";

                    strReturn = Convert.ToString((((DateTime)dt).Hour > 12 ? (((DateTime)dt).Hour - 12) : ((((DateTime)dt).Hour == 0 ? 12 : ((DateTime)dt).Hour)))
                        + ":" + (((DateTime)dt).Minute < 10 ? "0" + ((DateTime)dt).Minute.ToString() : ((DateTime)dt).Minute.ToString()) + " " + (((DateTime)dt).Hour >= 12 ? "PM" : "AM"));
                }
            }

            return strReturn;
        }

               /// <summary>
        /// Checks if an object is null
        /// </summary>
        /// <param name="o">Object to check</param>
        /// <returns>True if null, False is not null.</returns>
        /// <remarks></remarks>
        [DebuggerStepThrough]
        public static Boolean IsNull(this object o)
        {
            return ReferenceEquals(o, null);
        }

        #endregion
    }
}
