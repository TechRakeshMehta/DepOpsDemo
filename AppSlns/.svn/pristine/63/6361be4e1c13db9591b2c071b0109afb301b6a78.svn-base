#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MapUserRole.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using CoreWeb.Shell;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.IntsofSecurityModel.Providers;
using System.Threading;


#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to mapping between user with roles of security module.
    /// </summary>
    public partial class MapUserRole : BaseUserControl, IMapUserRoleView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private MapUserRolePresenter _presenter=new MapUserRolePresenter();
        private String _viewType;
        private MapUserRoleContract _viewContract;
        private Boolean IsNoUserGroup = true;
        private Boolean IsUserOfUserGroup = false;
        #endregion

        #endregion

        #region Properties

        #region ClientOnBoardingWizard

        /// <summary>
        /// Occurs when [save mapping click].
        /// </summary>
        public event EventHandler<EventArgs> SaveMappingClick;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is data load.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is data load; otherwise, <c>false</c>.
        /// </value>
        Boolean IMapUserRoleView.IsDataLoad
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is client on boarding wizard.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is client on boarding wizard; otherwise, <c>false</c>.
        /// </value>
        Boolean IMapUserRoleView.IsClientOnBoardingWizard
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is page load by client on boarding wizard.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is page load by client on boarding wizard; otherwise, <c>false</c>.
        /// </value>
        Boolean IsPageLoadByClientOnBoardingWizard
        {
            get
            {
                return Convert.ToBoolean(!SysXWebSiteUtils.SessionService.GetCustomData("IsPageMapUserRole").IsNull() ? SysXWebSiteUtils.SessionService.GetCustomData("IsPageMapUserRole") : false);
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("IsPageMapUserRole", value);
            }
        }

        /// <summary>
        /// Gets the value of Validation Group.
        /// </summary>
        String IMapUserRoleView.ValidationGroup
        {
            get
            {
                return fsucCmdBar.ValidationGroup;
            }
        }

        #endregion

        /// <summary>
        /// Gets or Sets the value for selected roles.
        /// </summary>
        Dictionary<Guid, KeyValuePair<String, Boolean>> IMapUserRoleView.SelectedItems
        {
            get
            {
                if (!ViewState["SelectedItems"].IsNull())
                {
                    return ViewState["SelectedItems"] as Dictionary<Guid, KeyValuePair<String, Boolean>>;
                }

                return new Dictionary<Guid, KeyValuePair<String, Boolean>>();
            }
            set
            {
                ViewState["SelectedItems"] = value;
            }
        }

        /// <summary>
        /// Presenter.
        /// </summary>
        /// <value>
        /// Represents Manage Tenant Presenter.
        /// </value>
        
        public MapUserRolePresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        /// <summary>
        /// AllRoles.
        /// </summary>
        /// <value>
        /// Gets or sets the list of all roles.
        /// </value>
        List<aspnet_Roles> IMapUserRoleView.AllRoles
        {
            set;
            get;
        }

        /// <summary>
        /// AllRolesOfProduct.
        /// </summary>
        /// <value>
        /// Gets or sets the list of all roles, created under tenant's product.
        /// </value>
        List<aspnet_Roles> IMapUserRoleView.AllRolesOfProduct
        {
            set;
            get;
        }

        /// <summary>
        /// CurrentUserRoles.
        /// </summary>
        /// <value>
        /// Gets or sets the list of all roles for current user.
        /// </value>
        List<aspnet_Roles> IMapUserRoleView.CurrentUserRoles
        {
            set;
            get;
        }

        /// <summary>
        /// SelectedUser.
        /// </summary>
        /// <value>
        /// Gets or sets the value for selected user.
        /// </value>
        OrganizationUser IMapUserRoleView.SelectedUser
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <value>
        /// The view contract.
        /// </value>
        MapUserRoleContract IMapUserRoleView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new MapUserRoleContract();
                }

                return _viewContract;
            }
        }

        /// <summary>
        /// Gets or Sets the value for all assigned roles to a selected user.
        /// </summary>
        /// <value>
        /// The Role Name.
        /// </value>
        String[] IMapUserRoleView.AllAssignedRoleName
        {
            get;
            set;
        }

        /// <summary>
        /// IsAdmin.
        /// </summary>
        /// <value>
        /// Gets or sets the value for user is admin or not?
        /// </value>
        Boolean IMapUserRoleView.IsAdmin
        {
            get
            {
                return base.IsSysXAdmin;
            }
        }

        /// <summary>
        /// CurrentUserID.
        /// </summary>
        /// <value>
        /// Gets or sets the value for current user's id.
        /// </value>
        Int32 IMapUserRoleView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IMapUserRoleView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        private List<BreadCrumbNode> ReturnPath
        {
            get
            {
                if (Session["BreadCrumb"].IsNull())
                {
                    return new List<BreadCrumbNode>();
                }

                return (Session["BreadCrumb"] as List<BreadCrumbNode>);
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// Override this method and set IsPolicyEnable = false to disable policy settings. - TG
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MAP_USER_ROLE);
                lblMapUserRole.Text = base.Title;
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

                //code added by user group team
                Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>();

                if (!Request.QueryString["args"].IsNull())
                {
                    encryptedQueryString.ToDecryptedQueryString(Request.QueryString["args"]);
                    IsNoUserGroup = encryptedQueryString.ContainsKey("UserGroup") ? Convert.ToBoolean(encryptedQueryString["UserGroup"]) : true;
                    IsUserOfUserGroup = encryptedQueryString.ContainsKey("UserGroupUser") ? Convert.ToBoolean(encryptedQueryString["UserGroupUser"]) : false;
                    if (!IsNoUserGroup)
                    {
                        CurrentViewContext.ViewContract.UserGroupId = Convert.ToInt32(encryptedQueryString["UserGroupId"]);
                        base.Title = "Map User Group Role";
                        lblMapUserRole.Text = base.Title;
                    }
                }
                base.OnInit(e);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>();

                if (!Request.QueryString["args"].IsNull())
                {
                    encryptedQueryString.ToDecryptedQueryString(Request.QueryString["args"]);
                    IsNoUserGroup = encryptedQueryString.ContainsKey("UserGroup") ? Convert.ToBoolean(encryptedQueryString["UserGroup"]) : true;
                    if (!IsNoUserGroup || IsUserOfUserGroup)
                    {
                        CurrentViewContext.ViewContract.UserGroupId = Convert.ToInt32(encryptedQueryString["UserGroupId"]);
                        CurrentViewContext.ViewContract.UserId = encryptedQueryString.ContainsKey("UserId") ? encryptedQueryString["UserId"] : String.Empty;
                        CurrentViewContext.ViewContract.OrganizationUserId = encryptedQueryString.ContainsKey("OrgUserId") ? Convert.ToInt32(encryptedQueryString["OrgUserId"]) : -1;
                    }
                }

                //ClientOnBoardingWiz: Implementation 14/01/2012
                //CurrentViewContext.ViewContract.UserId = encryptedQueryString.ContainsKey("UserId") ? encryptedQueryString["UserId"] : String.Empty;
                if (!CurrentViewContext.IsClientOnBoardingWizard )
                {
                    CurrentViewContext.ViewContract.UserId = encryptedQueryString.ContainsKey("UserId") ? encryptedQueryString["UserId"] : String.Empty;
                    CurrentViewContext.ViewContract.OrganizationUserId = encryptedQueryString.ContainsKey("OrgUserId") ? Convert.ToInt32(encryptedQueryString["OrgUserId"]) : -1;
                }

                //ClientOnBoardingWiz: Implementation 14/01/2012
                if (CurrentViewContext.IsDataLoad && (!IsPageLoadByClientOnBoardingWizard))
                {
                    grdUserRole.Rebind();
                    IsPageLoadByClientOnBoardingWizard = true;
                }

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    ViewState["SelectedItems"] = null;
                }

                Presenter.OnViewLoaded();

                if (CurrentViewContext.SelectedUser.IsNull())
                {
                    return;
                }

                if (!IsNoUserGroup && encryptedQueryString.ContainsKey("UserGroupName"))
                {
                    lblSuffix.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_FOR) + encryptedQueryString["UserGroupName"];
                    base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_USER_ROLE));
                }
                else
                {
                    lblSuffix.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_FOR) + CurrentViewContext.SelectedUser.FirstName.HtmlEncode() + SysXUtils.GetMessage(ResourceConst.SPACE) + CurrentViewContext.SelectedUser.LastName.HtmlEncode();
                    base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_USER_ROLE));
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Save Map User Role information.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //CurrentViewContext.ViewContract.DefaultPassword = radCpatchaDefaultPassword.CaptchaImage.Text.Trim();

                if (ConfigurationManager.AppSettings["CurrentEnvironment"].ToString() == "ILab")
                {
                    CurrentViewContext.ViewContract.DefaultPassword = "Password1";
                }
                else
                {
                    CurrentViewContext.ViewContract.DefaultPassword = radCpatchaDefaultPassword.CaptchaImage.Text.Trim();
                }
                Presenter.MappingUserRoles(IsNoUserGroup,IsUserOfUserGroup);

                if (CurrentViewContext.IsClientOnBoardingWizard)
                {
                    SaveMappingClick.Invoke(sender, e);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                RedirectToManageUser();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Grid View Events

        /// <summary>
        /// Retrieves information for mapping between user and roles.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdUserRole_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.RetrievingRoles(IsNoUserGroup,IsUserOfUserGroup); 
                grdUserRole.DataSource = (from allRolesOfProduct in CurrentViewContext.AllRolesOfProduct
                                          join currentRole
                                              in CurrentViewContext.CurrentUserRoles on allRolesOfProduct.RoleId
                                              equals currentRole.RoleId into joinedAllCur
                                          from currentRole in joinedAllCur.DefaultIfEmpty()
                                          select new { allRolesOfProduct, currentRole }).ToList().OrderBy(condition => condition.allRolesOfProduct.RoleDetail.CreatedOn).Select(
                                          condition =>
                                          new
                                          {
                                              condition.allRolesOfProduct.RoleId,
                                              RoleName = condition.allRolesOfProduct.RoleName.Split(new char[] { '_' }).FirstOrDefault(),
                                              condition.allRolesOfProduct.Description,
                                              RoleAssigned = (condition.currentRole != null)
                                          });
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Handles the change in the role's selection between pages.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdUserRole_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
              
              CheckBox cb=((CheckBox)e.Item.FindControl("chkAssigned"));
              if (!cb.IsNull() && !IsUserOfUserGroup)
              {
                  cb.Attributes.Add("onclick", "ClickRole(this," + CurrentViewContext.ViewContract.UserGroupId + ",'" + (Guid)((GridDataItem)e.Item).GetDataKeyValue("RoleId") + "')");
              }
                if (CurrentViewContext.SelectedItems.Count <= AppConsts.NONE)
                {
                    return;
                }

                if (!(e.Item is GridDataItem))
                {
                    return;
                }

                GridDataItem row = (GridDataItem)e.Item;
                Guid roleId = (Guid)row.GetDataKeyValue("RoleId");

                if (CurrentViewContext.SelectedItems.ContainsKey(roleId))
                {
                    ((CheckBox)row.FindControl("chkAssigned")).Checked = CurrentViewContext.SelectedItems[roleId].Value;
                  

                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdUserRole_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName)
                {
                    foreach (GridFilteringItem filterItem in grdUserRole.MasterTableView.GetItems(GridItemType.FilteringItem))
                    {
                        filterItem.Visible = false;
                    }
                    grdUserRole.ExportSettings.ExportOnlyData = true;
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Handles the change in the role's selection between pages.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void chkAssigned_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;

                if (checkBox.IsNull())
                {
                    return;
                }

                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                Dictionary<Guid, KeyValuePair<String, Boolean>> selectedItems = CurrentViewContext.SelectedItems;
                Guid roleId = (Guid)dataItem.GetDataKeyValue("RoleId");
                String roleName = (String)dataItem.GetDataKeyValue("RoleName");
                Boolean isChecked = ((CheckBox)dataItem.FindControl("chkAssigned")).Checked;

                if (selectedItems.ContainsKey(roleId))
                {
                    selectedItems[roleId] = new KeyValuePair<String, Boolean>(roleName, isChecked);
                }
                else
                {
                    selectedItems.Add(roleId, new KeyValuePair<String, Boolean>(roleName, isChecked));
                }

                CurrentViewContext.SelectedItems = selectedItems;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Redirect to Manager User page.
        /// </summary>
        public void RedirectToManageUser()
        {
            try
            {
                //ClientOnBoardingWiz: Implementation 14/01/2012
                if (!CurrentViewContext.IsClientOnBoardingWizard)
                {
                    if (ReturnPath.Count > AppConsts.NONE)
                    {
                        BreadCrumbNode node = ReturnPath.Where(condition => (condition.Level.Equals((ReturnPath.Count - AppConsts.ONE)))).FirstOrDefault();
                        Response.Redirect(!node.IsNull() ? node.NodeURL : String.Format("Default.aspx?ucid={0}", _viewType), false);
                    }
                    else
                    {
                        Response.Redirect(String.Format("Default.aspx?ucid={0}", _viewType), false);
                    }
                }
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion
    }
}