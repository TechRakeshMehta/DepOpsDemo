#region Header Comment BaseUserControl

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  BaseUserControl.cs
// Purpose:   
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
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.SharedObjects;
using Entity.ClientEntity;
using NLog;
using Entity.Navigation;
using System.Threading;
#endregion

#endregion

namespace CoreWeb
{
    /// <summary>
    /// This class handles all the operations related to base user control.
    /// </summary>
    /// <remarks></remarks>
    public class BaseUserControl : System.Web.UI.UserControl
    {
        #region Variables

        #region Public Variables

        /// <summary>
        /// Generic Delegate to call methods related to updating document list.
        /// </summary>
        public delegate void UpdateDocumentList(List<ApplicantDocuments> list);

        public UpdateDocumentList del;

        public delegate void ShowDeleteCheckBox(Boolean isDeleteApplicable, Int32 complianceItemId);

        public ShowDeleteCheckBox showChkBox;

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
        /// Logger instance to log the Data Entry 
        /// </summary>
        private static Logger _dataEntrylogger;

        /// <summary>
        /// Logger instance to log the Order flow steps
        /// </summary>
        private static Logger _orderFlowlogger;

        private static List<String> UCEncodingDisableList = (ConfigurationManager.AppSettings["UCDisableHTMLEncoding"].IsNull() ? "" : ConfigurationManager.AppSettings["UCDisableHTMLEncoding"]).Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

        private static List<String> UCCommonControlsList = (ConfigurationManager.AppSettings["UCCommonControls"].IsNull() ? "" : ConfigurationManager.AppSettings["UCCommonControls"]).Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        /// <remarks></remarks>
        public String Title
        {
            get;
            set;
        }
        public String BreadCrumbTitleKey { get; set; }


        public List<aspnet_Roles> CurrentUserRolesList
        {
            get
            {
                return SecurityManager.GetUserRolesById(SysXWebSiteUtils.SessionService.UserId);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [reset current context].
        /// </summary>
        /// <value><c>true</c> if [reset current context]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public Boolean ResetCurrentContext
        {
            get
            {
                return _resetCurrentContext;
            }
            set
            {
                _resetCurrentContext = value;
            }
        }

        /// <summary>
        /// Gets or sets the class module.
        /// </summary>
        /// <value>The class module.</value>
        /// <remarks></remarks>
        public String ClassModule
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is policy enable.
        /// </summary>
        /// <value><c>true</c> if this instance is policy enable; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public Boolean IsPolicyEnable
        {
            get
            {
                return _isPolicyEnabled;
            }
            set
            {
                _isPolicyEnabled = value;
            }
        }

        /// <summary>
        /// Current User Id.
        /// </summary>
        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is admin.
        /// </summary>
        /// <remarks></remarks>
        public Boolean IsSysXAdmin
        {
            get
            {
                return SysXWebSiteUtils.SessionService.IsSysXAdmin;
            }
        }

        /// <summary>
        /// Gets value of sysX membership user.
        /// </summary>
        /// <remarks></remarks>
        public SysXMembershipUser SysXMembershipUser
        {
            get
            {
                return (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            }
        }

        /// <summary>
        /// Gets or sets the name of the control.
        /// </summary>
        /// <value>The name of the control.</value>
        /// <remarks></remarks>
        public String ControlName
        {
            get
            {
                return _controlName;
            }
            set
            {
                _controlName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [remove Unused Session].
        /// </summary>
        /// <value><c>true</c> if [remove Unused Session]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public Boolean RemoveUnusedSession
        {
            get
            {
                return _removeUnusedSession;
            }
            set
            {
                _removeUnusedSession = value;
            }
        }


        /// <summary>
        /// Get the Save Form Data.
        /// </summary>
        public SaveForm SaveFormInformation
        {
            get
            {
                return (HttpContext.Current.Items["SaveForm"] as SaveForm);
            }
            set
            {
                if (HttpContext.Current.Items["SaveForm"].IsNull())
                {
                    _saveForm = new SaveForm(SaveFormData);
                    HttpContext.Current.Items["SaveForm"] = _saveForm;
                }
                else
                {
                    _saveForm = (SaveForm)HttpContext.Current.Items["SaveForm"];
                    _saveForm += value;
                    HttpContext.Current.Items["SaveForm"] = _saveForm;
                }
            }
        }


        /// <summary>
        /// Get the Save Form Data.
        /// </summary>
        //public GetAssetAttributesData RetrieveAssetAttribute
        //{
        //    get
        //    {
        //        return (HttpContext.Current.Items["AssetAttributes"] as GetAssetAttributesData);
        //    }
        //    set
        //    {
        //        if (HttpContext.Current.Items["AssetAttributes"].IsNull())
        //        {
        //            _assetAttribute = new GetAssetAttributesData(GetAttributeInformation);
        //            HttpContext.Current.Items["AssetAttributes"] = _assetAttribute;
        //        }
        //        else
        //        {
        //            _assetAttribute = (GetAssetAttributesData)HttpContext.Current.Items["AssetAttributes"];
        //            _assetAttribute += value;
        //            HttpContext.Current.Items["AssetAttributes"] = _assetAttribute;
        //        }
        //    }
        //}


        public Boolean CreateTabOnDemand
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets or sets the Help Enabled.
        /// </summary>
        /// <value>The title.</value>
        /// <remarks></remarks>
        private bool IsHelpEnabled
        {
            get
            {
                return ConfigurationManager.AppSettings["IsHelpEnabled"].IsNull() ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["IsHelpEnabled"]);
            }
        }

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #region Public Methods



        /// <summary>
        /// Virtual method used to save form data in the Database.
        /// </summary>
        /// <param name="parameter"></param>
        public virtual void SaveFormData(Object parameter)
        { }


        /// <summary>
        /// Gets the Attribute Information from Database.
        /// </summary>
        /// <param name="assetID">ID of an Asset.</param>
        /// <param name="assetUnitID">ID of an Unit of Asset.</param>
        /// <returns></returns>
        //public virtual ORMAsset GetAttributeInformation()
        //{
        //    return null;
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUserControl"/> class.
        /// </summary>
        /// <remarks></remarks>
        public BaseUserControl()
        {
            _resetCurrentContext = true;
            _removeUnusedSession = true;
        }

        /// <summary>
        /// Refreshes the menu.
        /// </summary>
        /// <remarks></remarks>
        public void RefreshMenu()
        {
            //var sysXDefaultMasterView = this.Page.Master as IDefaultMasterView;
            //if (sysXDefaultMasterView != null)
            //{

            //}
        }

        /// <summary>
        /// Formats the name of the control.
        /// </summary>
        /// <param name="controlFileName">Name of the control file.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String FormatControlName(String controlFileName)
        {
            controlFileName = controlFileName.Replace(".ascx", String.Empty);
            controlFileName = System.Text.RegularExpressions.Regex.Replace(controlFileName, "(\\B[A-Z])", " $1");
            return controlFileName.Trim();
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public String GetString(String key)
        {
            return String.Empty;
        }

        /// <summary>
        /// Shows the error message.
        /// </summary>
        /// <param name="strMessage">The STR message.</param>
        /// <remarks></remarks>
        public virtual void ShowErrorMessage(String strMessage)
        {
            var sysXDefaultMasterView = this.Page.Master as IDefaultMasterView;
            if (sysXDefaultMasterView != null)
            {
                sysXDefaultMasterView.ShowErrorMessage(strMessage);
            }
        }

        public virtual void ShowSearchErrorMessage(String strMessage)
        {
            var sysXDefaultMasterView = this.Page.Master as IDefaultMasterView;
            if (sysXDefaultMasterView != null)
            {
                sysXDefaultMasterView.ShowSearchErrorMessage(strMessage);
            }
        }

        /// <summary>
        /// Display message and set css class as per the message type
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="messageType"></param>
        public void ShowMessageOnPage(String errorMessage, MessageType messageType)
        {
            var sysXDefaultMasterView = this.Page.Master as IDefaultMasterView;
            if (sysXDefaultMasterView != null)
            {
                sysXDefaultMasterView.ShowMessageOnPage(errorMessage, messageType);
            }
        }

        /// <summary>
        /// Shows the info message.
        /// </summary>
        /// <param name="strMessage">The STR message.</param>
        /// <remarks></remarks>
        public virtual void ShowInfoMessage(String strMessage)
        {
            var sysXDefaultMasterView = this.Page.Master as IDefaultMasterView;
            if (sysXDefaultMasterView != null)
            {
                sysXDefaultMasterView.ShowInfoMessage(strMessage);
            }
        }

        /// <summary>
        /// Shows the info message.
        /// </summary>
        /// <param name="strMessage">The STR message.</param>
        /// <remarks></remarks>
        public virtual void ShowSuccessMessage(String strMessage)
        {
            var sysXDefaultMasterView = this.Page.Master as IDefaultMasterView;
            if (sysXDefaultMasterView != null)
            {
                sysXDefaultMasterView.ShowSuccessMessage(strMessage);
            }
        }

        public virtual void ShowAlertMessage(String strMessage, MessageType msgType)
        {
            String msgClass = "info";
            switch (msgType)
            {
                case MessageType.Error:
                    msgClass = "error";
                    break;
                case MessageType.Information:
                    msgClass = "info";
                    break;
                case MessageType.SuccessMessage:
                    msgClass = "sucs";
                    break;
                default:
                    msgClass = "info";
                    break;
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlertMessage"
                                                 , "$page.showAlertMessage('" + strMessage.ToString() + "','" + msgClass + "',true);", true);
        }

        /// <summary>
        /// Hides the error message.
        /// </summary>
        /// <remarks></remarks>
        public virtual void HideErrorMessage()
        {
            //((Label)((PlaceHolder)this.Parent).Page.Master.FindControl("lblError")).Text = "Error : " + errorMessage;    
            //Saurav Roy -23-08-2011 -hide error message 

            var sysXDefaultMasterView = this.Page.Master as IDefaultMasterView;
            if (!sysXDefaultMasterView.IsNull())
            {
                sysXDefaultMasterView.HideErrorMessage();
            }
        }

        /// <summary>
        /// Shows error message.
        /// </summary>
        /// <param name="errorMessage"></param>
        public virtual void ShowErrorInfoMessage(String errorMessage)
        {
            var sysXDefaultMasterView = this.Page.Master as IDefaultMasterView;
            if (sysXDefaultMasterView != null)
            {
                sysXDefaultMasterView.ShowErrorInfoMessage(errorMessage);
            }
        }

        /// <summary>
        /// Register postback controls inside an UpdatePanel control as triggers. 
        /// Controls that are registered by using this method update a whole page instead of updating only the UpdatePanel control's content
        /// </summary>
        /// <param name="registerControl"></param>
        public void RegisterControlForPostBack(Control registerControl)
        {
            var sysXDefaultMasterView = this.Page.Master as ISysXDefaultMasterView;
            if (sysXDefaultMasterView != null)
            {
                sysXDefaultMasterView.RegisterControlForPostBack(registerControl);
            }
        }

        public void SetPageTitle(string title)
        {
            if (this.Page != null)
            {
                BasePage page = this.Page as BasePage;
                if (page.IsNotNull())
                {
                    page.SetPageTitle(title);
                }

            }
        }

        public void HideTitleBars()
        {
            if (this.Page != null)
            {
                BasePage page = this.Page as BasePage;
                page.HideTitleBars();
            }
        }

        /// <summary>
        /// Handle insert or edit at a time.
        /// </summary>
        /// <param name="sysXInsertOrEdit"></param>
        /// <param name="e"></param>
        public void InsertOrEdit(WclGrid sysXInsertOrEdit, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.EditCommandName)
            {
                sysXInsertOrEdit.MasterTableView.IsItemInserted = false;
            }

            if (e.CommandName == RadGrid.InitInsertCommandName)
            {
                sysXInsertOrEdit.MasterTableView.ClearChildEditItems();
            }

            if (e.CommandName == RadGrid.DeleteCommandName)
            {
                sysXInsertOrEdit.MasterTableView.ClearChildEditItems();
            }
        }

        /// <summary>
        /// Provide features for export data of grid.
        /// </summary>
        public void ConfigureExport(WclGrid sysXConfigureExport)
        {
            try
            {
                sysXConfigureExport.ExportSettings.ExportOnlyData = true;
                sysXConfigureExport.ExportSettings.IgnorePaging = true;
                sysXConfigureExport.ExportSettings.OpenInNewWindow = true;
                sysXConfigureExport.MasterTableView.IsItemInserted = false;
                sysXConfigureExport.MasterTableView.ClearChildEditItems();
                sysXConfigureExport.MasterTableView.AllowFilteringByColumn = false; // Added to Remove Blank row in case of Export
            }
            catch (SysXException ex)
            {

            }
        }

        /// <summary>
        /// enable feature of grid.
        /// </summary>
        public void ConfigureNonExporting(WclGrid sysXConfigureExport)
        {
            try
            {
                sysXConfigureExport.ExportSettings.ExportOnlyData = false;
                sysXConfigureExport.ExportSettings.IgnorePaging = false;
                sysXConfigureExport.MasterTableView.AllowPaging = true;
                sysXConfigureExport.ExportSettings.OpenInNewWindow = false;
                sysXConfigureExport.MasterTableView.AllowFilteringByColumn = true; // Added to Remove Blank row in case of Export
            }
            catch (SysXException ex)
            {

            }
        }

        /// <summary>
        /// Method for hiding columns. 
        /// </summary>
        /// <param name="grdInsurance"></param>
        /// <param name="columnUniqieName"></param>
        public void HideGridColumn(WclGrid grdHideGridColumn, String[] columnUniqieName)
        {
            try
            {
                if (grdHideGridColumn.Items.Count > Convert.ToInt32(DefaultNumbers.None))
                {
                    foreach (String uniqueColumnName in columnUniqieName)
                    {
                        grdHideGridColumn.MasterTableView.GetColumn(uniqueColumnName).Visible = false;
                    }
                }
            }
            catch (SysXException ex)
            {

            }
        }

        /// <summary>
        /// Method for show columns. 
        /// </summary>
        /// <param name="grdInsurance"></param>
        /// <param name="columnUniqieName"></param>
        public void ShowGridColumn(WclGrid grdHideGridColumn, String[] columnUniqieName)
        {
            try
            {
                //if (grdHideGridColumn.Items.Count > Convert.ToInt32(DefaultNumbers.None))
                //{
                foreach (String uniqueColumnName in columnUniqieName)
                {
                    grdHideGridColumn.MasterTableView.GetColumn(uniqueColumnName).Visible = true;
                }
                // }
            }
            catch (SysXException ex)
            {

            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Applies the permission.
        /// </summary>
        /// <param name="controls">The controls.</param>
        /// <remarks></remarks>
        private void ApplyPermission(ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                if (!String.IsNullOrEmpty(ctrl.ID))
                {
                    Type type = ctrl.GetType();
                    PropertyInfo prpInfo = type.GetProperty("ReadOnly");

                    if (!prpInfo.IsNull())
                    {
                        prpInfo.SetValue(ctrl, true, null);
                    }
                    else
                    {
                        prpInfo = type.GetProperty("Enable");

                        if (!prpInfo.IsNull())
                        {
                            prpInfo.SetValue(ctrl, true, null);
                        }

                        prpInfo = type.GetProperty("AllowAutomaticInserts");

                        if (!prpInfo.IsNull())
                        {
                            prpInfo.SetValue(ctrl, false, null);
                        }

                        prpInfo = type.GetProperty("AllowAutomaticDeletes");

                        if (!prpInfo.IsNull())
                        {
                            prpInfo.SetValue(ctrl, false, null);
                        }

                        prpInfo = type.GetProperty("AllowAutomaticUpdates");

                        if (!prpInfo.IsNull())
                        {
                            prpInfo.SetValue(ctrl, false, null);
                        }
                    }
                }

                if (ctrl.HasControls())
                {
                    ApplyPermission(ctrl.Controls);
                }

                ApplyReadonly(ctrl);
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <remarks></remarks>

        private void Init()
        {
            List<int> LstHTMLOverrideDtls;
            if (String.IsNullOrEmpty(_controlName))
            {
                var baseType = GetType().BaseType;
                if (!baseType.IsNull())
                {
                    _controlName = baseType.Name + ".ascx";
                }
            }

            if (!UCCommonControlsList.Any(X => X == _controlName) && UCEncodingDisableList.Count > 0)
                Extensions.HTMLEncodeOverride = UCEncodingDisableList.Contains(_controlName) ? true : false;

            LstHTMLOverrideDtls = new List<int>()
            {
                Thread.CurrentThread.ManagedThreadId,
                Convert.ToInt32(Extensions.HTMLEncodeOverride)
            };
            Context.Items["CntxHTMLOverride"] = LstHTMLOverrideDtls;
            this.Unload += OnUnload;

            //Policy Implementation comment for the time being...
            String fullPath;
            String parentControlName = String.Empty;

            //No usercontrol name should contains '_'. else this code would not work. So during codereview make sure with usercontrol name - TG
            if (!ControlName.Contains('_'))
            {
                EncryptedQueryString args = null;
                String controlName;
                String rootFolder = String.Empty;

                // TG : Code Changes for Bug#41534
                if (!NamingContainer.BindingContainer.IsNull())
                {
                    rootFolder = (NamingContainer.BindingContainer).AppRelativeTemplateSourceDirectory;
                    rootFolder = rootFolder.Replace("~", String.Empty);
                    rootFolder = rootFolder.Replace("/", String.Empty);
                }

                if (!Request.QueryString["args"].IsNull())
                {
                    args = new EncryptedQueryString(Request.QueryString["args"]);
                }

                if (!args.IsNull() && args.ContainsKey("ParentControlName"))
                {
                    parentControlName = args["ParentControlName"];
                }

                if (!args.IsNull() && args.ContainsKey("Child"))
                {
                    controlName = args["Child"];
                }
                else
                {
                    controlName = ControlName;
                }

                fullPath = rootFolder + "\\" + controlName;
            }
            else
            {
                fullPath = ControlName.Replace("_", "\\");
            }

            if (this.Page.Master is BaseMasterPage)
            {

                var menu = ((BaseMasterPage)this.Page.Master).GetMenuRow();


                if (!menu.IsNull())
                {
                    SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.SESSION_MENU_ID, menu.ID);
                    //It applies the Read Only Permission to the selected feature.
                    if (menu.PermissionTypeId == Entity.Navigation.PermissionTypeEnum.ReadOnly)
                    {
                        ApplyPermission(Controls);
                    }
                    else if (menu.PermissionTypeId == Entity.Navigation.PermissionTypeEnum.NoAccess)
                    {
                        Response.Redirect("~/AccessDenied.aspx", false);
                    }
                }
            }

        }

        /// <summary>
        /// Set help content for controls.
        /// </summary>
        /// <param name="_page"></param>
        public void SetHelpContent()
        {
            Boolean isPublishable = false;
            Int32 pkHelpMainId = AppConsts.NONE;
            String userControlName = "";
            String controlName = "";

            var baseType = this.GetType().BaseType;
            if (baseType != null)
            {
                controlName = baseType.Name + ".ascx";
            }

            if (!controlName.IsNullOrEmpty())
            {
                if (controlName.Contains("_"))
                {
                    userControlName = (controlName.Substring((controlName.LastIndexOf("_")) + AppConsts.ONE));
                }
                else if (controlName.Contains("\\"))
                {
                    userControlName = (controlName.Substring((controlName.LastIndexOf("\\")) + AppConsts.ONE));
                }
                else
                {
                    userControlName = controlName;
                }

                DataTable dtHelpMain = LINQToDataTable(LookupManager.GetLookUpData<HelpMain>());

                DataTable dtHelpContent = LINQToDataTable(LookupManager.GetLookUpData<HelpContent>());

                if (!dtHelpMain.IsNullOrEmpty())
                {
                    DataRow[] drHelpMain = dtHelpMain.Select("ParentControlName = '" + userControlName + "'");

                    if (!drHelpMain.IsNullOrEmpty())
                    {
                        foreach (DataRow helpDataRow in drHelpMain)
                        {
                            isPublishable = Convert.ToBoolean(helpDataRow[(SysXHelpConsts.CSHHELP_ISPUBLISHABLE)]);
                            pkHelpMainId = Convert.ToInt32(helpDataRow[SysXHelpConsts.CSHHELP_HELPMAINID]);
                            break;
                        }

                        if (isPublishable)
                        {
                            if (!dtHelpContent.IsNullOrEmpty())
                            {
                                DataRow[] drHelpContent = dtHelpContent.Select("HelpMainId = '" + pkHelpMainId + "'");
                                if (!drHelpContent.IsNullOrEmpty())
                                    FindControlRecursive(this, drHelpContent);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Convert list to datatable##Created during testing with Chera's DB.
        /// </summary>
        /// <param name="_page"></param>
        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others

                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            oProps = null;
            varlist = null;

            return (dtReturn.Columns.Count > AppConsts.NONE ? dtReturn : null);
        }

        /// <summary>
        /// Find control recursive
        /// </summary>
        /// <param name="policy">user control and helpcontent list.</param>     
        /// <remarks></remarks>        
        protected void FindControlRecursive(Control ucControl, DataRow[] helpContent)
        {
            if (ucControl != null && ucControl.Controls.Count > AppConsts.NONE)
            {
                foreach (Control control in ucControl.Controls)
                {
                    if (!control.GetType().ToString().Equals("System.Web.UI.LiteralControl") || !control.GetType().ToString().Equals("System.Web.UI.ResourceBasedLiteralControl")) // Filter by control name if required!
                    {
                        if (!control.GetType().Name.ToString().Equals("SysXTreeList") && !control.GetType().Name.ToString().Equals("WclGrid")) // Condition used to by pass SysXtreeList for MapBlockFeature page
                        {
                            if (control.Controls.Count > AppConsts.NONE && !control.GetType().Name.ToString().Equals("WclComboBox") && !control.GetType().Name.ToString().Equals("SysXDatePicker") && !control.GetType().Name.ToString().Equals("SysXListBox")) // Parent control or having more controls in it line page, panel, multiview etc
                            {
                                FindControlRecursive(control, helpContent); // Recursive call
                            }
                            else // is last control.
                            {
                                if (control.ID != null && typeNameList.Contains(control.GetType().Name))
                                {
                                    ProcessHelpContent(helpContent, control); //Set tooltip text with respect to specific control                             
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Assign tooltip to controls
        /// </summary>
        /// <param name="Helpcontent list">Helpcontent list & control</param>   
        /// <remarks></remarks>
        private void ProcessHelpContent(DataRow[] helpContents, Control control)
        {
            foreach (DataRow helpContentDataRow in helpContents)
            {
                if (!helpContentDataRow[SysXHelpConsts.CSHHELP_TOOLTIPTEXT].IsNullOrEmpty())
                    if (!DBNull.Value.Equals(helpContentDataRow[SysXHelpConsts.CSHHELP_ISTOOLTIPENABLED]))
                        if (helpContentDataRow[SysXHelpConsts.CSHHELP_CONTROLNAME].ToString().Equals(control.ID) && Convert.ToBoolean(helpContentDataRow[SysXHelpConsts.CSHHELP_ISTOOLTIPENABLED]))
                            (control as System.Web.UI.WebControls.WebControl).ToolTip = helpContentDataRow[SysXHelpConsts.CSHHELP_TOOLTIPTEXT].ToString();
            }
        }

        /// <summary>
        /// Applies the policies.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="controls">The controls.</param>
        /// <remarks></remarks>
        private void ApplyPolicies(PolicySetUserControl policy, ControlCollection controls)
        {
            List<Policy> policies = policy.Policies.ToList();

            foreach (Control ctrl in controls)
            {
                /*
                 *TODO:  Commenting below code because we are not considering visible = false case in policy implementation... - TG
                List<Policy> policies = new List<Policy>();
                foreach (Policy policyItem in policy.PolicySet.Policies)
                {
                    policies.Add(policyItem);
                }*/

                if (!String.IsNullOrEmpty(ctrl.ID))
                {
                    Type type = ctrl.GetType();
                    String controlId = ctrl.ID;
                    Policy localPolicy = policies.Find(policyInfo => policyInfo.ControlID == controlId);

                    while (!localPolicy.IsNull())
                    {
                        foreach (PolicyProperty prop in localPolicy.PolicyProperties)
                        {
                            String propertyname = PolicyProperties.PolicyPropertyCollection[prop.PolicyPropertyName];
                            PropertyInfo prpInfo = type.GetProperty(propertyname);

                            if (!prpInfo.IsNull() || (ctrl is RadTreeList))
                            {
                                if (propertyname.ToLower().Equals("readonly"))
                                {
                                    if (prop.PolicyValue)
                                    {
                                        prpInfo.SetValue(ctrl, prop.PolicyValue, null);
                                    }
                                }
                                else
                                {
                                    if (prop.PolicyValue)
                                    {
                                        //If control is not radtreelist
                                        if (!(ctrl is RadTreeList))
                                        {
                                            prpInfo.SetValue(ctrl, false, null);
                                        }

                                        if (ctrl is RadGrid || ctrl is RadTreeList)
                                        {
                                            switch (propertyname.ToLower())
                                            {
                                                case "allowautomaticinserts":
                                                    DenyInsert(ctrl);
                                                    break;
                                                case "allowautomaticdeletes":
                                                    DenyDelete(ctrl);
                                                    break;
                                                case "allowautomaticupdates":
                                                    DenyUpdate(ctrl);
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        policies.Remove(localPolicy);
                        localPolicy = policies.Find(policyInfo => policyInfo.ControlID == controlId);
                    }
                }

                if (ctrl.HasControls())
                {
                    ApplyPolicies(policy, ctrl.Controls);
                }
            }
        }

        /// <summary>
        /// Applies the readonly.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <remarks></remarks>
        private void ApplyReadonly(Control control)
        {
            RadGrid radgrid;
            RadComboBox combobox;
            //WclCheckBox wclCheckBox;
            CheckBox checkBox;
            RadTreeList treeList;

            //Apply Readonly for grid
            if (control is GridBaseDataList)
            {
                radgrid = (RadGrid)control;
                radgrid.EnableViewState = false;
                radgrid.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;

                foreach (GridColumn column in radgrid.MasterTableView.Columns.Cast<GridColumn>().Where(column => column is GridButtonColumn || column is GridEditCommandColumn))
                {
                    column.Display = false;
                }

                foreach (Control ctrl in from GridDataItem item in radgrid.MasterTableView.Items from Control ctrl in item.Controls select ctrl)
                {
                    combobox = ctrl as RadComboBox;

                    if (!combobox.IsNull())
                    {
                        combobox.Enabled = false;
                    }

                    //wclCheckBox = ctrl as WclCheckBox;

                    //if (!wclCheckBox.IsNull())
                    //{
                    //    wclCheckBox.Enabled = false;
                    //}

                    checkBox = ctrl as CheckBox;

                    if (!checkBox.IsNull())
                    {
                        checkBox.Enabled = false;
                    }
                }

                //radgrid.Rebind();
            }

            //Apply readonly for Combo Box
            if (control is RadCodeBlock)
            {
                combobox = (RadComboBox)control;
                combobox.Enabled = false;
            }

            // Apply ReadOnly for TreeList
            if (control is RadTreeList)
            {
                treeList = (RadTreeList)control;
                foreach (TreeListColumn column in treeList.Columns.Where(column => column is TreeListEditCommandColumn || column is TreeListButtonColumn))
                {
                    column.Visible = false;
                }
            }

            if (control is WclButton)
            {
                (control as WclButton).Enabled = false;
            }

            if (control is WclSplitter)
            {
                (control as WclSplitter).Enabled = false;
            }
        }

        /// <summary>
        /// Denies the insert.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <remarks></remarks>
        private void DenyInsert(Control control)
        {
            if (control is GridBaseDataList)
            {
                ((RadGrid)control).MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
            }

            if (control is RadTreeList)
            {
                RadTreeList treeList = (RadTreeList)control;

                foreach (TreeListEditCommandColumn column in treeList.Columns.OfType<TreeListEditCommandColumn>())
                {
                    (column).ShowAddButton = false;
                }
            }
        }

        /// <summary>
        /// Denies the update.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <remarks></remarks>
        private void DenyUpdate(Control control)
        {
            if (control is GridBaseDataList)
            {
                foreach (GridEditCommandColumn column in ((RadGrid)control).MasterTableView.Columns.OfType<GridEditCommandColumn>())
                {
                    column.Display = false;
                }
            }

            if (control is RadTreeList)
            {
                RadTreeList treeList = (RadTreeList)control;

                foreach (TreeListEditCommandColumn column in treeList.Columns.OfType<TreeListEditCommandColumn>())
                {
                    (column).ShowEditButton = false;
                }
            }
        }

        /// <summary>
        /// Denies the delete.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <remarks></remarks>
        private void DenyDelete(Control control)
        {
            if (control is GridBaseDataList)
            {
                foreach (GridButtonColumn column in ((RadGrid)control).MasterTableView.Columns.OfType<GridButtonColumn>())
                {
                    column.Display = false;
                }
            }

            if (control is RadTreeList)
            {
                RadTreeList treeList = (RadTreeList)control;

                foreach (TreeListButtonColumn column in treeList.Columns.OfType<TreeListButtonColumn>())
                {
                    column.Visible = false;
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="E:Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        protected override void OnInit(EventArgs e)
        {
            //DisableParentNotification = true;

            if (_resetCurrentContext)
            {
                if (HttpContext.Current.Items.Contains("CurrentContext"))
                {
                    HttpContext.Current.Items["CurrentContext"] = String.Empty;
                }
            }

            if (_removeUnusedSession)
            {
                GlobalSessionRemove();
            }
            base.OnInit(e);
            this.HideErrorMessage();

            //String key = "FeaturePermission";
            //Permission permission = null;
            //if (HttpContext.Current.Items[key].IsNotNull())
            //{
            //    permission = (Permission)(HttpContext.Current.Items[key]);
            //}

            //if (!permission.IsNull() && permission.Name.ToUpper().Equals("READONLY"))
            //{
            //    _isPolicyEnabled = false;
            //    ApplyPermission(this.Controls);
            //}

            if (_isPolicyEnabled)
            {
                Init();
            }

            //Call to set help content method
            if (IsHelpEnabled == true)
            {
                SetHelpContent();
            }
        }

        /// <summary>
        /// OnUnload To validate HTMLEncodeOverride field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnload(object sender, EventArgs e)
        {
            List<int> LstHTMLOverrideDtls;
            object[] LogDetails;
            LstHTMLOverrideDtls = Context.Items["CntxHTMLOverride"] as List<int>;

            if (LstHTMLOverrideDtls.IsNotNull() && LstHTMLOverrideDtls.First() != Thread.CurrentThread.ManagedThreadId)
            {
                LogDetails = new object[]
                {
                   LstHTMLOverrideDtls.First(),
                   Convert.ToBoolean(LstHTMLOverrideDtls.Last()),
                   Thread.CurrentThread.ManagedThreadId,
                   Extensions.HTMLEncodeOverride,
                   GetType().BaseType.Name
                };
                LogError(new ArgumentException(string.Format("Warning : Thread id mismatch, the initial init values of ThreadId and HTMLEncodeOverride are {0} and {1} and the final unload values are {2} and {3} in {4}.ascx respectively", LogDetails)));
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
        /// Used to Log the debug  Message
        /// </summary>
        /// <param name="message"></param>
        protected void LogDebug(string message)
        {
            _exceptionService.HandleDebug(message);
        }
        protected void LogInfo(string message)
        {
            var logger = CoreWeb.Shell.SysXWebSiteUtils.LoggerService.GetLogger();
            logger.Info(message);
        }
        /// <summary>
        /// Used to Show Error Message UI
        /// </summary>
        /// <param name="sysEx">SysXException</param>
        /// <remarks></remarks>
        protected virtual void ShowErrorMessage(SysXException sysEx)
        {
            String errorDescription = sysEx.Message + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_ERROR_TYPE) + GetMessageFromResourceFile(sysEx);
            var placeHolder = this.Parent as PlaceHolder;

            if (placeHolder.IsNull())
            {
                return;
            }
            var sysXDefaultMasterView = placeHolder.Page.Master as IDefaultMasterView;

            if (sysXDefaultMasterView != null)
            {
                sysXDefaultMasterView.ShowErrorMessage(errorDescription);
            }
        }

        /// <summary>
        /// Used to Show the Error Message on UI
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <remarks></remarks>
        protected virtual void ShowErrorMessage(System.Exception ex)
        {
            String errorDescription = ex.Message + SysXUtils.GetMessage(ResourceConst.SPACE) + SysXUtils.GetMessage(ResourceConst.SECURITY_ERROR_TYPE) + GetMessageFromResourceFile(ex);
            var placeHolder = this.Parent as PlaceHolder;

            if (placeHolder.IsNull())
            {
                return;
            }

            var sysXDefaultMasterView = placeHolder.Page.Master as IDefaultMasterView; //ISysXDefaultMasterView;

            if (!sysXDefaultMasterView.IsNull())
            {
                sysXDefaultMasterView.ShowErrorMessage(errorDescription);
            }
        }

        /// <summary>
        /// Get the Name of Error Type from Resource file
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>String</returns>
        /// <remarks></remarks>
        protected String GetMessageFromResourceFile(System.Exception ex)
        {
            _resourceManager = ResourceManager.CreateFileBasedResourceManager("SysXMessages", Server.MapPath("~"), null);

            if (ex is SysXException)
            {
                return _resourceManager.GetString(Convert.ToString((int)Status.CustomException));
            }
            else if (ex is System.Exception)
            {
                return _resourceManager.GetString(Convert.ToString((int)Status.SystemException));
            }

            return String.Empty;
        }

        /// <summary>
        /// Log the Data Entry related information
        /// </summary>
        /// <param name="logMessage"></param>
        protected void LogDataEntry(String logMessage)
        {
            if (_dataEntrylogger == null)
            {
                _dataEntrylogger = LogManager.GetLogger("DataEntryLogger");
            }
            _dataEntrylogger.Info(logMessage);
        }

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
        /// Removes the global session.
        /// </summary>
        /// <remarks></remarks>
        private void GlobalSessionRemove()
        {
            if (!HttpContext.Current.IsNull() && !HttpContext.Current.Session["NotesOwnerContextID"].IsNull())
            {
                HttpContext.Current.Session.Remove("NotesOwnerContextID");
            }
        }

        #endregion


        #endregion

        private List<FeatureRoleAction> actionPermissionCollection
        {
            get;
            set;
        }


        public List<FeatureRoleAction> ActionPermission
        {
            get;
            set;
        }

        public virtual List<String> ChildScreenPathCollection
        {
            get { return null; }
        }


        public virtual List<ClsFeatureAction> ActionCollection
        {
            get { return null; }

        }


        //if (HttpContext.Current.IsNull())
        //    { 
        //        SysXAppDBEntities objContext = new SysXAppDBEntities();
        //        objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
        //        return objContext;
        //    }

        //    else
        //    {
        //        if (!HttpContext.Current.Items.Contains(APP_CONTEXT_KEY))
        //        {
        //            SysXAppDBEntities objContext = new SysXAppDBEntities();
        //            objContext.CommandTimeout = AppConsts.FIVE_HUNDRED.To<Int32>();
        //            HttpContext.Current.Items.Add(APP_CONTEXT_KEY, objContext);
        //        }

        //        return HttpContext.Current.Items[APP_CONTEXT_KEY] as SysXAppDBEntities;
        //    }



        protected virtual void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, String screenName = "")
        {

            if (!SysXWebSiteUtils.SessionService.IsNull())
            {
                if (!HttpContext.Current.IsNull() && !HttpContext.Current.Items.Contains(AppConsts.ACTION_PERMISSION))
                {


                    Int32 menuID = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_MENU_ID));
                    actionPermissionCollection = SecurityManager.GetRoleActionFeaturesTemp(menuID, CurrentUserId, SysXWebSiteUtils.SessionService.SysXBlockId).ToList();
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
                //    : actionPermissionCollection.Where(cond => cond.FeatureAction.ChildScreenName == screenName).ToList();
                ActionPermission = actionPermissionCollection.Where(cond => cond.FeatureAction.ChildScreenName == screenName).ToList();
                if (ActionPermission.IsNotNull() && ctrlCollection.IsNotNull())
                {

                    ActionPermission.Where(cond => !cond.FeatureAction.ControlActionId.IsNull()).ForEach(action =>
                        {
                            //String actionName = action.FeatureAction.Action;

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


        /// <summary>
        /// Applies the permission page wise (need to call in the end of page load of you page to set permissions.)
        /// </summary>
        public void ApplyPermissionsPageWise()
        {
            //MenuViewItem menu = ((Entity.Navigation.MenuViewItem)(HttpContext.Current.Session["CurrentMenuItem"]));
            MenuViewItem menu = (Entity.Navigation.MenuViewItem)SysXWebSiteUtils.SessionService.GetCustomData("CurrentMenuItem");
            if (!menu.IsNull())
            {
                //It applies the Read Only Permission to the selected feature.
                if (menu.PermissionTypeId == Entity.Navigation.PermissionTypeEnum.ReadOnly && ((menu.UIControlID == ScreenName.ManageTickets.GetStringValue())))
                {
                    List<Control> ctrlList = new List<Control>();
                    GetControlList<List<Control>>(this.Page.Controls, ctrlList);
                    ApplyPermissionsPageWiseOnControls(ctrlList);
                }
                else if (menu.PermissionTypeId == Entity.Navigation.PermissionTypeEnum.NoAccess)
                {
                    Response.Redirect("~/AccessDenied.aspx", false);
                }
            }
        }

        /// <summary>
        /// Get All the controls including nested controls on the page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controlCollection"></param>
        /// <param name="resultCollection"></param>
        private void GetControlList<T>(ControlCollection controlCollection, List<Control> resultCollection) where T : List<Control>
        {
            foreach (Control control in controlCollection)
            {
                if (control.HasControls())
                {
                    GetControlList<List<Control>>(control.Controls, resultCollection);
                }
                else
                {
                    resultCollection.Add(control);
                }
            }
        }

        /// <summary>
        /// Applies the permission.
        /// </summary>
        /// <param name="controls">The controls.</param>
        /// <remarks></remarks>
        private void ApplyPermissionsPageWiseOnControls(List<Control> controls)
        {
            foreach (Control ctrl in controls)
            {
                if (!String.IsNullOrEmpty(ctrl.ID))
                {
                    Type type = ctrl.GetType();
                    PropertyInfo prpInfo = type.GetProperty("ReadOnly");

                    if (!prpInfo.IsNull())
                    {
                        prpInfo.SetValue(ctrl, true, null);
                    }
                    else
                    {
                        prpInfo = type.GetProperty("Enable");

                        if (!prpInfo.IsNull())
                        {
                            prpInfo.SetValue(ctrl, true, null);
                        }

                        prpInfo = type.GetProperty("AllowAutomaticInserts");

                        if (!prpInfo.IsNull())
                        {
                            prpInfo.SetValue(ctrl, false, null);
                        }

                        prpInfo = type.GetProperty("AllowAutomaticDeletes");

                        if (!prpInfo.IsNull())
                        {
                            prpInfo.SetValue(ctrl, false, null);
                        }

                        prpInfo = type.GetProperty("AllowAutomaticUpdates");

                        if (!prpInfo.IsNull())
                        {
                            prpInfo.SetValue(ctrl, false, null);
                        }
                    }
                }

                //if (ctrl.HasControls())
                //{
                //    ApplyPermission(ctrl);
                //}

                ApplyReadonly(ctrl);
            }
        }
    }
}