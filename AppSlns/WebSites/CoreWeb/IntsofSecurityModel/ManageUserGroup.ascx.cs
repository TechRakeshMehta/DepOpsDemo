#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageUserGroup.ascx.cs
// Purpose:   To Manage user group
//

#endregion

#region Namespaces

#region System Namespace

using System;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Web.UI.WebControls;

#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel.Interface.Services;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTERSOFT.WEB.UI.WebControls;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    public partial class ManageUserGroup : BaseUserControl, IManageUserGroupView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ManageUserGroupPresenter _presenter=new ManageUserGroupPresenter();
        private ManageUserGroupContract _viewContract;
        private ISysXSessionService _sessionService = SysXWebSiteUtils.SessionService;
        private String _viewType;

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IManageUserGroupView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        

        #endregion

        #region public  Properties

        ///<summary>
        /// Gets or sets the value for Presenter.
        ///</summary>
        public ManageUserGroupPresenter Presenter
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
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ManageUserGroupContract IManageUserGroupView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManageUserGroupContract();
                }

                return _viewContract;
            }
        }

        /// <summary>
        /// Gets the CreatedBy field of the User.
        /// </summary>
        Int32 IManageUserGroupView.CreatedById
        {
            get
            {
                return _sessionService.OrganizationUserId;
            }
        }

        /// <summary>
        /// Sets the success message.
        /// </summary>
        String IManageUserGroupView.SuccessMessage
        {
            set
            {
                lblSuccess.ShowMessage(value, MessageType.SuccessMessage);
            }
        }

        /// <summary>
        /// Get Product Id of logged In user.
        /// </summary>
        Int32? IManageUserGroupView.AssignToProductId
        {
            get
            {
                return base.SysXMembershipUser.ProductId;
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();

            //Setting Screens name
            base.SetPageTitle("User Groups");
        }

        /// <summary>
        /// Override this method and set IsPolicyEnable = false to disable policy settings. - TG
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "User Group";// SysXUtils.GetMessage("User Group");
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
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

        #endregion

        #region Grid Event

        protected void grdUsrGrps_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            Int32 TenantId;
            CurrentViewContext.ViewContract.UserGoupName = ((WclTextBox)e.Item.FindControl("txtName")).Text.Trim();
            CurrentViewContext.ViewContract.UserGroupDescription = ((WclTextBox)e.Item.FindControl("txtDescription")).Text.Trim();
            try
            {
                CurrentViewContext.ViewContract.CreatedById = base.CurrentUserId;
                CurrentViewContext.ViewContract.IsAdmin = base.IsSysXAdmin;
                if (base.IsSysXAdmin)
                {
                    TenantId = Convert.ToInt32(((WclComboBox)e.Item.FindControl("cmbTenant")).SelectedValue);
                }
                else
                {
                    TenantId = Convert.ToInt32(base.SysXMembershipUser.TenantId);
                }
                CurrentViewContext.ViewContract.TenantId = TenantId;
                Presenter.InsertUserGroup();
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

        protected void grdUsrGrps_ItemDeleted(object sender, GridCommandEventArgs e)
        {
            CurrentViewContext.ViewContract.UserGoupId = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserGroupId"]);

            try
            {
                CurrentViewContext.ViewContract.CreatedById = base.CurrentUserId;
                CurrentViewContext.ViewContract.IsAdmin = base.IsSysXAdmin;
                Presenter.DeleteUserGroup();
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

        protected void grdUsrGrps_ItemUpdated(object sender, GridCommandEventArgs e)
        {
            CurrentViewContext.ViewContract.UserGoupId = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserGroupId"]);
            CurrentViewContext.ViewContract.UserGoupName = ((WclTextBox)e.Item.FindControl("txtName")).Text.Trim();
            CurrentViewContext.ViewContract.UserGroupDescription = ((WclTextBox)e.Item.FindControl("txtDescription")).Text.Trim();
            try
            {
                Int32 TenantId = Convert.ToInt32(((WclComboBox)e.Item.FindControl("cmbTenant")).SelectedValue);
                CurrentViewContext.ViewContract.TenantId = TenantId;
                CurrentViewContext.ViewContract.CreatedById = base.CurrentUserId;
                CurrentViewContext.ViewContract.IsAdmin = base.IsSysXAdmin;
                Presenter.UpdateUserGroup();
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

        protected void grdUsrGrps_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdUsrGrps.DataSource = Presenter.GetAllUserGroups(base.IsSysXAdmin, base.CurrentUserId);
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

        protected void grdName1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                WclGrid grid = (WclGrid)sender;
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                CurrentViewContext.ViewContract.UserGoupId = Convert.ToInt32(parentItem.GetDataKeyValue("UserGroupId"));
                grid.DataSource = Presenter.GetAllUsersInAgroup();
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

        protected void grdUsrGrps_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.SuccessMessage = String.Empty;
                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid rg = parentItem.ChildItem.FindControl("grdName1") as RadGrid;
                    rg.Rebind();
                }
               

                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdUsrGrps);
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

        protected void grdName1_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                WclGrid grid = (WclGrid)sender;
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                CurrentViewContext.ViewContract.UserGoupId = Convert.ToInt32(parentItem.GetDataKeyValue("UserGroupId"));
                CurrentViewContext.ViewContract.UsersInUserGroupID = Convert.ToInt32((e.Item).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UsersInUserGroupID"]);
                Presenter.RemoveUserInUserGroup();
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

        protected void grdName1_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                WclComboBox cmbUserList = (WclComboBox)e.Item.FindControl("cmbUsrList");
                if (!cmbUserList.SelectedItem.Text.Equals(AppConsts.COMBOBOX_ITEM_SELECT))
                {
                    CurrentViewContext.ViewContract.Aspnet_UserId = Guid.Parse(cmbUserList.SelectedValue);
                    WclGrid grid = (WclGrid)sender;
                    GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                    CurrentViewContext.ViewContract.UserGoupId = Convert.ToInt32(parentItem.GetDataKeyValue("UserGroupId"));
                    Presenter.AddUserInUserGroup();
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
        protected void grdUsrGrps_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditFormItem)
            {
                if (base.SysXMembershipUser.TenantTypeCode == TenantType.Employee.GetStringValue() || base.IsSysXAdmin)
                {

                    WclComboBox cmbTenantList = e.Item.FindControl("cmbTenant") as WclComboBox;
                    List<Tenant> tenants = Presenter.GetTenants().ToList();
                    Tenant tenant = new Tenant { TenantID = AppConsts.NONE, TenantName = AppConsts.COMBOBOX_ITEM_SELECT };
                    tenants.Insert(AppConsts.NONE, tenant);

                    if (!cmbTenantList.IsNull())
                        cmbTenantList.DataSource = tenants;

                    UserGroup userGroup = e.Item.DataItem as UserGroup;
                    if (!userGroup.IsNull())
                    {
                        List<UsersInUserGroup> usersInuserGroup = userGroup.UsersInUserGroups.Where(condtion => condtion.IsActive == true).ToList();
                        if (usersInuserGroup.Count > AppConsts.NONE)
                        {
                            if (!cmbTenantList.IsNull())
                                cmbTenantList.Enabled = false;
                        }
                    }
                }
                else
                {
                    HtmlGenericControl div = e.Item.FindControl("divTenant") as HtmlGenericControl;
                    if (!div.IsNull())
                    {
                        div.Visible = false;
                    }
                }

            }
        }
        protected void grdName1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditFormItem)
            {
                WclComboBox combo = e.Item.FindControl("cmbUsrList") as WclComboBox;
                if (!combo.IsNull())
                {
                    GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                    Int32 TenantId = Convert.ToInt32(parentItem.GetDataKeyValue("TenantID"));
                    CurrentViewContext.ViewContract.TenantId = Convert.ToInt32(TenantId);
                    if (TenantId > AppConsts.NONE)
                    {
                        combo.DataSource = Presenter.GetAspnetUsers(Convert.ToInt32(Presenter.GetTenantProductId())).Select(condition => condition.aspnet_Users);
                    }

                    combo.AddFirstEmptyItem();

                }

            }
        }

        protected void grdName1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            String orgUserId = AppConsts.ZERO;

            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                UsersInUserGroup usergroupRole = e.Item.DataItem as UsersInUserGroup;
                HtmlAnchor anchorRole = (HtmlAnchor)e.Item.FindControl("ancRoles");
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "UserGroupId", Convert.ToString(usergroupRole.UserGroupID) },
                                                                    { "UserId", Convert.ToString(usergroupRole.UserID) },
                                                                    { "Child", ChildControls.SecurityMapUserRole},
                                                                    { "ParentControlName", ChildControls.SecurityManageUserGroup},
                                                                    {"UserGroup", "false"},
                                                                    {"UserGroupUser","true"}
                                                                 };

                anchorRole.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                HtmlAnchor ancManageUsr = (HtmlAnchor)e.Item.FindControl("ancManageUsr");

                if (ancManageUsr != null)
                {
                    if (usergroupRole.aspnet_Users.OrganizationUsers.Any(fx => fx.Organization.Tenant.lkpTenantType.TenantTypeCode.Equals(TenantType.Company.GetStringValue())))
                    {

                        orgUserId = usergroupRole.aspnet_Users.OrganizationUsers.FirstOrDefault().OrganizationUserID.ToString();

                        Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>
                        {                            
                                { "UserGroupId", Convert.ToString(usergroupRole.UserGroupID) },
                                { "UserId", Convert.ToString(usergroupRole.UserID) },
                                { "IsUserLevel", "True" },
                                { "OrganizationUserId", orgUserId},
                                { "Child", @"UserControl\ManageQueue.ascx"},
                                { "PageSource","ManageQueue"}
                            
                        };

                        ancManageUsr.HRef = string.Format("~/Queues/Default.aspx?ucid={0}&args={1}", _viewType, encryptedQueryString.ToEncryptedQueryString());
                    }
                    else
                    {
                        ancManageUsr.Visible = false;
                    }


                }
            }
        }

        protected void grdUsrGrps_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                UserGroup usergroupRole = e.Item.DataItem as UserGroup;
                HtmlAnchor anchorRole = (HtmlAnchor)e.Item.FindControl("ancRoles");
                OrganizationUser organizationUser = Presenter.GetOrganizationUser(usergroupRole.CreatedByID);
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "UserGroupId", Convert.ToString(usergroupRole.UserGroupID) },
                                                                    { "UserId", organizationUser==null?String.Empty:organizationUser.UserID.ToString() },
                                                                    { "Child", ChildControls.SecurityMapUserRole},
                                                                    { "ParentControlName", ChildControls.SecurityManageUserGroup},
                                                                    {"UserGroup", "false"},
                                                                    {"UserGroupName", usergroupRole.UserGroupName}
                                                                 };

                anchorRole.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                HyperLink hypMngQueGrp = (HyperLink)e.Item.FindControl("hypMngQueGrp");
                String orgUserId = AppConsts.ZERO;
                if (hypMngQueGrp != null)
                {
                    if (base.IsSysXAdmin)
                    {
                        Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>
                        {
                            { "UserGroupId",Convert.ToString(usergroupRole.UserGroupID)},
                            { "IsUserLevel", "False" },
                            { "OrganizationUserId", orgUserId},
                            { "Child", @"UserControl\ManageQueue.ascx"},
                            { "PageSource","ManageQueue"}
                        };

                        hypMngQueGrp.NavigateUrl = String.Format("~/Queues/Default.aspx?ucid={0}&args={1}", _viewType, encryptedQueryString.ToEncryptedQueryString());
                    }
                    else
                    {
                        if (usergroupRole.RolesInUserGroups.Any(role => role.aspnet_Roles.aspnet_Users.Any(usr => usr.OrganizationUsers
                            .Any(org => org.Organization.Tenant.lkpTenantType.TenantTypeCode.Equals(TenantType.Employee.GetStringValue())))))
                        {

                            orgUserId = usergroupRole.RolesInUserGroups.Any(role => role.aspnet_Roles.aspnet_Users.Any(usr => usr.OrganizationUsers.Any())) ?
                                               usergroupRole.RolesInUserGroups.FirstOrDefault().aspnet_Roles.aspnet_Users.FirstOrDefault().OrganizationUsers.FirstOrDefault().OrganizationUserID.ToString()
                                               : AppConsts.ZERO;


                            Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>
                            {
                                { "UserGroupId",Convert.ToString(usergroupRole.UserGroupID)},
                                { "IsUserLevel", "False" },
                                { "OrganizationUserId", orgUserId},
                                { "Child", @"UserControl\ManageQueue.ascx"},
                                { "PageSource","ManageQueue"}
                            };

                            hypMngQueGrp.NavigateUrl = String.Format("~/Queues/Default.aspx?ucid={0}&args={1}", _viewType, encryptedQueryString.ToEncryptedQueryString());
                        }
                        else
                        {
                            hypMngQueGrp.Visible = false;
                        }
                    }
                }
            }
            if (e.Item.ItemType == GridItemType.EditFormItem && e.Item.IsInEditMode && e.Item.ItemIndex >= Convert.ToInt32(DefaultNumbers.None))
            {
                (e.Item.FindControl("cmbTenant") as WclComboBox).SelectedValue = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["TenantID"]).ToString();
            }
        }

        #endregion

        #region Control Events



        #endregion

        #endregion
    }
}