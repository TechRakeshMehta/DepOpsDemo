using INTSOF.ServiceDataContracts.Modules.Common;
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
    public class AgencyNodeMappingContract
    {
        [DataMember]
        public Int32 AgencyID { get; set; }
        [DataMember]
        public Int32 AgencyHierarchyAgencyID { get; set; }
        [DataMember]
        public List<Int32> SelectedAgencyIDs { get; set; }
        [DataMember]
        public String AgencyName { get; set; }
        [DataMember]
        public Int32 CurrentLoggedInUserID { get; set; }
        [DataMember]
        public Int32 AgencyHierarchyId { get; set; }  
        [DataMember]
        public Int32 AttestationDocumentID { get; set; }
        [DataMember]
        public String AttestationformSettingValue { get; set; }
        [DataMember]
        public String AttestationFileName { get; set; }
        [DataMember]
        public String AttestationDocumentPath { get; set; }
    }
}
