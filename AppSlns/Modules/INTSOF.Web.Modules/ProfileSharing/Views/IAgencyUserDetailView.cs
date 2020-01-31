using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IAgencyUserDetailView
    {
        Int32 TenantId { get; }

        Int32 AgencyId { get; set; }

        Int32 AgencyUserId { get; }

        AgencyUserContract AgencyUserInfo { get; set; }

        List<Agency> LstAgency
        {
            get;
            set;
        }

        List<ApplicantInvitationMetaData> LstSharedInfo
        {
            get;
            set;
        }

        List<AgencyUserTenantCmbContract> LstAgencyInstitutions
        {
            get;
            set;
        }


        List<lkpInvitationSharedInfoType> LstSharedInfoType
        {
            get;
            set;
        }

        Boolean AttestationRptPermission { get; set; }

        Int32 AgencyUserPermissionTypeId { get; set; }

        Int32 AgencyUserPermissionAccessTypeId { get; set; }


        List<Int32> lstApplicationInvitationMetaDataID { get; set; }

        List<InvitationSharedInfoMapping> lstInvitationSharedInfoType { get; set; }

        AgencyUserPermissionContract UserPermissionStatus { get; set; }
              
        Int32 OrganizationUserId { get; }

        Int32 CurrentLoggedInUserId { get; }

        Boolean IsLocked { get; set; }

        Boolean IsActive { get; set; }

        String EmailAddress { get; set; }

        String FirstName { get; set; }

        String LastName { get; set; }

        String Password { get; set; }
    }
}
