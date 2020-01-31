using Business.ReportExecutionService;
using CoreWeb.IntsofExceptionModel.Interface;
using CoreWeb.IntsofLoggerModel.Interface;
using Entity.ClientEntity;
using INTSOF.Contracts;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.QueueManagement;
using INTSOF.Utils;
using INTSOF.Utils.CommonPocoClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace Business.RepoManagers
{
    public class BackgroundProcessOrderManager
    {
        #region Backround Order Search
        /// <summary>
        /// Get the backround order detail based on  search parameters 
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        /// <param name="gridCustomPaging">gridCustomPaging</param>
        /// <param name="bkgOrderSearchContract">bkgOrderSearchContract</param>
        /// <returns>List<Entity.ClientEntity.BackroundOrderSearch></returns>
        public static List<Entity.ClientEntity.BackroundOrderSearch> GetBackroundOrderSearchDetail(Int32 tenantID, CustomPagingArgsContract gridCustomPaging, BkgOrderSearchContract bkgOrderSearchContract)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).GetBackroundOrderSearchDetail(gridCustomPaging, bkgOrderSearchContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Check the backround order is dispatched to clearstar or not
        /// </summary>
        /// <param name="tenantID">tenantID</param>
        /// <param name="bkgOrderId">bkgOrderId</param>
        /// <returns>ExternalVendorBkgOrderDetail</returns>
        public static Entity.ClientEntity.ExternalVendorBkgOrderDetail GetExternalVendorBkgOrderDetail(Int32 tenantID, Int32 bkgOrderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).GetExternalVendorBkgOrderDetail(bkgOrderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Get Institution Status color based on the selected intitution
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <returns>List<Entity.ClientEntity.InstitutionOrderFlag></returns>
        public static List<Entity.ClientEntity.InstitutionOrderFlag> GetInstitutionStatusColor(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetInstitutionStatusColor(tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Get the backround service group
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <returns>List<Entity.ClientEntity.BkgSvcGroup></returns>
        public static List<Entity.ClientEntity.BkgSvcGroup> GetBackroundServiceGroup(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBackroundServiceGroup();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Get backround order client status 
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <returns>List<Entity.ClientEntity.BkgOrderClientStatu></returns>
        public static List<Entity.ClientEntity.BkgOrderClientStatu> GetBkgOrderClientStatus(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgOrderClientStatus(tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Get the Institution order flag record
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="institutionStatusColorId">institutionStatusColorId</param>
        /// <returns>InstitutionOrderFlag</returns>
        public static InstitutionOrderFlag GetOrderInstitutionStatusColor(Int32 tenantId, Int32 institutionStatusColorId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetOrderInstitutionStatusColor(institutionStatusColorId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Get Client Status based on the orderID
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="orderId">orderId</param>
        /// <returns>Int32</returns>
        public static Int32 GetClientStatusByOrderId(Int32 tenantId, Int32 orderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetClientStatusByOrderId(orderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Update Order Client status in backround order.
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="orderId">orderId</param>
        /// <param name="ClinetStatusId">clinetStatusId</param>
        /// <param name="currentLoggedInUserId">currentLoggedInUserId</param>
        /// <returns></returns>
        public static Boolean UpdateOrderClientStatus(Int32 tenantId, Int32 orderId, Int32 clinetStatusId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).UpdateOrderClientStatus(orderId, clinetStatusId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Save client status history with respect to the notes
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="orderId">orderId</param>
        /// <param name="notes">notes</param>
        /// <param name="currentLoggedInUserId">currentLoggedInUserId</param>
        /// <returns></returns>
        public static Boolean SaveOrderClientStatusHistory(Int32 tenantId, Int32 orderId, String notes, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).SaveOrderClientStatusHistory(orderId, notes, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Get client status history based on the orderId 
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="orderId">orderId</param>
        /// <returns>List<Entity.ClientEntity.BkgOrderClientStatusHistory></returns>
        public static List<Entity.ClientEntity.BkgOrderClientStatusHistory> GetClientOrderStatusHistory(Int32 tenantId, Int32 orderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetClientOrderStatusHistory(orderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method is used to Get Service Form Status
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static BkgOrder GetBkgOrderDetail(Int32 tenantId, Int32 masterOrderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgOrderDetail(masterOrderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static Boolean UpdateBkgOrderArchiveStatus(Int32 tenantId, Int32 masterOrderId, Boolean archiveStatus, String eventDetailNotes, Int32 loggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).UpdateBkgOrderArchiveStatus(masterOrderId, archiveStatus, eventDetailNotes, loggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        #endregion

        #region Order Detail

        /// <summary>
        /// Method is used to get the Menu Item
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static OrderDetailMain GetOrderDetailMenuItem(Int32 tenantId, Int32 orderID)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetOrderDetailMenuItem(orderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        /// <summary>
        /// Method is used to get the Applicant Order Detail
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static ApplicantOrderDetail GetApplicantOrderDetail(Int32 tenantId, Int32 orderID)
        {

            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetApplicantOrderDetail(orderID, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        /// <summary>
        /// Method is used to get the Order Status
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static Boolean UpdateOrderStatus(Int32 tenantId, Int32 selectedOrderColorStatusId, Int32 orderId, Int32 selectedOrderStatusTypeId, Int32 loggedInUserId, Int32 orderPkgSvcGroupID, BkgOrderPackageSvcGroup bkgOrderPackageSvcGroup)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).UpdateOrderStatus(selectedOrderColorStatusId, orderId, selectedOrderStatusTypeId, loggedInUserId, orderPkgSvcGroupID, bkgOrderPackageSvcGroup);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }



        /// <summary>
        /// Method is used to Get Order Request Type
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<lkpOrderStatusType> GetOrderRequestType(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpOrderStatusType>(tenantId).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Get Bkg Order selected service details xml
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static string GetBkgOrderServiceDetails(Int32 tenantId, Int32 orderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgOrderServiceDetails(orderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Mwthod is used to Get BkgOrderCustomFormAttributesData
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="masterOrderId"></param>
        /// <param name="customFormId"></param>
        /// <returns></returns>
        public static BkgOrderDetailCustomFormDataContract GetBkgOrderCustomFormAttributesData(Int32 tenantId, Int32 masterOrderId, Int32 customFormId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgOrderCustomFormAttributesData(masterOrderId, customFormId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        /// <summary>
        /// Method is used to Get Notes By OrderId
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static DataTable GetNotesByOrderId(Int32 tenantId, Int32 orderID)
        {

            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetNotesByOrderId(orderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }
        /// <summary>
        /// Method is used to Add Note
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderNote"></param>
        /// <returns></returns>
        public static Boolean AddNote(Int32 tenantId, OrderNote orderNote)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).AddNote(orderNote);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        /// <summary>
        /// Method is used to Get Package By OrderId
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static List<PackageDetailsContract> GetPackageByOrderId(Int32 tenantId, Int32 orderID)
        {

            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetPackageByOrderId(orderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        /// <summary>
        /// Get OrganisationUserProfile By OrderId
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="masterOrderId">Master Order Id</param>
        /// <returns>Organization User Profile</returns>
        public static OrganizationUserProfile GetOrganisationUserProfileByOrderId(Int32 tenantId, Int32 masterOrderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetOrganisationUserProfileByOrderId(masterOrderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        /// <summary>
        /// Method is used to get the ServiceLine Price by OrderID
        /// </summary>
        /// <param name="orderID">orderID</param>
        /// <returns>List of BackroundOrderServiceLinePrice</returns>
        public static OrderServiceLineItemPriceInfo GetBackroundOrderServiceLinePriceByOrderID(Int32 tenantID, Int32 orderId, List<Int32> Bkg_PkgIDs = null)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).GetBackroundOrderServiceLinePriceByOrderID(orderId, Bkg_PkgIDs);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        /// <summary>
        /// Method is used to Get External Vendor Services By OrderId
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static List<ExternalVendorServiceContract> GetExternalVendorServicesByOrderId(Int32 tenantId, Int32 orderID)
        {

            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetExternalVendorServicesByOrderId(orderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        /// <summary>
        /// Method is used to get the Get Bkg Order Line Item Details
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static OrderLineDetailsContract GetBkgOrderLineItemDetails(Int32 tenantId, Int32 PSLI_ID)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgOrderLineItemDetails(PSLI_ID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        /// <summary>
        /// Get GetlkpOrderLineItemStatu
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns>List Of lkpBusinessChannelType</returns>
        public static List<lkpOrderLineItemResultStatu> GetlkpOrderLineItemResultStatus(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpOrderLineItemResultStatu>(tenantId).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        /// <summary>
        /// Method is used to Update Record To ADBCopy
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static Boolean UpdateRecordToADBCopy(Int32 tenantId, BkgOrderLineItemResultCopy bkgOrderLineItemResultCopy, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).UpdateRecordToADBCopy(bkgOrderLineItemResultCopy, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }
        #endregion

        #region Background Order Notification

        /// <summary>
        /// To Get Background Order Notification Data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderID"></param>
        /// <returns>List of BkgOrderNotificationDataContract</returns>
        public static List<BkgOrderNotificationDataContract> GetBkgOrderNotificationData(Int32 tenantId, Int32 chunkSize, String orderIDs = null)
        {
            try
            {
                return AssignBkgOrderNotificationDataToDataModel(BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgOrderNotificationData(chunkSize, orderIDs));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<BkgOrderNotificationDataContract> GetBkgOrderServiceFormNotificationDataForAdminEntry(Int32 tenantId, Int32 orderId, String serviceIds)
        {
            try
            {
                return AssignBkgOrderNotificationDataToDataModel(BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgOrderServiceFormNotificationDataForAdminEntry(orderId, serviceIds));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// To Get Background Order Result Completed Notification Data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderID"></param>
        /// <returns>List of BkgOrderNotificationDataContract</returns>
        public static List<BkgOrderNotificationDataContract> GetBkgOrderResultCompletedNotificationData(Int32 tenantId, Int32 chunkSize)
        {
            try
            {
                return AssignBkgOrderResultCompletedNotificationDataToDataModel(BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgOrderResultCompletedNotificationData(chunkSize));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static string GetBGPkgPDFAttachementStatus(Int32 tenantID, Int32 hierarchyNodeID)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).GetBGPkgPDFAttachementStatus(hierarchyNodeID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// To Get Service Group Completed Notification Data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderID"></param>
        /// <returns>List of BkgOrderNotificationDataContract</returns>
        public static List<BkgOrderNotificationDataContract> GetServiceGroupCompletedNotificationData(Int32 tenantId, Int32 chunkSize)
        {
            try
            {
                return AssignSvcGrpCompletedNotificationDataToDataModel(BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetSvcGrpResultCompletedNotificationData(chunkSize));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Assign the datatable record in BkgOrderNotificationDataContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List of BkgOrderNotificationDataContract</returns>
        private static List<BkgOrderNotificationDataContract> AssignBkgOrderNotificationDataToDataModel(DataTable table)
        {
            try
            {
                List<BkgOrderNotificationDataContract> bkgOrdreNotificationDataList = new List<BkgOrderNotificationDataContract>();

                IEnumerable<DataRow> rows = table.AsEnumerable();
                if (rows != null && rows.Count() > 0)
                {
                    foreach (var row in rows)
                    {
                        BkgOrderNotificationDataContract bkgOrdreNotificationData = new BkgOrderNotificationDataContract();

                        bkgOrdreNotificationData.OrderID = Convert.ToInt32(row["OrderID"]);
                        bkgOrdreNotificationData.HierarchyNodeID = Convert.ToInt32(row["HierarchyNodeID"]);
                        bkgOrdreNotificationData.BkgOrderID = Convert.ToInt32(row["BkgOrderID"]);
                        bkgOrdreNotificationData.ServiceID = Convert.ToInt32(row["ServiceID"]);
                        bkgOrdreNotificationData.ServiceAtachedFormID = Convert.ToInt32(row["ServiceAtachedFormID"]);
                        bkgOrdreNotificationData.ServiceAtachedFormName = Convert.ToString(row["ServiceAtachedFormName"]);
                        bkgOrdreNotificationData.SendAutomatically = Convert.ToBoolean(row["SendAutomatically"]);
                        bkgOrdreNotificationData.BkgOrderPackageSvcID = Convert.ToInt32(row["BkgOrderPackageSvcID"]);
                        bkgOrdreNotificationData.ServiceFormMappingID = Convert.ToInt32(row["ServiceFormMappingID"]);
                        bkgOrdreNotificationData.OrganizationUserID = Convert.ToInt32(row["OrganizationUserID"]);
                        bkgOrdreNotificationData.FirstName = Convert.ToString(row["FirstName"]);
                        bkgOrdreNotificationData.LastName = Convert.ToString(row["LastName"]);
                        bkgOrdreNotificationData.PrimaryEmailAddress = Convert.ToString(row["PrimaryEmailAddress"]);
                        if (row["SystemDocumentID"].GetType().Name != "DBNull")
                            bkgOrdreNotificationData.SystemDocumentID = Convert.ToInt32(row["SystemDocumentID"]);
                        if (row["DocumentName"].GetType().Name != "DBNull")
                            bkgOrdreNotificationData.DocumentName = Convert.ToString(row["DocumentName"]);
                        if (row["DocumentPath"].GetType().Name != "DBNull")
                            bkgOrdreNotificationData.DocumentPath = Convert.ToString(row["DocumentPath"]);
                        if (row["RefrenceID"].GetType().Name != "DBNull")
                            bkgOrdreNotificationData.RefrenceID = (Guid)(row["RefrenceID"]);
                        if (row["DocumentSize"].GetType().Name != "DBNull")
                            bkgOrdreNotificationData.DocumentSize = Convert.ToInt32(row["DocumentSize"]);
                        bkgOrdreNotificationData.ServiceName = Convert.ToString(row["ServiceName"]);
                        bkgOrdreNotificationData.ServiceGroupName = row["ServiceGroupName"].GetType().Name != "DBNull" ? Convert.ToString(row["ServiceGroupName"]) : String.Empty;
                        bkgOrdreNotificationData.PackageName = row["PackageName"].GetType().Name != "DBNull" ? Convert.ToString(row["PackageName"]) : String.Empty;
                        bkgOrdreNotificationData.OrderStatus = row["OrderStatus"].GetType().Name != "DBNull" ? Convert.ToString(row["OrderStatus"]) : String.Empty;
                        bkgOrdreNotificationData.OrderDate = row["OrderDate"].GetType().Name != "DBNull" ? Convert.ToDateTime(row["OrderDate"]) : DateTime.MinValue;
                        bkgOrdreNotificationData.NodeHierarchy = row["NodeHierarchy"].GetType().Name != "DBNull" ? Convert.ToString(row["NodeHierarchy"]) : String.Empty;
                        bkgOrdreNotificationData.OrderNumber = row["OrderNumber"].GetType().Name != "DBNull" ? Convert.ToString(row["OrderNumber"]) : String.Empty;
                        bkgOrdreNotificationDataList.Add(bkgOrdreNotificationData);
                    }
                }

                return bkgOrdreNotificationDataList;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Assign the datatable record in BkgOrderNotificationDataContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List of BkgOrderNotificationDataContract</returns>
        private static List<BkgOrderNotificationDataContract> AssignBkgOrderResultCompletedNotificationDataToDataModel(DataTable table)
        {
            try
            {
                List<BkgOrderNotificationDataContract> bkgOrdreNotificationDataList = new List<BkgOrderNotificationDataContract>();

                IEnumerable<DataRow> rows = table.AsEnumerable();
                if (rows != null && rows.Count() > 0)
                {
                    foreach (var row in rows)
                    {
                        BkgOrderNotificationDataContract bkgOrdreNotificationData = new BkgOrderNotificationDataContract();

                        bkgOrdreNotificationData.OrderID = Convert.ToInt32(row["OrderID"]);
                        bkgOrdreNotificationData.HierarchyNodeID = Convert.ToInt32(row["HierarchyNodeID"]);
                        bkgOrdreNotificationData.BkgOrderID = Convert.ToInt32(row["BkgOrderID"]);
                        bkgOrdreNotificationData.OrganizationUserID = Convert.ToInt32(row["OrganizationUserID"]);
                        bkgOrdreNotificationData.FirstName = Convert.ToString(row["FirstName"]);
                        bkgOrdreNotificationData.LastName = Convert.ToString(row["LastName"]);
                        bkgOrdreNotificationData.PrimaryEmailAddress = Convert.ToString(row["PrimaryEmailAddress"]);

                        if (row.Table.Columns.Contains("IsEmployment"))
                            bkgOrdreNotificationData.IsEmployment = Convert.ToBoolean(row["IsEmployment"]);

                        if (row.Table.Columns.Contains("OrderNumber"))
                            bkgOrdreNotificationData.OrderNumber = row["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(row["OrderNumber"]);

                        if (row.Table.Columns.Contains("IsAdminOrder"))
                            bkgOrdreNotificationData.IsAdminOrder = Convert.ToBoolean(row["IsAdminOrder"]);
                        //UAT-3453
                        if (row.Table.Columns.Contains("IsOrderFlag"))
                            bkgOrdreNotificationData.IsOrderFlag = row["IsOrderFlag"] == DBNull.Value ? false : Convert.ToBoolean(row["IsOrderFlag"]);

                        bkgOrdreNotificationDataList.Add(bkgOrdreNotificationData);
                    }
                }

                return bkgOrdreNotificationDataList;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Assign the datatable record in BkgOrderNotificationDataContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List of BkgOrderNotificationDataContract</returns>
        private static List<BkgOrderNotificationDataContract> AssignSvcGrpCompletedNotificationDataToDataModel(DataTable table)
        {
            try
            {
                List<BkgOrderNotificationDataContract> bkgOrdreNotificationDataList = new List<BkgOrderNotificationDataContract>();

                IEnumerable<DataRow> rows = table.AsEnumerable();
                if (rows != null && rows.Count() > 0)
                {
                    foreach (var row in rows)
                    {
                        BkgOrderNotificationDataContract bkgOrdreNotificationData = new BkgOrderNotificationDataContract();

                        bkgOrdreNotificationData.OrderID = Convert.ToInt32(row["OrderID"]);
                        bkgOrdreNotificationData.HierarchyNodeID = Convert.ToInt32(row["HierarchyNodeID"]);
                        bkgOrdreNotificationData.BkgOrderID = Convert.ToInt32(row["BkgOrderID"]);
                        bkgOrdreNotificationData.OrganizationUserID = Convert.ToInt32(row["OrganizationUserID"]);
                        bkgOrdreNotificationData.FirstName = Convert.ToString(row["FirstName"]);
                        bkgOrdreNotificationData.LastName = Convert.ToString(row["LastName"]);
                        bkgOrdreNotificationData.PrimaryEmailAddress = Convert.ToString(row["PrimaryEmailAddress"]);
                        bkgOrdreNotificationData.OrderPackageServiceGroupID = Convert.ToInt32(row["OrderPackageServiceGroupID"]);
                        bkgOrdreNotificationData.ServiceGroupID = Convert.ToInt32(row["ServiceGroupID"]);
                        bkgOrdreNotificationData.ServiceGroupName = Convert.ToString(row["ServiceGroupName"]);
                        bkgOrdreNotificationData.PackageServiceGroupID = Convert.ToInt32(row["PackageServiceGroupID"]);
                        if (row.Table.Columns.Contains("IsEmployment"))
                            bkgOrdreNotificationData.IsEmployment = Convert.ToBoolean(row["IsEmployment"]);

                        if (row.Table.Columns.Contains("ApplicantProfileAddress"))
                            bkgOrdreNotificationData.ApplicantProfileAddress = Convert.ToString(row["ApplicantProfileAddress"]);

                        if (row.Table.Columns.Contains("OrderNumber"))
                            bkgOrdreNotificationData.OrderNumber = row["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(row["OrderNumber"]);

                        bkgOrdreNotificationDataList.Add(bkgOrdreNotificationData);
                    }
                }

                return bkgOrdreNotificationDataList;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get Business Channel Type
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns>List Of lkpBusinessChannelType</returns>
        public static List<lkpBusinessChannelType> GetBusinessChannelType(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpBusinessChannelType>(tenantId).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get Service Form Status
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns>List Of lkpServiceFormStatu</returns>
        public static List<lkpServiceFormStatu> GetServiceFormStatus(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpServiceFormStatu>(tenantId).Where(cond => !cond.SFS_IsDeleted && cond.SFS_IsActive).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get Order Notify Status
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns>List Of lkpOrderNotifyStatu</returns>
        public static List<lkpOrderNotifyStatu> GetOrderNotifyStatus(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpOrderNotifyStatu>(tenantId).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get Order Notify Status
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns>List Of lkpOrderNotifyStatu</returns>
        public static List<lkpOrderNotificationType> GetOrderNotificationType(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpOrderNotificationType>(tenantId).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Create Background Order Notification
        /// </summary>        
        /// <param name="tenantId"></param>
        /// <param name="orderNotification"></param>
        /// <returns>OrderNotificationID</returns>
        public static Int32 CreateOrderNotification(Int32 tenantId, OrderNotification orderNotification)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).CreateOrderNotification(orderNotification);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Create Background Order Service Form
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="bkgOrderServiceForm"></param>
        /// <returns>BkgOrderServiceFormID</returns>
        public static Int32 CreateBkgOrderServiceForm(Int32 tenantId, BkgOrderServiceForm bkgOrderServiceForm)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).CreateBkgOrderServiceForm(bkgOrderServiceForm);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Update Background Order Notify Status
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="bkgOrder"></param>
        /// <returns>True/False</returns>
        public static Boolean UpdateBkgOrderNotifyStatus(Int32 tenantId, BkgOrder bkgOrd)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).UpdateBkgOrderNotifyStatus(bkgOrd);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Send notification when a manual service form status has been changed from send to student to in progress by agency.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="oldServiceFormStatusId"></param>
        /// <param name="newServiceFormStatusId"></param>
        /// <param name="manualServiceFormContract"></param>
        public static void SendSvcFormStsChangeNotification(Int32 tenantId, Int32 oldServiceFormStatusId, Int32 newServiceFormStatusId, ManualServiceFormContract manualServiceFormContract)
        {
            try
            {
                List<lkpServiceFormStatu> lkpServiceFormStatus = BackgroundProcessOrderManager.GetServiceFormStatus(tenantId);
                if (!lkpServiceFormStatus.IsNullOrEmpty())
                {
                    String sentToStudentSvcFormStsCode = ServiceFormStatus.SENT_TO_STUDENT.GetStringValue();
                    Int32 sentToStudentSvcFormStsID = lkpServiceFormStatus.FirstOrDefault(cond => cond.SFS_Code == sentToStudentSvcFormStsCode).SFS_ID;
                    String inProcessAgencySvcFormStsCode = ServiceFormStatus.IN_PROCESS_AGENCY.GetStringValue();
                    Int32 inProcessAgencySvcFormStsID = lkpServiceFormStatus.FirstOrDefault(cond => cond.SFS_Code == inProcessAgencySvcFormStsCode).SFS_ID;
                    //UAT-2156:New Notification for students with Comm Copy setting for Form Dispatched (Manual Service Forms).
                    String needToSendSvcFormStsCode = ServiceFormStatus.NEED_TO_SEND.GetStringValue();
                    Int32 needToSendSvcFormStsID = lkpServiceFormStatus.FirstOrDefault(cond => cond.SFS_Code == needToSendSvcFormStsCode).SFS_ID;


                    #region UAT-2671:Have Complio send out notifications to the students once the background order status  status has been changed to "In Progress with FBI" or "Rejected by FBI".

                    String inProcesssWithFBIStsCode = ServiceFormStatus.IN_PROCESS_FBI.GetStringValue();
                    Int32 inProcesssWithFBIStsID = lkpServiceFormStatus.FirstOrDefault(cond => cond.SFS_Code == inProcesssWithFBIStsCode).SFS_ID;

                    String isRejectedByFBIFirstStsCode = ServiceFormStatus.REJECTED_BY_FBI_FIRST.GetStringValue();
                    Int32 isRejectedByFBIFirstStsID = lkpServiceFormStatus.FirstOrDefault(cond => cond.SFS_Code == isRejectedByFBIFirstStsCode).SFS_ID;

                    String isRejectedByFBISecondStsCode = ServiceFormStatus.REJECTED_BY_FBI_SECOND.GetStringValue();
                    Int32 isRejectedByFBISecondStsID = lkpServiceFormStatus.FirstOrDefault(cond => cond.SFS_Code == isRejectedByFBISecondStsCode).SFS_ID;

                    String isRejectedByFBIThirdStsCode = ServiceFormStatus.REJECTED_BY_FBI_THIRD.GetStringValue();
                    Int32 isRejectedByFBIThirdStsID = lkpServiceFormStatus.FirstOrDefault(cond => cond.SFS_Code == isRejectedByFBIThirdStsCode).SFS_ID;

                    String isRejectedByFBIFourthStsCode = ServiceFormStatus.REJECTED_BY_FBI_FOURTH.GetStringValue();
                    Int32 isRejectedByFBIFourthStsID = lkpServiceFormStatus.FirstOrDefault(cond => cond.SFS_Code == isRejectedByFBIFourthStsCode).SFS_ID;

                    #endregion

                    //Create Dictionary
                    Dictionary<String, object> dictMailData = new Dictionary<String, object>();
                    if (manualServiceFormContract.ApplicantLastName.IsNullOrEmpty())
                        dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, manualServiceFormContract.ApplicantFirstName);
                    else
                        dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, manualServiceFormContract.ApplicantFirstName + " " + manualServiceFormContract.ApplicantLastName);
                    //dictMailData.Add(EmailFieldConstants.ORDER_NO, manualServiceFormContract.OrderID);
                    dictMailData.Add(EmailFieldConstants.ORDER_NO, manualServiceFormContract.OrderNumber);

                    //UAT-2156:New Notification for students with Comm Copy setting for Form Dispatched (Manual Service Forms) 
                    //Added new place holders in the dictionary.
                    dictMailData.Add(EmailFieldConstants.SERVICE_FORM_NAME, manualServiceFormContract.SFName);
                    dictMailData.Add(EmailFieldConstants.SERVICE_NAME, manualServiceFormContract.ServiceName);
                    dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, manualServiceFormContract.PackageName);

                    //UAT-2671
                    String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
                    dictMailData.Add(EmailFieldConstants.SERVICE_GROUP_NAME, manualServiceFormContract.ServiceGroupName);
                    dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);

                    //Create MockUp Data
                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                    if (manualServiceFormContract.ApplicantLastName.IsNullOrEmpty())
                        mockData.UserName = manualServiceFormContract.ApplicantFirstName;
                    else
                        mockData.UserName = manualServiceFormContract.ApplicantFirstName + " " + manualServiceFormContract.ApplicantLastName;
                    mockData.EmailID = manualServiceFormContract.ApplicantEmailAddress;
                    mockData.ReceiverOrganizationUserID = manualServiceFormContract.OrganizationUserId;

                    //Send notification when a manual service form status has been changed from send to student to in progress by agency.
                    if (oldServiceFormStatusId == sentToStudentSvcFormStsID && newServiceFormStatusId == inProcessAgencySvcFormStsID)
                    {
                        //Send mail
                        CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_RECEIVED_MANUAL_SERVICE_FORM, dictMailData, mockData, tenantId, manualServiceFormContract.HierarchyNodeID, null, null, true);

                        //Send Message
                        CommunicationManager.SaveMessageContent(CommunicationSubEvents.NOTIFICATION_FOR_RECEIVED_MANUAL_SERVICE_FORM, dictMailData, manualServiceFormContract.OrganizationUserId, tenantId);
                    }
                    //UAT-2156:New Notification for students with Comm Copy setting for Form Dispatched (Manual Service Forms) 
                    //send notification when a manual service form status is changed from "need to send" to "sent to student".
                    if (oldServiceFormStatusId == needToSendSvcFormStsID && newServiceFormStatusId == sentToStudentSvcFormStsID)
                    {
                        //Send mail
                        CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.Manual_Service_Form_Dispatched_Notification, dictMailData, mockData, tenantId, manualServiceFormContract.HierarchyNodeID, null, null, true);

                        //Send Message
                        CommunicationManager.SaveMessageContent(CommunicationSubEvents.Manual_Service_Form_Dispatched_Notification, dictMailData, manualServiceFormContract.OrganizationUserId, tenantId);
                    }
                    //UAT-2671:Have Complio send out notifications to the students once the background order status  status has been changed to "In Progress with FBI" or "Rejected by FBI".
                    if (oldServiceFormStatusId != inProcesssWithFBIStsID && newServiceFormStatusId == inProcesssWithFBIStsID)
                    {
                        //Send mail
                        CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_SVC_FORM_STATUS_CHANGED_TO_IN_PROCESS_TO_FBI, dictMailData, mockData, tenantId, manualServiceFormContract.HierarchyNodeID, null, null, true);
                    }
                    if ((oldServiceFormStatusId != isRejectedByFBIFirstStsID && newServiceFormStatusId == isRejectedByFBIFirstStsID)
                        || (oldServiceFormStatusId != isRejectedByFBISecondStsID && newServiceFormStatusId == isRejectedByFBISecondStsID)
                        || (oldServiceFormStatusId != isRejectedByFBIThirdStsID && newServiceFormStatusId == isRejectedByFBIThirdStsID)
                        || (oldServiceFormStatusId != isRejectedByFBIFourthStsID && newServiceFormStatusId == isRejectedByFBIFourthStsID))
                    {
                        //Send mail
                        CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_SVC_FORM_STATUS_CHANGED_TO_REJECTED_BY_FBI, dictMailData, mockData, tenantId, manualServiceFormContract.HierarchyNodeID, null, null, true);
                    }

                    #region UAT-3752
                    if (oldServiceFormStatusId != isRejectedByFBIFirstStsID && newServiceFormStatusId == isRejectedByFBIFirstStsID)
                    {
                        //Send notification mail
                        CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_SVC_FORM_STATUS_CHANGED_TO_REJECTED_BY_FBI_FIRST, dictMailData, mockData, tenantId, manualServiceFormContract.HierarchyNodeID, null, null, true);
                    }

                    if ((oldServiceFormStatusId != isRejectedByFBISecondStsID && newServiceFormStatusId == isRejectedByFBISecondStsID)
                        || (oldServiceFormStatusId != isRejectedByFBIThirdStsID && newServiceFormStatusId == isRejectedByFBIThirdStsID)
                        || (oldServiceFormStatusId != isRejectedByFBIFourthStsID && newServiceFormStatusId == isRejectedByFBIFourthStsID))
                    {
                        //Send notification mail
                        CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_SVC_FORM_STATUS_CHANGED_TO_REJECTED_BY_FBI_SECOND_THIRD_FOURTH, dictMailData, mockData, tenantId, manualServiceFormContract.HierarchyNodeID, null, null, true);
                    }
                    #endregion
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        #endregion

        #region Order Detail for Client Admin

        /// <summary>
        /// Method is used to get the order event history
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static List<OrderEventHistoryContract> GetOrderEventHistory(Int32 orderID, Int32 selectedTenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(selectedTenantId).GetOrderEventHistory(orderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method is used to get the Clent Admin Order Detail
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static OrderDetailClientAdmin GetOrderDetailsInfo(Int32 tenantId, Int32 orderID)
        {

            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetOrderDetailsInfo(orderID, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        /// <summary>
        /// Method is used to get the Clent Admin Service group Detail
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static List<OrderServiceGroupDetails> GetServiceGroupDetails(Int32 tenantId, Int32 orderID)
        {

            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetServiceGroupDetails(orderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        /// <summary>
        /// Method is used to get the Order Detail
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static OrderDetailHeaderInfo GetOrderDetailHeaderInfo(Int32 tenantId, Int32 orderID)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetOrderDetailHeaderInfo(orderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region CustomForms

        public static List<CustomFormDataContract> GetCustomFormsForThePackage(Int32 tenantId, String packageId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetCustomFormsForThePackage(packageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<BkgSvcAttributeOption> GetOptionValues(Int32 tenantId, Int32 attributeId)
        {
            try
            {
                List<BkgSvcAttributeOption> lst = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetOptionValues(attributeId);
                if (lst.IsNullOrEmpty())
                {
                    return new List<BkgSvcAttributeOption>();
                }
                return lst;

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static CustomFormDataContract GetAttributesForTheCustomForm(Int32 tenantId, String packageId, Int32 customFormId, String _languageCode = default(string))
        {
            try
            {
                if (String.IsNullOrEmpty(_languageCode))
                {
                    _languageCode = Languages.ENGLISH.GetStringValue();
                }
                CustomFormDataContract customFormDataContract = new CustomFormDataContract();
                List<AttributesForCustomFormContract> lstOfCustomFormAttributesAttributes = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId)
                                                                                                    .GetAttributesForTheCustomForm(packageId, customFormId, _languageCode);
                if (lstOfCustomFormAttributesAttributes.IsNotNull())
                {
                    customFormDataContract.lstCustomFormAttributes = lstOfCustomFormAttributesAttributes;
                }
                return customFormDataContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Update Background Order Color Status
        public static Boolean UpdateBkgOrderColorStatus(Int32 tenantId, Int32 backgroundProcessUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).AssignFlagToCompletedOrders(backgroundProcessUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Order Notification History

        public static OrderNotificationHistoryContract GetOrderNotificationHistory(Int32 orderId, Int32 tenantId)
        {
            try
            {
                OrderNotificationHistoryContract orderNotificationHistoryContract = null;
                var lstOrderNotificationDetail = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetOrderNotificationHistory(orderId);
                if (lstOrderNotificationDetail.IsNotNull())
                {
                    List<Int32> lstSysCommunicationId = lstOrderNotificationDetail.Where(cond => cond.ServiceFormId == 0).Select(x => x.SystemCommunicationId).ToList();
                    if (lstSysCommunicationId.Count > 0)
                    {
                        var dictionary = MessageManager.GetSubEventsNamesBySysCommId(lstSysCommunicationId);
                        if (dictionary.IsNotNull() && dictionary.Count > 0)
                        {
                            foreach (var item in dictionary)
                            {
                                var listItem = lstOrderNotificationDetail.Where(cond => cond.SystemCommunicationId == item.Key).FirstOrDefault();
                                listItem.NotificationType = item.Value;

                            }
                        }
                    }
                    orderNotificationHistoryContract = new OrderNotificationHistoryContract();
                    orderNotificationHistoryContract.OrderNotificationDetailList = lstOrderNotificationDetail;
                    orderNotificationHistoryContract.StatusList = LookupManager.GetLookUpData<lkpServiceFormStatu>(tenantId).Where(cond => !cond.SFS_IsDeleted && cond.SFS_IsActive).Select(cond => new LookupContract
                    {
                        Name = cond.SFS_Name,
                        ID = cond.SFS_ID
                    }).ToList();
                }
                return orderNotificationHistoryContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<LookupContract> GetHistoryByOrderNotificationId(Int32 orderNotificationId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetHistoryByOrderNotificationId(orderNotificationId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Update BkgOrderServiceForm status and creates history for Order Notification and BkgOrderServiceForm table.
        /// </summary>
        /// <param name="orderNotificationId">Order Notification ID</param>
        /// <param name="statusId">Status ID</param>
        /// <param name="tenantId">Tenant ID</param>
        /// <param name="loggedInUserId">Current Looged In User ID</param>
        /// <returns>True is status is updated,Else false</returns>
        public static Boolean UpdateOrderNotificationBkgOrderServiceForm(Int32 orderNotificationId, Int32 tenantId, Int32 loggedInUserId, Int32 statusId = 0, Int32 oldStatusId = 0, Int32 systemCommunicationId = 0, Guid? messageId = null)
        {
            try
            {
                //Find the current order notification row for the given ID.
                OrderNotification masterOrderNotification = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetOrderNotificationById(orderNotificationId);
                //Checks if the retrieved object is not null
                if (masterOrderNotification.IsNotNull())
                {
                    //Creates new order notification object 
                    OrderNotification newOrderNotification = new OrderNotification();
                    newOrderNotification.ONTF_OrderID = masterOrderNotification.ONTF_OrderID;
                    newOrderNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationId == 0 ? masterOrderNotification.ONTF_MSG_SystemCommunicationID : systemCommunicationId;
                    newOrderNotification.ONTF_MSG_MessageID = messageId == null ? masterOrderNotification.ONTF_MSG_MessageID : messageId;
                    newOrderNotification.ONTF_BusinessChannelTypeID = masterOrderNotification.ONTF_BusinessChannelTypeID;
                    newOrderNotification.ONTF_IsPostal = masterOrderNotification.ONTF_IsPostal;
                    newOrderNotification.ONTF_CreatedByID = loggedInUserId;
                    newOrderNotification.ONTF_CreatedOn = DateTime.Now;
                    newOrderNotification.ONTF_ParentNotificationID = masterOrderNotification.ONTF_ID; // Sets the first order notification as the parent.
                    newOrderNotification.ONTF_OrderNotificationTypeID = masterOrderNotification.ONTF_OrderNotificationTypeID;
                    newOrderNotification.ONTF_BkgPackageSvcGroupID = masterOrderNotification.ONTF_BkgPackageSvcGroupID;
                    newOrderNotification.ONTF_NotificationDetail = masterOrderNotification.ONTF_NotificationDetail;
                    if (masterOrderNotification.BkgOrderServiceForms.IsNotNull() && masterOrderNotification.BkgOrderServiceForms.Count > 0)
                    {
                        //Fetches the BkgOrderServiceForm for the given order notification ID.
                        BkgOrderServiceForm masterBkgOrderServiceForm = masterOrderNotification.BkgOrderServiceForms.FirstOrDefault(cond => cond.OSF_IsDeleted == false);
                        //Creates new row for the table BkgOrderServiceForm and sets the values from the current row in the newly added row to maintain history of the records.
                        BkgOrderServiceForm newBkgOrderServiceForm = new BkgOrderServiceForm();
                        newBkgOrderServiceForm.OSF_ServiceFormMappingID = masterBkgOrderServiceForm.OSF_ServiceFormMappingID;
                        newBkgOrderServiceForm.OSF_BkgOrderPackageSvcID = masterBkgOrderServiceForm.OSF_BkgOrderPackageSvcID;
                        newBkgOrderServiceForm.OSF_ServiceFormStatusID = masterBkgOrderServiceForm.OSF_ServiceFormStatusID;
                        newBkgOrderServiceForm.OrderNotification = newOrderNotification;
                        newBkgOrderServiceForm.OSF_IsDeleted = masterBkgOrderServiceForm.OSF_IsDeleted;
                        newBkgOrderServiceForm.OSF_CreatedBy = loggedInUserId;
                        newBkgOrderServiceForm.OSF_CreatedOn = DateTime.Now;
                        //if (oldStatusId != statusId)
                        //{
                        newBkgOrderServiceForm.OSF_OldServiceFormStatusID = oldStatusId == 0 ? (masterBkgOrderServiceForm.OSF_OldServiceFormStatusID == null ? masterBkgOrderServiceForm.OSF_ServiceFormStatusID : masterBkgOrderServiceForm.OSF_OldServiceFormStatusID) : oldStatusId;
                        newBkgOrderServiceForm.OSF_NewServiceFormStatusID = statusId == 0 ? (masterBkgOrderServiceForm.OSF_NewServiceFormStatusID == null ? masterBkgOrderServiceForm.OSF_ServiceFormStatusID : masterBkgOrderServiceForm.OSF_NewServiceFormStatusID) : statusId;
                        //}                        
                        newOrderNotification.BkgOrderServiceForms.Add(newBkgOrderServiceForm);
                        //Updates the current row in BkgOrderServiceForm with new status
                        masterBkgOrderServiceForm.OSF_ServiceFormStatusID = statusId == 0 ? masterBkgOrderServiceForm.OSF_ServiceFormStatusID : statusId;
                        masterBkgOrderServiceForm.OSF_ModifiedBy = loggedInUserId;
                        masterBkgOrderServiceForm.OSF_ModifiedOn = DateTime.Now;
                    }
                    return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).UpdateBkgOrderServiceFormStatus(newOrderNotification);

                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// UAT - 2702
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="backgroundPackageServiceId"></param>
        /// <param name="serviceTypeCode"></param>
        /// <returns></returns>
        public static OrderNotificationHistoryContract GetApplicantSpecificOrderNotificationHistory(Int32 orgUserID, Int32 tenantId)
        {
            try
            {
                OrderNotificationHistoryContract orderNotificationHistoryContract = null;
                var lstOrderNotificationDetail = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetApplicantSpecificOrderNotificationHistory(orgUserID);
                if (lstOrderNotificationDetail.IsNotNull())
                {
                    orderNotificationHistoryContract = new OrderNotificationHistoryContract();
                    orderNotificationHistoryContract.OrderNotificationDetailList = lstOrderNotificationDetail;
                }
                return orderNotificationHistoryContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region E Drug Screening Form

        public static String GetClearStarServiceId(Int32 tenantId, List<Int32> backgroundPackageServiceId, String serviceTypeCode)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetClearStarServiceIdAndExtVendorId(backgroundPackageServiceId, serviceTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 GetDPM_IDForEDSPackage(Int32 tenantId, List<Int32> backgroundPackageIds, String serviceTypeCode)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetDPM_IDForEDSPackage(backgroundPackageIds, serviceTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static String GetVendorAccountNumber(Int32 tenantId, Int32 extVendorId, Int32 DPM_ID)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetVendorAccountNumber(extVendorId, DPM_ID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<BkgAttributeGroupMapping> GetAttributeListByGroupId(Int32 tenantId, Int32 attributeGroupId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetAttributeListByGroupId(attributeGroupId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Entity.CustomFormAttributeGroup GetEDrugScreeningHtml(Int32 tenantId, Int32 customFormId, Int32 attributeGroupid)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetEDrugScreeningHtml(customFormId, attributeGroupid);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Entity.ZipCode GetZipCodeObjByZipCode(Int32 tenantId, String zipCode)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetZipCodeObjByZipCode(zipCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static String GetEDrugAttributeGroupId(Int32 tenantId, String eDrugSvcAttributeGrpCode)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetEDrugAttributeGroupId(eDrugSvcAttributeGrpCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<BkgOrderPackage> GetBackgroundPackageIdListByBkgOrderId(Int32 tenantId, Int32 bkgOrderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBackgroundPackageIdListByBkgOrderId(bkgOrderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static CustomFormDataGroup GetCustomFormDataGroupForEDSData(Int32 tenantId, Int32 bkgOrderId, String eDrugAttributegroupCode)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetCustomFormDataGroupForEDSData(bkgOrderId, eDrugAttributegroupCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<BkgAttributeGroupMapping> GetListBkgAttributeGroupMappingForEDrug(Int32 tenantId, String eDrugAttributeGroupName)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetListBkgAttributeGroupMappingForEDrug(eDrugAttributeGroupName);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean SaveCustomFormOrderDataForEDrug(Int32 tenantId, List<CustomFormOrderData> lstCustomFormOrderDataObj)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).SaveCustomFormOrderDataForEDrug(lstCustomFormOrderDataObj);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean UpdateBkgOrderSvcLineItem(Int32 tenantId, Int32 vendorId, Int32 bkgOrderId, String svcLineItemDisStatusCode, Int32 CurrentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).UpdateBkgOrderSvcLineItem(vendorId, bkgOrderId, svcLineItemDisStatusCode, CurrentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static String GetRegistrationIdAttributeName(Int32 tenantId, Guid registrationIdAttributeCode, Int32 attributeGroupId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetRegistrationIdAttributeName(registrationIdAttributeCode, attributeGroupId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static BkgOrder GetBkgOrderByOrderID(Int32 tenantId, Int32 masterOrderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgOrderByOrderID(masterOrderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateChanges(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).UpdateChanges();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean SaveWebCCFPDFDocument(Int32 tenantId, ApplicantDocument webCCfPdfDocument)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).SaveWebCCFPDFDocument(webCCfPdfDocument);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Int16 GetlkpRecordTypeIdByCode(Int32 tenantId, String recordTypeCode)
        {
            try
            {
                return LookupManager.GetLookUpData<Entity.ClientEntity.lkpRecordType>(tenantId).FirstOrDefault(x => x.IsDeleted == false && x.Code == recordTypeCode).RecordTypeID;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //Get List of states
        public static String GetStateNameByAbbreviation(Int32 tenantId, String stateAbbreviation)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetStateNameByAbbreviation(stateAbbreviation);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// This Parallel Task method is used to call the Handle Assignment method of queue Engine
        /// </summary>
        /// <param name="HandleAssignment">Is delegate that refer the Handle Assignment method of queue engine</param>
        /// <param name="handleAssignmentData">HandleAssignment data</param>
        /// <param name="loggerService">LoggerService (HttpContext.Current.ApplicationInstance of ISysXLoggerService )</param>
        /// <param name="exceptionService">ExceptionService (HttpContext.Current.ApplicationInstance of ISysXExceptionService)</param>
        public static void RunParallelTaskSaveCCFDataAndPDF(INTSOF.ServiceUtil.ParallelTaskContext.ParallelTask operation, Dictionary<String, Object> dicParam, ISysXLoggerService loggerService, ISysXExceptionService exceptionService, Int32 tenantId)
        {
            try
            {
                BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).RunParallelTaskSaveCCFDataAndPDF(operation, dicParam, loggerService, exceptionService);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        /// <summary>
        /// Gets the BOPId of the EDS Package, from all the BKGPackages
        /// </summary>
        /// <param name="lstBOPIds"></param>
        /// <returns></returns>
        public static Int32 GetEDSBkgOrderPkgId(List<Int32> lstBOPIds, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetEDSBkgOrderPkgId(lstBOPIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region D & R Document Entity Mapping

        public static List<DRDocsMappingContract> GetDRDocumentEntityMappingList(DRDocsMappingObjectIds docsEntityMappingFilters)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(SecurityManager.DefaultTenantID).GetDRDocumentEntityMappingList(docsEntityMappingFilters);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<LookupContract> GetAllDisclosureDocumentsList()
        {
            try
            {

                return new List<LookupContract>();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveUpdateDRDocumentsEntityMapping(DRDocsMappingObjectIds docsMappingObjectIds, Int32 loggedInUserId)
        {
            try
            {
                Entity.DisclosureDocumentMapping objDisclosureDocumentMapping = null;
                if (docsMappingObjectIds.DisclosureDocumentMappingId == 0)
                {
                    objDisclosureDocumentMapping = new Entity.DisclosureDocumentMapping();
                    objDisclosureDocumentMapping.DDM_CreatedBy = loggedInUserId;
                    objDisclosureDocumentMapping.DDM_CreatedOn = DateTime.Now;
                }
                else
                {
                    objDisclosureDocumentMapping = BALUtils.GetSecurityRepoInstance().GetDisclosureDocumentMappingById(docsMappingObjectIds.DisclosureDocumentMappingId);
                    if (objDisclosureDocumentMapping.IsNull())
                        return false;
                    objDisclosureDocumentMapping.DDM_ModifiedBy = loggedInUserId;
                    objDisclosureDocumentMapping.DDM_ModifiedOn = DateTime.Now;
                }

                if (docsMappingObjectIds.CountryId > 0)
                    objDisclosureDocumentMapping.DDM_CountryID = docsMappingObjectIds.CountryId;
                else
                    objDisclosureDocumentMapping.DDM_CountryID = null;

                if (docsMappingObjectIds.StateId > 0)
                    objDisclosureDocumentMapping.DDM_StateID = docsMappingObjectIds.StateId;
                else
                    objDisclosureDocumentMapping.DDM_StateID = null;

                if (docsMappingObjectIds.ServiceId > 0)
                    objDisclosureDocumentMapping.DDM_BackgroundServiceID = docsMappingObjectIds.ServiceId;
                else
                    objDisclosureDocumentMapping.DDM_BackgroundServiceID = null;

                if (docsMappingObjectIds.RegulatoryEntityTypeId > 0)
                    objDisclosureDocumentMapping.DDM_RegulatoryEntityTypeID = docsMappingObjectIds.RegulatoryEntityTypeId;
                else
                    objDisclosureDocumentMapping.DDM_RegulatoryEntityTypeID = null;

                if (docsMappingObjectIds.InstitutionHierarchyId > 0)
                    objDisclosureDocumentMapping.DDM_HierarchyNodeID = docsMappingObjectIds.InstitutionHierarchyId;
                else
                    objDisclosureDocumentMapping.DDM_HierarchyNodeID = null;

                if (docsMappingObjectIds.TenantId > 0)
                    objDisclosureDocumentMapping.DDM_TenantID = docsMappingObjectIds.TenantId;
                else
                    objDisclosureDocumentMapping.DDM_TenantID = null;

                objDisclosureDocumentMapping.DDM_DocumentID = docsMappingObjectIds.DocumentId;

                return BALUtils.GetSecurityRepoInstance().SaveUpdateDisclosureDocumentMapping(objDisclosureDocumentMapping);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteDRDocumentsEntityMapping(Int32 disclosureDocumentMappingId, Int32 loggedInUserId)
        {
            try
            {
                Entity.DisclosureDocumentMapping objDisclosureDocumentMapping = BALUtils.GetSecurityRepoInstance().GetDisclosureDocumentMappingById(disclosureDocumentMappingId);
                if (objDisclosureDocumentMapping.IsNotNull())
                {
                    objDisclosureDocumentMapping.DDM_IsDeleted = true;
                    objDisclosureDocumentMapping.DDM_ModifiedBy = loggedInUserId;
                    objDisclosureDocumentMapping.DDM_ModifiedOn = DateTime.Now;
                    return BALUtils.GetSecurityRepoInstance().SaveUpdateDisclosureDocumentMapping(objDisclosureDocumentMapping);
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean GetInstitution(Int32 disclosureDocumentMappingId, Int32 loggedInUserId)
        {
            try
            {
                Entity.DisclosureDocumentMapping objDisclosureDocumentMapping = BALUtils.GetSecurityRepoInstance().GetDisclosureDocumentMappingById(disclosureDocumentMappingId);
                if (objDisclosureDocumentMapping.IsNotNull())
                {
                    objDisclosureDocumentMapping.DDM_IsDeleted = true;
                    objDisclosureDocumentMapping.DDM_ModifiedBy = loggedInUserId;
                    objDisclosureDocumentMapping.DDM_ModifiedOn = DateTime.Now;
                    return BALUtils.GetSecurityRepoInstance().SaveUpdateDisclosureDocumentMapping(objDisclosureDocumentMapping);
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Supplement Service implementation

        /// <summary>
        /// Gets the Supplemental Service for a particular OrderPackageServiceGroup
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderPkgSvcGroupId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        //public static List<SupplementServicesInformation> GetSupplementServices(Int32 orderId, Int32 orderPkgSvcGroupId, Int32 tenantId)
        public static List<SupplementServicesInformation> GetSupplementServices(Int32 orderId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetSupplementServices(orderId);//, orderPkgSvcGroupId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<SupplementServiceItemInformation> GetSupplementServiceItem(Int32 tenantId, Int32 orderId, Int32 serviceId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetSupplementServiceItem(orderId, serviceId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<SupplementServiceItemCustomForm> GetListOfCustomFormsForSelectedItem(Int32 tenantId, String serviceItemId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetListOfCustomFormsForSelectedItem(serviceItemId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<SupplementServiceItemCustomForm> GetListOfCustomFormsForSelectedServices(Int32 tenantId, String packageServiceIds)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetListOfCustomFormsForSelectedServices(packageServiceIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }



        public static List<AttributesForCustomFormContract> GetListOfAttributesForSelectedItem(Int32 tenantId, Int32 customFormId, Int32 serviceItemId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetListOfAttributesForSelectedItem(customFormId, serviceItemId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static SourceServiceDetailForSupplement CheckSourceServicesForSupplement(Int32 tenantId, Int32 orderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).CheckSourceServicesForSupplement(orderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region MVR Fields in Personal Information Page
        public static List<AttributeFieldsOfSelectedPackages> GetMVRAttriGrpID(String packageIds, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetMVRAttriGrpID(packageIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Int32 GetCustomFormIDBYCode(String customFormCode)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(SecurityManager.DefaultTenantID).GetCustomFormIDBYCode(customFormCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        /// <summary>
        /// Generate Supplment order line items for a Service Group 
        /// </summary>
        /// <param name="supplementOrderData"></param>
        /// <param name="tenantId"></param>
        public static void GenerateSupplementOrder(SupplementOrderContract supplementOrderData, Int32 tenantId, List<Int32> lstPackageServiceIds)
        {
            try
            {
                supplementOrderData.OrderAdditionTypeId = LookupManager.GetLookUpData<lkpOrderAdditionType>(tenantId)
                                                          .FirstOrDefault(oAType => oAType.OAT_Code == OrderAdditionType.SUPPLEMENT_BACKGROUND_ORDER.GetStringValue()
                                                          && !oAType.OAT_IsDeleted).OAT_ID;

                supplementOrderData.OrderStatusIdPaid = LookupManager.GetLookUpData<lkpOrderStatu>(tenantId)
                                                          .FirstOrDefault(oStsType => oStsType.Code == ApplicantOrderStatus.Paid.GetStringValue()
                                                          && !oStsType.IsDeleted).OrderStatusID;

                supplementOrderData.OrderLineItemStatusId = LookupManager.GetLookUpData<lkpOrderLineItemStatu>(tenantId)
                                                             .Where(olists => olists.OLIS_Code == OrderLineItemStatusType.NEW.GetStringValue()
                                                             && !olists.OLIS_IsDeleted).FirstOrDefault().OLIS_ID;

                supplementOrderData.SvcLineItemDispatchStatusId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpSvcLineItemDispatchStatu>(tenantId)
                                                                 .FirstOrDefault(dispSts => dispSts.SLIDS_Code == SvcLineItemDispatchStatus.NOT_DISPATCHED.GetStringValue()
                                                                 && !dispSts.SLIDS_IsDeleted).SLIDS_ID;

                supplementOrderData.BkgOrderStatusTypeId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpOrderStatusType>(tenantId)
                                                                 .FirstOrDefault(dispSts => dispSts.Code == BackgroundOrderStatus.ADDITIONAL_WORK.GetStringValue())
                                                                 .OrderStatusTypeID;

                supplementOrderData.BkgOrderEventHistoryId = LookupManager.GetLookUpData<Entity.ClientEntity.lkpEventHistory>(tenantId)
                                                                 .FirstOrDefault(eh => eh.EH_Code == BkgOrderEvents.ORDER_UPDATED.GetStringValue())
                                                                 .EH_ID;
                Int32 paymentOptionId = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetPaymentTypeIdForOrder(supplementOrderData.OrderId);
                Boolean paymentTypeIsInvoice = false;
                List<lkpPaymentOption> paymentOption = LookupManager.GetLookUpData<Entity.ClientEntity.lkpPaymentOption>(tenantId).ToList();
                Int32 invoicePymntOptionId = paymentOption.FirstOrDefault(x => x.Code == PaymentOptions.InvoiceWithApproval.GetStringValue()).PaymentOptionID;
                Int32 invoiceWdoutApproval = paymentOption.FirstOrDefault(x => x.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()).PaymentOptionID;
                if (paymentOptionId == invoicePymntOptionId || paymentOptionId == invoiceWdoutApproval)
                { paymentTypeIsInvoice = true; }

                if (supplementOrderData.OrderPkgSvcGroupId > AppConsts.NONE)
                {
                    var _lkpReviewStatus = LookupManager.GetLookUpData<Entity.ClientEntity.lkpBkgSvcGrpReviewStatusType>(tenantId)
                                                                    .First(sgrs => sgrs.BSGRS_ReviewCode == BkgSvcGrpReviewStatusType.FIRST_REVIEW_COMPLETED.GetStringValue());
                    supplementOrderData.BkgSvcGrpReviewStatusTypeId = _lkpReviewStatus.BSGRS_ID;

                    supplementOrderData.BkgSvcGrpReviewStatusType = _lkpReviewStatus.BSGRS_ReviewStatusType;

                    lkpBkgSvcGrpStatusType _lkpBkgSvcGrpStatusType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpBkgSvcGrpStatusType>(tenantId)
                                                .First(sgrs => sgrs.BSGS_StatusCode == BkgSvcGrpStatusType.IN_PROGRESS.GetStringValue());
                    supplementOrderData.BkgSvcGrpStatusTypeId = _lkpBkgSvcGrpStatusType.BSGS_ID;

                    supplementOrderData.BkgSvcGrpStatusType = _lkpBkgSvcGrpStatusType.BSGS_StatusType;
                }

                BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GenerateSupplementOrder(supplementOrderData, paymentTypeIsInvoice, lstPackageServiceIds);
                StoredProcedureManagers.UpdateExtServiceVendorforLineItems(supplementOrderData.OrderId, tenantId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region Send Order Complete Mail

        /// <summary>
        /// Get OrganisationUser By OrderId
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="masterOrderId">Master Order Id</param>
        /// <returns>Organization User Profile</returns>
        public static OrganizationUser GetOrganisationUserByOrderId(Int32 tenantId, Int32 masterOrderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetOrganisationUserByOrderId(masterOrderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Int32 CheckIfOrderCompleteNotificationExistsByOrderID(Int32 tenantId, Int32 masterOrderId, String notificationType, Int32? packageServiceGroupID)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).CheckIfOrderCompleteNotificationExistsByOrderID(masterOrderId, notificationType, packageServiceGroupID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SendOrderCompleteResultMail(Int32 tenantId, OrganizationUser selectedUser, Int32 orderID, Int32 orderNotificationID, Int32 hierarchyNodeID, Int32 currentLoggedInUserId, Int32? svcGroupID, Int32? packageServiceGroupID, String svcGroupName, Boolean isClient, Boolean isEmployement, Int32 studenthierarchyNodeID, Boolean isOrderFlagged)
        {
            try
            {
                //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
                //List<OrganizationUserContract> lstClientAdmins = null;
                //if (isClient)
                //{
                //    lstClientAdmins = BackgroundProcessOrderManager.GetClientAdminWithNodePermission(tenantId, hierarchyNodeID);
                //}
                CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS;

                String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);

                List<Entity.lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();

                String docAttachmentTypeCode = DocumentAttachmentType.ORDER_COMPLETION_DOCUMENT.GetStringValue();
                Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                    Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

                //Create Dictionary for Mail And Message Data
                Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, isClient == true ? "Admin" : string.Concat(selectedUser.FirstName, " ", selectedUser.LastName));
                dictMailData.Add(EmailFieldConstants.APPLICATION_URL, string.Concat(applicationUrl));
                dictMailData.Add(EmailFieldConstants.SERVICE_GROUP_NAME, String.Concat(svcGroupName));
                dictMailData.Add(EmailFieldConstants.ORDER_NO, orderID);

                Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                if (isClient)
                {
                    mockData.UserName = "Admin";
                    mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
                    mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;
                }
                else
                {
                    mockData.UserName = string.Concat(selectedUser.FirstName, " ", selectedUser.LastName);
                    mockData.EmailID = selectedUser.PrimaryEmailAddress;
                    mockData.ReceiverOrganizationUserID = selectedUser.OrganizationUserID;
                }



                Entity.SystemCommunicationAttachment sysCommAttachment = new Entity.SystemCommunicationAttachment();

                var BGPkghierarchyNodeID = isClient ? hierarchyNodeID : studenthierarchyNodeID;
                String BGPkgPDFAttachementStatus = GetBGPkgPDFAttachementStatus(tenantId, BGPkghierarchyNodeID);
                if (BGPkgPDFAttachementStatus.Equals(PDFInclusionOptions.Excluded.GetStringValue()))
                {
                    if (svcGroupID == null)
                        commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS_WITHOUT_PDF_ATTACHMENT;
                    else
                        commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS_WITHOUT_PDF_ATTACHMENT;
                }
                else
                {
                    if (svcGroupID == null)
                        commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS;
                    else
                        commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS;
                }

                //UAT-3453
                String entityTypeCode = CommunicationEntityType.SCREENING_RESULT.GetStringValue();
                Int32 entityID = AppConsts.NONE;
                List<Entity.lkpScreeningResultType> lstLkpScreeningResultType = CommunicationManager.GetScreeningResultType();
                String screeningResultTypeCode = String.Empty;

                if (isOrderFlagged)
                {
                    screeningResultTypeCode = ScreeningResultType.Flagged.GetStringValue();
                    entityID = lstLkpScreeningResultType.Where(con => con.SRT_Code == screeningResultTypeCode && !con.SRT_IsDeleted).Select(sel => sel.SRT_ID).FirstOrDefault();
                }
                else
                {
                    screeningResultTypeCode = ScreeningResultType.Clear.GetStringValue();
                    entityID = lstLkpScreeningResultType.Where(con => con.SRT_Code == screeningResultTypeCode && !con.SRT_IsDeleted).Select(sel => sel.SRT_ID).FirstOrDefault();
                }


                if (!isEmployement && (!BGPkgPDFAttachementStatus.Equals(PDFInclusionOptions.Excluded.GetStringValue())))
                {
                    //Create Attachment
                    ParameterValue[] parameters;
                    if (svcGroupID != null)
                    {
                        if (!packageServiceGroupID.IsNullOrEmpty() && packageServiceGroupID > AppConsts.NONE)
                        {
                            parameters = new ParameterValue[5];
                        }
                        else
                        {
                            parameters = new ParameterValue[4];
                        }
                        parameters[3] = new ParameterValue();
                        parameters[3].Name = "PackageGroupID";
                        parameters[3].Value = svcGroupID.Value.ToString();//"2";
                        commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS;
                    }
                    else
                    {
                        if (!packageServiceGroupID.IsNullOrEmpty() && packageServiceGroupID > AppConsts.NONE)
                        {
                            parameters = new ParameterValue[4];
                        }
                        else
                        {
                            parameters = new ParameterValue[3];
                        }
                    }
                    if (!packageServiceGroupID.IsNullOrEmpty() && packageServiceGroupID > AppConsts.NONE)
                    {
                        var parmCount = parameters.Count();
                        parameters[parmCount - 1] = new ParameterValue();
                        parameters[parmCount - 1].Name = "BkgPkgSvcGrpID";
                        parameters[parmCount - 1].Value = packageServiceGroupID.ToString();
                    }

                    parameters[0] = new ParameterValue();
                    parameters[0].Name = "OrderID";
                    parameters[0].Value = orderID.ToString();
                    parameters[1] = new ParameterValue();
                    parameters[1].Name = "TenantID";
                    parameters[1].Value = tenantId.ToString(); // June
                    parameters[2] = new ParameterValue();
                    parameters[2].Name = "UserID";
                    parameters[2].Value = currentLoggedInUserId.ToString();

                    String reportName = "OrderCompletion";
                    String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                    String fileName = "OCR_" + tenantId.ToString() + "_" + orderID.ToString() + "_" + date + ".pdf";

                    byte[] reportContent = ReportManager.GetReportByteArray(reportName, parameters);

                    String retFilepath = ReportManager.ConvertByteArrayToReportFile(reportContent, fileName);

                    sysCommAttachment.SCA_OriginalDocumentID = -1;
                    sysCommAttachment.SCA_OriginalDocumentName = "OrderCompletedReport.pdf";
                    sysCommAttachment.SCA_DocumentPath = retFilepath;
                    sysCommAttachment.SCA_DocumentSize = reportContent.Length;
                    sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                    sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                    sysCommAttachment.SCA_IsDeleted = false;
                    sysCommAttachment.SCA_CreatedBy = currentLoggedInUserId;
                    sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                    sysCommAttachment.SCA_ModifiedBy = null;
                    sysCommAttachment.SCA_ModifiedOn = null;
                }
                //Send mail
                //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
                //Int32? newSystemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS, dictMailData, mockData, tenantId, hierarchyNodeID, null, null, true, lstClientAdmins);

                #region Code to generate report url place holder value

                Dictionary<String, String> reportUrlParameters = new Dictionary<String, String>
                                                         {
                                                            { "OrderID",  orderID.ToString()},
                                                             { "ReportType",  BackgroundReportType.ORDER_COMPLETION.GetStringValue()},
                                                             { "TenantId",  tenantId.ToString()},
                                                             {"HierarchyNodeID",Convert.ToString(hierarchyNodeID)},
                                                             {"IsReportSentToStudent",Convert.ToString(!isClient)},
                                                             {"OrganizationUserID",Convert.ToString(selectedUser.OrganizationUserID)}
                                                         };
                if (svcGroupID != null)
                {
                    reportUrlParameters.Add("PackageGroupID", svcGroupID.Value.ToString());
                    commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_SERVICE_GROUP_RESULTS;
                }

                StringBuilder reportUrl = new StringBuilder();
                reportUrl.Append(applicationUrl.Trim() + "?args=");
                reportUrl.Append(reportUrlParameters.ToEncryptedQueryString());

                #endregion


                dictMailData.Add(EmailFieldConstants.REPORT_URL, reportUrl.ToString());
                Int32? newSystemCommunicationID;

                //UAT-3453
                if (commSubEvent == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS_WITHOUT_PDF_ATTACHMENT || commSubEvent == CommunicationSubEvents.NOTIFICATION_FOR_COMPLETED_ORDER_RESULTS)
                    newSystemCommunicationID = CommunicationManager.SendPackageNotificationMail(commSubEvent, dictMailData, mockData, tenantId, hierarchyNodeID, entityID, entityTypeCode, true, true);
                else
                    newSystemCommunicationID = CommunicationManager.SendPackageNotificationMail(commSubEvent, dictMailData, mockData, tenantId, hierarchyNodeID, null, null, true, true);


                if (newSystemCommunicationID != null)
                {
                    if (!isEmployement && !BGPkgPDFAttachementStatus.Equals(PDFInclusionOptions.Excluded.GetStringValue()))
                    {
                        //Save Mail Attachment
                        sysCommAttachment.SCA_SystemCommunicationID = newSystemCommunicationID.Value;
                        Int32 sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                    }

                    if (orderNotificationID == 0)//&& !isClient
                    {
                        List<Entity.ClientEntity.lkpBusinessChannelType> businessChannelType = BackgroundProcessOrderManager.GetBusinessChannelType(tenantId);
                        List<Entity.ClientEntity.lkpOrderNotificationType> orderNotificationType = BackgroundProcessOrderManager.GetOrderNotificationType(tenantId);

                        String amsBusinessChannelCode = BusinessChannelType.AMS.GetStringValue();
                        Int16 businessChannelTypeID = businessChannelType.IsNotNull() && businessChannelType.Count > 0 ?
                            Convert.ToInt16(businessChannelType.FirstOrDefault(cond => cond.Code == amsBusinessChannelCode).BusinessChannelTypeID) : Convert.ToInt16(0);

                        String ordComDocumentTypeCode = OrderNotificationType.ORDER_RESULT.GetStringValue();
                        Int32 ordComDocumentTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
                            Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == ordComDocumentTypeCode).ONT_ID) : Convert.ToInt32(0);

                        OrderNotification ordNotification = new OrderNotification();
                        ordNotification.ONTF_OrderID = orderID;
                        ordNotification.ONTF_MSG_SystemCommunicationID = newSystemCommunicationID;
                        ordNotification.ONTF_MSG_MessageID = null;
                        ordNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
                        ordNotification.ONTF_IsPostal = false;
                        ordNotification.ONTF_CreatedByID = currentLoggedInUserId;
                        ordNotification.ONTF_CreatedOn = DateTime.Now;
                        ordNotification.ONTF_ModifiedByID = null;
                        ordNotification.ONTF_ModifiedDate = null;
                        ordNotification.ONTF_ParentNotificationID = null;
                        ordNotification.ONTF_OrderNotificationTypeID = ordComDocumentTypeID;
                        ordNotification.ONTF_BkgPackageSvcGroupID = packageServiceGroupID;
                        if (!String.IsNullOrEmpty(svcGroupName))
                        {
                            ordNotification.ONTF_NotificationDetail = "Complete Order Result for " + svcGroupName;
                        }
                        else
                        {
                            ordNotification.ONTF_NotificationDetail = "Complete Order Result for entire Service Group(s)";
                        }

                        Int32 ordNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenantId, ordNotification);
                        return true;
                    }
                    else if (orderNotificationID > 0)//&& !isClient
                    {
                        return BackgroundProcessOrderManager.UpdateOrderNotificationBkgOrderServiceForm(orderNotificationID, tenantId, currentLoggedInUserId, 0, 0, newSystemCommunicationID.Value, null);
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Disclosure And Release Form

        public static List<Entity.ClientEntity.ApplicantDocument> GetDisclosureReleaseDoc(Int32 tenantId, Int32 masterOrderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetDisclosureReleaseDoc(masterOrderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region Show EDS Document
        public static Boolean IsEdsServiceExitForOrder(Int32 tenantId, Int32 orderId, String serviceTypeCode)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).IsEdsServiceExitForOrder(orderId, serviceTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static ApplicantDocument GetApplicantDocumentForEds(Int32 tenantId, Int32 orderId, String documentTypeCode, String recordTypeCode)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetApplicantDocumentForEds(orderId, documentTypeCode, recordTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 GetSvcAttributeGroupIdByCode(Int32 tenantId, String eDrugSvcAttributeGrpCode)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetSvcAttributeGroupIdByCode(eDrugSvcAttributeGrpCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Show Residentail History Check

        /// <summary>
        /// Get the information for Residential History section (Attribute Group).
        /// Residential History section should not come at personal information page if no service in the selected packages required that.
        /// Returns Boolean Value. True: If residentaial History Attribute Group is mapped with Background Package Service Group, Show Residential History Section.
        /// Else returns false, Hide Residential History Section.
        /// </summary>
        /// <param name="backgorundPackagesID"></param>
        /// <returns></returns>
        public static List<PackageGroupContract> CheckShowResidentialHistory(Int32 tenantId, List<Int32> backgorundPackagesID)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).CheckShowResidentialHistory(backgorundPackagesID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Int32> GetBackGroundPackagesForOrderId(Int32 tenantId, Int32 orderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBackGroundPackagesForOrderId(orderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }



        #endregion

        #region Get Min and Max Ocuurance For Attibute Group

        public static Dictionary<String, Int32?> GetMinMaxOccurancesForAttributeGroup(Int32 tenantId, List<Int32> backgorundPackagesID, Guid attributeGrpCode)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetMinMaxOccurancesForAttributeGroup(backgorundPackagesID, attributeGrpCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region package level instruction text for Residential History
        public static Dictionary<Guid, String> ShowInstructionTextForResiHistory(Int32 tenantId, List<Int32> backgorundPackagesID)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).ShowInstructionTextForResiHistory(backgorundPackagesID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        /// <summary>
        /// Get All Client Admins having node permission
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="hierarchyNodeID"></param>
        /// <returns>List<OrganizationUserContract></returns>
        public static List<OrganizationUserContract> GetClientAdminWithNodePermission(Int32 tenantID, Int32 hierarchyNodeID)
        {
            try
            {
                return AssignClientAdminsToDataModel(BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).GetClientAdminWithNodePermission(tenantID, hierarchyNodeID));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Assign the datatable record in OrganizationUserContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List<OrganizationUserContract></returns>
        private static List<OrganizationUserContract> AssignClientAdminsToDataModel(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new OrganizationUserContract
                {
                    OrganizationUserId = x["OrganizationUserId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["OrganizationUserId"]),
                    FirstName = x["FirstName"].ToString(),
                    LastName = x["LastName"].ToString(),
                    FullName = x["FullName"].ToString(),
                    EmailAddress = x["EmailAddress"].ToString(),
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-586 WB: AMS: When adding supplemental services to an order, need to be able to see what the applicant has already entered (location, alias, etc.)
        public static List<AttributesForCustomFormContract> GetAttributeDataListForPreExistingSupplement(Int32 tenantId, Int32 groupId, Int32 masterOrderId, Int32 serviceItemId, Int32 serviceId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetAttributeDataListForPreExistingSupplement(groupId, masterOrderId, serviceItemId, serviceId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-777: As an applicant, I should be able to access the service form PDFs from my complio account

        /// <summary>
        /// To Get Automatic Services Forms for an Order.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderId"></param>
        /// <returns>List of ServiceFormContract</returns>
        public static List<ServiceFormContract> GetAutomaticServiceFormForOrder(Int32 tenantId, Int32 orderId)
        {
            try
            {
                return AssignValuesToServiceForm(BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetAutomaticServiceFormForOrder(orderId));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Assign the datatable record in ServiceFormContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List of ServiceFormContract</returns>
        private static List<ServiceFormContract> AssignValuesToServiceForm(DataTable table)
        {
            try
            {
                List<ServiceFormContract> lstServiceForm = new List<ServiceFormContract>();
                IEnumerable<DataRow> rows = table.AsEnumerable();
                if (rows.Count() > 0)
                {
                    lstServiceForm = rows.Select(x => new ServiceFormContract
                    {
                        ServiceFormName = x["ServiceFormName"].GetType().Name != "DBNull" ? Convert.ToString(x["ServiceFormName"]) : String.Empty,
                        ServiceName = x["ServiceName"].GetType().Name != "DBNull" ? Convert.ToString(x["ServiceName"]) : String.Empty,
                        DocumentPath = x["DocumentPath"].GetType().Name != "DBNull" ? Convert.ToString(x["DocumentPath"]) : String.Empty,
                        SystemDocumentID = x["SystemDocumentID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["SystemDocumentID"]) : 0,
                    }).ToList();
                }
                return lstServiceForm;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-807: Addition of a flagged report only notification

        /// <summary>
        /// To Get Background Flagged Order Result Completed Notification Data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderID"></param>
        /// <returns>List of BkgOrderNotificationDataContract</returns>
        public static List<BkgOrderNotificationDataContract> GetBkgFlaggedOrderResultCompletedNotificationData(Int32 tenantId, Int32 chunkSize)
        {
            try
            {
                return AssignBkgFlaggedOrderResultCompletedNotificationDataToDataModel(BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgFlaggedOrderResultCompletedNotificationData(chunkSize));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Assign the datatable record in BkgOrderNotificationDataContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List of BkgOrderNotificationDataContract</returns>
        private static List<BkgOrderNotificationDataContract> AssignBkgFlaggedOrderResultCompletedNotificationDataToDataModel(DataTable table)
        {
            try
            {
                List<BkgOrderNotificationDataContract> bkgOrderNotificationDataList = new List<BkgOrderNotificationDataContract>();

                IEnumerable<DataRow> rows = table.AsEnumerable();
                if (rows != null && rows.Count() > 0)
                {
                    foreach (var row in rows)
                    {
                        BkgOrderNotificationDataContract bkgOrderNotificationData = new BkgOrderNotificationDataContract();

                        bkgOrderNotificationData.OrderID = Convert.ToInt32(row["OrderID"]);
                        bkgOrderNotificationData.HierarchyNodeID = Convert.ToInt32(row["HierarchyNodeID"]);
                        bkgOrderNotificationData.BkgOrderID = Convert.ToInt32(row["BkgOrderID"]);
                        bkgOrderNotificationData.OrganizationUserID = Convert.ToInt32(row["OrganizationUserID"]);
                        bkgOrderNotificationData.FirstName = Convert.ToString(row["FirstName"]);
                        bkgOrderNotificationData.LastName = Convert.ToString(row["LastName"]);
                        bkgOrderNotificationData.PrimaryEmailAddress = Convert.ToString(row["PrimaryEmailAddress"]);
                        bkgOrderNotificationData.IsEmployment = Convert.ToBoolean(row["IsEmployment"]);

                        if (row.Table.Columns.Contains("OrderNumber"))
                            bkgOrderNotificationData.OrderNumber = row["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(row["OrderNumber"]);
                        bkgOrderNotificationDataList.Add(bkgOrderNotificationData);
                    }
                }

                return bkgOrderNotificationDataList;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-844 - Bkg Order Review details Screen
        public static BkgOrderPackageSvcGroup GetOrderPackageServiceGroupData(Int32 tenantID, int orderPkgSvcGroupID)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).GetOrderPackageServiceGroupData(orderPkgSvcGroupID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<lkpBkgSvcGrpReviewStatusType> GetServiceGroupReviewStatusList(Int32 tenantID)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpBkgSvcGrpReviewStatusType>(tenantID).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to get Service Group Review Status ID by Code
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="selectedSvcGroupReviewStatusTypeCode"></param>
        /// <returns></returns>
        public static Int32? GetSvcGroupReviewStatusTypeIdByCode(Int32 tenantID, String selectedSvcGroupReviewStatusTypeCode)
        {
            try
            {
                lkpBkgSvcGrpReviewStatusType reviewStatusType = LookupManager.GetLookUpData<lkpBkgSvcGrpReviewStatusType>(tenantID).FirstOrDefault(x => x.BSGRS_ReviewCode == selectedSvcGroupReviewStatusTypeCode && !x.BSGRS_IsDeleted);
                if (reviewStatusType != null)
                {
                    return reviewStatusType.BSGRS_ID;
                }
                return AppConsts.NONE;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        ///  Method to get Service Group Status ID by Code
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="searchDataContract"></param>
        /// <param name="gridCustomPaging"></param>
        /// <returns></returns>
        public static Int32? GetSvcGroupStatusTypeIdByCode(Int32 tenantID, String selectedSvcGroupStatusTypeCode)
        {
            try
            {
                lkpBkgSvcGrpStatusType statusType = LookupManager.GetLookUpData<lkpBkgSvcGrpStatusType>(tenantID).FirstOrDefault(x => x.BSGS_StatusCode == selectedSvcGroupStatusTypeCode && !x.BSGS_IsDeleted);
                if (statusType != null)
                {
                    return statusType.BSGS_ID;
                }
                return AppConsts.NONE;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        public static List<BkgOrderReviewQueueContract> GetBkgOrderReviewQueueData(Int32 tenantId, BkgOrderReviewQueueContract searchDataContract, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                DataTable tempDataTable = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgOrderReviewQueueData(searchDataContract, gridCustomPaging);
                return GetBkgOrderReviewQueueSearchData(tempDataTable);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<BkgOrderReviewQueueContract> GetBkgOrderReviewQueueSearchData(DataTable dataTable)
        {
            try
            {
                List<BkgOrderReviewQueueContract> lstBkgOrderReviewQueueContract = new List<BkgOrderReviewQueueContract>();
                IEnumerable<DataRow> rows = dataTable.AsEnumerable();
                lstBkgOrderReviewQueueContract = rows.Select(col =>
                      new BkgOrderReviewQueueContract
                      {
                          ApplicantFirstName = col["ApplicantFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ApplicantFirstName"]),
                          ApplicantLastName = col["ApplicantLastName"] == DBNull.Value ? String.Empty : Convert.ToString(col["ApplicantLastName"]),
                          CustomAttributes = col["CustomAttributes"] == DBNull.Value ? String.Empty : Convert.ToString(col["CustomAttributes"]),
                          HierarchyLabel = col["HierarchyLabel"] == DBNull.Value ? String.Empty : Convert.ToString(col["HierarchyLabel"]),
                          IsServiceGroupFlagged = col["IsServiceGroupFlagged"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(col["IsServiceGroupFlagged"]),
                          OrderCreatedDate = col["OrderCreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["OrderCreatedDate"]),
                          OrderID = Convert.ToInt32(col["OrderID"]),
                          OrderPackageSvcGrpID = Convert.ToInt32(col["OrderPackageSvcGrpID"]),
                          ReviewCriteria = col["ReviewCriteria"] == DBNull.Value ? String.Empty : Convert.ToString(col["ReviewCriteria"]),
                          SvcGrpLastUpdatedDate = col["SvcGrpLastUpdatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(col["SvcGrpLastUpdatedDate"]),
                          SvcgrpName = col["SvcgrpName"] == DBNull.Value ? String.Empty : Convert.ToString(col["SvcgrpName"]),
                          SvcGrpReviewStatusName = col["SvcGrpReviewStatusName"] == DBNull.Value ? String.Empty : Convert.ToString(col["SvcGrpReviewStatusName"]),
                          SvcGrpStatusName = col["SvcGrpStatusName"] == DBNull.Value ? String.Empty : Convert.ToString(col["SvcGrpStatusName"]),
                          TotalCount = Convert.ToInt32(col["TotalCount"]),
                          OrderNumber = col["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(col["OrderNumber"]),
                          SupplementAutomationStatusID = col["SupplementAutomationStatusID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["SupplementAutomationStatusID"]),
                      }).ToList();
                return lstBkgOrderReviewQueueContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static List<BkgReviewCriteria> GetAllReviewCriterias(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetAllReviewCriterias();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<lkpBkgSvcGrpReviewStatusType> GetAllSvcGrpReviewStatuses(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpBkgSvcGrpReviewStatusType>(tenantId).Where(cond => !cond.BSGRS_IsDeleted).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<lkpBkgSvcGrpStatusType> GetAllSvcGrpStatuses(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpBkgSvcGrpStatusType>(tenantId).Where(cond => !cond.BSGS_IsDeleted).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean AreServiceGroupsCompleted(Int32 tenantID, Int32 orderID)
        {
            try
            {
                Int32? serviceGroupCompletedID = GetSvcGroupStatusTypeIdByCode(tenantID, BkgSvcGrpStatusType.COMPLETED.GetStringValue());
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).AreServiceGroupsCompleted(orderID, serviceGroupCompletedID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<BkgAttributeGroupMapping> GetAllBkgAttributeGroupMapping(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetAllBkgAttributeGroupMapping();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT- 1159 WB: Add link to all Electronic service forms where the e-drug link is on the student dashboard.

        public static List<ServiceFormContract> GetAutomaticServiceFormForListOfOrders(int tenantID, string commaDelemittedOrderIDs)
        {
            try
            {
                return AssignValuesToServiceFormWithOrderID(BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).GetAutomaticServiceFormForListOfOrders(commaDelemittedOrderIDs));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Assign the datatable record in ServiceFormContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List of ServiceFormContract</returns>
        private static List<ServiceFormContract> AssignValuesToServiceFormWithOrderID(DataTable table)
        {
            try
            {
                List<ServiceFormContract> lstServiceForm = new List<ServiceFormContract>();
                IEnumerable<DataRow> rows = table.AsEnumerable();
                if (rows.Count() > 0)
                {
                    lstServiceForm = rows.Select(x => new ServiceFormContract
                    {
                        ServiceFormName = x["ServiceFormName"].GetType().Name != "DBNull" ? Convert.ToString(x["ServiceFormName"]) : String.Empty,
                        ServiceName = x["ServiceName"].GetType().Name != "DBNull" ? Convert.ToString(x["ServiceName"]) : String.Empty,
                        DocumentPath = x["DocumentPath"].GetType().Name != "DBNull" ? Convert.ToString(x["DocumentPath"]) : String.Empty,
                        SystemDocumentID = x["SystemDocumentID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["SystemDocumentID"]) : 0,
                        OrderID = x["OrderID"].GetType().Name != "DBNull" ? Convert.ToInt32(x["OrderID"]) : 0,
                    }).ToList();
                }
                return lstServiceForm;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-1177:System Updates for 613[Notification for Employment flag order]

        /// <summary>
        /// To Get Background Flagged Order Completed Employment Notification Data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderID"></param>
        /// <returns>List of BkgOrderNotificationDataContract</returns>
        public static List<BkgOrderNotificationDataContract> GetBkgFlaggedOrderEmploymentNotificationData(Int32 tenantId, Int32 chunkSize)
        {
            try
            {
                return AssignBkgFlaggedOrderCompletedEmploymentNotificationDataToDataModel(BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgFlaggedOrderEmploymentNotificationData(chunkSize));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Assign the Employment bkg Flag order record in BkgOrderNotificationDataContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List of BkgOrderNotificationDataContract</returns>
        private static List<BkgOrderNotificationDataContract> AssignBkgFlaggedOrderCompletedEmploymentNotificationDataToDataModel(DataTable table)
        {
            try
            {
                List<BkgOrderNotificationDataContract> bkgOrderNotificationDataList = new List<BkgOrderNotificationDataContract>();

                IEnumerable<DataRow> rows = table.AsEnumerable();
                if (rows != null && rows.Count() > 0)
                {
                    foreach (var row in rows)
                    {
                        BkgOrderNotificationDataContract bkgOrderNotificationData = new BkgOrderNotificationDataContract();

                        bkgOrderNotificationData.OrderID = Convert.ToInt32(row["OrderID"]);
                        bkgOrderNotificationData.HierarchyNodeID = Convert.ToInt32(row["HierarchyNodeID"]);
                        bkgOrderNotificationData.BkgOrderID = Convert.ToInt32(row["BkgOrderID"]);
                        bkgOrderNotificationData.OrganizationUserID = Convert.ToInt32(row["OrganizationUserID"]);
                        bkgOrderNotificationData.FirstName = Convert.ToString(row["FirstName"]);
                        bkgOrderNotificationData.LastName = Convert.ToString(row["LastName"]);
                        bkgOrderNotificationData.PrimaryEmailAddress = Convert.ToString(row["PrimaryEmailAddress"]);
                        bkgOrderNotificationData.ApplicantProfileAddress = row["ApplicantProfileAddress"].GetType().Name == "DBNull" ? String.Empty
                                                                           : Convert.ToString(row["ApplicantProfileAddress"]);
                        bkgOrderNotificationDataList.Add(bkgOrderNotificationData);
                    }
                }

                return bkgOrderNotificationDataList;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        /// <summary>
        /// Method is used to get the Clent Admin Service group Detail
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static List<ServiceLevelDetailsForOrderContract> GetServiceLevelDetailsForOrder(Int32 tenantId, Int32 orderID)
        {

            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetServiceLevelDetailsForOrder(orderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        #region UAT-1358: Complio Notification to applicant for PrintScan
        /// <summary>
        /// Method used to get is background service exist in order payment detail or not.
        /// </summary>
        /// <param name="tenantId">tenantid</param>
        /// <param name="orderPaymentDetailID">OrderPaymentDetailID</param>
        /// <param name="bkgServiceTypeCode">Servide type code</param>
        /// <returns></returns>
        public static Boolean IsBkgServiceExistInOrder(Int32 tenantId, Int32 orderPaymentDetailID, String bkgServiceTypeCode)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).IsBkgServiceExistInOrder(orderPaymentDetailID, bkgServiceTypeCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }


        #endregion

        #region  #region UAT-1455:Flag override should re-trigger data sync for the service group
        public static void RemoveDataSyncHistoryToRetriggerDataSync(Int32 tenantId, Int32 PSLI_ID, Int32 currentLoggedInUserID, Boolean isPackageFlaggedOverride)
        {

            try
            {
                BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).RemoveDataSyncHistoryToRetriggerDataSync(PSLI_ID, currentLoggedInUserID
                                                                                                                           , isPackageFlaggedOverride);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        public static Boolean GetBackGroundPackageFlaggedStatus(Int32 tenantId, Int32 PSLI_ID)
        {

            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBackGroundPackageFlaggedStatus(PSLI_ID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }
        #endregion


        public static List<BkgOrderNotificationDataContract> GetFlaggedEmploymentServiceGroupCompletedNotificationData(Int32 tenantId, Int32 chunkSize)
        {
            try
            {
                return AssignSvcGrpCompletedNotificationDataToDataModel(BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetFlaggedEmploymentServiceGroupCompletedNotificationData(chunkSize));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-1648:As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
        /// <summary>
        /// Get BkgOrderPackage List for BOPIDs
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="BOPIds">BOPIds</param>
        /// <returns>List of BkgOrderPackage</returns>
        public static List<BkgOrderPackage> GetBackgroundOrderPackageListById(Int32 tenantId, List<Int32> BOPIds)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBackgroundOrderPackageListById(BOPIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method is used to Get BkgOrderCustomFormAttributesData
        /// </summary>
        /// <param name="tenantId">tenantId</param>
        /// <param name="masterOrderId">masterOrderId</param>
        /// <param name="bopIds">bopIds</param>
        /// <param name="isIncludeMvrData">isIncludeMvrData</param>
        /// <returns>BkgOrderDetailCustomFormDataContract</returns>
        public static BkgOrderDetailCustomFormDataContract GetBkgORDCustomFormAttrDataForCompletingOrder(Int32 tenantId, Int32 masterOrderId, String bopIds,
                                                                                                         Boolean isIncludeMvrData)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgORDCustomFormAttrDataForCompletingOrder(masterOrderId, bopIds, isIncludeMvrData);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion
        public static List<BkgOrderNotificationDataContract> GetFlaggedServiceGroupCompletedNotificationData(Int32 tenantId, Int32 chunkSize)
        {
            try
            {
                return AssignSvcGrpCompletedNotificationDataToDataModel(BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetFlaggedSvcGrpResultCompletedNotificationData(chunkSize));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        #region UAT 1659 Notes section added on the background screening side.

        public static List<BkgOrderQueueNotesContract> GetBkgOrderNotes(Int32 tenantId, Int32 orderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgOrderNotes(orderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveBkgOrderNote(Int32 tenantId, Int32 orderId, String notes, Int32 loggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).SaveBkgOrderNote(orderId, notes, loggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        public static List<AttributeFieldsOfSelectedPackages> GetAttributeFieldsOfSelectedPackages(String packageIds, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetAttributeFieldsOfSelectedPackages(packageIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-1795 : Add D&A download button on Background Order Queue search.
        public static List<BkgOrderSearchQueueContract> GetAllDnADocument(Int32 tenantId, List<Int32> masterOrderIds)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetAllDnADocument(masterOrderIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-1852 : If a Service Group is complete, but other service groups are not complete within an order, the system should send the applicant an email.
        public static void SendingMailForBkgSvcGrpCompletion(Int32 tenantId, Int32 currentUserId)
        {
            try
            {
                DataTable serviceData = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).SendingMailForBkgSvcGrpCompletion();
                List<BkgServiceGroupMailContract> bkgsvcGrpData = ConvertDataTableToMailContrat(serviceData);
                if (!bkgsvcGrpData.IsNullOrEmpty())
                {
                    foreach (var bkgOrderId in bkgsvcGrpData)
                    {
                        SendSeriveGroupIncompleteStatusMail(bkgOrderId, tenantId, currentUserId);
                    }
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        private static List<BkgServiceGroupMailContract> ConvertDataTableToMailContrat(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new BkgServiceGroupMailContract
                {
                    ServiceGroupName = x["ServiceGroupName"].GetType().Name == "DBNull" ? String.Empty : x["ServiceGroupName"].ToString(),
                    ApplicantName = x["ApplicantName"].GetType().Name == "DBNull" ? String.Empty : x["ApplicantName"].ToString(),
                    PrimaryEmailaddress = x["PrimaryEmailaddress"].GetType().Name == "DBNull" ? String.Empty : x["PrimaryEmailaddress"].ToString(),
                    OrganizationUserId = x["OrganizationUserId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["OrganizationUserId"]),
                    OrderNumber = x["OrderNumber"] == DBNull.Value ? String.Empty : Convert.ToString(x["OrderNumber"]),
                    BkgOrderID = x["BkgOrderID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["BkgOrderID"]),
                    SelectedNodeID = x["SelectedNodeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["SelectedNodeID"])

                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        private static void SendSeriveGroupIncompleteStatusMail(BkgServiceGroupMailContract svcGrpDetails, Int32 tenantId, Int32 backgroundProcessUserId)
        {
            String ApplicantName = svcGrpDetails.ApplicantName;
            String PrimaryEmailaddress = svcGrpDetails.PrimaryEmailaddress;
            Int32 OrganizationUserId = svcGrpDetails.OrganizationUserId;
            String OrderNumber = svcGrpDetails.OrderNumber;
            String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
            String tenantName = SecurityManager.GetTenant(tenantId).TenantName;

            CommunicationSubEvents commSubEvent = CommunicationSubEvents.PENDING_SERVICE_GROUP_NOTIFICATION;

            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
            dictMailData.Add(EmailFieldConstants.APPLICANT_NAME, ApplicantName);
            dictMailData.Add(EmailFieldConstants.APPLICATION_URL, applicationUrl);
            dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
            dictMailData.Add(EmailFieldConstants.SERVICE_GROUP_NAME, svcGrpDetails.ServiceGroupName);
            dictMailData.Add(EmailFieldConstants.Order_Number, OrderNumber);

            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
            mockData.UserName = ApplicantName;
            mockData.EmailID = PrimaryEmailaddress;
            mockData.ReceiverOrganizationUserID = OrganizationUserId;

            CommunicationManager.SendMailForIncompleteServiceGroup(commSubEvent, mockData, dictMailData, tenantId, svcGrpDetails.SelectedNodeID);

            Int32 SubEvent = LookupManager.GetMessagingLookUpData<Entity.lkpCommunicationSubEvent>().Where(cond => cond.Code == commSubEvent.GetStringValue() && cond.IsDeleted == false)
                                .Select(condition => condition.CommunicationSubEventID).FirstOrDefault();
            Int32 bkgOrderID = svcGrpDetails.BkgOrderID;
            String entitySetName = "SendMailBkgSvcGrpCompletion";

            Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
            notificationDelivery.ND_OrganizationUserID = OrganizationUserId;
            notificationDelivery.ND_SubEventTypeID = SubEvent;
            notificationDelivery.ND_EntityId = bkgOrderID;
            notificationDelivery.ND_EntityName = entitySetName;
            notificationDelivery.ND_IsDeleted = false;
            notificationDelivery.ND_CreatedOn = DateTime.Now;
            notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
            //Delete notification delivery record if already exists for particular values.
            ComplianceDataManager.DeleteNotificationDeliveryListIfExist(notificationDelivery, tenantId, backgroundProcessUserId);
            ComplianceDataManager.AddNotificationDelivery(tenantId, notificationDelivery);
        }
        #endregion

        #region UAT-1996:Setting to allow Client admins the ability to edit color flags
        /// <summary>
        /// To update Order color flag Status
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="selectedOrderColorStatusId"></param>
        /// <param name="orderID"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public static Boolean UpdateOrderColorFlag(Int32 tenantId, Int32 selectedOrderColorStatusId, Int32 orderID, Int32 loggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).UpdateOrderColorFlag(selectedOrderColorStatusId, orderID, loggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        #endregion

        public static List<Entity.ClientEntity.ApplicantDocument> GetOrderAndBackgroundProfileRelatedDocuments(Int32 tenantId, Int32 masterOrderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetOrderAndBackgroundProfileRelatedDocuments(masterOrderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }
        public static Boolean UpdateOrderIntervalSearchRefOrderId(Int32 tenantId, Int32 newOrderId, Int32 refOrderId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).UpdateOrderIntervalSearchRefOrderId(newOrderId, refOrderId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static Int32 SaveAutoRecurringOrderHistory(Int32 tenantId, AutoRecurringOrderHistory autoRecurringOrderHistory)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).SaveAutoRecurringOrderHistory(autoRecurringOrderHistory);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdateAutoRecurringOrderHistory(Int32 tenantId, Int32 autoRecurringOrderHistoryId, DateTime? orderCompletionDate, Int32? newOrderId, String notes, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).UpdateAutoRecurringOrderHistory(autoRecurringOrderHistoryId, orderCompletionDate, newOrderId, notes, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-2062:System to determine and add additional searches in supplement (SSN Trace)
        public static List<SupplementAdditionalSearchContract> GetMatchedAdditionalSearchData(Int32 tenantId, String inputAdditionalSearchXML, Int32 masterOrderId)
        {
            try
            {
                return AssignMatchedAdditionalSearchDataToDataModel(BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId)
                                                                            .GetMatchedAdditionalSearchData(inputAdditionalSearchXML, masterOrderId));
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return new List<SupplementAdditionalSearchContract>();
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return new List<SupplementAdditionalSearchContract>();
            }
        }

        /// <summary>
        /// Assign the datatable record in SupplementAdditionalSearchContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List of SupplementAdditionalSearchContract</returns>
        private static List<SupplementAdditionalSearchContract> AssignMatchedAdditionalSearchDataToDataModel(DataTable table)
        {
            try
            {
                List<SupplementAdditionalSearchContract> SuppAdditionalSearchDataList = new List<SupplementAdditionalSearchContract>();
                if (!table.IsNullOrEmpty())
                {
                    IEnumerable<DataRow> rows = table.AsEnumerable();
                    if (rows != null && rows.Count() > 0)
                    {
                        foreach (var row in rows)
                        {
                            SupplementAdditionalSearchContract SuppAdditionalSearchData = new SupplementAdditionalSearchContract();
                            SuppAdditionalSearchData.FirstName = Convert.ToString(row["FirstName"]);
                            SuppAdditionalSearchData.LastName = Convert.ToString(row["LastName"]);
                            //UAT-2212:Addition of Alias Middle name that is required and has "no middle name"/"-----" functionality
                            SuppAdditionalSearchData.MiddleName = Convert.ToString(row["MiddleName"]);
                            SuppAdditionalSearchData.CountyName = Convert.ToString(row["County"]);
                            SuppAdditionalSearchData.StateAbbreviation = Convert.ToString(row["StateAbbreviation"]);
                            SuppAdditionalSearchData.StateName = Convert.ToString(row["StateName"]);

                            if (row.Table.Columns.Contains("IsNameUsedForSearch"))
                                SuppAdditionalSearchData.IsNameUsedForSearch = Convert.ToBoolean(row["IsNameUsedForSearch"]);

                            if (row.Table.Columns.Contains("IsLocationUsedForSearch"))
                                SuppAdditionalSearchData.IsLocationUsedForSearch = Convert.ToBoolean(row["IsLocationUsedForSearch"]);

                            SuppAdditionalSearchDataList.Add(SuppAdditionalSearchData);
                        }
                    }
                }

                return SuppAdditionalSearchDataList;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2117:"Continue" button behavior
        /// <summary>
        /// Method to check the background order to update(check the success indicator is applicable and all existing searchs of order are clear)
        /// </summary>
        /// <param name="tenantId">TenantID</param>
        /// <param name="masterOrderID">MasterOrderID</param>
        /// <returns>Truth values for Success indicator and existing searches clear or not and InstitutionColorFlagID</returns>
        public static Dictionary<String, String> CheckBackgroundOrderToUpdate(Int32 tenantId, Int32 masterOrderID, Int32 ordPackageSvcGrpID)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).CheckBackgroundOrderToUpdate(tenantId, masterOrderID, ordPackageSvcGrpID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2304: Random review of auto completed supplements

        /// <summary>
        /// Method to check the background order for all Svc Groups to update(check the success indicator is applicable and all existing searchs of order are clear)
        /// </summary>
        /// <param name="tenantId">TenantID</param>
        /// <param name="masterOrderID">MasterOrderID</param>
        /// <returns>Truth values for Success indicator and existing searches clear or not and InstitutionColorFlagID</returns>
        public static Dictionary<String, String> CheckBackgroundOrderForAllSvcGroupsToUpdate(Int32 tenantId, Int32 masterOrderID)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).CheckBackgroundOrderForAllSvcGroupsToUpdate(masterOrderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Save Supplement Automation Status and History
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderId"></param>
        /// <param name="supplementAutomationStatusList"></param>
        /// <param name="bkgSvcGrpStatusTypeCompletedID"></param>
        /// <param name="bkgSvcGrpReviewStatusTypeFirstReviewID"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public static Boolean SaveSupplementAutomationStatusAndHistory(Int32 tenantId, Int32 orderId, List<lkpSupplementAutomationStatu> supplementAutomationStatusList, Int32 bkgSvcGrpStatusTypeCompletedID, Int32 bkgSvcGrpReviewStatusTypeFirstReviewID, Int32 loggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).SaveSupplementAutomationStatusAndHistory(tenantId, orderId, supplementAutomationStatusList, bkgSvcGrpStatusTypeCompletedID, bkgSvcGrpReviewStatusTypeFirstReviewID, loggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Update Supplement Automation Status of order Pkg Svc Group
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderPkgSvcGroupID"></param>
        /// <param name="supplementAutomationReviewedStatusID"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public static Boolean UpdateSupplementAutomationStatus(Int32 tenantId, Int32 orderPkgSvcGroupID, Int32 supplementAutomationReviewedStatusID, Int32 loggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).UpdateSupplementAutomationStatus(orderPkgSvcGroupID, supplementAutomationReviewedStatusID, loggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Rollback Supplement Automation
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="orderID"></param>
        /// <param name="orderPkgSvcGroupID"></param>
        /// <param name="loggedInUserId"></param>
        /// <returns></returns>
        public static Boolean RollbackSupplementAutomation(Int32 tenantId, Int32 orderID, Int32 orderPkgSvcGroupID, Int32 loggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).RollbackSupplementAutomation(orderID, orderPkgSvcGroupID, loggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2399:If there are no red lines in SSN trace results and additional searches are added by the system, add the searches automatically without need for review.
        public static Dictionary<Int32, Boolean> CheckSupplementAutoSvcGrpForRandonReview(Int32 tenantID, String bkgOrderPackageSvcGroupIDs)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).CheckSupplementAutoSvcGrpForRandonReview(tenantID, bkgOrderPackageSvcGroupIDs);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return new Dictionary<Int32, Boolean>();
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                return new Dictionary<Int32, Boolean>();
            }
        }
        #endregion

        #region UAT-2319

        /// <summary>
        /// Method is used for sync data of those orders which are already purchased but data is copying after creation of mapping where item status of order is incomplete.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="currentLoggedInUserId"></param>
        public static void SyncDataForNewMapping(Int32 tenantId, Int32 currentLoggedInUserId)
        {
            try
            {
                List<PackageDocumentDataPointContract> _lstPkgDocDataPoints = AssignPackageDocumentDataPointContractToDataModel(BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetPackageDocumentDataPoints(currentLoggedInUserId));
                if (_lstPkgDocDataPoints.IsNotNull() && _lstPkgDocDataPoints.Count > 0)
                {
                    Boolean IsRecordInBkgCopyPackageData = false;
                    List<Int32> LstBkgCompliancePackageMappingID = _lstPkgDocDataPoints.Select(cmd => cmd.BkgCompliancePackageMappingID).Distinct().ToList();
                    List<Int32> LstItemID = _lstPkgDocDataPoints.Select(cmd => cmd.ItemID).Distinct().ToList();
                    List<Int32> LstMasterOrderID = _lstPkgDocDataPoints.Select(cmd => cmd.MasterOrderID).Distinct().ToList();

                    String fileName = String.Empty, orgFileName = String.Empty;
                    Dictionary<String, String> dataPointDocs = new Dictionary<String, String>();
                    List<Int32> psIDS = _lstPkgDocDataPoints.Select(col => col.PackageSubscriptionID).Distinct().ToList();
                    //UAT-1597
                    // List<PackageSubscriptionNotificationDataContract> lstOldPackageSubscription = GetPackageSubscriptionDataForNotification(tenantId, psIDS);

                    //UAT-1738:Create new attribute type for data-synced documents and update data sync procedure
                    List<lkpDocumentType> lstDocumentType = ComplianceDataManager.GetlkpDocumentType(tenantId);
                    // Get Document type id for 'Screening Document' type .
                    Int32 DocumentTypeId_ScreeningDocument = lstDocumentType.FirstOrDefault(cond =>
                                                                             cond.DMT_Code == DocumentType.SCREENING_DOCUMENT_ATTRIBUTE_TYPE_DOCUMENT.GetStringValue()
                                                                             && !cond.DMT_IsDeleted).DMT_ID;

                    List<lkpDataEntryDocumentStatu> dataEntryDocStatus = LookupManager.GetLookUpData<lkpDataEntryDocumentStatu>(tenantId);
                    Int16 dataEntryDocCompletedStatusId = dataEntryDocStatus.FirstOrDefault(cnd => cnd.LDEDS_Code == DataEntryDocumentStatus.COMPLETE.GetStringValue()
                                                          && !cnd.LDEDS_IsDeleted).LDEDS_ID;


                    short archieveStatusId = LookupManager.GetLookUpData<lkpArchiveState>(tenantId).FirstOrDefault(cond => cond.AS_Code == ArchiveState.Archived.GetStringValue()).AS_ID;
                    Int32 archiveStateIdOfCurrentSubscription = 0;
                    foreach (Int32 psID in psIDS)
                    {
                        archiveStateIdOfCurrentSubscription = _lstPkgDocDataPoints.Where(cond => cond.PackageSubscriptionID == psID).Select(x => x.ArchiveStateID).FirstOrDefault();

                        //prevent data to not copy from background to compliance if subscription archived.
                        if (archiveStateIdOfCurrentSubscription != archieveStatusId)
                        {
                            List<PackageDocumentDataPointContract> lstPkgDocDataPoints = _lstPkgDocDataPoints.Where(x => x.PackageSubscriptionID == psID).ToList();

                            Int32 packageSubscriptionID = lstPkgDocDataPoints[0].PackageSubscriptionID;

                            Int32 organizationUserId = lstPkgDocDataPoints[0].OrganizationUserID;

                            List<ApplicantDocument> lstAppDocuments = new List<ApplicantDocument>();
                            String docXml = null;

                            String svcGrpResDocCode = BkgDataPointType.SERVICE_GROUP_RESULT_DOCUMENT.GetStringValue();
                            String ordResDocCode = BkgDataPointType.ORDER_RESULT_DOCUMENT.GetStringValue();
                            List<String> docTypes = new List<string>();
                            docTypes.Add(svcGrpResDocCode);
                            docTypes.Add(ordResDocCode);

                            List<PackageDocumentDataPointContract> lstpkgDocumentDataPoint = lstPkgDocDataPoints.Where(x => docTypes.Contains(x.BkgDataPointTypeCode)).ToList();
                            if (lstpkgDocumentDataPoint != null && lstpkgDocumentDataPoint.Count > 0)
                            {
                                List<OrderResultDocMap> lstOrdResDocMap = new List<OrderResultDocMap>();
                                List<OrderResultDocMap> lstOrdResDocMapToBeUpdated = new List<OrderResultDocMap>();

                                docXml = "<documents>";
                                foreach (PackageDocumentDataPointContract pkgDocDataPoint in lstpkgDocumentDataPoint)
                                {
                                    //Code For UAT-1395 Changes
                                    String dataPointsDocsKey = String.Empty;
                                    if (pkgDocDataPoint.BkgDataPointTypeCode == BkgDataPointType.SERVICE_GROUP_RESULT_DOCUMENT.GetStringValue())
                                    {
                                        orgFileName = "BkgSvcGroupCompletionReport_" + pkgDocDataPoint.ServiceGroupName.ToString() + ".pdf";
                                        dataPointsDocsKey = Convert.ToString(pkgDocDataPoint.MasterOrderID) + "_" + Convert.ToString(pkgDocDataPoint.SGID);
                                    }
                                    else if (pkgDocDataPoint.BkgDataPointTypeCode == BkgDataPointType.ORDER_RESULT_DOCUMENT.GetStringValue())
                                    {
                                        orgFileName = "BkgOrderCompletionReport_" + pkgDocDataPoint.MasterOrderID.ToString() + ".pdf";
                                        dataPointsDocsKey = Convert.ToString(pkgDocDataPoint.MasterOrderID);
                                    }

                                    List<OrderResultDocMap> orderResultDocMap = BALUtils.GetComplianceDataRepoInstance(tenantId).GetOrderResultDocMapping(organizationUserId,
                                        pkgDocDataPoint.MasterOrderID, pkgDocDataPoint.SGID, pkgDocDataPoint.BkgDataPointTypeID);
                                    if (orderResultDocMap.IsNotNull() && orderResultDocMap.Count > 0)
                                    {
                                        //Commented below code for UAT-1395
                                        //if (!dataPointDocs.Contains(orgFileName))
                                        if (!dataPointDocs.ContainsKey(dataPointsDocsKey)
                                             || (dataPointDocs.ContainsKey(dataPointsDocsKey) && !dataPointDocs.ContainsValue(orgFileName))
                                            )
                                        {
                                            orderResultDocMap.ForEach(x =>
                                            {
                                                lstOrdResDocMapToBeUpdated.Add(x);
                                            });
                                        }
                                    }

                                    String sgid = pkgDocDataPoint.SGID.IsNotNull() ? pkgDocDataPoint.SGID.Value.ToString() : String.Empty;


                                    docXml = docXml + "<document>";
                                    docXml = docXml + "<BkgDataPointTypeID>" + pkgDocDataPoint.BkgDataPointTypeID.ToString() + "</BkgDataPointTypeID>";
                                    docXml = docXml + "<SGID>" + sgid + "</SGID>";
                                    //Implementation for UAT-1395:Data Sync Issues
                                    docXml = docXml + "<MasterOrderID>" + pkgDocDataPoint.MasterOrderID + "</MasterOrderID>";
                                    //UAT-1738
                                    //Set 'IsScreeningDocument' tag in document xml for further used in CopyData SP to map with 'Screening Document' attribute.
                                    docXml = docXml + "<IsScreeningDocument>" + pkgDocDataPoint.IsScreeningDocAttributeMapped + "</IsScreeningDocument>";
                                    //if ((orderResultDocMap.IsNull() || orderResultDocMap.Count == 0) || (orderResultDocMap.IsNotNull() && orderResultDocMap.Count > 0 && (prevOrganizationUserId != organizationUserId)))

                                    //Commented below code for UAT-1395
                                    //if (!dataPointDocs.Contains(orgFileName))
                                    if (!dataPointDocs.ContainsKey(dataPointsDocsKey)
                                        || (dataPointDocs.ContainsKey(dataPointsDocsKey) && !dataPointDocs.ContainsValue(orgFileName))
                                       )
                                    {
                                        dataPointDocs.Add(dataPointsDocsKey, orgFileName);
                                        Boolean aWSUseS3 = false;
                                        String filePath = WebConfigurationManager.AppSettings[AppConsts.APPLICANT_FILE_LOCATION];
                                        if (!WebConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                                        {
                                            aWSUseS3 = Convert.ToBoolean(WebConfigurationManager.AppSettings["AWSUseS3"]);
                                        }
                                        //Check whether use AWS S3, true if need to use
                                        if (aWSUseS3 == false)
                                        {
                                            if (!filePath.EndsWith("\\"))
                                            {
                                                filePath += "\\";
                                            }

                                            filePath += "Tenant(" + tenantId.ToString() + @")\";

                                            if (!Directory.Exists(filePath))
                                                Directory.CreateDirectory(filePath);
                                        }
                                        else
                                        {
                                            if (!filePath.EndsWith("//"))
                                            {
                                                filePath += "//";
                                            }

                                            filePath = filePath + "Tenant(" + tenantId.ToString() + @")/";
                                        }

                                        //Get Background Order Report
                                        ParameterValue[] parameters = new ParameterValue[2];
                                        if (pkgDocDataPoint.BkgDataPointTypeCode == BkgDataPointType.SERVICE_GROUP_RESULT_DOCUMENT.GetStringValue())
                                        {
                                            parameters = new ParameterValue[3];
                                            parameters[2] = new ParameterValue();
                                            parameters[2].Name = "PackageGroupID";
                                            parameters[2].Value = pkgDocDataPoint.SGID.ToString();
                                        }
                                        else if (pkgDocDataPoint.BkgDataPointTypeCode == BkgDataPointType.ORDER_RESULT_DOCUMENT.GetStringValue())
                                        {
                                            parameters = new ParameterValue[2];
                                        }
                                        parameters[0] = new ParameterValue();
                                        parameters[0].Name = "OrderID";
                                        parameters[0].Value = pkgDocDataPoint.MasterOrderID.ToString();
                                        parameters[1] = new ParameterValue();
                                        parameters[1].Name = "TenantID";
                                        parameters[1].Value = tenantId.ToString();



                                        //Get Report Content
                                        String reportName = "OrderCompletion";
                                        byte[] reportContent = ReportManager.GetReportByteArray(reportName, parameters);

                                        //Create Applicant Document
                                        ApplicantDocument applicantDocument = new ApplicantDocument();

                                        //Save Report in temporary Location
                                        String destFilePath = "Tenant(" + tenantId.ToString() + @")\";
                                        fileName = orgFileName;
                                        fileName = destFilePath + fileName;
                                        String returnFilePath = CommonFileManager.SaveDocument(reportContent, fileName, FileType.ApplicantFileLocation.GetStringValue());
                                        applicantDocument.DocumentPath = returnFilePath;
                                        applicantDocument.OrganizationUserID = pkgDocDataPoint.OrganizationUserID;
                                        applicantDocument.FileName = orgFileName;
                                        applicantDocument.Size = reportContent.Length;
                                        applicantDocument.Description = String.Empty;
                                        applicantDocument.CreatedByID = currentLoggedInUserId;
                                        applicantDocument.CreatedOn = DateTime.Now;
                                        applicantDocument.IsDeleted = false;
                                        //UAT-1738:Create new attribute type for data-synced documents and update data sync procedure
                                        if (pkgDocDataPoint.IsScreeningDocAttributeMapped)
                                        {
                                            applicantDocument.DocumentType = DocumentTypeId_ScreeningDocument;
                                            applicantDocument.DataEntryDocumentStatusID = dataEntryDocCompletedStatusId;
                                        }

                                        Int32 applicantDocumentId = ComplianceDataManager.AddApplicantDocument(applicantDocument, tenantId);
                                        String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                                        String newfileName = filePath + "UD_" + tenantId.ToString() + "_" + applicantDocumentId + "_" + date + ".pdf";
                                        ComplianceDataManager.UpdateDocumentPath(newfileName, returnFilePath, applicantDocumentId, tenantId, pkgDocDataPoint.OrganizationUserID);

                                        docXml = docXml + "<DocID>" + applicantDocumentId + "</DocID>";

                                        applicantDocument.DocumentPath = newfileName;
                                        applicantDocument.ApplicantDocumentID = applicantDocumentId;
                                        lstAppDocuments.Add(applicantDocument);

                                        OrderResultDocMap ordResDocMap = new OrderResultDocMap();
                                        ordResDocMap.ORDM_DocumentID = applicantDocumentId;
                                        ordResDocMap.ORDM_OrganizationUserID = organizationUserId;
                                        ordResDocMap.ORDM_MasterOrderID = pkgDocDataPoint.MasterOrderID;
                                        ordResDocMap.ORDM_ServiceGroupID = pkgDocDataPoint.SGID;
                                        ordResDocMap.ORDM_BkgDataPointTypeID = pkgDocDataPoint.BkgDataPointTypeID;
                                        ordResDocMap.ORDM_IsDeleted = false;
                                        ordResDocMap.ORDM_CreatedByID = currentLoggedInUserId;
                                        ordResDocMap.ORDM_CreatedOn = DateTime.Now;

                                        lstOrdResDocMap.Add(ordResDocMap);
                                    }
                                    else
                                    {
                                        docXml = docXml + "<DocID>" + orderResultDocMap[0].ORDM_DocumentID + "</DocID>";
                                    }
                                    docXml = docXml + "</document>";
                                }
                                docXml = docXml + "</documents>";

                                if (lstOrdResDocMap.IsNotNull() && lstOrdResDocMap.Count > 0)
                                    BALUtils.GetComplianceDataRepoInstance(tenantId).SaveOrderResultDocMap(lstOrdResDocMap);
                                if (lstOrdResDocMapToBeUpdated != null && lstOrdResDocMapToBeUpdated.Count > 0)
                                    BALUtils.GetComplianceDataRepoInstance(tenantId).UpdateOrderResultDocMap(lstOrdResDocMapToBeUpdated, currentLoggedInUserId);
                            }
                            String BkgCompliancePackageMappingIDs = String.Empty;
                            String ItemIDs = String.Empty;
                            String MasterOrderIDs = String.Empty;
                            if (!LstBkgCompliancePackageMappingID.IsNullOrEmpty())
                            {
                                BkgCompliancePackageMappingIDs = String.Join(",", LstBkgCompliancePackageMappingID);
                            }
                            if (!LstItemID.IsNullOrEmpty())
                            {
                                ItemIDs = String.Join(",", LstItemID);
                            }
                            if (!LstMasterOrderID.IsNullOrEmpty())
                            {
                                MasterOrderIDs = String.Join(",", LstMasterOrderID);
                            }
                            //List<Int32> lstAffectedCatDataIds = new List<Int32>();
                            //lstAffectedCatDataIds=BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).SyncDataForNewMapping(packageSubscriptionID, currentLoggedInUserId, docXml, tenantId, BkgCompliancePackageMappingIDs, ItemIDs, MasterOrderIDs);

                            IsRecordInBkgCopyPackageData = ComplianceDataManager.InsertRecordInBkgCopyPackageData(tenantId, packageSubscriptionID, docXml, currentLoggedInUserId);

                            if (lstAppDocuments != null && lstAppDocuments.Count > 0)
                            {
                                //Use Poco class so that Entity will not get updated while running parallel tasks
                                List<ApplicantDocumentPocoClass> lstApplicantDoc = new List<ApplicantDocumentPocoClass>();
                                foreach (var doc in lstAppDocuments)
                                {
                                    ApplicantDocumentPocoClass appDoc = new ApplicantDocumentPocoClass();
                                    appDoc.ApplicantDocumentID = doc.ApplicantDocumentID;
                                    appDoc.FileName = doc.FileName;
                                    appDoc.DocumentPath = doc.DocumentPath;
                                    appDoc.PdfDocPath = doc.PdfDocPath;
                                    appDoc.IsCompressed = doc.IsCompressed;
                                    appDoc.Size = doc.Size;
                                    lstApplicantDoc.Add(appDoc);
                                }

                                DocumentManager.ConvertApplicantDocumentToPDF(lstApplicantDoc, tenantId, currentLoggedInUserId);
                                ComplianceDataManager.MergeDocIntoUnifiedPdf(organizationUserId, tenantId, currentLoggedInUserId);
                            }
                            //RuleManager.ExecuteBusinessRules(psIDS, tenantId, currentLoggedInUserId, lstAffectedCatDataIds);
                        }

                    }
                    if (IsRecordInBkgCopyPackageData)
                    {
                        InsertSystemSeriveTrigger(currentLoggedInUserId, tenantId, LkpSystemService.BACKGROUND_COPY_PACKAGE_DATA);
                    }
                }
                //after sucessfully data sync the value of key is again set to zero in AppConfiguration table in security db.
                SecurityManager.UpdateAppConfiguration(AppConsts.Background_Data_Sync_In_Progress, AppConsts.ZERO);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

                //after sucessfully data sync the value of key is again set to zero in AppConfiguration table in security db.
                SecurityManager.UpdateAppConfiguration(AppConsts.Background_Data_Sync_In_Progress, AppConsts.ZERO);

                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);

                //after sucessfully data sync the value of key is again set to zero in AppConfiguration table in security db.
                SecurityManager.UpdateAppConfiguration(AppConsts.Background_Data_Sync_In_Progress, AppConsts.ZERO);

                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Assign the datatable record in PackageDocumentDataPointContract 
        /// </summary>
        /// <param name="table">table</param>
        /// <returns>List of PackageDocumentDataPointContract</returns>
        private static List<PackageDocumentDataPointContract> AssignPackageDocumentDataPointContractToDataModel(DataTable table)
        {
            try
            {
                List<PackageDocumentDataPointContract> lstPkgDocDataPoints = new List<PackageDocumentDataPointContract>();
                IEnumerable<DataRow> rows = table.AsEnumerable();
                if (rows != null && rows.Count() > 0)
                {
                    foreach (var row in rows)
                    {
                        PackageDocumentDataPointContract pkgDocDataPoint = new PackageDocumentDataPointContract();

                        pkgDocDataPoint.BkgDataPointTypeID = Convert.ToInt32(row["BkgDataPointTypeID"]);
                        pkgDocDataPoint.BkgDataPointTypeCode = Convert.ToString(row["BkgDataPointTypeCode"]);
                        if (row["SGID"].GetType().Name != "DBNull")
                            pkgDocDataPoint.SGID = Convert.ToInt32(row["SGID"]);
                        pkgDocDataPoint.MasterOrderID = Convert.ToInt32(row["MasterOrderID"]);
                        pkgDocDataPoint.OrganizationUserID = Convert.ToInt32(row["OrganizationUserID"]);
                        pkgDocDataPoint.ServiceGroupName = Convert.ToString(row["ServiceGroupName"]);
                        pkgDocDataPoint.PackageSubscriptionID = Convert.ToInt32(row["PackageSubscriptionID"]);
                        if (row["ArchiveStateID"].GetType().Name != "DBNull")
                            pkgDocDataPoint.ArchiveStateID = Convert.ToInt32(row["ArchiveStateID"]);
                        pkgDocDataPoint.IsScreeningDocAttributeMapped = row["IsScrDocumentAttMapped"].GetType().Name != "DBNull" ? Convert.ToBoolean(row["IsScrDocumentAttMapped"]) : false;

                        pkgDocDataPoint.BkgCompliancePackageMappingID = Convert.ToInt32(row["BCPM_ID"]);
                        pkgDocDataPoint.ItemID = Convert.ToInt32(row["BCPM_ComplianceItemID"]);

                        lstPkgDocDataPoints.Add(pkgDocDataPoint);
                    }
                }
                return lstPkgDocDataPoints;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// UAT-2370 : Supplement SSN Processing updates
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="backgroundProcessUserId"></param>
        /// <param name="BkgOrderID"></param>
        /// <param name="vendorBkgOrderLineItemDetailIDs"></param>
        public static void SendEmailWhenExceptionInSSNResult(Int32 tenantID, Int32 backgroundProcessUserId, Int32 BkgOrderID, String vendorBkgOrderLineItemDetailIDs)
        {
            List<BkgSSNExceptionNotificationDataContract> objBkgSSNExceptionNotificationDataContract = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).SendEmailWhenExceptionInSSNResult(BkgOrderID, vendorBkgOrderLineItemDetailIDs);
            if (!objBkgSSNExceptionNotificationDataContract.IsNullOrEmpty() && objBkgSSNExceptionNotificationDataContract.Count > 0)
            {

                Entity.AppConfiguration objAppConfiguration = SecurityManager.GetAppConfiguration(AppConsts.EMAILID_FOR_SSN_EXCEPTION);
                String EmailIDForSSNExcep = String.Empty;
                if (!objAppConfiguration.IsNullOrEmpty())
                {
                    EmailIDForSSNExcep = objAppConfiguration.AC_Value;
                }
                foreach (var NotificationData in objBkgSSNExceptionNotificationDataContract)
                {
                    String MaskedSSN = BackgroundSetupManager.GetMaskedSSN(NotificationData.SSN);
                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();

                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, NotificationData.ApplicantName);
                    dictMailData.Add(EmailFieldConstants.SSN, MaskedSSN);
                    dictMailData.Add(EmailFieldConstants.VENDOR_ORDER_NUMBER, NotificationData.VendorOrderLineItem);

                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                    mockData.EmailID = EmailIDForSSNExcep;
                    mockData.UserName = "Admin";

                    //Send mail
                    CommunicationSubEvents commSubEvent = CommunicationSubEvents.NOTIFICATION_FOR_SSN_TRACE_RE_QUEUED;

                    int? systemCommunicationID = CommunicationManager.SendPackageNotificationMail(commSubEvent, dictMailData, mockData, tenantID, NotificationData.SelectedNodeID, null, null, true);

                    Int32 subEventId = LookupManager.GetMessagingLookUpData<Entity.lkpCommunicationSubEvent>().Where(cond => cond.Code == commSubEvent.GetStringValue() && cond.IsDeleted == false).Select(condition => condition.CommunicationSubEventID).FirstOrDefault();
                    //Save Notification Delivery 
                    Entity.ClientEntity.NotificationDelivery notificationDelivery = new Entity.ClientEntity.NotificationDelivery();
                    notificationDelivery.ND_OrganizationUserID = NotificationData.OrganizationUserID;
                    notificationDelivery.ND_SubEventTypeID = subEventId;
                    notificationDelivery.ND_EntityId = NotificationData.VendorOrderLineItem;
                    notificationDelivery.ND_EntityName = "VendorLineItemOrderID";
                    notificationDelivery.ND_IsDeleted = false;
                    notificationDelivery.ND_CreatedOn = DateTime.Now;
                    notificationDelivery.ND_CreatedBy = backgroundProcessUserId;
                    ComplianceDataManager.AddNotificationDelivery(tenantID, notificationDelivery);
                }
            }
        }
        #endregion

        //UAT-2448
        public static List<CountryIdentificationDetailContract> GetCountryIdentificationDetails()
        {
            return BALUtils.GetSecurityRepoInstance().GetCountryIdentificationDetails();
        }

        #region UAT-2842-- Admin Create Screening Order

        public static List<AdminCreateOrderContract> GetAdminCreateOrderSearchData(Int32 tenantId, AdminOrderSearchContract searchContract, CustomPagingArgsContract gridCustomePaging)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetAdminCreateOrderSearchData(searchContract, gridCustomePaging);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-2842

        public static OrganizationUser GetOrganisationUserByUserID(Guid UserID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetOrganisationUserByUserID(UserID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<BackgroundPackagesContract> GetBkgPackageDetailsForAdminOrder(String DPMIds, Int32 tenantId)
        {
            try
            {
                DataTable table = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgPackageDetailsForAdminOrder(DPMIds);
                IEnumerable<DataRow> rows = table.AsEnumerable();
                return rows.Select(x => new BackgroundPackagesContract
                {
                    BPAId = Convert.ToInt32(x["BPAId"]),
                    BPAName = Convert.ToString(x["BPAName"]),
                    BPAViewDetails = Convert.ToBoolean(x["BPAViewDetails"]),
                    BPHMId = Convert.ToInt32(x["BPHMId"]),
                    IsExclusive = Convert.ToBoolean(x["IsExclusive"]),
                    BasePrice = Convert.ToDecimal(x["PackagePrice"]),
                    CustomPriceText = x["CustomPriceText"] != DBNull.Value ? Convert.ToString(x["CustomPriceText"]) : String.Empty,
                    MaxNumberOfYearforResidence = Convert.ToInt32(x["MaxNumberOfYearforResidence"]),
                    DisplayOrder = x["BPHM_Sequence"].IsNullOrEmpty() ? 0 : Convert.ToInt32(x["BPHM_Sequence"]),
                    PackageDetail = Convert.ToString(x["PackageDetail"]),
                    IsInvoiceOnlyAtPackageLevel = x["IsInvoiceOnlyAtPackageLevel"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(x["IsInvoiceOnlyAtPackageLevel"]),
                    DisplayNotesAbove = Convert.ToBoolean(x["DisplayNotesAbove"]),
                    DisplayNotesBelow = Convert.ToBoolean(x["DisplayNotesBelow"]),
                    InsitutionHierarchyNodeID = Convert.ToInt32(x["InsitutionHierarchyNodeID"]),
                }).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Int32> GetBkgSvcGroupDetailsByBkgPkgId(Int32 bkgPackageId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgSvcGroupDetailsByBkgPkgId(bkgPackageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<Int32> GetBackgroundServiceIdsBysvcGrpId(Int32 SvcGrpId, Int32 bkgPackageId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBackgroundServiceIdsBysvcGrpId(SvcGrpId, bkgPackageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static Order SaveAdminOrderDetails(Order orderDetails, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).SaveAdminOrderDetails(orderDetails);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Tuple<List<AdminCreateOrderContract>, List<BackgroundOrderData>> GetAdminOrderDetailsByOrderId(Int32 OrderID, Int32 tenantId)
        {
            try
            {
                DataSet ds = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetAdminOrderDetailsByOrderId(OrderID);

                IEnumerable<DataRow> rowsBkgOrderData = ds.Tables[1].AsEnumerable();
                var lstbkgOrderData = rowsBkgOrderData.Select(x => new
                {
                    BkgSvcAttributeGroupId = Convert.ToInt32(x["CFDG_BkgSvcAttributeGroupID"]),
                    CustomFormId = Convert.ToInt32(x["CFDG_CustomFormID"]),
                    InstanceId = x["CFDG_InstanceID"] != DBNull.Value ? Convert.ToInt32(x["CFDG_InstanceID"]) : AppConsts.NONE,
                    MappingID = x["CFOD_BkgAttributeGroupMappingID"] != DBNull.Value ? Convert.ToInt32(x["CFOD_BkgAttributeGroupMappingID"]) : AppConsts.NONE,
                    Value = x["CFOD_Value"] != DBNull.Value ? Convert.ToString(x["CFOD_Value"]) : String.Empty,
                    IsInternationalPhone = x["CFOED_IsInternationalPhone"] != DBNull.Value ? Convert.ToString(x["CFOED_IsInternationalPhone"]) : String.Empty
                }).ToList();


                List<BackgroundOrderData> lstBackgroundOrderData = new List<BackgroundOrderData>();
                var distnctBkgOrderData = lstbkgOrderData.DistinctBy(dst => new { dst.BkgSvcAttributeGroupId, dst.CustomFormId, dst.InstanceId }).ToList();
                foreach (var data in distnctBkgOrderData)
                {
                    BackgroundOrderData backgroundOrderData = new BackgroundOrderData();
                    backgroundOrderData.CustomFormData = new Dictionary<Int32, String>();
                    backgroundOrderData.CustomFormIntPhoneNumExtraData = new Dictionary<Int32, String>();

                    backgroundOrderData.BkgSvcAttributeGroupId = data.BkgSvcAttributeGroupId;
                    backgroundOrderData.CustomFormId = data.CustomFormId;
                    backgroundOrderData.InstanceId = data.InstanceId;

                    var lstCustomMappingData = lstbkgOrderData.Where(x => x.CustomFormId == data.CustomFormId && x.BkgSvcAttributeGroupId == data.BkgSvcAttributeGroupId && x.InstanceId == data.InstanceId);

                    foreach (var bkgOrderData in lstCustomMappingData)
                    {
                        if (bkgOrderData.MappingID > 0)
                        {
                            backgroundOrderData.CustomFormData.Add(bkgOrderData.MappingID, bkgOrderData.Value);
                            if (!bkgOrderData.IsInternationalPhone.IsNullOrEmpty())
                            {
                                backgroundOrderData.CustomFormIntPhoneNumExtraData.Add(bkgOrderData.MappingID, bkgOrderData.IsInternationalPhone);
                            }
                        }
                    }
                    lstBackgroundOrderData.Add(backgroundOrderData);
                }

                //BackgroundOrderData backgroundOrderData = new BackgroundOrderData();
                //backgroundOrderData.CustomFormData = new Dictionary<Int32, String>();
                //backgroundOrderData.CustomFormIntPhoneNumExtraData = new Dictionary<Int32, String>();
                //List<Int32> customFormIDs = lstbkgOrderData.Select(x => x.CustomFormId).Distinct().ToList<Int32>();
                //foreach (var customFormID in customFormIDs)
                //{
                //    var data = lstbkgOrderData.Where(x => x.CustomFormId == customFormID).FirstOrDefault();
                //    backgroundOrderData.BkgSvcAttributeGroupId = data.BkgSvcAttributeGroupId;
                //    backgroundOrderData.CustomFormId = data.CustomFormId;
                //    backgroundOrderData.InstanceId = data.InstanceId;
                //    foreach (var bkgOrderData in lstbkgOrderData.Where(x => x.CustomFormId == customFormID))
                //    {
                //        if (bkgOrderData.MappingID > 0)
                //        {
                //            backgroundOrderData.CustomFormData.Add(bkgOrderData.MappingID, bkgOrderData.Value);
                //            backgroundOrderData.CustomFormIntPhoneNumExtraData.Add(bkgOrderData.MappingID, bkgOrderData.IsInternationalPhone);
                //        }
                //    }
                //    lstBackgroundOrderData.Add(backgroundOrderData);
                //}

                IEnumerable<DataRow> rows = ds.Tables[0].AsEnumerable();
                List<AdminCreateOrderContract> lstAdminCreateOrderContract = rows.Select(x => new AdminCreateOrderContract
                {
                    OrderID = Convert.ToInt32(x["OrderID"]),
                    BKgOrderID = Convert.ToInt32(x["BkgOrderID"]),
                    UserID = x["UserID"].IsNotNull() ? Convert.ToString(x["UserID"]) : String.Empty,
                    BkgPackageHierarchyMappingID = x["BkgPackageHierarchyMappingID"] != DBNull.Value ? Convert.ToInt32(x["BkgPackageHierarchyMappingID"]) : AppConsts.NONE,
                    HierarchyNodeId = x["HierarchyNodeId"] != DBNull.Value ? Convert.ToInt32(x["HierarchyNodeId"]) : AppConsts.NONE,
                    SelectedNodeID = x["SelectedNodeID"] != DBNull.Value ? Convert.ToInt32(x["SelectedNodeID"]) : AppConsts.NONE,
                    NodeLabel = x["NodeLabel"] != DBNull.Value ? Convert.ToString(x["NodeLabel"]) : String.Empty,
                    AdminOrderStatusType = Convert.ToString(x["AdminOrderStatusType"]),
                    SSN = Convert.ToString(x["SSN"]),
                    AttestFCRAPrevisions = Convert.ToBoolean(x["AttestFCRAPrevisions"]),

                }).ToList();

                return new Tuple<List<AdminCreateOrderContract>, List<BackgroundOrderData>>(lstAdminCreateOrderContract, lstBackgroundOrderData);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Order GetAdminOrderDataByOrderId(Int32 OrderId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetAdminOrderDataByOrderId(OrderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Int32 GetDocumentTypeIdByCode(String docTypeCode, Int32 tenantId)
        {
            try
            {

                //List<lkpDocumentType> lstDocumentType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(tenantId).Where(x => x.DMT_IsDeleted == false).ToList();
                //if (docTypeCode == DocumentType.Disclosure_n_Release.GetStringValue())
                //{

                //}
                var documentType = LookupManager.GetLookUpData<lkpDocumentType>(tenantId).FirstOrDefault(x => x.DMT_Code == docTypeCode && !x.DMT_IsDeleted);
                if (documentType != null)
                {
                    return documentType.DMT_ID;
                }
                return AppConsts.NONE;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, Int32 ApplicantOrgUserID, String docTypeCode, Int32 tenantId)
        {
            try
            {
                Int32 ApplicantDocTypeId = GetDocumentTypeIdByCode(docTypeCode, tenantId);
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).IsDocumentAlreadyUploaded(documentName, documentSize, ApplicantOrgUserID, ApplicantDocTypeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsDandAAlreadyUploaded(Int32 ApplicantOrgUserID, Int32 docTypeId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).IsDandAAlreadyUploaded(ApplicantOrgUserID, docTypeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveApplicantDocumentDetails(Int32 tenantId, List<ApplicantDocument> lstApplicantDocument, Int32 currentLoggedInUserId, Int32 organizationUserProfileId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).SaveApplicantDocumentDetails(lstApplicantDocument);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveUpdateGenericDocumentmapping(Int32 tenantId, List<Int32> lstApplicantDocIds, Int32 currentLoggedInUserId, Int32 organizationUserProfileId)
        {
            try
            {
                String recordTypeCode = "AAAC";
                Int16 recordTypeId = GetlkpRecordTypeIdByCode(tenantId, recordTypeCode);
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).SaveUpdateGenericDocumentmapping(recordTypeId, lstApplicantDocIds, currentLoggedInUserId, organizationUserProfileId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<ApplicantDocumentContract> GetApplicantDocuments(Int32 tenantId, Int32 organizationUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetApplicantDocuments(organizationUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteApplicantDocuments(Int32 tenantId, Int32 organizationUserID, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).DeleteApplicantDocuments(organizationUserID, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }



        public static Boolean SaveCustomFormDetails(List<BackgroundOrderData> lstBkgOrderData, Int32 BkgOrderID, Int32 tenantId, Int32 currentLoggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).SaveCustomFormDetails(lstBkgOrderData, BkgOrderID, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteAdminOrderDetails(Int32 tenantId, Int32 currentLoggedInUserId, Int32 orderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).DeleteAdminOrderDetails(currentLoggedInUserId, orderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean DeleteOldOrganizationUserProfileData(Guid UserID, Int32 tenantId)
        {
            try
            {
                OrganizationUser OrgUserDetails = GetOrganisationUserByUserID(UserID, tenantId);
                BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).DeleteOldOrganizationUserProfileData(OrgUserDetails);
                return true;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static Boolean DeleteCustomFormData(Int32 BkgOrderID, Int32 currentLoggedInUserId, Int32 tenantId, String packageIDs)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).DeleteCustomFormData(BkgOrderID, currentLoggedInUserId, packageIDs);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-2842:(Transmit Order Functionality)
        public static Boolean TransmmitAdminOrders(Int32 tenantId, Int32 currentLoggedInUserId, List<Int32> orderIds)
        {
            try
            {

                Boolean response = false;
                response = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).TransmmitAdminOrders(tenantId, currentLoggedInUserId, orderIds);
                if (response)
                {
                    //Update External Service and Vendor for LineItems generated
                    orderIds.ForEach(ordId =>
                    {
                        StoredProcedureManagers.UpdateExtServiceVendorforLineItems(ordId, tenantId);
                    });
                    //Service Form Notification
                    var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                    var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                    Dictionary<String, Object> dicParam = new Dictionary<String, Object>();
                    dicParam.Add("TenantId", tenantId);
                    dicParam.Add("orderIds", String.Join(",", orderIds.ToList()));
                    dicParam.Add("CurrentLoggedInUserId", currentLoggedInUserId);
                    RunParallelTaskAdminOrderServiceFormNotification(CreateBkgOrderNotification, dicParam, LoggerService, ExceptiomService, tenantId);
                }
                return response;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean AdminOrderIsReadyToTransmit(Int32 tenantId, Int32 currentLoggedInUserId, String orderIds)
        {
            try
            {

                List<AdminOrderDetailReadyToTransmitContract> lstTempData = new List<AdminOrderDetailReadyToTransmitContract>();
                Boolean isOrdersReadyToTransmit = false;
                lstTempData = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).AdminOrderIsReadyToTransmit(tenantId, currentLoggedInUserId, orderIds);
                if (!lstTempData.IsNullOrEmpty())
                {
                    isOrdersReadyToTransmit = !lstTempData.Any(an => !an.IsOrderReadyToTransmit);
                }
                return isOrdersReadyToTransmit;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsAdminCreatedOrder(Int32 tenantId, Int32 orderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).IsAdminCreatedOrder(tenantId, orderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// This Parallel Task method is used to call the Handle Assignment method of queue Engine
        /// </summary>
        /// <param name="HandleAssignment">Is delegate that refer the Handle Assignment method of queue engine</param>
        /// <param name="handleAssignmentData">HandleAssignment data</param>
        /// <param name="loggerService">LoggerService (HttpContext.Current.ApplicationInstance of ISysXLoggerService )</param>
        /// <param name="exceptionService">ExceptionService (HttpContext.Current.ApplicationInstance of ISysXExceptionService)</param>
        public static void RunParallelTaskAdminOrderServiceFormNotification(INTSOF.ServiceUtil.ParallelTaskContext.ParallelTask operation, Dictionary<String, Object> dicParam, ISysXLoggerService loggerService, ISysXExceptionService exceptionService, Int32 tenantId)
        {
            try
            {
                BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).RunParallelTaskSaveCCFDataAndPDF(operation, dicParam, loggerService, exceptionService);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
            }
        }

        public static void CreateBkgOrderNotification(Dictionary<String, Object> dicParam)
        {
            try
            {
                Int32 tenant_Id;
                String orderIds;
                Int32 currentLoggedInUserId;

                if (dicParam.IsNotNull())
                {
                    orderIds = dicParam.GetValue("orderIds") as String;
                    dicParam.TryGetValue("CurrentLoggedInUserId", out currentLoggedInUserId);
                    dicParam.TryGetValue("TenantId", out tenant_Id);


                    String tenantName = SecurityManager.GetTenant(tenant_Id).TenantName;
                    String applicationUrl = WebSiteManager.GetInstitutionUrl(tenant_Id);
                    List<Entity.ClientEntity.lkpBusinessChannelType> businessChannelType = BackgroundProcessOrderManager.GetBusinessChannelType(tenant_Id);
                    List<Entity.ClientEntity.lkpServiceFormStatu> serviceFormStatus = BackgroundProcessOrderManager.GetServiceFormStatus(tenant_Id);
                    List<Entity.ClientEntity.lkpOrderNotifyStatu> orderNotifyStatus = BackgroundProcessOrderManager.GetOrderNotifyStatus(tenant_Id);
                    List<Entity.lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
                    List<Entity.ClientEntity.lkpOrderNotificationType> orderNotificationType = BackgroundProcessOrderManager.GetOrderNotificationType(tenant_Id);

                    String amsBusinessChannelCode = BusinessChannelType.AMS.GetStringValue();
                    Int16 businessChannelTypeID = businessChannelType.IsNotNull() && businessChannelType.Count > 0 ?
                        Convert.ToInt16(businessChannelType.FirstOrDefault(cond => cond.Code == amsBusinessChannelCode).BusinessChannelTypeID) : Convert.ToInt16(0);

                    String needToSendOrdSvcFormStsCode = ServiceFormStatus.NEED_TO_SEND.GetStringValue();
                    Int32 needToSendOrdSvcFormStsID = serviceFormStatus.IsNotNull() && serviceFormStatus.Count > 0 ?
                        Convert.ToInt32(serviceFormStatus.FirstOrDefault(cond => cond.SFS_Code == needToSendOrdSvcFormStsCode).SFS_ID) : Convert.ToInt32(0);

                    String sentToStudentOrdSvcFormStsCode = ServiceFormStatus.SENT_TO_STUDENT.GetStringValue();
                    Int32 sentToStudentOrdSvcFormStsID = serviceFormStatus.IsNotNull() && serviceFormStatus.Count > 0 ?
                        Convert.ToInt32(serviceFormStatus.FirstOrDefault(cond => cond.SFS_Code == sentToStudentOrdSvcFormStsCode).SFS_ID) : Convert.ToInt32(0);

                    String notifiedOrdNotifyStsCode = OrderNotifyStatus.NOTIFIED.GetStringValue();
                    Int32 notifiedOrdNotifyStsID = orderNotifyStatus.IsNotNull() && orderNotifyStatus.Count > 0 ?
                        Convert.ToInt32(orderNotifyStatus.FirstOrDefault(cond => cond.ONS_Code == notifiedOrdNotifyStsCode).ONS_ID) : Convert.ToInt32(0);

                    String errorOrdNotifyStsCode = OrderNotifyStatus.ERROR.GetStringValue();
                    Int32 errorOrdNotifyStsID = orderNotifyStatus.IsNotNull() && orderNotifyStatus.Count > 0 ?
                        Convert.ToInt32(orderNotifyStatus.FirstOrDefault(cond => cond.ONS_Code == errorOrdNotifyStsCode).ONS_ID) : Convert.ToInt32(0);

                    String docAttachmentTypeCode = DocumentAttachmentType.SYSTEM_DOCUMENT.GetStringValue();
                    Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                        Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

                    String svcFormDocumentTypeCode = OrderNotificationType.SERVICE_FORM_DOCUMENT.GetStringValue();
                    Int32 svcFormDocumentTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
                        Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == svcFormDocumentTypeCode).ONT_ID) : Convert.ToInt32(0);

                    String svcFormNotificationTypeCode = OrderNotificationType.SERVICE_FORM_NOTIFICATION.GetStringValue();
                    Int32 svcFormNotificationTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
                        Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == svcFormNotificationTypeCode).ONT_ID) : Convert.ToInt32(0);

                    String entityTypeCode = CommunicationEntityType.BACKGROUND_SERVICE_FORM.GetStringValue();

                    List<BkgOrderNotificationDataContract> bkgOrdreNotificationDataList = BackgroundProcessOrderManager.GetBkgOrderNotificationData(tenant_Id, AppConsts.NONE, orderIds);

                    if (bkgOrdreNotificationDataList.IsNotNull() && bkgOrdreNotificationDataList.Count > AppConsts.NONE)
                    {
                        Int32 ordNotificationID = 0, bkgOrderServiceFormID = 0;
                        Boolean isSucess = true;
                        Int32? systemCommunicationID = null;
                        Guid? messageID = null;
                        bkgOrdreNotificationDataList.Select(col => col.BkgOrderID).Distinct().ForEach(bkgOrderID =>
                        {
                            isSucess = true;

                            bkgOrdreNotificationDataList.Where(cond => cond.BkgOrderID == bkgOrderID).ForEach(condition =>
                            {
                                if (condition.SendAutomatically)
                                {
                                    //Create Dictionary for Messaging Contract
                                    Dictionary<String, object> dicMessageParam = new Dictionary<string, object>();
                                    dicMessageParam.Add("EntityID", condition.ServiceAtachedFormID);
                                    dicMessageParam.Add("EntityTypeCode", entityTypeCode);

                                    //Create Dictionary for Mail And Message Data
                                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                    dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(condition.FirstName, " ", condition.LastName));
                                    dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, condition.NodeHierarchy);
                                    dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, applicationUrl);
                                    dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                    dictMailData.Add(EmailFieldConstants.ORDER_NO, condition.OrderNumber);
                                    dictMailData.Add(EmailFieldConstants.ORDER_DATE, condition.OrderDate.ToShortDateString());
                                    dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, condition.PackageName);
                                    dictMailData.Add(EmailFieldConstants.SERVICE_NAME, condition.ServiceName);
                                    dictMailData.Add(EmailFieldConstants.SERVICE_FORM_NAME, condition.ServiceAtachedFormName);
                                    dictMailData.Add(EmailFieldConstants.SERVICE_FORM_DISPATCH_DATE, DateTime.Now.ToShortDateString());
                                    dictMailData.Add(EmailFieldConstants.SERVICE_GROUP_NAME, condition.ServiceGroupName);
                                    dictMailData.Add(EmailFieldConstants.ORDER_STATUS, condition.OrderStatus);

                                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                    mockData.UserName = string.Concat(condition.FirstName, " ", condition.LastName);
                                    mockData.EmailID = condition.PrimaryEmailAddress;
                                    mockData.ReceiverOrganizationUserID = condition.OrganizationUserID;

                                    //Send mail
                                    systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.DEFAULT_SVC_FORM, dictMailData, mockData, tenant_Id, condition.HierarchyNodeID, condition.ServiceAtachedFormID, entityTypeCode, true);

                                    systemCommunicationID = systemCommunicationID > 0 ? systemCommunicationID : null;
                                    //Save Mail Attachment
                                    if (condition.SystemDocumentID.HasValue)
                                    {
                                        Int32? sysCommAttachmentID = null;
                                        if (systemCommunicationID != null)
                                        {
                                            Entity.SystemCommunicationAttachment sysCommAttachment = new Entity.SystemCommunicationAttachment();
                                            sysCommAttachment.SCA_OriginalDocumentID = condition.SystemDocumentID.Value;
                                            sysCommAttachment.SCA_OriginalDocumentName = condition.DocumentName;
                                            sysCommAttachment.SCA_DocumentPath = condition.DocumentPath;
                                            sysCommAttachment.SCA_DocumentSize = condition.DocumentSize.HasValue ? condition.DocumentSize.Value : 0;
                                            sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                                            sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                                            sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                                            sysCommAttachment.SCA_IsDeleted = false;
                                            sysCommAttachment.SCA_CreatedBy = currentLoggedInUserId;
                                            sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                                            sysCommAttachment.SCA_ModifiedBy = null;
                                            sysCommAttachment.SCA_ModifiedOn = null;

                                            sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                                        }

                                        Dictionary<string, string> attachedFiles = new Dictionary<string, string>();
                                        List<Entity.ADBMessageDocument> messageDocument = new List<Entity.ADBMessageDocument>();

                                        Entity.ADBMessageDocument documentData = new Entity.ADBMessageDocument();
                                        documentData.DocumentName = condition.DocumentPath;
                                        documentData.OriginalDocumentName = condition.DocumentName;
                                        documentData.DocumentSize = condition.DocumentSize.Value;
                                        documentData.SystemCommunicationAttachmentID = sysCommAttachmentID;
                                        messageDocument.Add(documentData);

                                        attachedFiles = MessageManager.SaveDocumentAndGetDocumentId(messageDocument);
                                        if (!attachedFiles.IsNullOrEmpty())
                                        {
                                            String documentName = String.Empty;
                                            attachedFiles.ForEach(a => documentName += a.Key.ToString() + ";");

                                            dicMessageParam.Add("DocumentName", documentName);
                                        }
                                    }

                                    //Send Message
                                    messageID = CommunicationManager.SaveMessageContent(CommunicationSubEvents.DEFAULT_SVC_FORM, dictMailData, condition.OrganizationUserID, tenant_Id, dicMessageParam);
                                }
                                else
                                {
                                    systemCommunicationID = null;
                                    messageID = null;
                                }

                                OrderNotification ordNotification = new OrderNotification();
                                ordNotification.ONTF_OrderID = condition.OrderID;
                                ordNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
                                ordNotification.ONTF_MSG_MessageID = messageID;
                                ordNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
                                ordNotification.ONTF_IsPostal = condition.SendAutomatically == false ? true : false;
                                ordNotification.ONTF_CreatedByID = currentLoggedInUserId;
                                ordNotification.ONTF_CreatedOn = DateTime.Now;
                                ordNotification.ONTF_ModifiedByID = null;
                                ordNotification.ONTF_ModifiedDate = null;
                                ordNotification.ONTF_ParentNotificationID = null;
                                ordNotification.ONTF_OrderNotificationTypeID = condition.SendAutomatically == false ? svcFormNotificationTypeID : svcFormDocumentTypeID;
                                ordNotification.ONTF_NotificationDetail = "Service Form for " + condition.ServiceName;

                                ordNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenant_Id, ordNotification);
                                if (ordNotificationID == 0)
                                {
                                    isSucess = false;
                                }

                                //code added for UAT  - 787 Service Form history dropdown enhancements

                                else
                                {
                                    BkgOrderServiceForm bkgOrderServiceForm = new BkgOrderServiceForm();
                                    bkgOrderServiceForm.OSF_ServiceFormMappingID = condition.ServiceFormMappingID;
                                    bkgOrderServiceForm.OSF_BkgOrderPackageSvcID = condition.BkgOrderPackageSvcID;
                                    bkgOrderServiceForm.OSF_ServiceFormStatusID = condition.SendAutomatically == false ? needToSendOrdSvcFormStsID : sentToStudentOrdSvcFormStsID;
                                    bkgOrderServiceForm.OSF_OrderNotificationID = ordNotificationID;
                                    bkgOrderServiceForm.OSF_IsDeleted = false;
                                    bkgOrderServiceForm.OSF_CreatedBy = currentLoggedInUserId;
                                    bkgOrderServiceForm.OSF_CreatedOn = DateTime.Now;
                                    bkgOrderServiceForm.OSF_ModifiedBy = null;
                                    bkgOrderServiceForm.OSF_ModifiedOn = null;

                                    bkgOrderServiceFormID = BackgroundProcessOrderManager.CreateBkgOrderServiceForm(tenant_Id, bkgOrderServiceForm);

                                    if (bkgOrderServiceFormID == 0)
                                    {
                                        isSucess = false;
                                    }
                                    else
                                    {
                                        OrderNotification newOrdNotification = new OrderNotification();
                                        newOrdNotification.ONTF_OrderID = condition.OrderID;
                                        newOrdNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
                                        newOrdNotification.ONTF_MSG_MessageID = messageID;
                                        newOrdNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
                                        newOrdNotification.ONTF_IsPostal = condition.SendAutomatically == false ? true : false;
                                        newOrdNotification.ONTF_CreatedByID = currentLoggedInUserId;
                                        newOrdNotification.ONTF_CreatedOn = DateTime.Now;
                                        newOrdNotification.ONTF_ModifiedByID = null;
                                        newOrdNotification.ONTF_ModifiedDate = null;
                                        newOrdNotification.ONTF_ParentNotificationID = null;
                                        newOrdNotification.ONTF_OrderNotificationTypeID = condition.SendAutomatically == false ? svcFormNotificationTypeID : svcFormDocumentTypeID;
                                        newOrdNotification.ONTF_NotificationDetail = "Service Form for " + condition.ServiceName;
                                        newOrdNotification.ONTF_ParentNotificationID = ordNotificationID;
                                        Int32 newOrdNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenant_Id, newOrdNotification);
                                        if (newOrdNotificationID != 0)
                                        {
                                            BkgOrderServiceForm newBkgOrderServiceForm = new BkgOrderServiceForm();
                                            newBkgOrderServiceForm.OSF_ServiceFormMappingID = condition.ServiceFormMappingID;
                                            newBkgOrderServiceForm.OSF_BkgOrderPackageSvcID = condition.BkgOrderPackageSvcID;
                                            newBkgOrderServiceForm.OSF_ServiceFormStatusID = condition.SendAutomatically == false ? needToSendOrdSvcFormStsID : sentToStudentOrdSvcFormStsID;
                                            newBkgOrderServiceForm.OSF_NewServiceFormStatusID = condition.SendAutomatically == false ? needToSendOrdSvcFormStsID : sentToStudentOrdSvcFormStsID; ;
                                            newBkgOrderServiceForm.OSF_OldServiceFormStatusID = null;
                                            newBkgOrderServiceForm.OSF_OrderNotificationID = newOrdNotificationID;
                                            newBkgOrderServiceForm.OSF_IsDeleted = false;
                                            newBkgOrderServiceForm.OSF_CreatedBy = currentLoggedInUserId;
                                            newBkgOrderServiceForm.OSF_CreatedOn = DateTime.Now;
                                            newBkgOrderServiceForm.OSF_ModifiedBy = null;
                                            newBkgOrderServiceForm.OSF_ModifiedOn = null;

                                            Int32 newBkgOrderServiceFormID = BackgroundProcessOrderManager.CreateBkgOrderServiceForm(tenant_Id, newBkgOrderServiceForm);
                                        }
                                    }
                                }
                            });

                            BkgOrder bkgOrd = new BkgOrder();
                            bkgOrd.BOR_ID = bkgOrderID;
                            bkgOrd.BOR_OrderNotifyStatusID = isSucess == true ? notifiedOrdNotifyStsID : errorOrdNotifyStsID;
                            bkgOrd.BOR_ModifiedByID = currentLoggedInUserId;
                            bkgOrd.BOR_ModifiedOn = DateTime.Now;

                            BackgroundProcessOrderManager.UpdateBkgOrderNotifyStatus(tenant_Id, bkgOrd);
                        });

                    }
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Dictionary<String, Boolean> CheckOrderAvailabilityForTrasmit(String OrderIds, Int32 tenantId)
        {
            try
            {
                DataTable table = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).CheckOrderAvailabilityForTrasmit(OrderIds);
                IEnumerable<DataRow> rows = table.AsEnumerable();
                Dictionary<String, Boolean> result = new Dictionary<String, Boolean>();
                foreach (var x in rows)
                {
                    Boolean Status = Convert.ToBoolean(x["StatusID"]);
                    String OrderId = x["OrderNumber"].IsNotNull() ? Convert.ToString(x["OrderNumber"]) : String.Empty;
                    result.Add(OrderId, Status);
                }
                return result;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        //UAT-2154
        public static BkgOrder GetBkgOrderByBkgOrderId(Int32 tenantId, Int32 BkgOrderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgOrderByBkgOrderId(BkgOrderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion


        #region UAT:-2587:- Pop up for backgroung orders that will have multiple emails sent.

        public static List<BackroundServicesContract> AcknowledgeMessagePopUpContent(Int32 tenantId, String bkgPackageIds, Int32 selectedNodeId)
        {
            //pass ids to sp and call sp, return content

            return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).AcknowledgeMessagePopUpContent(bkgPackageIds, selectedNodeId);

        }

        #endregion

        #region UAT-3268:- Manage Additional Fee for Background package needed to Qualify for rotation.

        public static List<PkgAdditionalPaymentInfo> GetAdditionalPriceData(List<Int32> lstBkgHierarchyPkgId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetAdditionalPriceData(lstBkgHierarchyPkgId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<OrderPaymentDetail> GetAdditionalPaymentModes(List<Int32> lstOpdIds, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetAdditionalPaymentModes(lstOpdIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-3610
        public static void InsertSystemSeriveTrigger(Int32 currentUserId, Int32 tenantId, String systemServiceCode)
        {
            try
            {
                Entity.lkpSystemService reOccurRuleService = SecurityManager.GetSystemServiceByCode(systemServiceCode);
                Entity.SystemServiceTrigger systemServiceTrigger = new Entity.SystemServiceTrigger();
                if (reOccurRuleService != null)
                    systemServiceTrigger.SST_SystemServiceID = reOccurRuleService.SS_ID;
                systemServiceTrigger.SST_TenantID = tenantId;
                systemServiceTrigger.SST_IsActive = true;
                systemServiceTrigger.SST_CreatedByID = currentUserId;
                systemServiceTrigger.SST_CreatedOn = DateTime.Now;
                SecurityManager.AddSystemServiceTrigger(systemServiceTrigger);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-3453
        public static Boolean IsBkgOrderFlagged(Int32 tenantId, Int32 masterOrderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).IsBkgOrderFlagged(masterOrderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT 3521

        public static Dictionary<String, List<String>> GetDataForCascadingDropDown(Int32 tenantId, String searchId, Int32 AtrributeGroupId, Int32 AttributeID)
        {
            return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetDataForCascadingDropDown(searchId, AtrributeGroupId, AttributeID);
        }

        public static List<String> GetDataForBindingCascadingDropDown(Int32 tenantID, Int32 attributeGroupID, Int32 attributeId, String searchID)
        {
            return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).GetDataForBindingCascadingDropDown(attributeGroupID, attributeId, searchID);
        }

        #endregion

        public static String ValidatePageData(Int32 tenantID, StringBuilder xmlStringData, Boolean IsCustomFormScreen, string languageCode = "")
        {
            try
            {
                if (String.IsNullOrEmpty(languageCode))
                {
                    languageCode = Languages.ENGLISH.GetStringValue();
                }

                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).ValidatePageData(xmlStringData, IsCustomFormScreen, languageCode);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        #region UAT-3669

        public static void SendAlertMailForWebCCFError(Int32 backgroundProcessUserId)
        {
            try
            {
                String BlockedOrderReasonCode = lkpBlockedOrderReason.WebCCF_Didnot_Supplied_Registration_Id.GetStringValue();
                DataTable serviceData = BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(SecurityManager.DefaultTenantID).GetAlertMailDataForWebCCFError(backgroundProcessUserId, BlockedOrderReasonCode);
                AlertMailForBlockedOrderContract alertMailForBlockedOrderData = ConvertDataTableToAlertMailContract(serviceData);

                if (!alertMailForBlockedOrderData.IsNullOrEmpty())
                {
                    #region SEND MAIL

                    Entity.AppConfiguration objAppConfiguration = SecurityManager.GetAppConfiguration(AppConsts.IT_Helpdesk_EmailId);
                    String emailIdITHelpDesk = String.Empty;
                    if (!objAppConfiguration.IsNullOrEmpty())
                    {
                        emailIdITHelpDesk = objAppConfiguration.AC_Value;
                    }


                    String tenantName = alertMailForBlockedOrderData.TenantName;
                    Int32 MaxImpactCount = alertMailForBlockedOrderData.MaxImpactCount;
                    Int32 MaxImpactForTenant = alertMailForBlockedOrderData.MaxImpactForTenant;
                    String StartDate = alertMailForBlockedOrderData.StartDate.Value.IsNullOrEmpty() ? String.Empty : alertMailForBlockedOrderData.StartDate.ToString();
                    String EndDate = alertMailForBlockedOrderData.EndDate.Value.IsNullOrEmpty() ? String.Empty : alertMailForBlockedOrderData.EndDate.ToString();


                    CommunicationSubEvents commSubEvent = CommunicationSubEvents.IT_NOTIFICATION_EDS_REGISTRATION_ID_BLANK;

                    Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                    dictMailData.Add(EmailFieldConstants.EVENT_START_DATE, StartDate);
                    dictMailData.Add(EmailFieldConstants.EVENT_END_DATE, EndDate);
                    dictMailData.Add(EmailFieldConstants.Institution_With_Max_Impact, tenantName);
                    dictMailData.Add(EmailFieldConstants.MAX_IMPACT_COUNT, MaxImpactCount);
                    dictMailData.Add(EmailFieldConstants.Impact_Count, MaxImpactForTenant);

                    Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                    mockData.EmailID = emailIdITHelpDesk;
                    mockData.UserName = "Admin";

                    CommunicationManager.SendPackageNotificationMail(commSubEvent, dictMailData, mockData, AppConsts.NONE, AppConsts.NONE, null, null, true);

                    #endregion
                }
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static AlertMailForBlockedOrderContract ConvertDataTableToAlertMailContract(DataTable table)
        {
            try
            {
                IEnumerable<DataRow> rows = table.AsEnumerable();

                return rows.Select(x => new AlertMailForBlockedOrderContract
                {
                    MaxImpactCount = x["TotalActiveCount"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(x["TotalActiveCount"]),
                    TenantName = x["TenantName"].GetType().Name == "DBNull" ? String.Empty : x["TenantName"].ToString(),
                    MaxImpactForTenant = x["MaxImpactForTenant"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(x["MaxImpactForTenant"]),
                    StartDate = x["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(x["StartDate"]),
                    EndDate = x["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(x["EndDate"])

                }).FirstOrDefault();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        public static List<CustomFormAutoFillDataContract> GetConditionsforAttributes(Int32 tenantID, StringBuilder xmlStringData, String languageCode = "")
        {
            if (String.IsNullOrEmpty(languageCode))
            {
                languageCode = Languages.ENGLISH.GetStringValue();
            }

            return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).GetConditionsforAttributes(xmlStringData, languageCode);
        }

        public static bool SaveCustomFormApplicantData(Int32 tenantID, string xmlStringData, int applicantOrganisationId, int currentLoggedInUserId)
        {
            return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).SaveCustomFormApplicantData(xmlStringData, applicantOrganisationId, currentLoggedInUserId);
        }

        public static List<LookupContract> GetCustomAttributeOptionsData(Int32 tenantID, String attributeName)
        {
            return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).GetCustomAttributeOptionsData(attributeName);
        }

        #region UAT-3820
        /// <summary>
        /// Method is used to get the DataForReceivedFromStudentServiceFormStatus
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static DataTable GetDataForReceivedFromStudentServiceFormStatus(String TenantIDs, Int32 serviceFormStatusLimit)
        {

            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(SecurityManager.DefaultTenantID).GetDataForReceivedFromStudentServiceFormStatus(TenantIDs, serviceFormStatusLimit);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }
        #endregion

        #region Mobile API

        public static List<Entity.ClientEntity.BkgPackageSvcGroup> GetBkgSvcGroupByBkgPkgId(Int32 bkgPackageId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetBkgSvcGroupByBkgPkgId(bkgPackageId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-3745

        public static List<SystemDocBkgSvcMapping> GetApplicantDocsMappedWithSvc(Int32 tenantId, Int32 orderId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetApplicantDocsMappedWithSvc(orderId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region 4004 :- Clear Start profile linking.

        public static List<VendorProfileSvcLineItemContract> GetLineItemsDataforOrderID(Int32 tenantID, Int32 orderID)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).GetLineItemsDataforOrderID(orderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SaveUpdateSvcLineItemMapping(Int32 tenantID, Int32 currentLoggedInUserId, VendorProfileSvcLineItemContract vendorProfileSvcLineItemData)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).SaveUpdateSvcLineItemMapping(currentLoggedInUserId, vendorProfileSvcLineItemData);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static List<VendorProfileSvcLineItemContract> GetSvcLineItemsCreated(Int32 tenantID, Int32 orderID)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).GetSvcLineItemsCreated(orderID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region UAT-4162 :- Apply retry logic while pulling DS Registration details from ClearStar

        /// <summary>
        /// Get Drug screening orders whose data was not pulled from clearstar when order status get paid.
        /// </summary>
        /// <returns></returns>
        public static List<VendorProfileSvcLineItemContract> GetDSOrderToGetCSData(Int32 tenantId, Int32 chunkSize, Int32 maxRetryCount, Int32 retryTimeLag)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).GetDSOrderToGetCSData(chunkSize, maxRetryCount, retryTimeLag);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method to update drug screening orders retry count to get data from clearstar.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="bkgSvcLineItemId"></param>
        /// <returns></returns>
        public static Boolean UpdateRetryCountForDsOrders(Int32 tenantId, Dictionary<Int32, Int32> dicbkgSvcLineItems, Int32 loggedInUserId)
        {
            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantId).UpdateRetryCountForDsOrders(dicbkgSvcLineItems, loggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get lkpDataPullStatusType lookup data.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static List<lkpDataPullStatusType> GetDataPulledStatusType(Int32 tenantId)
        {
            try
            {
                return LookupManager.GetLookUpData<lkpDataPullStatusType>(tenantId).ToList();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion


        public static Boolean SendOrderNotificationForAutomaticSendSvcForms(Int32 tenantId, Int32 orderId, Int32 currentLoggedInUserId)
        {
            Entity.Tenant tenant = SecurityManager.GetTenant(tenantId);
            String tenantName = tenant.IsNullOrEmpty() ? String.Empty : tenant.TenantName;

            String applicationUrl = WebSiteManager.GetInstitutionUrl(tenantId);
            List<Entity.ClientEntity.lkpBusinessChannelType> businessChannelType = BackgroundProcessOrderManager.GetBusinessChannelType(tenantId);
            List<Entity.ClientEntity.lkpServiceFormStatu> serviceFormStatus = BackgroundProcessOrderManager.GetServiceFormStatus(tenantId);
            List<Entity.ClientEntity.lkpOrderNotifyStatu> orderNotifyStatus = BackgroundProcessOrderManager.GetOrderNotifyStatus(tenantId);
            List<Entity.lkpDocumentAttachmentType> docAttachmentType = CommunicationManager.GetDocumentAttachmentType();
            List<Entity.ClientEntity.lkpOrderNotificationType> orderNotificationType = BackgroundProcessOrderManager.GetOrderNotificationType(tenantId);

            String amsBusinessChannelCode = BusinessChannelType.AMS.GetStringValue();
            Int16 businessChannelTypeID = businessChannelType.IsNotNull() && businessChannelType.Count > 0 ?
                Convert.ToInt16(businessChannelType.FirstOrDefault(cond => cond.Code == amsBusinessChannelCode).BusinessChannelTypeID) : Convert.ToInt16(0);

            String needToSendOrdSvcFormStsCode = ServiceFormStatus.NEED_TO_SEND.GetStringValue();
            Int32 needToSendOrdSvcFormStsID = serviceFormStatus.IsNotNull() && serviceFormStatus.Count > 0 ?
                Convert.ToInt32(serviceFormStatus.FirstOrDefault(cond => cond.SFS_Code == needToSendOrdSvcFormStsCode).SFS_ID) : Convert.ToInt32(0);

            String sentToStudentOrdSvcFormStsCode = ServiceFormStatus.SENT_TO_STUDENT.GetStringValue();
            Int32 sentToStudentOrdSvcFormStsID = serviceFormStatus.IsNotNull() && serviceFormStatus.Count > 0 ?
                Convert.ToInt32(serviceFormStatus.FirstOrDefault(cond => cond.SFS_Code == sentToStudentOrdSvcFormStsCode).SFS_ID) : Convert.ToInt32(0);

            String notifiedOrdNotifyStsCode = OrderNotifyStatus.NOTIFIED.GetStringValue();
            Int32 notifiedOrdNotifyStsID = orderNotifyStatus.IsNotNull() && orderNotifyStatus.Count > 0 ?
                Convert.ToInt32(orderNotifyStatus.FirstOrDefault(cond => cond.ONS_Code == notifiedOrdNotifyStsCode).ONS_ID) : Convert.ToInt32(0);

            String errorOrdNotifyStsCode = OrderNotifyStatus.ERROR.GetStringValue();
            Int32 errorOrdNotifyStsID = orderNotifyStatus.IsNotNull() && orderNotifyStatus.Count > 0 ?
                Convert.ToInt32(orderNotifyStatus.FirstOrDefault(cond => cond.ONS_Code == errorOrdNotifyStsCode).ONS_ID) : Convert.ToInt32(0);

            String docAttachmentTypeCode = DocumentAttachmentType.SYSTEM_DOCUMENT.GetStringValue();
            Int16 docAttachmentTypeID = docAttachmentType.IsNotNull() && docAttachmentType.Count > 0 ?
                Convert.ToInt16(docAttachmentType.FirstOrDefault(cond => cond.DAT_Code == docAttachmentTypeCode).DAT_ID) : Convert.ToInt16(0);

            String svcFormDocumentTypeCode = OrderNotificationType.SERVICE_FORM_DOCUMENT.GetStringValue();
            Int32 svcFormDocumentTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
                Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == svcFormDocumentTypeCode).ONT_ID) : Convert.ToInt32(0);

            String svcFormNotificationTypeCode = OrderNotificationType.SERVICE_FORM_NOTIFICATION.GetStringValue();
            Int32 svcFormNotificationTypeID = orderNotificationType.IsNotNull() && orderNotificationType.Count > 0 ?
                Convert.ToInt32(orderNotificationType.FirstOrDefault(cond => cond.ONT_Code == svcFormNotificationTypeCode).ONT_ID) : Convert.ToInt32(0);

            String entityTypeCode = CommunicationEntityType.BACKGROUND_SERVICE_FORM.GetStringValue();

            Int32 ordNotificationID = 0, bkgOrderServiceFormID = 0;
            Boolean isSucess = true;
            Int32? systemCommunicationID = null;
            Guid? messageID = null;

            List<BkgOrderNotificationDataContract> bkgOrdreNotificationDataList = BackgroundProcessOrderManager.GetBkgOrderServiceFormNotificationDataForAdminEntry(tenantId, orderId, String.Empty);

            if (!bkgOrdreNotificationDataList.IsNullOrEmpty() && bkgOrdreNotificationDataList.Any())
            {
                bkgOrdreNotificationDataList.Select(col => col.BkgOrderID).Distinct().ForEach(bkgOrderID =>
                                {
                                    isSucess = true;

                                    bkgOrdreNotificationDataList.Where(cond => cond.BkgOrderID == bkgOrderID).ForEach(condition =>
                                    {
                                        if (condition.SendAutomatically)
                                        {
                                            //Create Dictionary for Messaging Contract
                                            //Dictionary<String, object> dicMessageParam = new Dictionary<string, object>();
                                            //dicMessageParam.Add("EntityID", condition.ServiceAtachedFormID);
                                            //dicMessageParam.Add("EntityTypeCode", entityTypeCode);

                                            //Create Dictionary for Mail And Message Data
                                            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                                            dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, string.Concat(condition.FirstName, " ", condition.LastName));
                                            dictMailData.Add(EmailFieldConstants.NODE_HIERARCHY, condition.NodeHierarchy);
                                            dictMailData.Add(EmailFieldConstants.INSTITUTION_URL, applicationUrl);
                                            dictMailData.Add(EmailFieldConstants.INSTITUTE_NAME, tenantName);
                                            dictMailData.Add(EmailFieldConstants.ORDER_NO, condition.OrderNumber);
                                            dictMailData.Add(EmailFieldConstants.ORDER_DATE, condition.OrderDate.ToShortDateString());
                                            dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, condition.PackageName);
                                            dictMailData.Add(EmailFieldConstants.SERVICE_NAME, condition.ServiceName);
                                            dictMailData.Add(EmailFieldConstants.SERVICE_FORM_NAME, condition.ServiceAtachedFormName);
                                            dictMailData.Add(EmailFieldConstants.SERVICE_FORM_DISPATCH_DATE, DateTime.Now.ToShortDateString());
                                            dictMailData.Add(EmailFieldConstants.SERVICE_GROUP_NAME, condition.ServiceGroupName);
                                            dictMailData.Add(EmailFieldConstants.ORDER_STATUS, condition.OrderStatus);

                                            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                                            mockData.UserName = string.Concat(condition.FirstName, " ", condition.LastName);
                                            mockData.EmailID = condition.PrimaryEmailAddress;
                                            mockData.ReceiverOrganizationUserID = condition.OrganizationUserID;

                                            //Send mail
                                            systemCommunicationID = CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.DEFAULT_SVC_FORM, dictMailData, mockData, tenantId, condition.HierarchyNodeID, condition.ServiceAtachedFormID, entityTypeCode, true);

                                            systemCommunicationID = systemCommunicationID > 0 ? systemCommunicationID : null;
                                            //Save Mail Attachment
                                            if (condition.SystemDocumentID.HasValue)
                                            {
                                                Int32? sysCommAttachmentID = null;
                                                if (systemCommunicationID != null)
                                                {
                                                    Entity.SystemCommunicationAttachment sysCommAttachment = new Entity.SystemCommunicationAttachment();
                                                    sysCommAttachment.SCA_OriginalDocumentID = condition.SystemDocumentID.Value;
                                                    sysCommAttachment.SCA_OriginalDocumentName = condition.DocumentName;
                                                    sysCommAttachment.SCA_DocumentPath = condition.DocumentPath;
                                                    sysCommAttachment.SCA_DocumentSize = condition.DocumentSize.HasValue ? condition.DocumentSize.Value : 0;
                                                    sysCommAttachment.SCA_SystemCommunicationID = systemCommunicationID.Value;
                                                    sysCommAttachment.SCA_DocAttachmentTypeID = docAttachmentTypeID;
                                                    sysCommAttachment.SCA_TenantID = SecurityManager.DefaultTenantID;
                                                    sysCommAttachment.SCA_IsDeleted = false;
                                                    sysCommAttachment.SCA_CreatedBy = currentLoggedInUserId;
                                                    sysCommAttachment.SCA_CreatedOn = DateTime.Now;
                                                    sysCommAttachment.SCA_ModifiedBy = null;
                                                    sysCommAttachment.SCA_ModifiedOn = null;

                                                    sysCommAttachmentID = CommunicationManager.SaveSystemCommunicationAttachment(sysCommAttachment);
                                                }

                                                //Dictionary<string, string> attachedFiles = new Dictionary<string, string>();
                                                //List<Entity.ADBMessageDocument> messageDocument = new List<Entity.ADBMessageDocument>();

                                                //Entity.ADBMessageDocument documentData = new Entity.ADBMessageDocument();
                                                //documentData.DocumentName = condition.DocumentPath;
                                                //documentData.OriginalDocumentName = condition.DocumentName;
                                                //documentData.DocumentSize = condition.DocumentSize.Value;
                                                //documentData.SystemCommunicationAttachmentID = sysCommAttachmentID;
                                                //messageDocument.Add(documentData);

                                                //attachedFiles = MessageManager.SaveDocumentAndGetDocumentId(messageDocument);
                                                //if (!attachedFiles.IsNullOrEmpty())
                                                //{
                                                //    String documentName = String.Empty;
                                                //    attachedFiles.ForEach(a => documentName += a.Key.ToString() + ";");

                                                //    dicMessageParam.Add("DocumentName", documentName);
                                                //}
                                            }

                                            //Send Message
                                            //  messageID = CommunicationManager.SaveMessageContent(CommunicationSubEvents.DEFAULT_SVC_FORM, dictMailData, condition.OrganizationUserID, tenantId, dicMessageParam);
                                        }
                                        else
                                        {
                                            systemCommunicationID = null;
                                            messageID = null;
                                        }

                                        OrderNotification ordNotification = new OrderNotification();
                                        ordNotification.ONTF_OrderID = condition.OrderID;
                                        ordNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
                                        ordNotification.ONTF_MSG_MessageID = messageID;
                                        ordNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
                                        ordNotification.ONTF_IsPostal = condition.SendAutomatically == false ? true : false;
                                        ordNotification.ONTF_CreatedByID = currentLoggedInUserId;
                                        ordNotification.ONTF_CreatedOn = DateTime.Now;
                                        ordNotification.ONTF_ModifiedByID = null;
                                        ordNotification.ONTF_ModifiedDate = null;
                                        ordNotification.ONTF_ParentNotificationID = null;
                                        ordNotification.ONTF_OrderNotificationTypeID = condition.SendAutomatically == false ? svcFormNotificationTypeID : svcFormDocumentTypeID;
                                        ordNotification.ONTF_NotificationDetail = "Service Form for " + condition.ServiceName;

                                        ordNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenantId, ordNotification);
                                        if (ordNotificationID == 0)
                                        {
                                            isSucess = false;
                                        }

                                        //code added for UAT  - 787 Service Form history dropdown enhancements

                                        else
                                        {
                                            BkgOrderServiceForm bkgOrderServiceForm = new BkgOrderServiceForm();
                                            bkgOrderServiceForm.OSF_ServiceFormMappingID = condition.ServiceFormMappingID;
                                            bkgOrderServiceForm.OSF_BkgOrderPackageSvcID = condition.BkgOrderPackageSvcID;
                                            bkgOrderServiceForm.OSF_ServiceFormStatusID = condition.SendAutomatically == false ? needToSendOrdSvcFormStsID : sentToStudentOrdSvcFormStsID;
                                            bkgOrderServiceForm.OSF_OrderNotificationID = ordNotificationID;
                                            bkgOrderServiceForm.OSF_IsDeleted = false;
                                            bkgOrderServiceForm.OSF_CreatedBy = currentLoggedInUserId;
                                            bkgOrderServiceForm.OSF_CreatedOn = DateTime.Now;
                                            bkgOrderServiceForm.OSF_ModifiedBy = null;
                                            bkgOrderServiceForm.OSF_ModifiedOn = null;

                                            bkgOrderServiceFormID = BackgroundProcessOrderManager.CreateBkgOrderServiceForm(tenantId, bkgOrderServiceForm);

                                            if (bkgOrderServiceFormID == 0)
                                            {
                                                isSucess = false;
                                            }
                                            else
                                            {
                                                OrderNotification newOrdNotification = new OrderNotification();
                                                newOrdNotification.ONTF_OrderID = condition.OrderID;
                                                newOrdNotification.ONTF_MSG_SystemCommunicationID = systemCommunicationID;
                                                newOrdNotification.ONTF_MSG_MessageID = messageID;
                                                newOrdNotification.ONTF_BusinessChannelTypeID = businessChannelTypeID;
                                                newOrdNotification.ONTF_IsPostal = condition.SendAutomatically == false ? true : false;
                                                newOrdNotification.ONTF_CreatedByID = currentLoggedInUserId;
                                                newOrdNotification.ONTF_CreatedOn = DateTime.Now;
                                                newOrdNotification.ONTF_ModifiedByID = null;
                                                newOrdNotification.ONTF_ModifiedDate = null;
                                                newOrdNotification.ONTF_ParentNotificationID = null;
                                                newOrdNotification.ONTF_OrderNotificationTypeID = condition.SendAutomatically == false ? svcFormNotificationTypeID : svcFormDocumentTypeID;
                                                newOrdNotification.ONTF_NotificationDetail = "Service Form for " + condition.ServiceName;
                                                newOrdNotification.ONTF_ParentNotificationID = ordNotificationID;
                                                Int32 newOrdNotificationID = BackgroundProcessOrderManager.CreateOrderNotification(tenantId, newOrdNotification);
                                                if (newOrdNotificationID != 0)
                                                {
                                                    BkgOrderServiceForm newBkgOrderServiceForm = new BkgOrderServiceForm();
                                                    newBkgOrderServiceForm.OSF_ServiceFormMappingID = condition.ServiceFormMappingID;
                                                    newBkgOrderServiceForm.OSF_BkgOrderPackageSvcID = condition.BkgOrderPackageSvcID;
                                                    newBkgOrderServiceForm.OSF_ServiceFormStatusID = condition.SendAutomatically == false ? needToSendOrdSvcFormStsID : sentToStudentOrdSvcFormStsID;
                                                    newBkgOrderServiceForm.OSF_NewServiceFormStatusID = condition.SendAutomatically == false ? needToSendOrdSvcFormStsID : sentToStudentOrdSvcFormStsID; ;
                                                    newBkgOrderServiceForm.OSF_OldServiceFormStatusID = null;
                                                    newBkgOrderServiceForm.OSF_OrderNotificationID = newOrdNotificationID;
                                                    newBkgOrderServiceForm.OSF_IsDeleted = false;
                                                    newBkgOrderServiceForm.OSF_CreatedBy = currentLoggedInUserId;
                                                    newBkgOrderServiceForm.OSF_CreatedOn = DateTime.Now;
                                                    newBkgOrderServiceForm.OSF_ModifiedBy = null;
                                                    newBkgOrderServiceForm.OSF_ModifiedOn = null;

                                                    Int32 newBkgOrderServiceFormID = BackgroundProcessOrderManager.CreateBkgOrderServiceForm(tenantId, newBkgOrderServiceForm);
                                                }
                                            }
                                        }
                                    });

                                    BkgOrder bkgOrd = new BkgOrder();
                                    bkgOrd.BOR_ID = bkgOrderID;
                                    bkgOrd.BOR_OrderNotifyStatusID = isSucess == true ? notifiedOrdNotifyStsID : errorOrdNotifyStsID;
                                    bkgOrd.BOR_ModifiedByID = currentLoggedInUserId;
                                    bkgOrd.BOR_ModifiedOn = DateTime.Now;

                                    BackgroundProcessOrderManager.UpdateBkgOrderNotifyStatus(tenantId, bkgOrd);
                                });

                return true;
            }
            return false;

        }

        public static DataTable GetDataForInProcessAgencyFromApplicantServiceFormStatus(Int32 serviceFormStatusLimit)
        {

            try
            {
                return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(SecurityManager.DefaultTenantID).GetDataForInProcessAgencyFromApplicantServiceFormStatus(serviceFormStatusLimit);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        //UAT-5114

        public static bool CheckIfOrderIsAdminEntryOrder(Int32 tenantID, Int32 bkgOrderId)
        {
            return BALUtils.GetBackgroundProcessOrderRepositoryRepoInstance(tenantID).CheckIfOrderIsAdminEntryOrder(bkgOrderId);
        }
    }
}