using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{

    [Serializable]
    public class ProfileSharingExpiryContract
    {
        public List<NonComplianceCategoryContract> NonComplianceCategoryContractList { get; set; }

        public List<RequirementExpiryCategoryContract> RequirementExpiryCategoryContractList { get; set; }
    }
    [Serializable]
    public class NonComplianceCategoryContract
    {
        public String ApplicantName { get; set; }

        public String Category { get; set; }
    }

    [Serializable]
    public class RequirementExpiryCategoryContract
    {
        public String RotationApplicantName { get; set; }

        public String RotationCategory { get; set; }

        public String RotationItem { get; set; }

        public String RotationExpirationDate { get; set; }
    }

   
}
