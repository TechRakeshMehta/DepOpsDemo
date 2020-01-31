#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  BasePage.cs
// Purpose:   Base Page for default page  
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Resources;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data;
using System.ComponentModel;

#endregion

#region Application Specific

using CoreWeb.Shell.Views;
using INTSOF.Utils;
using Entity;
using INTSOF.Logger;
using Telerik.Web.UI;
using CoreWeb.Shell;
using Business.RepoManagers;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell.MasterPages;
using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using INTERSOFT.WEB.UI.WebControls;
using Entity.ClientEntity;
using NLog;


#endregion

#endregion

namespace CoreWeb
{

    /// <summary>
    /// Summary description for BaseWebPage
    /// </summary>
    public class BaseWebPage : System.Web.UI.Page
    {
        #region Variables

        #region Variables

        #region Public Variables


        /// <summary>
        /// Generic Delegate to call methods related to saving of for Data.
        /// </summary>
        public delegate void SaveForm(Object parameter);

        /// <summary>
        /// Retreive the Asset Attribute Information from the Database using Attribute based Model.
        /// </summary>
        /// <param name="assetID">ID of an Asset</param>
        /// <param name="assetUnitID">ID of an Unit of Asset</param>
        /// <returns>It returns an object of ORMAsset.</returns>
        //public delegate ORMAsset GetAssetAttributesData();

        #endregion

        #region Private Variables

        /// <summary>
        /// Handles Logger.
        /// </summary>
        private ILogger _logger = null;

        /// <summary>
        /// stores control name.
        /// </summary>
        private String _controlName;

        /// <summary>
        /// Checks if the policy is enabled or not.
        /// </summary>
        private Boolean _isPolicyEnabled = true;

        /// <summary>
        /// Handles session services.
        /// </summary>
        private ISysXSessionService _sessionService = SysXWebSiteUtils.SessionService;

        /// <summary>
        /// Handles exception services.
        /// </summary>
        private ISysXExceptionService _exceptionService = SysXWebSiteUtils.ExceptionService;

        /// <summary>
        /// Handles resource manager.
        /// </summary>
        private ResourceManager _resourceManager;

        /// <summary>
        /// Reset the current context.
        /// </summary>
        private Boolean _resetCurrentContext;

        /// <summary>
        /// Removes the unused sessions.
        /// </summary>
        private Boolean _removeUnusedSession;


        private SaveForm _saveForm;

        //private GetAssetAttributesData _assetAttribute;        

        private List<String> typeNameList = (ConfigurationManager.AppSettings["ToolTipForControls"].IsNull() ? "" : ConfigurationManager.AppSettings["ToolTipForControls"]).Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

        /// <summary>
        /// Logger instance to log the Order flow steps
        /// </summary>
        private static NLog.Logger _orderFlowlogger;

        #endregion

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public String Title
        {
            get;
            set;
        }

        public String BreadCrumbTitleKey { get; set; }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public BaseWebPage()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        /// <summary>
        /// Shows the info message.
        /// </summary>
        /// <param name="strMessage">The STR message.</param>
        /// <remarks></remarks>
        public virtual void ShowSuccessMessage(String strMessage)
        {
            var sysXIChildPageView = this.Master as IChildPageView;
            if (sysXIChildPageView != null)
            {
                sysXIChildPageView.ShowSuccessMessage(strMessage);
            }
        }

        /// <summary>
        /// Shows the info message.
        /// </summary>
        /// <param name="strMessage">The STR message.</param>
        /// <remarks></remarks>
        public virtual void ShowInfoMessage(String strMessage)
        {
            var sysXIChildPageView = this.Master as IChildPageView;
            if (sysXIChildPageView != null)
            {
                sysXIChildPageView.ShowInfoMessage(strMessage);
            }
        }

        /// <summary>
        /// Shows error message.
        /// </summary>
        /// <param name="errorMessage"></param>
        public virtual void ShowErrorMessage(String errorMessage)
        {
            var sysXIChildPageView = this.Master as IChildPageView;
            if (sysXIChildPageView != null)
            {
                sysXIChildPageView.ShowErrorMessage(errorMessage);
            }
        }

        /// <summary>
        /// Hides the error message.
        /// </summary>
        /// <remarks></remarks>
        public virtual void HideErrorMessage()
        {
            var sysXChildPageView = this.Master as IChildPageView;
            if (!sysXChildPageView.IsNull())
            {
                sysXChildPageView.HideErrorMessage();
            }
        }

        /// <summary>
        /// Shows error message.
        /// </summary>
        /// <param name="errorMessage"></param>
        public virtual void ShowErrorInfoMessage(String errorMessage)
        {
            var sysXIChildPageView = this.Master as IChildPageView;
            if (sysXIChildPageView != null)
            {
                sysXIChildPageView.ShowErrorInfoMessage(errorMessage);
            }
        }

        /// <summary>
        /// Used to Log the Error Message
        /// </summary>
        /// <param name="errorMessage">Error Message</param>
        /// <param name="ex">Exception</param>
        /// <remarks></remarks>
        protected void LogError(String errorMessage, System.Exception ex)
        {
            _exceptionService.HandleError(errorMessage, ex);
        }

        /// <summary>
        /// Used to Log the Error Message
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <remarks></remarks>
        protected void LogError(System.Exception ex)
        {
            _exceptionService.HandleError(String.Empty, ex);
        }

        /// <summary>
        /// Sets Page / Screen Title
        /// </summary>
        /// <param name="title"></param>
        public void SetPageTitle(String title)
        {
            if (this.Master.IsNull())
            {
                return;
            }

            CoreWeb.Shell.MasterPages.IDefaultMasterView master = this.Master as CoreWeb.Shell.MasterPages.IDefaultMasterView;
            master.SetPageTitle(title);
        }

        /// <summary>
        /// Sets Module title
        /// </summary>
        /// <param name="title"></param>
        public void SetModuleTitle(String title)
        {
            if (this.Master.IsNull())
            {
                return;
            }

            CoreWeb.Shell.MasterPages.IDefaultMasterView master = this.Master as CoreWeb.Shell.MasterPages.IDefaultMasterView;
            master.SetModuleTitle(title);
        }

        #endregion

        #region Private Methods


        #endregion

        #region Static Methods

        /// <summary>
        /// Log the Order flow related information in different steps
        /// </summary>
        /// <param name="logMessage"></param>
        protected static void LogOrderFlowSteps(String logMessage)
        {
            if (_orderFlowlogger == null)
            {
                _orderFlowlogger = LogManager.GetLogger(NLogLoggerTypes.ORDER_FLOW_LOGGER.GetStringValue());
            }
            _orderFlowlogger.Info(logMessage);
        }


        /// <summary>
        /// Log the Order flow related information in different steps
        /// </summary>
        /// <param name="logMessage"></param>
        protected static void LogOrderPDFViewer(String logMessage)
        {
            if (_orderFlowlogger == null)
            {
                _orderFlowlogger = LogManager.GetLogger(NLogLoggerPDFView.PDF_Viewer_LOGGER.GetStringValue());
            }
            _orderFlowlogger.Info(logMessage);
        }

        static public void CustomizeComplianceNode(Telerik.Web.UI.RadTreeNode node, Entity.ClientEntity.GetRuleSetTree item, bool expand = false)
        {

            if (node == null || item == null) return;

            //Exapand/Collapse node 
            node.Expanded = expand;

            //Set Node type and icon
            switch (item.UICode)
            {
                case RuleSetTreeNodeType.PackageLabel:
                    node.ImageUrl = "~/App_Themes/Default/images/icons/pkgs.gif";
                    node.Attributes["_nodeDataType"] = "PKGG";
                    break;
                case RuleSetTreeNodeType.Package:
                    node.ImageUrl = "~/App_Themes/Default/images/icons/pkg.gif";
                    node.ToolTip = node.Text + " (Package)";
                    node.Attributes["_nodeDataType"] = "PKG";
                    break;
                case RuleSetTreeNodeType.CategoryLabel:
                    node.ImageUrl = "~/App_Themes/Default/images/icons/cats.gif";
                    node.Attributes["_nodeDataType"] = "CATG";
                    break;
                case RuleSetTreeNodeType.Category:
                    node.ImageUrl = "~/App_Themes/Default/images/icons/cat.gif";
                    node.ToolTip = node.Text + " (Category)";
                    node.Attributes["_nodeDataType"] = "CAT";
                    break;
                case RuleSetTreeNodeType.ItemLabel:
                    node.ImageUrl = "~/App_Themes/Default/images/icons/itms.gif";
                    node.Attributes["_nodeDataType"] = "ITMG";
                    break;
                case RuleSetTreeNodeType.Item:
                    node.ImageUrl = "~/App_Themes/Default/images/icons/itm.gif";
                    node.ToolTip = node.Text + " (Item)";
                    node.Attributes["_nodeDataType"] = "ITM";
                    break;
                case RuleSetTreeNodeType.AttributeLabel:
                    node.ImageUrl = "~/App_Themes/Default/images/icons/attrs.gif";
                    node.Attributes["_nodeDataType"] = "ATRG";
                    break;
                case RuleSetTreeNodeType.Attribute:
                    node.ImageUrl = "~/App_Themes/Default/images/icons/attr.gif";
                    node.ToolTip = node.Text + " (Attribute)";
                    node.Attributes["_nodeDataType"] = "ATR";
                    break;
                case RuleSetTreeNodeType.RuleSetLabel:
                    node.ImageUrl = "~/App_Themes/Default/images/icons/rsets.gif";
                    node.Attributes["_nodeDataType"] = "RSETG";
                    break;
                case RuleSetTreeNodeType.RuleSet:
                    node.ImageUrl = "~/App_Themes/Default/images/icons/rset.gif";
                    node.ToolTip = node.Text + " (Ruleset)";
                    node.Attributes["_nodeDataType"] = "RSET";
                    break;
                case RuleSetTreeNodeType.RuleLabel:
                    node.ImageUrl = "~/App_Themes/Default/images/icons/ruls.gif";
                    node.Attributes["_nodeDataType"] = "RULG";
                    break;
                case RuleSetTreeNodeType.Rule:
                    node.ImageUrl = "~/App_Themes/Default/images/icons/rul.gif";
                    node.ToolTip = node.Text + " (Rule)";
                    node.Attributes["_nodeDataType"] = "RUL";
                    break;
                default:
                    break;
            }
        }
        #endregion

        #endregion

        #region Events



        #endregion

        public List<FeatureRoleAction> ActionPermission
        {
            get;
            set;
        }

        public virtual List<ClsFeatureAction> ActionCollection
        {
            get { return null; }

        }

        private List<FeatureRoleAction> actionPermissionCollection
        {
            get;
            set;
        }

        protected virtual void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, String screenName = "")
        {

            if (!SysXWebSiteUtils.SessionService.IsNull())
            {

                if (!HttpContext.Current.IsNull() && !HttpContext.Current.Items.Contains(AppConsts.ACTION_PERMISSION))
                {
                    Int32 menuID = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_MENU_ID));
                    actionPermissionCollection = SecurityManager.GetRoleActionFeaturesTemp(menuID, SysXWebSiteUtils.SessionService.OrganizationUserId, SysXWebSiteUtils.SessionService.SysXBlockId).ToList();
                    HttpContext.Current.Items.Add(AppConsts.ACTION_PERMISSION, actionPermissionCollection);
                }
                else
                {
                    actionPermissionCollection = (List<FeatureRoleAction>)HttpContext.Current.Items[AppConsts.ACTION_PERMISSION];
                }

            }

            if (!actionPermissionCollection.IsNull())
            {
                //ActionPermission = screenName == String.Empty ? actionPermissionCollection.Where(cond => cond.FeatureAction.ChildScreenName == null).ToList()
                //   : actionPermissionCollection.Where(cond => cond.FeatureAction.ChildScreenName == screenName).ToList();
                ActionPermission = actionPermissionCollection.Where(cond => cond.FeatureAction.ChildScreenName == screenName).ToList();
                if (ActionPermission.IsNotNull() && ctrlCollection.IsNotNull())
                {
                    ActionPermission.Where(cond => !cond.FeatureAction.ControlActionId.IsNull()).ForEach(action =>
                    {
                        //Find out record from ctrlCollection based on ControlActionId
                        ClsFeatureAction objClsFeatureAction = ctrlCollection.Where(cond => cond.ControlActionId == action.FeatureAction.ControlActionId).FirstOrDefault();
                        if (!objClsFeatureAction.IsNull())
                        {
                            //It applies the Read Only Permission to the selected attribute.
                            if (action.PermissionID.Value == AppConsts.THREE)
                            {
                                Control cntrl = objClsFeatureAction.SystemControl;
                                if (cntrl.IsNotNull())
                                {
                                    cntrl.GetType().GetProperty("Enabled").SetValue(cntrl, false);
                                }
                            }
                            else if (action.PermissionID.Value == AppConsts.FOUR)
                            {
                                Control cntrl = objClsFeatureAction.SystemControl;
                                if (cntrl.IsNotNull())
                                {
                                    cntrl.GetType().GetProperty("Visible").SetValue(cntrl, false);
                                }
                            }
                        }

                    });
                }
            }
        }


        protected override void InitializeCulture()
        {
            Boolean isLanguageTransaltionEnable = ConfigurationManager.AppSettings["IsLanguageTranslation"].IsNullOrEmpty() ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["IsLanguageTranslation"]);
            Boolean IsLocationTenant = false;
            if (!Session["IsLocationTenant"].IsNullOrEmpty())
                //IsLocationTenant = Convert.ToBoolean(Session["IsLocationTenant"]);
                IsLocationTenant = Convert.ToBoolean(SysXWebSiteUtils.SessionService.GetCustomData("IsLocationTenant"));

            if (isLanguageTransaltionEnable && IsLocationTenant)
            {
                LanguageTranslateUtils.LanguageTranslateInit();
                base.InitializeCulture();
            }
        }

        protected virtual String GetLanguageCulture()
        {
            return LanguageTranslateUtils.GetCurrentLanguageCultureFromSession();
        }
    }
}