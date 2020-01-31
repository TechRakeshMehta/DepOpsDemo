#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;


#endregion

#region UserDefined

using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel;
using INTERSOFT.WEB.UI.WebControls;
using System.Text;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceOperations.Views;
using System.Web.UI.HtmlControls;
using INTSOF.UI.Contract.Globalization;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class OrderPayment : BaseUserControl, IOrderPaymentView
    {

        #region Variables

        #region Private Variables

        private Int32 _tenantId;
        private OrderPaymentPresenter _presenter = new OrderPaymentPresenter();
        private ApplicantOrderCart _applicantOrderCart;
        private FingerPrintAppointmentContract _fingerPrintAppointmentData;
        private String _viewType;
        #endregion

        #region Public Variables

        Boolean isPaymentApprovalRequired = false;

        #endregion

        #endregion

        #region  Public Properties

        public OrderPaymentPresenter Presenter
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

        List<PkgList> IOrderPaymentView.lstPaymentOptions
        {
            get
            {
                if (!ViewState["lstPaymentOptions"].IsNullOrEmpty())
                    return (List<PkgList>)ViewState["lstPaymentOptions"];
                return new List<PkgList>();
            }
            set
            {
                ViewState["lstPaymentOptions"] = value;
            }
        }

        Int32 IOrderPaymentView.DPPSId
        {
            get;
            set;
        }

        Int32 IOrderPaymentView.TenantId
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

        Int32 IOrderPaymentView.PaymentMode_InvoiceId
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

        Int32 IOrderPaymentView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Boolean IOrderPaymentView.ShowRushOrderForInvioce
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

        Boolean IOrderPaymentView.ShowRushOrder
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

        String IOrderPaymentView.PaymentModeCode
        {
            get;
            set;
        }

        String IOrderPaymentView.NextPagePath
        {
            get;
            set;
        }

        public Boolean IsCompliancePackageSelected
        {
            get
            {
                ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
                if (applicantOrderCart.IsNotNull())
                {
                    return applicantOrderCart.IsCompliancePackageSelected;
                }
                return false;
            }
        }

        public Boolean IsModifyShipping
        {
            get;
            set;
        }

        public Boolean IsMailingOptionUpgraded
        {
            get;
            set;
        }

        public Decimal MailingPrice
        {
            get;
            set;
        }

        IOrderPaymentView CurrentViewContext
        {
            get { return this; }
        }

        Int32 IOrderPaymentView.PaymentMode_InvoiceWdoutApprvlId
        {
            get
            {
                return (Int32)(ViewState["PaymentMode_InvoiceWdoutApprvlId"] ?? "0");
            }
            set
            {
                ViewState["PaymentMode_InvoiceWdoutApprvlId"] = value;
            }
        }

        public Boolean IsFromRescheduleScreen
        {
            get;
            set;
        }

        /// <summary>
        /// Id for the Credit Card Payment Mode
        /// </summary>
        Int32 IOrderPaymentView.PaymentMode_CreditCardId
        {
            get
            {
                return Convert.ToInt32(ViewState["CCId"]);
            }
            set
            {
                ViewState["CCId"] = value;
            }
        }

        /// <summary>
        /// Used to identify the current Order Request Type i.e. New order, Change subscription etc.
        /// </summary>
        public String OrderType
        {
            get
            {
                return Convert.ToString(ViewState[AppConsts.ORDER_REQUEST_TYPE_VIEWSTATE]);
            }
            set
            {
                ViewState[AppConsts.ORDER_REQUEST_TYPE_VIEWSTATE] = value;
            }
        }

        private Decimal CompliancePkgPrice
        {
            get
            {
                return ViewState["CompliancePkgPrice"].IsNull() ? 0 : Convert.ToDecimal(ViewState["CompliancePkgPrice"]);
            }
            set
            {
                ViewState["CompliancePkgPrice"] = value;
            }
        }

        //UAT-3268
        public List<BackgroundPackagesContract> lstRotationQualifyingBkgPkgs
        {
            get
            {
                if (!ViewState["lstRotationQualifyingBkgPkgs"].IsNullOrEmpty())
                    return (ViewState["lstRotationQualifyingBkgPkgs"]) as List<BackgroundPackagesContract>;
                return new List<BackgroundPackagesContract>();
            }
            set
            {
                ViewState["lstRotationQualifyingBkgPkgs"] = value;
            }
        }

        #region UAT-3601
        String IOrderPaymentView.PackageNameLabel
        {
            get
            {
                if (!ViewState["PackageNameLabel"].IsNullOrEmpty())
                    return Convert.ToString(ViewState["PackageNameLabel"]);
                return String.Empty;
            }
            set { ViewState["PackageNameLabel"] = value; }
        }
        #endregion

        #region Language Translation
        public String LanguageCode
        {
            get
            {
                LanguageContract langContract = LanguageTranslateUtils.GetCurrentLanguageFromSession();
                if (!langContract.IsNullOrEmpty())
                {
                    return langContract.LanguageCode;
                }
                return Languages.ENGLISH.GetStringValue();
            }
        }
        #endregion

        //UAT-3958
        public Boolean IsAnyOptionsApprovalReq
        {
            get
            {
                if (!ViewState["IsAnyOptionsApprovalReq"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsAnyOptionsApprovalReq"]);
                return false;
            }
            set
            {
                ViewState["IsAnyOptionsApprovalReq"] = value;
                hdnIsAnyOptionsApprovalReq.Value = ViewState["IsAnyOptionsApprovalReq"].ToString();
            }
        }

        //UAT-4057
        /// <summary>
        /// To check if payment options are same for all packages in order
        /// </summary>
        public Boolean IsAllPkgsPaymentOptionSame
        {
            get
            {
                if (!ViewState["IsAllPkgsPaymentOptionSame"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsAllPkgsPaymentOptionSame"]);
                return false;
            }
            set
            {
                ViewState["IsAllPkgsPaymentOptionSame"] = value;
                hdnIsCommonPaymentSelection.Value = ViewState["IsAllPkgsPaymentOptionSame"].ToString();
            }
        }

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
                hdnLanguageCode.Value = CurrentViewContext.LanguageCode;
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
                Presenter.GetLabelData(CurrentViewContext.TenantId);
                List<BackgroundPackagesContract> backgroundpackage = null;
                
                // CurrentViewContext.IsAnyOptionsApprovalReq = false;
                if (!this.IsPostBack)
                {
                    _applicantOrderCart = GetApplicantOrderCart();
                    backgroundpackage = _applicantOrderCart.lstApplicantOrder[0].lstPackages;
                    _fingerPrintAppointmentData = GetFingerPrintAppointmentData();
                    CheckOrder();                    
                    Presenter.OnViewInitialized();
                    Presenter.ShowRushOrderSetting();

                    IsModifyShipping = _applicantOrderCart.IsModifyShipping;
                    IsFromRescheduleScreen = _applicantOrderCart.IsFromReschedulingScreen;
                    IsMailingOptionUpgraded = _applicantOrderCart.IsMailingOptionUpgraded;
                    if (!IsModifyShipping)
                    {
                        BusinessRuleImplementations();
                        
                        // removed in merging
                        //rptPackages.Visible = true;
                    }
                    BindPaymentOptions(); // To do check

                    // if (!_applicantOrderCart.IsNullOrEmpty() && _applicantOrderCart.DPMId > AppConsts.NONE)
                    // hdnCreditCardPaymentModeApprovalCode.Value = Presenter.GetCreditCardPaymentModeApprovalCode(_applicantOrderCart.DPMId);

                    //Presenter.GetPaymentOptions(Convert.ToInt32(_applicantOrderCart.SelectedHierarchyNodeID), _applicantOrderCart.IsCompliancePackageSelected);
                    //cmbPaymentModes.DataSource = CurrentViewContext.lstPaymentOptions;
                    //cmbPaymentModes.DataBind();

                    //if (_applicantOrderCart.IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel)
                    //{
                    //    dvPrice.Visible = false;
                    //}
                    if (!IsModifyShipping)
                    {
                        String _currentStep = " (" + Resources.Language.STEP + " " + (_applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep) +
                            " " + Resources.Language.OF + " " + _applicantOrderCart.GetTotalOrderSteps() + ")";

                        base.SetPageTitle(_currentStep);
                    }

                    if (!_applicantOrderCart.IsNullOrEmpty() &&
                        (_applicantOrderCart.OrderRequestType == OrderRequestType.NewOrder.GetStringValue()
                        || _applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscription.GetStringValue()))
                        (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle(Resources.Language.CREATODR);
                    //UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
                    else if (!_applicantOrderCart.IsNullOrEmpty() &&
                              _applicantOrderCart.OrderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue())
                    {
                        (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle(Resources.Language.CMPLTORDER);
                    }
                    else if (!_applicantOrderCart.IsNullOrEmpty() &&
                           _applicantOrderCart.OrderRequestType == OrderRequestType.ModifyShipping.GetStringValue())
                    {
                        (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle(Resources.Language.MODIFYSHIPPINGADDRESS);
                    }
                    else
                        (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle(Resources.Language.RENEWALORDR);

                    //base.SetPageTitle("Order Payment");
                    CurrentViewContext.OrderType = _applicantOrderCart.OrderRequestType;
                    SetButtonText();
                    //UAT-4057
                    if (pnlPaymentTypeCommon.Visible)
                    {
                        ManagePriceDivShowHidePaymentModesCommon();
                        BindPaymentInstructionsPaymentModesCommon();
                    }
                    else
                    {
                        ManagePriceDivShowHide();
                        BindPaymentInstructions();
                    }

                    BindAdditionalPaymentInstructions();  //UAT-3268
                    //UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
                    HideControlsForCompleteOrderMode();
                    if (_applicantOrderCart.IsLocationServiceTenant)
                        SkipSubmitForNewSingleCard();

                    hdnIsLocationServiceTenant.Value = _applicantOrderCart.IsLocationServiceTenant.ToString(); //UAT-3958
                }
                //backgroundpackage = _applicantOrderCart.lstApplicantOrder[0].lstPackages;
                Presenter.OnViewLoaded();
                if (CurrentViewContext.ShowRushOrder && !txtRushOrderPrice.Text.IsNullOrEmpty())// Added rush order null or empty UAT-360:WB: Compliance Order: Applicant receives error message when attempting to purchase a subscription with the price of $0.00
                {
                    chkRushOrder.Visible = true;
                    dvRushOrderSrvc.Visible = true;

                }
                else
                {
                    chkRushOrder.Visible = false;
                    chkRushOrder.Checked = false;
                    dvRushOrderSrvc.Visible = false;
                    divRushOrder.Visible = false;
                }
                //ShowHideBillingDetailsCheckboxControl();
                dvNoCompliancePackage.Visible = IsCompliancePackageSelected;
                //(this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle("Create Order");
                //UAT-4057
                if (pnlPaymentTypeCommon.Visible)
                {
                    ManageApprovalRequiredPopupPaymentModesCommon();
                }
                else
                {
                    //UAT-3958
                    ManageApprovalRequiredPopup();
                }
                if (_applicantOrderCart.IsLocationServiceTenant && IsModifyShipping && _applicantOrderCart.OrderRequestType != OrderRequestType.CompleteOrderByApplicant.GetStringValue())
                {
                    //btnPrevious.Visible = false;
                    btnBackOrderReview.Visible = false;
                    //btnCancelOrder.Visible = false;
                    dvUserAgreement.Visible = true;
                    litText.Text = Presenter.GetCreditCardAgreement();
                    SetMailingPrice(_applicantOrderCart.MailingAddress);
                    hdnCredtPymntOptnId.Value = Presenter.GetCreditCardPaymentOptionTypeId();
                    //                    rptPackages.Visible = true;
                }

                if (_applicantOrderCart.IsLocationServiceTenant && _applicantOrderCart.OrderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue() && !(_applicantOrderCart.FingerPrintData.IsPrinterAvailable) && _applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNotNull() && _applicantOrderCart.lstApplicantOrder[0].lstPackages.Count>0 && (_applicantOrderCart.lstApplicantOrder[0].lstPackages.Any(x => x.ServiceCode == BkgServiceType.FingerPrint_Card.GetStringValue()) || _applicantOrderCart.lstApplicantOrder[0].lstPackages.Any(x => x.ServiceCode == BkgServiceType.Passport_Photo.GetStringValue())))
                {
                    BusinessRuleImplementations();
                    //btnBackOrderReview.Visible = false;
                    dvUserAgreement.Visible = true;
                    litText.Text = Presenter.GetCreditCardAgreement();
                    SetMailingPrice(_applicantOrderCart.MailingAddress);
                }

                if (_applicantOrderCart.IsLocationServiceTenant && CurrentViewContext.lstPaymentOptions.Count > 1 && !_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.IsNullOrEmpty()
                                && !_applicantOrderCart.FingerPrintData.BillingCode.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.BillingCodeAmount.IsNullOrEmpty()
                                && _applicantOrderCart.FingerPrintData.BillingCodeAmount > AppConsts.NONE)
                {
                    dvCommonBalAmt.Visible = true;
                    txtPaidByInst.Text = _applicantOrderCart.lstApplicantOrder[0].lstPackages.Sum(x => x.TotalBkgPackagePrice) <= _applicantOrderCart.FingerPrintData.BillingCodeAmount ? _applicantOrderCart.lstApplicantOrder[0].lstPackages.Sum(x => x.TotalBkgPackagePrice).ToString() : _applicantOrderCart.FingerPrintData.BillingCodeAmount.ToString();
                    txtBalanceAmount.Text = (_applicantOrderCart.lstApplicantOrder[0].lstPackages.Sum(x => x.TotalBkgPackagePrice) - _applicantOrderCart.FingerPrintData.BillingCodeAmount).ToString();                    
                }
                if (_applicantOrderCart.IsLocationServiceTenant && _applicantOrderCart.lstApplicantOrder[0].lstPackages.Sum(x => x.TotalBkgPackagePrice) <= _applicantOrderCart.FingerPrintData.BillingCodeAmount)
                {
                    dvCommonPaymentSelection.Visible = false;
                    dvUserAgreement.Visible = false;
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

        #region Button Events

        protected void btnExtra_Click(object sender, EventArgs e)
        {
            try
            {
                _applicantOrderCart = GetApplicantOrderCart();

                if (Convert.ToString(_applicantOrderCart.OrderRequestType) != OrderRequestType.RenewalOrder.GetStringValue()
                    && Convert.ToString(_applicantOrderCart.OrderRequestType) != OrderRequestType.ModifyShipping.GetStringValue())
                {
                    RedirectToPendingOrder();
                }
                else if (Convert.ToString(_applicantOrderCart.OrderRequestType) == OrderRequestType.ModifyShipping.GetStringValue())
                {
                    RedirectToModifyShipping();
                }
                else
                {
                    RedirectToRenewalOrder(_applicantOrderCart);
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _applicantOrderCart = GetApplicantOrderCart();

                if (_applicantOrderCart.lstCustomFormData.IsNull())
                {
                    if (_applicantOrderCart.IsLocationServiceTenant)
                        _applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = _applicantOrderCart.IsAdditionalDocumentExist ? AppConsts.EIGHT : AppConsts.SEVEN;
                    else
                        //Commented below code for UAT-1560
                        _applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = _applicantOrderCart.IsAdditionalDocumentExist ? AppConsts.SIX : AppConsts.FIVE;
                }
                else
                {
                    if (_applicantOrderCart.IsLocationServiceTenant)
                    {
                        _applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = _applicantOrderCart.IsAdditionalDocumentExist ?
                                                                                      _applicantOrderCart.lstCustomFormData.Count + AppConsts.SEVEN :
                                                                                     _applicantOrderCart.lstCustomFormData.Count + AppConsts.FIVE;
                    }
                    else
                    {
                        //Commented below code for UAT-1560
                        _applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = _applicantOrderCart.IsAdditionalDocumentExist ?
                                                                                      _applicantOrderCart.lstCustomFormData.Count + AppConsts.SIX :
                                                                                     _applicantOrderCart.lstCustomFormData.Count + AppConsts.FOUR;
                    }
                }
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                if (_applicantOrderCart.IsLocationServiceTenant && _applicantOrderCart.OrderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue())
                {
                    if (_applicantOrderCart.FingerPrintData.IsOutOfState || _applicantOrderCart.FingerPrintData.IsEventCode)
                    {
                        queryString = new Dictionary<String, String>
                                    {
                                        { AppConsts.CHILD, ChildControls.FINGER_PRINTDATA_CONTROL},
                                        { "TenantId", _applicantOrderCart.TenantId.ToString()},
                                        {"IsFromOrderHistoryScreen",true.ToString()}

                                    };
                        String Url = String.Format("~/FingerPrintSetUp/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                        Response.Redirect(Url, true);
                    }
                    else
                    {
                       // if (_applicantOrderCart.lstApplicantOrder[0].lstPackages.Count == 1 && _applicantOrderCart.lstApplicantOrder[0].lstPackages[0].ServiceCode != "AAAR" && _applicantOrderCart.lstApplicantOrder[0].lstPackages[0].ServiceCode != "AAAQ")
                       // {
                            queryString = new Dictionary<String, String> {
                                        { AppConsts.CHILD, ChildControls.APPLICANT_APPOINTMENT_SCHEDULE},
                                         { "TenantId", CurrentViewContext.TenantId.ToString()},
                                        { "IsFromOrderHistoryScreen", true.ToString()},
                                    };
                            String Url = String.Format("~/FingerPrintSetUp/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                            Response.Redirect(Url, true);
                     //   }
                        //else
                        //{
                        //    queryString = new Dictionary<String, String>
                        //                                 {
                        //                                    { AppConsts.CHILD,  ChildControls.ModifyShipping},
                        //                                     {"OrderID", _applicantOrderCart.lstApplicantOrder[0].OrderId.ToString()},
                        //                                    {"tenantId", CurrentViewContext.TenantId.ToString()},
                        //                                    {"PageType", "OrderPayment"},
                        //                                 };
                        //    Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                        //}

                    }
                }
                else
                {

                    _applicantOrderCart.AddOrderStageTrackID(OrderStages.OrderReview);
                    queryString = new Dictionary<String, String> {
                                                                    { "Child", ChildControls.ApplicantOrderReview }
                                                                 };
                    string url = String.Format("~/ComplianceOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url);
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

        protected void btnCancel_Click(object sender, EventArgs e)
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

                Dictionary<String, String> queryString;
                if (Convert.ToString(_applicantOrderCart.OrderRequestType) == OrderRequestType.RenewalOrder.GetStringValue()
                    || Convert.ToString(_applicantOrderCart.OrderRequestType) == OrderRequestType.ChangeSubscription.GetStringValue()
                    //UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
                    || (String.Compare(_applicantOrderCart.OrderRequestType, OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) != AppConsts.NONE
                        && _applicantOrderCart.IsReadOnly))
                {
                    //change done for UAt-827 Applicant Dashboard Redesign.
                    //if (_applicantOrderCart.ParentControlType == AppConsts.DASHBOARD)
                    //{
                    //    Response.Redirect(AppConsts.DASHBOARD_URL);
                    //}
                    //else
                    //{
                    //    queryString = new Dictionary<String, String> { { AppConsts.CHILD, ChildControls.PackageSubscription } };
                    //    Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                    //}
                    Response.Redirect(AppConsts.DASHBOARD_URL);
                }
                else
                {
                    Response.Redirect("~/Main/Default.aspx");
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
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #region DropDown and CheckBox Events

        protected void chkRushOrder_CheckedChanged(object sender, EventArgs e)
        {
            _applicantOrderCart = GetApplicantOrderCart();
            Panel _pmPanel = null;
            var cmbPaymentModes = GetComplianceCombo(out _pmPanel);

            // TEMPORARY CONDITION - If no compliance package selected then do not perform anything, until the pricing is added
            if (!_applicantOrderCart.IsCompliancePackageSelected)
                return;
            //UAT-4057
            if (pnlPaymentTypeCommon.Visible)
            {
                BindPaymentInstructionsPaymentModesCommon();
            }
            else
            {
                BindPaymentInstructions();
            }
            BindAdditionalPaymentInstructions();  //UAT-3268
            if (chkRushOrder.Checked)
            {
                if (cmbPaymentModes.SelectedValue != CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString() || (cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString()
                    && CurrentViewContext.ShowRushOrderForInvioce))
                {
                    _applicantOrderCart.IsRushOrderIncluded = true;

                    //BS, UAT-264
                    Decimal _netPrice = (_applicantOrderCart.CurrentPackagePrice.Value - _applicantOrderCart.SettleAmount)
                                        + Convert.ToDecimal(_applicantOrderCart.RushOrderPrice.Trim());
                    //_applicantOrderCart.GrandTotal = (_netPrice <= AppConsts.NONE ? AppConsts.NONE : _netPrice) + GetBackgroundPackagesPrice();
                    _applicantOrderCart.GrandTotal = _netPrice <= AppConsts.NONE ? AppConsts.NONE : _netPrice;
                }

                if (_applicantOrderCart.GrandTotal == AppConsts.NONE && GetBackgroundPackagesPrice() == AppConsts.NONE)
                {
                    //txtTotalPrice.Text = Convert.ToString(AppConsts.NONE);
                    txtTotalPrice.Text = AppConsts.NONE.ToString(CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
                    ShowHideCompliancePaymentOptions(_pmPanel, cmbPaymentModes, Convert.ToDecimal(_applicantOrderCart.GrandTotal));
                }
                else if (((_applicantOrderCart.GrandTotal - Convert.ToDecimal(_applicantOrderCart.RushOrderPrice.Trim())) <= 0
                            && !CurrentViewContext.ShowRushOrderForInvioce))
                {
                    txtTotalPrice.Text = Convert.ToString((_applicantOrderCart.GrandTotal + GetBackgroundPackagesPrice()), CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
                    ShowHideCompliancePaymentOptions(_pmPanel, cmbPaymentModes, Convert.ToDecimal((_applicantOrderCart.GrandTotal - Convert.ToDecimal(_applicantOrderCart.RushOrderPrice.Trim()))));
                    //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowHidePaymentType(true);", true);

                    //Telerik.Web.UI.RadComboBoxItem invItem = cmbPaymentModes.FindItemByValue(CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString());
                    //if (invItem.IsNotNull())
                    //{
                    //    cmbPaymentModes.Items.Remove(invItem);
                    //}
                }
                else
                {
                    txtTotalPrice.Text = Convert.ToString((_applicantOrderCart.GrandTotal + GetBackgroundPackagesPrice()), CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
                    //Convert.ToString(_applicantOrderCart.GrandTotal + GetBackgroundPackagesPrice());
                    ShowHideCompliancePaymentOptions(_pmPanel, cmbPaymentModes, Convert.ToDecimal(_applicantOrderCart.GrandTotal));
                }
            }
            else
            {
                divRush.Visible = false;
                _applicantOrderCart.IsRushOrderIncluded = false;

                //_applicantOrderCart.GrandTotal = Convert.ToDecimal(_applicantOrderCart.Amount.Trim()) + GetBackgroundPackagesPrice();
                //_applicantOrderCart.GrandTotal = Convert.ToDecimal(_applicantOrderCart.Amount.Trim());

                if (_applicantOrderCart.CompliancePackagesGrandTotal == AppConsts.NONE && GetBackgroundPackagesPrice() == AppConsts.NONE)
                {
                    txtTotalPrice.Text = AppConsts.NONE.ToString(CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
                    ShowHideCompliancePaymentOptions(_pmPanel, cmbPaymentModes, Convert.ToDecimal(_applicantOrderCart.CompliancePackagesGrandTotal));
                }
                else if (_applicantOrderCart.GrandTotal > AppConsts.NONE || GetBackgroundPackagesPrice() > AppConsts.NONE)
                {
                    txtTotalPrice.Text = Convert.ToString((_applicantOrderCart.CompliancePackagesGrandTotal + GetBackgroundPackagesPrice()), CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
                    //ShowHideCompliancePaymentOptions(_pmPanel, cmbPaymentModes, Convert.ToDecimal(_applicantOrderCart.CompliancePackagesGrandTotal));
                }
            }
            SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, _applicantOrderCart);

            var _isInvoiceSelected = IsInvoiceSelected();
            if (_isInvoiceSelected)
            {
                divRush.Visible = false;
            }
            else if (!_isInvoiceSelected && chkRushOrder.Checked)
            {
                divRush.Visible = true;
            }
            Panel2.Focus();
            //ShowHideBillingDetailsCheckboxControl();
        }

        protected void cmbPaymentModes_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            // To DO
            //if (((cmbPaymentModes.SelectedValue != CurrentViewContext.PaymentMode_InvoiceId.ToString() && cmbPaymentModes.SelectedValue != CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString())
            //    || ((cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceId.ToString() || cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString()) && CurrentViewContext.ShowRushOrderForInvioce))
            //    && CurrentViewContext.ShowRushOrder && !txtRushOrderPrice.Text.IsNullOrEmpty()) // added rush order nullor empty check UAT-360:WB: Compliance Order: Applicant receives error message when attempting to purchase a subscription with the price of $0.00
            //{
            //    chkRushOrder.Visible = true;
            //    dvRushOrderSrvc.Visible = true;
            //    divRushOrder.Visible = true;
            //}
            //else
            //{
            //    chkRushOrder.Visible = false;
            //    chkRushOrder.Checked = false;
            //    dvRushOrderSrvc.Visible = false;
            //    divRushOrder.Visible = false;
            //    chkRushOrder_CheckedChanged(null, null);
            //}

            //if (!cmbPaymentModes.SelectedValue.IsNullOrEmpty())
            //{
            //    String paymeentInstruction = Presenter.GetPaymentInstruction(Convert.ToInt32(cmbPaymentModes.SelectedValue));
            //    if (paymeentInstruction.IsNotNull())
            //    {
            //        divPaymentInstruction.Visible = true;
            //        litPaymentInstruction.Text = paymeentInstruction;
            //    }
            //    else
            //    {
            //        divPaymentInstruction.Visible = false;
            //    }
            //    //UAT - 832 : Package price is displaying in order history when invoice options were used for payment
            //    if ((cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceId.ToString()) || (cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString()))
            //    {
            //        dvPrice.Visible = false;
            //        divRush.Visible = false;
            //    }
            //    else
            //    {
            //        dvPrice.Visible = true;
            //        if (chkRushOrder.Checked)
            //        {
            //            divRush.Visible = true;
            //        }
            //    }
            //}
            //Panel2.Focus();
            //ShowHideBillingDetailsCheckboxControl();
        }

        protected void _cmbPaymentModes_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var cmbPaymentModes = sender as WclComboBox;
            if (((cmbPaymentModes.SelectedValue != CurrentViewContext.PaymentMode_InvoiceId.ToString()
                && cmbPaymentModes.SelectedValue != CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString())
              || ((cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceId.ToString() || cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString()) && CurrentViewContext.ShowRushOrderForInvioce))
              && CurrentViewContext.ShowRushOrder && !txtRushOrderPrice.Text.IsNullOrEmpty()) // added rush order nullor empty check UAT-360:WB: Compliance Order: Applicant receives error message when attempting to purchase a subscription with the price of $0.00
            {
                divPaymentDetailSubContent.Visible = true;
                chkRushOrder.Visible = true;
                dvRushOrderSrvc.Visible = true;
                divRushOrder.Visible = true;
            }
            else
            {
                chkRushOrder.Visible = false;
                chkRushOrder.Checked = false;
                dvRushOrderSrvc.Visible = false;
                divRushOrder.Visible = false;
                if ((cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceId.ToString() || cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString()))
                {
                    divPaymentDetailSubContent.Visible = false;
                }
                else
                {
                    divPaymentDetailSubContent.Visible = true;
                }
                //divPaymentDetailSubContent.Visible = false;
                chkRushOrder_CheckedChanged(null, null);
            }

            if (!cmbPaymentModes.SelectedValue.IsNullOrEmpty() && cmbPaymentModes.SelectedValue != AppConsts.ZERO)
            {
                //UAT - 832 : Package price is displaying in order history when invoice options were used for payment
                //if ((cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceId.ToString()) || (cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString()))
                //   {
                //    dvPrice.Visible = false;
                //    divRush.Visible = false;
                //}
                //else
                //{
                //    dvPrice.Visible = true;
                //    if (chkRushOrder.Checked)
                //    {
                //        divRush.Visible = true;
                //    }
                //}
                ManagePriceDivShowHide();
            }
            Panel2.Focus();
            BindPaymentInstructions();
            BindAdditionalPaymentInstructions();  //UAT-3268

        }

        #region UAT-4057
        protected void cmbPaymentModesCommon_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var cmbPaymentModesCommon = sender as WclComboBox;
            if (((cmbPaymentModesCommon.SelectedValue != CurrentViewContext.PaymentMode_InvoiceId.ToString() && cmbPaymentModesCommon.SelectedValue != CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString())
              || ((cmbPaymentModesCommon.SelectedValue == CurrentViewContext.PaymentMode_InvoiceId.ToString() || cmbPaymentModesCommon.SelectedValue == CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString()) && CurrentViewContext.ShowRushOrderForInvioce))
              && CurrentViewContext.ShowRushOrder && !txtRushOrderPrice.Text.IsNullOrEmpty()) // added rush order nullor empty check UAT-360:WB: Compliance Order: Applicant receives error message when attempting to purchase a subscription with the price of $0.00
            {
                divPaymentDetailSubContent.Visible = true;
                chkRushOrder.Visible = true;
                dvRushOrderSrvc.Visible = true;
                divRushOrder.Visible = true;
            }
            else
            {
                chkRushOrder.Visible = false;
                chkRushOrder.Checked = false;
                dvRushOrderSrvc.Visible = false;
                divRushOrder.Visible = false;
                if ((cmbPaymentModesCommon.SelectedValue == CurrentViewContext.PaymentMode_InvoiceId.ToString() || cmbPaymentModesCommon.SelectedValue == CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString()))
                //if (cmbPaymentModesCommon.SelectedValue == CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString())
                {
                    divPaymentDetailSubContent.Visible = false;
                }
                else
                {
                    divPaymentDetailSubContent.Visible = true;
                }
                chkRushOrder_CheckedChanged(null, null);
            }

            if (!cmbPaymentModesCommon.SelectedValue.IsNullOrEmpty() && cmbPaymentModesCommon.SelectedValue != AppConsts.ZERO)
            {
                ShowHideCommonPaymentType();
                //ManagePriceDivShowHidePaymentModesCommon();
            }
            Panel2.Focus();
            //BindPaymentInstructionsPaymentModesCommon();
            BindAdditionalPaymentInstructions();  //UAT-3268

        }

        protected void chkSplitPaymentTypeByPkg_CheckedChanged(object sender, EventArgs e)
        {
            ShowHideCommonPaymentType();
            //if (chkSplitPaymentTypeByPkg.Checked)
            //{
            //    pnlPaymentTypeCommon.Visible = false;

            //    foreach (RepeaterItem item in rptPackages.Items)
            //    {
            //        Panel pnlPaymentType = ((item.FindControl("pnlPaymentType") as Panel));
            //        if (!pnlPaymentType.IsNullOrEmpty())
            //            pnlPaymentType.Visible = true;
            //        WclComboBox _cmbPaymentModes = ((item.FindControl("cmbPaymentModes") as WclComboBox));
            //        if (!_cmbPaymentModes.IsNullOrEmpty())
            //            _cmbPaymentModes.Visible = true;

            //    }
            //    ManagePriceDivShowHide();
            //    BindPaymentInstructions();
            //    BusinessRuleImplementations();
            //}
            //else
            //{
            //    pnlPaymentTypeCommon.Visible = true;
            //    foreach (RepeaterItem item in rptPackages.Items)
            //    {
            //        Panel pnlPaymentType = ((item.FindControl("pnlPaymentType") as Panel));
            //        if (!pnlPaymentType.IsNullOrEmpty())
            //            pnlPaymentType.Visible = false;
            //        WclComboBox _cmbPaymentModes = ((item.FindControl("cmbPaymentModes") as WclComboBox));
            //        if (!_cmbPaymentModes.IsNullOrEmpty())
            //            _cmbPaymentModes.Visible = false;

            //    }
            //    ManagePriceDivShowHidePaymentModesCommon();
            //    BindPaymentInstructionsPaymentModesCommon();
            //}
        }
        #endregion

        #endregion

        #region Repeaters' Events

        protected void rptPackages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var _cmbPaymentModes = e.Item.FindControl("cmbPaymentModes") as WclComboBox;
            var _pmPanel = e.Item.FindControl("pnlPaymentType") as Panel;
            var _hdfPkgId = e.Item.FindControl("hdfPkgId") as HiddenField;
            var _hdfIsBkgPkg = e.Item.FindControl("hdfIsBkgPkg") as HiddenField;

            var dvPackageName = e.Item.FindControl("dvPackageName") as HtmlGenericControl;
            // var dvOrderSelection = e.Item.FindControl("dvOrderSelection") as HtmlGenericControl;
            #region UAT-3601
            dvPackageName.Style.Add("display", "block");
            dvPackageName.Attributes["title"] = Resources.Language.PKGNAME;
            var spnPackageName = e.Item.FindControl("spnPackageName") as HtmlGenericControl;
            //spnPackageName.InnerText = CurrentViewContext.PackageNameLabel.IsNullOrEmpty() ? AppConsts.PAYMENT_METHOD_PACKAGE_NAME_DEFAULT_LABEL : CurrentViewContext.PackageNameLabel;
            spnPackageName.InnerText = CurrentViewContext.PackageNameLabel.IsNullOrEmpty() ? Resources.Language.PKGNAME : CurrentViewContext.PackageNameLabel;
            #endregion

            //UAT-3850
            var _pnlPaymentByInst = e.Item.FindControl("pnlPaymentByInst") as Panel;
            var _pnlBalanceAmt = e.Item.FindControl("pnlBalanceAmt") as Panel;
            var _dvPaymentByInst = e.Item.FindControl("dvPaymentByInst") as HtmlGenericControl;
            var _dvBalanceAmt = e.Item.FindControl("dvBalanceAmt") as HtmlGenericControl;
            //if (_applicantOrderCart.IsLocationServiceTenant)
            //{
            //    dvPackageName.Style.Add("display", "none");
            //    //    dvOrderSelection.Style.Add("display", "block");
            //}
            //else
            //{
            //    //  dvOrderSelection.Style.Add("display", "none");
            //    dvPackageName.Style.Add("display", "block");
            //}

            string paymentOptions = String.Empty;

            if (_cmbPaymentModes.IsNotNull() && _hdfPkgId.IsNotNull() && _hdfIsBkgPkg.IsNotNull())
            {
                var _pkgId = Convert.ToInt32(_hdfPkgId.Value);

                var _bkgPkgs = _applicantOrderCart.lstApplicantOrder[0].lstPackages;

                Decimal _price = 0;
                //UAT-3850
                Decimal _pricePaidByInst = 0;
                var _isBkgPkg = Convert.ToBoolean(_hdfIsBkgPkg.Value);
                if (_isBkgPkg)
                {
                    _price = _bkgPkgs.First(bp => bp.BPAId == _pkgId).TotalBkgPackagePrice;
                }
                else
                {
                    //_price = this.CompliancePkgPrice;
                    // UAT 1185 Changes
                    foreach (var cmpPkg in _applicantOrderCart.CompliancePackages)
                    {
                        var _crntPkg = cmpPkg.Value;
                        if (_crntPkg.CompliancePackageID == _pkgId)
                        {
                            _price = Convert.ToDecimal(_crntPkg.GrandTotal);
                            break;
                        }
                    }
                }

                if (_price == 0)
                {
                    _cmbPaymentModes.Visible = false;
                    _pmPanel.Visible = false;
                    //if (Convert.ToBoolean(_hdfIsBkgPkg.Value))
                    //    return;
                }

                //UAT-3850
                if (IsFingerprintSplitPaymentSCenario())
                {
                    var _cmbPaymentModeBalanceAmt = e.Item.FindControl("cmbPaymentModeBalanceAmt") as WclComboBox;
                    var _txtPaidByInst = e.Item.FindControl("txtPaidByInst") as WclNumericTextBox;
                    var _txtBalanceAmount = e.Item.FindControl("txtBalanceAmount") as WclNumericTextBox;
                    var _dvBalanceAmount = e.Item.FindControl("dvBalanceAmount") as HtmlGenericControl;

                    _pmPanel.Visible = false;
                    _cmbPaymentModes.Visible = false;
                    _dvPaymentByInst.Style.Add("display", "block");
                    _dvBalanceAmt.Style.Add("display", "block");
                    _pnlPaymentByInst.Visible = true;
                    _pnlBalanceAmt.Visible = true;
                    hdnIsPaymentByInst.Value = "true";


                    _pricePaidByInst = _applicantOrderCart.FingerPrintData.BillingCodeAmount;

                    if (_price <= _pricePaidByInst)
                    {
                        _txtPaidByInst.Text = Convert.ToString(_price, CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
                        _txtBalanceAmount.Text = Convert.ToString(AppConsts.NONE, CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
                        _cmbPaymentModeBalanceAmt.Visible = false;
                        _dvBalanceAmount.Visible = false;
                    }
                    else
                    {
                        _txtPaidByInst.Text = Convert.ToString(_pricePaidByInst, CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
                        _txtBalanceAmount.Text = Convert.ToString(_price - _pricePaidByInst, CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
                    }

                    Int32 PaymentMode_InvoiceWdoutApprvlId = CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId; // for payment done by institution.
                    hdnPaymentByInstID.Value = PaymentMode_InvoiceWdoutApprvlId.ToString();

                    var _options = CurrentViewContext.lstPaymentOptions.Where(po => po.PkgId == _pkgId && po.IsBkgPkg == _isBkgPkg)
                                                    .Select(pos => pos.lstPaymentOptions).ToList();
                    //pkgList = CurrentViewContext.lstPaymentOptions;
                    _cmbPaymentModeBalanceAmt.DataSource = _options.FirstOrDefault();

                    _cmbPaymentModeBalanceAmt.DataTextField = "PaymentOptionName";
                    _cmbPaymentModeBalanceAmt.DataValueField = "PaymentOptionId";
                    _cmbPaymentModeBalanceAmt.DataBind();
                    _cmbPaymentModeBalanceAmt.AutoPostBack = true;
                    _cmbPaymentModeBalanceAmt.CausesValidation = false;
                }

                else
                {
                    var _options = CurrentViewContext.lstPaymentOptions.Where(po => po.PkgId == _pkgId && po.IsBkgPkg == _isBkgPkg)
                                                    .Select(pos => pos.lstPaymentOptions).ToList();
                    //pkgList = CurrentViewContext.lstPaymentOptions;
                    _cmbPaymentModes.DataSource = _options.FirstOrDefault();

                    _cmbPaymentModes.DataTextField = "PaymentOptionName";
                    _cmbPaymentModes.DataValueField = "PaymentOptionId";
                    _cmbPaymentModes.DataBind();
                    _cmbPaymentModes.AutoPostBack = true;
                    _cmbPaymentModes.CausesValidation = false;


                    if (!_cmbPaymentModes.Visible)
                    {
                        if (!_fingerPrintAppointmentData.IsNullOrEmpty() && !_fingerPrintAppointmentData.BillingCode.IsNullOrEmpty())
                        {
                            _cmbPaymentModes.SelectedValue = Convert.ToString(CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId);
                            //UAT-3850
                            _cmbPaymentModes.Visible = false;
                        }
                        else
                        {
                            _cmbPaymentModes.SelectedValue = Convert.ToString(CurrentViewContext.PaymentMode_CreditCardId);
                        }

                    }

                    //UAT-4057
                    if (CurrentViewContext.IsAllPkgsPaymentOptionSame == true &&
                        (!_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.IsLocationServiceTenant))
                    {
                        _cmbPaymentModes.Visible = false;
                        _pmPanel.Visible = false;
                    }

                    //else
                    //{
                    //    _cmbPaymentModes.Visible = true;
                    //    _pmPanel.Visible = true;
                    //}
                }

                //For CBI User
                if (CurrentViewContext.IsAllPkgsPaymentOptionSame == true &&
                    (!_applicantOrderCart.IsNullOrEmpty() && _applicantOrderCart.IsLocationServiceTenant))
                {
                    _cmbPaymentModes.Visible = false;
                    _pmPanel.Visible = false;
                    dvSplitPaymentTypeByPkg.Visible = false;

                    //UAT-5029
                    if ((_applicantOrderCart.FingerPrintData.BillingCodeAmount > AppConsts.NONE) && _applicantOrderCart.IsModifyShipping != true)
                    {
                        hdnIsPaymentByInst.Value = "true";
                        Int32 PaymentMode_InvoiceWdoutApprvlId = CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId; // for payment done by institution.
                        hdnPaymentByInstID.Value = PaymentMode_InvoiceWdoutApprvlId.ToString();
                    }

                }
            }

            ////UAT-3268
            //System.Web.UI.HtmlControls.HtmlGenericControl dvBasePrice = e.Item.FindControl("dvBasePrice") as System.Web.UI.HtmlControls.HtmlGenericControl;
            //System.Web.UI.HtmlControls.HtmlGenericControl dvAdditionalPrice = e.Item.FindControl("dvAdditionalPrice") as System.Web.UI.HtmlControls.HtmlGenericControl;
            //if (!CurrentViewContext.lstRotationQualifyingBkgPkgs.IsNullOrEmpty()
            //    && CurrentViewContext.lstRotationQualifyingBkgPkgs.Where(cond => cond.BPAId == Convert.ToInt32(_hdfPkgId.Value)).Select(sel => sel.IsReqToQualifyInRotation).FirstOrDefault())
            //{
            //    dvBasePrice.Visible = true;
            //    dvAdditionalPrice.Visible = true;
            //}
            //else
            //{
            //    dvBasePrice.Visible = false;
            //    dvAdditionalPrice.Visible = false;
            //}
        }

        private bool IsFingerprintSplitPaymentSCenario()
        {
            if (_applicantOrderCart.IsLocationServiceTenant && CurrentViewContext.lstPaymentOptions.Count > 1 && !_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.IsNullOrEmpty()
                                && !_applicantOrderCart.FingerPrintData.BillingCode.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.BillingCodeAmount.IsNullOrEmpty()
                                && _applicantOrderCart.FingerPrintData.BillingCodeAmount > AppConsts.NONE)
            {
                return false;
            }
            else
            {
                return !_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.IsNullOrEmpty()
                                  && !_applicantOrderCart.FingerPrintData.BillingCode.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.BillingCodeAmount.IsNullOrEmpty()
                                  && _applicantOrderCart.FingerPrintData.BillingCodeAmount > AppConsts.NONE;
            }
        }

        protected void rptrAdtnlPayment_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        #endregion

        #region Method

        #region Public
        public void SetMailingPrice(PreviousAddressContract mailingAddress)
        {
            if (_applicantOrderCart.IsMailingOptionUpgraded)
            {
                txtTotalPrice.Text = _applicantOrderCart.MailingPrice.ToString();
                //_applicantOrderCart.MailingPrice = Convert.ToDecimal(mailingAddress.MailingOptionPrice);
            }
            else
            {
                Decimal mailingPrice = Convert.ToDecimal(0.00);
                List<String> str = Presenter.GetServiceStatus(Convert.ToInt32(_applicantOrderCart.lstApplicantOrder[0].OrderId), CurrentUserId);
                var _count = 0;
                if (str.Any() && str.IsNotNull())
                {
                    foreach (var item in str)
                    {
                        if (item == CABSServiceStatus.RETURNED_TO_SENDER.ToString())
                        {
                            _count = _count + 1;
                        }
                    }
                }

                char[] splitParams = new char[] { '(', ')' };

                if (!mailingAddress.IsNullOrEmpty() || !mailingAddress.MailingOptionPrice.IsNullOrEmpty())
                {
                    String[] MailingPrice1 = mailingAddress.MailingOptionPrice.Split(splitParams);
                    mailingPrice = Convert.ToDecimal(MailingPrice1[1]);
                }

                if (_count > 1)
                {
                    mailingPrice = mailingPrice * _count;
                }


                if (_applicantOrderCart.OrderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue())
                {

                    txtTotalPrice.Text = Convert.ToDecimal(txtTotalPrice.Text).ToString();
                    _applicantOrderCart.MailingPrice = mailingPrice;
                }
                else {
                    txtTotalPrice.Text = mailingPrice.ToString();
                    _applicantOrderCart.MailingPrice = mailingPrice;
                }

                mailingAddress.MailingOptionPriceOnly = _applicantOrderCart.MailingPrice;


            }


        }
        #endregion

        #region Private

        private void CheckOrder()
        {
            _applicantOrderCart = GetApplicantOrderCart();
            RedirectIfIncorrectOrderStage(_applicantOrderCart);

            // This condition handle the case when applicant uses any offline payment mode and Press Browser back
            if (_applicantOrderCart.lstApplicantOrder[0].OrderId != AppConsts.NONE
               && _applicantOrderCart.OrderRequestType != OrderRequestType.CompleteOrderByApplicant.GetStringValue()
               && _applicantOrderCart.OrderRequestType != OrderRequestType.ModifyShipping.GetStringValue())
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

            Dictionary<String, String> queryString = new Dictionary<string, string>();

            var _invNums = _applicantOrderCart.InvoiceNumber;
            if (!_invNums.IsNullOrEmpty())
            {
                if (_invNums.Keys.Contains(PaymentOptions.Paypal.GetStringValue()))
                {
                    var _opt = Presenter.GetOnlinePayTransactionByInvNum(_invNums[PaymentOptions.Paypal.GetStringValue()]);
                    if (_opt.OrderPaymentDetails.First().lkpOrderStatu.Code == ApplicantOrderStatus.Paid.GetStringValue())
                    {
                        CheckOnlinePayment();
                    }
                }
                else
                    RedirectToPendingOrder();
            }
            else
            {
                RedirectToPendingOrder();
            }

            //if (orderStatus == ApplicantOrderStatus.Paid.GetStringValue())
            //{
            //    CheckOnlinePayment();
            //}
            //else if (orderStatus == ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue())
            //{
            //    RedirectToPendingOrder();
            //}
        }

        private void CheckOnlinePayment()
        {
            try
            {
                BaseUserControl.LogOrderFlowSteps("OrderPayment.ascx STEP - 1.1: Redirecting to Order Confirmation after successful Paypal order for Invoice Number(s): " + _applicantOrderCart.InvoiceNumber);
                //ErrorLog logFile = new ErrorLog("Data is sent from OrderReview page.");
                RedirectToOrderConfirmation();
            }
            catch (Exception ex)
            {
                // Exception can only occur due to Response.Redirect
                BaseUserControl.LogOrderFlowSteps("OrderPayment.ascx STEP - 1.2: Exception occured for Invoice Number(s): " + _applicantOrderCart.InvoiceNumber + ". Exception is: " + Convert.ToString(ex));
                //ErrorLog logFile = new ErrorLog("Problem in sending data from OrderReview page" + ex);
            }
        }

        private void RedirectToOrderConfirmation()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.CHILD,  ChildControls.ApplicantOrderConfirmation}
                                                                 };
            Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

        }

        /// <summary>
        /// Checks the order status track. If this page is not opened as per correct order status track then, redirected to correct order.
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        private void RedirectIfIncorrectOrderStage(ApplicantOrderCart applicantOrderCart)
        {
            CurrentViewContext.NextPagePath = Presenter.GetNextPagePathByOrderStageID(applicantOrderCart);

            //Redirect to next page path if Order Status track is not correct.
            if (CurrentViewContext.NextPagePath.IsNotNull())
            {
                Response.Redirect(CurrentViewContext.NextPagePath);
            }
            else
            {
                applicantOrderCart.AddOrderStageTrackID(OrderStages.OrderPayment);
            }
        }

        private void RedirectToRenewalOrder(ApplicantOrderCart applicantOrderCart)
        {
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
            //UAT-1560
            Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);
            Dictionary<String, String> queryString = new Dictionary<String, String>()
                                                         {
                                                            {"OrderId",applicantOrderCart.PrevOrderId.ToString()},
                                                            { "Child",  ChildControls.RenewalOrder}
                                                         };

            Response.Redirect(String.Format("Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        }

        private void RedirectToPendingOrder()
        {
            _applicantOrderCart.ClearOrderCart(_applicantOrderCart);
            Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,  ChildControls.ApplicantPendingOrder}
                                                                 };
            Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        }

        private void RedirectToModifyShipping()
        {
            _applicantOrderCart = GetApplicantOrderCart();
            bool IsCompleteYourPayment = false;
            var serviceStatus = Presenter.GetServiceStatus(_applicantOrderCart.lstApplicantOrder[0].OrderId, CurrentUserId);

            if (serviceStatus != null && serviceStatus[0] == CABSServiceStatus.RETURNED_TO_SENDER.GetStringValue() && _applicantOrderCart.IsModifyShippingPayment)
                IsCompleteYourPayment = true;
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                         {
                                                            { AppConsts.CHILD,  @"~/ComplianceOperations/UserControl/ModifyShippingInfo.ascx"},
                                                             {"OrderID", _applicantOrderCart.lstApplicantOrder[0].OrderId.ToString()},
                                        {"tenantId",CurrentViewContext.TenantId.ToString()},
                                        {"IsFromNewOrderClick",_applicantOrderCart.IsFromNewOrderClick.ToString()},
                                        {"PageType", "ModifyShipping"},
                                        {"ModifyShippingPayment", _applicantOrderCart.IsModifyShippingPayment.ToString()},
                                        {"IsCompleteYourPayment",IsCompleteYourPayment.ToString()}
                                                         };
            String url = String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }

        private ApplicantOrderCart GetApplicantOrderCart()
        {
            if (_applicantOrderCart.IsNull())
            {
                _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            }
            return _applicantOrderCart;
        }

        private FingerPrintAppointmentContract GetFingerPrintAppointmentData()
        {
            if (_fingerPrintAppointmentData.IsNull())
            {
                _fingerPrintAppointmentData = (FingerPrintAppointmentContract)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_LOCATION_CART);
            }
            return _fingerPrintAppointmentData;
        }

        private void BusinessRuleImplementations()
        {
            //To Do: Need to change after integration
            foreach (var applicantOrder in _applicantOrderCart.lstApplicantOrder)
            {
                if (!applicantOrder.DPPS_Id.IsNullOrEmpty()) // Case when no Compliance Package was selected
                    CurrentViewContext.DPPSId = applicantOrder.DPPS_Id.FirstOrDefault();
            }


            txtRushOrderPrice.Text = _applicantOrderCart.RushOrderPrice;
            //txtTotalPrice.Text = Convert.ToString(_applicantOrderCart.GrandTotal);

            this.CompliancePkgPrice = Convert.ToDecimal(_applicantOrderCart.CompliancePackagesGrandTotal);

            if (!_applicantOrderCart.IsCompliancePackageSelected) // Case when only Backgriound package was selected
                txtTotalPrice.Text = Convert.ToString(GetBackgroundPackagesPrice(), CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
            else
                txtTotalPrice.Text = Convert.ToString((_applicantOrderCart.CompliancePackagesGrandTotal + GetBackgroundPackagesPrice()), CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));

            chkRushOrder.Checked = _applicantOrderCart.IsRushOrderIncluded;
            divRush.Visible = _applicantOrderCart.IsRushOrderIncluded;
            hdnCredtPymntOptnId.Value = Presenter.GetCreditCardPaymentOptionTypeId();
            //Hide the Payment Type Dropdown when Grand Total is equal to $0.00
            //if (_applicantOrderCart.GrandTotal == AppConsts.NONE)
            //UAT-360:WB: Compliance Order: Applicant receives error message when attempting to purchase a subscription with the price of $0.00
            //if (Convert.ToDecimal(txtTotalPrice.Text.Trim()) == Convert.ToDecimal(AppConsts.NONE))
            //{

            //Check the Payment Option for the Current Package
            //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowHidePaymentType(false);", true);
            // _applicantOrderCart.HidePaymntInstruction = true;
            //  SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, _applicantOrderCart);
            //}

            if (!_applicantOrderCart.IsLocationServiceTenant) // Added this check to fix issue - UAT-4876||Bug ID: 25165
            {
                //Hide Rush Order Services if Payment Type is only Invoice
                // UAT 926 implmentation -- Check for only the Compliance Payment Combobox
                Panel cmpPnl = null;
                var _complianceCombo = GetComplianceCombo(out cmpPnl);
                if (_complianceCombo.IsNotNull())
                {
                    var _lstCmpPaymentOptions = _complianceCombo.DataSource as List<PkgPaymentOptions>;
                    //if (CurrentViewContext.lstPaymentOptions.IsNotNull() && CurrentViewContext.lstPaymentOptions.Count() == AppConsts.ONE
                    //    && CurrentViewContext.lstPaymentOptions.Any(cond => cond.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()) && !CurrentViewContext.ShowRushOrderForInvioce)
                    if (_complianceCombo.IsNotNull() && _lstCmpPaymentOptions.IsNotNull() && _lstCmpPaymentOptions.Count() == AppConsts.ONE
                       && _lstCmpPaymentOptions.Any(po => po.PaymentOptionCode == PaymentOptions.InvoiceWithApproval.GetStringValue())
                       && !CurrentViewContext.ShowRushOrderForInvioce)
                    {
                        divRushOrder.Visible = false;
                        divRush.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the price of all the background packages selected
        /// </summary>
        /// <returns></returns>
        private Decimal GetBackgroundPackagesPrice()
        {
            Decimal _backgroundPackagesPrice = 0;

            if (!_applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                foreach (var bkgPackage in _applicantOrderCart.lstApplicantOrder[0].lstPackages)
                {
                    //_backgroundPackagesPrice += bkgPackage.BasePrice; // Price from Pricing SP is to be displayed
                    _backgroundPackagesPrice += bkgPackage.TotalBkgPackagePrice;
                    //UAT-3268
                    //if (bkgPackage.IsReqToQualifyInRotation && !bkgPackage.AdditionalPrice.IsNullOrEmpty() && bkgPackage.AdditionalPrice > AppConsts.NONE)
                    //{
                    //    _backgroundPackagesPrice += Convert.ToDecimal(bkgPackage.AdditionalPrice);
                    //}
                }
            }
            return _backgroundPackagesPrice;
        }

        /// <summary>
        /// Set the button Text for 'Previous', 'Next' or 'Restart' etc, based on the type of Order
        /// </summary>
        private void SetButtonText()
        {
            WclButton _btnPrevious = cmdbarSubmit.ExtraCommandButtons.FirstOrDefault(btn => btn.ID == "btnBackOrderReview");
            WclButton _btnRestart = cmdbarSubmit.ExtraCommandButtons.FirstOrDefault(btn => btn.ID == "btnPrevious");
            WclButton _btnSubmit = cmdbarSubmit.ExtraCommandButtons.FirstOrDefault(btn => btn.ID == "btnSubmitOrder");
            //UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
            WclButton btnCancelOrder = cmdbarSubmit.ExtraCommandButtons.FirstOrDefault(btn => btn.ID == "btnCancelOrder");


            if (CurrentViewContext.OrderType == OrderRequestType.NewOrder.GetStringValue())
            {
                _btnRestart.Visible = false;
                //_btnPrevious.Text = AppConsts.PREVIOUS_BUTTON_TEXT;
                //_btnSubmit.Text = AppConsts.NEXT_BUTTON_TEXT;
                _btnPrevious.Text = Resources.Language.PREVIOUS;
                _btnSubmit.Text = Resources.Language.NEXT;
            }
            //UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
            else if (CurrentViewContext.OrderType == OrderRequestType.CompleteOrderByApplicant.GetStringValue())
            {
                _btnRestart.Visible = false;
                //_btnPrevious.Text = AppConsts.PREVIOUS_BUTTON_TEXT;
                //_btnSubmit.Text = AppConsts.NEXT_BUTTON_TEXT;
                _btnPrevious.Text = Resources.Language.PREVIOUS;
                _btnSubmit.Text = Resources.Language.NEXT;
                btnCancelOrder.Text = Resources.Language.CNCL;
            }
            else if (CurrentViewContext.OrderType == OrderRequestType.ModifyShipping.GetStringValue())
            {
                _btnSubmit.Text = Resources.Language.NEXT;
                _btnRestart.Text = Resources.Language.PREVIOUS;
            }
            else
            {
                _btnRestart.Visible = true;
                //_btnPrevious.Text = AppConsts.PREVIOUS_BUTTON_TEXT;
                //_btnSubmit.Text = AppConsts.NEXT_BUTTON_TEXT;
                _btnPrevious.Text = Resources.Language.PREVIOUS;
                _btnSubmit.Text = Resources.Language.NEXT;
            }
        }

        /// <summary>
        /// Generate the CSV's of the PBHMIds for input to fetch the Package Payment Options
        /// </summary>
        /// <returns></returns>
        private String GeneratePkgIdString()
        {
            var _pkgs = _applicantOrderCart.lstApplicantOrder[0].lstPackages;
            if (_pkgs.IsNullOrEmpty())
                return String.Empty;
            var _pkgNodeMappingIds = String.Empty;

            StringBuilder _sb = new StringBuilder();
            foreach (var pkg in _pkgs)
                _sb.Append(pkg.BPHMId + ",");

            if (_sb.Length > 0)
            {
                _pkgNodeMappingIds = Convert.ToString(_sb);
                _pkgNodeMappingIds = _pkgNodeMappingIds.Substring(0, _pkgNodeMappingIds.LastIndexOf(','));
            }
            return _pkgNodeMappingIds;
        }

        /// <summary>
        /// On page load this will work normally.
        /// On checkbox check changed, it will override to check the Net price and than show hide the panel and combobox
        /// </summary>
        /// <param name="cmpPnl"></param>
        /// <returns></returns>
        private WclComboBox GetComplianceCombo(out Panel cmpPnl)
        {

            WclComboBox _cmb = null;
            cmpPnl = null;
            if (IsFingerprintSplitPaymentSCenario()) return _cmb;
            foreach (RepeaterItem rptItem in rptPackages.Items)
            {
                Decimal _price = 0;
                var _hdfIsBkgPkg = rptItem.FindControl("hdfIsBkgPkg") as HiddenField;
                var _hdfPkgId = rptItem.FindControl("hdfPkgId") as HiddenField;
                var _pkgId = Convert.ToInt32(_hdfPkgId.Value);

                if (_hdfIsBkgPkg.IsNotNull() && !Convert.ToBoolean(_hdfIsBkgPkg.Value))
                {
                    //UAT-3779
                    var _pmPanel = cmpPnl = rptItem.FindControl("pnlPaymentType") as Panel;
                    _cmb = rptItem.FindControl("cmbPaymentModes") as WclComboBox;
                    foreach (var cmpPkg in _applicantOrderCart.CompliancePackages)
                    {
                        var _crntPkg = cmpPkg.Value;
                        if (_crntPkg.CompliancePackageID == _pkgId)
                        {
                            _price = Convert.ToDecimal(_crntPkg.GrandTotal);
                            break;
                        }
                    }
                    ShowHideCompliancePaymentOptions(_pmPanel, _cmb, _price);
                }
                else
                {

                    var _pmPanel = cmpPnl = rptItem.FindControl("pnlPaymentType") as Panel;
                    _cmb = rptItem.FindControl("cmbPaymentModes") as WclComboBox;

                    if (!_applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
                    {
                        foreach (var bkgPackage in _applicantOrderCart.lstApplicantOrder[0].lstPackages)
                        {
                            var _crntPkg = bkgPackage.BPAId;
                            if (_crntPkg == _pkgId)
                            {
                                _price = bkgPackage.TotalBkgPackagePrice;
                                break;
                            }
                        }
                    }
                    ShowHideCompliancePaymentOptions(_pmPanel, _cmb, _price);
                }
            }
            return _cmb;
        }

        /// <summary>
        /// Bind the Package level payment Options
        /// </summary>
        private void BindPaymentOptions()
        {
            var BillingCode = String.Empty;
            Decimal BillingCodeAmount = AppConsts.NONE; //UAT-3850
            if (!_fingerPrintAppointmentData.IsNullOrEmpty())
            {
                BillingCode = _fingerPrintAppointmentData.BillingCode;
                //UAT-3850
                BillingCodeAmount = _fingerPrintAppointmentData.BillingCodeAmount;
            }

            Presenter.GetPkgPaymentOptions(GenerateCompliancePkgIdString(),
                                               GeneratePkgIdString(),
                                               Convert.ToInt32(_applicantOrderCart.SelectedHierarchyNodeID),
                                               BillingCode, BillingCodeAmount
                                                       );
            //UAT-3268
            GetRotationQualifyingBkgPkgs();
            if (!CurrentViewContext.lstRotationQualifyingBkgPkgs.IsNullOrEmpty())
            {
                List<PkgAdditionalPaymentInfo> lstAdditionalPaymentOptions = Presenter.GetAdditionalPriceData();
                foreach (var Pkgs in CurrentViewContext.lstPaymentOptions)
                {
                    Pkgs.BasePrice = lstAdditionalPaymentOptions.Where(cond => cond.PackageID == Pkgs.PkgId).Select(sel => sel.BasePrice).FirstOrDefault();
                    Pkgs.AdditionalPaymentOption = lstAdditionalPaymentOptions.Where(cond => cond.PackageID == Pkgs.PkgId).Select(sel => sel.AdditionalPaymentOption).FirstOrDefault();
                    Pkgs.AdditionalPrice = lstAdditionalPaymentOptions.Where(cond => cond.PackageID == Pkgs.PkgId).Select(sel => sel.AdditionalPrice).FirstOrDefault();
                    Pkgs.AdditionalPaymentOptionID = lstAdditionalPaymentOptions.Where(cond => cond.PackageID == Pkgs.PkgId).Select(sel => sel.AdditionalPaymentOptionID).FirstOrDefault();
                }
            }

            //UAT-4057
            CheckIfCommonPaymentOptionToUseForAllPackages();

            //if (_applicantOrderCart.IsLocationServiceTenant && !BillingCode.IsNullOrEmpty() && !BillingCodeAmount.IsNullOrEmpty())
            //{
            //    if (CurrentViewContext.lstPaymentOptions.Count > 1)
            //    {
            //        var PkgName = string.Join(", ", from item in CurrentViewContext.lstPaymentOptions select item.PkgName);
            //        CurrentViewContext.lstPaymentOptions[0].PkgName = PkgName;
            //        //CurrentViewContext.lstPaymentOptions = CurrentViewContext.lstPaymentOptions.FirstOrDefault();
            //        CurrentViewContext.lstPaymentOptions.RemoveRange(1, CurrentViewContext.lstPaymentOptions.Count - 1);
            //    }
            //}

            rptPackages.DataSource = CurrentViewContext.lstPaymentOptions;
            rptPackages.DataBind();
        }

        private string GenerateCompliancePkgIdString()
        {
            var _pkgs = _applicantOrderCart.CompliancePackages.Values;
            if (_pkgs.IsNullOrEmpty())
                return String.Empty;

            var _pkgNodeMappingIds = String.Empty;

            StringBuilder _sb = new StringBuilder();
            foreach (var pkg in _pkgs)
                if (pkg.DPP_Id.IsNotNull() && pkg.DPP_Id > AppConsts.NONE)
                    _sb.Append(pkg.DPP_Id + ",");

            if (_sb.Length > 0)
            {
                _pkgNodeMappingIds = Convert.ToString(_sb);
                _pkgNodeMappingIds = _pkgNodeMappingIds.Substring(0, _pkgNodeMappingIds.LastIndexOf(','));
            }
            return _pkgNodeMappingIds;
        }
        /// <summary>
        /// Bind the Instructions for the Payment Mode selected
        /// </summary>
        /// <param name="cmbPaymentModes"></param>
        private void BindPaymentInstructions()
        {
            List<Entity.ClientEntity.lkpPaymentOption> lstClientPaymentOptns = new List<Entity.ClientEntity.lkpPaymentOption>();
            List<Entity.lkpPaymentOption> _lstMasterPaymentOptns = Presenter.GetMasterPaymentSettings(out lstClientPaymentOptns);
            List<String> lstPaymentOptnCode = new List<String>();

            foreach (RepeaterItem item in rptPackages.Items)
            {
                var _combo = (item.FindControl("cmbPaymentModes") as WclComboBox);
                var _cmbPaymentModeBalanceAmt = item.FindControl("cmbPaymentModeBalanceAmt") as WclComboBox; //UAT-3850
                if (_combo.Visible)
                {
                    var _selectedValue = (item.FindControl("cmbPaymentModes") as WclComboBox).SelectedValue;

                    if (!_selectedValue.IsNullOrEmpty() && _selectedValue != AppConsts.ZERO)
                    {
                        var _clientPaymentOptn = lstClientPaymentOptns.Where(po => po.PaymentOptionID == Convert.ToInt32(_selectedValue)
                                                                   && !po.IsDeleted).FirstOrDefault();
                        var _controlId = "pi_" + _clientPaymentOptn.Code;
                        var _isControlAdded = (pnlInstructions.FindControl(_controlId) as System.Web.UI.Control).IsNullOrEmpty() ? false : true;

                        if (_clientPaymentOptn.IsNotNull() && !_isControlAdded)
                        {
                            var _masterPaymentOption = _lstMasterPaymentOptns.Where(mpo => mpo.Code == _clientPaymentOptn.Code).First();

                            if (!_masterPaymentOption.InstructionText.IsNullOrEmpty())
                            {
                                System.Web.UI.Control _piInstructions = Page.LoadControl("~/ComplianceOperations/UserControl/PaymentInstructions.ascx");
                                (_piInstructions as PaymentInstructions).ID = _controlId;
                                (_piInstructions as PaymentInstructions).InstructionsText = _masterPaymentOption.InstructionText;
                                (_piInstructions as PaymentInstructions).PaymentModeText = _clientPaymentOptn.Name;
                                pnlInstructions.Controls.Add(_piInstructions);
                            }
                        }

                        //UAT-1480: WB: Updates to the Credit Card Agreement Statement on our websites (AMS and Complio)
                        if (_clientPaymentOptn.IsNotNull())
                        {
                            lstPaymentOptnCode.Add(_clientPaymentOptn.Code);
                        }
                        //if (_clientPaymentOptn.IsNotNull() && _clientPaymentOptn.Code == PaymentOptions.Credit_Card.GetStringValue())
                        //{
                        //    if (_isCreditCardControlAdded == false)
                        //    {
                        //        dvUserAgreement.Visible = _isCreditCardControlAdded = true;
                        //        litText.Text = Presenter.GetCreditCardAgreement();
                        //    }
                        //}
                        //else
                        //{
                        //    dvUserAgreement.Visible = false;
                        //}
                        //End
                    }
                }
                //UAT-3850
                if (_cmbPaymentModeBalanceAmt.Visible)
                {
                    List<Entity.ClientEntity.lkpPaymentOption> _lstClientPaymentOptn = new List<Entity.ClientEntity.lkpPaymentOption>();
                    lstClientPaymentOptns = Presenter.GetPaymentTypeList();

                    //Balance Amount case
                    var _selectedValue = (item.FindControl("cmbPaymentModeBalanceAmt") as WclComboBox).SelectedValue;
                    if (!_selectedValue.IsNullOrEmpty() && _selectedValue != AppConsts.ZERO)
                    {
                        Entity.ClientEntity.lkpPaymentOption _clientPaymentOptn = lstClientPaymentOptns.Where(po => po.PaymentOptionID == Convert.ToInt32(_selectedValue)
                                                                   && !po.IsDeleted).FirstOrDefault();
                        _lstClientPaymentOptn.Add(_clientPaymentOptn);
                    }

                    //Payment by institution case
                    if (!_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.IsNullOrEmpty()
                        && !_applicantOrderCart.FingerPrintData.BillingCode.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.BillingCodeAmount.IsNullOrEmpty() && _applicantOrderCart.FingerPrintData.BillingCodeAmount > AppConsts.NONE)
                    {
                        var _paymentByInstitutionOptionId = CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId;
                        Entity.ClientEntity.lkpPaymentOption _clientPaymentOptn = lstClientPaymentOptns.Where(po => po.PaymentOptionID == Convert.ToInt32(_paymentByInstitutionOptionId)
                                                                   && !po.IsDeleted).FirstOrDefault();
                        _lstClientPaymentOptn.Add(_clientPaymentOptn);
                    }

                    if (!_lstClientPaymentOptn.IsNullOrEmpty() && _lstClientPaymentOptn.Count > AppConsts.NONE)
                    {
                        foreach (Entity.ClientEntity.lkpPaymentOption _clientPaymentOptn in _lstClientPaymentOptn)
                        {
                            var _controlId = "pi_" + _clientPaymentOptn.Code;
                            var _isControlAdded = (pnlInstructions.FindControl(_controlId) as System.Web.UI.Control).IsNullOrEmpty() ? false : true;

                            if (_clientPaymentOptn.IsNotNull() && !_isControlAdded)
                            {
                                var _masterPaymentOption = _lstMasterPaymentOptns.Where(mpo => mpo.Code == _clientPaymentOptn.Code).First();

                                if (!_masterPaymentOption.InstructionText.IsNullOrEmpty())
                                {
                                    System.Web.UI.Control _piInstructions = Page.LoadControl("~/ComplianceOperations/UserControl/PaymentInstructions.ascx");
                                    (_piInstructions as PaymentInstructions).ID = _controlId;
                                    (_piInstructions as PaymentInstructions).InstructionsText = _masterPaymentOption.InstructionText;
                                    (_piInstructions as PaymentInstructions).PaymentModeText = _clientPaymentOptn.Name;
                                    pnlInstructions.Controls.Add(_piInstructions);
                                }
                            }
                            //UAT-1480: WB: Updates to the Credit Card Agreement Statement on our websites (AMS and Complio)
                            if (_clientPaymentOptn.IsNotNull())
                            {
                                lstPaymentOptnCode.Add(_clientPaymentOptn.Code);
                            }
                        }
                    }
                }
            }
            //UAT-1480: WB: Updates to the Credit Card Agreement Statement on our websites (AMS and Complio)
            if (!lstPaymentOptnCode.IsNullOrEmpty() && lstPaymentOptnCode.Contains(PaymentOptions.Credit_Card.GetStringValue()))
            {
                dvUserAgreement.Visible = true;
                litText.Text = Presenter.GetCreditCardAgreement();
            }
            else
            {
                dvUserAgreement.Visible = false;
            }
            //End
        }

        /// <summary>
        /// Manages the Show/Hide of the Pricing div, based on the Payment type selected
        /// </summary>
        private void ManagePriceDivShowHide()
        {
            Boolean _showDiv = true;
            foreach (RepeaterItem rptItem in rptPackages.Items)
            {
                if (!_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.BillingCodeAmount.IsNullOrEmpty() && _applicantOrderCart.FingerPrintData.BillingCodeAmount > AppConsts.NONE)
                {
                    var _cmbPaymentModeBalanceAmt = rptItem.FindControl("cmbPaymentModeBalanceAmt") as WclComboBox;
                    _showDiv = false;
                }
                else
                {
                    var _cmbPaymentModes = rptItem.FindControl("cmbPaymentModes") as WclComboBox;
                    if (_cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceId.ToString() || _cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString())
                    {
                        _showDiv = false;
                    }
                }
            }
            if (!_showDiv)
            {
                dvPrice.Visible = false;
                divRush.Visible = false;
                if (!IsCompliancePackageSelected || (!CurrentViewContext.ShowRushOrder || txtRushOrderPrice.Text.IsNullOrEmpty()))
                {
                    divPaymentDetailSubContent.Visible = false;
                }
            }
            else
            {
                dvPrice.Visible = true;
                if (chkRushOrder.Checked)
                {
                    divRush.Visible = true;
                }
            }
        }

        /// <summary>
        /// Manages the show hide of the Payment Options of the Compliance package
        /// </summary>
        /// <param name="cmpPnl"></param>
        /// <param name="cmbPaymentModes"></param>
        /// <param name="isVisible"></param>
        private static void ShowHideCompliancePaymentOptions(Panel cmpPnl, WclComboBox cmbPaymentModes, Decimal amount)
        {
            if (cmbPaymentModes.IsNull() || cmpPnl.IsNull()) return;

            if (amount <= AppConsts.NONE)
            {
                cmpPnl.Visible = false;
                cmbPaymentModes.Visible = false;
            }
            else
            {
                cmpPnl.Visible = true;
                cmbPaymentModes.Visible = true;
            }
        }

        /// <summary>
        /// Returns whether the payment option selected for the Compliance Pkg is Invoice type or not
        /// </summary>
        /// <param name="cmbPaymentModes"></param>
        /// <returns></returns>
        private bool IsInvoiceSelected()
        {
            //return cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceId.ToString() || cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString();
            foreach (RepeaterItem rptItem in rptPackages.Items)
            {
                var _cmbPaymentModes = rptItem.FindControl("cmbPaymentModes") as WclComboBox;
                if (_cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceId.ToString() || _cmbPaymentModes.SelectedValue == CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString())
                {
                    return true;
                }
            }
            return false; ;
        }

        #region UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"

        private void HideControlsForCompleteOrderMode()
        {
            if (String.Compare(_applicantOrderCart.OrderRequestType, OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) == AppConsts.NONE
                && _applicantOrderCart.IsReadOnly)
            {
                btnPrevious.Visible = false;
                base.SetPageTitle(String.Empty);
            }

        }

        #endregion

        #region UAT-3268

        private void BindAdditionalPaymentInstructions()
        {
            BindAdditionalPaymentType();
            List<Entity.ClientEntity.lkpPaymentOption> lstClientPaymentOptns = new List<Entity.ClientEntity.lkpPaymentOption>();
            List<Entity.lkpPaymentOption> _lstMasterPaymentOptns = Presenter.GetMasterPaymentSettings(out lstClientPaymentOptns);
            List<String> lstPaymentOptnCode = new List<String>();

            if (rptrAdtnlPayment.Items.Count > AppConsts.NONE)
            {
                foreach (RepeaterItem item in rptrAdtnlPayment.Items)
                {
                    HiddenField hdnAdditionalPaymentOptionID = (item.FindControl("hdnAdditionalPaymentOptionID") as HiddenField);
                    Int32 addnlPaymentOptionID = Convert.ToInt32(hdnAdditionalPaymentOptionID.Value);

                    if (!addnlPaymentOptionID.IsNullOrEmpty() && addnlPaymentOptionID > AppConsts.NONE)
                    {
                        var _additionalPaymentOptn = lstClientPaymentOptns.Where(po => po.PaymentOptionID == Convert.ToInt32(addnlPaymentOptionID)
                                                                   && !po.IsDeleted).FirstOrDefault();
                        var _controlId = "pi_" + _additionalPaymentOptn.Code;
                        var _isControlAdded = (pnlInstructions.FindControl(_controlId) as System.Web.UI.Control).IsNullOrEmpty() ? false : true;

                        if (_additionalPaymentOptn.IsNotNull() && !_isControlAdded)
                        {
                            var _masterPaymentOption = _lstMasterPaymentOptns.Where(mpo => mpo.Code == _additionalPaymentOptn.Code).First();

                            if (!_masterPaymentOption.InstructionText.IsNullOrEmpty())
                            {
                                System.Web.UI.Control _piInstructions = Page.LoadControl("~/ComplianceOperations/UserControl/PaymentInstructions.ascx");
                                (_piInstructions as PaymentInstructions).ID = _controlId;
                                (_piInstructions as PaymentInstructions).InstructionsText = _masterPaymentOption.InstructionText;
                                (_piInstructions as PaymentInstructions).PaymentModeText = _additionalPaymentOptn.Name;
                                pnlInstructions.Controls.Add(_piInstructions);
                            }
                        }

                        //UAT-1480: WB: Updates to the Credit Card Agreement Statement on our websites (AMS and Complio)
                        if (_additionalPaymentOptn.IsNotNull())
                        {
                            lstPaymentOptnCode.Add(_additionalPaymentOptn.Code);
                        }
                    }
                }
            }
        }

        private void GetRotationQualifyingBkgPkgs()
        {
            if (!_applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                CurrentViewContext.lstRotationQualifyingBkgPkgs = _applicantOrderCart.lstApplicantOrder[0].lstPackages.Where(cond => cond.IsReqToQualifyInRotation).ToList();
            }
        }

        #endregion

        private void BindAdditionalPaymentType()
        {
            GetRotationQualifyingBkgPkgs();
            List<PkgAdditionalPaymentInfo> lstAdditionalPaymentOptions = Presenter.GetAdditionalPriceData();
            if (!CurrentViewContext.lstRotationQualifyingBkgPkgs.IsNullOrEmpty() && !lstAdditionalPaymentOptions.Where(cond => cond.AdditionalPrice > AppConsts.NONE).ToList().IsNullOrEmpty())
            {
                dvAdditionalPaymentTypes.Visible = true;

                rptrAdtnlPayment.DataSource = lstAdditionalPaymentOptions.Where(cond => cond.AdditionalPrice > AppConsts.NONE).ToList();
                rptrAdtnlPayment.DataBind();
            }
            else
            {
                dvAdditionalPaymentTypes.Visible = false;
            }
        }

        private void SkipSubmitForNewSingleCard()
        {
            List<INTSOF.AuthNet.Business.PaymentProfileDetail> lstOldPaymentProfileDetails = new List<INTSOF.AuthNet.Business.PaymentProfileDetail>();
            //long customerProfileId = 0;
            SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
            Entity.AuthNetCustomerProfile customerProfile = Presenter.GetCustomerProfile(user.UserId);
            _applicantOrderCart.lstOldCustomerPaymentProfileId = new List<Int64>();

            if (!customerProfile.IsNullOrEmpty())
            {
                lstOldPaymentProfileDetails = INTSOF.AuthNet.Business.AuthorizeNetCreditCard.GetCustomerPaymentProfiles(Convert.ToInt64(customerProfile.CustomerProfileID));
                if (!lstOldPaymentProfileDetails.IsNullOrEmpty())
                    _applicantOrderCart.lstOldCustomerPaymentProfileId = lstOldPaymentProfileDetails.Select(sel => sel.CustomerPaymentProfileId).ToList();
            }
        }

        #region UAT-3958

        private void ManageApprovalRequiredPopup()
        {
            _applicantOrderCart = GetApplicantOrderCart();
            if (!_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.IsLocationServiceTenant)
            {
                CurrentViewContext.IsAnyOptionsApprovalReq = false;
                List<String> _lstApprovalReqPkgName = new List<string>();
                //END UAT
                foreach (RepeaterItem item in rptPackages.Items)
                {
                    var _cmbPaymentModes = item.FindControl("cmbPaymentModes") as WclComboBox;
                    var _hdfPkgId = item.FindControl("hdfPkgId") as HiddenField;
                    var _hdfIsBkgPkg = item.FindControl("hdfIsBkgPkg") as HiddenField;

                    Int32 _pkgId = AppConsts.NONE;
                    Boolean _isBkgPkg = false;
                    if (!_hdfPkgId.IsNullOrEmpty())
                        _pkgId = !_hdfPkgId.Value.IsNullOrEmpty() && Convert.ToInt32(_hdfPkgId.Value) > AppConsts.NONE ? Convert.ToInt32(_hdfPkgId.Value) : AppConsts.NONE;
                    if (!_hdfIsBkgPkg.IsNullOrEmpty())
                        _isBkgPkg = !_hdfIsBkgPkg.Value.IsNullOrEmpty() ? Convert.ToBoolean(_hdfIsBkgPkg.Value) : false;

                    if (_cmbPaymentModes.Visible == true && !_cmbPaymentModes.SelectedValue.IsNullOrEmpty())
                    {
                        //Boolean _isApprovalReq = false;
                        Int32 _selectedOptionId = Convert.ToInt32(_cmbPaymentModes.SelectedValue);
                        PkgList pkgListContract = new PkgList();

                        if (_pkgId > AppConsts.NONE)
                            pkgListContract = CurrentViewContext.lstPaymentOptions.Where(po => po.PkgId == _pkgId && po.IsBkgPkg == _isBkgPkg).FirstOrDefault();
                        //UAT-4357
                        if (!pkgListContract.IsNullOrEmpty() && pkgListContract.lstPaymentOptions.Where(cond2 => cond2.PaymentOptionId == _selectedOptionId && cond2.IsApprovalRequired).Count() > 0)
                            _lstApprovalReqPkgName.Add(pkgListContract.PkgName);

                        if (!pkgListContract.IsNullOrEmpty() && _selectedOptionId > AppConsts.NONE && !CurrentViewContext.IsAnyOptionsApprovalReq)
                        {
                            CurrentViewContext.IsAnyOptionsApprovalReq = pkgListContract.lstPaymentOptions.Where(con => con.PaymentOptionId == _selectedOptionId).FirstOrDefault().IsApprovalRequired;
                        }
                    }
                }
                //UAT-4537
                if (!_lstApprovalReqPkgName.IsNullOrEmpty())
                {
                    _applicantOrderCart.ApprovalPendingPackageName = String.Empty;
                    HtmlGenericControl div = new HtmlGenericControl("div");
                    div.Style.Add("float", "left");
                    HtmlGenericControl ul = new HtmlGenericControl("ul");

                    _lstApprovalReqPkgName.ForEach(x =>
                    {
                        HtmlGenericControl li = new HtmlGenericControl("li");
                        li.InnerText = x;
                        li.Style["list-style"] = "disc";
                        ul.Controls.Add(li);
                        _applicantOrderCart.ApprovalPendingPackageName = _applicantOrderCart.ApprovalPendingPackageName + ", '" + x + "'";
                    });
                    _applicantOrderCart.ApprovalPendingPackageName = _applicantOrderCart.ApprovalPendingPackageName.Remove(0, 1);
                    ul.Style["padding-left"] = "30px";
                    div.Controls.Add(ul);
                    pnlApprovalOrders.Controls.Add(div);
                }
                else
                    _applicantOrderCart.ApprovalPendingPackageName = String.Empty;
            }
        }

        #endregion

        #region UAT-4057

        /// <summary>
        /// It will check whether the order has price and is whole order has no price then system will not show common payment option.
        /// </summary>
        /// <returns></returns>
        private Boolean IsTotatlOrderPriceExists()
        {
            string totalOrderPrice = string.Empty;
            if (!_applicantOrderCart.IsCompliancePackageSelected) // Case when only Background package was selected
                totalOrderPrice = Convert.ToString(GetBackgroundPackagesPrice(), CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
            else
                totalOrderPrice = Convert.ToString((_applicantOrderCart.CompliancePackagesGrandTotal + GetBackgroundPackagesPrice()), CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue()));
            if (!totalOrderPrice.IsNullOrEmpty())
            {
                if (Convert.ToDouble(totalOrderPrice) > AppConsts.NONE)
                    return true;
                return false;
            }
            return false;
        }

        private void CheckIfCommonPaymentOptionToUseForAllPackages()
        {
            List<PkgPaymentOptions> lstPaymentOptions = new List<PkgPaymentOptions>();
            if (CurrentViewContext.lstPaymentOptions.Count() > AppConsts.ONE && IsTotatlOrderPriceExists())
            {
                lstPaymentOptions = CurrentViewContext.lstPaymentOptions.FirstOrDefault().lstPaymentOptions;
                var IsAllPkgsPaymentOptionNotSame = false;
                List<PkgPaymentOptions> currPkgPaymentOptions = new List<PkgPaymentOptions>();

                //same payment type functionality will only work when applicant is non CBI.
                if (!_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.IsLocationServiceTenant)
                {
                    foreach (var pkgPaymentOptions in CurrentViewContext.lstPaymentOptions.Select(k => k.lstPaymentOptions))
                    {
                        lstPaymentOptions = lstPaymentOptions.OrderBy(lst => lst.PaymentOptionCode).ToList();
                        currPkgPaymentOptions = pkgPaymentOptions.OrderBy(lst => lst.PaymentOptionCode).ToList();

                        if (lstPaymentOptions.Count == currPkgPaymentOptions.Count)
                        {
                            var diff = lstPaymentOptions.Where((x, i) => currPkgPaymentOptions[i].PaymentOptionCode != x.PaymentOptionCode);
                            if (diff == null || diff.Count() == AppConsts.NONE)
                            {
                                CurrentViewContext.IsAllPkgsPaymentOptionSame = true;
                            }
                            else
                            {
                                IsAllPkgsPaymentOptionNotSame = true;
                            }
                        }
                        else
                        {
                            IsAllPkgsPaymentOptionNotSame = true;
                        }
                    }
                }

                //same payment type functionality will only work when applicant is CBI.
                if (!_applicantOrderCart.IsNullOrEmpty() && _applicantOrderCart.IsLocationServiceTenant)
                {
                    foreach (var pkgPaymentOptions in CurrentViewContext.lstPaymentOptions.Select(k => k.lstPaymentOptions))
                    {
                        lstPaymentOptions = lstPaymentOptions.OrderBy(lst => lst.PaymentOptionCode).ToList();
                        currPkgPaymentOptions = pkgPaymentOptions.OrderBy(lst => lst.PaymentOptionCode).ToList();
                        foreach (var pkgPaymentoption in currPkgPaymentOptions)
                        {
                            bool paymentTypeExits = lstPaymentOptions.Any(x => x.PaymentOptionName == pkgPaymentoption.PaymentOptionName);
                            if (paymentTypeExits == false)
                            {
                                lstPaymentOptions.Add(pkgPaymentoption);
                            }
                        }
                        CurrentViewContext.IsAllPkgsPaymentOptionSame = true;
                    }
                }

                if (IsAllPkgsPaymentOptionNotSame)
                    CurrentViewContext.IsAllPkgsPaymentOptionSame = false;

                if (CurrentViewContext.IsAllPkgsPaymentOptionSame == false)
                {
                    dvCommonPaymentSelection.Visible = false;
                }
                else
                {
                    dvCommonPaymentSelection.Visible = true;
                    if (_applicantOrderCart.IsModifyShipping == true || (_applicantOrderCart.FingerPrintData.IsNotNull() && _applicantOrderCart.FingerPrintData.IsFromArchivedOrderScreen))
                    {
                        lstPaymentOptions = lstPaymentOptions.Where(x => x.PaymentOptionCode == PaymentOptions.Credit_Card.GetStringValue()).ToList();
                    }
                    cmbPaymentModesCommon.DataSource = lstPaymentOptions;
                    cmbPaymentModesCommon.DataTextField = "PaymentOptionName";
                    cmbPaymentModesCommon.DataValueField = "PaymentOptionId";
                    cmbPaymentModesCommon.DataBind();
                    cmbPaymentModesCommon.AutoPostBack = true;
                    cmbPaymentModesCommon.CausesValidation = false;
                    //UAT-4537
                    Int32 _selectedPaymentOption = Convert.ToInt32(cmbPaymentModesCommon.SelectedValue);
                    List<String> _lstApprovalReqPkgName = CurrentViewContext.lstPaymentOptions.Where(cond => cond.lstPaymentOptions.Where(po => po.PaymentOptionId == _selectedPaymentOption && po.IsApprovalRequired).Count() > 0).Select(sel => sel.PkgName).ToList();
                    if (!_lstApprovalReqPkgName.IsNullOrEmpty())
                    {
                        _applicantOrderCart.ApprovalPendingPackageName = String.Empty;
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        div.Style.Add("float", "left");
                        HtmlGenericControl ul = new HtmlGenericControl("ul");

                        _lstApprovalReqPkgName.ForEach(x =>
                        {
                            HtmlGenericControl li = new HtmlGenericControl("li");
                            li.InnerText = x;
                            li.Style["list-style"] = "disc";
                            ul.Controls.Add(li);
                            _applicantOrderCart.ApprovalPendingPackageName = _applicantOrderCart.ApprovalPendingPackageName + ", '" + x + "'";
                        });
                        _applicantOrderCart.ApprovalPendingPackageName = _applicantOrderCart.ApprovalPendingPackageName.Remove(0, 1);
                        ul.Style["padding-left"] = "30px";
                        div.Controls.Add(ul);
                        pnlApprovalOrders.Controls.Add(div);
                    }
                    else
                        _applicantOrderCart.ApprovalPendingPackageName = String.Empty;
                }

            }
            if (CurrentViewContext.lstPaymentOptions.Count() == AppConsts.ONE && _applicantOrderCart.IsModifyShipping == true)
            {
                CurrentViewContext.IsAllPkgsPaymentOptionSame = true;
                dvCommonPaymentSelection.Visible = true;
                lstPaymentOptions = CurrentViewContext.lstPaymentOptions.FirstOrDefault().lstPaymentOptions;
                lstPaymentOptions = lstPaymentOptions.Where(x => x.PaymentOptionCode == PaymentOptions.Credit_Card.GetStringValue()).ToList();
                cmbPaymentModesCommon.DataSource = lstPaymentOptions;
                cmbPaymentModesCommon.DataTextField = "PaymentOptionName";
                cmbPaymentModesCommon.DataValueField = "PaymentOptionId";
                cmbPaymentModesCommon.DataBind();
                cmbPaymentModesCommon.AutoPostBack = true;
                cmbPaymentModesCommon.CausesValidation = false;
            }
        }

        private void ManagePriceDivShowHidePaymentModesCommon()
        {
            Boolean _showDiv = true;
            if (!_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.IsNullOrEmpty()
                && !_applicantOrderCart.FingerPrintData.BillingCodeAmount.IsNullOrEmpty() && _applicantOrderCart.FingerPrintData.BillingCodeAmount > AppConsts.NONE)
            {
                _showDiv = false;
            }
            else
            {
                if (cmbPaymentModesCommon.SelectedValue == CurrentViewContext.PaymentMode_InvoiceId.ToString()
                  || cmbPaymentModesCommon.SelectedValue == CurrentViewContext.PaymentMode_InvoiceWdoutApprvlId.ToString())
                    { 
                    _showDiv = false;
                    }
            }
            if (!_showDiv)
            {
                dvPrice.Visible = false;
                divRush.Visible = false;
                if (!IsCompliancePackageSelected || (!CurrentViewContext.ShowRushOrder || txtRushOrderPrice.Text.IsNullOrEmpty()))
                {
                    divPaymentDetailSubContent.Visible = false;
                }
            }
            else
            {
                dvPrice.Visible = true;
                if (chkRushOrder.Checked)
                {
                    divRush.Visible = true;
                }
            }
        }

        private void BindPaymentInstructionsPaymentModesCommon()
        {
            List<Entity.ClientEntity.lkpPaymentOption> lstClientPaymentOptns = new List<Entity.ClientEntity.lkpPaymentOption>();
            List<Entity.lkpPaymentOption> _lstMasterPaymentOptns = Presenter.GetMasterPaymentSettings(out lstClientPaymentOptns);
            List<String> lstPaymentOptnCode = new List<String>();

            if (cmbPaymentModesCommon.Visible)
            {
                var _selectedValue = cmbPaymentModesCommon.SelectedValue;

                if (!_selectedValue.IsNullOrEmpty() && _selectedValue != AppConsts.ZERO)
                {
                    var _clientPaymentOptn = lstClientPaymentOptns.Where(po => po.PaymentOptionID == Convert.ToInt32(_selectedValue)
                                                               && !po.IsDeleted).FirstOrDefault();
                    var _controlId = "pi_" + _clientPaymentOptn.Code;
                    var _isControlAdded = (pnlInstructions.FindControl(_controlId) as System.Web.UI.Control).IsNullOrEmpty() ? false : true;

                    if (_clientPaymentOptn.IsNotNull() && !_isControlAdded)
                    {
                        var _masterPaymentOption = _lstMasterPaymentOptns.Where(mpo => mpo.Code == _clientPaymentOptn.Code).First();

                        if (!_masterPaymentOption.InstructionText.IsNullOrEmpty())
                        {
                            System.Web.UI.Control _piInstructions = Page.LoadControl("~/ComplianceOperations/UserControl/PaymentInstructions.ascx");
                            (_piInstructions as PaymentInstructions).ID = _controlId;
                            (_piInstructions as PaymentInstructions).InstructionsText = _masterPaymentOption.InstructionText;
                            (_piInstructions as PaymentInstructions).PaymentModeText = _clientPaymentOptn.Name;
                            pnlInstructions.Controls.Add(_piInstructions);
                        }
                    }

                    //UAT-1480: WB: Updates to the Credit Card Agreement Statement on our websites (AMS and Complio)
                    if (_clientPaymentOptn.IsNotNull())
                    {
                        lstPaymentOptnCode.Add(_clientPaymentOptn.Code);
                    }
                }
            }

            //UAT-1480: WB: Updates to the Credit Card Agreement Statement on our websites (AMS and Complio)
            if (!lstPaymentOptnCode.IsNullOrEmpty() && lstPaymentOptnCode.Contains(PaymentOptions.Credit_Card.GetStringValue()))
            {
                dvUserAgreement.Visible = true;
                litText.Text = Presenter.GetCreditCardAgreement();
            }
            else
            {
                dvUserAgreement.Visible = false;
            }
            //End
        }

        private void ManageApprovalRequiredPopupPaymentModesCommon()
        {
            _applicantOrderCart = GetApplicantOrderCart();
            if (!_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.IsLocationServiceTenant)
            {
                CurrentViewContext.IsAnyOptionsApprovalReq = false;
                foreach (RepeaterItem item in rptPackages.Items)
                {
                    //var _cmbPaymentModes = item.FindControl("cmbPaymentModes") as WclComboBox;
                    var _hdfPkgId = item.FindControl("hdfPkgId") as HiddenField;
                    var _hdfIsBkgPkg = item.FindControl("hdfIsBkgPkg") as HiddenField;

                    Int32 _pkgId = AppConsts.NONE;
                    Boolean _isBkgPkg = false;
                    if (!_hdfPkgId.IsNullOrEmpty())
                        _pkgId = !_hdfPkgId.Value.IsNullOrEmpty() && Convert.ToInt32(_hdfPkgId.Value) > AppConsts.NONE ? Convert.ToInt32(_hdfPkgId.Value) : AppConsts.NONE;
                    if (!_hdfIsBkgPkg.IsNullOrEmpty())
                        _isBkgPkg = !_hdfIsBkgPkg.Value.IsNullOrEmpty() ? Convert.ToBoolean(_hdfIsBkgPkg.Value) : false;

                    if (cmbPaymentModesCommon.Visible == true && !cmbPaymentModesCommon.SelectedValue.IsNullOrEmpty())
                    {
                        //Boolean _isApprovalReq = false;
                        Int32 _selectedOptionId = Convert.ToInt32(cmbPaymentModesCommon.SelectedValue);
                        PkgList pkgListContract = new PkgList();

                        if (_pkgId > AppConsts.NONE)
                            pkgListContract = CurrentViewContext.lstPaymentOptions.Where(po => po.PkgId == _pkgId && po.IsBkgPkg == _isBkgPkg).FirstOrDefault();

                        if (!pkgListContract.IsNullOrEmpty() && _selectedOptionId > AppConsts.NONE && !CurrentViewContext.IsAnyOptionsApprovalReq)
                        {
                            CurrentViewContext.IsAnyOptionsApprovalReq = pkgListContract.lstPaymentOptions.Where(con => con.PaymentOptionId == _selectedOptionId).FirstOrDefault().IsApprovalRequired;
                        }
                    }
                }
            }
        }

        private void ShowHideCommonPaymentType()
        {
            if (chkSplitPaymentTypeByPkg.Checked)
            {
                pnlPaymentTypeCommon.Visible = false;

                foreach (RepeaterItem item in rptPackages.Items)
                {
                    Panel pnlPaymentType = ((item.FindControl("pnlPaymentType") as Panel));
                    if (!pnlPaymentType.IsNullOrEmpty())
                        pnlPaymentType.Visible = true;
                    WclComboBox _cmbPaymentModes = ((item.FindControl("cmbPaymentModes") as WclComboBox));
                    if (!_cmbPaymentModes.IsNullOrEmpty())
                        _cmbPaymentModes.Visible = true;

                }
                ManageApprovalRequiredPopup();
                ManagePriceDivShowHide();
                BindPaymentInstructions();
                BusinessRuleImplementations();
            }
            else
            {
                pnlPaymentTypeCommon.Visible = true;
                foreach (RepeaterItem item in rptPackages.Items)
                {
                    Panel pnlPaymentType = ((item.FindControl("pnlPaymentType") as Panel));
                    if (!pnlPaymentType.IsNullOrEmpty())
                        pnlPaymentType.Visible = false;
                    WclComboBox _cmbPaymentModes = ((item.FindControl("cmbPaymentModes") as WclComboBox));
                    if (!_cmbPaymentModes.IsNullOrEmpty())
                        _cmbPaymentModes.Visible = false;

                }
                ManagePriceDivShowHidePaymentModesCommon();
                BindPaymentInstructionsPaymentModesCommon();
            }

        }

        #endregion

        #endregion

        #endregion

    }
}