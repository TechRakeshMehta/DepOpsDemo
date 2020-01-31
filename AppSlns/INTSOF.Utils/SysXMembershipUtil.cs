#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXMembershipUtil.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web.Hosting;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils
{
    /// <summary>
    /// This class handles membership utilities.
    /// </summary>
    /// <remarks></remarks>
    public static class SysXMembershipUtil
    {
        #region Variables

        #endregion

        #region Properties

        #endregion

        #region Events

        #endregion

        #region Methods

        #region public Static

        /// <summary>
        /// Checks the array parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="checkForNull">if set to <c>true</c> [check for null].</param>
        /// <param name="checkIfEmpty">if set to <c>true</c> [check if empty].</param>
        /// <param name="checkForCommas">if set to <c>true</c> [check for commas].</param>
        /// <param name="maxSize">Size of the max.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <remarks></remarks>
        public static void CheckArrayParameter(ref String[] param, Boolean checkForNull, Boolean checkIfEmpty, Boolean checkForCommas, Int32 maxSize, String paramName)
        {
            if (param.IsNull())
            {
                throw new ArgumentNullException(paramName);
            }
            if (param.Length < AppConsts.ONE)
            {
                throw new ArgumentException(String.Format("The array parameter '{0}' should not be empty.", paramName));
            }
            Hashtable hashtable = new Hashtable(param.Length);
            for (Int32 count = param.Length - AppConsts.ONE; count >= AppConsts.NONE; count--)
            {
                CheckParameter(ref param[count], checkForNull, checkIfEmpty, checkForCommas, maxSize, paramName + "[ " + count.ToString(CultureInfo.InvariantCulture) + " ]");
                
                if (hashtable.Contains(param[count]))
                {
                    throw new ArgumentException(String.Format("The array '{0}' should not contain duplicate values.", paramName));
                }

                hashtable.Add(param[count], param[count]);
            }
        }

        /// <summary>
        /// Checks the parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="checkForNull">if set to <c>true</c> [check for null].</param>
        /// <param name="checkIfEmpty">if set to <c>true</c> [check if empty].</param>
        /// <param name="checkForCommas">if set to <c>true</c> [check for commas].</param>
        /// <param name="maxSize">Size of the max.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <remarks></remarks>
        public static void CheckParameter(ref String param, Boolean checkForNull, Boolean checkIfEmpty, Boolean checkForCommas, Int32 maxSize, String paramName)
        {
            if (param.IsNull())
            {
                if (checkForNull)
                {
                    throw new ArgumentNullException(paramName);
                }
            }
            else
            {
                param = param.Trim();

                if (checkIfEmpty && (param.Length < AppConsts.ONE))
                {
                    throw new ArgumentException(String.Format("The parameter '{0}' must not be empty.", paramName));
                }

                if ((maxSize > AppConsts.NONE) && (param.Length > maxSize))
                {
                    throw new ArgumentException(String.Format("The parameter '{0}' is too long: it must not exceed {1} chars in length.", paramName, maxSize.ToString(CultureInfo.InvariantCulture)));
                }

                if (checkForCommas && param.Contains(","))
                {
                    throw new ArgumentException(String.Format("The parameter '{0}' must not contain commas.", paramName));
                }
            }
        }

        /// <summary>
        /// Checks the password parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="maxSize">Size of the max.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <remarks></remarks>
        public static void CheckPasswordParameter(ref String param, Int32 maxSize, String paramName)
        {
            if (param.IsNull())
            {
                throw new ArgumentNullException(paramName);
            }
            if (param.Length < AppConsts.ONE)
            {
                throw new ArgumentException(String.Format("The parameter '{0}' must not be empty.", paramName));
            }
            if ((maxSize > AppConsts.NONE) && (param.Length > maxSize))
            {
                throw new ArgumentException(String.Format("The parameter '{0}' is too long: it must not exceed {1} chars in length.", paramName, maxSize.ToString(CultureInfo.InvariantCulture)));
            }
        }

        /// <summary>
        /// Gets the boolean value.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool GetBooleanValue(NameValueCollection config, String valueName, Boolean defaultValue)
        {
            String sValue = config[valueName];

            if (!sValue.IsNull())
            {
                return defaultValue;
            }

            bool result;

            if (bool.TryParse(sValue, out result))
            {
                return result;
            }
            else
            {
                throw new ProviderException(String.Format("The value must be boolean (true or false) for property '{0}'.", valueName));
            }
        }

        /// <summary>
        /// Gets the default name of the app.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String GetDefaultAppName()
        {
            try
            {
                String applicationVirtualPath = HostingEnvironment.ApplicationVirtualPath;
                
                if (String.IsNullOrEmpty(applicationVirtualPath))
                {
                    applicationVirtualPath = Process.GetCurrentProcess().MainModule.ModuleName;
                    Int32 index = applicationVirtualPath.IndexOf('.');
                    
                    if (!index.Equals(-AppConsts.ONE))
                    {
                        applicationVirtualPath = applicationVirtualPath.Remove(index);
                    }
                }

                return String.IsNullOrEmpty(applicationVirtualPath) ? "/" : applicationVirtualPath;
            }
            catch
            {
                return "/";
            }
        }

        /// <summary>
        /// Gets the int value.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="valueName">Name of the value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="zeroAllowed">if set to <c>true</c> [zero allowed].</param>
        /// <param name="maxValueAllowed">The max value allowed.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static Int32 GetIntValue(NameValueCollection config, String valueName, Int32 defaultValue, Boolean zeroAllowed, Int32 maxValueAllowed)
        {
            String sValue = config[valueName];

            if (sValue.IsNull())
            {
                return defaultValue;
            }

            Int32 iValue;

            if (!Int32.TryParse(sValue, out iValue))
            {
                if (zeroAllowed)
                {
                    throw new ProviderException(String.Format("The value must be a non-negative 32-bit integer for property '{0}'", valueName));
                }

                throw new ProviderException(String.Format("The value must be a positive 32-bit integer for property '{0}'.", valueName));
            }

            if (zeroAllowed && iValue < AppConsts.NONE)
            {
                throw new ProviderException(String.Format("The value must be a non-negative 32-bit integer for property '{0}'", valueName));
            }

            if (!zeroAllowed && iValue <= AppConsts.NONE)
            {
                throw new ProviderException(String.Format("The value must be a positive 32-bit integer for property '{0}'.", valueName));
            }

            if (maxValueAllowed > AppConsts.NONE && iValue > maxValueAllowed)
            {
                throw new ProviderException(String.Format("The value '{0}' can not be greater than '{1}'.", valueName, maxValueAllowed.ToString(CultureInfo.InvariantCulture)));
            }

            return iValue;
        }

        /// <summary>
        /// It Validate's Parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="checkForNull">if set to <c>true</c> [check for null].</param>
        /// <param name="checkIfEmpty">if set to <c>true</c> [check if empty].</param>
        /// <param name="checkForCommas">if set to <c>true</c> [check for commas].</param>
        /// <param name="maxSize">Size of the max.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ValidateParameter(ref String param, Boolean checkForNull, Boolean checkIfEmpty, Boolean checkForCommas, Int32 maxSize)
        {
            if (param.IsNull())
            {
                return !checkForNull;
            }

            param = param.Trim();
            return (!checkIfEmpty || param.Length >= AppConsts.ONE) && (maxSize <= AppConsts.NONE || param.Length <= maxSize) && (!checkForCommas || !param.Contains(","));
        }

        /// <summary>
        /// Validates the password parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="maxSize">Size of the max.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool ValidatePasswordParameter(ref String param, int maxSize)
        {
            if (param.IsNull())
            {
                return false;
            }

            if (param.Length < AppConsts.ONE)
            {
                return false;
            }
            return (maxSize <= AppConsts.NONE) || (param.Length <= maxSize);
        }

        /// <summary>
        /// Generate Salt
        /// </summary>
        /// <returns>Generated Salt</returns>
        /// <remarks></remarks>
        public static String GenerateSalt()
        {
            Byte[] buf = new Byte[16];
            (new RNGCryptoServiceProvider()).GetBytes(buf);

            return Convert.ToBase64String(buf);
        }

        /// <summary>
        /// Hash password with provided salt using Membership.HashAlgorithmType
        /// </summary>
        /// <param name="pass">The pass.</param>
        /// <param name="salt">Salt</param>
        /// <returns>Hashed Password</returns>
        /// <remarks></remarks>
        public static String HashPasswordIWithSalt(String pass, String salt)
        {
            Byte[] bIn = Encoding.Unicode.GetBytes(pass);
            Byte[] bSalt = Convert.FromBase64String(salt);
            Byte[] bAll = new Byte[bSalt.Length + bIn.Length];
            Byte[] bRet = null;

            Buffer.BlockCopy(bSalt, AppConsts.NONE, bAll, AppConsts.NONE, bSalt.Length);
            Buffer.BlockCopy(bIn, AppConsts.NONE, bAll, bSalt.Length, bIn.Length);
            HashAlgorithm s = HashAlgorithm.Create("SHA1");
            bRet = s.ComputeHash(bAll);

            return Convert.ToBase64String(bRet);
        }

        /// <summary>
        /// Rounds to seconds.
        /// </summary>
        /// <param name="dt">The date time.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static DateTime RoundToSeconds(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

        #endregion

        #endregion
    }
}