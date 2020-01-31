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
using System.Xml;



#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class OrderReview : BaseUserControl, IOrderReviewView
    {
        #region Variables

        #region Private Variables

        private Int32 _tenantId;
        private OrganizationUserProfile _orgUserProfile;
        private OrderReviewPresenter _presenter = new OrderReviewPresenter();
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

        Boolean IOrderReviewView.IsSSNDisabled
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


        public OrderReviewPresenter Presenter
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

        public IOrderReviewView CurrentViewContext
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
        Boolean IOrderReviewView.IsLocationServiceTenant
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

        List<Entity.lkpSuffix> IOrderReviewView.lstSuffixes
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

        String IOrderReviewView.LanguageCode
        {
            get
            {
                LanguageContract languageContract = LanguageTranslateUtils.GetCurrentLanguageFromSession();
                if (!languageContract.IsNullOrEmpty())
                    return languageContract.LanguageCode;
                return Languages.ENGLISH.GetStringValue();
            }
        }
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

                    //_presenter.GetTenantId();
                    Presenter.OnViewInitialized();
                    Presenter.ShowRushOrderSetting();
                    Presenter.IsLocationServiceTenant();

                    //ucPersonAlias.IsLocationServiceTenant = CurrentViewContext.IsLocationServiceTenant;
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
                        hdnConfirmMsg.Value = Resources.Language.CONFIRMMSGCABS;
                    }


                    _applicantOrderCart = GetApplicantOrderCart();
                    //CBI|| CABS || To Get Suffix List
                    AddSuffix();

                    BindPersonalDetails();

                    GetPricingData();

                    //To show or hide Mailing address section on condition basis.
                    if (_applicantOrderCart.IsLocationServiceTenant && !_applicantOrderCart.MailingAddress.IsNullOrEmpty() && !_applicantOrderCart.MailingAddress.MailingOptionPrice.IsNullOrEmpty()
                        && (!_applicantOrderCart.FingerPrintData.IsEventCode && !_applicantOrderCart.FingerPrintData.IsOutOfState))
                    {
                        {
                            dvMailingAddress.Visible = true;
                            dvMailingState.Visible = !string.IsNullOrWhiteSpace(_applicantOrderCart.MailingAddress.StateName);
                        }
                    }

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
                //Hide the rush order service check box on the basis of client setting for Rush Order.
                //if (ShowRushOrder)
                //{
                //    chkRushOrder.Visible = true;
                //    dvRushOrderSrvc.Visible = true;
                //}
                //else
                //{
                //    chkRushOrder.Visible = false;
                //    chkRushOrder.Checked = false;
                //    dvRushOrderSrvc.Visible = false;
                //    divRushOrder.Visible = false;
                //}
                BindOtherDetails();

                if (!lstPackages.IsNullOrEmpty() && lstPackages.Count > 0)
                    CreateCustomForm();
                if (!_applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && _applicantOrderCart.lstApplicantOrder[0].lstPackages.Count > 0)
                {
                    residentialHistory.Visible = true;
                    BindResidentialHistory();
                }
                cmdbarEditProfile.ExtraButton.ToolTip = Resources.Language.ORDREVBTNEDITPRFLTOOLTIP;
                //cmdbarSubmit.ClearButton.ToolTip = "Submit and pay for your order";
                cmdbarSubmit.ClearButton.ToolTip = Resources.Language.SBMTNPAYYRORD;


                if (!applicantOrderCart.IsNullOrEmpty() &&
                    (applicantOrderCart.OrderRequestType == OrderRequestType.NewOrder.GetStringValue()
                    || applicantOrderCart.OrderRequestType == OrderRequestType.ChangeSubscription.GetStringValue()))
                    (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle(Resources.Language.CREATODR);
                else if (!applicantOrderCart.IsNullOrEmpty() &&
                          applicantOrderCart.OrderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue())
                {
                    (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle(Resources.Language.CMPLTORDER);
                }
                else
                    (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle("Renewal Order");
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

        #region Grid Events



        #endregion

        #region Button Events

        protected void btnEditProfile_Click(object sender, EventArgs e)
        {
            try
            {
                //_applicantOrderCart = GetApplicantOrderCart();
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
                else if (CurrentViewContext.OrderType == OrderRequestType.NewOrder.GetStringValue())
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
                else
                    RedirectToRenewalOrder(_applicantOrderCart);
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

            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,  ChildControls.OrderPayment},
                                                                    {"TenantId", CurrentViewContext.TenantId.ToString()}
                                                                 };

            Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

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

                Dictionary<String, String> queryString;
                if (Convert.ToString(_applicantOrderCart.OrderRequestType) == OrderRequestType.RenewalOrder.GetStringValue()
                    || Convert.ToString(_applicantOrderCart.OrderRequestType) == OrderRequestType.ChangeSubscription.GetStringValue()
                    //UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
                    || (String.Compare(_applicantOrderCart.OrderRequestType, OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) != AppConsts.NONE
                        && _applicantOrderCart.IsReadOnly))
                {
                    //change done for UAt-827 Applicant Dashboard Redesign.
                    //if (applicantOrderCart.ParentControlType == AppConsts.DASHBOARD)
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

        //protected void chkRushOrder_CheckedChanged(object sender, EventArgs e)
        //{
        //    _applicantOrderCart = GetApplicantOrderCart();
        //    if (chkRushOrder.Checked)
        //    {
        //        if (cmbPaymentModes.SelectedValue != this.PaymentMode_InvoiceId.ToString() || (cmbPaymentModes.SelectedValue == this.PaymentMode_InvoiceId.ToString()
        //            && this.ShowRushOrderForInvioce))
        //        {
        //            divRush.Visible = true;
        //            _applicantOrderCart.IsRushOrderIncluded = true;

        //            //BS, UAT-264
        //            Decimal _netPrice = (_applicantOrderCart.CurrentPackagePrice.Value - _applicantOrderCart.SettleAmount)
        //                                + Convert.ToDecimal(_applicantOrderCart.RushOrderPrice.Trim());
        //            _applicantOrderCart.GrandTotal = _netPrice <= AppConsts.NONE ? AppConsts.NONE : _netPrice;
        //        }

        //        if (_applicantOrderCart.GrandTotal == AppConsts.NONE)
        //        {
        //            txtTotalPrice.Text = Convert.ToString(AppConsts.NONE);
        //            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowHidePaymentType(false);", true);

        //        }
        //        else if (((_applicantOrderCart.GrandTotal - Convert.ToDecimal(_applicantOrderCart.RushOrderPrice.Trim())) <= 0
        //                    && !this.ShowRushOrderForInvioce))
        //        {
        //            txtTotalPrice.Text = Convert.ToString(_applicantOrderCart.GrandTotal);
        //            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowHidePaymentType(true);", true);

        //            Telerik.Web.UI.RadComboBoxItem invItem = cmbPaymentModes.FindItemByValue(this.PaymentMode_InvoiceId.ToString());
        //            if (invItem.IsNotNull())
        //            {
        //                cmbPaymentModes.Items.Remove(invItem);
        //            }
        //        }
        //        else
        //        {
        //            txtTotalPrice.Text = Convert.ToString(_applicantOrderCart.GrandTotal);
        //            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowHidePaymentType(true);", true);
        //        }
        //    }
        //    else
        //    {
        //        divRush.Visible = false;
        //        _applicantOrderCart.IsRushOrderIncluded = false;
        //        _applicantOrderCart.GrandTotal = Convert.ToDecimal(_applicantOrderCart.Amount.Trim());

        //        if (_applicantOrderCart.GrandTotal == AppConsts.NONE)
        //        {
        //            txtTotalPrice.Text = Convert.ToString(AppConsts.NONE);
        //            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowHidePaymentType(false);", true);
        //        }
        //        else if (_applicantOrderCart.GrandTotal > AppConsts.NONE)
        //        {
        //            txtTotalPrice.Text = Convert.ToString(_applicantOrderCart.GrandTotal);
        //            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowHidePaymentType(true);", true);

        //            if (CurrentViewContext.lstPaymentOptions.IsNull())
        //            {
        //                CurrentViewContext.DPPSId = _applicantOrderCart.lstApplicantOrder.Select(cond => cond).FirstOrDefault().DPPS_Id.FirstOrDefault();
        //                //_presenter.GetTenantId();
        //                Presenter.GetPaymentOptions();
        //            }

        //            String selVal = cmbPaymentModes.SelectedValue;

        //            cmbPaymentModes.DataSource = CurrentViewContext.lstPaymentOptions;
        //            cmbPaymentModes.DataBind();

        //            Telerik.Web.UI.RadComboBoxItem cmbItem = cmbPaymentModes.FindItemByValue(selVal);
        //            if (cmbItem.IsNotNull())
        //            {
        //                cmbPaymentModes.SelectedValue = selVal;
        //            }
        //        }
        //    }
        //    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, _applicantOrderCart);
        //}

        //protected void cmbPaymentModes_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    if ((cmbPaymentModes.SelectedValue != this.PaymentMode_InvoiceId.ToString() || (cmbPaymentModes.SelectedValue == this.PaymentMode_InvoiceId.ToString() && this.ShowRushOrderForInvioce)) && ShowRushOrder)
        //    {
        //        chkRushOrder.Visible = true;
        //        dvRushOrderSrvc.Visible = true;
        //        divRushOrder.Visible = true;
        //    }
        //    else
        //    {
        //        chkRushOrder.Visible = false;
        //        chkRushOrder.Checked = false;
        //        dvRushOrderSrvc.Visible = false;
        //        divRushOrder.Visible = false;
        //        chkRushOrder_CheckedChanged(null, null);
        //    }
        //}

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
            //CBI || CABS
            if (CurrentViewContext.IsLocationServiceTenant)
            {
                if (!CurrentViewContext.lstSuffixes.IsNullOrEmpty())
                {
                    if (_orgUserProfile.UserTypeID > 0 && !_orgUserProfile.UserTypeID.IsNullOrEmpty())
                    {
                        if (!CurrentViewContext.lstSuffixes.Where(cond => cond.SuffixID == _orgUserProfile.UserTypeID).FirstOrDefault().Suffix.IsNullOrEmpty())
                            lblLastName.Text = _orgUserProfile.UserTypeID.IsNullOrEmpty() ? _orgUserProfile.LastName : _orgUserProfile.LastName + " - " + CurrentViewContext.lstSuffixes.Where(cond => cond.SuffixID == _orgUserProfile.UserTypeID).FirstOrDefault().Suffix;
                        else
                            lblLastName.Text = _orgUserProfile.LastName;


                    }

                    //  lblSuffix.Text = _orgUserProfile.UserTypeID.IsNullOrEmpty() ? String.Empty : CurrentViewContext.lstSuffixes.Where(cond => cond.SuffixID == _orgUserProfile.UserTypeID).FirstOrDefault().Suffix;
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
                    lblShowZIPAndPostalCode.Text = Resources.Language.POSTALCODE;
                }
                else
                {
                    lblShowZIPAndPostalCode.Text = Resources.Language.ZIPCODE;
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
                if (CurrentViewContext.IsLocationServiceTenant && resHisoryProfile.StateName.IsNullOrEmpty())
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

            //Hide Rush Order Services if Payment Type is only Invoice
            //if (CurrentViewContext.lstPaymentOptions.IsNotNull() && CurrentViewContext.lstPaymentOptions.Count() == AppConsts.ONE
            //    && CurrentViewContext.lstPaymentOptions.Any(cond => cond.Code == PaymentOptions.Invoice.GetStringValue()) && !this.ShowRushOrderForInvioce)
            //{
            //    divRushOrder.Visible = false;
            //    divRush.Visible = false;
            //}
            ////If Rush Order is disabled, calculate Grand Total
            //if (this.ShowRushOrderForInvioce)
            //{
            //    Decimal _netPrice = (_applicantOrderCart.CurrentPackagePrice.Value - _applicantOrderCart.SettleAmount)
            //                                + Convert.ToDecimal(_applicantOrderCart.RushOrderPrice.Trim());
            //    _applicantOrderCart.GrandTotal = _netPrice <= AppConsts.NONE ? AppConsts.NONE : _netPrice;
            //    txtTotalPrice.Text = Convert.ToString(_applicantOrderCart.GrandTotal);
            //}
            //SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.APPLICANT_ORDER_CART, _applicantOrderCart);
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
                                                                    {AppConsts.CHILD,  ChildControls.ApplicantDisclaimerPage}
                                                                 };
            Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

        }

        private void RedirectToRequiredDocumentationPage()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.CHILD,  ChildControls.ApplicantRequiredDocumentationPage}
                                                                 };
            Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

        }
        #endregion

        private void RedirectToOrderConfirmation()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.CHILD,  ChildControls.ApplicantOrderConfirmation}
                                                                 };
            Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

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
                                                                    { AppConsts.CHILD,  ChildControls.ApplicantPendingOrder},
                                                                    { "TenantId", CurrentViewContext.TenantId.ToString()}
                                                                 };
            Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        }

        private void RedirectToRenewalOrder(ApplicantOrderCart applicantOrderCart)
        {
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            //UAT-1560
            Session.Remove(AppConsts.DISCLAIMER_ACCEPTED);
            Session.Remove(AppConsts.REQUIRED_DOCUMENTATION_ACCEPTED);
            Dictionary<String, String> queryString = new Dictionary<String, String>()
                                                         {
                                                            {"OrderId",applicantOrderCart.PrevOrderId.ToString()},
                                                            { "Child",  ChildControls.RenewalOrder}
                                                         };
            //Response.Redirect(String.Format("~/Main/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
            Response.Redirect(String.Format("Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
        }


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
                                                                    { AppConsts.CHILD, ChildControls.CustomFormLoad},
                                                                    {"CustomFormId",Convert.ToString(_formToLoad)},
                                                                    {"IsPrevious","1"},
                                                                    {"TenantId", CurrentViewContext.TenantId.ToString()}
                                                                 };
                    Response.Redirect(String.Format("~/BkgOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));

                    #endregion
                }
            }
            #endregion
            else
            {
                #region UAT - 4331 : change schedule appointment to step 2 of order flow
                ////// Comment previous code and replace with new one as per the changes required for UAT -4331

                Dictionary<String, String> queryString = new Dictionary<String, String>();

                //Redirect to Applicant Profile screen (in case of Archived Orders)
                if (applicantOrderCart.FingerPrintData.IsFromArchivedOrderScreen)
                {
                    applicantOrderCart.AddOrderStageTrackID(OrderStages.ApplicantProfile);
                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,  ChildControls.ApplicantProfile}
                                                                 };
                    Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                }
                else
                {
                    var _formToLoad = applicantOrderCart.lstFormExecuted.LastOrDefault();
                    // Redirect to Custom Forms
                    applicantOrderCart.AddOrderStageTrackID(OrderStages.CustomForms);
                    applicantOrderCart.IsEditMode = true;

                    queryString = new Dictionary<String, String>
                                                                  {
                                                                     { AppConsts.CHILD, ChildControls.CustomFormLoad},
                                                                     {"CustomFormId",Convert.ToString(_formToLoad)},
                                                                     { "TenantId", CurrentViewContext.TenantId.ToString()},
                                                                     {"IsPrevious","1"}
                                                                  };
                }
                Response.Redirect(String.Format("~/BkgOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));



                //Dictionary<String, String> queryString = new Dictionary<String, String>()
                //                                         {
                //                                           // {"OrderId",applicantOrderCart.PrevOrderId.ToString()},
                //                                            { "Child",  ChildControls.APPLICANT_APPOINTMENT_SCHEDULE}
                //                                         };
                ////Response.Redirect(String.Format("~/Main/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                //Response.Redirect(String.Format("~/FingerPrintSetUp/Default.aspx?args={0}", queryString.ToEncryptedQueryString()), false);
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
                _applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = AppConsts.TWO;
            }
            applicantOrderCart.EDrugScreeningRegistrationId = null;
            _applicantOrderCart.AddOrderStageTrackID(OrderStages.ApplicantProfile);
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "TenantId", CurrentViewContext.TenantId.ToString()},
                                                                    {AppConsts.CHILD , ChildControls.ApplicantProfile}
                                                                 };
            String url = String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
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
                //UAT-2862: Always return to Disclaimer page.
                //if (_applicantOrderCart.IsCompliancePackageSelected)
                //{
                // To avoid Redirection again by the browser back button navigation check method
                applicantOrderCart.AddOrderStageTrackID(OrderStages.Disclosure); //UAT - 5184
                _childControl = ChildControls.ApplicantDisclosurePage; //UAT - 5184
                //_childControl = ChildControls.ApplicantDisclaimerPage;
                //}
                //else
                //{
                //    // To avoid Redirection again by the browser back button navigation check method
                //    applicantOrderCart.AddOrderStageTrackID(OrderStages.Disclosure);
                //    _childControl = ChildControls.ApplicantDisclosurePage;
                //}
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,_childControl  }
                                                                 };
                Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
            }
            else
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                applicantOrderCart.AddOrderStageTrackID(OrderStages.RequiredDocumentation);
                queryString = new Dictionary<String, String>
                                                         {
                                                            { AppConsts.CHILD,  ChildControls.ApplicantRequiredDocumentationPage}
                                                         };
                Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
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
                        //Added check in UAT-4498//
                        //if (!Convert.ToBoolean(ordrLneItm.Element("IsDummyLineItem").Value))
                        //{
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
                        //}
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
                Response.Redirect(AppConsts.APPLICANT_MAIN_PAGE_NAME);
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
               // if (applicantOrderCart.IsLocationServiceTenant) 
                    //cmdbarSubmit.OnSubmitClientClick = "";
            }
            else if (CurrentViewContext.OrderType == OrderRequestType.CompleteOrderByApplicant.GetStringValue())
            {
                cmdbarSubmit.ClearButtonText = Resources.Language.NEXT;
                cmdbarSubmit.SaveButtonText = Resources.Language.CNCL;
            }
            else
            {
                //cmdbarSubmit.SubmitButtonText = AppConsts.RESTART_ORDER_BUTTON_TEXT;
                //cmdbarSubmit.ClearButtonText = AppConsts.NEXT_BUTTON_TEXT;

                cmdbarSubmit.SubmitButtonText = Resources.Language.RSTRTORDR;
                cmdbarSubmit.ClearButtonText = Resources.Language.NEXT;
            }
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
                            // Dummy line item check added in UAT-4498.

                            List<Boolean> _lstIsDummyLineItem = new List<Boolean>();


                            //foreach (var item in _orderLineItems)
                            //{
                            //    if (!item.Element("IsDummyLineItem").IsNullOrEmpty() && !item.Element("IsDummyLineItem").Value.IsNullOrEmpty())
                            //    {
                            //        _lstIsDummyLineItem.Add(XmlConvert.ToBoolean(item.Element("IsDummyLineItem").Value));
                            //    }
                            //    else
                            //    {
                            //        _lstIsDummyLineItem.Add(false);
                            //    }
                            //}
                            _orderLineItems.ForEach(x =>
                                                    _lstIsDummyLineItem.Add(!x.Element("IsDummyLineItem").IsNullOrEmpty()
                                                                && !x.Element("IsDummyLineItem").Value.IsNullOrEmpty() ? XmlConvert.ToBoolean(x.Element("IsDummyLineItem").Value)
                                                          : false)
                                                   );

                            if (!_orderLineItems.IsNullOrEmpty() && _lstIsDummyLineItem.Any(x => x == false))
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


        AppointmentSlotContract IOrderReviewView.AppointmentSlotContract
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

            if (applicantOrderCart.IsLocationServiceTenant && applicantOrderCart.FingerPrintData.IsNotNull() && applicantOrderCart.FingerPrintData.IsFromArchivedOrderScreen)
            {
                cmdbarEditProfile.Visible = false;
            }

            #endregion

            if (applicantOrderCart.IsLocationServiceTenant && applicantOrderCart.FingerPrintData.IsNotNull() && !applicantOrderCart.FingerPrintData.IsFromArchivedOrderScreen)
            {
                if (applicantOrderCart.FingerPrintData.IsOutOfState)
                {
                    dvOutOfStateAppointmentDetails.Visible = true;
                }
                else
                {
                    //To hide/show Applicants appointment detail section on condition basis. Prachee

                    if (!(!applicantOrderCart.FingerPrintData.IsPrinterAvailable
                        && applicantOrderCart.FingerPrintData.IsFingerPrintAndPassPhotoService
                         && !applicantOrderCart.lstApplicantOrder[0].lstPackages.Any(z => !z.IsOrderHistory)))
                    {
                        dvAppointmentDetails.Visible = true;
                        BindAppointmentData();
                    }
                }
                hdnIsLocTen.Value = applicantOrderCart.IsLocationServiceTenant.ToString();
            }
            else
            {
                dvAppointmentDetails.Visible = false;
            }
        }

        private void BindAppointmentData()
        {
            //Int32 OrderId = _applicantOrderCart.lstApplicantOrder[0].OrderId;
            //Presenter.GetAppointmentData(OrderId);
            if (!applicantOrderCart.FingerPrintData.IsNullOrEmpty())
            {
                //TimeSpan startTime = applicantOrderCart.FingerPrintData.StartTime;
                //TimeSpan endTime = CurrentViewContext.AppointmentSlotContract.SlotEndTimeTimeSpanFormat;
                //DateTime slotStartDateTime = applicantOrderCart.FingerPrintData.SlotDate;
                //DateTime slotEndDateTime = applicantOrderCart.FingerPrintData.SlotDate;

                lblLocationName.Text = String.IsNullOrEmpty(applicantOrderCart.FingerPrintData.LocationName) ? String.Empty : applicantOrderCart.FingerPrintData.LocationName;
                lblLocationAddress.Text = String.IsNullOrEmpty(applicantOrderCart.FingerPrintData.LocationAddress) ? String.Empty : applicantOrderCart.FingerPrintData.LocationAddress;
                lblAppointmentTiming.Text = applicantOrderCart.FingerPrintData.SlotDate.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture(LanguageCultures.ENGLISH_CULTURE.GetStringValue())) + " (" + applicantOrderCart.FingerPrintData.StartTime.ToString("hh:mm tt") + " - " + applicantOrderCart.FingerPrintData.EndTime.ToString("hh:mm tt") + ") ";
                lblSiteDescription.Text = String.IsNullOrEmpty(applicantOrderCart.FingerPrintData.LocationDescription) ? String.Empty : applicantOrderCart.FingerPrintData.LocationDescription;
                hdnLocId.Value = applicantOrderCart.FingerPrintData.LocationId.ToString();

                //UAT-3761

                if (applicantOrderCart.FingerPrintData.IsEventCode)
                {
                    dvLocAdd.Visible = false;
                    btnViewLocImage.Visible = false;
                    dvChangeApp.Visible = false;
                    lblLocationName.Text = String.IsNullOrEmpty(applicantOrderCart.FingerPrintData.EventName) ? String.Empty : applicantOrderCart.FingerPrintData.EventName;
                    lblSiteDescription.Text = String.IsNullOrEmpty(applicantOrderCart.FingerPrintData.EventDescription) ? String.Empty : applicantOrderCart.FingerPrintData.EventDescription;


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

        protected void btnEditAppointment_Click(object sender, EventArgs e)
        {
            try
            {
                _applicantOrderCart = GetApplicantOrderCart();
                _applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = AppConsts.TWO; //// UAT - 4331
                _applicantOrderCart.AddOrderStageTrackID(OrderStages.CustomFormsCompleted);
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                    {
                      {AppConsts.CHILD , ChildControls.APPLICANT_APPOINTMENT_SCHEDULE},
                      { "TenantId", CurrentViewContext.TenantId.ToString()}
                    };
                String url = (String.Format("~/FingerPrintSetUp/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
                Response.Redirect(url);
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

        protected void btnEditMailingAddress_Click(object sender, EventArgs e)
        {
            try
            {
                RedirectToApplicantProfile();
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




        //protected void btnViewLocImage_Click(object sender, EventArgs e)
        //{
        //    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "openImageSliderPopup();", true);
        //}


    }
}



