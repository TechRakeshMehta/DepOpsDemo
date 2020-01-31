using Business.RepoManagers;
using CoreWeb.FingerPrintSetUp.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using ExternalVendors.ClearStarVendor;
using INTSOF.Contracts;
using INTSOF.UI.Contract;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace CoreWeb.ComplianceOperations.UserControl
{
    public partial class ModifyShippingInfo : BaseUserControl, IModifyShippingInfoView
    {
        #region Variables

        #region Public Variables
        private ModifyShippingInfoPresenter _presenter = new ModifyShippingInfoPresenter();
        private Int32 _tenantId;
        #endregion

        #region Private Variables
        private Int32 OrderID;
        private Int32 TenantID;
        private bool IsFromRescheduleScreen;
        private bool IsFromNewOrderClick;
        private bool IsCompleteYourPayment;

        private String _viewType;
        Address addrress = null;
        //private String PopupTitle = "Modify Shipping";
        private ApplicantOrderCart _applicantOrderCart;
        private String FirstClassMail = "First Class Mail";
        private String PageType;
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public ModifyShippingInfoPresenter Presenter
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

        public int CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IModifyShippingInfoView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public String ParentControl
        {
            get
            {
                if (ViewState["ParentControl"].IsNotNull())
                {
                    return Convert.ToString(ViewState["ParentControl"]);
                }
                return null;
            }
            set
            {
                ViewState["ParentControl"] = value;
            }
        }
        public int SelectedTenantId
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

        int IModifyShippingInfoView.TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    //_tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }
        public int OrderId
        {
            get
            {
                if (!ViewState["OrderId"].IsNull())
                {
                    return (Int32)ViewState["OrderId"];
                }
                return 0;
            }
            set
            {
                ViewState["OrderId"] = value;
            }
        }

        public int LocationId
        {
            get
            {
                if (!ViewState["LocationId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["LocationId"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["LocationId"] = value;
            }
        }

        public Guid? AddressHandleId
        {
            get
            {
                if (ViewState["AddressHandleId"] != null)
                    return (Guid)ViewState["AddressHandleId"];
                return Guid.NewGuid();
            }
            set
            {
                ViewState["AddressHandleId"] = value;
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
                if (value != null)
                    AddressHandleId = value.AddressHandleID;
            }
        }

        Boolean IModifyShippingInfoView.IsLocationServiceTenant
        {
            get
            {
                if (!String.IsNullOrEmpty(Convert.ToString(ViewState["IsLocationServiceTenant"])))
                    return (Boolean)ViewState["IsLocationServiceTenant"];
                return false;
            }
            set
            {
                ViewState["IsLocationServiceTenant"] = value;
                hdnIsLocationTenant.Value = Convert.ToString(value);
            }
        }

        List<ServiceFeeItemRecordContract> IModifyShippingInfoView.lstMailingOptionsWithPrice
        {
            get
            {
                if (!ViewState["lstMailingOptionsWithPrice"].IsNullOrEmpty())
                    return (List<ServiceFeeItemRecordContract>)ViewState["lstMailingOptionsWithPrice"];
                return new List<ServiceFeeItemRecordContract>();
            }
            set
            {
                ViewState["lstMailingOptionsWithPrice"] = value;
            }
        }
        #endregion

        #region Private Properties

        #endregion

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
            decimal? selectedMailingOptionPrice = Convert.ToDecimal(0.00);
            SelectedMailingData selectedMailingDatas = new SelectedMailingData();
            //if (Request.QueryString["OrderID"] != null && !Request.QueryString["OrderID"].Trim().Equals(""))
            //    OrderID = Convert.ToInt32(Request.QueryString["OrderID"]);
            //if (Request.QueryString["tenantId"] != null && !Request.QueryString["tenantId"].Trim().Equals(""))
            //    TenantID = Convert.ToInt32(Request.QueryString["tenantId"]);
            (this.Page as CoreWeb.ComplianceOperations.Views.ComplianceOperationsDefault).SetModuleTitle(Resources.Language.MODIFYSHIPPINGADDRESS);
            _applicantOrderCart = GetApplicantOrderCart();
            CaptureQueryString();
            if (PageType == "ModifyShipping")
            {
                _applicantOrderCart.IsModifyShipping = true;
            }
            if (PageType == "OrderPayment" || PageType == "ScheduleAppointment")
            {
                btnPrevious.Visible = true;
            }            
            addrress = Presenter.GetMailingAddressDetails(TenantID, OrderID);
            if (_applicantOrderCart.IsFromNewOrderClick || IsFromNewOrderClick || IsCompleteYourPayment)
            {
                selectedMailingDatas = Presenter.GetSelectedMailingOptionPrice(TenantID, OrderID);
                selectedMailingOptionPrice = selectedMailingDatas.SelectedMailingOptionPrice;
                hdnSelectedMailingOptionPrice.Value = selectedMailingOptionPrice.ToString();
                hdnSelectedMailingOptionId.Value = selectedMailingDatas.SelectedMailingOptionId.ToString();
            }

            //MailingAddress = Presenter.IsLocationServiceTenant(TenantID);
            Presenter.IsLocationServiceTenant();
            Presenter.OnViewInitialized();
            MailingAddress.IsLocationServiceTenant = CurrentViewContext.IsLocationServiceTenant;
            //OrderPayment.IsModifyShipping = true;
            //OrderPayment.MailingPrice = 10;

            if (!IsPostBack)
            {
                BindMailingOption(selectedMailingDatas);
                if (!IsFromRescheduleScreen)
                {
                    BindAddressData();
                }
                dvAddress.Visible = true;
            }
        }

        #endregion

        #region Button Events

        protected void btnNext_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            Decimal mailingPrice = Convert.ToDecimal(0.00);
            //dvMailingOption.Visible = true;
            dvAddress.Visible = false;
            btnNext.Visible = false;

            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);

            PreviousAddressContract mailingAddress = new PreviousAddressContract();
            mailingAddress.Address1 = txtAddress.Text;
            mailingAddress.ZipCodeID = MailingAddress.MasterZipcodeID.Value;
            mailingAddress.Zipcode = MailingAddress.RSLZipCode;
            mailingAddress.CityName = MailingAddress.RSLCityName;
            mailingAddress.StateName = MailingAddress.RSLStateName;
            mailingAddress.Country = MailingAddress.RSLCountryName;
            mailingAddress.CountryId = MailingAddress.RSLCountryId;
            mailingAddress.MailingOptionId = cmbMailingOption.SelectedValue;
            if (cmbMailingOption.SelectedItem != null)
            {
                mailingAddress.MailingOptionPrice = cmbMailingOption.SelectedItem.Text;
            }
            mailingAddress.IsMailingChecked = false;
            _applicantOrderCart.MailingAddress = mailingAddress;

            //To fetch service code.
            XmlDocument xmlDoc = new XmlDocument();

            string BkgOrderServiceDetailsxml = Presenter.GetBkgOrderServiceDetails(_applicantOrderCart.lstApplicantOrder[0].OrderId);
            if (!BkgOrderServiceDetailsxml.IsNullOrEmpty())
            {
                xmlDoc.LoadXml(BkgOrderServiceDetailsxml);
                XmlNodeList elemlist = xmlDoc.GetElementsByTagName("ServiceType");

                for (int i = 0; i < elemlist.Count; i++)
                {
                    _applicantOrderCart.lstApplicantOrder[0].lstPackages[i].ServiceCode = elemlist[i].InnerText;
                }
            }

            if (IsFromRescheduleScreen)
            {
                dvAddress.Visible = false;
                dvMailingOption.Visible = false;
                _applicantOrderCart.IsModifyShipping = false;
                applicantOrderCart.IsFromReschedulingScreen = true;
                queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,  ChildControls.OrderPayment},
                                                                    {"TenantId", CurrentViewContext.TenantId.ToString()}
                                                                 };
                Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
            }

            if (applicantOrderCart.OrderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue())
            {
                //To Save updated mailing detail in address and addressext table in database.
                Guid mailingAddressHandleId = Presenter.UpdateMailingAddress(applicantOrderCart.MailingAddress);

            //To save mailing detail in XMl in cabs service order detail in database.
            if (!mailingAddressHandleId.IsNullOrEmpty())
                Presenter.UpdateMailingDetailXML(CurrentViewContext.TenantId, _applicantOrderCart.lstApplicantOrder[0].OrderId, mailingAddressHandleId, cmbMailingOption.SelectedValue, cmbMailingOption.SelectedItem.Text);

                foreach (var _item in _applicantOrderCart.lstApplicantOrder[0].lstPackages)
                {
                    if (_item.ServiceCode != BkgServiceType.SIMPLE.GetStringValue())
                    {    
                        char[] splitParams = new char[] { '(', ')' };

                        if (!mailingAddress.IsNullOrEmpty() || !mailingAddress.MailingOptionPrice.IsNullOrEmpty())
                        {
                            String[] MailingPrice1 = mailingAddress.MailingOptionPrice.Split(splitParams);
                            _item.TotalLineItemPrice = Convert.ToDecimal(MailingPrice1[1]);
                        }                        
                        _item.TotalBkgPackagePrice = _item.TotalLineItemPrice+_item.BasePrice;
                    }
                }

                if (_applicantOrderCart.lstPrevAddresses.Count() > 0)
                {
                    _applicantOrderCart.lstPrevAddresses[0].Address1 = mailingAddress.Address1;
                    _applicantOrderCart.lstPrevAddresses[0].Address2 = mailingAddress.Address2;
                    _applicantOrderCart.lstPrevAddresses[0].CityName = mailingAddress.CityName;
                    _applicantOrderCart.lstPrevAddresses[0].Country = mailingAddress.Country; 
                    _applicantOrderCart.lstPrevAddresses[0].StateName= mailingAddress.StateName;
                    _applicantOrderCart.lstPrevAddresses[0].Zipcode = mailingAddress.Zipcode;
                     //_applicantOrderCart.lstPrevAddresses[0].PreviousAddress = string.Concat( mailingAddress.CityName, ",", mailingAddress.StateName, ",", mailingAddress.Country,",", "Zipcode -"+ mailingAddress.Zipcode);

                }
                    

            }
            //for next previous if we select same option set IsPaymentReqInMdfyShpng false 
            if (_applicantOrderCart.IsFromNewOrderClick || IsFromNewOrderClick || IsCompleteYourPayment)
            {
                if (cmbMailingOption.SelectedValue == hdnSelectedMailingOptionId.Value) //except return to sender 
                {
                    _applicantOrderCart.IsPaymentReqInMdfyShpng = false;
                    _applicantOrderCart.MailingPrice = Convert.ToDecimal(hdnSelectedMailingOptionPrice.Value);
                }
            }
            else //Incase of return to sender , for next previous initially set IsPaymentReqInMdfyShpng false
            {
                _applicantOrderCart.IsPaymentReqInMdfyShpng = false;
            }
               
            
            //Presenter.SaveOrderPaymentInvoice(CurrentViewContext.TenantId, _applicantOrderCart.OrderId, CurrentViewContext.CurrentLoggedInUserId, _applicantOrderCart.IsModifyShipping);
            if (applicantOrderCart.OrderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue() )
            {
                queryString = new Dictionary<String, String>
                                                         {
                                                            { AppConsts.CHILD,  ChildControls.OrderPayment}
                                                         };
                Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
            }
            if (!mailingAddress.MailingOptionPrice.Contains(FirstClassMail))
            {
                dvAddress.Visible = false;
                dvMailingOption.Visible = false;

                if (IsFromNewOrderClick && !hdnSelectedMailingOptionId.Value.IsNullOrEmpty())
                {
                    int hdnSelectedMlngOptionId = Convert.ToInt32(hdnSelectedMailingOptionId.Value);
                    applicantOrderCart.IsFromNewOrderClick = IsFromNewOrderClick;
                    char[] splitParams = new char[] { '(', ')' };
                    decimal previousSlctdMailingPrice = Convert.ToDecimal(hdnSelectedMailingOptionPrice.Value);
                    if (!mailingAddress.IsNullOrEmpty() || !mailingAddress.MailingOptionPrice.IsNullOrEmpty())
                    {
                        String[] MailingPrice1 = mailingAddress.MailingOptionPrice.Split(splitParams);
                        mailingPrice = Convert.ToDecimal(MailingPrice1[1]);
                    }
                    mailingAddress.MailingOptionPriceOnly = mailingPrice;
                    if (!applicantOrderCart.IsModifyShippingPayment )
                    {
                        if (mailingPrice - previousSlctdMailingPrice > 0)
                        {
                            _applicantOrderCart.IsMailingOptionUpgraded = true;
                            _applicantOrderCart.IsPaymentReqInMdfyShpng = true;
                            _applicantOrderCart.MailingPrice = mailingPrice - previousSlctdMailingPrice;
                            //_applicantOrderCart.MailingPrice_full = mailingPrice;
                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,  ChildControls.OrderPayment},
                                                                    {"TenantId", CurrentViewContext.TenantId.ToString()}
                                                                 };
                        }
                        else
                        {
                            applicantOrderCart.AddOrderStageTrackID(OrderStages.CIMAccountSelection);
                            SaveModifyShippingData();
                            SendModifyShippingNotification();
                            queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.CHILD,  @"~/ComplianceOperations/UserControl/ModifyShippingConfirmation.ascx"}
                                                                 };

                        }
                    }
                    if (_applicantOrderCart.IsModifyShippingPayment)
                    {
                        if (hdnSelectedMlngOptionId != Convert.ToInt32(mailingAddress.MailingOptionId))
                        {
                                _applicantOrderCart.MailingPrice = mailingAddress.MailingOptionPriceOnly;
                        }
                        else if (hdnSelectedMlngOptionId == Convert.ToInt32(mailingAddress.MailingOptionId))
                        {
                            _applicantOrderCart.MailingPrice = Presenter.GetSentForOnlinePaymentAmount(TenantID, OrderID);
                            
                        }
                        _applicantOrderCart.IsMailingOptionUpgraded = true;
                        _applicantOrderCart.IsPaymentReqInMdfyShpng = true;
                        queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,  ChildControls.OrderPayment},
                                                                    {"TenantId", CurrentViewContext.TenantId.ToString()}
                                                                 };
                    }
                    String url = String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
                else
                {
                    _applicantOrderCart.IsPaymentReqInMdfyShpng = true;
                    queryString = new Dictionary<String, String>
                                                                 {
                                                                    { AppConsts.CHILD,  ChildControls.OrderPayment},
                                                                    {"TenantId", CurrentViewContext.TenantId.ToString()}
                                                                 };
                    String url = String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
            }
            else
            {
                Order _order = new Order();
                _order = ComplianceDataManager.GetOrderById(_tenantId, OrderID);
                applicantOrderCart.lstApplicantOrder[0].OrderId = OrderID;
                applicantOrderCart.lstApplicantOrder[0].OrderNumber = _order.OrderNumber;
                applicantOrderCart.AddOrderStageTrackID(OrderStages.CIMAccountSelection);
                SaveModifyShippingData();
                dvAddress.Visible = false;
                SendModifyShippingNotification();
                //Presenter.SaveOrderPaymentInvoice(CurrentViewContext.TenantId, _applicantOrderCart.OrderId, CurrentViewContext.CurrentLoggedInUserId, _applicantOrderCart.IsModifyShipping);

                queryString = new Dictionary<String, String>
                                                                 {
                                                                    {AppConsts.CHILD,  @"~/ComplianceOperations/UserControl/ModifyShippingConfirmation.ascx"}
                                                                 };
                Response.Redirect(String.Format("~/ComplianceOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString()));
            }
        }

        protected void btnNext2_Click(object sender, EventArgs e)
        {

            //dvOrderPayment.Visible = true;
            //dvAddress.Visible = false;
            ////dvMailingOption.Visible = false;
            //ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            //applicantOrderCart.TenantId = CurrentViewContext.TenantId;
            //applicantOrderCart.OrderId = OrderID;
            //Presenter.SaveModifyShippingData(applicantOrderCart);
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            if (PageType == "OrderPayment" || PageType == "ScheduleAppointment")
            {
                queryString = new Dictionary<String, String> {
                                        { AppConsts.CHILD, ChildControls.APPLICANT_APPOINTMENT_SCHEDULE},
                                         { "TenantId", TenantID.ToString()},
                                        { "IsFromOrderHistoryScreen", true.ToString()},
                                    };
                String Url = String.Format("~/FingerPrintSetUp/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                Response.Redirect(Url, true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
            String url = String.Format(AppConsts.APPLICANT_MAIN_PAGE_NAME);
            Response.Redirect(url);
        }

        public void SaveModifyShippingData()
        {
            //dvOrderPayment.Visible = false;3
            dvAddress.Visible = false;
            //dvMailingOption.Visible = false;
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            applicantOrderCart.TenantId = CurrentViewContext.TenantId;
            applicantOrderCart.OrderId = OrderID;
            Presenter.SaveModifyShippingData(applicantOrderCart);
        }

        public void SendModifyShippingNotification()
        {
            Order _order = new Order();
            _order = ComplianceDataManager.GetOrderById(_tenantId, OrderID);
            ApplicantOrderCart applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            Presenter.SendModifyShippingNotification(OrderID, _order.OrderNumber, applicantOrderCart.MailingAddress);
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OnCloseModifyShippingPopup();", true);
            Session.Remove(ResourceConst.APPLICANT_ORDER_CART);
            Session.Remove(ResourceConst.APPLICANT_DRUG_SCREENING);
            String url = String.Format(AppConsts.APPLICANT_MAIN_PAGE_NAME);
            Response.Redirect(url);
        }

        #endregion

        private void BindAddressData()
        {
            _applicantOrderCart = GetApplicantOrderCart();
            bool isPrinterAvailableAtOldLoc = Presenter.IsPrinterAvailableAtOldLoc(OrderID);
            if (addrress != null && _applicantOrderCart.MailingAddress.IsNullOrEmpty())
            {
                Entity.ClientEntity.Address address = addrress;
                MailingAddress.MasterZipcodeID = address.ZipCodeID;
                if (address.ZipCodeID == 0 && address.AddressExts.IsNotNull() && address.AddressExts.Count > 0)
                {
                    Entity.ClientEntity.AddressExt addressExt = address.AddressExts.FirstOrDefault();
                    if (CurrentViewContext.IsLocationServiceTenant)
                    {
                        MailingAddress.RSLCountryId = addressExt.AE_CountryID;
                    }
                    else
                    {
                        MailingAddress.RSLCountryId = addressExt.AE_CountryID;
                    }
                    MailingAddress.RSLStateName = addressExt.AE_StateName;
                    MailingAddress.RSLCityName = addressExt.AE_CityName;
                    MailingAddress.RSLZipCode = addressExt.AE_ZipCode;
                    txtAddress.Text = addressExt.Address.Address1;
                }
            }
            else
            {
                if (!(_applicantOrderCart.MailingAddress.IsNullOrEmpty()))
                {
                    txtAddress.Text = _applicantOrderCart.MailingAddress.Address1;
                    MailingAddress.MasterZipcodeID = _applicantOrderCart.MailingAddress.ZipCodeID;
                    MailingAddress.RSLZipCode = _applicantOrderCart.MailingAddress.Zipcode;
                    MailingAddress.RSLCityName = _applicantOrderCart.MailingAddress.CityName;
                    MailingAddress.RSLCountryId = _applicantOrderCart.MailingAddress.CountryId;
                    MailingAddress.RSLStateName = _applicantOrderCart.MailingAddress.StateName;
                    cmbMailingOption.SelectedValue = _applicantOrderCart.MailingAddress.MailingOptionId;
                }

                else if (_applicantOrderCart.OrderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue() && isPrinterAvailableAtOldLoc)
                {
                    var addressDetail = Presenter.GetAddressData(TenantID, OrderID);
                    if (!addressDetail.IsNullOrEmpty())
                    {
                        txtAddress.Text = addressDetail.Address1;
                        MailingAddress.MasterZipcodeID = addressDetail.ZipCodeID;
                        MailingAddress.RSLZipCode = addressDetail.Zipcode;
                        MailingAddress.RSLCityName = addressDetail.CityName;
                        MailingAddress.RSLCountryId = addressDetail.CountryId;
                        MailingAddress.RSLStateName = addressDetail.StateName;
                        // cmbMailingOption.SelectedValue = "1";
                    }
                }
                else if (_applicantOrderCart.OrderRequestType == OrderRequestType.CompleteOrderByApplicant.GetStringValue() && !isPrinterAvailableAtOldLoc)
                {
                    var shippingAddressDetail = Presenter.GetShippingAddressData(TenantID, OrderID);
                    if (!shippingAddressDetail.IsNullOrEmpty())
                    {
                        txtAddress.Text = shippingAddressDetail.Address1;
                        MailingAddress.MasterZipcodeID = shippingAddressDetail.ZipCodeID;
                        MailingAddress.RSLZipCode = shippingAddressDetail.Zipcode;
                        MailingAddress.RSLCityName = shippingAddressDetail.CityName;
                        MailingAddress.RSLCountryId = shippingAddressDetail.CountryId;
                        MailingAddress.RSLStateName = shippingAddressDetail.StateName;
                        cmbMailingOption.SelectedValue = shippingAddressDetail.MailingOptionId;
                        //cmbMailingOption.SelectedValue = "1";
                    }
                }
            }
        }

        private void CaptureQueryString()
        {
            //if (Request.QueryString["OrderID"] != null && !Request.QueryString["OrderID"].Trim().Equals(""))
            //    OrderID = Convert.ToInt32(Request.QueryString["OrderID"]);
            //if (Request.QueryString["tenantId"] != null && !Request.QueryString["tenantId"].Trim().Equals(""))
            //    TenantID = Convert.ToInt32(Request.QueryString["tenantId"]);

            Dictionary<String, String> args = new Dictionary<String, String>();
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (args.ContainsKey("OrderID"))
                {
                    OrderID = Convert.ToInt32(args["OrderID"].ToString());
                }
                if (args.ContainsKey("tenantId"))
                {
                    TenantID = Convert.ToInt32(args["tenantId"].ToString());
                }
                if (args.ContainsKey("IsFromRescheduleScreen"))
                {
                    IsFromRescheduleScreen = Convert.ToBoolean(args["IsFromRescheduleScreen"].ToString());
                }
                if (args.ContainsKey("IsFromNewOrderClick"))
                {
                    IsFromNewOrderClick = Convert.ToBoolean(args["IsFromNewOrderClick"].ToString());
                }
                if (args.ContainsKey("PageType"))
                {
                    PageType = Convert.ToString(args["PageType"].ToString());
                }
                if (args.ContainsKey("ModifyShippingPayment"))
                {
                    _applicantOrderCart.IsModifyShippingPayment = Convert.ToBoolean(args["ModifyShippingPayment"].ToString());
                }
                if (args.ContainsKey("IsCompleteYourPayment"))
                {
                    IsCompleteYourPayment = Convert.ToBoolean(args["IsCompleteYourPayment"].ToString());
                }
            }
        }

        public void BindMailingOption(SelectedMailingData selectedMailingOption)
        {
            Presenter.GetMailingOption();
            cmbMailingOption.DataSource = CurrentViewContext.lstMailingOptionsWithPrice;
            if (selectedMailingOption.IsNotNull() && selectedMailingOption.SelectedMailingOptionPrice.IsNotNull() && selectedMailingOption.SelectedMailingOptionId.IsNotNull())
            {
                cmbMailingOption.DataSource = CurrentViewContext.lstMailingOptionsWithPrice.Where(x => x.SIFR_Amount >= selectedMailingOption.SelectedMailingOptionPrice);
                cmbMailingOption.SelectedValue = selectedMailingOption.SelectedMailingOptionId.ToString();
            }
            cmbMailingOption.DataBind();
        }

        private ApplicantOrderCart GetApplicantOrderCart()
        {
            if (_applicantOrderCart.IsNull())
            {
                _applicantOrderCart = (ApplicantOrderCart)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART);
            }
            return _applicantOrderCart;
        }
    }
}