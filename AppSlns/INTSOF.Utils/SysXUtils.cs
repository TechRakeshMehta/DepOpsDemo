#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SysXUtils.cs
// Purpose:   Common Utility class for all modules.
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web;
using System.Web.Configuration;
using System.Resources;
using System.Text;

#endregion

#region Application Specific

#endregion

#endregion

namespace INTSOF.Utils
{
    /// <summary>
    /// Common utility class for all modules for binding combo box and getting resource messages.
    /// </summary>
    public static class SysXUtils
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static ResourceManager _resourceManager;

        private static Object _lockObject = new Object();

        #endregion

        #endregion

        #region Properties

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods
        /// <summary>        
        /// Writes log informtaion to flat file
        /// </summary>
        /// <param name="logMessage">string that is written to the log file</param>
        public static void WriteToLogFlatFile(string logMessage, string switchOnOff = "1")
        {
            System.IO.StreamWriter logWriter = null;
            string switchOnOffLog = switchOnOff;
            try
            {
                if (switchOnOffLog != "1")
                    return;

                switchOnOffLog = WebConfigurationManager.AppSettings["logSwitch"];
                if (switchOnOffLog != "1")
                    return;

                string fileName = "MenuDisappearingLog_" + DateTime.Today.Month.ToString() + DateTime.Today.Day.ToString() + DateTime.Today.Year.ToString() + ".txt";
                string logFile = (HttpContext.Current.Server.MapPath("~") + @"\Log\" + fileName);

                lock (_lockObject)
                {
                    if (!System.IO.File.Exists(logFile))
                    {
                        logWriter = new System.IO.StreamWriter(logFile);
                    }
                    else
                    {
                        logWriter = System.IO.File.AppendText(logFile);
                    }

                    logWriter.WriteLine(logMessage);
                    logWriter.WriteLine();
                    logWriter.Flush();
                }
            }
            catch
            {
            }
            finally
            {
                if (!logWriter.IsNull())
                    logWriter.Close();
            }
        }

        /// <summary>
        /// Get message based on key.
        /// </summary>
        /// <param name="key">key name for getting value/message.</param>
        /// <returns>value for corresponding key.</returns>
        /// <remarks></remarks>
        public static String GetMessage(String key)
        {
            _resourceManager = ResourceManager.CreateFileBasedResourceManager(WebConfigurationManager.AppSettings["ResourceFileName"], HttpContext.Current.Server.MapPath("~"), null);
            return _resourceManager.GetString(key);
        }


        public static string GetCode()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(6, true));
            return builder.ToString();
        }

        public static string GetCodeForSupplier()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(10, true));
            return builder.ToString();
        }

        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        public static String MessageSize(Int32 Bytes)
        {
            String filesize = "0 B";
            Decimal size = 0;
            if (Bytes >= 1048576)
            {
                size = Decimal.Divide(Bytes, 1048576);
                filesize = String.Format("{0:##}", size) + " MB";
            }
            if (Bytes >= 1024)
            {
                size = Decimal.Divide(Bytes, 1024);
                filesize = String.Format("{0:####}", size) + " KB";
            }
            if (Bytes > 0 & Bytes < 1024)
            {
                size = Decimal.Divide(Bytes, 1);
                filesize = String.Format("{0:####}", size) + " B";
            }

            return filesize;
        }
        public static String GetXmlEncodedString(String data)
        {
            // return System.Xml.XmlConvert.EncodeName(data);
            return data;
        }
        public static String GetXmlDecodedString(String data)
        {
            return System.Xml.XmlConvert.DecodeName(data);
        }

        public static DateTime GetMountainTime(DateTime dtTime)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dtTime, "Mountain Standard Time");
        }

        public static string GenerateRandomNo(Int32 size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            Int32 ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}