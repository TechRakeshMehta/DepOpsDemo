using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Repository
{
    public class AdminEntryPortalRepository : ClientBaseRepository, IAdminEntryPortalRepository
    {
        #region Variables
        private ADB_LibertyUniversity_ReviewEntities _dbContext;
        #endregion

        #region Default Constructor to initilize DB Context
        ///// <summary>
        ///// Default constructor to initialize the context
        ///// </summary>
        public AdminEntryPortalRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }


        #endregion

        #region Methods
        /// <summary>
        /// Retrieves a list of all Organization User based on OrganizationUserID.
        /// </summary>
        /// <param name="organizationUserId">The value for organization user's id.</param>
        /// <returns>
        /// OrganizationUser.
        /// </returns>
        OrganizationUser IAdminEntryPortalRepository.GetOrganizationUser(Int32 OrderId, Int32 organizationUserId)
        {
            Order applicantOrder = _dbContext.Orders.FirstOrDefault(cnd => cnd.OrderID == OrderId && cnd.IsDeleted == false);
            if (!applicantOrder.IsNullOrEmpty())
            {
                return applicantOrder.OrganizationUserProfile.OrganizationUser;
            }
            return new OrganizationUser();
        }

        ///<summary>
        ///Retrieve Applicant Order Cart Data on the basis of OrderId
        ///</summary>
        ///<param name="OrderId">
        ///</param>
        ///<returns>
        /// Object of ApplicantOrderCart
        ///</returns>
        ApplicantOrderCart IAdminEntryPortalRepository.GetApplicantCartData(Int32 OrderId)
        {
            ApplicantOrderCart appOrderCartData = new ApplicantOrderCart();


            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("ams.GetAdminEntryApplicantOrderCart", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", OrderId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        appOrderCartData.SelectedHierarchyNodeID = appOrderCartData.NodeId = ds.Tables[0].Rows[0]["SelectedHierarchyNodeID"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["SelectedHierarchyNodeID"]);
                        appOrderCartData.OrderRequestType = ds.Tables[0].Rows[0]["OrderRequestType"] == DBNull.Value ? null : Convert.ToString(ds.Tables[0].Rows[0]["OrderRequestType"]);
                        appOrderCartData.HierarchyNodeName = ds.Tables[0].Rows[0]["HierarchyNodeName"] == DBNull.Value ? null : Convert.ToString(ds.Tables[0].Rows[0]["HierarchyNodeName"]);
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        appOrderCartData.lstApplicantOrder = new List<ApplicantOrder>();
                        appOrderCartData.lstApplicantOrder.Add(new ApplicantOrder());
                        appOrderCartData.lstApplicantOrder[0].lstPackages = new List<BackgroundPackagesContract>();
                        appOrderCartData.lstApplicantOrder[0].lstPackages = ds.Tables[1].AsEnumerable().Select(col =>
                        new BackgroundPackagesContract
                        {
                            AdditionalPrice = col["AdditionalPrice"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(col["AdditionalPrice"]), // bop
                            BasePrice = col["BasePrice"] == DBNull.Value ? 0 : Convert.ToDecimal(col["BasePrice"]),//bop --- // bphm
                            BPAId = col["BPAId"] == DBNull.Value ? 0 : Convert.ToInt32(col["BPAId"]),//backgroundpackage
                            BPAName = Convert.ToString(col["BPAName"]),//backgroundpackage
                            BPAViewDetails = col["BPAViewDetails"] == DBNull.Value ? false : Convert.ToBoolean(col["BPAViewDetails"]),//backgroundpackage
                            BPHMId = col["BPHMId"] == DBNull.Value ? 0 : Convert.ToInt32(col["BPHMId"]),//BPHM
                            CustomPriceText = Convert.ToString(col["CustomPriceText"]),//BPHM
                            DisplayNotesAbove = col["DisplayNotesAbove"] == DBNull.Value ? false : Convert.ToBoolean(col["DisplayNotesAbove"]),//lkpPackageNotesPosition
                            DisplayNotesBelow = col["DisplayNotesBelow"] == DBNull.Value ? false : Convert.ToBoolean(col["DisplayNotesBelow"]),//lkpPackageNotesPosition
                            DisplayOrder = col["DisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(col["DisplayOrder"]),//backgroundpackage
                            InsitutionHierarchyNodeID = col["InsitutionHierarchyNodeID"] == DBNull.Value ? 0 : Convert.ToInt32(col["InsitutionHierarchyNodeID"]),//bphm
                            IsExclusive = col["IsExclusive"] == DBNull.Value ? false : Convert.ToBoolean(col["IsExclusive"]),//bphm
                            //IsInvoiceOnlyAtPackageLevel = col["IsInvoiceOnlyAtPackageLevel"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(col["IsInvoiceOnlyAtPackageLevel"]),//
                            IsReqToQualifyInRotation = col["IsReqToQualifyInRotation"] == DBNull.Value ? false : Convert.ToBoolean(col["IsReqToQualifyInRotation"]),//backgroundpackage
                            MaxNumberOfYearforResidence = col["MaxNumberOfYearforResidence"] == DBNull.Value ? -1 : Convert.ToInt32(col["MaxNumberOfYearforResidence"]),//bphm
                            // NodeLevel = col["NodeLevel"] == DBNull.Value ? 0 : Convert.ToInt32(col["NodeLevel"]),//
                            PackageDetail = Convert.ToString(col["PackageDetail"]),//backgroundpackage
                            PackagePasscode = Convert.ToString(col["PackagePasscode"])//,//backgroundpackage

                            // PackageTypeCode = Convert.ToString(col["PackageTypeCode"]),//
                            // TotalBkgPackagePrice = col["TotalBkgPackagePrice"] == DBNull.Value ? 0 : Convert.ToDecimal(col["TotalBkgPackagePrice"]),//
                            // TotalLineItemPrice = col["TotalLineItemPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(col["TotalLineItemPrice"])//
                        }).ToList();
                    }
                }
            }
            return appOrderCartData;
        }

        /// <summary>
        /// Method to update the already existing order for applicant Completing Order Process.
        /// </summary>
        /// <param name="applicantOrder">Existing Order</param>
        /// <param name="applicantOrderDataContract"> applicant Order Data Contract </param>
        /// <param name="orgUserID">Organization USER ID</param>
        /// <param name="compliancePackages">Compliance Package list</param>
        /// <returns></returns>
        public Dictionary<String, String> UpdateApplicantCompletingOrderProcess(Order applicantOrder, ApplicantOrderDataContract applicantOrderDataContract,
                                                                                 out String paymentModeCode, Int32 orgUserID, out List<Int32> newlyAddedOPDIds,
                                                                                 List<OrderCartCompliancePackage> compliancePackages = null)
        {
            Dictionary<String, String> _dicInvoiceNumbers = new Dictionary<String, String>();

            var _invoiceNumber = String.Empty;
            DateTime _creationDateTime = DateTime.Now;

            newlyAddedOPDIds = new List<Int32>();
            #region UAT 916 Changes
            paymentModeCode = String.Empty; //To be removed
            #endregion

            #region Delete All OPDs Those order status is "Sent for online payment"
            var deletedOPDS = DeleteOPDSForCompletingOrderProcess(applicantOrder, _creationDateTime);

            if (applicantOrderDataContract.IsBillingCodeAmountAvlbl)
            {
                applicantOrder.OrderPaymentDetails.Where(cond => !cond.OPD_IsDeleted).ForEach(opd =>
                {
                    var paymentdetail = opd;
                    if (!paymentdetail.IsNullOrEmpty())
                    {
                        paymentdetail.OPD_IsDeleted = true;
                        paymentdetail.OPD_ModifiedByID = applicantOrder.ModifiedByID;
                        paymentdetail.OPD_ModifiedOn = _creationDateTime;
                    }
                    deletedOPDS.Add(paymentdetail);
                }
                    );
            }


            #endregion

            //DeptProgramPackageSubscription programPackageSubscription = new DeptProgramPackageSubscription();

            //if (applicantOrderDataContract.ProgramPackageSubscriptionId.IsNotNull() && applicantOrderDataContract.ProgramPackageSubscriptionId > AppConsts.NONE)
            //    programPackageSubscription = GetDeptProgramPackageSubscriptionDetail(applicantOrderDataContract.ProgramPackageSubscriptionId);

            //Int32 _createdById = applicantOrderDataContract.OrganizationUserProfile.OrganizationUserID;
            Int32 _createdById = orgUserID;
            Int32 _organizationUserID = applicantOrderDataContract.OrganizationUserProfile.OrganizationUserID;

            #region Store Browser agent
            //Need to Update Browser agent setting
            if (!String.IsNullOrEmpty(applicantOrderDataContract.UserBrowserAgentString))
            {
                UserBrowserAgent _browserAgent = applicantOrder.UserBrowserAgents.FirstOrDefault(x => !x.UBA_IsDeleted);
                if (!_browserAgent.IsNullOrEmpty())
                {
                    _browserAgent.UBA_String = applicantOrderDataContract.UserBrowserAgentString;
                    _browserAgent.UBA_CreatedByID = _createdById;
                    _browserAgent.UBA_CreatedOn = _creationDateTime;
                }
            }

            #endregion

            _dbContext.SaveChanges();
            //Get BkgOrderPackage  Data for Opds that are deleted
            var _lstBkgOrderPkg = GetBkgOrderPackageDetail(deletedOPDS);

            //#region UAT-1185 get OrderID for each Compliance Packages
            //if (compliancePackages.IsNotNull() && compliancePackages.Count > AppConsts.NONE)
            //{
            //    foreach (OrderCartCompliancePackage cp in compliancePackages)
            //    {
            //        if (applicantOrder.DeptProgramPackageID.Equals(cp.DPP_Id))
            //        {
            //            cp.OrderId = applicantOrder.OrderID;
            //            cp.OrderNumber = applicantOrder.OrderNumber;
            //        }
            //        else
            //        {
            //            if (applicantOrder.OrderGroupOrderNavProp.IsNotNull() && applicantOrder.OrderGroupOrderNavProp.Count > AppConsts.NONE)
            //            {
            //                Order o = applicantOrder.OrderGroupOrderNavProp.FirstOrDefault(ord => ord.DeptProgramPackageID == cp.DPP_Id);
            //                if (o.IsNotNull())
            //                {
            //                    cp.OrderId = o.OrderID;
            //                    cp.OrderNumber = o.OrderNumber;
            //                }
            //            }
            //        }
            //    }
            //}
            //#endregion

            var _orderStatusId = 0;

            var _bkgPkgTypeId = applicantOrderDataContract.lstOrderPackageTypes.Where(opt => opt.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
            var _compliancePkgTypeId = applicantOrderDataContract.lstOrderPackageTypes.Where(opt => opt.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
            List<OrderPaymentDetail> newlyAddedOPDList = new List<OrderPaymentDetail>();
            foreach (var poId in applicantOrderDataContract.lstGroupedData)
            {
                bool mainOrderPaymentDetailEntry = false;
                if (poId.TotalAmount > AppConsts.NONE)
                {
                    _orderStatusId = GetOrderStatusId(poId.PaymentModeCode);
                }
                else
                {
                    _orderStatusId = GetOrderStatusCode(ApplicantOrderStatus.Paid.GetStringValue());
                }

                #region UAT-1185 Adjust amount of main order payment details if it has extra compliance too
                decimal adjustedAmount = 0;
                List<Int32> childOrderIds = new List<int>();
                foreach (var pkg in poId.lstPackages)
                {
                    if (!pkg.Value)
                    {
                        var pkgId = Convert.ToInt32(pkg.Key.Split('_')[0]);
                        if (pkgId == applicantOrder.DeptProgramPackage.DPP_CompliancePackageID)
                            mainOrderPaymentDetailEntry = true;
                        //else
                        //{
                        //OrderCartCompliancePackage cp = compliancePackages.First(ocp => ocp.CompliancePackageID == pkgId && ocp.OrderId > AppConsts.NONE);
                        //if (pkgId == cp.CompliancePackageID)
                        //{
                        //    adjustedAmount += cp.GrandTotal.IsNull() ? AppConsts.NONE : Convert.ToDecimal(cp.GrandTotal);
                        //    childOrderIds.Add(cp.OrderId);
                        //}
                        //}
                    }
                    else
                        mainOrderPaymentDetailEntry = true;
                }

                #endregion
                // Store Invoice Number and Payment Mode
                OrderPaymentDetail paymentDetails = null;
                Int32 firstExtraOrderID = 0;
                if (mainOrderPaymentDetailEntry)
                {
                    _invoiceNumber = GenerateInvoiceNumber(applicantOrder.OrderID, applicantOrderDataContract.TenantId, false, childOrderIds);
                    paymentDetails = AddOnlinePaymentTransaction(applicantOrder, _creationDateTime, _invoiceNumber,
                                               poId.TotalAmount, poId.PaymentModeId, _orderStatusId, applicantOrder.CreatedByID, adjustedAmount);

                    newlyAddedOPDList.Add(paymentDetails);

                    int cmpPkgId = 0;
                    //if (compliancePackages.IsNotNull() && compliancePackages.Count > AppConsts.NONE)
                    //{
                    //    var compliancePackage = compliancePackages.Find(cp => cp.OrderId.Equals(applicantOrder.OrderID));
                    //    if (compliancePackage.IsNotNull() && compliancePackage.CompliancePackageID > AppConsts.NONE)
                    //        cmpPkgId = compliancePackage.CompliancePackageID;
                    //}

                    AddOrderPaymentPackageDetail(poId, _lstBkgOrderPkg, paymentDetails, _bkgPkgTypeId, _compliancePkgTypeId, applicantOrder.CreatedByID, _creationDateTime, cmpPkgId);
                }
                else if (childOrderIds.Count > 0)
                {
                    firstExtraOrderID = childOrderIds[0];
                    childOrderIds.RemoveAt(0);

                    _invoiceNumber = GenerateInvoiceNumber(firstExtraOrderID, applicantOrderDataContract.TenantId, false, childOrderIds);

                }
                _dicInvoiceNumbers.Add(poId.PaymentModeCode, _invoiceNumber);

                #region UAT-1185 generate order payment details entries
                //if (compliancePackages.IsNotNull() && compliancePackages.Count > AppConsts.NONE && applicantOrder.OrderGroupOrderNavProp.IsNotNull() && applicantOrder.OrderGroupOrderNavProp.Count > AppConsts.NONE)
                //{
                //    foreach (var pkg in poId.lstPackages)
                //    {
                //        if (!pkg.Value)
                //        {
                //            var pkgId = Convert.ToInt32(pkg.Key.Split('_')[0]);

                //            //OrderCartCompliancePackage cp = compliancePackages.Find(p => p.CompliancePackageID.Equals(pkgId));
                //            //if (cp.IsNotNull())
                //            //{
                //            //    Order extraOrder = applicantOrder.OrderGroupOrderNavProp.FirstOrDefault(eo => eo.OrderID.Equals(cp.OrderId));
                //            //    if (extraOrder.IsNotNull())
                //            //    {
                //            //        if (paymentDetails.IsNull())
                //            //        {
                //            //            decimal cpAdjustedAmount = adjustedAmount - (cp.GrandTotal.IsNull() ? AppConsts.NONE : Convert.ToDecimal(cp.GrandTotal));
                //            //            paymentDetails = AddOnlinePaymentTransaction(extraOrder, _creationDateTime, _invoiceNumber,
                //            //                poId.TotalAmount, poId.PaymentModeId, _orderStatusId, applicantOrder.CreatedByID, cpAdjustedAmount);

                //            //            newlyAddedOPDList.Add(paymentDetails);

                //            //            AddOrderPaymentPackageDetail(poId, null, paymentDetails, _bkgPkgTypeId, _compliancePkgTypeId, applicantOrder.CreatedByID, _creationDateTime, cp.CompliancePackageID);
                //            //        }
                //            //        else
                //            //        {
                //            //            OrderPaymentDetail opd = AddOrderPaymentDetail(extraOrder, paymentDetails.OnlinePaymentTransaction, _creationDateTime, (cp.GrandTotal.IsNull() ? AppConsts.NONE : Convert.ToDecimal(cp.GrandTotal)), poId.PaymentModeId, _orderStatusId, applicantOrder.CreatedByID);
                //            //            newlyAddedOPDList.Add(opd);
                //            //            AddOrderPaymentPackageDetail(poId, null, opd, _bkgPkgTypeId, _compliancePkgTypeId, applicantOrder.CreatedByID, _creationDateTime, cp.CompliancePackageID);
                //            //        }
                //            //    }
                //            //}
                //        }
                //    }
                //}
                _dbContext.SaveChanges();
                #endregion
            }

            if (!newlyAddedOPDList.IsNullOrEmpty())
            {
                newlyAddedOPDIds = newlyAddedOPDList.Where(cnd => !cnd.OPD_IsDeleted).Select(slct => slct.OPD_ID).ToList();
            }
            return _dicInvoiceNumbers;
        }


        /// <summary>
        /// To delete data from Online Payment Transaction, Order Payment Details and OrderPkg Payment Details 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="invoiceNumber"></param>
        /// <param name="creationDateTime"></param>
        private List<OrderPaymentDetail> DeleteOPDSForCompletingOrderProcess(Order order, DateTime currentDateTime)
        {
            List<OrderPaymentDetail> lstDeletedOPDS = new List<OrderPaymentDetail>();
            String sentForOnlinePaymentCode = ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue();
            String ordrPkgTypeComplianceRushOrderCode = OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue();
            if (!order.IsNullOrEmpty())
            {
                order.OrderPaymentDetails.Where(cnd => !cnd.OPD_IsDeleted && cnd.lkpOrderStatu != null && cnd.lkpOrderStatu.Code == sentForOnlinePaymentCode).ForEach(opd =>
                {
                    if (!opd.OrderPkgPaymentDetails.Any(OPPD => OPPD.OPPD_BkgOrderPackageID == null && !OPPD.OPPD_IsDeleted
                                                                         && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeComplianceRushOrderCode))
                    {
                        var paymentdetail = opd;
                        if (!paymentdetail.IsNullOrEmpty())
                        {
                            paymentdetail.OPD_IsDeleted = true;
                            paymentdetail.OPD_ModifiedByID = order.ModifiedByID;
                            paymentdetail.OPD_ModifiedOn = currentDateTime;
                        }
                        //}
                        lstDeletedOPDS.Add(paymentdetail);
                    }
                });
            }
            return lstDeletedOPDS;
        }

        public DeptProgramPackageSubscription GetDeptProgramPackageSubscriptionDetail(Int32 programPackageSubscriptionId)
        {
            DeptProgramPackageSubscription programPackageSubscription = _dbContext.DeptProgramPackageSubscriptions.Where(ps => ps.DPPS_ID == programPackageSubscriptionId).FirstOrDefault();
            return programPackageSubscription;
        }
        /// <summary>
        /// Method to Get background order package details for BOP_Ids those are the part of "Sent For Online Payment" status OPDs.
        /// </summary>
        /// <param name="sentForOnlinePaymentDetailList">OPDs those status are "Sent For Online Payment"</param>
        /// <returns>List of BkgOrderPackage</returns>
        public List<BkgOrderPackage> GetBkgOrderPackageDetail(List<OrderPaymentDetail> sentForOnlinePaymentDetailList)
        {
            String ordrPkgTypeBkgCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
            List<Int32> lstBopIDs = new List<Int32>();
            List<BkgOrderPackage> lstBkgOrderPackageList = new List<BkgOrderPackage>();
            foreach (var opd in sentForOnlinePaymentDetailList)
            {
                var bopIds = opd.OrderPkgPaymentDetails.Where(OPPD => OPPD.OPPD_BkgOrderPackageID != null && !OPPD.OPPD_IsDeleted
                                                               && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeBkgCode).Select(slct => slct.OPPD_BkgOrderPackageID.Value)
                                                               .ToList();
                lstBopIDs.AddRange(bopIds);
            }
            lstBkgOrderPackageList = _dbContext.BkgOrderPackages.Where(cnd => !cnd.BOP_IsDeleted && lstBopIDs.Contains(cnd.BOP_ID)).ToList();
            return lstBkgOrderPackageList;
        }
        /// <summary>
        /// Gets the OrderStatusId by Payment Type Code
        /// </summary>
        /// <param name="paymentModeCode"></param>
        /// <returns></returns>
        private Int32 GetOrderStatusId(String paymentModeCode)
        {
            var _statusCode = String.Empty;
            if (paymentModeCode.ToLower() == PaymentOptions.Credit_Card.GetStringValue().ToLower() || paymentModeCode.ToLower() == PaymentOptions.Paypal.GetStringValue().ToLower())
                _statusCode = ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue();
            else
                _statusCode = ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue();

            return GetOrderStatusCode(_statusCode);
        }
        public Int32 GetOrderStatusCode(String status)
        {
            return _dbContext.lkpOrderStatus
                             .Where(orderSts => orderSts.Code.ToLower() == status.ToLower() && !orderSts.IsDeleted)
                             .FirstOrDefault().OrderStatusID;
        }
        /// <summary>
        /// To get Invoice Number
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="tenantId"></param>
        /// <param name="isRushOrder"></param>
        /// <param name="orderPaymentCount"></param>
        /// <returns></returns>
        public String GenerateInvoiceNumber(Int32 orderID, Int32 tenantId, Boolean isRushOrder, List<Int32> childOrderIds = null)
        {
            Int32 orderPaymentCount = GetOrderPaymentCount(orderID);
            string _invoiceNumber = "COM" + "-" + tenantId + "-" + orderID;
            if (childOrderIds.IsNotNull() && childOrderIds.Count > AppConsts.NONE)
            {
                foreach (Int32 childOrderId in childOrderIds)
                    _invoiceNumber = _invoiceNumber + "," + childOrderId;
            }
            if (orderPaymentCount > 0)
            {
                _invoiceNumber = _invoiceNumber + "-" + "F" + orderPaymentCount;
            }
            return _invoiceNumber;

        }
        /// <summary>
        /// Attaches the OnlinePaymentTransaction instance for the current Order,
        /// which further attaches the OrderPaymentDetails and OrderPkgPaymentDetails
        /// </summary>
        /// <param name="applicantOrder"></param>
        /// <param name="creationDateTime"></param>
        /// <param name="invoiceNumber"></param>
        /// <param name="totalAmount"></param>
        /// <param name="paymentModeId"></param>
        /// <param name="orderStatusId"></param>
        /// <returns></returns>
        public OrderPaymentDetail AddOnlinePaymentTransaction(Order applicantOrder, DateTime creationDateTime, String invoiceNumber,
                                                 Decimal totalAmount, Int32 paymentModeId, Int32 orderStatusId, Int32 currentUserId, decimal adjustedAmount = 0)
        {
            OnlinePaymentTransaction paymentTransaction = new OnlinePaymentTransaction();
            paymentTransaction.Amount = totalAmount;
            paymentTransaction.CreatedByID = currentUserId;
            paymentTransaction.CreatedOn = creationDateTime;
            paymentTransaction.IsDeleted = false;
            paymentTransaction.Invoice_num = invoiceNumber;
            _dbContext.OnlinePaymentTransactions.AddObject(paymentTransaction);

            return AddOrderPaymentDetail(applicantOrder, paymentTransaction, creationDateTime, totalAmount - adjustedAmount, paymentModeId, orderStatusId, currentUserId);
        }

        private void AddOrderPaymentPackageDetail(PkgPaymentGrouping poId, List<BkgOrderPackage> _lstBkgOrderPkg, OrderPaymentDetail paymentDetails, int _bkgPkgTypeId, int _compliancePkgTypeId, int createdByID, DateTime _creationDateTime, int cmpPkgId)
        {
            foreach (var pkg in poId.lstPackages)
            {
                var pkgId = Convert.ToInt32(pkg.Key.Split('_')[0]);
                var _bopId = 0;
                Int32 bkgAdditionalOrderPackageID = _dbContext.lkpOrderPackageTypes.Where(cond => !cond.OPT_IsDeleted && cond.OPT_Code == "AAAG").Select(sel => sel.OPT_ID).FirstOrDefault(); //UAT-3268
                if (pkg.Value)
                {
                    if (_lstBkgOrderPkg.IsNotNull() && _lstBkgOrderPkg.Count > AppConsts.NONE)
                    {
                        var bkgOrdPkgData = _lstBkgOrderPkg.Where(bop => bop.BkgPackageHierarchyMapping.BPHM_BackgroundPackageID == pkgId && bop.BOP_IsDeleted == false).FirstOrDefault();
                        _bopId = bkgOrdPkgData.IsNullOrEmpty() ? 0 : bkgOrdPkgData.BOP_ID;
                    }
                    else
                        continue;
                }
                else if (!pkgId.Equals(cmpPkgId) && cmpPkgId != AppConsts.NONE)
                    continue;
                OrderPkgPaymentDetail pkgPaymentDetails = new OrderPkgPaymentDetail
                {
                    OrderPaymentDetail = paymentDetails,
                    OPPD_BkgOrderPackageID = pkg.Value ? _bopId : (int?)null,
                    OPPD_OrderPackageTypeID = pkg.Value ? (pkg.Key.Contains("Additional") ? bkgAdditionalOrderPackageID : _bkgPkgTypeId) : _compliancePkgTypeId,
                    OPPD_IsDeleted = false,
                    OPPD_CreatedBy = createdByID,
                    OPPD_CreatedOn = _creationDateTime
                };
                paymentDetails.OrderPkgPaymentDetails.Add(pkgPaymentDetails);

            }

        }
        /// <summary>
        /// Attaches the OrderPaymentDetail instance for the Normal Order Flow Order
        /// </summary>
        /// <param name="applicantOrder"></param>
        /// <param name="paymentTransaction"></param>
        /// <param name="creationDateTime"></param>
        private OrderPaymentDetail AddOrderPaymentDetail(Order applicantOrder, OnlinePaymentTransaction paymentTransaction,
                                           DateTime creationDateTime, Decimal totalAmount, Int32 paymentModeId, Int32 orderStatusId, Int32 currentUserId)
        {
            OrderPaymentDetail paymentDetails = new OrderPaymentDetail();
            paymentDetails.OPD_OrderID = applicantOrder.OrderID;
            paymentDetails.OnlinePaymentTransaction = paymentTransaction;
            paymentDetails.OPD_IsDeleted = false;
            paymentDetails.OPD_CreatedOn = creationDateTime;
            paymentDetails.OPD_CreatedByID = currentUserId;
            paymentDetails.OPD_PaymentOptionID = paymentModeId;
            paymentDetails.OPD_Amount = totalAmount;
            paymentDetails.OPD_OrderStatusID = orderStatusId;
            paymentDetails.OPD_ApprovalDate = creationDateTime;
            paymentDetails.OPD_ApprovedBy = currentUserId;

            _dbContext.OrderPaymentDetails.AddObject(paymentDetails);
            return paymentDetails;
        }
        public Int32 GetOrderPaymentCount(Int32 orderID)
        {
            Int32 orderPaymentCount = _dbContext.OrderPaymentDetails.Count(x => x.OPD_OrderID == orderID);
            orderPaymentCount += 1;
            return orderPaymentCount;
        }
        /// <summary>
        /// Saves the reference number in th order table and changes th status from Pending paymnt Approved to Paid..
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <param name="orderStatusCode">StatusCode to be updated</param>
        /// <param name="currentLoggedInUserId">CurrentLoggedInUserId</param>
        /// <param name="referenceNumber">Reference Number</param>
        /// <returns>True, if status is updated. Else false.</returns>
        public Boolean UpdateOrderStatus(Int32 orderId, String orderStatusCode, Int32 currentLoggedInUserId, String referenceNumber,
            List<lkpEventHistory> lstEvents, List<lkpOrderStatusType> lstOrderStatusType, Int32 tenantId, Int32 orderPaymentDetailId = 0)
        {
            Int32 orderStatusId = _dbContext.lkpOrderStatus.FirstOrDefault(x => x.Code.Equals(orderStatusCode) && x.IsDeleted == false).OrderStatusID;

            //OrderPaymentDetail orderPaymentDetail = _dbContext.OrderPaymentDetails.FirstOrDefault(x => x.OPD_OrderID == orderId && x.OPD_IsDeleted == false);
            var orderPaymentDetail = _dbContext.OrderPaymentDetails.FirstOrDefault(opd => opd.OPD_OrderID == orderId
                                                                                                && opd.OPD_IsDeleted == false
                                                                                                && opd.OPD_ID == orderPaymentDetailId);

            //For UAT -2379
            string paymentDueOrderStatus = ApplicantOrderStatus.Payment_Due.GetStringValue();
            Int32 paymentDueOrderStatusId = _dbContext.lkpOrderStatus.FirstOrDefault(x => x.Code.Equals(paymentDueOrderStatus) && x.IsDeleted == false).OrderStatusID;
            string offllineSettlementPaymentType = PaymentOptions.OfflineSettlement.GetStringValue();
            Int32 offllineSettlementPaymentTypeId = _dbContext.lkpPaymentOptions.FirstOrDefault(x => x.Code.Equals(offllineSettlementPaymentType) && x.IsDeleted == false).PaymentOptionID;

            if (orderPaymentDetail.IsNotNull())
            {
                var currentDateTime = DateTime.Now;
                // UAT -2379 , if payment due is getting approved in bulk, set paymenttype as offlinestatement
                if (orderPaymentDetail.OPD_OrderStatusID == paymentDueOrderStatusId && orderPaymentDetail.OPD_PaymentOptionID.IsNull())
                {
                    orderPaymentDetail.OPD_PaymentOptionID = offllineSettlementPaymentTypeId;
                }
                orderPaymentDetail.Order.ApprovedBy = currentLoggedInUserId;
                orderPaymentDetail.Order.ApprovalDate = currentDateTime;
                orderPaymentDetail.OPD_ReferenceNo = referenceNumber;
                orderPaymentDetail.Order.OrderStatusID = orderStatusId;
                orderPaymentDetail.Order.ModifiedByID = currentLoggedInUserId;
                orderPaymentDetail.Order.ModifiedOn = currentDateTime;
                orderPaymentDetail.OPD_ModifiedByID = currentLoggedInUserId;
                orderPaymentDetail.OPD_ModifiedOn = currentDateTime;
                orderPaymentDetail.OPD_OrderStatusID = orderStatusId; // UAT 916
                orderPaymentDetail.OPD_ApprovedBy = currentLoggedInUserId;
                orderPaymentDetail.OPD_ApprovalDate = currentDateTime;


                if (orderPaymentDetail.Order.RushOrderStatusID.IsNotNull() && orderPaymentDetail.Order.RushOrderStatusID != orderStatusId)
                    orderPaymentDetail.Order.RushOrderStatusID = orderStatusId;

                String _orderPackageTypeCode = orderPaymentDetail.Order.lkpOrderPackageType.OPT_Code;
                String _orderBKgPkgTypeCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();

                List<OrderPkgPaymentDetail> lstOrderPkgPaymentDetail = _dbContext.OrderPkgPaymentDetails.Where(cnd => cnd.lkpOrderPackageType.OPT_Code == _orderBKgPkgTypeCode
                                                                                                        && cnd.OPPD_IsDeleted == false
                                                                                                        && cnd.OrderPaymentDetail.OPD_OrderID == orderId
                                                                                                        && cnd.OrderPaymentDetail.OPD_IsDeleted == false).ToList();
                // Confirm
                if (orderStatusCode == ApplicantOrderStatus.Paid.GetStringValue() &&
                     (_orderPackageTypeCode == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue() ||
                     _orderPackageTypeCode == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()))
                {
                    Int32 _orderId = orderPaymentDetail.OPD_OrderID;
                    Int32 _bkgOrderStatusTypeId = lstOrderStatusType.Where(ost => ost.Code == BackgroundOrderStatus.NEW.GetStringValue()).FirstOrDefault().OrderStatusTypeID;
                    AddBkgOrderEventHistory(lstEvents, _orderId, currentLoggedInUserId, AppConsts.Bkg_Order_Approved, BkgOrderEvents.ORDER_APPROVED.GetStringValue()
                                            , orderPaymentDetailId, _bkgOrderStatusTypeId);
                }
                //_dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Adds the Event history for the Order, when Approved/Rejected by admin
        /// Also called when online order is being approved
        /// Currently only for Order Creation, Approved & Rejected (No other case)
        /// </summary>
        /// <param name="lstEvents"></param>
        /// <param name="_orderId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <param name="bkgOrderStatusTypeId"></param>
        /// <param name="orderEventMessage"></param>
        /// <param name="orderEventCode"></param>
        private void AddBkgOrderEventHistory(List<lkpEventHistory> lstEvents, Int32 _orderId, Int32 currentLoggedInUserId,
            String orderEventMessage, String orderEventCode, Int32 orderPaymentDetailID, Int32 bkgOrderStatusTypeId)
        {
            BkgOrder _bkgOrder = _dbContext.BkgOrders.Where(bo => bo.BOR_MasterOrderID == _orderId && !bo.BOR_IsDeleted).FirstOrDefault();
            DateTime _dtCreationDateTime = DateTime.Now;

            if (!_bkgOrder.IsNullOrEmpty() && !lstEvents.IsNullOrEmpty())
            {
                // In Case of Payment Rejected, keep it Under PAYMENT_PENDING, bkgOrderStatusTypeId would be 0.
                if (bkgOrderStatusTypeId != AppConsts.NONE)
                {
                    //Check whether All Background Packages are Paid using View. View also contains check with BOP to consider Partial Cancelled Order.
                    if (orderPaymentDetailID != AppConsts.NONE && AreAllBkgPackagesPaid_ExceptOrderPaymentDetailID(_bkgOrder.BOR_ID, _bkgOrder.BOR_MasterOrderID,
                                                                                                                   orderPaymentDetailID)
                         && (_bkgOrder.lkpOrderStatusType.Code == OrderStatusType.PAYMENTPENDING.GetStringValue()))
                    {
                        _bkgOrder.BOR_OrderStatusTypeID = bkgOrderStatusTypeId;
                        _bkgOrder.BOR_ModifiedByID = currentLoggedInUserId;
                        _bkgOrder.BOR_ModifiedOn = _dtCreationDateTime;

                    }
                }

                Int32 _orderEventId = lstEvents.Where(ev => ev.EH_Code == orderEventCode && !ev.EH_IsDeleted).FirstOrDefault().EH_ID;
                BkgOrderEventHistory _bgkOrderEventHistory = new BkgOrderEventHistory
                {
                    BOEH_EventHistoryId = _orderEventId,
                    BOEH_BkgOrderID = _bkgOrder.BOR_ID,
                    BOEH_CreatedByID = currentLoggedInUserId,
                    BOEH_CreatedOn = _dtCreationDateTime,
                    BOEH_IsDeleted = false,
                    BOEH_OrderEventDetail = orderEventMessage
                };

                _dbContext.BkgOrderEventHistories.AddObject(_bgkOrderEventHistory);
            }
        }
        private Boolean AreAllBkgPackagesPaid_ExceptOrderPaymentDetailID(Int32 bkgOrderID, Int32 orderID, Int32 orderPaymentDetailId)
        {
            String orderPackageTypeCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
            List<Int32> bkgOrderPackagesIDs = _dbContext.BkgOrderPackages.Where(cond => cond.BOP_BkgOrderID == bkgOrderID && cond.BOP_IsDeleted == false)
                                                                                .Select(col => col.BOP_ID).ToList();

            List<OrderPkgPaymentDetail> orderPkgPaymentDetailList = _dbContext.OrderPkgPaymentDetails
                                                                                  .Where(cond => cond.lkpOrderPackageType.OPT_Code == orderPackageTypeCode
                                                                                  && cond.OPPD_IsDeleted == false
                                                                                  && bkgOrderPackagesIDs.Contains(cond.OPPD_BkgOrderPackageID.Value)
                                                                                  && cond.OrderPaymentDetail.OPD_IsDeleted == false
                                                                                  && cond.OrderPaymentDetail.OPD_ID != orderPaymentDetailId
                                                                           ).ToList();

            return (orderPkgPaymentDetailList.All(x => x.OrderPaymentDetail.lkpOrderStatu.Code == ApplicantOrderStatus.Paid.GetStringValue()));
        }

        /// <summary>
        /// Save changes for Package Subscription
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean UpdatePackageSubscription()
        {
            if (_dbContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// This method is used for save the context of the DB for the case of BkgPackages only
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Boolean SaveDbContext(Int32 orderId)
        {
            if (_dbContext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// To create optional category entry in ApplicantComplianceCategoryData table
        /// </summary>
        /// <param name="packageSubscriptionIdsXML"></param>
        /// <param name="currentUserId"></param>
        public void CreateOptionalCategoryEntry(String packageSubscriptionIdsXML, Int32 currentUserId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_CreateOptionalCategoryEntry", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageSubscriptionIDs", packageSubscriptionIdsXML);
                command.Parameters.AddWithValue("@SystemUserID", currentUserId);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
        }
        //public Int32 AddPackageSubscriptions(PackageSubscription packageSubscription)
        //{
        //    if (!_dbContext.PackageSubscriptions.Any(x => x.OrderID == packageSubscription.OrderID && !x.IsDeleted))
        //    {
        //        _dbContext.PackageSubscriptions.AddObject(packageSubscription);
        //        _dbContext.SaveChanges();
        //    }
        //    return packageSubscription.PackageSubscriptionID;
        //    //return true;
        //}
        /// <summary>
        /// Updates IsDeleted = 1 for all the Applicant Subscriptions in 'ThirdPartyComplianceDataUpload' table 
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="currentUserId"></param>
        public void UpdateApplicantSubcriptions(Int32 organizationUserId, Int32 currentUserId)
        {
            var _lst = _dbContext.ThirdPartyComplianceDataUploads.Where(tpucd => tpucd.TPCDU_OrganizationUserID == organizationUserId
                                                                              && tpucd.TPCDU_IsDeleted == false).ToList();

            foreach (var subs in _lst)
            {
                subs.TPCDU_IsDeleted = true;
                subs.TPCDU_ModifiedOn = DateTime.Now;
                subs.TPCDU_ModifiedByID = currentUserId;
            }
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// Add the object for Compliance Data upload to third party like Sales Force
        /// </summary>
        /// <param name="_tpUploadData"></param>
        public void AddThirdPartyDataUpload(ThirdPartyComplianceDataUpload _tpUploadData)
        {
            _dbContext.ThirdPartyComplianceDataUploads.AddObject(_tpUploadData);
            _dbContext.SaveChanges();
        }
        public Boolean UpdateChanges()
        {
            if (_dbContext.SaveChanges() > 0)
                return true;
            return false;
        }
        public List<PackageSubscriptionList> CopyPackageData(List<SourceTargetSubscriptionList> subscriptionID, Int32 currentLoggedInUserID)
        {
            string str = "<Subscriptions>";
            foreach (SourceTargetSubscriptionList obj in subscriptionID)
            {
                str += "<Subscription>";
                str += "<SourceSubscriptionID>" + obj.SourceSubscriptionID.ToString() + "</SourceSubscriptionID>";
                str += "<TargetSubscriptionID>" + obj.TargetSubscriptionID.ToString() + "</TargetSubscriptionID>";
                str += "</Subscription>";
            }
            str += "</Subscriptions>";
            return _dbContext.CopyPackageData(str, currentLoggedInUserID).ToList();
        }
        public void SaveDataSyncHistory(String subscriptionXml, Int32 currentLoggedInUSerID, Int32 tenantId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("ams.usp_InsertRecordInDataSyncHistory", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SubscriptionIdXML", subscriptionXml);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUSerID);
                command.Parameters.AddWithValue("@TenantID", tenantId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
            }

        }
        /// <summary>
        /// Need to get the id of added document
        /// </summary>
        /// <param name="applicantDocument"></param>
        /// <returns></returns>
        public Int32 AddApplicantDocument(ApplicantDocument applicantDocument)
        {
            if (applicantDocument != null)
            {
                applicantDocument.Code = Guid.NewGuid();
                _dbContext.ApplicantDocuments.AddObject(applicantDocument);
            }
            _dbContext.SaveChanges();

            return applicantDocument.ApplicantDocumentID;


        }
        /// <summary>
        /// Save Order Result Document Mapping
        /// </summary>
        /// <param name="lstOrdResDocMap"></param>
        /// <returns>true/false</returns>
        public Boolean SaveOrderResultDocMap(List<OrderResultDocMap> lstOrdResDocMap)
        {
            foreach (OrderResultDocMap ordResDocMap in lstOrdResDocMap)
            {
                _dbContext.OrderResultDocMaps.AddObject(ordResDocMap);
            }
            if (_dbContext.SaveChanges() > 0)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Update Order Result Document Mapping
        /// </summary>
        /// <param name="lstOrdResDocMap"></param>
        /// <returns>true/false</returns>
        public Boolean UpdateOrderResultDocMap(List<OrderResultDocMap> lstOrdResDocMap, Int32 currentLoggedInUserId)
        {
            foreach (OrderResultDocMap ordResDocMap in lstOrdResDocMap)
            {
                List<ApplicantDocument> appDoc = _dbContext.ApplicantDocuments.Where(x => x.ApplicantDocumentID == ordResDocMap.ORDM_DocumentID && x.IsDeleted == false).ToList();
                appDoc.ForEach(doc =>
                {
                    doc.IsDeleted = true;
                    doc.ModifiedByID = currentLoggedInUserId;
                    doc.ModifiedOn = DateTime.Now;
                });

                List<ApplicantDocumentMerging> appDocMer = _dbContext.ApplicantDocumentMergings.Where(x => x.ADM_ApplicantDocumentID == ordResDocMap.ORDM_DocumentID && x.ADM_IsDeleted == false).ToList();
                appDocMer.ForEach(doc =>
                {
                    doc.ADM_IsDeleted = true;
                    doc.ADM_ModifiedByID = currentLoggedInUserId;
                    doc.ADM_ModifiedOn = DateTime.Now;
                });

                List<ApplicantComplianceDocumentMap> appCmpDocMap = _dbContext.ApplicantComplianceDocumentMaps.Where(x => x.ApplicantDocumentID == ordResDocMap.ORDM_DocumentID && x.IsDeleted == false).ToList();
                appCmpDocMap.ForEach(doc =>
                {

                    doc.IsDeleted = true;
                    doc.ModifiedByID = currentLoggedInUserId;
                    doc.ModifiedOn = DateTime.Now;

                });

                List<OrderResultDocMap> orderResultDocMap = _dbContext.OrderResultDocMaps.Where(x => x.ORDM_ID == ordResDocMap.ORDM_ID && x.ORDM_IsDeleted == false).ToList();
                orderResultDocMap.ForEach(doc =>
                {
                    doc.ORDM_IsDeleted = true;
                    doc.ORDM_ModifiedByID = currentLoggedInUserId;
                    doc.ORDM_ModifiedOn = DateTime.Now;
                });

                _dbContext.SaveChanges();
            }

            return true;
        }
        public Boolean UpdateDocumentPath(String newFileName, Int32 documentId)
        {
            ApplicantDocument applicantDocument = new ApplicantDocument();
            applicantDocument = _dbContext.ApplicantDocuments.FirstOrDefault(x => x.ApplicantDocumentID == documentId);

            applicantDocument.DocumentPath = newFileName;

            _dbContext.SaveChanges();

            return true;

        }
        public Boolean UpdateIsDocAssociated(Int32 packageSubscriptionID, Boolean isDocAssociated, Int32 currentLoggedInuserID)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("usp_UpdateIsDocumentAssociated", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageSubscription_ID", packageSubscriptionID);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInuserID);
                command.ExecuteScalar();
                return true;
            }
        }
        public void UpdateApplicanDetailsClient(OrganizationUser organizationUser, Dictionary<String, Object> dicAddressData, Int32 addressIdMaster, List<Entity.ResidentialHistory> lstResendentialHistory, List<Entity.PersonAlia> lstPersonAlias)
        {
            DALUtils.LoggerService.GetLogger().Info("Control Transfered to Method 'UpdateApplicanDetailsClient' to copy data for the organisationsation User with Id :" + organizationUser.OrganizationUserID + ".");
            OrganizationUser orgUserToUpdate = _dbContext.OrganizationUsers
                                                .Include("AddressHandle").Include("AddressHandle.Addresses")
                                                .Where(orgUsers => orgUsers.OrganizationUserID == organizationUser.OrganizationUserID).FirstOrDefault();
            if (orgUserToUpdate.IsNotNull())
            {
                DALUtils.LoggerService.GetLogger().Info("Organisation User to be updated is found.");
                orgUserToUpdate.FirstName = organizationUser.FirstName;
                orgUserToUpdate.MiddleName = organizationUser.MiddleName;
                orgUserToUpdate.LastName = organizationUser.LastName;
                orgUserToUpdate.Alias1 = organizationUser.Alias1;
                orgUserToUpdate.Alias2 = organizationUser.Alias2;
                orgUserToUpdate.Alias3 = organizationUser.Alias3;
                orgUserToUpdate.Gender = organizationUser.Gender;
                orgUserToUpdate.DOB = organizationUser.DOB;
                orgUserToUpdate.SSN = organizationUser.SSN;
                orgUserToUpdate.SecondaryPhone = organizationUser.SecondaryPhone;
                orgUserToUpdate.SecondaryEmailAddress = organizationUser.SecondaryEmailAddress.Trim();
                orgUserToUpdate.PrimaryEmailAddress = organizationUser.PrimaryEmailAddress.Trim();
                orgUserToUpdate.PhoneNumber = organizationUser.PhoneNumber;
                orgUserToUpdate.ModifiedByID = organizationUser.ModifiedByID;
                orgUserToUpdate.ModifiedOn = DateTime.Now;
                //UAT-2447
                orgUserToUpdate.IsInternationalPhoneNumber = organizationUser.IsInternationalPhoneNumber;
                orgUserToUpdate.IsInternationalSecondaryPhone = organizationUser.IsInternationalSecondaryPhone;
                //CBI|| CABS
                orgUserToUpdate.UserTypeID = organizationUser.UserTypeID.IsNullOrEmpty() ? (Int32?)null : organizationUser.UserTypeID;

                if (orgUserToUpdate.AddressHandle.IsNotNull() && orgUserToUpdate.AddressHandle.Addresses.IsNotNull())
                {
                    DALUtils.LoggerService.GetLogger().Info("Address Handle row exists for the Organisation User.");
                    Address userAddress = orgUserToUpdate.AddressHandle.Addresses.Where(add => add.AddressHandleID == orgUserToUpdate.AddressHandleID).FirstOrDefault();
                    var curResHisAddress = lstResendentialHistory.FirstOrDefault(cond => cond.RHI_IsCurrentAddress == true && cond.RHI_IsDeleted == false);
                    if (userAddress.IsNotNull() && curResHisAddress.IsNotNull())
                    {
                        DALUtils.LoggerService.GetLogger().Info("Address from the Security DB to be copied :" + curResHisAddress.RHI_AddressId + ".");
                        DALUtils.LoggerService.GetLogger().Info("Address Id Exists for the user in tenant DB :" + userAddress.AddressID + ".");
                        if (userAddress.AddressID != curResHisAddress.RHI_AddressId)
                        {
                            DALUtils.LoggerService.GetLogger().Info("Address Id in Security and Tenant DB are different and needs to be updated.");
                            AddressExt addressExtNew = null;
                            if (curResHisAddress.Address.ZipCodeID == 0)
                            {
                                DALUtils.LoggerService.GetLogger().Info("Address to be copied is Non US Address.");
                                if (curResHisAddress.Address.AddressExts.FirstOrDefault().IsNotNull())
                                {
                                    DALUtils.LoggerService.GetLogger().Info("Address Extension Row found in the Security DB.");
                                    Entity.AddressExt currentAddressExt = curResHisAddress.Address.AddressExts.FirstOrDefault();
                                    addressExtNew = new AddressExt();
                                    addressExtNew.AE_ID = currentAddressExt.AE_ID;
                                    addressExtNew.AE_CountryID = currentAddressExt.AE_CountryID;
                                    addressExtNew.AE_StateName = currentAddressExt.AE_StateName;
                                    addressExtNew.AE_CityName = currentAddressExt.AE_CityName;
                                    addressExtNew.AE_ZipCode = currentAddressExt.AE_ZipCode;
                                    addressExtNew.AE_County = currentAddressExt.AE_County;//UAT-3910
                                }
                            }
                            addressIdMaster = curResHisAddress.RHI_AddressId;
                            Guid addressHandleId = curResHisAddress.Address.AddressHandleID.IsNotNull() ? curResHisAddress.Address.AddressHandleID : Guid.NewGuid();
                            DALUtils.LoggerService.GetLogger().Info("Address ID from Security DB :" + addressIdMaster + ".");
                            DALUtils.LoggerService.GetLogger().Info("Address Handle ID from Security DB :" + addressHandleId + ".");
                            AddAddressHandle(addressHandleId);
                            //AddAddress(dicAddressData, addressHandleId, organizationUser.OrganizationUserID, addressIdMaster, addressExtNew);
                            AddAddress(dicAddressData, addressHandleId, Convert.ToInt32(organizationUser.ModifiedByID), addressIdMaster, addressExtNew);
                            orgUserToUpdate.AddressHandleID = addressHandleId;
                        }
                    }
                }

                if (lstResendentialHistory.IsNotNull())
                {
                    if (lstResendentialHistory.Count > 0)
                    {
                        var curResHisAddress = lstResendentialHistory.FirstOrDefault(cond => cond.RHI_IsCurrentAddress == true && cond.RHI_IsDeleted == false);
                        if (curResHisAddress.IsNotNull())
                        {
                            ResidentialHistory currentResedentialHistory = orgUserToUpdate.ResidentialHistories.FirstOrDefault(x => x.RHI_IsDeleted == false && x.RHI_IsCurrentAddress == true);
                            if (currentResedentialHistory.IsNotNull())
                            {
                                currentResedentialHistory.RHI_AddressId = curResHisAddress.RHI_AddressId;
                                currentResedentialHistory.RHI_ResidenceStartDate = curResHisAddress.RHI_ResidenceStartDate;
                                currentResedentialHistory.RHI_ModifiedByID = orgUserToUpdate.ModifiedByID;
                                currentResedentialHistory.RHI_ModifiedOn = DateTime.Now;
                                currentResedentialHistory.RHI_SequenceOrder = curResHisAddress.RHI_SequenceOrder;
                                currentResedentialHistory.RHI_MotherMaidenName = curResHisAddress.RHI_MotherMaidenName;
                                currentResedentialHistory.RHI_IdentificationNumber = curResHisAddress.RHI_IdentificationNumber;
                                currentResedentialHistory.RHI_DriverLicenseNumber = curResHisAddress.RHI_DriverLicenseNumber;
                            }
                            else
                            {
                                currentResedentialHistory = new ResidentialHistory();
                                currentResedentialHistory.RHI_ID = curResHisAddress.RHI_ID;
                                currentResedentialHistory.RHI_AddressId = curResHisAddress.RHI_AddressId;
                                currentResedentialHistory.RHI_IsCurrentAddress = true;
                                currentResedentialHistory.RHI_IsPrimaryResidence = false;
                                currentResedentialHistory.RHI_ResidenceStartDate = curResHisAddress.RHI_ResidenceStartDate;
                                currentResedentialHistory.RHI_IsDeleted = false;
                                currentResedentialHistory.RHI_CreatedByID = Convert.ToInt32(orgUserToUpdate.ModifiedByID);
                                currentResedentialHistory.RHI_CreatedOn = DateTime.Now;
                                currentResedentialHistory.RHI_OrganizationUserID = orgUserToUpdate.OrganizationUserID;
                                currentResedentialHistory.RHI_SequenceOrder = curResHisAddress.RHI_SequenceOrder;
                                currentResedentialHistory.RHI_MotherMaidenName = curResHisAddress.RHI_MotherMaidenName;
                                currentResedentialHistory.RHI_IdentificationNumber = curResHisAddress.RHI_IdentificationNumber;
                                currentResedentialHistory.RHI_DriverLicenseNumber = curResHisAddress.RHI_DriverLicenseNumber;

                                _dbContext.ResidentialHistories.AddObject(currentResedentialHistory);
                            }
                            lstResendentialHistory.Remove(curResHisAddress);
                        }
                    }

                    if (lstResendentialHistory.Count > 0)
                    {
                        // List of Resedential Histories associated with the organisaion User ID.
                        List<ResidentialHistory> lstCurrentResidentialHistory = orgUserToUpdate.ResidentialHistories.Where(x => x.RHI_IsDeleted == false).ToList();

                        foreach (var prevAddress in lstResendentialHistory)
                        {
                            ResidentialHistory newResHisObj = lstCurrentResidentialHistory.FirstOrDefault(x => x.RHI_ID == prevAddress.RHI_ID);
                            if (newResHisObj.IsNotNull())
                            {
                                if (newResHisObj.RHI_AddressId != prevAddress.RHI_AddressId)
                                {
                                    AddAddressHandle(prevAddress.Address.AddressHandleID);

                                    AddNewPreviousAddress(prevAddress, Convert.ToInt32(organizationUser.ModifiedByID));
                                    newResHisObj.RHI_AddressId = prevAddress.RHI_AddressId;
                                }
                                newResHisObj.RHI_ResidenceStartDate = prevAddress.RHI_ResidenceStartDate;
                                newResHisObj.RHI_ResidenceEndDate = prevAddress.RHI_ResidenceEndDate;
                                newResHisObj.RHI_IsCurrentAddress = prevAddress.RHI_IsCurrentAddress;
                                newResHisObj.RHI_IsDeleted = prevAddress.RHI_IsDeleted;

                                newResHisObj.RHI_ModifiedByID = organizationUser.ModifiedByID;
                                newResHisObj.RHI_ModifiedOn = DateTime.Now;
                                newResHisObj.RHI_SequenceOrder = prevAddress.RHI_SequenceOrder;
                                newResHisObj.RHI_MotherMaidenName = prevAddress.RHI_MotherMaidenName;
                                newResHisObj.RHI_IdentificationNumber = prevAddress.RHI_IdentificationNumber;
                                newResHisObj.RHI_DriverLicenseNumber = prevAddress.RHI_DriverLicenseNumber;
                            }
                            else
                            {
                                AddAddressHandle(prevAddress.Address.AddressHandleID);
                                AddNewResidentialHistory(prevAddress, organizationUser.OrganizationUserID, Convert.ToInt32(organizationUser.ModifiedByID));
                            }
                        }
                    }
                }
                if (lstPersonAlias.IsNotNull())
                {
                    foreach (Entity.PersonAlia tempPersonAlias in lstPersonAlias)
                    {
                        PersonAlia personAlias = orgUserToUpdate.PersonAlias.FirstOrDefault(x => x.PA_IsDeleted == false && x.PA_ID == tempPersonAlias.PA_ID);
                        if (personAlias.IsNotNull())
                        {
                            personAlias.PA_FirstName = tempPersonAlias.PA_FirstName;
                            personAlias.PA_LastName = tempPersonAlias.PA_LastName;
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            personAlias.PA_MiddleName = tempPersonAlias.PA_MiddleName;
                            personAlias.PA_IsDeleted = tempPersonAlias.PA_IsDeleted;

                            personAlias.PA_ModifiedBy = orgUserToUpdate.ModifiedByID;
                            personAlias.PA_ModifiedOn = DateTime.Now;

                            //CBI|| CABS 
                            PersonAliasExtension personAliasExtension = _dbContext.PersonAliasExtensions.Where(con => !con.PAE_IsDeleted && con.PAE_PersonAliasID == personAlias.PA_ID).FirstOrDefault();
                            if (!tempPersonAlias.PersonAliasExtensions.IsNullOrEmpty())
                            {
                                if (!personAliasExtension.IsNullOrEmpty())
                                {
                                    personAliasExtension.PAE_Suffix = tempPersonAlias.PersonAliasExtensions.FirstOrDefault().PAE_Suffix;
                                    personAliasExtension.PAE_ModifiedBy = Convert.ToInt32(organizationUser.ModifiedByID);
                                    personAliasExtension.PAE_ModifiedOn = DateTime.Now;
                                }

                                else
                                {
                                    personAliasExtension = new PersonAliasExtension();
                                    personAliasExtension.PAE_Suffix = tempPersonAlias.PersonAliasExtensions.FirstOrDefault().PAE_Suffix;
                                    personAliasExtension.PAE_CreatedBy = Convert.ToInt32(organizationUser.ModifiedByID);
                                    personAliasExtension.PAE_CreatedOn = DateTime.Now;

                                    personAlias.PersonAliasExtensions.Add(personAliasExtension);
                                }

                            }
                            else if (!personAliasExtension.IsNullOrEmpty())
                            {
                                personAliasExtension.PAE_ModifiedBy = Convert.ToInt32(organizationUser.ModifiedByID);
                                personAliasExtension.PAE_ModifiedOn = DateTime.Now;
                                personAliasExtension.PAE_IsDeleted = true;
                            }
                        }
                        else
                        {
                            personAlias = new PersonAlia();
                            personAlias.PA_ID = tempPersonAlias.PA_ID;
                            personAlias.PA_AliasIdentifier = tempPersonAlias.PA_AliasIdentifier;
                            personAlias.PA_OrganizationUserID = tempPersonAlias.PA_OrganizationUserID;
                            personAlias.PA_FirstName = tempPersonAlias.PA_FirstName;
                            personAlias.PA_LastName = tempPersonAlias.PA_LastName;
                            personAlias.PA_MiddleName = tempPersonAlias.PA_MiddleName;
                            personAlias.PA_IsDeleted = tempPersonAlias.PA_IsDeleted;
                            //personAlias.PA_CreatedBy = organizationUser.OrganizationUserID;
                            personAlias.PA_CreatedBy = Convert.ToInt32(organizationUser.ModifiedByID);
                            personAlias.PA_CreatedOn = DateTime.Now;
                            orgUserToUpdate.PersonAlias.Add(personAlias);

                            //CBI|| CABS
                            if (!tempPersonAlias.PersonAliasExtensions.IsNullOrEmpty())
                            {
                                PersonAliasExtension personAliasExtension = new PersonAliasExtension();
                                personAliasExtension.PAE_Suffix = tempPersonAlias.PersonAliasExtensions.FirstOrDefault().PAE_Suffix;
                                personAliasExtension.PAE_CreatedBy = Convert.ToInt32(organizationUser.ModifiedByID);
                                personAliasExtension.PAE_CreatedOn = DateTime.Now;

                                personAlias.PersonAliasExtensions.Add(personAliasExtension);
                            }
                        }
                    }
                }
                _dbContext.SaveChanges();
            }
        }
        public void AddAddressHandle(Guid addressHandleId)
        {
            AddressHandle addressHandleNew = new AddressHandle();
            addressHandleNew.AddressHandleID = addressHandleId;
            _dbContext.AddressHandles.AddObject(addressHandleNew);
        }
        public void AddAddress(Dictionary<String, Object> dicAddressData, Guid addressHandleId, Int32 currentUserId, Int32 addressIdMaster, AddressExt addressExtn)
        {
            Address addressNew = new Address
            {
                AddressID = addressIdMaster,
                Address1 = Convert.ToString(dicAddressData.GetValue("address1")),
                Address2 = Convert.ToString(dicAddressData.GetValue("address2")),
                ZipCodeID = Convert.ToInt32(dicAddressData.GetValue("zipcodeid")),
                AddressHandleID = addressHandleId,
                CreatedOn = DateTime.Now,
                CreatedByID = currentUserId
            };
            if (addressExtn.IsNotNull())
                addressNew.AddressExts.Add(addressExtn);
            _dbContext.Addresses.AddObject(addressNew);
        }
        private void AddNewPreviousAddress(Entity.ResidentialHistory newResHisObj, Int32 currentUserId)
        {
            Address addressNew = new Address();
            addressNew.AddressID = newResHisObj.Address.AddressID;
            addressNew.Address1 = newResHisObj.Address.Address1;
            addressNew.Address2 = newResHisObj.Address.Address2;
            if (newResHisObj.Address.ZipCodeID > 0)
                addressNew.ZipCodeID = newResHisObj.Address.ZipCodeID;
            else
            {
                if (newResHisObj.Address.AddressExts.IsNotNull() && newResHisObj.Address.AddressExts.Count > 0)
                {
                    Entity.AddressExt MasterAddressExt = newResHisObj.Address.AddressExts.FirstOrDefault();
                    AddressExt addressExtNew = new AddressExt();
                    addressExtNew.AE_ID = MasterAddressExt.AE_ID;
                    addressExtNew.AE_CountryID = MasterAddressExt.AE_CountryID;
                    addressExtNew.AE_StateName = MasterAddressExt.AE_StateName;
                    addressExtNew.AE_CityName = MasterAddressExt.AE_CityName;
                    addressExtNew.AE_ZipCode = MasterAddressExt.AE_ZipCode;
                    addressNew.AddressExts.Add(addressExtNew);
                }
            }
            addressNew.AddressHandleID = newResHisObj.Address.AddressHandleID;
            addressNew.CreatedOn = DateTime.Now;
            addressNew.CreatedByID = currentUserId;
            _dbContext.Addresses.AddObject(addressNew);
        }
        public void AddNewResidentialHistory(Entity.ResidentialHistory newResHisObj, Int32 currentUserId, Int32 orgUserId)
        {

            AddNewPreviousAddress(newResHisObj, orgUserId);

            Entity.ClientEntity.ResidentialHistory objResHistory = new ResidentialHistory();
            objResHistory.RHI_ID = newResHisObj.RHI_ID;
            objResHistory.RHI_IsCurrentAddress = false;
            objResHistory.RHI_IsPrimaryResidence = newResHisObj.RHI_IsPrimaryResidence;
            objResHistory.RHI_ResidenceStartDate = newResHisObj.RHI_ResidenceStartDate;
            objResHistory.RHI_ResidenceEndDate = newResHisObj.RHI_ResidenceEndDate;
            objResHistory.RHI_IsDeleted = false;
            objResHistory.RHI_CreatedByID = orgUserId;
            objResHistory.RHI_CreatedOn = DateTime.Now;
            objResHistory.RHI_OrganizationUserID = currentUserId;
            objResHistory.RHI_AddressId = newResHisObj.RHI_AddressId;
            objResHistory.RHI_SequenceOrder = newResHisObj.RHI_SequenceOrder;
            objResHistory.RHI_MotherMaidenName = newResHisObj.RHI_MotherMaidenName;
            objResHistory.RHI_IdentificationNumber = newResHisObj.RHI_IdentificationNumber;
            objResHistory.RHI_DriverLicenseNumber = newResHisObj.RHI_DriverLicenseNumber;
            _dbContext.ResidentialHistories.AddObject(objResHistory);
        }

        /// <summary>
        /// Generate New Compliance and Background orders 
        /// </summary>
        /// <param name="organizationUserProfile"></param>
        /// <param name="addressId"></param>
        /// <param name="addressHandleId"></param>
        /// <param name="userOrder"></param>
        /// <param name="programPackageSubscriptionId"></param>
        /// <param name="selectedPaymentModeId"></param>
        /// <param name="tenantId"></param>
        /// <param name="lstAttributeValues"></param>
        /// <param name="lastNodeDPMId"></param>
        /// <param name="lstBackgroundPackages"></param>
        /// <param name="paymentModeCode"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Dictionary<String, String> SaveApplicantOrderProcessClient(Order applicantOrder, ApplicantOrderDataContract applicantOrderDataContract, Int16 svcLineItemDispatchStatusId, out String paymentModeCode, out Int32 orderId, Int32 orgUserID, Int32? orderRequestNewOrderTypeId = null, List<OrderCartCompliancePackage> compliancePackages = null)
        {
            Dictionary<String, String> _dicInvoiceNumbers = new Dictionary<String, String>();

            var _invoiceNumber = String.Empty;

            paymentModeCode = String.Empty; //To be removed 

            DeptProgramPackageSubscription programPackageSubscription = new DeptProgramPackageSubscription();

            if (applicantOrderDataContract.ProgramPackageSubscriptionId.IsNotNull() && applicantOrderDataContract.ProgramPackageSubscriptionId > AppConsts.NONE)
                programPackageSubscription = GetDeptProgramPackageSubscriptionDetail(applicantOrderDataContract.ProgramPackageSubscriptionId);

            DateTime _creationDateTime = DateTime.Now;
            DateTime _modifiedDateTime = DateTime.Now;

            Int32 _createdById = orgUserID;
            Int32 _modifiedById = orgUserID;
            Int32 _organizationUserID = applicantOrderDataContract.OrganizationUserProfile.OrganizationUserID;


            #region Admin Entry Portal Settings
            //int dpm_ID = applicantOrder.SelectedNodeID.IsNullOrEmpty() ? AppConsts.NONE : applicantOrder.SelectedNodeID.Value;
            //var accountSetting = _dbContext.DeptProgramAdminEntryAcctSettings.Where(x => x.DPAEAS_DeptProgramMappingID == dpm_ID).FirstOrDefault();
            #endregion
            ////#region Store Áddress Exts, Residential Histories etc.

            //if (applicantOrderDataContract.OrganizationUserProfile.AddressHandle.IsNotNull() && applicantOrderDataContract.OrganizationUserProfile.AddressHandle.Addresses.IsNotNull())
            //{
            //    AddressHandle addressHandleClient = applicantOrderDataContract.OrganizationUserProfile.AddressHandle;
            //    //[SS]:Admin Entry
            //    //addressHandleClient.AddressHandleID = applicantOrderDataContract.AddressHandleIdMaster;
            //    //_dbContext.AddressHandles.AddObject(addressHandleClient);
            //    Entity.ResidentialHistoryProfile currentAddress = applicantOrderDataContract.lstResidentialHistoryProfile.Where(x => x.RHIP_IsCurrentAddress).FirstOrDefault();
            //    Address addressClient = applicantOrderDataContract.OrganizationUserProfile.AddressHandle.Addresses.FirstOrDefault();
            //    if (addressClient.ZipCodeID == 0)
            //    {
            //        if (currentAddress.IsNotNull())
            //        {
            //            if (currentAddress.Address.AddressExts.IsNotNull() && currentAddress.Address.AddressExts.Count > 0)
            //            {
            //                Entity.AddressExt masterAddressExt = currentAddress.Address.AddressExts.FirstOrDefault();
            //                AddressExt addressExt = new AddressExt();
            //                addressExt.AE_ID = masterAddressExt.AE_ID;
            //                addressExt.AE_AddressID = applicantOrderDataContract.AddressIdMaster;
            //                addressExt.AE_CountryID = masterAddressExt.AE_CountryID;
            //                addressExt.AE_StateName = masterAddressExt.AE_StateName;
            //                addressExt.AE_CityName = masterAddressExt.AE_CityName;
            //                addressExt.AE_ZipCode = masterAddressExt.AE_ZipCode;
            //                addressExt.AE_County = masterAddressExt.AE_County;
            //                addressClient.AddressExts.Add(addressExt);
            //            }
            //        }
            //    }
            //    addressClient.AddressID = applicantOrderDataContract.AddressIdMaster;
            //    addressClient.AddressHandleID = applicantOrderDataContract.AddressHandleIdMaster;
            //    addressClient.CreatedOn = _creationDateTime;
            //    addressClient.CreatedByID = _createdById;
            //    addressClient.IsActive = true;
            //    //[SS]:Admin Entry
            //    //_dbContext.Addresses.AddObject(addressClient);
            //    //applicantOrderDataContract.OrganizationUserProfile.AddressHandleID = applicantOrderDataContract.AddressHandleIdMaster;

            //    //Add current address into residential history profile table.
            //    ResidentialHistoryProfile currentResHistoryProfile = new ResidentialHistoryProfile();
            //    currentResHistoryProfile.RHIP_ID = currentAddress.RHIP_ID;
            //    currentResHistoryProfile.RHIP_IsCurrentAddress = true;
            //    currentResHistoryProfile.RHIP_IsPrimaryResidence = currentAddress.RHIP_IsPrimaryResidence;
            //    currentResHistoryProfile.RHIP_ResidenceStartDate = currentAddress.RHIP_ResidenceStartDate;
            //    currentResHistoryProfile.RHIP_ResidenceEndDate = currentAddress.RHIP_ResidenceEndDate;
            //    currentResHistoryProfile.RHIP_IsDeleted = currentAddress.RHIP_IsDeleted;
            //    currentResHistoryProfile.RHIP_CreatedBy = currentAddress.RHIP_CreatedBy;
            //    currentResHistoryProfile.RHIP_CreatedOn = currentAddress.RHIP_CreatedOn;
            //    currentResHistoryProfile.RHIP_SequenceOrder = currentAddress.RHIP_SequenceOrder;
            //    currentResHistoryProfile.RHIP_MotherMaidenName = currentAddress.RHIP_MotherMaidenName;
            //    currentResHistoryProfile.RHIP_IdentificationNumber = currentAddress.RHIP_IdentificationNumber;
            //    currentResHistoryProfile.RHIP_DriverLicenseNumber = currentAddress.RHIP_DriverLicenseNumber;
            //    currentResHistoryProfile.OrganizationUserProfile = applicantOrderDataContract.OrganizationUserProfile;
            //    currentResHistoryProfile.Address = addressClient;
            //    _dbContext.ResidentialHistoryProfiles.AddObject(currentResHistoryProfile);
            //    //applicantOrderDataContract.lstResidentialHistoryProfile.Remove(currentAddress);
            //}

            //if (applicantOrderDataContract.lstResidentialHistoryProfile.IsNotNull())
            //{
            //    if (applicantOrderDataContract.lstResidentialHistoryProfile.Count > 0)
            //    {
            //        foreach (var newResHisProfile in applicantOrderDataContract.lstResidentialHistoryProfile)
            //        {
            //            if (!newResHisProfile.RHIP_IsCurrentAddress)
            //            {
            //                AddAddressHandle(newResHisProfile.Address.AddressHandleID);
            //                Address addressNew = new Address();
            //                addressNew.AddressID = newResHisProfile.Address.AddressID;
            //                addressNew.Address1 = newResHisProfile.Address.Address1;
            //                addressNew.Address2 = newResHisProfile.Address.Address2;
            //                addressNew.ZipCodeID = newResHisProfile.Address.ZipCodeID;
            //                if (addressNew.ZipCodeID == 0)
            //                {
            //                    if (newResHisProfile.Address.AddressExts.IsNotNull() && newResHisProfile.Address.AddressExts.Count > 0)
            //                    {
            //                        Entity.AddressExt masterAddressExt = newResHisProfile.Address.AddressExts.FirstOrDefault();
            //                        AddressExt addressExtNew = new AddressExt();
            //                        addressExtNew.AE_ID = masterAddressExt.AE_ID;
            //                        addressExtNew.AE_CountryID = masterAddressExt.AE_CountryID;
            //                        addressExtNew.AE_StateName = masterAddressExt.AE_StateName;
            //                        addressExtNew.AE_CityName = masterAddressExt.AE_CityName;
            //                        addressExtNew.AE_ZipCode = masterAddressExt.AE_ZipCode;
            //                        addressNew.AddressExts.Add(addressExtNew);
            //                    }
            //                }
            //                addressNew.AddressHandleID = newResHisProfile.Address.AddressHandleID;
            //                addressNew.CreatedOn = _creationDateTime;
            //                addressNew.CreatedByID = _createdById;
            //                _dbContext.Addresses.AddObject(addressNew);

            //                Entity.ClientEntity.ResidentialHistoryProfile objResHistoryProfile = new ResidentialHistoryProfile();
            //                objResHistoryProfile.RHIP_ID = newResHisProfile.RHIP_ID;
            //                objResHistoryProfile.RHIP_IsCurrentAddress = false;
            //                objResHistoryProfile.RHIP_IsPrimaryResidence = newResHisProfile.RHIP_IsPrimaryResidence;
            //                objResHistoryProfile.RHIP_ResidenceStartDate = newResHisProfile.RHIP_ResidenceStartDate;
            //                objResHistoryProfile.RHIP_ResidenceEndDate = newResHisProfile.RHIP_ResidenceEndDate;
            //                objResHistoryProfile.RHIP_IsDeleted = newResHisProfile.RHIP_IsDeleted;
            //                objResHistoryProfile.RHIP_CreatedBy = _createdById;
            //                objResHistoryProfile.RHIP_CreatedOn = _creationDateTime;
            //                objResHistoryProfile.RHIP_SequenceOrder = newResHisProfile.RHIP_SequenceOrder;
            //                objResHistoryProfile.RHIP_MotherMaidenName = newResHisProfile.RHIP_MotherMaidenName;
            //                objResHistoryProfile.RHIP_IdentificationNumber = newResHisProfile.RHIP_IdentificationNumber;
            //                objResHistoryProfile.RHIP_DriverLicenseNumber = newResHisProfile.RHIP_DriverLicenseNumber;
            //                objResHistoryProfile.OrganizationUserProfile = applicantOrderDataContract.OrganizationUserProfile;
            //                objResHistoryProfile.Address = addressNew;
            //                _dbContext.ResidentialHistoryProfiles.AddObject(objResHistoryProfile);
            //            }
            //        }
            //    }
            //}

            #endregion

            #region Store Personal Aliases

            //if (applicantOrderDataContract.lstPersonAliasProfile.IsNotNull())
            //{
            //    foreach (Entity.PersonAliasProfile tempPersonAlias in applicantOrderDataContract.lstPersonAliasProfile)
            //    {
            //        PersonAliasProfile newPersonAliasPofile = new PersonAliasProfile();
            //        newPersonAliasPofile.PAP_ID = tempPersonAlias.PAP_ID;
            //        newPersonAliasPofile.PAP_FirstName = tempPersonAlias.PAP_FirstName;
            //        newPersonAliasPofile.PAP_MiddleName = tempPersonAlias.PAP_MiddleName;
            //        newPersonAliasPofile.PAP_LastName = tempPersonAlias.PAP_LastName;
            //        newPersonAliasPofile.PAP_IsDeleted = tempPersonAlias.PAP_IsDeleted;
            //        newPersonAliasPofile.PAP_CreatedBy = _createdById;
            //        newPersonAliasPofile.PAP_CreatedOn = _creationDateTime;

            //        if (!tempPersonAlias.PersonAliasProfileExtensions.IsNullOrEmpty())
            //        {
            //            PersonAliasProfileExtension personalAliasProfileExtension = new PersonAliasProfileExtension();
            //            personalAliasProfileExtension.PAPE_ID = tempPersonAlias.PersonAliasProfileExtensions.FirstOrDefault().PAPE_ID;
            //            personalAliasProfileExtension.PAPE_PersonAliasProfileID = tempPersonAlias.PersonAliasProfileExtensions.FirstOrDefault().PAPE_PersonAliasProfileID;
            //            personalAliasProfileExtension.PAPE_Suffix = tempPersonAlias.PersonAliasProfileExtensions.FirstOrDefault().PAPE_Suffix;
            //            personalAliasProfileExtension.PAPE_CreatedBy = _createdById;
            //            personalAliasProfileExtension.PAPE_CreatedOn = _creationDateTime;
            //            newPersonAliasPofile.PersonAliasProfileExtensions.Add(personalAliasProfileExtension);
            //        }
            //        applicantOrderDataContract.OrganizationUserProfile.PersonAliasProfiles.Add(newPersonAliasPofile);
            //    }
            //}
            #endregion

            applicantOrderDataContract.OrganizationUserProfile.CreatedByID = _createdById;
            applicantOrderDataContract.OrganizationUserProfile.CreatedOn = _creationDateTime;
            // _dbContext.OrganizationUserProfiles.AddObject(applicantOrderDataContract.OrganizationUserProfile);

            if (applicantOrderDataContract.ProgramPackageSubscriptionId > AppConsts.NONE)
            {
                applicantOrder.DeptProgramPackageID = programPackageSubscription.DPPS_DeptProgramPackageID;
                applicantOrder.SubscriptionLabel = programPackageSubscription.SubscriptionOption.Label;
            }

            applicantOrder.OrganizationUserProfileID = applicantOrderDataContract.OrganizationUserProfile.OrganizationUserProfileID;

            //if (applicantOrder.RushOrderPrice != null)
            //{
            //var paymentOptnCode = GetPaymentOptionCodeById(applicantOrderDataContract.CompliancePkgPaymentOptionId);
            //applicantOrder.RushOrderStatusID = GetOrderStatusId(paymentOptnCode);
            //}

            applicantOrder.OrderDate = _creationDateTime;
            if (applicantOrder.SubscriptionMonth == null && applicantOrderDataContract.ProgramPackageSubscriptionId > AppConsts.NONE)
            {
                applicantOrder.SubscriptionMonth = programPackageSubscription.SubscriptionOption.Month;
                applicantOrder.SubscriptionYear = programPackageSubscription.SubscriptionOption.Year;
            }

            //if (programPackageSubscriptionId > AppConsts.NONE)
            //    applicantOrder.SubscriptionLabel = programPackageSubscription.SubscriptionOption.Label;

            applicantOrder.IsDeleted = false;
            //applicantOrder.CreatedOn = _creationDateTime;
            applicantOrder.ModifiedOn = _modifiedDateTime;
            //applicantOrder.CreatedByID = _createdById;
            applicantOrder.ModifiedByID = _modifiedById;
            applicantOrder.SelectedNodeID = applicantOrderDataContract.LastNodeDPMId;
            //applicantOrder.OrderStatusID = 1; // TO be Reomved
            //Set the Archived State to Active.
            applicantOrder.ArchiveStateID = GetArchiveStateIDByCode(ArchiveState.Active);

            //UAT 264
            //Update the Previous Order as Cancelled            
            //if (_status == ApplicantOrderStatus.Paid.GetStringValue()) -- UAT 916 change
            if (applicantOrder.GrandTotal == AppConsts.NONE)
            {
                applicantOrder.ApprovalDate = _creationDateTime;
                //Get the previous Order using the current Order PreviousOrderID
                Order previousOrder = GetPreviousOrderDetail(applicantOrder.PreviousOrderID);
                if (previousOrder.IsNotNull())
                {
                    String _prevStatus = ApplicantOrderStatus.Cancelled.GetStringValue();
                    previousOrder.OrderStatusID = GetOrderStatusCode(_prevStatus);
                }
            }

            #region Store Browser agent

            if (!String.IsNullOrEmpty(applicantOrderDataContract.UserBrowserAgentString))
            {
                UserBrowserAgent _browserAgent = new UserBrowserAgent
                {
                    UBA_OrderID = applicantOrder.OrderID,
                    UBA_String = applicantOrderDataContract.UserBrowserAgentString,
                    UBA_IsDeleted = false,
                    UBA_CreatedByID = _createdById,
                    UBA_CreatedOn = _creationDateTime
                };
                applicantOrder.UserBrowserAgents.Add(_browserAgent);
            }

            #endregion

            // #region Set Previous order ID For Repurchasing of Archived and Expired Package [UAT-977: Additional work towards archive ability]
            //Int32? prevOrderIdRepurchasingOrder = null;
            //if (orderRequestNewOrderTypeId.IsNotNull() && applicantOrder.OrderRequestTypeID == orderRequestNewOrderTypeId && !programPackageSubscription.IsNullOrEmpty() && !programPackageSubscription.DeptProgramPackage.IsNullOrEmpty())
            //{
            //    PackageSubscription packageSubscriptionOfRepurchaseOrder = null;
            //    //packageSubscriptionOfRepurchaseOrder = GetPackageSubscriptionByPackageID(programPackageSubscription.DeptProgramPackage.DPP_CompliancePackageID, _organizationUserID);

            //    //if (!packageSubscriptionOfRepurchaseOrder.IsNullOrEmpty() && ((packageSubscriptionOfRepurchaseOrder.lkpArchiveState.IsNotNull() && packageSubscriptionOfRepurchaseOrder.lkpArchiveState.AS_Code == ArchiveState.Archived.GetStringValue()
            //    //      && packageSubscriptionOfRepurchaseOrder.ExpiryDate.Value.Date < DateTime.Now.Date)
            //    //      //UAT-1220: WB: As an applicant, I should be able to place a new order for a package which is already expired and retain my entered data
            //    //      || packageSubscriptionOfRepurchaseOrder.ExpiryDate.Value.Date < DateTime.Now.Date)
            //    //    )
            //    //{
            //    //    prevOrderIdRepurchasingOrder = packageSubscriptionOfRepurchaseOrder.OrderID;
            //    //}
            //}

            //if (prevOrderIdRepurchasingOrder.IsNotNull() && prevOrderIdRepurchasingOrder > AppConsts.NONE)
            //{
            //    applicantOrder.PreviousOrderID = prevOrderIdRepurchasingOrder;
            //}
            // #endregion

            //_dbContext.Orders.AddObject(applicantOrder);

            //#region UAT-1185 Save extra compliance Orders here
            //if (compliancePackages.IsNotNull())
            //{
            //    List<OrderCartCompliancePackage> lstExtraCompliancePackages = compliancePackages.FindAll(cp => cp.DPP_Id != applicantOrder.DeptProgramPackageID).ToList();
            //    if (lstExtraCompliancePackages.IsNotNull())
            //    {
            //        foreach (OrderCartCompliancePackage cp in lstExtraCompliancePackages)
            //        {
            //            Order newOrder = new Order();
            //            newOrder.OrganizationUserProfileID = applicantOrder.OrganizationUserProfileID;
            //            newOrder.DeptProgramPackageID = cp.DPP_Id;
            //            newOrder.OrderStatusID = applicantOrder.OrderStatusID;
            //            newOrder.OrderDate = applicantOrder.OrderDate;
            //            newOrder.OrderMachineIP = applicantOrder.OrderMachineIP;
            //            newOrder.CreatedByID = applicantOrder.CreatedByID;
            //            newOrder.CreatedOn = applicantOrder.CreatedOn;
            //            newOrder.OrderRequestTypeID = applicantOrder.OrderRequestTypeID;
            //            newOrder.ArchiveStateID = applicantOrder.ArchiveStateID;
            //            newOrder.HierarchyNodeID = applicantOrder.HierarchyNodeID;
            //            newOrder.SelectedNodeID = applicantOrder.SelectedNodeID;
            //            newOrder.OrderPackageType = applicantOrderDataContract.lstOrderPackageTypes.Where(opt => opt.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;

            //            newOrder.TotalPrice = cp.CurrentPackagePrice;
            //            newOrder.ProgramDuration = cp.ProgramDuration;
            //            newOrder.GrandTotal = cp.GrandTotal;
            //            //newOrder.PackageBundleId = applicantOrder.PackageBundleId;  //UAT-3283
            //            programPackageSubscription = GetDeptProgramPackageSubscriptionDetail(cp.DPPS_ID);
            //            if (programPackageSubscription.IsNotNull())
            //            {
            //                newOrder.SubscriptionLabel = programPackageSubscription.SubscriptionOption.Label;
            //                newOrder.SubscriptionMonth = programPackageSubscription.SubscriptionOption.Month;
            //                newOrder.SubscriptionYear = programPackageSubscription.SubscriptionOption.Year;
            //            }

            //            applicantOrder.OrderGroupOrderNavProp.Add(newOrder);

            //        }
            //        //UAT-3283
            //        if (!applicantOrder.OrderBundlePackages.IsNullOrEmpty() && applicantOrder.OrderBundlePackages.Count > AppConsts.NONE)
            //        {
            //            foreach (OrderBundlePackage orderBundlePackage in applicantOrder.OrderBundlePackages)
            //            {
            //                orderBundlePackage.Order = applicantOrder;
            //                orderBundlePackage.OBP_CreatedBy = applicantOrder.CreatedByID;
            //                orderBundlePackage.OBP_CreatedOn = _creationDateTime;
            //            }
            //        }
            //        //UTA-3757
            //        if (!applicantOrder.OrderApplicantSignatures.IsNullOrEmpty() && applicantOrder.OrderApplicantSignatures.Count > AppConsts.NONE)
            //        {
            //            foreach (OrderApplicantSignature orderApplicantSignature in applicantOrder.OrderApplicantSignatures)
            //            {
            //                orderApplicantSignature.Order = applicantOrder;
            //                orderApplicantSignature.OAS_CreatedBy = applicantOrder.CreatedByID;
            //                orderApplicantSignature.OAS_CreatedOn = _creationDateTime;
            //            }
            //        }
            //    }
            //}
            //#endregion

            #region Add data of Background Packages
            var _lstBkgOrderPkg = new List<BkgOrderPackage>();

            if (!applicantOrderDataContract.lstBackgroundPackages.IsNullOrEmpty())
            {
                #region Add BkgOrder Table Data

                // BkgOrder _bkgOrder = new BkgOrder();
                BkgOrder _bkgOrder = _dbContext.BkgOrders.FirstOrDefault(cnd => cnd.BOR_MasterOrderID == applicantOrder.OrderID);
                //_bkgOrder.Order = applicantOrder;
                if (!_bkgOrder.IsNullOrEmpty())
                {
                    //_bkgOrder.BOR_OrganizationUserProfileID = applicantOrderDataContract.OrganizationUserProfile.OrganizationUserProfileID; //                
                    _bkgOrder.BOR_GrandTotal = applicantOrderDataContract.lstBackgroundPackages.Sum(x => x.TotalBkgPackagePrice);
                    _bkgOrder.BOR_TotalPrice = applicantOrderDataContract.lstBackgroundPackages.Sum(x => x.TotalBkgPackagePrice);
                    _bkgOrder.BOR_InstitutionStatusColorID = null;
                    _bkgOrder.BOR_BkgOrderClientStatus = null;
                    _bkgOrder.BOR_ADBAdminNotes = null;
                    _bkgOrder.BOR_InstitutionAcceptanceStatusTypeID = null;
                    _bkgOrder.BOR_InstitutionRejectionReasonID = null;
                    _bkgOrder.BOR_IsArchived = false;
                    _bkgOrder.BOR_IsDeleted = false;
                    _bkgOrder.BOR_ModifiedByID = _createdById;
                    _bkgOrder.BOR_ModifiedOn = _creationDateTime;
                    _bkgOrder.BOR_OrderStatusTypeID = applicantOrderDataContract.BkgOrderStatusTypeId;
                    _bkgOrder.BOR_OrderResultsRequestedByApplicant = applicantOrderDataContract.IsSendBackgroundReport;

                    //Implement Changes for BkgAdminEntryOrderDetail Table

                    BkgAdminEntryOrderDetail bkgAdminEntryOrderDetail = _bkgOrder.BkgAdminEntryOrderDetails.Where(con => !con.BAEOD_IsDeleted).FirstOrDefault();
                    bkgAdminEntryOrderDetail = _dbContext.BkgAdminEntryOrderDetails.Where(con => !con.BAEOD_IsDeleted && con.BAEOD_BkgOrderID == _bkgOrder.BOR_ID).FirstOrDefault();
                    if (!bkgAdminEntryOrderDetail.IsNullOrEmpty())
                    {
                        bkgAdminEntryOrderDetail.BAEOD_OrderDraftStatusID = !applicantOrderDataContract.BkgAdminEntryOrderDetailDraftStatusId.IsNullOrEmpty()
                                                                                && applicantOrderDataContract.BkgAdminEntryOrderDetailDraftStatusId > AppConsts.NONE
                                                                            ? applicantOrderDataContract.BkgAdminEntryOrderDetailDraftStatusId : bkgAdminEntryOrderDetail.BAEOD_OrderDraftStatusID;
                        bkgAdminEntryOrderDetail.BAEOD_OrderStatusID = !applicantOrderDataContract.BkgAdminEntryOrderDetailStatusId.IsNullOrEmpty()
                                                                            && applicantOrderDataContract.BkgAdminEntryOrderDetailStatusId > AppConsts.NONE
                                                                       ? Convert.ToInt16(applicantOrderDataContract.BkgAdminEntryOrderDetailStatusId) : bkgAdminEntryOrderDetail.BAEOD_OrderStatusID;
                        bkgAdminEntryOrderDetail.BAEOD_ModifiedBy = _createdById;
                        bkgAdminEntryOrderDetail.BAEOD_ModifiedOn = _creationDateTime;
                        if (!applicantOrderDataContract.BkgAdminEntryOrderDetail_TransmittDate.IsNullOrEmpty())
                        {
                            bkgAdminEntryOrderDetail.BAEOD_TransmitDate = applicantOrderDataContract.BkgAdminEntryOrderDetail_TransmittDate.Value;
                        }
                        //UAT-4775
                        bkgAdminEntryOrderDetail.BAEOD_OrderHoldStatusID = !applicantOrderDataContract.BkgAdminEntryOrderDetailHoldStatusId.IsNullOrEmpty()
                                                                                && applicantOrderDataContract.BkgAdminEntryOrderDetailHoldStatusId > AppConsts.NONE
                                                                            ? applicantOrderDataContract.BkgAdminEntryOrderDetailHoldStatusId : null;


                    }
                }
                #endregion

                #region Add TransactionGroup table data

                TransactionGroup _transactionGrp = new TransactionGroup();
                //_transactionGrp.Order = applicantOrder;
                _transactionGrp.TG_OrderID = applicantOrder.OrderID;
                _transactionGrp.TG_TxnDate = _creationDateTime;
                _transactionGrp.TG_CreatedByID = _createdById;
                _transactionGrp.TG_CreatedOn = _creationDateTime;

                #endregion

                List<Int32> _lstPackageIds = applicantOrderDataContract.lstBackgroundPackages.Select(bp => bp.BPAId).ToList();

                // Get the list of All the Service groups of the selected packages
                // Check if Nullable type should be there 
                List<BkgPackageSvcGroup> _lstBkgPackageSvcGroups = _dbContext.BkgPackageSvcGroups.Include("BkgPackageSvcs").Where(bpsg => _lstPackageIds.Contains(bpsg.BPSG_BackgroundPackageID) && !bpsg.BPSG_IsDeleted
                                                         && !bpsg.BkgSvcGroup.BSG_IsDeleted && !bpsg.BackgroundPackage.BPA_IsDeleted && bpsg.BackgroundPackage.BPA_IsActive).ToList();
                List<Int32> _lstPHMIds = new List<Int32>();

                foreach (var pkg in applicantOrderDataContract.lstBackgroundPackages)
                {
                    #region Bkg Package
                    var _pkgPricingData = applicantOrderDataContract.lstPricingData.Where(pd => pd.PackageId == pkg.BPAId).FirstOrDefault();
                    _lstPHMIds.Add(pkg.BPHMId);

                    #region  Add BkgOrderPackage Table Data

                    //BkgOrderPackage _bkgOrderPackage = new BkgOrderPackage();
                    //_bkgOrderPackage.BkgOrder = _bkgOrder;
                    BkgOrderPackage _bkgOrderPackage = _bkgOrder.BkgOrderPackages.FirstOrDefault(cnd => cnd.BOP_BkgPackageHierarchyMappingID == pkg.BPHMId && !cnd.BOP_IsDeleted);
                    _bkgOrderPackage.BOP_IsDeleted = false;
                    _bkgOrderPackage.BOP_ModifiedByID = _createdById;
                    _bkgOrderPackage.BOP_ModifiedOn = _creationDateTime;
                    _bkgOrderPackage.BOP_BkgPackageHierarchyMappingID = pkg.BPHMId;
                    _bkgOrderPackage.BOP_BasePrice = pkg.TotalBkgPackagePrice == AppConsts.NONE ? AppConsts.NONE : pkg.BasePrice;
                    _bkgOrderPackage.BOP_TotalLineItemPrice = pkg.TotalBkgPackagePrice == AppConsts.NONE ? AppConsts.NONE : (pkg.TotalBkgPackagePrice - pkg.BasePrice);
                    //UAT-3268
                    if (pkg.IsReqToQualifyInRotation)
                        _bkgOrderPackage.BOP_AdditionalPrice = pkg.AdditionalPrice;

                    _lstBkgOrderPkg.Add(_bkgOrderPackage);
                    #endregion

                    //changes done for UAT - 1371 - WB: Service group without line items should not be created with order
                    List<Int32> lstBkgSvcGrpIdsWithLineItems = new List<Int32>();
                    if (!_pkgPricingData.lstOrderLineItems.IsNullOrEmpty())
                    {
                        lstBkgSvcGrpIdsWithLineItems = _pkgPricingData.lstOrderLineItems.Select(col => col.PackageSvcGrpID).Distinct().ToList();
                    }
                    List<BkgPackageSvcGroup> _lstTempPkgSvcGroups = _lstBkgPackageSvcGroups.Where(bpsg => bpsg.BPSG_BackgroundPackageID == pkg.BPAId && lstBkgSvcGrpIdsWithLineItems.Contains(bpsg.BPSG_ID)).ToList();

                    // Add all the service groups related to the particular Package
                    foreach (var pkgSvcGroup in _lstTempPkgSvcGroups)
                    {
                        #region  Add BkgOrderPackageSvcGroup Table Data

                        BkgOrderPackageSvcGroup _bkgOrderPackageSvcGroup = new BkgOrderPackageSvcGroup();
                        //_bkgOrderPackageSvcGroup.BkgOrderPackage = _bkgOrderPackage;
                        _bkgOrderPackageSvcGroup.OPSG_BkgOrderPackageID = _bkgOrderPackage.BOP_ID;
                        _bkgOrderPackageSvcGroup.OPSG_BkgSvcGroupID = Convert.ToInt32(pkgSvcGroup.BPSG_BkgSvcGroupID);
                        _bkgOrderPackageSvcGroup.OPSG_IsDeleted = false;
                        _bkgOrderPackageSvcGroup.OPSG_CreatedByID = _createdById;
                        _bkgOrderPackageSvcGroup.OPSG_CreatedOn = _creationDateTime;
                        _bkgOrderPackageSvcGroup.OPSG_SvcGrpReviewStatusTypeID = applicantOrderDataContract.NewSvcGrpReviewStatusTypeId;
                        _bkgOrderPackageSvcGroup.OPSG_SvcGrpStatusTypeID = applicantOrderDataContract.NewSvcGrpStatusTypeId;

                        //[SS]:Admin ENtry
                        _bkgOrderPackage.BkgOrderPackageSvcGroups.Add(_bkgOrderPackageSvcGroup);
                        #endregion

                        foreach (var service in pkgSvcGroup.BkgPackageSvcs)
                        {
                            if (!service.BPS_IsDeleted && !service.BackgroundService.BSE_IsDeleted)
                            {
                                #region Add BkgOrderPackageSvc Table Data

                                BkgOrderPackageSvc _bkgOrderPackageSvc = new BkgOrderPackageSvc();
                                _bkgOrderPackageSvc.BkgOrderPackageSvcGroup = _bkgOrderPackageSvcGroup;
                                _bkgOrderPackageSvc.BOPS_BackgroundServiceID = service.BPS_BackgroundServiceID;
                                _bkgOrderPackageSvc.BOPS_IsDeleted = false;
                                _bkgOrderPackageSvc.BOPS_CreatedByID = _createdById;
                                _bkgOrderPackageSvc.BOPS_CreatedOn = _creationDateTime;

                                #endregion

                                if (!_pkgPricingData.IsNullOrEmpty() && !_pkgPricingData.lstOrderLineItems.IsNullOrEmpty())
                                {
                                    List<OrderLineItem_PricingData> _lstLineItems = _pkgPricingData.lstOrderLineItems.Where(oli => oli.PackageServiceId == service.BPS_ID).ToList();
                                    foreach (var _lineItem in _lstLineItems)
                                    {
                                        #region Add BkgOrderPackageSvcLineItem Table Data
                                        BkgOrderPackageSvcLineItem _bkgOrdPkgSvcLineItem = new BkgOrderPackageSvcLineItem();
                                        _bkgOrdPkgSvcLineItem.BkgOrderPackageSvc = _bkgOrderPackageSvc;
                                        _bkgOrdPkgSvcLineItem.PSLI_OrderLineItemStatusID = applicantOrderDataContract.OrderLineItemStatusId;
                                        _bkgOrdPkgSvcLineItem.PSLI_ServiceItemID = _lineItem.PackageServiceItemId;
                                        _bkgOrdPkgSvcLineItem.PSLI_IsDeleted = false;
                                        _bkgOrdPkgSvcLineItem.PSLI_CreatedByID = _createdById;
                                        _bkgOrdPkgSvcLineItem.PSLI_CreatedOn = _creationDateTime;
                                        _bkgOrdPkgSvcLineItem.PSLI_DispatchedExternalVendor = svcLineItemDispatchStatusId;
                                        _bkgOrdPkgSvcLineItem.PSLI_NeedsExternalDispatch = true;
                                        _bkgOrdPkgSvcLineItem.PSLI_Description = _lineItem.Description;
                                        _bkgOrdPkgSvcLineItem.PSLI_AdminEntryLineItemStatusID = applicantOrderDataContract.AdminEntryLineItemStatusId;
                                        //UAT-4162//
                                        if (!_bkgOrdPkgSvcLineItem.BkgOrderPackageSvc.BackgroundService.IsNullOrEmpty() && _bkgOrdPkgSvcLineItem.BkgOrderPackageSvc.BackgroundService.lkpBkgSvcType.BST_Code == BkgServiceType.ELECTRONICDRUGSCREEN.GetStringValue())
                                        {
                                            String dueDataPullStatusTypeCode = DataPullStatusType.DUE_STATUS.GetStringValue();
                                            _bkgOrdPkgSvcLineItem.PSLI_DataPulledStatusTypeID = _dbContext.lkpDataPullStatusTypes.Where(cond => cond.DPST_Code == dueDataPullStatusTypeCode && !cond.DPST_IsDeleted).FirstOrDefault().DPST_ID;
                                        }
                                        #endregion

                                        if (!_lineItem.Price.IsNullOrEmpty())
                                        {
                                            #region Add Transaction Table Data if Line Item Price is available
                                            if (!_lineItem.PackageOrderItemPriceId.IsNullOrEmpty() && _lineItem.PackageOrderItemPriceId != AppConsts.NONE)
                                            {
                                                Transaction _transaction = new Transaction();
                                                _transaction.TransactionGroup = _transactionGrp;
                                                _transaction.BkgOrderPackageSvcLineItem = _bkgOrdPkgSvcLineItem;
                                                _transaction.TD_PackageServiceItemPriceID = _lineItem.PackageOrderItemPriceId;
                                                _transaction.TD_Amount = _lineItem.Price;
                                                _transaction.TD_IsDeleted = false;
                                                _transaction.TD_CreatedByID = _createdById;
                                                _transaction.TD_CreatedOn = _creationDateTime;
                                                _transaction.TD_Description = _lineItem.PriceDescription;
                                                _bkgOrdPkgSvcLineItem.Transactions.Add(_transaction);
                                            }
                                            #endregion
                                        }
                                        if (!_lineItem.lstFees.IsNullOrEmpty())
                                        {
                                            foreach (var fee in _lineItem.lstFees)
                                            {
                                                #region Add Transaction Table Data if Line Item Fees is available
                                                if (!fee.PackageOrderItemFeeId.IsNullOrEmpty() && fee.PackageOrderItemFeeId != AppConsts.NONE)
                                                {
                                                    Transaction _transaction = new Transaction();
                                                    _transaction.TransactionGroup = _transactionGrp;
                                                    _transaction.BkgOrderPackageSvcLineItem = _bkgOrdPkgSvcLineItem;
                                                    _transaction.TD_PackageServiceItemFeeID = fee.PackageOrderItemFeeId;
                                                    _transaction.TD_Amount = fee.Amount;
                                                    _transaction.TD_IsDeleted = false;
                                                    _transaction.TD_CreatedByID = _createdById;
                                                    _transaction.TD_CreatedOn = _creationDateTime;
                                                    _transaction.TD_Description = fee.Description;
                                                    _bkgOrdPkgSvcLineItem.Transactions.Add(_transaction);
                                                }
                                                #endregion
                                            }
                                        }

                                        foreach (var _bkgSvcAttDataGroup in _lineItem.lstBkgSvcAttributeDataGroup)
                                        {
                                            Int32? _instanceId = AppConsts.NONE;
                                            if (_bkgSvcAttDataGroup.InstanceId != AppConsts.NONE)
                                                _instanceId = this.GetInstanceId(applicantOrderDataContract, _bkgSvcAttDataGroup.AttributeGroupId, _bkgSvcAttDataGroup.InstanceId);
                                            else
                                                _instanceId = null;

                                            if (_instanceId != AppConsts.NONE)
                                            {
                                                BkgOrderLineItemDataMapping _lineItemDataMapping = new BkgOrderLineItemDataMapping();

                                                _lineItemDataMapping.BkgOrderPackageSvcLineItem = _bkgOrdPkgSvcLineItem;
                                                _lineItemDataMapping.OLIDM_BkgSvcAttributeGroupID = _bkgSvcAttDataGroup.AttributeGroupId;
                                                _lineItemDataMapping.OLIDM_InstanceID = _instanceId;
                                                _lineItemDataMapping.OLIDM_CreatedByID = _createdById;
                                                _lineItemDataMapping.OLIDM_CreatedOn = _creationDateTime;
                                                _lineItemDataMapping.OLIDM_IsDeleted = false;
                                                _bkgOrdPkgSvcLineItem.BkgOrderLineItemDataMappings.Add(_lineItemDataMapping);

                                                foreach (var _attrData in _bkgSvcAttDataGroup.lstAttributeData)
                                                {
                                                    BkgOrderLineItemDataUsedAttrb _lineItemDataUsedAttr = new BkgOrderLineItemDataUsedAttrb();
                                                    _lineItemDataUsedAttr.BkgOrderLineItemDataMapping = _lineItemDataMapping;
                                                    _lineItemDataUsedAttr.OLIDUA_BkgAttributeGroupMappingID = _attrData.AttributeGroupMappingID;
                                                    _lineItemDataUsedAttr.OLIDUA_AttributeValue = _attrData.AttributeValue;

                                                    _lineItemDataMapping.BkgOrderLineItemDataUsedAttrbs.Add(_lineItemDataUsedAttr);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }

                Int16? _supplementaTypeId = null;
                _bkgOrder.BOR_NeedFirstReview = this.GetPackageSettings(_lstPHMIds, out _supplementaTypeId);
                _bkgOrder.BOR_PackageSupplementalTypeID = _supplementaTypeId;

                #region Add BkgOrderEventHistory table data

                BkgOrderEventHistory _bkgOrderEventHistory = new BkgOrderEventHistory
                {
                    BkgOrder = _bkgOrder,
                    BOEH_OrderEventDetail = AppConsts.Applicant_Invitation_Completed,
                    BOEH_IsDeleted = false,
                    BOEH_CreatedByID = applicantOrderDataContract.OrganizationUserProfile.CreatedByID,
                    BOEH_CreatedOn = _creationDateTime,
                    BOEH_EventHistoryId = applicantOrderDataContract.OrderCreatedStatusId
                };

                // if applicant invite setting is set as transmitted.
                if (!applicantOrderDataContract.AdminOrderInprogressEventID.IsNullOrEmpty() && applicantOrderDataContract.AdminOrderInprogressEventID > AppConsts.NONE)
                {

                    BkgOrderEventHistory _bkgOrderEventHistoryforInprogress = new BkgOrderEventHistory
                    {
                        BkgOrder = _bkgOrder,
                        BOEH_OrderEventDetail = AppConsts.Bkg_Order_Draft_To_Inprogess,
                        BOEH_IsDeleted = false,
                        BOEH_CreatedByID = applicantOrderDataContract.OrganizationUserProfile.CreatedByID,
                        BOEH_CreatedOn = _creationDateTime,
                        BOEH_EventHistoryId = applicantOrderDataContract.AdminOrderInprogressEventID
                    };
                }

                //if on hold setting is yes, then add on hold event type in event history.
                if (!applicantOrderDataContract.AdminOrderOnHoldEventID.IsNullOrEmpty() && applicantOrderDataContract.AdminOrderOnHoldEventID > AppConsts.NONE)
                {

                    BkgOrderEventHistory _bkgOrderEventHistoryforInprogress = new BkgOrderEventHistory
                    {
                        BkgOrder = _bkgOrder,
                        BOEH_OrderEventDetail = AppConsts.Admin_Entry_Order_On_Hold,
                        BOEH_IsDeleted = false,
                        BOEH_CreatedByID = applicantOrderDataContract.OrganizationUserProfile.CreatedByID,
                        BOEH_CreatedOn = _creationDateTime,
                        BOEH_EventHistoryId = applicantOrderDataContract.AdminOrderOnHoldEventID
                    };
                }

                #endregion

                #region Add Dynamic custom form data
                if (!applicantOrderDataContract.lstBkgOrderData.IsNullOrEmpty())
                {
                    foreach (var bkgOrderData in applicantOrderDataContract.lstBkgOrderData)
                    {
                        CustomFormDataGroup _customFormDataGroup = new CustomFormDataGroup
                        {
                            //BkgOrder = _bkgOrder,
                            CFDG_BkgOrderID = _bkgOrder.BOR_ID,
                            CFDG_BkgSvcAttributeGroupID = bkgOrderData.BkgSvcAttributeGroupId,
                            CFDG_CustomFormID = bkgOrderData.CustomFormId,
                            CFDG_InstanceID = bkgOrderData.InstanceId,
                            CFDG_IsDeleted = false,
                            CFDG_CreatedBy = applicantOrderDataContract.OrganizationUserProfile.CreatedByID,
                            CFDG_CreatedOn = _creationDateTime
                        };


                        foreach (var customFormData in bkgOrderData.CustomFormData)
                        {
                            CustomFormOrderData _customFormOrderData = new CustomFormOrderData
                            {
                                CustomFormDataGroup = _customFormDataGroup,
                                CFOD_BkgAttributeGroupMappingID = customFormData.Key,
                                CFOD_Value = customFormData.Value,
                                CFOD_IsDeleted = false,
                                CFOD_ModifiedBy = applicantOrderDataContract.OrganizationUserProfile.CreatedByID,
                                CFOD_ModifiedOn = _creationDateTime
                            };

                            //UAT-2447
                            if (!bkgOrderData.CustomFormIntPhoneNumExtraData.IsNullOrEmpty() && bkgOrderData.CustomFormIntPhoneNumExtraData.ContainsKey(customFormData.Key)
                                && Convert.ToBoolean(bkgOrderData.CustomFormIntPhoneNumExtraData[customFormData.Key]))
                            {
                                CustomFormOrderExtraData _customFormOrderExtraData = new CustomFormOrderExtraData();
                                _customFormOrderExtraData.CustomFormOrderData = _customFormOrderData;
                                _customFormOrderExtraData.CFOED_IsInternationalPhone = Convert.ToBoolean(bkgOrderData.CustomFormIntPhoneNumExtraData[customFormData.Key]);
                                _customFormOrderExtraData.CFOED_IsDeleted = false;
                                _customFormOrderExtraData.CFOED_CreatedBy = applicantOrderDataContract.OrganizationUserProfile.CreatedByID;
                                _customFormOrderExtraData.CFOED_CreatedOn = _creationDateTime;
                            }

                        }

                        _dbContext.CustomFormDataGroups.AddObject(_customFormDataGroup);
                    }
                }

                #endregion

                // _dbContext.BkgOrders.AddObject(_bkgOrder);
            }
            #endregion

            _dbContext.SaveChanges();
            //[SS]:Admin Entry Portal
            //_dbContext.Refresh(RefreshMode.StoreWins, applicantOrder);

            //#region UAT-1185 get OrderID for each Compliance Packages
            //if (compliancePackages.IsNotNull() && compliancePackages.Count > AppConsts.NONE)
            //{
            //    foreach (OrderCartCompliancePackage cp in compliancePackages)
            //    {
            //        if (applicantOrder.DeptProgramPackageID.Equals(cp.DPP_Id))
            //        {
            //            cp.OrderId = applicantOrder.OrderID;
            //            cp.OrderNumber = applicantOrder.OrderNumber;
            //        }
            //        else
            //        {
            //            if (applicantOrder.OrderGroupOrderNavProp.IsNotNull() && applicantOrder.OrderGroupOrderNavProp.Count > AppConsts.NONE)
            //            {
            //                Order o = applicantOrder.OrderGroupOrderNavProp.FirstOrDefault(ord => ord.DeptProgramPackageID == cp.DPP_Id);
            //                if (o.IsNotNull())
            //                {
            //                    _dbContext.Refresh(RefreshMode.StoreWins, o);
            //                    cp.OrderId = o.OrderID;
            //                    cp.OrderNumber = o.OrderNumber;
            //                }
            //            }
            //        }
            //    }
            //}
            //#endregion

            #region UAT 1067 - Hierarchy for orders (background and screening) should display as the full hierarchy sleected during the order, not the node the package lives on.

            List<Int32> dpmIDS = new List<int>();
            dpmIDS.Add(Convert.ToInt32(applicantOrder.SelectedNodeID));
            #endregion

            #region Applicant Hierarchy Mapping and custom attribute value Entry
            List<ApplicantHierarchyMapping> _applicantHierarchyMappings = _dbContext.ApplicantHierarchyMappings.Where(ahm => dpmIDS.Contains(ahm.AHM_HierarchyNodeID) && ahm.AHM_OrganizationUserID == _organizationUserID && ahm.AHM_IsDeleted == false).ToList();

            Int32 _ahmId = 0;
            List<ApplicantHierarchyMapping> lstAppHierMapping;

            if (_applicantHierarchyMappings == null && _applicantHierarchyMappings.Count == AppConsts.NONE)
            {
                lstAppHierMapping = new List<ApplicantHierarchyMapping>();

                //Save id of compliance package node in case there was a compliance package in the order. Else it should be the id of the background package node. Or both 
                foreach (var dpmID in dpmIDS)
                {
                    ApplicantHierarchyMapping appHierMapping = new ApplicantHierarchyMapping();
                    appHierMapping.AHM_OrganizationUserID = _organizationUserID;
                    appHierMapping.AHM_HierarchyNodeID = Convert.ToInt32(dpmID);
                    appHierMapping.AHM_IsDeleted = false;
                    appHierMapping.AHM_CreatedByID = _createdById;
                    appHierMapping.AHM_CreatedOn = _creationDateTime;
                    _dbContext.ApplicantHierarchyMappings.AddObject(appHierMapping);
                    lstAppHierMapping.Add(appHierMapping);
                }

                _dbContext.SaveChanges();

                foreach (var attributeValues in applicantOrderDataContract.lstAttributeValues)
                {
                    _ahmId = lstAppHierMapping.FirstOrDefault(x => x.AHM_HierarchyNodeID == attributeValues.HierarchyNodeID).AHM_ID;
                    _dbContext.CustomAttributeValues.AddObject(new CustomAttributeValue
                    {
                        CAV_CustomAttributeMappingID = Convert.ToInt32(attributeValues.CAMId),
                        CAV_AttributeValue = attributeValues.CAValue,
                        CAV_IsDeleted = false,
                        CAV_CreatedByID = _createdById,
                        CAV_CreatedOn = _creationDateTime,
                        CAV_RecordID = _ahmId
                    });
                }
            }
            else
            {
                List<Int32> nodeIds = new List<int>();
                foreach (var appHierMapping in _applicantHierarchyMappings)
                {
                    _ahmId = appHierMapping.AHM_ID;
                    Int32 _hierarchyNodeId = appHierMapping.AHM_HierarchyNodeID;
                    nodeIds.Add(_hierarchyNodeId);
                    List<TypeCustomAttributes> tempCustomAttributes = applicantOrderDataContract.lstAttributeValues.IsNotNull() ? applicantOrderDataContract.lstAttributeValues.Where(x => x.HierarchyNodeID == _hierarchyNodeId).ToList() : new List<TypeCustomAttributes>();

                    foreach (var _customAttrValue in tempCustomAttributes)
                    {
                        if (_customAttrValue.CAVId == AppConsts.NONE)
                        {
                            _dbContext.CustomAttributeValues.AddObject(new CustomAttributeValue
                            {
                                CAV_IsDeleted = false,
                                CAV_CreatedByID = _createdById,
                                CAV_CreatedOn = _creationDateTime,
                                CAV_CustomAttributeMappingID = Convert.ToInt32(_customAttrValue.CAMId),
                                CAV_AttributeValue = _customAttrValue.CAValue,
                                CAV_RecordID = _ahmId
                            });
                        }
                        else
                        {
                            CustomAttributeValue _customAttributeValueToUpdate = _dbContext.CustomAttributeValues.Where(cav => cav.CAV_CustomAttributeValueID == _customAttrValue.CAVId).FirstOrDefault();

                            if (_customAttributeValueToUpdate.IsNotNull())
                            {
                                _customAttributeValueToUpdate.CAV_ModifiedByID = _createdById;
                                _customAttributeValueToUpdate.CAV_ModifiedOn = _creationDateTime;
                                _customAttributeValueToUpdate.CAV_AttributeValue = _customAttrValue.CAValue;
                            }
                        }
                    }
                }

                nodeIds = dpmIDS.Where(x => !nodeIds.Contains(x)).ToList();

                if (nodeIds != null && nodeIds.Count > 0)
                {
                    lstAppHierMapping = new List<ApplicantHierarchyMapping>();
                    //Save id of compliance package node in case there was a compliance package in the order. Else it should be the id of the background package node. Or both 
                    foreach (var dpmID in nodeIds)
                    {
                        ApplicantHierarchyMapping appHierMapping = new ApplicantHierarchyMapping();
                        appHierMapping.AHM_OrganizationUserID = _organizationUserID;
                        appHierMapping.AHM_HierarchyNodeID = Convert.ToInt32(dpmID);
                        appHierMapping.AHM_IsDeleted = false;
                        appHierMapping.AHM_CreatedByID = _createdById;
                        appHierMapping.AHM_CreatedOn = _creationDateTime;
                        _dbContext.ApplicantHierarchyMappings.AddObject(appHierMapping);
                        lstAppHierMapping.Add(appHierMapping);
                    }

                    _dbContext.SaveChanges();

                    foreach (var dpmID in nodeIds)
                    {
                        List<TypeCustomAttributes> tempCustomAttributes = applicantOrderDataContract.lstAttributeValues.IsNotNull() ? applicantOrderDataContract.lstAttributeValues.Where(x => x.HierarchyNodeID == dpmID).ToList() : new List<TypeCustomAttributes>();

                        foreach (var attributeValues in tempCustomAttributes)
                        {
                            _ahmId = lstAppHierMapping.FirstOrDefault(x => x.AHM_HierarchyNodeID == attributeValues.HierarchyNodeID).AHM_ID;
                            _dbContext.CustomAttributeValues.AddObject(new CustomAttributeValue
                            {
                                CAV_CustomAttributeMappingID = Convert.ToInt32(attributeValues.CAMId),
                                CAV_AttributeValue = attributeValues.CAValue,
                                CAV_IsDeleted = false,
                                CAV_CreatedByID = _createdById,
                                CAV_CreatedOn = _creationDateTime,
                                CAV_RecordID = _ahmId
                            });
                        }
                    }
                }
            }

            _dbContext.SaveChanges();
            #endregion

            var _orderStatusId = 0;

            var _bkgPkgTypeId = applicantOrderDataContract.lstOrderPackageTypes.Where(opt => opt.OPT_Code == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;
            var _compliancePkgTypeId = applicantOrderDataContract.lstOrderPackageTypes.Where(opt => opt.OPT_Code == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue()).FirstOrDefault().OPT_ID;

            foreach (var poId in applicantOrderDataContract.lstGroupedData)
            {
                bool mainOrderPaymentDetailEntry = true;
                //[Commenetd: for Admin Data entry]
                //if (poId.TotalAmount > AppConsts.NONE)
                //{
                //    if (poId.PaymentModeCode.Contains("Additional") == true)
                //    {
                //        var paymentCode = poId.PaymentModeCode.Split('-')[0];
                //        _orderStatusId = GetOrderStatusId(paymentCode);
                //    }
                //    else
                //        _orderStatusId = GetOrderStatusId(poId.PaymentModeCode);
                //}
                //else
                //{
                //    _orderStatusId = GetOrderStatusCode(ApplicantOrderStatus.Paid.GetStringValue());
                //}
                _orderStatusId = GetOrderStatusCode(ApplicantOrderStatus.Paid.GetStringValue());

                //#region UAT-1185 Adjust amount of main order payment details if it has extra compliance too
                decimal adjustedAmount = 0;
                //List<Int32> childOrderIds = new List<int>();
                //if (!poId.lstPackages.IsNullOrEmpty())   //Check in UAT-3268
                //{
                //    foreach (var pkg in poId.lstPackages)
                //    {
                //        if (!pkg.Value)
                //        {
                //            var pkgId = Convert.ToInt32(pkg.Key.Split('_')[0]);
                //            if (pkgId == applicantOrder.DeptProgramPackage.DPP_CompliancePackageID)
                //                mainOrderPaymentDetailEntry = true;
                //            else
                //            {
                //                OrderCartCompliancePackage cp = compliancePackages.First(ocp => ocp.CompliancePackageID == pkgId && ocp.OrderId > AppConsts.NONE);
                //                if (pkgId == cp.CompliancePackageID)
                //                {
                //                    adjustedAmount += cp.GrandTotal.IsNull() ? AppConsts.NONE : Convert.ToDecimal(cp.GrandTotal);
                //                    childOrderIds.Add(cp.OrderId);
                //                }
                //            }
                //        }
                //        else
                //            mainOrderPaymentDetailEntry = true;
                //    }
                //}
                //else
                //{
                //    mainOrderPaymentDetailEntry = true;  //UAT-3268
                //}

                //#endregion
                // Store Invoice Number and Payment Mode
                OrderPaymentDetail paymentDetails = null;
                //Int32 firstExtraOrderID = 0;
                if (mainOrderPaymentDetailEntry)
                {
                    List<OrderPaymentDetail> lstOrderPaymentDetail = _dbContext.OrderPaymentDetails.Where(cnd => cnd.OPD_OrderID == applicantOrder.OrderID && !cnd.OPD_IsDeleted).ToList();
                    lstOrderPaymentDetail.ForEach(opd =>
                    {
                        opd.OPD_IsDeleted = true;
                        opd.OPD_ModifiedByID = _organizationUserID;
                        opd.OPD_ModifiedOn = DateTime.Now;
                    });

                    _invoiceNumber = GenerateInvoiceNumber(applicantOrder.OrderID, applicantOrderDataContract.TenantId, false, null);
                    paymentDetails = AddOnlinePaymentTransaction(applicantOrder, _creationDateTime, _invoiceNumber, poId.TotalAmount, poId.PaymentModeId, _orderStatusId, applicantOrder.CreatedByID, adjustedAmount);

                    int cmpPkgId = 0;
                    if (compliancePackages.IsNotNull() && compliancePackages.Count > AppConsts.NONE)
                    {
                        var compliancePackage = compliancePackages.Find(cp => cp.OrderId.Equals(applicantOrder.OrderID));
                        if (compliancePackage.IsNotNull() && compliancePackage.CompliancePackageID > AppConsts.NONE)
                            cmpPkgId = compliancePackage.CompliancePackageID;
                    }

                    AddOrderPaymentPackageDetail(poId, _lstBkgOrderPkg, paymentDetails, _bkgPkgTypeId, _compliancePkgTypeId, applicantOrder.CreatedByID, _creationDateTime, cmpPkgId);
                    //}
                    //else if (childOrderIds.Count > 0)
                    //{
                    //    firstExtraOrderID = childOrderIds[0];
                    //    childOrderIds.RemoveAt(0);

                    //    _invoiceNumber = GenerateInvoiceNumber(firstExtraOrderID, applicantOrderDataContract.TenantId, false, childOrderIds);

                }
                _dicInvoiceNumbers.Add(poId.PaymentModeCode, _invoiceNumber);

                #region UAT-1185 generate order payment details entries
                //if (compliancePackages.IsNotNull() && compliancePackages.Count > AppConsts.NONE && applicantOrder.OrderGroupOrderNavProp.IsNotNull() && applicantOrder.OrderGroupOrderNavProp.Count > AppConsts.NONE)
                //{
                //    foreach (var pkg in poId.lstPackages)
                //    {
                //        if (!pkg.Value)
                //        {
                //            var pkgId = Convert.ToInt32(pkg.Key.Split('_')[0]);

                //            OrderCartCompliancePackage cp = compliancePackages.Find(p => p.CompliancePackageID.Equals(pkgId));
                //            if (cp.IsNotNull())
                //            {
                //                Order extraOrder = applicantOrder.OrderGroupOrderNavProp.FirstOrDefault(eo => eo.OrderID.Equals(cp.OrderId));
                //                if (extraOrder.IsNotNull())
                //                {
                //                    if (paymentDetails.IsNull())
                //                    {
                //                        decimal cpAdjustedAmount = adjustedAmount - (cp.GrandTotal.IsNull() ? AppConsts.NONE : Convert.ToDecimal(cp.GrandTotal));
                //                        paymentDetails = AddOnlinePaymentTransaction(extraOrder, _creationDateTime, _invoiceNumber,
                //                            poId.TotalAmount, poId.PaymentModeId, _orderStatusId, applicantOrder.CreatedByID, cpAdjustedAmount);

                //                        AddOrderPaymentPackageDetail(poId, null, paymentDetails, _bkgPkgTypeId, _compliancePkgTypeId, applicantOrder.CreatedByID, _creationDateTime, cp.CompliancePackageID);
                //                    }
                //                    else
                //                    {
                //                        OrderPaymentDetail opd = AddOrderPaymentDetail(extraOrder, paymentDetails.OnlinePaymentTransaction, _creationDateTime, (cp.GrandTotal.IsNull() ? AppConsts.NONE : Convert.ToDecimal(cp.GrandTotal)), poId.PaymentModeId, _orderStatusId, applicantOrder.CreatedByID);
                //                        AddOrderPaymentPackageDetail(poId, null, opd, _bkgPkgTypeId, _compliancePkgTypeId, applicantOrder.CreatedByID, _creationDateTime, cp.CompliancePackageID);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                _dbContext.SaveChanges();
                #endregion

            }

            orderId = applicantOrder.OrderID;
            return _dicInvoiceNumbers;
        }
        public String GetPaymentOptionCodeById(Int32 paymentOptionId)
        {
            return _dbContext.lkpPaymentOptions.Where(payOptns => payOptns.PaymentOptionID == paymentOptionId && !payOptns.IsDeleted).FirstOrDefault().Code;
        }
        private Int16? GetArchiveStateIDByCode(ArchiveState archiveState)
        {
            String archiveStateCode = archiveState.GetStringValue();
            lkpArchiveState lkpArchiveState = _dbContext.lkpArchiveStates.FirstOrDefault(x => x.AS_Code == archiveStateCode && x.AS_IsDeleted == false);
            if (lkpArchiveState.IsNotNull())
            {
                return lkpArchiveState.AS_ID;
            }
            return null;
        }
        public Order GetPreviousOrderDetail(Int32? orderID)
        {
            return _dbContext.Orders.Where(cond => cond.OrderID == orderID).FirstOrDefault();
        }
        //public PackageSubscription GetPackageSubscriptionByPackageID(Int32 compliancePackageID, Int32 organizationUserID)
        //{
        //    return _dbContext.PackageSubscriptions.FirstOrDefault(ps => ps.CompliancePackageID == compliancePackageID
        //   && ps.OrganizationUserID == organizationUserID && !ps.IsDeleted);
        //}
        /// <summary>
        /// Get the instanceId, based on the SvcAttributeGroup
        /// </summary>
        /// <param name="orderDataContract"></param>
        /// <param name="attributeGrpId"></param>
        /// <param name="uniqueIdentifier"></param>
        /// <returns></returns>        
        private Int32 GetInstanceId(ApplicantOrderDataContract orderDataContract, Int32 attributeGrpId, Int32 sequenceId)
        {
            BkgSvcAttributeGroup _attributeGrp = orderDataContract.lstSvcAttributeGrps.Where(attrGrp => attrGrp.BSAD_ID == attributeGrpId && !attrGrp.BSAD_IsDeleted).FirstOrDefault();

            if (_attributeGrp.IsNullOrEmpty())
                return AppConsts.MINUS_ONE;

            if (_attributeGrp.BSAD_Name == SvcAttributeGroups.PERSONAL_INFORMATION.GetStringValue())
            {
                return orderDataContract.OrganizationUserProfile.OrganizationUserProfileID;
            }
            else if (_attributeGrp.BSAD_Name == SvcAttributeGroups.RESIDENTIAL_HISTORY.GetStringValue())
            {
                return orderDataContract.lstResidentialHistoryProfile.Where(rhp => rhp.RHIP_SequenceOrder == sequenceId).FirstOrDefault().RHIP_ID;
            }
            else if (_attributeGrp.BSAD_Name == SvcAttributeGroups.PERSONAL_ALIAS.GetStringValue())
            {
                return orderDataContract.lstPersonAliasProfile.Where(pap => pap.PAP_SequenceId == sequenceId).FirstOrDefault().PAP_ID;
            }

            return sequenceId;
        }
        /// <summary>
        /// Gets the package settings of :
        /// 1. NeedsFirstReview - Will be False if no package is having True or any value set for it or supplementalTypeId is Null
        /// 2. PackageSupplementalType
        /// </summary>
        /// <param name="_lstPHMIds"></param>
        /// <param name="supplementalTypeId"></param>
        /// <returns></returns>
        private Boolean GetPackageSettings(List<Int32> _lstPHMIds, out Int16? supplementalTypeId)
        {
            supplementalTypeId = null;
            Boolean _needsFirstReview = false;
            List<BkgPackageHierarchyMapping> _lstHierarchyMapping = _dbContext.BkgPackageHierarchyMappings.Include("lkpPackageSupplementalType").Where(bphm => _lstPHMIds.Contains(bphm.BPHM_ID) && !bphm.BPHM_IsDeleted && bphm.BPHM_IsActive).ToList();

            _needsFirstReview = _lstHierarchyMapping.Any(bphm => bphm.BPHM_NeedFirstReview == true && !bphm.BPHM_NeedFirstReview.IsNullOrEmpty() && bphm.BPHM_IsActive && !bphm.BPHM_IsDeleted);

            foreach (var hiearchyMapping in _lstHierarchyMapping)
            {
                if (!hiearchyMapping.BPHM_PkgSupplementalTypeID.IsNullOrEmpty())
                {
                    if (hiearchyMapping.lkpPackageSupplementalType.PST_Code == BkgPackageSupplementalType.ANY.GetStringValue())
                    {
                        supplementalTypeId = hiearchyMapping.BPHM_PkgSupplementalTypeID;
                        break;
                    }
                    else if (hiearchyMapping.lkpPackageSupplementalType.PST_Code == BkgPackageSupplementalType.FLAGGED.GetStringValue())
                    {
                        supplementalTypeId = hiearchyMapping.BPHM_PkgSupplementalTypeID;
                        break;
                    }
                    else if (hiearchyMapping.lkpPackageSupplementalType.PST_Code == BkgPackageSupplementalType.NONE.GetStringValue())
                    {
                        supplementalTypeId = hiearchyMapping.BPHM_PkgSupplementalTypeID;
                        break;
                    }
                }
            }
            if (supplementalTypeId.IsNullOrEmpty())
                _needsFirstReview = false;
            return _needsFirstReview;
        }
        public Boolean SaveUpdateApplicantUserGroupCustomAttribute(List<ApplicantUserGroupMapping> lstApplicantUserGroupMapping_Added, Int32 loggedInUserID)
        {
            if (!lstApplicantUserGroupMapping_Added.IsNullOrEmpty())
            {
                foreach (ApplicantUserGroupMapping applicantUserGroupMapping in lstApplicantUserGroupMapping_Added)
                {
                    _dbContext.ApplicantUserGroupMappings.AddObject(applicantUserGroupMapping);
                }
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }
        public Boolean SaveApplicantEsignatureDocument(Int32 applicantDisclaimerDocumentId, List<Int32?> applicantDisclosureDocumentIds, Int32 orderId, Int32 orgUserProfileId, Int32 currentLoggedInUserId, String orderNumber)
        {

            String description = String.Empty;
            Boolean result = false;
            if (applicantDisclaimerDocumentId > 0 && orderId.IsNotNull())
            {
                description = String.Format("E signed disclaimer document for order number {0} ", orderNumber);
                Int16 recordTypeId = _dbContext.lkpRecordTypes.Where(obj => obj.Code == "AAAB" && !obj.IsDeleted).FirstOrDefault().RecordTypeID;
                UpdateSignedDocument(applicantDisclaimerDocumentId, description, orderId, currentLoggedInUserId, recordTypeId);
                result = true;
            }
            if (applicantDisclosureDocumentIds != null && applicantDisclosureDocumentIds.Count > 0 && orderId > 0 && orgUserProfileId > 0)
            {
                foreach (Int32 applicantDisclosureDocumentId in applicantDisclosureDocumentIds)
                {
                    description = String.Format("E signed discloser document for order number {0} ", orderNumber);
                    Int16 recordTypeId = _dbContext.lkpRecordTypes.Where(obj => obj.Code == "AAAB" && !obj.IsDeleted).FirstOrDefault().RecordTypeID;
                    UpdateSignedDocument(applicantDisclosureDocumentId, description, orderId, currentLoggedInUserId, recordTypeId, orgUserProfileId);
                    result = true;
                }
            }
            return result;
        }
        private Boolean UpdateSignedDocument(Int32 applicantdocumentId, String description, Int32 orderId, Int32 currentLoggedInUserId, Int16 recordTypeId, Int32 orgUserProfileId = 0)
        {
            ApplicantDocument applicantDocument = _dbContext.ApplicantDocuments.Where(obj => obj.ApplicantDocumentID == applicantdocumentId).FirstOrDefault();

            if (applicantDocument.IsNotNull())
            {
                applicantDocument.IsDeleted = false;
                applicantDocument.ModifiedOn = DateTime.Now;
                applicantDocument.ModifiedByID = currentLoggedInUserId;
                applicantDocument.Description = description;

                GenericDocumentMapping genericDocumentMapping = null;

                if (applicantDocument.lkpDocumentType.DMT_Code == DocumentType.DisclaimerDocument.GetStringValue() || applicantDocument.lkpDocumentType.DMT_Code == DocumentType.Disclosure_n_Release.GetStringValue())
                {
                    genericDocumentMapping = new GenericDocumentMapping();
                    genericDocumentMapping.GDM_RecordID = orderId;
                    genericDocumentMapping.GDM_RecordTypeID = recordTypeId;
                    genericDocumentMapping.GDM_ApplicantDocumentID = applicantdocumentId;
                    genericDocumentMapping.GDM_IsDeleted = false;
                    genericDocumentMapping.GDM_CreatedOn = DateTime.Now;
                    genericDocumentMapping.GDM_CreatedBy = currentLoggedInUserId;
                    _dbContext.GenericDocumentMappings.AddObject(genericDocumentMapping);
                }


                //Code commented in UAT-4755 : Req-8
                //if (applicantDocument.lkpDocumentType.DMT_Code == DocumentType.DisclaimerDocument.GetStringValue() || applicantDocument.lkpDocumentType.DMT_Code == DocumentType.DisclosureDocument.GetStringValue())
                //{
                //    genericDocumentMapping = new GenericDocumentMapping();
                //    genericDocumentMapping.GDM_RecordID = orderId;
                //    genericDocumentMapping.GDM_RecordTypeID = recordTypeId;
                //    genericDocumentMapping.GDM_ApplicantDocumentID = applicantdocumentId;
                //    genericDocumentMapping.GDM_IsDeleted = false;
                //    genericDocumentMapping.GDM_CreatedOn = DateTime.Now;
                //    genericDocumentMapping.GDM_CreatedBy = currentLoggedInUserId;
                //    _dbContext.GenericDocumentMappings.AddObject(genericDocumentMapping);
                //}
                //else
                //{
                //    if (applicantDocument.lkpDocumentType.DMT_Code == DocumentType.Disclosure_n_Release.GetStringValue() && orgUserProfileId > 0)
                //    {
                //        Int16 bkgRecordTypeId = _dbContext.lkpRecordTypes.Where(obj => obj.Code == "AAAC" && !obj.IsDeleted).FirstOrDefault().RecordTypeID;
                //        genericDocumentMapping = new GenericDocumentMapping();
                //        genericDocumentMapping.GDM_RecordID = orgUserProfileId;
                //        genericDocumentMapping.GDM_RecordTypeID = bkgRecordTypeId;
                //        genericDocumentMapping.GDM_ApplicantDocumentID = applicantdocumentId;
                //        genericDocumentMapping.GDM_IsDeleted = false;
                //        genericDocumentMapping.GDM_CreatedOn = DateTime.Now;
                //        genericDocumentMapping.GDM_CreatedBy = currentLoggedInUserId;
                //        _dbContext.GenericDocumentMappings.AddObject(genericDocumentMapping);
                //    }
                //}

                if (_dbContext.SaveChanges() > 0)
                    return true;
            }

            return false;
        }
        public List<ApplicantDocument> UpdateApplicantAdditionalEsignatureDocument(List<Int32?> applicantAdditionalDocumentId, Int32 orderId, Int32 orgUserProfileId,
                                                                                 Int32 currentLoggedInUserId, Boolean needToSaveMapping, Int16 recordTypeId,
                                                                                 Int16 dataEntryDocCompletedStatusID, List<Int32?> additionalDocumentSendToStudent, List<SystemDocBkgSvcMapping> lstSystemDocBkgSvcMapping = null)
        {

            List<ApplicantDocument> applicantDocumentList = new List<ApplicantDocument>();
            if (!applicantAdditionalDocumentId.IsNullOrEmpty())
            {
                applicantDocumentList = _dbContext.ApplicantDocuments.Where(obj => applicantAdditionalDocumentId.Contains(obj.ApplicantDocumentID)).ToList();

                if (!applicantDocumentList.IsNullOrEmpty())
                {
                    applicantDocumentList.ForEach(appDoc =>
                    {
                        appDoc.IsDeleted = false;
                        appDoc.ModifiedOn = DateTime.Now;
                        appDoc.ModifiedByID = currentLoggedInUserId;
                        appDoc.Description = "E Signed Additional Document";
                        if (appDoc.IsSearchableOnly == false && needToSaveMapping)
                        {
                            appDoc.DataEntryDocumentStatusID = dataEntryDocCompletedStatusID;
                        }
                        //UAT-1835, NYU Migration 3 of 3: Automatic Interval Searching.
                        GenericDocumentMapping genericDocumentMapping = new GenericDocumentMapping();
                        genericDocumentMapping.GDM_RecordID = orderId;
                        genericDocumentMapping.GDM_RecordTypeID = recordTypeId;
                        genericDocumentMapping.GDM_ApplicantDocumentID = appDoc.ApplicantDocumentID;
                        //UAT-1759:Create the ability to mark an "Additional Documents" that the students complete in the order flow as "Send to student"
                        Boolean docNeedToSendToStudent = additionalDocumentSendToStudent.IsNullOrEmpty() ? false : additionalDocumentSendToStudent.Contains(appDoc.ApplicantDocumentID);
                        genericDocumentMapping.GDM_SendToStudent = docNeedToSendToStudent;

                        if (appDoc.IsSearchableOnly == false && needToSaveMapping)
                        {
                            genericDocumentMapping.GDM_IsDeleted = false;
                        }
                        else
                        {
                            genericDocumentMapping.GDM_IsDeleted = true;
                        }
                        genericDocumentMapping.GDM_CreatedOn = DateTime.Now;
                        genericDocumentMapping.GDM_CreatedBy = currentLoggedInUserId;

                        _dbContext.GenericDocumentMappings.AddObject(genericDocumentMapping);
                        //UAT-3745
                        if (!lstSystemDocBkgSvcMapping.IsNullOrEmpty())
                        {
                            SystemDocBkgSvcMapping systemDocBkgSvcMapping = lstSystemDocBkgSvcMapping.Where(con => con.ApplicantDocumentID == appDoc.ApplicantDocumentID).FirstOrDefault();
                            if (!systemDocBkgSvcMapping.IsNullOrEmpty())
                            {
                                GenericDocumentMapping genericDocumentMappingWithBkgSvc = new GenericDocumentMapping();

                                genericDocumentMappingWithBkgSvc.GDM_RecordID = systemDocBkgSvcMapping.BkgServiceID;
                                genericDocumentMappingWithBkgSvc.GDM_RecordTypeID = systemDocBkgSvcMapping.RecordTypeID;
                                genericDocumentMappingWithBkgSvc.GDM_ApplicantDocumentID = systemDocBkgSvcMapping.ApplicantDocumentID;
                                genericDocumentMappingWithBkgSvc.GDM_SystemDocumentID = systemDocBkgSvcMapping.SystemDocumentID;

                                Boolean docNeedToSendToStudentInBkgSvc = additionalDocumentSendToStudent.IsNullOrEmpty() ? false : additionalDocumentSendToStudent.Contains(appDoc.ApplicantDocumentID);

                                genericDocumentMappingWithBkgSvc.GDM_IsDeleted = false;
                                genericDocumentMappingWithBkgSvc.GDM_CreatedOn = DateTime.Now;
                                genericDocumentMappingWithBkgSvc.GDM_CreatedBy = currentLoggedInUserId;

                                _dbContext.GenericDocumentMappings.AddObject(genericDocumentMappingWithBkgSvc);
                            }
                        }
                    });
                }
            }
            if (_dbContext.SaveChanges() > 0)
                return applicantDocumentList;
            return null;

        }

        public List<ApplicantDocument> UpdateAdditionalDocumentStatusForApproveOrder(Int32 orderId, Int32 currentloggedInUserId, String docTypeCode,
                                                                                     Int16 dataEntryDocNewStatusID, String recordTypeCode, Int32 orgUserId)
        {
            List<GenericDocumentMapping> lstDocumentMappingToUpdate = _dbContext.GenericDocumentMappings.Where(cnd => !cnd.GDM_IsDeleted && cnd.ApplicantDocument.lkpDocumentType.DMT_Code == docTypeCode && !cnd.ApplicantDocument.IsDeleted
                                                                                                       && cnd.ApplicantDocument.OrganizationUserID == orgUserId).ToList();
            //UAT-3745 
            String bkgServiceTypeCode = RecordType.Background_Service.GetStringValue();
            lstDocumentMappingToUpdate = lstDocumentMappingToUpdate.Where(con => con.lkpRecordType.Code != bkgServiceTypeCode).ToList();

            List<ApplicantDocument> listUpdatedApplicantDocument = new List<ApplicantDocument>();
            if (!lstDocumentMappingToUpdate.IsNullOrEmpty())
            {
                lstDocumentMappingToUpdate.ForEach(map =>
                {
                    map.GDM_IsDeleted = true;
                    map.GDM_ModifiedOn = DateTime.Now;
                    map.GDM_ModifiedBy = currentloggedInUserId;

                    map.ApplicantDocument.DataEntryDocumentStatusID = dataEntryDocNewStatusID;
                    map.ApplicantDocument.ModifiedByID = currentloggedInUserId;
                    map.ApplicantDocument.ModifiedOn = DateTime.Now;
                    listUpdatedApplicantDocument.Add(map.ApplicantDocument);
                });

                if (_dbContext.SaveChanges() > 0)
                    return listUpdatedApplicantDocument;
            }
            return new List<ApplicantDocument>();
        }



        public void AddUpdateAdminEntryUserData(Entity.OrganizationUser organizationUserData, OrganizationUserProfile UserProfile)
        {

            OrganizationUser organizationUser = _dbContext.OrganizationUsers.FirstOrDefault(x => x.OrganizationUserID == organizationUserData.OrganizationUserID);// SecurityManager.GetOrganizationUserDetailByOrganizationUserId(UserProfile.OrganizationUserID);
            if (!organizationUser.IsNullOrEmpty())
            {
                organizationUser.UserID = organizationUserData.UserID;
                organizationUser.OrganizationID = organizationUserData.OrganizationID;
                organizationUser.BillingAddressID = organizationUserData.BillingAddressID;
                organizationUser.ContactID = organizationUserData.ContactID;
                organizationUser.UserTypeID = organizationUserData.UserTypeID;
                organizationUser.DepartmentID = organizationUserData.DepartmentID;
                organizationUser.SysXBlockID = organizationUserData.SysXBlockID;
                organizationUser.AddressHandleID = organizationUserData.AddressHandleID;
                organizationUser.FirstName = organizationUserData.FirstName;
                organizationUser.LastName = organizationUserData.LastName;
                organizationUser.IsMessagingUser = organizationUserData.IsMessagingUser;
                organizationUser.ModifiedByID = organizationUserData.OrganizationUserID;
                organizationUser.ModifiedOn = DateTime.Now;
                organizationUser.PhotoName = organizationUserData.PhotoName;
                organizationUser.OriginalPhotoName = organizationUserData.OriginalPhotoName;
                organizationUser.DOB = organizationUserData.DOB;
                organizationUser.SSN = organizationUserData.SSN;
                organizationUser.Gender = organizationUserData.Gender;
                organizationUser.PhoneNumber = organizationUserData.PhoneNumber;
                organizationUser.MiddleName = organizationUserData.MiddleName;
                organizationUser.Alias1 = organizationUserData.Alias1;
                organizationUser.Alias2 = organizationUserData.Alias2;
                organizationUser.Alias3 = organizationUserData.Alias3;
                organizationUser.PrimaryEmailAddress = organizationUserData.PrimaryEmailAddress;
                organizationUser.SecondaryEmailAddress = organizationUserData.SecondaryEmailAddress;
                organizationUser.SecondaryPhone = organizationUserData.SecondaryPhone;
                organizationUser.IsInternationalPhoneNumber = organizationUserData.IsInternationalPhoneNumber;
                organizationUser.IsInternationalSecondaryPhone = organizationUserData.IsInternationalSecondaryPhone;

                #region Add / Update Alias Data Here

                //if (!organizationUserData.IsNullOrEmpty())
                //{
                //    foreach (var personAlias in organizationUserData.PersonAlias.ToList())
                //    {
                //        //if entry already exists//Update 
                //        PersonAlia currentPersonAlias = organizationUser.PersonAlias.Where(x => x.PA_ID == personAlias.PA_ID).FirstOrDefault();

                //        if (!currentPersonAlias.IsNullOrEmpty())
                //        {
                //            currentPersonAlias.PA_FirstName = personAlias.PA_FirstName;
                //            currentPersonAlias.PA_LastName = personAlias.PA_LastName;
                //            currentPersonAlias.PA_MiddleName = personAlias.PA_MiddleName;
                //            currentPersonAlias.PA_ModifiedBy = organizationUser.OrganizationUserID;
                //            currentPersonAlias.PA_ModifiedOn = DateTime.Now;

                //            #region PersonAliasSuffixMappings update
                //            // personalias update functionality
                //            // Get PersonAliasSuffixMappings on the basis of PersonAliasID --
                //            //If It Exists // then Update
                //            //Else Add It
                //            if (!currentPersonAlias.PersonAliasSuffixMappings.Where(con => !con.PASM_IsDeleted).ToList().IsNullOrEmpty())
                //            {
                //                PersonAliasSuffixMapping currentPersonAliasSuffix = currentPersonAlias.PersonAliasSuffixMappings.FirstOrDefault(x => !x.PASM_IsDeleted);
                //                currentPersonAliasSuffix.PASM_SuffixId = Convert.ToInt32(currentPersonAliasSuffix.PASM_SuffixId);
                //                currentPersonAliasSuffix.PASM_ModifiedBy = organizationUser.OrganizationUserID;
                //                currentPersonAliasSuffix.PASM_ModifiedOn = DateTime.Now;
                //            }
                //            else
                //            {
                //                PersonAliasSuffixMapping newPersonAliasSuffix = new PersonAliasSuffixMapping();
                //                newPersonAliasSuffix.PASM_ID = personAlias.PersonAliasSuffixMappings.FirstOrDefault().PASM_ID;
                //                newPersonAliasSuffix.PASM_CreatedBy = organizationUser.OrganizationUserID;
                //                newPersonAliasSuffix.PASM_CreatedOn = DateTime.Now;
                //                newPersonAliasSuffix.PASM_SuffixId = Convert.ToInt32(personAlias.PersonAliasSuffixMappings.FirstOrDefault().PASM_SuffixId);
                //            }

                //            #endregion
                //        }

                //        else
                //        {
                //            //add entries into table if doesnt exist
                //            PersonAlia newPersonAlias = new PersonAlia();
                //            newPersonAlias.PA_ID = personAlias.PA_ID;
                //            newPersonAlias.PA_FirstName = personAlias.PA_FirstName;
                //            newPersonAlias.PA_MiddleName = personAlias.PA_MiddleName;
                //            newPersonAlias.PA_LastName = personAlias.PA_LastName;
                //            newPersonAlias.PA_CreatedBy = organizationUser.OrganizationUserID;
                //            newPersonAlias.PA_CreatedOn = DateTime.Now;

                //            #region // Add PersonAliasSuffixMappings Here//
                //            PersonAliasSuffixMapping newPersonAliasSuffix = new PersonAliasSuffixMapping();
                //            newPersonAliasSuffix.PASM_ID = personAlias.PersonAliasSuffixMappings.FirstOrDefault().PASM_ID;
                //            newPersonAliasSuffix.PASM_CreatedBy = organizationUser.OrganizationUserID;
                //            newPersonAliasSuffix.PASM_CreatedOn = DateTime.Now;
                //            newPersonAliasSuffix.PASM_SuffixId = Convert.ToInt32(personAlias.PersonAliasSuffixMappings.FirstOrDefault().PASM_SuffixId);

                //            newPersonAlias.PersonAliasSuffixMappings.Add(newPersonAliasSuffix);
                //            organizationUser.PersonAlias.Add(newPersonAlias);
                //            #endregion
                //        }
                //    }
                //}
                ////Delete Existing aliases from table
                //else
                //{
                //    List<PersonAlia> lstExistingPersonAliases = organizationUser.PersonAlias.Where(con => !con.PA_IsDeleted).ToList();
                //    foreach (var personAlias in lstExistingPersonAliases)
                //    {
                //        personAlias.PA_IsDeleted = true;
                //        personAlias.PA_ModifiedBy = organizationUser.OrganizationUserID;
                //        personAlias.PA_ModifiedOn = DateTime.Now;
                //    }
                //}
                #endregion

                #region New Implementation-- Add/Upadte Alias Data here
                if (!organizationUserData.IsNullOrEmpty())
                {
                    List<PersonAlia> lstExistingPersonAliases = organizationUser.PersonAlias.Where(con => !con.PA_IsDeleted).ToList();
                    foreach (var personAlias in lstExistingPersonAliases)
                    {
                        personAlias.PA_IsDeleted = true;
                        personAlias.PA_ModifiedBy = organizationUser.OrganizationUserID;
                        personAlias.PA_ModifiedOn = DateTime.Now;
                    }

                    //foreach (var personAlias in organizationUserData.PersonAlias.ToList())
                    foreach (var personAlias in organizationUserData.PersonAlias.Where(con => !con.PA_IsDeleted).ToList())
                    {
                        //if entry already exists//Update 
                        //if (personAlias.PA_IsDeleted == false)
                        //{
                        //add entries into table if doesnt exist
                        PersonAlia newPersonAlias = new PersonAlia();
                        newPersonAlias.PA_ID = personAlias.PA_ID;
                        newPersonAlias.PA_FirstName = personAlias.PA_FirstName;
                        newPersonAlias.PA_MiddleName = personAlias.PA_MiddleName;
                        newPersonAlias.PA_LastName = personAlias.PA_LastName;
                        newPersonAlias.PA_CreatedBy = organizationUser.OrganizationUserID;
                        newPersonAlias.PA_CreatedOn = DateTime.Now;

                        #region // Add PersonAliasSuffixMappings Here//

                        if (!personAlias.PersonAliasSuffixMappings.IsNullOrEmpty() && !personAlias.PersonAliasSuffixMappings.FirstOrDefault().PASM_SuffixId.IsNullOrEmpty()
                            && personAlias.PersonAliasSuffixMappings.FirstOrDefault().PASM_SuffixId > AppConsts.NONE)
                        {
                            PersonAliasSuffixMapping newPersonAliasSuffix = new PersonAliasSuffixMapping();
                            newPersonAliasSuffix.PASM_ID = personAlias.PersonAliasSuffixMappings.FirstOrDefault().PASM_ID;
                            newPersonAliasSuffix.PASM_CreatedBy = organizationUser.OrganizationUserID;
                            newPersonAliasSuffix.PASM_CreatedOn = DateTime.Now;
                            newPersonAliasSuffix.PASM_SuffixId = Convert.ToInt32(personAlias.PersonAliasSuffixMappings.FirstOrDefault().PASM_SuffixId);

                            newPersonAlias.PersonAliasSuffixMappings.Add(newPersonAliasSuffix);
                        }
                        organizationUser.PersonAlias.Add(newPersonAlias);
                        #endregion
                        //}
                    }
                }
                #endregion

                #region Add/Update User Profile
                if (!organizationUserData.OrganizationUserProfiles.IsNullOrEmpty() && organizationUserData.OrganizationUserProfiles.Count > AppConsts.NONE)
                {
                    //foreach (var UserProfile in organizationUserData.OrganizationUserProfiles)
                    //{

                    #region Old- Implementation
                    //var OrganizationUserProfile = organizationUserData.OrganizationUserProfiles.FirstOrDefault(x => x.OrganizationUserProfileID == UserProfile.OrganizationUserProfileID);

                    //Entity.ClientEntity.OrganizationUserProfile organizationUserProfile = _dbContext.OrganizationUserProfiles.Where(con => con.OrganizationUserID == organizationUserData.OrganizationUserID
                    //                                                                        && con.OrganizationUserProfileID == OrganizationUserProfile.OrganizationUserProfileID).FirstOrDefault();
                    //if (!organizationUserProfile.IsNullOrEmpty())
                    //{
                    //    organizationUserProfile.OrganizationUserID = organizationUserProfile.OrganizationUserID;
                    //    organizationUserProfile.UserTypeID = organizationUserProfile.UserTypeID;
                    //    organizationUserProfile.AddressHandleID = organizationUserProfile.AddressHandleID;
                    //    organizationUserProfile.FirstName = organizationUserProfile.FirstName;
                    //    organizationUserProfile.LastName = organizationUserProfile.LastName;
                    //    organizationUserProfile.VerificationCode = organizationUserProfile.VerificationCode;
                    //    organizationUserProfile.OfficeReturnDateTime = organizationUserProfile.OfficeReturnDateTime;
                    //    organizationUserProfile.IsDeleted = organizationUserProfile.IsDeleted;
                    //    organizationUserProfile.IsActive = organizationUserProfile.IsActive;
                    //    organizationUserProfile.ExpireDate = organizationUserProfile.ExpireDate;
                    //    organizationUserProfile.CreatedByID = organizationUserProfile.CreatedByID;
                    //    organizationUserProfile.CreatedOn = organizationUserProfile.CreatedOn;
                    //    organizationUserProfile.ModifiedByID = organizationUserProfile.ModifiedByID;
                    //    organizationUserProfile.ModifiedOn = organizationUserProfile.ModifiedOn;
                    //    organizationUserProfile.PhotoName = organizationUserProfile.PhotoName;
                    //    organizationUserProfile.OriginalPhotoName = organizationUserProfile.OriginalPhotoName;
                    //    organizationUserProfile.DOB = organizationUserProfile.DOB;
                    //    organizationUserProfile.SSN = organizationUserProfile.SSN;
                    //    organizationUserProfile.Gender = organizationUserProfile.Gender;
                    //    organizationUserProfile.PhoneNumber = organizationUserProfile.PhoneNumber;
                    //    organizationUserProfile.MiddleName = organizationUserProfile.MiddleName;
                    //    organizationUserProfile.Alias1 = organizationUserProfile.Alias1;
                    //    organizationUserProfile.Alias2 = organizationUserProfile.Alias2;
                    //    organizationUserProfile.Alias3 = organizationUserProfile.Alias3;
                    //    organizationUserProfile.PrimaryEmailAddress = organizationUserProfile.PrimaryEmailAddress;
                    //    organizationUserProfile.SecondaryEmailAddress = organizationUserProfile.SecondaryEmailAddress;
                    //    organizationUserProfile.SecondaryPhone = organizationUserProfile.SecondaryPhone;
                    //    //UAT-2447
                    //    organizationUserProfile.IsInternationalPhoneNumber = organizationUserProfile.IsInternationalPhoneNumber;
                    //    organizationUserProfile.IsInternationalSecondaryPhone = organizationUserProfile.IsInternationalSecondaryPhone;
                    //    organizationUserProfile.ModifiedByID = organizationUserData.OrganizationUserID;
                    //    organizationUserProfile.ModifiedOn = DateTime.Now;
                    #endregion

                    #region New- Implementation
                    var OrganizationUserProfileDB = organizationUserData.OrganizationUserProfiles.FirstOrDefault(x => x.OrganizationUserProfileID == UserProfile.OrganizationUserProfileID);

                    Entity.ClientEntity.OrganizationUserProfile organizationUserProfile = _dbContext.OrganizationUserProfiles.Where(con => con.OrganizationUserID == organizationUserData.OrganizationUserID
                                                                                            && con.OrganizationUserProfileID == OrganizationUserProfileDB.OrganizationUserProfileID).FirstOrDefault();
                    if (!organizationUserProfile.IsNullOrEmpty())
                    {
                        organizationUserProfile.OrganizationUserID = OrganizationUserProfileDB.OrganizationUserID;
                        organizationUserProfile.UserTypeID = OrganizationUserProfileDB.UserTypeID;
                        organizationUserProfile.AddressHandleID = OrganizationUserProfileDB.AddressHandleID;
                        organizationUserProfile.FirstName = OrganizationUserProfileDB.FirstName;
                        organizationUserProfile.LastName = OrganizationUserProfileDB.LastName;
                        organizationUserProfile.VerificationCode = OrganizationUserProfileDB.VerificationCode;
                        organizationUserProfile.OfficeReturnDateTime = OrganizationUserProfileDB.OfficeReturnDateTime;
                        organizationUserProfile.IsDeleted = OrganizationUserProfileDB.IsDeleted;
                        organizationUserProfile.IsActive = OrganizationUserProfileDB.IsActive;
                        organizationUserProfile.ExpireDate = OrganizationUserProfileDB.ExpireDate;
                        organizationUserProfile.CreatedByID = OrganizationUserProfileDB.CreatedByID;
                        organizationUserProfile.CreatedOn = OrganizationUserProfileDB.CreatedOn;
                        organizationUserProfile.ModifiedByID = OrganizationUserProfileDB.ModifiedByID;
                        organizationUserProfile.ModifiedOn = OrganizationUserProfileDB.ModifiedOn;
                        organizationUserProfile.PhotoName = OrganizationUserProfileDB.PhotoName;
                        organizationUserProfile.OriginalPhotoName = OrganizationUserProfileDB.OriginalPhotoName;
                        organizationUserProfile.DOB = OrganizationUserProfileDB.DOB;
                        organizationUserProfile.SSN = OrganizationUserProfileDB.SSN;
                        organizationUserProfile.Gender = OrganizationUserProfileDB.Gender;
                        organizationUserProfile.PhoneNumber = OrganizationUserProfileDB.PhoneNumber;
                        organizationUserProfile.MiddleName = OrganizationUserProfileDB.MiddleName;
                        organizationUserProfile.Alias1 = OrganizationUserProfileDB.Alias1;
                        organizationUserProfile.Alias2 = OrganizationUserProfileDB.Alias2;
                        organizationUserProfile.Alias3 = OrganizationUserProfileDB.Alias3;
                        organizationUserProfile.PrimaryEmailAddress = OrganizationUserProfileDB.PrimaryEmailAddress;
                        organizationUserProfile.SecondaryEmailAddress = OrganizationUserProfileDB.SecondaryEmailAddress;
                        organizationUserProfile.SecondaryPhone = OrganizationUserProfileDB.SecondaryPhone;
                        organizationUserProfile.IsInternationalPhoneNumber = OrganizationUserProfileDB.IsInternationalPhoneNumber;
                        organizationUserProfile.IsInternationalSecondaryPhone = OrganizationUserProfileDB.IsInternationalSecondaryPhone;
                        organizationUserProfile.ModifiedByID = OrganizationUserProfileDB.OrganizationUserID;
                        organizationUserProfile.ModifiedOn = DateTime.Now;
                        #endregion



                        #region Add/update organization User profile alias

                        #region Add Update Organization User Profile/ Profile Suffix Mapping
                        //if (!organizationUserProfile.IsNullOrEmpty())
                        //{
                        //    //List<PersonAliasProfile> lstAliasProfile = organizationUserProfile.PersonAliasProfiles.Where(con => !con.PAP_IsDeleted).ToList();
                        //    foreach (var personAliasProfile in OrganizationUserProfile.PersonAliasProfiles.ToList())
                        //    {
                        //        //if entry already exists//update

                        //        PersonAliasProfile currentPersonAliasProfile = _dbContext.PersonAliasProfiles.Where(x => x.PAP_ID == personAliasProfile.PAP_ID && !x.PAP_IsDeleted).FirstOrDefault();
                        //        if (!currentPersonAliasProfile.IsNullOrEmpty())
                        //        {
                        //            currentPersonAliasProfile.PAP_FirstName = personAliasProfile.PAP_FirstName;
                        //            currentPersonAliasProfile.PAP_LastName = personAliasProfile.PAP_LastName;
                        //            currentPersonAliasProfile.PAP_MiddleName = personAliasProfile.PAP_MiddleName;
                        //            currentPersonAliasProfile.PAP_ModifiedBy = organizationUser.OrganizationUserID;
                        //            currentPersonAliasProfile.PAP_ModifiedOn = DateTime.Now;

                        //            // personaliasprofile update functionality
                        //            // Get PersonAliasProfileSuffixMappings on the basis of PersonAliasProfileID --
                        //            //If It Exists // then Update
                        //            //Else Add It

                        //            if (!currentPersonAliasProfile.PersonAliasProfileSuffixMappings.Where(con => !con.PAPSM_IsDeleted).ToList().IsNullOrEmpty())
                        //            {
                        //                PersonAliasProfileSuffixMapping currentPersonAliasProfileSuffix = currentPersonAliasProfile.PersonAliasProfileSuffixMappings.FirstOrDefault(x => !x.PAPSM_IsDeleted);
                        //                currentPersonAliasProfileSuffix.PAPSM_SuffixId = Convert.ToInt32(currentPersonAliasProfileSuffix.PAPSM_SuffixId);
                        //                currentPersonAliasProfileSuffix.PAPSM_ModifiedBy = organizationUser.OrganizationUserID;
                        //                currentPersonAliasProfileSuffix.PAPSM_ModifiedOn = DateTime.Now;
                        //                //organizationUserProfile.PersonAliasProfiles.PersonAliasProfileSuffixMappings.Add(currentPersonAliasProfile);
                        //            }
                        //            else
                        //            {
                        //                PersonAliasProfileSuffixMapping newPersonAliasProfileSuffix = new PersonAliasProfileSuffixMapping();
                        //                newPersonAliasProfileSuffix.PAPSM_CreatedBy = organizationUser.OrganizationUserID;
                        //                newPersonAliasProfileSuffix.PAPSM_CreatedOn = DateTime.Now;
                        //                newPersonAliasProfileSuffix.PAPSM_SuffixId = Convert.ToInt32(personAliasProfile.PersonAliasProfileSuffixMappings.FirstOrDefault().PAPSM_SuffixId);
                        //                //organizationUserProfile.PersonAliasProfiles.PersonAliasProfileSuffixMappings.Add(newPersonAliasProfileSuffix);
                        //            }
                        //        }

                        //        //organizationUserProfile.PersonAliasProfiles.Add(currentPersonAliasProfile);
                        //        else
                        //        {
                        //            //add entries into table if doesnt exist
                        //            PersonAliasProfile newPersonAliasProfile = new PersonAliasProfile();
                        //            newPersonAliasProfile.PAP_ID = personAliasProfile.PAP_ID;
                        //            newPersonAliasProfile.PAP_FirstName = personAliasProfile.PAP_FirstName;
                        //            newPersonAliasProfile.PAP_MiddleName = personAliasProfile.PAP_MiddleName;
                        //            newPersonAliasProfile.PAP_LastName = personAliasProfile.PAP_LastName;
                        //            newPersonAliasProfile.PAP_CreatedBy = organizationUser.OrganizationUserID;
                        //            newPersonAliasProfile.PAP_OrganizationUserProfileID = organizationUserProfile.OrganizationUserProfileID;
                        //            newPersonAliasProfile.PAP_CreatedOn = DateTime.Now;


                        //            PersonAliasProfileSuffixMapping newPersonAliasProfileSuffix = new PersonAliasProfileSuffixMapping();
                        //            newPersonAliasProfileSuffix.PAPSM_CreatedBy = organizationUser.OrganizationUserID;
                        //            newPersonAliasProfileSuffix.PAPSM_CreatedOn = DateTime.Now;
                        //            newPersonAliasProfileSuffix.PAPSM_SuffixId = Convert.ToInt32(personAliasProfile.PersonAliasProfileSuffixMappings.FirstOrDefault().PAPSM_SuffixId);

                        //            newPersonAliasProfile.PersonAliasProfileSuffixMappings.Add(newPersonAliasProfileSuffix);
                        //            _dbContext.PersonAliasProfiles.AddObject(newPersonAliasProfile);
                        //            //organizationUserProfile.PersonAliasProfiles.Add(newPersonAliasProfile);
                        //        }
                        //    }
                        //}
                        ////delete enteries
                        //else
                        //{
                        //    List<PersonAliasProfile> lstExistingPersonAliasProfiles = organizationUserProfile.PersonAliasProfiles.Where(con => !con.PAP_IsDeleted).ToList();
                        //    foreach (var personAliasProfiles in lstExistingPersonAliasProfiles)
                        //    {
                        //        personAliasProfiles.PAP_IsDeleted = true;
                        //        personAliasProfiles.PAP_ModifiedBy = organizationUser.OrganizationUserID;
                        //        personAliasProfiles.PAP_ModifiedOn = DateTime.Now;
                        //    }

                        //}
                        #endregion


                        #region OUP Suffix Mapping

                        if (!OrganizationUserProfileDB.OrganizationUserProfileSuffixMappings.Where(con => !con.OUPSM_IsDeleted).IsNullOrEmpty()
                            && !organizationUserProfile.IsNullOrEmpty() && !organizationUserProfile.UserTypeID.IsNullOrEmpty() && organizationUserProfile.UserTypeID > AppConsts.NONE)
                        {
                            OrganizationUserProfileSuffixMapping oupSuffixMapping = new OrganizationUserProfileSuffixMapping();
                            oupSuffixMapping.OUPSM_OrganizationUserProfileId = organizationUserProfile.OrganizationUserProfileID;
                            oupSuffixMapping.OUPSM_SuffixId = Convert.ToInt32(organizationUserProfile.UserTypeID);
                            oupSuffixMapping.OUPSM_CreatedOn = DateTime.Now;
                            oupSuffixMapping.OUPSM_CreatedBy = organizationUser.OrganizationUserID;

                            organizationUserProfile.OrganizationUserProfileSuffixMappings.Add(oupSuffixMapping);
                        }

                        #endregion


                        #region New Implementation-- Profile Suffix Mapping/ User Profile Update
                        if (!organizationUserProfile.IsNullOrEmpty())
                        {
                            List<PersonAliasProfile> lstExistingPersonAliasProfiles = organizationUserProfile.PersonAliasProfiles.Where(con => !con.PAP_IsDeleted).ToList();
                            foreach (var personAliasProfiles in lstExistingPersonAliasProfiles)
                            {
                                personAliasProfiles.PAP_IsDeleted = true;
                                personAliasProfiles.PAP_ModifiedBy = organizationUser.OrganizationUserID;
                                personAliasProfiles.PAP_ModifiedOn = DateTime.Now;
                            }

                            //List<PersonAliasProfile> lstAliasProfile = organizationUserProfile.PersonAliasProfiles.Where(con => !con.PAP_IsDeleted).ToList();
                            foreach (var personAliasProfile in OrganizationUserProfileDB.PersonAliasProfiles.Where(con => !con.PAP_IsDeleted).ToList())
                            {
                                //add entries into table if doesnt exist
                                PersonAliasProfile newPersonAliasProfile = new PersonAliasProfile();
                                newPersonAliasProfile.PAP_ID = personAliasProfile.PAP_ID;
                                newPersonAliasProfile.PAP_FirstName = personAliasProfile.PAP_FirstName;
                                newPersonAliasProfile.PAP_MiddleName = personAliasProfile.PAP_MiddleName;
                                newPersonAliasProfile.PAP_LastName = personAliasProfile.PAP_LastName;
                                newPersonAliasProfile.PAP_CreatedBy = organizationUser.OrganizationUserID;
                                newPersonAliasProfile.PAP_OrganizationUserProfileID = organizationUserProfile.OrganizationUserProfileID;
                                newPersonAliasProfile.PAP_CreatedOn = DateTime.Now;

                                if (!personAliasProfile.PersonAliasProfileSuffixMappings.IsNullOrEmpty() && !personAliasProfile.PersonAliasProfileSuffixMappings.FirstOrDefault().PAPSM_SuffixId.IsNullOrEmpty()
                                    && personAliasProfile.PersonAliasProfileSuffixMappings.FirstOrDefault().PAPSM_SuffixId > AppConsts.NONE)
                                {
                                    PersonAliasProfileSuffixMapping newPersonAliasProfileSuffix = new PersonAliasProfileSuffixMapping();
                                    newPersonAliasProfileSuffix.PAPSM_CreatedBy = organizationUser.OrganizationUserID;
                                    newPersonAliasProfileSuffix.PAPSM_CreatedOn = DateTime.Now;
                                    newPersonAliasProfileSuffix.PAPSM_SuffixId = Convert.ToInt32(personAliasProfile.PersonAliasProfileSuffixMappings.FirstOrDefault().PAPSM_SuffixId);

                                    newPersonAliasProfile.PersonAliasProfileSuffixMappings.Add(newPersonAliasProfileSuffix);
                                }

                                _dbContext.PersonAliasProfiles.AddObject(newPersonAliasProfile);
                                //organizationUserProfile.PersonAliasProfiles.Add(newPersonAliasProfile);
                            }
                        }

                        #endregion

                        #endregion
                        #region Add organization User Profile residence history
                        //if (organizationUserProfile.OrganizationUser.ResidentialHistories.IsNotNull())
                        //{
                        //    List<ResidentialHistory> lstResidentialHistories = organizationUserProfile.OrganizationUser.ResidentialHistories.Where(x => x.RHI_IsDeleted == false).ToList();

                        //    foreach (ResidentialHistory resHistory in lstResidentialHistories)
                        //    {
                        //        ResidentialHistoryProfile residentialHistoryProfile = new ResidentialHistoryProfile();
                        //        residentialHistoryProfile.RHIP_AddressId = resHistory.RHI_AddressId;
                        //        residentialHistoryProfile.RHIP_IsCurrentAddress = resHistory.RHI_IsCurrentAddress.HasValue ? resHistory.RHI_IsCurrentAddress.Value : false;
                        //        residentialHistoryProfile.RHIP_IsPrimaryResidence = resHistory.RHI_IsPrimaryResidence.HasValue ? resHistory.RHI_IsPrimaryResidence.Value : false;
                        //        residentialHistoryProfile.RHIP_ResidenceStartDate = resHistory.RHI_ResidenceStartDate;
                        //        residentialHistoryProfile.RHIP_ResidenceEndDate = resHistory.RHI_ResidenceEndDate;
                        //        residentialHistoryProfile.RHIP_IsDeleted = resHistory.RHI_IsDeleted.HasValue ? resHistory.RHI_IsDeleted.Value : false;
                        //        residentialHistoryProfile.RHIP_CreatedBy = resHistory.RHI_CreatedByID;
                        //        residentialHistoryProfile.RHIP_CreatedOn = resHistory.RHI_CreatedOn;
                        //        residentialHistoryProfile.RHIP_SequenceOrder = resHistory.RHI_SequenceOrder;
                        //        residentialHistoryProfile.RHIP_OrganizationUserProfileID = organizationUserProfile.OrganizationUserProfileID;
                        //        _dbContext.ResidentialHistoryProfiles.AddObject(residentialHistoryProfile);
                        //        //organizationUserProfile.ResidentialHistoryProfiles.Add(residentialHistoryProfile);
                        //    }
                        //}

                        #endregion


                        #region New Implementation-- Residential History Profiles
                        if (!organizationUserProfile.IsNullOrEmpty())
                        {
                            List<ResidentialHistoryProfile> lstResidentialHistories = organizationUserProfile.ResidentialHistoryProfiles.Where(x => x.RHIP_IsDeleted == false).ToList();
                            foreach (var resHistory in lstResidentialHistories)
                            {
                                resHistory.RHIP_IsDeleted = true;
                                resHistory.RHIP_ModifiedBy = organizationUser.OrganizationUserID;
                                resHistory.RHIP_ModifiedOn = DateTime.Now;
                            }


                            foreach (Entity.ResidentialHistoryProfile resHistoryProfile in OrganizationUserProfileDB.ResidentialHistoryProfiles.Where(con => !con.RHIP_IsDeleted).ToList())
                            {

                                AddAddressHandle(resHistoryProfile.Address.AddressHandleID);
                                Address addressNew = new Address();
                                addressNew.AddressID = resHistoryProfile.Address.AddressID;
                                addressNew.Address1 = resHistoryProfile.Address.Address1;
                                addressNew.Address2 = resHistoryProfile.Address.Address2;
                                addressNew.IsActive = true;
                                addressNew.ZipCodeID = resHistoryProfile.Address.ZipCodeID;
                                if (addressNew.ZipCodeID == 0)
                                {
                                    if (resHistoryProfile.Address.AddressExts.IsNotNull() && resHistoryProfile.Address.AddressExts.Count > 0)
                                    {
                                        Entity.AddressExt masterAddressExt = resHistoryProfile.Address.AddressExts.FirstOrDefault();
                                        AddressExt addressExtNew = new AddressExt();
                                        addressExtNew.AE_ID = masterAddressExt.AE_ID;
                                        addressExtNew.AE_CountryID = masterAddressExt.AE_CountryID;
                                        addressExtNew.AE_StateName = masterAddressExt.AE_StateName;
                                        addressExtNew.AE_CityName = masterAddressExt.AE_CityName;
                                        addressExtNew.AE_ZipCode = masterAddressExt.AE_ZipCode;
                                        addressNew.AddressExts.Add(addressExtNew);
                                    }
                                }
                                addressNew.AddressHandleID = resHistoryProfile.Address.AddressHandleID;
                                organizationUserProfile.AddressHandleID = resHistoryProfile.Address.AddressHandleID;
                                addressNew.CreatedOn = DateTime.Now;
                                addressNew.CreatedByID = organizationUser.OrganizationUserID;
                                //_dbContext.Addresses.AddObject(addressNew);

                                //applicantOrderDataContract.OrganizationUserProfile.AddressHandleID = applicantOrderDataContract.AddressHandleIdMaster;

                                ResidentialHistoryProfile residentialHistoryProfile = new ResidentialHistoryProfile();
                                residentialHistoryProfile.RHIP_ID = resHistoryProfile.RHIP_ID;
                                residentialHistoryProfile.RHIP_IsCurrentAddress = resHistoryProfile.RHIP_IsCurrentAddress;
                                residentialHistoryProfile.RHIP_IsPrimaryResidence = resHistoryProfile.RHIP_IsPrimaryResidence;
                                residentialHistoryProfile.RHIP_ResidenceStartDate = resHistoryProfile.RHIP_ResidenceStartDate;
                                residentialHistoryProfile.RHIP_ResidenceEndDate = resHistoryProfile.RHIP_ResidenceEndDate;
                                residentialHistoryProfile.RHIP_IsDeleted = resHistoryProfile.RHIP_IsDeleted;
                                residentialHistoryProfile.RHIP_CreatedBy = organizationUser.OrganizationUserID;
                                residentialHistoryProfile.RHIP_CreatedOn = resHistoryProfile.RHIP_CreatedOn;
                                residentialHistoryProfile.RHIP_SequenceOrder = resHistoryProfile.RHIP_SequenceOrder;
                                //residentialHistoryProfile.RHIP_IdentificationNumber = resHistoryProfile.RHIP_IdentificationNumber;
                                //residentialHistoryProfile.RHIP_MotherMaidenName = residentialHistoryProfile.RHIP_MotherMaidenName;
                                //residentialHistoryProfile.RHIP_DriverLicenseNumber = residentialHistoryProfile.RHIP_DriverLicenseNumber;
                                residentialHistoryProfile.Address = addressNew;
                                residentialHistoryProfile.RHIP_OrganizationUserProfileID = organizationUserProfile.OrganizationUserProfileID;
                                _dbContext.ResidentialHistoryProfiles.AddObject(residentialHistoryProfile);
                                //organizationUserProfile.ResidentialHistoryProfiles.Add(residentialHistoryProfile);
                            }
                        }
                        #endregion

                    }
                    //}
                }

                #endregion
                #region Update Organization User

                //if (organizationUserData.ResidentialHistories.IsNotNull())
                //{
                //    foreach (var prevAddress in organizationUserData.ResidentialHistories)
                //    {
                //        ResidentialHistory newResHisObj = organizationUser.ResidentialHistories.FirstOrDefault(x => x.RHI_ID == prevAddress.RHI_ID);
                //        if (newResHisObj.IsNotNull())
                //        {
                //            if (newResHisObj.RHI_IsCurrentAddress == true && prevAddress.RHI_IsCurrentAddress == true)
                //            {
                //                newResHisObj.RHI_AddressId = prevAddress.RHI_AddressId;
                //                newResHisObj.RHI_ResidenceStartDate = prevAddress.RHI_ResidenceStartDate;
                //                newResHisObj.RHI_ModifiedByID = prevAddress.RHI_ModifiedByID;
                //                newResHisObj.RHI_ModifiedOn = prevAddress.RHI_ModifiedOn;
                //                newResHisObj.RHI_SequenceOrder = prevAddress.RHI_SequenceOrder;
                //            }
                //            else
                //            {
                //                if (!(newResHisObj.RHI_IsDeleted == true && prevAddress.RHI_IsDeleted == true))
                //                {
                //                    if (prevAddress.RHI_IsDeleted == true)
                //                    {
                //                        newResHisObj.RHI_IsDeleted = true;
                //                    }
                //                    else
                //                    {
                //                        newResHisObj.RHI_AddressId = prevAddress.RHI_AddressId;
                //                        newResHisObj.RHI_ResidenceStartDate = prevAddress.RHI_ResidenceStartDate;
                //                        newResHisObj.RHI_ResidenceEndDate = prevAddress.RHI_ResidenceEndDate;
                //                        newResHisObj.RHI_IsCurrentAddress = prevAddress.RHI_IsCurrentAddress;
                //                    }
                //                    newResHisObj.RHI_ModifiedByID = prevAddress.RHI_ModifiedByID;
                //                    newResHisObj.RHI_ModifiedOn = prevAddress.RHI_ModifiedOn;
                //                    newResHisObj.RHI_SequenceOrder = prevAddress.RHI_SequenceOrder;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            if (prevAddress.RHI_IsDeleted == false)
                //            {
                //                newResHisObj = new ResidentialHistory();
                //                newResHisObj.RHI_ID = prevAddress.RHI_ID;
                //                newResHisObj.RHI_ResidenceStartDate = prevAddress.RHI_ResidenceStartDate;
                //                newResHisObj.RHI_ResidenceEndDate = prevAddress.RHI_ResidenceEndDate;
                //                newResHisObj.RHI_OrganizationUserID = prevAddress.RHI_OrganizationUserID;
                //                newResHisObj.RHI_AddressId = prevAddress.RHI_AddressId;
                //                newResHisObj.RHI_IsCurrentAddress = prevAddress.RHI_IsCurrentAddress;
                //                newResHisObj.RHI_IsPrimaryResidence = prevAddress.RHI_IsPrimaryResidence;
                //                newResHisObj.RHI_IsDeleted = prevAddress.RHI_IsDeleted;
                //                newResHisObj.RHI_CreatedByID = prevAddress.RHI_CreatedByID;
                //                newResHisObj.RHI_CreatedOn = prevAddress.RHI_CreatedOn;
                //                newResHisObj.RHI_SequenceOrder = prevAddress.RHI_SequenceOrder;
                //                _dbContext.ResidentialHistories.AddObject(newResHisObj);
                //                //organizationUser.ResidentialHistories.Add(newResHisObj);
                //            }
                //        }
                //    }
                //}
                #endregion

                #region New Implementation-- Residential Histories
                if (!organizationUser.IsNullOrEmpty())
                {
                    List<ResidentialHistory> lstResidentialHistories = organizationUser.ResidentialHistories.Where(x => x.RHI_IsDeleted == false).ToList();
                    foreach (var resHistory in lstResidentialHistories)
                    {
                        resHistory.RHI_IsDeleted = true;
                        resHistory.RHI_ModifiedByID = organizationUser.OrganizationUserID;
                        resHistory.RHI_ModifiedOn = DateTime.Now;
                    }

                    foreach (var prevAddress in organizationUserData.ResidentialHistories.Where(x => x.RHI_IsDeleted == false))
                    {
                        AddAddressHandle(prevAddress.Address.AddressHandleID);
                        Address addressNew = new Address();
                        addressNew.AddressID = prevAddress.Address.AddressID;
                        addressNew.Address1 = prevAddress.Address.Address1;
                        addressNew.Address2 = prevAddress.Address.Address2;
                        addressNew.IsActive = true;
                        addressNew.ZipCodeID = prevAddress.Address.ZipCodeID;
                        if (addressNew.ZipCodeID == 0)
                        {
                            if (prevAddress.Address.AddressExts.IsNotNull() && prevAddress.Address.AddressExts.Count > 0)
                            {
                                Entity.AddressExt masterAddressExt = prevAddress.Address.AddressExts.FirstOrDefault();
                                AddressExt addressExtNew = new AddressExt();
                                addressExtNew.AE_ID = masterAddressExt.AE_ID;
                                addressExtNew.AE_CountryID = masterAddressExt.AE_CountryID;
                                addressExtNew.AE_StateName = masterAddressExt.AE_StateName;
                                addressExtNew.AE_CityName = masterAddressExt.AE_CityName;
                                addressExtNew.AE_ZipCode = masterAddressExt.AE_ZipCode;
                                addressNew.AddressExts.Add(addressExtNew);
                            }
                        }
                        addressNew.AddressHandleID = prevAddress.Address.AddressHandleID;
                        addressNew.CreatedOn = DateTime.Now;
                        addressNew.CreatedByID = organizationUser.OrganizationUserID;
                        //_dbContext.Addresses.AddObject(addressNew);

                        if (prevAddress.RHI_IsDeleted == false)
                        {
                            ResidentialHistory newResHisObj = new ResidentialHistory();
                            newResHisObj.RHI_ID = prevAddress.RHI_ID;
                            newResHisObj.RHI_ResidenceStartDate = prevAddress.RHI_ResidenceStartDate.HasValue ? prevAddress.RHI_ResidenceStartDate : null;
                            newResHisObj.RHI_ResidenceEndDate = prevAddress.RHI_ResidenceEndDate.HasValue ? prevAddress.RHI_ResidenceEndDate : null;
                            newResHisObj.RHI_OrganizationUserID = prevAddress.RHI_OrganizationUserID;
                            //newResHisObj.RHI_AddressId = prevAddress.RHI_AddressId;
                            newResHisObj.RHI_IsCurrentAddress = prevAddress.RHI_IsCurrentAddress.HasValue ? prevAddress.RHI_IsCurrentAddress : false;
                            newResHisObj.RHI_IsPrimaryResidence = prevAddress.RHI_IsPrimaryResidence.HasValue ? prevAddress.RHI_IsPrimaryResidence : false;
                            newResHisObj.RHI_IsDeleted = prevAddress.RHI_IsDeleted.HasValue ? prevAddress.RHI_IsDeleted : false;
                            newResHisObj.RHI_CreatedByID = organizationUser.OrganizationUserID;
                            newResHisObj.RHI_CreatedOn = prevAddress.RHI_CreatedOn;
                            //newResHisObj.RHI_DriverLicenseNumber = prevAddress.RHI_DriverLicenseNumber;
                            //newResHisObj.RHI_MotherMaidenName = prevAddress.RHI_MotherMaidenName;
                            //newResHisObj.RHI_IdentificationNumber = prevAddress.RHI_IdentificationNumber;
                            newResHisObj.RHI_SequenceOrder = prevAddress.RHI_SequenceOrder.HasValue ? prevAddress.RHI_SequenceOrder : AppConsts.NONE;
                            // newResHisObj.Address = new Address();
                            newResHisObj.Address = addressNew;
                            //_dbContext.Addresses.AddObject(addressNew);
                            _dbContext.ResidentialHistories.AddObject(newResHisObj);
                            //organizationUser.ResidentialHistories.Add(newResHisObj);
                        }
                    }
                }
                #endregion

            }

            UpdateChanges();
        }
        //#endregion

        #region Notifications
        DataTable IAdminEntryPortalRepository.GetDraftOrders(int chunkSize, int daysOld, int subEventId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetAdminEntryDarftOrderDetails", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DaysOld", daysOld);
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                command.Parameters.AddWithValue("@subEventId", subEventId);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                }
                return new DataTable();
            }
        }

        DataTable IAdminEntryPortalRepository.GetInvitationPendingStatusOrderForApplicant(Int32 chunkSize, Int32 daysOld, Int32 subEventId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetAdminEntryDarftPendingInvitaionOrders", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DaysOld", daysOld);
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                command.Parameters.AddWithValue("@subEventId", subEventId);


                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                }
                return new DataTable();
            }
        }


        bool IAdminEntryPortalRepository.DeleteDraftOrders(Int32 DaysOld, Int32 backgroundProcessUserId)
        {
            DateTime dt = DateTime.Now.AddDays(-DaysOld);
            var orders = _dbContext.BkgAdminEntryOrderDetails.Where(odr => odr.BAEOD_CreatedOn <= dt && odr.lkpAdminEntryOrderStatu.AEOS_Code == "AAAB" && odr.BAEOD_IsDeleted != true).ToList();
            foreach (var order in orders)
            {
                order.BAEOD_IsDeleted = true;
                order.BAEOD_ModifiedBy = backgroundProcessUserId;
                order.BAEOD_ModifiedOn = DateTime.Now;

                if (order.BkgOrder != null)
                {
                    order.BkgOrder.BOR_IsDeleted = true;
                    order.BkgOrder.BOR_ModifiedByID = backgroundProcessUserId;
                    order.BkgOrder.BOR_ModifiedOn = DateTime.Now;

                    if (order.BkgOrder.OrganizationUserProfile != null && order.BkgOrder.OrganizationUserProfile.IsDeleted == false)
                    {
                        order.BkgOrder.OrganizationUserProfile.IsDeleted = true;
                        order.BkgOrder.OrganizationUserProfile.ModifiedByID = backgroundProcessUserId;
                        order.BkgOrder.OrganizationUserProfile.ModifiedOn = DateTime.Now;
                    }
                    order.BkgOrder.Order.IsDeleted = true;
                    order.BkgOrder.Order.ModifiedByID = backgroundProcessUserId;
                    order.BkgOrder.Order.ModifiedOn = DateTime.Now;
                }
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;

            return false;
        }

        bool IAdminEntryPortalRepository.ChangeBkgOrdersStatusCompletedToArchived(Int32 DaysOld, Int32 bkgAdminEntryOrderID, Int32 backgroundProcessUserId
                                                                                 , List<lkpAdminEntryOrderStatu> lstlkpAdminEntryOrderStatus
                                                                                 , List<lkpEventHistory> lstlkpEventHistory)
        {
            DateTime dt = DateTime.Now.AddDays(-DaysOld);
            short ArchivedStatusId = lstlkpAdminEntryOrderStatus.Where(stat => stat.AEOS_Code == "AAAC").FirstOrDefault().AEOS_ID;
            short CompleteStatusId = lstlkpAdminEntryOrderStatus.Where(stat => stat.AEOS_Code == "AAAD").FirstOrDefault().AEOS_ID;

            Int32 eventHistory_Archived = lstlkpEventHistory.FirstOrDefault(cnd => cnd.EH_Code == "AAAJ").EH_ID;

            //_dbContext.BkgOrders.Where(con=>con.)



            var order = _dbContext.BkgAdminEntryOrderDetails.Where(odr => odr.BkgOrder.BOR_OrderCompleteDate <= dt && odr.BAEOD_OrderStatusID == CompleteStatusId
                                                                        && odr.BAEOD_ID == bkgAdminEntryOrderID && odr.BAEOD_IsDeleted != true).FirstOrDefault();


            if (order != null)
            {
                //if (order.BAEOD_OrderDraftStatusID.Equals(CompleteStatusId))
                order.BAEOD_OrderStatusID = ArchivedStatusId;
                order.BAEOD_ModifiedBy = backgroundProcessUserId;
                order.BAEOD_ModifiedOn = DateTime.Now;

                BkgOrderEventHistory eventHistory = new BkgOrderEventHistory
                {
                    BOEH_BkgOrderID = order.BAEOD_BkgOrderID,
                    BOEH_EventHistoryId = eventHistory_Archived,
                    BOEH_CreatedByID = backgroundProcessUserId,
                    BOEH_CreatedOn = DateTime.Now,
                    BOEH_OrderEventDetail = "Changed Order status from Complete to Archived"
                };
                _dbContext.BkgOrderEventHistories.AddObject(eventHistory);
            }

            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;

            return false;
        }

        DataTable IAdminEntryPortalRepository.GetAutoArchiveTimeLineDays(int chunkSize)
        {

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetAutoArchiveTimeLineDays", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                }
                return new DataTable();
            }
        }

        DataTable IAdminEntryPortalRepository.GetRecentCompletedOrders(int chunkSize, string EntityName, Int32 subEventId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetCompletedOrders", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ChunkSize", chunkSize);
                command.Parameters.AddWithValue("@EntityName", EntityName);
                command.Parameters.AddWithValue("@subEventId", subEventId);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                }
                return new DataTable();
            }
        }

        #endregion

        String IAdminEntryPortalRepository.GetApplicantInviteContent(Int32 orderId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_GetApplicantInviteLandingScreenContent", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", orderId);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var row = ds.Tables[0].Rows[0];
                        return row["Content"].ToString();
                    }
                }
                return String.Empty;
            }
        }
    }
}
