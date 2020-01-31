using System;
using System.Runtime.Serialization;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class RequirementVerificationQueueContract : SearchContract
    {
        [DataMember]
        public Boolean IsRotUserVericationItemDetail { get; set; }
        [DataMember]
        public Int32 OrganizationUserID { get; set; }
        [DataMember]
        public Int32 ClinicalRotationID { get; set; }
        [DataMember]
        public DateTime? RotationStartDate { get; set; }
        [DataMember]
        public DateTime? RotationEndDate { get; set; }
        [DataMember]
        public DateTime? SubmissionDate { get; set; }
        [DataMember]
        public Int32? AgencyID { get; set; }
        [DataMember]
        public String AgencyName { get; set; }
        [DataMember]
        public Int32? RequirementPackageTypeID { get; set; }
        [DataMember]
        public String RequirementPackageTypeName { get; set; }
        [DataMember]
        public Int32 RequirementPackageSubscriptionID { get; set; }
        [DataMember]
        public Int32? LoggedInUserId { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }
        [DataMember]
        public Int32 RequirementItemId { get; set; }
        [DataMember]
        public Int32 ApplicantRequirementItemId { get; set; }
        [DataMember]
        public Int32 ApplicantRequirementCategoryId { get; set; }
        [DataMember]
        public Int32 CurrentPageIndex { get; set; }
        [DataMember]
        public CustomPagingArgsContract GridCustomPagingArguments { get; set; }

        public String XML { get { return GetXml(); } }

        [DataMember]
        public bool IsRotationPackageVerificationQueue { get; set; }

        [DataMember]
        public String RequirementPackageID { get; set; }

        [DataMember]
        public String RequirementPackageName { get; set; }
        
        [DataMember]
        public String UserType { get; set; } //UAT-3703

        //UAT-2197:
        [DataMember]
        public String SelectedRequirementPackageTypes { get; set; }

        [DataMember]
        public bool IsCurrentRotation { get; set; }
        [DataMember]
        public String SelectedTenantIDs { get; set; }  //UAT 2975
        [DataMember]
        public String AssignedUserName { get; set; } //UAT 2975
        [DataMember]
        public Int32 FlatVerificationDataID { get; set; } //UAT 2975
        [DataMember]
        public Int32 RequirementCategoryID { get; set; } //UAT 2975
        [DataMember]
        public String RequirementCategoryName { get; set; } //UAT 2975
        [DataMember]
        public String RequirementItemName { get; set; } //UAT 2975
        [DataMember]
        public Boolean IsRotationVerificationUserWorkQueue { get; set; } //UAT 2975 
        [DataMember]
        public String RequirementItemVerificationCode { get; set; } //UAT 2975 
        [DataMember]
        public String ComplioID { get; set; } //UAT 2975 

        [DataMember] //UAT-3245
        public String DPMIds { get; set; }
        [DataMember]
        public String InstituteHierarchySelectedNode { get; set; }
        [DataMember]
        public String SelectedAgencyIds { get; set; }
        [DataMember]
        public Int32 NodeId { get; set; }
        [DataMember]
        public Int32 SelectedRootNodeId { get; set; }

        [DataMember]
        public int? ReqReviewByID { get; set; }

        [DataMember]
        public string ReqReviewByDesc { get; set; }

        [DataMember]
        public string ReqCategoryLabel { get; set; }  //UAT-4705

        [DataMember]
        public string ReqItemLabel { get; set; } //UAT-4705

        private String GetXml()
        {
            var serializer = new XmlSerializer(typeof(RequirementVerificationQueueContract));
            var sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, this);
            }
            return sb.ToString();
        }


        public List<String> FilterColumns { get; set; }
        public List<String> FilterOperators { get; set; }
        public ArrayList FilterValues { get; set; }
        public List<String> FilterTypes { get; set; }
    }

    [Serializable]
    public class RFQSelectedDataContract
    {
        public Int32 OrganizationUserId { get; set; }
        public Int32 RotationId { get; set; }
        public Int32 RPSId { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ReqPkgSubscriptionIDList
    {
        [DataMember]
        public Int32 RequirementPackageSubscriptionID { get; set; }
        [DataMember]
        public Int32 ApplicantRequirementItemId { get; set; }
        [DataMember]
        public Int32 RequirementItemId { get; set; }
        [DataMember]
        public Int32 ApplicantRequirementCategoryId { get; set; }
        [DataMember]
        public Int32 RotationId { get; set; }
        [DataMember]
        public Int32 ApplicantId { get; set; }
        [DataMember]
        public Int32 RequirementCategoryID { get; set; }
        [DataMember]
        public Int32 TenantID { get; set; }
        [DataMember]
        public Int32 AgencyId { get; set; }
        [DataMember]
        public Int32 NextSubscriptionID { get; set; }
        [DataMember]
        public Int32 NextApplicantRequirementItemId { get; set; }
        [DataMember]
        public Int32 NextApplicantRequirementCategoryId { get; set; }
        [DataMember]
        public Int32 NextRequirementItemId { get; set; }
        [DataMember]
        public Int32 NextClinicalRotationID { get; set; }
        [DataMember]
        public Int32 NextOrganizationUserID { get; set; }
        [DataMember]
        public Int32 NextRequirementCategoryID { get; set; }
        [DataMember]
        public Int32 NextTenantID { get; set; }
        [DataMember]
        public Int32 NextAgencyId { get; set; }
        [DataMember]
        public Int32 PrevSubscriptionID { get; set; }
        [DataMember]
        public Int32 PrevApplicantRequirementItemId { get; set; }
        [DataMember]
        public Int32 PrevApplicantRequirementCategoryId { get; set; }
        [DataMember]
        public Int32 PrevRequirementItemId { get; set; }
        [DataMember]
        public Int32 PrevClinicalRotationID { get; set; }
        [DataMember]
        public Int32 PrevOrganizationUserID { get; set; }
        [DataMember]
        public Int32 PrevRequirementCategoryID { get; set; }
        [DataMember]
        public Int32 PrevTenantID { get; set; }
        [DataMember]
        public Int32 PrevAgencyId { get; set; }

    }

    [Serializable]
    [DataContract]
    public class ManageReqPkgSubscriptionContract
    {
        [DataMember]
        public ReqPkgSubscriptionIDList PreviousSubscription { get; set; }
        [DataMember]
        public ReqPkgSubscriptionIDList CurrentSubscription { get; set; }
        [DataMember]
        public ReqPkgSubscriptionIDList NextSubscription { get; set; }
    }
}
