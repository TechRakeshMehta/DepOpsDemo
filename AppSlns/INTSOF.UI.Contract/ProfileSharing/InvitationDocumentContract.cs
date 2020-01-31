using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class InvitationDocumentContract
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public Int32 ApplicantID { get; set; }
        public Int32 ProfileSharingInvitationID { get; set; }
        public String ApplicantName { get; set; }
        public String FileName { get; set; }
        public String ItemName { get; set; }
        public Int32 ComplianceCategoryID { get; set; }
        public String CategoryName { get; set; }
        public Int32 ApplicantDocumentID { get; set; }
        public String DocumentPath { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public Int32 TotalCount { get; set; }
        public Int32 SnapshotID { get; set; }
        public Int32? ItemAttributeID { get; set; }
        public Int32? CategoryItemID { get; set; }
        public Int32 DocMapID { get; set; }
        public Boolean IsExceptionDoc { get; set; }
        public Boolean IsInvitationSourceApplicant { get; set; }
        public Int32 PackageSubscriptionID { get; set; }
        public Int32 CompliancePackageID { get; set; }
        public String PackageName { get; set; }
        public Int32 MasterOrderID { get; set; }
        public Int32 BkgSvcGroupID { get; set; }
        public Boolean IsFlagged { get; set; }
    }

    [Serializable]
    public class InvitationIDsContract
    {
        public Int32 ProfileSharingInvitationID { get; set; }
        public Boolean IsInvitationSourceApplicant { get; set; }
        public Int32 TenantId { get; set; } //UAT:2475
    }
}
