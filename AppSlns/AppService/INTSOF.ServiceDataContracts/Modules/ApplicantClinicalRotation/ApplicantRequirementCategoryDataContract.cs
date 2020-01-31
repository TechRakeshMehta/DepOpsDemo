using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation
{

    [Serializable]
    [DataContract]
    public class ApplicantRequirementCategoryDataContract
    {
        [DataMember]
        public Int32 RequirementCategoryDataID { get; set; }

        [DataMember]
        public Int32 RequirementCategoryID { get; set; }

        [DataMember]
        public Int32 RequirementCategoryStatusID { get; set; }

        [DataMember]
        public String RequirementCategoryStatus { get; set; }

        [DataMember]
        public String RequirementCategoryStatusCode { get; set; }
        //UAT-3122
        [DataMember]
        public String RequirementCategoryName { get; set; }
        
        //UAT 3106
        [DataMember]
        public String CategoryRuleStatusID { get; set; }

        [DataMember]
        public List<ApplicantRequirementItemDataContract> ApplicantRequirementItemData { get; set; }
    }

}