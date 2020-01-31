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
    public class AgencyHierarchyPackageContract
    {
        [DataMember]
        public Int32 AgencyHierarchyPackageID { get; set; }
        [DataMember]
        public Int32 RequirementPackageID { get; set; }
        [DataMember]
        public Int32 AgencyHierarchyID { get; set; }
        [DataMember]
        public Int32 CurrentLoggedInUser { get; set; }
    }
}
