using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation
{
    [Serializable]
    [DataContract]
    public class RequirementPackageSubscriptionContract
    {
        [DataMember]
        public Int32 RequirementPackageSubscriptionID { get; set; }
        
        [DataMember]
        public Int32 HierarchyID { get; set; }
        
		[DataMember]
        public Int32 ApplicantOrgUserID { get; set; }

        [DataMember]
        public Int32 RequirementPackageID { get; set; }

        [DataMember]
        public String RequirementPackageName { get; set; }

        [DataMember]
        public Int32 RequirementPackageSubscriptionStatusId { get; set; }

        [DataMember]
        public String RequirementPackageSubscriptionStatusCode { get; set; }

        [DataMember]
        public List<ApplicantRequirementCategoryDataContract> ApplicantRequirementCategoryData { get; set; }

        [DataMember]
        public String Notes { get; set; }
        //UAT-3122
        [DataMember]
        public String ComplioID { get; set; }

        [DataMember]
        public Int32 RotationID { get; set; }//UAT-3364
        [DataMember]
        public DateTime? RotationEndDate { get; set; } //UAT-5040
    }
}
