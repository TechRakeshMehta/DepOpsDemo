using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.ClinicalRotation.UserControl;
using CoreWeb.ClinicalRotation.Views;
using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RequirementItemDataPanel : BaseUserControl, IRequirementItemDataPanel
    {
        #region Private Variables

        private RequirementItemDataPanelPresenter _presenter = new RequirementItemDataPanelPresenter();
        private string _userrole;

        String nextCategoryURL;
        String prevCategoryURL;
        #endregion

        #region Properties

        #region Private

        #endregion

        #region Public

        public IRequirementItemDataPanel CurrentViewContext
        {
            get { return this; }
        }

        public RequirementItemDataPanelPresenter Presenter
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

        public String prevSubscriptionURL
        {
            get
            {
                if (ViewState["prevSubscriptionURL"].IsNotNull())
                    return Convert.ToString(ViewState["prevSubscriptionURL"]);
                return String.Empty;
            }
            set
            {
                ViewState["prevSubscriptionURL"] = value;
            }
        }
        public String nextSubscriptionURL
        {
            get
            {
                if (ViewState["nextSubscriptionURL"].IsNotNull())
                    return Convert.ToString(ViewState["nextSubscriptionURL"]);
                return String.Empty;
            }
            set
            {
                ViewState["nextSubscriptionURL"] = value;
            }
        }
        //UAT-4461
        List<RequirementVerificationDetailContract> IRequirementItemDataPanel.lstReqPkgSubData
        {
            get;
            set;
        }
        public ManageReqPkgSubscriptionContract NextPrevNavigationData //UAT-4461
        {
            get
            {
                if (Session["NextPrevNavigationData"].IsNotNull())
                    return Session["NextPrevNavigationData"] as ManageReqPkgSubscriptionContract;
                return new ManageReqPkgSubscriptionContract();
            }
            set
            {
                Session["NextPrevNavigationData"] = value;
            }
        }
        public Boolean IsCurrentARIDRecordAlreadySaved //UAT-4461
        {
            get
            {
                return Convert.ToBoolean(Session["IsCurrentARIDRecordAlreadySaved"] ?? false);
            }
            set
            {
                Session["IsCurrentARIDRecordAlreadySaved"] = value;
            }
        }
        public Int32 ApplicantRequirementItemDataId //UAT-4461
        {
            get
            {
                return Convert.ToInt32(Session["ApplicantRequirementItemDataId"] ?? "0");
            }
            set
            {
                Session["ApplicantRequirementItemDataId"] = value;
            }
        }
        public List<ReqPkgSubscriptionIDList> lstApplicantDataForNavigation
        {
            get
            {
                if (ViewState["lstApplicantDataForNavigation"].IsNotNull())
                    return ViewState["lstApplicantDataForNavigation"] as List<ReqPkgSubscriptionIDList>;
                return new List<ReqPkgSubscriptionIDList>();
            }
            set
            {
                ViewState["lstApplicantDataForNavigation"] = value;
            }
        }
        RequirementPackageSubscriptionContract IRequirementItemDataPanel.RotationSubscriptionDetail
        {
            get
            {
                if (ViewState["RotationSubscriptionDetail"].IsNotNull())
                    return ViewState["RotationSubscriptionDetail"] as RequirementPackageSubscriptionContract;
                return new RequirementPackageSubscriptionContract();
            }
            set
            {
                ViewState["RotationSubscriptionDetail"] = value;
            }
        }

        /// <summary>
        /// Represents the Category Level Data
        /// </summary>
        List<RequirementVerificationDetailContract> IRequirementItemDataPanel.lstCategoryData
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the CategoryId
        /// </summary>
        Int32 IRequirementItemDataPanel.CurrentReqCategoryId
        {
            get;
            set;
        }

        /// Represents the SubscriptionId
        Int32 IRequirementItemDataPanel.CurrentReqSubsciptionId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the CategoryId
        /// </summary>
        Int32 IRequirementItemDataPanel.TenantId
        {
            get;
            set;
        }

        String IRequirementItemDataPanel.ControlUseType
        {
            get;
            set;
        }

        /// <summary>
        /// Contains the complete data of the verification details, which needs to be saved/updated
        /// </summary>
        RequirementVerificationData IRequirementItemDataPanel.DataToSave
        {
            get;
            set;
        }

        String IRequirementItemDataPanel.ReqItemDataLoaderControlId
        {
            get
            {
                return "ucReqItemDataLoaderControl_" + CurrentViewContext.CurrentReqCategoryId;
            }
        }

        /// <summary>
        /// Represents the ID of the currently logged in user.
        /// </summary>
        Int32 IRequirementItemDataPanel.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// Represents the ID of the Applicant
        /// </summary>
        Int32 IRequirementItemDataPanel.ApplicantId
        {
            get;
            set;
        }

        /// <summary>
        /// Current Rotation ID
        /// </summary>
        Int32 IRequirementItemDataPanel.CurrentRotationId
        {
            get;
            set;
        }

        string IRequirementItemDataPanel.EntityPermissionName
        {
            get;
            set;
        }

        //UAT:-3049
        Int32 IRequirementItemDataPanel.AgencyId
        {
            get;
            set;
        }
        public Int32 CurrentTenantId_Global
        {
            get
            {
                if (!ViewState["CurrentTenantId"].IsNullOrEmpty())
                    return (Int32)(ViewState["CurrentTenantId"]);
                else
                    return 0;
            }
            set
            {
                ViewState["CurrentTenantId"] = value;
            }
        }

        #endregion

        #endregion

        protected override void OnInit(EventArgs e)
        {
            try
            {

                var args = new Dictionary<String, String>();
                String userType = String.Empty;
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);
                    if (args.ContainsKey(ProfileSharingQryString.ControlUseType))
                    {
                        userType = args[ProfileSharingQryString.ControlUseType].ToLower().Trim();
                    }

                    if (!userType.IsNullOrEmpty() && (userType == AppConsts.SUPPORT_PORTAL_DETAIL_USE_TYPE_CODE.ToLower().Trim() || userType == AppConsts.SUPPORT_PORTAL_DETAIL_INSTRUCTOR_USE_TYPE_CODE.ToLower().Trim()))
                    {
                        btnNext.Text = "Save and Next";
                        btnPrevious.Text = "Save and Previous";
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
        protected void Page_Load(object sender, EventArgs e)
        {
            CaptureQueryString();
            //Start-UAT-4461
            HiddenField hdnApplicantReqItemID = this.Parent.FindControl("hdnApplicantReqItemID") as HiddenField; //UAT-4461
            if (hdnApplicantReqItemID.IsNotNull() && hdnApplicantReqItemID.Value != "" && hdnApplicantReqItemID.Value != Convert.ToString(CurrentViewContext.ApplicantRequirementItemDataId))
            {
                Session["NextPrevNavigationData"] = null;
                Session["IsCurrentARIDRecordAlreadySaved"] = null;
                Session["ApplicantRequirementItemDataId"] = null;
            }
            //END UAT

            //UAT 2371
            Presenter.GetSystemEntityUserPermission(CurrentViewContext.CurrentLoggedInUserId, CurrentViewContext.TenantId);

            LoadRequirementItems();
            if (!CurrentViewContext.CurrentTenantId_Global.IsNullOrEmpty() && !CurrentViewContext.CurrentRotationId.IsNullOrEmpty() && !CurrentViewContext.CurrentReqSubsciptionId.IsNullOrEmpty())
                Presenter.GetRequirementPackageCategoryData();


            //if (CurrentViewContext.ControlUseType == AppConsts.ROTATION_MEMBER_SEARCH_USE_TYPE_CODE.ToLower().Trim())
            //{
            //    cmdBar.SaveButton.Enabled = false;
            //    cmdBar.SubmitButton.Enabled = false;
            //}
            ////UAT-1800: Add Rotation information (if any) to portfolio search details screens
            //else 
            if (CurrentViewContext.ControlUseType == AppConsts.ROTATION_PORTFOLIO_SEARCH_USE_TYPE_CODE.ToLower().Trim())
            {
                cmdBar.SubmitButton.Enabled = false;
            }
            if (!this.IsPostBack)
                HandleNextPrevNavigation();


            #region UAT 2371
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            var _userType = GetUserType(user);
            if (_userType == UserType.CLIENTADMIN)
            {
                if (!String.IsNullOrEmpty(CurrentViewContext.EntityPermissionName) && Convert.ToString(CurrentViewContext.EntityPermissionName).ToUpper() == "NONE")
                {
                    cmdBar.SaveButton.Enabled = false;
                    cmdBar.ExtraButton.Enabled = false;
                }
            }
            #endregion
        }

        #region Events

        public UserType GetUserType(SysXMembershipUser user)
        {
            if (user.IsApplicant.IsNotNull() && user.IsApplicant)
                return UserType.APPLICANT;
            else if (!user.IsApplicant && user.TenantTypeCode == TenantType.Institution.GetStringValue())
                return UserType.CLIENTADMIN;
            else if (!user.IsApplicant && (user.TenantTypeCode == TenantType.Compliance_Reviewer.GetStringValue()))
                return UserType.THIRDPARTYADMIN;
            else if (user.IsSharedUser.IsNotNull() && user.IsSharedUser)
                return UserType.SHAREDUSER;
            else
                return UserType.SUPERADMIN;
        }

        /// <summary>
        /// Cancel and return to Queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBar_CancelClick(object sender, EventArgs e)
        {
            try
            {
                #region UAT-3345
                Object control = pnlLoader.FindServerControlRecursively(CurrentViewContext.ReqItemDataLoaderControlId);
                var loadercontrol = control as RequirementItemDataLoader;
                if (loadercontrol.IsNotNull())
                {
                    loadercontrol.saveDocumentMappings();
                }
                #endregion

                //UAT-2224: Admin access to upload/associate documents on rotation package items.
                //2366-Add the ability for "Item 1 must be dated before/after item 2" ui rules in rotation packages.
                Tuple<Boolean, Dictionary<Int32, String>> saveResponse = SaveData();

                Boolean _isDocFieldValidationFailed = saveResponse.Item1;
                if (_isDocFieldValidationFailed)
                {
                    EnableDisableAllValidations();
                }
                else
                {
                    Dictionary<Int32, String> dicResponse = saveResponse.Item2;
                    Boolean isSuccess = !dicResponse.Keys.Any(); ;
                    String errorMsg = dicResponse.Values.FirstOrDefault();

                    if (isSuccess)
                    {
                        if (!prevSubscriptionURL.IsNullOrEmpty())
                            Response.Redirect(prevSubscriptionURL, true);
                    }
                    else
                    {
                        ShowErrorMessages(dicResponse);
                        EnableDisableAllValidations();
                    }
                }
            }
            catch (Exception ex)
            {
                base.ShowInfoMessage("Data could not be saved.");
                base.LogError(ex);
            }



            //if (CurrentViewContext.ControlUseType == AppConsts.ROTATION_MEMBER_SEARCH_USE_TYPE_CODE.ToLower().Trim())
            //{
            //    ReturnToRotationMemberSearchQueue();
            //}
            ////UAT-1800: Add Rotation information (if any) to portfolio search details screens
            //else if (CurrentViewContext.ControlUseType == AppConsts.ROTATION_PORTFOLIO_SEARCH_USE_TYPE_CODE.ToLower().Trim())
            //{
            //    ReturnToPortfolioDetailScreen();
            //}
            //else if (CurrentViewContext.ControlUseType == AppConsts.SUPPORT_PORTAL_DETAIL_USE_TYPE_CODE.ToLower().Trim())
            //{
            //    Dictionary<String, String> queryString = new Dictionary<String, String>
            //                      {
            //                         { "TenantId",  Convert.ToString(CurrentViewContext.TenantId) },
            //                         { "Child",ChildControls.SupportPortalDetails },
            //                        { "OrganizationUserId",Convert.ToString(CurrentViewContext.ApplicantId)},
            //                        {"PageType",WorkQueueType.SupportPortalDetail.ToString()},
            //                        {"UserId",String.Empty},
            //                        {"PageTypeChild",WorkQueueType.SupportPortalDetail.ToString()}
            //                      };
            //    String URL = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
            //    Response.Redirect(URL, true);
            //}
            //else if (CurrentViewContext.ControlUseType == AppConsts.ROTATION_VERIFICATION_USER_WORK__QUEUE_TYPE_CODE.ToLower().Trim())
            //{
            //    Dictionary<String, String> queryString = new Dictionary<String, String>
            //                 {
            //                  { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
            //                 { "Child",  @"~\ClinicalRotation\RotationVerificationUserWorkQueue.ascx"}
            //                               };
            //    String URL = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
            //    Response.Redirect(URL, true);
            //}
            //else if (CurrentViewContext.ControlUseType == AppConsts.ASSIGN_ROTATION_VERIFICATION_QUEUE_TYPE_CODE.ToLower().Trim())
            //{
            //    Dictionary<String, String> queryString = new Dictionary<String, String>
            //               {
            //                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
            //                 { "Child",  @"~\ClinicalRotation\AssignRotationVerificationRecords.ascx"}
            //              };
            //    String URL = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
            //    Response.Redirect(URL, true);
            //}

            ////UAT-3049:- Redirect to Rotation Detail UC 
            //else if (CurrentViewContext.ControlUseType == AppConsts.ROTATION_DETAIL_USE_TYPE_CODE.ToLower().Trim())
            //{
            //    Dictionary<String, String> queryString = new Dictionary<String, String>
            //               {
            //                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
            //                    {"ID", Convert.ToString(CurrentViewContext.CurrentRotationId)},
            //                    {ProfileSharingQryString.AgencyId, CurrentViewContext.AgencyId.ToString()},
            //                  { "Child",  @"~\ClinicalRotation\UserControl\RotationDetailForm.ascx"}
            //              };
            //    String url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
            //    Response.Redirect(url, true);
            //}

            //else
            //{
            //    RedirectToQueue();
            //}
        }

        private void ShowErrorMessages(Dictionary<int, string> dicResponse)
        {
            if (dicResponse.Keys.Any(k => k == AppConsts.NONE))
            {
                lblMessage.Text = String.Join("", dicResponse.Where(d => d.Key == AppConsts.NONE)
                    .Select(d => d.Value)
                    .ToList());
                lblMessage.CssClass = "error";
            }
            else
            {
                lblMessage.Text = "";
                lblMessage.CssClass = "";
            }
            var _ctrl = pnlLoader.FindServerControlRecursively(CurrentViewContext.ReqItemDataLoaderControlId);

            if (_ctrl.IsNotNull() && _ctrl is RequirementItemDataLoader)
            {
                var _loaderControl = _ctrl as RequirementItemDataLoader;
                _loaderControl.SetUIValidationMessage(dicResponse);
            }
        }

        /// <summary>
        /// Save and Move to next Record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBar_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                #region UAT-3345
                Object control = pnlLoader.FindServerControlRecursively(CurrentViewContext.ReqItemDataLoaderControlId);
                var loadercontrol = control as RequirementItemDataLoader;
                if (loadercontrol.IsNotNull())
                {
                    loadercontrol.saveDocumentMappings();
                }
                #endregion
                //UAT-2224: Admin access to upload/associate documents on rotation package items.
                //2366-Add the ability for "Item 1 must be dated before/after item 2" ui rules in rotation packages.
                Tuple<Boolean, Dictionary<Int32, String>> saveResponse = SaveData();

                Boolean _isDocFieldValidationFailed = saveResponse.Item1;
                if (_isDocFieldValidationFailed)
                {
                    EnableDisableAllValidations();
                }
                else
                {
                    Dictionary<Int32, String> dicResponse = saveResponse.Item2;
                    Boolean isSuccess = !dicResponse.Keys.Any(); ;
                    String errorMsg = dicResponse.Values.FirstOrDefault();

                    if (isSuccess)
                    {
                        var _selectedData = SysXWebSiteUtils.SessionService.GetCustomData("QueueData") as List<RFQSelectedDataContract>;
                        var _nextRecord = _selectedData.SkipWhile(rfq => rfq.RPSId != CurrentViewContext.CurrentReqSubsciptionId).Skip(1).FirstOrDefault();

                        if (_nextRecord.IsNotNull())
                        {
                            var queryString = new Dictionary<String, String>
                                                                 {
                                                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
                                                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                                                                    { ProfileSharingQryString.ReqPkgSubscriptionId,Convert.ToString( _nextRecord.RPSId )},
                                                                    { ProfileSharingQryString.RotationId,Convert.ToString( _nextRecord.RotationId) },
                                                                    { ProfileSharingQryString.ApplicantId ,  Convert.ToString(_nextRecord.OrganizationUserId ) }
                                                                 };
                            var url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                            Response.Redirect(url, true);
                        }
                        else
                        {
                            RedirectToQueue();
                        }
                    }
                    else
                    {
                        ShowErrorMessages(dicResponse);
                        EnableDisableAllValidations();
                    }
                }
            }
            catch (Exception ex)
            {
                base.ShowInfoMessage("Data could not be saved.");
                base.LogError(ex);
            }
        }


        /// <summary>
        /// Save and Return to Queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdBar_SaveClick(object sender, EventArgs e)
        {
            try
            {
                #region UAT-3345
                Object control = pnlLoader.FindServerControlRecursively(CurrentViewContext.ReqItemDataLoaderControlId);
                var loadercontrol = control as RequirementItemDataLoader;
                if (loadercontrol.IsNotNull())
                {
                    loadercontrol.saveDocumentMappings();
                }
                #endregion
                //UAT-2224: Admin access to upload/associate documents on rotation package items.
                //2366-Add the ability for "Item 1 must be dated before/after item 2" ui rules in rotation packages.
                Tuple<Boolean, Dictionary<Int32, String>> saveResponse = SaveData();

                Boolean _isDocFieldValidationFailed = saveResponse.Item1;
                if (_isDocFieldValidationFailed)
                {
                    EnableDisableAllValidations();
                    lblMessage.Text = "";
                }
                else
                {
                    Dictionary<Int32, String> dicResponse = saveResponse.Item2;
                    Boolean isSuccess = !dicResponse.Keys.Any();
                    String errorMsg = dicResponse.Values.FirstOrDefault();

                    if (isSuccess)
                    {

                        #region UAT-3345
                        LoadRequirementItems(); //to reload controls after save(UAT-3345)
                        this.Parent.Parent.Focus();
                        lblMessage.Text = "Item(s) updated successfully.";
                        lblMessage.CssClass = "sucs";
                        #endregion

                        ScriptManager.RegisterStartupScript(this, this.GetType(), Convert.ToString(Guid.NewGuid()),
                                                        "ChangeApplicantPanelData('" + CurrentViewContext.CurrentReqSubsciptionId + "','" + CurrentViewContext.TenantId + "','" + CurrentViewContext.CurrentRotationId + "');", true);

                        #region Commented Redirection code on Save All Changes Button - UAT 3345
                        //UAT-1800: Add Rotation information (if any) to portfolio search details screens
                        //if (CurrentViewContext.ControlUseType == AppConsts.ROTATION_PORTFOLIO_SEARCH_USE_TYPE_CODE.ToLower().Trim())
                        //{
                        //    ReturnToPortfolioDetailScreen();
                        //}
                        //else if (CurrentViewContext.ControlUseType == AppConsts.ROTATION_MEMBER_SEARCH_USE_TYPE_CODE.ToLower().Trim())
                        //{
                        //    ReturnToRotationMemberSearchQueue();
                        //}
                        //else if (CurrentViewContext.ControlUseType == AppConsts.SUPPORT_PORTAL_DETAIL_USE_TYPE_CODE.ToLower().Trim())
                        //{
                        //    Dictionary<String, String> queryString = new Dictionary<String, String>
                        //          {
                        //             { "TenantId",  Convert.ToString(CurrentViewContext.TenantId) },
                        //             { "Child",ChildControls.SupportPortalDetails },
                        //            { "OrganizationUserId",Convert.ToString(CurrentViewContext.ApplicantId)},
                        //            {"PageType",WorkQueueType.SupportPortalDetail.ToString()},
                        //            {"UserId",String.Empty},
                        //            {"PageTypeChild",WorkQueueType.SupportPortalDetail.ToString()}
                        //          };
                        //    String url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                        //    Response.Redirect(url, true);
                        //}
                        //else if (CurrentViewContext.ControlUseType == AppConsts.ROTATION_VERIFICATION_USER_WORK__QUEUE_TYPE_CODE.ToLower().Trim())
                        //{

                        //    //Dictionary<String, String> queryString = new Dictionary<String, String>
                        //    // {
                        //    //  { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
                        //    // { "Child",  @"~\ClinicalRotation\RotationVerificationUserWorkQueue.ascx"}
                        //    //               };
                        //    //String url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                        //    //Response.Redirect(url, true);
                        //}
                        //else if (CurrentViewContext.ControlUseType == AppConsts.ASSIGN_ROTATION_VERIFICATION_QUEUE_TYPE_CODE.ToLower().Trim())
                        //{

                        //  //  Dictionary<String, String> queryString = new Dictionary<String, String>
                        //  // {
                        //  //      { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
                        //  //   { "Child",  @"~\ClinicalRotation\AssignRotationVerificationRecords.ascx"}
                        //  //};
                        //  //  String url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                        //  //  Response.Redirect(url, true);
                        //}

                        ////UAT-3049:- Redirect to Rotation Detail UC 
                        //else if (CurrentViewContext.ControlUseType == AppConsts.ROTATION_DETAIL_USE_TYPE_CODE.ToLower().Trim())
                        //{
                        //  //  Dictionary<String, String> queryString = new Dictionary<String, String>
                        //  // {
                        //  //      { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
                        //  //      {"ID", Convert.ToString(CurrentViewContext.CurrentRotationId)},
                        //  //      {ProfileSharingQryString.AgencyId, CurrentViewContext.AgencyId.ToString()},
                        //  //      { "Child",  @"~\ClinicalRotation\UserControl\RotationDetailForm.ascx"}
                        //  //};
                        //  //  String url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                        //  //  Response.Redirect(url, true);
                        //}

                        //else
                        //{
                        //    RedirectToQueue();
                        //}
                        #endregion
                    }
                    else
                    {
                        ShowErrorMessages(dicResponse);
                        EnableDisableAllValidations();
                    }
                }
            }
            catch (Exception ex)
            {
                base.ShowInfoMessage("Data could not be saved.");
                base.LogError(ex);
            }
        }

        #endregion

        #region Public Methods

        private void LoadRequirementItems()
        {
            Presenter.GetCategoryData();
            pnlLoader.Controls.Clear();

            if (!CurrentViewContext.lstCategoryData.IsNullOrEmpty())
            {
                foreach (var item in CurrentViewContext.lstCategoryData)
                {
                    if (item.ApplDocId > AppConsts.NONE)
                    {
                        HiddenField hdnFirstDocumentID = this.Parent.FindControl("hdnFirstDocumentID") as HiddenField;
                        if (hdnFirstDocumentID.IsNotNull())
                        {
                            hdnFirstDocumentID.Value = Convert.ToString(item.ApplDocId);
                        }
                        break;
                    }
                }

                var _currentCategory = CurrentViewContext.lstCategoryData.First();
                lblCategoryName.Text = _currentCategory.CatName.HtmlEncode();
                litCategoryStatus.Text = _currentCategory.CatStatusName;
                //CurrentViewContext.CurrentReqCategoryId = _currentCategory.CatId;
                //var _assignedCatList = SysXWebSiteUtils.SessionService.GetCustomData("AssignedCatList") as List<RequirementVerificationDetailContract>;

                # region UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes)
                StringBuilder sb = new StringBuilder();
                //String cat = _currentCategory.CategoryExplanatoryNotes;
                if (!_currentCategory.CategoryExplanatoryNotes.IsNullOrEmpty())
                {
                    sb.Append(_currentCategory.CategoryExplanatoryNotes);
                }

                //Commented IN UAT-4254
                //if (!_currentCategory.RequirementDocumentLink.IsNullOrEmpty())
                //{
                //    //String RequirementDocumentLinkText = _currentCategory.RequirementDocumentLinkLabel.IsNullOrEmpty() ? "More Information" : _currentCategory.RequirementDocumentLinkLabel;  //UAT-3161  // Commented as not required on three panel screens.
                //    sb.Append("&nbsp");
                //    sb.Append("<br /><a href=\"" + _currentCategory.RequirementDocumentLink + "\" onclick=\"\" target=\"_blank\");'>" + "\r<p>" + "More Information" + "&nbsp" + "</a>");
                //}
                //Added in UAT-4254
                if (!_currentCategory.lstReqCatDocUrls.IsNullOrEmpty() && _currentCategory.lstReqCatDocUrls.Count > AppConsts.NONE)
                {
                    foreach (RequirementCategoryDocUrl catUrl in _currentCategory.lstReqCatDocUrls)
                    {
                        String urlLabel = !String.IsNullOrEmpty(catUrl.RequirementCatDocUrlLabel) ? catUrl.RequirementCatDocUrlLabel : "More Information";
                        sb.Append("&nbsp");
                        sb.Append("<br /><a href=\"" + catUrl.RequirementCatDocUrl + "\" onclick=\"\" target=\"_blank\");'>" + "\r<p>" + urlLabel + "&nbsp" + "</a>");
                    }
                }
                //END


                _currentCategory.CategoryExplanatoryNotes = sb.ToString();
                if (!_currentCategory.CategoryExplanatoryNotes.IsNullOrEmpty())
                {
                    litExplanatoryNotes.Text = String.Format("<span class='expl-title'></span><span class='expl-dur'></span>{0}", _currentCategory.CategoryExplanatoryNotes);
                }
                else
                {
                    litExplanatoryNotes.Text = String.Empty;
                }

                if (!_currentCategory.CategoryDescription.IsNullOrEmpty())
                {
                    litCategoryDesc.Text = String.Format("<span class='expl-title'></span><span class='expl-dur'></span>{0}", _currentCategory.CategoryDescription.HtmlEncode());
                }
                else
                {
                    litCategoryDesc.Text = String.Empty;
                }



                #endregion


                Control requirementItemLoader = Page.LoadControl("~/ClinicalRotation/UserControl/RequirementItemDataLoader.ascx");

                requirementItemLoader.ID = CurrentViewContext.ReqItemDataLoaderControlId;
                (requirementItemLoader as IRequirementItemDataLoader).lstCategoryData = CurrentViewContext.lstCategoryData.OrderBy(ord => ord.RequirementItemDisplayOrder).ToList();
                if (CurrentViewContext.lstCategoryData.All(cond => !cond.IsCategoryDataMovementAllowed))
                    imageADEdisabled.Visible = true;
                (requirementItemLoader as IRequirementItemDataLoader).TenantId = CurrentViewContext.TenantId;
                (requirementItemLoader as IRequirementItemDataLoader).CategoryId = CurrentViewContext.CurrentReqCategoryId;
                (requirementItemLoader as IRequirementItemDataLoader).ControlUseType = CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty;
                //UAT-2224: Admin access to upload/associate documents on rotation package items.
                (requirementItemLoader as IRequirementItemDataLoader).SelectedApplicantId_Global = CurrentViewContext.ApplicantId;
                (requirementItemLoader as IRequirementItemDataLoader).CurrentRequirementPackageSubscriptionID_Global = CurrentViewContext.CurrentReqSubsciptionId;
                (requirementItemLoader as IRequirementItemDataLoader).EntityPermissionName = CurrentViewContext.EntityPermissionName;
                (requirementItemLoader as IRequirementItemDataLoader).CurrentTenantId_Global = CurrentViewContext.CurrentTenantId_Global;

                pnlLoader.Controls.Add(requirementItemLoader);
                List<RequirementVerificationDetailContract> lstAssignedCategories = new List<RequirementVerificationDetailContract>();
                foreach (var assignedCategory in CurrentViewContext.lstCategoryData)
                {
                    if (assignedCategory.AssignToUserID == CurrentUserId)
                    {
                        lstAssignedCategories.Add(assignedCategory);
                    }
                }
                var b = lstAssignedCategories;
            }
        }

        private void HandleNextPrevNavigation()
        {
            //Note:-Submission Date in Rotation Verification Queue is wrong.So, We may have ticket in future to fix - submission date issue then save and next and save and previous navigation should be test and updated as well.

            #region New code for user work queue
            var _assignedCatList = SysXWebSiteUtils.SessionService.GetCustomData("AssignedCatList") as List<RequirementVerificationDetailContract>;
            //if()
            //var _currentCategory = CurrentViewContext.lstCategoryData.First();
            //var CurrentCategoryId = _currentCategory.CatId;
            //List<Int32> assignedCatgories = new List<int>();
            //assignedCatgories.Add(1);
            //assignedCatgories.Add(2);
            //assignedCatgories.Add(6);
            //assignedCatgories.Add(9);
            //int currentCat = 5;
            //int prevCat = 0;
            //int nextCat = 0;
            //int prevAssignedCategoryID = AppConsts.NONE;
            //int nextAssignedCategoryID = AppConsts.NONE;
            //if (!_assignedCatList.IsNullOrEmpty() && _assignedCatList.Count > 1)
            //{
            //    if (_assignedCatList.Where(d => d.CatId < CurrentCategoryId).Any())
            //    {
            //        if (_assignedCatList.Where(d => d.CatId < CurrentCategoryId).Count() > 1)
            //        {
            //            prevAssignedCategoryID = _assignedCatList.Where(d => d.CatId < CurrentCategoryId).LastOrDefault().CatId;
            //        }
            //        else
            //        {
            //            prevAssignedCategoryID = _assignedCatList.Where(d => d.CatId < CurrentCategoryId).FirstOrDefault().CatId;
            //        }
            //    }
            //    if (_assignedCatList.Where(d => d.CatId > CurrentCategoryId).Any())
            //    {
            //        if (_assignedCatList.Where(d => d.CatId > CurrentCategoryId).Count() > 1)
            //        {
            //            nextAssignedCategoryID = _assignedCatList.Where(d => d.CatId > CurrentCategoryId).FirstOrDefault().CatId;
            //        }
            //        else
            //        {
            //            nextAssignedCategoryID = _assignedCatList.Where(d => d.CatId > CurrentCategoryId).LastOrDefault().CatId;
            //        }
            //    }
            //}
            //else
            //{
            //    prevAssignedCategoryID = AppConsts.NONE;
            //    nextAssignedCategoryID = AppConsts.NONE;
            //}
            #endregion
            HiddenField hdnApplicantReqItemID = this.Parent.FindControl("hdnApplicantReqItemID") as HiddenField; //UAT-4461
            HiddenField hdnPrevCatagoryID = this.Parent.FindControl("hdnPrevCatagoryID") as HiddenField;
            HiddenField hdnNextCatagoryID = this.Parent.FindControl("hdnNextCatagoryID") as HiddenField;
            HiddenField hdnFirstCatagoryID = this.Parent.FindControl("hdnFirstCatagoryID") as HiddenField;
            HiddenField hdnPageType = this.Parent.FindControl("hdnPageType") as HiddenField;
            Int32 prevCatID = AppConsts.NONE;
            Int32 nextCatID = AppConsts.NONE;
            Int32 nextTenantID = AppConsts.NONE;
            Int32 prevTenantID = AppConsts.NONE;
            Int32 nextRotationID = AppConsts.NONE;
            Int32 prevRotationID = AppConsts.NONE;
            Int32 nextSubscriptionID = AppConsts.NONE;
            Int32 prevSubscriptionID = AppConsts.NONE;
            Int32 nextApplicantRequirementItemID = AppConsts.NONE;
            Int32 prevApplicantRequirementItemID = AppConsts.NONE;
            Int32 nextRequirementItemID = AppConsts.NONE;
            Int32 prevRequirementItemID = AppConsts.NONE;
            //Int32 nextRequirementCategoryID = AppConsts.NONE;
            //Int32 prevRequirementCartegoryID = AppConsts.NONE;
            Int32 nextAgencyID = AppConsts.NONE;
            Int32 prevAgencyID = AppConsts.NONE;
            Int32 nextApplicantRequirementCategoryID = AppConsts.NONE;
            Int32 prevApplicantRequirementCategoryID = AppConsts.NONE;
            Int32 nextApplicantID = AppConsts.NONE;
            Int32 prevApplicantID = AppConsts.NONE;
            Int32 CurrentCategoryID = AppConsts.NONE;
            if (CurrentViewContext.ControlUseType.ToLower().Equals(AppConsts.ROTATION_VERIFICATION_QUEUE.ToLower()) && !hdnApplicantReqItemID.IsNullOrEmpty() && hdnApplicantReqItemID.Value != "")
                CurrentViewContext.ApplicantRequirementItemDataId = Convert.ToInt32(hdnApplicantReqItemID.Value);
            ManageReqPkgSubscriptionContract objManageReqPkgSubscriptionContract = SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.MANAGE_REQ_TRI_PANEL_NAVIGATION_SESSION_KEY) as ManageReqPkgSubscriptionContract;
            if (CurrentViewContext.ControlUseType.ToLower().Equals(AppConsts.ROTATION_VERIFICATION_QUEUE.ToLower()) && !CurrentViewContext.IsCurrentARIDRecordAlreadySaved)
                CurrentViewContext.NextPrevNavigationData = objManageReqPkgSubscriptionContract;
            else if (CurrentViewContext.ControlUseType.ToLower().Equals(AppConsts.ROTATION_VERIFICATION_QUEUE.ToLower()))
                objManageReqPkgSubscriptionContract = CurrentViewContext.NextPrevNavigationData;
            if (!objManageReqPkgSubscriptionContract.IsNullOrEmpty())
            {

                var nextSubs = objManageReqPkgSubscriptionContract.NextSubscription;
                var currSubs = objManageReqPkgSubscriptionContract.CurrentSubscription;
                CurrentCategoryID = !currSubs.IsNullOrEmpty() ? currSubs.RequirementCategoryID : 0;
                if (!nextSubs.IsNullOrEmpty())
                {
                    if (nextSubs.NextRequirementCategoryID > AppConsts.NONE)
                    {
                        nextCatID = nextSubs.NextRequirementCategoryID;
                        nextAgencyID = nextSubs.NextAgencyId;
                        nextSubscriptionID = nextSubs.NextSubscriptionID;
                        nextRequirementItemID = nextSubs.NextRequirementItemId;
                        //nextRequirementCategoryID = nextSubs.NextRequirementCategoryID;
                        nextRotationID = nextSubs.NextClinicalRotationID;
                        nextTenantID = nextSubs.NextTenantID;
                        nextApplicantRequirementItemID = nextSubs.NextApplicantRequirementItemId;
                        nextApplicantRequirementCategoryID = nextSubs.NextApplicantRequirementCategoryId;
                        nextApplicantID = nextSubs.NextOrganizationUserID;
                    }
                    else
                    {
                        nextCatID = nextSubs.RequirementCategoryID;
                        nextAgencyID = nextSubs.AgencyId;
                        nextSubscriptionID = nextSubs.RequirementPackageSubscriptionID;
                        nextRequirementItemID = nextSubs.RequirementItemId;
                        //nextRequirementCategoryID = nextSubs.NextRequirementCategoryID;
                        nextRotationID = nextSubs.RotationId;
                        nextTenantID = nextSubs.TenantID;
                        nextApplicantRequirementItemID = nextSubs.ApplicantRequirementItemId;
                        nextApplicantRequirementCategoryID = nextSubs.ApplicantRequirementCategoryId;
                        nextApplicantID = nextSubs.ApplicantId;
                    }
                }


                var prevSubs = objManageReqPkgSubscriptionContract.PreviousSubscription;
                if (!prevSubs.IsNullOrEmpty())
                {
                    if (prevSubs.RequirementCategoryID == currSubs.RequirementCategoryID)
                    {
                        prevCatID = prevSubs.PrevRequirementCategoryID;
                        prevAgencyID = prevSubs.PrevAgencyId;
                        prevSubscriptionID = prevSubs.PrevSubscriptionID;
                        prevRequirementItemID = prevSubs.PrevRequirementItemId;
                        //prevRequirementCategoryID = prevSubs.prevRequirementCategoryID;
                        prevRotationID = prevSubs.PrevClinicalRotationID;
                        prevTenantID = prevSubs.PrevTenantID;
                        prevApplicantRequirementItemID = prevSubs.PrevApplicantRequirementItemId;
                        prevApplicantRequirementCategoryID = prevSubs.PrevApplicantRequirementCategoryId;
                        prevApplicantID = prevSubs.PrevOrganizationUserID;
                    }
                    else
                    {
                        prevCatID = prevSubs.RequirementCategoryID;
                        prevAgencyID = prevSubs.AgencyId;
                        prevSubscriptionID = prevSubs.RequirementPackageSubscriptionID;
                        prevRequirementItemID = prevSubs.RequirementItemId;
                        //prevRequirementCategoryID = prevSubs.prevRequirementCategoryID;
                        prevRotationID = prevSubs.RotationId;
                        prevTenantID = prevSubs.TenantID;
                        prevApplicantRequirementItemID = prevSubs.ApplicantRequirementItemId;
                        prevApplicantRequirementCategoryID = prevSubs.ApplicantRequirementCategoryId;
                        prevApplicantID = prevSubs.ApplicantId;
                    }
                }
            }
            //Start UAT-4461
            if (!CurrentViewContext.ControlUseType.IsNullOrEmpty() &&
               (CurrentViewContext.ControlUseType.ToLower().Equals(AppConsts.ROTATION_VERIFICATION_QUEUE.ToLower())))
            {
                //Note:-Submission Date in Rotation Verification Queue is wrong.So, We may have ticket in future to fix - submission date issue then save and next and save and previous navigation should be test and updated as well.
                Presenter.GetApplicantDataByRPSid();
                //Start UAT-4461
                Int32 NextPendingCatID = 0;
                Int32 PrevPendingCatID = 0;
                var prevSubs = objManageReqPkgSubscriptionContract.PreviousSubscription;
                var nextSubs = objManageReqPkgSubscriptionContract.NextSubscription;
                if (!CurrentViewContext.lstApplicantDataForNavigation.IsNullOrEmpty() && (!hdnPrevCatagoryID.Value.IsNullOrEmpty() || hdnNextCatagoryID.Value.IsNullOrEmpty()))
                {
                    List<Int32> lstDataEnteredCategoryId = CurrentViewContext.lstApplicantDataForNavigation.Select(sel => sel.RequirementCategoryID).Distinct().ToList();
                    Int32 prevCategoryIdIndex = -1;
                    Int32 nextCategoryIdIndex = -1;
                    var lstReqPkgSubDataWithIndex = CurrentViewContext.lstReqPkgSubData.WithIndex();
                    if (hdnPrevCatagoryID.Value != "0")
                        prevCategoryIdIndex = lstReqPkgSubDataWithIndex.Where(con => con.Value.CatId == Convert.ToInt32(hdnPrevCatagoryID.Value)).Select(sel => sel.Index).FirstOrDefault();
                    else if (hdnNextCatagoryID.Value != "0")
                        nextCategoryIdIndex = lstReqPkgSubDataWithIndex.Where(con => con.Value.CatId == Convert.ToInt32(hdnNextCatagoryID.Value)).Select(sel => sel.Index).FirstOrDefault();
                    if (prevCategoryIdIndex < 0 && nextCategoryIdIndex < 0)
                    {
                        PrevPendingCatID = 0;
                        NextPendingCatID = 0;
                    }
                    else
                    {
                        Int32 CurrentItemIndex = -1;
                        if (prevCategoryIdIndex >= 0)
                            CurrentItemIndex = lstReqPkgSubDataWithIndex.Where(cond => cond.Index > prevCategoryIdIndex).FirstOrDefault().IsNullOrEmpty() ? -1 : lstReqPkgSubDataWithIndex.Where(cond => cond.Index > prevCategoryIdIndex).Select(sel => sel.Index).FirstOrDefault();
                        else
                            CurrentItemIndex = lstReqPkgSubDataWithIndex.Where(con => con.Index < nextCategoryIdIndex).LastOrDefault().IsNullOrEmpty() ? -1 : lstReqPkgSubDataWithIndex.Where(con => con.Index < nextCategoryIdIndex).Select(sel => sel.Index).LastOrDefault();

                        var lstCatWithDataEntryExistsWithIndex = lstReqPkgSubDataWithIndex.Where(con => lstDataEnteredCategoryId.Contains(con.Value.CatId)).ToList();
                        PrevPendingCatID = lstCatWithDataEntryExistsWithIndex.Where(cond => cond.Index < CurrentItemIndex).LastOrDefault().IsNotNull() ? lstCatWithDataEntryExistsWithIndex.Where(cond => cond.Index < CurrentItemIndex).LastOrDefault().Value.CatId : 0;
                        NextPendingCatID = lstCatWithDataEntryExistsWithIndex.Where(cond => cond.Index > CurrentItemIndex).FirstOrDefault().IsNotNull() ? lstCatWithDataEntryExistsWithIndex.Where(cond => cond.Index > CurrentItemIndex).FirstOrDefault().Value.CatId : 0;
                    }

                }
                //END UAT-4461

                Dictionary<String, String> prevQueryString = new Dictionary<String, String>();
                prevQueryString = new Dictionary<String, String>
                        {
                            { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
                            { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                            { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(CurrentViewContext.CurrentReqSubsciptionId) },
                            { ProfileSharingQryString.ApplicantId , Convert.ToString(CurrentViewContext.ApplicantId) },
                            {"CategoryID", Convert.ToString(PrevPendingCatID) },
                            {"RotationId",CurrentViewContext.CurrentRotationId.IsNotNull() ? Convert.ToString(CurrentViewContext.CurrentRotationId) : AppConsts.ZERO },
                            {"AgencyId", CurrentViewContext.AgencyId.IsNotNull()? Convert.ToString(CurrentViewContext.AgencyId) : AppConsts.ZERO},
                            {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
                            {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
                              {"SelectedReqComplianceCategoryId",Convert.ToString(PrevPendingCatID)}
                        };
                if (Convert.ToInt32(PrevPendingCatID) > AppConsts.NONE)
                {
                    prevSubscriptionURL = lnkBtnPreviousCategory.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, prevQueryString.ToEncryptedQueryString());
                    imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrow-blue.png";
                    // prevSubscriptionURL = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                }
                else
                {
                    //set seeeion data
                    #region Bind Prev Subs Data
                    if (!objManageReqPkgSubscriptionContract.IsNullOrEmpty())
                    {

                        if (prevCatID > AppConsts.NONE && prevSubs.PrevClinicalRotationID != 0)
                        {
                            Dictionary<String, String> prevqueryString = new Dictionary<String, String>();
                            prevqueryString = new Dictionary<String, String>
                        {

                             { ProfileSharingQryString.SelectedTenantId, Convert.ToString(prevSubs.PrevTenantID) },
                            { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                            { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(prevSubs.PrevSubscriptionID) },
                            { ProfileSharingQryString.ApplicantId , Convert.ToString(prevSubs.PrevOrganizationUserID) },
                            {"CategoryID", Convert.ToString(prevSubs.PrevRequirementCategoryID) },
                            {"RotationId",!prevSubs.PrevClinicalRotationID.IsNullOrEmpty() ? Convert.ToString(prevSubs.PrevClinicalRotationID) : AppConsts.ZERO },
                            {"AgencyId", !prevSubs.PrevAgencyId.IsNullOrEmpty()? Convert.ToString(prevSubs.PrevAgencyId) : AppConsts.ZERO},
                            {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
                            {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
                             {"SelectedReqComplianceCategoryId",Convert.ToString(prevSubs.PrevRequirementCategoryID)},
                              {"SelectedReqPackageSubscriptionId",Convert.ToString( prevSubs.PrevSubscriptionID)},
                               {"ApplicantRequirementItemId", prevSubs.PrevApplicantRequirementItemId.ToString() },
                               {"RequirementItemId", prevSubs.PrevRequirementItemId.ToString() },
                        };
                            prevSubscriptionURL = lnkBtnPreviousCategory.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, prevqueryString.ToEncryptedQueryString());
                            imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrow-blue.png";
                        }
                        else
                        {
                            imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";

                        }
                        #endregion
                        // imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
                    }

                }

                Dictionary<String, String> nxtqueryString = new Dictionary<String, String>();
                nxtqueryString = new Dictionary<String, String>
                        {
                            { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
                            { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                            { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(CurrentViewContext.CurrentReqSubsciptionId) },
                            { ProfileSharingQryString.ApplicantId , Convert.ToString(CurrentViewContext.ApplicantId) },
                            {"CategoryID", Convert.ToString(NextPendingCatID) },
                            {"RotationId",CurrentViewContext.CurrentRotationId.IsNotNull() ? Convert.ToString(CurrentViewContext.CurrentRotationId) : AppConsts.ZERO },
                            {"AgencyId", CurrentViewContext.AgencyId.IsNotNull()? Convert.ToString(CurrentViewContext.AgencyId) : AppConsts.ZERO },
                            {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
                            {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
                              {"SelectedReqComplianceCategoryId",Convert.ToString(NextPendingCatID)}
                        };
                if (Convert.ToInt32(NextPendingCatID) > AppConsts.NONE)
                {
                    nextSubscriptionURL = lnkBtnNextCategory.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, nxtqueryString.ToEncryptedQueryString());
                    imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrow.png";
                    cmdBar.ExtraButton.Enabled = true;
                    btnNext.Enabled = true;
                }
                else
                {
                    #region Bind Next Subs Data
                    if (!objManageReqPkgSubscriptionContract.IsNullOrEmpty())
                    {

                        if (nextCatID > AppConsts.NONE && nextSubs.NextClinicalRotationID != 0)
                        {
                            Dictionary<String, String> nextqueryString = new Dictionary<String, String>();
                            nextqueryString = new Dictionary<String, String>
                                {
                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(nextSubs.NextTenantID) },
                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                                    { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(nextSubs.NextSubscriptionID) },
                                    { ProfileSharingQryString.ApplicantId , Convert.ToString(nextSubs.NextOrganizationUserID) },
                                    {"CategoryID", nextSubs.NextRequirementCategoryID.ToString() },
                                    {"RotationId",!nextSubs.NextClinicalRotationID.IsNullOrEmpty() ? Convert.ToString(nextSubs.NextClinicalRotationID) : AppConsts.ZERO },
                                    {"AgencyId", !nextSubs.NextAgencyId.IsNullOrEmpty()? Convert.ToString(nextSubs.NextAgencyId) : AppConsts.ZERO },
                                    {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
                                    {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
                                     {"SelectedReqComplianceCategoryId",Convert.ToString( nextSubs.NextRequirementItemId)},
                                       {"ApplicantRequirementItemId", nextSubs.NextApplicantRequirementItemId.ToString() },
                                       {"SelectedReqPackageSubscriptionId",Convert.ToString( nextSubs.NextSubscriptionID)},
                                        {"RequirementItemId", nextSubs.NextRequirementItemId.ToString() },
                                };

                            nextSubscriptionURL = lnkBtnNextCategory.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, nextqueryString.ToEncryptedQueryString());
                            imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrow.png";
                            cmdBar.ExtraButton.Enabled = true;
                        }
                        else
                        {
                            imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
                            cmdBar.ExtraButton.Enabled = false;
                        }
                    }



                    #endregion
                    //imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
                    //cmdBar.ExtraButton.Enabled = false;
                }
                if (!objManageReqPkgSubscriptionContract.IsNullOrEmpty())
                {
                    if ((objManageReqPkgSubscriptionContract.NextSubscription.IsNullOrEmpty() && Convert.ToInt32(NextPendingCatID) == AppConsts.NONE) || (!nextSubs.IsNullOrEmpty() && nextSubs.NextClinicalRotationID == 0 && Convert.ToInt32(NextPendingCatID) == AppConsts.NONE))
                    {
                        btnNext.Enabled = false;
                    }
                    else
                    {
                        btnNext.Enabled = true;
                    }

                    if ((objManageReqPkgSubscriptionContract.PreviousSubscription.IsNullOrEmpty() && Convert.ToInt32(PrevPendingCatID) == AppConsts.NONE) || (!prevSubs.IsNullOrEmpty() && prevSubs.PrevClinicalRotationID == 0 && Convert.ToInt32(PrevPendingCatID) == AppConsts.NONE))
                    {
                        btnPrevious.Enabled = false;
                    }
                    else
                    {
                        btnPrevious.Enabled = true;
                    }

                }
                return;

            }
            //END UAT-4461
            if (!CurrentViewContext.ControlUseType.IsNullOrEmpty()
                && (CurrentViewContext.ControlUseType.ToLower().Equals(AppConsts.ROTATION_VERIFICATION_USER_WORK__QUEUE_TYPE_CODE.ToLower()))
                //|| (CurrentViewContext.ControlUseType.ToLower().Equals(AppConsts.ROTATION_VERIFICATION_QUEUE.ToLower())))
                )
            {

                #region Bind Prev Subs Data
                var prevSubs = objManageReqPkgSubscriptionContract.PreviousSubscription;
                #region UAT-4461
                var currSubs = objManageReqPkgSubscriptionContract.CurrentSubscription;
                IEnumerable<Extensions.Item<RequirementVerificationDetailContract>> _requirementCategoryWithIndex = null;
                Int32 CurrentCategoryIndex = AppConsts.NONE;
                Int32 objManageReqPkgSubsCatIndex = AppConsts.NONE;
                if (!CurrentViewContext.lstReqPkgSubData.IsNullOrEmpty() && CurrentViewContext.CurrentReqSubsciptionId == currSubs.RequirementPackageSubscriptionID)
                    _requirementCategoryWithIndex = CurrentViewContext.lstReqPkgSubData.WithIndex();
                if (!_requirementCategoryWithIndex.IsNullOrEmpty() && CurrentCategoryID != CurrentViewContext.CurrentReqCategoryId)
                {
                    CurrentCategoryIndex = _requirementCategoryWithIndex.Where(cond => cond.Value.CatId == CurrentViewContext.CurrentReqCategoryId).Select(sel => sel.Index).FirstOrDefault() + 1;
                    objManageReqPkgSubsCatIndex = _requirementCategoryWithIndex.Where(cond => cond.Value.CatId == CurrentCategoryID).Select(sel => sel.Index).FirstOrDefault() + 1;
                }
                #endregion
                if (!prevSubs.IsNullOrEmpty() && prevSubs.PrevRequirementCategoryID > AppConsts.NONE && prevCatID > AppConsts.NONE)
                {
                    Dictionary<String, String> prevqueryString = new Dictionary<String, String>();
                    prevqueryString = new Dictionary<String, String>
                        {
                            { ProfileSharingQryString.SelectedTenantId, Convert.ToString(prevTenantID) },
                            { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                            { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(prevSubscriptionID) },
                            { ProfileSharingQryString.ApplicantId , Convert.ToString(prevApplicantID) },
                            {"CategoryID", Convert.ToString(prevCatID) },
                            {"RotationId",prevRotationID.IsNotNull() ? Convert.ToString(prevRotationID) : AppConsts.ZERO },
                            {"AgencyId", prevAgencyID.IsNotNull()? Convert.ToString(prevAgencyID) : AppConsts.ZERO},
                            {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
                            {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
                             {"SelectedReqComplianceCategoryId",Convert.ToString(prevCatID)},
                              {"SelectedReqPackageSubscriptionId",Convert.ToString( prevSubscriptionID)},
                               {"ApplicantRequirementItemId", prevApplicantRequirementItemID.ToString() },
                               {"RequirementItemId", prevRequirementItemID.ToString() },
                        };
                    prevSubscriptionURL = lnkBtnPreviousCategory.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, prevqueryString.ToEncryptedQueryString());
                    imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrow-blue.png";

                }
                else
                {
                    imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
                    btnPrevious.Enabled = false;
                }
                #endregion

                #region Bind Next Subs Data

                var nextSubs = objManageReqPkgSubscriptionContract.NextSubscription;
                if (!nextSubs.IsNullOrEmpty() && nextSubs.NextRequirementCategoryID > AppConsts.NONE && nextCatID > AppConsts.NONE)
                {
                    Dictionary<String, String> nextqueryString = new Dictionary<String, String>();
                    nextqueryString = new Dictionary<String, String>
                                {
                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(nextTenantID) },
                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                                    { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(nextSubscriptionID) },
                                    { ProfileSharingQryString.ApplicantId , Convert.ToString(nextApplicantID) },
                                    {"CategoryID", nextCatID.ToString() },
                                    {"RotationId",nextRotationID.IsNotNull() ? Convert.ToString(nextRotationID) : AppConsts.ZERO },
                                    {"AgencyId", nextAgencyID.IsNotNull()? Convert.ToString(nextAgencyID) : AppConsts.ZERO },
                                    {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
                                    {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
                                     {"SelectedReqComplianceCategoryId",Convert.ToString( nextCatID)},
                                       {"ApplicantRequirementItemId", nextApplicantRequirementItemID.ToString() },
                                       {"SelectedReqPackageSubscriptionId",Convert.ToString( nextSubscriptionID)},
                                        {"RequirementItemId", nextRequirementItemID.ToString() },
                                };

                    nextSubscriptionURL = lnkBtnNextCategory.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, nextqueryString.ToEncryptedQueryString());
                    imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrow.png";
                    cmdBar.ExtraButton.Enabled = true;
                }
                else
                {
                    imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
                    cmdBar.ExtraButton.Enabled = false;
                    btnNext.Enabled = false;
                }
                #region UAT-4461
                if (CurrentCategoryIndex > 0 && objManageReqPkgSubsCatIndex > 0)
                {
                    if (CurrentCategoryIndex > objManageReqPkgSubsCatIndex)
                    {
                        Dictionary<String, String> prevqueryString = new Dictionary<String, String>();
                        prevqueryString = new Dictionary<String, String>
                        {
                            { ProfileSharingQryString.SelectedTenantId, Convert.ToString(currSubs.TenantID) },
                            { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                            { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(currSubs.RequirementPackageSubscriptionID) },
                            { ProfileSharingQryString.ApplicantId , Convert.ToString(currSubs.ApplicantId) },
                            {"CategoryID", Convert.ToString(currSubs.RequirementCategoryID) },
                            {"RotationId",prevRotationID.IsNotNull() ? Convert.ToString(currSubs.RotationId) : AppConsts.ZERO },
                            {"AgencyId", prevAgencyID.IsNotNull()? Convert.ToString(currSubs.AgencyId) : AppConsts.ZERO},
                            {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
                            {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
                             {"SelectedReqComplianceCategoryId",Convert.ToString(currSubs.RequirementCategoryID)},
                              {"SelectedReqPackageSubscriptionId",Convert.ToString( currSubs.RequirementPackageSubscriptionID)},
                               {"ApplicantRequirementItemId", currSubs.ApplicantRequirementItemId.ToString() },
                               {"RequirementItemId", currSubs.RequirementItemId.ToString() },
                        };
                        prevSubscriptionURL = lnkBtnPreviousCategory.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, prevqueryString.ToEncryptedQueryString());
                    }
                    else
                    {
                        Dictionary<String, String> nextqueryString = new Dictionary<String, String>();
                        nextqueryString = new Dictionary<String, String>
                        {
                            { ProfileSharingQryString.SelectedTenantId, Convert.ToString(currSubs.TenantID) },
                            { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                            { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(currSubs.RequirementPackageSubscriptionID) },
                            { ProfileSharingQryString.ApplicantId , Convert.ToString(currSubs.ApplicantId) },
                            {"CategoryID", Convert.ToString(currSubs.RequirementCategoryID) },
                            {"RotationId",prevRotationID.IsNotNull() ? Convert.ToString(currSubs.RotationId) : AppConsts.ZERO },
                            {"AgencyId", prevAgencyID.IsNotNull()? Convert.ToString(currSubs.AgencyId) : AppConsts.ZERO},
                            {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
                            {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
                             {"SelectedReqComplianceCategoryId",Convert.ToString(currSubs.RequirementCategoryID)},
                              {"SelectedReqPackageSubscriptionId",Convert.ToString( currSubs.RequirementPackageSubscriptionID)},
                               {"ApplicantRequirementItemId", currSubs.ApplicantRequirementItemId.ToString() },
                               {"RequirementItemId", currSubs.RequirementItemId.ToString() },
                        };
                        nextSubscriptionURL = lnkBtnNextCategory.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, nextqueryString.ToEncryptedQueryString());
                    }
                }
                #endregion
                //if (Convert.ToInt32(hdnNextCatagoryID.Value) == AppConsts.NONE)
                //{
                //    btnNext.Enabled = false;
                //    imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
                //}
                //else
                //{
                //    btnNext.Enabled = true;
                //    imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrow.png";
                //}
                //if (Convert.ToInt32(hdnPrevCatagoryID.Value) == AppConsts.NONE)
                //{
                //    btnPrevious.Enabled = false;
                //    imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
                //}
                //else
                //{
                //    btnPrevious.Enabled = true;
                //    imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrow-blue.png";
                //}
                #endregion

                return;
            }




            if (hdnPrevCatagoryID.IsNotNull() && hdnNextCatagoryID.IsNotNull())
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                        {
                            { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
                            { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                            { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(CurrentViewContext.CurrentReqSubsciptionId) },
                            { ProfileSharingQryString.ApplicantId , Convert.ToString(CurrentViewContext.ApplicantId) },
                            {"CategoryID", Convert.ToString(hdnPrevCatagoryID.Value) },
                            {"RotationId",CurrentViewContext.CurrentRotationId.IsNotNull() ? Convert.ToString(CurrentViewContext.CurrentRotationId) : AppConsts.ZERO },
                            {"AgencyId", CurrentViewContext.AgencyId.IsNotNull()? Convert.ToString(CurrentViewContext.AgencyId) : AppConsts.ZERO},
                            {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
                            {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
                              {"SelectedReqComplianceCategoryId",Convert.ToString(hdnPrevCatagoryID.Value)}
                        };
                if (Convert.ToInt32(hdnPrevCatagoryID.Value) > AppConsts.NONE)
                {
                    prevSubscriptionURL = lnkBtnPreviousCategory.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                    imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrow-blue.png";
                    // prevSubscriptionURL = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                }
                else
                {
                    //set seeeion data
                    #region Bind Prev Subs Data
                    if (!objManageReqPkgSubscriptionContract.IsNullOrEmpty())
                    {
                        var prevSubs = objManageReqPkgSubscriptionContract.PreviousSubscription;
                        if (prevCatID > AppConsts.NONE)
                        {
                            Dictionary<String, String> prevqueryString = new Dictionary<String, String>();
                            prevqueryString = new Dictionary<String, String>
                        {
                            { ProfileSharingQryString.SelectedTenantId, Convert.ToString(prevSubs.TenantID) },
                            { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                            { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(prevSubs.RequirementPackageSubscriptionID) },
                            { ProfileSharingQryString.ApplicantId , Convert.ToString(prevSubs.ApplicantId) },
                            {"CategoryID", Convert.ToString(prevCatID) },
                            {"RotationId",prevSubs.RotationId.IsNotNull() ? Convert.ToString(prevSubs.RotationId) : AppConsts.ZERO },
                            {"AgencyId", prevSubs.AgencyId.IsNotNull()? Convert.ToString(prevSubs.AgencyId) : AppConsts.ZERO},
                            {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
                            {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
                             {"SelectedReqComplianceCategoryId",Convert.ToString(prevCatID)},
                              {"SelectedReqPackageSubscriptionId",Convert.ToString( prevSubs.RequirementPackageSubscriptionID)},
                               {"ApplicantRequirementItemId", prevSubs.ApplicantRequirementItemId.ToString() },
                               {"RequirementItemId", prevSubs.RequirementItemId.ToString() },
                        };
                            prevSubscriptionURL = lnkBtnPreviousCategory.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, prevqueryString.ToEncryptedQueryString());
                            imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrow-blue.png";

                        }
                        else
                        {
                            imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
                        }
                        #endregion
                        // imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
                    }
                }


            }

            if (hdnPrevCatagoryID.IsNotNull() && hdnNextCatagoryID.IsNotNull())
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                        {
                            { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
                            { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                            { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(CurrentViewContext.CurrentReqSubsciptionId) },
                            { ProfileSharingQryString.ApplicantId , Convert.ToString(CurrentViewContext.ApplicantId) },
                            {"CategoryID", Convert.ToString(hdnNextCatagoryID.Value) },
                            {"RotationId",CurrentViewContext.CurrentRotationId.IsNotNull() ? Convert.ToString(CurrentViewContext.CurrentRotationId) : AppConsts.ZERO },
                            {"AgencyId", CurrentViewContext.AgencyId.IsNotNull()? Convert.ToString(CurrentViewContext.AgencyId) : AppConsts.ZERO },
                            {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
                            {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
                              {"SelectedReqComplianceCategoryId",Convert.ToString(hdnNextCatagoryID.Value)}
                        };
                if (Convert.ToInt32(hdnNextCatagoryID.Value) > AppConsts.NONE)
                {
                    nextSubscriptionURL = lnkBtnNextCategory.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
                    imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrow.png";
                    cmdBar.ExtraButton.Enabled = true;
                    btnNext.Enabled = true;
                }
                else
                {
                    #region Bind Next Subs Data
                    if (!objManageReqPkgSubscriptionContract.IsNullOrEmpty())
                    {
                        var nextSubs = objManageReqPkgSubscriptionContract.NextSubscription;
                        if (nextCatID > AppConsts.NONE)
                        {
                            Dictionary<String, String> nextqueryString = new Dictionary<String, String>();
                            nextqueryString = new Dictionary<String, String>
                                {
                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(nextSubs.TenantID) },
                                    { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
                                    { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(nextSubs.RequirementPackageSubscriptionID) },
                                    { ProfileSharingQryString.ApplicantId , Convert.ToString(nextSubs.ApplicantId) },
                                    {"CategoryID", nextCatID.ToString() },
                                    {"RotationId",nextSubs.RotationId.IsNotNull() ? Convert.ToString(nextSubs.RotationId) : AppConsts.ZERO },
                                    {"AgencyId", nextSubs.AgencyId.IsNotNull()? Convert.ToString(nextSubs.AgencyId) : AppConsts.ZERO },
                                    {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
                                    {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
                                      {"SelectedReqComplianceCategoryId",Convert.ToString( nextCatID)},
                                       {"ApplicantRequirementItemId", nextSubs.ApplicantRequirementItemId.ToString() },
                                       {"SelectedReqPackageSubscriptionId",Convert.ToString( nextSubs.RequirementPackageSubscriptionID)},
                                        {"RequirementItemId", nextSubs.RequirementItemId.ToString() },
                                };

                            nextSubscriptionURL = lnkBtnNextCategory.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, nextqueryString.ToEncryptedQueryString());
                            imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrow.png";
                            cmdBar.ExtraButton.Enabled = true;
                        }
                    }
                    else
                    {
                        imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
                        cmdBar.ExtraButton.Enabled = false;
                    }


                    #endregion
                    //imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
                    //cmdBar.ExtraButton.Enabled = false;
                }

                if (!objManageReqPkgSubscriptionContract.IsNullOrEmpty())
                {
                    if (objManageReqPkgSubscriptionContract.NextSubscription.IsNullOrEmpty() && Convert.ToInt32(hdnNextCatagoryID.Value) == AppConsts.NONE)
                    {
                        btnNext.Enabled = false;
                    }
                    else
                    {
                        btnNext.Enabled = true;
                    }

                    if (objManageReqPkgSubscriptionContract.PreviousSubscription.IsNullOrEmpty() && Convert.ToInt32(hdnPrevCatagoryID.Value) == AppConsts.NONE)
                    {
                        btnPrevious.Enabled = false;
                    }
                    else
                    {
                        btnPrevious.Enabled = true;
                    }
                }
            }
            if (CurrentViewContext.ControlUseType == null)
            {
                lnkBtnNextCategory.HRef = String.Empty;
                lnkBtnPreviousCategory.HRef = String.Empty;
                lnkBtnPreviousCategory.Disabled = true;
                lnkBtnNextCategory.Disabled = true;
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
                imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
            }
            //if (!CurrentViewContext.ControlUseType.IsNullOrEmpty() && !(CurrentViewContext.ControlUseType.ToLower().Equals(AppConsts.ROTATION_VERIFICATION_USER_WORK__QUEUE_TYPE_CODE.ToLower())
            //     ||CurrentViewContext.ControlUseType.ToLower().Equals(AppConsts.ASSIGN_ROTATION_VERIFICATION_QUEUE_TYPE_CODE.ToLower())))
            //{
            //    lnkBtnNextCategory.HRef = String.Empty;
            //    lnkBtnPreviousCategory.HRef = String.Empty;
            //    lnkBtnPreviousCategory.Disabled = true;
            //    lnkBtnNextCategory.Disabled = true;
            //    btnNext.Enabled = false;
            //    btnPrevious.Enabled = false;
            //    imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
            //    imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
            //}
            if (Convert.ToInt32(hdnPrevCatagoryID.Value) == AppConsts.NONE)
            {
                lnkBtnPreviousCategory.Disabled = true;
                imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
                lnkBtnPreviousCategory.HRef = String.Empty;
            }
            if (Convert.ToInt32(hdnNextCatagoryID.Value) == AppConsts.NONE)
            {
                lnkBtnNextCategory.Disabled = true;
                imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
                lnkBtnNextCategory.HRef = String.Empty;
            }
            //if (objManageReqPkgSubscriptionContract == null)
            //{
            //    lnkBtnPreviousCategory.Disabled = true;
            //    imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
            //    lnkBtnPreviousCategory.HRef = String.Empty;
            //    lnkBtnNextCategory.Disabled = true;
            //    imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
            //    lnkBtnNextCategory.HRef = String.Empty;
            //    btnNext.Enabled = false;
            //    btnPrevious.Enabled = false;
            //}
            //UAT-3528
            if (!CurrentViewContext.ControlUseType.IsNullOrEmpty() && (CurrentViewContext.ControlUseType == AppConsts.ROTATION_DETAIL_USE_TYPE_CODE.ToLower()
                || CurrentViewContext.ControlUseType == AppConsts.SUPPORT_PORTAL_DETAIL_USE_TYPE_CODE.ToLower()
                || CurrentViewContext.ControlUseType == AppConsts.ROTATION_PORTFOLIO_SEARCH_USE_TYPE_CODE.ToLower()
                || CurrentViewContext.ControlUseType == AppConsts.ROTATION_MEMBER_SEARCH_USE_TYPE_CODE.ToLower()
                 || CurrentViewContext.ControlUseType == AppConsts.SUPPORT_PORTAL_DETAIL_INSTRUCTOR_USE_TYPE_CODE.ToLower()))
            {
                if (hdnPrevCatagoryID.IsNotNull())
                {
                    if (Convert.ToInt32(hdnPrevCatagoryID.Value) == AppConsts.NONE)
                        btnPrevious.Enabled = false;
                    else
                        btnPrevious.Enabled = true;
                }
                if (hdnNextCatagoryID.IsNotNull())
                {
                    if (Convert.ToInt32(hdnNextCatagoryID.Value) == AppConsts.NONE)
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
            }
            else if (objManageReqPkgSubscriptionContract == null)
            {
                lnkBtnPreviousCategory.Disabled = true;
                imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
                lnkBtnPreviousCategory.HRef = String.Empty;
                lnkBtnNextCategory.Disabled = true;
                imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
                lnkBtnNextCategory.HRef = String.Empty;
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
            }
        }
        private void OldHandleNextPrevNavigation()
        {


            //HiddenField hdnPrevCatagoryID = this.Parent.FindControl("hdnPrevCatagoryID") as HiddenField;
            //HiddenField hdnNextCatagoryID = this.Parent.FindControl("hdnNextCatagoryID") as HiddenField;
            //HiddenField hdnPageType = this.Parent.FindControl("hdnPageType") as HiddenField;
            //if (hdnPrevCatagoryID.IsNotNull() && hdnNextCatagoryID.IsNotNull())
            //{
            //    String selectedPrevCatId = !Session["PrevCategoryID"].IsNullOrEmpty() && Convert.ToInt32(Session["PrevCategoryID"]) > AppConsts.NONE ? Session["PrevCategoryID"].ToString() : hdnPrevCatagoryID.Value;
            //    Dictionary<String, String> queryString = new Dictionary<String, String>();
            //    queryString = new Dictionary<String, String>
            //            {
            //                { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
            //                { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
            //                { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(CurrentViewContext.CurrentReqSubsciptionId) },
            //                { ProfileSharingQryString.ApplicantId , Convert.ToString(CurrentViewContext.ApplicantId) },
            //                {"CategoryID", Convert.ToString(CurrentViewContext.CurrentReqCategoryId)},//Convert.ToString(hdnPrevCatagoryID.Value) },
            //                {"RotationId",CurrentViewContext.CurrentRotationId.IsNotNull() ? Convert.ToString(CurrentViewContext.CurrentRotationId) : AppConsts.ZERO },
            //                {"AgencyId", CurrentViewContext.AgencyId.IsNotNull()? Convert.ToString(CurrentViewContext.AgencyId) : AppConsts.ZERO},
            //                {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
            //                {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
            //                 {"SelectedReqComplianceCategoryId",selectedPrevCatId}
            //            };
            //    if (Convert.ToInt32(hdnPrevCatagoryID.Value) > AppConsts.NONE && !CurrentViewContext.ControlUseType.IsNullOrEmpty() && !CurrentViewContext.ControlUseType.ToLower().Equals(AppConsts.ROTATION_VERIFICATION_USER_WORK__QUEUE_TYPE_CODE.ToLower()))
            //    {
            //        lnkBtnPreviousCategory.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
            //        imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrow-blue.png";
            //        prevCategoryURL = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
            //        prevSubscriptionURL = prevCategoryURL;
            //    }
            //    else
            //    {
            //        if (Convert.ToInt32(Session["PrevReqPackageSubscriptionID"]) != AppConsts.NONE)
            //        {
            //            String PrevCatId = !Session["PrevCategoryID"].IsNullOrEmpty() && Convert.ToInt32(Session["PrevCategoryID"]) > AppConsts.NONE ? Session["PrevCategoryID"].ToString() : hdnPrevCatagoryID.Value;

            //            Dictionary<String, String> prevqueryString = new Dictionary<String, String>();
            //            prevqueryString = new Dictionary<String, String>
            //            {
            //                { ProfileSharingQryString.SelectedTenantId, Convert.ToString(Session["PrevTenantID"]) },
            //                { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
            //                { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(Session["PrevReqPackageSubscriptionID"]) },
            //                { ProfileSharingQryString.ApplicantId , Convert.ToString(Session["PrevApplicantID"]) },
            //                {"RotationId",Convert.ToString(Session["PrevRotationID"])},
            //                {"AgencyId", CurrentViewContext.AgencyId.IsNotNull()? Convert.ToString(CurrentViewContext.AgencyId) : AppConsts.ZERO },
            //                {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
            //                {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
            //                 {"SelectedReqComplianceCategoryId",PrevCatId}
            //            };

            //            lnkBtnPreviousCategory.HRef = String.Empty;
            //            prevSubscriptionURL = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, prevqueryString.ToEncryptedQueryString());
            //            imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
            //            lnkBtnPreviousCategory.Disabled = true;
            //            //cmdBar.ExtraButton.Text = "Save and Next Subscription";
            //        }
            //        else
            //        {
            //            lnkBtnPreviousCategory.HRef = String.Empty;
            //            //cmdBar.ExtraButton.Enabled = false;
            //            imgPreviousCategory.ImageUrl = "~/Resources/Mod/Compliance/images/left-arrowd.png";
            //            lnkBtnPreviousCategory.Disabled = true;
            //            btnPrevious.Enabled = false;
            //        }
            //    }
            //}

            //if (hdnPrevCatagoryID.IsNotNull() && hdnNextCatagoryID.IsNotNull())
            //{
            //    String selectedNextCatID = !Session["NextCategoryID"].IsNullOrEmpty() && Convert.ToInt32(Session["NextCategoryID"]) > AppConsts.NONE ? Session["NextCategoryID"].ToString() : hdnNextCatagoryID.Value;

            //    Dictionary<String, String> queryString = new Dictionary<String, String>();
            //    queryString = new Dictionary<String, String>
            //            {
            //                { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
            //                { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
            //                { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(CurrentViewContext.CurrentReqSubsciptionId) },
            //                { ProfileSharingQryString.ApplicantId , Convert.ToString(CurrentViewContext.ApplicantId) },
            //                  {"CategoryID", Convert.ToString(CurrentViewContext.CurrentReqCategoryId)},//Convert.ToString(hdnNextCatagoryID.Value) },
            //                {"RotationId",CurrentViewContext.CurrentRotationId.IsNotNull() ? Convert.ToString(CurrentViewContext.CurrentRotationId) : AppConsts.ZERO },
            //                {"AgencyId", CurrentViewContext.AgencyId.IsNotNull()? Convert.ToString(CurrentViewContext.AgencyId) : AppConsts.ZERO },
            //                {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
            //                {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
            //                 {"SelectedReqComplianceCategoryId",selectedNextCatID}
            //            };
            //    if (Convert.ToInt32(hdnNextCatagoryID.Value) > AppConsts.NONE && !CurrentViewContext.ControlUseType.IsNullOrEmpty() && !CurrentViewContext.ControlUseType.ToLower().Equals(AppConsts.ROTATION_VERIFICATION_USER_WORK__QUEUE_TYPE_CODE.ToLower()))
            //    {
            //        lnkBtnNextCategory.HRef = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
            //        imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrow.png";
            //        nextCategoryURL = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
            //        nextSubscriptionURL = nextCategoryURL;
            //        //cmdBar.ExtraButton.Enabled = true;
            //    }
            //    else
            //    {
            //        if (Convert.ToInt32(Session["NextReqPackageSubscriptionID"]) != AppConsts.NONE)
            //        {
            //            String nextCatID = !Session["NextCategoryID"].IsNullOrEmpty() ? Session["NextCategoryID"].ToString() : hdnNextCatagoryID.Value;
            //            Dictionary<String, String> nextqueryString = new Dictionary<String, String>();
            //            nextqueryString = new Dictionary<String, String>
            //            {
            //                { ProfileSharingQryString.SelectedTenantId, Convert.ToString(Session["NextTenantID"]) },
            //                { "Child",  AppConsts.REQUIREMENT_VERIFICATION_DETAIL__NEW_CONTROL},
            //                { ProfileSharingQryString.ReqPkgSubscriptionId, Convert.ToString(Session["NextReqPackageSubscriptionID"]) },
            //                { ProfileSharingQryString.ApplicantId , Convert.ToString(Session["NextApplicantID"]) },
            //                {"RotationId",Convert.ToString(Session["NextRotationID"])},
            //                {"AgencyId", CurrentViewContext.AgencyId.IsNotNull()? Convert.ToString(CurrentViewContext.AgencyId) : AppConsts.ZERO },
            //                {ProfileSharingQryString.ControlUseType, CurrentViewContext.ControlUseType.IsNotNull() ? CurrentViewContext.ControlUseType : String.Empty},
            //                {"PageType", hdnPageType.Value.IsNotNull() ? hdnPageType.Value : String.Empty},
            //                 {"SelectedReqComplianceCategoryId",nextCatID}
            //            };

            //            lnkBtnNextCategory.HRef = String.Empty;
            //            imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
            //            nextSubscriptionURL = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, nextqueryString.ToEncryptedQueryString());
            //            lnkBtnNextCategory.Disabled = true;
            //            //cmdBar.ExtraButton.Text = "Save and Next Subscription";
            //        }
            //        else
            //        {
            //            //cmdBar.ExtraButton.Enabled = false;
            //            lnkBtnNextCategory.HRef = String.Empty;
            //            imgNextCategory.ImageUrl = "~/Resources/Mod/Compliance/images/right-arrowd.png";
            //            lnkBtnNextCategory.Disabled = true;
            //            btnNext.Enabled = false;
            //        }
            //    }
            //}
        }

        #endregion

        #region Private Methods

        private void ReturnToRotationMemberSearchQueue()
        {
            var queryString = new Dictionary<String, String>();

            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
                                                                    { "Child",  @"~\ClinicalRotation\RotationMemberSearch.ascx"}
                                                                 };
            string url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        /// <summary>
        /// UAT-1800: Return to portfolio search details screens
        /// </summary>
        private void ReturnToPortfolioDetailScreen()
        {
            var queryString = new Dictionary<String, String>();

            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TenantId", Convert.ToString(CurrentViewContext.TenantId) },
                                                                    { "OrganizationUserId", Convert.ToString(CurrentViewContext.ApplicantId) },
                                                                    { "Child",  @"~\SearchUI\UserControl\ApplicantPortfolioDetails.ascx"},
                                                                    {"PageType", WorkQueueType.ApplicantPortFolioSearch.ToString()}
                                                                 };
            string url = String.Format("~/SearchUI/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        /// <summary>
        /// Redirect the user back to Queue.
        /// </summary>
        private void RedirectToQueue()
        {
            var queryString = new Dictionary<String, String>();

            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { ProfileSharingQryString.SelectedTenantId, Convert.ToString(CurrentViewContext.TenantId) },
                                                                    { "Child",  @"~\ClinicalRotation\RequirementVerificationQueue.ascx"}
                                                                 };
            string url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", String.Empty, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        /// <summary>
        /// Save the data of the Verification details screen.
        /// </summary>
        private Tuple<Boolean, Dictionary<Int32, String>> SaveData()
        {
            CurrentViewContext.DataToSave = new RequirementVerificationData();
            CurrentViewContext.DataToSave.lstData = new List<RequirementVerificationCategoryData>();
            CurrentViewContext.DataToSave.RPSId = CurrentViewContext.CurrentReqSubsciptionId;
            Boolean _isDocFieldValidationFailed = false;

            var _ctrl = pnlLoader.FindServerControlRecursively(CurrentViewContext.ReqItemDataLoaderControlId);

            if (_ctrl.IsNotNull() && _ctrl is RequirementItemDataLoader)
            {
                var _loaderControl = _ctrl as RequirementItemDataLoader;
                CurrentViewContext.DataToSave.lstData.Add(_loaderControl.GetItemData());
            }

            //UAT-2224: Admin access to upload/associate documents on rotation package items.
            foreach (var requirementVerificationCategoryData in CurrentViewContext.DataToSave.lstData)
            {
                if (requirementVerificationCategoryData.lstItemData.Any(x => x.IsDocFieldValidationFailed))
                {
                    _isDocFieldValidationFailed = true;
                }
                if (CurrentViewContext.ControlUseType.ToLower().Equals(AppConsts.ROTATION_VERIFICATION_QUEUE.ToLower()))
                {
                    var lstItemData = requirementVerificationCategoryData.lstItemData;
                    var currentARIDData = lstItemData.Where(sel => sel.ApplicantItemDataId == CurrentViewContext.ApplicantRequirementItemDataId).FirstOrDefault();
                    if (!currentARIDData.IsNullOrEmpty() && (currentARIDData.ItemStatusCode == RequirementItemStatus.APPROVED.GetStringValue() || currentARIDData.ItemStatusCode == RequirementItemStatus.NOT_APPROVED.GetStringValue()))
                        CurrentViewContext.IsCurrentARIDRecordAlreadySaved = true;
                }
            }

            Dictionary<Int32, String> saveResponse = new Dictionary<Int32, string>();
            if (!_isDocFieldValidationFailed)
            {
                saveResponse = Presenter.SaveData();
            }
            return new Tuple<Boolean, Dictionary<Int32, String>>(_isDocFieldValidationFailed, saveResponse);
        }

        /// <summary>
        /// Enable/Disable all validations
        /// </summary>
        private void EnableDisableAllValidations()
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "EnableDisableAllValidations();", true);
        }

        /// <summary>
        /// Get Query string values
        /// </summary>
        private void CaptureQueryString()
        {
            var args = new Dictionary<String, String>();

            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);

                if (args.ContainsKey(ProfileSharingQryString.SelectedTenantId))
                {
                    CurrentViewContext.TenantId = Convert.ToInt32(args[ProfileSharingQryString.SelectedTenantId]);
                }

                if (args.ContainsKey(ProfileSharingQryString.ApplicantId))
                {
                    CurrentViewContext.ApplicantId = Convert.ToInt32(args[ProfileSharingQryString.ApplicantId]);
                }

                if (args.ContainsKey(ProfileSharingQryString.ReqPkgSubscriptionId))
                {
                    CurrentViewContext.CurrentReqSubsciptionId = Convert.ToInt32(args[ProfileSharingQryString.ReqPkgSubscriptionId]);
                }

                if (args.ContainsKey(ProfileSharingQryString.ControlUseType))
                {
                    CurrentViewContext.ControlUseType = args[ProfileSharingQryString.ControlUseType].ToLower().Trim();
                }

                if (args.ContainsKey("CategoryID"))
                {

                    if (Convert.ToInt32(args["CategoryID"]) > AppConsts.NONE)
                    {
                        CurrentViewContext.CurrentReqCategoryId = Convert.ToInt32(args["CategoryID"]);
                    }
                }
                else
                {
                    HiddenField hdnFirstCatagoryID = this.Parent.FindControl("hdnFirstCatagoryID") as HiddenField;
                    if (hdnFirstCatagoryID.IsNotNull())
                    {
                        CurrentViewContext.CurrentReqCategoryId = Convert.ToInt32(hdnFirstCatagoryID.Value);
                    }
                }

                if (args.ContainsKey("RotationId"))
                {
                    CurrentViewContext.CurrentRotationId = Convert.ToInt32(args["RotationId"]);
                }

                //UAT-3049
                //if (args.ContainsKey(ProfileSharingQryString.RotationId))
                //{
                //    rotationId = Convert.ToInt32(args[ProfileSharingQryString.RotationId]);
                //}
                if (args.ContainsKey("AgencyId"))
                {
                    CurrentViewContext.AgencyId = Convert.ToInt32(args["AgencyId"]);
                }
            }
        }

        #endregion

        protected void cmdBar_ExtraClick(object sender, EventArgs e)
        {
            try
            {
                #region UAT-3345
                Object control = pnlLoader.FindServerControlRecursively(CurrentViewContext.ReqItemDataLoaderControlId);
                var loadercontrol = control as RequirementItemDataLoader;
                if (loadercontrol.IsNotNull())
                {
                    loadercontrol.saveDocumentMappings();
                }
                #endregion

                //UAT-2224: Admin access to upload/associate documents on rotation package items.
                //2366-Add the ability for "Item 1 must be dated before/after item 2" ui rules in rotation packages.
                Tuple<Boolean, Dictionary<Int32, String>> saveResponse = SaveData();

                Boolean _isDocFieldValidationFailed = saveResponse.Item1;
                if (_isDocFieldValidationFailed)
                {
                    EnableDisableAllValidations();
                }
                else
                {
                    Dictionary<Int32, String> dicResponse = saveResponse.Item2;
                    Boolean isSuccess = !dicResponse.Keys.Any();
                    String errorMsg = dicResponse.Values.FirstOrDefault();

                    if (isSuccess)
                    {
                        if (!nextSubscriptionURL.IsNullOrEmpty())
                            Response.Redirect(nextSubscriptionURL, true);
                    }
                    else
                    {
                        ShowErrorMessages(dicResponse);
                        EnableDisableAllValidations();
                    }
                }
            }
            catch (Exception ex)
            {
                base.ShowInfoMessage("Data could not be saved.");
                base.LogError(ex);
            }
        }



    }
}