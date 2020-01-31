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
    public class UniversalAttributeContract
    {
        [DataMember]
        public Boolean IsNewPackage { get; set; }
        [DataMember]
        public Int32 UniversalAttributeID { get; set; }
        [DataMember]
        public String UniversalAttributeName { get; set; }
        [DataMember]
        public Int32 UniReqItemMappingID { get; set; }
        [DataMember]
        public Int32 UniItmAttrMappingID { get; set; }
        [DataMember]
        public Int32 ReqItmFldMappingID { get; set; }
        [DataMember]
        public Int32 UniReqAttrMappingID { get; set; }
        [DataMember]
        public String UniversalAttrDataTypeCode { get; set; }
        [DataMember]
        public Int32 UniversalFieldMappingID { get; set; }
        [DataMember]
        public Int32 UniversalFieldID { get; set; }

        [DataMember]
        public List<InputTypeComplianceAttributeServiceContract> lstAttributeInputData { get; set; }
        [DataMember]
        public List<UniversalAtrOptionMapServiceContract> lstOptionMapping { get; set; }
    }
}
