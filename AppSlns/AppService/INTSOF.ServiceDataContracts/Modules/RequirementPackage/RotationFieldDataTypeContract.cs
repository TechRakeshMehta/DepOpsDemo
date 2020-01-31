using System;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [DataContract]
    public class RotationFieldDataTypeContract
    {
        [DataMember]
        public Int32 DataTypeID { get; set; }
        [DataMember]
        public String DataTypeName { get; set; }
        [DataMember]
        public String DataTypeCode { get; set; }
    }
}

