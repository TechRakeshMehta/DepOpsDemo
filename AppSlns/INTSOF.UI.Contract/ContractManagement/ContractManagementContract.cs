using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ContractManagement
{
    public class ContractManagementContract
    {
        public Int32 ContractId { get; set; }
        public Int32 TenantId { get; set; }
        public String AffiliationName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public String ExpCode { get; set; }
        public String Contacts { get; set; }
        public String Sites { get; set; }
        public String HierarchyNodes { get; set; }

        public String ExpTypeIds { get; set; }

        /// <summary>
        /// Code for selected ExpirationType in Add/Update
        /// </summary>
        public String ExpTypeCode { get; set; }

        /// <summary>
        /// ID for selected ExpirationType in Add/Update
        /// </summary>
        public Int32? ExpTypeId { get; set; }

        /// <summary>
        /// Number of Months if Term is selected as Expiration Type
        /// </summary>
        public Int32? TermMonths { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// List of Sites corresponding to the Contract.
        /// </summary>
        public List<SiteContract> lstSites { get; set; }

        /// <summary>
        /// List of Contacts corresponding to the Contract.
        /// </summary>
        public List<ContactContract> lstContacts { get; set; }

        /// <summary>
        /// CodeCSV of the DaysBeforeFrequency
        /// </summary>
        public String DaysBeforeFrequency { get; set; }

        /// <summary>
        /// Number of days after expiration
        /// </summary>
        public Int32? AfterFrequency { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        public String Notes { get; set; }

        public List<Int32> lstNodeIds { get; set; }


        public Int32 OrganisationUserID { get; set; }
        public String UserName { get; set; }
        public String UserEmailAddress { get; set; }
        public String SubEventCode { get; set; }


        public Int32 TotalRecordCount { get; set; }

        public Int32? CurrentLoggedInUserId { get; set; }
        #region Contract Document

        /// <summary>
        /// List of Sites corresponding to the Contract.
        /// </summary>
        public List<ContractDocumentContract> ContractDocumentContractList { get; set; }

        #endregion

        public String SearchType { get; set; }

        public Int32? ContractTypeId { get; set; }
        

        public Int32? DocumentStatusId { get; set; }

        public String ContractTypeIdList { get; set; }

        public String ContractTypeNames { get; set; }

        public Int32 SiteID { get; set; }

        public string SiteName { get; set; }
    }
}
