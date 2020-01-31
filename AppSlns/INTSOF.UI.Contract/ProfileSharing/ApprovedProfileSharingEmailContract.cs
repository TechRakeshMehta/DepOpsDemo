using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class ApprovedProfileSharingEmailContract
    {
        public Entity.SharedDataEntity.ProfileSharingInvitation ProfileSharingInvitationDetails { get; set; }
        public Int32 ProfileSharingInvitationID { get; set; }
        public Entity.ClientEntity.ClinicalRotation RotationDetails { get; set; }
        public List<Entity.OrganizationUser> GetAgencyUserList { get; set; }
        public Entity.SharedDataEntity.Agency AgencyDetails { get; set; }
        public Int32 TenantID { get; set; }
        public String TenantName { get; set; }

        public Entity.OrganizationUser ApplicantDetails { get; set; }
    }
}
