using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.PlacementMatching
{
    [Serializable]
    [DataContract]
    public class SharedCustomAttributesContract
    {
        [DataMember]
        public Int32 SharedCustomAttributeID { get; set; }
        [DataMember]
        public String AttributeName { get; set; }
        [DataMember]
        public String AttributeLabel { get; set; }
        [DataMember]
        public Int32 AttributeDataTypeID { get; set; }
        [DataMember]
        public String AttributeDataTypeCode { get; set; }
        [DataMember]
        public String AttributeDataType { get; set; }
        [DataMember]
        public Int32 AttributeUseTypeID { get; set; }
        [DataMember]
        public String AttributeUseTypeCode { get; set; }
        [DataMember]
        public String AttributeUseType { get; set; }
        [DataMember]
        public Boolean IsActive { get; set; }
        [DataMember]
        public Boolean IsRequired { get; set; }
        [DataMember]
        public Int32? StringLength { get; set; }
        [DataMember]
        public String RegularExpression { get; set; } 
        [DataMember]
        public String RegExpErrorMsg { get; set; }
        [DataMember]
        public Int32? RelatedCustomAttributeID { get; set; }
        [DataMember]
        public Int32 AgencyHierarchyRootNodeID{ get; set; }
        [DataMember]
        public String AgencyName { get; set; }
        [DataMember]
        public Int32 SharedCustomAttributeMappingID { get; set; }
    }
}
