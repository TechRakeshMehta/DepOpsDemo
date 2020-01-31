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
    public class SchoolNodeAssociationDataContract
    {
        [DataMember]
        public Int32 TenantID { get; set; }

        [DataMember]
        public String TenantName { get; set; }

        [DataMember]
        public String DPM_Label { get; set; }

        [DataMember]
        public Int32 DPM_ID { get; set; }

        [DataMember]
        public Int32 AgencyHierarchyID { get; set; }

        [DataMember]
        public Boolean IsStudentShare { get; set; }

        [DataMember]
        public Boolean IsAdminShare { get; set; }

        [DataMember]
        public String CommaSeparatedDpmIds { get; set; }

        [DataMember]
        public String CommaSeparatedDpmlabel { get; set; }

    }
}
