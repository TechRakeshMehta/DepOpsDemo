using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IScheduleInvitation
    {
        String DelimittedTrackingPkgIDs { get; set; }

        Int32 RotationID { get; set; }

        Int32 TenantID { get; set; }

        String DelimittedOrgUserIDs { get; set; }

        Int32 AgencyID { get; set; }

        ProfileSharingExpiryContract ProfileSharingExpiryContract { get; set; }
    }
}
