using Business.RepoManagers;
using Entity;
using ExternalVendors;
using INTSOF.ServiceUtil;
using INTSOF.UI.Contract.Services;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.BkgOperations;
using System.Xml.Linq;
using ExternalVendors.ClearStarVendor;
using INTSOF.UI.Contract;

namespace VendorOrderProcessService
{
    public static class CreateBulkOrderService
    {
        #region Private Variables

        private static Int32 _recordChunkSize;
        private static String CreateBulkOrderServiceLogger;
        private static Boolean _isServiceLoggingEnabled;
        private static Guid LCSAttCode = new Guid("1ADA97AE-9100-4BE6-B829-C914B7FA8750");//Driver's License State
        private static Guid LCNAttCode = new Guid("515BEF57-9072-4D2A-A97A-0C248BB045F9");////License Number
        private static Int32 _backgroundProcessUserId = 0;
        private static Int32 _recordRepeatedBulkOrderChunkSize;
        #region UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
        private static String _noMiddleNameText;
        #endregion
        #endregion

        #region Constructor

        static CreateBulkOrderService()
        {
            CreateBulkOrderServiceLogger = "BkgOrderStatusServiceLogger";
            _recordChunkSize = AppConsts.CHUNK_SIZE_FOR_CREATE_BULK_ORDER_SERVICE;
            //UAT-2697
            _recordRepeatedBulkOrderChunkSize = AppConsts.CHUNK_SIZE_FOR_CREATE_BULK_ORDER_SERVICE;

            if (ConfigurationManager.AppSettings["IsServiceLoggingEnabled"].IsNotNull())
            {
                _isServiceLoggingEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["IsServiceLoggingEnabled"]);
            }
            else
            {
                _isServiceLoggingEnabled = false;
            }
            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
            if (ConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY].IsNotNull())
            {
                _noMiddleNameText = ConfigurationManager.AppSettings[AppConsts.NO_MIDDLE_NAME_TEXT_KEY];
            }
            else
            {
                _noMiddleNameText = String.Empty;
            }

            if (ConfigurationManager.AppSettings["CreateRepeatedBulkOrderServiceChunkSize"].IsNotNull())
            {
                _recordRepeatedBulkOrderChunkSize = Convert.ToInt32(ConfigurationManager.AppSettings["CreateRepeatedBulkOrderServiceChunkSize"]);
            }
        }

        #endregion

        #region [Public Methods]
        /// <summary>
        /// UAT-1835, NYU Migration 3 of 3: Automatic Interval Searching
        /// </summary>
        /// <param name="tenant_Id"></param>
        public static void CreateBulkOrder(Int32? tenant_Id = null)
        {
            try
            {
                // ServiceContext.init();
                ServiceLogger.Info("Calling CreateBulkOrder: " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                _backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Entity.Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                ServiceLogger.Info("Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDBConnection.TestConnString(clntDbConf.CDB_ConnectionString, CreateBulkOrderServiceLogger))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        Boolean executeLoop = true;

                        while (executeLoop)
                        {
                            ServiceLogger.Info("Processing the chunk of Create Bulk Order Data: " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                            List<BulkOrderUpload> bulkOrderUploadList = ComplianceDataManager.GetBulkOrderDataForIntervalSearch(tenantId, _recordChunkSize);

                            if (bulkOrderUploadList.IsNotNull() && bulkOrderUploadList.Count > AppConsts.NONE)
                            {
                                ServiceLogger.Info("Start Processing of Create Bulk Order Data : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                                foreach (BulkOrderUpload bulkOrderUpload in bulkOrderUploadList)
                                {
                                    ServiceLogger.Info("Start Processing of Create Bulk Order Data for bulk Order Upload Id " + bulkOrderUpload.BOU_ID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                                    CreateOrder(tenantId, bulkOrderUpload.BOU_OrderID.Value, bulkOrderUpload.BOU_ApplicantID.Value, bulkOrderUpload.BOU_ID);

                                    ServiceLogger.Info("Ending Processing of Create Bulk Order Data for bulk Order Upload Id " + bulkOrderUpload.BOU_ID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                                }

                                ServiceLogger.Info("Ended foreach loop on CreateBulkOrder " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                                executeLoop = true;
                                ServiceContext.ReleaseDBContextItems();
                            }
                            else
                            {
                                executeLoop = false;
                                ServiceContext.ReleaseDBContextItems();
                            }
                        }

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
                            Int32 bkgOrderServiceUserId = appConfig.IsNotNull() ? Convert.ToInt32(appConfig.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.CreateBulkOrder.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = bkgOrderServiceUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }

                    ServiceContext.ReleaseDBContextItems();
                }

                ServiceLogger.Info("Ended foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in CreateBulkOrder method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2}",
                                    ex.Message, ex.InnerException, ex.StackTrace + " current context key : " + ServiceContext.currentThreadContextKeyString), CreateBulkOrderServiceLogger);
            }
        }


        /// <summary>
        /// UAT-2697, New NYU Bulk Upload Feature
        /// </summary>
        /// <param name="tenant_Id"></param>
        public static void CreateBulkOrderForRepeatedSearch(Int32? tenant_Id = null)
        {
            try
            {
                // ServiceContext.init();
                ServiceLogger.Info("Calling CreateBulkOrderForRepeatedSearch: " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.BACKGROUND_PROCESS_USER_ID);
                _backgroundProcessUserId = appConfiguration.IsNotNull() ? Convert.ToInt32(appConfiguration.AC_Value) : AppConsts.BACKGROUND_PROCESS_USER_VALUE;

                List<ClientDBConfiguration> clientDbConfs = SecurityManager.GetClientDBConfiguration().Select(
                 item =>
                 {
                     ClientDBConfiguration config = new ClientDBConfiguration();
                     config.CDB_TenantID = item.CDB_TenantID;
                     config.CDB_ConnectionString = item.CDB_ConnectionString;
                     config.Tenant = new Entity.Tenant();
                     config.Tenant.TenantName = item.Tenant.TenantName; return config;
                 }).ToList();

                if (clientDbConfs != null && clientDbConfs.Count > 0 && tenant_Id != null)
                {
                    clientDbConfs = clientDbConfs.Where(x => x.CDB_TenantID == tenant_Id).ToList();
                }

                ServiceLogger.Info("Started foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                foreach (ClientDBConfiguration clntDbConf in clientDbConfs)
                {
                    if (CheckDBConnection.TestConnString(clntDbConf.CDB_ConnectionString, CreateBulkOrderServiceLogger))
                    {
                        DateTime jobStartTime = DateTime.Now;
                        DateTime jobEndTime;
                        Int32 tenantId = clntDbConf.CDB_TenantID;
                        tenant_Id = tenantId;
                        Boolean executeLoop = true;

                        while (executeLoop)
                        {
                            ServiceLogger.Info("Processing the chunk of Create Bulk Order Data Repeated Search: " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                            List<BulkOrderUpload> bulkOrderUploadList = ComplianceDataManager.GetBulkOrderDataForRepeatedSearchOrder(tenantId, _recordRepeatedBulkOrderChunkSize);

                            if (bulkOrderUploadList.IsNotNull() && bulkOrderUploadList.Count > AppConsts.NONE)
                            {
                                ServiceLogger.Info("Start Processing of Create Bulk Order Data Repeated Search : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                                foreach (BulkOrderUpload bulkOrderUpload in bulkOrderUploadList)
                                {
                                    ServiceLogger.Info("Start Processing of Create Bulk Order Data Repeated Search  for bulk Order Upload Id " + bulkOrderUpload.BOU_ID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                                    List<Int32> createdOrderIDs = ComplianceDataManager.CreateBulkOrderForRepeatedSearch(bulkOrderUpload.BOU_ID.ToString(), _backgroundProcessUserId, tenantId);

                                    ServiceLogger.Info("Start Processing of Update External Service and Vendor for LineItems generated Upload Id " + bulkOrderUpload.BOU_ID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                                    //Update External Service and Vendor for LineItems generated
                                    createdOrderIDs.ForEach(ordId =>
                                    {
                                        StoredProcedureManagers.UpdateExtServiceVendorforLineItems(ordId, tenantId);

                                        SendNotificationForBulkRepeatedOrder(ordId, tenantId, _backgroundProcessUserId);
                                    });
                                    ServiceLogger.Info("END Processing of Update External Service and Vendor for LineItems generated Upload Id " + bulkOrderUpload.BOU_ID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                                    ServiceLogger.Info("Start Processing of Update External Service and Vendor for LineItems generated Upload Id " + bulkOrderUpload.BOU_ID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                                    Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                                    dicParam.Add("TenantId", tenantId);
                                    dicParam.Add("orderIds", String.Join(",", createdOrderIDs.ToList()));
                                    dicParam.Add("CurrentLoggedInUserId", _backgroundProcessUserId);
                                    BackgroundProcessOrderManager.CreateBkgOrderNotification(dicParam);

                                    ServiceLogger.Info("END Processing of Update External Service and Vendor for LineItems generated Upload Id " + bulkOrderUpload.BOU_ID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                                    ServiceLogger.Info("Ending Processing of Create Bulk Order Data Repeated Search for bulk Order Upload Id " + bulkOrderUpload.BOU_ID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                                }

                                ServiceLogger.Info("Ended foreach loop on CreateBulkOrderForRepeatedSearch " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                                executeLoop = true;
                                ServiceContext.ReleaseDBContextItems();
                            }
                            else
                            {
                                executeLoop = false;
                                ServiceContext.ReleaseDBContextItems();
                            }
                        }

                        //Save service logging data to DB
                        if (_isServiceLoggingEnabled)
                        {
                            jobEndTime = DateTime.Now;
                            Entity.AppConfiguration appConfig = SecurityManager.GetAppConfiguration(AppConsts.BKG_ORDER_SERVICE_USER_ID);
                            Int32 bkgOrderServiceUserId = appConfig.IsNotNull() ? Convert.ToInt32(appConfig.AC_Value) : AppConsts.BKG_ORDER_SERVICE_USER_ID_VALUE;
                            ServiceLoggingContract serviceLoggingContract = new ServiceLoggingContract();
                            serviceLoggingContract.ServiceName = ServiceName.BkgOrderService.GetStringValue();
                            serviceLoggingContract.JobName = JobName.CreateBulkOrderForRepeatedSearch.GetStringValue();
                            serviceLoggingContract.TenantID = tenantId;
                            serviceLoggingContract.JobStartTime = jobStartTime;
                            serviceLoggingContract.JobEndTime = jobEndTime;
                            //serviceLoggingContract.Comments = "";
                            serviceLoggingContract.IsDeleted = false;
                            serviceLoggingContract.CreatedBy = bkgOrderServiceUserId;
                            serviceLoggingContract.CreatedOn = DateTime.Now;
                            SecurityManager.SaveServiceLoggingDetail(serviceLoggingContract);
                        }
                    }

                    ServiceContext.ReleaseDBContextItems();
                }

                ServiceLogger.Info("Ended foreach loop on ClientDbConfigurations: " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
            }
            catch (Exception ex)
            {
                ServiceLogger.Error(String.Format("An Error has occured in CreateBulkOrderForRepeatedSearch method, the details of which are: {0}, Inner Exception: {1}, Stack Trace: {2} , current TenantId is: {3}",
                                    ex.Message, ex.InnerException, ex.StackTrace, tenant_Id + " current context key : " + ServiceContext.currentThreadContextKeyString), CreateBulkOrderServiceLogger);
            }
        }
        #endregion

        #region [Private Methods]

        private static void CreateOrder(Int32 tenantId, Int32 refOrderId, Int32 currentOrgUserId, Int32 bulkOrderUploadID)
        {
            int autoRecurringOrderHistoryId = 0;
            int? newOrderId = null;

            try
            {
                //Adding into AutoRecurringOrderHistory
                ServiceLogger.Info("Adding order initialize date into AutoRecurringOrderHistory for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                AutoRecurringOrderHistory autoRecurringOrderHistory = new AutoRecurringOrderHistory();
                autoRecurringOrderHistory.AROH_OrgUserId = currentOrgUserId;
                autoRecurringOrderHistory.AROH_RefOrderId = refOrderId;
                autoRecurringOrderHistory.AROH_OrderInitializeDate = DateTime.Now;
                autoRecurringOrderHistory.AROH_IsDeleted = false;
                autoRecurringOrderHistory.AROH_CreatedBy = _backgroundProcessUserId;
                autoRecurringOrderHistory.AROH_CreatedOn = DateTime.Now;
                autoRecurringOrderHistoryId = BackgroundProcessOrderManager.SaveAutoRecurringOrderHistory(tenantId, autoRecurringOrderHistory);

                ServiceLogger.Info("Added successfully order initialize date into AutoRecurringOrderHistory for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);


                ServiceLogger.Info("Start processing to create order for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                Order currentOrder = ComplianceDataManager.GetOrderById(tenantId, refOrderId);
                BkgOrder bkgOrder = ComplianceDataManager.GetBkgOrderDetailByID(tenantId, refOrderId);


                ServiceLogger.Info("Start filling applicant cart for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                ApplicantOrderCart applicantOrderCart = null;

                if (applicantOrderCart == null)
                {
                    applicantOrderCart = new ApplicantOrderCart();
                    applicantOrderCart.lstApplicantOrder = new List<ApplicantOrder>();
                    applicantOrderCart.lstApplicantOrder.Add(new ApplicantOrder() { });
                }

                applicantOrderCart.IsReadOnly = true;

                if (!currentOrder.IsNullOrEmpty())
                {
                    Boolean isBkgPackageIncluded = false;
                    Boolean ifInvoiceIsOnlyPaymentOptions = false;
                    List<Int32> lstBopIds = new List<int>();
                    isBkgPackageIncluded = true;
                    String compPkgTypeCode = String.Empty;

                    applicantOrderCart.SelectedHierarchyNodeID = currentOrder.SelectedNodeID;

                    if (applicantOrderCart.IsNotNull())
                        applicantOrderCart.CompliancePackages = new Dictionary<string, OrderCartCompliancePackage>();


                    if (isBkgPackageIncluded)
                    {
                        //bkgOrderPkgLst = GetBkgOrderPackageDetail(sentForOnlinePaymentDetailList, tenantId, ref lstBopIds);

                        ServiceLogger.Info("Getting background packages for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                        List<BkgOrderPackage> bkgOrderPkgLst = new List<BkgOrderPackage>();
                        bkgOrderPkgLst = ComplianceDataManager.GetBkgOrderPackagesByBkgOrderId(tenantId, bkgOrder.BOR_ID);

                        lstBopIds = bkgOrderPkgLst.Select(cond => cond.BOP_ID).ToList();

                        ServiceLogger.Info("Adding Background package for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                        AddBackgroundPackageDataToSession(applicantOrderCart, bkgOrderPkgLst);

                        ServiceLogger.Info("Generating customer data for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                        GenerateCustomFormData(applicantOrderCart, currentOrder.OrderID, tenantId, lstBopIds);
                    }
                    else if (!applicantOrderCart.IsNullOrEmpty() && !applicantOrderCart.lstApplicantOrder.IsNullOrEmpty())
                    {
                        applicantOrderCart.lstApplicantOrder[0].lstPackages = null;
                    }

                    applicantOrderCart.IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel = ifInvoiceIsOnlyPaymentOptions;

                    ServiceLogger.Info("Getting latest profile id for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                    SetApplicantProfileDataInSession(applicantOrderCart, currentOrder, tenantId, currentOrgUserId);

                    applicantOrderCart.OrderRequestType = OrderRequestType.NewOrder.GetStringValue();

                    //Pricing
                    if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
                    {
                        //Changed on 12-June-14.
                        ServiceLogger.Info("Getting pricing for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                        String personalDataXML = ConvertApplicantDataIntoXML(applicantOrderCart, tenantId);
                        Boolean _isXMLGenerated;
                        String _pricingInputXML = StoredProcedureManagers.GetPricingDataInputXML(personalDataXML, tenantId, applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData,
                            applicantOrderCart.lstApplicantOrder[0].lstPackages, out _isXMLGenerated);

                        String _pricingDataXML = StoredProcedureManagers.GetPricingData(_pricingInputXML, tenantId);

                        applicantOrderCart.lstApplicantOrder[0].PricingDataXML = _pricingDataXML;

                        if (!String.IsNullOrEmpty(_pricingDataXML))
                        {
                            XDocument doc = XDocument.Parse(_pricingDataXML);
                            var _packages = doc.Root.Descendants("Packages")
                                               .Descendants("Package")
                                               .Select(element => element)
                                               .ToList();

                            ServiceLogger.Info("Processing pricing data for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                            List<Package_PricingData> _lstData = new List<Package_PricingData>();
                            foreach (var pkg in _packages)
                            {
                                #region Update the BkgPackage Price for ALL BkgPackages, in Session, from the Pricing SP calculations

                                Int32 _packageId = Convert.ToInt32(pkg.Element("PackageId").Value);

                                // To be removed
                                Decimal _totalLineItemPrice = pkg.Element("TotalPrice").Value.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToDecimal(pkg.Element("TotalPrice").Value);

                                BackgroundPackagesContract _bkgPackagesContract = applicantOrderCart.lstApplicantOrder[0].lstPackages
                                                                                    .Where(bpkg => bpkg.BPAId == _packageId).FirstOrDefault();

                                if (_bkgPackagesContract.IsNotNull())
                                    _bkgPackagesContract.TotalBkgPackagePrice = _totalLineItemPrice;

                                _lstData.Add(new Package_PricingData
                                {
                                    PackageId = _packageId,
                                    TotalBkgPackagePrice = _totalLineItemPrice
                                });

                                #endregion
                            }
                        }

                        //Save
                        var _lstClientPaymentOptions = ComplianceDataManager.GetClientPaymentOptions(tenantId);
                        int pkgId = 0;

                        if (!bkgOrder.IsNullOrEmpty() && !bkgOrder.BkgOrderPackages.IsNullOrEmpty()
                            && !bkgOrder.BkgOrderPackages.FirstOrDefault().BkgPackageHierarchyMapping.IsNullOrEmpty()
                            && !bkgOrder.BkgOrderPackages.FirstOrDefault().BkgPackageHierarchyMapping.BackgroundPackage.IsNullOrEmpty())
                        {
                            pkgId = bkgOrder.BkgOrderPackages.FirstOrDefault().BkgPackageHierarchyMapping.BackgroundPackage.BPA_ID;
                        }


                        List<ApplicantOrderPaymentOptions> _paymentModesData = new List<ApplicantOrderPaymentOptions>();
                        _paymentModesData.Add(new ApplicantOrderPaymentOptions
                        {
                            isbkg = true,
                            isZP = false,
                            poid = _lstClientPaymentOptions.Where(po => po.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()).First().PaymentOptionID,
                            pkgid = pkgId
                        });

                        ApplicantOrderDataContract _applicantOrderDataContract = new ApplicantOrderDataContract();

                        String _invoiceNumbers = String.Empty;
                        var _dicInvoiceNumber = new Dictionary<String, String>();
                        String _paymentModeCode = String.Empty;
                        Boolean _isUpdateMainProfile = false;
                        Int32 _prgPackageSubscriptionId = 0;
                        String _errorMessage = String.Empty;

                        Entity.ClientEntity.OrganizationUserProfile _orgUserProfile = new Entity.ClientEntity.OrganizationUserProfile();

                        foreach (var applicantOrder in applicantOrderCart.lstApplicantOrder)
                        {
                            _orgUserProfile = applicantOrder.OrganizationUserProfile;
                            _isUpdateMainProfile = applicantOrder.UpdatePersonalDetails;
                        }

                        Int32 organizationUserID = currentOrgUserId;

                        applicantOrderCart.IsBiilingInfoSameAsAccountInfo = true;

                        #region Read Data from Pricing XML

                        List<Package_PricingData> _lstPricingData = null;
                        if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
                        {
                            _pricingDataXML = applicantOrderCart.lstApplicantOrder[0].PricingDataXML;
                            if (!String.IsNullOrEmpty(_pricingDataXML))
                            {
                                _lstPricingData = new List<Package_PricingData>();
                                _lstPricingData = GenerateDataFromPricingXML(_pricingDataXML);
                            }
                        }

                        #endregion

                        Order _order = new Order();

                        #region Set Order object, based on OrderRequestType
                        _order.OrderMachineIP = null; //applicantOrderCart.lstApplicantOrder[0].ClientMachineIP;
                        _order.TotalPrice = 0;
                        _order.ProgramDuration = null;
                        _order.HierarchyNodeID = GetHierarchyNodeIDByPackageType(tenantId, applicantOrderCart);
                        _order.OriginalSettlementPrice = Convert.ToDecimal(applicantOrderCart.SettleAmount);

                        _order.SelectedNodeID = applicantOrderCart.SelectedHierarchyNodeID;

                        if (applicantOrderCart.OrderRequestType != null)
                            _order.OrderRequestTypeID = ComplianceDataManager.GetLKPOrderRequestType(applicantOrderCart.OrderRequestType, tenantId).ORT_ID;

                        // No need to subtract the Settlement Amount from the Grand Total, as it was already done, 
                        // before it was added to the session, from the PendingOrder.cs (AddCompliancePackageDataToSession() function)
                        _order.GrandTotal = Convert.ToDecimal(applicantOrderCart.GrandTotal) + GetBackgroundPackagesPrice(applicantOrderCart);

                        ServiceLogger.Info("Generating grouped amount for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                        GenerateGroupedAmount(applicantOrderCart, _paymentModesData, tenantId, _applicantOrderDataContract, _lstClientPaymentOptions);

                        _applicantOrderDataContract.lstOrderPackageTypes = LookupManager.GetLookUpData<lkpOrderPackageType>(tenantId);
                        _applicantOrderDataContract.lstOrderStatus = LookupManager.GetLookUpData<lkpOrderStatu>(tenantId);


                        #endregion

                        if (applicantOrderCart.lstApplicantOrder.IsNotNull() && applicantOrderCart.lstApplicantOrder[0].OrderId == AppConsts.NONE)
                        {
                            Int32 _newOrderId = 0;
                            _order.CreatedByID = organizationUserID;

                            #region Set OrderPackageType, based on the Packages selected

                            _order.OrderPackageType = GetOrderPackageType(tenantId, applicantOrderCart);

                            #endregion

                            #region Set Data in Contract class & Save in database

                            _orgUserProfile.OrganizationUserID = currentOrgUserId;
                            List<TypeCustomAttributes> lst = applicantOrderCart.GetCustomAttributeValues();

                            //Set Is Complance Package Selected
                            _applicantOrderDataContract.IsCompliancePackageSelected = applicantOrderCart.IsCompliancePackageSelected;
                            _applicantOrderDataContract.OrganizationUserProfile = _orgUserProfile;
                            _applicantOrderDataContract.ProgramPackageSubscriptionId = _prgPackageSubscriptionId;
                            _applicantOrderDataContract.lstGroupedData = applicantOrderCart.lstPaymentGrouping;
                            _applicantOrderDataContract.TenantId = tenantId;
                            _applicantOrderDataContract.lstAttributeValues = lst;
                            _applicantOrderDataContract.LastNodeDPMId = Convert.ToInt32(applicantOrderCart.SelectedHierarchyNodeID);
                            _applicantOrderDataContract.lstBackgroundPackages = applicantOrderCart.lstApplicantOrder[0].lstPackages;
                            _applicantOrderDataContract.lstPricingData = _lstPricingData;
                            if (bkgOrder.IsNotNull())
                            {
                                _applicantOrderDataContract.IsSendBackgroundReport = bkgOrder.BOR_OrderResultsRequestedByApplicant.IsNotNull() ? bkgOrder.BOR_OrderResultsRequestedByApplicant.Value : false;//applicantOrderCart.lstApplicantOrder[0].IsSendBackgroundReport;
                            }
                            // SelectedPaymentModeId = paymentModeId, UAT 916

                            //UAT 1438: Enhancement to allow students to select a User Group.
                            _applicantOrderDataContract.IsUserGroupCustomAttributeExist = applicantOrderCart.IsUserGroupCustomAttributeExist;
                            if (applicantOrderCart.IsUserGroupCustomAttributeExist)
                            {
                                _applicantOrderDataContract.lstAttributeValuesForUserGroup = ComplianceDataManager.AddCustomAttributeValuesForUserGroup(applicantOrderCart.lstCustomAttributeUserGroupIDs, currentOrgUserId, organizationUserID); //applicantOrderCart.GetCustomAttributeValuesForUserGroup();
                            }


                            if (!applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.IsNullOrEmpty())
                                _applicantOrderDataContract.lstBkgOrderData = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData;



                            //UAT-3283 //  _order.PackageBundleId = applicantOrderCart.SelectedPkgBundleId.HasValue ? applicantOrderCart.SelectedPkgBundleId.Value : (int?)null;

                            // UAT 916

                            ServiceLogger.Info("Submitting new order for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                            _dicInvoiceNumber = ComplianceDataManager.SubmitApplicantOrder(_order, _applicantOrderDataContract, _isUpdateMainProfile,
                                applicantOrderCart.lstPrevAddresses, applicantOrderCart.lstPersonAlias, out _paymentModeCode, out _newOrderId, organizationUserID, null, null, applicantOrderCart.MailingAddress, applicantOrderCart.FingerPrintData);

                            _invoiceNumbers = GetInvoiceNumbers(_dicInvoiceNumber);

                            #endregion

                            #region Call Method to update EDS status and EDS custom Form Data

                            //This method is called to handle the scenario where amount is zero and order is approved with zero amount automatically.

                            ServiceLogger.Info("Updating EDSStatus for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                            UpdateEDSStatus(_applicantOrderDataContract, _newOrderId, _order);

                            ComplianceDataManager.InsertAutomaticInvitationLog(_applicantOrderDataContract.TenantId, _newOrderId, AppConsts.NONE); //UAT-2388

                            //Send Notification for print scan
                            //UAT-1358:Complio Notification to applicant for PrintScan
                            ServiceLogger.Info("Sending Print Scan Notification  for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                            SendPrintScanNotification(_newOrderId, _order, null, false, _applicantOrderDataContract.TenantId);


                            //Add Disclosure & Other documents for order
                            ServiceLogger.Info("Mapping prev order documents with new order for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                            List<Entity.ClientEntity.ApplicantDocument> lstDnRDocuments = BackgroundProcessOrderManager.GetOrderAndBackgroundProfileRelatedDocuments(tenantId, refOrderId);
                            if (!lstDnRDocuments.IsNullOrEmpty() && lstDnRDocuments.Count > 0)
                            {
                                List<ApplicantDocument> lstAppDocuments = new List<ApplicantDocument>();

                                String backgroundProfileRecordTypeCode = RecordType.BackgroundProfile.GetStringValue();
                                String orderRecordTypeCode = RecordType.Order.GetStringValue();

                                Int32 backgroundProfileRecordTypeId = BackgroundProcessOrderManager.GetlkpRecordTypeIdByCode(tenantId, backgroundProfileRecordTypeCode);
                                Int32 orderRecordTypeId = BackgroundProcessOrderManager.GetlkpRecordTypeIdByCode(tenantId, orderRecordTypeCode);

                                foreach (ApplicantDocument item in lstDnRDocuments)
                                {
                                    short itemRecordTypeId = item.GenericDocumentMappings.FirstOrDefault().GDM_RecordTypeID;
                                    bool isMappingDeleted = false;

                                    if (!item.GenericDocumentMappings.IsNullOrEmpty() && !item.GenericDocumentMappings.FirstOrDefault().IsNullOrEmpty())
                                    {
                                        isMappingDeleted = item.GenericDocumentMappings.FirstOrDefault().GDM_IsDeleted;
                                    }

                                    ApplicantDocument applicantDocument = new ApplicantDocument();
                                    applicantDocument.FileName = item.FileName;
                                    applicantDocument.OrganizationUserID = currentOrgUserId;
                                    applicantDocument.Description = item.Description;
                                    applicantDocument.IsDeleted = item.IsDeleted;
                                    applicantDocument.CreatedByID = currentOrgUserId;
                                    applicantDocument.CreatedOn = DateTime.Now;
                                    applicantDocument.DocumentType = item.DocumentType;
                                    applicantDocument.DocumentPath = item.DocumentPath;
                                    applicantDocument.Size = item.Size;

                                    GenericDocumentMapping genericDocumentMapping = new GenericDocumentMapping();
                                    genericDocumentMapping.GDM_RecordID = itemRecordTypeId == backgroundProfileRecordTypeId ? applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.OrganizationUserProfileID : _newOrderId;
                                    genericDocumentMapping.GDM_RecordTypeID = itemRecordTypeId;
                                    genericDocumentMapping.GDM_IsDeleted = isMappingDeleted;
                                    genericDocumentMapping.GDM_CreatedBy = currentOrgUserId;
                                    genericDocumentMapping.GDM_CreatedOn = DateTime.Now;
                                    applicantDocument.GenericDocumentMappings.Add(genericDocumentMapping);

                                    lstAppDocuments.Add(applicantDocument);
                                }

                                ApplicantRequirementManager.SaveApplicantUploadDocument(lstAppDocuments, tenantId);
                            }

                            #endregion

                            //Will have to change if multiple orders at a time
                            newOrderId = _newOrderId;
                            applicantOrderCart.lstApplicantOrder[0].OrderId = _newOrderId;
                            applicantOrderCart.lstApplicantOrder[0].OrderNumber = _order.OrderNumber;
                        }

                        applicantOrderCart.InvoiceNumber = _dicInvoiceNumber;
                        applicantOrderCart.IncrementOrderStepCount();
                        applicantOrderCart.AddOrderStageTrackID(OrderStages.OrderPayment);

                        ServiceLogger.Info("Sending notifications for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                        #region Notification Logic for Email & Message

                        StringBuilder _sbNotificationMsg = new StringBuilder();

                        // UAT 916 -- _paymentModeCode
                        foreach (String paymentModeCode in _dicInvoiceNumber.Keys)
                        {
                            OrderPaymentDetail opd = _order.OrderPaymentDetails.Where(cond => cond.lkpPaymentOption.Code.Equals(paymentModeCode) && !cond.OPD_IsDeleted).FirstOrDefault();
                            if (opd.IsNotNull())
                            {
                                SendNotifications(paymentModeCode, tenantId, _errorMessage, _orgUserProfile, _order);
                                _sbNotificationMsg.Append("OrderId: " + _order.OrderID);
                            }
                            //UAT-1185 Sending multiple order notification
                            List<Int32> orderIds = ComplianceDataManager.GetOrderAndTenantID(_dicInvoiceNumber[paymentModeCode])["OrderID"];

                            if (orderIds.IsNotNull())
                            {
                                foreach (Int32 orderId_ in orderIds)
                                {
                                    if (!_order.OrderID.Equals(orderId_))
                                    {

                                        Order extraOrder = _order.OrderGroupOrderNavProp.FirstOrDefault(o => o.OrderID.Equals(orderId_));
                                        if (extraOrder.IsNotNull() && extraOrder.OrderPaymentDetails.IsNotNull())
                                        {
                                            opd = extraOrder.OrderPaymentDetails.Where(cond => cond.lkpPaymentOption.Code.Equals(paymentModeCode) && !cond.OPD_IsDeleted).FirstOrDefault();
                                            if (opd.IsNotNull())
                                            {
                                                SendNotifications(paymentModeCode, tenantId, _errorMessage, _orgUserProfile, extraOrder);
                                                _sbNotificationMsg.Append(", Extra OrderId: " + orderId_);
                                            }
                                        }
                                    }
                                }
                            }
                            _sbNotificationMsg.Append(", ");
                        }

                        #endregion

                        //Update lastorderPlaced Date in BulkOrderUpload table in case of bulk order
                        ServiceLogger.Info("Updating last order placed date for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                        BackgroundSetupManager.UpdateLastOrderPlacedDate(tenantId, bulkOrderUploadID, currentOrgUserId);

                        ServiceLogger.Info("Updating Reference Order Id for " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                        BackgroundProcessOrderManager.UpdateOrderIntervalSearchRefOrderId(tenantId, applicantOrderCart.lstApplicantOrder[0].OrderId, refOrderId, _backgroundProcessUserId);
                        ServiceLogger.Info("Updated sucessfully reference order id for " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                        //Updating OrderCompletionDate into AutoRecurringOrderHistory
                        ServiceLogger.Info("Updating OrderCompletionDate into AutoRecurringOrderHistory for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                        BackgroundProcessOrderManager.UpdateAutoRecurringOrderHistory(tenantId, autoRecurringOrderHistoryId, DateTime.Now, newOrderId, "Order Placed Successfully!", _backgroundProcessUserId);
                        ServiceLogger.Info("Updated OrderCompletionDate into AutoRecurringOrderHistory for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                        ServiceLogger.Info("Order created sucessfully for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceLogger.Info("Updating last order placed date in CATCH BLOCK for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                BackgroundSetupManager.UpdateLastOrderPlacedDate(tenantId, bulkOrderUploadID, currentOrgUserId);
                ServiceLogger.Info("Updated last order placed date in CATCH BLOCK for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                //Updating exception into AutoRecurringOrderHistory
                ServiceLogger.Info("Updating exception into AutoRecurringOrderHistory in CATCH BLOCK for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);
                BackgroundProcessOrderManager.UpdateAutoRecurringOrderHistory(tenantId, autoRecurringOrderHistoryId, null, newOrderId, string.Concat(ex.Message + Environment.NewLine + ex.StackTrace), _backgroundProcessUserId);
                ServiceLogger.Info("Updated exception into AutoRecurringOrderHistory in CATCH BLOCK for bulk Order Upload Id " + bulkOrderUploadID.ToString() + " : " + DateTime.Now.ToString(), CreateBulkOrderServiceLogger);

                throw;
            }
        }

        private static void SetApplicantProfileDataInSession(ApplicantOrderCart applicantOrderCart, Order currentOrder, Int32 tenantId, Int32 currentOrgUserId)
        {
            //Fetch latest OrganizationUserProfile.

            Entity.ClientEntity.OrganizationUserProfile latestOrganizationUserProfile = ComplianceDataManager.GetOrganizationUserProfileByID(tenantId, currentOrgUserId);

            Entity.ClientEntity.OrganizationUserProfile organizationUserProfile = new Entity.ClientEntity.OrganizationUserProfile();
            organizationUserProfile.FirstName = latestOrganizationUserProfile.FirstName;
            organizationUserProfile.LastName = latestOrganizationUserProfile.LastName;
            organizationUserProfile.MiddleName = latestOrganizationUserProfile.MiddleName;
            organizationUserProfile.Gender = latestOrganizationUserProfile.Gender;
            organizationUserProfile.DOB = latestOrganizationUserProfile.DOB;
            //organizationUserProfile.PrimaryEmailAddress = txtPrimaryEmail.Text;
            //
            organizationUserProfile.PrimaryEmailAddress = latestOrganizationUserProfile.PrimaryEmailAddress;
            organizationUserProfile.SecondaryEmailAddress = latestOrganizationUserProfile.SecondaryEmailAddress;
            organizationUserProfile.SecondaryPhone = latestOrganizationUserProfile.SecondaryPhone;
            organizationUserProfile.SSN = GetDecryptedSSN(latestOrganizationUserProfile.OrganizationUserProfileID, tenantId);
            organizationUserProfile.PhoneNumber = latestOrganizationUserProfile.PhoneNumber;
            organizationUserProfile.OrganizationUserID = latestOrganizationUserProfile.OrganizationUserID;
            organizationUserProfile.AddressHandleID = latestOrganizationUserProfile.AddressHandleID;
            organizationUserProfile.IsActive = true;
            //UAT-2447
            organizationUserProfile.IsInternationalPhoneNumber = latestOrganizationUserProfile.IsInternationalPhoneNumber;
            organizationUserProfile.IsInternationalSecondaryPhone = latestOrganizationUserProfile.IsInternationalSecondaryPhone;

            organizationUserProfile.AddressHandle = new Entity.ClientEntity.AddressHandle
            {
                AddressHandleID = latestOrganizationUserProfile.AddressHandle.AddressHandleID
            };

            var Addresses = latestOrganizationUserProfile.AddressHandle.Addresses.FirstOrDefault();
            organizationUserProfile.AddressHandle.Addresses = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<Entity.ClientEntity.Address>();
            organizationUserProfile.AddressHandle.Addresses.Add(new Entity.ClientEntity.Address
            {
                AddressHandleID = latestOrganizationUserProfile.AddressHandle.AddressHandleID,
                Address1 = Addresses.Address1,
                Address2 = Addresses.Address2,
                ZipCodeID = Addresses.ZipCodeID
            });

            if (applicantOrderCart != null)
            {
                String clientMachineIP = currentOrder.OrderMachineIP;
                Boolean isUserGroupCustomAttributeExist = false;
                isUserGroupCustomAttributeExist = IsUserGroupCustomAttributeExist(CustomAttributeUseTypeContext.Hierarchy.GetStringValue(), currentOrder.SelectedNodeID.Value, tenantId, currentOrgUserId);
                applicantOrderCart.AddOrganizationUserProfile(organizationUserProfile, false, clientMachineIP);

                applicantOrderCart.IsUserGroupCustomAttributeExist = isUserGroupCustomAttributeExist;
                if (isUserGroupCustomAttributeExist)
                {
                    //TPDO:25/01/2016
                    applicantOrderCart.lstCustomAttributeUserGroupIDs = new List<Int32>();
                    //applicantOrderCart.lstCustomAttributeUserGroupIDs = (caOtherDetails).GetUserGroupCustomAttributeValues();
                }

                //TPDO:25/01/2016
                //applicantOrderCart.lstApplicantOrder[0].IsSendBackgroundReport = chkSendBkgReport.Checked;
                applicantOrderCart.lstApplicantOrder[0].MVRIsValidDriverLicenseAndState = false;

                #region UAT-1578 : Addition of SMS notification
                applicantOrderCart.lstApplicantOrder[0].IsReceiveTextNotification = false;
                applicantOrderCart.lstApplicantOrder[0].PhoneNumber = String.Empty;
                #endregion

                List<Entity.ResidentialHistory> lstResidentialHistory = SecurityManager.GetUserResidentialHistories(currentOrgUserId).ToList();
                Entity.ResidentialHistory currentAddressDB = lstResidentialHistory.FirstOrDefault(cnd => cnd.RHI_IsCurrentAddress.HasValue == true && cnd.RHI_IsCurrentAddress.Value == true);
                var addressLookup = GetAddressLookupByHandlerId(Convert.ToString(currentAddressDB.Address.AddressHandleID), tenantId);

                PreviousAddressContract currentAddress = new PreviousAddressContract();
                currentAddress.Address1 = currentAddressDB.Address.Address1;
                currentAddress.Address2 = currentAddressDB.Address.Address2;
                currentAddress.ZipCodeID = currentAddressDB.Address.ZipCodeID;
                currentAddress.Zipcode = addressLookup.ZipCode;
                currentAddress.CityName = addressLookup.CityName;
                currentAddress.StateName = addressLookup.FullStateName;
                currentAddress.Country = addressLookup.CountryName;
                //currentAddress.CountryId = addressLookup.CountryId;
                if (currentAddressDB.Address.ZipCodeID > 0)
                {
                    currentAddress.CountyName = addressLookup.CountyName;
                }
                currentAddress.ResidenceStartDate = currentAddressDB.RHI_ResidenceStartDate;
                currentAddress.ResidenceEndDate = currentAddressDB.RHI_ResidenceEndDate;
                currentAddress.isCurrent = currentAddressDB.RHI_IsCurrentAddress.HasValue && currentAddressDB.RHI_IsCurrentAddress.Value == true ? true : false;
                currentAddress.ResHistorySeqOrdID = currentAddressDB.RHI_SequenceOrder.IsNullOrEmpty() ? AppConsts.ONE : currentAddressDB.RHI_SequenceOrder.Value;

                applicantOrderCart.lstPrevAddresses = new List<PreviousAddressContract>();
                applicantOrderCart.lstPrevAddresses.Add(currentAddress);


                Int32 resHisSequenceNo = AppConsts.ONE;
                foreach (var addressHist in lstResidentialHistory.Where(add => (add.RHI_IsCurrentAddress.HasValue && add.RHI_IsCurrentAddress.Value == false) || add.RHI_IsCurrentAddress.IsNull()).ToList())
                {
                    resHisSequenceNo += AppConsts.ONE;
                    var prevAddressLookup = GetAddressLookupByHandlerId(Convert.ToString(addressHist.Address.AddressHandleID), tenantId);

                    PreviousAddressContract prevAddress = new PreviousAddressContract();
                    prevAddress.Address1 = addressHist.Address.Address1;
                    prevAddress.Address2 = addressHist.Address.Address2;
                    prevAddress.ZipCodeID = addressHist.Address.ZipCodeID;
                    prevAddress.Zipcode = prevAddressLookup.ZipCode;
                    prevAddress.CityName = prevAddressLookup.CityName;
                    prevAddress.StateName = prevAddressLookup.FullStateName;
                    prevAddress.Country = prevAddressLookup.CountryName;
                    //prevAddress.CountryId = prevAddressExt.AE_CountryID;
                    if (addressHist.Address.ZipCodeID > 0)
                    {
                        prevAddress.CountyName = prevAddressLookup.CountyName;
                    }
                    prevAddress.ResidenceStartDate = addressHist.RHI_ResidenceStartDate;
                    prevAddress.ResidenceEndDate = addressHist.RHI_ResidenceEndDate;
                    prevAddress.isCurrent = addressHist.RHI_IsCurrentAddress.HasValue && addressHist.RHI_IsCurrentAddress.Value == true ? true : false;
                    prevAddress.ResHistorySeqOrdID = addressHist.RHI_SequenceOrder.IsNullOrEmpty() ? resHisSequenceNo : addressHist.RHI_SequenceOrder.Value;
                    //applicantOrderCart.lstPrevAddresses = new List<PreviousAddressContract>();
                    applicantOrderCart.lstPrevAddresses.Add(prevAddress);
                    applicantOrderCart.IsResidentialHistoryVisible = true;
                }

                List<Entity.ClientEntity.PersonAliasProfile> lstPersonAlias = latestOrganizationUserProfile.PersonAliasProfiles.Where(alias => !alias.PAP_IsDeleted).ToList();

                applicantOrderCart.lstPersonAlias = new List<PersonAliasContract>();

                Int32 sequenceNo = AppConsts.NONE;

                lstPersonAlias.ForEach(alias =>
                {
                    sequenceNo += 1;
                    PersonAliasContract personAlias = new PersonAliasContract();
                    personAlias.FirstName = alias.PAP_FirstName;
                    personAlias.LastName = alias.PAP_LastName;
                    personAlias.ID = alias.PAP_ID;
                    personAlias.AliasSequenceId = sequenceNo;
                    //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                    personAlias.MiddleName = alias.PAP_MiddleName.IsNullOrEmpty() ? _noMiddleNameText : alias.PAP_MiddleName;

                    applicantOrderCart.lstPersonAlias.Add(personAlias);
                });

                applicantOrderCart.IsAccountUpdated = false;
            }
        }

        private static void AddBackgroundPackageDataToSession(ApplicantOrderCart applicantOrderCart, List<BkgOrderPackage> bkgOrderPkgLst)
        {
            List<BackgroundPackagesContract> _lstBackgroundPackages = new List<BackgroundPackagesContract>();

            bkgOrderPkgLst.ForEach(bop =>
            {
                //#region UAT-1867: Added this check to reolve issue Price was not dispaying in completeOrder process for bkgPackage, Added this check temporarily Need to verify again
                //List<Entity.ClientEntity.lkpPaymentOption> lstPaymentOptions = new List<Entity.ClientEntity.lkpPaymentOption>();
                //Boolean? IsInvoiceOnlyAtPackageLevel = null;
                //List<BkgPackagePaymentOption> lstBkgPackagePaymentOptions = bop.BkgPackageHierarchyMapping.BkgPackagePaymentOptions.Where(cond => !cond.BPPO_IsDeleted).ToList();
                //if (lstBkgPackagePaymentOptions.IsNotNull() && lstBkgPackagePaymentOptions.Count > 0)
                //{
                //    lstPaymentOptions = lstBkgPackagePaymentOptions.Select(col => col.lkpPaymentOption).ToList();
                //}
                //if (lstPaymentOptions.Count == 0)
                //{
                //    IsInvoiceOnlyAtPackageLevel = null;
                //}
                //else if (lstPaymentOptions.Count == 1)
                //{
                //    IsInvoiceOnlyAtPackageLevel = lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue() || t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
                //}
                //else if (lstPaymentOptions.Count == 2)
                //{
                //    IsInvoiceOnlyAtPackageLevel = lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()) && lstPaymentOptions.Any(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
                //}
                //else
                //{
                //    IsInvoiceOnlyAtPackageLevel = false;
                //}
                //#endregion

                _lstBackgroundPackages.Add(new BackgroundPackagesContract
                {
                    BPAId = bop.BkgPackageHierarchyMapping.BackgroundPackage.BPA_ID,
                    BPAName = String.IsNullOrEmpty(bop.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Label) ?
                                    bop.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Name : bop.BkgPackageHierarchyMapping.BackgroundPackage.BPA_Label,
                    IsExclusive = bop.BkgPackageHierarchyMapping.BPHM_IsExclusive,
                    BPHMId = bop.BkgPackageHierarchyMapping.BPHM_ID,
                    BasePrice = (bop.BOP_BasePrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_BasePrice.Value),
                    //+ (bop.BOP_TotalLineItemPrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_TotalLineItemPrice.Value) 
                    //TotalBkgPackagePrice = (bop.BOP_BasePrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_BasePrice.Value)
                    //                        + (bop.BOP_TotalLineItemPrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_TotalLineItemPrice.Value),
                    //TotalLineItemPrice = bop.BOP_TotalLineItemPrice.IsNullOrEmpty() ? AppConsts.NONE : bop.BOP_TotalLineItemPrice.Value,
                    IsInvoiceOnlyAtPackageLevel = false // IsInvoiceOnlyAtPackageLevel
                });
            });
            applicantOrderCart.lstApplicantOrder[0].lstPackages = _lstBackgroundPackages;
        }

        private static void GenerateCustomFormData(ApplicantOrderCart applicantOrderCart, Int32 orderId, Int32 tenantId, List<Int32> lstBopIds)
        {
            List<Int32> lstGroupIds = new List<Int32>();
            List<Int32> lstInstanceIds = null;
            Dictionary<Int32, String> customFormData = null;
            List<BkgOrderDetailCustomFormUserData> lstRefinedData = null;
            List<BackgroundOrderData> lstBackGroundOrderData = new List<BackgroundOrderData>();
            List<AttributeFieldsOfSelectedPackages> lstAttrMVRGrp = new List<AttributeFieldsOfSelectedPackages>();

            //Get MVR related Attributes.
            lstAttrMVRGrp = GetAttributeFieldsOfSelectedPackages(GetPackageIdString(applicantOrderCart), tenantId);
            Boolean isIncludeMvrData = false;
            if (!lstAttrMVRGrp.IsNullOrEmpty() && lstAttrMVRGrp.Select(x => x.AttributeGrpId).FirstOrDefault() > 0)
            {
                isIncludeMvrData = true;
            }
            List<BkgOrderDetailCustomFormUserData> lstDataForCustomForm = GetAttributesCustomFormIdOrderId(orderId, isIncludeMvrData, lstBopIds, tenantId);
            List<Int32> lstCustomFormIds = lstDataForCustomForm.Where(cmd => cmd.CustomFormID != AppConsts.NONE).DistinctBy(x => x.CustomFormID).Select(x => x.CustomFormID).ToList();
            //Int32 mvrBkgSvcAttributeGroupId = Convert.ToInt32(lstDataForCustomForm.Where(x=>x.)).FirstOrDefault());
            //applicantOrderCart.lstApplicantOrder[0].MVRIsValidDriverLicenseAndState = true;
            if (!lstCustomFormIds.IsNullOrEmpty())
            {
                foreach (Int32 customFormID in lstCustomFormIds)
                {
                    List<BkgOrderDetailCustomFormUserData> lstDistinctCustomFormData = new List<BkgOrderDetailCustomFormUserData>();
                    lstDistinctCustomFormData = lstDataForCustomForm.Where(cst => cst.CustomFormID == customFormID).ToList();

                    //Removing MVR Attribute groups
                    lstDistinctCustomFormData.RemoveAll(cond => lstAttrMVRGrp.Any(n => n.AttributeGrpId == cond.AttributeGroupID));

                    lstGroupIds = lstDistinctCustomFormData.DistinctBy(x => x.AttributeGroupID).Select(x => x.AttributeGroupID).ToList();
                    for (Int32 grpId = 0; grpId < lstGroupIds.Count; grpId++)
                    {
                        lstInstanceIds = lstDistinctCustomFormData.Where(cond => cond.AttributeGroupID == lstGroupIds[grpId]).DistinctBy(x => x.InstanceID).Select(x => x.InstanceID).ToList();
                        for (Int32 instId = 0; instId < lstInstanceIds.Count; instId++)
                        {
                            customFormData = new Dictionary<Int32, String>();
                            BackgroundOrderData backgroundOrderData = new BackgroundOrderData();
                            lstRefinedData = lstDistinctCustomFormData.Where(cond => cond.InstanceID == lstInstanceIds[instId] && cond.AttributeGroupID == lstGroupIds[grpId]).ToList();

                            backgroundOrderData.InstanceId = lstInstanceIds[instId];
                            backgroundOrderData.BkgSvcAttributeGroupId = lstGroupIds[grpId];
                            backgroundOrderData.CustomFormId = customFormID;
                            foreach (var element in lstRefinedData)
                            {
                                if (!customFormData.ContainsKey(element.AttributeGroupMappingID))
                                    customFormData.Add(element.AttributeGroupMappingID, element.Value);
                            }
                            backgroundOrderData.CustomFormData = customFormData;
                            lstBackGroundOrderData.Add(backgroundOrderData);
                        }
                    }
                }

                #region Mvr Field Set in The Session

                if (isIncludeMvrData)
                {
                    Int32 mvrBkgSvcAttributeGroupId = Convert.ToInt32(lstAttrMVRGrp.Select(x => x.AttributeGrpId).FirstOrDefault());
                    applicantOrderCart.lstApplicantOrder[0].MVRIsValidDriverLicenseAndState = true;

                    BackgroundOrderData backgroundOrderDataMVR = new BackgroundOrderData();
                    backgroundOrderDataMVR.InstanceId = AppConsts.ONE;
                    //backgroundOrderDataMVR.CustomFormId = (_presenter.GetCustomFormIDBYCode() > 0) ? _presenter.GetCustomFormIDBYCode() : AppConsts.NONE;
                    backgroundOrderDataMVR.CustomFormId = AppConsts.ONE;
                    backgroundOrderDataMVR.BkgSvcAttributeGroupId = Convert.ToInt32(lstAttrMVRGrp.Select(x => x.AttributeGrpId).FirstOrDefault());
                    backgroundOrderDataMVR.CustomFormData = new Dictionary<Int32, String>();
                    Int32 mappingID = 0;
                    mappingID = lstAttrMVRGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCNAttCode)).AttributeGrpMapingID;
                    var MVRDataLiscenceNumber = lstDataForCustomForm.FirstOrDefault(cmd => cmd.CustomFormID == AppConsts.ONE && cmd.AttributeGroupMappingID == mappingID);
                    backgroundOrderDataMVR.CustomFormData.Add(mappingID, MVRDataLiscenceNumber.IsNullOrEmpty() ? String.Empty : MVRDataLiscenceNumber.Value);
                    applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberID = mappingID;
                    mappingID = lstAttrMVRGrp.FirstOrDefault(x => x.BSA_Code == Convert.ToString(LCSAttCode)).AttributeGrpMapingID;

                    var MVRDataState = lstDataForCustomForm.FirstOrDefault(cmd => cmd.CustomFormID == AppConsts.ONE && cmd.AttributeGroupMappingID == mappingID);
                    backgroundOrderDataMVR.CustomFormData.Add(mappingID, MVRDataState.IsNullOrEmpty() ? String.Empty : MVRDataState.Value);
                    applicantOrderCart.lstApplicantOrder[0].MVRDvrLicenseNumberStateID = mappingID;

                    lstBackGroundOrderData.Insert(AppConsts.NONE, backgroundOrderDataMVR);
                }
                #endregion
                applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData = lstBackGroundOrderData;
            }
        }

        private static List<AttributeFieldsOfSelectedPackages> GetAttributeFieldsOfSelectedPackages(String packageIds, Int32 tenantId)
        {
            List<Entity.ClientEntity.AttributeFieldsOfSelectedPackages> lstAttributeFields = BackgroundProcessOrderManager.GetAttributeFieldsOfSelectedPackages(packageIds, tenantId);
            if (!lstAttributeFields.IsNullOrEmpty())
            {
                return lstAttributeFields.Where(cond => (cond.BSA_Code.ToUpper().Equals("1ADA97AE-9100-4BE6-B829-C914B7FA8750")
                                                                        || cond.BSA_Code.ToUpper().Equals("515BEF57-9072-4D2A-A97A-0C248BB045F9"))
                                                                       && cond.AttributeGrpCode.ToUpper().Equals("CF76960D-2120-46FE-9E03-01C218F8A336")).ToList();
            }
            else
            {
                return new List<AttributeFieldsOfSelectedPackages>();
            }
        }

        private static string GetPackageIdString(ApplicantOrderCart applicantOrderCart)
        {
            String packages = String.Empty;
            if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                applicantOrderCart.lstApplicantOrder[0].lstPackages.ForEach(x => packages += Convert.ToString(x.BPAId) + ",");
                //packages = "4";
                if (packages.EndsWith(","))
                    packages = packages.Substring(0, packages.Length - 1);
            }
            return packages;
        }

        private static List<BkgOrderDetailCustomFormUserData> GetAttributesCustomFormIdOrderId(Int32 masterOrderId, Boolean isIncludeMvrData, List<Int32> lstBopIds, Int32 tenantId)
        {
            String bopIds = String.Join(",", lstBopIds);

            BkgOrderDetailCustomFormDataContract bkgOrderDetailCustomFormDataContract = BackgroundProcessOrderManager.GetBkgORDCustomFormAttrDataForCompletingOrder
                                                                                        (tenantId, masterOrderId, bopIds, isIncludeMvrData);
            if (bkgOrderDetailCustomFormDataContract.IsNotNull())
            {
                //if (bkgOrderDetailCustomFormDataContract.lstCustomFormAttributes.IsNotNull())
                //    View.lstCustomFormAttributes = bkgOrderDetailCustomFormDataContract.lstCustomFormAttributes;
                //else
                //    View.lstCustomFormAttributes = new List<AttributesForCustomFormContract>();
                if (bkgOrderDetailCustomFormDataContract.lstDataForCustomForm.IsNotNull())
                    return bkgOrderDetailCustomFormDataContract.lstDataForCustomForm;
                else
                    return new List<BkgOrderDetailCustomFormUserData>();
            }
            return new List<BkgOrderDetailCustomFormUserData>();
        }

        private static bool IsUserGroupCustomAttributeExist(String useTypeCode, Int32 selectedDPMId, Int32 tenantId, Int32 currentUserId)
        {
            return ComplianceDataManager.GetCustomAttributesByNodes(useTypeCode, selectedDPMId, currentUserId, tenantId)
                .Any(x => x.CADataTypeCode.ToLower().Trim() == CustomAttributeDatatype.User_Group.GetStringValue().ToLower().Trim());
        }

        private static vw_AddressLookUp GetAddressLookupByHandlerId(String addressHandleId, Int32 tenantId)
        {
            Guid addHandleId = new Guid(addressHandleId);
            return ComplianceDataManager.GetAddressLookupByHandlerId(addHandleId, tenantId);
        }

        private static String GetDecryptedSSN(Int32 orgUserID, Int32 tenantId)
        {
            return ComplianceSetupManager.GetFormattedString(orgUserID, true, tenantId);
        }

        private static String ConvertApplicantDataIntoXML(ApplicantOrderCart applicantOrderCart, Int32 tenantId, Boolean ConsiderCurrentResidenceOnly = false, Boolean isCallForRequiredDocumentaion = false)
        {
            if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty() || isCallForRequiredDocumentaion)
            {
                XmlDocument _doc = new XmlDocument();
                XmlElement _rootNode = (XmlElement)_doc.AppendChild(_doc.CreateElement("PersonalDetails"));

                if (!applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile.IsNullOrEmpty())
                {
                    #region GENERATE ORGANIZATION USER PROFILE RELATED XML

                    Entity.ClientEntity.OrganizationUserProfile _orgUserProfile = applicantOrderCart.lstApplicantOrder[0].OrganizationUserProfile;
                    XmlNode _profileInformationNode = _rootNode.AppendChild(_doc.CreateElement("ProfileInformation"));
                    _profileInformationNode.AppendChild(_doc.CreateElement("OrganizationUserID")).InnerText = Convert.ToString(_orgUserProfile.OrganizationUserID);
                    _profileInformationNode.AppendChild(_doc.CreateElement("FirstName")).InnerText = _orgUserProfile.FirstName;
                    _profileInformationNode.AppendChild(_doc.CreateElement("LastName")).InnerText = _orgUserProfile.LastName;
                    _profileInformationNode.AppendChild(_doc.CreateElement("DOB")).InnerText = Convert.ToString(_orgUserProfile.DOB);
                    _profileInformationNode.AppendChild(_doc.CreateElement("SSN")).InnerText = _orgUserProfile.SSN;
                    _profileInformationNode.AppendChild(_doc.CreateElement("Gender")).InnerText = Convert.ToString(_orgUserProfile.Gender);
                    _profileInformationNode.AppendChild(_doc.CreateElement("PhoneNumber")).InnerText = _orgUserProfile.PhoneNumber;
                    _profileInformationNode.AppendChild(_doc.CreateElement("MiddleName")).InnerText = _orgUserProfile.MiddleName;
                    _profileInformationNode.AppendChild(_doc.CreateElement("PrimaryEmailAddress")).InnerText = _orgUserProfile.PrimaryEmailAddress;
                    _profileInformationNode.AppendChild(_doc.CreateElement("SecondaryEmailAddress")).InnerText = _orgUserProfile.SecondaryEmailAddress;
                    _profileInformationNode.AppendChild(_doc.CreateElement("SecondaryPhone")).InnerText = _orgUserProfile.SecondaryPhone;

                    _rootNode.AppendChild(_profileInformationNode);

                    #endregion
                }
                if (!applicantOrderCart.lstPersonAlias.IsNullOrEmpty())
                {
                    #region GENERATE PERSONAL ALIAS RELATED XML

                    XmlNode _aliasesNode = _rootNode.AppendChild(_doc.CreateElement("Aliases"));
                    foreach (var alias in applicantOrderCart.lstPersonAlias)
                    {
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        String aliasMiddleName = alias.MiddleName.IsNullOrEmpty() ? _noMiddleNameText : alias.MiddleName;
                        XmlNode expChild = _aliasesNode.AppendChild(_doc.CreateElement("Alias"));
                        expChild.AppendChild(_doc.CreateElement("InstanceId")).InnerText = Convert.ToString(alias.AliasSequenceId);
                        //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                        expChild.AppendChild(_doc.CreateElement("AliasName")).InnerText = alias.FirstName + " " + aliasMiddleName + " " + alias.LastName;
                        _rootNode.AppendChild(_aliasesNode);
                    }

                    #endregion
                }
                if (!applicantOrderCart.lstPrevAddresses.IsNullOrEmpty())
                {
                    #region GENERATE RESIDENTIAL ADDRESS RELATED XML

                    XmlNode _residentialAddressesNode = _rootNode.AppendChild(_doc.CreateElement("ResidentialAddresses"));
                    foreach (var _prevAddress in applicantOrderCart.lstPrevAddresses)
                    {
                        if (ConsiderCurrentResidenceOnly && (_prevAddress.isCurrent.IsNull() || !_prevAddress.isCurrent))
                        {
                            continue;
                        }
                        if (!_prevAddress.isDeleted)
                        {
                            //_prevAddress.UniqueId = Convert.ToString(Guid.NewGuid());
                            XmlNode _residentialAddressNode = _residentialAddressesNode.AppendChild(_doc.CreateElement("ResidentialAddress"));
                            _residentialAddressNode.AppendChild(_doc.CreateElement("InstanceId")).InnerText = Convert.ToString(_prevAddress.ResHistorySeqOrdID);
                            //_residentialAddressNode.AppendChild(_doc.CreateElement("InstanceId")).InnerText = Convert.ToString(rnd.Next(999999));
                            _residentialAddressNode.AppendChild(_doc.CreateElement("Address1")).InnerText = _prevAddress.Address1;
                            _residentialAddressNode.AppendChild(_doc.CreateElement("Address2")).InnerText = _prevAddress.Address2;
                            _residentialAddressNode.AppendChild(_doc.CreateElement("ZipCodeId")).InnerText = Convert.ToString(_prevAddress.ZipCodeID);
                            _residentialAddressNode.AppendChild(_doc.CreateElement("ZipCode")).InnerText = _prevAddress.Zipcode;
                            _residentialAddressNode.AppendChild(_doc.CreateElement("CityName")).InnerText = _prevAddress.CityName;
                            _residentialAddressNode.AppendChild(_doc.CreateElement("CountyName")).InnerText = _prevAddress.CountyName;
                            _residentialAddressNode.AppendChild(_doc.CreateElement("StateName")).InnerText = _prevAddress.StateName;
                            _residentialAddressNode.AppendChild(_doc.CreateElement("CountryName")).InnerText = _prevAddress.Country;
                            _residentialAddressNode.AppendChild(_doc.CreateElement("ResidingFrom")).InnerText = Convert.ToString(_prevAddress.ResidenceStartDate);
                            _residentialAddressNode.AppendChild(_doc.CreateElement("ResidingTo")).InnerText = Convert.ToString(_prevAddress.ResidenceEndDate);
                        }
                    }

                    #endregion
                }

                return _doc.OuterXml;

            }
            return String.Empty;
        }

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

        private static void GenerateGroupedAmount(ApplicantOrderCart applicantOrderCart, List<ApplicantOrderPaymentOptions> lstPkgPaymentOptns
            , Int32 tenantId, ApplicantOrderDataContract orderDataContract, List<Entity.ClientEntity.lkpPaymentOption> _lstClientPaymentOptions)
        {
            applicantOrderCart.lstPaymentGrouping = new List<PkgPaymentGrouping>();

            var _distinctPOIds = lstPkgPaymentOptns.DistinctBy(x => x.poid).ToList();

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
                                                                orderDataContract);

                applicantOrderCart.lstPaymentGrouping.Add(_pkgPayGroup);
            }
        }

        private static Decimal CalculateGroupAmount(PkgPaymentGrouping _pkgPayGroup, List<BackgroundPackagesContract> lstBkgPackages,
                                                  List<ApplicantOrderPaymentOptions> lstPkgPaymentOptns, List<OrderCartCompliancePackage> lstCompliancePackages,
                                              ApplicantOrderDataContract orderDataContract)
        {
            Decimal _totalAmount = 0;
            _pkgPayGroup.lstPackages = new Dictionary<String, Boolean>();
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
            return _totalAmount;
        }

        private static Decimal GetBackgroundPackagesPrice(ApplicantOrderCart applicantOrderCart)
        {
            Decimal _backgroundPackagesPrice = 0;

            if (!applicantOrderCart.lstApplicantOrder[0].lstPackages.IsNullOrEmpty())
            {
                foreach (var bkgPackage in applicantOrderCart.lstApplicantOrder[0].lstPackages)
                {
                    _backgroundPackagesPrice += (bkgPackage.TotalBkgPackagePrice.IsNullOrEmpty() ? AppConsts.NONE : bkgPackage.TotalBkgPackagePrice);
                    // _backgroundPackagesPrice += bkgPackage.TotalPrice.IsNullOrEmpty() ? AppConsts.NONE : bkgPackage.TotalPrice;
                }
            }
            return _backgroundPackagesPrice;
        }

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

        private static String GetInvoiceNumbers(Dictionary<String, String> _dicInvoiceNumber)
        {
            StringBuilder _sbInvNumbers = new StringBuilder();
            foreach (var invNo in _dicInvoiceNumber)
            {
                _sbInvNumbers.Append(invNo.Key + " - " + invNo.Value + " || ");
            }

            return Convert.ToString(_sbInvNumbers).Substring(0, Convert.ToString(_sbInvNumbers).LastIndexOf("||") - 1);
        }

        private static void UpdateEDSStatus(ApplicantOrderDataContract applicantOrderDataContract, Int32 orderId, Order userOrder, List<Int32> newlyAddedOPDIds = null)
        {
            if (userOrder.IsNotNull())
            {
                OrderPaymentDetail _orderPaymentDetail = null;

                foreach (OrderPaymentDetail opd in userOrder.OrderPaymentDetails.Where(slct => !slct.OPD_IsDeleted
                                                                                        && (newlyAddedOPDIds == null || newlyAddedOPDIds.Contains(slct.OPD_ID))
                                                                                       ))
                {
                    if (!opd.IsNullOrEmpty() && ComplianceDataManager.IsOrderPaymentIncludeEDSService(applicantOrderDataContract.TenantId, opd.OPD_ID) && opd.lkpPaymentOption.IsNotNull() && opd.lkpPaymentOption.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue())
                    {
                        _orderPaymentDetail = opd;
                        break;
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
                            objClearstarCCf.SaveCCFDataAndPDF(dicParam);
                        }
                    }
                    #endregion
                }
            }
        }

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
                }
            }
        }

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


        private static void SendNotificationForBulkRepeatedOrder(Int32 orderID, Int32 tenantID, Int32 currentLoggedInUserID)
        {

            Order orderData = ComplianceDataManager.GetOrderById(tenantID, orderID);

            if (!orderData.IsNullOrEmpty())
            {
                var orderPaymentDetail = orderData.OrderPaymentDetails.FirstOrDefault();
                if (!orderPaymentDetail.IsNullOrEmpty())
                {
                    //Send mail
                    if (!SecurityManager.IsLocationServiceTenant(tenantID))
                    {
                        CommunicationManager.SendOrderApprovalMail(orderPaymentDetail, currentLoggedInUserID, tenantID);
                        CommunicationManager.SendOrderApprovalMessage(orderPaymentDetail, currentLoggedInUserID, tenantID);
                    }
                }
            }
        }
        #endregion
    }
}
