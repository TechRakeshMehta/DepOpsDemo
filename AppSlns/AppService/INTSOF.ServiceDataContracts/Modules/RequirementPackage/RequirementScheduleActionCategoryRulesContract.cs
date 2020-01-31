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
    public class RequirementScheduleActionCategoryRulesContract
    {
        [DataMember]
        public Int32 RequirementPackageSubscriptionID { get; set; }

        [DataMember]
        public Int32 ApplicantOrgUserID { get; set; }

        [DataMember]
        public String ApplicantName { get; set; }

        [DataMember]
        public String PrimaryEmailAddress { get; set; }

        [DataMember]
        public Int32 RequirementPackageID { get; set; }

        [DataMember]
        public Int32 RequirementPackageSubscriptionStatusID { get; set; }

        [DataMember]
        public String RequirementPackageSubscriptionStatusCode { get; set; }

        [DataMember]
        public Int32 RequirementItemID { get; set; }

        [DataMember]
        public Int32 RequirementCategoryID { get; set; }

        [DataMember]
        public Int32 RequirementFieldID { get; set; }

        [DataMember]
        public Int32 ScheduleActionId { get; set; }

        [DataMember]
        public String RequirementStatus { get; set; }
    }
}
