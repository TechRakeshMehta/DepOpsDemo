using INTSOF.Utils;
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
    public class AgencyNodeContract
    {
        [DataMember]
        public Int32 NodeId { get; set; }
        [DataMember]
        public String NodeName { get; set; }
        [DataMember]
        public String NodeLabel { get; set; }
        [DataMember]
        public String NodeDescription { get; set; }
        [DataMember]
        public Boolean IsDeleted { get; set; }
        [DataMember]
        public Int32 CurrentLoggedInUser { get; set; }
        [DataMember]
        public String MappedRootHierachies { get; set; } //UAT-3652
        [DataMember]
        public Int32 TotalRecordCount { get; set; }
    }
}
