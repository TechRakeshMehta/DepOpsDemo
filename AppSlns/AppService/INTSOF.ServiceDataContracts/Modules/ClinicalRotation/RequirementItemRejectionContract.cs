using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [DataContract]
    public class RequirementItemRejectionContract
    {
        [DataMember]
        public Int32 ReqPkgSubscriptionID { get; set; }

        [DataMember]
        public String ApplicantName { get; set; }

        [DataMember]
        public String ApplicnatEmailAddress { get; set; }

        [DataMember]
        public Int32 ApplicantOrganizationUserId { get; set; }

        [DataMember]
        public String RequirementItemName { get; set; }


        [DataMember]
        public String RequirementCategoryName { get; set; }


        [DataMember]
        public String ItemRejectionReason { get; set; }

        [DataMember]
        public String AgencyName { get; set; }

        [DataMember]
        public String RotationHierachyIds { get; set; }
    }
}
