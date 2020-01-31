using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class UniversalAtrOptionMapServiceContract
    {
        [DataMember]
        public String RequirementOptionText { get; set; }
        [DataMember]
        public Dictionary<Int32, String> lstAttributeOptionValue { get; set; }
        [DataMember]
        public Int32 RequirementOptionID { get; set; }
        [DataMember]
        public Int32 UniversalAtrOptionID { get; set; }
        [DataMember]
        public Int32 UniversalReqAtrMappingID { get; set; }
        [DataMember]
        public String UniversalAttributeName { get; set; }
    }
}
