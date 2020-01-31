using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ContractManagement;
using INTSOF.Utils;
using INTSOF.UI.Contract.ContractManagement;
using Entity.ClientEntity;

namespace Business.RepoManagers
{
    public class ContractManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static ContractManager()
        {
            BALUtils.ClassModule = "ContractManager";
        }

        #endregion

        #region Manage Contract

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantId"></param>
        public static List<ContractManagementContract> GetContractSearch(ContractManagementContract objContract, CustomPagingArgsContract customPagingArgsContract, Int32 currentLoggedInUserTenantId)
        {
            try
            {
                if (!objContract.ExpTypeCode.IsNullOrEmpty())
                {
                    objContract.ExpTypeIds = LookupManager.GetLookUpData<lkpContractExpirationType>(objContract.TenantId)
                                                                     .Where(cond => cond.CET_Code == objContract.ExpTypeCode)
                                                                    .FirstOrDefault().CET_ID.ToString();
                }
                return BALUtils.GetContractRepoInstance(objContract.TenantId).GetContractSearch(objContract, customPagingArgsContract, currentLoggedInUserTenantId);
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
        /// Get the contractid of all record
        /// </summary>
        /// <param name="tenantId"></param>
        public static List<Int32> GetContractIDs(ContractManagementContract objContract, CustomPagingArgsContract customPagingArgsContract, Int32 currentLoggedInUserTenantId,Int32 VirtualRecordCount)
        {
            try
            {
                if (!objContract.ExpTypeCode.IsNullOrEmpty())
                {
                    objContract.ExpTypeIds = LookupManager.GetLookUpData<lkpContractExpirationType>(objContract.TenantId)
                                                                     .Where(cond => cond.CET_Code == objContract.ExpTypeCode)
                                                                    .FirstOrDefault().CET_ID.ToString();
                }
                return BALUtils.GetContractRepoInstance(objContract.TenantId).GetContractIDs(objContract, customPagingArgsContract, currentLoggedInUserTenantId,VirtualRecordCount);
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
        /// Save the new Contract and its related data.
        /// </summary>
        /// <param name="contractData"></param>
        /// <param name="currentUserId"></param>
        public static void SaveContract(ContractManagementContract contractData, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                var _lkpContractExpirationType = LookupManager.GetLookUpData<lkpContractExpirationType>(tenantId);
                var _selectedExpType = _lkpContractExpirationType.Where(cet => cet.CET_Code == contractData.ExpTypeCode).FirstOrDefault();
                if (_selectedExpType.IsNotNull())
                {
                    contractData.ExpTypeId = _selectedExpType.CET_ID;
                }
                BALUtils.GetContractRepoInstance(tenantId).SaveContract(contractData, currentUserId,_lkpContractExpirationType);
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
        /// Save the new Contract and its related data.
        /// </summary>
        /// <param name="contractData"></param>
        /// <param name="currentUserId"></param>
        public static void UpdateContract(ContractManagementContract contractData, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                var _lkpContractExpirationType = LookupManager.GetLookUpData<lkpContractExpirationType>(tenantId);
                var _selectedExpType = _lkpContractExpirationType.Where(cet => cet.CET_Code == contractData.ExpTypeCode).FirstOrDefault();
                if (_selectedExpType.IsNotNull())
                {
                    contractData.ExpTypeId = _selectedExpType.CET_ID;
                }
                BALUtils.GetContractRepoInstance(tenantId).UpdateContract(contractData, currentUserId,_lkpContractExpirationType);
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

        public static Boolean DeleteContract(Int32 contractID, Int32 currentUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(tenantId).DeleteContract(contractID, currentUserId);
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
        /// Get detils of the selected Contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public static ContractManagementContract GetContractDetails(Int32 contractId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(tenantId).GetContractDetails(contractId);
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
        /// Gets the List of Contract Sites and Contacts
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public static Tuple<List<SiteContract>, List<ContactContract>> GetContractSitesContacts(Int32 contractId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(tenantId).GetContractSitesContacts(contractId);
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

        #region Manage Contract Types

        /// <summary>
        /// Get Contract Types
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static IQueryable<ContractType> GetContractTypes(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(tenantId).GetContractTypes();
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
        /// Get Contract Type by Id
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="contractTypeId"></param>
        /// <returns></returns>
        public static ContractType GetContractTypeById(Int32 tenantId, Int32 contractTypeId)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(tenantId).GetContractTypeById(contractTypeId);
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
        /// Save Contract Types
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public static Boolean SaveContractTypes(Int32 tenantId, ContractType contractType)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(tenantId).SaveContractTypes(contractType);
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
        /// Update Contract Types
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static Boolean UpdateContractTypes(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(tenantId).UpdateContractTypes();
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

        public static Boolean IsContractTypeMapped(Int32 tenantId, Int32 contractTypeId)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(tenantId).IsContractTypeMapped(contractTypeId);
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

        public static String GetLastContractTypeCode(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(tenantId).GetLastContractTypeCode();
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


        #region ContractNotificationData

        public static List<ContractManagementContract> GetContractNotificationDetails(String SubEventCode, int TenantID, int Chunksize)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(TenantID).GetContractNotificationData(SubEventCode, TenantID, Chunksize);
                // .GetClinicalRotationQueueData(clinicalRotationDetailContract, customPagingArgsContract);
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


        #region SiteNotificationData

        public static List<ContractManagementContract> GetSiteNotificationDetails(String SubEventCode, int TenantID, int Chunksize)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(TenantID).GetSiteNotificationDetails(SubEventCode, TenantID, Chunksize);
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

        #region Contract Document

        /// <summary>
        /// Get the list of Sites associated with a Contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="tenantId"></param>
        public static List<ContractDocumentContract> GetContractDocuments(Int32 contractId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(tenantId).GetContractDocuments(contractId);
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
        /// Get the list of Sites associated with a Contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="tenantId"></param>
        public static List<SiteDocumentContract> GetSiteDocuments(Int32 siteId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(tenantId).GetSiteDocuments(siteId);
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

        public static List<ContractSite> GetContractsites(Int32 contractId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(tenantId).GetContractsites(contractId);
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

        public static List<ContactContract> GetSiteContacts(Int32 contractId, Int32 siteId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetContractRepoInstance(tenantId).GetSiteContacts(contractId,siteId);
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

    }
}
