using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class BadgeFormNotificationDataContract
    {
        public Int32 BadgeFormNotificationID { get; set; }
        public Int32 ItemDataId { get; set; }
        public Int32 ProfileSharingInvitationGroupID { get; set; }
        public String ItemTypeCode { get; set; }
        public String PackageName { get; set; }
        public String CategoryName { get; set; }
        public String ItemName { get; set; }
        public Int32 AppOrgUserID { get; set; }
        public Int32 ItemID { get; set; }
        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }
        public String ApplicantEmailID { get; set; }
        public String BadgeFormDocumentIDs { get; set; }
        public bool IsItemApproved { get; set; }
        public bool IsShareApproved { get; set; }
        public Int32 SystemCommunicationID { get; set; }
        public List<Entity.ClientEntity.ApplicantDocument> lstApplicantDocuments { get; set; }
        public BadgeFormDocumentDataContract badgeFormDocumentData { get; set; }
        public bool IsRecordUpdated { get; set; }
        public bool IsDataByItemApproval { get; set; }
        public Int32 SharedReqItemId { get; set; }
        //UAT-3254
        public Int32 SelectedNodeID { get; set; }
        public String RotationHierarchyIds { get; set; }
    }
}
