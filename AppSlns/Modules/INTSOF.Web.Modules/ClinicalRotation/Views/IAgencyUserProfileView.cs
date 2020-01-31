using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.ProfileSharing;
using Entity.SharedDataEntity;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IAgencyUserProfileView
    {
        Int32 TenantID { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        Guid UserID { get; }
        Boolean SuccessMsg { get; set; }
        OrganizationUserContract OrganizationUser { get; set; }
        AgencyUserContract AgencyUserDetails { get; set; }
        List<lkpInvitationSharedInfoType> LstSharedInfoType { get; set; }
        Boolean IsMasterAgencyUser { get; set; }
        Boolean IsApplicantsSharedUser { get; set; }
        Int32 OrganisationUserID { get; }
    }
}
