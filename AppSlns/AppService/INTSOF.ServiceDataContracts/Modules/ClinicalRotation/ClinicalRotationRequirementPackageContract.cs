using System;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class ClinicalRotationRequirementPackageContract
    {
        [DataMember]
        public Int32 ClinicalRotationRequirementPackageID { get; set; }
        [DataMember]
        public Int32 ClinicalRotationID { get; set; }
        [DataMember]
        public Int32 RequirementPackageID { get; set; }
        [DataMember]
        public String RequirementPackageName { get; set; }
        [DataMember]
        public Guid RequirementPackageCode { get; set; }
        [DataMember]
        public Boolean IsCopied { get; set; }

        [DataMember]
        public Boolean IsActive { get; set; }
        [DataMember]
        public String RequirementPackageLabel { get; set; }
        [DataMember]
        public bool IsArchived { get; set; }
    }
}


