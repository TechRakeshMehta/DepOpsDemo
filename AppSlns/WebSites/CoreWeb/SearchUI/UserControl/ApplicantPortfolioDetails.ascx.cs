#region Namespaces

#region SystemDefined

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.IntsofSecurityModel;
using System.Web.UI.WebControls;
using Business.RepoManagers;
using System.Configuration;
using Telerik.Web.UI;
using INTSOF.UI.Contract.ProfileSharing;
using WebSiteUtils.SharedObjects;
using INTERSOFT.WEB.UI.WebControls;

#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public partial class ApplicantPortfolioDetails : BaseUserControl, IApplicantPortfolioDetailsView
    {
        #region Variables

        #region Private Variables

        private ApplicantPortfolioDetailsPresenter _presenter = new ApplicantPortfolioDetailsPresenter();
        private List<InvitationDataContract> lstInvitations = new List<InvitationDataContract>();
        private String _viewType;
        private Int32 tenantId = 0;
        private Boolean? _isAdminLoggedIn = null;
        #endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties

        #region Public Properties


        public ApplicantPortfolioDetailsPresenter Presenter
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

        public Boolean IsLocked
        {
            get
            {
                return chkLocked.Checked;
            }
            set
            {
                chkLocked.Checked = value;
            }
        }

        public Boolean IsActive
        {
            get
            {
                return chkActive.Checked;
            }
            set
            {
                chkActive.Checked = value;
            }
        }

        /*UAT-2930*/
        //public Boolean IsTwoFactorAuthentication
        //{
        //    get;
        //    set;
        //}
        //UAT-3068
        String IApplicantPortfolioDetailsView.SelectedAuthenticationType
        {
            get
            {
                return rdbSpecifyAuthentication.SelectedValue;
            }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    rdbSpecifyAuthentication.SelectedValue = INTSOF.Utils.AuthenticationMode.None.GetStringValue();
                }
                else
                {
                    rdbSpecifyAuthentication.SelectedValue = value;
                }
            }
        }
        //public Boolean IsTwoFactorAuthenticationVerified
        //{
        //    get;
        //    set;
        //}
        String IApplicantPortfolioDetailsView.IsUserTwoFactorAuthenticatedPrevious
        {
            get
            {
                if (!ViewState["IsUserTwoFactorAuthenticatedPrevious"].IsNull())
                {
                    return Convert.ToString(ViewState["IsUserTwoFactorAuthenticatedPrevious"]);
                }
                return String.Empty;
            }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    ViewState["IsUserTwoFactorAuthenticatedPrevious"] = INTSOF.Utils.AuthenticationMode.None.GetStringValue();
                }
                else
                {
                    ViewState["IsUserTwoFactorAuthenticatedPrevious"] = value;
                }
            }
        }
        /*UAT-2930*/

        public String QueueType
        {
            get
            {
                if (!ViewState["QueueType"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["QueueType"]);
                }
                return null;
            }
            set
            {
                ViewState["QueueType"] = value;
            }
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

        List<InvitationDataContract> IApplicantPortfolioDetailsView.lstInvitationsSent
        {
            set
            {
                lstInvitations = value;
            }
            get
            {
                return lstInvitations;
            }
        }

        public IApplicantPortfolioDetailsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Whether the invitation is sent successfully.
        /// </summary>
        Boolean IApplicantPortfolioDetailsView.IsInvitationSent
        {
            get;
            set;
        }

        //UAT 2467
        public List<INTSOF.ServiceDataContracts.Modules.ClinicalRotation.AttestationDocumentContract> LstInvitationDocumentContract
        {
            get;
            set;
        }

        public Boolean IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.Value;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        public String UserId
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

        public String QueueTypeChild
        {
            get
            {
                if (!ViewState["QueueTypeChild"].IsNullOrEmpty())
                {
                    return Convert.ToString(ViewState["QueueTypeChild"]);
                }
                return null;
            }
            set
            {
                ViewState["QueueTypeChild"] = value;
            }
        }
        #endregion

        #endregion

        #region Events

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
                base.SetPageTitle("Applicant Portfolio Detail");
                base.Title = "Applicant Portfolio";
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
                //To check if cancel button is clicked on Edit Profile page
                //and get session values for controls
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
                    if (args.ContainsKey("PageType"))
                    {
                        QueueType = Convert.ToString(args["PageType"]);
                        ucApplicantRequirementRotations.QueueType = QueueType;
                    }
                    //UAT-2971
                    if (args.ContainsKey("UserId"))
                    {
                        UserId = Convert.ToString(args["UserId"]);
                    }

                    if (args.ContainsKey("PageTypeChild"))
                    {
                        QueueTypeChild = Convert.ToString(args["PageTypeChild"]);
                        ucApplicantRequirementRotations.QueueTypeChild = QueueTypeChild;
                    }
                }

                if (Presenter.IsDefaultTenant)
                {
                    divUserKeyAttributes.Visible = true;
                    fsucCmdBarPortfolio.ShowButtons(CommandBarButtons.Save);
                    fsucCmdBarPortfolio.ShowButtons(CommandBarButtons.Extra);
                    Presenter.SetPageControls();
                    //UAT-2930
                    if (Presenter.ShowhideTwoFactorAuthentication())
                    {
                        divtwofactorAuthentication.Visible = true;
                        //if (CurrentViewContext.IsTwoFactorAuthentication)
                        //{
                        //    rdbSpecifyAuthentication.SelectedValue = "True";
                        //    hdnIsTwoFactorAuthenticationPrevious.Value = "True";
                        //    rdbSpecifyAuthentication.Enabled = true;
                        //    //if (!CurrentViewContext.IsTwoFactorAuthenticationVerified)
                        //    //    spnIsTwoFactorAuthVerified.InnerText = "(Not Verified)";
                        //    //else
                        //    //    spnIsTwoFactorAuthVerified.InnerText = "";
                        //}
                        //else
                        //{
                        //    rdbSpecifyAuthentication.SelectedValue = "False";
                        //    rdbSpecifyAuthentication.Enabled = false;
                        //    //spnIsTwoFactorAuthVerified.InnerText = "";
                        //}
                    }
                    else
                    {
                        divtwofactorAuthentication.Visible = false;
                    }


                    OrganizationUser = this.OrganizationUser;
                    ucApplicantPortfolioProfile.OrganizationUser = this.OrganizationUser;

                    ApplyActionLevelPermission(ActionCollection, "Applicant Portfolio Detail");
                }
                else
                {
                    divUserKeyAttributes.Visible = false;
                }

                //RouteBack();
                RoutePageBack();

                #region UAT-2918: Integration Account Linking Exposure

                if (IsAdminLoggedIn == true)
                {
                    divIntegrationSection.Visible = true;
                    ucApplicantPortfolioIntegration.Visible = true;
                    ucApplicantPortfolioIntegration.LoadData = true;
                }
                else
                {
                    ucApplicantPortfolioIntegration.Visible = false;
                    divIntegrationSection.Visible = false;
                    ucApplicantPortfolioIntegration.LoadData = false;
                }
                #endregion

                if (QueueType == WorkQueueType.SupportPortalDetail.ToString())
                {
                    lnkGoBack.InnerText = "Back to Support Portal Detail";
                }
            }


            // BindInvitations();
            Presenter.OnViewLoaded();

            //UAT 2702
            ucApplicantOrderNotification.OrganizationUserId = CurrentViewContext.OrganizationUserId;
            ucApplicantOrderNotification.SelectedTenantID = CurrentViewContext.SelectedTenantId;
            //ucApplicantPortfolioProfile.EventReloadTwoFactor += new ApplicantPortfolioProfile.ReloadTwoFactor(ucReload_Success);
        }

        #region Button Events

        protected void fsucCmdBarPortfolio_ExtraClick(object sender, EventArgs e)
        {
            try
            {
                Password = radCpatchaPassword.CaptchaImage.Text;
                Entity.OrganizationUser orgUser = Presenter.GetOrganizationUser();
                if (!orgUser.IsNullOrEmpty())
                {
                    if (Presenter.ResetPassword(orgUser))
                    {
                        Dictionary<String, Object> contents = new Dictionary<String, Object>
                                                              {                                                                  
                                                              {EmailFieldConstants.USER_FULL_NAME,FirstName + " " + LastName},
                                                                  {EmailFieldConstants.PASSWORD,Password}
                                                                  ,{EmailFieldConstants.RECEIVER_ORGANIZATION_USER_ID,orgUser.OrganizationUserID},
                                                                  { EmailFieldConstants.INSTITUTION_URL,Presenter.GetInstitutionURL()} // UAT- 4306
                                                              };
                        SecurityManager.PrepareAndSendSystemMail(EmailAddress, contents, CommunicationSubEvents.NOTIFICATION_PASSWORD_RESET_BY_ADMIN, null, true);
                        Boolean mailStatus = true;
                        if (mailStatus)
                        {
                            base.ShowSuccessMessage(SysXUtils.GetMessage(ResourceConst.SECURITY_PASSWORD_SEND_SUCEESFULLY));
                        }
                        else
                        {
                            base.ShowInfoMessage(SysXUtils.GetMessage(ResourceConst.SECURITY_FAILED_TO_SEND_EMAIL));
                        }
                    }
                }
                else
                {
                    base.ShowErrorMessage("Password did not reset succesfully.");
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

        protected void fsucCmdBarPortfolio_SaveClick(object sender, EventArgs e)
        {
            try
            {
                #region UAT-2930
                //if (Convert.ToString(rdbSpecifyAuthentication.SelectedValue) == "NONE")
                //{
                //    CurrentViewContext.IsTwoFactorAuthentication = true;
                //    rdbSpecifyAuthentication.Enabled = true;
                //}
                //else
                //{
                //    CurrentViewContext.IsTwoFactorAuthentication = false;
                //    rdbSpecifyAuthentication.Enabled = false;
                //    //spnIsTwoFactorAuthVerified.InnerText = String.Empty;
                //}
                //CurrentViewContext.IsUserTwoFactorAuthenticatedPrevious = Convert.ToBoolean(hdnIsTwoFactorAuthenticationPrevious.Value);
                #endregion
                if (Presenter.UpdateUser())
                {
                    //if (CurrentViewContext.IsTwoFactorAuthentication == true)
                    //    hdnIsTwoFactorAuthenticationPrevious.Value = "True";
                    //else
                    //    hdnIsTwoFactorAuthenticationPrevious.Value = "False";

                    base.ShowSuccessMessage("User Details updated successfully.");
                }
                else
                {
                    base.ShowErrorMessage("User Details not updated successfully.");
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

        #endregion

        #endregion


        #region Methods

        /// <summary>
        /// To route Page Back
        /// </summary>
        /// <param name="showSuccessMessage"></param>
        private void RoutePageBack()
        {
            String childcontrolPath = null;
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            if (QueueType == WorkQueueType.SupportPortalDetail.ToString())
            {
                childcontrolPath = ChildControls.SupportPortalDetails;
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", childcontrolPath },
                                                                    {"OrganizationUserId", CurrentViewContext.OrganizationUserId.ToString()},
                                                                    {"TenantId",CurrentViewContext.SelectedTenantId.ToString()},
                                                                    {"UserId",UserId.ToString()},
                                                                    {"PageType",QueueType}
                                                                };
            }
            else
            {
                if (SearchInstanceId != 0)
                {
                    childcontrolPath = ChildControls.ApplicantPortFolioSearchCopyPage;
                }
                else if (QueueType == WorkQueueType.ComprehensiveSearch.ToString())
                {
                    childcontrolPath = ChildControls.ApplicantComprehensiveSearchPage;
                }

                else
                {
                    childcontrolPath = ChildControls.ApplicantPortFolioSearchPage;
                }
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", childcontrolPath },
                                                                    {"CancelClicked", "CancelClicked" },
                                                                    {"SearchInstanceId", SearchInstanceId.ToString() },
                                                                    { "MasterPageTabIndex", MasterPageTabIndex.ToString()},
                                                                 };
            }
            string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            lnkGoBack.HRef = url;
        }


        //private void RouteBack()
        //{
        //    Dictionary<String, String> queryString = new Dictionary<String, String>
        //                                                         { 
        //                                                            { "Child", ChildControls.ApplicantMessageGrid },
        //                                                            {"OrganizationUserId", Convert.ToString(OrganizationUserId)},
        //                                                            {"SearchInstanceId", SearchInstanceId.ToString() },
        //                                                            { "MasterPageTabIndex", MasterPageTabIndex.ToString()},
        //                                                            {"TenantId",Convert.ToString(CurrentViewContext.SelectedTenantId)}
        //                                                         };
        //    string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
        //    //Response.Redirect(url, true);

        //    //lnkMsgs.HRef = url;
        //}

        #endregion

        #region Apply Permissions

        public override List<ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();

                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Save",
                    CustomActionLabel = "Save User Details",
                    ScreenName = "Applicant Portfolio Detail"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Reset",
                    CustomActionLabel = "Reset Password",
                    ScreenName = "Applicant Portfolio Detail"
                });
                return actionCollection;
            }

        }

        protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            permission.ForEach(x =>
            {
                switch (x.PermissionID)
                {
                    case AppConsts.ONE:
                        {
                            break;
                        }
                    case AppConsts.THREE:
                        {
                            if (x.FeatureAction.CustomActionId == "Save")
                            {
                                fsucCmdBarPortfolio.SaveButton.Enabled = false;
                            }
                            else if (x.FeatureAction.CustomActionId == "Reset")
                            {
                                fsucCmdBarPortfolio.ExtraButton.Enabled = false;
                            }
                            break;
                        }
                    case AppConsts.FOUR:
                        {
                            if (x.FeatureAction.CustomActionId == "Save")
                            {
                                fsucCmdBarPortfolio.HideButtons(CommandBarButtons.Save);
                            }
                            else if (x.FeatureAction.CustomActionId == "Reset")
                            {
                                fsucCmdBarPortfolio.HideButtons(CommandBarButtons.Extra);
                            }
                            break;
                        }
                }

            }
                );
        }

        #endregion

        #region UAT 2467

        protected void grdInvitations_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.BindInvitations();
                grdInvitations.DataSource = CurrentViewContext.lstInvitationsSent;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to bind invitation.");
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to bind invitation.");
            }
        }


        protected void grdInvitations_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    InvitationDataContract gridData = (InvitationDataContract)e.Item.DataItem;
                    LinkButton lnkViewAttestation = (LinkButton)e.Item.FindControl("lnkViewAttestation");
                    lnkViewAttestation.Visible = !Convert.ToBoolean(gridData.IsIndividualShare);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to bind invitation.");
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to bind invitation.");
            }
        }

        protected void grdInvitations_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ViewAttestation")
                {
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String profileSharingInvID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ID"].ToString();
                    ExportDocuments(Convert.ToInt32(profileSharingInvID));
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to download attestation.");
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowInfoMessage("Failed to download attestation.");
            }
        }

        /// <summary>
        /// Export Documents
        /// </summary>
        /// <param name="profileSharingInvID"></param>
        private void ExportDocuments(Int32 profileSharingInvID)
        {
            Presenter.GetAttestationDocumentsToExport(profileSharingInvID);
            if (!CurrentViewContext.LstInvitationDocumentContract.IsNullOrEmpty())
            {
                ifrExportDocument.Src = ProfileSharingHelper.ExportAttestationDocument(CurrentViewContext.LstInvitationDocumentContract, CurrentViewContext.SelectedTenantId);
            }
            else
            {
                base.ShowInfoMessage("No document(s) found to export.");
            }
        }
        #endregion


        //protected void ucReload_Success(object sender)
        //{
        //    if (Presenter.IsDefaultTenant)
        //    {
        //        Presenter.SetPageControls();
        //    }
        //}
    }
}

