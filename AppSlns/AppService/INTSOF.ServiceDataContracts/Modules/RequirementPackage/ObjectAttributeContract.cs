using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class ObjectAttributeContract
    {
        [DataMember]
        public Boolean IsSignatureRequired { get; set; }
        [DataMember]
        public Boolean IsRequiredToView { get; set; }
        [DataMember]
        public Boolean IsRequiredToOpen { get; set; }
        [DataMember]
        public Boolean IsRequired { get; set; }
        [DataMember]
        public String BoxOpenTime { get; set; }
    }
}

