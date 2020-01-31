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
    public class RequirementObjectTreeContract
    {
        [DataMember]
        public Int32 RequirementObjectTreeID { get; set; }
        [DataMember]
        public Int32 ObjectID { get; set; }
        [DataMember]
        public Int32 ObjectTypeID { get; set; }
        [DataMember]
        public String ObjectTypeCode { get; set; }
        [DataMember]
        public Int32? ParentID { get; set; }
        [DataMember]
        public Int32? ParentObjectID { get; set; }
        [DataMember]
        public Boolean IsNewRecordInserted { get; set; }
        [DataMember]
        public String ObjectAttributeValue { get; set; }
        [DataMember]
        public Int32? ObjectAttributeTypeId { get; set; }
        [DataMember]
        public String ObjectAttributeTypeCode { get; set; }
    }
}
