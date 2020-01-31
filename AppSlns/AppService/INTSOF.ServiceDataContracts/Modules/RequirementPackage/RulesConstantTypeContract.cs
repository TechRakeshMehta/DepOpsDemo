using System;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RulesConstantTypeContract
    {
        [DataMember]
        public Int32 ConstantTypeID { get; set; }
        [DataMember]
        public String ConstantTypeName { get; set; }
        [DataMember]
        public String ConstantTypeCode { get; set; }
        [DataMember]
        public String ConstantTypeGroup { get; set; }
    }
}
