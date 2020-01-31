#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Linq;
using System.Collections.Generic;


#endregion

#region UserDefined

using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Entity.ClientEntity;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.BkgOperations.Views;
using INTSOF.UI.Contract.BkgOperations;
using System.Data;
using System.Web.UI.WebControls;
using Business.RepoManagers;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.Configuration;
using INTSOF.UI.Contract.FingerPrintSetup;
using System.Configuration;
using INTSOF.UI.Contract.Globalization;
using System.Globalization;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class OrderConfirmation : BaseUserControl, IOrderConfirmationView
    {
        #region Variables

        #region Private Variables

        private OrderConfirmationPresenter _presenter = new OrderConfirmationPresenter();
        private OrganizationUserProfile _orgUserProfile;
        private Boolean _isRenewalOrder;
        OrganizationUserProfile _organizationUserProfile;
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

        Boolean IOrderConfirmationView.IsSSNDisabled
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
        public Boolean IsLocationServiceTenant
        {
            get
            {
                if (ViewState["IsLocationServiceTenant"] != null)
                    return Convert.ToBoolean(ViewState["IsLocationServiceTenant"]);
                return false;
            }
            set
            {
                ViewState["IsLocationServiceTenant"] = value;
            }
        }

        //UAT-781
        String IOrderConfirmationView.DecryptedSSN { get; set; }

        #region UAT-2212: Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality

        private String NoMiddleNameText
        {
            get
            {
                String noMiddleNameText = String.Empty;
                if (!CurrentViewContext.IsLocationServiceTenant)
                    noMiddleNameText = WebConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY];
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

        public Entity.ZipCode ApplicantZipCodeDetails
        {
            get;
            set;
        }

        public OrderConfirmationPresenter Presenter
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

        public Int32 TenantId
        {
            get
            {
                if (ViewState["TenantId"] == null)
                {
                    //Get User from Session
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    ViewState["TenantId"] = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    return Convert.ToInt32(ViewState["TenantId"]);
                }
                else
                {
                    return Convert.ToInt32(ViewState["TenantId"]);
                }
            }
        }

        public Int32 ZipCodeId
        {
            get;
            set;
        }       

        public IOrderConfirmationView CurrentViewContext
        {
            get { return this; }
        }

        public AppointmentOrderScheduleContract AppointmentDetailContract
        {
            get
            {
                if (!ViewState["AppointmentDetailContract"].IsNullOrEmpty())
                {
                    return ViewState["AppointmentDetailContract"] as AppointmentOrderScheduleContract;
                }
                return new AppointmentOrderScheduleContract();
            }
            set
            {
                ViewState["AppointmentDetailContract"] = value;
            }
        }

        #region Custum form properties
        public List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }

        public List<Int32> DPPSIds
        {
            get;
            set;
        }

        public List<BackgroundOrderData> lstBackgroundOrderData
        {
            get
            {
                applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                return applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData;
            }
        }

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
        #endregion

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

        //public DeptProgramPackageSubscription SelectedPackageDetails
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Get and set next page path.
        /// </summary>
        public String NextPagePath
        {
            get;
            set;
        }

        public String InstitutionHierarchy
        {
            get;
            set;
        }

        public List<OrderCartCompliancePackage> CompliancePackages { get; set; }

        /// <summary>
        /// Data of the Order being shown
        /// </summary>
        public List<Order> OrderData
        {
            get;
            set;
        }

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

        public DataTable lstExternalPackages
        {
            get;
            set;
        }

        //UAT 1438
        public List<UserGroup> selectedUserGrpList
        {
            get;
            set;
        }

        //public Boolean IfInvoiceOnlyPymntOptn
        //{
        //    get
        //    {
        //        if (!String.IsNullOrEmpty(Convert.ToString(ViewState["IfInvoiceOnlyPymnOptn"])))
        //            return (Boolean)ViewState["IfInvoiceOnlyPymnOptn"];
        //        return false;
        //    }
        //    set
        //    {
        //        ViewState["IfInvoiceOnlyPymnOptn"] = value;
        //    }
        //}

        Boolean IOrderConfirmationView.IsOrderStatusPaid
        {
            get;
            set;
        }

        /// <summary>
        /// List of Instructions to bind 
        /// </summary>
        public List<Tuple<String, String>> lstClientPaymentOptns
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<OrderPaymentDetail> lstOPDs { get; set; }

        /// <summary>
        /// Store the OPD-ID, in case single package is being used
        /// </summary>
        Int32 IOrderConfirmationView.OrderPaymentDetaildId { get; set; }

        //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
        public Int32 OrgUsrID
        {
            get
            {
                if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                    return Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
                else
                    return CurrentLoggedInUserId;
            }
        }

        #region UAT-1648
        /// <summary>
        /// Property identify the OPDs those are recently added in Applicant Completing Order Process for "SentForOnlinePayment".
        /// </summary>
        List<Int32> IOrderConfirmationView.RecentAddedOPDs { get; set; }

        String IOrderConfirmationView.OrderRequestType { get; set; }
        #endregion

        public List<OrderPaymentDetail> lstAdditionalPaymentModes
        {
            get
            {
                if (!ViewState["lstAdditionalPaymentModes"].IsNullOrEmpty())
                {
                    return (ViewState["lstAdditionalPaymentModes"]) as List<OrderPaymentDetail>;
                }
                return new List<OrderPaymentDetail>();
            }
            set
            {
                ViewState["lstAdditionalPaymentModes"] = value;
            }
        }

        Boolean IsCreateOrderSummaryHTMLFileOnRender
        {
            get;
            set;
        }

        List<Entity.lkpSuffix> IOrderConfirmationView.lstSuffixes
        {
            get
            {
                if (!ViewState["lstSuffixes"].IsNullOrEmpty())
                    return (List<Entity.lkpSuffix>)ViewState["lstSuffixes"];
                return new List<Entity.lkpSuffix>();
            }
            set
            {
                ViewState["lstSuffixes"] = value;
            }
        }

        String IOrderConfirmationView.LanguageCode
        {
            get
            {
                LanguageContract languageContract = LanguageTranslateUtils.GetCurrentLanguageFromSession();
                if (!languageContract.IsNullOrEmpty())
                    return languageContract.LanguageCode;
                return Languages.ENGLISH.GetStringValue();
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
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
            if (!this.IsPostBack)
            {

                Presenter.IsLocationServiceTenant();
                Dictionary<String, String> queryString = new Dictionary<string, string>();
                queryString.ToDecryptedQueryString(Request.QueryString["args"]);

                //queryString.Count() will be 2 in case of the source was the Normal order creation. For online payment it will be 1
                if (queryString.Count() > 1 && !queryString["error"].IsNullOrEmpty())
                {
                    base.ShowErrorMessage("An error occured while placing the order.");
                    BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 1: Error : " + queryString["error"] + " occured, while Placing the order by OrgUserId: " + CurrentViewContext.OrgUsrID);
                    pnl.Visible = false;
                }
                else
                {
                    try
                    {
                        pnl.Visible = true;

                        ApplicantOrderCart _applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;
                        SetApplicationOrderCart(_applicantOrderCart);                        

                        CurrentViewContext.OrderPaymentDetaildId = _applicantOrderCart.OrderPaymentdetailId;

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

                            string strOrderID = _applicantOrderCart.AllOrderIDs;
                            if (strOrderID.IsNullOrEmpty())
                                strOrderID = Convert.ToString(applicantOrder.OrderId);

                            string strOrderNumber = _applicantOrderCart.AllOrderNumbers;
                            if (strOrderNumber.IsNullOrEmpty())
                                strOrderNumber = Convert.ToString(applicantOrder.OrderNumber);

                            lblOrderId.Text = strOrderID;
                            lblOrderNumber.Text = strOrderNumber;
                            //UAT-1059:Remove I.P. address and mask social security number from order summary, commented below code
                            //lblIPAddress.Text = applicantOrder.ClientMachineIP;

                            break;
                        }

                        //To show or hide Mailing address section on condition basis.
                        if (_applicantOrderCart.IsLocationServiceTenant && !_applicantOrderCart.MailingAddress.IsNullOrEmpty() && !_applicantOrderCart.MailingAddress.MailingOptionPrice.IsNullOrEmpty()
                            && (!_applicantOrderCart.FingerPrintData.IsEventCode && !_applicantOrderCart.FingerPrintData.IsOutOfState))
                        {
                            {
                                dvMailingAddress.Visible = true;
                                dvMailingState.Visible = !string.IsNullOrWhiteSpace(_applicantOrderCart.MailingAddress.StateName);
                            }
                        }

                        BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 1: Page_Load for order: " + lblOrderId.Text);

                        #region UAT-1648
                        CurrentViewContext.RecentAddedOPDs = _applicantOrderCart.RecentAddedOPDs.IsNullOrEmpty() ? new List<Int32>() : _applicantOrderCart.RecentAddedOPDs;
                        CurrentViewContext.OrderRequestType = _applicantOrderCart.OrderRequestType;
                        #endregion
                        if (Presenter.IsOrderPaymentDone(lblOrderId.Text))
                        {
                            //UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.
                            var paymentOptionCreditCard = CurrentViewContext.lstOPDs.Where(opd => opd.OPD_PaymentOptionID.IsNotNull()
                                                           && opd.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue()).ToList();
                            String pendingSchoolApprovalStatusCode = ApplicantOrderStatus.Pending_School_Approval.GetStringValue();

                            if (!paymentOptionCreditCard.IsNullOrEmpty() && paymentOptionCreditCard.Any(x => x.lkpOrderStatu.Code == pendingSchoolApprovalStatusCode))
                            {
                                BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 1.1: Order status is Pending School Approval. Order Payment is not completed for order: " + lblOrderId.Text);
                                String _message = "Your order has been placed successfully. Before charging your credit card, your institution will need to approve your order.";
                                lblMessage.Visible = true;
                                //UAT-4537
                                if (!_applicantOrderCart.ApprovalPendingPackageName.IsNullOrEmpty())
                                    _message = "Your Order has been placed successfully. You have been charged with " + _applicantOrderCart.ApprovalPendingPackageName + " package(s) after your institution approval.";
                                lblMessage.Text = _message.HtmlEncode();
                            }
                            else if (_applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscriptionByAdmin.GetStringValue()
                               && (CurrentViewContext.AppChangeSubPaymentTypeCode == PaymentOptions.InvoiceWithApproval.GetStringValue()
                              || CurrentViewContext.AppChangeSubPaymentTypeCode == PaymentOptions.Money_Order.GetStringValue())
                              )
                            {
                                base.ShowSuccessMessage("Your new subscription will become active when your balance payment gets approved.");
                            }
                            //else if Balance Payment for Credit card and Paypal transactions
                            else if (_applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscriptionByAdmin.GetStringValue())
                            {
                                base.ShowSuccessMessage("Thanks for paying the balance amount.");
                            }
                            else if (_applicantOrderCart.lstApplicantOrder[0].LstOrderStageTrackID.Contains(OrderStages.OrderPaymentDetails) &&
                                      applicantOrderCart.ChangePaymentTypeCode.IsNotNull() &&
                                    (
                                         _applicantOrderCart.ChangePaymentTypeCode.ToLower() == PaymentOptions.InvoiceWithApproval.GetStringValue().ToLower()
                                      || _applicantOrderCart.ChangePaymentTypeCode.ToLower() == PaymentOptions.Money_Order.GetStringValue().ToLower()
                                    )
                                )
                            {
                                base.ShowSuccessMessage("Your new subscription will become active when your balance payment gets approved.");
                            }
                            else
                            {
                                //base.ShowSuccessMessage("Your order has been successfully placed.");
                                base.ShowSuccessMessage(Resources.Language.ORDERSUCCESSPLACED);
                            }
                            BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 1.1:  Order Payment DONE for order: " + lblOrderId.Text);

                            // UAT 3521
                            
                            if (_applicantOrderCart.IsLocationServiceTenant && !lblOrderId.Text.IsNullOrEmpty() && _applicantOrderCart.ChangePaymentTypeCode.IsNullOrWhiteSpace())
                            {
                                String BillingCode = string.Empty;
                                String CbiUniqueId = string.Empty;
                                if (!_applicantOrderCart.FingerPrintData.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.CBIUniqueID.IsNullOrEmpty() && _applicantOrderCart.FingerPrintData.BillingCode.IsNullOrEmpty())
                                {
                                    if (CurrentViewContext.lstOPDs.Any(x => x.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()))
                                    {
                                        BillingCode = _applicantOrderCart.FingerPrintData.BillingCode;
                                        CbiUniqueId = _applicantOrderCart.FingerPrintData.CBIUniqueID;
                                    }
                                }
                                Boolean isCompleteYourOrderClick = _applicantOrderCart.OrderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue() ? true : false;
                                ReserveSlotContract reserveSlotContract = Presenter.SubmitApplicantAppointment(Convert.ToInt32(lblOrderId.Text), _applicantOrderCart, BillingCode, CbiUniqueId, isCompleteYourOrderClick);//FingerPrintDataManager.SubmitApplicantAppointment(Convert.ToInt32(lblOrderId.Text), CurrentViewContext.OrgUsrID, _applicantOrderCart.FingerPrintData, CurrentViewContext.TenantId);


                                //if (_applicantOrderCart.IsChangePayment)
                                //{
                                //    // Code to Save Order Payment Invoice Data
                                //    // UAT-5031 : Start
                                //    bool result1 = Presenter.SaveOrderPaymentInvoice(CurrentViewContext.TenantId, _applicantOrderCart.OrderId, CurrentViewContext.CurrentLoggedInUserId, _applicantOrderCart.IsModifyShipping);
                                //    // UAT-5031 : End
                                //}

                                if (!reserveSlotContract.IsNullOrEmpty() && reserveSlotContract.ApplicantAppointmentID > AppConsts.NONE)
                                {
                                    if (!String.IsNullOrEmpty(reserveSlotContract.ErrorMsg))
                                    {
                                        base.ShowErrorInfoMessage(reserveSlotContract.ErrorMsg);
                                    }
                                    else
                                    {
                                        // Code to Save Order Payment Invoice Data
                                        // UAT-5031 : Start
                                        bool result = Presenter.SaveOrderPaymentInvoice(CurrentViewContext.TenantId , _applicantOrderCart.OrderId, CurrentViewContext.CurrentLoggedInUserId, _applicantOrderCart.IsModifyShipping);
                                        // UAT-5031 : End

                                        Presenter.GetAppointmentOrderDetailData(reserveSlotContract.ApplicantAppointmentID);
                                        AppointmentSlotContract AppSlotContract = new AppointmentSlotContract();
                                        if (_applicantOrderCart.FingerPrintData.SlotID > AppConsts.ONE)
                                        {
                                            AppSlotContract.SlotDate = _applicantOrderCart.FingerPrintData.StartTime.Date;
                                            AppSlotContract.SlotStartTime = _applicantOrderCart.FingerPrintData.StartTime.ToString("HH:mm");
                                            AppSlotContract.SlotEndTime = _applicantOrderCart.FingerPrintData.EndTime.ToString("HH:mm");
                                        }
                                        AppSlotContract.ApplicantOrgUserId = CurrentViewContext.CurrentLoggedInUserId;
                                        AppSlotContract.IsEventType = _applicantOrderCart.FingerPrintData.IsEventCode;
                                        AppSlotContract.EventName = _applicantOrderCart.FingerPrintData.EventName;
                                        AppSlotContract.EventDescription = _applicantOrderCart.FingerPrintData.EventDescription;
                                        AppSlotContract.IsOutOfStateAppointment = _applicantOrderCart.FingerPrintData.IsOutOfState;
                                        Presenter.SendOrderCreateMail(AppSlotContract);
                                    }
                                }
                            }

                            if (_applicantOrderCart.IsLocationServiceTenant && !_applicantOrderCart.ChangePaymentTypeCode.IsNullOrWhiteSpace())
                            {
                                bool result = Presenter.SaveOrderPaymentInvoice(CurrentViewContext.TenantId, _applicantOrderCart.OrderId, CurrentViewContext.CurrentLoggedInUserId, _applicantOrderCart.IsModifyShipping);
                            }

                        }
                        else
                        {
                            BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 1.1:  Order Payment NOT DONE for order: " + lblOrderId.Text);
                            base.ShowInfoMessage("Your payment is not completed for this Order.");
                        }

                        //CBI|| CABS || To Get Suffix List
                        AddSuffix();
                        //
                        BindInstructions();
                        BindAdditionalPaymentModes(); //UAT-3268
                        BindPaymentModes(_applicantOrderCart);
                        BindConfirmationData(_applicantOrderCart);
                        BindPackageData(_applicantOrderCart.IsLocationServiceTenant);
                        BindOtherDetails(_applicantOrderCart);
                        //UAT 1438
                        //BindOtherDetailsUserGroup(_applicantOrderCart);

                        //hide disclaimer if compliance package is not selected.
                        if (_applicantOrderCart.IsCompliancePackageSelected)
                        {
                            dvDisclaimer.Style.Add("display", "block");
                            if (divPaymentInstruction.Visible)
                            {
                                divHrDisc.Style.Add("display", "block");
                            }
                        }
                        if (IsAnyInvoiceTypePkg(applicantOrderCart))
                        {
                            divRushOrder.Style.Add("display", "none");
                            dvTotalPrice.Style.Add("display", "none");
                        }

                        //String _currentStep = " (Step " + (_applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep) +
                        //            " of " + _applicantOrderCart.GetTotalOrderSteps() + ")";

                        String _currentStep = " (" + Resources.Language.STEP + " " + (_applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep) + " " +
                              Resources.Language.OF + " " + _applicantOrderCart.GetTotalOrderSteps() + ")";


                        base.SetPageTitle(_currentStep);

                        if (!_applicantOrderCart.IsNullOrEmpty() &&
                             (_applicantOrderCart.OrderRequestType == OrderRequestType.NewOrder.GetStringValue()
                             || _applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscription.GetStringValue()))
                            (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle(Resources.Language.CREATODR);
                        //UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
                        else if (!_applicantOrderCart.IsNullOrEmpty() &&
                                  _applicantOrderCart.OrderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue())
                        {
                            (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle(Resources.Language.CMPLTORDER);
                            base.SetPageTitle(String.Empty);
                        }
                        else
                            (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle("Renewal Order");

                        // Confirm
                        CopyBkgDataToCompliancePackage(_applicantOrderCart);

                        //UAT-2970
                        BindCreditCardUserAgreement();
                    }
                    catch (Exception ex)
                    {
                        pnl.Visible = false;
                        base.ShowErrorMessage("An error occured while loading the order details.");
                        BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 1: Error in loading the order details, order placed by: " + CurrentViewContext.OrgUsrID + ". Error is: " + Convert.ToString(ex) + " ");
                    }
                }

                Presenter.GetSSNSetting();
            }
            hdnTenantID.Value = Convert.ToString(CurrentViewContext.TenantId);

            CreateCustomForm();
            if (!lstPackages.IsNullOrEmpty() && lstPackages.Count > 0)
            {
                residentialHistory.Visible = true;
                BindResidentialHistory();
            }
            Presenter.OnViewLoaded();
            //cbbuttons.SubmitButton.ToolTip = "Click to return to the dashboard";
            cbbuttons.SubmitButton.ToolTip = Resources.Language.CLKRETTODASHBOARD;
            //Commented SSN: UAT-1059:Remove I.P. address and mask social security number from order summary
            divSSN.Visible = !(CurrentViewContext.IsSSNDisabled);
            //UAT-3784
            Guid Id = Guid.NewGuid();
            hdnFileIdentifier.Value = Id.ToString();

            IsCreateOrderSummaryHTMLFileOnRender = ConfigurationManager.AppSettings["IsCreateOrderSummaryHTMLFileOnRender"] == null ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["IsCreateOrderSummaryHTMLFileOnRender"].ToLower());
            hdnIsCreateHTMLFileOnRender.Value = IsCreateOrderSummaryHTMLFileOnRender.ToString();
            ManageSSN();
        }

        #region UAT-3784

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            System.IO.StringWriter output = new System.IO.StringWriter();
            base.Render(new System.Web.UI.HtmlTextWriter(output));

            if (IsCreateOrderSummaryHTMLFileOnRender)
            {
                String htmlContent = output.ToString();
                htmlContent = htmlContent.Replace("submit", "hidden");
                ComplianceOperationsDefault.CreateHtmlFileOnRender(htmlContent, lblOrderId.Text, lblOrderNumber.Text, hdnFileIdentifier.Value);
            }
            writer.Write(output.ToString());

        }

        #endregion
        protected void rptBackgroundPackages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                var _bopId = (e.Item.FindControl("hdfBOPId") as HiddenField).Value;
                if (!ShowPkgPrice(true, Convert.ToInt32(_bopId)))
                {
                    System.Web.UI.HtmlControls.HtmlGenericControl dvBkgPackagePrice = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("dvBkgPackagePrice");
                    dvBkgPackagePrice.Style.Add("display", "none");
                }

                System.Web.UI.HtmlControls.HtmlGenericControl divBPT = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("divBPT");
                var _lblPaymentType = e.Item.FindControl("lblPaymentType") as Label;
                var _bpaId = Convert.ToInt32((e.Item.FindControl("hdfBPAId") as HiddenField).Value);
                var _pkgs = applicantOrderCart.lstApplicantOrder[0].lstPackages;

                //var _bkgPkgPrice = _pkgs.Where(bp => bp.BPAId == _bpaId).First().TotalBkgPackagePrice;

                var _bkgPkgPrice = decimal.Round(Convert.ToDecimal((e.Item.FindControl("hdnfPrice") as HiddenField).Value));
                if (!CurrentViewContext.IsLocationServiceTenant && _bkgPkgPrice.IsNotNull() && _bkgPkgPrice > 0) // Added addition location service tenant check in UAT-3850
                {
                    var _payType = GetPaymentType(true, Convert.ToInt32(_bopId));
                    _lblPaymentType.Text = _payType;
                    divBPT.Visible = true;
                }
                else
                    divBPT.Visible = false;

                var divPackagePrice = (e.Item.FindControl("dvBkgPackagePrice") as HtmlGenericControl);
                if (!divPackagePrice.IsNullOrEmpty())
                    divPackagePrice.Visible = CurrentViewContext.IsLocationServiceTenant ? false : true;
                var spnBkgPckg = (e.Item.FindControl("spnBkgPackage") as HtmlGenericControl);
                if (!spnBkgPckg.IsNullOrEmpty())
                    //[bhupender_22Oct2018]:Below seems to be incorrect as Order Selection is configurable text. However, I am also using the same patter due to lack of time in analysis.
                    //spnBkgPckg.InnerText = CurrentViewContext.IsLocationServiceTenant ? "Order Selection" : "Background Package"; 
                    spnBkgPckg.InnerText = CurrentViewContext.IsLocationServiceTenant ? Resources.Language.ORDERSELECTION : "Background Package";
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

        #region Grid Events

        /// <summary>
        /// Set the Payment Mode Type text based on the Code 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptPaymentModes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var _hdfPaymentModeCode = e.Item.FindControl("hdfPaymentType") as HiddenField;
            var _priceDiv = e.Item.FindControl("divPrice") as HtmlGenericControl;
            var spnAmt = e.Item.FindControl("spnAmt") as HtmlGenericControl;

            if (_hdfPaymentModeCode.IsNotNull() && _priceDiv.IsNotNull())
            {
                if ((!CurrentViewContext.IsLocationServiceTenant && _hdfPaymentModeCode.Value == PaymentOptions.InvoiceWithApproval.GetStringValue()) ||
                    _hdfPaymentModeCode.Value == PaymentOptions.InvoiceWithOutApproval.GetStringValue())
                {
                    _priceDiv.Visible = false;
                }

                //UAT-3850
                if (!spnAmt.IsNullOrEmpty())
                {
                    spnAmt.InnerText = Resources.Language.AMOUNT + ": ";
                    ApplicantOrderCart _applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;
                    if (CurrentViewContext.IsLocationServiceTenant && !_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.BillingCode.IsNullOrEmpty()
                    && !_applicantOrderCart.FingerPrintData.BillingCodeAmount.IsNullOrEmpty() && _applicantOrderCart.FingerPrintData.BillingCodeAmount > AppConsts.NONE)
                    {
                        _priceDiv.Visible = true;
                        if (_hdfPaymentModeCode.Value == PaymentOptions.InvoiceWithOutApproval.GetStringValue())
                            spnAmt.InnerText = Resources.Language.PAIDBYINST + ": ";
                        else
                            spnAmt.InnerText = Resources.Language.BALANCEAMT + ": ";
                    }
                }

            }
        }


        #endregion

        #region Button Events

        protected void CmdBarSubmit_Click(object sender, EventArgs e)
        {
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
            String url = String.Format(AppConsts.APPLICANT_MAIN_PAGE_NAME);

            BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 5: 'Finish' clicked, Redirecitng to dashboard wih url: " + url + ", for OrderId(s): " + lblOrderId.Text);
            Response.Redirect(url);
        }
        #endregion

        #region DropDown Events



        #endregion

        #endregion

        #region Methods

        #region Public Methods


        #endregion

        #region Private Methods

        /// <summary>
        /// To bind organization user profile data
        /// </summary>
        /// <param name="_applicantOrderCart"></param>
        void BindConfirmationData(ApplicantOrderCart _applicantOrderCart)
        {
            _orgUserProfile = new OrganizationUserProfile();
            //ApplicantOrderCart _applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;

            foreach (var applicantOrder in _applicantOrderCart.lstApplicantOrder)
            {
                _orgUserProfile = applicantOrder.OrganizationUserProfile;
                // Temporary
                CurrentViewContext.DPPSIds = new List<int>();
                // Temporary
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
                    //CurrentViewContext.DPPSIds.Add(applicantOrder.DPPS_Id.FirstOrDefault());
                }
                break;
            }

            if (_orgUserProfile.IsNotNull())
            {

                #region UAT-781 ENCRYPTED SSN
                Presenter.GetDecryptedSSN(_orgUserProfile.OrganizationUserProfileID, true);
                #endregion

                //Show Personal Information
                CurrentViewContext.GenderId = Convert.ToInt32(_orgUserProfile.Gender);
                Presenter.GetGender();

                lblFirstName.Text = (_orgUserProfile.FirstName).HtmlEncode();
                lblLastName.Text = _orgUserProfile.LastName.HtmlEncode();
                //lblMiddleName.Text = _orgUserProfile.MiddleName;
                //UAT-2212: Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                lblMiddleName.Text = (_orgUserProfile.MiddleName.IsNullOrEmpty() ? NoMiddleNameText : _orgUserProfile.MiddleName).HtmlEncode();
                //CBI || CABS
                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    if (!CurrentViewContext.lstSuffixes.IsNullOrEmpty())
                    {
                        if (!CurrentViewContext.lstSuffixes.Where(cond => cond.SuffixID == _orgUserProfile.UserTypeID).Select(x => x.Suffix).FirstOrDefault().IsNullOrEmpty())
                            lblLastName.Text = (_orgUserProfile.UserTypeID.IsNullOrEmpty() ? _orgUserProfile.LastName : _orgUserProfile.LastName + " - " + CurrentViewContext.lstSuffixes.Where(cond => cond.SuffixID == _orgUserProfile.UserTypeID).FirstOrDefault().Suffix).HtmlEncode();
                        else
                            lblLastName.Text = _orgUserProfile.LastName.HtmlEncode();
                    }
                }

                if (_orgUserProfile.DOB.HasValue)
                {
                    lblDateOfBirth.Text = Presenter.GetMaskDOB(_orgUserProfile.DOB.Value.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue())));
                }
                lblGender.Text = CurrentViewContext.Gender;
                //Commented SSN: UAT-1059:Remove I.P. address and mask social security number from order summary
                //lblSSN.Text = Presenter.GetMaskedSSN(_orgUserProfile.SSN); //UAT-781
                lblSSN.Text = Presenter.GetMaskedSSN(CurrentViewContext.DecryptedSSN); //UAT-781
                lblEmail.Text = _orgUserProfile.PrimaryEmailAddress.HtmlEncode();

                //Assigment of mailing address detail.
                if (CurrentViewContext.IsLocationServiceTenant && !_applicantOrderCart.MailingAddress.IsNullOrEmpty() && !_applicantOrderCart.MailingAddress.MailingOptionPrice.IsNullOrWhiteSpace())
                {
                    lblMailingOption.Text = _applicantOrderCart.MailingAddress.MailingOptionPrice;
                    lblMailingAddress.Text = _applicantOrderCart.MailingAddress.Address1.HtmlEncode();
                    lblMailingCity.Text = _applicantOrderCart.MailingAddress.CityName.HtmlEncode();
                    lblMailingCountry.Text = _applicantOrderCart.MailingAddress.Country;
                    lblMailingState.Text = _applicantOrderCart.MailingAddress.StateName.HtmlEncode();
                    lblMailingZipCode.Text = _applicantOrderCart.MailingAddress.Zipcode.HtmlEncode();
                    if (_applicantOrderCart.MailingAddress.StateName.IsNullOrEmpty())
                     {
                       MailingLblNameZIPOPostalCode.Text = Resources.Language.POSTALCODE;
                     }
                    else
                     {
                       MailingLblNameZIPOPostalCode.Text = Resources.Language.ZIP;
                     }
                }

                //UAT-2447
                if (_orgUserProfile.IsInternationalPhoneNumber)
                {
                    lblPhone.Text = _orgUserProfile.PhoneNumber.HtmlEncode();
                }
                else
                {
                    lblPhone.Text = Presenter.GetFormattedPhoneNumber(_orgUserProfile.PhoneNumber);
                }

                //Show Residing From/To - Temporary
                if (_applicantOrderCart.lstPrevAddresses.IsNotNull() && _applicantOrderCart.lstPrevAddresses.Count > 0)
                {
                    PreviousAddressContract resHisoryProfile = _applicantOrderCart.lstPrevAddresses.FirstOrDefault(cond => cond.isCurrent == true);
                    if (resHisoryProfile.IsNotNull())
                    {

                        lblAddress1.Text = (_applicantOrderCart.IsLocationServiceTenant ?
                            resHisoryProfile.Address1
                            : resHisoryProfile.Address1 + "," + resHisoryProfile.Address2).HtmlEncode();
                        lblZip.Text = resHisoryProfile.Zipcode.HtmlEncode();
                        lblCity.Text = resHisoryProfile.CityName.HtmlEncode();
                        //UAT-3910
                        if (CurrentViewContext.IsLocationServiceTenant && resHisoryProfile.StateName.IsNullOrEmpty())
                        {
                            dvState.Visible = false;
                            LblNameZIPOPostalCode.Text = Resources.Language.POSTALCODE;
                          //  MailingLblNameZIPOPostalCode.Text= Resources.Language.POSTALCODE;
                        }
                        else
                        {
                            dvState.Visible = true;

                            LblNameZIPOPostalCode.Text = Resources.Language.ZIP;
                           // MailingLblNameZIPOPostalCode.Text = Resources.Language.ZIP;
                        }
                        lblState.Text = resHisoryProfile.StateName.HtmlEncode();
                        lblCountry.Text = resHisoryProfile.Country;
                        lblResidingFrom.Text = resHisoryProfile.ResidenceStartDate.HasValue ? resHisoryProfile.ResidenceStartDate.Value.ToShortDateString() : String.Empty;
                        lblResidingTo.Text = resHisoryProfile.ResidenceEndDate.HasValue ? resHisoryProfile.ResidenceEndDate.Value.ToShortDateString() : "until date";
                        lblMotherName.Text = resHisoryProfile.MotherName.HtmlEncode();
                        lblIdentificationNumber.Text = resHisoryProfile.IdentificationNumber;
                        lblCriminalLicenseNumber.Text = resHisoryProfile.LicenseNumber;
                    }
                }
                else
                {
                    Entity.ResidentialHistory currentResHistory = Presenter.GetCurrentResidentialHistory(_orgUserProfile.OrganizationUserID);
                    if (currentResHistory.IsNotNull())
                    {
                        lblAddress1.Text = (currentResHistory.Address.Address1 + "," + currentResHistory.Address.Address2).HtmlEncode();
                        if (currentResHistory.Address.ZipCodeID > 0)
                        {
                            lblZip.Text = currentResHistory.Address.ZipCode.ZipCode1.HtmlEncode();
                            lblCity.Text = currentResHistory.Address.ZipCode.City.CityName.HtmlEncode();
                            lblState.Text = currentResHistory.Address.ZipCode.City.State.StateName.HtmlEncode();
                            lblCountry.Text = currentResHistory.Address.ZipCode.City.State.Country.FullName;
                        }
                        else
                        {
                            if (currentResHistory.Address.AddressExts.IsNotNull() && currentResHistory.Address.AddressExts.Count > 0)
                            {
                                Entity.AddressExt addressExt = currentResHistory.Address.AddressExts.FirstOrDefault();
                                lblZip.Text = addressExt.AE_ZipCode.HtmlEncode();
                                lblCity.Text = addressExt.AE_CityName.HtmlEncode();
                                lblState.Text = addressExt.AE_StateName.HtmlEncode();
                                lblCountry.Text = addressExt.Country.FullName;
                            }
                        }
                        lblResidingFrom.Text = currentResHistory.RHI_ResidenceStartDate.HasValue ? currentResHistory.RHI_ResidenceStartDate.Value.ToShortDateString() : String.Empty;
                        lblResidingTo.Text = currentResHistory.RHI_ResidenceEndDate.HasValue ? currentResHistory.RHI_ResidenceEndDate.Value.ToShortDateString() : "until date";
                    }
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
        }

        /// <summary>
        /// To bind other details i.e. Custom Attributes
        /// </summary>
        /// <param name="_applicantOrderCart"></param>
        private void BindOtherDetails(ApplicantOrderCart _applicantOrderCart)
        {
            caOtherDetails.lstTypeCustomAttributes = _applicantOrderCart.GetCustomAttributeValues();
            caOtherDetails.DataSourceModeType = DataSourceMode.ExternalList;
            caOtherDetails.Title = "Other Details";
            caOtherDetails.ControlDisplayMode = DisplayMode.Labels;
            if (caOtherDetails.lstTypeCustomAttributes.Count > 0)
            {
                divHR.Style.Add("display", "block");
            }
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


        ///// <summary>
        ///// UAT 1438: Enhancement to allow students to select a User Group.
        ///// To bind other details i.e. Custom Attributes USer Group
        ///// </summary>
        ///// <param name="_applicantOrderCart"></param>
        //private void BindOtherDetailsUserGroup(ApplicantOrderCart _applicantOrderCart)
        //{
        //    if (!_applicantOrderCart.GetCustomAttributeValuesForUserGroup().IsNullOrEmpty())
        //    {
        //        OtherDetailsUserGroup.DataSourceModeType = DataSourceMode.ExternalList;
        //        OtherDetailsUserGroup.Title = "Other Details";
        //        OtherDetailsUserGroup.ControlDisplayMode = DisplayMode.Labels;
        //        OtherDetailsUserGroup.ShowUserGroupCustomAttribute = false;
        //        OtherDetailsUserGroup.TenantId = CurrentViewContext.TenantId;
        //        OtherDetailsUserGroup.CurrentLoggedInUserId = CurrentLoggedInUserId;
        //        OtherDetailsUserGroup.ShowReadOnlyUserGroupCustomAttribute = true;
        //        OtherDetailsUserGroup.Visible = true;
        //    }
        //}

        /// <summary>
        /// To bind Package data
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        void BindPackageData(Boolean isLocationServiceTenant)
        {

            String orderRequestType = Convert.ToString(applicantOrderCart.OrderRequestType);
            lblInstitutionHierarchy.Text = InstitutionHierarchy.HtmlEncode();
            // hdrPackageDetail.InnerText = CurrentViewContext.IsLocationServiceTenant ? "Order Details" : "Package Detail";
            hdrPackageDetail.InnerText = CurrentViewContext.IsLocationServiceTenant ? Resources.Language.ORDRDTLS : "Package Detail";
            //hdrOrderDetail.InnerText = CurrentViewContext.IsLocationServiceTenant ? "Order Selection Details" : "Order Detail";
            hdrOrderDetail.InnerText = CurrentViewContext.IsLocationServiceTenant ? Resources.Language.ORDSELDTLS : Resources.Language.ORDRDTLS;
            //hdrPersonalInfo.InnerText = CurrentViewContext.IsLocationServiceTenant ? "Profile Details" : "Personal Information";
            hdrPersonalInfo.InnerText = CurrentViewContext.IsLocationServiceTenant ? Resources.Language.PROFILEDETAILS : "Personal Information";
            //Temporary
            Decimal? _bkgPkgPrice = GetBackgroundPackagesPrice(applicantOrderCart);
           

            if (applicantOrderCart.CompliancePackagesGrandTotal != null)
            {
                lblTotalPrice.Text = "$ " + Convert.ToString(decimal.Round((applicantOrderCart.CompliancePackagesGrandTotal + (_bkgPkgPrice ?? 0)) ?? 0, 2));
            }
            else
            {
                lblTotalPrice.Text = "$ " + Convert.ToString(decimal.Round(_bkgPkgPrice ?? 0, 2));
            }

            if (isLocationServiceTenant && !applicantOrderCart.ChangePaymentTypeCode.IsNullOrEmpty())
            {
                _bkgPkgPrice = GetCABSBackgroundPackagesPrice(applicantOrderCart.lstApplicantOrder[0].OrderId);
                lblTotalPrice.Text = "$ " + Convert.ToString(decimal.Round(_bkgPkgPrice ?? 0, 2));
            }

            if (applicantOrderCart.IsRushOrderIncluded)
            {
                lblRushOrderPrice.Text = "$ " + Convert.ToString(Decimal.Round(Convert.ToDecimal(applicantOrderCart.RushOrderPrice), 2));
            }

            var _isCompliancePkgVisible = false;
            if (CurrentViewContext.OrderPaymentDetaildId > 0 || (CurrentViewContext.lstOPDs.IsNotNull() && CurrentViewContext.lstOPDs.Count > AppConsts.NONE))
            {
                var _opd = CurrentViewContext.lstOPDs.Where(x => x.OrderPkgPaymentDetails.Any(y => y.OPPD_BkgOrderPackageID.IsNull()
                                              && !y.OPPD_IsDeleted
                                              && y.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue())).FirstOrDefault();

                if (_opd.IsNotNull())
                    _isCompliancePkgVisible = true;
            }

            //Show Compliance Package Details
            if ((CurrentViewContext.DPPSIds.IsNotNull() && CurrentViewContext.DPPSIds.Count > AppConsts.NONE && applicantOrderCart.ChangePaymentTypeCode.IsNullOrEmpty())
                || _isCompliancePkgVisible)
            {
                BindCompliancePackages(); //UAT-3283

                if (CompliancePackages.IsNotNull() && CompliancePackages.Count > AppConsts.NONE)
                {
                    //START UAT-3283
                    foreach (OrderCartCompliancePackage cp in CompliancePackages)
                    {
                        foreach (RepeaterItem cpItem in rptCompliancePkgs.Items)
                        {
                            var _packageId = Convert.ToInt32((cpItem.FindControl("hdfPackageId") as HiddenField).Value);
                            if (cp.CompliancePackageID == _packageId)
                            {
                                //string controlSuffix = (ctrlIndex == 1 ? "" : "_" + ctrlIndex);
                                HtmlGenericControl ctrlDivCompliancePackage = cpItem.FindControl("divCompliancePackage") as HtmlGenericControl;
                                // ctrlIndex++;
                                if (ctrlDivCompliancePackage.IsNotNull())
                                    ctrlDivCompliancePackage.Visible = true;

                                if (!ShowPkgPrice(false, 0))
                                {
                                    HtmlGenericControl ctrlDvPackagePrice = cpItem.FindControl("dvPackagePrice") as HtmlGenericControl;
                                    if (ctrlDvPackagePrice.IsNotNull())
                                        ctrlDvPackagePrice.Style.Add("display", "none");
                                }


                                Label ctrlLblSubscriptione = cpItem.FindControl("lblSubscription") as Label;
                                if (ctrlLblSubscriptione.IsNotNull())
                                    ctrlLblSubscriptione.Text = cp.SubscriptionPeriodMonths;

                                Label ctrlLblPackage = cpItem.FindControl("lblPackage") as Label;
                                if (ctrlLblPackage.IsNotNull())
                                    ctrlLblPackage.Text = cp.PackageName;

                                if (applicantOrderCart.CompliancePackages.IsNotNull() && applicantOrderCart.CompliancePackages.Count > AppConsts.NONE)
                                {
                                    OrderCartCompliancePackage cartcp = applicantOrderCart.CompliancePackages.Values.FirstOrDefault(ccp => ccp.OrderId == cp.OrderId);
                                    if (cartcp.IsNotNull())
                                    {
                                        HtmlGenericControl ctrlDivCPT = cpItem.FindControl("divCPT") as HtmlGenericControl;
                                        if (ctrlDivCPT.IsNotNull())
                                        {
                                            ctrlDivCPT.Visible = cartcp.Amount.IsNotNull() && Convert.ToDecimal(cartcp.Amount) > 0 ? true : false;
                                            if (ctrlDivCPT.Visible)
                                            {
                                                var _payType = GetPaymentType(false, 0, cartcp.OrderId);
                                                Label ctrlLblCompliancePkgPaymentType = cpItem.FindControl("lblCompliancePkgPaymentType") as Label;
                                                if (ctrlLblCompliancePkgPaymentType.IsNotNull())
                                                    ctrlLblCompliancePkgPaymentType.Text = _payType;
                                            }
                                            if (cartcp.Amount != null)
                                            {
                                                Label ctrlLblPrice = cpItem.FindControl("lblPrice") as Label;
                                                if (ctrlLblPrice.IsNotNull())
                                                    ctrlLblPrice.Text = "$ " + Convert.ToString(decimal.Round(Convert.ToDecimal(cartcp.Amount), 2));
                                            }
                                        }
                                        if (Convert.ToString(applicantOrderCart.OrderRequestType) == OrderRequestType.RenewalOrder.GetStringValue())
                                        {
                                            Label ctrlLblSubscription = cpItem.FindControl("lblSubscription") as Label;
                                            if (ctrlLblSubscription.IsNotNull())
                                                ctrlLblSubscription.Text = Convert.ToString(cartcp.RenewalDuration);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //END UAT-3283

                    //Commented in UAT-3283

                    //int ctrlIndex = 1;
                    //foreach (OrderCartCompliancePackage cp in CompliancePackages)
                    //{
                    //    string controlSuffix = (ctrlIndex == 1 ? "" : "_" + ctrlIndex);
                    //    HtmlGenericControl ctrlDivCompliancePackage = (HtmlGenericControl)FindControl("divCompliancePackage" + controlSuffix);
                    //   // ctrlIndex++;
                    //    if (ctrlDivCompliancePackage.IsNotNull())
                    //        ctrlDivCompliancePackage.Visible = true;

                    //    if (!ShowPkgPrice(false, 0))
                    //    {
                    //        HtmlGenericControl ctrlDvPackagePrice = (HtmlGenericControl)FindControl("dvPackagePrice" + controlSuffix);
                    //        if (ctrlDvPackagePrice.IsNotNull())
                    //            ctrlDvPackagePrice.Style.Add("display", "none");
                    //    }


                    //    Label ctrlLblSubscriptione = (Label)FindControl("lblSubscription" + controlSuffix);
                    //    if (ctrlLblSubscriptione.IsNotNull())
                    //        ctrlLblSubscriptione.Text = cp.SubscriptionPeriodMonths;

                    //    Label ctrlLblPackage = (Label)FindControl("lblPackage" + controlSuffix);
                    //    if (ctrlLblPackage.IsNotNull())
                    //        ctrlLblPackage.Text = cp.PackageName;

                    //    if (applicantOrderCart.CompliancePackages.IsNotNull() && applicantOrderCart.CompliancePackages.Count > AppConsts.NONE)
                    //    {
                    //        OrderCartCompliancePackage cartcp = applicantOrderCart.CompliancePackages.Values.FirstOrDefault(ccp => ccp.OrderId == cp.OrderId);
                    //        if (cartcp.IsNotNull())
                    //        {
                    //            HtmlGenericControl ctrlDivCPT = (HtmlGenericControl)FindControl("divCPT" + controlSuffix);
                    //            if (ctrlDivCPT.IsNotNull())
                    //            {
                    //                ctrlDivCPT.Visible = cartcp.Amount.IsNotNull() && Convert.ToDecimal(cartcp.Amount) > 0 ? true : false;
                    //                if (ctrlDivCPT.Visible)
                    //                {
                    //                    var _payType = GetPaymentType(false, 0, cartcp.OrderId);
                    //                    Label ctrlLblCompliancePkgPaymentType = (Label)FindControl("lblCompliancePkgPaymentType" + controlSuffix);
                    //                    if (ctrlLblCompliancePkgPaymentType.IsNotNull())
                    //                        ctrlLblCompliancePkgPaymentType.Text = _payType;
                    //                }
                    //                if (cartcp.Amount != null)
                    //                {
                    //                    Label ctrlLblPrice = (Label)FindControl("lblPrice" + controlSuffix);
                    //                    if (ctrlLblPrice.IsNotNull())
                    //                        ctrlLblPrice.Text = "$ " + Convert.ToString(decimal.Round(Convert.ToDecimal(cartcp.Amount), 2));
                    //                }
                    //            }
                    //            if (Convert.ToString(applicantOrderCart.OrderRequestType) == OrderRequestType.RenewalOrder.GetStringValue())
                    //            {
                    //                Label ctrlLblSubscription = (Label)FindControl("lblSubscription" + controlSuffix);
                    //                if (ctrlLblSubscription.IsNotNull())
                    //                    ctrlLblSubscription.Text = Convert.ToString(cartcp.RenewalDuration);
                    //            }
                    //        }
                    //    }
                    //}
                }

            }

            //Show Background Package Data - Temporary
            ////List<Int32> bkgHierarchyMappingIds = new List<int>();
            ////if (!applicantOrderCart.lstApplicantOrder.IsNullOrEmpty())
            ////{
            ////    foreach (var applicantOrder in applicantOrderCart.lstApplicantOrder)
            ////    {
            ////        if (applicantOrder.lstPackages.IsNotNull() && applicantOrder.lstPackages.Count > AppConsts.NONE)
            ////        {
            ////            bkgHierarchyMappingIds = applicantOrder.lstPackages.Select(condition => condition.BPHMId).ToList();
            ////        }
            ////        break;
            ////    }
            ////}

            Presenter.GetOrderBkgPackageDetails(lblOrderId.Text);
            if (lstExternalPackages.Rows.Count > 0)
            {
                divBackgroundPackage.Visible = true;
                var _dt = lstExternalPackages.Clone();
                if (applicantOrderCart.OrderPaymentdetailId > 0)
                {
                    //var _lstBOPs = CurrentViewContext.lstOPDs.Select(x => x.OrderPkgPaymentDetails.Select(col => col.OPPD_BkgOrderPackageID)).ToList();

                    var _lstBOPIDs = new List<Int32>();

                    foreach (var opd in CurrentViewContext.lstOPDs)
                    {
                        var orderPkgPaymentDeatils = opd.OrderPkgPaymentDetails.Where(cond => !cond.OPPD_IsDeleted);
                        foreach (var oppd in orderPkgPaymentDeatils)
                        {
                            if (oppd.OPPD_BkgOrderPackageID.IsNotNull() && oppd.lkpOrderPackageType.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue())
                                _lstBOPIDs.Add(Convert.ToInt32(oppd.OPPD_BkgOrderPackageID));
                        }
                    }

                    foreach (var bopID in _lstBOPIDs)
                    {
                        DataRow dataRow = lstExternalPackages
                       .AsEnumerable()
                       .Where(row => row.Field<Int32>("BkgOrderPackageID") == bopID).FirstOrDefault();
                        _dt.Rows.Add(dataRow.ItemArray);
                    }

                    //for (int i = 0; i < _lstBOPs.Count(); i++)
                    //{
                    //    if (_lstBOPs[i].FirstOrDefault().IsNotNull())
                    //    {
                    //        DataRow row = lstExternalPackages.Select("BkgOrderPackageID=" + _lstBOPs[i].FirstOrDefault().OPPD_BkgOrderPackageID).FirstOrDefault();
                    //        _dt.Rows.Add(row);
                    //    }
                    //}
                    rptBackgroundPackages.DataSource = _dt;
                }
                //UAT-1648:As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
                else if (String.Compare(applicantOrderCart.OrderRequestType, OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) == AppConsts.NONE)
                {
                    var lstBOPIDs = new List<Int32>();
                    DataTable bkgPackages = new DataTable();
                    foreach (var opd in CurrentViewContext.lstOPDs.Where(opd => !opd.OPD_IsDeleted && applicantOrderCart.RecentAddedOPDs.Contains(opd.OPD_ID)))
                    {
                        var orderPkgPaymentDeatils = opd.OrderPkgPaymentDetails.Where(cond => !cond.OPPD_IsDeleted);
                        foreach (var oppd in orderPkgPaymentDeatils)
                        {
                            if (oppd.OPPD_BkgOrderPackageID.IsNotNull() && oppd.lkpOrderPackageType.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue())
                                lstBOPIDs.Add(Convert.ToInt32(oppd.OPPD_BkgOrderPackageID));
                        }
                    }

                    foreach (var bopID in lstBOPIDs)
                    {
                        DataRow dataRow = lstExternalPackages
                       .AsEnumerable()
                       .Where(row => row.Field<Int32>("BkgOrderPackageID") == bopID).FirstOrDefault();
                        _dt.Rows.Add(dataRow.ItemArray);
                    }
                    rptBackgroundPackages.DataSource = _dt;
                }
                else
                    rptBackgroundPackages.DataSource = CurrentViewContext.lstExternalPackages;

                rptBackgroundPackages.DataBind();
            }

            //if (bkgHierarchyMappingIds.IsNotNull() && bkgHierarchyMappingIds.Count > AppConsts.NONE)
            //{
            //    divBackgroundPackage.Visible = true;
            //    Presenter.GetOrderBkgPackageDetails(Convert.ToInt32(lblOrderId.Text));

            //    var _dt = new DataTable();
            //    if (applicantOrderCart.OrderPaymentdetailId > 0)
            //    { 
            //        var _lstBOPs = CurrentViewContext.lstOPDs.Select(x => x.OrderPkgPaymentDetails).ToList();

            //        for (int i = 0; i < _lstBOPs.Count(); i++)
            //        {
            //            DataRow[] row = _dt.Select("BkgOrderPackageID=" + _lstBOPs[i]);
            //            _dt.Rows.Add(row);
            //        }
            //        rptBackgroundPackages.DataSource = _dt;
            //    }
            //    else
            //        rptBackgroundPackages.DataSource = CurrentViewContext.lstExternalPackages;

            //    rptBackgroundPackages.DataBind();
            //}
        }

        /// <summary>
        /// Gets the price of all the background packages selected
        /// </summary>
        /// <returns></returns>
        private static Decimal GetBackgroundPackagesPrice(ApplicantOrderCart applicantOrderCart)
        {
            Decimal _backgroundPackagesPrice = 0;

            if (!applicantOrderCart.lstApplicantOrder.IsNullOrEmpty() && !applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                foreach (var bkgPackage in applicantOrderCart.lstApplicantOrder[0].lstPackages)
                {
                    _backgroundPackagesPrice += (bkgPackage.TotalBkgPackagePrice.IsNullOrEmpty() ? AppConsts.NONE : bkgPackage.TotalBkgPackagePrice);
                }
            }
            return _backgroundPackagesPrice;
        }

        private Decimal GetCABSBackgroundPackagesPrice(int OrderId)
        {
            Decimal _backgroundPackagesPrice = 0;

            if (OrderId > 0)
            {
                _backgroundPackagesPrice = Presenter.GetOrderPriceTotal(OrderId);
            }
            return _backgroundPackagesPrice;
        }


        /// <summary>
        /// Checks the order status track. If this page is not opened as per correct order status track then, redirected to correct order.
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        private void RedirectIfIncorrectOrderStage(ApplicantOrderCart applicantOrderCart)
        {
            Presenter.GetNextPagePathByOrderStageID(applicantOrderCart);

            //Redirect to next page path if Order Status track is not correct.
            if (CurrentViewContext.NextPagePath.IsNotNull())
            {
                Response.Redirect(CurrentViewContext.NextPagePath);
            }
        }

        /// <summary>
        /// To set application Order cart
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        private void SetApplicationOrderCart(ApplicantOrderCart applicantOrderCart)
        {
            RedirectIfIncorrectOrderStage(applicantOrderCart);
            applicantOrderCart.AddOrderStageTrackID(OrderStages.OnlineConfirmation);
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ORDER_CONFIRMATION, applicantOrderCart);
        }

        private void CreateCustomForm()
        {
            ApplicantOrderCart _applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;
            String packages = String.Empty;
            packages = GetPackageIdString();
            List<Int32> lstCustomForms = new List<Int32>();
            List<Int32> lstGroupIds = new List<Int32>();
            Presenter.GetAttributeFieldsOfSelectedPackages(packages);

            if (!String.IsNullOrEmpty(packages))
            {
                BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 3.1: Method 'Presenter.GetAttributeFieldsOfSelectedPackages' executed successfully for PackageId(s): " +
                                                   packages + " and OrderId(s)" + lblOrderId.Text);
            }

            List<AttributeFieldsOfSelectedPackages> lstCriminalAttributes = CurrentViewContext.LstInternationCriminalSrchAttributes;
            if (!_applicantOrderCart.lstPrevAddresses.IsNullOrEmpty())
            {
                PreviousAddressContract resHisoryProfile = _applicantOrderCart.lstPrevAddresses.FirstOrDefault(cond => cond.isCurrent == true);
                if (resHisoryProfile.IsNotNull() && resHisoryProfile.CountryId != AppConsts.COUNTRY_USA_ID)
                {
                    if (!lstCriminalAttributes.IsNullOrEmpty())
                    {
                        if (lstCriminalAttributes.Any(x => x.BSA_Code == LCNAttCode.ToString() && x.IsAttributeDisplay))
                        {
                            divCriminalLicenseNumber.Visible = true;
                        }
                        if (lstCriminalAttributes.Any(x => x.BSA_Code == MotherNameAttrCode.ToString() && x.IsAttributeDisplay))
                        {
                            divMothersName.Visible = true;
                        }
                        if (lstCriminalAttributes.Any(x => x.BSA_Code == IdentificationNumberAttrCode.ToString() && x.IsAttributeDisplay))
                        {
                            divIdentificationNumber.Visible = true;
                        }
                    }
                }
            }

            if (!lstBackgroundOrderData.IsNullOrEmpty())
            {
                lstCustomForms = lstBackgroundOrderData.Where(x => x.CustomFormId != AppConsts.ONE).DistinctBy(x => x.CustomFormId).Select(x => x.CustomFormId).ToList();
                #region E DRUG SCREENING
                Presenter.GetEDrugAttributeGroupIdAndFormId();

                // StringBuilder _sb = new StringBuilder();
                var _webCcfLoaded = false;
                var _customFormsLoaded = false;
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
                            _webCCFForm.IsReview = false;
                            _webCCFForm.IsOrderConfirmation = true;
                            _webCCFForm.ShowRegistrationID = CurrentViewContext.IsOrderStatusPaid;
                            _webCCFForm.CustomFormId = lstCustomForms[custId];
                            _webCCFForm.AttributeGroupId = lstGroupIds[grpId];
                            _webCCFForm.LstBackgroundOrderData = newLstBackGroundOrderData;
                            _webCCFForm.LstAttributeForCustomFormContract = lstCustomFormAttributes;
                            pnlLoader.Controls.Add(_webCCFForm);
                            _webCcfLoaded = true;
                            //_sb.Append(" WebCCF.ascx loaded for CustomFormId: " + custId + " and BkgSvcAttributeGroupId: " + grpId);
                        }
                        else
                        {
                            StringBuilder xmlStringData = new StringBuilder();
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

                            CustomFormHtlm _customForm = Page.LoadControl("~/BkgOperations/UserControl/CustomFormHtlm.ascx") as CustomFormHtlm;
                            _customForm.lstCustomFormAttributes = lstCustomFormAttributes;
                            _customForm.groupId = lstGroupIds[grpId];
                            //Total Number Of Instane for a particular group
                            _customForm.InstanceId = newLstBackGroundOrderData.Where(x => x.BkgSvcAttributeGroupId == lstGroupIds[grpId] && x.CustomFormId == lstCustomForms[custId]).Count();
                            _customForm.CustomFormId = lstCustomForms[custId];
                            _customForm.tenantId = TenantId;
                            _customForm.lstBackgroundOrderData = newLstBackGroundOrderData;
                            _customForm.IsOrderConfirmation = true;
                            pnlLoader.Controls.Add(_customForm);
                            _customFormsLoaded = true;
                            //_sb.Append(" CustomFormHtlm.ascx loaded for CustomFormId: " + custId + " and BkgSvcAttributeGroupId: " + grpId);
                        }
                    }
                }
                BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 3.2: For OrderId(s): " + lblOrderId.Text +
                    ", Custom forms loaded: " + (_customFormsLoaded ? "Yes" : "No") +
                    ", WebCcf Loaded:" + (_webCcfLoaded ? "Yes" : "No"));
            }
        }

        public void BindResidentialHistory()
        {
            if (!lstResendialHistory.IsNullOrEmpty() && IsResidentialHistoryVisible)
            {
                BkgOrderResidentialHistories userControl = null;
                userControl = LoadControl("~/BkgOperations/UserControl/BkgOrderResidentialHistories.ascx") as BkgOrderResidentialHistories;
                userControl.OrderConfirmation = true;
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
                BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 4: UserControl BkgOrderResidentialHistories.ascx loaded successfully for OrderId(s)" + lblOrderId.Text);
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
        /// Copy Data from AMS(Background) Package to Compliance Package
        /// </summary>
        private void CopyBkgDataToCompliancePackage(ApplicantOrderCart _applicantOrderCart)
        {
            if ((!_applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && _applicantOrderCart.IsCompliancePackageSelected)
                || (_applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && _applicantOrderCart.IsCompliancePackageSelected))
            {
                if (_applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscriptionByAdmin.GetStringValue()
                  &&
                  (CurrentViewContext.AppChangeSubPaymentTypeCode == PaymentOptions.InvoiceWithApproval.GetStringValue()
                 || CurrentViewContext.AppChangeSubPaymentTypeCode == PaymentOptions.Money_Order.GetStringValue()))
                {
                    //base.ShowSuccessMessage("Your new subscription will become active when your balance payment gets approved.");
                }
                //else if Balance Payment for Credit card and Paypal transactions
                else if (_applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscriptionByAdmin.GetStringValue())
                {
                    //base.ShowSuccessMessage("Thanks for paying the balance amount.");
                }
                else if (
                      _applicantOrderCart.lstApplicantOrder[0].LstOrderStageTrackID.Contains(OrderStages.OrderPaymentDetails) &&
                      _applicantOrderCart.ChangePaymentTypeCode.IsNotNull() &&
                      (
                           _applicantOrderCart.ChangePaymentTypeCode == PaymentOptions.InvoiceWithApproval.GetStringValue()
                        || _applicantOrderCart.ChangePaymentTypeCode == PaymentOptions.Money_Order.GetStringValue())
                      )
                {
                    //base.ShowSuccessMessage("Your new subscription will become active when your balance payment gets approved.");
                }
                else if (_applicantOrderCart.OrderRequestType != OrderRequestType.ChangeSubscription.GetStringValue())
                {
                    if (CurrentViewContext.DPPSIds.Count > 0)
                    {
                        Presenter.CopyBkgDataToCompliancePackage(lblOrderId.Text);
                        /*UAT-1476:When a tracking package is ordered and there was already a previous package with entered data,
                         then there would be data movement as if there were a subscription change.*/
                        //UAT_issueFix 06/07/2017 Release 127
                        //Presenter.CopyCompPackageDataForNewOrder(lblOrderId.Text);

                        BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 2: Methods 'Presenter.CopyBkgDataToCompliancePackage' and 'Presenter.CopyCompPackageDataForNewOrder' completed successfully."
                            + " for OrderId(s):" + lblOrderId.Text);
                    }
                }
            }
        }

        /// <summary>
        /// Binds the Payment Modes
        /// </summary>
        private void BindPaymentModes(ApplicantOrderCart applicantOrderCart)
        {
            var _records = CurrentViewContext.lstOPDs.Where(opd => opd.OPD_Amount.IsNotNull()
                                                                          && opd.OPD_IsDeleted == false && opd.OPD_Amount > 0).ToList();
            //UAT-3268
            if (!CurrentViewContext.lstAdditionalPaymentModes.IsNullOrEmpty())
            {
                List<Int32> lstAdditionPaymentModeIds = CurrentViewContext.lstAdditionalPaymentModes.Select(sel => sel.OPD_ID).ToList();
                _records = _records.Where(cond => !lstAdditionPaymentModeIds.Contains(cond.OPD_ID)).ToList();
            }
            //End //UAT-3268

            if (_records.Count == 0)
            {
                divPaymentTypes.Visible = false;
                return;
            }

            rptPaymentModes.DataSource = _records;
            rptPaymentModes.DataBind();
        }

        /// <summary>
        /// Returns whether the Compliance Package Price should be displayed or not, 
        /// depending on whether Payment Mode for the group, to which the Package belongs to,
        /// was InvoiceWithApproval or InvoiceWithOutApproval type.
        /// </summary>
        /// <param name="orderCart"></param>
        /// <returns></returns>
        private Boolean ShowPkgPrice(Boolean isBkgPkg, Int32 bopId = 0, Int32 orderId = AppConsts.NONE)
        {
            Boolean _showPkgPrice = true;

            foreach (var opd in CurrentViewContext.lstOPDs)
            {
                if (opd.lkpPaymentOption.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || opd.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue())
                {
                    if (!isBkgPkg && opd.OrderPkgPaymentDetails.Where(oppd => oppd.OPPD_BkgOrderPackageID.IsNull() &&
                       oppd.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()
                       && oppd.OPPD_IsDeleted == false
                       && (oppd.OrderPaymentDetail.OPD_OrderID == orderId || orderId == AppConsts.NONE)).Any())
                    {
                        _showPkgPrice = false;
                        break;
                    }
                    else if (isBkgPkg)
                    {
                        var _lstOPPD = opd.OrderPkgPaymentDetails.Where(oppd => oppd.OPPD_BkgOrderPackageID.IsNotNull() &&
                            oppd.lkpOrderPackageType.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()
                            && oppd.OPPD_IsDeleted == false).ToList();

                        foreach (var oppd in _lstOPPD)
                        {
                            if (oppd.OPPD_BkgOrderPackageID == bopId)
                            {
                                _showPkgPrice = false;
                                break;
                            }
                        }
                    }
                }
            }
            return _showPkgPrice;
        }

        /// <summary>
        /// Returns whether there is any Invoice Payment type Package in the Order
        /// </summary>
        /// <param name="orderCart"></param>
        /// <returns></returns>
        private Boolean IsAnyInvoiceTypePkg(ApplicantOrderCart orderCart)
        {
            if (CurrentViewContext.OrderData.IsNotNull())
            {
                foreach (Order order in CurrentViewContext.OrderData)
                {
                    var lstOrdPayDetails = order.OrderPaymentDetails;
                    if (lstOrdPayDetails.Where(opd =>
                                                    opd.lkpPaymentOption.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()
                                                 || opd.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()
                                                 ).Any())
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Bind the Payment Instructions for all the Payment Modes selected
        /// </summary>
        private void BindInstructions()
        {
            rptInstructions.DataSource = CurrentViewContext.lstClientPaymentOptns;
            rptInstructions.DataBind();
            if (rptInstructions.Items.Count != AppConsts.NONE)
                divPaymentInstruction.Visible = true;
            else
                divPaymentInstruction.Visible = false;
        }

        /// <summary>
        /// Get the Payment Type based on the Package type
        /// </summary>
        /// <param name="isBkgPackage"></param>
        /// <param name="bopId"></param>
        /// <returns></returns>
        private String GetPaymentType(Boolean isBkgPackage, Int32 bopId = 0, Int32 orderId = AppConsts.NONE)
        {
            //var _lstOpd = new List<OrderPaymentDetail>();

            //_lstOpd = CurrentViewContext.OrderData.OrderPaymentDetails.Where(opd => opd.OPD_IsDeleted == false).ToList();
            var _opdPaymentType = String.Empty;

            foreach (var opd in CurrentViewContext.lstOPDs)
            {
                if (isBkgPackage)
                {
                    var _oppd = opd.OrderPkgPaymentDetails.Where(oppd => oppd.OPPD_IsDeleted == false
                            && oppd.OPPD_BkgOrderPackageID.IsNotNull() && oppd.lkpOrderPackageType.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()
                            && oppd.OPPD_BkgOrderPackageID == bopId).FirstOrDefault();

                    if (_oppd.IsNotNull())
                    {
                        _opdPaymentType = opd.lkpPaymentOption.Name;
                        break;
                    }
                }
                else
                {
                    var _oppd = opd.OrderPkgPaymentDetails.Where(oppd => oppd.OPPD_IsDeleted == false
                             && oppd.OPPD_BkgOrderPackageID.IsNull()
                             && oppd.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()
                             && (oppd.OrderPaymentDetail.OPD_OrderID == orderId || orderId == AppConsts.NONE))
                             .FirstOrDefault();

                    if (_oppd.IsNotNull())
                    {
                        _opdPaymentType = opd.lkpPaymentOption.Name;
                        break;
                    }
                }
            }
            return _opdPaymentType;
        }

        //UAT-2970
        private void BindCreditCardUserAgreement()
        {
            Boolean isNeedToShowUserAgreement = false;

            foreach (var opd in CurrentViewContext.lstOPDs)
            {
                isNeedToShowUserAgreement = opd.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue();
                if (isNeedToShowUserAgreement)
                {
                    break;
                }
            }
            if (isNeedToShowUserAgreement)
            {
                dvUserAgreement.Visible = true;
                litText.Text = Presenter.GetCreditCardAgreement();
                if (IsLocationServiceTenant)
                { 
                    dvUserAgreement.Attributes.Add("style", "page-break-inside:avoid");
                }
            }
            else
            {
                dvUserAgreement.Visible = false;
            }
        }

        #region UAT-3268
        /// <summary>
        /// Bind Data for additional price payment modes.
        /// </summary>
        private void BindAdditionalPaymentModes()
        {
            CurrentViewContext.lstAdditionalPaymentModes = Presenter.GetAdditionalPaymentModes();
            if (lstAdditionalPaymentModes.Count == 0)
            {
                dvAdditionalPaymentType.Visible = false;
                return;
            }
            rptAdditionalPaymentModes.DataSource = CurrentViewContext.lstAdditionalPaymentModes;
            rptAdditionalPaymentModes.DataBind();
        }

        protected void rptAdditionalPaymentModes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var _hdfAdditionalPaymentCode = e.Item.FindControl("hdfAdditionalPaymentType") as HiddenField;
            var _additionalPriceDiv = e.Item.FindControl("divAdditionalPrice") as HtmlGenericControl;

            if (_hdfAdditionalPaymentCode.IsNotNull() && _additionalPriceDiv.IsNotNull())
            {
                if (_hdfAdditionalPaymentCode.Value == PaymentOptions.InvoiceWithApproval.GetStringValue() ||
                    _hdfAdditionalPaymentCode.Value == PaymentOptions.InvoiceWithOutApproval.GetStringValue())
                {
                    _additionalPriceDiv.Visible = false;
                }
            }
        }
        #endregion

        #region UAT-3283

        private void BindCompliancePackages()
        {
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            if (!applicantOrderCart.CompliancePackages.IsNullOrEmpty() && applicantOrderCart.CompliancePackages.Count > AppConsts.NONE)
            {
                rptCompliancePkgs.DataSource = applicantOrderCart.CompliancePackages.Values.ToList();
                rptCompliancePkgs.DataBind();
            }
        }

        #endregion

        private void ManageSSN()
        {
            String AppSSN = lblSSN.Text.Trim();
            AppSSN = AppSSN.Replace(@"-", "");
            if (AppSSN == AppConsts.DefaultSSN || AppSSN == "#####1111")
            {
                if (applicantOrderCart.IsLocationServiceTenant)
                {
                    divSSN.Visible = false;
                }
            }
        }

        private void AddSuffix()
        {
            if (CurrentViewContext.IsLocationServiceTenant)
            {
                Presenter.GetSuffixes();
            }
        }
        #endregion

        #endregion
    }
}


