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
    public class SchoolNodeAssociationContract
    {
        [DataMember]
        public Int32 AgencyHierarchyInstitutionNodeID;

        [DataMember]
        public Int32 AgencyHierarchyID;

        [DataMember]
        public List<Int32> DPM_IDs;

        [DataMember]
        public Int32 CurrentLoggedInUserID;

        [DataMember]
        public Boolean IsStudentShare { get; set; }

        [DataMember]
        public Boolean IsAdminShare { get; set; }

        [DataMember]
        public Int32 DPM_ID;

    }
}
