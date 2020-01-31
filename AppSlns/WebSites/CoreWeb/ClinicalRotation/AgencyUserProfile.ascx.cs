using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.ProfileSharing;
using Entity.SharedDataEntity;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class AgencyUserProfile : BaseUserControl, IAgencyUserProfileView
    {

        #region VARIABLES

        #region PRIVATE VARIABLES

        private AgencyUserProfilePresenter _presenter = new AgencyUserProfilePresenter();
        private Int32 tenantId = 0;

        #endregion

        #endregion

        #region PROPERTIES

        public AgencyUserProfilePresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public IAgencyUserProfileView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IAgencyUserProfileView.TenantID
        {
            get
            {
                if (tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantId;
            }
            set { tenantId = value; }
        }

        Int32 IAgencyUserProfileView.CurrentLoggedInUserID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AgencyViewAdminOrgUsrID"]);
                }
                else
                {
                    return SysXWebSiteUtils.SessionService.OrganizationUserId;
                }
            }
        }

        Int32 IAgencyUserProfileView.OrganisationUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Guid IAgencyUserProfileView.UserID
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                return user.UserId;
            }
        }

        Boolean IAgencyUserProfileView.SuccessMsg
        {
            get;
            set;
        }

        OrganizationUserContract IAgencyUserProfileView.OrganizationUser
        {
            get;
            set;
        }

        AgencyUserContract IAgencyUserProfileView.AgencyUserDetails
        {
            get;
            set;
        }

        List<lkpInvitationSharedInfoType> IAgencyUserProfileView.LstSharedInfoType
        {
            get;
            set;
        }

        Boolean IAgencyUserProfileView.IsMasterAgencyUser
        {
            get
            {
                if (!ViewState["IsMasterAgencyUser"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsMasterAgencyUser"]);
                }
                return false;
            }
            set
            {
                ViewState["IsMasterAgencyUser"] = value;
            }
        }

        Boolean IAgencyUserProfileView.IsApplicantsSharedUser
        {
            get
            {
                if (!ViewState["IsApplicantsSharedUser"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsApplicantsSharedUser"]);
                }
                return false;
            }
            set
            {
                ViewState["IsApplicantsSharedUser"] = value;
            }
        }

        #endregion

        #region EVENTS

        #region PAGE EVENTS

        protected override void OnInit(EventArgs e)
        {
            try
            {
                Session["BreadCrumb"] = null;
                base.OnInit(e);
                base.Title = "Agency User Profile";
                base.SetPageTitle("Agency User Profile");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CaptureQuerystring();
                    BindControls();
                }
                BasePage page = (base.Page) as BasePage;
                page.SetModuleTitle("Security");
                //UAT-4475
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = RedirectToChangePassword(queryString);
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

        #region UAT-4475

        /// <summary>
        /// Used for open the change password page.
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        private Dictionary<string, string> RedirectToChangePassword(Dictionary<String, String> queryString)
        {
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", @"~/IntsofSecurityModel/ChangePassword.ascx"},
                                                                    //{"PageType",PageType}
                                                                 };            
            lnkChangePassword.NavigateUrl = String.Format("~/IntsofSecurityModel/default.aspx?ucid={0}&args={1}", "AgencyUser", queryString.ToEncryptedQueryString());

            //Adding tooltip for the user profile link only if link is enabled
            if (!string.IsNullOrWhiteSpace(lnkChangePassword.NavigateUrl))
            {
                lnkChangePassword.ToolTip = Resources.Language.CLICKTOCHANGEPASSWORD;
            }

            //Setting iframe as a target 
            lnkChangePassword.Target = "pageFrame";
            return queryString;
        }

        #endregion

        #region CONTROL EVENTS

        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBar_SaveClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.OrganizationUser = new OrganizationUserContract();
                CurrentViewContext.OrganizationUser.FirstName = txtFirstName.Text.Trim();
                CurrentViewContext.OrganizationUser.MiddleName = txtMiddleName.Text.Trim();
                CurrentViewContext.OrganizationUser.LastName = txtLastName.Text.Trim();
                CurrentViewContext.OrganizationUser.OrganizationUserID = CurrentViewContext.CurrentLoggedInUserID;

                if (!CurrentViewContext.IsApplicantsSharedUser && CurrentViewContext.IsMasterAgencyUser)
                {
                    CurrentViewContext.AgencyUserDetails = new AgencyUserContract();
                    CurrentViewContext.AgencyUserDetails.AGU_ComplianceSharedInfoTypeID = Convert.ToInt32(cmbCompliancePermissions.SelectedValue);
                    CurrentViewContext.AgencyUserDetails.AGU_ReqRotationSharedInfoTypeID = Convert.ToInt32(cmbRotationPermissions.SelectedValue);
                    CurrentViewContext.AgencyUserDetails.lstInvitationSharedInfoTypeID = cmbBackgroundPermissions.CheckedItems.Select(x => Convert.ToInt32(x.Value)).ToList();

                }
                Presenter.UpdateAgencyUserDetails();
                base.ShowSuccessMessage("Profile updated successfully.");
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

        protected void fsucCmdBar_CancelClick(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                     { "SelectedTenantID", Convert.ToString(CurrentViewContext.TenantID) },
                                                                    { "Child", ChildControls.SharedUserDashboard}
                                                                 };
                Response.Redirect(String.Format("~/ProfileSharing/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
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

        protected void fsucCmdBar_ClearClick(object sender, EventArgs e)
        {
            try
            {
                String _viewType = "AgencyUser";
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",ChildControls.OtherAccountLinkingNew},
                                                                    {"IsApplicantsSharedUser",CurrentViewContext.IsApplicantsSharedUser.ToString()}
                                                                 };
                string url = String.Format("~/IntsofSecurityModel/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
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

        #region PRIVATE METHODS

        /// <summary>
        /// To bind Controls
        /// </summary>
        private void BindControls()
        {
            Presenter.GetUserData();
            txtFirstName.Text = CurrentViewContext.OrganizationUser.FirstName;
            txtMiddleName.Text = CurrentViewContext.OrganizationUser.MiddleName;
            txtLastName.Text = CurrentViewContext.OrganizationUser.LastName;
            txtEmail.Text = CurrentViewContext.OrganizationUser.Email;

            if (CurrentViewContext.IsApplicantsSharedUser)
            {
                dvPermission.Visible = false;
                dvRotationPermission.Visible = false;
                lblTitle.Text = "Shared User Details";
            }
            else
            {
                lblTitle.Text = "Agency User Details";

                //If master agency user then show/bind permission controls
                CurrentViewContext.IsMasterAgencyUser = CurrentViewContext.AgencyUserDetails.AGU_AgencyUserPermission;
                if (CurrentViewContext.IsMasterAgencyUser)
                {
                    dvPermission.Visible = true;
                    dvRotationPermission.Visible = true;

                    //Bind permission controls
                    BindPermissionControls();
                    cmbCompliancePermissions.SelectedValue = Convert.ToString(CurrentViewContext.AgencyUserDetails.AGU_ComplianceSharedInfoTypeID);
                    cmbRotationPermissions.SelectedValue = Convert.ToString(CurrentViewContext.AgencyUserDetails.AGU_ReqRotationSharedInfoTypeID);

                    List<Int32> lstInvitationSharedInfoTypeID = CurrentViewContext.AgencyUserDetails.lstInvitationSharedInfoTypeID;
                    if (!lstInvitationSharedInfoTypeID.IsNullOrEmpty())
                    {
                        foreach (RadComboBoxItem item in cmbBackgroundPermissions.Items)
                        {
                            if (lstInvitationSharedInfoTypeID.Contains(Convert.ToInt32(item.Value)))
                                item.Checked = true;
                        }
                    }
                }
                else
                {
                    dvPermission.Visible = false;
                    dvRotationPermission.Visible = false;
                }
            }
        }

        /// <summary>
        /// Bind Permission controls
        /// </summary>
        private void BindPermissionControls()
        {
            Presenter.GetSharedInfoType();

            cmbCompliancePermissions.DataSource = CurrentViewContext.LstSharedInfoType.Where(x => x.MasterInfoTypeCode == SharedInfoMasterType.MASTERTYPE_COMPLIANCE.GetStringValue());
            cmbCompliancePermissions.DataBind();

            cmbRotationPermissions.DataSource = CurrentViewContext.LstSharedInfoType.Where(x => x.MasterInfoTypeCode == SharedInfoMasterType.MASTERTYPE_REQUIREMENT_ROTATION.GetStringValue());
            cmbRotationPermissions.DataBind();

            cmbBackgroundPermissions.DataSource = CurrentViewContext.LstSharedInfoType.Where(x => x.MasterInfoTypeCode == SharedInfoMasterType.MASTERTYPE_BACKGROUND.GetStringValue());
            cmbBackgroundPermissions.DataBind();
        }

        /// <summary>
        /// Sets the properties from the arguments recieved through querystring.
        /// </summary>
        /// <param name="args"></param>
        private void CaptureQuerystring()
        {
            var args = new Dictionary<String, String>();
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("IsApplicantsSharedUser"))
            {
                CurrentViewContext.IsApplicantsSharedUser = Convert.ToBoolean(args["IsApplicantsSharedUser"]);
            }

        }

        #endregion
    }
}