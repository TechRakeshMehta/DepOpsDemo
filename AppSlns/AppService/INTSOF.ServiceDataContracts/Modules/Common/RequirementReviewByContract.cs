using System;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RequirementReviewByContract
    {
        [DataMember]
        public Int32 ID { get; set; }

        [DataMember]
        public String Code { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String Description { get; set; }
    }
}
