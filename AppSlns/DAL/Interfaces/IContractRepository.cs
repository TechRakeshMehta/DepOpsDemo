using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ContractManagement;
using INTSOF.Utils;

namespace DAL.Interfaces
{
    public interface IContractRepository
    {
        #region Manage Contracts

        List<ContractManagementContract> GetContractSearch(ContractManagementContract objContract, CustomPagingArgsContract customPagingArgsContract, Int32 currentLoggedInUserTenantId);

        /// <summary>
        /// Save the new Contract and its related data.
        /// </summary>
        /// <param name="contractData"></param>
        /// <param name="currentUserId"></param>
        void SaveContract(ContractManagementContract contractData, Int32 currentUserId,List<lkpContractExpirationType> lkpContractExpirationType);

        /// <summary>
        /// Update the Contract
        /// </summary>
        /// <param name="contractData"></param>
        /// <param name="currentUserId"></param>
        void UpdateContract(ContractManagementContract contractData, Int32 currentUserId,List<lkpContractExpirationType> lkpContractExpirationType);

        Boolean DeleteContract(Int32 contractID, Int32 currentUserId);

        /// <summary>
        /// Get detils of the selected Contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        ContractManagementContract GetContractDetails(Int32 contractId);

        /// <summary>
        /// Gets the List of Contract Sites and Contacts
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        Tuple<List<SiteContract>, List<ContactContract>> GetContractSitesContacts(Int32 contractId);

        #endregion

        #region Manage Contract Types

        IQueryable<ContractType> GetContractTypes();
        ContractType GetContractTypeById(Int32 contractTypeId);
        Boolean SaveContractTypes(ContractType contractType);
        Boolean UpdateContractTypes();
        Boolean IsContractTypeMapped(Int32 contractTypeId);
        String GetLastContractTypeCode();

        #endregion

        #region ContractNotificationData

        List<ContractManagementContract> GetContractNotificationData(String SubEventCode, int TenantID, int Chunksize);
        
        #endregion

        #region SiteNotificationData
        List<ContractManagementContract> GetSiteNotificationDetails(String SubEventCode, int TenantID, int Chunksize);
        #endregion


        #region Contract Document

        /// <summary>
        /// Get the list of Documents associated with a Contract
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="tenantId"></param>
        List<ContractDocumentContract> GetContractDocuments(Int32 contractId);

        #endregion

        /// <summary>
        /// Get the list of Documents associated with a Site
        /// </summary>
        /// <param name="contractId"></param>
        /// <param name="tenantId"></param>
        List<SiteDocumentContract> GetSiteDocuments(Int32 siteId);

        List<ContractSite> GetContractsites(Int32 contractId);

        List<ContactContract> GetSiteContacts(Int32 contractId, Int32 siteId);
        /// <summary>
        /// Get the Contract id of all records
        /// </summary>
        /// <param name="objContract"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <param name="currentLoggedInUserTenantId"></param>
        /// <returns></returns>
        List<Int32> GetContractIDs(ContractManagementContract objContract, CustomPagingArgsContract customPagingArgsContract, Int32 currentLoggedInUserTenantId, Int32 VirtualRecordCount);

    }
}
