using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

namespace CoreWeb.CommonControls.Views
{
    public interface IProfileShareDetails
    {
        /// <summary>
        /// Represents the ID of the Rotation for which details are to be fetched.
        /// </summary>
        Int32 ClinicalRotationId { get; set; }
        
        Boolean IsStartDateRequired { get; set; }

        Boolean IsEndDateRequired { get; set; }

        /// <summary>
        /// Id of the Tenant to which the Rotation belongs to.
        /// </summary>
        Int32 TenantId { get; set; }

        Int32 InvitationID { get; set; }
        Int32 InvitationGroupID { get; set; }

        Int32 OrganisationUserID { get; }

        Int32 CurrentLoggedInUserId { get; }
        Boolean IsEditableByAgencyUser { get; set; }
        Boolean IsAgencyUser { get; set; }
        Boolean IsRestrictToLoadFresshData { get; set; }
        ProfileSharingInvitationDetailsContract ProfileSharingDetails { get; set; }
    }
}
