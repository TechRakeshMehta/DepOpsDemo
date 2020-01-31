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
    public class AgencyHierarchyDataContract
    {
        [DataMember]
        public Int32 AgencyHierarchyID { get; set; }
        [DataMember]
        public String AgencyHierarchyLabel { get; set; }
        [DataMember]
        public Int32 CurrentPageIndex { get; set; }
        [DataMember]
        public Int32 VirtualPageCount { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public Int32 AgencyNodeID { get; set; }
        [DataMember]
        public Int32 DisplayOrder { get; set; }  //UAT-3237
    }
}
