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
    public class RequirementObjectPropertiesContract
    {
        [DataMember]
        public Int32 RequirementObjPropID { get; set; }
        [DataMember]
        public Int32 RequirementObjPropCategoryID { get; set; }
        [DataMember]
        public Int32? RequirementObjPropCategoryItemID { get; set; }
        [DataMember]
        public Int32? RequirementObjPropItemFieldID { get; set; }
        [DataMember]
        public Boolean? RequirementObjPropIsCustomSettings { get; set; }
        [DataMember]
        public Boolean RequirementObjPropIsEditableByAdmin { get; set; }
        [DataMember]
        public Boolean RequirementObjPropIsEditableByApplicant { get; set; }
        [DataMember]
        public Boolean RequirementObjPropIsEditableByClientAdmin { get; set; }
    
    }
}
