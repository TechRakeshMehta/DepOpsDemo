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
using System.Collections;
using System.Resources;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

#endregion

#region Application Specific

using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;

#endregion

#endregion

namespace INTSOF.Utils.UI
{

    /// <summary>
    /// Common utility class for all modules for binding combobox and getting resource messages.
    /// </summary>
    public static class SysXUtils
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private static ResourceManager _resourceManager;

        #endregion

        #endregion

        #region Properties

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// UAT-2302-As a student, I should see a popup on login if i have logged in using a non-preferred
        /// </summary>
        public static bool CheckValidBrowser()
        {
            //Get Browser Detail from BrowserDetail.cs belongs to INTSOF.Utils.UI
            string regexFilePath = HttpContext.Current.Server.MapPath(AppConsts.PREFERRED_BROWSER_REGEX_FILEPATH);
            var LoadRegexToParserMethod = Parser.GetDefault(regexFilePath);
            var BrowserDetails = LoadRegexToParserMethod.Parse(HttpContext.Current.Request.UserAgent);
            //use app constants and use enums
            bool preferredBrowser = true;
            string WebBrowserName = BrowserDetails.UserAgent.Family.Trim();
            string WebBrowserVersion = BrowserDetails.UserAgent.Major;

            if (WebBrowserName.ToLower().Contains(AppConsts.PREFERRED_BROWSER_NAME_IE) || WebBrowserName.ToLower().Contains(AppConsts.PREFERRED_BROWSER_NAME_INTERNET_EXPLORER))
            {
                if (Math.Round(Convert.ToDecimal(WebBrowserVersion)) <= Convert.ToInt16(PreferredBrowserVersions.InternetExplorer))
                    preferredBrowser = false;
            }
            else if (WebBrowserName.ToLower().Contains(AppConsts.PREFERRED_BROWSER_NAME_CHROME))
            {
                if (Math.Round(Convert.ToDecimal(WebBrowserVersion)) <= Convert.ToInt16(PreferredBrowserVersions.Chrome))
                    preferredBrowser = false;
            }
            else if (WebBrowserName.ToLower().Contains(AppConsts.PREFERRED_BROWSER_NAME_FIREFOX) || WebBrowserName.ToLower().Contains(AppConsts.PREFERRED_BROWSER_NAME_MOZILLA))
            {
                if (Math.Round(Convert.ToDecimal(WebBrowserVersion)) <= Convert.ToInt16(PreferredBrowserVersions.Mozilla))
                    preferredBrowser = false;
            }
            else if (WebBrowserName.ToLower().Contains(AppConsts.PREFERRED_BROWSER_NAME_SAFARI))
            {
                if (Math.Round(Convert.ToDecimal(WebBrowserVersion)) <= Convert.ToInt16(PreferredBrowserVersions.Safari))
                    preferredBrowser = false;
            }
            else
            {
                preferredBrowser = false;
            }
            return preferredBrowser;
        }

        /// <summary>
        /// Bind the ComboBox with Datasource
        /// </summary>
        /// <param name="sysComboBox">Name of the Combox Control</param>
        /// <param name="datasource">Datasource to be bind with ComboBox</param>
        /// <param name="textField">TextField to be dispalyed inside ComboBox</param>
        /// <param name="valueField">ValueField to be dispalyed inside ComboBox</param>
        public static void BindComboBox(WclComboBox sysComboBox, IList datasource, String textField, String valueField)
        {
            if (!sysComboBox.IsNull())
            {
                sysComboBox.DataSource = datasource;
                sysComboBox.DataTextField = textField;
                sysComboBox.DataValueField = valueField;
                sysComboBox.DataBind();
            }
        }

        /// <summary>
        /// Get message based on key.
        /// </summary>
        /// <param name="key">key name for getting value/message.</param>
        /// <returns>value for corresponding key.</returns>
        public static String GetMessage(String key)
        {
            String fileName = WebConfigurationManager.AppSettings["ResourceFileName"];
            _resourceManager = ResourceManager.CreateFileBasedResourceManager(fileName, HttpContext.Current.Server.MapPath("~"), null);
            return _resourceManager.GetString(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlCollection"></param>
        public static void DisableChildControls(ControlCollection controlCollection)
        {
            foreach (Control c in controlCollection)
            {
                if (c is WclComboBox)
                {
                    ((WclComboBox)c).Enabled = false;
                }
                else if (c is WclTextBox)
                {
                    ((WclTextBox)c).Enabled = false;
                }
                else if (c is WclNumericTextBox)
                {
                    ((WclNumericTextBox)c).Enabled = false;
                }
                else if (c is RadioButton)
                {
                    ((RadioButton)c).Enabled = false;
                }
                else if (c is CheckBox)
                {
                    ((CheckBox)c).Enabled = false;
                }
                //else if (c is WclCheckBox)
                //{
                //    ((WclCheckBox)c).Enabled = false;
                //}
                else if (c is WclDatePicker)
                {
                    ((WclDatePicker)c).Enabled = false;
                }
                else if (c is WclDateTimePicker)
                {
                    ((WclDateTimePicker)c).Enabled = false;
                }
                else if (c is WclGrid)
                {
                    ((WclGrid)c).Enabled = false;
                }
                //else if (c is WclGridTableView)
                //{
                //    ((WclGridTableView)c).Enabled = false;
                //}
            }
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}