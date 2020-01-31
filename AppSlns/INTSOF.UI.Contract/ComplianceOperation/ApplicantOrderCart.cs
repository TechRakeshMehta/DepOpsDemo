using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;
using System.Collections.Specialized;
using INTSOF.Utils;
using System.Collections;
using INTSOF.UI.Contract.BkgOperations;
using System.Runtime.Serialization;
using INTSOF.UI.Contract.MobileAPI;
using INTSOF.UI.Contract.FingerPrintSetup;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class ApplicantOrderCart
    {
        #region Methods

        public void ClearOrderCart(ApplicantOrderCart applicantOrderCart)
        {
            if (applicantOrderCart.IsNotNull())
            {
                applicantOrderCart.lstApplicantOrder = null;
                applicantOrderCart.lstDepProgramMappingId = null;
                applicantOrderCart.alNodeIds = null;

                //applicantOrderCart.DPP_Id = null;
                //applicantOrderCart.GrandTotal = null;
                //applicantOrderCart.RenewalDuration = 0;
                //applicantOrderCart.Amount = null;
                //applicantOrderCart.RushOrderPrice = null;
                //applicantOrderCart.IsRushOrderIncluded = false;
                //applicantOrderCart.InvoiceNumber = null;
                //applicantOrderCart.ProgramDuration = null;
                if (Convert.ToString(applicantOrderCart.OrderRequestType) == INTSOF.Utils.OrderRequestType.ChangeSubscription.GetStringValue()
                    || Convert.ToString(applicantOrderCart.OrderRequestType) == INTSOF.Utils.OrderRequestType.RenewalOrder.GetStringValue())
                {
                    applicantOrderCart.DPP_Id = 0;
                    applicantOrderCart.DPPS_ID = 0;
                    applicantOrderCart.GrandTotal = 0;
                    applicantOrderCart.RenewalDuration = 0;
                    applicantOrderCart.Amount = null;
                    applicantOrderCart.RushOrderPrice = null;
                    applicantOrderCart.IsRushOrderIncluded = false;
                    applicantOrderCart.InvoiceNumber = null;
                    applicantOrderCart.ProgramDuration = null;

                }
                else
                    applicantOrderCart.CompliancePackages = null;
                applicantOrderCart.CurrentCompliancePackageTypeInContext = null;

                applicantOrderCart.ClearCustomAttributeValues();
                applicantOrderCart.DefaultNodeId = null;
                //applicantOrderCart.SelectedDeptProgramId = AppConsts.NONE;
                applicantOrderCart.ApplicantDisclaimerDocumentId = null;
                applicantOrderCart.ApplicantDisclosureDocumentIds = null;
                applicantOrderCart.IsCompliancePackageSelected = false;
                applicantOrderCart.lstCustomFormData = null;
                applicantOrderCart.lstFormExecuted = null;
                applicantOrderCart.CurrentCustomFormId = AppConsts.NONE;
                applicantOrderCart.EDrugScreeningRegistrationId = null;
                applicantOrderCart.IsEditMode = false;
            }
        }

        public void ClearPackageSelectionData()
        {
            this.lstDepProgramMappingId = null;

            this.CompliancePackages = null;
            this.CurrentCompliancePackageTypeInContext = null;

            this.alNodeIds = null;
            this.DefaultNodeId = null;
            this.ClearCustomAttributeValues();
            this.IsCompliancePackageSelected = false;
            this.IsOrderFlowConfirmation = false; //UAT - 2802
            if (lstApplicantOrder.IsNotNull() && lstApplicantOrder.Count > 0)
            {
                lstApplicantOrder[0].DPPS_Id = null;
                lstApplicantOrder[0].lstPackages = null;
                lstApplicantOrder[0].PreviousOrderStep = AppConsts.NONE;
                lstApplicantOrder[0].TotalOrderSteps = AppConsts.NONE;
            }
        }
        public void ClearPackageSelectionData(string CompliancePackageType)
        {
            if (CompliancePackages.IsNotNull() && CompliancePackages.Keys.Count > 0 && CompliancePackages.Keys.Contains(CompliancePackageType))
            {
                this.CompliancePackages.Remove(CompliancePackageType);
                if (CompliancePackages.Count == 0)
                    ClearPackageSelectionData();
            }
        }
        public ApplicantOrder GetApplicantOrder()
        {
            if (this.lstApplicantOrder == null)
                this.lstApplicantOrder = new List<ApplicantOrder>();

            if (this.lstApplicantOrder.Count == 0)
                this.lstApplicantOrder.Add(new ApplicantOrder());

            return this.lstApplicantOrder[0];

        }

        public void AddDeptProgramPackageSubscriptionId(Int32 deptProgramPackageSubscriptionId)
        {
            ApplicantOrder applicantOrder = GetApplicantOrder();
            if (applicantOrder.DPPS_Id == null)
            {
                applicantOrder.DPPS_Id = new List<Int32>();
                applicantOrder.DPPS_Id.Add(deptProgramPackageSubscriptionId);
            }
            else if (applicantOrder.DPPS_Id != null && applicantOrder.DPPS_Id.Count > 0)
                applicantOrder.DPPS_Id[0] = deptProgramPackageSubscriptionId;
        }

        /// <summary>
        /// Increases the Step number by one, on each next press
        /// </summary> 
        public void IncrementOrderStepCount()
        {
            ApplicantOrder applicantOrder = GetApplicantOrder();

            //if (applicantOrder.PreviousOrderStep.IsNullOrEmpty())
            //    applicantOrder.PreviousOrderStep = AppConsts.NONE;

            if (applicantOrder.PreviousOrderStep == AppConsts.NONE)
                applicantOrder.PreviousOrderStep += AppConsts.ONE;

            applicantOrder.PreviousOrderStep += AppConsts.ONE;
        }

        /// <summary>
        /// Descresase the Step number by one, on each next press
        /// </summary> 
        public void DecrementOrderStepCount()
        {
            ApplicantOrder applicantOrder = GetApplicantOrder();
            //if (!applicantOrder.PreviousOrderStep.IsNullOrEmpty())
            applicantOrder.PreviousOrderStep -= AppConsts.ONE;
        }


        /// <summary>
        /// Get the Total number of Steps in the Order
        /// </summary>
        /// <param name="deptProgramPackageSubscriptionId"></param>
        public Int32 GetTotalOrderSteps()
        {
            ApplicantOrder applicantOrder = GetApplicantOrder();
            return applicantOrder.TotalOrderSteps;
        }

        /// <summary>
        /// Set the total number of Order steps during new order or Place rush order for existing order.
        /// </summary>
        /// <param name="totalSteps"></param>
        public void SetTotalOrderSteps(Int32 totalSteps)
        {
            ApplicantOrder applicantOrder = GetApplicantOrder();
            if (applicantOrder.TotalOrderSteps.IsNullOrEmpty())
                applicantOrder.TotalOrderSteps = AppConsts.NONE;

            applicantOrder.TotalOrderSteps = totalSteps;
        }

        public void AddOrganizationUserProfile(OrganizationUserProfile organizationUserProfile, Boolean updatePersonalDetails, String clientMachineIP = null)
        {
            ApplicantOrder applicantOrder = GetApplicantOrder();
            applicantOrder.OrganizationUserProfile = organizationUserProfile;
            applicantOrder.UpdatePersonalDetails = updatePersonalDetails;

            if (clientMachineIP.IsNotNull())
            {
                applicantOrder.ClientMachineIP = clientMachineIP;
            }
        }

        public void AddCustomAttributeValues(List<TypeCustomAttributes> lstTypeCustomAttributes)
        {
            if (lstTypeCustomAttributes.IsNotNull() && lstTypeCustomAttributes.Count() > 0)
                this.lstCustomAttributeValues = lstTypeCustomAttributes;
        }

        public List<TypeCustomAttributes> GetCustomAttributeValues()
        {
            if (this.lstCustomAttributeValues.IsNotNull() && lstCustomAttributeValues.Count() > 0)
                return this.lstCustomAttributeValues;
            else
                return new List<TypeCustomAttributes>();
        }

        ///// <summary>
        ///// UAT 1438: Enhancement to allow students to select a User Group. 
        ///// </summary>
        ///// <returns></returns>
        //public List<Int32> GetCustomAttributeValuesForUserGroup()
        //{
        //    if (this.lstUserGroupCustomAttributeMapping.IsNotNull() && lstUserGroupCustomAttributeMapping.Count() > 0)
        //        return this.lstUserGroupCustomAttributeMapping;
        //    else
        //        return new List<ApplicantUserGroupMapping>();
        //}

        public void ClearCustomAttributeValues()
        {
            if (this.lstCustomAttributeValues.IsNotNull())
                lstCustomAttributeValues = null;
        }

        public void AddOrderStageTrackID(Int32 orderStageID)
        {
            ApplicantOrder applicantOrder = GetApplicantOrder();

            if (applicantOrder.LstOrderStageTrackID.IsNull())
            {
                applicantOrder.LstOrderStageTrackID = new List<Int32>()
                {
                    orderStageID
                };
            }
            else if (applicantOrder.LstOrderStageTrackID.Last() != orderStageID)
            {
                applicantOrder.LstOrderStageTrackID.Add(orderStageID);
            }
        }

        private OrderCartCompliancePackage getCompliancePackage()
        {
            if (CompliancePackages == null)
                CompliancePackages = new Dictionary<string, OrderCartCompliancePackage>();

            if (CompliancePackages.Count == 0)
            {
                CompliancePackages.Add(string.IsNullOrEmpty(CurrentCompliancePackageTypeInContext) ? string.Empty : CurrentCompliancePackageTypeInContext, new OrderCartCompliancePackage());
                return CompliancePackages.Values.First();
            }
            if (string.IsNullOrEmpty(CurrentCompliancePackageTypeInContext))
                return CompliancePackages.Values.First();

            if (CompliancePackages.Keys.Contains(CurrentCompliancePackageTypeInContext))
                return CompliancePackages[CurrentCompliancePackageTypeInContext];
            else
            {
                CompliancePackages.Add(CurrentCompliancePackageTypeInContext, new OrderCartCompliancePackage());
                return CompliancePackages[CurrentCompliancePackageTypeInContext];
            }

        }
        #endregion

        /// <summary>
        /// List of All the Orders
        /// </summary>
        public List<ApplicantOrder> lstApplicantOrder { get; set; }

        /// <summary>
        /// Will be true if ANY Compliance Package is selected 
        /// </summary>
        public Boolean IsCompliancePackageSelected { get; set; }

        public Boolean IsPaymentApprovalRequired { get; set; }
        public List<Int32> lstDepProgramMappingId { get; set; }

        public Int32 DepartmentId { get; set; }

        public Int32 AdminOrderSteps { get; set; }
        public String ApprovalPendingPackageName { get; set; }
        public Boolean IsAdminOrderCart { get; set; }

        public String EDrugScreeningRegistrationExpirationDate { get; set; }

        /// <summary>
        /// Tells whether this order process has multiple bundles or not
        /// </summary>
        public Boolean HasMultipleBundles
        {
            get
            {
                if (!this.lstSelectedPkgBundleId.IsNullOrEmpty())
                    return this.lstSelectedPkgBundleId.Count() > 1;
                else return false;
            }
        }
        /// <summary>
        /// Tells which bundle is in context. Used to single out a package on basis of administrative v/s tracking package when multiple bundles are part of order.
        /// </summary>
        public Int32 BundleInContext { get; set; }

        public Boolean IsOrderFlowConfirmation { get; set; }

        public Boolean IsModifyShipping { get; set; }

        public String ShippedService { get; set; }
        public Boolean IscabsFreshSelected { get; set; }
        public String ShippedServiceTrackingNumber { get; set; }

        public Boolean IsFromReschedulingScreen { get; set; }

        public bool IsModifyShippingPayment { get; set; }

        public Decimal MailingPrice { get; set; }
        public Boolean IsMailingOptionUpgraded { get; set; }
        public Boolean IsPaymentReqInMdfyShpng { get; set; }
        public bool IsFromNewOrderClick { get; set; }

        private String _CurrentCompliancePackageTypeInContext;
        public String CurrentCompliancePackageTypeInContext
        {
            get
            {
                if (!this.HasMultipleBundles)
                    return _CurrentCompliancePackageTypeInContext;
                else
                {
                    return String.Concat(_CurrentCompliancePackageTypeInContext, "_", Convert.ToString(this.BundleInContext));
                }
            }
            set
            {
                _CurrentCompliancePackageTypeInContext = value;
            }
        }

        public Dictionary<string, OrderCartCompliancePackage> CompliancePackages { get; set; }

        /// <summary>
        /// ID of the Node to which the CompliancePackages belong to. 
        /// </summary>
        public Int32 DPMId
        {
            get;
            set;
        }

        public Int32? DPP_Id
        {
            get
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp == null)
                    return null;
                return cp.DPP_Id;
            }
            set
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp != null)
                    cp.DPP_Id = value;
            }
        }

        public Decimal? CompliancePackagesGrandTotal
        {
            get
            {
                Decimal? total = 0;
                if (CompliancePackages.IsNotNull() && CompliancePackages.Count > 0)
                {
                    foreach (OrderCartCompliancePackage cp in CompliancePackages.Values)
                        total += (cp.GrandTotal.IsNull() ? 0 : cp.GrandTotal);
                    return total;
                }
                else
                    return null;
            }
        }

        public Decimal? GrandTotal
        {
            get
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp == null)
                    return null;
                return cp.GrandTotal;
            }
            set
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp != null)
                    cp.GrandTotal = value;
            }
        }

        public Int32 RenewalDuration
        {
            get
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp == null)
                    return 0;
                return cp.RenewalDuration;
            }
            set
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp != null)
                    cp.RenewalDuration = value;
            }
        }

        public String Amount
        {
            get
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp == null)
                    return null;
                return cp.Amount;
            }
            set
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp != null)
                    cp.Amount = value;
            }
        }

        public String RushOrderPrice
        {
            get
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp == null)
                    return null;
                return cp.RushOrderPrice;
            }
            set
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp != null)
                    cp.RushOrderPrice = value;
            }
        }

        public Boolean IsRushOrderIncluded
        {
            get
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp == null)
                    return false;
                return cp.IsRushOrderIncluded;
            }
            set
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp != null)
                    cp.IsRushOrderIncluded = value;
            }
        }

        public Dictionary<String, String> InvoiceNumber
        {
            get
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp == null)
                    return null;
                return cp.InvoiceNumber;
            }
            set
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp != null)
                    cp.InvoiceNumber = value;
            }
        }

        public Int32? ProgramDuration
        {
            get
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp == null)
                    return null;
                return cp.ProgramDuration;
            }
            set
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp != null)
                    cp.ProgramDuration = value;
            }
        }


        public Decimal? CurrentPackagePrice
        {
            get
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp == null)
                    return null;
                return cp.CurrentPackagePrice;
            }
            set
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp != null)
                    cp.CurrentPackagePrice = value;
            }
        }
        public Int32 PrevOrderId
        {
            get
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp == null)
                    return 0;
                return cp.PrevOrderId;
            }
            set
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp != null)
                    cp.PrevOrderId = value;
            }
        }

        public Int32 DPPS_ID
        {
            get
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp == null)
                    return 0;
                return cp.DPPS_ID;
            }
            set
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp != null)
                    cp.DPPS_ID = value;
            }
        }

        public Int32 OrderId
        {
            get
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp == null)
                    return 0;
                return cp.OrderId;
            }
            set
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp != null)
                    cp.OrderId = value;
            }
        }
        public string AllOrderIDs
        {
            get
            {
                if (CompliancePackages.IsNotNull() && CompliancePackages.Count > 0)
                {
                    string allOrderIDs = string.Empty;
                    foreach (OrderCartCompliancePackage cp in CompliancePackages.Values)
                        if (cp.OrderId > AppConsts.NONE)
                            allOrderIDs = allOrderIDs + ", " + cp.OrderId;

                    if (!allOrderIDs.IsNullOrEmpty())
                        return allOrderIDs.Substring(2);
                }
                else if (lstApplicantOrder.IsNotNull() && lstApplicantOrder.Count > 0)
                    return lstApplicantOrder[0].OrderId.ToString();
                return null;
            }
        }

        public string AllOrderNumbers
        {
            get
            {
                if (CompliancePackages.IsNotNull() && CompliancePackages.Count > 0)
                {
                    string allOrderIDs = string.Empty;
                    foreach (OrderCartCompliancePackage cp in CompliancePackages.Values)
                        if (!cp.OrderNumber.IsNullOrEmpty())
                            allOrderIDs = allOrderIDs + ", " + cp.OrderNumber;

                    if (!allOrderIDs.IsNullOrEmpty())
                        return allOrderIDs.Substring(2);
                }
                else if (lstApplicantOrder.IsNotNull() && lstApplicantOrder.Count > 0)
                    return lstApplicantOrder[0].OrderNumber.ToString();
                return null;
            }
        }

        public Int32 CompliancePackageID
        {
            get
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp == null)
                    return 0;
                return cp.CompliancePackageID;
            }
            set
            {
                OrderCartCompliancePackage cp = getCompliancePackage();
                if (cp != null)
                    cp.CompliancePackageID = value;
            }
        }

        public Boolean IsAccountUpdated { get; set; }

        public Boolean IsBiilingInfoSameAsAccountInfo { get; set; }

        public List<Int32> lstFormExecuted { get; set; }

        /// <summary>
        /// Institution Id of the last node selected in the pending order screen. Used to get the associated Custom attributes for this institution.
        /// </summary>
        public Int32 NodeId { get; set; }

        /// <summary>
        /// Store the CSV of the selected node id's on each level i.e for each combo
        /// </summary>
        public ArrayList alNodeIds { get; set; }

        public Decimal SettleAmount { get; set; }

        List<TypeCustomAttributes> lstCustomAttributeValues { get; set; }

        /// <summary>
        /// UAT 1438: Enhancement to allow students to select a User Group. 
        /// </summary>
        List<ApplicantUserGroupMapping> lstUserGroupCustomAttributeMapping { get; set; }

        /// <summary>
        /// Used to distinguish the Renewal, new order and change program orders
        /// </summary>
        public String OrderRequestType { get; set; }


        public Int32? DefaultNodeId { get; set; }




        public List<CustomFormDataContract> lstCustomFormData { get; set; }

        /// <summary>
        /// DPM-Id of the last node selected in the pending order hierarchy.
        /// </summary>
        public Int32? SelectedHierarchyNodeID { get; set; }

        public Int32? ApplicantDisclaimerDocumentId { get; set; }

        public List<Int32?> ApplicantDisclosureDocumentIds { get; set; }
        public List<PreviousAddressContract> lstPrevAddresses { get; set; }
        public PreviousAddressContract MailingAddress { get; set; }
        public List<PersonAliasContract> lstPersonAlias { get; set; }
        public Boolean IsResidentialHistoryVisible { get; set; }

        

    public Boolean IfInvoiceIsOnlyPaymentOptionAvailableAtNodeLevel { get; set; }

        public Boolean HidePaymntInstruction { get; set; }

        public Boolean IsRushOrder { get; set; }

        public Boolean IsBalancePayment { get; set; }

        public Int32 TenantId { get; set; }

        /// <summary>
        /// Property to identify whether the user is Editing any already filled details in Order flow
        /// </summary>
        public Boolean IsEditMode { get; set; }

        /// <summary>
        /// Maintains the Id of the CustomForm which is to be reloaded on Back button press from browser
        /// </summary>
        public Int32 CurrentCustomFormId { get; set; }

        /// <summary>
        /// RegistrationId of EDrug screening, used to manage the Back button after successful registration
        /// </summary>
        public String EDrugScreeningRegistrationId { get; set; }

        /// <summary>
        /// Property to maintain from which screen the Order was started i.e. Dashboard, LandingPage or Change subscription
        /// DO NOT CLEAR THIS ON RESTART ORDER  
        /// </summary>
        public String PendingOrderNavigationFrom { get; set; }

        /// <summary>
        /// used for checking whether user come from dashboard or not.
        /// </summary>
        public String ParentControlType { get; set; }

        /// <summary>
        /// Store the Grouped list of Payment Modes, total Amount and associated Packages.
        /// </summary>
        public List<PkgPaymentGrouping> lstPaymentGrouping { get; set; }

        /// <summary>
        /// Store the Payment Type code used in case of Applicant Change Payment type.
        /// Will be stored ONLY in case applicant changes Payment Type for a package. 
        /// </summary>
        public String ChangePaymentTypeCode
        {
            get;
            set;
        }

        /// <summary>
        /// Order payment detail id for which order card is created
        /// </summary>
        public Int32 OrderPaymentdetailId
        {
            get;
            set;
        }

        public Int32 ChangeSubscriptionCompliancePackageTypeId { get; set; }

        //UAT-3283
        //public Int32? SelectedPkgBundleId
        //{
        //    get;
        //    set;
        //}

        public List<Int32> lstSelectedPkgBundleId
        {
            get;
            set;
        }

        //END UAT-3283

        //UAT 1438
        public Boolean IsUserGroupCustomAttributeExist { get; set; }

        public List<Int32> lstCustomAttributeUserGroupIDs { get; set; }


        //public Int32 LastSlctdUSrGrpIDs { get; set; } //UAT-3455
        public List<Int32> lstPreviousSelectedUserGroupIds { get; set; }

        public IList<UserGroup> lstUsrGrpSavedValues { get; set; }


        /// <summary>
        /// Property to Identity that data should be readonly nothing on 'Order Review' screen is editable.
        /// </summary>
        public Boolean IsReadOnly { get; set; }

        /// <summary>
        /// Property identify the OPDs those are recently added in Applicant Completing Order Process for "SentForOnlinePayment".
        /// </summary>
        public List<Int32> RecentAddedOPDs { get; set; }

        /// <summary>
        /// Property identity the Required documentation (Additional Documents) id to save in Applicant Document.
        /// </summary>
        public List<Int32?> ApplicantAdditionalDocumentIds { get; set; }

        /// <summary>
        /// Property identity the Required documentation (Additional Documents) id need to send to student.
        /// </summary>
        public List<Int32?> AdditionalDocSendToStudent { get; set; }

        /// <summary>
        /// Property Identify that additional document exist for order or not
        /// </summary>
        public Boolean IsAdditionalDocumentExist { get; set; }

        /// <summary>
        /// Property to handle UAT 1760, to avoid duplicate transaction submission in authorize.net.
        /// Default will be 'False'.
        /// </summary>
        public Boolean IsPaymentResponsePending { get; set; }
        //UAT-1834
        public Boolean IsBulkOrder { get; set; }
        public Int32 BulkOrderUploadID { get; set; }

        //UAT-2625
        public String DisclosureAgeGroupType { get; set; }

        public FingerPrintAppointmentContract FingerPrintData { get; set; }

        public Boolean IsLocationServiceTenant { get; set; }
        public List<Int64> lstOldCustomerPaymentProfileId { get; set; }

        //UAT-3757
        public byte[] bufferSignature { get; set; }

        public String BKgPackagePasscode { get; set; } //UAT-3771
        //UAT-3745
        public List<SystemDocBkgSvcMapping> lstSystemDocBkgSvcMapping { get; set; }
        
        #region Admin Entry
        public String HierarchyNodeName { get; set; }

        /// <summary>
        /// Used For Admin Entry Portal - Applicant Order flow 
        /// </summary>
        /// <value>
        /// remains true for Admin Entry Portal
        /// </value>
        public Boolean IsAdminEntryPortalOrder { get; set; }       
        public List<OrderLineItem> lstOrderLineItems{ get; set; }


        #endregion

        //UAT-4558
        public Dictionary<Int32, Int32> dicApplicantDocSysDocMapping { get; set; }
        //END
    }

    [Serializable]
    public class PkgPaymentGrouping
    {
        /// <summary>
        /// Payment Mode Code of the Package i.e. Code of Client lkpPaymentOptions
        /// </summary>
        public String PaymentModeCode { get; set; }

        /// <summary>
        /// Payment Mode Id of the Package i.e. PK of Client lkpPaymentOptions
        /// </summary>
        public Int32 PaymentModeId { get; set; }

        /// <summary>
        /// Total Amount for the Grouped packages
        /// </summary>
        public Decimal TotalAmount { get; set; }


        /// <summary>
        /// Packages involved in particular Payment Mode. 
        /// Key is the Package_ID + GUID (Keep it unique) 
        /// Value is whether it is Background package or not.
        /// </summary>
        public Dictionary<String, Boolean> lstPackages
        {
            get;
            set;
        }

        //UAT 4537
        public Boolean IsApprovalRequiredPaymentGrouping { get; set; }

    }

    [Serializable]
    public class ApplicantOnlineTransactionDetails
    {
        public NameValueCollection lstTransactionDetails { get; set; }

        public void AddTransactionDetails(NameValueCollection transactionDetails)
        {
            lstTransactionDetails = new NameValueCollection();

            lstTransactionDetails.Add(transactionDetails);
        }
    }

    [Serializable]
    public class OrderSubscriptionIds
    {
        public int OrderId { get; set; }
    }
    [Serializable]
    public class OrderCartCompliancePackage
    {

        public Int32? DPP_Id { get; set; }

        /// <summary>
        /// Represents the Price of a particular Compliance Package
        /// </summary>
        public Decimal? GrandTotal { get; set; }

        public Int32 RenewalDuration { get; set; }

        public String Amount { get; set; }

        public String RushOrderPrice { get; set; }

        public Boolean IsRushOrderIncluded { get; set; }

        public Dictionary<String, String> InvoiceNumber { get; set; }

        public Int32? ProgramDuration { get; set; }

        public Decimal? CurrentPackagePrice { get; set; }

        public Int32 PrevOrderId { get; set; }

        public Int32 DPPS_ID { get; set; }

        public Int32 OrderId { get; set; }

        /// <summary>
        /// Represents the ID of a particular Compliance Package
        /// </summary>
        public Int32 CompliancePackageID { get; set; }

        public String PackageName { get; set; }

        public String SubscriptionPeriodMonths { get; set; }

        public String OrderNumber { get; set; }

    }

    [Serializable]
    [DataContract]
    public class FingerPrintAppointmentContract
    {
        //Added on Date : 26/09/2019
        [DataMember]
        public Int32 OrderId { get; set; }
        [DataMember]
        public Int32 LocationId { get; set; }
        [DataMember]
        public Int32? SlotID { get; set; }
        [DataMember]
        public Int32 EventSlotId { get; set; }
        [DataMember]
        public DateTime StartTime { get; set; }
        [DataMember]
        public DateTime EndTime { get; set; }
        [DataMember]
        public Int32 ReserverSlotID { get; set; }
        [DataMember]
        public DateTime SlotDate { get; set; }
        [DataMember]
        public String LocationName { get; set; }
        [DataMember]
        //public String LocationDescription { get; set; }
        public String LocationAddress { get; set; }
        [DataMember]
        public Boolean IsLocationServiceTenant { get; set; }
        [DataMember]
        public string LocationDescription { get; set; }
        [DataMember]
        public Boolean IsPrinterAvailable { get; set; }
        [DataMember]
        public bool IsEventCode { get; set; }
        [DataMember]
        public Int32 EventID { get; set; }
        [DataMember]
        public string EventName { get; set; }
        [DataMember]
        public string EventDescription { get; set; }
        [DataMember]
        public DateTime EventDate { get; set; }
        [DataMember]
        public string TempEventCode { get; set; }
        [DataMember]

        public String CBIUniqueID { get; set; }
        [DataMember]
        public Boolean IsSSNRequired { get; set; }
        [DataMember]
        public Dictionary<Int32, String> lstAutoFilledAttributes { get; set; }
        [DataMember]
        public bool IsOutOfState { get; set; }
        [DataMember]
        public String BillingCode { get; set; }
        [DataMember]
        public Decimal BillingCodeAmount { get; set; }
        [DataMember]
        public Boolean IsLegalNameChange { get; set; }
        [DataMember]
        public String EventCode { get; set; }
        [DataMember]
        public String AcctNameOrAcctNumber { get; set; }
        [DataMember]
        public List<LookupContract> lstCBIUniqueIds { get; set; }
        [DataMember]
        public String SelectedCBIUniqueId { get; set; }
        [DataMember]
        public Boolean IsLocationUpdate { get; set; }
        [DataMember]
        public Boolean IsPassportPhotoAvailable { get; set; }
        [DataMember]
        public Boolean IsConsent { get; set; }
        [DataMember]
        public bool IsLocationType { get { return !IsOutOfState && !IsEventCode; } }
        [DataMember]
        public bool IsFingerPrintAndPassPhotoService { get; set; }
        [DataMember]
        public bool IsMailingOnly { get; set; }
        [DataMember]
        public Int32 ArchivedOrderID { get; set; }
        [DataMember]
        public bool IsFromArchivedOrderScreen { get; set; }
        [DataMember]
        public Int32 ArchivedOrgUserID { get; set; }
    }

    [Serializable]
    [DataContract]
    public class CustomFormAutoFillDataContract
    {
        [DataMember]
        public Int32 AttributeGroupID { get; set; }
        [DataMember]
        public Int32 AttributeID { get; set; }
        [DataMember]
        public Int32 InstanceId { get; set; }
        [DataMember]
        public String HeaderLabel { get; set; }
        [DataMember]
        public Boolean IsRequired { get; set; }
        [DataMember]
        public Boolean IsEnabled { get; set; }
        [DataMember]
        public Boolean IsAttributeHidden { get; set; }
        [DataMember]
        public Boolean IsAttributeGroupHidden { get; set; }

    }

    [Serializable]
    [DataContract]
    public class RescheduleAppointmentInfo
    {
        [DataMember]
        public FingerPrintAppointmentContract locationDetail { get; set; }
        [DataMember]
        public OrderDetailsContract orderDetail { get; set; }

    }
}
