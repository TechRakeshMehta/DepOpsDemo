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
    public class ApplicantRequirementItemDataContract
    {
        [DataMember]
        public Int32 RequirementItemDataID { get; set; }
        [DataMember]
        public String ApplicantName { get; set; }
        [DataMember]
        public Int32 RequirementCategoryDataID { get; set; }

        [DataMember]
        public Int32 RequirementItemID { get; set; }

        [DataMember]
        public String RequirementItemName { get; set; }

        [DataMember]
        public Int32 RequirementItemStatusID { get; set; }

        [DataMember]
        public String RequirementItemStatus { get; set; }

        [DataMember]
        public String RequirementItemStatusCode { get; set; }

        [DataMember]
        public List<ApplicantRequirementFieldDataContract> ApplicantRequirementFieldData { get; set; }

        /// <summary>
        /// UAT-2226, Allow the rejection reason to populate to the student when an admin leaves a rejection note for rotation requirements
        /// </summary>
        [DataMember]
        public String RejectionReason { get; set; }
        [DataMember]
        public String ItemMovementTypeCode { get; set; }
    }
}
