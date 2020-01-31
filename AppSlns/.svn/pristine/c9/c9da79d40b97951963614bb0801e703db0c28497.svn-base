using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.Linq;
using Business.RepoManagers;
using CoreWeb.Shell;
using Entity;
using Entity.ClientEntity;
using ExternalVendors.ClearStarVendor;
using INTSOF.Contracts;
using INTSOF.UI.Contract;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using INTSOF.Utils.CommonPocoClasses;
using CoreWeb.IntsofSecurityModel;
using System.Drawing.Drawing2D;
using CoreWeb.ProfileSharing.Views;
using Newtonsoft.Json;
using System.Xml;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ComplianceOperationsDefault : BasePage, IDefaultView
    {
        #region VARIABLES

        #region Private Variables

        private DefaultViewPresenter _presenter = new DefaultViewPresenter();
        //Commented below code related to Print Receipt bug[SS]:[07/04/2016]
        //private static String _filePath = String.Empty;
        // private static String _fileIdentifier = String.Empty;

        #endregion

        #endregion

        #region PROPERTIES
        public static Color PenColor { get; set; }
        public static Color Background { get; set; }
        public static int Height { get; set; }
        public static int Width { get; set; }
        public static int PenWidth { get; set; }
        public static int FontSize { get; set; }
        public DefaultViewPresenter Presenter
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

        public IDefaultView CurrentViewContext
        {
            get { return this; }
        }


        #endregion

        #region EVENTS

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
        }

        /// <summary>
        /// Page OnInitComplete event.
        /// </summary>
        /// <param name="e">Event</param>
        protected override void OnInitComplete(EventArgs e)
        {

            try
            {
                base.dynamicPlaceHolder = phDynamic;
                base.OnInitComplete(e);
                //SetModuleTitle("Compliance");
                SetModuleTitle(Resources.Language.COMPILANCE);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Submits the Normal applicant Complio and AMS Order
        /// </summary>
        /// <param name="paymentModeId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static String SubmitOrder(String selectedPaymentModeData, Boolean isBillingInfoSameAsAccountInfo, String url)
        {
            var javaScriptSerializer = new JavaScriptSerializer();
            var _paymentModesData = javaScriptSerializer.Deserialize<List<ApplicantOrderPaymentOptions>>(selectedPaymentModeData);

            #region Variables
            ApplicantOrderDataContract _applicantOrderDataContract = new ApplicantOrderDataContract();


            String _invoiceNumbers = String.Empty;
            var _dicInvoiceNumber = new Dictionary<String, String>();
            Boolean isRushOrder = false;
            String _paymentModeCode = String.Empty;
            Boolean _isUpdateMainProfile = false;
            Int32 _prgPackageSubscriptionId = 0;
            Int32 _tenantId = SecurityManager.GetOrganizationUser(SysXWebSiteUtils.SessionService.OrganizationUserId).Organization.TenantID.Value;
            String _errorMessage = String.Empty;
             String _redirectUrlType = String.Empty;

            ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;
            FingerPrintAppointmentContract _fingerPrintAppointmentData = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_LOCATION_CART) as FingerPrintAppointmentContract;
            if (applicantOrderCart != null && _fingerPrintAppointmentData != null
                   && applicantOrderCart.IsLocationServiceTenant && !applicantOrderCart.FingerPrintData.BillingCode.IsNullOrEmpty()
                 && !applicantOrderCart.FingerPrintData.BillingCodeAmount.IsNullOrEmpty() && applicantOrderCart.FingerPrintData.BillingCodeAmount > AppConsts.NONE)
            { 
                Decimal BillingCodeAmount = applicantOrderCart.FingerPrintData.BillingCodeAmount;
                var paymentOptions = ComplianceDataManager.GetClientPaymentOptions(_tenantId);

                var billingCodeId = paymentOptions.Where(po => po.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()).First().PaymentOptionID;

                // generate exception in case of all payment using billing code and billing code amount is less than order total amount to be paid  
                Boolean allAmtByBillingCode = _paymentModesData.Any(x => x.poid != billingCodeId);
                Decimal TotalAmt = GetBackgroundPackagesPrice(applicantOrderCart);
                if (!allAmtByBillingCode && BillingCodeAmount < TotalAmt)
                {
                    throw new Exception("Generate exception in case of all payment using billing code and billing code amount is less than order total amount to be paid");
                }
            }

                if (applicantOrderCart != null && _fingerPrintAppointmentData!=null
                    && applicantOrderCart.IsLocationServiceTenant)
            {
                //FingerPrintAppointmentContract _fingerPrintAppointmentData = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_LOCATION_CART) as FingerPrintAppointmentContract;
                if (!_fingerPrintAppointmentData.IsOutOfState && !applicantOrderCart.FingerPrintData.IsFromArchivedOrderScreen)
                {
                    // _fingerPrintAppointmentData.ReserverSlotID
                    if (FingerPrintSetUpManager.IsReservedSlotExpired(_fingerPrintAppointmentData.ReserverSlotID, SysXWebSiteUtils.SessionService.OrganizationUserId, false))
                    {
                        //_applicantOrderCart = GetApplicantOrderCart();
                        applicantOrderCart.lstApplicantOrder[0].PreviousOrderStep = AppConsts.TWO; //// UAT - 4331
                        applicantOrderCart.AddOrderStageTrackID(OrderStages.CustomFormsCompleted);
                        Dictionary<String, String> queryString1 = new Dictionary<String, String>();
                        if (_fingerPrintAppointmentData.IsEventCode)
                        {
                            queryString1 = new Dictionary<String, String>
                                    {
                                        { AppConsts.CHILD, ChildControls.FINGER_PRINTDATA_CONTROL},
                                        { "TenantId", _tenantId.ToString()},
                                        {"IsFromOrderHistoryScreen",false.ToString()}, //// UAT - 4331
                                        {"IsReservedSlotExpired", true.ToString()}
                                    };
                        }
                        else
                        {
                            queryString1 = new Dictionary<String, String>
                            {
                              {AppConsts.CHILD , ChildControls.APPLICANT_APPOINTMENT_SCHEDULE},
                              {"TenantId", _tenantId.ToString()},
                              {"IsReservedSlotExpired", true.ToString()}
                            };
                        }
                        String url1 = (String.Format("../FingerPrintSetUp/Default.aspx?args={0}", queryString1.ToEncryptedQueryString()));
                        //Response.Redirect(url);
                        _redirectUrlType = "internal";
                        return javaScriptSerializer.Serialize(new { redirectUrl = url1, redirectUrlType = _redirectUrlType });
                    }
                }
            }

            Entity.ClientEntity.OrganizationUserProfile _orgUserProfile = new Entity.ClientEntity.OrganizationUserProfile();

            foreach (var applicantOrder in applicantOrderCart.lstApplicantOrder)
            {
                _orgUserProfile = applicantOrder.OrganizationUserProfile;
                _isUpdateMainProfile = applicantOrder.UpdatePersonalDetails;
            }

            if (!applicantOrderCart.lstApplicantOrder.IsNullOrEmpty() &&
                applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.IsNotNull())
            {
                BasePage.LogOrderFlowSteps("Default.aspx - STEP 1: Call to 'SubmitOrder' started for OrgUserId:" + applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserID);
            }

            //Start UAT-3850
            if (!applicantOrderCart.IsNullOrEmpty() && !applicantOrderCart.FingerPrintData.IsNullOrEmpty() && !applicantOrderCart.FingerPrintData.BillingCode.IsNullOrEmpty()
                 && !applicantOrderCart.FingerPrintData.BillingCodeAmount.IsNullOrEmpty() && applicantOrderCart.FingerPrintData.BillingCodeAmount > AppConsts.NONE)
            {
                _applicantOrderDataContract.IsBillingCodeAmountAvlbl = true;
                _applicantOrderDataContract.BillingCodeAmount = applicantOrderCart.FingerPrintData.BillingCodeAmount;
            }
            //End
            #region UAT-1578 : Addition of SMS notification
            //Added check do not save data in Applicant completing order process for "Sent for online payment" types.[UAT-1648]
            if (String.Compare(Convert.ToString(applicantOrderCart.OrderRequestType), OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) != AppConsts.NONE
                && String.Compare(Convert.ToString(applicantOrderCart.OrderRequestType), OrderRequestType.ModifyShipping.GetStringValue(), true) != AppConsts.NONE)
            {
                Boolean result = SaveUpdateSMSNotificationData(applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserID, applicantOrderCart.lstApplicantOrder[0].PhoneNumber,
                                         applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserID, applicantOrderCart.lstApplicantOrder[0].IsReceiveTextNotification);

                BasePage.LogOrderFlowSteps("Default.aspx - STEP 1.1: Call to method 'SaveUpdateSMSNotificationData' completed successfully for OrgUserId:" + applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserID);

                if (result && !applicantOrderCart.lstApplicantOrder[0].IsReceiveTextNotification)
                {
                    if (SaveAuthenticationData(applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserID))
                    {
                        BasePage.LogOrderFlowSteps("Default.aspx - STEP 1.2: Call to method 'SaveAuthenticationData' completed successfully for OrgUserId:" + applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserID);
                    }
                    else
                    {
                        BasePage.LogOrderFlowSteps("Default.aspx - STEP 1.2: Call to method 'SaveAuthenticationData' not completed successfully for OrgUserId:" + applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserID);
                    }
                }

            }
            #endregion
            if (!applicantOrderCart.DPPS_ID.IsNullOrEmpty())
                _prgPackageSubscriptionId = applicantOrderCart.DPPS_ID;
            else if (applicantOrderCart.lstApplicantOrder.IsNotNull() && applicantOrderCart.lstApplicantOrder.Count > 0 && applicantOrderCart.lstApplicantOrder[0].DPPS_Id.IsNotNull() && applicantOrderCart.lstApplicantOrder[0].DPPS_Id.Count > 0)
                _prgPackageSubscriptionId = applicantOrderCart.lstApplicantOrder[0].DPPS_Id[0];

            //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see. 
            Int32 organizationUserID = 0;
            if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                organizationUserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
            else
                organizationUserID = SysXWebSiteUtils.SessionService.OrganizationUserId;
            //End

            #endregion

            var _lstClientPaymentOptions = ComplianceDataManager.GetClientPaymentOptions(_tenantId);

            var _ccPaymentOptionId = _lstClientPaymentOptions.Where(po => po.Code == PaymentOptions.Credit_Card.GetStringValue()).First().PaymentOptionID;
            var _paypalPaymentOptionId = _lstClientPaymentOptions.Where(po => po.Code == PaymentOptions.Paypal.GetStringValue()).First().PaymentOptionID;

            var _lstSelectedPOIds = _paymentModesData.Where(pmd => !pmd.isZP).Select(x => x.poid);

            if (_lstSelectedPOIds.Contains(_ccPaymentOptionId) && _lstSelectedPOIds.Contains(_paypalPaymentOptionId))
                return String.Empty;

            #region BillingInfo

            applicantOrderCart.IsBiilingInfoSameAsAccountInfo = isBillingInfoSameAsAccountInfo;

            #endregion

            #region Read Data from Pricing XML

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

            #endregion
            //Added check to save data only if order request type is not for Applicant completing order process for "Sent for online payment" type.[UAT-1648]
            Order _order = new Order();
            if (String.Compare(Convert.ToString(applicantOrderCart.OrderRequestType), OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) != AppConsts.NONE
                && String.Compare(Convert.ToString(applicantOrderCart.OrderRequestType), OrderRequestType.ModifyShipping.GetStringValue(), true) != AppConsts.NONE)
            {
                BasePage.LogOrderFlowSteps("Default.aspx - STEP 2: OrderRequestType for OrgUserId:" + organizationUserID + " is " + applicantOrderCart.OrderRequestType);

                #region Set Order object, based on OrderRequestType
                ////_order.PaymentOptionID = Convert.ToInt32(paymentModeId); UAT 916
                _order.OrderMachineIP = applicantOrderCart.lstApplicantOrder[0].ClientMachineIP;
                _order.TotalPrice = Convert.ToDecimal(applicantOrderCart.CurrentPackagePrice);
                _order.ProgramDuration = applicantOrderCart.ProgramDuration;
                //_order.HierarchyNodeID = applicantOrderCart.SelectedHierarchyNodeID;
                //Get HiearchyNodeID. It should be the id of compliance package node in case there was a compliance package in the order. Else it should be the id of the background package node. 
                _order.HierarchyNodeID = GetHierarchyNodeIDByPackageType(_tenantId, applicantOrderCart);
                _order.OriginalSettlementPrice = Convert.ToDecimal(applicantOrderCart.SettleAmount);
                //Set SelecteHierarchyNodeID in SelectedNodeID of order [UAT-1067]
                _order.SelectedNodeID = applicantOrderCart.SelectedHierarchyNodeID;

                if (applicantOrderCart.OrderRequestType != null)
                    _order.OrderRequestTypeID = ComplianceDataManager.GetLKPOrderRequestType(applicantOrderCart.OrderRequestType, _tenantId).ORT_ID;

                if (applicantOrderCart.IsRushOrderIncluded)
                {
                    isRushOrder = true;
                    _order.RushOrderPrice = Convert.ToDecimal(applicantOrderCart.RushOrderPrice.Trim());
                    _order.IsRushOrderForExistingOrder = false;

                    //UAT 264
                    Decimal _netPrice = (applicantOrderCart.CurrentPackagePrice.Value - applicantOrderCart.SettleAmount)
                                    + Convert.ToDecimal(applicantOrderCart.RushOrderPrice.Trim());
                    _order.GrandTotal = _netPrice <= 0 ? 0 : _netPrice + GetBackgroundPackagesPrice(applicantOrderCart);

                    GenerateGroupedAmount(applicantOrderCart, _paymentModesData, _tenantId, _applicantOrderDataContract, _lstClientPaymentOptions);
                }
                else
                {
                    // No need to subtract the Settlement Amount from the Grand Total, as it was already done, 
                    // before it was added to the session, from the PendingOrder.cs (AddCompliancePackageDataToSession() function)
                    _order.GrandTotal = Convert.ToDecimal(applicantOrderCart.GrandTotal) + GetBackgroundPackagesPrice(applicantOrderCart);

                    GenerateGroupedAmount(applicantOrderCart, _paymentModesData, _tenantId, _applicantOrderDataContract, _lstClientPaymentOptions);
                }
                if (Convert.ToString(applicantOrderCart.OrderRequestType) == OrderRequestType.RenewalOrder.GetStringValue())
                {
                    BasePage.LogOrderFlowSteps("Default.aspx - STEP 2.1: Renewal Order for OrgUserId:" + organizationUserID + " and Previous OrderId " + applicantOrderCart.PrevOrderId);
                    _order.PreviousOrderID = applicantOrderCart.PrevOrderId;
                    _order.SubscriptionYear = 0;
                    _order.SubscriptionMonth = applicantOrderCart.RenewalDuration;
                }
                else if (Convert.ToString(applicantOrderCart.OrderRequestType) == OrderRequestType.ChangeSubscription.GetStringValue())
                {
                    // For Change subscription, there will be only single record/package selected
                    //var _paymentOptnId = _paymentModesData.First().poid;
                    //applicantOrderCart.CSPaymentTypeCode = _lstClientPaymentOptions.Where(po => po.PaymentOptionID == _paymentOptnId).First().Code;

                    BasePage.LogOrderFlowSteps("Default.aspx - STEP 2.1: ChangeSubscription  for OrgUserId:" + organizationUserID + " and Previous OrderId " + applicantOrderCart.PrevOrderId);

                    _order.PreviousOrderID = applicantOrderCart.PrevOrderId;
                    //BS
                    var packageSubs = ComplianceDataManager.GetPackageSubscriptionDetailByOrderId(_tenantId, applicantOrderCart.PrevOrderId);
                    if (packageSubs.IsNotNull())
                    {
                        _order.PreviousSubscriptionID = packageSubs.PackageSubscriptionID;
                        DeptProgramPackageSubscription programPackageSubscription = ComplianceDataManager.GetDeptProgramPackageSubscriptionDetail(_tenantId, _prgPackageSubscriptionId);

                        //Applicant Change Program and mapping exists: Populated when Order is created using AddInMappingQueue method.
                        if (programPackageSubscription.IsNotNull())
                        {
                            Entity.MappingRequestData mappingRequestData = new Entity.MappingRequestData
                            {
                                FromTenantID = SecurityManager.GetOrganizationUser(packageSubs.OrganizationUserID.Value).Organization.Tenant.TenantID,
                                ToTenantID = _tenantId,
                                FromPackageID = packageSubs.CompliancePackageID,
                                ToPackageID = programPackageSubscription.DeptProgramPackage.DPP_CompliancePackageID,
                                FromPackageName = packageSubs.CompliancePackage.PackageName,
                                ToPackageName = programPackageSubscription.DeptProgramPackage.CompliancePackage.PackageName,
                                //FromNodeId = packageSubs.Order.HierarchyNodeID.Value,
                                //ToNodeId = _order.HierarchyNodeID.Value
                                //[03202015]: UAT-1067:Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, 
                                //not the node the package lives on. 
                                FromNodeId = packageSubs.Order.SelectedNodeID.Value,
                                ToNodeId = _order.SelectedNodeID.Value
                            };
                            if (mappingRequestData.FromPackageID != mappingRequestData.ToPackageID)
                            {
                                List<Entity.MappingRequestData> lstMappingRequest = new List<Entity.MappingRequestData>() { mappingRequestData };
                                //lstMappingRequest = MobilityManager.AddRecordsInMappingQueue(lstMappingRequest, SysXWebSiteUtils.SessionService.OrganizationUserId);
                                lstMappingRequest = MobilityManager.AddRecordsInMappingQueue(lstMappingRequest, organizationUserID);

                                if (lstMappingRequest.IsNotNull() && lstMappingRequest.Count > 0)
                                {
                                    _order.IsMappingSkipped = lstMappingRequest[0].IsMappingSkipped;
                                }
                            }
                        }
                    }
                }

                _applicantOrderDataContract.lstOrderPackageTypes = LookupManager.GetLookUpData<lkpOrderPackageType>(_tenantId);
                _applicantOrderDataContract.lstOrderStatus = LookupManager.GetLookUpData<lkpOrderStatu>(_tenantId);


                #endregion

                if (applicantOrderCart.lstApplicantOrder.IsNotNull() && applicantOrderCart.lstApplicantOrder[0].OrderId == AppConsts.NONE)
                {
                    Int32 _orderId = 0;
                    _order.CreatedByID = organizationUserID;

                    //UAT-3757
                    if (!applicantOrderCart.bufferSignature.IsNullOrEmpty())
                    {
                        OrderApplicantSignature orderApplicantSignature = new OrderApplicantSignature();
                        orderApplicantSignature.OAS_Signature = applicantOrderCart.bufferSignature;
                        _order.OrderApplicantSignatures.Add(orderApplicantSignature);
                    }

                    #region Set OrderPackageType, based on the Packages selected

                    _order.OrderPackageType = GetOrderPackageType(_tenantId, applicantOrderCart);

                    #endregion

                    #region Set Data in Contract class & Save in database

                    _orgUserProfile.OrganizationUserID = SysXWebSiteUtils.SessionService.OrganizationUserId;
                    List<TypeCustomAttributes> lst = applicantOrderCart.GetCustomAttributeValues();

                    //Set Is Complance Package Selected
                    _applicantOrderDataContract.IsCompliancePackageSelected = applicantOrderCart.IsCompliancePackageSelected;
                    _applicantOrderDataContract.OrganizationUserProfile = _orgUserProfile;
                    _applicantOrderDataContract.ProgramPackageSubscriptionId = _prgPackageSubscriptionId;
                    _applicantOrderDataContract.lstGroupedData = applicantOrderCart.lstPaymentGrouping;
                    _applicantOrderDataContract.TenantId = _tenantId;
                    _applicantOrderDataContract.lstAttributeValues = lst;
                    _applicantOrderDataContract.LastNodeDPMId = Convert.ToInt32(applicantOrderCart.SelectedHierarchyNodeID);
                    _applicantOrderDataContract.lstBackgroundPackages = applicantOrderCart.lstApplicantOrder[0].lstPackages;
                    _applicantOrderDataContract.lstPricingData = _lstPricingData;
                    _applicantOrderDataContract.IsSendBackgroundReport = applicantOrderCart.lstApplicantOrder[0].IsSendBackgroundReport;
                    // SelectedPaymentModeId = paymentModeId, UAT 916

                    //UAT 1438: Enhancement to allow students to select a User Group.
                    _applicantOrderDataContract.IsUserGroupCustomAttributeExist = applicantOrderCart.IsUserGroupCustomAttributeExist;
                    if (applicantOrderCart.IsUserGroupCustomAttributeExist)
                    {
                        _applicantOrderDataContract.lstAttributeValuesForUserGroup = ComplianceDataManager.AddCustomAttributeValuesForUserGroup(applicantOrderCart.lstCustomAttributeUserGroupIDs, SysXWebSiteUtils.SessionService.OrganizationUserId, organizationUserID); //applicantOrderCart.GetCustomAttributeValuesForUserGroup();
                    }


                    if (!applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.IsNullOrEmpty())
                        _applicantOrderDataContract.lstBkgOrderData = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData;

                    Boolean _storeBrowserAgent = Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_STORE_BROWSER_AGENTS]);

                    if (_storeBrowserAgent)
                        _applicantOrderDataContract.UserBrowserAgentString = HttpContext.Current.Request.UserAgent;

                    //UAT-3283
                    //_order.PackageBundleId = applicantOrderCart.SelectedPkgBundleId.HasValue ? applicantOrderCart.SelectedPkgBundleId.Value : (int?)null;
                    //Add OrderBundlePackages
                    if (!applicantOrderCart.lstSelectedPkgBundleId.IsNullOrEmpty())
                    {
                        foreach (Int32 selectedBundleId in applicantOrderCart.lstSelectedPkgBundleId)
                        {
                            OrderBundlePackage orderBundlePkg = new OrderBundlePackage();
                            orderBundlePkg.OBP_BundlePackageID = selectedBundleId;
                            _order.OrderBundlePackages.Add(orderBundlePkg);
                        }
                    }

                    //UAT 3521
                    if (applicantOrderCart.IsLocationServiceTenant)
                    {
                        String _orderNumber = String.Empty;
                        _orderNumber = "#OrderID#" + "-" + _tenantId + "-" + SysXUtils.GenerateRandomNo(2) + "-" + SysXUtils.RandomString(2, false) + "-" + applicantOrderCart.FingerPrintData.LocationId;
                        _order.OrderNumber = _orderNumber;
                    }

                    // UAT 916
                    _dicInvoiceNumber = ComplianceDataManager.SubmitApplicantOrder(_order, _applicantOrderDataContract, _isUpdateMainProfile,
                       applicantOrderCart.lstPrevAddresses, applicantOrderCart.lstPersonAlias, out _paymentModeCode, out _orderId, organizationUserID,
                       (applicantOrderCart.CompliancePackages.IsNotNull() && applicantOrderCart.CompliancePackages.Count > 0 ? applicantOrderCart.CompliancePackages.Values.ToList() : null),
                       applicantOrderCart.lstApplicantOrder, applicantOrderCart.MailingAddress, applicantOrderCart.FingerPrintData);


                    _invoiceNumbers = GetInvoiceNumbers(_dicInvoiceNumber);

                    if (applicantOrderCart.IsLocationServiceTenant && _orderId != AppConsts.NONE && _applicantOrderDataContract.IsBillingCodeAmountAvlbl)
                    {
                        FingerPrintDataManager.SubmitOrderBillingCodeMapping(_tenantId, _orderId, applicantOrderCart.FingerPrintData.BillingCode, organizationUserID);
                    }

                    BasePage.LogOrderFlowSteps("Default.aspx - STEP 3: Method 'SubmitApplicantOrder' executed successfully for OrgUserId:" + organizationUserID + " and Invoice numbers per payment mode code are: " + _invoiceNumbers);

                    #endregion

                    #region Call Method to update EDS status and EDS custom Form Data

                    //This method is called to handle the scenario where amount is zero and order is approved with zero amount automatically.
                    UpdateEDSStatus(_applicantOrderDataContract, _orderId, _order);

                    ComplianceDataManager.InsertAutomaticInvitationLog(_tenantId, _orderId, SysXWebSiteUtils.SessionService.OrganizationUserId); //UAT-2388

                    //UAT-4498
                    ComplianceDataManager.CopyDataForDummyLineItem(_orderId, _tenantId, SysXWebSiteUtils.SessionService.OrganizationUserId);

                    //Send Notification for print scan
                    //UAT-1358:Complio Notification to applicant for PrintScan
                    SendPrintScanNotification(_orderId, _order, null, false, _applicantOrderDataContract.TenantId);
                    //Int32 currentLoggedInUserId = SysXWebSiteUtils.SessionService.OrganizationUserId;

                    if (applicantOrderCart.ApplicantDisclaimerDocumentId.IsNotNull() || (applicantOrderCart.ApplicantDisclosureDocumentIds.IsNotNull() && applicantOrderCart.ApplicantDisclosureDocumentIds.Count > 0))
                    {
                        ComplianceDataManager.SaveApplicantEsignatureDocument(_tenantId, Convert.ToInt32(applicantOrderCart.ApplicantDisclaimerDocumentId), applicantOrderCart.ApplicantDisclosureDocumentIds, _orderId, _applicantOrderDataContract.OrganizationUserProfile.OrganizationUserProfileID, organizationUserID, _order.OrderNumber);
                        BasePage.LogOrderFlowSteps("Default.aspx - STEP 4: Method 'SaveApplicantEsignatureDocument' executed successfully for OrgUserId:" + organizationUserID + " and OrderId: " + _orderId);

                        if (_order.OrderGroupOrderNavProp.IsNotNull() && _order.OrderGroupOrderNavProp.Count > 0)
                        {
                            foreach (Order childOrder in _order.OrderGroupOrderNavProp)
                            {
                                ComplianceDataManager.SaveApplicantEsignatureDocument(_tenantId, Convert.ToInt32(applicantOrderCart.ApplicantDisclaimerDocumentId), applicantOrderCart.ApplicantDisclosureDocumentIds, childOrder.OrderID, _applicantOrderDataContract.OrganizationUserProfile.OrganizationUserProfileID, organizationUserID, childOrder.OrderNumber);
                                BasePage.LogOrderFlowSteps("Default.aspx - STEP 4.1: Method 'SaveApplicantEsignatureDocument' executed successfully for Child Orders of OrgUserId:" + organizationUserID + " and OrderId: " + childOrder.OrderID);
                            }
                        }
                    }
                    #endregion
                    #region UAT-1560:WB: We should be able to add documents that need to be signed to the order process
                    //Update Additional Documents of Applicants in Applicant Document table.
                    if (applicantOrderCart.ApplicantAdditionalDocumentIds.IsNotNull() && applicantOrderCart.ApplicantAdditionalDocumentIds.Count > 0
                        && applicantOrderCart.IsAdditionalDocumentExist)
                    {
                        Boolean isSubscriptionExist = ComplianceDataManager.IsSubscriptionExistForApplicant(organizationUserID, _tenantId);
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
                        List<ApplicantDocument> additionalDocList = ComplianceDataManager.UpdateApplicantAdditionalEsignatureDocument(_tenantId,
                                                                                         applicantOrderCart.ApplicantAdditionalDocumentIds, applicantOrderCart.dicApplicantDocSysDocMapping, _orderId,
                                                                                         _applicantOrderDataContract.OrganizationUserProfile.OrganizationUserProfileID,
                                                                                         organizationUserID, needToSaveMappingInGenricDocMapping, applicantOrderCart.AdditionalDocSendToStudent
                                                                                         , lstSystemDocBkgSvcMapping);
                        if (!additionalDocList.IsNullOrEmpty() && isSubscriptionExist)
                        {
                            //UAT-4558
                            List<Int32> lstOrderIds = new List<Int32>();
                            lstOrderIds.Add(_order.OrderID);
                            if (_order.OrderGroupOrderNavProp.IsNotNull() && _order.OrderGroupOrderNavProp.Count > 0)
                            {
                                foreach (Order o in _order.OrderGroupOrderNavProp)
                                    lstOrderIds.Add(o.OrderID);
                            }
                            foreach (Int32 ordId in lstOrderIds)
                            {
                                ComplianceDataManager.AddComplianceFileUploadDocMap(ordId, _tenantId, organizationUserID);
                            }
                            //END
                            DocumentManager.CallParallelTaskPdfConversionMergingForAppDoc(additionalDocList, _tenantId, organizationUserID, organizationUserID);
                            BasePage.LogOrderFlowSteps("Default.aspx - STEP 5: Method 'UpdateApplicantAdditionalEsignatureDocument' executed successfully " +
                                                       "and Parallel Task 'CallParallelTaskPdfConversionMergingForAppDoc' started for OrgUserId:" +
                                                       organizationUserID + " and OrderId: " + _orderId);
                        }
                    }
                    #endregion
                    //Will have to change if multiple orders at a time
                    applicantOrderCart.lstApplicantOrder[0].OrderId = _orderId;
                    applicantOrderCart.lstApplicantOrder[0].OrderNumber = _order.OrderNumber;
                }
                else
                {
                    _order.OrderID = applicantOrderCart.lstApplicantOrder[0].OrderId;
                    //_order.ModifiedByID = SysXWebSiteUtils.SessionService.OrganizationUserId;
                    _order.ModifiedByID = organizationUserID;

                    var _ordStsDC = new OrderStatusDataContract();
                    var _lstOrderStatus = ComplianceDataManager.GetOrderStatusList(_tenantId)
                                        .Where(orderSts => !orderSts.IsDeleted).ToList();

                    var _bkgPkgTypeId = _applicantOrderDataContract.lstOrderPackageTypes.Where(opt => opt.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
                    var _compliancePkgTypeId = _applicantOrderDataContract.lstOrderPackageTypes.Where(opt => opt.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;

                    var lstBkgOrder = _order.BkgOrders.FirstOrDefault();
                    var _rushOrdstatusId = AppConsts.NONE;

                    var lst = new List<OrderStatusDataContract>();
                    foreach (var grpdData in applicantOrderCart.lstPaymentGrouping)
                    {
                        String statusCode = String.Empty;

                        if (grpdData.PaymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower()
                            || grpdData.PaymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())
                            statusCode = ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue();
                        else
                            statusCode = ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue();

                        var _statusId = _lstOrderStatus.Where(ordSts => ordSts.Code == statusCode).FirstOrDefault().OrderStatusID;

                        _ordStsDC.Amount = grpdData.TotalAmount;
                        _ordStsDC.PaymentOptionId = grpdData.PaymentModeId;
                        _ordStsDC.StatusId = _statusId;

                        _ordStsDC.lstPackages = new List<OrderPkgPaymentDetail>();

                        foreach (var pkg in grpdData.lstPackages)
                        {
                            var pkgId = Convert.ToInt32(pkg.Key.Split('_')[0]);
                            var _bopId = AppConsts.NONE;
                            // If it is BkgPackage, get its BOPID 
                            if (pkg.Value)
                            {
                                var _bop = lstBkgOrder.BkgOrderPackages.FirstOrDefault(bop => bop.BkgPackageHierarchyMapping.BPHM_BackgroundPackageID == pkgId
                                     && !bop.BOP_IsDeleted);
                                if (_bop.IsNotNull())
                                    _bopId = _bop.BOP_ID;
                            }

                            // 'OrderPaymentDetails' object will be attached by the 'AddOnlinePaymentTransaction' 
                            //  method call, in 'UpdateFailedOrder'
                            var _ordPkgPayDetails = new OrderPkgPaymentDetail();
                            _ordPkgPayDetails.OPPD_IsDeleted = false;
                            _ordPkgPayDetails.OPPD_BkgOrderPackageID = (!pkg.Value || _bopId == AppConsts.NONE) ? (Int32?)null : _bopId;
                            _ordPkgPayDetails.OPPD_OrderPackageTypeID = pkg.Value == true ? _bkgPkgTypeId : _compliancePkgTypeId;
                            //_ordPkgPayDetails.OPPD_CreatedBy = SysXWebSiteUtils.SessionService.OrganizationUserId;
                            _ordPkgPayDetails.OPPD_CreatedBy = organizationUserID;
                            _ordStsDC.lstPackages.Add(_ordPkgPayDetails);
                        }
                        lst.Add(_ordStsDC);


                        if (isRushOrder)
                        {
                            // Check for the Compliance package in the current groupd being traversed
                            var _cmpPkg = grpdData.lstPackages.Where(x => !x.Value).FirstOrDefault();

                            // If it is having a Compliance package, and is Rush Order, use the Same status for RushOrder, as that of group
                            if (_cmpPkg.IsNotNull())
                                _rushOrdstatusId = _statusId;
                        }
                    }
                    _dicInvoiceNumber = ComplianceDataManager.UpdateFailedOrder(_tenantId, _order, isRushOrder, lst, _rushOrdstatusId);

                    _invoiceNumbers = GetInvoiceNumbers(_dicInvoiceNumber);
                    BasePage.LogOrderFlowSteps("Default.aspx - STEP 3: Method 'ComplianceDataManager.UpdateFailedOrder' executed"
                    + " for OrgUserId:" + organizationUserID + " and Invoice numbers per payment mode code are: " + _invoiceNumbers);
                }
            }
            // [UAT-1648]:As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
            else if (String.Compare(Convert.ToString(applicantOrderCart.OrderRequestType), OrderRequestType.CompleteOrderByApplicant.GetStringValue(), true) == AppConsts.NONE)
            {
                BasePage.LogOrderFlowSteps("Default.aspx - STEP 2: OrderRequestType for OrgUserId:" + organizationUserID + " is: " + applicantOrderCart.OrderRequestType);

                Int32 _orderId = applicantOrderCart.lstApplicantOrder[0].OrderId;

                //Get Existing order for completing order process.
                _order = ComplianceDataManager.GetOrderById(_tenantId, _orderId);
                List<Int32> newlyAddedOPDIdList = new List<Int32>();

                #region Set Applicant Order Data Contract from applicant order cart data.
                GenerateGroupedAmount(applicantOrderCart, _paymentModesData, _tenantId, _applicantOrderDataContract, _lstClientPaymentOptions);
                _applicantOrderDataContract.IsCompliancePackageSelected = applicantOrderCart.IsCompliancePackageSelected;
                _applicantOrderDataContract.OrganizationUserProfile = _orgUserProfile;
                _applicantOrderDataContract.ProgramPackageSubscriptionId = _prgPackageSubscriptionId;
                _applicantOrderDataContract.lstGroupedData = applicantOrderCart.lstPaymentGrouping;
                _applicantOrderDataContract.TenantId = _tenantId;
                _applicantOrderDataContract.LastNodeDPMId = Convert.ToInt32(applicantOrderCart.SelectedHierarchyNodeID);
                _applicantOrderDataContract.lstBackgroundPackages = applicantOrderCart.lstApplicantOrder[0].lstPackages;
                _applicantOrderDataContract.lstPricingData = _lstPricingData;
                _applicantOrderDataContract.IsSendBackgroundReport = applicantOrderCart.lstApplicantOrder[0].IsSendBackgroundReport;
                #endregion

                //Get browser agent setting from webconfig
                Boolean _storeBrowserAgent = Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_STORE_BROWSER_AGENTS]);
                if (_storeBrowserAgent)
                    _applicantOrderDataContract.UserBrowserAgentString = HttpContext.Current.Request.UserAgent;

                _applicantOrderDataContract.lstOrderPackageTypes = LookupManager.GetLookUpData<lkpOrderPackageType>(_tenantId);
                _applicantOrderDataContract.lstOrderStatus = LookupManager.GetLookUpData<lkpOrderStatu>(_tenantId);

                //Update Existing Order Data.
                _dicInvoiceNumber = ComplianceDataManager.UpdateApplicantCompletingOrderProcess(_order, _applicantOrderDataContract, out _paymentModeCode,
                                                                                                organizationUserID, out newlyAddedOPDIdList, (applicantOrderCart.CompliancePackages.IsNotNull()
                                                                                                && applicantOrderCart.CompliancePackages.Count > 0 ?
                                                                                                applicantOrderCart.CompliancePackages.Values.ToList() : null));

                BasePage.LogOrderFlowSteps("Default.aspx - STEP 3: Method 'ComplianceDataManager.UpdateApplicantCompletingOrderProcess' executed successfully"
                + " for OrgUserId:" + organizationUserID + " and OrderId: " + _orderId);

                applicantOrderCart.RecentAddedOPDs = newlyAddedOPDIdList;
                #region Call Method to update EDS status and EDS custom Form Data

                //This method is called to handle the scenario where amount is zero and order is approved with zero amount automatically.
                UpdateEDSStatus(_applicantOrderDataContract, _orderId, _order, newlyAddedOPDIdList);

                ComplianceDataManager.InsertAutomaticInvitationLog(_tenantId, _orderId, SysXWebSiteUtils.SessionService.OrganizationUserId); //UAT-2388
                //Send Notification for print scan
                //UAT-1358:Complio Notification to applicant for PrintScan
                SendPrintScanNotification(_orderId, _order, null, false, _applicantOrderDataContract.TenantId, newlyAddedOPDIdList);
                #endregion
                //Will have to change if multiple orders at a time
                applicantOrderCart.lstApplicantOrder[0].OrderId = _orderId;
                applicantOrderCart.lstApplicantOrder[0].OrderNumber = _order.OrderNumber;

                //UAT-4498
                ComplianceDataManager.CopyDataForDummyLineItem(_orderId, _tenantId, SysXWebSiteUtils.SessionService.OrganizationUserId);

            }
            else if (String.Compare(Convert.ToString(applicantOrderCart.OrderRequestType), OrderRequestType.ModifyShipping.GetStringValue(), true) == AppConsts.NONE)
            {
                BasePage.LogOrderFlowSteps("Default.aspx - STEP 2: OrderRequestType for OrgUserId:" + organizationUserID + " is: " + applicantOrderCart.OrderRequestType);

                Int32 _orderId = applicantOrderCart.lstApplicantOrder[0].OrderId;

                //Get Existing order for completing order process.
                _order = ComplianceDataManager.GetOrderById(_tenantId, _orderId);
                List<Int32> newlyAddedOPDIdList = new List<Int32>();

                #region Set Applicant Order Data Contract from applicant order cart data.
                GenerateGroupedAmount(applicantOrderCart, _paymentModesData, _tenantId, _applicantOrderDataContract, _lstClientPaymentOptions);
                _applicantOrderDataContract.IsCompliancePackageSelected = applicantOrderCart.IsCompliancePackageSelected;
                _applicantOrderDataContract.OrganizationUserProfile = _orgUserProfile;
                _applicantOrderDataContract.ProgramPackageSubscriptionId = _prgPackageSubscriptionId;
                _applicantOrderDataContract.lstGroupedData = applicantOrderCart.lstPaymentGrouping;
                _applicantOrderDataContract.TenantId = _tenantId;
                _applicantOrderDataContract.LastNodeDPMId = Convert.ToInt32(applicantOrderCart.SelectedHierarchyNodeID);
                _applicantOrderDataContract.lstBackgroundPackages = applicantOrderCart.lstApplicantOrder[0].lstPackages;
                _applicantOrderDataContract.lstPricingData = _lstPricingData;
                _applicantOrderDataContract.IsSendBackgroundReport = applicantOrderCart.lstApplicantOrder[0].IsSendBackgroundReport;
                #endregion
                
                //Get browser agent setting from webconfig
                Boolean _storeBrowserAgent = Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.APP_SETTING_STORE_BROWSER_AGENTS]);
                if (_storeBrowserAgent)
                    _applicantOrderDataContract.UserBrowserAgentString = HttpContext.Current.Request.UserAgent;

                _applicantOrderDataContract.lstOrderPackageTypes = LookupManager.GetLookUpData<lkpOrderPackageType>(_tenantId);
                _applicantOrderDataContract.lstOrderStatus = LookupManager.GetLookUpData<lkpOrderStatu>(_tenantId);

                //Update Existing Order Data.
                _dicInvoiceNumber = ComplianceDataManager.UpdateApplicantModifyShippingProcess(_order, _applicantOrderDataContract, applicantOrderCart, out _paymentModeCode,
                                                                                                organizationUserID, out newlyAddedOPDIdList, (applicantOrderCart.CompliancePackages.IsNotNull()
                                                                                                && applicantOrderCart.CompliancePackages.Count > 0 ?
                                                                                                applicantOrderCart.CompliancePackages.Values.ToList() : null), applicantOrderCart.IsModifyShipping);

                BasePage.LogOrderFlowSteps("Default.aspx - STEP 3: Method 'ComplianceDataManager.UpdateApplicantCompletingOrderProcess' executed successfully"
                + " for OrgUserId:" + organizationUserID + " and OrderId: " + _orderId);

                applicantOrderCart.RecentAddedOPDs = newlyAddedOPDIdList;
                if (applicantOrderCart.IsPaymentReqInMdfyShpng && applicantOrderCart.IsModifyShipping && applicantOrderCart.IsLocationServiceTenant && applicantOrderCart.RecentAddedOPDs.Count() > 0)
                {
                    applicantOrderCart.TenantId = _tenantId;
                    SecurityManager.SaveModifyShippingData(applicantOrderCart, applicantOrderCart.IsLocationServiceTenant, organizationUserID);
                    ComplianceDataManager.SaveOrderPaymentInvoice(_tenantId, _orderId, organizationUserID, applicantOrderCart.IsModifyShipping);
                }
                #region Call Method to update EDS status and EDS custom Form Data

                //This method is called to handle the scenario where amount is zero and order is approved with zero amount automatically.
                 UpdateEDSStatus(_applicantOrderDataContract, _orderId, _order, newlyAddedOPDIdList);

                ComplianceDataManager.InsertAutomaticInvitationLog(_tenantId, _orderId, SysXWebSiteUtils.SessionService.OrganizationUserId); //UAT-2388
                //Send Notification for print scan
                //UAT-1358:Complio Notification to applicant for PrintScan
                SendPrintScanNotification(_orderId, _order, null, false, _applicantOrderDataContract.TenantId, newlyAddedOPDIdList);
                #endregion
                //Will have to change if multiple orders at a time
                applicantOrderCart.lstApplicantOrder[0].OrderId = _orderId;
                applicantOrderCart.lstApplicantOrder[0].OrderNumber = _order.OrderNumber;

                //UAT-4498
                ComplianceDataManager.CopyDataForDummyLineItem(_orderId, _tenantId, SysXWebSiteUtils.SessionService.OrganizationUserId);
                ComplianceDataManager.AddDataInXMLForModifyShipping(_tenantId, _order, _applicantOrderDataContract, organizationUserID, applicantOrderCart.IsLocationServiceTenant,applicantOrderCart.MailingAddress,applicantOrderCart.FingerPrintData);

            }

            applicantOrderCart.InvoiceNumber = _dicInvoiceNumber;
            applicantOrderCart.IncrementOrderStepCount();
            applicantOrderCart.AddOrderStageTrackID(OrderStages.OrderPayment);

            #region Redirect URL Logic

            //generatedInvoiceNumber = Guid.NewGuid().ToString(); // TO BE REMOVED
            String _redirectUrl = String.Empty;
            var _onlineInvoiceNumber = String.Empty;

            var _lstPG = _applicantOrderDataContract.lstGroupedData.Select(x => x);
            Boolean isPaypalSelected = _lstPG.Any(pmt => pmt.PaymentModeCode == PaymentOptions.Paypal.GetStringValue());
            Boolean isCreditCardSelected = _lstPG.Any(pmt => pmt.PaymentModeCode == PaymentOptions.Credit_Card.GetStringValue()
                                                            && pmt.TotalAmount > 0);

            var _onlineModeCode = String.Empty;
            if (!isCreditCardSelected) // This will make sure that Even if the CC with Amount zero was in Cart, than do not use that Payment Mode code
                _onlineModeCode = PaymentOptions.Paypal.GetStringValue();
            else
                _onlineModeCode = PaymentOptions.Credit_Card.GetStringValue();

            //UAT 4537 Allow the CC payment method packages that don’t require approval to go through, even while other packages within the same order are still pending approval 
            String _commaSeparatedInvoiceNumber = String.Empty;
            var _onlinePaymentType = new KeyValuePair<String, String>();
            if (_dicInvoiceNumber.ContainsKey(PaymentOptions.Credit_Card.GetStringValue()) && _dicInvoiceNumber.ContainsKey(PaymentOptions.Credit_Card_With_Approval_Required.GetStringValue()))
            {
                _onlinePaymentType = _dicInvoiceNumber.Where(d => d.Key.Contains(_onlineModeCode)).FirstOrDefault();
                _commaSeparatedInvoiceNumber = String.Join(";", _dicInvoiceNumber.Where(d => d.Key.Contains(_onlineModeCode)).Select(x => x.Value).ToArray());
            }
            else
                _onlinePaymentType = _dicInvoiceNumber.Where(d => d.Key == _onlineModeCode).FirstOrDefault();

            if (_onlinePaymentType.IsNotNull())
                _onlineInvoiceNumber = _onlinePaymentType.Value;

            //UAT-1185 Passing multiple order ids to payment pages
            string strOrderID = string.Empty;
            if (_onlineInvoiceNumber.IsNotNull())
                strOrderID = ExtractOrderNumbersFromInvoiveNumber(_onlineInvoiceNumber);
            if (strOrderID.IsNullOrEmpty())
                strOrderID = _order.OrderID.ToString();

            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "invnum", !String.IsNullOrEmpty(_commaSeparatedInvoiceNumber)?_commaSeparatedInvoiceNumber: _onlineInvoiceNumber},
                                                                    {"OrderId", strOrderID}
                                                                 };


            if (String.IsNullOrEmpty(_invoiceNumbers))
            {
                _invoiceNumbers = GetInvoiceNumbers(_dicInvoiceNumber);
            }

            // UAT 916 --  Manage the PaymentModeCode here & Handle Invoice Number
            //if (String.IsNullOrEmpty(generatedInvoiceNumber) || (!(_paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower()) &&
            //!(_paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())))
            if (!isCreditCardSelected && !isPaypalSelected)
            {
                // In case, crash in order generation
                applicantOrderCart.AddOrderStageTrackID(OrderStages.OnlineConfirmation);
                _errorMessage = (_dicInvoiceNumber.Count == 0 || _dicInvoiceNumber.Any(d => d.Value.IsNullOrEmpty())) ? "Error in order placement." : String.Empty;
                _redirectUrl = RedirectConfirmationPage(_errorMessage);
                _redirectUrlType = "internal";

                BasePage.LogOrderFlowSteps("Default.aspx - STEP 6: Payment options other then 'Credit Card' or 'Paypal', were selected by OrgUserId: " + organizationUserID + " and Invoice Number(s): " + _invoiceNumbers);
            }
            //else if (_paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower())
            else if (isCreditCardSelected)
            {
                _redirectUrl = "Pages/CIMAccountSelection.aspx";
                _redirectUrl = String.Format(_redirectUrl + "?args={0}", queryString.ToEncryptedQueryString());
                _redirectUrlType = "internal";

                BasePage.LogOrderFlowSteps("Default.aspx - STEP 6: Redirecting to CIMAccountSelection.aspx for: " + organizationUserID + " and Invoice Number(s): " + _invoiceNumbers);
            }
            //else if (_order.lkpOrderStatu.IsNotNull() && _order.lkpOrderStatu.Code == ApplicantOrderStatus.Paid.GetStringValue())
            else if (AreAllOrdersPaid(_order))
            {
                applicantOrderCart.AddOrderStageTrackID(OrderStages.OnlineConfirmation);
                //_errorMessage = String.IsNullOrEmpty(generatedInvoiceNumber) ? "Error in order placement." : String.Empty;
                _redirectUrl = RedirectConfirmationPage(_errorMessage);
                _redirectUrlType = "internal";

                BasePage.LogOrderFlowSteps("Default.aspx - STEP 6: All orders are PAID, redirecting to Order Confirmation page for OrgUserId: " + organizationUserID + " and Invoice Number(s): " + _invoiceNumbers);
            }
            //else if (_paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())
            else if (isPaypalSelected)
            {
                _redirectUrl = "Pages/PaypalPaymentSubmission.aspx";
                _redirectUrl = String.Format(_redirectUrl + "?args={0}", queryString.ToEncryptedQueryString());
                _redirectUrlType = "external";

                BasePage.LogOrderFlowSteps("Default.aspx - STEP 6: Redirecting to PaypalPaymentSubmission.aspx for: " + organizationUserID + " and Invoice Number(s): " + _invoiceNumbers);
            }

            if (!url.ToLower().StartsWith("http"))
            {
                url = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + url;
            }

            //if (_paymentModeCode == PaymentOptions.Credit_Card.GetStringValue() || _paymentModeCode == PaymentOptions.Paypal.GetStringValue())
            if (isCreditCardSelected || isPaypalSelected)
            {
                Int32 timeout = GetSessionTimeoutValue();
                ApplicationDataManager.AddWebApplicationData(_onlineInvoiceNumber, url, timeout);

                BasePage.LogOrderFlowSteps("Default.aspx - STEP 6.1: Method 'ApplicationDataManager.AddWebApplicationData' executed successfully for: " + organizationUserID + " and Invoice Number(s): " + _invoiceNumbers);
            }

            #endregion

            #region Notification Logic for Email & Message

            StringBuilder _sbNotificationMsg = new StringBuilder();

            // UAT 916 -- _paymentModeCode
            foreach (String paymentModeCode in _dicInvoiceNumber.Keys)
            {
                //UAT-3268
                OrderPaymentDetail opd = new OrderPaymentDetail();
                if (paymentModeCode.Contains("Additional"))
                {
                    var AdditionalPaymentModeCode = paymentModeCode.Split('-')[0];
                    opd = _order.OrderPaymentDetails.Where(cond => cond.lkpPaymentOption.Code.Equals(AdditionalPaymentModeCode) && !cond.OPD_IsDeleted).FirstOrDefault();
                }
                else
                {
                    //End-UAT-3268
                    opd = _order.OrderPaymentDetails.Where(cond => cond.lkpPaymentOption.Code.Equals(paymentModeCode) && !cond.OPD_IsDeleted).FirstOrDefault();
                }
                if (opd.IsNotNull())
                {
                    if (paymentModeCode.Contains("Additional"))
                    {
                        var AdditionalPaymentModeCode = paymentModeCode.Split('-')[0];
                        SendNotifications(AdditionalPaymentModeCode, _tenantId, _errorMessage, _orgUserProfile, _order);
                    }
                    else
                    {
                        SendNotifications(paymentModeCode, _tenantId, _errorMessage, _orgUserProfile, _order);
                    }
                    _sbNotificationMsg.Append("OrderId: " + _order.OrderID);
                }

                if (paymentModeCode == PaymentOptions.InvoiceWithOutApproval.GetStringValue()
                    || (opd.IsNotNull() && opd.OPD_Amount == AppConsts.NONE))
                {
                    ComplianceDataManager.SendAdditionalDocumentsToStudent(_tenantId, _order, organizationUserID);
                }
                //UAT-1185 Sending multiple order notification
                List<Int32> orderIds = ComplianceDataManager.GetOrderAndTenantID(_dicInvoiceNumber[paymentModeCode])["OrderID"];

                if (orderIds.IsNotNull())
                {
                    foreach (Int32 orderId in orderIds)
                    {
                        if (!_order.OrderID.Equals(orderId))
                        {

                            Order extraOrder = _order.OrderGroupOrderNavProp.FirstOrDefault(o => o.OrderID.Equals(orderId));
                            if (extraOrder.IsNotNull() && extraOrder.OrderPaymentDetails.IsNotNull())
                            {
                                if (paymentModeCode == PaymentOptions.InvoiceWithOutApproval.GetStringValue())
                                {
                                    ComplianceDataManager.SendAdditionalDocumentsToStudent(_tenantId, extraOrder, organizationUserID);
                                }
                                opd = extraOrder.OrderPaymentDetails.Where(cond => cond.lkpPaymentOption.Code.Equals(paymentModeCode) && !cond.OPD_IsDeleted).FirstOrDefault();
                                if (opd.IsNotNull())
                                {
                                    SendNotifications(paymentModeCode, _tenantId, _errorMessage, _orgUserProfile, extraOrder);
                                    _sbNotificationMsg.Append(", Extra OrderId: " + orderId);
                                }
                            }
                        }
                    }
                }
                _sbNotificationMsg.Append(", ");
            }

            BasePage.LogOrderFlowSteps("Default.aspx - STEP 7: Method 'SendNotifications' executed successfully for " + Convert.ToString(_sbNotificationMsg) + " and call to 'SubmitOrder' completed successfully.");

            ////SendNotifications(_paymentModeCode, _tenantId, _errorMessage, _orgUserProfile, _order);
            
            #endregion

            #region UAT-1834: NYU Migration 2 of 3: Applicant Complete Order Process

            //Update Order ID and status in BulkOrderUpload table in case of bulk order
            if (applicantOrderCart.IsBulkOrder && applicantOrderCart.lstApplicantOrder[0].OrderId > AppConsts.NONE && String.IsNullOrEmpty(_errorMessage))
            {
                BackgroundSetupManager.UpdateBulkOrder(_tenantId, applicantOrderCart.lstApplicantOrder[0].OrderId, applicantOrderCart.BulkOrderUploadID, SysXWebSiteUtils.SessionService.OrganizationUserId);
            }

            #endregion

            return javaScriptSerializer.Serialize(new { redirectUrl = _redirectUrl, redirectUrlType = _redirectUrlType });
        }

        /// <summary>
        /// Get the Invoice numbers string from the Dictionary
        /// </summary>
        /// <param name="_dicInvoiceNumber"></param>
        /// <returns></returns>
        private static String GetInvoiceNumbers(Dictionary<String, String> _dicInvoiceNumber)
        {
            ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;
            StringBuilder _sbInvNumbers = new StringBuilder();
            foreach (var invNo in _dicInvoiceNumber)
            {
                _sbInvNumbers.Append(invNo.Key + " - " + invNo.Value + " || ");
            }
            if (!applicantOrderCart.IsNullOrEmpty() && applicantOrderCart.IsLocationServiceTenant)
            {
                if (_sbInvNumbers.ToString().Length > 0)
                    return Convert.ToString(_sbInvNumbers).Substring(0, Convert.ToString(_sbInvNumbers).LastIndexOf("||") - 1);
                else
                    return string.Empty;
            }
            else
                return Convert.ToString(_sbInvNumbers).Substring(0, Convert.ToString(_sbInvNumbers).LastIndexOf("||") - 1);

        }

        private static string ExtractOrderNumbersFromInvoiveNumber(string invoiceNumber)
        {
            string[] invoiceNumberParts = invoiceNumber.Split('-');
            if (invoiceNumberParts.IsNotNull() && invoiceNumberParts.Length > 2)
            {
                return invoiceNumberParts[2];
            }
            return null;
        }

        /// <summary>
        /// Submit the Order when Applicant changes Payment type for any package
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="paymentModeId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [WebMethod]
        public static String SubmitOrderPayTypeChanged(Int32 orderId, Int32 paymentModeId, Int32 ordPaymentDetailId, String url)
        {
            String _redirectUrlType = String.Empty;
            String _paymentModeCode = String.Empty;
            String _errorMessage = String.Empty;
            String _redirectUrl = String.Empty;
            Dictionary<String, String> queryString = new Dictionary<String, String>();

            ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;
            Entity.ClientEntity.OrganizationUserProfile _orgUserProfile = new Entity.ClientEntity.OrganizationUserProfile();

            //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
            //Int32 organizationUserID = SysXWebSiteUtils.SessionService.OrganizationUserId;
            //Int32 _tenantId = SecurityManager.GetOrganizationUser(organizationUserID).Organization.TenantID.Value;
            Int32 _tenantId = SecurityManager.GetOrganizationUser(SysXWebSiteUtils.SessionService.OrganizationUserId).Organization.TenantID.Value;

            Int32 organizationUserID = 0;
            if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                organizationUserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
            else
                organizationUserID = SysXWebSiteUtils.SessionService.OrganizationUserId;
            //End

            _paymentModeCode = ComplianceDataManager.GetPaymentOptionCodeById(paymentModeId, _tenantId);


            String orderStatuscode = String.Empty;
            //If payment mode is Credit Card or Paypal
            if (_paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower() ||
                _paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())
            {
                orderStatuscode = ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue();
                _redirectUrlType = "external";
            }
            else //else if payment mode is Money Order Or Invoice
            {
                orderStatuscode = ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue();
                _redirectUrlType = "internal";
            }

            foreach (var applicantOrder in applicantOrderCart.lstApplicantOrder)
            {
                _orgUserProfile = applicantOrder.OrganizationUserProfile;
            }

            Order _applicantOrder = ComplianceDataManager.GetOrderById(_tenantId, orderId);
            _applicantOrder.PaymentOptionID = paymentModeId;
            _applicantOrder.ModifiedByID = organizationUserID;

            var _generatedInvoiceNumber = String.Empty;
            var _insertedOPDetailId = AppConsts.NONE;

            _generatedInvoiceNumber = ComplianceDataManager.UpdateOrderByID(_applicantOrder, orderStatuscode, ordPaymentDetailId, paymentModeId, out _insertedOPDetailId, _tenantId);

            applicantOrderCart.OrderPaymentdetailId = _insertedOPDetailId;
            var _dicInvoiceNumbers = new Dictionary<String, String>();
            _dicInvoiceNumbers.Add(_paymentModeCode, _generatedInvoiceNumber);
            if (applicantOrderCart.IsLocationServiceTenant)
                FingerPrintDataManager.SavePaymentTypeAuditChange(_paymentModeCode, _applicantOrder.OrderID, _tenantId, organizationUserID, ordPaymentDetailId);
            #region UAT-1697: Add new client setting to make it where all subscription renewals nees to be approved, even if payment method is invoice without approval
            Boolean ifRenewalOrderApprovalRequired = false;
            String orderRequestTypeCode = _applicantOrder.lkpOrderRequestType.ORT_Code;
            if (orderRequestTypeCode == OrderRequestType.RenewalOrder.GetStringValue() &&
                _paymentModeCode.ToLower() == PaymentOptions.InvoiceWithOutApproval.GetStringValue().ToLower())
            {
                //Check for client settings
                String rnwlOrderAprvlRqdCode = Setting.SUBSCRIPTION_RENEWAL_NEED_APPROVAL.GetStringValue();
                ClientSetting rnwlOrderAprvlRqdSetting = ComplianceDataManager.GetClientSetting(_tenantId).Where(cond => cond.lkpSetting.Code
                                                                                           == rnwlOrderAprvlRqdCode && !cond.CS_IsDeleted).FirstOrDefault();
                if (!rnwlOrderAprvlRqdSetting.IsNullOrEmpty() &&
                    !rnwlOrderAprvlRqdSetting.CS_SettingValue.IsNullOrEmpty())
                {
                    ifRenewalOrderApprovalRequired = Convert.ToBoolean(Convert.ToInt32(rnwlOrderAprvlRqdSetting.CS_SettingValue));
                }
            }
            #endregion

            if (_paymentModeCode.ToLower() == PaymentOptions.InvoiceWithOutApproval.GetStringValue().ToLower()
                && !ifRenewalOrderApprovalRequired)
            {
                DateTime expirydate = DateTime.Now;
                if (_applicantOrder.SubscriptionYear.HasValue)
                {
                    expirydate = expirydate.AddYears(_applicantOrder.SubscriptionYear.Value);
                }
                if (_applicantOrder.SubscriptionMonth.HasValue)
                {
                    expirydate = expirydate.AddMonths(_applicantOrder.SubscriptionMonth.Value);
                }

                Int32 _packageId = AppConsts.NONE;

                // Handle case when no Compliance package is selected
                if (!_applicantOrder.DeptProgramPackage.IsNullOrEmpty())
                    _packageId = _applicantOrder.DeptProgramPackage.DPP_CompliancePackageID;

                String refrenceNumber = "N/A";

                //ComplianceDataManager.UpdateOrderStatus(_tenantId, _applicantOrder.OrderID, ApplicantOrderStatus.Paid.GetStringValue(), _packageId,
                //                   _applicantOrder.CreatedByID), _applicantOrder.OrganizationUserProfile.OrganizationUserID, refrenceNumber
                //                  , expirydate, _insertedOPDetailId);
                ComplianceDataManager.UpdateOrderStatus(_tenantId, _applicantOrder.OrderID, ApplicantOrderStatus.Paid.GetStringValue(), _packageId,
                                  Convert.ToInt32(_applicantOrder.ModifiedByID), _applicantOrder.OrganizationUserProfile.OrganizationUserID, refrenceNumber
                                 , expirydate, _insertedOPDetailId);

                // OrderPaymentDetail orderPaymentDetail = ComplianceDataManager.GetOrderDetailById(_tenantId, _applicantOrder.OrderID);
                OrderPaymentDetail orderPaymentDetail = ComplianceDataManager.GetAllPaymentDetailsOfOrderByOrderID(_tenantId, _applicantOrder.OrderID).Where(x => x.lkpPaymentOption.Code.Equals(_dicInvoiceNumbers.Keys.FirstOrDefault())).FirstOrDefault();
                //Send E-mail to user: 
                if (!orderPaymentDetail.IsNullOrEmpty())
                {
                    String orderPackageType = orderPaymentDetail.Order.lkpOrderPackageType.OPT_Code;
                    Int32? systemCommunicationID = null;
                    Guid? messageID = null;

                    //Update EdS Data.
                    UpdateEDSDataForChangePaymentType(orderPaymentDetail, _applicantOrder.OrderID, _applicantOrder, _tenantId);

                    //Send Print Scan notification
                    //UAT-1358:Complio Notification to applicant for PrintScan
                    SendPrintScanNotification(_applicantOrder.OrderID, _applicantOrder, orderPaymentDetail, true, _tenantId);

                    //Send mail
                    if (!applicantOrderCart.IsLocationServiceTenant)
                        systemCommunicationID = CommunicationManager.SendOrderApprovalMail(orderPaymentDetail, _applicantOrder.CreatedByID, _tenantId);

                    #region UAT-3389
                    Dictionary<String, object> dicMessageParam = new Dictionary<String, object>();
                    if (
                        (
                        orderPaymentDetail.Order.lkpOrderPackageType.OPT_Code.Equals(OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue())
                        ||
                        orderPaymentDetail.Order.lkpOrderPackageType.OPT_Code.Equals(OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue())
                        )
                        &&
                        systemCommunicationID.HasValue && systemCommunicationID > AppConsts.NONE
                       )
                    {
                        var res = ComplianceDataManager.AttachOrderApprovalDocuments(_tenantId, _applicantOrder.OrderID, orderPaymentDetail.Order.OrganizationUserProfile.OrganizationUserID, systemCommunicationID.Value);
                        if (res.Item1)
                        {
                            dicMessageParam = res.Item2;
                        }
                    }
                    #endregion
                    if (!applicantOrderCart.IsLocationServiceTenant)
                        messageID = CommunicationManager.SendOrderApprovalMessage(orderPaymentDetail, _applicantOrder.CreatedByID, _tenantId, dicMessageParam);
                }
            }

            applicantOrderCart.InvoiceNumber = _dicInvoiceNumbers;
            applicantOrderCart.ChangePaymentTypeCode = _paymentModeCode;

            var _onlineInvoiceNumber = String.Empty;
            var _onlinePaymentType = _dicInvoiceNumbers.Where(x => x.Key == PaymentOptions.Credit_Card.GetStringValue()
                                                                    ||
                                                                   x.Key == PaymentOptions.Paypal.GetStringValue()
                                                             ).FirstOrDefault();
            if (_onlinePaymentType.IsNotNull())
                _onlineInvoiceNumber = _onlinePaymentType.Value;

            queryString = new Dictionary<String, String>
                                                        {
                                                           { "invnum", _onlineInvoiceNumber },
                                                           {"OrderId", Convert.ToString(orderId)}
                                                        };
            //If payment mode is Invoice Or Money Order
            //if (String.IsNullOrEmpty(generatedInvoiceNumber) || (!(_paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower()) &&
            //!(_paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())))
            if (!(_paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower()) &&
                !(_paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower()))
            {
                // In case, crash in order generation
                //applicantOrderCart.AddOrderStageTrackID(OrderStages.OnlineConfirmation);
                _errorMessage = (_dicInvoiceNumbers.Count == 0
                                    ||
                                 _dicInvoiceNumbers.Any(d => d.Value.IsNullOrEmpty()))
                 ? "Error in order placement." : String.Empty;
                _redirectUrl = RedirectConfirmationPage(_errorMessage);
            }
            else if (_paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower())
            {
                _redirectUrl = "Pages/CIMAccountSelection.aspx";
                _redirectUrl = String.Format(_redirectUrl + "?args={0}", queryString.ToEncryptedQueryString());
                _redirectUrlType = "internal";
            }
            else if (_paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())
            {
                _redirectUrl = "Pages/PaypalPaymentSubmission.aspx";
                _redirectUrl = String.Format(_redirectUrl + "?args={0}", queryString.ToEncryptedQueryString());
            }

            //Send Mail and Message Notification
            if (String.IsNullOrEmpty(_errorMessage))
            {
                if (_paymentModeCode.Equals(PaymentOptions.Money_Order.GetStringValue(), StringComparison.OrdinalIgnoreCase) && !applicantOrderCart.IsLocationServiceTenant)
                {
                    CommunicationManager.SendOrderCreationMailMoneyOrder(_applicantOrder, _orgUserProfile, _tenantId, _paymentModeCode);
                    CommunicationManager.SendOrderCreationMessageMoneyOrder(_applicantOrder, _orgUserProfile, _tenantId, _paymentModeCode);
                }
                else if (_paymentModeCode.Equals(PaymentOptions.InvoiceWithApproval.GetStringValue(), StringComparison.OrdinalIgnoreCase)
                    || (_paymentModeCode.ToLower() == PaymentOptions.InvoiceWithOutApproval.GetStringValue().ToLower()
                        && ifRenewalOrderApprovalRequired))
                {
                    CommunicationManager.SendOrderCreationMailInvoice(_applicantOrder, _orgUserProfile, _tenantId, _paymentModeCode);
                    CommunicationManager.SendOrderCreationMessageInvoice(_applicantOrder, _orgUserProfile, _tenantId, _paymentModeCode);
                }
            }
            if (!url.ToLower().StartsWith("http"))
            {
                url = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + url;
            }
            if (_paymentModeCode == PaymentOptions.Credit_Card.GetStringValue() || _paymentModeCode == PaymentOptions.Paypal.GetStringValue())
            {
                Int32 timeout = GetSessionTimeoutValue();
                ApplicationDataManager.AddWebApplicationData(_onlineInvoiceNumber, url, timeout);
            }
            //return HttpUtility.UrlEncode(_redirectUrl);
            //return _redirectUrl;
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Serialize(new { redirectUrl = _redirectUrl, redirectUrlType = _redirectUrlType });
        }

        /// <summary>
        /// Place Rush order for an existing order
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="deptPrgPackageSubscriptionId"></param>
        /// <param name="paymentModeId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static String SubmitRushOrder(Int32 orderId, Int32 deptPrgPackageSubscriptionId, Int32 paymentModeId, Boolean isBillingInfoSameAsAccountInfo, String url)
        {
            //UAT-1261: WB: As an ADB admin, I should be able to "login" as any student to see what they see.
            //Int32 organizationUserID = SysXWebSiteUtils.SessionService.OrganizationUserId;
            //Int32 tenantId = SecurityManager.GetOrganizationUser(organizationUserID).Organization.TenantID.Value;

            Int32 tenantId = SecurityManager.GetOrganizationUser(SysXWebSiteUtils.SessionService.OrganizationUserId).Organization.TenantID.Value;
            Int32 organizationUserID = 0;
            if (!System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"].IsNullOrEmpty())
                organizationUserID = Convert.ToInt32(System.Web.HttpContext.Current.Session["AppViewAdminOrgUsrID"]);
            else
                organizationUserID = SysXWebSiteUtils.SessionService.OrganizationUserId;
            //End

            String paymentModeCode = String.Empty;
            String _errorMessage = String.Empty;
            Decimal rushOrderPrice = 0;
            Decimal grandTotal = 0;
            String _redirectUrlType = String.Empty;
            String orderStatuscode = ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue();
            Order _order = ComplianceDataManager.GetOrderById(tenantId, orderId);
            paymentModeCode = ComplianceDataManager.GetPaymentOptionCodeById(paymentModeId, tenantId);
            var deptProgramPackageSubscription = ComplianceDataManager.GetApplicantPackageDetails(deptPrgPackageSubscriptionId, tenantId);

            #region BillingInfo
            ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;
            applicantOrderCart.IsBiilingInfoSameAsAccountInfo = isBillingInfoSameAsAccountInfo;
            applicantOrderCart.IsRushOrder = true;
            #endregion

            if (deptProgramPackageSubscription.DPPS_RushOrderAdditionalPrice.HasValue)
            {
                rushOrderPrice = deptProgramPackageSubscription.DPPS_RushOrderAdditionalPrice.Value;
                _order.RushOrderPrice = rushOrderPrice;
            }
            if (_order.TotalPrice.HasValue)
            {
                grandTotal = _order.GrandTotal.Value + rushOrderPrice;
            }

            //_order.OrderPackageType = GetOrderPackageTypes(tenantId, OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue());
            _order.GrandTotal = grandTotal;
            _order.RushOrderMachineIP = Convert.ToString(HttpContext.Current.Session["ClientMachineIP"]);
            _order.ModifiedByID = organizationUserID;
            OrderPaymentDetail _ordPayDetail = new OrderPaymentDetail();
            String generatedInvoiceNumber = ComplianceDataManager.UpdateRushOrderByOrderID(_order, orderStatuscode, paymentModeId, tenantId, out _ordPayDetail);

            applicantOrderCart.OrderPaymentdetailId = _ordPayDetail.IsNotNull() ? _ordPayDetail.OPD_ID : AppConsts.NONE;

            Dictionary<String, String> _dic = new Dictionary<string, string>();
            _dic.Add(paymentModeCode, generatedInvoiceNumber);

            applicantOrderCart.InvoiceNumber = _dic;
            String _redirectUrl = String.Empty;
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            //UAT 916
            /////applicantOrderCart.InvoiceNumber = generatedInvoiceNumber;
            queryString = new Dictionary<String, String>
                                                        {
                                                           { "invnum", generatedInvoiceNumber },
                                                           {"OrderId", Convert.ToString(orderId)}
                                                        };

            if (paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower())
            {
                _redirectUrl = "Pages/CIMAccountSelection.aspx";
                _redirectUrlType = "internal";
                _redirectUrl = String.Format(_redirectUrl + "?args={0}", queryString.ToEncryptedQueryString());
            }
            else if (paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())
            {
                _redirectUrl = "Pages/PaypalPaymentSubmission.aspx";
                _redirectUrl = String.Format(_redirectUrl + "?args={0}", queryString.ToEncryptedQueryString());
            }

            //if (!url.StartsWith("http://"))
            //    url = "http://" + url;

            if (!url.ToLower().StartsWith("http"))
            {
                url = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter + url;
            }

            //System.Web.HttpContext.Current.Application[generatedInvoiceNumber] = url;  
            Int32 timeout = GetSessionTimeoutValue();
            ApplicationDataManager.AddWebApplicationData(generatedInvoiceNumber, url, timeout);
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            return javaScriptSerializer.Serialize(new { redirectUrl = _redirectUrl, redirectUrlType = _redirectUrlType });
            //return _redirectUrl;
        }

        /// <summary>
        /// Redirect applicant to Confirmation Page, if order gets paid immediately
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static String RedirectConfirmationPage(String errorMessage)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", ChildControls.ApplicantOrderConfirmation},
                                                                           { "error",errorMessage }
                                                                 };
            String url = String.Format("Default.aspx?args={0}", queryString.ToEncryptedQueryString());
            return url;
        }

        public static String RedirectRushOrderConfirmPage(String errorMessage)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 {
                                                                    { "Child", ChildControls.ApplicantRushOrderConfirmPage},
                                                                    { "error",errorMessage }
                                                                 };
            String url = String.Format("Default.aspx?args={0}", queryString.ToEncryptedQueryString());
            return url;
        }

        [WebMethod]
        public static Boolean IsDocumentAlreadyUploaded(String documentName, String documentSize, String organizationUserId = "", Boolean isPersonalDocumentScreen = false)
        {
            Int32 _organizationUserID = organizationUserId.IsNullOrEmpty() ? 0 : Convert.ToInt32(organizationUserId);
            if (_organizationUserID <= 0)
            {
                _organizationUserID = SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
            Int32 _tenantId = SecurityManager.GetOrganizationUser(_organizationUserID).Organization.TenantID.Value;
            return ComplianceDataManager.IsDocumentAlreadyUploaded(documentName, Convert.ToInt32(documentSize), _organizationUserID, _tenantId, isPersonalDocumentScreen);
        }

        [WebMethod]
        public static String CreateHtmlFile(String strHtmlCode, String orderNo, String orderNumber)
        {
            strHtmlCode = HttpUtility.UrlDecode(strHtmlCode , System.Text.Encoding.Default);
            return SavePageHtmlContet(strHtmlCode, orderNo, orderNumber);

            //Commented below code related to Print Receipt bug[SS]:[07/04/2016]
            //if (SavePageHtmlContet(strHtmlCode, orderNo))
            //{
            //    return _fileIdentifier;
            //}
            //else
            //{
            //    return String.Empty;
            //}
        }

        public static String CreateHtmlFileOnRender(String strHtmlCode, String orderNo, String orderNumber, String fileIdentifier = null)
        {
            strHtmlCode = HttpUtility.UrlDecode(strHtmlCode);
            return SavePageHtmlContet(strHtmlCode, orderNo, orderNumber, fileIdentifier);
        }
        /// <summary>
        /// Web method to update the utilityFeatureUsage
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="ignoreAlert">ignoreAlert</param>
        [WebMethod]
        public static void UpdateUtilityFeature(Int32 organizationUserId, Boolean ignoreAlert)
        {
            ComplianceDataManager.UpdateUtilityFeatureUsage(organizationUserId, UtilityFeatures.Ctrl_Save.GetStringValue(), ignoreAlert);
        }

        [WebMethod]
        public static void RemindLater(Boolean ignoreAlert)
        {
            HttpContext.Current.Session["RemindLater"] = ignoreAlert;
        }

        /// <summary>
        /// Web method to update the utilityFeatureUsage for Dock/UnDock Features
        /// </summary>
        /// <param name="userId">organizationUserId</param>
        /// <param name="ignoreAlert">ignoreAlert</param>
        [WebMethod]
        public static void UpdateUtilityFeatureForDockUnDock(Int32 organizationUserId, Boolean ignoreAlert)
        {
            ComplianceDataManager.UpdateUtilityFeatureUsage(organizationUserId, UtilityFeatures.Dock_UnDock.GetStringValue(), ignoreAlert);
        }

        /// <summary>
        /// Web method to Retset the utilityFeatureUsage for Dock/UnDock Features
        /// </summary>
        /// <param name="userId">organizationUserId</param>
        /// <param name="ignoreAlert">ignoreAlert</param>
        [WebMethod]
        public static void ResetUtilityFeatureForDockUnDock(Int32 organizationUserId, Boolean ignoreAlert)
        {
            if (organizationUserId > 0)
            {
                ComplianceDataManager.SaveUpdateUtilityFeatureUsage(organizationUserId, true, UtilityFeatures.Dock_UnDock.GetStringValue());
            }
        }

        [WebMethod]
        public static void RemindLaterForDockUnDock(Boolean ignoreAlert)
        {
            HttpContext.Current.Session["RemindLaterDockUnDock"] = ignoreAlert;
        }

        [WebMethod]
        public static String GetDataForUnifiedDocument(String documentId, String tenantId)
        {
            if (!documentId.IsNullOrEmpty() && !tenantId.IsNullOrEmpty())
            {
                var unifiedDocumentData = ComplianceDataManager.GetDataRelatedToUnifiedDocument(Convert.ToInt32(documentId), Convert.ToInt32(tenantId));
                if (unifiedDocumentData.IsNullOrEmpty())
                    return null;
                else
                    return "" + unifiedDocumentData.ADM_StartPageNum + ";" + unifiedDocumentData.ADM_UnifiedPdfDocumentID + "";
            }
            return null;
        }

        [WebMethod]
        public static String GetDataForFailedUnifiedDocument(String documentId, String tenantId)
        {
            if (!documentId.IsNullOrEmpty() && !tenantId.IsNullOrEmpty())
            {
                String documentType = "FailedUnifiedDocument";
                String _redirectUrl = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?tenantId={0}&documentId={1}&DocumentType={2}", tenantId, documentId, documentType);
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                return javaScriptSerializer.Serialize(new { redirectUrl = _redirectUrl });
            }
            return null;
        }

        //UAT-613
        /// <summary>
        /// Web method to SaveUpdate the Explanatory State for the User
        /// </summary>
        /// <param name="userId">organizationUserId</param>
        /// <param name="ExplanatoryState">explanatoryState</param>
        [WebMethod]
        public static void SaveUpdateExplanatoryState(String userId, String explanatoryTabId)
        {
            String explanationTabState = null;
            Guid userIdTemp = new Guid(userId);
            if (explanatoryTabId.IsNullOrEmpty() || explanatoryTabId.Trim().ToLower().Equals(("spnAdminExplanation").Trim().ToLower()))
            {
                explanationTabState = AppConsts.ADMIN_EXPLANATORY_STATE;
            }
            else if (explanatoryTabId.Trim().ToLower().Equals(("spnApplicantExplanation").Trim().ToLower()))
            {
                explanationTabState = AppConsts.APPLICANT_EXPLANATORY_STATE;
            }
            else if (explanatoryTabId.Trim().ToLower().Equals(("closedState").Trim().ToLower()))
            {
                explanationTabState = AppConsts.CLOSED_EXPLANATORY_STATE;
            }
            ComplianceDataManager.SaveUpdateExplanatoryState(userIdTemp, explanationTabState);
        }

        #region UAT-796

        [WebMethod]
        public static String ResetAutoRenewalStatus(Int32 tenantID, Int32 orderID, Int32 currentUserID, String buttonid)
        {
            String orderRenwalStatus = ComplianceDataManager.ResetAutoRenewalStatus(tenantID, orderID, currentUserID);
            string result = "{\"orderRenwalStatus\":\"" + Convert.ToString(orderRenwalStatus) + "\",\"buttonid\": \"" + buttonid + "\"}";
            return result;
        }

        #endregion

        #region Data Entry Time Tracking

        [WebMethod]
        public static String DataEntryTracking(Int32 tenantID)
        {
            DataEntryTrackingContract dataEntryTimeTracking = null;
            dataEntryTimeTracking = (DataEntryTrackingContract)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.DATA_ENTRY_TRACKING);

            if (dataEntryTimeTracking.IsNotNull())
            {
                dataEntryTimeTracking.ExitTime = DateTime.Now;
                ComplianceDataManager.DataEntryTimeTracking(dataEntryTimeTracking, tenantID);
                HttpContext.Current.Session.Remove(ResourceConst.DATA_ENTRY_TRACKING);
                return "Success";
            }
            return "";
        }

        #endregion

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Generate the Grouped pricing of the Packages, Grouped by lkpPaymentOption Code
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        /// <param name="lstPkgPaymentOptns"></param>
        /// <param name="compliancePkgPrice"></param>
        private static void GenerateGroupedAmount(ApplicantOrderCart applicantOrderCart, List<ApplicantOrderPaymentOptions> lstPkgPaymentOptns
            , Int32 tenantId, ApplicantOrderDataContract orderDataContract, List<Entity.ClientEntity.lkpPaymentOption> _lstClientPaymentOptions)
        {
            applicantOrderCart.lstPaymentGrouping = new List<PkgPaymentGrouping>();
            var _distinctPOIds = new List<ApplicantOrderPaymentOptions>();
            var _ccPaymentOptionId = _lstClientPaymentOptions.Where(po => po.Code == PaymentOptions.Credit_Card.GetStringValue()).First().PaymentOptionID;
            //UAT 4537 Allow the CC payment method packages that don’t require approval to go through, even while other packages within the same order are still pending approval 
            if (applicantOrderCart != null
                   && !applicantOrderCart.IsLocationServiceTenant && lstPkgPaymentOptns.IsNotNull() && lstPkgPaymentOptns.Count > AppConsts.NONE && lstPkgPaymentOptns.Count(x => x.poid == _ccPaymentOptionId && x.isAnyOptionsApprovalReq == true || x.isAnyOptionsApprovalReq == false) > AppConsts.ONE)
            {
                List<ApplicantOrderPaymentOptions> _cCPOIds = lstPkgPaymentOptns.Where(x => x.poid == _ccPaymentOptionId).ToList();
                _distinctPOIds = lstPkgPaymentOptns.DistinctBy(x => x.poid).Except(_cCPOIds).ToList();
                applicantOrderCart.lstPaymentGrouping = GenerateGroupedAmountForCreditCardPaymentType(applicantOrderCart, _cCPOIds, orderDataContract, _lstClientPaymentOptions, applicantOrderCart.IsLocationServiceTenant);
            }
            else
                _distinctPOIds = lstPkgPaymentOptns.DistinctBy(x => x.poid).ToList();

            foreach (var poItem in _distinctPOIds)
            {
                var _lstPkgs = lstPkgPaymentOptns.Where(po => po.poid == poItem.poid).ToList();

                PkgPaymentGrouping _pkgPayGroup = new PkgPaymentGrouping();
                _pkgPayGroup.PaymentModeCode = _lstClientPaymentOptions.Where(po => po.PaymentOptionID == poItem.poid).FirstOrDefault().Code;
                _pkgPayGroup.PaymentModeId = poItem.poid;
                _pkgPayGroup.TotalAmount = CalculateGroupAmount(_pkgPayGroup,
                                                                applicantOrderCart.lstApplicantOrder[0].lstPackages,
                                                                _lstPkgs,
                                                                applicantOrderCart.CompliancePackages.Values.ToList(),
                                                                orderDataContract, applicantOrderCart.IsLocationServiceTenant);

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
        /// Calculate the Grouped Amount for CC payment Options with approval required or without approval required.
        /// </summary>
        /// <param name="applicantOrderCart"></param>
        /// <param name="lstPkgPaymentOptns"></param>
        /// <param name="orderDataContract"></param>
        /// <param name="_lstClientPaymentOptions"></param>
        /// <returns></returns>
        private static List<PkgPaymentGrouping> GenerateGroupedAmountForCreditCardPaymentType(ApplicantOrderCart applicantOrderCart, List<ApplicantOrderPaymentOptions> lstPkgPaymentOptns
           , ApplicantOrderDataContract orderDataContract, List<Entity.ClientEntity.lkpPaymentOption> _lstClientPaymentOptions, Boolean IsLocationServiceTenant)
        {
            var _distinctPOIds = lstPkgPaymentOptns.DistinctBy(m => new { m.poid, m.isAnyOptionsApprovalReq }).ToList();

            List<PkgPaymentGrouping> lstPkgPaymentGrouping = new List<PkgPaymentGrouping>();
            foreach (var poItem in _distinctPOIds)
            {
                var _lstPkgs = lstPkgPaymentOptns.Where(po => po.poid == poItem.poid && po.isAnyOptionsApprovalReq == poItem.isAnyOptionsApprovalReq).ToList();

                PkgPaymentGrouping _pkgPayGroup = new PkgPaymentGrouping();
                _pkgPayGroup.PaymentModeCode = _lstClientPaymentOptions.Where(po => po.PaymentOptionID == poItem.poid).FirstOrDefault().Code;
                _pkgPayGroup.PaymentModeId = poItem.poid;
                _pkgPayGroup.TotalAmount = CalculateGroupAmount(_pkgPayGroup,
                                                                applicantOrderCart.lstApplicantOrder[0].lstPackages,
                                                                _lstPkgs,
                                                                applicantOrderCart.CompliancePackages.Values.ToList(),
                                                                orderDataContract, IsLocationServiceTenant);
                //UAT 4537
                _pkgPayGroup.IsApprovalRequiredPaymentGrouping = poItem.isAnyOptionsApprovalReq;
                // applicantOrderCart.lstPaymentGrouping.Add(_pkgPayGroup);
                lstPkgPaymentGrouping.Add(_pkgPayGroup);

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
                        //applicantOrderCart.lstPaymentGrouping.Add(_additionalPayOption);
                        lstPkgPaymentGrouping.Add(_additionalPayOption);
                    }
                }
            }

            return lstPkgPaymentGrouping;
        }

        /// <summary>
        /// Calculate the Grouped Amount for Different payment Options selected
        /// </summary>
        /// <param name="lstBkgPackages"></param>
        /// <param name="lstPkgPaymentOptns"></param>
        /// <returns></returns>
        private static Decimal CalculateGroupAmount(PkgPaymentGrouping _pkgPayGroup, List<BackgroundPackagesContract> lstBkgPackages,
                                                List<ApplicantOrderPaymentOptions> lstPkgPaymentOptns, List<OrderCartCompliancePackage> lstCompliancePackages,
                                            ApplicantOrderDataContract orderDataContract, Boolean IsLocationServiceTenant)
        {
            Decimal _totalAmount = 0;
            _pkgPayGroup.lstPackages = new Dictionary<String, Boolean>();
            if (IsLocationServiceTenant) //For CABS additional services, subtract billing amount from total price.
            {
                foreach (var pkg in lstPkgPaymentOptns)
                {
                    if (pkg.isbkg)
                    {
                        var _price = lstBkgPackages.Where(x => x.BPAId == pkg.pkgid).First().TotalBkgPackagePrice;
                        _totalAmount += _price.IsNull() ? AppConsts.NONE : _price;
                        
                    }
                    else
                    {
                        var _price = lstCompliancePackages.Where(x => x.CompliancePackageID == pkg.pkgid).FirstOrDefault().GrandTotal;
                        _totalAmount += Convert.ToDecimal(_price.IsNull() ? AppConsts.NONE : _price);
                        //orderDataContract.CompliancePkgPaymentOptionId = pkg.poid; TBD
                    }

                    _pkgPayGroup.lstPackages.Add(pkg.pkgid + "_" + Guid.NewGuid().ToString(), pkg.isbkg);
                }
                // UAT-3850
                if (orderDataContract.IsBillingCodeAmountAvlbl)
                {
                    if (_totalAmount > orderDataContract.BillingCodeAmount)
                    {
                        //    if (_pkgPayGroup.PaymentModeCode.ToLower() == PaymentOptions.InvoiceWithOutApproval.GetStringValue().ToLower())
                        //        _totalAmount = _totalAmount;
                        //    //else
                        //    //    _totalAmount = _totalAmount - orderDataContract.BillingCodeAmount;
                        //}
                        //else
                        //{
                        if (_pkgPayGroup.PaymentModeCode.ToLower() == PaymentOptions.InvoiceWithOutApproval.GetStringValue().ToLower())
                            _totalAmount = orderDataContract.BillingCodeAmount;
                        else
                            _totalAmount = _totalAmount - orderDataContract.BillingCodeAmount;
                    }
                }
                //end 
                return _totalAmount;
            }
            else
            {
                foreach (var pkg in lstPkgPaymentOptns)
                {
                    if (pkg.isbkg)
                    {
                        var _price = lstBkgPackages.Where(x => x.BPAId == pkg.pkgid).First().TotalBkgPackagePrice;
                        _totalAmount += _price.IsNull() ? AppConsts.NONE : _price;

                        // UAT-3850
                        if (orderDataContract.IsBillingCodeAmountAvlbl)
                        {
                            if (_totalAmount > orderDataContract.BillingCodeAmount)
                            {
                                //    if (_pkgPayGroup.PaymentModeCode.ToLower() == PaymentOptions.InvoiceWithOutApproval.GetStringValue().ToLower())
                                //        _totalAmount = _totalAmount;
                                //    //else
                                //    //    _totalAmount = _totalAmount - orderDataContract.BillingCodeAmount;
                                //}
                                //else
                                //{
                                if (_pkgPayGroup.PaymentModeCode.ToLower() == PaymentOptions.InvoiceWithOutApproval.GetStringValue().ToLower())
                                    _totalAmount = orderDataContract.BillingCodeAmount;
                                else
                                    _totalAmount = _totalAmount - orderDataContract.BillingCodeAmount;
                            }
                        }
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
        }

        /// <summary>
        /// Method to update the EDS Related Data For customformdata and also update the external vendor dispatch status.
        /// </summary>
        /// <param name="applicantOrderDataContract">applicantOrderDataContract</param>
        /// <param name="orderId">orderId</param>
        /// <param name="userOrder">userOrder</param>
        private static void UpdateEDSStatus(ApplicantOrderDataContract applicantOrderDataContract, Int32 orderId, Order userOrder, List<Int32> newlyAddedOPDIds = null)
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
                        if (!opd.IsNullOrEmpty() && ComplianceDataManager.IsOrderPaymentIncludeEDSService(applicantOrderDataContract.TenantId, opd.OPD_ID) && opd.lkpPaymentOption.IsNotNull() && opd.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue())
                        {
                            _orderPaymentDetail = opd;
                            break;
                        }
                    }
                }

                if (orderId > 0 && !_orderPaymentDetail.IsNullOrEmpty())
                {
                    String _prevStatus = ApplicantOrderStatus.Paid.GetStringValue();
                    Int32 orderStatusId = ComplianceDataManager.GetOrderStatusList(applicantOrderDataContract.TenantId).Where(orderSts => orderSts.Code.ToLower() == _prevStatus.ToLower() && !orderSts.IsDeleted)
                                 .FirstOrDefault().OrderStatusID;
                    #region E-DRUG SCREENING
                    BkgOrder bkgOrderObj = BackgroundProcessOrderManager.GetBkgOrderByOrderID(applicantOrderDataContract.TenantId, orderId);
                    if (!bkgOrderObj.IsNullOrEmpty() && !applicantOrderDataContract.lstBackgroundPackages.IsNullOrEmpty() && (applicantOrderDataContract.lstBackgroundPackages.Count() > 0))
                    {
                        List<Int32> lstBackgroundPackageId = applicantOrderDataContract.lstBackgroundPackages.Select(cnd => cnd.BPAId).ToList();
                        String extVendorId = String.Empty;
                        ClearStarCCF objClearstarCCf = new ClearStarCCF();

                        ClearStarWebCCFContract clearStarWebCCFContract = new ClearStarWebCCFContract();
                        var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                        var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                        String result = BackgroundProcessOrderManager.GetClearStarServiceId(applicantOrderDataContract.TenantId, lstBackgroundPackageId, BkgServiceType.ELECTRONICDRUGSCREEN.GetStringValue());
                        if (!result.IsNullOrEmpty())
                        {
                            String[] separator = { "," };
                            String[] splitIds = result.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            extVendorId = splitIds[1];
                        }

                        //Update BkgOrderSvcLineItem Status to DisptachOnHold_WaitingForEDSData for background package that contains EDS service.
                        if (!extVendorId.IsNullOrEmpty())
                        {
                            BackgroundProcessOrderManager.UpdateBkgOrderSvcLineItem(applicantOrderDataContract.TenantId, Convert.ToInt32(extVendorId), bkgOrderObj.BOR_ID, SvcLineItemDispatchStatus.DISPTACH_ON_HOLD_WAITING_FOR_EDS_DATA.GetStringValue(), userOrder.CreatedByID);
                            BasePage.LogOrderFlowSteps("Default.aspx - STEP 3.1: Method 'BackgroundProcessOrderManager.UpdateBkgOrderSvcLineItem' executed successfully for OrderId:" + orderId);
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
                            BasePage.LogOrderFlowSteps("Default.aspx - STEP 3.2: Parallel Task 'BackgroundProcessOrderManager.RunParallelTaskSaveCCFDataAndPDF' started for OrderId:" + orderId
                                 + " and BkgOrderId: " + bkgOrderObj.BOR_ID);
                        }
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// Method to update the EDS Related Data For customformdata and also update the external vendor dispatch status.
        /// </summary>
        /// <param name="applicantOrderDataContract">applicantOrderDataContract</param>
        /// <param name="orderId">orderId</param>
        /// <param name="userOrder">userOrder</param>
        private static void UpdateEDSDataForChangePaymentType(OrderPaymentDetail orderPaymentDetail, Int32 orderId, Order userOrder, Int32 tenantID)
        {
            if (userOrder.IsNotNull())
            {
                OrderPaymentDetail _orderPaymentDetail = null;
                if (!orderPaymentDetail.IsNullOrEmpty() && ComplianceDataManager.IsOrderPaymentIncludeEDSService(tenantID, orderPaymentDetail.OPD_ID))
                {
                    _orderPaymentDetail = orderPaymentDetail;
                }



                if (orderId > 0 && !_orderPaymentDetail.IsNullOrEmpty())
                {
                    String _prevStatus = ApplicantOrderStatus.Paid.GetStringValue();
                    Int32 orderStatusId = ComplianceDataManager.GetOrderStatusList(tenantID).Where(orderSts => orderSts.Code.ToLower() == _prevStatus.ToLower() && !orderSts.IsDeleted)
                                 .FirstOrDefault().OrderStatusID;
                    #region E-DRUG SCREENING
                    BkgOrder bkgOrderObj = BackgroundProcessOrderManager.GetBkgOrderByOrderID(tenantID, orderId);
                    if (!bkgOrderObj.IsNullOrEmpty() && !bkgOrderObj.BkgOrderPackages.IsNullOrEmpty() && (bkgOrderObj.BkgOrderPackages.Count() > 0))
                    {
                        List<Int32> lstBackgroundPackageId = bkgOrderObj.BkgOrderPackages.Select(x => x.BkgPackageHierarchyMapping.BPHM_BackgroundPackageID).ToList();
                        String extVendorId = String.Empty;
                        ClearStarCCF objClearstarCCf = new ClearStarCCF();

                        ClearStarWebCCFContract clearStarWebCCFContract = new ClearStarWebCCFContract();
                        var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                        var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                        String result = BackgroundProcessOrderManager.GetClearStarServiceId(tenantID, lstBackgroundPackageId, BkgServiceType.ELECTRONICDRUGSCREEN.GetStringValue());
                        if (!result.IsNullOrEmpty())
                        {
                            String[] separator = { "," };
                            String[] splitIds = result.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            extVendorId = splitIds[1];
                        }

                        //Update BkgOrderSvcLineItem Status to DisptachOnHold_WaitingForEDSData for background package that contains EDS service.
                        if (!extVendorId.IsNullOrEmpty())
                        {
                            //BackgroundProcessOrderManager.UpdateBkgOrderSvcLineItem(tenantID, Convert.ToInt32(extVendorId), bkgOrderObj.BOR_ID, SvcLineItemDispatchStatus.DISPTACH_ON_HOLD_WAITING_FOR_EDS_DATA.GetStringValue(), userOrder.CreatedByID);
                            BackgroundProcessOrderManager.UpdateBkgOrderSvcLineItem(tenantID, Convert.ToInt32(extVendorId), bkgOrderObj.BOR_ID, SvcLineItemDispatchStatus.DISPTACH_ON_HOLD_WAITING_FOR_EDS_DATA.GetStringValue(), Convert.ToInt32(userOrder.ModifiedByID));
                        }
                        //Update status PSLI_DispatchedExternalVendor from DisptachOnHold_WaitingForEDSData to Dispatched
                        if (_orderPaymentDetail.IsNotNull() && _orderPaymentDetail.OPD_OrderStatusID.IsNotNull() && _orderPaymentDetail.OPD_OrderStatusID == orderStatusId && !extVendorId.IsNullOrEmpty())
                        {
                            //Create dictionary for parallel task parameter.
                            Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                            dicParam.Add("BkgOrderId", bkgOrderObj.BOR_ID);
                            dicParam.Add("TenantId", tenantID);
                            dicParam.Add("ExtVendorId", Convert.ToInt32(extVendorId));
                            dicParam.Add("BPHMId_List", bkgOrderObj.BkgOrderPackages.Select(x => x.BOP_BkgPackageHierarchyMappingID).ToList());
                            dicParam.Add("RegistrationId", String.Empty);
                            //dicParam.Add("CurrentLoggedInUserId", userOrder.CreatedByID);
                            dicParam.Add("CurrentLoggedInUserId", userOrder.ModifiedByID);
                            dicParam.Add("OrganizationUserId", bkgOrderObj.OrganizationUserProfile.OrganizationUserID);
                            dicParam.Add("OrganizationUserProfileId", bkgOrderObj.BOR_OrganizationUserProfileID);
                            dicParam.Add("ApplicantName", string.Concat(bkgOrderObj.OrganizationUserProfile.FirstName, " ", bkgOrderObj.OrganizationUserProfile.LastName));
                            dicParam.Add("PrimaryEmailAddress", bkgOrderObj.OrganizationUserProfile.PrimaryEmailAddress);
                            //Pass selectedNodeId in place of HierarchyId [UAT-1067]
                            //dicParam.Add("HierarchyNodeId", bkgOrderObj.Order.HierarchyNodeID.Value);
                            dicParam.Add("HierarchyNodeId", bkgOrderObj.Order.SelectedNodeID.Value);
                            BackgroundProcessOrderManager.RunParallelTaskSaveCCFDataAndPDF(objClearstarCCf.SaveCCFDataAndPDF, dicParam, LoggerService, ExceptiomService, tenantID);
                        }
                    }
                    #endregion
                }
            }
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
                    // _backgroundPackagesPrice += bkgPackage.TotalPrice.IsNullOrEmpty() ? AppConsts.NONE : bkgPackage.TotalPrice;

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

                    //UAT-4498
                    _orderLineItem.IsDummyLineItem = XmlConvert.ToBoolean(_ordLineItem.Element("IsDummyLineItem").Value);

                    _packagePricingData.lstOrderLineItems.Add(_orderLineItem);
                }

                #endregion

                _lstData.Add(_packagePricingData);
            }
            return _lstData;
        }

        private static String SavePageHtmlContet(String pageHtml, String orderNo, String orderNumber, String fileGuid = null)
        {
            if (!String.IsNullOrEmpty(pageHtml))
            {
                ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;
                Int32 WebSiteId = Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.CLIENT_WEB_SITE_ID));
                String LoginPageImageUrl = String.Empty;
                if (WebSiteId > AppConsts.NONE && applicantOrderCart.IsLocationServiceTenant)
                    LoginPageImageUrl = WebSiteManager.GetWebSiteHeaderHtml(WebSiteId);
                if (!LoginPageImageUrl.IsNullOrEmpty())
                {
                    LoginPageImageUrl = LoginPageImageUrl.Substring(LoginPageImageUrl.IndexOf("src=") + 4, (LoginPageImageUrl.IndexOf("/></div>") - (LoginPageImageUrl.IndexOf("src=") + 4)));
                    LoginPageImageUrl = LoginPageImageUrl.Replace("..", "").Replace("/", "\\").Replace("\"", "");
                }
                String LocTenantCompanyName = SecurityManager.GetLocationTenantCompanyName();
                String filePath = String.Empty;
                String fileIdentifier = String.Empty;
                String orderConfirnmationfileName = String.Empty;
                String applicantFileLocation = String.Empty;
                filePath = ConfigurationManager.AppSettings["TemporaryFileLocation"].ToString();
                if (!filePath.EndsWith("\\"))
                {
                    filePath += "\\";
                }
                filePath += @"HTMLConversion\";
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                Guid Id;
                if (!fileGuid.IsNullOrEmpty())
                {
                    Id = Guid.Parse(fileGuid);
                }
                else
                {
                    Id = Guid.NewGuid();
                }

                String fileName = "TempFile_" + Id + ".txt";
                if (!String.IsNullOrEmpty(orderNo))
                {
                    orderConfirnmationfileName = "OrderConfirmation" + "_" + orderNumber;
                }
                fileIdentifier = Convert.ToString(Id);
                if (!File.Exists(fileName))
                {
                    filePath = Path.Combine(filePath, fileName);

                    using (FileStream fs = File.Create(filePath))
                    {
                        //UAT 1212 Complio Receipt (Order Summary) Enhancement
                        StringBuilder customHtmlString = new StringBuilder();
                        //Without address header
                        //customHtmlString.Append("<div><img src=\"\\Resources\\Mod\\Shared\\images\\login\\adbRecieptLogo.png\" style=\"height:80px;\"><hr/></div>");
                        //With address header
                        if (applicantOrderCart.IsLocationServiceTenant)
                            customHtmlString.Append("<div><img src=\"" + LoginPageImageUrl + "\" style=\"height: 80px;\"><div style=\"float: right; height: 80px; width: 200px;\"><p style=\"line-height:2px;\">" + LocTenantCompanyName + "</p><p style=\"line-height:2px;\"> 110 16th Street 8th Floor</p><p style=\"line-height:2px;\"> Denver, CO 80202</p> <p style=\"line-height:2px;\">Phone: (720) 292-2722</p> </div><hr /></div>");
                        else
                            customHtmlString.Append("<div><img src=\"\\Resources\\Mod\\Shared\\images\\login\\adbRecieptLogo.png\" style=\"height: 80px;\"><div style=\"float: right; height: 80px; width: 200px;\"><p style=\"line-height:2px;\">American DataBank, L.L.C.</p><p style=\"line-height:2px;\"> 110 16th Street 8th Floor</p><p style=\"line-height:2px;\"> Denver, CO 80202</p> <p style=\"line-height:2px;\">Phone: (800) 200-0853</p> </div><hr /></div>");
                        Int32 startIndex = pageHtml.IndexOf("150%");
                        StringBuilder tmp = new StringBuilder();
                        tmp.Append(pageHtml);
                        if (startIndex > -1)
                        {
                            tmp.Replace("150%", "120%", startIndex, 4);
                        }

                        customHtmlString.Append(tmp.ToString());
                        Byte[] info = new UTF8Encoding(true).GetBytes(customHtmlString.ToString());
                        fs.Write(info, 0, info.Length);
                    }
                    //Check whether use AWS S3, true if need to use
                    Boolean aWSUseS3 = false;
                    if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                    {
                        aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                    }
                    if (aWSUseS3 == false)
                    {
                        //Move file to other location
                        applicantFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"].ToString();
                        if (!applicantFileLocation.EndsWith("\\"))
                        {
                            applicantFileLocation += "\\";
                        }
                        applicantFileLocation += @"HTMLConversion\";
                        if (!Directory.Exists(applicantFileLocation))
                        {
                            Directory.CreateDirectory(applicantFileLocation);
                        }
                        applicantFileLocation = Path.Combine(applicantFileLocation, fileName);
                        MoveFile(filePath, applicantFileLocation);
                    }
                    else
                    {
                        applicantFileLocation = ConfigurationManager.AppSettings["ApplicantFileLocation"].ToString();
                        if (!applicantFileLocation.EndsWith("//"))
                        {
                            applicantFileLocation += "//";
                        }
                        //AWS code to save document to S3 location
                        AmazonS3Documents objAmazonS3 = new AmazonS3Documents();
                        String destFolder = applicantFileLocation + @"HTMLConversion/";
                        String returnFilePath = objAmazonS3.SaveDocument(filePath, fileName, destFolder);
                        applicantFileLocation = returnFilePath; //destFolder + fileName;
                        try
                        {
                            if (!String.IsNullOrEmpty(filePath))
                                File.Delete(filePath);
                        }
                        catch (Exception) { }
                    }
                    //SaveFilePath(_filePath, Id, orderConfirnmationfileName);
                    SaveFilePath(applicantFileLocation, Id, orderConfirnmationfileName);
                    return fileIdentifier;
                }
                else
                {
                    return String.Empty;
                }
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Move file to other location
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationFilePath"></param>
        private static void MoveFile(String sourceFilePath, String destinationFilePath)
        {
            if (!String.IsNullOrEmpty(sourceFilePath))
            {
                File.Copy(sourceFilePath, destinationFilePath);
            }
            try
            {
                if (!String.IsNullOrEmpty(sourceFilePath))
                    File.Delete(sourceFilePath);
            }
            catch (Exception) { }
        }

        private static Boolean SaveFilePath(String filePath, Guid Id, String fileName)
        {
            TempFile tempFile = new TempFile();
            tempFile.TF_Path = filePath;
            tempFile.TF_Identifier = Id;
            tempFile.TF_IsDeleted = false;
            tempFile.TF_CreatedOn = DateTime.Now;
            tempFile.TF_CreatedByID = 1;
            tempFile.TF_FileName = fileName;
            return SecurityManager.SavePageHtmlContentLocation(tempFile);
        }

        /// <summary>
        /// Get the session timeout value to set the application data timeout value.
        /// </summary>
        /// <returns>Timeout Value in Second. (300 by default)</returns>
        private static Int32 GetSessionTimeoutValue()
        {
            Int32 timeout = 300;
            var sessionSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");

            //Check if session section is defined in Web.Config or not.
            if (sessionSection.IsNotNull())
            {
                timeout = (Int32)sessionSection.Timeout.TotalSeconds;
            }
            return timeout;
        }

        /// <summary>
        /// Gets the Order Package type, based on the packages selected
        /// </summary>
        /// <param name="_tenantId"></param>
        /// <param name="applicantOrderCart"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        private static Int32 GetOrderPackageType(Int32 tenantId, ApplicantOrderCart applicantOrderCart)
        {
            List<lkpOrderPackageType> _lstOrderPackageType = LookupManager.GetLookUpData<lkpOrderPackageType>(tenantId);

            if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.IsCompliancePackageSelected)
            {
                return _lstOrderPackageType.Where(opt => opt.OPT_Code == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
            }
            else if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && !applicantOrderCart.IsCompliancePackageSelected)
            {
                return _lstOrderPackageType.Where(opt => opt.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
            }
            else if (applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.IsCompliancePackageSelected)
            {
                return _lstOrderPackageType.Where(opt => opt.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
            }
            return AppConsts.MINUS_ONE;
        }

        /// <summary>
        /// Gets the HierarchyNodeID on basis of Order Package type(the packages selected)
        /// </summary>
        /// <param name="_tenantId"></param>
        /// <param name="applicantOrderCart"></param>
        /// <returns>HierarchyID</returns>
        private static Int32 GetHierarchyNodeIDByPackageType(Int32 tenantId, ApplicantOrderCart applicantOrderCart)
        {
            Int32? dppID = null;
            Int32? bphmID = null;
            if ((!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.IsCompliancePackageSelected)
                || (applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.IsCompliancePackageSelected))
            {
                if (!applicantOrderCart.DPP_Id.IsNullOrEmpty())
                    dppID = applicantOrderCart.DPP_Id;
                return ComplianceDataManager.GetHierarchyNodeID(dppID, bphmID, tenantId);
            }
            else if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && !applicantOrderCart.IsCompliancePackageSelected)
            {
                if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() && applicantOrderCart.lstApplicantOrder[0].lstPackages.Count > 0)
                    bphmID = applicantOrderCart.lstApplicantOrder[0].lstPackages[0].BPHMId;
                return ComplianceDataManager.GetHierarchyNodeID(dppID, bphmID, tenantId);
            }
            return AppConsts.MINUS_ONE;
        }

        /// <summary>
        /// Send Email and Messages related to Order Process
        /// </summary>
        /// <param name="_paymentModeCode"></param>
        /// <param name="_tenantId"></param>
        /// <param name="_errorMessage"></param>
        /// <param name="_orgUserProfile"></param>
        /// <param name="_order"></param>
        private static void SendNotifications(String _paymentModeCode, Int32 _tenantId, String _errorMessage, Entity.ClientEntity.OrganizationUserProfile _orgUserProfile, Order _order)
        {


            #region UAT-1697: Add new client setting to make it where all subscription renewals nees to be approved, even if payment method is invoice without approval
            Boolean ifRenewalOrderApprovalRequired = false;
            String orderRequestTypeCode = _order.lkpOrderRequestType.IsNotNull() ? _order.lkpOrderRequestType.ORT_Code : String.Empty;
            if (orderRequestTypeCode == OrderRequestType.RenewalOrder.GetStringValue() &&
                _paymentModeCode.ToLower() == PaymentOptions.InvoiceWithOutApproval.GetStringValue().ToLower())
            {
                //Check for client settings
                String rnwlOrderAprvlRqdCode = Setting.SUBSCRIPTION_RENEWAL_NEED_APPROVAL.GetStringValue();
                ClientSetting rnwlOrderAprvlRqdSetting = ComplianceDataManager.GetClientSetting(_tenantId).Where(cond => cond.lkpSetting.Code
                                                                                           == rnwlOrderAprvlRqdCode && !cond.CS_IsDeleted).FirstOrDefault();
                if (!rnwlOrderAprvlRqdSetting.IsNullOrEmpty() &&
                    !rnwlOrderAprvlRqdSetting.CS_SettingValue.IsNullOrEmpty())
                {
                    ifRenewalOrderApprovalRequired = Convert.ToBoolean(Convert.ToInt32(rnwlOrderAprvlRqdSetting.CS_SettingValue));
                }
            }
            #endregion

            //Send Mail and Message Notification
            if (String.IsNullOrEmpty(_errorMessage))
            {
                if (_paymentModeCode.Equals(PaymentOptions.Money_Order.GetStringValue(), StringComparison.OrdinalIgnoreCase) && !SecurityManager.IsLocationServiceTenant(_tenantId))
                {
                    CommunicationManager.SendOrderCreationMailMoneyOrder(_order, _orgUserProfile, _tenantId, _paymentModeCode);
                    CommunicationManager.SendOrderCreationMessageMoneyOrder(_order, _orgUserProfile, _tenantId, _paymentModeCode);
                }
                else if (_paymentModeCode.Equals(PaymentOptions.InvoiceWithApproval.GetStringValue(), StringComparison.OrdinalIgnoreCase)
                    || (_paymentModeCode.Equals(PaymentOptions.InvoiceWithOutApproval.GetStringValue(), StringComparison.OrdinalIgnoreCase)
                         && ifRenewalOrderApprovalRequired) //UAT-1697 related changes.
                    )
                {
                    CommunicationManager.SendOrderCreationMailInvoice(_order, _orgUserProfile, _tenantId, _paymentModeCode);
                    CommunicationManager.SendOrderCreationMessageInvoice(_order, _orgUserProfile, _tenantId, _paymentModeCode);
                }
            }
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

        #endregion

        #region UAT-1358:Complio Notification to applicant for PrintScan
        /// <summary>
        /// Method to Send the notification for print scan service type order.
        /// </summary>
        /// <param name="applicantOrderDataContract">applicantOrderDataContract</param>
        /// <param name="orderId">orderId</param>
        /// <param name="userOrder">userOrder</param>
        /// <param name="orderPaymentDetail">orderPaymentDetail</param>
        /// <param name="isChangePaymentTypeRequest">isChangePaymentTypeRequest</param>
        /// <param name="tenantId">tenantId</param>
        private static void SendPrintScanNotification(Int32 orderId, Order userOrder,
                                                      OrderPaymentDetail orderPaymentDetail, Boolean isChangePaymentTypeRequest, Int32 tenantId, List<Int32> newlyAddedOPDIds = null)
        {
            if (userOrder.IsNotNull())
            {
                OrderPaymentDetail _orderPaymentDetail = null;
                String bkgServiceTypeCode = BkgServiceType.PRINT_SCAN.GetStringValue();

                String orderPaidStatusCode = ApplicantOrderStatus.Paid.GetStringValue();
                Int32 orderPaidStatusId = ComplianceDataManager.GetOrderStatusList(tenantId).Where(orderSts => orderSts.Code.ToLower() == orderPaidStatusCode.ToLower()
                                          && !orderSts.IsDeleted).FirstOrDefault().OrderStatusID;

                if (isChangePaymentTypeRequest)
                {
                    if (orderPaymentDetail.IsNotNull() && BackgroundProcessOrderManager.IsBkgServiceExistInOrder(tenantId, orderPaymentDetail.OPD_ID, bkgServiceTypeCode))
                    {
                        _orderPaymentDetail = orderPaymentDetail;
                    }
                }
                else
                {
                    foreach (OrderPaymentDetail opd in userOrder.OrderPaymentDetails.Where(slct => !slct.OPD_IsDeleted
                                                                                        && (newlyAddedOPDIds == null || newlyAddedOPDIds.Contains(slct.OPD_ID))
                                                                                       ))
                    {
                        if (!opd.IsNullOrEmpty() && BackgroundProcessOrderManager.IsBkgServiceExistInOrder(tenantId, opd.OPD_ID, bkgServiceTypeCode)
                            && opd.lkpPaymentOption.IsNotNull() && opd.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue())
                        {
                            _orderPaymentDetail = opd;
                            break;
                        }
                    }
                }

                if (!_orderPaymentDetail.IsNullOrEmpty() && _orderPaymentDetail.OPD_OrderStatusID.IsNotNull() && _orderPaymentDetail.OPD_OrderStatusID == orderPaidStatusId)
                {
                    CommunicationManager.SendNotificationForPrintScan(_orderPaymentDetail, tenantId);
                    BasePage.LogOrderFlowSteps("Default.aspx - STEP 3.3: Method 'CommunicationManager.SendNotificationForPrintScan' executed successfully for OrderId:" + orderId);
                }
            }
        }
        #endregion

        #region UAT-1538:Unified Document/ single document option and updates to document exports
        /// <summary>
        /// Web method to Retset the utilityFeatureUsage for Unified/Single document view type
        /// </summary>
        /// <param name="userId">organizationUserId</param>
        /// <param name="ignoreAlert">viewType</param>
        [WebMethod]
        public static void SaveUpdateDocumentViewSetting(Int32 organizationUserId, String viewType)
        {
            if (organizationUserId > 0)
            {
                ComplianceDataManager.SaveUpdateDocumentViewSetting(organizationUserId, viewType);
            }
        }

        [WebMethod]
        public static String GetSingleDocumentForPDFViewer(String documentId, String tenantId)
        {
            if (!documentId.IsNullOrEmpty() && !tenantId.IsNullOrEmpty())
            {
                Dictionary<String, String> requestSingleDocViewerArgs = new Dictionary<String, String>();
                requestSingleDocViewerArgs = new Dictionary<String, String>
                                                                 {
                                                                    {"OrganizationUserId", AppConsts.ZERO },
                                                                    {"SelectedTenantId", tenantId},
                                                                    {"SelectedCatUnifiedStartPageID",AppConsts.ZERO},
                                                                    {"DocumentId",documentId},
                                                                    {"IsRequestAuth",Convert.ToString(AppConsts.TRUE)},
                                                                    {"DocumentViewType",UtilityFeatures.Single_Document.GetStringValue()}
                                                                 };
                //hdnSelectedCatUnifiedStartPageID.Value = Convert.ToString(selectedCatUnifiedStartPageID);
                String _redirectUrl = String.Format(@"/ComplianceOperations/UnifiedPdfDocViewer.aspx?args={0}", requestSingleDocViewerArgs.ToEncryptedQueryString());
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                return javaScriptSerializer.Serialize(new { redirectUrl = _redirectUrl });
            }
            return null;
        }
        #endregion

        #region UAT-1578 : Addition of SMS notification
        public static Boolean SaveUpdateSMSNotificationData(Int32 applicantID, String phoneNumber, Int32 currentLoggedInUserID, Boolean receiveTextNotification)
        {
            try
            {
                SMSNotificationManager.SaveUpdateSMSData(applicantID, phoneNumber, currentLoggedInUserID, receiveTextNotification);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region UAT-2836
        [WebMethod]
        public static List<Tuple<Int32, String, String>> GetCategoryUpdatedUrl(String PackageSubscription, String Tenant)
        {
            Int32 PackageSubscriptionId = Convert.ToInt32(PackageSubscription);
            Int32 TenantId = Convert.ToInt32(Tenant);
            //UAT 3106
            //UAT 3683
            //String OptionalCategoryClientSetting = AppConsts.STR_ONE;
            Boolean OptionalCategoryClientSetting = ComplianceDataManager.GetOptionalCategorySettingForNode(TenantId, AppConsts.NONE, PackageSubscriptionId, SubscriptionTypeCategorySetting.COMPLIANCE_PACKAGE.GetStringValue());
            //ClientSetting OptionalCategorySetting = ComplianceDataManager.GetClientSetting(TenantId, Setting.EXECUTE_COMPLIANCE_RULE_WHEN_OPTIONAL_CATEGORY_COMPLIANCE_RULE_MET.GetStringValue());
            //if (!OptionalCategorySetting.IsNullOrEmpty())
            //{
            //    OptionalCategoryClientSetting = OptionalCategorySetting.CS_SettingValue;
            //}

            if (PackageSubscriptionId <= AppConsts.NONE || TenantId <= AppConsts.NONE)
            {
                return new List<Tuple<int, string, string>>();
            }

            List<INTSOF.Utils.CommonPocoClasses.ComplianceCategoryPocoClass> lstCategoryDetails = Business.RepoManagers.ComplianceDataManager.GetApplicantComplianceCategoryData(PackageSubscriptionId, TenantId);

            List<Tuple<Int32, String, String>> result = new List<Tuple<Int32, String, String>>();

            foreach (var categoryDetails in lstCategoryDetails)
            {
                if (!categoryDetails.CategoryStatusName.IsNullOrEmpty() && !categoryDetails.CategoryStatusCode.IsNullOrEmpty())
                {
                    var catComplianceStatusName = categoryDetails.CategoryStatusName;
                    var catComplianceStatus = categoryDetails.CategoryStatusCode;
                    var CategoryExceptionStatusCode = categoryDetails.CategoryExceptionStatusCode == null ? String.Empty : categoryDetails.CategoryExceptionStatusCode;

                    if (catComplianceStatus.IsNullOrEmpty() || (catComplianceStatus == ApplicantCategoryComplianceStatus.Incomplete.GetStringValue()
                            && !String.IsNullOrEmpty(CategoryExceptionStatusCode)
                            && CategoryExceptionStatusCode == lkpCategoryExceptionStatus.EXCEPTION_APPLIED.GetStringValue()))
                    {
                        catComplianceStatusName = "Pending Review";
                        catComplianceStatus = ApplicantCategoryComplianceStatus.Pending_Review.GetStringValue();
                    }

                    string url = "";

                    if (catComplianceStatus.IsNullOrEmpty() || catComplianceStatus == ApplicantCategoryComplianceStatus.Incomplete.GetStringValue())
                    {
                        url = String.Format("../Resources/Mod/Compliance/icons/no16.png");
                    }
                    else if (!(String.IsNullOrEmpty(CategoryExceptionStatusCode)) && CategoryExceptionStatusCode == "AAAD" && catComplianceStatus == ApplicantCategoryComplianceStatus.Approved.GetStringValue())
                    {
                        url = String.Format("../Resources/Mod/Compliance/icons/yesx16.png");
                    }
                    else if (catComplianceStatus == ApplicantCategoryComplianceStatus.Approved.GetStringValue())
                    {
                        url = String.Format("../Resources/Mod/Compliance/icons/yes16.png");
                    }
                    else if (catComplianceStatus == ApplicantCategoryComplianceStatus.Approved_With_Exception.GetStringValue())
                    {
                        url = String.Format("../Resources/Mod/Compliance/icons/yesx16.png");
                    }
                    else if (catComplianceStatus == ApplicantCategoryComplianceStatus.Pending_Review.GetStringValue())
                    {
                        url = String.Format("../Resources/Mod/Compliance/icons/attn16.png");
                    }

                    //UAT 3106 
                    if (!categoryDetails.IsComplianceRequired)
                    {
                        if (!OptionalCategoryClientSetting) //UAT 3683
                        {
                            url = String.Format("../Resources/Mod/Compliance/icons/optional.png");
                        }
                        else if (categoryDetails.RulesStatusID.Trim() != "1")
                        {
                            url = String.Format("../Resources/Mod/Compliance/icons/optional.png");
                        }
                        else if (categoryDetails.RulesStatusID.Trim() == "1")
                        {
                            url = String.Format("../Resources/Mod/Compliance/icons/yes16.png");
                        }
                    }

                    String tooltipComplianceStatus = String.Empty;
                    if (catComplianceStatusName == "Approved" && CategoryExceptionStatusCode == "AAAD")
                    {
                        tooltipComplianceStatus = "Approved Override";
                    }
                    else
                    {
                        tooltipComplianceStatus = catComplianceStatusName;
                    }

                    result.Add(new Tuple<Int32, String, String>(categoryDetails.CategoryId, url, tooltipComplianceStatus));
                }
            }

            return result;
        }
        #endregion

        #region UAT-2970:Update Order confirmation emails to include the order confirmation document that gets added to the manage documents

        [WebMethod]
        public static void SetOrderConfirmationDocForCreditCard(String tenantIdForApp, String loggedInUserIdForApp, String orderIdForApp, String orderPaymentDetailIdForApp)
        {
            Int32 tenantId = AppConsts.NONE;
            Int32 orderId = AppConsts.NONE;
            Int32 currentLoggedInUser = AppConsts.NONE;
            OrderPaymentDetail opd = new OrderPaymentDetail();
            List<OrderPaymentDetail> lstOrderPaymentDetail = new List<OrderPaymentDetail>();
            Boolean isOrderHasCreditCardPaymentType = false;
            ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;
            if (orderPaymentDetailIdForApp != null && orderIdForApp != null && tenantIdForApp != null)
            {
                //case when order is approved by client admin.
                tenantId = Convert.ToInt32(tenantIdForApp);
                orderId = Convert.ToInt32(orderIdForApp);
                currentLoggedInUser = loggedInUserIdForApp != null ? Convert.ToInt32(loggedInUserIdForApp) : AppConsts.NONE;
                Int32 opdId = Convert.ToInt32(orderPaymentDetailIdForApp);

                if (tenantId > AppConsts.NONE && orderId > AppConsts.NONE)
                    lstOrderPaymentDetail = ComplianceDataManager.GetAllPaymentDetailsOfOrderByOrderID(tenantId, orderId);

                if (!lstOrderPaymentDetail.IsNullOrEmpty() && opdId > AppConsts.NONE)
                {
                    opd = lstOrderPaymentDetail.Where(con => con.OPD_ID == opdId && !con.OPD_IsDeleted).FirstOrDefault();
                    isOrderHasCreditCardPaymentType = opd.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue();
                }
            }
            if (isOrderHasCreditCardPaymentType && !opd.IsNullOrEmpty() && tenantId > AppConsts.NONE && (applicantOrderCart.IsNullOrEmpty() || !applicantOrderCart.IsLocationServiceTenant))
            {
                CommunicationManager.SendOrderConfirmationDocEmail(tenantId, currentLoggedInUser, opd, String.Empty);
            }
        }

        [WebMethod]
        public static String CheckExistingOrderInformation(String pkgID)
        {
            Int32 packageID = Convert.ToInt32(pkgID);
            Int32 currentLoggedInUser = SysXWebSiteUtils.SessionService.OrganizationUserId;
            Int32 TenantID = (((CoreWeb.IntsofSecurityModel.SysXMembershipUser)(SysXWebSiteUtils.SessionService.SysXMembershipUser))).TenantId.Value;

            var response = new Dictionary<String, String>();
            var orderDetails = ComplianceDataManager.GetExistingOrderList(packageID, currentLoggedInUser, TenantID);
            if (!orderDetails.IsNullOrEmpty())
            {
                //var orderStatus = orderDetails.OrderPaymentDetails.Where(e => e.OrderPkgPaymentDetails.FirstOrDefault().OPPD_BkgOrderPackageID == null).FirstOrDefault().lkpOrderStatu;
                var orderStatus = orderDetails.OrderPaymentDetails.Where(e => e.OrderPkgPaymentDetails.Where(d => d.OPPD_BkgOrderPackageID == null).FirstOrDefault().OPPD_BkgOrderPackageID == null).FirstOrDefault().lkpOrderStatu;
                if (!orderStatus.IsNullOrEmpty())
                {
                    if (orderStatus.Code.Equals(ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue()))
                    {
                        response.Add("IsOrderExisted", AppConsts.TRUE);
                        response.Add("IsSubscriptionRenew", AppConsts.FALSE);
                        response.Add("OrderNumber", orderDetails.OrderNumber);
                    }
                    var subscriptionOptionList = orderDetails.DeptProgramPackage.DeptProgramPackageSubscriptions.ToList();

                    var packageSubscription = ComplianceDataManager.GetPackageSubscriptionDetailByOrderId(TenantID, orderDetails.OrderID);

                    #region Renew Subscription Logic
                    Boolean isRenewSubscription = false;
                    Guid subscriptionOptionCode = new Guid(SubscriptionOptions.CustomMonthly.GetStringValue());

                    if (packageSubscription.Order.DeptProgramPackage.IsNotNull() && packageSubscription.Order.DeptProgramPackage.DeptProgramPackageSubscriptions.IsNotNull())
                    {
                        DeptProgramPackageSubscription depProgramPackageSubscription = packageSubscription.Order.DeptProgramPackage.DeptProgramPackageSubscriptions.Where(cond => cond.SubscriptionOption.Code == subscriptionOptionCode && cond.SubscriptionOption.IsSystem && !cond.DPPS_IsDeleted && (cond.DPPS_TotalPrice != null)).FirstOrDefault();
                        if (depProgramPackageSubscription != null && depProgramPackageSubscription.DPPS_TotalPrice != null &&
                            ((packageSubscription.Order != null && (packageSubscription.Order.ProgramDuration ?? 0) <= 0) || IsSubscriptionRenewalValid(packageSubscription.Order, TenantID)))
                        {
                            isRenewSubscription = true;
                        }
                    }
                    #endregion


                    if (isRenewSubscription && !packageSubscription.IsNullOrEmpty() && packageSubscription.ExpiryDate < DateTime.Now)
                    {
                        response.Add("IsOrderExisted", AppConsts.TRUE);
                        response.Add("IsSubscriptionRenew", AppConsts.TRUE);
                        response.Add("OrderNumber", orderDetails.OrderNumber);
                    }
                    else
                    {
                        response.Add("IsOrderExisted", AppConsts.FALSE);
                        response.Add("IsSubscriptionRenew", AppConsts.FALSE);
                        response.Add("OrderNumber", orderDetails.OrderNumber);
                    }
                }
                else
                {
                    response.Add("IsOrderExisted", AppConsts.FALSE);
                    response.Add("IsSubscriptionRenew", AppConsts.FALSE);
                    response.Add("OrderNumber", AppConsts.ZERO);
                }

            }
            else
            {
                response.Add("IsOrderExisted", AppConsts.FALSE);
                response.Add("IsSubscriptionRenew", AppConsts.FALSE);
                response.Add("OrderNumber", AppConsts.ZERO);
            }
            var res = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            return res;
        }
        public static Boolean IsSubscriptionRenewalValid(Entity.ClientEntity.Order orderDetail, Int32 ApplicantTenantId)
        {
            List<Entity.ClientEntity.Order> orderList = ComplianceDataManager.GetOrderListOfPreviousOrder(ApplicantTenantId, orderDetail);
            if (orderList.Count > 0)
            {
                Int32 totalSubscriptionYear = Convert.ToInt32(orderList.Where(cnd => cnd.SubscriptionYear != null).Sum(x => x.SubscriptionYear));
                Int32 totalSubscriptionMonth = Convert.ToInt32(orderList.Where(cnd => cnd.SubscriptionMonth != null).Sum(x => x.SubscriptionMonth));
                Int32 subscriptionDuration = (totalSubscriptionYear * 12) + totalSubscriptionMonth;
                Int32 remainingProgDuration = Convert.ToInt32(orderDetail.ProgramDuration - subscriptionDuration);
                if (subscriptionDuration <= remainingProgDuration)
                    return true;
            }
            return false;
        }
        #endregion

        //UAT-3068 Add text message option to two factor authentication options. (applicant should be able to choose Google authenticator or to get code in text message)
        public static Boolean SaveAuthenticationData(Int32 CurrentUserId)
        {
            if (CurrentUserId > AppConsts.NONE)
            {
                String userID = SecurityManager.GetUserIdFromOrgUserId(CurrentUserId);
                if (!userID.IsNullOrEmpty())
                {
                    String authenticationType = INTSOF.Utils.AuthenticationMode.None.GetStringValue();
                    return SecurityManager.SaveAuthenticationData(userID, authenticationType, CurrentUserId);
                }
            }
            return false;
        }
        [WebMethod]
        public static Boolean IsShortSignature(String hiddenOutput)
        {

            SetSignatureFrameParameters();
            var signatureBuffer = GetSignatureImageBuffer(hiddenOutput);
            if (signatureBuffer.IsNotNull() && signatureBuffer.Length < Convert.ToInt32(WebConfigurationManager.AppSettings["MinApplicantSignLength"]))
                return true;
            else
                return false;

        }
        public static void SetSignatureFrameParameters()
        {
            PenColor = Color.Black;
            Background = Color.White;
            Height = 150;
            Width = 648;
            PenWidth = 2;
            FontSize = 24;
        }

        public static byte[] GetSignatureImageBuffer(String jsonStr)
        {

            System.Drawing.Bitmap signatureImage = SigJsonToImage(jsonStr);
            // Save out to memory and then to a file.
            MemoryStream mm = new MemoryStream();

            signatureImage.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
            byte[] bufferSignature = mm.GetBuffer();
            //We dispose of all objects to make sure the files don't stay locked.
            signatureImage.Dispose();
            mm.Dispose();
            return bufferSignature;


        }

        public static Bitmap SigJsonToImage(string json)
        {

            Bitmap signatureImage = new Bitmap(Width, Height);
            signatureImage.MakeTransparent();
            using (Graphics signatureGraphic = Graphics.FromImage(signatureImage))
            {
                signatureGraphic.Clear(Background);
                signatureGraphic.SmoothingMode = SmoothingMode.AntiAlias;
                Pen pen = new Pen(PenColor, PenWidth);
                List<SignatureLine> lines = (List<SignatureLine>)JsonConvert.DeserializeObject(json ?? string.Empty, typeof(List<SignatureLine>));
                foreach (SignatureLine line in lines)
                {
                    signatureGraphic.DrawLine(pen, line.lx, line.ly, line.mx, line.my);
                }
            }
            return signatureImage;

        }

        [WebMethod]
        public static Boolean Geolocation(string dataString, string ZipCode)
        {
            HttpContext.Current.Session["ApplicantLngLat"] = dataString.IsNullOrEmpty() ? String.Empty : dataString; ;
            HttpContext.Current.Session["ZipCode"] = ZipCode.IsNullOrEmpty() ? AppConsts.DEFAULT_ZIPCODE_LOCATION : ZipCode;
            return true;
        }
    }
}
