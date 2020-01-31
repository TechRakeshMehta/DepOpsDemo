//TODO: Not yet finalized code is to be removed or the current file will be removed.
//#region Header Comment Block
//// 
//// Copyright 2011 Intersoft Data Labs.
//// All rights are reserved.  Reproduction or transmission in whole or in part, in
//// any form or by any means, electronic, mechanical or otherwise, is prohibited
//// without the prior written consent of the copyright owner.
//// 
//// Filename:  SysXMembershipUtil.cs
//// Purpose:   
////

//#endregion

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Collections;
//using System.Globalization;
//using System.Collections.Specialized;
//using System.Configuration.Provider;
//using System.Web.Hosting;
//using System.Diagnostics;
//using System.Security.Cryptography;

//namespace CoreWeb.SysXSecurityModel
//{
//    internal static class SysXMembershipUtil
//    {
//        #region Methods

//        #region Internal Static

//        internal static void CheckArrayParameter(ref string[] param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
//        {
//            if (param == null)
//            {
//                throw new ArgumentNullException(paramName);
//            }
//            if (param.Length < 1)
//            {
//                throw new ArgumentException(String.Format("The array parameter '{0}' should not be empty.", paramName));
//            }
//            Hashtable hashtable = new Hashtable(param.Length);
//            for (int i = param.Length - 1; i >= 0; i--)
//            {
//                CheckParameter(ref param[i], checkForNull, checkIfEmpty, checkForCommas, maxSize, paramName + "[ " + i.ToString(CultureInfo.InvariantCulture) + " ]");
//                if (hashtable.Contains(param[i]))
//                {
//                    throw new ArgumentException(String.Format("The array '{0}' should not contain duplicate values.", paramName));
//                }
//                hashtable.Add(param[i], param[i]);
//            }
//        }

//        internal static void CheckParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
//        {
//            if (param == null)
//            {
//                if (checkForNull)
//                {
//                    throw new ArgumentNullException(paramName);
//                }
//            }
//            else
//            {
//                param = param.Trim();
//                if (checkIfEmpty && (param.Length < 1))
//                {
//                    throw new ArgumentException(String.Format("The parameter '{0}' must not be empty.", paramName));
//                }
//                if ((maxSize > 0) && (param.Length > maxSize))
//                {
//                    throw new ArgumentException(String.Format("The parameter '{0}' is too long: it must not exceed {1} chars in length.", paramName, maxSize.ToString(CultureInfo.InvariantCulture)));
//                }
//                if (checkForCommas && param.Contains(","))
//                {
//                    throw new ArgumentException(String.Format("The parameter '{0}' must not contain commas.", paramName));
//                }
//            }
//        }

//        internal static void CheckPasswordParameter(ref string param, int maxSize, string paramName)
//        {
//            if (param == null)
//            {
//                throw new ArgumentNullException(paramName);
//            }
//            if (param.Length < 1)
//            {
//                throw new ArgumentException(String.Format("The parameter '{0}' must not be empty.", paramName));
//            }
//            if ((maxSize > 0) && (param.Length > maxSize))
//            {
//                throw new ArgumentException(String.Format("The parameter '{0}' is too long: it must not exceed {1} chars in length.", paramName, maxSize.ToString(CultureInfo.InvariantCulture)));
//            }
//        }

//        internal static bool GetBooleanValue(NameValueCollection config, string valueName, bool defaultValue)
//        {
//            string sValue = config[valueName];
//            if (sValue == null)
//            {
//                return defaultValue;
//            }

//            bool result;
//            if (bool.TryParse(sValue, out result))
//            {
//                return result;
//            }
//            else
//            {
//                throw new ProviderException(string.Format("The value must be boolean (true or false) for property '{0}'.", valueName));
//            }
//        }

//        internal static string GetDefaultAppName()
//        {
//            try
//            {
//                string applicationVirtualPath = HostingEnvironment.ApplicationVirtualPath;
//                if (string.IsNullOrEmpty(applicationVirtualPath))
//                {
//                    applicationVirtualPath = Process.GetCurrentProcess().MainModule.ModuleName;
//                    int index = applicationVirtualPath.IndexOf('.');
//                    if (index != -1)
//                    {
//                        applicationVirtualPath = applicationVirtualPath.Remove(index);
//                    }
//                }
//                if (string.IsNullOrEmpty(applicationVirtualPath))
//                {
//                    return "/";
//                }
//                return applicationVirtualPath;
//            }
//            catch
//            {
//                return "/";
//            }
//        }

//        internal static int GetIntValue(NameValueCollection config, string valueName, int defaultValue, bool zeroAllowed, int maxValueAllowed)
//        {
//            string sValue = config[valueName];

//            if (sValue == null)
//            {
//                return defaultValue;
//            }

//            int iValue;
//            if (!Int32.TryParse(sValue, out iValue))
//            {
//                if (zeroAllowed)
//                {
//                    throw new ProviderException(string.Format("The value must be a non-negative 32-bit integer for property '{0}'", valueName));
//                }

//                throw new ProviderException(string.Format("The value must be a positive 32-bit integer for property '{0}'.", valueName));
//            }

//            if (zeroAllowed && iValue < 0)
//            {
//                throw new ProviderException(string.Format("The value must be a non-negative 32-bit integer for property '{0}'", valueName));
//            }

//            if (!zeroAllowed && iValue <= 0)
//            {
//                throw new ProviderException(string.Format("The value must be a positive 32-bit integer for property '{0}'.", valueName));
//            }

//            if (maxValueAllowed > 0 && iValue > maxValueAllowed)
//            {
//                throw new ProviderException(string.Format("The value '{0}' can not be greater than '{1}'.", valueName, maxValueAllowed.ToString(CultureInfo.InvariantCulture)));
//            }

//            return iValue;
//        }

//        internal static bool ValidateParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize)
//        {
//            if (param == null)
//            {
//                return !checkForNull;
//            }

//            param = param.Trim();
//            if ((checkIfEmpty && param.Length < 1) ||
//                 (maxSize > 0 && param.Length > maxSize) ||
//                 (checkForCommas && param.Contains(",")))
//            {
//                return false;
//            }

//            return true;
//        }

//        internal static bool ValidatePasswordParameter(ref string param, int maxSize)
//        {
//            if (param == null)
//            {
//                return false;
//            }
//            if (param.Length < 1)
//            {
//                return false;
//            }
//            if ((maxSize > 0) && (param.Length > maxSize))
//            {
//                return false;
//            }
//            return true;
//        }

//        /// <summary>
//        /// Generate Salt
//        /// </summary>
//        /// <returns>Generated Salt</returns>
//        internal static string GenerateSalt()
//        {
//            byte[] buf = new byte[16];
//            (new RNGCryptoServiceProvider()).GetBytes(buf);
//            return Convert.ToBase64String(buf);
//        }

//        /// <summary>
//        /// Hash password with provided salt using Membership.HashAlgorithmType
//        /// </summary>
//        /// <param name="password">Password</param>
//        /// <param name="salt">Salt</param>
//        /// <returns>Hashed Password</returns>
//        internal static string HashPasswordIWithSalt(string password, string salt)
//        {
//            byte[] bytes = Encoding.Unicode.GetBytes(password);
//            byte[] src = Convert.FromBase64String(salt);
//            byte[] dst = new byte[src.Length + bytes.Length];
//            byte[] inArray = null;
//            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
//            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

//            HashAlgorithm algorithm = HashAlgorithm.Create(System.Web.Security.Membership.HashAlgorithmType);
//            inArray = algorithm.ComputeHash(dst);

//            return Convert.ToBase64String(inArray);
//        }

//        internal static DateTime RoundToSeconds(DateTime dt)
//        {
//            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
//        }

//        #endregion

//        #endregion
        
//    }
//}