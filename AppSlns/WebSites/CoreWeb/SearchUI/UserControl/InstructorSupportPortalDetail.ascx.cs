#region Namespaces

#region SystemDefined

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;
using System.Web;
using INTERSOFT.WEB.UI.WebControls;

#endregion

#region UserDefined
using INTSOF.UI.Contract.SysXSecurityModel;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.Utils;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using System.Configuration;
using Telerik.Web.UI;
#endregion

#endregion

namespace CoreWeb.SearchUI.Views
{
    public partial class InstructorSupportPortalDetail : BaseUserControl, IInstructorSupportPortalDetailView
    {
        #region Variables

        #region Private Variables

        private InstructorSupportPortalDetailPresenter _presenter = new InstructorSupportPortalDetailPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        private String _userId = String.Empty;
        #endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties

        #region Public Properties


        public InstructorSupportPortalDetailPresenter Presenter
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

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        /// <summary>
        /// To set or get Search Instance Id
        /// </summary>
        public Int32 SearchInstanceId
        {
            get
            {
                if (!ViewState["SearchInstanceId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SearchInstanceId"]);
                }
                return 0;
            }
            set
            {
                ViewState["SearchInstanceId"] = value;
            }
        }


        public Int32 OrganizationUserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("OrganizationUserId"))
                    {
                        return (Convert.ToInt32(args["OrganizationUserId"]));
                    }
                }
                return base.CurrentUserId;
            }

            set
            {
                OrganizationUserId = value;
            }
        }

        public Int32 SelectedTenantId
        {
            get
            {
                if (!ViewState["SelectedTenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SelectedTenantId"]);
                }
                return 0;
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        /// <summary>
        /// To set or get Master Page Tab Index
        /// </summary>
        public Int32 MasterPageTabIndex
        {
            get
            {
                if (!ViewState["MasterPageTabIndex"].IsNull())
                {
                    return Convert.ToInt32(ViewState["MasterPageTabIndex"]);
                }
                return 0;
            }
            set
            {
                ViewState["MasterPageTabIndex"] = value;
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (tenantId == 0)
                {
                    //tenantId = Presenter.GetTenantId();
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

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        /// <remarks></remarks>
        public String Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default password.
        /// </summary>
        /// <value>The default password.</value>
        /// <remarks></remarks>
        public String DefaultPassword
        {
            get;
            set;
        }

        public String EmailAddress
        {
            get;
            set;
        }

        public String FirstName
        {
            get;
            set;
        }

        public String LastName
        {
            get;
            set;
        }
        public String MiddleName
        {
            get;
            set;
        }
        public Entity.OrganizationUser OrganizationUser
        {
            get
            {
                if (ViewState["OrganizationUser"] != null)
                    return (Entity.OrganizationUser)ViewState["OrganizationUser"];
                return null;
            }
            set
            {
                ViewState["OrganizationUser"] = value;
            }
        }
        public INTSOF.ServiceDataContracts.Modules.Common.OrganizationUserContract OrganisationUser { get; set; }


        public IInstructorSupportPortalDetailView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }
        public String OrgUserId
        {
            get
            {
                if (!ViewState["UserId"].IsNull())
                {
                    return Convert.ToString(ViewState["UserId"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["UserId"] = value;
            }
        }
        public String UserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("UserId"))
                    {
                        return (Convert.ToString(args["UserId"]));
                    }
                }
                return String.Empty;
            }
        }
        List<INTSOF.ServiceDataContracts.Modules.ClinicalRotation.ClinicalRotationDetailContract> IInstructorSupportPortalDetailView.ClinicalRotationData
        {
            get
            {
                if (!(ViewState["ClinicalRotationData"] is List<INTSOF.ServiceDataContracts.Modules.ClinicalRotation.ClinicalRotationDetailContract>))
                {
                    ViewState["ClinicalRotationData"] = new List<INTSOF.ServiceDataContracts.Modules.ClinicalRotation.ClinicalRotationDetailContract>();
                }
                return (List<INTSOF.ServiceDataContracts.Modules.ClinicalRotation.ClinicalRotationDetailContract>)ViewState["ClinicalRotationData"];
            }
            set
            {
                ViewState["ClinicalRotationData"] = value;
            }
        }

        public Int32 LoggedInUserTenantId
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
        }

        #region UAT-4313
        List<INTSOF.ServiceDataContracts.Modules.ClientContact.ClientContactNotesContract> IInstructorSupportPortalDetailView.ClientContactNotes
        {
            get
            {
                if (!(ViewState["ClientContactNotes"] is List<INTSOF.ServiceDataContracts.Modules.ClientContact.ClientContactNotesContract>))
                {
                    ViewState["ClientContactNotes"] = new List<INTSOF.ServiceDataContracts.Modules.ClientContact.ClientContactNotesContract>();
                }
                return (List<INTSOF.ServiceDataContracts.Modules.ClientContact.ClientContactNotesContract>)ViewState["ClientContactNotes"];
            }
            set
            {
                ViewState["ClientContactNotes"] = value;
            }
        }

        public Int32 ClientContactId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("ClientContactId"))
                    {
                        return (Convert.ToInt32(args["ClientContactId"]));
                    }
                }
                return 0;
            }

            set
             {
                ClientContactId = value;
            }
        }
        #endregion



        #endregion

        #endregion

        #region Events

        #region Page Events
        /// <summary>
        /// OnInit event to set page titles
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.SetPageTitle("Support Portal Detail");
                base.Title = "Support Portal";
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
        /// Page_Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();

                Dictionary<String, String> args = new Dictionary<String, String>();

                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey("SearchInstanceId"))
                    {
                        SearchInstanceId = Convert.ToInt32(args["SearchInstanceId"]);
                    }
                    if (args.ContainsKey("TenantId"))
                    {
                        CurrentViewContext.SelectedTenantId = Convert.ToInt32(args["TenantId"]);
                    }                   

                    if (args.ContainsKey("MasterPageTabIndex"))
                    {
                        MasterPageTabIndex = Convert.ToInt32(args["MasterPageTabIndex"]);
                    }

                    if (!OrganizationUserId.IsNullOrEmpty())
                    {
                        OrgUserId = Convert.ToString(CurrentViewContext.OrganisationUser.UserID);
                    }

                }

            }

            hdnSelectedTenantID.Value = SelectedTenantId.ToString();
            hdnCurrentloggedInUserId.Value = CurrentLoggedInUserId.ToString();
            Presenter.OnViewLoaded();
            BindControls();

        }
        #endregion

        #region Button Events

        protected void lnkGoBack_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", AppConsts.SUPPORT_PORTAL },
                                                                    {"CancelClick","true"}
                                                                 };
                string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
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
        protected void btnInstructorLogin_Click(object sender, EventArgs e)
        {
            SwitchToInstuctorOrPreceptor(OrgUserId);
        }
        #endregion

        #region Grid Events

        #region Rotation Grid Events
        protected void grdRotations_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetRotationDetail();
                grdRotations.DataSource = CurrentViewContext.ClinicalRotationData;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to bind support portal order data.");
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to bind support portal order data.");
            }

        }

        protected void grdRotations_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewDetail")
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String rotationID = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RotationId"]);
                    String ReqPkgSubscriptionId = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PkgSubscriptionId"]);

                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.SelectedTenantId) },
                                                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                                                                    { ProfileSharingQryString.ReqPkgSubscriptionId, ReqPkgSubscriptionId },
                                                                    { ProfileSharingQryString.RotationId, rotationID },
                                                                    { ProfileSharingQryString.ApplicantId , Convert.ToString(CurrentViewContext.OrganizationUserId) },
                                                                    { ProfileSharingQryString.ControlUseType,AppConsts.SUPPORT_PORTAL_DETAIL_INSTRUCTOR_USE_TYPE_CODE },
                                                                    {ProfileSharingQryString.ClientContactId, CurrentViewContext.ClientContactId.ToString() },
                                                                    {"UserId",UserId}
                                                                 };


                    string url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
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
        #region Notes Grid
        protected void grdClientContactNotes_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    ClientContactNotesContract ccNotesContract = new ClientContactNotesContract();
                    WclTextBox txtNotes = e.Item.FindControl("txtNotes") as WclTextBox;
                    String saveMsg = "Note saved successfully.";
                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        ccNotesContract.NoteId = Convert.ToInt32((e.Item as GridEditableItem).GetDataKeyValue("NoteId"));
                        saveMsg = "Note updated successfully.";
                    }
                    ccNotesContract.Notes = txtNotes.Text;
                    ccNotesContract.InstructorOrgUserId = CurrentViewContext.OrganizationUserId;
                    ccNotesContract.ClientContactId = CurrentViewContext.ClientContactId;

                    if (!ccNotesContract.IsNullOrEmpty())
                    {
                        if (Presenter.SaveClientContactNotes(ccNotesContract))
                        {
                            e.Canceled = false;
                            base.ShowSuccessMessage(saveMsg);
                            grdClientContactNotes.Rebind();
                        }
                        else
                        {
                            e.Canceled = true;
                            base.ShowErrorMessage("Some error has occurred. Please try again.");
                        }
                    }


                }
                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    Int32 noteId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["NoteId"]);
                    if (!noteId.IsNullOrEmpty() && noteId > AppConsts.NONE)
                    {
                        if (Presenter.DeleteClientContactNotes(noteId))
                        {
                            e.Canceled = false;
                            base.ShowSuccessMessage("Note deleted successfully.");
                            grdClientContactNotes.Rebind();
                        }
                        else
                        {
                            e.Canceled = true;
                            base.ShowErrorMessage("Some error has occurred. Please try again.");
                        }
                    }
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

        protected void grdClientContactNotes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            Presenter.GetClientContactNotes();
            grdClientContactNotes.DataSource = CurrentViewContext.ClientContactNotes;
        }

        protected void grdClientContactNotes_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclTextBox txtNotes = e.Item.FindControl("txtNotes") as WclTextBox;

                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        ClientContactNotesContract clientContactNotesContract = e.Item.DataItem as ClientContactNotesContract;
                        txtNotes.Text = clientContactNotesContract.Notes;
                    }

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
        #endregion

        #region DropDown Events        

        #endregion

        #endregion

        #region Methods 

        private void BindControls()
        {
            Presenter.GetUserData();
            txtFirstName.Text = CurrentViewContext.OrganisationUser.FirstName;
            txtMiddleName.Text = CurrentViewContext.OrganisationUser.MiddleName;
            txtLastName.Text = CurrentViewContext.OrganisationUser.LastName;
            txtEmail.Text = CurrentViewContext.OrganisationUser.Email;
            txtSSN.Text = CurrentViewContext.OrganisationUser.SSN;
        }

        private void SwitchToInstuctorOrPreceptor(String organizationUserID)
        {
            String switchingTargetURL = ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL].IsNullOrEmpty()
                                                                            ? String.Empty
                                                                            : Convert.ToString(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_SHARED_USER_LOGIN_URL]);
            if (!(switchingTargetURL.Trim().StartsWith("http://", StringComparison.OrdinalIgnoreCase) || switchingTargetURL.Trim().StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            {
                if (HttpContext.Current != null)
                {
                    switchingTargetURL = string.Concat(HttpContext.Current.Request.Url.Scheme, "://", switchingTargetURL.Trim());
                }
                else
                {
                    switchingTargetURL = string.Concat("http://", switchingTargetURL.Trim());
                }
            }
            RedirectToTargetSwitchingViewInstructor(organizationUserID, switchingTargetURL);
        }
        private void RedirectToTargetSwitchingViewInstructor(String UserID, String switchingTargetURL)
        {
            Dictionary<String, ApplicantInsituteDataContract> applicantData = new Dictionary<String, ApplicantInsituteDataContract>();
            ApplicantInsituteDataContract appInstData = new ApplicantInsituteDataContract();
            appInstData.UserID = UserID;
            appInstData.TagetInstURL = switchingTargetURL;
            appInstData.TokenCreatedTime = DateTime.Now;
            appInstData.UserTypeSwitchViewCode = UserTypeSwitchView.InstructorOrPreceptor.GetStringValue();
            appInstData.AdminOrgUserID = CurrentViewContext.CurrentLoggedInUserId;
            String key = Guid.NewGuid().ToString();

            Dictionary<String, ApplicantInsituteDataContract> applicationData = Presenter.GetDataByKey("ApplicantInstData");
            if (applicationData != null)
            {
                applicantData = applicationData;
                applicantData.Add(key, appInstData);
                Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
            }
            else
            {
                applicantData.Add(key, appInstData);
                Presenter.AddWebApplicationData("ApplicantInstData", applicantData);
            }

            //Log out from application then redirect to selected tenant url, append key in querystring.
            // On login page get data from Application Variable.
            //Presenter.DoLogOff(true);
            Presenter.AddImpersonationHistory(UserID, CurrentViewContext.CurrentLoggedInUserId);
            //Redirect to login page
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TokenKey", key  }
                                                                 };

            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenInstructorView('" + String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}&DeletePrevUsrState=true", key) + "');", true);
            //Response.Redirect(String.Format(appInstData.TagetInstURL + "/Login.aspx?TokenKey={0}", key));
        }
        #endregion

    }
}