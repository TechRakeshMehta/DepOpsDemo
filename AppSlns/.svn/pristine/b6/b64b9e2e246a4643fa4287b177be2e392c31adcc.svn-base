#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageUsers.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Configuration;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using System.Linq;
#endregion

#region Application Specific

using Entity;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.Shell.Views;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using Business.RepoManagers;
using System.Web.UI;
using System.Collections;
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.UI.Contract.CommonControls;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to managing users in security module.
    /// </summary>
    /// <remarks></remarks>
    public partial class ManageUsers : BaseUserControl, IManageUsersView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ManageUsersPresenter _presenter = new ManageUsersPresenter();
        private String _viewType;
        private ManageUsersContract _viewContract = null;
        private CustomPagingArgsContract _manageUserCustomPaging = null;

        #endregion

        #endregion

        #region Properties

        #region ClientOnBoardingWizard

        Boolean IManageUsersView.IsDataLoad
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
        Boolean IManageUsersView.IsClientOnBoardingWizard
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the value of Validation Group.
        /// </summary>
        String IManageUsersView.ValidationGroup
        {
            get
            {
                return grmUsrList.ValidationSettings.ValidationGroup;
            }
        }

        Boolean IsPageLoadByClientOnBoardingWizard
        {
            get
            {
                return Convert.ToBoolean(!SysXWebSiteUtils.SessionService.GetCustomData(SysXUtils.GetMessage(ResourceConst.SECURITY_IS_PAGE_MANAGE_USERS)).IsNull() ? SysXWebSiteUtils.SessionService.GetCustomData(SysXUtils.GetMessage(ResourceConst.SECURITY_IS_PAGE_MANAGE_USERS)) : false);
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData(SysXUtils.GetMessage(ResourceConst.SECURITY_IS_PAGE_MANAGE_USERS), value);
            }
        }

        #endregion

        #region Client Profile

        Boolean IManageUsersView.IsClientProfile
        {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Gets or sets the presenter.
        /// </summary>
        /// <value>The presenter.</value>
        /// <remarks></remarks>

        public ManageUsersPresenter Presenter
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
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        /// <remarks></remarks>
        String IManageUsersView.ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current user id.
        /// </summary>
        /// <remarks></remarks>
        Int32 IManageUsersView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// Gets or sets the name of the reset user.
        /// </summary>
        /// <value>The name of the reset user.</value>
        /// <remarks></remarks>
        String IManageUsersView.ResetUserName
        {
            set
            {
                ViewState["resetUserName"] = value;
            }
            get
            {
                return Convert.ToString(ViewState["resetUserName"]);
            }
        }

        /// <summary>
        /// Gets the product id.
        /// </summary>
        /// <remarks></remarks>
        Int32? IManageUsersView.ProductId
        {
            get
            {
                if (!CurrentViewContext.IsClientOnBoardingWizard && !CurrentViewContext.IsClientProfile)
                {
                    return base.SysXMembershipUser.ProductId;
                }
                else
                {
                    return CurrentViewContext.ViewContract.ProductIdFromClientWizard;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is admin.
        /// </summary>
        /// <remarks></remarks>
        Boolean IManageUsersView.IsAdmin
        {
            get
            {
                return base.IsSysXAdmin;
            }
        }

        /// <summary>
        /// Gets or sets all organization.
        /// </summary>
        /// <value>All organization.</value>
        /// <remarks></remarks>
        List<Organization> IManageUsersView.AllOrganization
        {
            set;
            get;
        }

        /// <summary>
        /// Gets or sets all user name prefix.
        /// </summary>
        /// <value>All Prefix.</value>
        /// <remarks></remarks>
        List<OrganizationUserNamePrefix> IManageUsersView.AllUserNamePrefix
        {
            set;
            get;
        }

        /// <summary>
        /// Sets the mapped organization users.
        /// </summary>
        /// <value>The mapped organization users.</value>
        /// <remarks></remarks>
        List<OrganizationUserContract> IManageUsersView.MappedOrganizationUsers
        {
            set
            {
                grmUsrList.DataSource = value;
            }
        }

        /// <summary>
        /// Gets or sets the created by organization users.
        /// </summary>
        /// <value>The created by organization users.</value>
        /// <remarks></remarks>
        List<OrganizationUserContract> IManageUsersView.CreatedByOrganizationUsers
        {
            set;
            get;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        ManageUsersContract IManageUsersView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManageUsersContract();
                }

                return _viewContract;
            }
        }

        List<OrganizationUser> IManageUsersView.lstClientAdminUsers
        {
            get
            {
                return ViewState["lstClientAdminUsers"] as List<OrganizationUser>;
            }
            set
            {
                ViewState["lstClientAdminUsers"] = value;
            }
        }

        Guid IManageUsersView.CopyFromClientAdminUserID { get; set; }

        Int32 IManageUsersView.CopyFromClientAdminOrgID { get; set; }

        Int32 IManageUsersView.SelectedTenantId
        {
            get
            {
                return ViewState["SelectedTenantID"].IsNotNull() ? Convert.ToInt32(ViewState["SelectedTenantID"]) : 0;
            }
            set
            {
                ViewState["SelectedTenantID"] = value;
            }
        }
        #region Paging

        /// <summary>
        /// CurrentPageIndex
        /// </summary>
        /// <value> Gets or sets the value for CurrentPageIndex.</value>
        Int32 IManageUsersView.CurrentPageIndex
        {
            get
            {
                return grmUsrList.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grmUsrList.MasterTableView.CurrentPageIndex = value == 0 ? 0 : value - 1;
            }
        }

        /// <summary>
        /// PageSize
        /// </summary>
        /// <value> Gets the value for PageSize.</value>
        Int32 IManageUsersView.PageSize
        {
            get
            {
                // Maximum 100 record allowed from DB. 
                //return grdItemData.PageSize > 100 ? 100 : grdItemData.PageSize;
                return grmUsrList.PageSize;
            }
        }

        /// <summary>
        /// VirtualPageCount
        /// </summary>
        /// <value> Sets the value for VirtualPageCount.</value>
        Int32 IManageUsersView.VirtualPageCount
        {
            set
            {
                grmUsrList.VirtualItemCount = value;
                grmUsrList.MasterTableView.VirtualItemCount = value;
            }
        }

        CustomPagingArgsContract IManageUsersView.ManageUserCustomPaging
        {
            get
            {
                if (_manageUserCustomPaging.IsNull())
                {
                    _manageUserCustomPaging = new CustomPagingArgsContract();
                }
                return _manageUserCustomPaging;
            }
        }

        #endregion

        /// <summary>
        /// SuccessMessage</summary>
        /// <value>
        /// Gets or sets the value for Success message.</value>
        String IManageUsersView.SuccessMessage
        {
            get;
            set;
        }

       
        public Entity.OrganizationUser ExistingOrganisationUser
        {
            get;
            set;
        }

        public String SelectedLinkingProfileOrgUsername
        {
            get;
            set;
        }
        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IManageUsersView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion

        #endregion

        #region Events

        public event EventHandler<EventArgs> MangeRolesClick;

        public event EventHandler<EventArgs> MapInstitutionClick;

        #region Page Events

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// Calls WebClientApplication.BuildItemWithCurrentContext after the events are handled
        /// to fire the DI engine.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_USERS);
                //lblManageUser.Text = base.Title;
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Dictionary<String, String> args = new Dictionary<String, String>();

                //if (!Request.QueryString["args"].IsNull())
                //{
                //    args.ToDecryptedQueryString(Request.QueryString["args"]);
                //}

                ////ClientOnBoardingWiz: Implementation 14/01/2012
                ////CurrentViewContext.ViewContract.TenantId = args.ContainsKey("TenantId") ? Int32.Parse(args["TenantId"]) : AppConsts.NONE;
                //if (!CurrentViewContext.IsClientOnBoardingWizard && !CurrentViewContext.IsClientProfile)
                //{
                //    CurrentViewContext.ViewContract.TenantId = args.ContainsKey("TenantId") ? Int32.Parse(args["TenantId"]) : AppConsts.NONE;
                //}

                ////ClientOnBoardingWiz: Implementation 14/01/2012
                //if (CurrentViewContext.IsDataLoad && (!IsPageLoadByClientOnBoardingWizard))
                //{
                //    grmUsrList.Rebind();
                //    IsPageLoadByClientOnBoardingWizard = true;
                //}

                //CurrentViewContext.ViewContract.AssignToRoleId = args.ContainsKey("RoleDetailId") ? Convert.ToString(args["RoleDetailId"]) : String.Empty;
                //CurrentViewContext.ViewContract.AssignToRoleName = new[] { args.ContainsKey("RoleDetailName") ? Convert.ToString(args["RoleDetailName"]) : String.Empty };
                //CurrentViewContext.ViewContract.AssignToProductId = args.ContainsKey("ProductID") ? Convert.ToInt32(args["ProductID"]) : AppConsts.NONE;
                //CurrentViewContext.ViewContract.AssignToDepartmentId = args.ContainsKey("AssignToDepartmentId") ? Convert.ToInt32(args["AssignToDepartmentId"]) : AppConsts.NONE;
                //CurrentViewContext.ViewContract.IsLinkOnTenant = args.ContainsKey("IsLinkOnTenant") && args["IsLinkOnTenant"].Equals("yes", StringComparison.InvariantCultureIgnoreCase);
                //CurrentViewContext.ViewContract.IsComingThroughTenant = args.ContainsKey("IsComingThroughTenant") && args["IsComingThroughTenant"].Equals("yes", StringComparison.InvariantCultureIgnoreCase);

                CaptureQueryString(); //UAT-3394

                //if (!this.IsPostBack)
                //{
                //    Presenter.OnViewInitialized();
                //}
                //Presenter.OnViewLoaded();

                //if (!CurrentViewContext.ViewContract.TenantName.IsNullOrEmpty())
                //{
                //    lblManageTenantSuffix.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_FOR) + CurrentViewContext.ViewContract.TenantName;
                //}

                //String roleName = args.ContainsKey("RoleDetailDescription") ? args["RoleDetailDescription"] : String.Empty;
                //lblManageUser.Text = String.IsNullOrEmpty(roleName) ? base.Title : roleName + "&nbsp;>&nbsp;" + base.Title;

                //base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_USERS));
                //lblSuccess.Visible = false;
                //lblSuccess.Text = String.Empty;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Grid Related Events

        /// <summary>
        /// Performs an update operation for a User.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grmUsrList_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                var profileLinkingData = GetSessionValues();
                if (profileLinkingData.ManageUsersContract.IsNullOrEmpty())
                {
                    CurrentViewContext.ViewContract.UserName = (e.Item.FindControl("txtUserName") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.FirstName = (e.Item.FindControl("txtFirstName") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.LastName = (e.Item.FindControl("txtLastName") as WclTextBox).Text.Trim();

                    #region UAT-2447
                    if ((e.Item.FindControl("chkIsMaskingRequired") as WclCheckBox).Checked)
                    {
                        CurrentViewContext.ViewContract.MobileAlias = (e.Item.FindControl("unmaskTxtMobile") as WclTextBox).Text.Trim();
                    }
                    else
                    {
                        CurrentViewContext.ViewContract.MobileAlias = (e.Item.FindControl("maskTxtMobile") as WclMaskedTextBox).Text.Trim();
                    }
                    CurrentViewContext.ViewContract.IsInternationalPhoneNumber = (e.Item.FindControl("chkIsMaskingRequired") as WclCheckBox).Checked;
                    #endregion
                    CurrentViewContext.ViewContract.Organizations = (e.Item.FindControl("cmbOrganization") as WclComboBox).SelectedValue;
                    CurrentViewContext.ViewContract.Active = Convert.ToBoolean((e.Item.FindControl("chkActive") as IsActiveToggle).Checked);
                    //Added IsApplicant check
                    // CurrentViewContext.ViewContract.IsApplicant = (e.Item.FindControl("chkApplicant") as CheckBox).Checked;
                    CurrentViewContext.ViewContract.IsMessagingUse = false;
                    CurrentViewContext.ViewContract.IsLockedOut = false;
                    CurrentViewContext.ViewContract.DefaultPassword = ConfigurationManager.AppSettings["CurrentEnvironment"] == "ILab" ? "Password1" : radCpatchaPassword.CaptchaImage.Text.Trim();
                    CurrentViewContext.ViewContract.PrefixName = (e.Item.FindControl("cmbUsernamePrefix") as WclComboBox).SelectedValue.Equals(AppConsts.ZERO) ? String.Empty : (e.Item.FindControl("cmbUsernamePrefix") as WclComboBox).SelectedItem.Text;
                    CurrentViewContext.ViewContract.EmailAddress = (e.Item.FindControl("txtEmailAddress") as WclTextBox).Text.Trim();
                    //UAT-985: WB: Simplification of Admin Account creation.
                    CurrentViewContext.ViewContract.Password = (e.Item.FindControl("txtPassword") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.IsNewPassword = Convert.ToBoolean((e.Item.FindControl("rblChangePasswordUponFirstLogin") as RadioButtonList).SelectedValue);

                    //UAT-2257-Create client admins to mirror permissions of existing admins
                    if (!(e.Item.FindControl("cmbClientAdmin") as WclComboBox).SelectedValue.IsNullOrEmpty())
                    {
                        CurrentViewContext.CopyFromClientAdminOrgID = Convert.ToInt32((e.Item.FindControl("cmbClientAdmin") as WclComboBox).SelectedValue);
                        CurrentViewContext.CopyFromClientAdminUserID = CurrentViewContext.lstClientAdminUsers.Where(con => con.OrganizationUserID == CurrentViewContext.CopyFromClientAdminOrgID).Select(con => con.UserID).FirstOrDefault();
                    }


                    #region UAT-3360
                    //Presenter.IsAdminLoggedIn();
                    if (CurrentViewContext.IsAdmin && CurrentViewContext.SelectedTenantId != SecurityManager.DefaultTenantID)
                    {
                        if (Presenter.CheckAccountLinkingExistingProfile(CurrentViewContext.ViewContract.UserName, CurrentViewContext.ViewContract.EmailAddress))
                        {
                            SetSessionValues(CurrentViewContext.ViewContract, CurrentViewContext.ViewContract.UserName, CurrentViewContext.ViewContract.EmailAddress);
                            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "UserAccountLinking();", true);
                            e.Canceled = true;
                            return;
                        }
                    }
                    #endregion

                    Presenter.AddUser();
                }
                else
                {

                    if (profileLinkingData.IsProfileLinked)
                    {
                        CurrentViewContext.SelectedLinkingProfileOrgUsername = profileLinkingData.SelectedLinkingProfileOrgUsername;
                        Presenter.BindExistingProfile();

                        CurrentViewContext.ViewContract.UserName = profileLinkingData.ManageUsersContract.UserName;
                        CurrentViewContext.ViewContract.FirstName = profileLinkingData.ManageUsersContract.FirstName;
                        CurrentViewContext.ViewContract.LastName = profileLinkingData.ManageUsersContract.LastName;
                        CurrentViewContext.ViewContract.MobileAlias = profileLinkingData.ManageUsersContract.MobileAlias;
                        CurrentViewContext.ViewContract.IsInternationalPhoneNumber = profileLinkingData.ManageUsersContract.IsInternationalPhoneNumber;
                        CurrentViewContext.ViewContract.Organizations = profileLinkingData.ManageUsersContract.Organizations;
                        CurrentViewContext.ViewContract.Active = profileLinkingData.ManageUsersContract.Active;
                        CurrentViewContext.ViewContract.IsMessagingUse = false;
                        CurrentViewContext.ViewContract.IsLockedOut = false;
                        CurrentViewContext.ViewContract.DefaultPassword = profileLinkingData.ManageUsersContract.DefaultPassword;
                        CurrentViewContext.ViewContract.PrefixName = profileLinkingData.ManageUsersContract.PrefixName;
                        CurrentViewContext.ViewContract.EmailAddress = profileLinkingData.ManageUsersContract.EmailAddress;
                        //UAT-985: WB: Simplification of Admin Account creation.
                        CurrentViewContext.ViewContract.Password = profileLinkingData.ManageUsersContract.Password;
                        CurrentViewContext.ViewContract.IsNewPassword = profileLinkingData.ManageUsersContract.IsNewPassword;

                        //UAT-2257-Create client admins to mirror permissions of existing admins
                        if (!profileLinkingData.CopyFromClientAdminOrgID.IsNullOrEmpty())
                        {
                            CurrentViewContext.CopyFromClientAdminOrgID = profileLinkingData.CopyFromClientAdminOrgID;
                            CurrentViewContext.CopyFromClientAdminUserID = profileLinkingData.CopyFromClientAdminUserID;
                        }
                        Presenter.AddLinkedProfile();
                    }
                    else
                    {
                        if (profileLinkingData.IsCancelButtonClicked)
                        {
                            e.Canceled = true;
                            return;
                        }
                    } 
                } 
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                }
                else
                {
                    e.Canceled = false;
                    grmUsrList.MasterTableView.CurrentPageIndex = grmUsrList.PageCount;
                    lblSuccess.Visible = true;
                    lblSuccess.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a list of all users with it's details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grmUsrList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                //Presenter.IsDepartmentOrOrganizationExistsForProduct();

                //if (!CurrentViewContext.ViewContract.IsMyOrganizationExists && !Page.Master.IsNull() && !CurrentViewContext.IsClientOnBoardingWizard && !CurrentViewContext.IsClientProfile)
                //{
                //    ((Label)Page.Master.FindControl("lblError")).Text = SysXUtils.GetMessage(ResourceConst.SECURITY_NO_ORGANIZATION_DEPARTMENT_EXIST);
                //}
                CurrentViewContext.ManageUserCustomPaging.CurrentPageIndex = CurrentViewContext.CurrentPageIndex;
                CurrentViewContext.ManageUserCustomPaging.PageSize = CurrentViewContext.PageSize;
                CurrentViewContext.ManageUserCustomPaging.FilterColumns = CurrentViewContext.ViewContract.FilterColumns;
                CurrentViewContext.ManageUserCustomPaging.FilterOperators = CurrentViewContext.ViewContract.FilterOperators;
                CurrentViewContext.ManageUserCustomPaging.FilterValues = CurrentViewContext.ViewContract.FilterValues;
                CurrentViewContext.ManageUserCustomPaging.FilterTypes = CurrentViewContext.ViewContract.FilterTypes;
                Presenter.RetrievingUsers();
                SetFilterValues();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Performs an update operation for User details.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grmUsrList_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.OrganizationUserId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserID"]);
                CurrentViewContext.ViewContract.UserId = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"]);
                CurrentViewContext.ViewContract.ExistingUserName = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserName"]);
                CurrentViewContext.ViewContract.UserName = (e.Item.FindControl("txtUserName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.FirstName = (e.Item.FindControl("txtFirstName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.LastName = (e.Item.FindControl("txtLastName") as WclTextBox).Text.Trim();
                #region UAT-2447
                if ((e.Item.FindControl("chkIsMaskingRequired") as WclCheckBox).Checked)
                {
                    CurrentViewContext.ViewContract.MobileAlias = (e.Item.FindControl("unmaskTxtMobile") as WclTextBox).Text.Trim();
                }
                else
                {
                    CurrentViewContext.ViewContract.MobileAlias = (e.Item.FindControl("maskTxtMobile") as WclMaskedTextBox).Text.Trim();
                }
                CurrentViewContext.ViewContract.IsInternationalPhoneNumber = (e.Item.FindControl("chkIsMaskingRequired") as WclCheckBox).Checked;
                #endregion
                CurrentViewContext.ViewContract.Organizations = (e.Item.FindControl("cmbOrganization") as WclComboBox).SelectedValue;
                CurrentViewContext.ViewContract.Active = Convert.ToBoolean((e.Item.FindControl("chkActive") as IsActiveToggle).Checked);
                //Updated the User IsApplicant Status
                // CurrentViewContext.ViewContract.IsApplicant = (e.Item.FindControl("chkApplicant") as CheckBox).Checked;
                //CurrentViewContext.ViewContract.EmailAddress = CurrentViewContext.ViewContract.UserName;  //Commented to fix bug no. 3657
                CurrentViewContext.ViewContract.EmailAddress = (e.Item.FindControl("txtEmailAddress") as WclTextBox).Text.Trim(); //Added to fix bug no. 3657
                CurrentViewContext.ViewContract.IsLockedOut = (e.Item.FindControl("chkUnlockUser") as CheckBox).Checked;

                //UAT-2257-Create client admins to mirror permissions of existing admins
                if (!(e.Item.FindControl("cmbClientAdmin") as WclComboBox).SelectedValue.IsNullOrEmpty())
                {
                    CurrentViewContext.CopyFromClientAdminOrgID = Convert.ToInt32((e.Item.FindControl("cmbClientAdmin") as WclComboBox).SelectedValue);
                    CurrentViewContext.CopyFromClientAdminUserID = CurrentViewContext.lstClientAdminUsers.Where(con => con.OrganizationUserID == CurrentViewContext.CopyFromClientAdminOrgID).Select(con => con.UserID).FirstOrDefault();
                }


                Presenter.UpdateUser();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                }
                else
                {
                    e.Canceled = false;
                }

                lblSuccess.Visible = true;
                lblSuccess.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Performs a delete operation for User.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grmUsrList_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.OrganizationUserId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserID"]);
                CurrentViewContext.ViewContract.UserId = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UserID"]);
                CurrentViewContext.ViewContract.ExistingUserName = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["aspnet_users.UserName"]);
                Presenter.DeleteUser();
                lblSuccess.Visible = true;
                lblSuccess.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Event handler. Called by grmUsrList for item command events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grmUsrList_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.EditCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    grmUsrList.MasterTableView.IsItemInserted = false;
                }
                else
                {
                    grmUsrList.MasterTableView.ClearChildEditItems();
                }

                #region UAT-2447
                if (e.CommandName == "Save" || e.CommandName == "Update")
                {
                    if (!e.Item.IsNullOrEmpty())
                    {
                        ShowHidePhoneControls((e.Item.FindControl("chkIsMaskingRequired") as WclCheckBox).Checked, e.Item as GridEditFormItem);
                    }
                }
                #endregion

                //ClientOnBoardingWiz: Implementation 14/01/2012
                if (e.CommandName.Equals("ManageRoles"))
                {
                    MangeRolesClick.Invoke(e.CommandArgument, e);
                }
                if (e.CommandName.Equals("MapInstitution"))
                {
                    MapInstitutionClick.Invoke(e.CommandArgument, e);
                }
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grmUsrList);

                }

                if (e.CommandName == RadGrid.FilterCommandName)
                {
                    Pair filter = (Pair)e.CommandArgument;
                    ViewState["FilterPair"] = filter;
                }
                FilterGridColumn();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Event handler. Called by grmUsrList for item created events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grmUsrList_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormInsertItem))
                {
                    GridEditFormItem gridEditFormItem = (GridEditFormItem)e.Item;
                    (gridEditFormItem.FindControl("lblUnlockUser") as Label).Visible = false;
                    (gridEditFormItem.FindControl("chkUnlockUser") as CheckBox).Visible = false;
                    (gridEditFormItem.FindControl("chkActive") as IsActiveToggle).IsActiveEnable = false;
                }
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem gridEditFormItem = (GridEditFormItem)e.Item;
                    WclComboBox ddlOrg = (WclComboBox)gridEditFormItem.FindControl("cmbOrganization");
                    WclComboBox cmbUsernamePrefix = (WclComboBox)gridEditFormItem.FindControl("cmbUsernamePrefix");
                    Label lblOrg = (Label)gridEditFormItem.FindControl("lblOrgTitle");
                    //UAT-2257 
                    WclComboBox cmbClientAdmin = (WclComboBox)gridEditFormItem.FindControl("cmbClientAdmin");


                    //Modified code to set Organization label
                    /*if (CurrentViewContext.IsAdmin)
                    {
                        lblOrg.Text = CurrentViewContext.ViewContract.DisplayOrgType = SysXUtils.GetMessage(ResourceConst.SECURITY_ORGANIZATION);
                    }
                    else
                    {
                        lblOrg.Text = CurrentViewContext.ViewContract.DisplayOrgType = SysXUtils.GetMessage(ResourceConst.SECURITY_DEPARTMENT);
                    } */

                    //Added code to set Organization label
                    lblOrg.Text = CurrentViewContext.ViewContract.DisplayOrgType = SysXUtils.GetMessage(ResourceConst.SECURITY_ORGANIZATION);

                    RequiredFieldValidator validator = (RequiredFieldValidator)gridEditFormItem.FindControl("rfvOrganization");
                    RegularExpressionValidator revPasswordValidator = (RegularExpressionValidator)gridEditFormItem.FindControl("revPassword");

                    if (!(e.Item.DataItem is GridInsertionObject))
                    {
                        var organizationUser = (e.Item).DataItem as OrganizationUserContract;

                        if (!organizationUser.IsNull())
                        {
                            CurrentViewContext.ViewContract.OrganizationUserId = organizationUser.OrganizationUserID;
                        }
                    }
                    CaptureQueryString(); //UAT-3394
                    Presenter.RetrievingOrganizations();

                    if (CurrentViewContext.AllOrganization.IsNotNull())
                    {
                        if (CurrentViewContext.AllOrganization.Count > SysXClientConsts.Zero)
                        {
                            Organization organization = new Organization
                            {
                                OrganizationName = AppConsts.COMBOBOX_ITEM_SELECT,
                                OrganizationID = AppConsts.NONE
                            };

                            CurrentViewContext.AllOrganization.Insert(AppConsts.NONE, organization);
                        }

                        ddlOrg.DataSource = CurrentViewContext.AllOrganization;
                        ddlOrg.DataBind();
                        lblOrg.Text = CurrentViewContext.ViewContract.DisplayOrgType;

                        if (CurrentViewContext.ViewContract.TenantId > AppConsts.NONE || CurrentViewContext.ViewContract.AssignToDepartmentId > AppConsts.NONE || ((!CurrentViewContext.ViewContract.AssignToRoleId.IsNullOrEmpty()) && IsSysXAdmin))
                        // MB: //if (CurrentViewContext.ViewContract.IsComingThroughTenant || CurrentViewContext.ViewContract.TenantId > AppConsts.NONE || CurrentViewContext.ViewContract.AssignToDepartmentId > AppConsts.NONE || ((!CurrentViewContext.ViewContract.AssignToRoleId.IsNullOrEmpty()) && IsSysXAdmin))
                        {
                            ddlOrg.SelectedValue = Convert.ToString(CurrentViewContext.ViewContract.OrganizationId);
                            HandleUserDetailSection(gridEditFormItem, CurrentViewContext.ViewContract.OrganizationId);
                            ddlOrg.Enabled = false;
                        }
                        else if (e.Item is GridEditFormInsertItem)
                        {
                            //Modified code to set Organization dropdown value and to handle user detail section accrodingly
                            if (IsSysXAdmin)
                            {
                                HandleUserDetailSection(gridEditFormItem, CurrentViewContext.ViewContract.OrganizationId);
                                ddlOrg.Enabled = true;
                            }
                            else
                            {
                                ddlOrg.SelectedValue = Convert.ToString(CurrentViewContext.ViewContract.OrganizationId);
                                HandleUserDetailSection(gridEditFormItem, CurrentViewContext.ViewContract.OrganizationId);
                                ddlOrg.Enabled = false;
                            }
                        }
                        else
                        {
                            OrganizationUserContract organizationUser = (e.Item).DataItem as OrganizationUserContract;

                            if (!organizationUser.IsNull())
                            {
                                ddlOrg.SelectedValue = Convert.ToString(organizationUser.OrganizationID);
                                HandleUserDetailSection(gridEditFormItem, organizationUser.OrganizationID);
                            }

                            ddlOrg.Enabled = false;
                        }

                        //UAT-2257
                        HtmlGenericControl dvClientAdmin = gridEditFormItem.FindControl("dvClientAdmin") as HtmlGenericControl;
                        if (!ddlOrg.SelectedValue.IsNullOrEmpty() && ddlOrg.SelectedValue != AppConsts.ZERO && ddlOrg.SelectedValue != SecurityManager.DefaultTenantID.ToString())
                        {
                            CurrentViewContext.SelectedTenantId = Convert.ToInt32(CurrentViewContext.AllOrganization.Where(con => con.OrganizationID == Convert.ToInt32(ddlOrg.SelectedValue)).Select(con => con.TenantID).FirstOrDefault());
                            Presenter.GetAdminUserByTenantId(CurrentViewContext.SelectedTenantId);
                            if (!CurrentViewContext.lstClientAdminUsers.IsNullOrEmpty())
                            {
                                cmbClientAdmin.Items.Clear();
                                cmbClientAdmin.DataSource = CurrentViewContext.lstClientAdminUsers;
                                cmbClientAdmin.DataBind();
                                cmbClientAdmin.SelectedIndex = AppConsts.NONE;
                            }
                            OrganizationUserContract organizationUser = (e.Item).DataItem as OrganizationUserContract;

                            if (!organizationUser.IsNull() && (organizationUser.IsApplicant || organizationUser.IsSharedUser))
                            {
                                dvClientAdmin.Visible = false;
                            }
                        }
                        else
                        {
                            dvClientAdmin.Visible = false;
                        }
                    }

                    //bind UserName Prefix value
                    if (!cmbUsernamePrefix.IsNull())
                    {
                        cmbUsernamePrefix.Text = String.Empty;
                        Presenter.RetrievingOrganizationUserNamePrefix();
                        if (!CurrentViewContext.AllUserNamePrefix.IsNull())
                        {
                            //insert none in list
                            OrganizationUserNamePrefix usernameprefix = new OrganizationUserNamePrefix
                            {
                                UserNamePrefix = AppConsts.COMBOBOX_ITEM_NONE,
                                OrganizationUserNamePrefixID = AppConsts.NONE
                            };

                            CurrentViewContext.AllUserNamePrefix.Insert(AppConsts.NONE, usernameprefix);
                            cmbUsernamePrefix.DataSource = CurrentViewContext.AllUserNamePrefix;
                            cmbUsernamePrefix.DataBind();
                        }
                    }

                    //validator.ErrorMessage = SysXUtils.GetMessage(CurrentViewContext.IsAdmin ? ResourceConst.SECURITY_ORGANIZATION_REQUIRED : ResourceConst.SECURITY_DEPARTMENT_REQUIRED);
                    validator.ErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_ORGANIZATION_REQUIRED);
                    revPasswordValidator.ErrorMessage = Resources.SysXMessages.ResourceManager.GetString(ResourceConst.SECURITY_PASSWORD_LENGTH);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Event handler. Called by grmUsrList for item data bound events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grmUsrList_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    OrganizationUserContract organizationUser = (OrganizationUserContract)e.Item.DataItem;

                    GridDataItem dataItem = (GridDataItem)e.Item;
                    //UAT-2447
                    //dataItem["MobileAlias"].Text = Presenter.GetFormattedPhoneNumber(Convert.ToString(dataItem["MobileAlias"].Text));

                    HtmlAnchor anchorRole = (HtmlAnchor)e.Item.FindControl("ancRole");
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "UserId", Convert.ToString( organizationUser.UserID) }, //Convert.ToString( organizationUser.aspnet_Users.UserId) },
                                                                   { "OrgUserId", Convert.ToString( organizationUser.OrganizationUserID) },
                                                                    { "Child", ChildControls.SecurityMapUserRole},
                                                                    { "ParentControlName", ChildControls.SecurityManageUser}
                                                                 };

                    anchorRole.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());

                    HtmlAnchor ancInstitution = (HtmlAnchor)e.Item.FindControl("ancInstitution");
                    Dictionary<String, String> queryString1 = new Dictionary<String, String>
                                                                 { 
                                                                    { "UserId", Convert.ToString( organizationUser.UserID) }, //Convert.ToString( organizationUser.aspnet_Users.UserId) },
                                                                    { "OrgUserId", Convert.ToString( organizationUser.OrganizationUserID) },
                                                                    { "Child", ChildControls.SecurityMapUserInstitution},
                                                                    { "ParentControlName", ChildControls.SecurityManageUser}
                                                                 };

                    ancInstitution.HRef = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString1.ToEncryptedQueryString());

                    CurrentViewContext.ViewContract.CreatedById = organizationUser.CreatedByID;

                    if (organizationUser.IsSystem)
                    {
                        e.Item.FindControl("ancRole").Visible = false;
                        (e.Item as GridEditableItem)["ManageRoles"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                        (e.Item as GridEditableItem)["DeleteColumn"].Controls[AppConsts.NONE].Visible = false;
                        (e.Item as GridEditableItem)["DeleteColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                        (e.Item as GridEditableItem)["EditCommandColumn"].Controls[AppConsts.NONE].Visible = false;
                        (e.Item as GridEditableItem)["EditCommandColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                    }

                    if ((organizationUser.IsApplicant == true) || (organizationUser.TenantID == SecurityManager.DefaultTenantID) || organizationUser.IsSystem || !(organizationUser.TenantTypeCode.Equals(TenantType.Institution.GetStringValue()))) //null expection occurred here temp commented for UAT 1204
                    {
                        (e.Item as GridEditableItem)["MapInstitution"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                        ancInstitution.Visible = false;
                    }

                    if (Convert.ToString(organizationUser.LastActivityDate).Equals(Convert.ToString(DateTime.MaxValue)))  //aspnet_Users.LastActivityDate
                    {
                        (e.Item as GridEditableItem)["Activity"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                    }

                    #region Code commented by Vipin
                    //Code Added by Vipin for displaying "Manage Queue" link
                    //If TenantType equals to "Employee" then only show the ManageQueue link
                    //HyperLink hlManageQueue = (e.Item.FindControl("hlManageQueue") as HyperLink);

                    //if (!organizationUser.Organization.IsNull())
                    //{
                    //    if (!organizationUser.Organization.Tenant.IsNull())
                    //    {
                    //        if (!organizationUser.Organization.Tenant.lkpTenantType.IsNull())
                    //        {
                    //            if (organizationUser.Organization.Tenant.lkpTenantType.TenantTypeCode.Equals(TenantType.Employee.GetStringValue()))
                    //            {
                    //                Dictionary<String, String> manageQueueQueryString = new Dictionary<String, String>
                    //                                             { 
                    //                                                { "UserId", Convert.ToString(organizationUser.aspnet_Users.UserId) },
                    //                                                { "Child", @"UserControl\ManageQueue.ascx"},
                    //                                                {"PageSource","ManageQueue"},
                    //                                             };
                    //                hlManageQueue.Visible = true;
                    //                hlManageQueue.NavigateUrl = String.Format("~/Queues/Default.aspx?ucid{0}&args={1}", _viewType, manageQueueQueryString.ToEncryptedQueryString());
                    //            }
                    //            else
                    //            {
                    //                hlManageQueue.Visible = false;
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                }

                if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
                {
                    GridEditFormItem editFormItem = (GridEditFormItem)e.Item;
                    WclTextBox userName = editFormItem.FindControl("txtUserName") as WclTextBox;

                    if (!userName.IsNull())
                    {
                        String clickedInUserId = userName.Text;

                        if (base.SysXMembershipUser.UserName.Equals(clickedInUserId))
                        {
                            IsActiveToggle chkActive = (IsActiveToggle)editFormItem.FindControl("chkActive");
                            chkActive.IsActiveEnable = false;
                        }
                    }

                    CurrentViewContext.ResetUserName = userName.Text;

                    #region UAT-2447
                    var organizationUserData = e.Item.DataItem as OrganizationUserContract;
                    var gridItem = e.Item;
                    if (!organizationUserData.IsNullOrEmpty())
                    {
                        (gridItem.FindControl("chkIsMaskingRequired") as WclCheckBox).Checked = organizationUserData.IsInternationalPhoneNumber;
                        ShowHidePhoneControls(organizationUserData.IsInternationalPhoneNumber, e.Item as GridEditFormItem);
                    }
                    else
                    {
                        (gridItem.FindControl("dvMasking") as HtmlControl).Style["display"] = "block";
                        (gridItem.FindControl("dvUnmasking") as HtmlControl).Style["display"] = "none";
                        (gridItem.FindControl("rfvTxtMobile") as RequiredFieldValidator).Enabled = true;
                        (gridItem.FindControl("revTxtMobile") as RegularExpressionValidator).Enabled = true;
                        (gridItem.FindControl("rfvTxtMobilePrmyNonMasking") as RequiredFieldValidator).Enabled = false;
                        (gridItem.FindControl("revTxtMobilePrmyNonMasking") as RegularExpressionValidator).Enabled = false;
                    }
                    #endregion
                }
                if (!(e.Item is GridEditFormInsertItem) && e.Item.IsInEditMode)
                {
                    WclTextBox userName = e.Item.FindControl("txtUserName") as WclTextBox;
                    RegularExpressionValidator revUserName = e.Item.FindControl("revUserName") as RegularExpressionValidator;
                    revUserName.Enabled = false;
                    //UAT-2199:As an adb admin, I should be able to edit admin usernames on the manage users screen.
                    userName.Enabled = true;

                    // Reset password button visibility false in case if User is already locked.
                    GridEditFormItem editFormItem = (GridEditFormItem)e.Item;
                    CheckBox chkUnlockUser = (CheckBox)editFormItem.FindControl("chkUnlockUser");

                    if (chkUnlockUser.Checked)
                    {
                        CommandBar commandBar = (e.Item.FindControl("fsucCmdBarMUser") as CommandBar);

                        if (!commandBar.IsNull())
                        {
                            commandBar.ExtraButton.Visible = false;
                        }
                    }

                    //UAT-985: WB: Simplification of Admin Account creation.
                    //Hide passwords sections in edit mode
                    HtmlGenericControl divPasswordSection = editFormItem.FindControl("divPasswordSection") as HtmlGenericControl;
                    HtmlGenericControl divChangePasswordUponFirstLogin = editFormItem.FindControl("divChangePasswordUponFirstLogin") as HtmlGenericControl;
                    divPasswordSection.Visible = divChangePasswordUponFirstLogin.Visible = false;
                }

                #region ClientOnBoardingWizard

                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    if (CurrentViewContext.IsClientOnBoardingWizard || CurrentViewContext.IsClientProfile)
                    {
                        ((GridColumn)e.Item.OwnerTableView.Columns.FindByUniqueName("ManageRoles")).Visible = false;
                        ((GridColumn)e.Item.OwnerTableView.Columns.FindByUniqueName("ManageRolesWiz")).Visible = true;
                    }
                }

                #endregion
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Button Related Event

        /// <summary>
        /// Reset Password.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.Password = radCpatchaPassword.CaptchaImage.Text;

                Presenter.ResetPassword();
                var masterPage = Page.Master;

                if (!masterPage.IsNull())
                {
                    Label lblError = (Label)masterPage.FindControl("lblError");

                    Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                              {
                                                                  //{"FirstName", CurrentViewContext.ViewContract.FirstName},
                                                                  //{"LastName", CurrentViewContext.ViewContract.LastName},
                                                                  //{"Password", CurrentViewContext.ViewContract.Password}
                                                                  {EmailFieldConstants.USER_FULL_NAME,CurrentViewContext.ViewContract.FirstName + " " + CurrentViewContext.ViewContract.LastName},
                                                                  {EmailFieldConstants.PASSWORD,CurrentViewContext.ViewContract.Password},
                                                                  {EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID,CurrentViewContext.ViewContract.OrganizationUserId},
                                                                  { EmailFieldConstants.INSTITUTION_URL,Presenter.GetInstitutionURL()}  // UAT- 4306
                                                              };
                    SecurityManager.PrepareAndSendSystemMail(CurrentViewContext.ViewContract.EmailAddress, contents, CommunicationSubEvents.NOTIFICATION_PASSWORD_RESET_BY_ADMIN, null, true);
                    Boolean mailStatus = true;
                    //SysXEmailService.SendMail(contents, String.Format("ADB password reset request Details for {0}  {1}", CurrentViewContext.ViewContract.FirstName, CurrentViewContext.ViewContract.LastName), CurrentViewContext.ViewContract.EmailAddress, AppConsts.PasswordReset, AppConsts.Normal);
                    lblError.Text = mailStatus ? SysXUtils.GetMessage(ResourceConst.SECURITY_PASSWORD_SEND_SUCEESFULLY) : SysXUtils.GetMessage(ResourceConst.SECURITY_FAILED_TO_SEND_EMAIL);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// On Change of the Organization dropdown, decision is made for the username. If the organization is of type Supplier, email address will
        /// be used as a username, else username field is used a username with the prefix selected befor it.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected void cmbOrganization_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Int32 selectedOrganizationId = Convert.ToInt32(e.Value);
            Organization SelectedOrganization = CurrentViewContext.AllOrganization.Where(con => con.OrganizationID == selectedOrganizationId).FirstOrDefault();
            CurrentViewContext.SelectedTenantId = Convert.ToInt32(SelectedOrganization.TenantID);

            WclComboBox cmbOrganization = sender as WclComboBox;

            if (cmbOrganization.IsNotNull())
            {
                //UAT-2257
                HtmlGenericControl dvClientAdmin = cmbOrganization.Parent.NamingContainer.FindControl("dvClientAdmin") as HtmlGenericControl;
                if (selectedOrganizationId == SecurityManager.DefaultTenantID || selectedOrganizationId == AppConsts.NONE)
                {
                    dvClientAdmin.Visible = false;
                }
                else
                {
                    dvClientAdmin.Visible = true;
                    Presenter.GetAdminUserByTenantId(CurrentViewContext.SelectedTenantId);
                    WclComboBox cmbClientAdmins = (WclComboBox)cmbOrganization.Parent.NamingContainer.FindControl("cmbClientAdmin");
                    cmbClientAdmins.Items.Clear();
                    cmbClientAdmins.DataSource = CurrentViewContext.lstClientAdminUsers.ToList();
                    cmbClientAdmins.DataBind();
                    cmbClientAdmins.SelectedIndex = AppConsts.NONE;

                }
                Label lblEmailAddress = cmbOrganization.Parent.NamingContainer.FindControl("lblEmailAddress") as Label;
                var divUserNameSection = cmbOrganization.Parent.NamingContainer.FindControl("divUserNameSection");
                var divEmailSection = cmbOrganization.Parent.NamingContainer.FindControl("divEmailSection");
                var divUserMain = cmbOrganization.Parent.NamingContainer.FindControl("divUserMain");
                //UAT-985: WB: Simplification of Admin Account creation.
                var divPasswordSection = cmbOrganization.Parent.NamingContainer.FindControl("divPasswordSection");
                var divChangePasswordUponFirstLogin = cmbOrganization.Parent.NamingContainer.FindControl("divChangePasswordUponFirstLogin");
                WclComboBox cmbUsernamePrefix = (WclComboBox)cmbOrganization.Parent.NamingContainer.FindControl("cmbUsernamePrefix");

                //UAT-2447
                Boolean chkIsMaskingRequired = (cmbOrganization.Parent.NamingContainer.FindControl("chkIsMaskingRequired") as WclCheckBox).Checked;
                ShowHidePhoneControls(chkIsMaskingRequired, cmbOrganization.Parent.NamingContainer as GridEditFormItem);

                // If the Organization is of Supplier type, need to display only  the email address field.
                if (cmbOrganization.SelectedValue.Equals(AppConsts.ZERO))
                {
                    //Added code to hide email, username, main, password sections
                    //UAT-985: WB: Simplification of Admin Account creation. Added password sections
                    divEmailSection.Visible = divUserNameSection.Visible = divUserMain.Visible = divPasswordSection.Visible =
                        divChangePasswordUponFirstLogin.Visible = false;
                }
                else
                {
                    //bind UserName Prefix value
                    if (cmbUsernamePrefix.IsNotNull())
                    {
                        cmbUsernamePrefix.Text = String.Empty;
                        CurrentViewContext.ViewContract.OrganizationId = selectedOrganizationId;
                        Presenter.RetrievingOrganizationUserNamePrefix();
                        if (CurrentViewContext.AllUserNamePrefix.IsNotNull())
                        {
                            //insert none in list
                            OrganizationUserNamePrefix usernameprefix = new OrganizationUserNamePrefix
                            {
                                UserNamePrefix = AppConsts.COMBOBOX_ITEM_NONE,
                                OrganizationUserNamePrefixID = AppConsts.NONE
                            };
                            CurrentViewContext.AllUserNamePrefix.Insert(AppConsts.NONE, usernameprefix);

                            cmbUsernamePrefix.DataSource = CurrentViewContext.AllUserNamePrefix;
                            cmbUsernamePrefix.DataBind();
                        }
                    }

                    CurrentViewContext.ViewContract.IsSupplier = Presenter.IsOrganizationOfSupplier(selectedOrganizationId);
                    if (CurrentViewContext.ViewContract.IsSupplier)
                    {
                        divUserNameSection.Visible = false;
                        //UAT-985: WB: Simplification of Admin Account creation. Added password sections
                        divUserMain.Visible = divEmailSection.Visible = divPasswordSection.Visible = divChangePasswordUponFirstLogin.Visible = true;

                        if (lblEmailAddress != null)
                        {
                            lblEmailAddress.Text = @"Username/Email Address";
                            (cmbOrganization.Parent.NamingContainer.FindControl("txtUserName") as WclTextBox).Text = String.Empty;
                        }
                    }
                    else
                    {
                        //UAT-985: WB: Simplification of Admin Account creation. Added password sections
                        divEmailSection.Visible = divUserNameSection.Visible = divUserMain.Visible =
                            divPasswordSection.Visible = divChangePasswordUponFirstLogin.Visible = true;

                        if (lblEmailAddress != null)
                        {
                            lblEmailAddress.Text = @"Email Address";
                        }
                    }
                }
                WclMaskedTextBox maskTxtMobile = (sender as WclComboBox).NamingContainer.FindControl("maskTxtMobile") as WclMaskedTextBox;
                if (maskTxtMobile.IsNotNull())
                {
                    maskTxtMobile.Focus();
                }
            }
        }


        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public void SetFilterValues()
        {
            if (!CurrentViewContext.ViewContract.FilterColumns.IsNullOrEmpty() && CurrentViewContext.ViewContract.FilterColumns.Count > 0)
            {
                CurrentViewContext.ViewContract.FilterColumns.ForEach(x =>
                    grmUsrList.Columns.FindByUniqueName(x).CurrentFilterValue = CurrentViewContext.ViewContract.FilterValues[CurrentViewContext.ViewContract.FilterColumns.IndexOf(x)].ToString()
                    );
            }
        }

        //public void ClearViewStatesForFilter()
        //{
        //    ViewState["FilterColumns"] = null;
        //    ViewState["FilterOperators"] = null;
        //    ViewState["FilterPair"] = null;
        //    ViewState["FilterValues"] = null;
        //    ViewState["FilterTypes"] = null;
        //}

        #endregion

        #region Private Method

        private void HandleUserDetailSection(GridEditFormItem gridEditFormItem, Int32 organizationId)
        {
            var lblEmailAddress = gridEditFormItem.FindControl("lblEmailAddress") as Label;
            var divUserNameSection = gridEditFormItem.FindControl("divUserNameSection");
            var divEmailSection = gridEditFormItem.FindControl("divEmailSection");
            var divUserMain = gridEditFormItem.FindControl("divUserMain");
            //UAT-985: WB: Simplification of Admin Account creation. Added password sections
            var divPasswordSection = gridEditFormItem.FindControl("divPasswordSection");
            var divChangePasswordUponFirstLogin = gridEditFormItem.FindControl("divChangePasswordUponFirstLogin");

            // If the Organization is of Supplier type, need to display only  the email address field.
            if (organizationId.Equals(AppConsts.NONE))
            {
                //Added code to hide email, username, main and password sections
                //UAT-985: WB: Simplification of Admin Account creation. Added password sections
                divEmailSection.Visible = divUserNameSection.Visible = divUserMain.Visible =
                    divPasswordSection.Visible = divChangePasswordUponFirstLogin.Visible = false;
            }
            else
            {
                if (Presenter.IsOrganizationOfSupplier(organizationId))
                {
                    divUserNameSection.Visible = false;
                    divUserMain.Visible = true;
                    //UAT-985: WB: Simplification of Admin Account creation. Added password sections
                    divEmailSection.Visible = divPasswordSection.Visible = divChangePasswordUponFirstLogin.Visible = true;

                    if (lblEmailAddress != null)
                    {
                        lblEmailAddress.Text = @"Username/Email Address";
                    }
                }
                else
                {
                    divUserMain.Visible = true;
                    divEmailSection.Visible = true;
                    //UAT-985: WB: Simplification of Admin Account creation. Added password sections
                    divUserNameSection.Visible = divPasswordSection.Visible = divChangePasswordUponFirstLogin.Visible = true;

                    if (lblEmailAddress != null)
                    {
                        lblEmailAddress.Text = @"Email Address";
                    }
                }
            }
        }

        private void FilterGridColumn()
        {
            if (!ViewState["SortExpression"].IsNull())
            {
                CurrentViewContext.ManageUserCustomPaging.SortExpression = Convert.ToString(ViewState["SortExpression"]);
                CurrentViewContext.ManageUserCustomPaging.SortDirectionDescending = Convert.ToBoolean(ViewState["SortDirection"]);
            }
            CurrentViewContext.ViewContract.FilterColumns = ViewState["FilterColumns"] == null ? new List<String>() : (List<String>)(ViewState["FilterColumns"]);
            CurrentViewContext.ViewContract.FilterOperators = ViewState["FilterOperators"] == null ? new List<String>() : (List<String>)(ViewState["FilterOperators"]);
            CurrentViewContext.ViewContract.FilterValues = ViewState["FilterValues"] == null ? new ArrayList() : (ArrayList)(ViewState["FilterValues"]);
            CurrentViewContext.ViewContract.FilterTypes = ViewState["FilterTypes"] == null ? new List<String>() : (List<String>)(ViewState["FilterTypes"]);

            if (ViewState["FilterPair"] != null)
            {
                Pair filter = (Pair)ViewState["FilterPair"];
                Int32 filterIndex = CurrentViewContext.ViewContract.FilterColumns.IndexOf(filter.Second.ToString());
                String filterValue = grmUsrList.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).CurrentFilterValue;
                if (filter.First.ToString() != GridKnownFunction.NoFilter.ToString()) //&& ActionType != ViewMode.Search.ToString()
                {
                    String filterTypes = grmUsrList.MasterTableView.Columns.FindByUniqueName(filter.Second.ToString()).DataTypeName;
                    if (filterIndex != -1)
                    {
                        CurrentViewContext.ViewContract.FilterOperators[filterIndex] = filter.First.ToString();
                        CurrentViewContext.ViewContract.FilterTypes[filterIndex] = filterTypes.ToString();
                        if (filterTypes == "System.Decimal")
                        {
                            CurrentViewContext.ViewContract.FilterValues[filterIndex] = Convert.ToDecimal(filterValue);
                        }
                        else if (filterTypes == "System.Int32")
                        {
                            CurrentViewContext.ViewContract.FilterValues[filterIndex] = Convert.ToInt32(filterValue);
                        }
                        else if (filterTypes == "System.DateTime")
                        {
                            if (!filterValue.IsNullOrEmpty())
                            {
                                try
                                {
                                    //try to convert any value to date
                                    CurrentViewContext.ViewContract.FilterValues[filterIndex] = Convert.ToDateTime(filterValue);
                                }
                                catch
                                {
                                    //date filter value could not be converted, set filter value to any default date
                                    CurrentViewContext.ViewContract.FilterValues[filterIndex] = Convert.ToDateTime("01/01/1901");
                                    //return;
                                }
                            }

                            //To set IsNull filter to other Date format filter and set to any default date in case of Null date
                            if (CurrentViewContext.ViewContract.FilterOperators.Contains("IsNull"))
                            {
                                CurrentViewContext.ViewContract.FilterOperators[filterIndex] = "NullOtherThanString";
                                CurrentViewContext.ViewContract.FilterValues[filterIndex] = Convert.ToDateTime("01/01/1901");
                            }
                        }
                        else
                        {
                            CurrentViewContext.ViewContract.FilterValues[filterIndex] = filterValue;
                        }
                    }
                    else
                    {
                        CurrentViewContext.ViewContract.FilterColumns.Add(filter.Second.ToString());
                        CurrentViewContext.ViewContract.FilterOperators.Add(filter.First.ToString());
                        CurrentViewContext.ViewContract.FilterTypes.Add(filterTypes.ToString());
                        if (filterTypes == "System.Decimal")
                        {
                            CurrentViewContext.ViewContract.FilterValues.Add(Convert.ToDecimal(filterValue));
                        }
                        else if (filterTypes == "System.Int32")
                        {
                            CurrentViewContext.ViewContract.FilterValues.Add(Convert.ToInt32(filterValue));
                        }
                        else if (filterTypes == "System.DateTime")
                        {
                            if (!filterValue.IsNullOrEmpty())
                            {
                                try
                                {
                                    //try to convert any value to date
                                    CurrentViewContext.ViewContract.FilterValues.Add(Convert.ToDateTime(filterValue));
                                }
                                catch
                                {
                                    //date filter value could not be converted, set filter value to any default date
                                    CurrentViewContext.ViewContract.FilterValues.Add(Convert.ToDateTime("01/01/1901"));
                                    //return;
                                }
                            }

                            //To set IsNull filter to other Date format filter and set to any default date in case of Null date
                            if (CurrentViewContext.ViewContract.FilterOperators.Contains("IsNull"))
                            {
                                Int32 index = CurrentViewContext.ViewContract.FilterOperators.IndexOf("IsNull");
                                CurrentViewContext.ViewContract.FilterOperators[index] = "NullOtherThanString";
                                CurrentViewContext.ViewContract.FilterValues.Add(Convert.ToDateTime("01/01/1901"));
                            }
                        }
                        else
                        {
                            CurrentViewContext.ViewContract.FilterValues.Add(filterValue);
                        }
                    }
                }
                else if (filterIndex != -1)
                {
                    CurrentViewContext.ViewContract.FilterOperators.RemoveAt(filterIndex);
                    CurrentViewContext.ViewContract.FilterValues.RemoveAt(filterIndex);
                    CurrentViewContext.ViewContract.FilterColumns.RemoveAt(filterIndex);
                    CurrentViewContext.ViewContract.FilterTypes.RemoveAt(filterIndex);
                }

                ViewState["FilterColumns"] = CurrentViewContext.ViewContract.FilterColumns;
                ViewState["FilterOperators"] = CurrentViewContext.ViewContract.FilterOperators;
                ViewState["FilterValues"] = CurrentViewContext.ViewContract.FilterValues;
                ViewState["FilterTypes"] = CurrentViewContext.ViewContract.FilterTypes;
                ViewState["FilterPair"] = null;
            }
        }

        #endregion

        protected void grmUsrList_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    ViewState["SortExpression"] = e.SortExpression;
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.ManageUserCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.ManageUserCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    ViewState["SortExpression"] = String.Empty;
                    ViewState["SortDirection"] = false;
                    CurrentViewContext.ManageUserCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.ManageUserCustomPaging.SortDirectionDescending = false;
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

        #endregion

        protected void cmbClientAdmin_DataBound(object sender, EventArgs e)
        {
            try
            {
                WclComboBox cmbClientAdmin = sender as WclComboBox;
                cmbClientAdmin.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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
        #region UAT-2447
        protected void chkIsMaskingRequired_CheckedChanged(object sender, EventArgs e)
        {
            var chkIsMaskingRequired = sender as WclCheckBox;
            ShowHidePhoneControls(chkIsMaskingRequired.Checked, chkIsMaskingRequired.Parent.NamingContainer as GridEditFormItem);
        }

        private void ShowHidePhoneControls(Boolean IsInternationalNumber, GridEditFormItem pnlControl)
        {
            if (IsInternationalNumber)
            {
                (pnlControl.FindControl("dvMasking") as HtmlControl).Style["display"] = "none";
                (pnlControl.FindControl("dvUnmasking") as HtmlControl).Style["display"] = "block";
                (pnlControl.FindControl("rfvTxtMobile") as RequiredFieldValidator).Enabled = false;
                (pnlControl.FindControl("revTxtMobile") as RegularExpressionValidator).Enabled = false;
                (pnlControl.FindControl("rfvTxtMobilePrmyNonMasking") as RequiredFieldValidator).Enabled = true;
                (pnlControl.FindControl("revTxtMobilePrmyNonMasking") as RegularExpressionValidator).Enabled = true;
            }
            else
            {
                (pnlControl.FindControl("dvMasking") as HtmlControl).Style["display"] = "block";
                (pnlControl.FindControl("dvUnmasking") as HtmlControl).Style["display"] = "none";
                (pnlControl.FindControl("rfvTxtMobile") as RequiredFieldValidator).Enabled = true;
                (pnlControl.FindControl("revTxtMobile") as RegularExpressionValidator).Enabled = true;
                (pnlControl.FindControl("rfvTxtMobilePrmyNonMasking") as RequiredFieldValidator).Enabled = false;
                (pnlControl.FindControl("revTxtMobilePrmyNonMasking") as RegularExpressionValidator).Enabled = false;
            }
        }

        private void CaptureQueryString() //UAT-3394
        {
            Dictionary<String, String> args = new Dictionary<String, String>();

            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
            }

            //ClientOnBoardingWiz: Implementation 14/01/2012
            //CurrentViewContext.ViewContract.TenantId = args.ContainsKey("TenantId") ? Int32.Parse(args["TenantId"]) : AppConsts.NONE;
            if (!CurrentViewContext.IsClientOnBoardingWizard && !CurrentViewContext.IsClientProfile)
            {
                CurrentViewContext.ViewContract.TenantId = args.ContainsKey("TenantId") ? Int32.Parse(args["TenantId"]) : AppConsts.NONE;
            }

            //ClientOnBoardingWiz: Implementation 14/01/2012
            if (CurrentViewContext.IsDataLoad && (!IsPageLoadByClientOnBoardingWizard))
            {
                grmUsrList.Rebind();
                IsPageLoadByClientOnBoardingWizard = true;
            }

            CurrentViewContext.ViewContract.AssignToRoleId = args.ContainsKey("RoleDetailId") ? Convert.ToString(args["RoleDetailId"]) : String.Empty;
            CurrentViewContext.ViewContract.AssignToRoleName = new[] { args.ContainsKey("RoleDetailName") ? Convert.ToString(args["RoleDetailName"]) : String.Empty };
            CurrentViewContext.ViewContract.AssignToProductId = args.ContainsKey("ProductID") ? Convert.ToInt32(args["ProductID"]) : AppConsts.NONE;
            CurrentViewContext.ViewContract.AssignToDepartmentId = args.ContainsKey("AssignToDepartmentId") ? Convert.ToInt32(args["AssignToDepartmentId"]) : AppConsts.NONE;
            CurrentViewContext.ViewContract.IsLinkOnTenant = args.ContainsKey("IsLinkOnTenant") && args["IsLinkOnTenant"].Equals("yes", StringComparison.InvariantCultureIgnoreCase);
            CurrentViewContext.ViewContract.IsComingThroughTenant = args.ContainsKey("IsComingThroughTenant") && args["IsComingThroughTenant"].Equals("yes", StringComparison.InvariantCultureIgnoreCase);


            Presenter.GetTenantName();

            if (!CurrentViewContext.ViewContract.TenantName.IsNullOrEmpty())
            {
                lblManageTenantSuffix.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_FOR) + CurrentViewContext.ViewContract.TenantName.HtmlEncode();
            }

            String roleName = args.ContainsKey("RoleDetailDescription") ? args["RoleDetailDescription"] : String.Empty;
            lblManageUser.Text = String.IsNullOrEmpty(roleName) ? base.Title : roleName + "&nbsp;>&nbsp;" + base.Title;

            base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_USERS));
            lblSuccess.Visible = false;
            lblSuccess.Text = String.Empty;

        }


        #endregion

        #region UAT-3360
        private void SetSessionValues(ManageUsersContract manageUsersContract, String userName, String email)
        {
            AccountLinkingProfileContract accountLinkingProfileContract = new AccountLinkingProfileContract();
            accountLinkingProfileContract.ManageUsersContract = manageUsersContract;
            accountLinkingProfileContract.Username = userName;
            accountLinkingProfileContract.UserEmail = email;
            accountLinkingProfileContract.CopyFromClientAdminOrgID = CurrentViewContext.CopyFromClientAdminOrgID;
            accountLinkingProfileContract.CopyFromClientAdminUserID = CurrentViewContext.CopyFromClientAdminUserID;
            //Session for maintaining control values
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ACCOUNT_LINKING_SESSION_KEY, accountLinkingProfileContract);
        }
        private AccountLinkingProfileContract GetSessionValues()
        {
            AccountLinkingProfileContract accountLinkingProfileContract = new AccountLinkingProfileContract();
            var res = (AccountLinkingProfileContract)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.ACCOUNT_LINKING_SESSION_KEY);
            if (!res.IsNullOrEmpty())
            {
                accountLinkingProfileContract = res;
            }
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ACCOUNT_LINKING_SESSION_KEY, null);
            return accountLinkingProfileContract;
        }
        protected void AccountLinkingPostback_Click(object sender, EventArgs e)
        {
            RadGrid grid = (this.FindControl("grmUsrList") as RadGrid);

            if (grid.MasterTableView.IsItemInserted)
            {
                (grid.MasterTableView.GetItems(GridItemType.CommandItem)[0] as GridCommandItem).FireCommandEvent(RadGrid.PerformInsertCommandName, string.Empty);
                //  GridDataInsertItem insertItem = (GridDataInsertItem)grid.MasterTableView.GetInsertItem(); // accessing grid insert item
                //  insertItem.FireCommandEvent("PerformInsert", string.Empty);
            }
            else if (grid.EditItems.Count > 0)
            {
                grid.EditItems[0].FireCommandEvent(RadGrid.UpdateCommandName, string.Empty);
            }
            //else if(your condition for deletion)
            //{
            //    (grid.MasterTableView.GetItems(GridItemType.Item)[0] as GridDataItem).FireCommandEvent(RadGrid.DeleteCommandName, string.Empty);
            //}
        }
        #endregion
    }
}
