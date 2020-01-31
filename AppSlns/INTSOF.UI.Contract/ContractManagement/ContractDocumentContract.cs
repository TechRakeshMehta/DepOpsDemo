using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ContractManagement
{
    /// <summary>
    /// Contract class for ContractDocumentMapping entity
    /// </summary>
    [Serializable]
    public class ContractDocumentContract
    {
        public Int32 ContractDocumentMappingID { get; set; }
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

