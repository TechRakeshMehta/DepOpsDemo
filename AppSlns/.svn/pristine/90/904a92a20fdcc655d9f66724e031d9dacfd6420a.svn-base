#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Linq;


#endregion

#region UserDefined

using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.Shell.Views;
using System.Threading;
using CoreWeb.IntsofSecurityModel;
using System.Xml.Linq;
using INTSOF.UI.Contract.BkgOperations;
using CoreWeb.BkgOperations.Views;
using INTSOF.UI.Contract.BkgOperations;
using System.Web.UI;
using System.Web.Configuration;
using INTSOF.UI.Contract.FingerPrintSetup;
using System.Text;
using INTSOF.UI.Contract.Globalization;
using System.Globalization;
using ExternalVendors.ClearStarVendor;
using INTSOF.UI.Contract;
using INTSOF.Contracts;
using Business.RepoManagers;
using INTSOF.UI.Contract.Templates;



#endregion

#endregion

namespace CoreWeb.AdminEntryPortal.Views
{
    public partial class AdminEntryOrderReview : BaseUserControl, IAdminEntryOrderReviewView
    {
        #region Variables

        #region Private Variables

        private Int32 _tenantId;
        private OrganizationUserProfile _orgUserProfile;
        private AdminEntryOrderReviewPresenter _presenter = new AdminEntryOrderReviewPresenter();
        OrganizationUserProfile _organizationUserProfile;
        private ApplicantOrderCart _applicantOrderCart;
        private ApplicantOrderCart applicantOrderCart = new ApplicantOrderCart();
        private Guid LCNAttCode = new Guid("515BEF57-9072-4D2A-A97A-0C248BB045F9");////License Number
        private Guid MotherNameAttrCode = new Guid("3DA8912A-6337-4B8F-93C4-88BFC3032D2D");////Mother's Maiden Name
        private Guid IdentificationNumberAttrCode = new Guid("AAB51E52-2A9B-42AB-9A9D-D1AFFC18E211");////Identification Number

        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties

        private Int32 MVRDvrLicenseNumberID
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                return applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberID;
            }
        }

        private Int32 MVRDvrLicenseNumberStateID
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                return applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberStateID;
            }
        }

        private List<PreviousAddressContract> lstResendialHistory
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                return applicantOrderCart.lstPrevAddresses;
            }
        }

        private Boolean IsResidentialHistoryVisible
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                return applicantOrderCart.IsResidentialHistoryVisible;
            }
        }


        private Dictionary<Int32, String> GetDataForMVR
        {
            get
            {
                return lstBackgroundOrderData.FirstOrDefault(x => x.CustomFormId == AppConsts.ONE && x.InstanceId == AppConsts.ONE).CustomFormData;
            }
        }

        Boolean IAdminEntryOrderReviewView.IsSSNDisabled
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsSSNDisabled"] ?? false);
            }
            set
            {
                ViewState["IsSSNDisabled"] = value;
            }
        }

        #region UAT-2212: Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality

        private String NoMiddleNameText
        {
            get
            {
                String noMiddleNameText = String.Empty;
                if (!CurrentViewContext.IsLocationServiceTenant)
                    noMiddleNameText = AppConsts.NO_MIDDLE_NAME_TEXT_AEP;
                if (noMiddleNameText.IsNull())
                {
                    noMiddleNameText = String.Empty;
                }
                return noMiddleNameText;
            }
        }
        #endregion
        #endregion

        #region Public Properties

        public Int32 GeneratedOrderId
        {
            get;
            set;
        }


        #region Custom Form

        /// <summary>
        /// Needed For custom Form 
        /// </summary>
        public List<BackgroundPackagesContract> lstPackages
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (applicantOrderCart.IsNotNull())
                {
                    if (applicantOrderCart.lstApplicantOrder.IsNotNull())
                    {
                        return applicantOrderCart.lstApplicantOrder[0].lstPackages;
                    }
                }
                return new List<BackgroundPackagesContract>();
            }
        }

        public List<BackgroundOrderData> lstBackgroundOrderData
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                return applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData;
            }
        }


        public List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }

        #endregion


        public AdminEntryOrderReviewPresenter Presenter
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

        public List<lkpPaymentOption> lstPaymentOptions
        {
            get;
            set;
        }

        public Boolean ShowRushOrderForInvioce
        {
            get
            {
                return (Boolean)(ViewState["ShowRushOrderForInvioce"] ?? "0");
            }
            set
            {
                ViewState["ShowRushOrderForInvioce"] = value;
            }
        }

        public Int32 PaymentMode_InvoiceId
        {
            get
            {
                return (Int32)(ViewState["PaymentMode_InvoiceId"] ?? "0");
            }
            set
            {
                ViewState["PaymentMode_InvoiceId"] = value;
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set
            {
                _tenantId = value;
            }
        }

        public IAdminEntryOrderReviewView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 ZipCodeId
        {
            get;
            set;
        }

        public Entity.ZipCode ApplicantZipCodeDetails
        {
            get;
            set;
        }

        public Int32 DPPSId
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public String Gender
        {
            get;
            set;
        }

        public Int32 GenderId
        {
            get;
            set;
        }

        public String PaymentModeCode
        {
            get;
            set;
        }

        public Boolean UpdateOriginalData
        {
            get;
            set;
        }

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        public String NextPagePath
        {
            get;
            set;
        }

        public Boolean ShowRushOrder
        {
            get
            {
                return (Boolean)(ViewState["ShowRushOrder"] ?? "0");
            }
            set
            {
                ViewState["ShowRushOrder"] = value;
            }
        }
        public List<PersonAliasContract> PersonAliasList
        {
            get
            {
                return ucPersonAlias.PersonAliasList;
            }
            set
            {
                ucPersonAlias.PersonAliasList = value;
            }
        }

        /// <summary>
        /// Used to identify the current Order Request Type i.e. New order, Change subscription etc.
        /// </summary>
        public String OrderType
        {
            get
            {
                if (!ViewState[AppConsts.ORDER_REQUEST_TYPE_VIEWSTATE].IsNullOrEmpty())
                    return Convert.ToString(ViewState[AppConsts.ORDER_REQUEST_TYPE_VIEWSTATE]);

                return String.Empty;
            }
            set
            {
                ViewState[AppConsts.ORDER_REQUEST_TYPE_VIEWSTATE] = value;
            }
        }

        //UAT 1438
        public List<UserGroup> selectedUserGrpList
        {
            get;
            set;
        }

        #region E DRUG SCREENING PROPERTIES
        public Int32 EDrugScreenCustomFormId
        {
            get;
            set;
        }
        public Int32 EDrugScreenAttributeGroupId
        {
            get;
            set;
        }
        #endregion

        public List<AttributeFieldsOfSelectedPackages> LstInternationCriminalSrchAttributes
        {
            get
            {
                if (ViewState["LstInternationCriminalSrchAttributes"] != null)
                    return (List<AttributeFieldsOfSelectedPackages>)ViewState["LstInternationCriminalSrchAttributes"];
                return null;
            }
            set
            {
                ViewState["LstInternationCriminalSrchAttributes"] = value;
            }
        }

        //CBI || CABS || Related Properties
        Boolean IAdminEntryOrderReviewView.IsLocationServiceTenant
        {
            get
            {
                if (!ViewState["IsLocationServiceTenant"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsLocationServiceTenant"]);
                return false;
            }
            set
            {
                ViewState["IsLocationServiceTenant"] = value;
            }
        }

        List<Entity.lkpAdminEntrySuffix> IAdminEntryOrderReviewView.lstSuffixes
        {
            get
            {
                if (!ViewState["lstSuffixes"].IsNullOrEmpty())
                    return (List<Entity.lkpAdminEntrySuffix>)ViewState["lstSuffixes"];
                return new List<Entity.lkpAdminEntrySuffix>();
            }
            set
            {
                ViewState["lstSuffixes"] = value;
            }
        }

        String IAdminEntryOrderReviewView.LanguageCode
        {
            get
            {
                LanguageContract languageContract = LanguageTranslateUtils.GetCurrentLanguageFromSession();
                if (!languageContract.IsNullOrEmpty())
                    return languageContract.LanguageCode;
                return Languages.ENGLISH.GetStringValue();
            }
        }
        #region Admin Entry Portal
        public List<OrderPaymentDetail> lstOPDs { get; set; }
        /// <summary>
        /// Payment Type Code for the Payment OptionID used for ApplicantBalancePayment scenario
        /// </summary>
        public String AppChangeSubPaymentTypeCode
        {
            get
            {
                return Convert.ToString(ViewState["ACSPTCode"]);
            }
            set
            {
                ViewState["ACSPTCode"] = value;
            }
        }
        public Boolean IsOrderStatusPaid
        {
            get
            {
                if (!ViewState["IsOrderStatusPaid"].IsNullOrEmpty())
                {
                    return (Boolean)ViewState["IsOrderStatusPaid"];
                }
                return false;
            }
            set
            {
                ViewState["IsOrderStatusPaid"] = value;
            }
        }
        public List<Int32> DPPSIds { get; set; }
        public List<OrderCartCompliancePackage> CompliancePackages
        {
            get
            {
                if (!ViewState["CompliancePackages"].IsNullOrEmpty())
                {
                    return ViewState["CompliancePackages"] as List<OrderCartCompliancePackage>;
                }
                return new List<OrderCartCompliancePackage>();
            }
            set
            {
                ViewState["CompliancePackages"] = value;
            }
        }
        public String InstitutionHierarchy { get; set; }
        public List<Tuple<String, String>> lstClientPaymentOptns { get; set; }
        #endregion
        //
        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                hdnLanguageCode.Value = CurrentViewContext.LanguageCode;
                base.OnInit(e);
                base.Title = Resources.Language.ORDER;
                base.BreadCrumbTitleKey = "Key_ORDER";
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
                if (!this.IsPostBack)
                {
                    //Release 158 CBI
                    ucPersonAlias.SelectedTenantId = CurrentViewContext.TenantId;
                    Presenter.OnViewInitialized();
                    Presenter.ShowRushOrderSetting();
                    Presenter.IsLocationServiceTenant();
                    ucPersonAlias.PageType = PersonAliasPageType.OrderReview.GetStringValue();
                }

                Presenter.OnViewLoaded();

                if (!IsPostBack)
                {
                    //CheckOrder(); - MOVED TO NEW ORDER PAYMENT SCREEN
                    RedirectIfIncorrectOrderStage();

                    if (!CurrentViewContext.IsLocationServiceTenant)
                    {
                        #region UAT-1560: WB: We should be able to add documents that need to be signed to the order process
                        if (!IsAdditionalDocumentExist)
                        {
                            if (!IsDisclaimerAccepted())
                            {
                                RedirectToDisclaimerPage();
                            }
                        }
                        else
                        {
                            if (!IsRequiredDocumentationAccepted())
                            {
                                RedirectToRequiredDocumentationPage();
                            }
                        }
                        #endregion
                        lblAddress1Cptn.Text = Resources.Language.ADDRESS1;
                    }
                    else
                    {
                        lblAddress1Cptn.Text = Resources.Language.ADDRESS;
                        dvAddress2.Visible = false;
                    }


                    _applicantOrderCart = GetApplicantOrderCart();
                    //CBI|| CABS || To Get Suffix List
                    AddSuffix();
                    //
                    BindPersonalDetails();

                    GetPricingData();

                    if (_applicantOrderCart.IsLocationServiceTenant && _applicantOrderCart.FingerPrintData.IsEventCode)
                    {
                        _applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = AppConsts.FIVE;
                    }

                    String _currentStep = " (" + Resources.Language.STEP + " " + (_applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep) +
                              " " + Resources.Language.OF + " " + _applicantOrderCart.GetTotalOrderSteps() + ")";


                    base.SetPageTitle(_currentStep);
                    Presenter.GetSSNSetting();

                    CurrentViewContext.OrderType = _applicantOrderCart.OrderRequestType;

                    HideControlsForCompleteOrderMode();
                }

                SetButtonText();

                BindOtherDetails();

                if (!lstPackages.IsNullOrEmpty() && lstPackages.Count > 0)
                    CreateCustomForm();
                if (!_applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && _applicantOrderCart.lstApplicantOrder[0].lstPackages.Count > 0)
                {
                    residentialHistory.Visible = true;
                    BindResidentialHistory();
                }
                cmdbarEditProfile.ExtraButton.ToolTip = Resources.Language.ORDREVBTNEDITPRFLTOOLTIP;

                cmdbarSubmit.ClearButton.ToolTip = Resources.Language.SBMTNPAYYRORD;
                //if (!applicantOrderCart.IsNullOrEmpty() &&
                //    (applicantOrderCart.OrderRequestType == OrderRequestType.NewOrder.GetStringValue()
                //    || applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscription.GetStringValue()))
                //    (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle(Resources.Language.CREATODR);
                //else if (!applicantOrderCart.IsNullOrEmpty() &&
                //          applicantOrderCart.OrderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue())
                //{
                (this.Page as CoreWeb.AdminEntryPortal.Views.Default).SetModuleTitle(Resources.Language.CMPLTORDER);
                //}
                //else
                //    (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle("Renewal Order");
                divSSN.Visible = !(CurrentViewContext.IsSSNDisabled);

                //UAT-3541 CBI || CABS
                HideShowAppointmentInfo();
                ManageSSN();
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
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion


        #region Button Events

        protected void btnEditProfile_Click(object sender, EventArgs e)
        {
            try
            {
                //_applicantOrderCart = GetApplicantOrderCart();
                //_applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = AppConsts.ONE;
                //_applicantOrderCart.IsEditMode = true;
                RedirectToApplicantProfile();
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
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Event for Backward navigation i.e. Previous or Restart Order button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdbarSubmit_SaveClick(object sender, EventArgs e)
        {
            try
            {
                _applicantOrderCart = GetApplicantOrderCart();

                if (CurrentViewContext.OrderType != OrderRequestType.RenewalOrder.GetStringValue() &&
                    CurrentViewContext.OrderType != OrderRequestType.NewOrder.GetStringValue())
                {
                    _applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = AppConsts.NONE;
                    RedirectToPendingOrder();
                }
                else// resif(CurrentViewContext.OrderType == OrderRequestType.NewOrder.GetStringValue())
                {
                    applicantOrderCart.DecrementOrderStepCount();

                    if (!CurrentViewContext.IsLocationServiceTenant)
                    {
                        RedirectToDisclosureOrDisclaimer();
                    }
                    else
                    {

                        RedirectToAppointmentSchedular(_applicantOrderCart);
                    }

                }
                //else
                //    RedirectToRenewalOrder(_applicantOrderCart);
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
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// Event for Forward navigation i.e. Accept/Proceed/Next Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdbarSubmit_SubmitClick(object sender, EventArgs e)
        {
            //UAT-1768 related changes.
            if (!_applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                Boolean ifLinetItemsExistForEachPackage = CheckIfLineItemHasBeenGeneratedForEachPackage();
                //UAT - 3380 - Added extra condition so that the validation message does not display when user completes the order
                if (!ifLinetItemsExistForEachPackage && String.Compare(Convert.ToString(applicantOrderCart.OrderRequestType), OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) != AppConsts.NONE)
                {
                    base.ShowInfoMessage(Resources.Language.WRNGORDERREVIEWORDRFLOW);
                    return;
                }
            }
            _applicantOrderCart.IncrementOrderStepCount();
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, _applicantOrderCart);

            ////AdminEntryPortalCode  Write Code for Create Payment And Order Completion 

            #region Notification to client admin user to confirm applicant profile submitted/transmitted (if applicant request sent)  

            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
            dictMailData.Add(EmailFieldConstants.TENANT_ID, TenantId);
            dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, applicantOrderCart.HierarchyNodeName);
            dictMailData.Add(EmailFieldConstants.STUDENT_FIRST, applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.FirstName);
            dictMailData.Add(EmailFieldConstants.STUDENT_LAST, applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.LastName);
            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
            //mockData.UserName = string.Concat(applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.FirstName + ' ' + applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.LastName);
            //mockData.EmailID = applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.PrimaryEmailAddress;
            //mockData.ReceiverOrganizationUserID = applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserID;

            mockData.UserName = AppConsts.BACKGROUND_PROCESS_USER_NAME;
            mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
            mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

            //OffTimeRevokedAppointment.ApplicantOrgUserId > AppConsts.NONE ? OffTimeRevokedAppointment.ApplicantOrgUserId : OffTimeRevokedAppointment.ApplicantOrgUserId;
            var CommSubEvnt = CommunicationSubEvents.NOTIFICATION_TO_CLIENT_ADMIN_FOR_CONFIRM_SUBMIT;

            //var sendMail = CommunicationManager.SentMailForApplicantProfileSubmitted(CommSubEvnt, mockData, dictMailData, TenantId, Convert.ToInt32(applicantOrderCart.SelectedHierarchyNodeID));

            #endregion

            if (UpdateOrderAndCompletePayment())
            {
                var sendMail = CommunicationManager.SentMailForApplicantProfileSubmitted(CommSubEvnt, mockData, dictMailData, TenantId, Convert.ToInt32(applicantOrderCart.SelectedHierarchyNodeID));



                Presenter.UpdateApplicatInviteToken(applicantOrderCart.TenantId, applicantOrderCart.OrderId);

                SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, null);
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                     {
                                                                        { AppConsts.CHILD,  ChildControls.AdminEntryOrderConfirmation},
                                                                     };
                string url = String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }

            //Dictionary<String, String> queryString = new Dictionary<String, String>
            //                                                     {
            //                                                        { AppConsts.CHILD,  ChildControls.OrderPayment},
            //                                                        {"TenantId", CurrentViewContext.TenantId.ToString()}
            //                                                     };

            //Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

        }

        protected void cmdbarSubmit_CancelClick(object sender, EventArgs e)
        {
            try
            {
                _applicantOrderCart = GetApplicantOrderCart();
                Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
                Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
                Session.Remove(AppConsts.DISCLOSURE_ACCEPTED);
                //UAT-1560
                Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);
                Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);

                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.ADMIN_ENTRY_APPLICANT_LANDING_SCREEN},
                                                                    { "OrderId", applicantOrderCart.OrderId.ToString()},
                                                                    { "TenantId",applicantOrderCart.TenantId.ToString() }
                                                                 };
                string url = String.Format("~/AdminEntryPortal/Default.aspx?ucid={0}&args={1}", null, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
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

        #region Public Methods



        #endregion

        #region Private Methods

        private ApplicantOrderCart GetApplicantOrderCart()
        {
            if (_applicantOrderCart.IsNull())
            {
                _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            }
            return _applicantOrderCart;
        }

        private void CheckOrder()
        {
            _applicantOrderCart = GetApplicantOrderCart();
            RedirectIfIncorrectOrderStage();

            if (_applicantOrderCart.lstApplicantOrder[0].OrderId != AppConsts.NONE)
            {
                Order order = Presenter.GetOrderById(_applicantOrderCart.lstApplicantOrder[0].OrderId);

                if (order.IsNotNull())
                {
                    CheckOrderStatus(order);
                }
                else
                {
                    RedirectToPendingOrder();
                }
            }
        }

        private void CheckOrderStatus(Order order)
        {
            //String orderStatus = order.lkpOrderStatu.Code;

            String compliancePackageTypeCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
            String orderStatus = order.OrderPaymentDetails
                   .Where(opd => opd.OrderPkgPaymentDetails
                   .Any(oppd => oppd.lkpOrderPackageType.OPT_Code == compliancePackageTypeCode && !oppd.OPPD_IsDeleted)
                     && !opd.OPD_IsDeleted).FirstOrDefault().lkpOrderStatu.Code;

            if (orderStatus == ApplicantOrderStatus.Paid.GetStringValue())
            {
                CheckOnlinePayment();
            }
            else if (orderStatus == ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue())
            {
                RedirectToPendingOrder();
            }
            else
            {
                cmdbarEditProfile.ExtraButton.Enabled = false;
                CommandBar cmdbarEditPackage = (CommandBar)ucPackageDetails.FindControl("cmdbarEditPackage");
                cmdbarEditPackage.ExtraButton.Enabled = false;
            }
        }

        /// <summary>
        /// Checks the order status track. If this page is not opened as per correct order status track then, redirected to correct order.
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        private void RedirectIfIncorrectOrderStage()
        {
            GetApplicantOrderCart();
            RedirectInvalidOrder(_applicantOrderCart);
            Presenter.GetNextPagePathByOrderStageID(_applicantOrderCart);

            //Redirect to next page path if Order Status track is not correct.
            if (CurrentViewContext.NextPagePath.IsNotNull())
            {
                Response.Redirect(CurrentViewContext.NextPagePath);
            }
            else
            {
                _applicantOrderCart.AddOrderStageTrackID(OrderStages.OrderReview);
            }
        }

        private void CheckOnlinePayment()
        {
            try
            {
                ErrorLog logFile = new ErrorLog("Data is sent from OrderReview page.");
                RedirectToOrderConfirmation();
            }
            catch (Exception ex)
            {
                ErrorLog logFile = new ErrorLog("Problem in sending data from OrderReview page" + ex);
            }
        }

        private void BindPersonalDetails()
        {

            _orgUserProfile = new OrganizationUserProfile();

            foreach (var applicantOrder in _applicantOrderCart.lstApplicantOrder)
            {
                _orgUserProfile = applicantOrder.OrganizationUserProfile;
                CurrentViewContext.UpdateOriginalData = applicantOrder.UpdatePersonalDetails;
            }
            foreach (string cptype in _applicantOrderCart.CompliancePackages.Keys)
            {
                OrderCartCompliancePackage cp = _applicantOrderCart.CompliancePackages[cptype];
                if (cp.DPPS_ID.IsNotNull() && cp.DPPS_ID > AppConsts.NONE)
                {
                    if (ucPackageDetails.DPPSIds.IsNull())
                        ucPackageDetails.DPPSIds = new Dictionary<string, int>();
                    ucPackageDetails.DPPSIds.Add(cptype, cp.DPPS_ID);
                }
            }
            //txtRushOrderPrice.Text = _applicantOrderCart.RushOrderPrice;
            //txtTotalPrice.Text = Convert.ToString(_applicantOrderCart.GrandTotal);
            //chkRushOrder.Checked = _applicantOrderCart.IsRushOrderIncluded;
            //divRush.Visible = _applicantOrderCart.IsRushOrderIncluded;
            ////dvRushOrderSrvc.Visible = _applicantOrderCart.IsRushOrderIncluded;
            CurrentViewContext.GenderId = Convert.ToInt32(_orgUserProfile.Gender);
            Presenter.GetGender();

            lblFirstName.Text = _orgUserProfile.FirstName;
            lblLastName.Text = _orgUserProfile.LastName;
            //UAT-2212: Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
            lblMiddleName.Text = _orgUserProfile.MiddleName.IsNullOrEmpty() ? NoMiddleNameText : _orgUserProfile.MiddleName;

            if (!CurrentViewContext.lstSuffixes.IsNullOrEmpty())
            {
                if (_orgUserProfile.UserTypeID > 0 && !_orgUserProfile.UserTypeID.IsNullOrEmpty())
                {
                    if (!CurrentViewContext.lstSuffixes.Where(cond => cond.LAES_ID == _orgUserProfile.UserTypeID).FirstOrDefault().LAES_Suffix.IsNullOrEmpty())
                        lblLastName.Text = _orgUserProfile.UserTypeID.IsNullOrEmpty() ? _orgUserProfile.LastName : _orgUserProfile.LastName + " - " + CurrentViewContext.lstSuffixes.Where(cond => cond.LAES_ID == _orgUserProfile.UserTypeID).FirstOrDefault().LAES_Suffix;
                    else
                        lblLastName.Text = _orgUserProfile.LastName;
                }
            }


            if (_orgUserProfile.DOB.HasValue)
            {
                lblDateOfBirth.Text = _orgUserProfile.DOB.Value.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
            }
            lblGender.Text = CurrentViewContext.Gender;
            lblSSN.Text = Presenter.GetFormattedSSN(_orgUserProfile.SSN);
            lblEmail.Text = _orgUserProfile.PrimaryEmailAddress;
            lblSecondaryEmail.Text = _orgUserProfile.SecondaryEmailAddress;

            //UAT-2447
            if (_orgUserProfile.IsInternationalPhoneNumber)
            {
                lblPhone.Text = _orgUserProfile.PhoneNumber;
            }
            else
            {
                lblPhone.Text = Presenter.GetFormattedPhoneNumber(_orgUserProfile.PhoneNumber);
            }
            if (_orgUserProfile.IsInternationalSecondaryPhone)
            {
                lblSecondaryPhone.Text = _orgUserProfile.SecondaryPhone;
            }
            else
            {
                lblSecondaryPhone.Text = Presenter.GetFormattedPhoneNumber(_orgUserProfile.SecondaryPhone);
            }

            //Show Residing From/To
            PreviousAddressContract resHisoryProfile = _applicantOrderCart.lstPrevAddresses.FirstOrDefault(cond => cond.isCurrent == true);
            if (resHisoryProfile.IsNotNull())
            {
                lblAddress1.Text = resHisoryProfile.Address1;
                lblAddress2.Text = resHisoryProfile.Address2;
                lblZip.Text = resHisoryProfile.Zipcode;
                lblCity.Text = resHisoryProfile.CityName;
                //UAT-3910
                if (resHisoryProfile.StateName.IsNullOrEmpty())
                {
                    dvState.Visible = false;

                    lblShowZIPAndPostal.Text = Resources.Language.POSTALCODE;
                }
                else
                {
                    lblShowZIPAndPostal.Text = Resources.Language.ZIPCODE;
                    dvState.Visible = true;
                }
                lblState.Text = resHisoryProfile.StateName;
                lblCountry.Text = resHisoryProfile.Country;
                lblResidingFrom.Text = resHisoryProfile.ResidenceStartDate.HasValue ? resHisoryProfile.ResidenceStartDate.Value.ToShortDateString() : String.Empty;
                lblResidingTo.Text = resHisoryProfile.ResidenceEndDate.HasValue ? resHisoryProfile.ResidenceEndDate.Value.ToShortDateString() : "until date";
                lblMotherName.Text = resHisoryProfile.MotherName;
                lblIdentificationNumber.Text = resHisoryProfile.IdentificationNumber;
                lblCriminalLicenseNumber.Text = resHisoryProfile.LicenseNumber;
            }

            if (_applicantOrderCart.lstPersonAlias.IsNotNull() && _applicantOrderCart.lstPersonAlias.Count > 0)
            {
                dvpersonalAlias.Visible = true;
                CurrentViewContext.PersonAliasList = _applicantOrderCart.lstPersonAlias.ToList();
            }

            //but for background side: different tables : not DeptProgramPackageSubscription

            Presenter.GetPaymentOptions();

            //cmbPaymentModes.DataSource = CurrentViewContext.lstPaymentOptions;
            //cmbPaymentModes.DataBind();

            //Hide the Payment Type Dropdown when Grand Total is equal to $0.00
            if (_applicantOrderCart.GrandTotal == AppConsts.NONE)
            {
                //Check the Payment Option for the Current Package
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowHidePaymentType(false);", true);
            }

            if (!MVRDvrLicenseNumberID.IsNullOrEmpty() && MVRDvrLicenseNumberID > 0)
            {
                dvDriverLicenseNo.Visible = true;
                if (GetDataForMVR.ContainsKey(MVRDvrLicenseNumberID))
                {
                    lblDriverLiscence.Text = GetDataForMVR[MVRDvrLicenseNumberID];
                }
            }

            if (!MVRDvrLicenseNumberStateID.IsNullOrEmpty() && MVRDvrLicenseNumberStateID > 0)
            {
                dvDriverLicenseState.Visible = true;
                if (GetDataForMVR.ContainsKey(MVRDvrLicenseNumberStateID))
                {
                    lblDriverLicenceState.Text = GetDataForMVR[MVRDvrLicenseNumberStateID];
                }

            }


        }

        #region UAT-1560:WB: We should be able to add documents that need to be signed to the order process
        //Commented UAT-1560
        private Boolean IsDisclaimerAccepted()
        {
            if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.DISCLAIMER_ACCEPTED).IsNotNull() && (Boolean)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.DISCLAIMER_ACCEPTED))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Boolean IsRequiredDocumentationAccepted()
        {
            if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED).IsNotNull() && (Boolean)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Boolean IsAdditionalDocumentExist
        {
            get
            {
                ApplicantOrderCart appOrderCart = GetApplicantOrderCart();
                if (!appOrderCart.IsNullOrEmpty())
                {
                    return appOrderCart.IsAdditionalDocumentExist;
                }
                return false;
            }
        }


        #endregion

        #region UAT-1560
        private void RedirectToDisclaimerPage()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.CHILD,  ChildControls.AdminEntryApplicantDisclaimerPage}
                                                                 };
            Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

        }

        private void RedirectToRequiredDocumentationPage()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.CHILD,  ChildControls.AdminEntryApplicantRequiredDocumentationPage}
                                                                 };
            Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

        }
        #endregion

        private void RedirectToOrderConfirmation()
        {
            //// AdminEntryPortalCode Write code for payment completion and save order Here

            //Dictionary<String, String> queryString = new Dictionary<String, String>();
            //queryString = new Dictionary<String, String>
            //                                                     {
            //                                                        {AppConsts.CHILD,  ChildControls.ApplicantOrderConfirmation}
            //                                                     };
            //Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

        }

        private void RedirectToPendingOrder()
        {
            //  Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            _applicantOrderCart.ClearOrderCart(_applicantOrderCart);
            //UAT-1560
            Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
            Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);
            Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.ADMIN_ENTRY_APPLICANT_LANDING_SCREEN},
                                                                    { "OrderId", applicantOrderCart.OrderId.ToString()},
                                                                    { "TenantId",applicantOrderCart.TenantId.ToString() }
                                                                 };
            Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        }

        //private void RedirectToRenewalOrder(ApplicantOrderCart applicantOrderCart)
        //{
        //    //Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
        //    ////UAT-1560
        //    //Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
        //    //Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);
        //    //Dictionary<String, String> queryString = new Dictionary<String, String>()
        //    //                                             {
        //    //                                                {"OrderId",applicantOrderCart.PrevOrderId.ToString()},
        //    //                                                { "Child",  ChildControls.RenewalOrder}
        //    //                                             };
        //    ////Response.Redirect(String.Format("~/Main/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        //    //Response.Redirect(String.Format("Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        //}


        private void RedirectToAppointmentSchedular(ApplicantOrderCart applicantOrderCart)
        {
            // Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
            Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);

            #region Order flow of Event Code
            if ((applicantOrderCart.IsLocationServiceTenant && applicantOrderCart.FingerPrintData.IsEventCode) || (applicantOrderCart.IsLocationServiceTenant && applicantOrderCart.FingerPrintData.IsOutOfState))
            {

                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                Dictionary<String, String> queryString;
                if (OrderType == OrderRequestType.NewOrder.GetStringValue())
                {
                    #region Based on order types i.e. Only Compliance or Compliance + Background etc., redirect the user
                    applicantOrderCart.DecrementOrderStepCount();


                    var _formToLoad = applicantOrderCart.lstFormExecuted.LastOrDefault();
                    // Redirect to Custom Forms
                    applicantOrderCart.AddOrderStageTrackID(OrderStages.CustomForms);
                    applicantOrderCart.IsEditMode = true;
                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD, ChildControls.AdminEntryCustomFormLoad},
                                                                    {"CustomFormId",Convert.ToString(_formToLoad)},
                                                                    {"IsPrevious","1"},
                                                                    {"TenantId", CurrentViewContext.TenantId.ToString()}
                                                                 };
                    Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

                    #endregion
                }
            }
            #endregion
            else
            {
                #region UAT - 4331 : change schedule appointment to step 2 of order flow
                ////// Comment previous code and replace with new one as per the changes required for UAT -4331

                Dictionary<String, String> queryString = new Dictionary<String, String>();

                var _formToLoad = applicantOrderCart.lstFormExecuted.LastOrDefault();
                // Redirect to Custom Forms
                applicantOrderCart.AddOrderStageTrackID(OrderStages.CustomForms);
                applicantOrderCart.IsEditMode = true;
                queryString = new Dictionary<String, String>
                                                                  {
                                                                     { AppConsts.CHILD, ChildControls.AdminEntryCustomFormLoad},
                                                                     {"CustomFormId",Convert.ToString(_formToLoad)},
                                                                     { "TenantId", CurrentViewContext.TenantId.ToString()},
                                                                     {"IsPrevious","1"}
                                                                  };
                Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

                #endregion
            }
        }
        private void RedirectToApplicantProfile()
        {
            _applicantOrderCart = GetApplicantOrderCart();

            //// UAT - 4331 
            if (_applicantOrderCart.IsLocationServiceTenant && !_applicantOrderCart.FingerPrintData.IsNullOrEmpty())
            {
                if (!_applicantOrderCart.FingerPrintData.IsEventCode && !_applicantOrderCart.FingerPrintData.IsOutOfState)
                    _applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = AppConsts.FOUR;
                else
                    _applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = AppConsts.THREE;
            }
            else
            {
                _applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = AppConsts.ONE;
            }
            applicantOrderCart.EDrugScreeningRegistrationId = null;
            _applicantOrderCart.AddOrderStageTrackID(OrderStages.ApplicantProfile);
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TenantId", CurrentViewContext.TenantId.ToString()},
                                                                    { "OrderId",_applicantOrderCart.lstApplicantOrder[0].OrderId.ToString() },
                                                                    {AppConsts.CHILD , AppConsts.ADMIN_ENTRY_APPLICANT_INFORMATION}
                                                                 };
            String url = String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
            Response.Redirect(url);
        }

        /// <summary>
        /// Redirect the user to Disclosure or Disclaimer page, on click of 'Previous' button, depending on the Compliance package selection.
        /// </summary>
        private void RedirectToDisclosureOrDisclaimer()
        {
            String _childControl = String.Empty;

            //Commented Below Code UAT-1560
            if (!applicantOrderCart.IsAdditionalDocumentExist)
            {

                // To avoid Redirection again by the browser back button navigation check method
                applicantOrderCart.AddOrderStageTrackID(OrderStages.Disclosure);//UAT-5184
                _childControl = ChildControls.AdminEntryApplicantDisclosurePage;//UAT-5184
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,_childControl  }
                                                                 };
                Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
            }
            else
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                applicantOrderCart.AddOrderStageTrackID(OrderStages.RequiredDocumentation);
                queryString = new Dictionary<String, String>
                                                         {
                                                            { AppConsts.CHILD,  ChildControls.AdminEntryApplicantRequiredDocumentationPage}
                                                         };
                Response.Redirect(String.Format("~/AdminEntryPortal/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
            }
        }

        private void BindOtherDetails()
        {
            if (_applicantOrderCart.IsNullOrEmpty())
                _applicantOrderCart = GetApplicantOrderCart();
            //UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
            if (String.Compare(_applicantOrderCart.OrderRequestType, OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) == AppConsts.NONE
                && _applicantOrderCart.IsReadOnly)
            {
                caOtherDetails.TypeCode = CustomAttributeUseTypeContext.Hierarchy.GetStringValue();
                caOtherDetails.DataSourceModeType = DataSourceMode.Ids;
                caOtherDetails.IsOrder = true;
                caOtherDetails.ValueRecordId = _applicantOrderCart.SelectedHierarchyNodeID.Value;
            }
            else
            {
                caOtherDetails.lstTypeCustomAttributes = _applicantOrderCart.GetCustomAttributeValues();
                caOtherDetails.DataSourceModeType = DataSourceMode.ExternalList;
            }
            caOtherDetails.Title = "Other Details";
            caOtherDetails.ControlDisplayMode = DisplayMode.ReadOnlyLabels;
            caOtherDetails.IsReadOnly = true;
            // Read only mode, so only data is required for display. 
            //Other wise  properties like Maxlength, IsREquired will not be available.

            #region UAT 1438: Enhancement to allow students to select a User Group.

            caOtherDetails.TenantId = CurrentViewContext.TenantId;
            caOtherDetails.CurrentLoggedInUserId = CurrentViewContext.CurrentLoggedInUserId;
            if (_applicantOrderCart.IsUserGroupCustomAttributeExist)
            {
                Presenter.GetUserGroupListFromUserIDs(_applicantOrderCart.lstCustomAttributeUserGroupIDs);
                caOtherDetails.lstUserGroupsForUser = selectedUserGrpList;
                caOtherDetails.ShowUserGroupCustomAttribute = true;
                caOtherDetails.ShowReadOnlyUserGroupCustomAttribute = true;
            }

            #endregion
        }


        /// <summary>
        /// Call the Pricing stored procedure and
        /// 1. Updates the  BkgPackage prices, from the SP data.
        /// 2. Saves the XML of Pricing SP in session, for further processing
        /// </summary>
        private void GetPricingData()
        {
            if (_applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
                return;
            //UAT-1648
            if (String.Compare(Convert.ToString(applicantOrderCart.OrderRequestType), OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) == AppConsts.NONE)
            {
                List<Int32> Bkg_pkgIDs = new List<Int32>();
                List<Package_PricingData> _lstData = new List<Package_PricingData>();
                foreach (var bkgPkg in _applicantOrderCart.lstApplicantOrder[0].lstPackages)
                {
                    _lstData.Add(new Package_PricingData
                    {
                        PackageId = bkgPkg.BPAId,
                        TotalBkgPackagePrice = bkgPkg.TotalBkgPackagePrice
                    });
                    Bkg_pkgIDs.Add(bkgPkg.BPAId);
                }

                //UAT-1867:Add breakdown of fees in total price on the order review screen (before submit).
                //Gettting data from Db which for completing order from Order history screen.
                Int32 OrderId = _applicantOrderCart.lstApplicantOrder[0].OrderId;
                List<BackroundOrderServiceLinePrice> _lstBackroundOrderServiceLinePrice = new List<BackroundOrderServiceLinePrice>();
                _lstBackroundOrderServiceLinePrice = Presenter.GetBackgroundOrderServiceLinePriceData(OrderId, Bkg_pkgIDs);
                ucPackageDetails.lstBkgOrderSvcLineData = _lstBackroundOrderServiceLinePrice;

                ucPackageDetails.lstPackagePrices = _lstData;
                ucPackageDetails.lstExternalPackages = _applicantOrderCart.lstApplicantOrder[0].lstPackages;
            }
            else
            {
                String _pricingDataXML = _presenter.GetPricingData(_applicantOrderCart, this.TenantId);
                _applicantOrderCart.lstApplicantOrder[0].PricingDataXML = _pricingDataXML;

                if (!String.IsNullOrEmpty(_pricingDataXML))
                {
                    XDocument doc = XDocument.Parse(_pricingDataXML);
                    var _packages = doc.Root.Descendants("Packages")
                                       .Descendants("Package")
                                       .Select(element => element)
                                       .ToList();

                    List<Package_PricingData> _lstData = new List<Package_PricingData>();
                    foreach (var pkg in _packages)
                    {
                        #region Update the BkgPackage Price for ALL BkgPackages, in Session, from the Pricing SP calculations

                        Int32 _packageId = Convert.ToInt32(pkg.Element("PackageId").Value);

                        // To be removed
                        Decimal _totalLineItemPrice = pkg.Element("TotalPrice").Value.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToDecimal(pkg.Element("TotalPrice").Value);

                        BackgroundPackagesContract _bkgPackagesContract = _applicantOrderCart.lstApplicantOrder[0].lstPackages
                                                                            .Where(bpkg => bpkg.BPAId == _packageId).FirstOrDefault();

                        if (_bkgPackagesContract.IsNotNull())
                            _bkgPackagesContract.TotalBkgPackagePrice = _totalLineItemPrice;

                        //UAT-3268
                        //if (_bkgPackagesContract.IsReqToQualifyInRotation && !_bkgPackagesContract.AdditionalPrice.IsNullOrEmpty() && _bkgPackagesContract.AdditionalPrice > AppConsts.NONE)
                        //    _bkgPackagesContract.TotalBkgPackagePrice += Convert.ToDecimal(_bkgPackagesContract.AdditionalPrice);

                        _lstData.Add(new Package_PricingData
                        {
                            PackageId = _packageId,
                            TotalBkgPackagePrice = _totalLineItemPrice
                        });

                        #endregion
                    }
                    #region UAT-1867:Add breakdown of fees in total price on the order review screen (before submit).
                    //here we are geeting order line items from XML (_pricingDataXML)
                    var _orderLineItem = doc.Root.Descendants("Packages")
                                       .Descendants("Package")
                                       .Descendants("OrderLineItems")
                                       .Descendants("OrderLineItem")
                                       .Select(element => element)
                                       .ToList();

                    List<BackroundOrderServiceLinePrice> _lstBkgorderServiceLinedata = new List<BackroundOrderServiceLinePrice>();

                    foreach (var ordrLneItm in _orderLineItem)
                    {
                        Int32 _bkgServiceId = Convert.ToInt32(ordrLneItm.Element("PackageServiceID").Value);
                        String _bkgServiceName = ordrLneItm.Element("Description").Value;
                        Decimal _amount = ordrLneItm.Element("Price").Value.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToDecimal(ordrLneItm.Element("Price").Value);


                        var fees = ordrLneItm
                                      .Descendants("Fees")
                                      .Descendants("Fee")
                                      .Select(element => element)
                                      .ToList();
                        Decimal _totalAdjAmnt = AppConsts.NONE;
                        String _description = String.Empty;
                        foreach (var fee in fees)
                        {
                            Decimal _adjAmount = fee.Elements("Amount").FirstOrDefault().Value.IsNullOrEmpty() ?
                                AppConsts.NONE : Convert.ToDecimal(fee.Elements("Amount").FirstOrDefault().Value);
                            _totalAdjAmnt = _totalAdjAmnt + _adjAmount;
                            if (_description.IsNullOrEmpty() && _description.IsNullOrWhiteSpace())
                            {
                                _description = fee.Elements("Description").FirstOrDefault().Value;
                            }
                        }


                        //if (!pkg.Descendants("Fees").Descendants("Fee").Elements("Amount").IsNullOrEmpty())
                        //{
                        //    _adjAmount = pkg.Descendants("Fees").Descendants("Fee").Elements("Amount").FirstOrDefault().Value.IsNullOrEmpty() ?
                        //        AppConsts.NONE : Convert.ToDecimal(pkg.Descendants("Fees").Descendants("Fee").Elements("Amount").FirstOrDefault().Value);

                        //    _description = pkg.Descendants("Fees").Descendants("Fee").Elements("Description").FirstOrDefault().Value;
                        //}

                        Decimal _netAmount = _amount + _totalAdjAmnt;

                        _lstBkgorderServiceLinedata.Add(new BackroundOrderServiceLinePrice
                        {
                            BackgroundServiceID = _bkgServiceId,
                            BackgroundServiceName = _bkgServiceName,
                            Amount = _amount,
                            AdjAmount = _totalAdjAmnt,
                            NetAmount = _netAmount,
                            Description = _description
                        });
                    }
                    #endregion


                    // To Display the updated prices, based on the Pricing SP calculations
                    ucPackageDetails.lstPackagePrices = _lstData;

                    //UAT-1867:Add breakdown of fees in total price on the order review screen (before submit).
                    //passing list of service line items with their prices and description to the PackageDetails screen to bind the background service price breakdown grid.
                    ucPackageDetails.lstBkgOrderSvcLineData = _lstBkgorderServiceLinedata;
                }
            }
        }

        private void CreateCustomForm()
        {
            String packages = String.Empty;
            packages = GetPackageIdString();
            List<Int32> lstCustomForms = new List<Int32>();
            List<Int32> lstGroupIds = new List<Int32>();
            Presenter.GetAttributeFieldsOfSelectedPackages(packages);
            List<AttributeFieldsOfSelectedPackages> lstCriminalAttributes = CurrentViewContext.LstInternationCriminalSrchAttributes;
            PreviousAddressContract resHisoryProfile = _applicantOrderCart.lstPrevAddresses.FirstOrDefault(cond => cond.isCurrent == true);
            if (resHisoryProfile.IsNotNull() && resHisoryProfile.CountryId != AppConsts.COUNTRY_USA_ID)
            {
                if (!lstCriminalAttributes.IsNullOrEmpty())
                {
                    if (lstCriminalAttributes.Any(x => x.BSA_Code == LCNAttCode.ToString() && x.IsAttributeDisplay))
                    {
                        divInternationalCriminalSearchAttributes.Visible = true;
                        divCriminalLicenseNumber.Visible = true;
                    }
                    if (lstCriminalAttributes.Any(x => x.BSA_Code == MotherNameAttrCode.ToString() && x.IsAttributeDisplay))
                    {
                        divInternationalCriminalSearchAttributes.Visible = true;
                        divMothersName.Visible = true;
                    }
                    if (lstCriminalAttributes.Any(x => x.BSA_Code == IdentificationNumberAttrCode.ToString() && x.IsAttributeDisplay))
                    {
                        divInternationalCriminalSearchAttributes.Visible = true;
                        divIdentificationNumber.Visible = true;
                    }
                }
            }
            if (!lstBackgroundOrderData.IsNullOrEmpty())
            {

                lstCustomForms = lstBackgroundOrderData.Where(x => x.CustomFormId != AppConsts.ONE).DistinctBy(x => x.CustomFormId).Select(x => x.CustomFormId).ToList();
                #region E DRUG SCREENING
                Presenter.GetEDrugAttributeGroupIdAndFormId();

                #endregion
                for (Int32 custId = 0; custId < lstCustomForms.Count; custId++)
                {
                    Presenter.GetAttributesForTheCustomForm(packages, lstCustomForms[custId], LanguageTranslateUtils.GetCurrentLanguageFromSession().LanguageCode);
                    List<BackgroundOrderData> newLstBackGroundOrderData = new List<BackgroundOrderData>();
                    newLstBackGroundOrderData = lstBackgroundOrderData.Where(x => x.CustomFormId == lstCustomForms[custId]).Select(x => x).ToList();
                    lstGroupIds = newLstBackGroundOrderData.DistinctBy(x => x.BkgSvcAttributeGroupId).Select(x => x.BkgSvcAttributeGroupId).ToList();
                    for (Int32 grpId = 0; grpId < lstGroupIds.Count; grpId++)
                    {

                        if ((EDrugScreenAttributeGroupId > 0 && EDrugScreenCustomFormId > 0) && (lstGroupIds[grpId] == EDrugScreenAttributeGroupId && lstCustomForms[custId] == EDrugScreenCustomFormId))
                        {
                            WebCCF _webCCFForm = Page.LoadControl("~/BkgOperations/UserControl/WebCCF.ascx") as WebCCF;
                            _webCCFForm.IsReview = true;
                            _webCCFForm.IsOrderConfirmation = false;
                            _webCCFForm.CustomFormId = lstCustomForms[custId];
                            _webCCFForm.AttributeGroupId = lstGroupIds[grpId];
                            _webCCFForm.LstBackgroundOrderData = newLstBackGroundOrderData;
                            _webCCFForm.LstAttributeForCustomFormContract = lstCustomFormAttributes;
                            pnlLoader.Controls.Add(_webCCFForm);
                        }
                        else
                        {
                            StringBuilder xmlStringData = new StringBuilder();

                            //comment this if  not needed
                            if (applicantOrderCart.IsLocationServiceTenant)
                            {
                                xmlStringData.Append("<Attributes>");
                                foreach (BackgroundOrderData item in newLstBackGroundOrderData)
                                {
                                    foreach (var dic in item.CustomFormData.Where(cond => !cond.Value.IsNullOrEmpty()))
                                    {
                                        xmlStringData.Append("<Attribute><InstanceID>" + item.InstanceId + "</InstanceID><AttributeID>" + dic.Key + "</AttributeID><AttributeValue>" + System.Security.SecurityElement.Escape(dic.Value) + "</AttributeValue></Attribute>");
                                    }
                                }
                                xmlStringData.Append("</Attributes>");
                            }

                            List<CustomFormAutoFillDataContract> lstAttributes = Presenter.GetConditionsforAttributes(xmlStringData);

                            lstAttributes.Where(l => !string.IsNullOrWhiteSpace(l.HeaderLabel)).ForEach(cond =>
                            {
                                lstCustomFormAttributes.Where(l => l.AttributeGroupId == cond.AttributeGroupID).ForEach(s => s.SectionTitle = cond.HeaderLabel);
                            });

                            AdminEntryCustomFormHtml _customForm = Page.LoadControl("~/AdminEntryPortal/UserControl/AdminEntryCustomFormHtml.ascx") as AdminEntryCustomFormHtml;
                            _customForm.lstCustomFormAttributes = lstCustomFormAttributes;
                            _customForm.groupId = lstGroupIds[grpId];
                            //Total Number Of Instane for a particular group
                            _customForm.InstanceId = newLstBackGroundOrderData.Where(x => x.BkgSvcAttributeGroupId == lstGroupIds[grpId] && x.CustomFormId == lstCustomForms[custId]).Count();
                            _customForm.CustomFormId = lstCustomForms[custId];
                            _customForm.tenantId = TenantId;
                            _customForm.lstBackgroundOrderData = newLstBackGroundOrderData;
                            _customForm.IsReadOnly = true;
                            // UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
                            if (String.Compare(_applicantOrderCart.OrderRequestType, OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) == AppConsts.NONE
                                && _applicantOrderCart.IsReadOnly)
                            {
                                _customForm.ShowEditDetailButton = false;
                            }
                            else
                            {
                                _customForm.ShowEditDetailButton = true;
                            }
                            pnlLoader.Controls.Add(_customForm);
                        }
                    }
                }
            }
        }

        private void BindResidentialHistory()
        {
            if (!lstResendialHistory.IsNullOrEmpty() && IsResidentialHistoryVisible)
            {
                BkgOrderResidentialHistories userControl = null;
                userControl = LoadControl("~/BkgOperations/UserControl/BkgOrderResidentialHistories.ascx") as BkgOrderResidentialHistories;
                userControl.OrderReview = true;
                userControl.lstPrevAddresses = lstResendialHistory;
                List<AttributeFieldsOfSelectedPackages> lstCriminalAttributes = CurrentViewContext.LstInternationCriminalSrchAttributes;
                if (!lstCriminalAttributes.IsNullOrEmpty())
                {
                    if (lstCriminalAttributes.Any(x => x.BSA_Code == LCNAttCode.ToString() && x.IsAttributeDisplay))
                    {
                        userControl.ShowCriminalAttribute_License = true;
                    }
                    if (lstCriminalAttributes.Any(x => x.BSA_Code == MotherNameAttrCode.ToString() && x.IsAttributeDisplay))
                    {
                        userControl.ShowCriminalAttribute_MotherName = true;
                    }
                    if (lstCriminalAttributes.Any(x => x.BSA_Code == IdentificationNumberAttrCode.ToString() && x.IsAttributeDisplay))
                    {
                        userControl.ShowCriminalAttribute_Identification = true;
                    }
                }
                residentialHistory.Controls.Add(userControl);
            }
        }

        /// <summary>
        /// Gets the ',' seperated string  of list of package Ids.
        /// </summary>
        /// <returns></returns>
        private string GetPackageIdString()
        {
            String packages = String.Empty;
            if (!lstPackages.IsNullOrEmpty())
            {
                lstPackages.ForEach(x => packages += Convert.ToString(x.BPAId) + ",");
                //packages = "4";
                if (packages.EndsWith(","))
                    packages = packages.Substring(0, packages.Length - 1);
            }
            return packages;
        }

        /// <summary>
        /// Redirect the user to dashboard, if applicant order cart is empty
        /// </summary>
        /// <param name="applicantOrder"></param>
        private void RedirectInvalidOrder(ApplicantOrderCart applicantOrderCart)
        {
            if (applicantOrderCart.IsNullOrEmpty())
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child",  AppConsts.ADMIN_ENTRY_APPLICANT_LANDING_SCREEN},
                                                                    { "OrderId", "0"},
                                                                    { "TenantId",CurrentViewContext.TenantId.ToString() }
                                                                 };
                string url = String.Format("~/AdminEntryPortal/Default.aspx?ucid={0}&args={1}", null, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
            }
        }

        /// <summary>
        /// Set the button Text for 'Previous', 'Next' or 'Restart' etc, based on the type of Order
        /// </summary>
        private void SetButtonText()
        {
            if (CurrentViewContext.OrderType == OrderRequestType.NewOrder.GetStringValue())
            {
                //cmdbarSubmit.SubmitButtonText = AppConsts.PREVIOUS_BUTTON_TEXT;
                //cmdbarSubmit.ClearButtonText = AppConsts.NEXT_BUTTON_TEXT;
                cmdbarSubmit.SubmitButtonText = Resources.Language.PREVIOUS;
                cmdbarSubmit.ClearButtonText = Resources.Language.NEXT;
            }
            //else if (CurrentViewContext.OrderType == OrderRequestType.CompleteOrderByApplicant.GetStringValue())
            //{
            //    cmdbarSubmit.ClearButtonText = Resources.Language.NEXT;
            //    cmdbarSubmit.SaveButtonText = Resources.Language.CNCL;
            //}
            //else
            //{
            //    //cmdbarSubmit.SubmitButtonText = AppConsts.RESTART_ORDER_BUTTON_TEXT;
            //    //cmdbarSubmit.ClearButtonText = AppConsts.NEXT_BUTTON_TEXT;

            //    cmdbarSubmit.SubmitButtonText = Resources.Language.RSTRTORDR;
            //    cmdbarSubmit.ClearButtonText = Resources.Language.NEXT;
            //}
        }

        #region UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
        private void HideControlsForCompleteOrderMode()
        {
            if (String.Compare(_applicantOrderCart.OrderRequestType, OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) == AppConsts.NONE
                && _applicantOrderCart.IsReadOnly)
            {
                cmdbarEditProfile.Visible = false;
                cmdbarSubmit.SubmitButton.Style["Display"] = "none";
                base.SetPageTitle(String.Empty);
                headerText.InnerText = "Order Review: Please review your order details below.";
            }
            else
            {
                headerText.InnerText = Resources.Language.ORDERREVIEWTITLE;
            }

        }

        #endregion


        private Boolean CheckIfLineItemHasBeenGeneratedForEachPackage()
        {
            bool checkIfLinetItemsExistForEachPackage = false;

            Dictionary<Int32, Boolean> dcntryIfLineItemExist = new Dictionary<Int32, Boolean>();
            String _pricingDataXML = _applicantOrderCart.lstApplicantOrder[0].PricingDataXML;

            if (!String.IsNullOrEmpty(_pricingDataXML))
            {
                XDocument doc = XDocument.Parse(_pricingDataXML);
                var _packages = doc.Root.Descendants("Packages")
                                   .Descendants("Package")
                                   .Select(element => element)
                                   .ToList();

                List<Package_PricingData> _lstData = new List<Package_PricingData>();
                foreach (var pkg in _packages)
                {
                    Int32 _packageId = Convert.ToInt32(pkg.Element("PackageId").Value);
                    if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.Where(cond => cond.BPAId == _packageId).Select(sel => sel.IsReqToQualifyInRotation).FirstOrDefault())   //Check Related to UAT-3268
                    {
                        var _orderLineItemNode = pkg.Descendants("OrderLineItems");
                        if (!_orderLineItemNode.IsNullOrEmpty())
                        {
                            var _orderLineItems = _orderLineItemNode.Descendants("OrderLineItem")
                                                .Select(element => element)
                                                .ToList();

                            if (!_orderLineItems.IsNullOrEmpty())
                                dcntryIfLineItemExist.Add(_packageId, true);
                            else
                                dcntryIfLineItemExist.Add(_packageId, false);
                        }
                        else
                            dcntryIfLineItemExist.Add(_packageId, false);
                    }
                    //UAT-3268
                    else
                    {
                        dcntryIfLineItemExist.Add(_packageId, true);
                    }
                }
            }
            checkIfLinetItemsExistForEachPackage = dcntryIfLineItemExist.Any(cond => cond.Value == true);
            //checkIfLinetItemsExistForEachPackage = !dcntryIfLineItemExist.Any(cond => cond.Value == false);
            return checkIfLinetItemsExistForEachPackage;
        }

        private void ManageSSN()
        {
            String AppSSN = lblSSN.Text.Trim();
            AppSSN = AppSSN.Replace(@"-", "");
            if (AppSSN == AppConsts.DefaultSSN)
            {
                if (applicantOrderCart.IsLocationServiceTenant)
                {
                    divSSN.Visible = false;
                }
            }
        }
        #endregion

        #endregion

        #region UAT-3541 || CBI || CABS


        AppointmentSlotContract IAdminEntryOrderReviewView.AppointmentSlotContract
        {
            get
            {
                if (!ViewState["AppointmentSlotContract"].IsNullOrEmpty())
                    return (AppointmentSlotContract)(ViewState["AppointmentSlotContract"]);
                return new AppointmentSlotContract();
            }
            set
            {
                ViewState["AppointmentSlotContract"] = value;
            }
        }
        private void HideShowAppointmentInfo()
        {
            #region 3804

            if (applicantOrderCart.IsLocationServiceTenant)
            {
                DivPrivacyActNotification.Visible = true;
            }
            else
            {
                DivPrivacyActNotification.Visible = false;
            }

            #endregion

            if (applicantOrderCart.IsLocationServiceTenant && applicantOrderCart.FingerPrintData.IsNotNull())
            {
                if (applicantOrderCart.FingerPrintData.IsOutOfState)
                {
                    dvOutOfStateAppointmentDetails.Visible = true;
                }
                else
                {
                    //dvAppointmentDetails.Visible = true;
                    BindAppointmentData();
                }
                hdnIsLocTen.Value = applicantOrderCart.IsLocationServiceTenant.ToString();
            }
            //else
            //{
            //    dvAppointmentDetails.Visible = false;
            //}
        }

        private void BindAppointmentData()
        {
            //Int32 OrderId = _applicantOrderCart.lstApplicantOrder[0].OrderId;
            //Presenter.GetAppointmentData(OrderId);
            if (!applicantOrderCart.FingerPrintData.IsNullOrEmpty())
            {
                //lblLocationName.Text = String.IsNullOrEmpty(applicantOrderCart.FingerPrintData.LocationName) ? String.Empty : applicantOrderCart.FingerPrintData.LocationName;
                //lblLocationAddress.Text = String.IsNullOrEmpty(applicantOrderCart.FingerPrintData.LocationAddress) ? String.Empty : applicantOrderCart.FingerPrintData.LocationAddress;
                //lblAppointmentTiming.Text = applicantOrderCart.FingerPrintData.SlotDate.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue())) + " (" + applicantOrderCart.FingerPrintData.StartTime.ToString("hh:mm tt") + " - " + applicantOrderCart.FingerPrintData.EndTime.ToString("hh:mm tt") + ") ";
                //lblSiteDescription.Text = String.IsNullOrEmpty(applicantOrderCart.FingerPrintData.LocationDescription) ? String.Empty : applicantOrderCart.FingerPrintData.LocationDescription;
                hdnLocId.Value = applicantOrderCart.FingerPrintData.LocationId.ToString();

                //UAT-3761

                //if (applicantOrderCart.FingerPrintData.IsEventCode)
                //{
                //    dvLocAdd.Visible = false;
                //    btnViewLocImage.Visible = false;
                //    dvChangeApp.Visible = false;
                //    lblLocationName.Text = String.IsNullOrEmpty(applicantOrderCart.FingerPrintData.EventName) ? String.Empty : applicantOrderCart.FingerPrintData.EventName;
                //    lblSiteDescription.Text = String.IsNullOrEmpty(applicantOrderCart.FingerPrintData.EventDescription) ? String.Empty : applicantOrderCart.FingerPrintData.EventDescription;


                //}
            }
        }

        private void AddSuffix()
        {

            Presenter.GetAdminEntrySuffixes();


        }

        #endregion


        #region Admin Entry Portal
        private Boolean UpdateOrderAndCompletePayment()
        {
            applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            return SaveOrderPaymentDetails(applicantOrderCart);
        }

        private Boolean SaveOrderPaymentDetails(ApplicantOrderCart applicantOrderCart)
        {
            Boolean IsOrderPaymentDetailsExist = Presenter.IsOrderPaymentDetailExist(applicantOrderCart.OrderId);
            var _lstClientPaymentOptions = Presenter.GetClientPaymentOptions();
            var _ccPaymentOptionId = _lstClientPaymentOptions.Where(po => po.Code == PaymentOptions.Credit_Card.GetStringValue()).First().PaymentOptionID;
            var _paypalPaymentOptionId = _lstClientPaymentOptions.Where(po => po.Code == PaymentOptions.Paypal.GetStringValue()).First().PaymentOptionID;
            var _InvoiceWithoutApprovalOptionId = _lstClientPaymentOptions.Where(po => po.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()).First().PaymentOptionID;

            List<ApplicantOrderPaymentOptions> _paymentModesData = new List<ApplicantOrderPaymentOptions>();
            foreach (var item in applicantOrderCart.lstApplicantOrder[0].lstPackages)
            {
                ApplicantOrderPaymentOptions orderPaymentOptions = new ApplicantOrderPaymentOptions();
                orderPaymentOptions.additionalPoid = null;
                orderPaymentOptions.isbkg = true;
                orderPaymentOptions.isZP = false;
                orderPaymentOptions.pkgid = item.BPAId;
                orderPaymentOptions.poid = _InvoiceWithoutApprovalOptionId;
                _paymentModesData.Add(orderPaymentOptions);

            }

            #region Variables
            ApplicantOrderDataContract _applicantOrderDataContract = new ApplicantOrderDataContract();


            String _invoiceNumbers = String.Empty;
            var _dicInvoiceNumber = new Dictionary<String, String>();
            Boolean isRushOrder = false;
            String _paymentModeCode = String.Empty;
            Boolean _isUpdateMainProfile = false;
            Int32 _prgPackageSubscriptionId = 0;
            Int32 _tenantId = CurrentViewContext.TenantId;
            String _errorMessage = String.Empty;
            String _redirectUrlType = String.Empty;

            GenerateGroupedAmount(applicantOrderCart, _paymentModesData, _tenantId, _lstClientPaymentOptions);

            //var test = applicantOrderCart.ApplicantDisclaimerDocumentId;            
            Presenter.AddUpdateAdminEntryUserData(applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile, CurrentViewContext.CurrentLoggedInUserId, CurrentViewContext.TenantId
                                                 , applicantOrderCart.lstPersonAlias, applicantOrderCart.lstPrevAddresses);

            List<Entity.ResidentialHistoryProfile> lstResidentialHistoryProfile = new List<Entity.ResidentialHistoryProfile>();
            lstResidentialHistoryProfile = SecurityManager.GetUserResidentialHistoryProfiles(applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserProfileID);

            List<Package_PricingData> _lstPricingData = null;
            if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                String _pricingDataXML = applicantOrderCart.lstApplicantOrder[0].PricingDataXML;
                if (!String.IsNullOrEmpty(_pricingDataXML))
                {
                    _lstPricingData = new List<Package_PricingData>();
                    _lstPricingData = GenerateDataFromPricingXML(_pricingDataXML);
                }
            }
            _applicantOrderDataContract.IsCompliancePackageSelected = applicantOrderCart.IsCompliancePackageSelected;
            _applicantOrderDataContract.OrganizationUserProfile = applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile;
            _applicantOrderDataContract.ProgramPackageSubscriptionId = _prgPackageSubscriptionId;
            _applicantOrderDataContract.lstGroupedData = applicantOrderCart.lstPaymentGrouping;
            _applicantOrderDataContract.TenantId = _tenantId;
            _applicantOrderDataContract.lstAttributeValues = applicantOrderCart.GetCustomAttributeValues();
            _applicantOrderDataContract.LastNodeDPMId = Convert.ToInt32(applicantOrderCart.SelectedHierarchyNodeID);
            _applicantOrderDataContract.lstBackgroundPackages = applicantOrderCart.lstApplicantOrder[0].lstPackages;
            _applicantOrderDataContract.lstPricingData = _lstPricingData;
            _applicantOrderDataContract.IsSendBackgroundReport = applicantOrderCart.lstApplicantOrder[0].IsSendBackgroundReport;
            _applicantOrderDataContract.lstResidentialHistoryProfile = lstResidentialHistoryProfile;//SecurityManager.GetUserResidentialHistoryProfiles(applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserProfileID);
            // SelectedPaymentModeId = paymentModeId, UAT 916
            _applicantOrderDataContract.lstPersonAliasProfile = SecurityManager.GetUserPersonAliasProfiles(applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserProfileID);

            _applicantOrderDataContract.IsUserGroupCustomAttributeExist = applicantOrderCart.IsUserGroupCustomAttributeExist;
            if (applicantOrderCart.IsUserGroupCustomAttributeExist)
            {
                // _applicantOrderDataContract.lstAttributeValuesForUserGroup = Presenter.AddCustomAttributeValuesForUserGroup(applicantOrderCart.lstCustomAttributeUserGroupIDs
                //                                                                        , SysXWebSiteUtils.SessionService.OrganizationUserId, organizationUserID);
            }


            Order order = ComplianceDataManager.GetOrderDetailsByOrderId(CurrentViewContext.TenantId, applicantOrderCart.lstApplicantOrder[0].OrderId);

            //Order order = new Order();
            //order.OrderID = applicantOrderCart.lstApplicantOrder[0].OrderId;
            //order.OrganizationUserProfileID = applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserProfileID;
            //order.IsDeleted = false;
            //order.CreatedByID = CurrentLoggedInUserId;
            //order.CreatedOn = DateTime.Now;
            //order.SelectedNodeID = applicantOrderCart.SelectedHierarchyNodeID;
            //order.HierarchyNodeID = GetHierarchyNodeIDByPackageType(_tenantId, applicantOrderCart);
            //order.ArchiveStateID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpArchiveState>(_tenantId).Where(cond => cond.AS_Code == ArchiveState.Active.GetStringValue()).FirstOrDefault().AS_ID;

            //  order.OrderStatusID =   get status id on the basis of permissions

            //order.OrderRequestTypeID = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderRequestType>(_tenantId).Where(cond => cond.ORT_Code == OrderRequestType.NewOrder.GetStringValue()).FirstOrDefault().ORT_ID;
            //order.OrderPackageType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderPackageType>(_tenantId).Where(cond => cond.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
            //order.OrderDate = DateTime.Now;
            //order.OrderMachineIP = applicantOrderCart.lstApplicantOrder[0].ClientMachineIP;
            ////order.TotalPrice = Convert.ToDecimal("0.0000");
            //order.GrandTotal = applicantOrderCart.GrandTotal;
            //order.PaymentOptionID = applicantOrderCart.lstPaymentGrouping.FirstOrDefault().PaymentModeId;
            //order.OriginalSettlementPrice = Convert.ToDecimal("0.0000");
            //order.OrderStatusID = 2;
            //order.OrderNumber = applicantOrderCart.lstApplicantOrder[0].OrderNumber;

            Entity.ClientEntity.TransactionGroup _transactionGrp = new Entity.ClientEntity.TransactionGroup();
            //Changes for Admin Entry
            //_transactionGrp.Order = order;
            _transactionGrp.TG_OrderID = applicantOrderCart.OrderId;
            _transactionGrp.TG_TxnDate = DateTime.Now;
            _transactionGrp.TG_CreatedByID = CurrentLoggedInUserId;
            _transactionGrp.TG_CreatedOn = DateTime.Now;


            Entity.ClientEntity.OrganizationUserProfile _orgUserProfile = applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile; // new OrganizationUserProfile() 

            foreach (var applicantOrder in applicantOrderCart.lstApplicantOrder)
            {
                //_orgUserProfile = applicantOrder.OrganizationUserProfile;
                _isUpdateMainProfile = applicantOrder.UpdatePersonalDetails;
            }

            Int32 organizationUserID = _orgUserProfile.OrganizationUserID;// SysXWebSiteUtils.SessionService.OrganizationUserId;

            #endregion



            var _lstSelectedPOIds = _paymentModesData.Where(pmd => !pmd.isZP).Select(x => x.poid);


            #region BillingInfo

            applicantOrderCart.IsBiilingInfoSameAsAccountInfo = false;

            #endregion

            #region Read Data from Pricing XML

            //List<Package_PricingData> _lstPricingData = null;
            //if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            //{
            //    String _pricingDataXML = applicantOrderCart.lstApplicantOrder[0].PricingDataXML;
            //    if (!String.IsNullOrEmpty(_pricingDataXML))
            //    {
            //        _lstPricingData = new List<Package_PricingData>();
            //        _lstPricingData = GenerateDataFromPricingXML(_pricingDataXML);
            //    }
            //}

            #endregion
            //Added check to save data only if order request type is not for Applicant completing order process for "Sent for online payment" type.[UAT-1648]
            //Order _order = new Order();
            if (String.Compare(Convert.ToString(applicantOrderCart.OrderRequestType), OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) != AppConsts.NONE)
            {
                #region Set Order object, based on OrderRequestType                
                //_order.OrderMachineIP = applicantOrderCart.lstApplicantOrder[0].ClientMachineIP;
                //_order.TotalPrice = Convert.ToDecimal(applicantOrderCart.CurrentPackagePrice);
                //_order.ProgramDuration = applicantOrderCart.ProgramDuration;
                //_order.HierarchyNodeID = GetHierarchyNodeIDByPackageType(_tenantId, applicantOrderCart);
                //_order.OriginalSettlementPrice = Convert.ToDecimal(applicantOrderCart.SettleAmount);
                ////Set SelecteHierarchyNodeID in SelectedNodeID of order [UAT-1067]
                //_order.SelectedNodeID = applicantOrderCart.SelectedHierarchyNodeID;

                order.OrderMachineIP = applicantOrderCart.lstApplicantOrder[0].ClientMachineIP;
                //order.TotalPrice = Convert.ToDecimal(applicantOrderCart.CurrentPackagePrice);
                order.ProgramDuration = applicantOrderCart.ProgramDuration;
                //order.HierarchyNodeID = GetHierarchyNodeIDByPackageType(_tenantId, applicantOrderCart);
                order.OriginalSettlementPrice = Convert.ToDecimal(applicantOrderCart.SettleAmount);
                //Set SelecteHierarchyNodeID in SelectedNodeID of order [UAT-1067]
                order.SelectedNodeID = applicantOrderCart.SelectedHierarchyNodeID;

                //if (applicantOrderCart.OrderRequestType != null)
                //    _order.OrderRequestTypeID = Presenter.GetLKPOrderRequestTypeID(applicantOrderCart.OrderRequestType);

                if (applicantOrderCart.OrderRequestType != null)
                    order.OrderRequestTypeID = Presenter.GetLKPOrderRequestTypeID(applicantOrderCart.OrderRequestType);

                //if (applicantOrderCart.IsRushOrderIncluded)
                //{
                //    isRushOrder = true;
                //    _order.RushOrderPrice = Convert.ToDecimal(applicantOrderCart.RushOrderPrice.Trim());
                //    _order.IsRushOrderForExistingOrder = false;

                //    //UAT 264
                //    Decimal _netPrice = (applicantOrderCart.CurrentPackagePrice.Value - applicantOrderCart.SettleAmount)
                //                    + Convert.ToDecimal(applicantOrderCart.RushOrderPrice.Trim());
                //    _order.GrandTotal = _netPrice <= 0 ? 0 : _netPrice + GetBackgroundPackagesPrice(applicantOrderCart);

                //    GenerateGroupedAmount(applicantOrderCart, _paymentModesData, _tenantId, _applicantOrderDataContract, _lstClientPaymentOptions);
                //}
                //else
                //{
                // No need to subtract the Settlement Amount from the Grand Total, as it was already done, 
                // before it was added to the session, from the PendingOrder.cs (AddCompliancePackageDataToSession() function)
                //_order.GrandTotal = Convert.ToDecimal(applicantOrderCart.GrandTotal) + GetBackgroundPackagesPrice(applicantOrderCart);

                order.GrandTotal = Convert.ToDecimal(applicantOrderCart.GrandTotal) + GetBackgroundPackagesPrice(applicantOrderCart);

                //GenerateGroupedAmount(applicantOrderCart, _paymentModesData, _tenantId, _lstClientPaymentOptions);
                //}

                _applicantOrderDataContract.lstOrderPackageTypes = Presenter.GetOrderPackageTypeList();
                _applicantOrderDataContract.lstOrderStatus = Presenter.GetOrderStatusList();


                #endregion
                //[SS]:Commented for Admin Entry
                //if (applicantOrderCart.lstApplicantOrder.IsNotNull() && applicantOrderCart.lstApplicantOrder[0].OrderId == AppConsts.NONE)
                //{
                Int32 _orderId = order.OrderID;
                //Int32 _orderId = applicantOrderCart.OrderId;
                //_order.CreatedByID = organizationUserID;
                //_order.OrderID = _orderId;
                //order.CreatedByID = organizationUserID;
                //order.OrderID = _orderId;
                //order.OrderNumber = applicantOrderCart.lstApplicantOrder[0].OrderNumber;
                //UAT-3757
                if (!applicantOrderCart.bufferSignature.IsNullOrEmpty())
                {
                    OrderApplicantSignature orderApplicantSignature = new OrderApplicantSignature();
                    orderApplicantSignature.OAS_Signature = applicantOrderCart.bufferSignature;
                    orderApplicantSignature.OAS_OrderId = _orderId;
                    orderApplicantSignature.OAS_CreatedOn = DateTime.Now;
                    orderApplicantSignature.OAS_ModifiedOn = DateTime.Now;
                    order.OrderApplicantSignatures.Add(orderApplicantSignature);
                }

                #region Set OrderPackageType, based on the Packages selected

                //_order.OrderPackageType = GetOrderPackageType(_tenantId, applicantOrderCart);
                //order.OrderPackageType = GetOrderPackageType(_tenantId, applicantOrderCart);
                //order.OrderPaymentDetails = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<OrderPaymentDetail>();
                #endregion

                #region Set Data in Contract class & Save in database

                _orgUserProfile.OrganizationUserID = _orgUserProfile.OrganizationUserID;// SysXWebSiteUtils.SessionService.OrganizationUserId;
                List<TypeCustomAttributes> lst = applicantOrderCart.GetCustomAttributeValues();

                //Set Is Complance Package Selected
                //_applicantOrderDataContract.IsCompliancePackageSelected = applicantOrderCart.IsCompliancePackageSelected;
                //_applicantOrderDataContract.OrganizationUserProfile = _orgUserProfile;
                //_applicantOrderDataContract.ProgramPackageSubscriptionId = _prgPackageSubscriptionId;
                //_applicantOrderDataContract.lstGroupedData = applicantOrderCart.lstPaymentGrouping;
                //_applicantOrderDataContract.TenantId = _tenantId;
                //_applicantOrderDataContract.lstAttributeValues = lst;
                //_applicantOrderDataContract.LastNodeDPMId = Convert.ToInt32(applicantOrderCart.SelectedHierarchyNodeID);
                //_applicantOrderDataContract.lstBackgroundPackages = applicantOrderCart.lstApplicantOrder[0].lstPackages;
                //_applicantOrderDataContract.lstPricingData = _lstPricingData;
                //_applicantOrderDataContract.IsSendBackgroundReport = applicantOrderCart.lstApplicantOrder[0].IsSendBackgroundReport;
                // SelectedPaymentModeId = paymentModeId, UAT 916

                //UAT 1438: Enhancement to allow students to select a User Group.
                _applicantOrderDataContract.IsUserGroupCustomAttributeExist = applicantOrderCart.IsUserGroupCustomAttributeExist;
                if (applicantOrderCart.IsUserGroupCustomAttributeExist)
                {
                    _applicantOrderDataContract.lstAttributeValuesForUserGroup = Presenter.AddCustomAttributeValuesForUserGroup(applicantOrderCart.lstCustomAttributeUserGroupIDs, SysXWebSiteUtils.SessionService.OrganizationUserId, organizationUserID); //applicantOrderCart.GetCustomAttributeValuesForUserGroup();
                }

                if (!applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.IsNullOrEmpty())
                    _applicantOrderDataContract.lstBkgOrderData = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData;

                Boolean _storeBrowserAgent = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings[AppConsts.APP_SETTING_STORE_BROWSER_AGENTS]);

                if (_storeBrowserAgent)
                    _applicantOrderDataContract.UserBrowserAgentString = System.Web.HttpContext.Current.Request.UserAgent;

                //UAT 3521
                //if (applicantOrderCart.IsLocationServiceTenant)
                //{
                //    String _orderNumber = String.Empty;
                //    _orderNumber = "#OrderID#" + "-" + _tenantId + "-" + SysXUtils.GenerateRandomNo(2) + "-" + SysXUtils.RandomString(2, false) + "-" + applicantOrderCart.FingerPrintData.LocationId;
                //    _order.OrderNumber = _orderNumber;
                //}

                // UAT 916
                //_dicInvoiceNumber = Presenter.SubmitApplicantOrder(_order, _applicantOrderDataContract, _isUpdateMainProfile,
                //    applicantOrderCart.lstPrevAddresses, applicantOrderCart.lstPersonAlias, out _paymentModeCode, out _orderId, organizationUserID, (applicantOrderCart.CompliancePackages.IsNotNull() && applicantOrderCart.CompliancePackages.Count > 0 ? applicantOrderCart.CompliancePackages.Values.ToList() : null), applicantOrderCart.lstApplicantOrder);

                _dicInvoiceNumber = Presenter.SubmitApplicantOrder(order, _applicantOrderDataContract, _isUpdateMainProfile,
                    applicantOrderCart.lstPrevAddresses, applicantOrderCart.lstPersonAlias, out _paymentModeCode, out _orderId, organizationUserID, (applicantOrderCart.CompliancePackages.IsNotNull() && applicantOrderCart.CompliancePackages.Count > 0 ? applicantOrderCart.CompliancePackages.Values.ToList() : null), applicantOrderCart.lstApplicantOrder);


                _invoiceNumbers = GetInvoiceNumbers(_dicInvoiceNumber);

                #endregion

                #region Call Method to update EDS status and EDS custom Form Data

                OrderPaymentDetail orderPaymentDetail = ComplianceDataManager.GetAllPaymentDetailsOfOrderByOrderID(_tenantId, order.OrderID).Where(x => x.lkpPaymentOption.Code.Equals(_dicInvoiceNumber.Keys.FirstOrDefault())).FirstOrDefault();
                if (!ComplianceDataManager.GetOrderById(TenantId, order.OrderID).OrderPaymentDetails.IsNullOrEmpty())
                {
                    foreach (var item in ComplianceDataManager.GetOrderById(TenantId, order.OrderID).OrderPaymentDetails.ToList())
                    {
                        order.OrderPaymentDetails.Add(item);
                    }
                }

                //Changes related to UAT-5114

                if (!String.IsNullOrEmpty(_applicantOrderDataContract.ApplicantInviteSubmitStatusTypeCode)
                        && _applicantOrderDataContract.ApplicantInviteSubmitStatusTypeCode == ApplicantInviteSubmitStatusType.TRANSMIT.GetStringValue())
                {

                    //This method is called to handle the scenario where amount is zero and order is approved with zero amount automatically.
                    UpdateEDSStatus(_applicantOrderDataContract, _orderId, order);
                }

                if (applicantOrderCart.ApplicantDisclaimerDocumentId.IsNotNull() || (applicantOrderCart.ApplicantDisclosureDocumentIds.IsNotNull() && applicantOrderCart.ApplicantDisclosureDocumentIds.Count > 0))
                {
                    //Presenter.SaveApplicantEsignatureDocument(_tenantId, Convert.ToInt32(applicantOrderCart.ApplicantDisclaimerDocumentId), applicantOrderCart.ApplicantDisclosureDocumentIds, _orderId, _applicantOrderDataContract.OrganizationUserProfile.OrganizationUserProfileID, organizationUserID, _order.OrderNumber);

                    Presenter.SaveApplicantEsignatureDocument(_tenantId, Convert.ToInt32(applicantOrderCart.ApplicantDisclaimerDocumentId), applicantOrderCart.ApplicantDisclosureDocumentIds, _orderId, _applicantOrderDataContract.OrganizationUserProfile.OrganizationUserProfileID, organizationUserID, order.OrderNumber);


                    //BasePage.LogOrderFlowSteps("Default.aspx - STEP 4: Method 'SaveApplicantEsignatureDocument' executed successfully for OrgUserId:" + organizationUserID + " and OrderId: " + _orderId);

                    //if (_order.OrderGroupOrderNavProp.IsNotNull() && _order.OrderGroupOrderNavProp.Count > 0)
                    //{
                    if (order.OrderGroupOrderNavProp.IsNotNull() && order.OrderGroupOrderNavProp.Count > 0)
                    {
                        foreach (Order childOrder in order.OrderGroupOrderNavProp)
                        {
                            Presenter.SaveApplicantEsignatureDocument(_tenantId, Convert.ToInt32(applicantOrderCart.ApplicantDisclaimerDocumentId), applicantOrderCart.ApplicantDisclosureDocumentIds, childOrder.OrderID, _applicantOrderDataContract.OrganizationUserProfile.OrganizationUserProfileID, organizationUserID, childOrder.OrderNumber);
                            // BasePage.LogOrderFlowSteps("Default.aspx - STEP 4.1: Method 'SaveApplicantEsignatureDocument' executed successfully for Child Orders of OrgUserId:" + organizationUserID + " and OrderId: " + childOrder.OrderID);
                        }
                    }
                }
                #endregion
                //[SS]:Commented for Admin Entry
                //#region UAT-1560:WB: We should be able to add documents that need to be signed to the order process
                //Update Additional Documents of Applicants in Applicant Document table.
                if (applicantOrderCart.ApplicantAdditionalDocumentIds.IsNotNull() && applicantOrderCart.ApplicantAdditionalDocumentIds.Count > 0
                    && applicantOrderCart.IsAdditionalDocumentExist)
                {
                    Boolean isSubscriptionExist = Presenter.IsSubscriptionExistForApplicant(organizationUserID, _tenantId);
                    //Use this mapping at the time compliance package subscription created for not approved orders.
                    Boolean needToSaveMappingInGenricDocMapping = false;
                    if (!isSubscriptionExist)
                    {
                        needToSaveMappingInGenricDocMapping = true;
                    }

                    //UAT-3745
                    List<SystemDocBkgSvcMapping> lstSystemDocBkgSvcMapping = !applicantOrderCart.lstSystemDocBkgSvcMapping.IsNullOrEmpty() ? applicantOrderCart.lstSystemDocBkgSvcMapping : null;
                    //End

                    //Added lstSystemDocBkgSvcMapping in UAT-3745
                    List<ApplicantDocument> additionalDocList = Presenter.UpdateApplicantAdditionalEsignatureDocument(_tenantId, applicantOrderCart.ApplicantAdditionalDocumentIds, _orderId, _applicantOrderDataContract.OrganizationUserProfile.OrganizationUserProfileID,
                                                                                     organizationUserID, needToSaveMappingInGenricDocMapping, applicantOrderCart.AdditionalDocSendToStudent, lstSystemDocBkgSvcMapping);
                }
                //#endregion
                //Will have to change if multiple orders at a time
                applicantOrderCart.lstApplicantOrder[0].OrderId = _orderId;
                //applicantOrderCart.lstApplicantOrder[0].OrderNumber = _order.OrderNumber;
                applicantOrderCart.lstApplicantOrder[0].OrderNumber = order.OrderNumber;
                //}

                //[SS]:Commented for Admin Entry
                //else
                //{
                //    _order.OrderID = applicantOrderCart.lstApplicantOrder[0].OrderId;
                //    //_order.ModifiedByID = SysXWebSiteUtils.SessionService.OrganizationUserId;
                //    _order.ModifiedByID = organizationUserID;

                //    var _ordStsDC = new OrderStatusDataContract();
                //    var _lstOrderStatus = Presenter.GetOrderStatusList().Where(orderSts => !orderSts.IsDeleted).ToList();

                //    var _bkgPkgTypeId = _applicantOrderDataContract.lstOrderPackageTypes.Where(opt => opt.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
                //    var _compliancePkgTypeId = _applicantOrderDataContract.lstOrderPackageTypes.Where(opt => opt.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;

                //    var lstBkgOrder = _order.BkgOrders.FirstOrDefault();
                //    var _rushOrdstatusId = AppConsts.NONE;

                //    var lst = new List<OrderStatusDataContract>();
                //    foreach (var grpdData in applicantOrderCart.lstPaymentGrouping)
                //    {
                //        String statusCode = String.Empty;

                //        if (grpdData.PaymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower()
                //            || grpdData.PaymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())
                //            statusCode = ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue();
                //        else
                //            statusCode = ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue();

                //        var _statusId = _lstOrderStatus.Where(ordSts => ordSts.Code == statusCode).FirstOrDefault().OrderStatusID;

                //        _ordStsDC.Amount = grpdData.TotalAmount;
                //        _ordStsDC.PaymentOptionId = grpdData.PaymentModeId;
                //        _ordStsDC.StatusId = _statusId;

                //        _ordStsDC.lstPackages = new List<OrderPkgPaymentDetail>();

                //        foreach (var pkg in grpdData.lstPackages)
                //        {
                //            var pkgId = Convert.ToInt32(pkg.Key.Split('_')[0]);
                //            var _bopId = AppConsts.NONE;
                //            // If it is BkgPackage, get its BOPID 
                //            if (pkg.Value)
                //            {
                //                var _bop = lstBkgOrder.BkgOrderPackages.FirstOrDefault(bop => bop.BkgPackageHierarchyMapping.BPHM_BackgroundPackageID == pkgId
                //                     && !bop.BOP_IsDeleted);
                //                if (_bop.IsNotNull())
                //                    _bopId = _bop.BOP_ID;
                //            }

                //            // 'OrderPaymentDetails' object will be attached by the 'AddOnlinePaymentTransaction' 
                //            //  method call, in 'UpdateFailedOrder'
                //            var _ordPkgPayDetails = new OrderPkgPaymentDetail();
                //            _ordPkgPayDetails.OPPD_IsDeleted = false;
                //            _ordPkgPayDetails.OPPD_BkgOrderPackageID = (!pkg.Value || _bopId == AppConsts.NONE) ? (Int32?)null : _bopId;
                //            _ordPkgPayDetails.OPPD_OrderPackageTypeID = pkg.Value == true ? _bkgPkgTypeId : _compliancePkgTypeId;
                //            //_ordPkgPayDetails.OPPD_CreatedBy = SysXWebSiteUtils.SessionService.OrganizationUserId;
                //            _ordPkgPayDetails.OPPD_CreatedBy = organizationUserID;
                //            _ordStsDC.lstPackages.Add(_ordPkgPayDetails);
                //        }
                //        lst.Add(_ordStsDC);


                //        //if (isRushOrder)
                //        //{
                //        //    // Check for the Compliance package in the current groupd being traversed
                //        //    //var _cmpPkg = grpdData.lstPackages.Where(x => !x.Value).FirstOrDefault();

                //        //    // If it is having a Compliance package, and is Rush Order, use the Same status for RushOrder, as that of group
                //        //    //if (_cmpPkg.IsNotNull())
                //        //    //    _rushOrdstatusId = _statusId;
                //        //}
                //    }
                //    _dicInvoiceNumber = Presenter.UpdateFailedOrder(_tenantId, _order, isRushOrder, lst, _rushOrdstatusId);

                //    _invoiceNumbers = GetInvoiceNumbers(_dicInvoiceNumber);
                //    //BasePage.LogOrderFlowSteps("Default.aspx - STEP 3: Method 'ComplianceDataManager.UpdateFailedOrder' executed"
                //    //+ " for OrgUserId:" + organizationUserID + " and Invoice numbers per payment mode code are: " + _invoiceNumbers);
                //}
            }
            //[SS]:Commented for Admin Entry
            ////// [UAT-1648]:As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
            // else if (String.Compare(Convert.ToString(applicantOrderCart.OrderRequestType), OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) == AppConsts.NONE)
            //{
            //    //BasePage.LogOrderFlowSteps("Default.aspx - STEP 2: OrderRequestType for OrgUserId:" + organizationUserID + " is: " + applicantOrderCart.OrderRequestType);

            //    Int32 _orderId = applicantOrderCart.lstApplicantOrder[0].OrderId.IsNullOrEmpty() || applicantOrderCart.lstApplicantOrder[0].OrderId == AppConsts.NONE ? applicantOrderCart.OrderId : applicantOrderCart.lstApplicantOrder[0].OrderId;

            //    //Get Existing order for completing order process.
            //    _order = Presenter.GetOrderById(_orderId);
            //    List<Int32> newlyAddedOPDIdList = new List<Int32>();

            //    #region Set Applicant Order Data Contract from applicant order cart data.
            //    GenerateGroupedAmount(applicantOrderCart, _paymentModesData, _tenantId, _lstClientPaymentOptions);
            //    _applicantOrderDataContract.IsCompliancePackageSelected = applicantOrderCart.IsCompliancePackageSelected;
            //    _applicantOrderDataContract.OrganizationUserProfile = _orgUserProfile;
            //    _applicantOrderDataContract.ProgramPackageSubscriptionId = _prgPackageSubscriptionId;
            //    _applicantOrderDataContract.lstGroupedData = applicantOrderCart.lstPaymentGrouping;
            //    _applicantOrderDataContract.TenantId = _tenantId;
            //    _applicantOrderDataContract.LastNodeDPMId = Convert.ToInt32(applicantOrderCart.SelectedHierarchyNodeID);
            //    _applicantOrderDataContract.lstBackgroundPackages = applicantOrderCart.lstApplicantOrder[0].lstPackages;
            //    _applicantOrderDataContract.lstPricingData = _lstPricingData;
            //    _applicantOrderDataContract.IsSendBackgroundReport = applicantOrderCart.lstApplicantOrder[0].IsSendBackgroundReport;
            //    #endregion

            //    //Get browser agent setting from webconfig
            //    Boolean _storeBrowserAgent = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings[AppConsts.APP_SETTING_STORE_BROWSER_AGENTS]);
            //    if (_storeBrowserAgent)
            //        _applicantOrderDataContract.UserBrowserAgentString = System.Web.HttpContext.Current.Request.UserAgent;

            //    _applicantOrderDataContract.lstOrderPackageTypes = Presenter.GetOrderPackageTypeList();
            //    _applicantOrderDataContract.lstOrderStatus = Presenter.GetOrderStatusList();

            //    //Update Existing Order Data.
            //    _dicInvoiceNumber = Presenter.UpdateApplicantCompletingOrderProcess(_order, _applicantOrderDataContract, out _paymentModeCode, organizationUserID, out newlyAddedOPDIdList, (applicantOrderCart.CompliancePackages.IsNotNull()
            //                                                                                    && applicantOrderCart.CompliancePackages.Count > 0 ? applicantOrderCart.CompliancePackages.Values.ToList() : null));

            //    //BasePage.LogOrderFlowSteps("Default.aspx - STEP 3: Method 'ComplianceDataManager.UpdateApplicantCompletingOrderProcess' executed successfully"
            //    //+ " for OrgUserId:" + organizationUserID + " and OrderId: " + _orderId);

            //    applicantOrderCart.RecentAddedOPDs = newlyAddedOPDIdList;
            //    #region Call Method to update EDS status and EDS custom Form Data

            //    //This method is called to handle the scenario where amount is zero and order is approved with zero amount automatically.
            //    UpdateEDSStatus(_applicantOrderDataContract, _orderId, _order, newlyAddedOPDIdList);

            //    //ComplianceDataManager.InsertAutomaticInvitationLog(_tenantId, _orderId, SysXWebSiteUtils.SessionService.OrganizationUserId); //UAT-2388
            //    //                                                                                                                             //Send Notification for print scan
            //    //UAT-1358:Complio Notification to applicant for PrintScan
            //    //SendPrintScanNotification(_orderId, _order, null, false, _applicantOrderDataContract.TenantId, newlyAddedOPDIdList);
            //    #endregion
            //    //Will have to change if multiple orders at a time
            //    applicantOrderCart.lstApplicantOrder[0].OrderId = _orderId;
            //    applicantOrderCart.lstApplicantOrder[0].OrderNumber = _order.OrderNumber;

            //}
            // }

            applicantOrderCart.InvoiceNumber = _dicInvoiceNumber;
            // applicantOrderCart.IncrementOrderStepCount();
            applicantOrderCart.AddOrderStageTrackID(OrderStages.OrderPayment);

            #region Redirect URL Logic

            //generatedInvoiceNumber = Guid.NewGuid().ToString(); // TO BE REMOVED
            String _redirectUrl = String.Empty;
            // var _onlineInvoiceNumber = String.Empty;

            var _lstPG = _applicantOrderDataContract.lstGroupedData.Select(x => x);
            Boolean isPaypalSelected = _lstPG.Any(pmt => pmt.PaymentModeCode == PaymentOptions.Paypal.GetStringValue());
            Boolean isCreditCardSelected = _lstPG.Any(pmt => pmt.PaymentModeCode == PaymentOptions.Credit_Card.GetStringValue()
                                                            && pmt.TotalAmount > 0);

            //var _onlineModeCode = String.Empty;
            //if (!isCreditCardSelected) // This will make sure that Even if the CC with Amount zero was in Cart, than do not use that Payment Mode code
            //    _onlineModeCode = PaymentOptions.Paypal.GetStringValue();
            //else
            //    _onlineModeCode = PaymentOptions.Credit_Card.GetStringValue();

            //var _onlinePaymentType = _dicInvoiceNumber.Where(d => d.Key == _onlineModeCode).FirstOrDefault();


            //if (_onlinePaymentType.IsNotNull())
            //    _onlineInvoiceNumber = _onlinePaymentType.Value;

            //UAT-1185 Passing multiple order ids to payment pages
            string strOrderID = string.Empty;
            //if (_onlineInvoiceNumber.IsNotNull())
            //    strOrderID = ExtractOrderNumbersFromInvoiveNumber(_onlineInvoiceNumber);
            if (strOrderID.IsNullOrEmpty())
                strOrderID = order.OrderID.ToString();

            //Dictionary<String, String> queryString = new Dictionary<String, String>();
            //queryString = new Dictionary<String, String>
            //                                                     {
            //                                                        { "invnum", _onlineInvoiceNumber},
            //                                                        {"OrderId", strOrderID}
            //                                                     };


            if (String.IsNullOrEmpty(_invoiceNumbers))
            {
                _invoiceNumbers = GetInvoiceNumbers(_dicInvoiceNumber);
            }

            // UAT 916 --  Manage the PaymentModeCode here & Handle Invoice Number
            //if (String.IsNullOrEmpty(generatedInvoiceNumber) || (!(_paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower()) &&
            //!(_paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())))
            //[SS]:Commented for Admin Entry
            //--------------------------------------------------
            //if (!isCreditCardSelected && !isPaypalSelected)
            //{
            //    // In case, crash in order generation
            //    applicantOrderCart.AddOrderStageTrackID(OrderStages.OnlineConfirmation);
            //    SubmitOrder(applicantOrderCart);
            //    //_errorMessage = (_dicInvoiceNumber.Count == 0 || _dicInvoiceNumber.Any(d => d.Value.IsNullOrEmpty())) ? "Error in order placement." : String.Empty;
            //    //_redirectUrl = RedirectConfirmationPage(_errorMessage);
            //    //_redirectUrlType = "internal";

            //    //BasePage.LogOrderFlowSteps("Default.aspx - STEP 6: Payment options other then 'Credit Card' or 'Paypal', were selected by OrgUserId: " + organizationUserID + " and Invoice Number(s): " + _invoiceNumbers);
            //}
            //-----------------------------------------
            //else if (_paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower())
            //else if (isCreditCardSelected)
            //{
            //    _redirectUrl = "Pages/CIMAccountSelection.aspx";
            //    _redirectUrl = String.Format(_redirectUrl + "?args={0}", queryString.ToEncryptedQueryString());
            //    _redirectUrlType = "internal";

            //    BasePage.LogOrderFlowSteps("Default.aspx - STEP 6: Redirecting to CIMAccountSelection.aspx for: " + organizationUserID + " and Invoice Number(s): " + _invoiceNumbers);
            //}
            //else if (_order.lkpOrderStatu.IsNotNull() && _order.lkpOrderStatu.Code == ApplicantOrderStatus.Paid.GetStringValue())
            //[SS]:Commented for Admin Entry
            //--------------------------------------------------
            //else if (AreAllOrdersPaid(_order))
            //{
            //    applicantOrderCart.AddOrderStageTrackID(OrderStages.OnlineConfirmation);
            //    SubmitOrder(applicantOrderCart);
            //    //_errorMessage = String.IsNullOrEmpty(generatedInvoiceNumber) ? "Error in order placement." : String.Empty;
            //    //_redirectUrl = RedirectConfirmationPage(_errorMessage);
            //    // _redirectUrlType = "internal";

            //    // BasePage.LogOrderFlowSteps("Default.aspx - STEP 6: All orders are PAID, redirecting to Order Confirmation page for OrgUserId: " + organizationUserID + " and Invoice Number(s): " + _invoiceNumbers);
            //}
            //--------------------------------------------------
            //else if (_paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())
            //else if (isPaypalSelected)
            //{
            //    _redirectUrl = "Pages/PaypalPaymentSubmission.aspx";
            //    _redirectUrl = String.Format(_redirectUrl + "?args={0}", queryString.ToEncryptedQueryString());
            //    _redirectUrlType = "external";

            //    BasePage.LogOrderFlowSteps("Default.aspx - STEP 6: Redirecting to PaypalPaymentSubmission.aspx for: " + organizationUserID + " and Invoice Number(s): " + _invoiceNumbers);
            //}

            //if (!url.ToLower().StartsWith("http"))
            //{
            //    url = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + url;
            //}

            ////if (_paymentModeCode == PaymentOptions.Credit_Card.GetStringValue() || _paymentModeCode == PaymentOptions.Paypal.GetStringValue())
            //if (isCreditCardSelected || isPaypalSelected)
            //{
            //    Int32 timeout = GetSessionTimeoutValue();
            //    ApplicationDataManager.AddWebApplicationData(_onlineInvoiceNumber, url, timeout);

            //    BasePage.LogOrderFlowSteps("Default.aspx - STEP 6.1: Method 'ApplicationDataManager.AddWebApplicationData' executed successfully for: " + organizationUserID + " and Invoice Number(s): " + _invoiceNumbers);
            //}

            #endregion

            if (!_dicInvoiceNumber.IsNullOrEmpty() && _dicInvoiceNumber.Count > AppConsts.NONE)
            {
                if (!String.IsNullOrEmpty(_applicantOrderDataContract.ApplicantInviteSubmitStatusTypeCode)
                            && _applicantOrderDataContract.ApplicantInviteSubmitStatusTypeCode == ApplicantInviteSubmitStatusType.TRANSMIT.GetStringValue())
                {
                    //Send service form automatically
                    BackgroundProcessOrderManager.SendOrderNotificationForAutomaticSendSvcForms(TenantId, applicantOrderCart.OrderId, applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserID);
                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// Gets the Order Package type, based on the packages selected
        /// </summary>
        /// <param name="_tenantId"></param>
        /// <param name="applicantOrderCart"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        private Int32 GetOrderPackageType(Int32 tenantId, ApplicantOrderCart applicantOrderCart)
        {
            List<lkpOrderPackageType> _lstOrderPackageType = Presenter.GetOrderPackageTypeList();

            //if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.IsCompliancePackageSelected)
            //{
            //    return _lstOrderPackageType.Where(opt => opt.OPT_Code == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
            //}
            //else if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && !applicantOrderCart.IsCompliancePackageSelected)
            //{
            //    return _lstOrderPackageType.Where(opt => opt.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
            //}
            //else if (applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.IsCompliancePackageSelected)
            //{
            //    return _lstOrderPackageType.Where(opt => opt.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
            //}
            //return AppConsts.MINUS_ONE;
            return _lstOrderPackageType.Where(opt => opt.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
        }

        /// <summary>
        /// Checks whether all the associated orders are paid or not
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private static Boolean AreAllOrdersPaid(Order order)
        {
            // If there is NO OrderPaymentDetail entry 
            // OR 
            // There is any entry but there is at least on entry with order not Paid
            return order.OrderPaymentDetails.IsNullOrEmpty() ||
                order.OrderPaymentDetails.Any(opd => opd.OPD_IsDeleted == false
                                    && (opd.lkpOrderStatu.IsNull()
                                        ||
                                        (
                                          opd.lkpOrderStatu.IsNotNull()
                                            &&
                                          opd.lkpOrderStatu.Code != ApplicantOrderStatus.Paid.GetStringValue()
                                         )
                                       )
                    ) ? false : true;



            return false;
        }
        /// <summary>
        /// Get the Invoice numbers string from the Dictionary
        /// </summary>
        /// <param name="_dicInvoiceNumber"></param>
        /// <returns></returns>
        private static String GetInvoiceNumbers(Dictionary<String, String> _dicInvoiceNumber)
        {
            StringBuilder _sbInvNumbers = new StringBuilder();
            foreach (var invNo in _dicInvoiceNumber)
            {
                _sbInvNumbers.Append(invNo.Key + " - " + invNo.Value + " || ");
            }

            return Convert.ToString(_sbInvNumbers).Substring(0, Convert.ToString(_sbInvNumbers).LastIndexOf("||") - 1);
        }

        /// <summary>
        /// Method to update the EDS Related Data For customformdata and also update the external vendor dispatch status.
        /// </summary>
        /// <param name="applicantOrderDataContract">applicantOrderDataContract</param>
        /// <param name="orderId">orderId</param>
        /// <param name="userOrder">userOrder</param>
        private void UpdateEDSStatus(ApplicantOrderDataContract applicantOrderDataContract, Int32 orderId, Order userOrder, List<Int32> newlyAddedOPDIds = null)
        {
            if (userOrder.IsNotNull())
            {
                OrderPaymentDetail _orderPaymentDetail = null;
                List<OrderPaymentDetail> lstOrderPaymentDetail = new List<OrderPaymentDetail>();

                if (!userOrder.OrderPaymentDetails.IsNullOrEmpty())
                    lstOrderPaymentDetail = userOrder.OrderPaymentDetails.Where(slct => !slct.OPD_IsDeleted && (newlyAddedOPDIds == null || newlyAddedOPDIds.Contains(slct.OPD_ID))).ToList();

                if (!lstOrderPaymentDetail.IsNullOrEmpty())
                {
                    foreach (OrderPaymentDetail opd in lstOrderPaymentDetail)
                    {
                        bool IsOrderPaymentIncludeEDSService = Presenter.IsOrderPaymentIncludeEDSService(opd.OPD_ID);
                        if (!opd.IsNullOrEmpty() && IsOrderPaymentIncludeEDSService && opd.lkpPaymentOption.IsNotNull() && opd.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue())
                        {
                            _orderPaymentDetail = opd;
                            break;
                        }
                    }
                }

                if (orderId > 0 && !_orderPaymentDetail.IsNullOrEmpty())
                {
                    String _prevStatus = ApplicantOrderStatus.Paid.GetStringValue();
                    List<lkpOrderStatu> orderStatusList = Presenter.GetOrderStatusList();
                    Int32 orderStatusId = orderStatusList.Where(orderSts => orderSts.Code.ToLower() == _prevStatus.ToLower() && !orderSts.IsDeleted).FirstOrDefault().OrderStatusID;
                    #region E-DRUG SCREENING
                    BkgOrder bkgOrderObj = Presenter.GetBkgOrderByOrderID(orderId);
                    if (!bkgOrderObj.IsNullOrEmpty() && !applicantOrderDataContract.lstBackgroundPackages.IsNullOrEmpty() && (applicantOrderDataContract.lstBackgroundPackages.Count() > 0))
                    {
                        List<Int32> lstBackgroundPackageId = applicantOrderDataContract.lstBackgroundPackages.Select(cnd => cnd.BPAId).ToList();
                        String extVendorId = String.Empty;
                        ClearStarCCF objClearstarCCf = new ClearStarCCF();

                        ClearStarWebCCFContract clearStarWebCCFContract = new ClearStarWebCCFContract();
                        var LoggerService = (System.Web.HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                        var ExceptiomService = (System.Web.HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                        String result = Presenter.GetClearStarServiceId(lstBackgroundPackageId, BkgServiceType.ELECTRONICDRUGSCREEN.GetStringValue());
                        if (!result.IsNullOrEmpty())
                        {
                            String[] separator = { "," };
                            String[] splitIds = result.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            extVendorId = splitIds[1];
                        }

                        //Update BkgOrderSvcLineItem Status to DisptachOnHold_WaitingForEDSData for background package that contains EDS service.
                        if (!extVendorId.IsNullOrEmpty())
                        {
                            //BackgroundProcessOrderManager.UpdateBkgOrderSvcLineItem(applicantOrderDataContract.TenantId, Convert.ToInt32(extVendorId), bkgOrderObj.BOR_ID, SvcLineItemDispatchStatus.DISPTACH_ON_HOLD_WAITING_FOR_EDS_DATA.GetStringValue(), userOrder.CreatedByID);
                            //BasePage.LogOrderFlowSteps("Default.aspx - STEP 3.1: Method 'BackgroundProcessOrderManager.UpdateBkgOrderSvcLineItem' executed successfully for OrderId:" + orderId);
                        }
                        //Update status PSLI_DispatchedExternalVendor from DisptachOnHold_WaitingForEDSData to Dispatched
                        if (_orderPaymentDetail.IsNotNull() && _orderPaymentDetail.OPD_OrderStatusID.IsNotNull() && _orderPaymentDetail.OPD_OrderStatusID == orderStatusId && !extVendorId.IsNullOrEmpty())
                        {
                            //Create dictionary for parallel task parameter.
                            Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                            dicParam.Add("BkgOrderId", bkgOrderObj.BOR_ID);
                            dicParam.Add("TenantId", applicantOrderDataContract.TenantId);
                            dicParam.Add("ExtVendorId", Convert.ToInt32(extVendorId));
                            dicParam.Add("BPHMId_List", applicantOrderDataContract.lstBackgroundPackages.Select(slct => slct.BPHMId).ToList());
                            dicParam.Add("RegistrationId", String.Empty);
                            dicParam.Add("CurrentLoggedInUserId", userOrder.CreatedByID);
                            dicParam.Add("OrganizationUserId", bkgOrderObj.OrganizationUserProfile.OrganizationUserID);
                            dicParam.Add("OrganizationUserProfileId", bkgOrderObj.BOR_OrganizationUserProfileID);
                            dicParam.Add("ApplicantName", string.Concat(bkgOrderObj.OrganizationUserProfile.FirstName, " ", bkgOrderObj.OrganizationUserProfile.LastName));
                            dicParam.Add("PrimaryEmailAddress", bkgOrderObj.OrganizationUserProfile.PrimaryEmailAddress);
                            //Pass selectedNodeId in place of HierarchyId [UAT-1067]
                            //dicParam.Add("HierarchyNodeId", bkgOrderObj.Order.HierarchyNodeID.Value);
                            dicParam.Add("HierarchyNodeId", bkgOrderObj.Order.SelectedNodeID.Value);
                            BackgroundProcessOrderManager.RunParallelTaskSaveCCFDataAndPDF(objClearstarCCf.SaveCCFDataAndPDF, dicParam, LoggerService, ExceptiomService, applicantOrderDataContract.TenantId);
                            //BasePage.LogOrderFlowSteps("Default.aspx - STEP 3.2: Parallel Task 'BackgroundProcessOrderManager.RunParallelTaskSaveCCFDataAndPDF' started for OrderId:" + orderId
                            //     + " and BkgOrderId: " + bkgOrderObj.BOR_ID);
                        }
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// Generate the Grouped pricing of the Packages, Grouped by lkpPaymentOption Code
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        /// <param name="lstPkgPaymentOptns"></param>
        /// <param name="compliancePkgPrice"></param>
        private static void GenerateGroupedAmount(ApplicantOrderCart applicantOrderCart, List<ApplicantOrderPaymentOptions> lstPkgPaymentOptns, Int32 tenantId, List<Entity.ClientEntity.lkpPaymentOption> _lstClientPaymentOptions)
        {
            applicantOrderCart.lstPaymentGrouping = new List<PkgPaymentGrouping>();

            var _distinctPOIds = lstPkgPaymentOptns.DistinctBy(x => x.poid).ToList();


            foreach (var poItem in _distinctPOIds)
            {
                var _lstPkgs = lstPkgPaymentOptns.Where(po => po.poid == poItem.poid).ToList();

                PkgPaymentGrouping _pkgPayGroup = new PkgPaymentGrouping();
                _pkgPayGroup.PaymentModeCode = _lstClientPaymentOptions.Where(po => po.PaymentOptionID == poItem.poid).FirstOrDefault().Code;
                _pkgPayGroup.PaymentModeId = poItem.poid;
                _pkgPayGroup.TotalAmount = CalculateGroupAmount(_pkgPayGroup, applicantOrderCart.lstApplicantOrder[0].lstPackages, _lstPkgs, applicantOrderCart.CompliancePackages.Values.ToList());
                applicantOrderCart.lstPaymentGrouping.Add(_pkgPayGroup);

            }

            //UAT-3268
            var _distinctAdditionalPOIds = lstPkgPaymentOptns.Where(cond => !cond.additionalPoid.IsNullOrEmpty()).ToList();
            if (!_distinctAdditionalPOIds.IsNullOrEmpty())
            {
                foreach (var additionalPoItem in _distinctAdditionalPOIds)
                {
                    if (!additionalPoItem.additionalPoid.IsNullOrEmpty() && Convert.ToInt32(additionalPoItem.additionalPoid) > AppConsts.NONE && additionalPoItem.isbkg == true)
                    {
                        PkgPaymentGrouping _additionalPayOption = new PkgPaymentGrouping();
                        _additionalPayOption.PaymentModeCode = _lstClientPaymentOptions.Where(po => po.PaymentOptionID == Convert.ToInt32(additionalPoItem.additionalPoid)).FirstOrDefault().Code + "-Additional";
                        _additionalPayOption.PaymentModeId = Convert.ToInt32(additionalPoItem.additionalPoid);
                        _additionalPayOption.TotalAmount = Convert.ToDecimal(applicantOrderCart.lstApplicantOrder[0].lstPackages.Where(cond => cond.BPAId == additionalPoItem.pkgid).Select(sel => sel.AdditionalPrice).FirstOrDefault());
                        _additionalPayOption.lstPackages = new Dictionary<String, Boolean>();
                        _additionalPayOption.lstPackages.Add(additionalPoItem.pkgid + "_" + Guid.NewGuid().ToString() + "_Additional", additionalPoItem.isbkg);
                        //_additionalPayOption.lstPackages = new Dictionary<string, bool>();
                        applicantOrderCart.lstPaymentGrouping.Add(_additionalPayOption);
                    }
                }
            }
        }

        /// <summary>
        /// Calculate the Grouped Amount for Different payment Options selected
        /// </summary>
        /// <param name="lstBkgPackages"></param>
        /// <param name="lstPkgPaymentOptns"></param>
        /// <returns></returns>
        private static Decimal CalculateGroupAmount(PkgPaymentGrouping _pkgPayGroup, List<BackgroundPackagesContract> lstBkgPackages, List<ApplicantOrderPaymentOptions> lstPkgPaymentOptns, List<OrderCartCompliancePackage> lstCompliancePackages)
        {
            Decimal _totalAmount = 0;
            _pkgPayGroup.lstPackages = new Dictionary<String, Boolean>();
            foreach (var pkg in lstPkgPaymentOptns)
            {
                if (pkg.isbkg)
                {
                    var _price = lstBkgPackages.Where(x => x.BPAId == pkg.pkgid).First().TotalBkgPackagePrice;
                    _totalAmount += _price.IsNull() ? AppConsts.NONE : _price;

                    // UAT-3850
                    //if (orderDataContract.IsBillingCodeAmountAvlbl)
                    //{
                    //    if (_totalAmount > orderDataContract.BillingCodeAmount)
                    //    {

                    //        if (_pkgPayGroup.PaymentModeCode.ToLower() == PaymentOptions.InvoiceWithOutApproval.GetStringValue().ToLower())
                    //            _totalAmount = orderDataContract.BillingCodeAmount;
                    //        else
                    //            _totalAmount = _totalAmount - orderDataContract.BillingCodeAmount;
                    //    }
                    //}
                    //end 
                }
                else
                {
                    var _price = lstCompliancePackages.Where(x => x.CompliancePackageID == pkg.pkgid).FirstOrDefault().GrandTotal;
                    _totalAmount += Convert.ToDecimal(_price.IsNull() ? AppConsts.NONE : _price);
                    //orderDataContract.CompliancePkgPaymentOptionId = pkg.poid; TBD
                }

                _pkgPayGroup.lstPackages.Add(pkg.pkgid + "_" + Guid.NewGuid().ToString(), pkg.isbkg);
            }
            return _totalAmount;
        }

        /// <summary>
        /// Gets the price of all the background packages selected
        /// </summary>
        /// <returns></returns>
        private static Decimal GetBackgroundPackagesPrice(ApplicantOrderCart applicantOrderCart)
        {
            Decimal _backgroundPackagesPrice = 0;

            if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                foreach (var bkgPackage in applicantOrderCart.lstApplicantOrder[0].lstPackages)
                {
                    _backgroundPackagesPrice += (bkgPackage.TotalBkgPackagePrice.IsNullOrEmpty() ? AppConsts.NONE : bkgPackage.TotalBkgPackagePrice);
                }
            }
            return _backgroundPackagesPrice;
        }
        /// <summary>
        /// Gets the HierarchyNodeID on basis of Order Package type(the packages selected)
        /// </summary>
        /// <param name="_tenantId"></param>
        /// <param name="applicantOrderCart"></param>
        /// <returns>HierarchyID</returns>
        private Int32 GetHierarchyNodeIDByPackageType(Int32 tenantId, ApplicantOrderCart applicantOrderCart)
        {
            Int32? dppID = null;
            Int32? bphmID = null;
            if ((!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.IsCompliancePackageSelected)
                || (applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.IsCompliancePackageSelected))
            {
                if (!applicantOrderCart.DPP_Id.IsNullOrEmpty())
                    dppID = applicantOrderCart.DPP_Id;
                return Presenter.GetHierarchyNodeID(dppID, bphmID);
            }
            else if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && !applicantOrderCart.IsCompliancePackageSelected)
            {
                if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].lstPackages.Count > 0)
                    bphmID = applicantOrderCart.lstApplicantOrder[0].lstPackages[0].BPHMId;
                return Presenter.GetHierarchyNodeID(dppID, bphmID);
            }
            return AppConsts.MINUS_ONE;
        }

        /// <summary>
        /// Generate the Data from the Pricing stored procedure XML
        /// </summary>
        /// <returns></returns>
        private static List<Package_PricingData> GenerateDataFromPricingXML(String _pricingDataXML)
        {
            XDocument doc = XDocument.Parse(_pricingDataXML);

            // GET <package> TAG'S INSIDE <Packages> TAG
            var _packages = doc.Root.Descendants("Packages")
                               .Descendants("Package")
                               .Select(element => element)
                               .ToList();

            List<Package_PricingData> _lstData = new List<Package_PricingData>();
            foreach (var pkg in _packages)
            {

                Int32 _packageId = Convert.ToInt32(pkg.Element("PackageId").Value);
                Package_PricingData _packagePricingData = new Package_PricingData();
                _packagePricingData.PackageId = _packageId;

                // To be removed
                _packagePricingData.TotalBkgPackagePrice = pkg.Element("TotalPrice").Value.IsNullOrEmpty() ? 0 : Convert.ToDecimal(pkg.Element("TotalPrice").Value);

                #region ADD DATA OF <OrderLineItem> TAG'S INSIDE <OrderLineItems> TAG

                var _orderLineItems = pkg.Descendants("OrderLineItems").Descendants("OrderLineItem")
                                         .Select(element => element)
                                         .ToList();

                _packagePricingData.lstOrderLineItems = new List<OrderLineItem_PricingData>();

                foreach (var _ordLineItem in _orderLineItems)
                {
                    OrderLineItem_PricingData _orderLineItem = new OrderLineItem_PricingData();
                    _orderLineItem.PackageSvcGrpID = Convert.ToInt32(_ordLineItem.Element("PackageSvcGrpID").Value);
                    _orderLineItem.PackageServiceId = Convert.ToInt32(_ordLineItem.Element("PackageServiceID").Value);
                    _orderLineItem.PackageServiceItemId = Convert.ToInt32(_ordLineItem.Element("PackageServiceItemID").Value);
                    _orderLineItem.Description = _ordLineItem.Element("Description").Value;

                    _orderLineItem.PackageOrderItemPriceId = String.IsNullOrEmpty(_ordLineItem.Element("PackageOrderItemPriceID").Value) ? AppConsts.NONE :
                                                             Convert.ToInt32(_ordLineItem.Element("PackageOrderItemPriceID").Value);

                    _orderLineItem.Price = String.IsNullOrEmpty(_ordLineItem.Element("Price").Value) ?
                                           AppConsts.NONE : Convert.ToDecimal(_ordLineItem.Element("Price").Value);

                    _orderLineItem.PriceDescription = _ordLineItem.Element("PriceDescription").Value;

                    #region ADD DATA OF <Fee> TAG'S INSIDE  <Fees> TAG

                    var _fees = _ordLineItem.Descendants("Fees").Descendants("Fee")
                                                 .Select(element => element)
                                                 .ToList();

                    _orderLineItem.lstFees = new List<Fee_PricingData>();
                    foreach (var _fee in _fees)
                    {
                        _orderLineItem.lstFees.Add(new Fee_PricingData
                        {
                            Amount = String.IsNullOrEmpty(_fee.Element("Amount").Value)
                                        ? AppConsts.NONE
                                        : Convert.ToDecimal(_fee.Element("Amount").Value),
                            Description = _fee.Element("Description").Value,

                            PackageOrderItemFeeId = String.IsNullOrEmpty(_fee.Element("PackageOrderItemFeeID").Value)
                                                       ? (Int32?)null
                                                       : Convert.ToInt32(_fee.Element("PackageOrderItemFeeID").Value),
                        });
                    }

                    #endregion

                    #region ADD DATA OF <BkgSvcAttributeDataGroup> TAG

                    var _bkgAttrDataGrps = _ordLineItem.Descendants("BkgSvcAttributeDataGroup")
                                                                   .Select(element => element)
                                                                   .ToList();

                    _orderLineItem.lstBkgSvcAttributeDataGroup = new List<BkgSvcAttributeDataGroup_PricingData>();
                    foreach (var _bkgAttrDataGrp in _bkgAttrDataGrps)
                    {
                        Int32 _instanceId = AppConsts.NONE;

                        if (!String.IsNullOrEmpty(_bkgAttrDataGrp.Element("InstanceID").Value))
                            _instanceId = Convert.ToInt32(_bkgAttrDataGrp.Element("InstanceID").Value);

                        BkgSvcAttributeDataGroup_PricingData _bkgSvcAttrDataGrpPricingData = new BkgSvcAttributeDataGroup_PricingData
                        {
                            AttributeGroupId = Convert.ToInt32(_bkgAttrDataGrp.Element("AttributeGroupID").Value),
                            InstanceId = _instanceId
                        };

                        //if (String.IsNullOrEmpty(_instanceId))
                        var _attributeData = _bkgAttrDataGrp.Descendants("BkgSvcAttributes").Descendants("BkgSvcAttributeData")
                                                      .Select(element => element)
                                                      .ToList();

                        _bkgSvcAttrDataGrpPricingData.lstAttributeData = new List<AttributeData_PricingData>();
                        foreach (var _attrData in _attributeData)
                        {
                            #region ADD DATA OF BkgSvcAttributeData TAG

                            String _attributeGrpMappingId = _attrData.Element("AttributeGroupMapingID").Value;

                            if (!String.IsNullOrEmpty(_attributeGrpMappingId))
                            {
                                _bkgSvcAttrDataGrpPricingData.lstAttributeData.Add(new AttributeData_PricingData
                                {
                                    AttributeGroupMappingID = Convert.ToInt32(_attributeGrpMappingId),
                                    AttributeValue = _attrData.Element("Value").Value
                                });
                            }

                            #endregion
                        }

                        _orderLineItem.lstBkgSvcAttributeDataGroup.Add(_bkgSvcAttrDataGrpPricingData);
                    }
                    #endregion

                    _packagePricingData.lstOrderLineItems.Add(_orderLineItem);
                }

                #endregion

                _lstData.Add(_packagePricingData);
            }
            return _lstData;
        }

        private void SubmitOrder(ApplicantOrderCart applicantOrderCart)
        {
            applicantOrderCart.AddOrderStageTrackID(OrderStages.OnlineConfirmation);
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ORDER_CONFIRMATION, applicantOrderCart);
            string strOrderID = _applicantOrderCart.AllOrderIDs;
            //CurrentViewContext.OrderPaymentDetaildId = _applicantOrderCart.OrderPaymentdetailId;

            foreach (var applicantOrder in _applicantOrderCart.lstApplicantOrder)
            {
                CurrentViewContext.DPPSIds = new List<int>();
                // Temporary RAJEEV TBD
                if (_applicantOrderCart.CompliancePackages.IsNotNull() && _applicantOrderCart.CompliancePackages.Count > AppConsts.NONE)
                {
                    CurrentViewContext.DPPSIds = _applicantOrderCart.CompliancePackages.Values.Where(cp => cp.DPPS_ID > AppConsts.NONE).Select(cp => cp.DPPS_ID).ToList();
                }
                if (!(CurrentViewContext.DPPSIds.IsNotNull() && CurrentViewContext.DPPSIds.Count > AppConsts.NONE) && applicantOrder.DPPS_Id.IsNotNull())
                {
                    var dppsId = applicantOrder.DPPS_Id.FirstOrDefault();
                    if (dppsId > AppConsts.NONE)
                    {
                        CurrentViewContext.DPPSIds.Add(dppsId);
                    }
                }


                if (strOrderID.IsNullOrEmpty())
                    strOrderID = Convert.ToString(applicantOrder.OrderId);

                string strOrderNumber = _applicantOrderCart.AllOrderNumbers;
                if (strOrderNumber.IsNullOrEmpty())
                    strOrderNumber = Convert.ToString(applicantOrder.OrderNumber);

                //lblOrderId.Text = strOrderID;
                //lblOrderNumber.Text = strOrderNumber;
                //UAT-1059:Remove I.P. address and mask social security number from order summary, commented below code
                //lblIPAddress.Text = applicantOrder.ClientMachineIP;

                break;
            }

            // BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 1: Page_Load for order: " + lblOrderId.Text);

            #region UAT-1648
            List<Int32> RecentAddedOPDs = _applicantOrderCart.RecentAddedOPDs.IsNullOrEmpty() ? new List<Int32>() : _applicantOrderCart.RecentAddedOPDs;
            String OrderRequestType = _applicantOrderCart.OrderRequestType;
            #endregion
            if (Presenter.IsOrderPaymentDone(strOrderID, _applicantOrderCart.OrderRequestType, RecentAddedOPDs))
            {
                //UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.
                var paymentOptionCreditCard = CurrentViewContext.lstOPDs.Where(opd => opd.OPD_PaymentOptionID.IsNotNull()
                                               && opd.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue()).ToList();
                String pendingSchoolApprovalStatusCode = ApplicantOrderStatus.Pending_School_Approval.GetStringValue();

                if (!paymentOptionCreditCard.IsNullOrEmpty() && paymentOptionCreditCard.Any(x => x.lkpOrderStatu.Code == pendingSchoolApprovalStatusCode))
                {
                    //    BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 1.1: Order status is Pending School Approval. Order Payment is not completed for order: " + lblOrderId.Text);
                    //    String _message = "Your order has been placed successfully. Before charging your credit card, your institution will need to approve your order.";
                    //    lblMessage.Text = _message;
                    //    lblMessage.Visible = true;
                }
                else
                if (_applicantOrderCart.OrderRequestType == INTSOF.Utils.OrderRequestType.ChangeSubscriptionByAdmin.GetStringValue()
                   && (CurrentViewContext.AppChangeSubPaymentTypeCode == PaymentOptions.InvoiceWithApproval.GetStringValue()
                  || CurrentViewContext.AppChangeSubPaymentTypeCode == PaymentOptions.Money_Order.GetStringValue())
                  )
                {
                    //    base.ShowSuccessMessage("Your new subscription will become active when your balance payment gets approved.");
                }
                //else if Balance Payment for Credit card and Paypal transactions
                else if (_applicantOrderCart.OrderRequestType == INTSOF.Utils.OrderRequestType.ChangeSubscriptionByAdmin.GetStringValue())
                {
                    //    base.ShowSuccessMessage("Thanks for paying the balance amount.");
                }
                else if (_applicantOrderCart.lstApplicantOrder[0].LstOrderStageTrackID.Contains(OrderStages.OrderPaymentDetails) &&
                          applicantOrderCart.ChangePaymentTypeCode.IsNotNull() &&
                        (
                             _applicantOrderCart.ChangePaymentTypeCode.ToLower() == PaymentOptions.InvoiceWithApproval.GetStringValue().ToLower()
                          || _applicantOrderCart.ChangePaymentTypeCode.ToLower() == PaymentOptions.Money_Order.GetStringValue().ToLower()
                        )
                   )
                {
                    //    base.ShowSuccessMessage("Your new subscription will become active when your balance payment gets approved.");
                }
                else
                {
                    //    //base.ShowSuccessMessage("Your order has been successfully placed.");
                    //    base.ShowSuccessMessage(Resources.Language.ORDERSUCCESSPLACED);
                }
                // BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 1.1:  Order Payment DONE for order: " + lblOrderId.Text);
                
            }
            else
            {
                //    BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 1.1:  Order Payment NOT DONE for order: " + lblOrderId.Text);
                //    base.ShowInfoMessage("Your payment is not completed for this Order.");
            }
        }
       


        #endregion

        //protected void btnEditAppointment_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        _applicantOrderCart = GetApplicantOrderCart();
        //        _applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = AppConsts.TWO; //// UAT - 4331
        //        _applicantOrderCart.AddOrderStageTrackID(OrderStages.CustomFormsCompleted);
        //        Dictionary<String, String> queryString = new Dictionary<String, String>();
        //        queryString = new Dictionary<String, String>
        //            {
        //              {AppConsts.CHILD , ChildControls.APPLICANT_APPOINTMENT_SCHEDULE},
        //              { "TenantId", CurrentViewContext.TenantId.ToString()}
        //            };
        //        String url = (String.Format("~/FingerPrintSetUp/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        //        Response.Redirect(url);
        //    }
        //    catch (SysXException ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        base.LogError(ex);
        //        base.ShowErrorMessage(ex.Message);
        //    }
        //} 
    }
}


