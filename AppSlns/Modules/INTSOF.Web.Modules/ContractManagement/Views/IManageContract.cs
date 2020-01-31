using INTSOF.UI.Contract.ContractManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.ContractManagement.Views
{
    public interface IManageContract
    {
        Int32 TenantID
        {
            get;
            set;
        }

        Int32 SelectedTenantID
        {
            get;
            set;
        }

        Boolean IsReset
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }

        List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        ContractManagementContract SearchContract
        {
            get;
            set;
        }

        List<ContractManagementContract> ContractDataList
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the List of Sites under the selected Contract
        /// </summary>
        List<SiteContract> lstSiteContracts
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the List of Contracts under the selected Contract
        /// </summary>
        List<ContactContract> lstContactContract
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the contractid to which the current Sites belong to
        /// </summary>
        Int32 ContractId
        {
            get;
            set;
        }

        /// <summary>
        /// OrganizationUserID of the loggedIn user.
        /// </summary>
        Int32 CurrentUserId
        {
            get;
        }

        /// <summary>
        /// Contract Data to Save
        /// </summary>
        ContractManagementContract ContractData
        {
            get;
            set;
        }

        /// <summary>
        /// ID of the Root node 
        /// </summary>
        String RootDPMId
        {
            get;
            set;
        }

        #region Contract Document

        /// <summary>
        /// Represents the List of Documents under the selected Contract
        /// </summary>
        List<ContractDocumentContract> ContractDocumentContractList
        {
            get;
            set;
        }



        #endregion

        /// <summary>
        /// Represents the List of Documents under the selected Site
        /// </summary>
        List<SiteDocumentContract> SiteDocumentContractList
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the SiteId 
        /// </summary>
        Int32 SiteId
        {
            get;
            set;
        }

        Boolean IsReadOnlyPermission
        {
            get;
            set;
        }

        #region Custom paging parameters
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        Int32 PageSize
        {
            get;
            set;
        }

        Int32 VirtualRecordCount
        {
            get;
            set;
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Represents the List of Contracts under the selected site
        /// </summary>
        List<ContactContract> lstSiteContacts
        {
            get;
            set;
        }

        List<Int32> lstContractIDs
        {
            get;
            set;
        }
    }
}
