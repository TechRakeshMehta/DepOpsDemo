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
    public class ApplicantRequirementParameterContract
    {
        [DataMember]
        public Int32 RequirementPkgSubscriptionId { get; set; }
        [DataMember]
        public Int32 RequirementCategoryId { get; set; }
        [DataMember]
        public Int32 RequirementItemId { get; set; }
        [DataMember]
        public Int32 TenantId { get; set; }

        #region Save/Update Applicant Requirement Data
        [DataMember]
        public ApplicantRequirementItemDataContract AppRequirementItemData { get; set; }
        [DataMember]
        public ApplicantRequirementCategoryDataContract AppRequirementCategoryData { get; set; }
        [DataMember]
        public List<ApplicantRequirementFieldDataContract> AppRequirementFieldDataList { get; set; }
        [DataMember]
        public Dictionary<Int32, Int32> AppFieldDocuments { get; set; }

        [DataMember]
        public Dictionary<Int32,ApplicantDocumentContract> SignedApplicantDocuments { get; set; }

        [DataMember]
        public Boolean IsUIValidationApplicable { get; set; }

        [DataMember]
        public Int32 RequirementPackageId { get; set; }

        //UAT-2213
        [DataMember]
        public Boolean IsNewPackage { get; set; }
        #endregion

        [DataMember]
        public String prevCategoryStatusCode { get; set; }
        [DataMember]
        public String NewCategoryStatusCode { get; set; }
        [DataMember]
        public Int32 AppRequirementItemDataID { get; set; }
        //Start UAT-5062
        [DataMember]
        public Boolean IsUploadDocUpdated { get; set; }
        //End UAT-5062
    }
}
