using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class SharedUserInvitationDocumentContract
    {
        public Int32 InvitationDocumentID { get; set; }
        public Int32 ProfileSharingInvitationGroupID { get; set; }
        public Int32 AgencyID { get; set; }
        public Int32 TenantID { get; set; }
        public Int32 ClinicalRotationID { get; set; }
        public Int32 ApplicantOrgUserID { get; set; }
        public String FileName { get; set; }
        public String Description { get; set; }
        public String MD5Hash { get; set; }
    }
}
