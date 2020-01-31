using CoreWeb.BkgOperations.Views;
using CoreWeb.ComplianceOperations.Views;
using CoreWeb.FingerPrintSetUp.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.UI.Contract.Globalization;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceOperations.UserControl
{
    public partial class ModifyShippingConfirmation : BaseUserControl, IModifyShippingConfirmationView
    {

        #region Private Variables

        private String _viewType;

        private ModifyShippingConfirmationPresenter _presenter = new ModifyShippingConfirmationPresenter();
        private OrganizationUserProfile _orgUserProfile;
        private ApplicantOrderCart applicantOrderCart = new ApplicantOrderCart();

        #endregion


        #region Properties
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
        public List<OrderPaymentDetail> lstOPDs { get; set; }
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
        public List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }
        List<Entity.lkpSuffix> IModifyShippingConfirmationView.lstSuffixes
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
        Int32 IModifyShippingConfirmationView.OrderPaymentDetaildId { get; set; }
        public String InstitutionHierarchy
        {
            get;
            set;
        }
        Boolean IModifyShippingConfirmationView.IsOrderStatusPaid
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
        public List<Tuple<String, String>> lstClientPaymentOptns
        {
            get;
            set;
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
        public DataTable lstExternalPackages
        {
            get;
            set;
        }
        String IModifyShippingConfirmationView.DecryptedSSN { get; set; }
        String IModifyShippingConfirmationView.LanguageCode
        {
            get
            {
                LanguageContract languageContract = LanguageTranslateUtils.GetCurrentLanguageFromSession();
                if (!languageContract.IsNullOrEmpty())
                    return languageContract.LanguageCode;
                return Languages.ENGLISH.GetStringValue();
            }
        }
        public IModifyShippingConfirmationView CurrentViewContext
        {
            get { return this; }
        }

        public ModifyShippingConfirmationPresenter Presenter
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
        Boolean IsCreateOrderSummaryHTMLFileOnRender
        {
            get;
            set;
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

        List<Int32> IModifyShippingConfirmationView.RecentAddedOPDs { get; set; }

        String IModifyShippingConfirmationView.OrderRequestType { get; set; }
        Boolean IModifyShippingConfirmationView.IsSSNDisabled
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

        public List<Int32> DPPSIds
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
        public String NextPagePath
        {
            get;
            set;
        }
        public List<OrderCartCompliancePackage> CompliancePackages { get; set; }
        public List<Order> OrderData
        {
            get;
            set;
        }
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
        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = Resources.Language.MODIFYSHIPPINGADDRESS;
                base.BreadCrumbTitleKey = Resources.Language.MODIFYSHIPPINGADDRESS;
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
                (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle(Resources.Language.MODIFYSHIPPINGADDRESS);
                Presenter.IsLocationServiceTenant();
                Presenter.OnViewLoaded();
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
                            if(applicantOrder.OrderNumber.IsNullOrEmpty())
                            applicantOrder.OrderNumber = Presenter.GetOrderNumber(applicantOrder.OrderId);
                            strOrderNumber = Convert.ToString(applicantOrder.OrderNumber);

                        lblOrderId.Text = strOrderID;
                        lblOrderNumber.Text = strOrderNumber;
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

                    BaseUserControl.LogOrderFlowSteps("ModifyShippingOrderConfirmation.ascx - STEP 1: Page_Load for order: " + lblOrderId.Text);
                    CurrentViewContext.RecentAddedOPDs = _applicantOrderCart.RecentAddedOPDs.IsNullOrEmpty() ? new List<Int32>() : _applicantOrderCart.RecentAddedOPDs;
                    CurrentViewContext.OrderRequestType = _applicantOrderCart.OrderRequestType;

                    if (Presenter.IsOrderPaymentDone(lblOrderId.Text))
                    {
                        var paymentOptionCreditCard = CurrentViewContext.lstOPDs.Where(opd => opd.OPD_PaymentOptionID.IsNotNull()
                                                       && opd.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue()).ToList();

                        base.ShowSuccessMessage(Resources.Language.ORDERSUCCESSPLACED);

                        BaseUserControl.LogOrderFlowSteps("ModifyShippingOrderConfirmation.ascx - STEP 1.1:  Order Payment DONE for order: " + lblOrderId.Text);

                        if (_applicantOrderCart.IsLocationServiceTenant && !lblOrderId.Text.IsNullOrEmpty() && _applicantOrderCart.ChangePaymentTypeCode.IsNullOrWhiteSpace())
                        {
                            String BillingCode = string.Empty;
                            String CbiUniqueId = string.Empty;
                            if (!_applicantOrderCart.FingerPrintData.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.CBIUniqueID.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.BillingCode.IsNullOrEmpty())
                            {
                                if (CurrentViewContext.lstOPDs.Any(x => x.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()))
                                {
                                    BillingCode = _applicantOrderCart.FingerPrintData.BillingCode;
                                    CbiUniqueId = _applicantOrderCart.FingerPrintData.CBIUniqueID;
                                }
                            }
                            Boolean isCompleteYourOrderClick = _applicantOrderCart.OrderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue() ? true : false;
                        }
                    }
                    else
                    {
                        BaseUserControl.LogOrderFlowSteps("ModifyShippingOrderConfirmation.ascx - STEP 1.1:  Order Payment NOT DONE for order: " + lblOrderId.Text);
                        base.ShowInfoMessage("Your payment is not completed for this Order.");
                    }

                    AddSuffix();
                    BindInstructions();
                    BindPaymentModes(_applicantOrderCart);
                    BindConfirmationData(_applicantOrderCart);
                    BindPackageData(_applicantOrderCart.IsLocationServiceTenant);

                    String _currentStep = " (" + Resources.Language.STEP + " " + (_applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep) + " " +
                          Resources.Language.OF + " " + _applicantOrderCart.GetTotalOrderSteps() + ")";


                    //base.SetPageTitle(_currentStep);
                    (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle("Modify Shipping");

                    // Confirm
                    CopyBkgDataToCompliancePackage(_applicantOrderCart);

                    //UAT-2970
                    BindCreditCardUserAgreement();
                }
                catch (Exception ex)
                {
                    pnl.Visible = false;
                    base.ShowErrorMessage("An error occured while loading the order details.");
                    BaseUserControl.LogOrderFlowSteps("ModifyShippingOrderConfirmation.ascx - STEP 1: Error in loading the order details, order placed by: " + CurrentViewContext.OrgUsrID + ". Error is: " + Convert.ToString(ex) + " ");
                }


                Presenter.GetSSNSetting();
            }
            hdnTenantID.Value = Convert.ToString(CurrentViewContext.TenantId);
            CreateCustomForm();
            Presenter.OnViewLoaded();
            cbbuttons.SubmitButton.ToolTip = Resources.Language.CLKRETTODASHBOARD;
            divSSN.Visible = !(CurrentViewContext.IsSSNDisabled);
            Guid Id = Guid.NewGuid();
            hdnFileIdentifier.Value = Id.ToString();

            IsCreateOrderSummaryHTMLFileOnRender = ConfigurationManager.AppSettings["IsCreateOrderSummaryHTMLFileOnRender"] == null ? false : Convert.ToBoolean(ConfigurationManager.AppSettings["IsCreateOrderSummaryHTMLFileOnRender"].ToLower());
            hdnIsCreateHTMLFileOnRender.Value = IsCreateOrderSummaryHTMLFileOnRender.ToString();
            ManageSSN();
        }

        #endregion

        #region Button Events
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
                    if (CurrentViewContext.IsLocationServiceTenant && !_applicantOrderCart.IsNullOrEmpty() && !_applicantOrderCart.FingerPrintData.IsNullOrEmpty() && ((!_applicantOrderCart.FingerPrintData.BillingCode.IsNullOrEmpty()
                    && !_applicantOrderCart.FingerPrintData.BillingCodeAmount.IsNullOrEmpty() && _applicantOrderCart.FingerPrintData.BillingCodeAmount > AppConsts.NONE) || _applicantOrderCart.IsModifyShipping))
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
        protected void GOTODASHBOARD_Click(object sender, EventArgs e)
        {
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
            String url = String.Format(AppConsts.APPLICANT_MAIN_PAGE_NAME);
            Response.Redirect(url);
        }

        #endregion

        #region Method
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

        private void SetApplicationOrderCart(ApplicantOrderCart applicantOrderCart)
        {
            RedirectIfIncorrectOrderStage(applicantOrderCart);
            applicantOrderCart.AddOrderStageTrackID(OrderStages.OnlineConfirmation);
            SysXWebSiteUtils.SessionService.SetCustomData(AppConsts.ORDER_CONFIRMATION, applicantOrderCart);
        }
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

        private void RedirectIfIncorrectOrderStage(ApplicantOrderCart applicantOrderCart)
        {
            Presenter.GetNextPagePathByOrderStageID(applicantOrderCart);

            //Redirect to next page path if Order Status track is not correct.
            if (CurrentViewContext.NextPagePath.IsNotNull())
            {
                Response.Redirect(CurrentViewContext.NextPagePath);
            }
        }

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
        private void AddSuffix()
        {
            if (CurrentViewContext.IsLocationServiceTenant)
            {
                Presenter.GetSuffixes();
            }
        }

        private void BindInstructions()
        {
            rptInstructions.DataSource = CurrentViewContext.lstClientPaymentOptns;
            rptInstructions.DataBind();
            if (rptInstructions.Items.Count != AppConsts.NONE)
                divPaymentInstruction.Visible = true;
            else
                divPaymentInstruction.Visible = false;
        }

        private void BindPaymentModes(ApplicantOrderCart applicantOrderCart)
        {
            var _records = CurrentViewContext.lstOPDs.Where(opd => opd.OPD_Amount.IsNotNull()
                                                                          && opd.OPD_IsDeleted == false && opd.OPD_Amount > 0).ToList();
            if (!CurrentViewContext.lstAdditionalPaymentModes.IsNullOrEmpty())
            {
                List<Int32> lstAdditionPaymentModeIds = CurrentViewContext.lstAdditionalPaymentModes.Select(sel => sel.OPD_ID).ToList();
                _records = _records.Where(cond => !lstAdditionPaymentModeIds.Contains(cond.OPD_ID)).ToList();
            }
            if (_records.Count == 0)
            {
                divPaymentTypes.Visible = false;
                return;
            }

            rptPaymentModes.DataSource = _records;
            rptPaymentModes.DataBind();
        }

        void BindConfirmationData(ApplicantOrderCart _applicantOrderCart)
        {
            _orgUserProfile = new OrganizationUserProfile();
            //ApplicantOrderCart _applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;

            foreach (var applicantOrder in _applicantOrderCart.lstApplicantOrder)
            {
                _orgUserProfile = applicantOrder.OrganizationUserProfile;
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

                lblFirstName.Text = _orgUserProfile.FirstName;
                lblLastName.Text = _orgUserProfile.LastName;

                //UAT-2212: Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                lblMiddleName.Text = _orgUserProfile.MiddleName.IsNullOrEmpty() ? NoMiddleNameText : _orgUserProfile.MiddleName;
                //CBI || CABS
                if (CurrentViewContext.IsLocationServiceTenant)
                {
                    if (!CurrentViewContext.lstSuffixes.IsNullOrEmpty())
                    {
                        if (!CurrentViewContext.lstSuffixes.Where(cond => cond.SuffixID == _orgUserProfile.UserTypeID).Select(x => x.Suffix).FirstOrDefault().IsNullOrEmpty())
                            lblLastName.Text = _orgUserProfile.UserTypeID.IsNullOrEmpty() ? _orgUserProfile.LastName : _orgUserProfile.LastName + " - " + CurrentViewContext.lstSuffixes.Where(cond => cond.SuffixID == _orgUserProfile.UserTypeID).FirstOrDefault().Suffix;
                        else
                            lblLastName.Text = _orgUserProfile.LastName;
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
                lblEmail.Text = _orgUserProfile.PrimaryEmailAddress;

                //Assigment of mailing address detail.
                if (!_applicantOrderCart.MailingAddress.IsNullOrEmpty() && !_applicantOrderCart.MailingAddress.MailingOptionPrice.IsNullOrWhiteSpace())
                {
                    lblMailingOption.Text = _applicantOrderCart.MailingAddress.MailingOptionPrice;
                    lblMailingAddress.Text = _applicantOrderCart.MailingAddress.Address1;
                    lblMailingCity.Text = _applicantOrderCart.MailingAddress.CityName;
                    lblMailingCountry.Text = _applicantOrderCart.MailingAddress.Country;
                    lblMailingState.Text = _applicantOrderCart.MailingAddress.StateName;
                    lblMailingZipCode.Text = _applicantOrderCart.MailingAddress.Zipcode;
                    if (CurrentViewContext.IsLocationServiceTenant && _applicantOrderCart.MailingAddress.StateName.IsNullOrEmpty())
                    {

                        lblNameMailingZipOrPostalCode.Text = Resources.Language.POSTALCODE;
                    }
                    else
                    {


                        lblNameMailingZipOrPostalCode.Text = Resources.Language.ZIP;
                    }
                }

                //UAT-2447
                if (_orgUserProfile.IsInternationalPhoneNumber)
                {
                    lblPhone.Text = _orgUserProfile.PhoneNumber;
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

                        lblAddress1.Text = _applicantOrderCart.IsLocationServiceTenant ?
                            resHisoryProfile.Address1
                            : resHisoryProfile.Address1 + "," + resHisoryProfile.Address2;
                        lblZip.Text = resHisoryProfile.Zipcode;
                        lblCity.Text = resHisoryProfile.CityName;
                        //UAT-3910
                        if (CurrentViewContext.IsLocationServiceTenant && resHisoryProfile.StateName.IsNullOrEmpty())
                        {
                            dvState.Visible = false;
                            LblNameZIPOPostalCode.Text = Resources.Language.POSTALCODE;
                        }
                        else
                        {
                            dvState.Visible = true;

                            LblNameZIPOPostalCode.Text = Resources.Language.ZIP;
                        }
                        lblState.Text = resHisoryProfile.StateName;
                        lblCountry.Text = resHisoryProfile.Country;
                    }
                }
                else
                {
                    Entity.ResidentialHistory currentResHistory = Presenter.GetCurrentResidentialHistory(_orgUserProfile.OrganizationUserID);
                    if (currentResHistory.IsNotNull())
                    {
                        lblAddress1.Text = currentResHistory.Address.Address1 + "," + currentResHistory.Address.Address2;
                        if (currentResHistory.Address.ZipCodeID > 0)
                        {
                            lblZip.Text = currentResHistory.Address.ZipCode.ZipCode1;
                            lblCity.Text = currentResHistory.Address.ZipCode.City.CityName;
                            lblState.Text = currentResHistory.Address.ZipCode.City.State.StateName;
                            lblCountry.Text = currentResHistory.Address.ZipCode.City.State.Country.FullName;
                        }
                        else
                        {
                            if (currentResHistory.Address.AddressExts.IsNotNull() && currentResHistory.Address.AddressExts.Count > 0)
                            {
                                Entity.AddressExt addressExt = currentResHistory.Address.AddressExts.FirstOrDefault();
                                lblZip.Text = addressExt.AE_ZipCode;
                                lblCity.Text = addressExt.AE_CityName;
                                lblState.Text = addressExt.AE_StateName;
                                lblCountry.Text = addressExt.Country.FullName;
                            }
                        }
                    }
                }
            }
        }

        void BindPackageData(Boolean isLocationServiceTenant)
        {

            String orderRequestType = Convert.ToString(applicantOrderCart.OrderRequestType);
            lblInstitutionHierarchy.Text = InstitutionHierarchy;

            hdrPackageDetail.InnerText = CurrentViewContext.IsLocationServiceTenant ? Resources.Language.ORDRDTLS : "Package Detail";

            hdrOrderDetail.InnerText = CurrentViewContext.IsLocationServiceTenant ? Resources.Language.ORDSELDTLS : Resources.Language.ORDRDTLS;

            hdrPersonalInfo.InnerText = CurrentViewContext.IsLocationServiceTenant ? Resources.Language.PROFILEDETAILS : "Personal Information";
            int orderID = Convert.ToInt32(lblOrderId.Text);
            Decimal? _bkgPkgPrice = GetBackgroundPackagesPrice(orderID);
            lblTotalPrice.Text = "$ " + Convert.ToString(decimal.Round(_bkgPkgPrice ?? 0, 2));


            if (CurrentViewContext.OrderPaymentDetaildId > 0 || (CurrentViewContext.lstOPDs.IsNotNull() && CurrentViewContext.lstOPDs.Count > AppConsts.NONE))
            {
                var _opd = CurrentViewContext.lstOPDs.Where(x => x.OrderPkgPaymentDetails.Any(y => y.OPPD_BkgOrderPackageID.IsNull()
                                              && !y.OPPD_IsDeleted
                                              && y.lkpOrderPackageType.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue())).FirstOrDefault();
            }

            Presenter.GetOrderBkgPackageDetails(lblOrderId.Text);
            if (lstExternalPackages.Rows.Count > 0)
            {
                //divBackgroundPackage.Visible = true;
                var _dt = lstExternalPackages.Clone();
                if (applicantOrderCart.OrderPaymentdetailId > 0)
                {

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
                    //rptBackgroundPackages.DataSource = _dt;
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
                    //rptBackgroundPackages.DataSource = _dt;
                }
                //else
                ///rptBackgroundPackages.DataSource = CurrentViewContext.lstExternalPackages;

                //rptBackgroundPackages.DataBind();
            }
        }

        private Decimal GetBackgroundPackagesPrice(int OrderId)
        {
            Decimal _backgroundPackagesPrice = 0;

            if (OrderId > 0)
            {
                _backgroundPackagesPrice = Presenter.GetOrderPriceTotal(OrderId);
            }
            return _backgroundPackagesPrice;
        }

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

                        BaseUserControl.LogOrderFlowSteps("ModifyShippingOrderConfirmation.ascx - STEP 2: Methods 'Presenter.CopyBkgDataToCompliancePackage' and 'Presenter.CopyCompPackageDataForNewOrder' completed successfully."
                            + " for OrderId(s):" + lblOrderId.Text);
                    }
                }
            }
        }

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
            }
            else
            {
                dvUserAgreement.Visible = false;
            }
        }
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
                BaseUserControl.LogOrderFlowSteps("ModifyShippingOrderConfirmation.ascx - STEP 3.1: Method 'Presenter.GetAttributeFieldsOfSelectedPackages' executed successfully for PackageId(s): " +
                                                   packages + " and OrderId(s)" + lblOrderId.Text);
            }

            List<AttributeFieldsOfSelectedPackages> lstCriminalAttributes = CurrentViewContext.LstInternationCriminalSrchAttributes;
            //if (!_applicantOrderCart.lstPrevAddresses.IsNullOrEmpty())
            //{
            //    PreviousAddressContract resHisoryProfile = _applicantOrderCart.lstPrevAddresses.FirstOrDefault(cond => cond.isCurrent == true);
            //}

            if (!lstBackgroundOrderData.IsNullOrEmpty())
            {
                lstCustomForms = lstBackgroundOrderData.Where(x => x.CustomFormId != AppConsts.ONE).DistinctBy(x => x.CustomFormId).Select(x => x.CustomFormId).ToList();
                var _webCcfLoaded = false;
                var _customFormsLoaded = false;
                for (Int32 custId = 0; custId < lstCustomForms.Count; custId++)
                {
                    Presenter.GetAttributesForTheCustomForm(packages, lstCustomForms[custId], LanguageTranslateUtils.GetCurrentLanguageFromSession().LanguageCode);
                    List<BackgroundOrderData> newLstBackGroundOrderData = new List<BackgroundOrderData>();
                    newLstBackGroundOrderData = lstBackgroundOrderData.Where(x => x.CustomFormId == lstCustomForms[custId]).Select(x => x).ToList();
                    lstGroupIds = newLstBackGroundOrderData.DistinctBy(x => x.BkgSvcAttributeGroupId).Select(x => x.BkgSvcAttributeGroupId).ToList();
                    for (Int32 grpId = 0; grpId < lstGroupIds.Count; grpId++)
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
                BaseUserControl.LogOrderFlowSteps("ModifyShippingOrderConfirmation.ascx - STEP 3.2: For OrderId(s): " + lblOrderId.Text +
                    ", Custom forms loaded: " + (_customFormsLoaded ? "Yes" : "No") +
                    ", WebCcf Loaded:" + (_webCcfLoaded ? "Yes" : "No"));
            }
        }

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
        protected void CmdBarSubmit_Click(object sender, EventArgs e)
        {
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
            String url = String.Format(AppConsts.APPLICANT_MAIN_PAGE_NAME);

            BaseUserControl.LogOrderFlowSteps("OrderConfirmation.ascx - STEP 5: 'Finish' clicked, Redirecitng to dashboard wih url: " + url + ", for OrderId(s): " + lblOrderId.Text);
            Response.Redirect(url);
        }
        #endregion

    }
}