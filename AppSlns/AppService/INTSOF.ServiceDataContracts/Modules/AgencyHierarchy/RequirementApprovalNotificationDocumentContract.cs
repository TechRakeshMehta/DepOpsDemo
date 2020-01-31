using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.AgencyHierarchy
{
    [Serializable]
    [DataContract]
    public class RequirementApprovalNotificationDocumentContract
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public Int32? Size { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public Int32 CreatedBy { get; set; }

        [DataMember]
        public String DocumentPath { get; set; }

        [DataMember]
        public string DocumentTypeCode { get; set; }

        [DataMember]
        public Int32 AgencyHierarchySystemDocumentID { get; set; }

        [DataMember]
        public Int32 ClientSystemDocumentID { get; set; }

        [DataMember]
        public Int32? AgencyID { get; set; }
    }
}
