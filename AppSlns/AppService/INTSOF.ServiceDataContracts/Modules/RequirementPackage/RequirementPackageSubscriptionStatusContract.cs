using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RequirementPackageSubscriptionStatusContract
    {
        [DataMember]
        public Int32 RequirementPackageSubscriptionID { get; set; }
        [DataMember]
        public string RequirementCategoryStatusCode { get; set; }
        [DataMember]
        public string RotationName { get; set; }
        [DataMember]
        public string PackageName { get; set; }
        [DataMember]
        public string ApplicantName { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public Int32 OrganizationUserID { get; set; }
        [DataMember]
        public Int32 RotationID { get; set; }
        [DataMember]
        public string AgencyName { get; set; }
        [DataMember]
        public Int32 ClinicalRotationSubscriptionID { get; set; }
    }
}
