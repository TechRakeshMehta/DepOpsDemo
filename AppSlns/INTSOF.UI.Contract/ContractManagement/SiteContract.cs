using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ContractManagement
{
    /// <summary>
    /// Contract class for ContractSites table
    /// </summary>
    [Serializable]
    public class SiteContract
    {

        public Int32 TempSiteId { get; set; }

        public Int32 SiteId { get; set; }

        public String SiteName { get; set; }

        public String SiteAddress { get; set; }

        /// <summary>
        /// MappingId of the Contract and SiteContract i.e. CSCM_ID of 'ContractSitesContractMapping' table
        /// </summary>
        public Int32 ContractSiteMappingId { get; set; }


        /// <summary>
        /// 'True' if the Contact is deleted temporariy i.e. the final save is not done.
        /// </summary>
        public Boolean IsTempDeleted { get; set; }

        public List<SiteDocumentContract> lstSiteDocumentContract { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public String ExpCode { get; set; }

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

        public List<ContactContract> lstSiteContacts { get; set; }

        public String ContractTypeIdList { get; set; }
    }

    [Serializable]
    public class SiteDocumentContract
    {
        public Int32 SiteDocumentMappingID { get; set; }
        public String DocumentName { get; set; }
        public Int32 ContractID { get; set; }
        public Int32 ClientSystemDocumentID { get; set; }
        public DateTime? DocStartDate { get; set; }
        public DateTime? DocEndDate { get; set; }
        public Int32? ParentDocID { get; set; }
        public Int32 TempDocID { get; set; }
        public Int32 DocTypeID { get; set; }
        public String DocTypeCode { get; set; }
        public String DocPath { get; set; }
        public Int32 DocSize { get; set; }
        public String DocFileName { get; set; }
        public String DocumentTypeName { get; set; }
        public Boolean IsCreateVersion { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsDeleted { get; set; }
        public Boolean IsNewVersion { get; set; }
        public Boolean IsDocUpdated { get; set; }

        public Int32? DocStatusID { get; set; }
        public String DocStatusName { get; set; }
    }

}
