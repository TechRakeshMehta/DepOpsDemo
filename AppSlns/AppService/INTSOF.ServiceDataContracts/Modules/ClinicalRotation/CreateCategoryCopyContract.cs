using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class CreateCategoryCopyContract
    {
        [DataMember]
        public Int32 OldRequirementCategoryID { get; set; }

        [DataMember]
        public String SelectedReqPackageIds { get; set; }

        [DataMember]
        public String CategoryName { get; set; }

        [DataMember]
        public String CategoryLabel { get; set; }

        [DataMember]
        public Int32 CurrentLoggedInUserId { get; set; }

        [DataMember]
        public String ExplanatoryNotes { get; set; }

        [DataMember]
        public Boolean IsComplianceRequired { get; set; }

        [DataMember]
        public DateTime? ComplianceReqStartDate { get; set; }

        [DataMember]
        public DateTime? ComplianceReqEndDate { get; set; }

        [DataMember]
        public Int32 UniversalCategoryID { get; set; }

        [DataMember]
        public Int32 UniversalReqCatMappingID { get; set; }

        [DataMember]
        public String RequirementDocumentLink { get; set; }

        [DataMember]
        public Boolean AllowDataMovement { get; set; }

        //UAT-3161
        [DataMember]
        public String RequirementDocumentLinkLabel { get; set; }

        //UAT-3805
        [DataMember]
        public Boolean SendItemDoconApproval { get; set; }

        [DataMember]
        public Dictionary<String, Boolean> SelectedlstEditableBy { get; set; }

        //UAT-4259
        [DataMember]
        public Boolean TriggerOtherCategoryRules { get; set; }

        //UAT-4254
        [DataMember]
        public List<RequirementCategoryDocUrl> lstReqCatUrls { get; set; }
        
    }
}
