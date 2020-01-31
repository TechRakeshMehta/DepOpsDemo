#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageConfigurationPresenter.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using INTSOF.SharedObjects;

#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using Business.RepoManagers;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class has the method's implementation which performs all the CRUD(Create/ Read/ Update/ Delete) operation for managing configuration with its details.
    /// </summary>
    public class ManageConfigurationPresenter : Presenter<IManageConfigurationView>
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Performs an insertion for a new database configuration.
        /// </summary>
        public void SaveDBConfiguration()
        {
            SysXConfig sysXConfig = new SysXConfig();

            if (SecurityManager.IsSysXKeyExists(View.ViewContract.Key))
            {
                View.ErrorMessage = View.ViewContract.Key + SysXUtils.GetMessage(ResourceConst.SPACE) + "SysXKey already exists.";
            }
            else
            {
                sysXConfig.Value = View.ViewContract.Value;
                sysXConfig.SysXKey = View.ViewContract.Key;
                SecurityManager.SaveDBConfiguration(sysXConfig);
            }
        }

        /// <summary>
        /// Perform updates for database configurations.
        /// </summary>
        public void UpdateDbConfiguration()
        {
            SysXConfig sysXConfig = SecurityManager.GetSysXConfig(View.ViewContract.Key);

            View.ErrorMessage = String.Empty;
            sysXConfig.SysXKey = View.ViewContract.Key;
            sysXConfig.Value = View.ViewContract.Value;

            SecurityManager.UpdateSysXConfig(sysXConfig);
        }

        /// <summary>
        /// Perform updates for application configurations.
        /// </summary>
        public void UpdateAppConfiguration()
        {
            try
            {
                var configDocument = new XmlDocument();
                configDocument.Load(View.WebConfigurationFullName);
                XmlNode appSettings = configDocument.GetElementsByTagName("appSettings")[0];

                foreach (XmlNode node in (from XmlNode node in appSettings.ChildNodes where node.NodeType == XmlNodeType.Element where node.Attributes != null && (node.Name.Equals("add") && node.Attributes["key"].Value.Equals(View.ViewContract.Key)) select node).Where(node => node.Attributes != null).Where(node => node.Attributes != null))
                {
                    if (node.Attributes != null)
                    {
                        node.Attributes["value"].Value = View.ViewContract.Value;
                    }
                }
                configDocument.Save(View.WebConfigurationFullName);
            }
            catch (System.Exception ex)
            {
                throw new SysXException(SysXUtils.GetMessage(ResourceConst.SECURITY_APPLICATION_SETTING) + SysXUtils.GetMessage(ResourceConst.SPACE) + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all database configuration settings.
        /// </summary>
        public void RetrievingDbConfigurationSettings()
        {
            View.DbConfigurations = SecurityManager.GetSysXConfigs();
        }

        /// <summary>
        ///  Retrieves all application configuration settings.
        /// </summary>
        public void RetrievingAppConfigurationSettings()
        {
            var configDocument = new XmlDocument();
            configDocument.Load(View.WebConfigurationFullName);
            XmlNode appSettings = configDocument.GetElementsByTagName("appSettings")[0];

            Dictionary<String, String> appConfigs = (appSettings.ChildNodes.Cast<XmlNode>().Where(
                node => node.NodeType == XmlNodeType.Element).Where(node => node.Name.Equals("add"))).ToDictionary(node => node.Attributes != null ? node.Attributes["key"].Value : null, node => node.Attributes != null ? node.Attributes["value"].Value : null);
            View.AppConfigurations = appConfigs;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}