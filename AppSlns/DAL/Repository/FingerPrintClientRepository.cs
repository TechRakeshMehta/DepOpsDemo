using DAL.Interfaces;
using Entity.ClientEntity;
using Entity.LocationEntity;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.BkgOperations;
using System.Data.Entity;
using System.Xml;
using System.Xml.Linq;

namespace DAL.Repository
{
    public class FingerPrintClientRepository : ClientBaseRepository, IFingerPrintClientRepository
    {
        #region Variables
        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        #endregion

        #region Default Constructor to initilize DB Context

        ///// <summary>
        ///// Default constructor to initialize the context
        ///// </summary>
        public FingerPrintClientRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;

        }



        #endregion



        #region ApplicantOrderFlow

        Dictionary<Int32, Int32> IFingerPrintClientRepository.GetLocationHierarchy(Int32 locationID)
        {
            Dictionary<Int32, Int32> dicLocationHierarchy = new Dictionary<Int32, Int32>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[ams].[usp_GetLocationHierarchy]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LocationID", locationID);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    dicLocationHierarchy.Add(Convert.ToInt32(dr["ID"]), Convert.ToInt32(dr["NodeID"]));
                }
                con.Close();
            }
            return dicLocationHierarchy;
        }

        ReserveSlotContract IFingerPrintClientRepository.SubmitApplicantAppointment(ReserveSlotContract reserveSlotContract, Int32 currentLoggedInUserId, Boolean isCompleteYourOrderClick = false)
        {

            ReserveSlotContract reserveSlotData = new ReserveSlotContract();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                List<SqlParameter> sqlParameterCollection = new List<SqlParameter>
                        {
                              new SqlParameter("@SlotId", reserveSlotContract.SlotID),
                            new SqlParameter("@RS_ID",reserveSlotContract.ReservedSlotID),
                            new SqlParameter("@TenantID",reserveSlotContract.TenantID),
                            new SqlParameter("@ApplicantID",reserveSlotContract.AppOrgUserID),
                            new SqlParameter("@OrderID",reserveSlotContract.OrderID),
                            new SqlParameter("@LocationID",reserveSlotContract.LocationID),
                            new SqlParameter("@ServiceLineItem",reserveSlotContract.ServiceLineItemID),
                            new SqlParameter("@CurrentLoggedInUserID",currentLoggedInUserId),
                            new SqlParameter("@IsEventCode",reserveSlotContract.IsEventTypeCode),
                            new SqlParameter("@IsLocationUpdate",reserveSlotContract.IsLocationUpdate),
                            new SqlParameter("@IsOutOfState", reserveSlotContract.IsOutOfState),
                            new SqlParameter("@IsRejectReschedule",reserveSlotContract.IsRejectedReschedule),
                            new SqlParameter("@BillingCode",reserveSlotContract.BillingCode),
                            new SqlParameter("@CBIUniqueId",reserveSlotContract.CbiUniqueId),
                            new SqlParameter("@isCompleteYourOrderClick", isCompleteYourOrderClick),
                            new SqlParameter("@IsMailingOnly", reserveSlotContract.IsFingerPrintAndPassPhotoService),
                        };


                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "[ams].[usp_ConfirmAppointment]", sqlParameterCollection.ToArray()))
                {
                    if (dr.HasRows)
                    {
                        Guid guidParseResult = Guid.Empty;
                        while (dr.Read())
                        {
                            reserveSlotData.ApplicantAppointmentID = dr["ApplicantAppointmentID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ApplicantAppointmentID"]);
                            reserveSlotData.IsConfirmed = dr["IsConfirmed"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsConfirmed"]);
                            reserveSlotData.SlotID = dr["SlotID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["SlotID"]);
                            reserveSlotData.ErrorMsg = dr["ErrorMsg"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ErrorMsg"]);
                            reserveSlotData.SuccessMsg = dr["SuccessMsg"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SuccessMsg"]);
                        }
                    }
                }
            }

            return reserveSlotData;
        }

        Boolean IFingerPrintClientRepository.SubmitOrderBillingCodeMapping(Int32 orderID, String billingCode, Int32 currentLoggedInUserID)
        {
            Int32 CbiBillingStatusId = _dbContext.CBIBillingStatus.Where(cond => cond.CBS_BillingCode == billingCode
                                                                   && !cond.CBS_IsDeleted
                                                                   && cond.CBS_IsEnabled)
                                                                   .Select(cond => cond.CBS_ID)
                                                                   .FirstOrDefault();
            if (CbiBillingStatusId != AppConsts.NONE)
            {

                OrderBillingCodeMapping _billingCodeMapping = new OrderBillingCodeMapping
                {
                    OBCM_BillingCode = billingCode,
                    OBCM_CbiBillingStatusId = CbiBillingStatusId,
                    OBCM_OrderId = orderID,
                    OBCM_IsDeleted = false,
                    OBCM_CreatedBy = currentLoggedInUserID,
                    OBCM_CreatedOn = DateTime.Now
                };
                _dbContext.OrderBillingCodeMappings.AddObject(_billingCodeMapping);
                if (_dbContext.SaveChanges() > AppConsts.NONE)
                    return true;
                else
                    return false;
            }
            return false;

        }

        #endregion

        #region ApplicantAppointmentHistory
        Boolean IFingerPrintClientRepository.SaveUpdateApplicantAppointmentHistory(AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserId)
        {
            ApplicantAppointmentDetail applicantAppointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == scheduleAppointmentContract.OrderId
                                                                        && x.AAD_OrganizationUserID == scheduleAppointmentContract.ApplicantOrgUserId && !x.AAD_IsDeleted && x.AAD_IsActive).FirstOrDefault();


            FingerPrintApplicantAppointment cabsServiceDetail = base.LocationDBContext.FingerPrintApplicantAppointments.Where(x => x.FPAA_OrderID == scheduleAppointmentContract.OrderId
                                                                         && x.FPAA_OrganizationUserID == scheduleAppointmentContract.ApplicantOrgUserId && !x.FPAA_IsDeleted).FirstOrDefault();

            ApplicantAppointmentDetailsExt applicantAppointmentExt = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == applicantAppointmentDetail.AAD_ID
                                                                      && !x.AADE_IsDeleted && x.AADE_IsActive).FirstOrDefault();


            var appExt = new ApplicantAppointmentDetailsExt();
            ApplicantAppointmentDetail appAppointmentDetail = new ApplicantAppointmentDetail();
            if (applicantAppointmentDetail != null)
            {

                applicantAppointmentDetail.AAD_IsActive = false;
                applicantAppointmentDetail.AAD_ModifiedBy = currentLoggedInUserId;
                applicantAppointmentDetail.AAD_ModifiedOn = DateTime.Now;

                TimeSpan sTime = new TimeSpan();
                TimeSpan eTime = new TimeSpan();
                TimeSpan.TryParse(scheduleAppointmentContract.SlotStartTime, out sTime);
                TimeSpan.TryParse(scheduleAppointmentContract.SlotEndTime, out eTime);
                appAppointmentDetail.AAD_AppointmentStartTime = scheduleAppointmentContract.StartDateTime.HasValue ? scheduleAppointmentContract.StartDateTime.Value : (DateTime?)null;
                appAppointmentDetail.AAD_AppointmentEndTime = scheduleAppointmentContract.EndDateTime.HasValue ? scheduleAppointmentContract.EndDateTime.Value : (DateTime?)null;
                appAppointmentDetail.AAD_CreatedBy = currentLoggedInUserId;
                appAppointmentDetail.AAD_CreatedOn = DateTime.Now;
                appAppointmentDetail.AAD_IsActive = true;
                appAppointmentDetail.AAD_IsDeleted = false;
                appAppointmentDetail.AAD_OrderID = scheduleAppointmentContract.OrderId;
                appAppointmentDetail.AAD_OrganizationUserID = scheduleAppointmentContract.ApplicantOrgUserId;
                _dbContext.ApplicantAppointmentDetails.AddObject(appAppointmentDetail);

                appExt.AADE_IsActive = true;
                appExt.AADE_MailingAddressID = applicantAppointmentExt.AADE_MailingAddressID;
                appExt.AADE_MailingOptionID = applicantAppointmentExt.AADE_MailingOptionID;
                appExt.AADE_ServiceTypeID = applicantAppointmentExt.AADE_ServiceTypeID;
                appExt.AADE_TotalPrice = applicantAppointmentExt.AADE_TotalPrice;
                appExt.AADE_CreatedBy = currentLoggedInUserId;
                appExt.AADE_CreatedOn = DateTime.Now;
                appExt.AADE_IsReceiptDispatched = applicantAppointmentExt.AADE_IsReceiptDispatched;
                appAppointmentDetail.ApplicantAppointmentDetailsExts.Add(appExt);
                applicantAppointmentExt.AADE_IsActive = false;

            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
            {
                CabsUpdatePriceAndpayment(currentLoggedInUserId, appExt.AADE_ID, applicantAppointmentExt.AADE_ID);
                return true;
            }
            else
                return false;
        }


        Boolean IFingerPrintClientRepository.RescheduleApplicantAppointmentHistory(AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserId)
        {
            try
            {
                int cabsTypeStatusId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;
                ApplicantAppointmentDetail applicantAppointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == scheduleAppointmentContract.OrderId
                                                                            && x.AAD_OrganizationUserID == scheduleAppointmentContract.ApplicantOrgUserId && !x.AAD_IsDeleted && x.AAD_IsActive).FirstOrDefault();


                FingerPrintApplicantAppointment cabsServiceDetail = base.LocationDBContext.FingerPrintApplicantAppointments.Where(x => x.FPAA_OrderID == scheduleAppointmentContract.OrderId
                                                                             && x.FPAA_OrganizationUserID == scheduleAppointmentContract.ApplicantOrgUserId && !x.FPAA_IsDeleted).FirstOrDefault();

                List<ApplicantAppointmentDetailsExt> applicantAppointmentExts = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == applicantAppointmentDetail.AAD_ID
                                                                          && !x.AADE_IsDeleted && x.AADE_IsActive).ToList();


                if (applicantAppointmentDetail != null)
                {
                    ApplicantAppointmentDetail appAppointmentDetail = new ApplicantAppointmentDetail();
                    applicantAppointmentDetail.AAD_IsActive = false;
                    applicantAppointmentDetail.AAD_ModifiedBy = currentLoggedInUserId;
                    applicantAppointmentDetail.AAD_ModifiedOn = DateTime.Now;
                    TimeSpan sTime = new TimeSpan();
                    TimeSpan eTime = new TimeSpan();
                    TimeSpan.TryParse(scheduleAppointmentContract.SlotStartTime, out sTime);
                    TimeSpan.TryParse(scheduleAppointmentContract.SlotEndTime, out eTime);
                    appAppointmentDetail.AAD_AppointmentStartTime = scheduleAppointmentContract.StartDateTime.HasValue ? scheduleAppointmentContract.StartDateTime.Value : (DateTime?)null;
                    appAppointmentDetail.AAD_AppointmentEndTime = scheduleAppointmentContract.EndDateTime.HasValue ? scheduleAppointmentContract.EndDateTime.Value : (DateTime?)null;
                    appAppointmentDetail.AAD_CreatedBy = currentLoggedInUserId;
                    appAppointmentDetail.AAD_CreatedOn = DateTime.Now;
                    appAppointmentDetail.AAD_IsActive = true;
                    appAppointmentDetail.AAD_IsDeleted = false;
                    appAppointmentDetail.AAD_OrderID = scheduleAppointmentContract.OrderId;
                    appAppointmentDetail.AAD_OrganizationUserID = scheduleAppointmentContract.ApplicantOrgUserId;
                    _dbContext.AddToApplicantAppointmentDetails(appAppointmentDetail);
                    _dbContext.SaveChanges();
                    int cabsStatus = cabsServiceDetail.FPAA_Status.Value;
                    int CabsNewStatus = base.LocationDBContext.lkpAppointmentStatus.Where(x => x.AS_Code == "AAAA").FirstOrDefault().AS_ID;
                    // Status For Addittional Services
                    List<lkpCABSServiceStatu> AllAdditionalStatus = _dbContext.lkpCABSServiceStatus.ToList();
                    int RejectAdditional = AllAdditionalStatus.Where(x => x.CSS_Code == "AAAD").FirstOrDefault().CSS_ID;
                    int NewAdditional = AllAdditionalStatus.Where(x => x.CSS_Code == "AAAE").FirstOrDefault().CSS_ID;

                    if (applicantAppointmentExts.IsNotNull())
                    {
                        foreach (var applicantAppointmentExtItems in applicantAppointmentExts)
                        {
                            var appExt = new ApplicantAppointmentDetailsExt();
                            appExt.AADE_IsActive = applicantAppointmentExtItems.AADE_IsActive;
                            appExt.AADE_MailingAddressID = applicantAppointmentExtItems.AADE_MailingAddressID;
                            appExt.AADE_MailingOptionID = applicantAppointmentExtItems.AADE_MailingOptionID;
                            appExt.AADE_MailingPrice = applicantAppointmentExtItems.AADE_MailingPrice;
                            appExt.AADE_ServiceTypeID = applicantAppointmentExtItems.AADE_ServiceTypeID;
                            appExt.AADE_TotalPrice = applicantAppointmentExtItems.AADE_TotalPrice;
                            appExt.AADE_CreatedBy = currentLoggedInUserId;
                            appExt.AADE_CreatedOn = DateTime.Now;
                            #region cabs type 
                            if (applicantAppointmentExtItems.AADE_ServiceTypeID == cabsTypeStatusId)
                            {
                                String Status = base.LocationDBContext.lkpAppointmentStatus.Where(x => x.AS_ID == cabsServiceDetail.FPAA_Status).FirstOrDefault().AS_Code;
                                if (!(Status != FingerPrintAppointmentStatus.FINGERPRINTS_COMPLETED_SUCCESSFULLY.GetStringValue()
                                    && Status != FingerPrintAppointmentStatus.COMPLETED.GetStringValue()
                                    ))
                                {
                                    appExt.AADE_ParentAppAppointmentDetailID = applicantAppointmentExtItems.AADE_AppAppointmentDetailID;
                                    appExt.AADE_DispatchedDate = applicantAppointmentExtItems.AADE_DispatchedDate;
                                    appExt.AADE_RetryCount = applicantAppointmentExtItems.AADE_RetryCount;
                                    appExt.AADE_CBIPCNNumber = applicantAppointmentExtItems.AADE_CBIPCNNumber;
                                    appExt.AADE_FingerprintReceiptID = applicantAppointmentExtItems.AADE_FingerprintReceiptID;
                                    appExt.AADE_IsReceiptDispatched = applicantAppointmentExtItems.AADE_IsReceiptDispatched;
                                    appExt.AADE_ReferencedOrderID = applicantAppointmentExtItems.AADE_ReferencedOrderID;
                                    appExt.AADE_TrackingNumber = applicantAppointmentExtItems.AADE_TrackingNumber;
                                    appExt.AADE_SubmittedToCBI = applicantAppointmentExtItems.AADE_SubmittedToCBI;
                                }
                            }
                            #endregion cabs type 
                            if (applicantAppointmentExtItems.AADE_ServiceTypeID != cabsTypeStatusId)
                            {
                                if (applicantAppointmentExtItems.AADE_ServiceStatus == RejectAdditional || applicantAppointmentExtItems.AADE_ServiceStatus == NewAdditional)
                                {
                                    appExt.AADE_ServiceStatus = NewAdditional;
                                }
                                else
                                {
                                    appExt.AADE_ServiceStatus = applicantAppointmentExtItems.AADE_ServiceStatus;
                                    appExt.AADE_ParentAppAppointmentDetailID = applicantAppointmentExtItems.AADE_AppAppointmentDetailID;
                                    appExt.AADE_DispatchedDate = applicantAppointmentExtItems.AADE_DispatchedDate;
                                    appExt.AADE_RetryCount = applicantAppointmentExtItems.AADE_RetryCount;
                                    appExt.AADE_FingerprintReceiptID = applicantAppointmentExtItems.AADE_FingerprintReceiptID;
                                    appExt.AADE_IsReceiptDispatched = applicantAppointmentExtItems.AADE_IsReceiptDispatched;
                                    appExt.AADE_ReferencedOrderID = applicantAppointmentExtItems.AADE_ReferencedOrderID;
                                    appExt.AADE_TrackingNumber = applicantAppointmentExtItems.AADE_TrackingNumber;
                                }
                            }
                            appExt.AADE_AppAppointmentDetailID = appAppointmentDetail.AAD_ID;
                            applicantAppointmentExtItems.AADE_IsActive = false;
                            _dbContext.AddToApplicantAppointmentDetailsExts(appExt);
                            if (_dbContext.SaveChanges() > AppConsts.NONE)
                            {
                                CabsUpdatePriceAndpayment(currentLoggedInUserId, appExt.AADE_ID, applicantAppointmentExtItems.AADE_ID);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void CabsUpdatePriceAndpayment(int currentLoggedInUserId, int NewAADE_ID, int OldAADE_ID)
        {

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@CurrentLoggedInUserID",currentLoggedInUserId),
                            new SqlParameter("@OldAADE_id",OldAADE_ID),
                            new SqlParameter("@NewAADE_Id", NewAADE_ID ),
                        };
                base.OpenSQLDataReaderConnection(con);
                base.ExecuteSQLDataReader(con, "ams.usp_CabsUpdatePriceAndpayment", sqlParameterCollection);

            }
        }

        public void UpdateApplicantAppointmentExt(Int32 OrderID, PreviousAddressContract mailingAddress, Boolean isLocationServiceTenant, Int32 orgUserID, bool IsPaymentReqInMdfyShpng, Decimal MailingPrice = 0)
        {
            List<lkpCABSServiceStatu> cABSServiceStatusList = _dbContext.lkpCABSServiceStatus.ToList();
            int _cabsTypeStatusId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;
            int _returnToSender = cABSServiceStatusList.Where(x => x.CSS_Code == "AAAC").FirstOrDefault().CSS_ID;
            int _newStatus = cABSServiceStatusList.Where(x => x.CSS_Code == "AAAE").FirstOrDefault().CSS_ID;
            int _pendingShippment = cABSServiceStatusList.Where(x => x.CSS_Code == "AAAA").FirstOrDefault().CSS_ID;
            string oldStatusName = cABSServiceStatusList.Where(x => x.CSS_ID == _returnToSender).Select(x => x.CSS_Name).FirstOrDefault();
            string newStatusName = cABSServiceStatusList.Where(x => x.CSS_ID == _pendingShippment).Select(x => x.CSS_Name).FirstOrDefault(); ;
            ApplicantAppointmentDetail applicantAppointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == OrderID
                                                                        && x.AAD_OrganizationUserID == orgUserID && !x.AAD_IsDeleted && x.AAD_IsActive).FirstOrDefault();

            List<ApplicantAppointmentDetailsExt> applicantAppointmentExt = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == applicantAppointmentDetail.AAD_ID && x.AADE_ServiceTypeID != _cabsTypeStatusId && (x.AADE_ServiceStatus == _returnToSender || x.AADE_ServiceStatus == _newStatus)
                                                                      && !x.AADE_IsDeleted && x.AADE_IsActive).ToList();
            lkpAppointmentChangeType changeTypelist = _dbContext.lkpAppointmentChangeTypes.FirstOrDefault(lkp => lkp.ACT_Code == "AAAK");
            int locationId = LocationDBContext.FingerPrintApplicantAppointments.Where(x => x.FPAA_OrderID == OrderID).Select(x => x.FPAA_LocationID).FirstOrDefault();
            int previousAppAppointmentDetailExtId = 0;
            if (applicantAppointmentDetail != null && applicantAppointmentExt.IsNotNull())
            {
                foreach (var applicantAppointmentExtItems in applicantAppointmentExt)
                {
                    if (!(applicantAppointmentExtItems.AADE_ServiceTypeID == _cabsTypeStatusId))
                    {
                        previousAppAppointmentDetailExtId = applicantAppointmentExtItems.AADE_ID;
                        LocationServiceAppointmentAudit locationServiceAppAudit = new LocationServiceAppointmentAudit
                        {
                            LSAA_AppointmentID = applicantAppointmentExtItems.AADE_AppAppointmentDetailID,
                            LSAA_ChangeTypeID = changeTypelist.ACT_ID,
                            LSAA_CreatedBy = orgUserID,
                            LSAA_CreatedOn = DateTime.Now,
                            LSAA_DateUpdated = DateTime.Now,
                            LSAA_Description = "Modify Shipping > Shipping details has been modifed.",
                            LSAA_IsDeleted = false,
                            LSAA_IsOnsite = false,
                            LSAA_IsOutOfState = false,
                            LSAA_LocationId = locationId,
                            LSAA_NewValue = "PreviousShipping",
                            LSAA_OldValue = "NewShipping"
                        };
                        _dbContext.LocationServiceAppointmentAudits.AddObject(locationServiceAppAudit);
                        if (applicantAppointmentExtItems.AADE_ServiceStatus == _returnToSender || applicantAppointmentExtItems.AADE_ServiceStatus == _newStatus)
                        {
                            var previousAppExt = applicantAppointmentExt;
                            var appExt = new ApplicantAppointmentDetailsExt();
                            Decimal totalPrice = Decimal.Add(Convert.ToDecimal(applicantAppointmentExtItems.AADE_TotalPrice - applicantAppointmentExtItems.AADE_MailingPrice), mailingAddress.MailingOptionPriceOnly);
                            appExt.AADE_IsActive = true;
                            appExt.AADE_AppAppointmentDetailID = applicantAppointmentExtItems.AADE_AppAppointmentDetailID;
                            appExt.AADE_IsReceiptDispatched = applicantAppointmentExtItems.AADE_IsReceiptDispatched;
                            appExt.AADE_DispatchedDate = applicantAppointmentExtItems.AADE_DispatchedDate;
                            appExt.AADE_TrackingNumber = applicantAppointmentExtItems.AADE_TrackingNumber;
                            appExt.AADE_MailingAddressID = mailingAddress.MailingAddressId;
                            appExt.AADE_MailingOptionID = Convert.ToInt32(mailingAddress.MailingOptionId);
                            appExt.AADE_ServiceTypeID = applicantAppointmentExtItems.AADE_ServiceTypeID;
                            appExt.AADE_MailingPrice = mailingAddress.MailingOptionPriceOnly;
                            appExt.AADE_ParentAppAppointmentDetailID = applicantAppointmentExtItems.AADE_ParentAppAppointmentDetailID;
                            appExt.AADE_TotalPrice = applicantAppointmentExtItems.AADE_TotalPrice.IsNullOrEmpty() ? applicantAppointmentExtItems.AADE_TotalPrice : totalPrice;
                            if (applicantAppointmentExtItems.AADE_ServiceStatus == _returnToSender && !IsPaymentReqInMdfyShpng)
                            {
                                appExt.AADE_ServiceStatus = _pendingShippment;
                                LocationServiceAppointmentAudit locationServiceAppointmentAudit = new LocationServiceAppointmentAudit
                                {
                                    LSAA_AppointmentID = appExt.AADE_AppAppointmentDetailID,
                                    LSAA_ChangeTypeID = changeTypelist.ACT_ID,
                                    LSAA_CreatedBy = orgUserID,
                                    LSAA_CreatedOn = DateTime.Now,
                                    LSAA_DateUpdated = DateTime.Now,
                                    LSAA_Description = "Additional Service Status Change > " + oldStatusName + " --> " + newStatusName,
                                    LSAA_IsDeleted = false,
                                    LSAA_IsOnsite = false,
                                    LSAA_IsOutOfState = false,
                                    LSAA_LocationId = locationId,
                                    LSAA_NewValue = _pendingShippment.ToString(),
                                    LSAA_OldValue = _returnToSender.ToString()
                                };
                                _dbContext.LocationServiceAppointmentAudits.AddObject(locationServiceAppointmentAudit);
                            }
                            else
                            {
                                appExt.AADE_ServiceStatus = applicantAppointmentExtItems.AADE_ServiceStatus;
                            }
                            appExt.AADE_CreatedBy = orgUserID;
                            appExt.AADE_CreatedOn = DateTime.Now;
                            appExt.AADE_ABIPCNNumber = applicantAppointmentExtItems.AADE_ABIPCNNumber;
                            appExt.AADE_CBIPCNNumber = applicantAppointmentExtItems.AADE_CBIPCNNumber;
                            appExt.AADE_FingerPrintFileDocumentID = applicantAppointmentExtItems.AADE_FingerPrintFileDocumentID;
                            applicantAppointmentDetail.ApplicantAppointmentDetailsExts.Add(appExt);
                            applicantAppointmentExtItems.AADE_IsActive = false;


                        }
                    }
                }
            }
            _dbContext.SaveChanges();

            if (!IsPaymentReqInMdfyShpng)
            {
                EntityConnection connection = _dbContext.Connection as EntityConnection;
                int newApplicantAppointmentExtId = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == applicantAppointmentDetail.AAD_ID && x.AADE_ServiceTypeID != _cabsTypeStatusId && (x.AADE_ServiceStatus == _pendingShippment || x.AADE_ServiceStatus == _newStatus)
                                                                       && !x.AADE_IsDeleted && x.AADE_IsActive).Select(x => x.AADE_ID).FirstOrDefault();

                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@CurrentLoggedInUserID",orgUserID),
                            new SqlParameter("@OldAADE_id",previousAppAppointmentDetailExtId),
                            new SqlParameter("@NewAADE_Id",newApplicantAppointmentExtId),
                        };
                    base.OpenSQLDataReaderConnection(con);
                    base.ExecuteSQLDataReader(con, "ams.usp_CabsUpdatePriceAndpayment", sqlParameterCollection);
                }
            }
        }

        public void UpdateApplicantAppointmentDetailExt(Int32 OrderID, PreviousAddressContract mailingAddress, Boolean isLocationServiceTenant, Int32 orgUserID)
        {
            int _cabsTypeStatusId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;
            int _returnToSender = _dbContext.lkpCABSServiceStatus.Where(x => x.CSS_Code == "AAAC").FirstOrDefault().CSS_ID;
            int _newStatus = _dbContext.lkpCABSServiceStatus.Where(x => x.CSS_Code == "AAAE").FirstOrDefault().CSS_ID;
            int _pendingShippment = _dbContext.lkpCABSServiceStatus.Where(x => x.CSS_Code == "AAAA").FirstOrDefault().CSS_ID;

            ApplicantAppointmentDetail applicantAppointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == OrderID
                                                                        && x.AAD_OrganizationUserID == orgUserID && !x.AAD_IsDeleted && x.AAD_IsActive).FirstOrDefault();

            List<ApplicantAppointmentDetailsExt> applicantAppointmentExt = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == applicantAppointmentDetail.AAD_ID && x.AADE_ServiceTypeID != _cabsTypeStatusId && (x.AADE_ServiceStatus == _returnToSender || x.AADE_ServiceStatus == _newStatus)
                                                                      && !x.AADE_IsDeleted && x.AADE_IsActive).ToList();

            if (applicantAppointmentDetail != null && applicantAppointmentExt.IsNotNull())
            {
                foreach (var applicantAppointmentExtItems in applicantAppointmentExt)
                {
                    if (!(applicantAppointmentExtItems.AADE_ServiceTypeID == _cabsTypeStatusId))
                    {
                        if (applicantAppointmentExtItems.AADE_ServiceStatus == _returnToSender || applicantAppointmentExtItems.AADE_ServiceStatus == _newStatus)
                        {
                            if (applicantAppointmentExtItems.AADE_ServiceStatus == _returnToSender)
                            {
                                applicantAppointmentExtItems.AADE_ServiceStatus = _pendingShippment;
                                List<lkpCABSServiceStatu> cABSServiceStatusList = _dbContext.lkpCABSServiceStatus.ToList();
                                string oldStatusName = cABSServiceStatusList.Where(x => x.CSS_ID == _returnToSender).Select(x => x.CSS_Name).FirstOrDefault();
                                string newStatusName = cABSServiceStatusList.Where(x => x.CSS_ID == _pendingShippment).Select(x => x.CSS_Name).FirstOrDefault();
                                lkpAppointmentChangeType changeTypelist = _dbContext.lkpAppointmentChangeTypes.FirstOrDefault(lkp => lkp.ACT_Code == "AAAK");
                                int locationId = LocationDBContext.FingerPrintApplicantAppointments.Where(x => x.FPAA_OrderID == OrderID).Select(x => x.FPAA_LocationID).FirstOrDefault();
                                LocationServiceAppointmentAudit locationServiceAppointmentAudit = new LocationServiceAppointmentAudit
                                {
                                    LSAA_AppointmentID = applicantAppointmentExtItems.AADE_AppAppointmentDetailID,
                                    LSAA_ChangeTypeID = changeTypelist.ACT_ID,
                                    LSAA_CreatedBy = orgUserID,
                                    LSAA_CreatedOn = DateTime.Now,
                                    LSAA_DateUpdated = DateTime.Now,
                                    LSAA_Description = "Additional Service Status Change > " + oldStatusName + " --> " + newStatusName,
                                    LSAA_IsDeleted = false,
                                    LSAA_IsOnsite = false,
                                    LSAA_IsOutOfState = false,
                                    LSAA_LocationId = locationId,
                                    LSAA_NewValue = _pendingShippment.ToString(),
                                    LSAA_OldValue = _returnToSender.ToString()
                                };
                                _dbContext.LocationServiceAppointmentAudits.AddObject(locationServiceAppointmentAudit);
                            }
                            else
                            {
                                applicantAppointmentExtItems.AADE_ServiceStatus = applicantAppointmentExtItems.AADE_ServiceStatus;
                            }
                            applicantAppointmentExtItems.AADE_ModifiedOn = DateTime.Now;
                            applicantAppointmentExtItems.AADE_ModifiedBy = orgUserID;
                        }

                    }
                }
            }
            _dbContext.SaveChanges();
        }

        Address IFingerPrintClientRepository.GetMailingAddressDetails(Int32 OrderID, Int32 orgUserID)
        {
            int cabsTypeStatusId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;
            Address address = null;
            Int32 applicantAppointmentDetailId = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == OrderID
                                                                       && x.AAD_OrganizationUserID == orgUserID && !x.AAD_IsDeleted && x.AAD_IsActive).Select(x => x.AAD_ID).FirstOrDefault(); ;

            var mailingAddressId = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == applicantAppointmentDetailId && x.AADE_ServiceTypeID != cabsTypeStatusId
                                                                     && !x.AADE_IsDeleted && x.AADE_IsActive).Select(x => x.AADE_MailingAddressID).FirstOrDefault();

            if (mailingAddressId.IsNotNull())
            {
                address = _dbContext.Addresses.Include("AddressExts").Where(x => x.AddressID == mailingAddressId && x.IsActive).FirstOrDefault();
            }
            return address;
        }

        decimal IFingerPrintClientRepository.GetOrderPriceTotal(Int32 OrderID, Int32 orgUserID)
        {
            decimal orderPrice = 0.0M;
            decimal? orderTotalPrice = _dbContext.vwOrderDetails.Where(obj => obj.OrderId == OrderID && obj.OrganizationUserID == orgUserID).Select(obj => obj.Amount).FirstOrDefault();
            orderPrice = orderTotalPrice ?? 0;
            return orderPrice;
        }

        string IFingerPrintClientRepository.GetOrderNumber(Int32 OrderID, Int32 orgUserID)
        {
            return _dbContext.vwOrderDetails.Where(obj => obj.OrderId == OrderID && obj.OrganizationUserID == orgUserID).Select(obj => obj.OrderNumber).FirstOrDefault();
        }
        SelectedMailingData IFingerPrintClientRepository.GetSelectedMailingOptionPrice(Int32 OrderID, Int32 orgUserID)
        {
            SelectedMailingData selectedData = new SelectedMailingData();
            ApplicantAppointmentDetailsExt selectedMailingData = null;
            int cabsTypeStatusId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;
            Int32 applicantAppointmentDetailId = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == OrderID
                                                                       && x.AAD_OrganizationUserID == orgUserID && !x.AAD_IsDeleted && x.AAD_IsActive).Select(x => x.AAD_ID).FirstOrDefault(); ;

            selectedMailingData = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == applicantAppointmentDetailId && x.AADE_ServiceTypeID != cabsTypeStatusId
                                                                    && !x.AADE_IsDeleted && x.AADE_IsActive).FirstOrDefault();
            if (selectedMailingData.IsNotNull())
            {
                selectedData.SelectedMailingOptionId = selectedMailingData.AADE_MailingOptionID;
                selectedData.SelectedMailingOptionPrice = selectedMailingData.AADE_MailingPrice;
            }
            return selectedData;
        }

        decimal IFingerPrintClientRepository.GetSentForOnlinePaymentAmount(Int32 OrderID)
        {
            Int32 orderPaymentStatus = _dbContext.lkpOrderStatus.Where(x => x.Code == "OSPMS").Select(x => x.OrderStatusID).FirstOrDefault();
            decimal amount;
            if (OrderID > 0)
            {
                decimal? sfopAmount = _dbContext.OrderPaymentDetails.Where(x => x.OPD_OrderID == OrderID && x.OPD_OrderStatusID == orderPaymentStatus && !x.OPD_IsDeleted).OrderByDescending(x => x.OPD_ID).Select(x => x.OPD_Amount).FirstOrDefault();
                amount = sfopAmount ?? 0;
                return amount;
            }
            return Convert.ToDecimal(0.00);
        }

        /// <summary>
        /// To get address detail in case of fingerprint card and passport are to be mailed.
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="orgUserID"></param>
        /// <returns></returns>

        PreviousAddressContract IFingerPrintClientRepository.GetAddressData(Int32 OrderID, Int32 orgUserID)
        {
            Address address = null;
            OrganizationUserProfile organisationDetail = null;
            PreviousAddressContract addressDetail = new PreviousAddressContract();
            var orderDetail = _dbContext.Orders.Where(o => o.OrderID == OrderID).FirstOrDefault();
            if (!orderDetail.IsNullOrEmpty())
            {
                organisationDetail = _dbContext.OrganizationUserProfiles.Where(o => o.OrganizationUserProfileID == orderDetail.OrganizationUserProfileID).FirstOrDefault();
            }
            if (!organisationDetail.IsNullOrEmpty())
            {
                address = _dbContext.Addresses.Where(a => a.AddressHandleID == organisationDetail.AddressHandleID && a.IsActive).FirstOrDefault();
            }
            if (!address.IsNullOrEmpty())
            {
                var addressExt = _dbContext.AddressExts.Where(x => x.AE_AddressID == address.AddressID).FirstOrDefault();
                if (!addressExt.IsNullOrEmpty())
                {
                    addressDetail.ZipCodeID = address.ZipCodeID == null ? 0 : Convert.ToInt32(address.ZipCodeID);
                    addressDetail.Zipcode = addressExt.AE_ZipCode;
                    addressDetail.StateName = addressExt.AE_StateName;
                    addressDetail.CityName = addressExt.AE_CityName;
                    addressDetail.CountryId = addressExt.AE_CountryID;
                    addressDetail.Address1 = address.Address1;
                }
            }
            return addressDetail;
        }

        PreviousAddressContract IFingerPrintClientRepository.GetShippingAddressData(Int32 OrderID, Int32 orgUserID)
        {
            PreviousAddressContract addressDetail = new PreviousAddressContract();
            String mailingOptionID = string.Empty;
            XmlNodeList xmlnode;
            Guid addressHandleID = new Guid();
            var cabsServiceDetail = _dbContext.CABSServiceOrderDetails.Where(y => y.CSOD_OrderID == OrderID).Select(y => y.CSOD_ServiceDetails).FirstOrDefault();
            if (cabsServiceDetail != null)
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(cabsServiceDetail);
                xmlnode = xdoc.GetElementsByTagName("BkgpkgData");
                for (int i = 0; i <= xmlnode.Count - 1; i++)
                {
                    if (xmlnode[i].ChildNodes.Count > 8)
                    {
                        string mailingOptionId = xmlnode[i].ChildNodes.Item(8).InnerText.Trim();
                        string mailingAddress = xmlnode[i].ChildNodes.Item(10).InnerText.Trim();
                        if (!mailingAddress.IsNullOrEmpty() && !mailingOptionId.IsNullOrEmpty())
                        {
                            mailingOptionID = mailingOptionId;
                            addressHandleID = new Guid(mailingAddress);
                        }
                    }
                }
            }
            var address = _dbContext.Addresses.Where(a => a.AddressHandleID == addressHandleID && a.IsActive).FirstOrDefault();

            if (!address.IsNullOrEmpty())
            {
                var addressExt = _dbContext.AddressExts.Where(x => x.AE_AddressID == address.AddressID).FirstOrDefault();
                if (!addressExt.IsNullOrEmpty())
                {
                    addressDetail.ZipCodeID = address.ZipCodeID == null ? 0 : Convert.ToInt32(address.ZipCodeID);
                    addressDetail.Zipcode = addressExt.AE_ZipCode;
                    addressDetail.StateName = addressExt.AE_StateName;
                    addressDetail.CityName = addressExt.AE_CityName;
                    addressDetail.CountryId = addressExt.AE_CountryID;
                    addressDetail.Address1 = address.Address1;
                    addressDetail.MailingOptionId = mailingOptionID;
                }
            }
            return addressDetail;
        }

        void IFingerPrintClientRepository.UpdateMailingDetailXML(Int32 orderID, Guid mailingAddressHandleId, String mailingoptionID, String mailingOptionPrice)
        {
            XmlNodeList xmlnode;
            var cabsServiceDetail = _dbContext.CABSServiceOrderDetails.Where(y => y.CSOD_OrderID == orderID).Select(y => y.CSOD_ServiceDetails).FirstOrDefault();
            if (cabsServiceDetail != null)
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(cabsServiceDetail);
                xmlnode = xdoc.GetElementsByTagName("BkgpkgData");
                for (int i = 0; i <= xmlnode.Count - 1; i++)
                {
                    if (xmlnode[i].ChildNodes.Count > 8)
                    {
                        string mailingOptionId = xmlnode[i].ChildNodes.Item(8).InnerText.Trim();
                        string mailingAddress = xmlnode[i].ChildNodes.Item(10).InnerText.Trim();
                        if (!mailingAddress.IsNullOrEmpty() && !mailingOptionId.IsNullOrEmpty())
                        {
                            xmlnode[i].ChildNodes.Item(8).InnerText = mailingOptionId;
                            xmlnode[i].ChildNodes.Item(9).InnerText = mailingOptionPrice;
                            xmlnode[i].ChildNodes.Item(10).InnerText = mailingAddressHandleId.ToString();
                        }
                    }
                }
            }
        }

        Int32 IFingerPrintClientRepository.GetOrderHeirarchyNodeId(int OrderID)
        {
            var order = _dbContext.Orders.Where(o => o.OrderID == OrderID && !o.IsDeleted).FirstOrDefault();
            if (!order.IsNullOrEmpty())
            {
                var heirarchyNodeId = order.HierarchyNodeID ?? AppConsts.MINUS_ONE;
                return heirarchyNodeId;
            }
            return AppConsts.MINUS_ONE;
        }

        Order IFingerPrintClientRepository.GetOrderByOrderId(int OrderID)
        {
            Order order = _dbContext.Orders.Where(o => o.OrderID == OrderID && !o.IsDeleted).FirstOrDefault();
            return order;
        }

        bool IsAADRow(bool cabsServiceInProgress, int AdditionalServices)
        {
            if (cabsServiceInProgress) return false;
            if (AdditionalServices > 0) return false;

            return true;
        }


        bool IsCabsserviceInProgress(String Status, int OrderId)
        {

            BkgOrder bkgorder = _dbContext.BkgOrders.Where(x => x.BOR_MasterOrderID == OrderId
                                                                     && !x.BOR_IsDeleted)
                                                                     .FirstOrDefault();

            if (bkgorder != null)
            {
                string OrderStatus = _dbContext.lkpOrderStatusTypes.Where(x => x.OrderStatusTypeID == bkgorder.BOR_OrderStatusTypeID)
                                                                     .FirstOrDefault().Code;
                if (OrderStatus == "AAAB")
                {
                    return true;
                }
            }



            List<String> AppointmentStatusList = new List<String>();
            AppointmentStatusList.Add(FingerPrintAppointmentStatus.ACTIVE.GetStringValue());
            AppointmentStatusList.Add(FingerPrintAppointmentStatus.MISSED.GetStringValue());
            AppointmentStatusList.Add(FingerPrintAppointmentStatus.MISSED_AND_NOTIFIED.GetStringValue());
            AppointmentStatusList.Add(FingerPrintAppointmentStatus.REJECTED_BY_ABI.GetStringValue());
            AppointmentStatusList.Add(FingerPrintAppointmentStatus.REVOKED.GetStringValue());
            AppointmentStatusList.Add(FingerPrintAppointmentStatus.REVOKED_AND_NOTIFIED.GetStringValue());
            AppointmentStatusList.Add(FingerPrintAppointmentStatus.REJECTED_BY_CBI.GetStringValue());
            AppointmentStatusList.Add(FingerPrintAppointmentStatus.REJECTED_BY_FBI.GetStringValue());
            AppointmentStatusList.Add(FingerPrintAppointmentStatus.MANUALLY_REJECTED_ORDER.GetStringValue());
            if (AppointmentStatusList.Contains(Status))
            {
                return false;
            }
            else
            {

                return true;
            }
        }


        Boolean IFingerPrintClientRepository.ResetApplicantAppointmenBkgOrderStatus(AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserId)
        {
            var bkgOrders = _dbContext.BkgOrders.Where(x => x.BOR_MasterOrderID == scheduleAppointmentContract.OrderId
                                                                        && !x.BOR_IsDeleted).FirstOrDefault();
            if (bkgOrders != null)
            {
                var code = OrderStatusType.NEW.GetStringValue();
                var NewOrderStatusTypeID = _dbContext.lkpOrderStatusTypes.First(s => s.Code == code).OrderStatusTypeID;
                bkgOrders.BOR_ModifiedByID = currentLoggedInUserId;
                bkgOrders.BOR_ModifiedOn = DateTime.Now;
                bkgOrders.BOR_OrderStatusTypeID = NewOrderStatusTypeID;
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            else
                return false;
        }

        #endregion


        #region check services Status 

        Boolean IFingerPrintClientRepository.HideReschedule(AppointmentSlotContract scheduleAppointmentContract)
        {
            // CABS Service Type
            int cabsTypeStatusId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;
            // Appointment Status Complete
            int CABSCompleteAppStatus = base.LocationDBContext.lkpAppointmentStatus.Where(x => x.AS_Code == "AAAI").FirstOrDefault().AS_ID;

            ApplicantAppointmentDetail applicantAppointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == scheduleAppointmentContract.OrderId
                                                                        && x.AAD_OrganizationUserID == scheduleAppointmentContract.ApplicantOrgUserId
                                                                        && !x.AAD_IsDeleted
                                                                        && x.AAD_IsActive)
                                                                        .FirstOrDefault();

            if (applicantAppointmentDetail != null)
            {
                List<ApplicantAppointmentDetailsExt> applicantAppointmentDetailsExt = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == applicantAppointmentDetail.AAD_ID
                                                                           && !x.AADE_IsDeleted && x.AADE_IsActive
                                                                           ).ToList();

                FingerPrintApplicantAppointment fingerPrintApplicantAppointment = base.LocationDBContext.FingerPrintApplicantAppointments.Where(x => x.FPAA_OrderID == scheduleAppointmentContract.OrderId
                                                                             && x.FPAA_OrganizationUserID == scheduleAppointmentContract.ApplicantOrgUserId && !x.FPAA_IsDeleted).FirstOrDefault();

                string fingerPrintApplicantAppointmentStatus = base.LocationDBContext.lkpAppointmentStatus.Where(x => x.AS_ID == fingerPrintApplicantAppointment.FPAA_Status).FirstOrDefault().AS_Code;

                if (applicantAppointmentDetailsExt.Where(x => x.AADE_ServiceTypeID == cabsTypeStatusId).Count() > 0 && fingerPrintApplicantAppointment.FPAA_Status != CABSCompleteAppStatus)
                {
                    if (fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.ACTIVE.GetStringValue())
                    {
                        BkgOrder bkgOrder = _dbContext.BkgOrders.Where(x => x.BOR_MasterOrderID == scheduleAppointmentContract.OrderId
                                                                                     && !x.BOR_IsDeleted).FirstOrDefault();

                        string CABSServiceStatus = _dbContext.lkpOrderStatusTypes.Where(x => x.OrderStatusTypeID == bkgOrder.BOR_OrderStatusTypeID).FirstOrDefault().Code;

                        if (CABSServiceStatus == OrderStatusType.INPROGRESS.GetStringValue())
                        {
                            return true;
                        }

                        else
                        { return false; }
                    }

                    if (fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.REVOKED.GetStringValue()
                    || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.MISSED.GetStringValue()
                    || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.REVOKED_AND_NOTIFIED.GetStringValue()
                    || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.MISSED_AND_NOTIFIED.GetStringValue()
                   || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.REJECTED_BY_ABI.GetStringValue()
                   || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.REJECTED_BY_CBI.GetStringValue()
                   || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.MANUALLY_REJECTED_ORDER.GetStringValue()
                   || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.REJECTED_BY_FBI.GetStringValue()
                   )
                    {
                        return false;
                    }

                    if (fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.PENDING_ABI_REVIEW.GetStringValue()
                       || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.SUBMITTED_TO_CBI.GetStringValue()
                       || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.TECHNICAL_REVIEW.GetStringValue()
                       || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.FINGERPRINT_FILE_ERROR.GetStringValue()
                       || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.COMPLETED.GetStringValue()
                       || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.FINGERPRINTS_COMPLETED_SUCCESSFULLY.GetStringValue()
                       || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.CONTACT_AGENCY_EMPLOYER.GetStringValue()
                       )
                    {
                        return true;
                    }
                }
                if (applicantAppointmentDetailsExt.Where(x => x.AADE_ServiceTypeID != cabsTypeStatusId).Count() > 0 && fingerPrintApplicantAppointment.FPAA_Status == CABSCompleteAppStatus
                    || applicantAppointmentDetailsExt.Where(x => x.AADE_ServiceTypeID != cabsTypeStatusId).Count() > 0)
                {
                    applicantAppointmentDetailsExt = applicantAppointmentDetailsExt.Where(ext => ext.AADE_ServiceTypeID != cabsTypeStatusId).ToList();
                    var addServiceStatusId = applicantAppointmentDetailsExt.FirstOrDefault().AADE_ServiceStatus;
                    lkpCABSServiceStatu lkpCABSServiceStatu = _dbContext.lkpCABSServiceStatus.Where(x => x.CSS_ID == addServiceStatusId).FirstOrDefault();
                    if (lkpCABSServiceStatu.CSS_Code == CABSServiceStatus.NEW.GetStringValue()
                        || lkpCABSServiceStatu.CSS_Code == CABSServiceStatus.REJECTSERVICE.GetStringValue())
                    {
                        return false;
                    }
                    else
                    { return true; }
                }
            }
            return true;
        }
        int? IFingerPrintClientRepository.GetRevertToMoneyDetails(List<int> PaymentDetailsIds)

        {
            if (PaymentDetailsIds != null)
            {
                int invoiceFPypeId = _dbContext.lkpInvoiceTypes.Where(x => x.LIT_InvoiceType == "AAAA" && !x.LIT_IsDeleted).Select(x => x.LIT_ID).FirstOrDefault();
                foreach (int OPD_ID in PaymentDetailsIds)
                {
                    OrderPaymentInvoiceMapping OrderPaymentInvoiceMappingsDetails = _dbContext.OrderPaymentInvoiceMappings.Where(y => y.OPIM_OrderPaymentDetailsId == OPD_ID).FirstOrDefault();
                    if (!OrderPaymentInvoiceMappingsDetails.IsNullOrEmpty() && !OrderPaymentInvoiceMappingsDetails.OPIM_OrderPaymentInvoiceId.IsNullOrEmpty())
                    {
                        int OrderPaymentInvoiceItems = _dbContext.OrderPaymentInvoiceItems.Where(y => y.OPII_OrderPaymentInvoiceId == OrderPaymentInvoiceMappingsDetails.OPIM_OrderPaymentInvoiceId && y.OPII_lkpInvoiceTypeId == invoiceFPypeId && y.OPII_IsDeleted == false).Count();
                        if (OrderPaymentInvoiceItems > 0)
                        {
                            return OPD_ID;
                        }
                    }
                }
            }
            return null;
        }

        Boolean IFingerPrintClientRepository.HideCancel(Int32 OrderId)
        {
            // CABS Service Type
            int cabsTypeStatusId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;
            ApplicantAppointmentDetail applicantAppointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == OrderId
                                                                        && !x.AAD_IsDeleted
                                                                        && x.AAD_IsActive)
                                                                        .FirstOrDefault();

            if (applicantAppointmentDetail != null)
            {
                List<ApplicantAppointmentDetailsExt> applicantAppointmentDetailsExt = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == applicantAppointmentDetail.AAD_ID
                                                                           && !x.AADE_IsDeleted && x.AADE_IsActive
                                                                           && x.AADE_ServiceTypeID != cabsTypeStatusId
                                                                           ).ToList();

                if (applicantAppointmentDetailsExt.Count() > 0)
                {
                    var addServiceStatusId = applicantAppointmentDetailsExt.FirstOrDefault().AADE_ServiceStatus;
                    lkpCABSServiceStatu lkpCABSServiceStatu = _dbContext.lkpCABSServiceStatus.Where(x => x.CSS_ID == addServiceStatusId).FirstOrDefault();
                    if (lkpCABSServiceStatu.CSS_Code != CABSServiceStatus.NEW.GetStringValue())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        Boolean IFingerPrintClientRepository.HideRescheduleForAdmin(int OrderId)
        {
            // CABS Service Type
            int cabsTypeStatusId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;
            // Appointment Status Complete
            int CABSCompleteAppStatus = base.LocationDBContext.lkpAppointmentStatus.Where(x => x.AS_Code == "AAAI").FirstOrDefault().AS_ID;

            ApplicantAppointmentDetail applicantAppointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == OrderId
                                                                        && !x.AAD_IsDeleted
                                                                        && x.AAD_IsActive)
                                                                        .FirstOrDefault();

            if (applicantAppointmentDetail != null)
            {
                List<ApplicantAppointmentDetailsExt> applicantAppointmentDetailsExt = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == applicantAppointmentDetail.AAD_ID
                                                                           && !x.AADE_IsDeleted && x.AADE_IsActive
                                                                           ).ToList();

                FingerPrintApplicantAppointment fingerPrintApplicantAppointment = base.LocationDBContext.FingerPrintApplicantAppointments.Where(x => x.FPAA_OrderID == OrderId
                                                                              && !x.FPAA_IsDeleted).FirstOrDefault();

                string fingerPrintApplicantAppointmentStatus = base.LocationDBContext.lkpAppointmentStatus.Where(x => x.AS_ID == fingerPrintApplicantAppointment.FPAA_Status).FirstOrDefault().AS_Code;

                if (applicantAppointmentDetailsExt.Where(x => x.AADE_ServiceTypeID == cabsTypeStatusId).Count() > 0 && fingerPrintApplicantAppointment.FPAA_Status != CABSCompleteAppStatus)
                {
                    if (fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.ACTIVE.GetStringValue())
                    {
                        BkgOrder bkgOrder = _dbContext.BkgOrders.Where(x => x.BOR_MasterOrderID == OrderId
                                                                                     && !x.BOR_IsDeleted).FirstOrDefault();

                        string CABSServiceStatus = _dbContext.lkpOrderStatusTypes.Where(x => x.OrderStatusTypeID == bkgOrder.BOR_OrderStatusTypeID).FirstOrDefault().Code;

                        if (CABSServiceStatus == OrderStatusType.INPROGRESS.GetStringValue())
                        {
                            return true;
                        }

                        else
                        { return false; }
                    }

                    if (fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.REVOKED.GetStringValue()
                     || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.MISSED.GetStringValue()
                     || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.REVOKED_AND_NOTIFIED.GetStringValue()
                     || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.MISSED_AND_NOTIFIED.GetStringValue()
                    || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.REJECTED_BY_ABI.GetStringValue()
                    || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.REJECTED_BY_CBI.GetStringValue()
                    || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.MANUALLY_REJECTED_ORDER.GetStringValue()
                    || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.REJECTED_BY_FBI.GetStringValue()
                    )
                    {
                        return false;
                    }

                    if (fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.PENDING_ABI_REVIEW.GetStringValue()
                       || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.SUBMITTED_TO_CBI.GetStringValue()
                       || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.TECHNICAL_REVIEW.GetStringValue()
                       || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.FINGERPRINT_FILE_ERROR.GetStringValue()
                       || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.COMPLETED.GetStringValue()
                       || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.FINGERPRINTS_COMPLETED_SUCCESSFULLY.GetStringValue()
                       || fingerPrintApplicantAppointmentStatus == FingerPrintAppointmentStatus.CONTACT_AGENCY_EMPLOYER.GetStringValue()
                       )
                    {
                        return true;
                    }
                }
                if (applicantAppointmentDetailsExt.Where(x => x.AADE_ServiceTypeID != cabsTypeStatusId).Count() > 0 && fingerPrintApplicantAppointment.FPAA_Status == CABSCompleteAppStatus
                    || applicantAppointmentDetailsExt.Where(x => x.AADE_ServiceTypeID != cabsTypeStatusId).Count() > 0)
                {
                    applicantAppointmentDetailsExt = applicantAppointmentDetailsExt.Where(ext => ext.AADE_ServiceTypeID != cabsTypeStatusId).ToList();
                    var addServiceStatusId = applicantAppointmentDetailsExt.FirstOrDefault().AADE_ServiceStatus;
                    lkpCABSServiceStatu lkpCABSServiceStatu = _dbContext.lkpCABSServiceStatus.Where(x => x.CSS_ID == addServiceStatusId).FirstOrDefault();
                    if (lkpCABSServiceStatu.CSS_Code == CABSServiceStatus.NEW.GetStringValue()
                        || lkpCABSServiceStatu.CSS_Code == CABSServiceStatus.REJECTSERVICE.GetStringValue())
                    {
                        return false;
                    }
                    else
                    { return true; }
                }
            }
            return true;
        }


        Boolean IFingerPrintClientRepository.HideABIReviewForFulfilment(int OrderId)
        {
            int cabsTypeStatusId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;
            ApplicantAppointmentDetail applicantAppointmentDetails = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == OrderId
                                                                        && !x.AAD_IsDeleted
                                                                        && x.AAD_IsActive)
                                                                        .FirstOrDefault();
            if (applicantAppointmentDetails != null)
            {
                List<ApplicantAppointmentDetailsExt> cabsServicedetailsexts = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == applicantAppointmentDetails.AAD_ID
                                                                   && !x.AADE_IsDeleted && x.AADE_IsActive
                                                                   ).ToList();
                if (cabsServicedetailsexts.Where(x => x.AADE_ServiceTypeID == cabsTypeStatusId).Count() > 0)
                {
                    return false;
                }
                if (cabsServicedetailsexts.Where(x => x.AADE_ServiceTypeID != cabsTypeStatusId).Count() > 0)
                {
                    return true;
                }

            }
            return false;
        }



        #endregion

        #region CBI Billing Status
        List<CBIBillingStatusContract> IFingerPrintClientRepository.GetCBIBillingStatus(CustomPagingArgsContract customPagingArgsContract, CBIBillingStatusContract billingStatusContract)
        {
            List<CBIBillingStatusContract> lstCBIBillingStatusContract = new List<CBIBillingStatusContract>();

            String orderBy = "CBIUniqueID";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@CBIUniqueId",billingStatusContract.CBIUniqueID),
                            new SqlParameter("@AccountName",billingStatusContract.AccountName),
                            new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                            new SqlParameter("@PageSize", customPagingArgsContract.PageSize)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.GetCBIBillingStatusData", sqlParameterCollection))
                {

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            CBIBillingStatusContract CBIBillingStatusContract = new CBIBillingStatusContract();
                            CBIBillingStatusContract.CBIUniqueID = dr["CBIUniqueID"].ToString();
                            CBIBillingStatusContract.BillingCode = dr["CBS_BillingCode"].ToString();
                            CBIBillingStatusContract.IsEnabled = dr["CBS_IsEnabled"] == DBNull.Value ? false : Convert.ToBoolean(dr["CBS_IsEnabled"]);
                            CBIBillingStatusContract.AccountAddress = dr["AcctAdr"].ToString();
                            CBIBillingStatusContract.AccountCity = dr["AcctCty"].ToString();
                            CBIBillingStatusContract.AccountName = dr["AcctNam"].ToString();
                            CBIBillingStatusContract.AccountState = dr["ACCTSTA"].ToString();
                            CBIBillingStatusContract.AccountZIP = dr["AcctZip"].ToString();
                            CBIBillingStatusContract.TotalCount = Convert.ToInt32(dr["TotalCount"]);
                            CBIBillingStatusContract.Amount = dr["cbs_amount"] == DBNull.Value ? AppConsts.NONE : Convert.ToDecimal(dr["cbs_amount"]);
                            lstCBIBillingStatusContract.Add(CBIBillingStatusContract);
                        }
                    }
                }
            }

            return lstCBIBillingStatusContract;
        }


        Boolean IFingerPrintClientRepository.SaveCBIBillingStatus(CBIBillingStatusContract billingStatusContract, Int32 currentLoggedInUserId)
        {
            try
            {
                CBIBillingStatu OBjcBIBillingStatu = _dbContext.CBIBillingStatus.Where(cond => cond.CBS_CBIUniqueId == billingStatusContract.CBIUniqueID && cond.CBS_IsDeleted == false).FirstOrDefault();
                if (OBjcBIBillingStatu.IsNullOrEmpty())
                {
                    CBIBillingStatu CBIbillingStatus = new CBIBillingStatu();
                    CBIbillingStatus.CBS_BillingCode = billingStatusContract.BillingCode;
                    CBIbillingStatus.CBS_CBIUniqueId = billingStatusContract.CBIUniqueID;
                    CBIbillingStatus.CBS_IsEnabled = billingStatusContract.IsEnabled;
                    CBIbillingStatus.CBS_CreatedById = currentLoggedInUserId;
                    CBIbillingStatus.CBS_CreatedDate = DateTime.Now;
                    //UAT-3850
                    CBIbillingStatus.CBS_Amount = billingStatusContract.Amount;
                    _dbContext.CBIBillingStatus.AddObject(CBIbillingStatus);
                }
                else
                {
                    OBjcBIBillingStatu.CBS_IsEnabled = billingStatusContract.IsEnabled;
                    OBjcBIBillingStatu.CBS_ModifiedBy = currentLoggedInUserId;
                    OBjcBIBillingStatu.CBS_BillingCode = billingStatusContract.BillingCode;
                    OBjcBIBillingStatu.CBS_ModifiedDate = DateTime.Now;
                    //UAT-3850
                    OBjcBIBillingStatu.CBS_Amount = billingStatusContract.Amount;
                }
                if (_dbContext.SaveChanges() > AppConsts.NONE)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        #endregion

        Boolean IFingerPrintClientRepository.SaveTenantLocationMapping(Int32 selectedLocationID, Int32 tenantId, Int32 selectedDpmId, Int32 currentLoggedInUserId)
        {
            if (selectedLocationID > AppConsts.NONE)
            {
                //location hierarchy Mapping in Tenant database
                InstHierarchyLocationMapping locationMapping = new InstHierarchyLocationMapping();
                locationMapping.IHLM_DeptProgramMappingID = selectedDpmId;
                locationMapping.IHLM_LocationID = selectedLocationID;
                locationMapping.IHLM_IsDeleted = false;
                locationMapping.IHLM_CreatedBy = currentLoggedInUserId;
                locationMapping.IHLM_CreatedOn = DateTime.Now;
                _dbContext.InstHierarchyLocationMappings.AddObject(locationMapping);

                //location Tenant Mapping in location database
                LocationTenantMapping locationTenantMapping = new LocationTenantMapping();
                locationTenantMapping.LTM_LocationID = selectedLocationID;
                locationTenantMapping.LTM_TenantID = tenantId;
                locationTenantMapping.LTM_CreatedBy = currentLoggedInUserId;
                locationTenantMapping.LTM_CreatedOn = DateTime.Now;
                base.LocationDBContext.LocationTenantMappings.AddObject(locationTenantMapping);
            }

            if (_dbContext.SaveChanges() > AppConsts.NONE && base.LocationDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;

        }

        List<Int32> IFingerPrintClientRepository.GetDPMLocationIDs(Int32 tenantId, Int32 selectedDpmId)
        {
            return _dbContext.InstHierarchyLocationMappings.Where(a => a.IHLM_DeptProgramMappingID == selectedDpmId && !a.IHLM_IsDeleted).Select(a => a.IHLM_LocationID).ToList();

        }

        Boolean IFingerPrintClientRepository.DeleteTenantLocationMapping(Int32 tenantId, Int32 selectedDPMId, Int32 selectedLocationId, Int32 currentLoggedInUserId)
        {
            InstHierarchyLocationMapping instHierarchyLocMapping = _dbContext.InstHierarchyLocationMappings.Where(cond => !cond.IHLM_IsDeleted && cond.IHLM_DeptProgramMappingID == selectedDPMId && cond.IHLM_LocationID == selectedLocationId).FirstOrDefault();
            LocationTenantMapping locTenantMapping = base.LocationDBContext.LocationTenantMappings.Where(cond => !cond.LTM_IsDeleted && cond.LTM_LocationID == selectedLocationId && cond.LTM_TenantID == tenantId).FirstOrDefault();

            if (!instHierarchyLocMapping.IsNullOrEmpty())
            {
                instHierarchyLocMapping.IHLM_IsDeleted = true;
                instHierarchyLocMapping.IHLM_ModifiedBy = currentLoggedInUserId;
                instHierarchyLocMapping.IHLM_ModifiedOn = DateTime.Now;
            }
            if (!locTenantMapping.IsNullOrEmpty())
            {
                locTenantMapping.LTM_IsDeleted = true;
                locTenantMapping.LTM_ModifiedBy = currentLoggedInUserId;
                locTenantMapping.LTM_ModifiedOn = DateTime.Now;
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE && base.LocationDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        Boolean IFingerPrintClientRepository.DeleteTenantNodeLocationMapping(Int32 tenantId, Int32 selectedDPMId, Int32 currentLoggedInUserId)
        {
            List<InstHierarchyLocationMapping> lstInstHierarchyLocMapping = _dbContext.InstHierarchyLocationMappings.Where(cond => !cond.IHLM_IsDeleted && cond.IHLM_DeptProgramMappingID == selectedDPMId).ToList();
            List<Int32> lstLocationId = lstInstHierarchyLocMapping.Select(sel => sel.IHLM_LocationID).ToList();
            List<LocationTenantMapping> lstlocTenantMapping = base.LocationDBContext.LocationTenantMappings.Where(cond => !cond.LTM_IsDeleted && lstLocationId.Contains(cond.LTM_LocationID) && cond.LTM_TenantID == tenantId).ToList();
            foreach (InstHierarchyLocationMapping instHierarchyLocMapping in lstInstHierarchyLocMapping)
            {

                instHierarchyLocMapping.IHLM_IsDeleted = true;
                instHierarchyLocMapping.IHLM_ModifiedBy = currentLoggedInUserId;
                instHierarchyLocMapping.IHLM_ModifiedOn = DateTime.Now;
            }

            foreach (LocationTenantMapping locTenantMapping in lstlocTenantMapping)
            {

                locTenantMapping.LTM_IsDeleted = true;
                locTenantMapping.LTM_ModifiedBy = currentLoggedInUserId;
                locTenantMapping.LTM_ModifiedOn = DateTime.Now;
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE && base.LocationDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }

        //Boolean IFingerPrintClientRepository.SaveRescheduledAppointment(Int32 tenantId, Int32 currentLoggedInUserId, AppointmentSlotContract appointSlotContract)
        //{
        //if (!appointSlotContract.IsNullOrEmpty())
        //{
        //    FingerPrintApplicantAppointment fingerPrintApplicantAppointment = base.LocationDBContext.FingerPrintApplicantAppointments.Where(cond => !cond.FPAA_IsDeleted && cond.FPAA_ID == appointSlotContract.ApplicantAppointmentId && cond.FPAA_TenantID == tenantId && cond.FPAA_OrderID == appointSlotContract.OrderId).FirstOrDefault();
        //    ApplicantAppointmentDetail applicantAppointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(cond => !cond.AAD_IsDeleted && cond.AAD_IsActive && cond.AAD_OrderID == appointSlotContract.OrderId && cond.AAD_OrganizationUserID == appointSlotContract.ApplicantOrgUserId).FirstOrDefault();
        //    ApplicantAppointmentDetail applicantAppointDetail = new ApplicantAppointmentDetail();

        //    fingerPrintApplicantAppointment.FPAA_ScheduleSlotID = appointSlotContract.SlotID;
        //    fingerPrintApplicantAppointment.FPAA_ModifiedBy = currentLoggedInUserId;
        //    fingerPrintApplicantAppointment.FPAA_ModifiedOn = DateTime.Now;

        //    applicantAppointmentDetail.AAD_IsActive = false;
        //    applicantAppointmentDetail.AAD_ModifiedBy = currentLoggedInUserId;
        //    applicantAppointmentDetail.AAD_ModifiedOn = DateTime.Now;

        //    TimeSpan startTime = new TimeSpan();
        //    TimeSpan endTime = new TimeSpan();
        //    TimeSpan.TryParse(appointSlotContract.SlotStartTime, out startTime);
        //    TimeSpan.TryParse(appointSlotContract.SlotEndTime, out endTime);
        //    applicantAppointDetail.AAD_OrganizationUserID = appointSlotContract.ApplicantOrgUserId;
        //    applicantAppointDetail.AAD_OrderID = appointSlotContract.OrderId;
        //    applicantAppointDetail.AAD_IsActive = true;
        //    applicantAppointDetail.AAD_AppointmentStartTime = appointSlotContract.SlotDate.Value.Add(startTime);
        //    applicantAppointDetail.AAD_AppointmentEndTime = appointSlotContract.SlotDate.Value.Add(endTime);
        //    applicantAppointDetail.AAD_CreatedBy = currentLoggedInUserId;
        //    applicantAppointDetail.AAD_CreatedOn = DateTime.Now;
        //    _dbContext.ApplicantAppointmentDetails.AddObject(applicantAppointDetail);
        //}

        //if (_dbContext.SaveChanges() > AppConsts.NONE && base.LocationDBContext.SaveChanges() > AppConsts.NONE)
        //    return true;
        //return false;
        //}

        Boolean IFingerPrintClientRepository.DeleteApplicantAppointment(Int32 tenantId, Int32 orderId, Int32 currentLoggedInUserId, Int32 applicantAppointmentId, Decimal? RefundAmount)
        {

            Int32 ApplicantAppointmentId = 0;
            String OldOrderStatusName = String.Empty;
            String oldOrderStatusId = String.Empty;
            Int32 orderStatusId = 0;
            var applicantLocationId = 0;
            bool isOnsiteAppointment = false;
            bool isOutOfStateAppointment = false;

            if (tenantId > AppConsts.NONE && orderId > AppConsts.NONE && applicantAppointmentId > AppConsts.NONE)
            {
                FingerPrintApplicantAppointment fingerPrintApplAppointment = base.LocationDBContext.FingerPrintApplicantAppointments.Where(cond => !cond.FPAA_IsDeleted && cond.FPAA_ID == applicantAppointmentId && cond.FPAA_TenantID == tenantId && cond.FPAA_OrderID == orderId).FirstOrDefault();
                ApplicantAppointmentDetail applicantAppointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(cond => !cond.AAD_IsDeleted && cond.AAD_IsActive && cond.AAD_OrderID == orderId).FirstOrDefault();
                ApplicantAppointmentId = applicantAppointmentDetail.AAD_ID;
                var code = FingerPrintAppointmentStatus.CANCELLED.GetStringValue();
                orderStatusId = base.LocationDBContext.lkpAppointmentStatus.FirstOrDefault(x => x.AS_Code == code && x.AS_IsDeleted == false).AS_ID;
                OldOrderStatusName = base.LocationDBContext.lkpAppointmentStatus.FirstOrDefault(x => x.AS_ID == fingerPrintApplAppointment.FPAA_Status && x.AS_IsDeleted == false).AS_Name;
                oldOrderStatusId = fingerPrintApplAppointment.FPAA_Status.ToString();
                applicantLocationId = fingerPrintApplAppointment.FPAA_LocationID;
                isOnsiteAppointment = fingerPrintApplAppointment.FPAA_IsOnsiteAppointment == null ? false : Convert.ToBoolean(fingerPrintApplAppointment.FPAA_IsOnsiteAppointment);
                isOutOfStateAppointment = fingerPrintApplAppointment.FPAA_IsOutOfState;


                if (RefundAmount.IsNotNull())
                {
                    fingerPrintApplAppointment.FPAA_RefundAmount = Convert.ToDouble(RefundAmount);
                    fingerPrintApplAppointment.FPAA_RefundDate = DateTime.Now;
                }

                //var refundamount = _dbContext.OrderPaymentDetails.FirstOrDefault(cnd => cnd.OPD_OrderID == orderId && !cnd.OPD_IsDeleted).OPD_Amount;
                //var paymentMode = _dbContext.OrderPaymentDetails.FirstOrDefault(cnd => cnd.OPD_OrderID == orderId && !cnd.OPD_IsDeleted).lkpPaymentOption.Code;


                //fingerPrintApplAppointment.FPAA_IsDeleted = true;
                fingerPrintApplAppointment.FPAA_ModifiedBy = currentLoggedInUserId;
                fingerPrintApplAppointment.FPAA_ModifiedOn = DateTime.Now;
                fingerPrintApplAppointment.FPAA_Status = orderStatusId;




                applicantAppointmentDetail.AAD_IsDeleted = true;
                applicantAppointmentDetail.AAD_ModifiedBy = currentLoggedInUserId;
                applicantAppointmentDetail.AAD_ModifiedOn = DateTime.Now;

            }
            if (_dbContext.SaveChanges() > AppConsts.NONE && base.LocationDBContext.SaveChanges() > AppConsts.NONE)
            {
                LocationServiceAppointmentAudit AppointmentAuditData = new LocationServiceAppointmentAudit();
                AppointmentAuditData.LSAA_AppointmentID = ApplicantAppointmentId;
                var statusCode = LocationAppointmentAuditChangeType.STATUS_CHANGE.GetStringValue();
                AppointmentAuditData.LSAA_ChangeTypeID = _dbContext.lkpAppointmentChangeTypes.Where(x => x.ACT_Code == statusCode).FirstOrDefault().ACT_ID;
                AppointmentAuditData.LSAA_CreatedBy = currentLoggedInUserId;
                AppointmentAuditData.LSAA_CreatedOn = DateTime.Now;
                AppointmentAuditData.LSAA_DateUpdated = DateTime.Now;
                AppointmentAuditData.LSAA_Description = "Status Changed > " + OldOrderStatusName + " --> Cancelled";
                AppointmentAuditData.LSAA_NewValue = orderStatusId.ToString();
                AppointmentAuditData.LSAA_OldValue = oldOrderStatusId;
                AppointmentAuditData.LSAA_LocationId = applicantLocationId;
                AppointmentAuditData.LSAA_IsOnsite = isOnsiteAppointment;
                AppointmentAuditData.LSAA_IsOutOfState = isOutOfStateAppointment;
                _dbContext.LocationServiceAppointmentAudits.AddObject(AppointmentAuditData);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }


        Boolean IFingerPrintClientRepository.UpdateAppointmentHistory(Int32 orderId, string orderStatuscode, Int32 paymentModeId, Int32 currentLoggedInUserId, Int32 tenantId, Int32 applicantAppointmentId, decimal refundAmount)
        {
            if (tenantId > AppConsts.NONE && orderId > AppConsts.NONE && applicantAppointmentId > AppConsts.NONE)
            {
                FingerPrintApplicantAppointment fingerPrintApplAppointment = base.LocationDBContext.FingerPrintApplicantAppointments.Where(cond => !cond.FPAA_IsDeleted && cond.FPAA_ID == applicantAppointmentId && cond.FPAA_TenantID == tenantId && cond.FPAA_OrderID == orderId).FirstOrDefault();

                fingerPrintApplAppointment.FPAA_RefundAmount = (fingerPrintApplAppointment.FPAA_RefundAmount ?? 0) + (double)refundAmount;
                fingerPrintApplAppointment.FPAA_RefundDate = DateTime.Now;
                fingerPrintApplAppointment.FPAA_ModifiedBy = currentLoggedInUserId;
                fingerPrintApplAppointment.FPAA_ModifiedOn = DateTime.Now;

                if (LocationDBContext.SaveChanges() > AppConsts.NONE)
                    return true;
            }
            return false;
        }





        Boolean IFingerPrintClientRepository.IsOrderPaymentStatusPending(Int32 tenantId, Int32 orderId, Int32 currentLoggedInUserId)
        {
            string paymentStatus = OrderStatusType.PAYMENTPENDING.GetStringValue();
            lkpOrderStatusType lkpOrderStatusType = _dbContext.lkpOrderStatusTypes.Where(y => y.Code == paymentStatus).FirstOrDefault();
            BkgOrder bkgOrder = _dbContext.BkgOrders.Where(cond => !cond.BOR_IsDeleted && cond.BOR_MasterOrderID == orderId && cond.BOR_OrderStatusTypeID == lkpOrderStatusType.OrderStatusTypeID).FirstOrDefault();
            if (bkgOrder != null)
            {
                return true;
            }
            else
                return false;
        }


        Boolean IFingerPrintClientRepository.ApprovePayment(Int32 tenantId, Int32 orderId, Int32 orderPaymentDetailId, Int32 orderStatusPaidId, Int32 currentLoggedInUserId, Int32 newOrderStatusTypeID)
        {
            var paymentOptions = _dbContext.lkpOrderStatus.Where(x => !x.IsDeleted).ToList();
            int paidOption = paymentOptions.Where(x => x.Code == ApplicantOrderStatus.Paid.GetStringValue()).FirstOrDefault().OrderStatusID;
            int cancelledOption = paymentOptions.Where(x => x.Code == ApplicantOrderStatus.Cancelled.GetStringValue()).FirstOrDefault().OrderStatusID;
            int modifyShpngSntForOnlnePymntOption = paymentOptions.Where(x => x.Code == ApplicantOrderStatus.Modify_Shipping_Send_For_Online_Payment.GetStringValue()).FirstOrDefault().OrderStatusID;
            orderPaymentDetailId = _dbContext.OrderPaymentDetails.Where(cond => !cond.OPD_IsDeleted && cond.OPD_OrderID == orderId && cond.OPD_OrderStatusID != paidOption && cond.OPD_OrderStatusID != cancelledOption && cond.OPD_OrderStatusID != modifyShpngSntForOnlnePymntOption).Select(x => x.OPD_ID).FirstOrDefault();
            OrderPaymentDetail orderPaymentDetail = _dbContext.OrderPaymentDetails.Where(cond => !cond.OPD_IsDeleted && cond.OPD_OrderID == orderId && cond.OPD_ID == orderPaymentDetailId).FirstOrDefault();
            String oldOrderStatusName = _dbContext.lkpOrderStatus.Where(x => !x.IsDeleted && x.OrderStatusID == orderPaymentDetail.OPD_OrderStatusID).FirstOrDefault().Name;
            String oldOrderStatusId = orderPaymentDetail.OPD_OrderStatusID.ToString();
            orderPaymentDetail.OPD_OrderStatusID = orderStatusPaidId;
            orderPaymentDetail.OPD_ModifiedByID = currentLoggedInUserId;
            orderPaymentDetail.OPD_ModifiedOn = DateTime.Now;
            orderPaymentDetail.OPD_ApprovalDate = DateTime.Now;
            orderPaymentDetail.OPD_ApprovedBy = currentLoggedInUserId;

            BkgOrder bkgOrder = _dbContext.BkgOrders.Where(cond => !cond.BOR_IsDeleted && cond.BOR_MasterOrderID == orderId).FirstOrDefault();

            bkgOrder.BOR_OrderStatusTypeID = newOrderStatusTypeID;
            bkgOrder.BOR_ModifiedByID = currentLoggedInUserId;
            bkgOrder.BOR_ModifiedOn = DateTime.Now;
            Int32 AppointmentId = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == bkgOrder.BOR_MasterOrderID && !x.AAD_IsDeleted).FirstOrDefault().AAD_ID;
            FingerPrintApplicantAppointment fingerPrintApplicantAppointment = base.LocationDBContext.FingerPrintApplicantAppointments.Where(cond => cond.FPAA_OrderID == orderId
                                                                              && !cond.FPAA_IsDeleted).FirstOrDefault();
            if (_dbContext.SaveChanges() > AppConsts.NONE)
            {
                LocationServiceAppointmentAudit locationServiceAuditData = new LocationServiceAppointmentAudit();
                locationServiceAuditData.LSAA_AppointmentID = AppointmentId;
                var statusCode = LocationAppointmentAuditChangeType.STATUS_CHANGE.GetStringValue();
                var appointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == orderId && !x.AAD_IsDeleted && x.AAD_IsActive).FirstOrDefault();
                int cabsPrice = 0;
                var CabsService = appointmentDetail.ApplicantAppointmentDetailsExts.Where(x => x.lkpBkgSvcType.BST_Code == "AAAA").ToList();
                if (CabsService.Count != 0)
                {
                    cabsPrice = Convert.ToInt32(CabsService.FirstOrDefault().AADE_TotalPrice);
                }
                Decimal AmountPaidByInstitue = 0;
                var OPDH_Institue = appointmentDetail.Order.OrderPaymentDetailHistories.Where(x => x.OPDH_PaymentOptionID == 5).ToList();
                if (OPDH_Institue.Count != 0)
                {
                    AmountPaidByInstitue = Convert.ToInt32(OPDH_Institue.FirstOrDefault().OPDH_Amount);
                }
                if (cabsPrice != 0 && AmountPaidByInstitue != 0 && cabsPrice <= AmountPaidByInstitue)
                {
                    statusCode = LocationAppointmentAuditChangeType.ADDITIONAL_SERVICES_STATUS_CHANGE.GetStringValue();
                }
                locationServiceAuditData.LSAA_ChangeTypeID = _dbContext.lkpAppointmentChangeTypes.Where(x => x.ACT_Code == statusCode).FirstOrDefault().ACT_ID;
                locationServiceAuditData.LSAA_CreatedBy = currentLoggedInUserId;
                locationServiceAuditData.LSAA_CreatedOn = DateTime.Now;
                locationServiceAuditData.LSAA_DateUpdated = DateTime.Now;
                locationServiceAuditData.LSAA_Description = "Status Changed > " + oldOrderStatusName + " --> Paid";
                locationServiceAuditData.LSAA_OldValue = oldOrderStatusId;
                locationServiceAuditData.LSAA_NewValue = orderStatusPaidId.ToString();
                locationServiceAuditData.LSAA_LocationId = fingerPrintApplicantAppointment.FPAA_LocationID;
                locationServiceAuditData.LSAA_IsOnsite = fingerPrintApplicantAppointment.FPAA_IsOnsiteAppointment.IsNullOrEmpty() ? false : fingerPrintApplicantAppointment.FPAA_IsOnsiteAppointment;
                locationServiceAuditData.LSAA_IsOutOfState = fingerPrintApplicantAppointment.FPAA_IsOutOfState;
                _dbContext.LocationServiceAppointmentAudits.AddObject(locationServiceAuditData);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool GetIfApprovePaymentReqd(Int32 tenantId, Int32 orderId, Int32 currentLoggedInUserId)
        {
            List<OrderPaymentDetail> orderPaymentDetail = _dbContext.OrderPaymentDetails.Where(cond => !cond.OPD_IsDeleted && cond.OPD_OrderID == orderId).ToList();
            var paymentOptions = _dbContext.lkpOrderStatus.Where(x => !x.IsDeleted).ToList();
            int paidOption = paymentOptions.Where(x => x.Code == ApplicantOrderStatus.Paid.GetStringValue()).FirstOrDefault().OrderStatusID;
            int cancelledOption = paymentOptions.Where(x => x.Code == ApplicantOrderStatus.Cancelled.GetStringValue()).FirstOrDefault().OrderStatusID;
            int modifyShpngSntForOnlnePymntOption = paymentOptions.Where(x => x.Code == ApplicantOrderStatus.Modify_Shipping_Send_For_Online_Payment.GetStringValue()).FirstOrDefault().OrderStatusID;
            bool result = false;
            if (orderPaymentDetail.IsNotNull())
            {
                result = orderPaymentDetail.Any(x => x.OPD_OrderStatusID != paidOption && x.OPD_OrderStatusID != cancelledOption && x.OPD_OrderStatusID != modifyShpngSntForOnlnePymntOption);
            }
            return result;
        }

        public bool GetIsRevertToMoneyOrder(Int32 tenantId, Int32 orderId, Int32 currentLoggedInUserId, int orderPaymentDetailId)
        {
            bool result = false;
            int invoiceShippingTypeId = _dbContext.lkpInvoiceTypes.Where(x => x.LIT_InvoiceType == "AAAC" && !x.LIT_IsDeleted).Select(x => x.LIT_ID).FirstOrDefault();
            int OPIM_OrderPaymentInvoiceId = _dbContext.OrderPaymentInvoiceMappings.Where(x => x.OPIM_OrderPaymentDetailsId == orderPaymentDetailId && !x.OPIM_IsDeleted).Select(x => x.OPIM_OrderPaymentInvoiceId).FirstOrDefault();
            int orderpymntInvoice = _dbContext.OrderPaymentInvoices.Where(x => x.OPI_ID == OPIM_OrderPaymentInvoiceId && !x.OPI_IsDeleted).Select(x => x.OPI_ID).FirstOrDefault();
            result = _dbContext.OrderPaymentInvoiceItems.Any(x => x.OPII_OrderPaymentInvoiceId == orderpymntInvoice && !x.OPII_IsDeleted && x.OPII_lkpInvoiceTypeId != invoiceShippingTypeId);
            return result;
        }


        Boolean IFingerPrintClientRepository.CanRevertToMoneyOrder(Int32 orderId)
        {
            var cancelledStatusName = OrderStatusType.CANCELLED.GetStringValue();
            var paymentOptionNameMO = PaymentOptions.Money_Order.GetStringValue();
            var paymentStatusNamePending = ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue();
            var paymentOptionNameCC = PaymentOptions.Credit_Card.GetStringValue();
            var paymentStatusNameComplete = ApplicantOrderStatus.Paid.GetStringValue();

            var entity = _dbContext.OrderPaymentDetails
            .Where(cond => cond.OPD_OrderID == orderId
            && !cond.Order.IsDeleted
            && cond.Order.lkpOrderStatu.Name != cancelledStatusName)
            .ToList();

            var result = entity != null
            && entity.Any(cond => cond.lkpPaymentOption.Code == paymentOptionNameMO
            && cond.lkpOrderStatu.Code == paymentStatusNamePending
            && cond.OPD_IsDeleted)
            && entity.Any(cond => cond.lkpPaymentOption.Code == paymentOptionNameCC
            && cond.lkpOrderStatu.Code == paymentStatusNameComplete
            && !cond.OPD_IsDeleted);

            return result;
        }

        AppointmentSlotContract IFingerPrintClientRepository.GetBkgOrderWithAppointmentData(Int32 tenantId, Int32 OrderId, Int32 ApplicantOrgUserID)
        {
            AppointmentSlotContract appointmentSlotContract = new AppointmentSlotContract();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@TenantID",tenantId),
                    new SqlParameter("@OrderId",OrderId),
                    new SqlParameter("@ApplicantOrgUserID",ApplicantOrgUserID)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetBkgOrderWithAppointmentData", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            appointmentSlotContract.LocationId = dr["FPL_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FPL_ID"]);
                            appointmentSlotContract.LocationName = dr["FPL_Name"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FPL_Name"]);
                            appointmentSlotContract.LocationAddress = dr["FPL_FullAddress"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FPL_FullAddress"]);
                            appointmentSlotContract.LocDescription = dr["FPL_Description"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FPL_Description"]);
                            //In case of onsite appointment FPL_Name return event Name else Location Name.
                            appointmentSlotContract.EventName = dr["FPL_Name"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FPL_Name"]);
                            appointmentSlotContract.EventDescription = dr["FPL_Description"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FPL_Description"]);
                            appointmentSlotContract.SlotDate = dr["FPSS_SlotDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FPSS_SlotDate"]);
                            appointmentSlotContract.SlotID = dr["FPAA_ScheduleSlotID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["FPAA_ScheduleSlotID"]);
                            appointmentSlotContract.ApplicantAppointmentId = dr["FPAA_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["FPAA_ID"]);
                            appointmentSlotContract.ApplicantOrgUserId = dr["FPAA_OrganizationUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["FPAA_OrganizationUserID"]);
                            appointmentSlotContract.OrderId = dr["FPAA_OrderID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["FPAA_OrderID"]);
                            appointmentSlotContract.SlotStartTimeTimeSpanFormat = dr["FPSS_SlotStartTime"] == DBNull.Value ? TimeSpan.MinValue : (TimeSpan)(dr["FPSS_SlotStartTime"]);
                            appointmentSlotContract.SlotEndTimeTimeSpanFormat = (dr["FPSS_SlotEndTime"]) == DBNull.Value ? TimeSpan.MinValue : (TimeSpan)(dr["FPSS_SlotEndTime"]);
                            appointmentSlotContract.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                            appointmentSlotContract.SlotStartTime = Convert.ToString(appointmentSlotContract.SlotStartTimeTimeSpanFormat);
                            appointmentSlotContract.SlotEndTime = Convert.ToString(appointmentSlotContract.SlotEndTimeTimeSpanFormat);
                            appointmentSlotContract.FingerPrintDocumentId = dr["FingerPrintDocumentId"] != DBNull.Value ? Convert.ToInt32(dr["FingerPrintDocumentId"]) : 0;
                            appointmentSlotContract.OrderStatusCode = dr["OrderStatusCode"] == DBNull.Value ? null : Convert.ToString(dr["OrderStatusCode"]);
                            appointmentSlotContract.AppointmentStatus = dr["FPAA_Status"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FPAA_Status"]);
                            appointmentSlotContract.IsOnsiteAppointment = dr["IsOnsiteAppointment"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsOnsiteAppointment"]);
                            appointmentSlotContract.AppointmentStatusCode = dr["AppointmentStatusCode"].ToString();
                            appointmentSlotContract.IsOutOfStateAppointment = dr["FPAA_IsOutOfState"] == DBNull.Value ? false : Convert.ToBoolean(dr["FPAA_IsOutOfState"]);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return appointmentSlotContract;
        }

        List<ValidateRegexDataContract> IFingerPrintClientRepository.GetPersonalInformationExpressions(Int32 BkgPkgID, String languageCode)
        {
            List<ValidateRegexDataContract> lstValidateRegexDataContract = new List<ValidateRegexDataContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@PackageId",BkgPkgID),
                    new SqlParameter("@LanguageCode",languageCode)
                };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetValidateRegexOfInputs", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ValidateRegexDataContract validateRegexDataContract = new ValidateRegexDataContract();
                            validateRegexDataContract.BSA_ID = dr["BSA_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["BSA_ID"]);
                            validateRegexDataContract.DataTypeID = dr["DataTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DataTypeID"]);
                            validateRegexDataContract.BAGM_Code = dr["BAGM_Code"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BAGM_Code"]).ToUpper();
                            validateRegexDataContract.FieldName = dr["FieldName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FieldName"]);
                            validateRegexDataContract.ValidateExpression = dr["ValidateExpression"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ValidateExpression"]);
                            validateRegexDataContract.ValidationMessage = dr["ValidationMessage"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ValidationMessage"]);

                            lstValidateRegexDataContract.Add(validateRegexDataContract);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstValidateRegexDataContract;
        }

        Boolean IFingerPrintClientRepository.SaveFingerPrintDocumentsStaging(List<String> lstFileName, Int32 currentLoggedInUserId)
        {
            if (!lstFileName.IsNullOrEmpty())
            {
                DateTime _currentTimeStamp = DateTime.Now;
                foreach (String docName in lstFileName)
                {
                    Boolean isDocAlreadySaved = _dbContext.FingerPrintDocumentStagings.Where(Cond => !Cond.FPDS_IsDeleted && Cond.FPDS_DocName.ToLower() == docName.ToLower()).Any();
                    if (!isDocAlreadySaved.IsNullOrEmpty() && !isDocAlreadySaved)
                    {
                        FingerPrintDocumentStaging fingerPrintDocStaging = new FingerPrintDocumentStaging();
                        fingerPrintDocStaging.FPDS_DocName = docName;
                        fingerPrintDocStaging.FPDS_CreatedBy = currentLoggedInUserId;
                        fingerPrintDocStaging.FPDS_CreatedOn = _currentTimeStamp;
                        _dbContext.FingerPrintDocumentStagings.AddObject(fingerPrintDocStaging);
                    }
                }
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        List<String> IFingerPrintClientRepository.GetFingerPrintDocStaging()
        {
            List<String> lstFingerPrintDocName = new List<String>();
            lstFingerPrintDocName = _dbContext.FingerPrintDocumentStagings.Where(cond => !cond.FPDS_IsDeleted).Select(sel => sel.FPDS_DocName).ToList();
            return lstFingerPrintDocName;
        }

        List<FingerPrintOrderContract> IFingerPrintClientRepository.GetInProgressFingerPrintOrders(Int32 completedOrderStatusTypeId)
        {
            List<FingerPrintOrderContract> lstFingerPrintOrderContract = new List<FingerPrintOrderContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                base.OpenSQLDataReaderConnection(con);
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetInProgressFingerPrintOrders"))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            FingerPrintOrderContract fingerPrintOrderContract = new FingerPrintOrderContract();
                            fingerPrintOrderContract.OrderId = dr["OrderID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["OrderID"]);
                            fingerPrintOrderContract.OrderNumber = dr["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(dr["OrderNumber"]);
                            fingerPrintOrderContract.ApplicantOrgUserID = dr["ApplicantOrgUserID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["ApplicantOrgUserID"]);
                            fingerPrintOrderContract.OrderResultXML = dr["OrderResultXML"] == DBNull.Value ? String.Empty : Convert.ToString(dr["OrderResultXML"]);
                            fingerPrintOrderContract.BkgOrderId = dr["BkgOrderId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BkgOrderId"]);
                            fingerPrintOrderContract.BkgOrderStatusId = dr["BkgOrderStatusId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["BkgOrderStatusId"]);
                            fingerPrintOrderContract.BkgOrderStatusCode = dr["BkgOrderStatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["BkgOrderStatusCode"]);
                            fingerPrintOrderContract.SkipABIReview = dr["SkipABIReview"] == DBNull.Value ? false : Convert.ToBoolean(dr["SkipABIReview"]);
                            fingerPrintOrderContract.AADID = dr["AppointmentDetailId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AppointmentDetailId"]);
                            lstFingerPrintOrderContract.Add(fingerPrintOrderContract);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstFingerPrintOrderContract;
        }

        Boolean IFingerPrintClientRepository.SaveFingerprintApplicantDocument(List<FingerPrintOrderContract> lstFingerPrintOrderContract, Int32 bkgProcessUserID)
        {
            List<Int32> lstApplicantDocId = new List<Int32>();

            if (!lstFingerPrintOrderContract.IsNullOrEmpty())
            {
                foreach (FingerPrintOrderContract FingerPrintOrderContract in lstFingerPrintOrderContract)
                {
                    if (!FingerPrintOrderContract.IsNullOrEmpty() && !FingerPrintOrderContract.ApplicantDocument.IsNullOrEmpty())
                        _dbContext.ApplicantDocuments.AddObject(FingerPrintOrderContract.ApplicantDocument);
                }
            }

            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            //{
            //    lstFingerPrintOrderContract.ForEach(x =>
            //        {
            //            Int32 AppDocId = x.ApplicantDocument.ApplicantDocumentID;
            //            lstApplicantDocId.Add(AppDocId);
            //        });
            //}
            return false;
        }

        Boolean IFingerPrintClientRepository.SaveFingerprintApplicantImages(List<FingerPrintOrderContract> lstFingerPrintOrderContract, Int32 bkgProcessUserID)
        {
            List<Int32> lstApplicantDocId = new List<Int32>();

            if (!lstFingerPrintOrderContract.IsNullOrEmpty()
                && lstFingerPrintOrderContract.Any(fp => fp.ApplicantFingerPrintFileImages != null
                && fp.ApplicantFingerPrintFileImages.Any()))
            {
                foreach (var fingerPrintFileImage in lstFingerPrintOrderContract
                    .Where(s => s.ApplicantFingerPrintFileImages != null)
                    .SelectMany(s => s.ApplicantFingerPrintFileImages))
                {
                    if (!fingerPrintFileImage.IsNullOrEmpty() && !fingerPrintFileImage.AFFI_FilePath.IsNullOrEmpty())
                        _dbContext.ApplicantFingerPrintFileImages.AddObject(new ApplicantFingerPrintFileImage
                        {
                            AFFI_ApplicantAppointmentDetailID = fingerPrintFileImage.AFFI_ApplicantAppointmentDetailID,
                            AFFI_DocumentPath = fingerPrintFileImage.AFFI_FilePath,
                            AFFI_FileName = fingerPrintFileImage.AFFI_FileName,
                            AFFI_CreatedBy = bkgProcessUserID,
                            AFFI_CreatedOn = DateTime.Now
                        });
                }
            }

            return _dbContext.SaveChanges() > AppConsts.NONE;
        }

        Boolean IFingerPrintClientRepository.DeleteFingerPrintDocStaging(List<String> lstFingerPrintDocStagingToDelete, Int32 bkgProcessUserID)
        {
            DateTime _currentTimeStamp = DateTime.Now;
            List<FingerPrintDocumentStaging> lstFingerPrintDocStaging = _dbContext.FingerPrintDocumentStagings.Where(cond => lstFingerPrintDocStagingToDelete.Contains(cond.FPDS_DocName) && !cond.FPDS_IsDeleted).ToList();
            if (!lstFingerPrintDocStaging.IsNullOrEmpty())
            {
                foreach (FingerPrintDocumentStaging fingerPrintDocumentStaging in lstFingerPrintDocStaging)
                {
                    fingerPrintDocumentStaging.FPDS_IsDeleted = true;
                    fingerPrintDocumentStaging.FPDS_ModifiedBy = bkgProcessUserID;
                    fingerPrintDocumentStaging.FPDS_ModifiedOn = _currentTimeStamp;
                }
            }

            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IFingerPrintClientRepository.AddDocInApplicantAppointmentDetail(List<FingerPrintOrderContract> lstFingerPrintOrderContract, Int32 bkgProcessUserID)
        {
            if (!lstFingerPrintOrderContract.IsNullOrEmpty())
            {
                DateTime _currentTimeStamp = DateTime.Now;
                List<lkpAppointmentStatu> AppointmentStatusList = LocationDBContext.lkpAppointmentStatus.ToList();
                lkpAppointmentStatu pendingAppointmentStatus = AppointmentStatusList.FirstOrDefault(x => x.AS_Code == FingerPrintAppointmentStatus.PENDING_ABI_REVIEW.GetStringValue());

                foreach (FingerPrintOrderContract item in lstFingerPrintOrderContract)
                {
                    ApplicantAppointmentDetail applicantAppointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(cond => !cond.AAD_IsDeleted
                        && cond.AAD_OrganizationUserID == item.ApplicantOrgUserID
                        && cond.AAD_OrderID == item.OrderId
                        && cond.AAD_IsActive).FirstOrDefault();

                    List<ApplicantAppointmentDetailsExt> applicantAppointmentDetailExt = _dbContext.ApplicantAppointmentDetailsExts.
                        Where(cond => !cond.AADE_IsDeleted
                        && cond.AADE_IsActive
                        && cond.ApplicantAppointmentDetail.AAD_OrganizationUserID == item.ApplicantOrgUserID
                        && cond.ApplicantAppointmentDetail.AAD_OrderID == item.OrderId
                        && cond.ApplicantAppointmentDetail.AAD_IsActive).ToList();
                    if (!applicantAppointmentDetailExt.IsNullOrEmpty() && !item.ApplicantDocument.IsNullOrEmpty())
                    {
                        foreach (var ext in applicantAppointmentDetailExt)
                        {

                            ext.AADE_FingerPrintFileDocumentID = item.ApplicantDocument.ApplicantDocumentID;
                            ext.AADE_ModifiedOn = _currentTimeStamp;
                            ext.AADE_ModifiedBy = bkgProcessUserID;
                        }
                    }
                    if (!AppointmentStatusList.IsNullOrEmpty() && AppointmentStatusList.Count > AppConsts.NONE && !item.SkipABIReview)
                    {
                        FingerPrintApplicantAppointment fingerPrintApplicantAppointment = LocationDBContext.FingerPrintApplicantAppointments.FirstOrDefault(x => x.FPAA_OrderID == item.OrderId && !x.FPAA_IsDeleted);

                        UpdateFingerPrintApplicantAppointmentAudit(fingerPrintApplicantAppointment, AppointmentStatusList, pendingAppointmentStatus, bkgProcessUserID, applicantAppointmentDetail.AAD_ID);
                        //if (!fingerPrintApplicantAppointment.IsNullOrEmpty())
                        //{
                        //    lkpAppointmentStatu oldStatus = AppointmentStatusList.FirstOrDefault(x => x.AS_ID == fingerPrintApplicantAppointment.FPAA_Status.Value);
                        //    fingerPrintApplicantAppointment.FPAA_Status = pendingAppointmentStatus.AS_ID;
                        //    fingerPrintApplicantAppointment.FPAA_ModifiedBy = bkgProcessUserID;
                        //    fingerPrintApplicantAppointment.FPAA_ModifiedOn = DateTime.Now;
                        //    if (LocationDBContext.SaveChanges() > AppConsts.NONE)
                        //    {
                        //        LocationServiceAppointmentAudit locationServiceAppointmentAudit = new LocationServiceAppointmentAudit
                        //        {
                        //            LSAA_AppointmentID = applicantAppointmentDetail.AAD_ID,
                        //            LSAA_ChangeTypeID = changeTypelist.ACT_ID,
                        //            LSAA_CreatedBy = bkgProcessUserID,
                        //            LSAA_CreatedOn = DateTime.Now,
                        //            LSAA_DateUpdated = DateTime.Now,
                        //            LSAA_Description = "Status Change > " + oldStatus.AS_Name + " --> " + pendingAppointmentStatus.AS_Name,
                        //            LSAA_IsDeleted = false,
                        //            LSAA_IsOnsite = fingerPrintApplicantAppointment.FPAA_IsOnsiteAppointment,
                        //            LSAA_IsOutOfState = fingerPrintApplicantAppointment.FPAA_IsOutOfState,
                        //            LSAA_LocationId = fingerPrintApplicantAppointment.FPAA_LocationID,
                        //            LSAA_NewValue = pendingAppointmentStatus.AS_ID.ToString(),
                        //            LSAA_OldValue = oldStatus.AS_ID.ToString()
                        //        };


                        //    }

                        //}

                    }
                }
            }

            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Boolean IFingerPrintClientRepository.SaveUpdateAppointmentAudit(AppointmentOrderScheduleContract AppOrdSchdContract, AppointmentSlotContract scheduleAppointmentContract, Int32 currentLoggedInUserId, Int32 OldStatusId)
        {

            var appointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == scheduleAppointmentContract.OrderId
                                                                        && x.AAD_OrganizationUserID == scheduleAppointmentContract.ApplicantOrgUserId && !x.AAD_IsDeleted && x.AAD_IsActive).FirstOrDefault();
            var FingerprintsCompletedSuccessfullyStatus = base.LocationDBContext.lkpAppointmentStatus.Where(x => x.AS_Code == "AAAI" && !x.AS_IsDeleted).FirstOrDefault();
            List<lkpAppointmentChangeType> changeTypelist = _dbContext.lkpAppointmentChangeTypes.Where(x => !x.ACT_IsDeleted).ToList();

            // Status For Addittional Services
            List<lkpCABSServiceStatu> AllAdditionalStatus = _dbContext.lkpCABSServiceStatus.ToList();
            lkpCABSServiceStatu RejectAdditional = AllAdditionalStatus.Where(x => x.CSS_Code == "AAAD").FirstOrDefault();
            lkpCABSServiceStatu NewAdditional = AllAdditionalStatus.Where(x => x.CSS_Code == "AAAE").FirstOrDefault();
            int cabsTypeStatusId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;

            List<ApplicantAppointmentDetailsExt> applicantAppointmentExts = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == appointmentDetail.AAD_ID
                                                          && !x.AADE_IsDeleted && x.AADE_IsActive && x.AADE_ParentAppAppointmentDetailID == null).ToList();

            //// Enter the values for Mode change into the audit table
            if (AppOrdSchdContract.IsOnsiteAppointment != scheduleAppointmentContract.IsOnsiteAppointment)
            {
                LocationServiceAppointmentAudit AppointmentAuditData = new LocationServiceAppointmentAudit();
                AppointmentAuditData.LSAA_AppointmentID = appointmentDetail.IsNullOrEmpty() ? 0 : appointmentDetail.AAD_ID;
                String oldAppointmentMode = AppOrdSchdContract.IsOnsiteAppointment ? "OnSite Event" : "Colorado Fingerprinting Site";
                String newAppointmentMode = (scheduleAppointmentContract.IsOnsiteAppointment.HasValue && scheduleAppointmentContract.IsOnsiteAppointment.Value) ? "OnSite Event" : "Colorado Fingerprinting Site";
                var stsCode = LocationAppointmentAuditChangeType.MODE_CHANGE.GetStringValue();
                AppointmentAuditData.LSAA_ChangeTypeID = changeTypelist.Where(x => x.ACT_Code.Trim() == stsCode).FirstOrDefault().ACT_ID;
                AppointmentAuditData.LSAA_CreatedBy = currentLoggedInUserId;
                AppointmentAuditData.LSAA_CreatedOn = DateTime.Now;
                AppointmentAuditData.LSAA_DateUpdated = DateTime.Now;
                AppointmentAuditData.LSAA_Description = "Mode Changed > " + oldAppointmentMode + " --> " + newAppointmentMode;
                AppointmentAuditData.LSAA_NewValue = null;
                AppointmentAuditData.LSAA_OldValue = null;
                AppointmentAuditData.LSAA_LocationId = scheduleAppointmentContract.LocationId;
                AppointmentAuditData.LSAA_IsOnsite = scheduleAppointmentContract.IsEventType;
                AppointmentAuditData.LSAA_IsOutOfState = false;
                _dbContext.LocationServiceAppointmentAudits.AddObject(AppointmentAuditData);
            }

            ////  Enter the value for location change audit
            if (AppOrdSchdContract.LocationId != scheduleAppointmentContract.LocationId)
            {
                if (applicantAppointmentExts.IsNotNull())
                {
                        if (OldStatusId != FingerprintsCompletedSuccessfullyStatus.AS_ID)
                        {
                            LocationServiceAppointmentAudit AppointmentAuditData = new LocationServiceAppointmentAudit();
                            AppointmentAuditData.LSAA_AppointmentID = appointmentDetail.IsNullOrEmpty() ? 0 : appointmentDetail.AAD_ID;
                            var locStsCode = LocationAppointmentAuditChangeType.LOCATION_CHANGE.GetStringValue();
                            var newLoc = base.LocationDBContext.FingerPrintLocations.Where(x => x.FPL_ID == scheduleAppointmentContract.LocationId).FirstOrDefault();
                            AppointmentAuditData.LSAA_AppointmentID = appointmentDetail.IsNullOrEmpty() ? 0 : appointmentDetail.AAD_ID;
                            AppointmentAuditData.LSAA_ChangeTypeID = changeTypelist.Where(x => x.ACT_Code.Trim() == locStsCode).FirstOrDefault().ACT_ID;
                            AppointmentAuditData.LSAA_CreatedBy = currentLoggedInUserId;
                            AppointmentAuditData.LSAA_CreatedOn = DateTime.Now;
                            AppointmentAuditData.LSAA_DateUpdated = DateTime.Now;
                            AppointmentAuditData.LSAA_Description = "Location Changed > " + AppOrdSchdContract.LocationName + " --> " + newLoc.FPL_Name;
                            AppointmentAuditData.LSAA_NewValue = newLoc.FPL_ID.ToString();
                            AppointmentAuditData.LSAA_OldValue = AppOrdSchdContract.LocationId.ToString();
                            AppointmentAuditData.LSAA_LocationId = newLoc.FPL_ID;
                            AppointmentAuditData.LSAA_IsOnsite = scheduleAppointmentContract.IsEventType;
                            AppointmentAuditData.LSAA_IsOutOfState = false;
                            _dbContext.LocationServiceAppointmentAudits.AddObject(AppointmentAuditData);
                        }
                        else
                        {
                            LocationServiceAppointmentAudit AppointmentAuditData = new LocationServiceAppointmentAudit();
                            AppointmentAuditData.LSAA_AppointmentID = appointmentDetail.IsNullOrEmpty() ? 0 : appointmentDetail.AAD_ID;
                            var locStsCode = LocationAppointmentAuditChangeType.ADDITIONAL_SERVICES_STATUS_CHANGE.GetStringValue();
                            var newLoc = base.LocationDBContext.FingerPrintLocations.Where(x => x.FPL_ID == scheduleAppointmentContract.LocationId).FirstOrDefault();
                            AppointmentAuditData.LSAA_AppointmentID = appointmentDetail.IsNullOrEmpty() ? 0 : appointmentDetail.AAD_ID;
                            AppointmentAuditData.LSAA_ChangeTypeID = changeTypelist.Where(x => x.ACT_Code.Trim() == locStsCode).FirstOrDefault().ACT_ID;
                            AppointmentAuditData.LSAA_CreatedBy = currentLoggedInUserId;
                            AppointmentAuditData.LSAA_CreatedOn = DateTime.Now;
                            AppointmentAuditData.LSAA_DateUpdated = DateTime.Now;
                            AppointmentAuditData.LSAA_Description = "Location Changed > " + AppOrdSchdContract.LocationName + " --> " + newLoc.FPL_Name;
                            AppointmentAuditData.LSAA_NewValue = newLoc.FPL_ID.ToString();
                            AppointmentAuditData.LSAA_OldValue = AppOrdSchdContract.LocationId.ToString();
                            AppointmentAuditData.LSAA_LocationId = newLoc.FPL_ID;
                            AppointmentAuditData.LSAA_IsOnsite = scheduleAppointmentContract.IsEventType;
                            AppointmentAuditData.LSAA_IsOutOfState = false;
                            _dbContext.LocationServiceAppointmentAudits.AddObject(AppointmentAuditData);
                        }
                }
            }            

            ////  Enter the values for schedule change audit
            if (AppOrdSchdContract.SlotId != scheduleAppointmentContract.SlotID && AppOrdSchdContract.SlotId > AppConsts.NONE && scheduleAppointmentContract.SlotID > AppConsts.NONE)
            {
                if (applicantAppointmentExts.IsNotNull())
                {
                    if (OldStatusId != FingerprintsCompletedSuccessfullyStatus.AS_ID)
                    {
                        LocationServiceAppointmentAudit AppointmentAuditData = new LocationServiceAppointmentAudit();
                        AppointmentAuditData.LSAA_AppointmentID = appointmentDetail.IsNullOrEmpty() ? 0 : appointmentDetail.AAD_ID;
                        var stsCode = LocationAppointmentAuditChangeType.SCHEDULE_CHANGE.GetStringValue();
                        AppointmentAuditData.LSAA_ChangeTypeID = changeTypelist.Where(x => x.ACT_Code.Trim() == stsCode).FirstOrDefault().ACT_ID;
                        AppointmentAuditData.LSAA_CreatedBy = currentLoggedInUserId;
                        AppointmentAuditData.LSAA_CreatedOn = DateTime.Now;
                        AppointmentAuditData.LSAA_DateUpdated = DateTime.Now;
                        AppointmentAuditData.LSAA_Description = "Schedule Changed > " + (AppOrdSchdContract.StartDateTime.HasValue ? AppOrdSchdContract.StartDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null) + " --> " + (scheduleAppointmentContract.StartDateTime.HasValue ? scheduleAppointmentContract.StartDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null);
                        AppointmentAuditData.LSAA_NewValue = scheduleAppointmentContract.SlotID.ToString();
                        AppointmentAuditData.LSAA_OldValue = AppOrdSchdContract.SlotId.ToString();
                        AppointmentAuditData.LSAA_LocationId = scheduleAppointmentContract.LocationId;
                        AppointmentAuditData.LSAA_IsOnsite = scheduleAppointmentContract.IsEventType;
                        AppointmentAuditData.LSAA_IsOutOfState = false;
                        _dbContext.LocationServiceAppointmentAudits.AddObject(AppointmentAuditData);
                    }
                    else
                    {
                        LocationServiceAppointmentAudit AppointmentAuditData = new LocationServiceAppointmentAudit();
                        AppointmentAuditData.LSAA_AppointmentID = appointmentDetail.IsNullOrEmpty() ? 0 : appointmentDetail.AAD_ID;
                        var stsCode = LocationAppointmentAuditChangeType.ADDITIONAL_SERVICES_STATUS_CHANGE.GetStringValue();
                        AppointmentAuditData.LSAA_ChangeTypeID = changeTypelist.Where(x => x.ACT_Code.Trim() == stsCode).FirstOrDefault().ACT_ID;
                        AppointmentAuditData.LSAA_CreatedBy = currentLoggedInUserId;
                        AppointmentAuditData.LSAA_CreatedOn = DateTime.Now;
                        AppointmentAuditData.LSAA_DateUpdated = DateTime.Now;
                        AppointmentAuditData.LSAA_Description = "Schedule Changed > " + (AppOrdSchdContract.StartDateTime.HasValue ? AppOrdSchdContract.StartDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null) + " --> " + (scheduleAppointmentContract.StartDateTime.HasValue ? scheduleAppointmentContract.StartDateTime.Value.ToString("MM/dd/yyyy hh:mm tt") : null);
                        AppointmentAuditData.LSAA_NewValue = scheduleAppointmentContract.SlotID.ToString();
                        AppointmentAuditData.LSAA_OldValue = AppOrdSchdContract.SlotId.ToString();
                        AppointmentAuditData.LSAA_LocationId = scheduleAppointmentContract.LocationId;
                        AppointmentAuditData.LSAA_IsOnsite = scheduleAppointmentContract.IsEventType;
                        AppointmentAuditData.LSAA_IsOutOfState = false;
                        _dbContext.LocationServiceAppointmentAudits.AddObject(AppointmentAuditData);
                    }
                }
            }


            //// Enter the values for status change in audit
            var activeStatus = base.LocationDBContext.lkpAppointmentStatus.Where(x => x.AS_Code == "AAAA" && !x.AS_IsDeleted).FirstOrDefault();
            var FingerprintsCompletedStatus = base.LocationDBContext.lkpAppointmentStatus.Where(x => x.AS_Code == "AAAG" && !x.AS_IsDeleted).FirstOrDefault();
            if (OldStatusId > AppConsts.NONE && OldStatusId != activeStatus.AS_ID && OldStatusId != FingerprintsCompletedSuccessfullyStatus.AS_ID && OldStatusId != FingerprintsCompletedStatus.AS_ID)
            {
                LocationServiceAppointmentAudit AppointmentAuditData = new LocationServiceAppointmentAudit();
                AppointmentAuditData.LSAA_AppointmentID = appointmentDetail.IsNullOrEmpty() ? 0 : appointmentDetail.AAD_ID;
                String oldStatusName = base.LocationDBContext.lkpAppointmentStatus.Where(x => x.AS_ID == OldStatusId && !x.AS_IsDeleted).FirstOrDefault().AS_Name;
                var stsCode = LocationAppointmentAuditChangeType.STATUS_CHANGE.GetStringValue();
                AppointmentAuditData.LSAA_ChangeTypeID = changeTypelist.Where(x => x.ACT_Code.Trim() == stsCode).FirstOrDefault().ACT_ID;
                AppointmentAuditData.LSAA_CreatedBy = currentLoggedInUserId;
                AppointmentAuditData.LSAA_CreatedOn = DateTime.Now;
                AppointmentAuditData.LSAA_DateUpdated = DateTime.Now;
                AppointmentAuditData.LSAA_Description = "Status Changed > " + oldStatusName + " --> " + activeStatus.AS_Name;
                AppointmentAuditData.LSAA_NewValue = activeStatus.AS_ID.ToString();
                AppointmentAuditData.LSAA_OldValue = OldStatusId.ToString();
                AppointmentAuditData.LSAA_LocationId = scheduleAppointmentContract.LocationId;
                AppointmentAuditData.LSAA_IsOnsite = scheduleAppointmentContract.IsEventType;
                AppointmentAuditData.LSAA_IsOutOfState = false;
                _dbContext.LocationServiceAppointmentAudits.AddObject(AppointmentAuditData);
            }

            //// Enter the values for Additional Service Status change in audit
            if (applicantAppointmentExts.IsNotNull())
            {
                foreach (var applicantAppointmentExtItems in applicantAppointmentExts)
                {                                                         
                    if (applicantAppointmentExtItems.AADE_ServiceTypeID != cabsTypeStatusId)
                    {
                        if (applicantAppointmentExtItems.AADE_ServiceStatus == RejectAdditional.CSS_ID)
                        {
                            LocationServiceAppointmentAudit AppointmentAuditData = new LocationServiceAppointmentAudit();
                            AppointmentAuditData.LSAA_AppointmentID = appointmentDetail.IsNullOrEmpty() ? 0 : appointmentDetail.AAD_ID;
                            string oldStatusName = AllAdditionalStatus.Where(x => x.CSS_ID == applicantAppointmentExtItems.AADE_ServiceStatus).FirstOrDefault().CSS_Name;
                            var stsCode = LocationAppointmentAuditChangeType.ADDITIONAL_SERVICES_STATUS_CHANGE.GetStringValue();
                            AppointmentAuditData.LSAA_ChangeTypeID = changeTypelist.Where(x => x.ACT_Code.Trim() == stsCode).FirstOrDefault().ACT_ID;      
                            AppointmentAuditData.LSAA_CreatedBy = currentLoggedInUserId;
                            AppointmentAuditData.LSAA_CreatedOn = DateTime.Now;
                            AppointmentAuditData.LSAA_DateUpdated = DateTime.Now;
                            AppointmentAuditData.LSAA_Description = "Additional Service Status Change > " + oldStatusName + " --> " + NewAdditional.CSS_Name;
                            AppointmentAuditData.LSAA_NewValue = NewAdditional.CSS_ID.ToString();
                            AppointmentAuditData.LSAA_OldValue = applicantAppointmentExtItems.AADE_ServiceStatus.ToString();
                            AppointmentAuditData.LSAA_LocationId = scheduleAppointmentContract.LocationId;
                            AppointmentAuditData.LSAA_IsOnsite = scheduleAppointmentContract.IsEventType;
                            AppointmentAuditData.LSAA_IsOutOfState = false;
                            _dbContext.LocationServiceAppointmentAudits.AddObject(AppointmentAuditData);
                        }                        
                    }                    
                }
            }



            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;

        }
        Boolean IFingerPrintClientRepository.SaveUpdateApointmentRefundAudit(RefundHistory refundData, Int32 ApplicantOrgUserId, Int32 CurrentUserId)
        {
            LocationServiceAppointmentAudit AppointmentAuditData = new LocationServiceAppointmentAudit();
            var appointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == refundData.RH_OrderID && x.AAD_OrganizationUserID == ApplicantOrgUserId
                                                                                 && !x.AAD_IsDeleted && x.AAD_IsActive).FirstOrDefault();
            var applicantAppointment = base.LocationDBContext.FingerPrintApplicantAppointments.Where(x => x.FPAA_OrderID == refundData.RH_OrderID && x.FPAA_OrganizationUserID == ApplicantOrgUserId
                                                                                 && !x.FPAA_IsDeleted).FirstOrDefault();
            AppointmentAuditData.LSAA_AppointmentID = appointmentDetail.IsNullOrEmpty() ? 0 : appointmentDetail.AAD_ID;
            var stsCode = LocationAppointmentAuditChangeType.REFUND_UPDATE.GetStringValue();
            AppointmentAuditData.LSAA_ChangeTypeID = _dbContext.lkpAppointmentChangeTypes.Where(x => x.ACT_Code == stsCode).FirstOrDefault().ACT_ID;
            AppointmentAuditData.LSAA_CreatedBy = CurrentUserId;
            AppointmentAuditData.LSAA_CreatedOn = DateTime.Now;
            AppointmentAuditData.LSAA_DateUpdated = DateTime.Now;
            var msg = refundData.RH_IsSuccess.Value ? "The amount of $" + refundData.RH_Amount.ToString("0.00") + " was successfully refunded." : "Refund of amount $" + refundData.RH_Amount.ToString("0.00") + " was failed.";
            AppointmentAuditData.LSAA_Description = msg;
            AppointmentAuditData.LSAA_NewValue = refundData.RH_Amount.ToString();
            AppointmentAuditData.LSAA_LocationId = applicantAppointment.FPAA_LocationID;
            AppointmentAuditData.LSAA_IsOnsite = applicantAppointment.FPAA_IsOnsiteAppointment.IsNull() ? false : applicantAppointment.FPAA_IsOnsiteAppointment;
            AppointmentAuditData.LSAA_IsOutOfState = applicantAppointment.FPAA_IsOutOfState;
            _dbContext.LocationServiceAppointmentAudits.AddObject(AppointmentAuditData);
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }


        /// <summary>
        /// usp_GetMailingDetail
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        PreviousAddressContract IFingerPrintClientRepository.GetMailingDetail(Int32 OrderID)
        {
            PreviousAddressContract objMailingAddressDetail = new PreviousAddressContract();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@OrderID",OrderID)
                };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetMailingDetail", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            objMailingAddressDetail.Address1 = dr["Address1"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Address1"]);
                            objMailingAddressDetail.MailingOption = dr["MailingOption"] == DBNull.Value ? string.Empty : Convert.ToString(dr["MailingOption"]);
                            objMailingAddressDetail.CityName = dr["CityName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CityName"]);
                            objMailingAddressDetail.StateName = dr["StateName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["StateName"]);
                            objMailingAddressDetail.Zipcode = dr["Zipcode"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Zipcode"]);
                            objMailingAddressDetail.CountyName = dr["Country"] == DBNull.Value ? string.Empty : Convert.ToString(dr["Country"]);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return objMailingAddressDetail;
        }


        Boolean IFingerPrintClientRepository.IsPrinterAvailableAtOldLoc(Int32 OrderId)
        {
            var cabsServiceDetail = _dbContext.CABSServiceOrderDetails.Where(y => y.CSOD_OrderID == OrderId).Select(y => y.CSOD_ServiceDetails).FirstOrDefault();
            XmlNodeList xmlnode;
            bool IsPrinterAvailableAtOldLoc = false;
            //string mailingOption = string.Empty;
            if (cabsServiceDetail != null)
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(cabsServiceDetail);
                xmlnode = xdoc.GetElementsByTagName("BkgpkgData");
                for (int i = 0; i <= xmlnode.Count - 1; i++)
                {
                    if (xmlnode[i].ChildNodes.Count > 8)
                    {
                        string mailingOption = xmlnode[i].ChildNodes.Item(8).InnerText.Trim();
                        if (Convert.ToInt32(mailingOption) > 0)
                        {
                            IsPrinterAvailableAtOldLoc = false;
                            //return mailingOption;
                        }
                    }
                    else
                    {
                        IsPrinterAvailableAtOldLoc = true;
                    }
                }
            }

            return IsPrinterAvailableAtOldLoc;

        }
        Boolean IFingerPrintClientRepository.SaveUpdateAppointmentStatusAudit(AppointmentOrderScheduleContract AppointmentData, String statusCode, Int32 BackgroundProcessId, Int32 OldStatusId)
        {
            LocationServiceAppointmentAudit AppointmentAuditData = new LocationServiceAppointmentAudit();
            var appointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == AppointmentData.OrderId && x.AAD_OrganizationUserID == AppointmentData.ApplicantOrgUserId
                                                                                 & !x.AAD_IsDeleted && x.AAD_IsActive).FirstOrDefault();

            List<ApplicantAppointmentDetailsExt> applicantAppointmentExts = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == appointmentDetail.AAD_ID
                                                      && !x.AADE_IsDeleted && x.AADE_IsActive && x.AADE_ParentAppAppointmentDetailID == null).ToList();
            
            var AppointmentStatusList = base.LocationDBContext.lkpAppointmentStatus.ToList();

            String OldStatusName = AppointmentStatusList.Where(x => x.AS_ID == OldStatusId && !x.AS_IsDeleted).FirstOrDefault().AS_Name;
            String NewStatusName = AppointmentStatusList.Where(x => x.AS_Code == statusCode && !x.AS_IsDeleted).FirstOrDefault().AS_Name;
            Int32 NewStatusCodeID = AppointmentStatusList.Where(x => x.AS_Code == statusCode && !x.AS_IsDeleted).FirstOrDefault().AS_ID; ;
            var stsCode = LocationAppointmentAuditChangeType.STATUS_CHANGE.GetStringValue();
            var AddServicestsCode = LocationAppointmentAuditChangeType.ADDITIONAL_SERVICES_STATUS_CHANGE.GetStringValue();
            int cabsTypeStatusId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;

            if (applicantAppointmentExts.IsNotNull())
            {
                foreach (var applicantAppointmentExtItems in applicantAppointmentExts)
                {
                    if (applicantAppointmentExtItems.AADE_ServiceTypeID == cabsTypeStatusId)
                    {
                        AppointmentAuditData.LSAA_AppointmentID = appointmentDetail.IsNullOrEmpty() ? 0 : appointmentDetail.AAD_ID;
                        AppointmentAuditData.LSAA_ChangeTypeID = _dbContext.lkpAppointmentChangeTypes.Where(x => x.ACT_Code == stsCode).FirstOrDefault().ACT_ID;
                        AppointmentAuditData.LSAA_CreatedBy = BackgroundProcessId;
                        AppointmentAuditData.LSAA_CreatedOn = DateTime.Now;
                        AppointmentAuditData.LSAA_DateUpdated = DateTime.Now;
                        AppointmentAuditData.LSAA_Description = "Status Changed > " + OldStatusName + " --> " + NewStatusName;
                        AppointmentAuditData.LSAA_NewValue = NewStatusCodeID.ToString();
                        AppointmentAuditData.LSAA_OldValue = OldStatusId.ToString();
                        AppointmentAuditData.LSAA_LocationId = AppointmentData.LocationId;
                        AppointmentAuditData.LSAA_IsOnsite = AppointmentData.IsOnsiteAppointment;
                        AppointmentAuditData.LSAA_IsOutOfState = AppointmentData.IsOutOfStateAppointment;
                        _dbContext.LocationServiceAppointmentAudits.AddObject(AppointmentAuditData);
                    }
                    else
                    {
                        AppointmentAuditData.LSAA_AppointmentID = appointmentDetail.IsNullOrEmpty() ? 0 : appointmentDetail.AAD_ID;
                        AppointmentAuditData.LSAA_ChangeTypeID = _dbContext.lkpAppointmentChangeTypes.Where(x => x.ACT_Code == AddServicestsCode).FirstOrDefault().ACT_ID;
                        AppointmentAuditData.LSAA_CreatedBy = BackgroundProcessId;
                        AppointmentAuditData.LSAA_CreatedOn = DateTime.Now;
                        AppointmentAuditData.LSAA_DateUpdated = DateTime.Now;
                        AppointmentAuditData.LSAA_Description = "Additional Service Status Change > " + OldStatusName + " --> " + NewStatusName;
                        AppointmentAuditData.LSAA_NewValue = NewStatusCodeID.ToString();
                        AppointmentAuditData.LSAA_OldValue = OldStatusId.ToString();
                        AppointmentAuditData.LSAA_LocationId = AppointmentData.LocationId;
                        AppointmentAuditData.LSAA_IsOnsite = AppointmentData.IsOnsiteAppointment;
                        AppointmentAuditData.LSAA_IsOutOfState = AppointmentData.IsOutOfStateAppointment;
                        _dbContext.LocationServiceAppointmentAudits.AddObject(AppointmentAuditData);
                    }
                }
            }

            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        string IFingerPrintClientRepository.GetServiceDescription()
        {

            var result = _dbContext.BackgroundServices.Where(cond => !cond.BSE_IsDeleted && (cond.BSE_Name == AppConsts.CBI_SERVICE_NAME))
           .Select(cond => cond.BSE_Description).FirstOrDefault();
            return result.ToString();
        }

        Dictionary<String, String> IFingerPrintClientRepository.ValidateCBIUniqueID(String CBIUniqueID)
        {
            Dictionary<String, String> dic = new Dictionary<String, String>();

            var attributeId = _dbContext.CascadingAttributeOptions.Where(cond => cond.BkgSvcAttribute.BSA_Name == "CBIUniqueID"
                                                       && !cond.BkgSvcAttribute.BSA_IsDeleted
                                                       && cond.CAO_Value == CBIUniqueID
                                                       && !cond.CAO_IsDeleted).Select(sel => sel.CAO_AttributeID).FirstOrDefault();
            if (attributeId > AppConsts.NONE)
            {
                dic.Add("IsValidID", "True");
                String IsSSNRequired = _dbContext.CascadingAttributeOptions.Where(cond => cond.BkgSvcAttribute.BSA_Name == "SSNReqd"
                                                       && !cond.BkgSvcAttribute.BSA_IsDeleted
                                                       && cond.CAO_SourceValue == CBIUniqueID
                                                       && !cond.CAO_IsDeleted).Select(sel => sel.CAO_Value).FirstOrDefault();
                dic.Add("IsSSNRequired", IsSSNRequired);
                dic.Add(attributeId.ToString(), CBIUniqueID);

                String reasonFingerprintedValue = _dbContext.CascadingAttributeOptions.Where(cond => cond.BkgSvcAttribute.BSA_Name == "CBIUniqueID"
                                       && !cond.BkgSvcAttribute.BSA_IsDeleted
                                       && cond.CAO_Value == CBIUniqueID
                                       && !cond.CAO_IsDeleted).Select(sel => sel.CAO_SourceValue).FirstOrDefault();

                Int32 reasonFingerprintedID = _dbContext.CascadingAttributeOptions.Where(cond => cond.BkgSvcAttribute.BSA_Name == "Reason Fingerprinted"
                       && !cond.BkgSvcAttribute.BSA_IsDeleted
                       && cond.CAO_Value == reasonFingerprintedValue
                       && !cond.CAO_IsDeleted).Select(sel => sel.CAO_AttributeID).FirstOrDefault();

                dic.Add(reasonFingerprintedID.ToString(), reasonFingerprintedValue);

                var cbiBillingCode = "";
                EntityConnection connection = _dbContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlParameter[] sqlParameterCollection = new SqlParameter[]
                    {
                        new SqlParameter("@cbiUniqueId",CBIUniqueID)
                    };
                    base.OpenSQLDataReaderConnection(con);

                    using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "[ams].[usp_GetCBIBillingCode]", sqlParameterCollection))
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                cbiBillingCode = dr["CBS_BillingCode"].ToString();
                            }
                        }
                    }
                    base.CloseSQLDataReaderConnection(con);
                }
                dic.Add(AppConsts.CBIBillingCode, cbiBillingCode);

                string conditionTypeCode = null;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlParameter[] sqlParameterCollection = new SqlParameter[]
                    {
                        new SqlParameter("@AttributeValue",CBIUniqueID),
                        new SqlParameter("@AttributeId",attributeId)
                    };
                    base.OpenSQLDataReaderConnection(con);

                    using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "[ams].[usp_GetSpecialConditionsforAttribute]", sqlParameterCollection))
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                conditionTypeCode = dr["ConditionTypeCode"] == DBNull.Value ? string.Empty : dr["ConditionTypeCode"].ToString();
                            }
                        }
                    }
                    base.CloseSQLDataReaderConnection(con);
                }
                if (!conditionTypeCode.IsNullOrEmpty())
                {
                    if (conditionTypeCode == ConditionType.LEGAL_NAME_CHANGE.GetStringValue())
                    {
                        dic.Add("IsLegalNameChange", "true");
                    }
                }
                else
                {
                    dic.Add("IsLegalNameChange", "false");
                }
            }
            else
            {
                dic.Add("IsValidID", "false");
            }

            return dic;

        }

        // UAT-4271
        List<LookupContract> IFingerPrintClientRepository.GetCBIUniqueIdByAcctNameOrNumber(String acctNameOrNumber)
        {
            List<LookupContract> lstCBIUniqueIds = new List<LookupContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                        new SqlParameter("@acctNameOrNumber",acctNameOrNumber)
                };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "[ams].[usp_GetCBIUniqueIdByAcctNameOrNumber]", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            LookupContract LookupContract = new LookupContract();
                            LookupContract.Code = Convert.ToString(dr["cbiUniqueId"]);
                            LookupContract.Name = Convert.ToString(dr["displayText"]);
                            lstCBIUniqueIds.Add(LookupContract);

                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstCBIUniqueIds;
        }

        Boolean IFingerPrintClientRepository.SavePaymentTypeAuditChange(String PaymentModeCode, Int32 OrderId, Int32 CurrentLoggedInUserId, Int32 oldOrderPaymentDetailId)
        {
            LocationServiceAppointmentAudit AppointmentAuditData = new LocationServiceAppointmentAudit();
            var stsCode = LocationAppointmentAuditChangeType.PAYMENT_TYPE_UPDATE.GetStringValue();
            var appointmentDetail = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == OrderId && !x.AAD_IsDeleted && x.AAD_IsActive).FirstOrDefault();
            int cabsPrice = 0;
            var CabsService =  appointmentDetail.ApplicantAppointmentDetailsExts.Where(x => x.lkpBkgSvcType.BST_Code == "AAAA").ToList();
            if (CabsService.Count !=0 )
            {
                cabsPrice = Convert.ToInt32(CabsService.FirstOrDefault().AADE_TotalPrice);
            }
            Decimal AmountPaidByInstitue = 0;
            var OPDH_Institue = appointmentDetail.Order.OrderPaymentDetailHistories.Where(x => x.OPDH_PaymentOptionID == 5).ToList();
            if (OPDH_Institue.Count != 0)
            {
                AmountPaidByInstitue = Convert.ToInt32(OPDH_Institue.FirstOrDefault().OPDH_Amount);
            }
            if(cabsPrice !=0 && AmountPaidByInstitue!=0 && cabsPrice<= AmountPaidByInstitue)
            {
                stsCode = LocationAppointmentAuditChangeType.ADDITIONAL_SERVICES_STATUS_CHANGE.GetStringValue();
            }
            AppointmentAuditData.LSAA_AppointmentID = appointmentDetail.IsNullOrEmpty() ? 0 : appointmentDetail.AAD_ID;
            var oldOrderPaymentModeId = _dbContext.OrderPaymentDetails.Where(x => x.OPD_ID == oldOrderPaymentDetailId).FirstOrDefault().OPD_PaymentOptionID;
            var paymentOptions = _dbContext.lkpPaymentOptions.Where(x => !x.IsDeleted).ToList();
            var applicantAppointmentData = base.LocationDBContext.FingerPrintApplicantAppointments.Where(x => x.FPAA_OrderID == OrderId && !x.FPAA_IsDeleted).FirstOrDefault();
            AppointmentAuditData.LSAA_ChangeTypeID = _dbContext.lkpAppointmentChangeTypes.Where(x => x.ACT_Code == stsCode).FirstOrDefault().ACT_ID;
            AppointmentAuditData.LSAA_CreatedBy = CurrentLoggedInUserId;
            AppointmentAuditData.LSAA_CreatedOn = DateTime.Now;
            AppointmentAuditData.LSAA_DateUpdated = DateTime.Now;
            AppointmentAuditData.LSAA_Description = "Payment Type Update > " + paymentOptions.Where(x => x.PaymentOptionID == oldOrderPaymentModeId).FirstOrDefault().Name + " --> " + paymentOptions.Where(x => x.Code == PaymentModeCode).FirstOrDefault().Name;
            AppointmentAuditData.LSAA_NewValue = paymentOptions.Where(x => x.Code == PaymentModeCode).FirstOrDefault().PaymentOptionID.ToString();
            AppointmentAuditData.LSAA_OldValue = oldOrderPaymentModeId.ToString();
            AppointmentAuditData.LSAA_LocationId = applicantAppointmentData.FPAA_LocationID;
            AppointmentAuditData.LSAA_IsOnsite = applicantAppointmentData.FPAA_IsOnsiteAppointment;
            AppointmentAuditData.LSAA_IsOutOfState = applicantAppointmentData.FPAA_IsOutOfState;
            _dbContext.LocationServiceAppointmentAudits.AddObject(AppointmentAuditData);
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        #region CDR Export data
        void IFingerPrintClientRepository.ImportDataToTable(string dataToImport, CDRFileDetailContract fileDetailContract, int? backgroundProcessUserId)
        {
            DataTable tblcsv = AddColumnsToDataTable();
            foreach (string csvRow in dataToImport.Split('\n'))
            {
                if (!string.IsNullOrEmpty(csvRow))
                {
                    //Adding each row into datatable  
                    tblcsv.Rows.Add();
                    int count = 0;
                    //byteOrderMArk is an unwanted character. will simply remove this character from the read line.
                    char byteOrderMark = (char)65279;
                    foreach (string FileRec in Regex.Split(csvRow, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))"))
                    {
                        string value = FileRec.Replace("\"", string.Empty);
                        if (!value.IsNullOrEmpty())
                        {
                            if (value.ToCharArray()[0].Equals(byteOrderMark)) { value = value.Remove(0, 1); }
                        }

                        tblcsv.Rows[tblcsv.Rows.Count - 1][count] = value;
                        // tblcsv.Rows[tblcsv.Rows.Count - 1][count] = FileRec;
                        count++;
                    }
                }
            }

            int totalRows = tblcsv.Rows.Count;
            int totalColumns = tblcsv.Columns.Count;

            if (tblcsv.Rows[0][totalColumns - 1].ToString() == "Id\r")
            {
                fileDetailContract.FileFromID = Convert.ToInt64(tblcsv.Rows[1][totalColumns - 1].ToString());
                fileDetailContract.FileToID = Convert.ToInt64(tblcsv.Rows[totalRows - 1][totalColumns - 1].ToString());
            }

            // Save File details
            if (!fileDetailContract.IsNullOrEmpty())
            {
                CDRFileDetail _CDRfileDetail = new CDRFileDetail();
                _CDRfileDetail.FileName = fileDetailContract.FileName;
                _CDRfileDetail.FilePath = fileDetailContract.FilePath;
                _CDRfileDetail.FileFromID = fileDetailContract.FileFromID;
                _CDRfileDetail.FileToID = fileDetailContract.FileToID;
                _CDRfileDetail.FileCreatedDate = fileDetailContract.FileCreatedDate;
                base.LocationDBContext.CDRFileDetails.AddObject(_CDRfileDetail);
                if (base.LocationDBContext.SaveChanges() > 0)
                {
                    fileDetailContract.FileId = _CDRfileDetail.FileID;
                }
            }


            DataTable dtToComapareHeaders = AddColumnsToDataTable();
            dtToComapareHeaders.Rows.Add(new object[] { "Location", "Date","Time","Tn Originating", "Direction", "Originating City", "Originating State", "Tn Dialed"
             , "Destination City","Destination State","Internal","Call Type","Duration Minutes","Charge","Account Code","Answered By Target","Id\r"});

            var compareResult = tblcsv.AsEnumerable().Intersect(dtToComapareHeaders.AsEnumerable(),
                                               DataRowComparer.Default);

            if (compareResult.Count() > 0)
            {
                DataRow dr = tblcsv.Rows[AppConsts.NONE];
                dr.Delete();
                DataTable finalDt = new DataTable();
                finalDt.Columns.Add("CED_Location", typeof(string));
                finalDt.Columns.Add("CED_Date", typeof(DateTime));
                finalDt.Columns.Add("CED_Time", typeof(string));
                finalDt.Columns.Add("CED_TnOriginating", typeof(string));
                finalDt.Columns.Add("CED_Direction", typeof(string));
                finalDt.Columns.Add("CED_OriginatingCity", typeof(string));
                finalDt.Columns.Add("CED_OriginatingState", typeof(string));
                finalDt.Columns.Add("CED_TnDialed", typeof(Int64));
                finalDt.Columns.Add("CED_DestinationCity", typeof(string));
                finalDt.Columns.Add("CED_DestinationState", typeof(string));
                finalDt.Columns.Add("CED_Internal", typeof(string));
                finalDt.Columns.Add("CED_CallType", typeof(string));
                finalDt.Columns.Add("CED_DurationMinutes", typeof(decimal));
                finalDt.Columns.Add("CED_Charge", typeof(decimal));
                finalDt.Columns.Add("CED_AccountCode", typeof(string));
                finalDt.Columns.Add("CED_AnsweredByTarget", typeof(string));
                finalDt.Columns.Add("CED_CDRID", typeof(Int64));

                finalDt = tblcsv.Copy();

                DataColumn newColumn = new DataColumn("CED_FileID", typeof(int));
                newColumn.DefaultValue = fileDetailContract.FileId;
                finalDt.Columns.Add(newColumn);
                DataColumn createdBy = new DataColumn("CED_CreatedBy", typeof(int));
                createdBy.DefaultValue = backgroundProcessUserId;
                finalDt.Columns.Add(createdBy);
                DataColumn createdOn = new DataColumn("CED_CreatedOn", typeof(DateTime));
                createdOn.DefaultValue = DateTime.Now;
                finalDt.Columns.Add(createdOn);
                DataColumn isDeleted = new DataColumn("CED_IsDeleted", typeof(bool));
                isDeleted.DefaultValue = false;
                finalDt.Columns.Add(isDeleted);

                EntityConnection connection = base.LocationDBContext.Connection as EntityConnection;

                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        sqlBulkCopy.DestinationTableName = "[ams].[CDRExportData]";
                        sqlBulkCopy.ColumnMappings.Add("CED_Location", "CED_Location");
                        sqlBulkCopy.ColumnMappings.Add("CED_Date", "CED_Date");
                        sqlBulkCopy.ColumnMappings.Add("CED_Time", "CED_Time");
                        sqlBulkCopy.ColumnMappings.Add("CED_TnOriginating", "CED_TnOriginating");
                        sqlBulkCopy.ColumnMappings.Add("CED_Direction", "CED_Direction");
                        sqlBulkCopy.ColumnMappings.Add("CED_OriginatingCity", "CED_OriginatingCity");
                        sqlBulkCopy.ColumnMappings.Add("CED_OriginatingState", "CED_OriginatingState");
                        sqlBulkCopy.ColumnMappings.Add("CED_TnDialed", "CED_TnDialed");
                        sqlBulkCopy.ColumnMappings.Add("CED_DestinationCity", "CED_DestinationCity");
                        sqlBulkCopy.ColumnMappings.Add("CED_DestinationState", "CED_DestinationState");
                        sqlBulkCopy.ColumnMappings.Add("CED_Internal", "CED_Internal");
                        sqlBulkCopy.ColumnMappings.Add("CED_CallType", "CED_CallType");
                        sqlBulkCopy.ColumnMappings.Add("CED_DurationMinutes", "CED_DurationMinutes");
                        sqlBulkCopy.ColumnMappings.Add("CED_Charge", "CED_Charge");
                        sqlBulkCopy.ColumnMappings.Add("CED_AccountCode", "CED_AccountCode");
                        sqlBulkCopy.ColumnMappings.Add("CED_AnsweredByTarget", "CED_AnsweredByTarget");
                        sqlBulkCopy.ColumnMappings.Add("CED_CDRID", "CED_CDRID");
                        sqlBulkCopy.ColumnMappings.Add("CED_FileID", "CED_FileID");
                        sqlBulkCopy.ColumnMappings.Add("CED_CreatedBy", "CED_CreatedBy");
                        sqlBulkCopy.ColumnMappings.Add("CED_CreatedOn", "CED_CreatedOn");
                        sqlBulkCopy.ColumnMappings.Add("CED_IsDeleted", "CED_IsDeleted");

                        //inserting Datatable Records to DataBase             
                        sqlBulkCopy.WriteToServer(finalDt);
                        con.Close();
                    }
                }
            }

        }

        DataTable AddColumnsToDataTable()
        {
            DataTable dtForCDR = new DataTable();
            dtForCDR.Columns.Add("CED_Location", typeof(string));
            dtForCDR.Columns.Add("CED_Date", typeof(string));
            dtForCDR.Columns.Add("CED_Time", typeof(string));
            dtForCDR.Columns.Add("CED_TnOriginating", typeof(string));
            dtForCDR.Columns.Add("CED_Direction", typeof(string));
            dtForCDR.Columns.Add("CED_OriginatingCity", typeof(string));
            dtForCDR.Columns.Add("CED_OriginatingState", typeof(string));
            dtForCDR.Columns.Add("CED_TnDialed", typeof(string));
            dtForCDR.Columns.Add("CED_DestinationCity", typeof(string));
            dtForCDR.Columns.Add("CED_DestinationState", typeof(string));
            dtForCDR.Columns.Add("CED_Internal", typeof(string));
            dtForCDR.Columns.Add("CED_CallType", typeof(string));
            dtForCDR.Columns.Add("CED_DurationMinutes", typeof(string));
            dtForCDR.Columns.Add("CED_Charge", typeof(string));
            dtForCDR.Columns.Add("CED_AccountCode", typeof(string));
            dtForCDR.Columns.Add("CED_AnsweredByTarget", typeof(string));
            dtForCDR.Columns.Add("CED_CDRID", typeof(string));
            return dtForCDR;
        }

        Int64 IFingerPrintClientRepository.GetLastRecordInsertedId()
        {

            var result = base.LocationDBContext.CDRExportDatas.Where(cond => !Convert.ToBoolean(cond.CED_IsDeleted)).OrderByDescending(cond => cond.CED_CDRID).Select(cond => cond.CED_CDRID).FirstOrDefault();
            return Convert.ToInt64(result);
        }
        #endregion

        List<FileResultStatusUpdateContract> IFingerPrintClientRepository.GetAllUpdatedFileResults(Int32 ChunkSize, Int32? ApplicantAppointmentId)
        {
            List<FileResultStatusUpdateContract> fileResultStatusUpdateContract = new List<FileResultStatusUpdateContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@ChunkSize",ChunkSize),
                    //new SqlParameter("@ApplicantAppointmentId",ApplicantAppointmentId)
                };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetFingerPrintFileResult", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            FileResultStatusUpdateContract fileResultStatusdata = new FileResultStatusUpdateContract();
                            fileResultStatusdata.ApplicantName = Convert.ToString(dr["ApplicantName"]);
                            fileResultStatusdata.AppointmentDetailID = dr["AppointmentDetailID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AppointmentDetailID"]);
                            fileResultStatusdata.OrderNumber = Convert.ToString(dr["OrderNumber"]);
                            fileResultStatusdata.PCNNumber = Convert.ToString(dr["PCNNumber"]);
                            fileResultStatusdata.Result = Convert.ToString(dr["Result"]);
                            fileResultStatusdata.UserId = dr["UserId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["UserId"]);
                            fileResultStatusdata.CBIFileID = dr["CBIFileID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CBIFileID"]);
                            fileResultStatusdata.UserEmailId = Convert.ToString(dr["UserEmailId"]);
                            fileResultStatusdata.HierarchyNodeId = dr["HierarchyNodeId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["HierarchyNodeId"]);
                            fileResultStatusdata.IsOutofStateOrder = dr["IsOutofStateOrder"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsOutofStateOrder"]);
                            fileResultStatusdata.StateFees = dr["StateFees"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["StateFees"]);
                            fileResultStatusdata.FBIFees = dr["FBIFees"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["FBIFees"]);
                            //fileResultStatusdata.Extention = Convert.ToString(dr["Extention"]);
                            fileResultStatusdata.AppointmentStatus = Convert.ToString(dr["AppointmentStatus"]);
                            fileResultStatusdata.OrderID = dr["OrderID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["OrderID"]);
                            fileResultStatusdata.finalresult = Convert.ToString(dr["finalresult"]);
                            fileResultStatusdata.IsDataError = dr["IsDataError"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsDataError"]);
                            //fileResultStatusdata.isFbiErrSubmit = dr["isFbiErrSubmit"] == DBNull.Value ? false : Convert.ToBoolean(dr["isFbiErrSubmit"]);
                            //fileResultStatusdata.isFbisuccess = dr["isFbisuccess"] == DBNull.Value ? false : Convert.ToBoolean(dr["isFbisuccess"]);
                            fileResultStatusUpdateContract.Add(fileResultStatusdata);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return fileResultStatusUpdateContract;
        }

        Int32 IFingerPrintClientRepository.GetOrderIdByOrderNumber(string orderNumber)
        {
            return _dbContext.Orders.Where(o => o.OrderNumber == orderNumber).Select(o => o.OrderID).FirstOrDefault();
        }

        List<LocationContract> IFingerPrintClientRepository.GetLocationForRescheduling(Int32 orderId, string lng, string lat)
        {
            List<LocationContract> lstFingerprintLocContract = new List<LocationContract>();

            ///@todo: Need to Implement pagination


            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                List<SqlParameter> sqlParameterCollection = new List<SqlParameter>
                        {
                            new SqlParameter("@MasterOrderID",orderId),
                            new SqlParameter("@lng",lng),
                            new SqlParameter("@lat",lat)

                        };


                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetLocationsForRescheduling", sqlParameterCollection.ToArray()))
                {
                    int intParseResult;
                    if (dr.HasRows)
                    {
                        Guid guidParseResult = Guid.Empty;
                        while (dr.Read())
                        {
                            LocationContract locationContract = new LocationContract();
                            locationContract.LocationID = Convert.ToInt32(dr["FPL_ID"]);
                            locationContract.LocationName = dr["FPL_Name"].ToString();
                            locationContract.FullAddress = dr["FPL_FullAddress"].ToString();
                            locationContract.PlaceID = dr["FPL_PlaceID"].ToString();
                            locationContract.Longitude = dr["FPL_Longitude"].ToString();
                            locationContract.Latitude = dr["FPL_Latitude"].ToString();
                            locationContract.Zipcode = dr["FPL_ZipCode"].ToString();
                            locationContract.Description = dr["FPL_Description"].ToString();
                            locationContract.IsPrinterAvailable = Convert.ToBoolean(dr["FPL_IsPrinterAvailable"]);
                            locationContract.IsPhotoService = Convert.ToBoolean(dr["FPL_IsPassPhotoService"]);


                            if (int.TryParse(dr["FPL_ScheduleMasterID"].ToString(), out intParseResult))
                            {
                                locationContract.ScheduleMasterID = intParseResult;
                            }

                            lstFingerprintLocContract.Add(locationContract);
                        }
                    }
                }
            }

            return lstFingerprintLocContract;
        }

        List<LocationContract> IFingerPrintClientRepository.GetApplicantAvailableLocation(Int32 TenantId, string lng, string lat, String orderRequestType, Int32 orderId)
        {
            List<LocationContract> lstFingerprintLocContract = new List<LocationContract>();

            ///@todo: Need to Implement pagination

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                List<SqlParameter> sqlParameterCollection = new List<SqlParameter>
                        {
                            new SqlParameter("@MasterOrderID",orderId),
                            new SqlParameter("@lng",lng),
                            new SqlParameter("@lat",lat)

                        };


                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetLocationsForRescheduling", sqlParameterCollection.ToArray()))
                {
                    int intParseResult;
                    if (dr.HasRows)
                    {
                        Guid guidParseResult = Guid.Empty;
                        while (dr.Read())
                        {
                            LocationContract locationContract = new LocationContract();
                            locationContract.LocationID = Convert.ToInt32(dr["FPL_ID"]);
                            locationContract.LocationName = dr["FPL_Name"].ToString();
                            locationContract.FullAddress = dr["FPL_FullAddress"].ToString();
                            locationContract.PlaceID = dr["FPL_PlaceID"].ToString();
                            locationContract.Longitude = dr["FPL_Longitude"].ToString();
                            locationContract.Latitude = dr["FPL_Latitude"].ToString();
                            locationContract.Zipcode = dr["FPL_ZipCode"].ToString();
                            locationContract.Description = dr["FPL_Description"].ToString();
                            locationContract.IsPrinterAvailable = Convert.ToBoolean(dr["FPL_IsPrinterAvailable"]);
                            locationContract.IsPhotoService = Convert.ToBoolean(dr["FPL_IsPassPhotoService"]);


                            if (int.TryParse(dr["FPL_ScheduleMasterID"].ToString(), out intParseResult))
                            {
                                locationContract.ScheduleMasterID = intParseResult;
                            }

                            lstFingerprintLocContract.Add(locationContract);
                        }
                    }
                }
            }
            return lstFingerprintLocContract;
        }

        List<BackgroundServiceContract> IFingerPrintClientRepository.GetOrderBackgroundServices(int OrderId)
        {
            List<BackgroundServiceContract> lstBackgroundServiceContract = new List<BackgroundServiceContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                List<SqlParameter> sqlParameterCollection = new List<SqlParameter>
                        {
                            new SqlParameter("@MasterOrderID",OrderId),
                        };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetOrderBackgroundServicesByOrderID", sqlParameterCollection.ToArray()))
                {

                    if (dr.HasRows)
                    {
                        Guid guidParseResult = Guid.Empty;
                        while (dr.Read())
                        {
                            BackgroundServiceContract backgroundServiceContract = new BackgroundServiceContract();
                            backgroundServiceContract.BSE_ID = Convert.ToInt32(dr["BSE_ID"]);
                            backgroundServiceContract.BSE_Name = dr["BSE_Name"].ToString();
                            backgroundServiceContract.BST_Code = dr["BST_Code"].ToString();
                            backgroundServiceContract.BSE_Description = dr["BSE_Description"].ToString();
                            backgroundServiceContract.BSE_SvcTypeID = Convert.ToInt32(dr["BSE_SvcTypeID"]);


                            lstBackgroundServiceContract.Add(backgroundServiceContract);
                        }
                    }
                }
            }

            return lstBackgroundServiceContract;
        }


        Boolean IFingerPrintClientRepository.AdditionalServicesNotshipped(int OrderId)
        {
            int cabsTypeStatusId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;
            List<int> applicantAppointmentDetails = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == OrderId
                                                                         && !x.AAD_IsDeleted && x.AAD_IsActive).Select(x => x.AAD_ID).ToList();
            int ShippedId = _dbContext.lkpCABSServiceStatus.Where(x => x.CSS_Code == CABSServiceStatus.PENDING_SHIPMENT.GetDescription()).FirstOrDefault().CSS_ID;
            int NotShippedCount = _dbContext.ApplicantAppointmentDetailsExts.Where(x => applicantAppointmentDetails.Contains(x.AADE_AppAppointmentDetailID)
                                                                        && !x.AADE_IsDeleted && x.AADE_IsActive
                                                                        && x.AADE_ServiceTypeID != cabsTypeStatusId
                                                                        && x.AADE_ServiceStatus != ShippedId
                                                                         ).Count();
            if (NotShippedCount > 0)
                return true;
            else
                return false;
        }

        Boolean IFingerPrintClientRepository.AdditionalServicesExist(int OrderID)
        {
            var cabsServiceDetail = _dbContext.CABSServiceOrderDetails.Where(y => y.CSOD_OrderID == OrderID).Select(y => y.CSOD_ServiceDetails).FirstOrDefault();
            XmlDocument doc = new XmlDocument();
            var document = XDocument.Parse(cabsServiceDetail);

            var HasAdditionalServices = document.Elements("BkgpkgAttributes")
                .Where(x => x.Elements("BkgpkgData")
                             .Where(c => c.Element("ServiceType").Value != "AAAA")
                             .Any())
                .Count();
            return HasAdditionalServices > 0 ? true : false;
        }

        LocationContract IFingerPrintClientRepository.GetLocationByOrderId(int OrderId)
        {
            LocationContract OrderLocation = new LocationContract();
            int LocationId = LocationDBContext.FingerPrintApplicantAppointments.Where(x => x.FPAA_OrderID == OrderId).FirstOrDefault().FPAA_LocationID;
            FingerPrintLocation Location = LocationDBContext.FingerPrintLocations.Where(x => x.FPL_ID == LocationId).FirstOrDefault();
            if (Location != null)
            {
                OrderLocation.LocationID = Location.FPL_ID;
                OrderLocation.LocationName = Location.FPL_Name;
                OrderLocation.IsPrinterAvailable = Location.FPL_IsPrinterAvailable;

            }
            return OrderLocation;
        }

        LocationContract IFingerPrintClientRepository.GetLocationByLocationid(int LocationId)
        {
            LocationContract OrderLocation = new LocationContract();
            FingerPrintLocation Location = LocationDBContext.FingerPrintLocations.Where(x => x.FPL_ID == LocationId).FirstOrDefault();
            if (Location != null)
            {
                OrderLocation.LocationID = Location.FPL_ID;
                OrderLocation.LocationName = Location.FPL_Name;
                OrderLocation.IsPrinterAvailable = Location.FPL_IsPrinterAvailable;

            }
            return OrderLocation;
        }



        void IFingerPrintClientRepository.UpdateStatusForFileResult(FileResultStatusUpdateContract fileResult, Int32 CurrentLoggedInUserId, Boolean IsSubmittedToCBI, Boolean IsContactAgency)
        {
            SqlConnection conn;
            SqlCommand comm;

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (conn = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                comm = new SqlCommand("ams.usp_CBIFileResultStatusChangeAudit", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@ApplicantDetailId", fileResult.AppointmentDetailID);
                comm.Parameters.AddWithValue("@Result", fileResult.Result);
                comm.Parameters.AddWithValue("@CBIFileID", fileResult.CBIFileID);
                comm.Parameters.AddWithValue("@CurrentLoggedInUserID", CurrentLoggedInUserId);
                comm.Parameters.AddWithValue("@ISSubmittedToCBI", IsSubmittedToCBI);
                comm.Parameters.AddWithValue("@FinalResult", fileResult.finalresult);
                comm.Parameters.AddWithValue("@IsDataError", fileResult.IsDataError);
                comm.Parameters.AddWithValue("@IsContactAgency", IsContactAgency);
                //comm.Parameters.AddWithValue("@isFbiErrSubmit", fileResult.isFbiErrSubmit);
                //comm.Parameters.AddWithValue("@IsStateFBIApplicable", fileResult.IsStateFBIApplicable);
                //comm.Parameters.AddWithValue("@isCbiSuccess", fileResult.isCbiSuccess);
                //comm.Parameters.AddWithValue("@isFbisuccess", fileResult.isFbisuccess);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                comm.ExecuteNonQuery();
                conn.Close();
            }
        }
        List<FingerPrintRecieptContract> IFingerPrintClientRepository.GetUserRicieptFileData(Int32 ChunkSize)
        {
            List<FingerPrintRecieptContract> fingerPrintRecieptContract = new List<FingerPrintRecieptContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@ChunkSize",ChunkSize)
                };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetFingerPrintSendRecieptData", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            FingerPrintRecieptContract fingerprintRecieptdata = new FingerPrintRecieptContract();
                            fingerprintRecieptdata.ApplicantName = Convert.ToString(dr["ApplicantName"]);
                            fingerprintRecieptdata.OrderNumber = Convert.ToString(dr["OrderNumber"]);
                            fingerprintRecieptdata.PCNNumber = Convert.ToString(dr["PCNNumber"]);
                            fingerprintRecieptdata.UserEmail = Convert.ToString(dr["UserEmail"]);
                            fingerprintRecieptdata.UserId = dr["UserId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["UserId"]);
                            fingerprintRecieptdata.RecieptId = dr["RecieptId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RecieptId"]);
                            fingerprintRecieptdata.AppointmentId = dr["AppointmentId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AppointmentId"]);
                            fingerprintRecieptdata.HierarchyNodeID = dr["HierarchyNodeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["HierarchyNodeID"]);
                            fingerprintRecieptdata.DocFileName = Convert.ToString(dr["DocFileName"]);
                            fingerprintRecieptdata.DocPath = Convert.ToString(dr["DocPath"]);
                            fingerprintRecieptdata.DocSize = dr["DocSize"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DocSize"]);
                            fingerprintRecieptdata.DocumentID = dr["DocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DocumentID"]);
                            fingerprintRecieptdata.DocTypeId = dr["DocTypeId"] == DBNull.Value ? (Int16?)0 : Convert.ToInt16(dr["DocTypeId"]);
                            fingerPrintRecieptContract.Add(fingerprintRecieptdata);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return fingerPrintRecieptContract;
        }
        Boolean IFingerPrintClientRepository.UpdateFileRecieptDispatched(Int32 ApplicantAppointmentId, Int32 UserId)
        {
            var ApplicantAppointmentDataExt = _dbContext.ApplicantAppointmentDetailsExts.Where(cond => cond.AADE_IsActive && !cond.AADE_IsDeleted &&
              cond.ApplicantAppointmentDetail.AAD_ID == ApplicantAppointmentId
              && cond.ApplicantAppointmentDetail.AAD_IsActive && !cond.ApplicantAppointmentDetail.AAD_IsDeleted).FirstOrDefault();

            //var ApplicantAppointmentData = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_ID == ApplicantAppointmentId
            //&& x.AAD_IsActive && !x.AAD_IsDeleted).FirstOrDefault();

            if (ApplicantAppointmentDataExt.IsNotNull())
            {
                ApplicantAppointmentDataExt.AADE_IsReceiptDispatched = true;
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;

        }

        #region Language Translation
        public List<LanguageTranslateContract> GetLanguageTranslationData(CustomPagingArgsContract GridCustomPagingArgs, int organizationUserId, LanguageTranslateContract filterContract)
        {

            List<LanguageTranslateContract> languageTranslateContractLst = new List<LanguageTranslateContract>();

            String orderBy = "EntityTypeId";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(GridCustomPagingArgs.SortExpression) ? orderBy : GridCustomPagingArgs.SortExpression;
            ordDirection = !GridCustomPagingArgs.SortDirectionDescending && !GridCustomPagingArgs.SortExpression.IsNullOrEmpty() ? "asc" : "desc";

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetLanguageTranslationBkgSvcAttributes", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderBy", orderBy.IsNullOrEmpty() ? null : orderBy);
                command.Parameters.AddWithValue("@OrderDirection", ordDirection.IsNullOrEmpty() ? null : ordDirection);
                command.Parameters.AddWithValue("@PageSize", GridCustomPagingArgs.PageSize);
                command.Parameters.AddWithValue("@PageIndex", GridCustomPagingArgs.CurrentPageIndex);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    LanguageTranslateContract languageTranslateContract = new LanguageTranslateContract();
                    languageTranslateContract.SystemSpecificLanguageTextId = dr["SystemSpecificLanguageTextId"] != DBNull.Value ? Convert.ToInt32(dr["SystemSpecificLanguageTextId"]) : 0;
                    languageTranslateContract.EntityTypeId = dr["EntityTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EntityTypeId"]) : 0;
                    languageTranslateContract.EnglishText = Convert.ToString(dr["EnglishText"]);
                    languageTranslateContract.SpanishText = Convert.ToString(dr["SpanishText"]);
                    languageTranslateContract.EntityId = dr["EntityId"] != DBNull.Value ? Convert.ToInt32(dr["EntityId"]) : 0;
                    languageTranslateContract.TotalCount = dr["TotalCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TotalCount"]);
                    languageTranslateContractLst.Add(languageTranslateContract);
                }
                con.Close();
            }
            return languageTranslateContractLst;
        }

        Boolean IFingerPrintClientRepository.SaveLanguageTranslateDetails(LanguageTranslateContract languageTranslateContract, Int32 CurrentUserId)
        {
            if (!languageTranslateContract.IsNullOrEmpty() && languageTranslateContract.SystemSpecificLanguageTextId > 0)
            {
                var systemSpecificLanguageTextdtls = _dbContext.SystemSpecificLanguageTexts.Where(x => x.SELT_EntityId == languageTranslateContract.EntityId && x.SELT_EntityTypeId == languageTranslateContract.EntityTypeId
                                                                                     && !x.SELT_IsDeleted && x.SELT_ID == languageTranslateContract.SystemSpecificLanguageTextId).FirstOrDefault();

                systemSpecificLanguageTextdtls.SELT_TranslationText = languageTranslateContract.SpanishText;
                systemSpecificLanguageTextdtls.SELT_ModifiedByID = CurrentUserId;
                systemSpecificLanguageTextdtls.SELT_ModifiedOn = DateTime.Now;
                if (_dbContext.SaveChanges() > AppConsts.NONE)
                    return true;
            }
            else
            {
                SystemSpecificLanguageText systemSpecificLanguage = new SystemSpecificLanguageText();
                systemSpecificLanguage.SELT_TranslationText = languageTranslateContract.SpanishText;
                systemSpecificLanguage.SELT_EntityId = languageTranslateContract.EntityId;
                systemSpecificLanguage.SELT_EntityTypeId = languageTranslateContract.EntityTypeId;
                systemSpecificLanguage.SELT_CreatedByID = CurrentUserId;
                systemSpecificLanguage.SELT_CreatedOn = DateTime.Now;
                systemSpecificLanguage.SELT_IsDeleted = false;
                _dbContext.SystemSpecificLanguageTexts.AddObject(systemSpecificLanguage);
                if (_dbContext.SaveChanges() > AppConsts.NONE)
                    return true;
            }
            return false;
        }

        List<SystemSpecificLanguageText> IFingerPrintClientRepository.GetSystemSpecificTranslatedText(int entityTypeID)
        {
            return _dbContext.SystemSpecificLanguageTexts.Where(col => col.SELT_EntityTypeId == entityTypeID && col.SELT_IsDeleted == false).ToList();
        }

        #endregion

        Boolean IFingerPrintClientRepository.IsFileSentToCbi(Int32 OrderId)
        {
            var appointment = _dbContext.ApplicantAppointmentDetailsExts.Where(cond => cond.AADE_IsActive && !cond.AADE_IsDeleted
              && cond.AADE_SubmittedToCBI == true && cond.ApplicantAppointmentDetail.AAD_OrderID == OrderId).FirstOrDefault();
            //  var appointment = _dbContext.ApplicantAppointmentDetails.Where(t => t.AAD_OrderID == OrderId && t.AAD_SubmittedToCBI == true).FirstOrDefault();
            if (!appointment.IsNullOrEmpty())
                return true;
            return false;
        }

        #region Mobile Web API Methods

        CustomFormDataContract IFingerPrintClientRepository.GetCustomAttributes(Int32 packageID, String cBIUniqueID, String langCode)
        {
            CustomFormDataContract customattributes = new CustomFormDataContract();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[ams].[usp_GetCustomAttributesDataByCBIUniqueID]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageID", packageID);
                command.Parameters.AddWithValue("@CBIUniqueID", cBIUniqueID);
                command.Parameters.AddWithValue("@LanguageCode", langCode);

                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > AppConsts.NONE)
                    {
                        ds.Tables[0].AsEnumerable().ForEach(x =>
                        {
                            customattributes.customFormId = x["CustomFormID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["CustomFormID"]);
                            customattributes.CustomFormName = x["CustomFormName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(x["CustomFormName"]);
                            customattributes.instanceId = 1;
                        });
                    }
                    if (ds.Tables[0].Rows.Count > AppConsts.NONE)
                    {
                        if (customattributes.lstCustomFormAttributes.IsNullOrEmpty())
                        {
                            customattributes.lstCustomFormAttributes = new List<AttributesForCustomFormContract>();
                        }
                        customattributes.lstCustomFormAttributes = SetAttributesForCustomForm(ds.Tables[1]);

                    }
                }
                con.Close();
            }

            return customattributes;
        }


        public static List<AttributesForCustomFormContract> SetAttributesForCustomForm(DataTable table)
        {

            IEnumerable<DataRow> rows = table.AsEnumerable();
            return rows.Select(x => new AttributesForCustomFormContract
            {
                PackageID = Convert.ToInt32(Convert.ToString(x["PackageID"])),
                AtrributeGroupMappingId = Convert.ToInt32(Convert.ToString(x["AtrributeGroupMappingId"])),
                AttributeGroupId = Convert.ToInt32(Convert.ToString(x["AttributeGroupId"])),
                AttriButeGroupName = Convert.ToString(x["AttriButeGroupName"]),
                AttributeId = Convert.ToInt32(Convert.ToString(x["AttributeId"])),
                AttributeName = Convert.ToString(x["AttributeName"]),
                AttributeType = Convert.ToString(x["AttributeType"]),
                IsDisplay = Convert.ToString(x["IsDisplay"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(x["IsDisplay"])),
                IsHiddenFromUI = Convert.ToString(x["IsHiddenFromUI"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(x["IsHiddenFromUI"])),
                IsRequired = Convert.ToString(x["IsRequired"]).IsNullOrEmpty() ? false : Convert.ToBoolean(Convert.ToString(x["IsRequired"])),
                CustomFieldsDisplaySequence = Convert.ToInt32(Convert.ToString(x["CustomFieldsDisplaySequence"])),
                SectionTitle = Convert.ToString(x["SectionTitle"]),
                Sequence = Convert.ToInt32(Convert.ToString(x["Sequence"])),
                Occurence = Convert.ToInt32(Convert.ToString(x["Occurence"])),
                DisplayColumns = Convert.ToInt32(Convert.ToString(x["DisplayColumn"])),
                MinimumOccurence = Convert.ToString(x["minimumoccurence"]).IsNullOrEmpty() ? 1 : Convert.ToInt32(Convert.ToString(x["minimumoccurence"])),
                MaximumOccurence = Convert.ToString(x["MAXOccurence"]).IsNullOrEmpty() ? 10 : Convert.ToInt32(Convert.ToString(x["MAXOccurence"])),
                CustomHtml = Convert.ToString(x["CustomHtml"]),
                AttributeTypeCode = Convert.ToString(x["AttributeTypeCode"]),
                AttributeGroupMappingCode = Convert.ToString(x["BAGM_Code"]),
                MinimumValue = Convert.ToString(x["MinimumValue"]),
                MaximumValue = Convert.ToString(x["MaximumValue"]),
                InstructionText = Convert.ToString(x["InstructionText"]),
                IsDecisionField = Convert.ToBoolean(x["IsDecisionField"]),
                AttributeCode = Convert.ToString(x["AttributeCode"]),
                ParentAttributeGroupMappingId = Convert.ToString(x["ParentAttributeGroupMappingId"]).IsNullOrEmpty() ? 0 : Convert.ToInt32(x["ParentAttributeGroupMappingId"]),
                ValidateExpression = Convert.ToString(x["ValidateExpression"]).IsNullOrEmpty() ? String.Empty : Convert.ToString(x["ValidateExpression"]),
                ValidationMessage = Convert.ToString(x["ValidationMessage"]).IsNullOrEmpty() ? String.Empty : Convert.ToString(x["ValidationMessage"]),
                Name = Convert.ToString(x["Name"]),
                AttributeDataValue = Convert.ToString(x["AttributeValue"]),
                BkgSvcAttributeGroupCode = Convert.ToString(x["BkgSvcAttributeGroupCode"]).IsNull() ? String.Empty : Convert.ToString(x["BkgSvcAttributeGroupCode"]),
                ServiceTypeCode = Convert.ToString(x["ServiceTypeCode"]).IsNull() ? String.Empty : Convert.ToString(x["ServiceTypeCode"])
            }).OrderBy(o => o.Sequence).ToList();
        }


        #endregion

        #region UAT-3850

        CBIBillingStatu IFingerPrintClientRepository.GetCBIBillingStatusData(String cbiUniqueId, String billingCode)
        {
            if (!cbiUniqueId.IsNullOrEmpty() && !billingCode.IsNullOrEmpty())
                return _dbContext.CBIBillingStatus.Where(cond => !cond.CBS_IsDeleted && cond.CBS_IsEnabled && cond.CBS_CBIUniqueId == cbiUniqueId && cond.CBS_BillingCode == billingCode).FirstOrDefault();
            return new CBIBillingStatu();
        }

        OrderBillingCodeMapping IFingerPrintClientRepository.GetOrderBillingCode(Int32 orderId)
        {
            OrderBillingCodeMapping orderBillingCodeMapping = new OrderBillingCodeMapping();
            orderBillingCodeMapping = _dbContext.OrderBillingCodeMappings.Where(cond => !cond.OBCM_IsDeleted && cond.OBCM_OrderId == orderId).FirstOrDefault();
            return orderBillingCodeMapping;
        }

        #endregion


        String IFingerPrintClientRepository.GetUserProfileName(Int32 OrderId)
        {
            var order = _dbContext.Orders.Where(t => t.OrderID == OrderId && !t.IsDeleted).FirstOrDefault();
            if (!order.IsNullOrEmpty())
            {
                var userName = order.OrganizationUserProfile.IsNullOrEmpty() ? string.Empty : (order.OrganizationUserProfile.FirstName + " " + order.OrganizationUserProfile.LastName);
                return userName;
            }
            return string.Empty;
        }

        #region UAT -4088
        public Boolean RejectOutOfStateOrderByCBI(Int32 ApplicantAppointmentId, Int32 CurrentLoggedInUserId)
        {
            if (ApplicantAppointmentId > AppConsts.NONE)
            {
                EntityConnection connection = _dbContext.Connection as EntityConnection;

                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("ams.usp_ManualRejectOutOfStateOrder", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FingerPrinApplicantAppointmentId", ApplicantAppointmentId);
                    //command.Parameters.AddWithValue("@TenantId", TenantId);
                    command.Parameters.AddWithValue("@CurrentLoggedInUserId", CurrentLoggedInUserId);

                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    command.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            return false;
        }


        List<FileResultStatusUpdateContract> IFingerPrintClientRepository.GetAllManuallyRejectedOrders(Int32 FingerprintApplicantAppointmentId)
        {
            List<FileResultStatusUpdateContract> fileResultStatusUpdateContract = new List<FileResultStatusUpdateContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AppointmentID",FingerprintApplicantAppointmentId)
                };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_GetAllManuallyRejectedOrders", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            FileResultStatusUpdateContract fileResultStatusdata = new FileResultStatusUpdateContract();
                            fileResultStatusdata.AppointmentDetailID = dr["AppointmentDetailID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AppointmentDetailID"]);
                            fileResultStatusdata.ApplicantName = Convert.ToString(dr["ApplicantName"]);
                            fileResultStatusdata.UserId = dr["UserId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["UserId"]);
                            fileResultStatusdata.UserEmailId = Convert.ToString(dr["UserEmailId"]);
                            fileResultStatusdata.OrderNumber = Convert.ToString(dr["OrderNumber"]);
                            fileResultStatusdata.HierarchyNodeId = dr["HierarchyNodeId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["HierarchyNodeId"]);
                            fileResultStatusdata.IsOutofStateOrder = dr["IsOutofStateOrder"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsOutofStateOrder"]);
                            fileResultStatusdata.OrderID = dr["OrderID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["OrderID"]);
                            fileResultStatusUpdateContract.Add(fileResultStatusdata);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return fileResultStatusUpdateContract;
        }
        #endregion

        #region UAT-4270
        Boolean IFingerPrintClientRepository.SaveManualFingerPrintFile(ApplicantDocument appDocument, int FingerprintAppointmentId, Int32 CurrentLoggedinUserId, Boolean IsAbiReviewUpload)
        {
            Boolean result = false;
            _dbContext.ApplicantDocuments.AddObject(appDocument);
            String inProgressStatusCode = String.Empty;
            if (IsAbiReviewUpload)
            {
                inProgressStatusCode = FingerPrintAppointmentStatus.ACTIVE.GetStringValue();
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
            {
                EntityConnection connection = _dbContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlParameter[] sqlParameterCollection = new SqlParameter[]
                    {
                    new SqlParameter("@documentId",appDocument.ApplicantDocumentID),
                    new SqlParameter("@FingerprintApplicantAppointmentId",FingerprintAppointmentId.ToString()),
                    new SqlParameter("@currentLoggedInUserId",CurrentLoggedinUserId),
                    new SqlParameter("@latestStatusCode",inProgressStatusCode)
                    };
                    base.OpenSQLDataReaderConnection(con);

                    using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_SaveManualFingerPrintFileData", sqlParameterCollection))
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                result = dr["result"] == DBNull.Value ? false : Convert.ToBoolean(dr["result"]);
                            }
                        }
                    }
                    base.CloseSQLDataReaderConnection(con);
                }
            }
            return result;
        }
        #endregion
        #region ABI Review
        public Boolean UpdateAppointmentStatus(String appointmentStatusCode, Int32 fingerPrintApplicantAppointmentID, Int32 CurrentLoggedinUserId)
        {
            if (fingerPrintApplicantAppointmentID > AppConsts.NONE)
            {
                List<lkpAppointmentStatu> AppointmentStatusList = LocationDBContext.lkpAppointmentStatus.ToList();
                FingerPrintApplicantAppointment fingerPrintApplicantAppointment = LocationDBContext.FingerPrintApplicantAppointments.FirstOrDefault(x => x.FPAA_ID == fingerPrintApplicantAppointmentID && !x.FPAA_IsDeleted);
                lkpAppointmentStatu lkpAppointmentStatu = AppointmentStatusList.FirstOrDefault(x => x.AS_Code == appointmentStatusCode);
                Int32 AppointmentDetailId = _dbContext.ApplicantAppointmentDetails.FirstOrDefault(x => x.AAD_OrderID == fingerPrintApplicantAppointment.FPAA_OrderID && !x.AAD_IsDeleted && x.AAD_IsActive).AAD_ID;
                var response = UpdateFingerPrintApplicantAppointmentAudit(fingerPrintApplicantAppointment, AppointmentStatusList, lkpAppointmentStatu, CurrentLoggedinUserId, AppointmentDetailId);
                if (_dbContext.SaveChanges() > AppConsts.NONE)
                    return true;
                return response;
            }
            return false;
        }

        private Boolean UpdateFingerPrintApplicantAppointmentAudit(FingerPrintApplicantAppointment fingerPrintApplicantAppointment, List<lkpAppointmentStatu> AppointmentStatusList, lkpAppointmentStatu newStatusData, Int32 CurrentUserId, Int32 applicantAppointmentDetailID)
        {
            String AppointmentStatusChangeTypeCode = LocationAppointmentAuditChangeType.STATUS_CHANGE.GetStringValue();
            lkpAppointmentChangeType changeTypelist = _dbContext.lkpAppointmentChangeTypes.FirstOrDefault(x => x.ACT_Code == AppointmentStatusChangeTypeCode);
            int cabsTypeServiceId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;
            bool IsCabsServiceExist = false;
            int pendingAppointmentStatus = LocationDBContext.lkpAppointmentStatus.Where(x => x.AS_Code == "AAAP").Select(x => x.AS_ID).FirstOrDefault();
            if (applicantAppointmentDetailID > 0)
            {
                IsCabsServiceExist = _dbContext.ApplicantAppointmentDetailsExts.Any(x => x.AADE_AppAppointmentDetailID == applicantAppointmentDetailID && x.AADE_ServiceTypeID == cabsTypeServiceId && x.AADE_ParentAppAppointmentDetailID == null);
            }
            if (!fingerPrintApplicantAppointment.IsNullOrEmpty() && IsCabsServiceExist)
            {
                lkpAppointmentStatu oldStatus = AppointmentStatusList.FirstOrDefault(x => x.AS_ID == fingerPrintApplicantAppointment.FPAA_Status.Value);
                fingerPrintApplicantAppointment.FPAA_Status = newStatusData.AS_ID;
                fingerPrintApplicantAppointment.FPAA_ModifiedBy = CurrentUserId;
                fingerPrintApplicantAppointment.FPAA_ModifiedOn = DateTime.Now;
                if (LocationDBContext.SaveChanges() > AppConsts.NONE)
                {
                    LocationServiceAppointmentAudit locationServiceAppointmentAudit = new LocationServiceAppointmentAudit
                    {
                        LSAA_AppointmentID = applicantAppointmentDetailID,
                        LSAA_ChangeTypeID = changeTypelist.ACT_ID,
                        LSAA_CreatedBy = CurrentUserId,
                        LSAA_CreatedOn = DateTime.Now,
                        LSAA_DateUpdated = DateTime.Now,
                        LSAA_Description = "Status Change > " + oldStatus.AS_Name + " --> " + newStatusData.AS_Name,
                        LSAA_IsDeleted = false,
                        LSAA_IsOnsite = fingerPrintApplicantAppointment.FPAA_IsOnsiteAppointment,
                        LSAA_IsOutOfState = fingerPrintApplicantAppointment.FPAA_IsOutOfState,
                        LSAA_LocationId = fingerPrintApplicantAppointment.FPAA_LocationID,
                        LSAA_NewValue = newStatusData.AS_ID.ToString(),
                        LSAA_OldValue = oldStatus.AS_ID.ToString()
                    };
                    _dbContext.LocationServiceAppointmentAudits.AddObject(locationServiceAppointmentAudit);
                    return true;
                }
            }
            return false;

        }
        #endregion

        public ApplicantFingerPrintFileImageContract GetFingerPrintImageData(Int32 ApplicantAppointmentDetailID)
        {

            return _dbContext.ApplicantFingerPrintFileImages
                .Where(x => x.AFFI_ApplicantAppointmentDetailID == ApplicantAppointmentDetailID
                && !x.AFFI_IsDeleted)
                .OrderByDescending(x => x.AFFI_ID)
                .Select(x => new ApplicantFingerPrintFileImageContract()
                {
                    AFFI_FileName = x.AFFI_FileName,
                    AFFI_FilePath = x.AFFI_DocumentPath,
                    AFFI_ID = x.AFFI_ID,
                    AFFI_ApplicantAppointmentDetailID = x.AFFI_ApplicantAppointmentDetailID
                })
                .FirstOrDefault();
        }

        public Boolean ChangeSendToCBIAppointmentStatus(List<AppointmentOrderScheduleContract> appointmentSchedules, Int32 CurrentLoggedinUserId)
        {
            List<FingerPrintApplicantAppointment> lstfingerPrintApplicantAppointment = new List<FingerPrintApplicantAppointment>();
            List<ApplicantAppointmentDetail> lstapplicantAppointmentDetail = new List<ApplicantAppointmentDetail>();
            List<LocationServiceAppointmentAudit> lstLocationServiceAppointmentAudit = new List<LocationServiceAppointmentAudit>();
            List<lkpAppointmentStatu> lkpAppointmentStatus = new List<lkpAppointmentStatu>();
            lkpAppointmentStatu objNewStatus = new lkpAppointmentStatu();

            lkpAppointmentChangeType changeTypelist = _dbContext.lkpAppointmentChangeTypes.FirstOrDefault(lkp => lkp.ACT_Code == "AAAC");

            if (appointmentSchedules.Any())
            {
                var tenantId = appointmentSchedules.Select(a => Convert.ToInt32(a.TenantID)).FirstOrDefault();
                var orderIds = appointmentSchedules.Select(ord => ord.OrderId).ToList();
                lstfingerPrintApplicantAppointment = LocationDBContext.FingerPrintApplicantAppointments
                    .Where(x => !x.FPAA_IsDeleted
                    && x.FPAA_TenantID == tenantId
                    && orderIds.Any(ord => ord == x.FPAA_OrderID))
                    .ToList();

                lstapplicantAppointmentDetail = _dbContext.ApplicantAppointmentDetails
                    .Where(x => !x.AAD_IsDeleted
                    && x.AAD_IsActive
                    && orderIds.Any(ord => ord == x.AAD_OrderID))
                    .ToList();


                lkpAppointmentStatus = LocationDBContext.lkpAppointmentStatus.Where(x => !x.AS_IsDeleted).ToList();

                objNewStatus = lkpAppointmentStatus.Where(x => x.AS_Code == FingerPrintAppointmentStatus.ACTIVE.GetStringValue()).FirstOrDefault();

                if (lstfingerPrintApplicantAppointment.Any())
                {

                    foreach (FingerPrintApplicantAppointment item in lstfingerPrintApplicantAppointment)
                    {
                        var oldStatusName = lkpAppointmentStatus.FirstOrDefault(s => s.AS_ID == item.FPAA_Status).AS_Name;
                        var oldStatusId = item.FPAA_Status;

                        item.FPAA_Status = objNewStatus.AS_ID;
                        item.FPAA_ModifiedBy = CurrentLoggedinUserId;
                        item.FPAA_ModifiedOn = DateTime.Now;

                        var aadId = lstapplicantAppointmentDetail.Where(aad => aad.AAD_OrderID == item.FPAA_OrderID)
                            .Select(aad => aad.AAD_ID)
                            .FirstOrDefault();

                        if (aadId > 0)
                        {
                            LocationServiceAppointmentAudit locationServiceAppointmentAudit = new LocationServiceAppointmentAudit
                            {
                                LSAA_AppointmentID = aadId,
                                LSAA_ChangeTypeID = changeTypelist.ACT_ID,
                                LSAA_CreatedBy = CurrentLoggedinUserId,
                                LSAA_CreatedOn = DateTime.Now,
                                LSAA_DateUpdated = DateTime.Now,
                                LSAA_Description = "Status Change > " + oldStatusName + " --> " + objNewStatus.AS_Name,
                                LSAA_IsDeleted = false,
                                LSAA_IsOnsite = item.FPAA_IsOnsiteAppointment,
                                LSAA_IsOutOfState = item.FPAA_IsOutOfState,
                                LSAA_LocationId = item.FPAA_LocationID,
                                LSAA_NewValue = objNewStatus.AS_ID.ToString(),
                                LSAA_OldValue = oldStatusId.ToString()
                            };
                            _dbContext.LocationServiceAppointmentAudits.AddObject(locationServiceAppointmentAudit);
                        }
                    }
                }

            }

            return _dbContext.SaveChanges() > AppConsts.NONE && LocationDBContext.SaveChanges() > AppConsts.NONE;
        }

        List<LocationServiceAppointmentAuditContract> IFingerPrintClientRepository.GetOrderHistoryList(Int32 OrderID, bool IsCABSAppointment)
        {
            List<LocationServiceAppointmentAuditContract> appointmentAuditHistoryLst = new List<LocationServiceAppointmentAuditContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("ams.usp_ManageAppointmentOrderHistory", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderId", OrderID);
                command.Parameters.AddWithValue("@IsCABSAppointment", IsCABSAppointment);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    LocationServiceAppointmentAuditContract AppointmentAuditHistory = new LocationServiceAppointmentAuditContract();
                    AppointmentAuditHistory.Description = Convert.ToString(dr["Description"]);
                    AppointmentAuditHistory.UpdatedBy = Convert.ToString(dr["CreatedBy"]);
                    AppointmentAuditHistory.UpdationDate = Convert.ToDateTime(dr["CreatedOn"]);
                    appointmentAuditHistoryLst.Add(AppointmentAuditHistory);
                }
                con.Close();
            }
            return appointmentAuditHistoryLst;
        }

        String IFingerPrintClientRepository.GetPackageNameForCompleteOrder(int orderId, String serviceCode, bool isIdRequired)
        {
            string PackageName = "";
            string PackagrId = "";
            EntityConnection connection = _dbContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("AMS.usp_getPackageNameForCompleteOrder", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrderID", orderId);
                command.Parameters.AddWithValue("@ServiceCode", serviceCode);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    PackageName = Convert.ToString(dr["BPA_Name"]);
                    PackagrId = Convert.ToString(dr["BPA_ID"]);
                }
                con.Close();

            }
            if (isIdRequired)
                return PackagrId;
            return PackageName;
        }
        String IFingerPrintClientRepository.GetShippingLineItemName(String serviceCode)
        {
            string res = "";
            int Code = Int32.Parse(serviceCode);
            res = _dbContext.lkpCabsMailingOptions.Where(a => a.CMO_ID == Code).Select(a => a.CMO_Name).FirstOrDefault();
            return res;
        }
        PreviousAddressContract IFingerPrintClientRepository.GetAddressThroughAddressHandleID(string MailingAddressHandleId)
        {
            Guid MailAddHandleID = Guid.Parse(MailingAddressHandleId);
            PreviousAddressContract mailingAddress = new PreviousAddressContract();
            mailingAddress = _dbContext.Addresses
          .Join(_dbContext.AddressExts,
              ad => ad.AddressID,
              adExt => adExt.AE_AddressID,
              (ad, adExt) => new PreviousAddressContract
              {
                  ID = ad.AddressID,
                  Address1 = ad.Address1,
                  Address2 = ad.Address2,
                  MailingAddressHandleId = ad.AddressHandleID,
                  CountryId = adExt.AE_CountryID,
                  Country = adExt.AE_County,
                  StateName = adExt.AE_StateName,
                  Zipcode = adExt.AE_ZipCode,
                  CityName = adExt.AE_CityName

              }).Where(x => x.MailingAddressHandleId == MailAddHandleID).First();
            return mailingAddress;
        }

        #region OrderService Detail
        /// <summary>
        /// To get the specific detail related to an order on the basis of ordeId and current user's logged user ID..
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="currentLoggedInUserID"></param>
        /// <returns></returns>
        List<OrderDetailContract> IFingerPrintClientRepository.OrderSerivceDetail(Int32 orderID, Int32 currentLoggedInUserID)
        {
            List<OrderDetailContract> orderDetailContract = new List<OrderDetailContract>();


            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@OrderID",orderID)//,
                   // new SqlParameter("@OrgUserID",currentLoggedInUserID)
                };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "ams.usp_FingerprintAppointmentFulFillmentQueueOrderDetail", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            OrderDetailContract orderDetaildata = new OrderDetailContract();
                            orderDetaildata.ServiceName = dr["ServiceName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ServiceName"]);
                            orderDetaildata.ServiceStatus = dr["ServiceStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ServiceStatus"]);
                            orderDetaildata.OrderNumber = dr["OrderNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dr["OrderNumber"]);
                            orderDetaildata.IsMailing = dr["IsMailing"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsMailing"]);
                            orderDetaildata.OrderID = dr["OrderId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["OrderId"]);
                            orderDetaildata.DetailExtID = dr["DetailExtID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DetailExtID"]);
                            orderDetaildata.ServiceCode = dr["ServiceCode"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ServiceCode"]);
                            orderDetaildata.TrackingNumber = dr["TrackingNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dr["TrackingNumber"]);
                            if (dr["Quantity"] != DBNull.Value)
                            {
                                orderDetaildata.Quantity = dr["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(dr["Quantity"]);
                            }
                            if (dr["Price"] != DBNull.Value)
                            {
                                orderDetaildata.Price = dr["Price"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Price"]);
                            }
                            if (dr["Amount"] != DBNull.Value)
                            {
                                orderDetaildata.Amount = dr["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Amount"]);
                            }
                            if (dr["FCAdditionalPrice"] != DBNull.Value)
                            {
                                orderDetaildata.FCAdditionalPrice = dr["FCAdditionalPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["FCAdditionalPrice"]);
                            }
                            if (dr["PPQuantity"] != DBNull.Value)
                            {
                                orderDetaildata.PPQuantity = dr["PPQuantity"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PPQuantity"]);
                            }
                            if (dr["PPAdditionalPrice"] != DBNull.Value)
                            {
                                orderDetaildata.PPAdditionalPrice = dr["PPAdditionalPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["PPAdditionalPrice"]);
                            }
                            orderDetailContract.Add(orderDetaildata);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return orderDetailContract;
        }

        #endregion

        /// <summary>
        /// To Get the CABS service statues list.
        /// </summary>
        /// <returns></returns>
        List<ServiceStatusContract> IFingerPrintClientRepository.GetServiceStatues()
        {
            List<ServiceStatusContract> serviceStatusLst = _dbContext.lkpCABSServiceStatus
               .Select(x => new ServiceStatusContract()
               {
                   StatusID = x.CSS_ID,
                   StatusName = x.CSS_Name,
                   StatusCode = x.CSS_Code,
                   OrderBy = x.CSS_OrderBy
               })
               .OrderBy(x => x.OrderBy)
               .ToList();

            return serviceStatusLst;
        }
        /// <summary>
        /// To Get the CABS service statues list.
        /// </summary>
        /// <returns></returns>
        void IFingerPrintClientRepository.SaveServiceStatus(Int32 detailExtID, Int32 serviceStatusID, bool IsServiceStatusRejected, int CurrentLoggedInUserID)
        {
            int rejectServiceStatus = _dbContext.lkpCABSServiceStatus.Where(a => a.CSS_Code == "AAAD").Select(a => a.CSS_ID).FirstOrDefault();
            int shippedServiceStatus = _dbContext.lkpCABSServiceStatus.Where(a => a.CSS_Code == "AAAB").Select(a => a.CSS_ID).FirstOrDefault();
            lkpAppointmentChangeType changeTypelist = _dbContext.lkpAppointmentChangeTypes.FirstOrDefault(lkp => lkp.ACT_Code == "AAAK");
            string oldStatusName, newStatusName = string.Empty;
            int orderId, locationId;
            if (!detailExtID.IsNullOrEmpty() && !serviceStatusID.IsNullOrEmpty())
            {
                var appDetailExtsRecord = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_ID == detailExtID).FirstOrDefault();
                int? previousStatus = appDetailExtsRecord.AADE_ServiceStatus;
                if (!appDetailExtsRecord.IsNullOrEmpty())
                {
                    appDetailExtsRecord.AADE_ServiceStatus = serviceStatusID;
                    if (serviceStatusID == shippedServiceStatus)
                    {
                        appDetailExtsRecord.AADE_DispatchedDate = DateTime.Now;
                        appDetailExtsRecord.AADE_TrackingNumber = null;
                    }

                    if (appDetailExtsRecord.AADE_ParentAppAppointmentDetailID.IsNotNull() && IsServiceStatusRejected)
                    {
                        var previousAppDetailExtRecord = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == appDetailExtsRecord.AADE_ParentAppAppointmentDetailID && x.AADE_ServiceTypeID == appDetailExtsRecord.AADE_ServiceTypeID).ToList();
                        if (previousAppDetailExtRecord.IsNotNull())
                        {
                            foreach (var items in previousAppDetailExtRecord)
                            {
                                items.AADE_ServiceStatus = rejectServiceStatus;
                                appDetailExtsRecord.AADE_ParentAppAppointmentDetailID = null;
                                appDetailExtsRecord.AADE_TrackingNumber = null;
                                oldStatusName = _dbContext.lkpCABSServiceStatus.Where(x => x.CSS_ID == previousStatus).Select(x => x.CSS_Name).FirstOrDefault();
                                newStatusName = _dbContext.lkpCABSServiceStatus.Where(x => x.CSS_ID == rejectServiceStatus).Select(x => x.CSS_Name).FirstOrDefault();
                                orderId = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_ID == appDetailExtsRecord.AADE_AppAppointmentDetailID).Select(x => x.AAD_OrderID).FirstOrDefault();
                                locationId = LocationDBContext.FingerPrintApplicantAppointments.Where(x => x.FPAA_OrderID == orderId).Select(x => x.FPAA_LocationID).FirstOrDefault();
                                LocationServiceAppointmentAudit locationServiceAppointmentAudit = new LocationServiceAppointmentAudit
                                {
                                    LSAA_AppointmentID = items.AADE_AppAppointmentDetailID,
                                    LSAA_ChangeTypeID = changeTypelist.ACT_ID,
                                    LSAA_CreatedBy = CurrentLoggedInUserID,
                                    LSAA_CreatedOn = DateTime.Now,
                                    LSAA_DateUpdated = DateTime.Now,
                                    LSAA_Description = "Additional Service Status Change > " + oldStatusName + " --> " + newStatusName,
                                    LSAA_IsDeleted = false,
                                    LSAA_IsOnsite = false,
                                    LSAA_IsOutOfState = false,
                                    LSAA_LocationId = locationId,
                                    LSAA_NewValue = rejectServiceStatus.ToString(),
                                    LSAA_OldValue = previousStatus.ToString()
                                };
                                _dbContext.LocationServiceAppointmentAudits.AddObject(locationServiceAppointmentAudit);
                            }
                        }
                    }
                }

                if (appDetailExtsRecord.AADE_AppAppointmentDetailID > 0)
                {
                    string newStatusAddSvc;
                    orderId = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_ID == appDetailExtsRecord.AADE_AppAppointmentDetailID).Select(x => x.AAD_OrderID).FirstOrDefault();
                    locationId = LocationDBContext.FingerPrintApplicantAppointments.Where(x => x.FPAA_OrderID == orderId).Select(x => x.FPAA_LocationID).FirstOrDefault();
                    oldStatusName = newStatusName.IsNullOrEmpty() ? _dbContext.lkpCABSServiceStatus.Where(x => x.CSS_ID == previousStatus).Select(x => x.CSS_Name).FirstOrDefault() : newStatusName;
                    newStatusAddSvc = _dbContext.lkpCABSServiceStatus.Where(x => x.CSS_ID == serviceStatusID).Select(x => x.CSS_Name).FirstOrDefault();
                    LocationServiceAppointmentAudit locationServiceAppointmentAudit = new LocationServiceAppointmentAudit
                    {
                        LSAA_AppointmentID = appDetailExtsRecord.AADE_AppAppointmentDetailID,
                        LSAA_ChangeTypeID = changeTypelist.ACT_ID,
                        LSAA_CreatedBy = CurrentLoggedInUserID,
                        LSAA_CreatedOn = DateTime.Now,
                        LSAA_DateUpdated = DateTime.Now,
                        LSAA_Description = "Additional Service Status Change > " + oldStatusName + " --> " + newStatusAddSvc,
                        LSAA_IsDeleted = false,
                        LSAA_IsOnsite = false,
                        LSAA_IsOutOfState = false,
                        LSAA_LocationId = locationId,
                        LSAA_NewValue = serviceStatusID.ToString(),
                        LSAA_OldValue = previousStatus.ToString()
                    };
                    _dbContext.LocationServiceAppointmentAudits.AddObject(locationServiceAppointmentAudit);
                }
                _dbContext.SaveChanges();
            }
        }





        List<String> IFingerPrintClientRepository.GetServiceStatus(Int32 OrderID, Int32 orgUserID)
        {
            List<Int32> lstStatus = new List<Int32>();
            int cabsTypeServiceId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;
            var status = _dbContext.ApplicantAppointmentDetails.Join(_dbContext.ApplicantAppointmentDetailsExts, r => r.AAD_ID, p => p.AADE_AppAppointmentDetailID,
            (r, p) => new { p.AADE_ServiceStatus, r.AAD_OrderID, r.AAD_IsActive, r.AAD_OrganizationUserID, p.AADE_IsActive, p.AADE_ServiceTypeID }).Where(x => x.AAD_OrderID == OrderID
            && x.AAD_OrganizationUserID == orgUserID && x.AAD_IsActive && x.AADE_IsActive && x.AADE_ServiceTypeID != cabsTypeServiceId).Select(x => x.AADE_ServiceStatus);
            if (status.IsNotNull())
            {
                IEnumerable<String> StatusCode = status.Join(_dbContext.lkpCABSServiceStatus, p => p.Value, r => r.CSS_ID, (p, r) => new { r.CSS_Code, p.Value }).Select(x => x.CSS_Code);
                return StatusCode.ToList();
            }
            return new List<string>();
        }

        /// <summary>
        /// To save the tracking number.  // This need to change tommorrow.
        /// </summary>
        /// <returns></returns>
        void IFingerPrintClientRepository.SaveTrackingNumber(Int32 detailExtID, string trackingnum)
        {
            if (!detailExtID.IsNullOrEmpty() && !trackingnum.IsNullOrEmpty())
            {
                var appDetailExtsRecord = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_ID == detailExtID).FirstOrDefault();
                if (!appDetailExtsRecord.IsNullOrEmpty())
                {
                    appDetailExtsRecord.AADE_TrackingNumber = trackingnum;
                    // appDetailExtsRecord.AADE_DispatchedDate = DateTime.Now;
                    _dbContext.SaveChanges();
                }
            }
        }

        public string GetOrderAppointmentStatus(Int32 OrderID, Int32 orgUserID)
        {
            if (OrderID > 0 && orgUserID > 0)
            {
                int? orderAppointmentStatusId = LocationDBContext.FingerPrintApplicantAppointments.Where(x => x.FPAA_OrderID == OrderID && !x.FPAA_IsDeleted).Select(x => x.FPAA_Status).FirstOrDefault();
                if (orderAppointmentStatusId.IsNotNull() && orderAppointmentStatusId > 0)
                    return LocationDBContext.lkpAppointmentStatus.Where(x => x.AS_ID == orderAppointmentStatusId).Select(x => x.AS_Code).FirstOrDefault();
            }
            return string.Empty;
        }

        public bool GetIsOrderRescheduled(Int32 OrderID, Int32 orgUserID)
        {
            if (OrderID > 0 && orgUserID > 0)
            {
                int orderAppointmentDetailId = _dbContext.ApplicantAppointmentDetails.Where(x => x.AAD_OrderID == OrderID && x.AAD_IsActive).Select(x => x.AAD_ID).FirstOrDefault();
                if (orderAppointmentDetailId > 0)
                    return _dbContext.ApplicantAppointmentDetailsExts.Any(x => x.AADE_AppAppointmentDetailID == orderAppointmentDetailId && x.AADE_ParentAppAppointmentDetailID != null && x.AADE_ServiceStatus != null && x.AADE_IsActive && !x.AADE_IsDeleted);
            }
            return false;
        }

        public bool CheckShipmntPriorAppointment(int OrderID, int CurrentLoggedInUserID, int AppointmentDetailExtId)
        {
            if (OrderID > 0 && CurrentLoggedInUserID > 0 && AppointmentDetailExtId > 0)
            {
                var shipmentDate = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_ID == AppointmentDetailExtId).Select(x => x.AADE_DispatchedDate).FirstOrDefault();
                int? SlotId = LocationDBContext.FingerPrintApplicantAppointments.Where(x => x.FPAA_OrderID == OrderID && !x.FPAA_IsDeleted).Select(x => x.FPAA_ScheduleSlotID).FirstOrDefault();
                if (SlotId.IsNotNull() && shipmentDate.IsNotNull())
                {
                    var SlotDate = LocationDBContext.FingerPrintScheduleSlots.Where(x => x.FPSS_SlotID == SlotId && !x.FPSS_IsDeleted).Select(x => x.FPSS_SlotDate).FirstOrDefault();
                    var SlotEndTime = LocationDBContext.FingerPrintScheduleSlots.Where(x => x.FPSS_SlotID == SlotId && !x.FPSS_IsDeleted).Select(x => x.FPSS_SlotEndTime).FirstOrDefault().ToString();
                    if (SlotDate.IsNotNull() && SlotEndTime.IsNotNull())
                        if (Convert.ToDateTime(shipmentDate) < Convert.ToDateTime(Convert.ToDateTime(SlotDate).Add(TimeSpan.Parse(SlotEndTime))))
                        {
                            return true;
                        }
                }
            }
            return false;
        }

        public DateTime? GetAdditionalServiceShipmentDate(Int32 OrderID, Int32 CurrentLoggedInUserID, Int32 AppointmentDetailExtId)
        {
            int cabsTypeStatusId = _dbContext.lkpBkgSvcTypes.Where(x => x.BST_Code == "AAAA").FirstOrDefault().BST_ID;

            DateTime? shipmentDate = null;
            if (OrderID > 0 && CurrentLoggedInUserID > 0 && AppointmentDetailExtId > 0)
            {
                shipmentDate = _dbContext.ApplicantAppointmentDetailsExts.Where(x => x.AADE_AppAppointmentDetailID == AppointmentDetailExtId && x.AADE_ServiceTypeID != cabsTypeStatusId && x.AADE_IsActive).Select(x => x.AADE_DispatchedDate).FirstOrDefault();
            }
            return shipmentDate;
        }

        #region Archive order
        public DataTable GetBkgPkgPrevOrderDetails(Int32 tenantId, Int32 orgainizatuionUserId)
        {
            EntityConnection connection = base.ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("ams.usp_GetBkgPkgPrevOrderDetails", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@TenantId", tenantId);
                _command.Parameters.AddWithValue("@OrgainizatuionUserId", orgainizatuionUserId);
                SqlDataAdapter _adp = new SqlDataAdapter();
                _adp.SelectCommand = _command;
                DataSet _ds = new DataSet();
                _adp.Fill(_ds);
                if (_ds.Tables.Count > 0)
                    return _ds.Tables[0];
            }
            return new DataTable();
        }
        #endregion
    }
}
