using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;

namespace CoreWeb.ProfileSharing.Views
{
    public interface ISharedInvitations
    {
        Boolean IsReset { get; set; }

        Int32 SelectedTenantID { get; set; }

        Int32 SelectedAgencyID { get; set; }

        List<Entity.Tenant> lstTenant { get; set; }

        List<AgencyDetailContract> lstAgency { get; set; }

        Int32 ClientTenantID { get; set; }

        Boolean IsAdminLoggedIn { get; set; }

        Int32 CurrentUserID { get; }

        List<ProfileSharingInvitationGroupContract> LstInvitationGroup { get; set; }

        Int32 SelectedInvitationGroupID { get; set; }

        List<InvitationDocument> LstInvitationDocument { get; set; }

        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        SharedInvitationContract SearchContract
        {
            get;
        }

        Int32 PageSize
        {
            get;
            set;
        }

        Int32 VirtualRecordCount
        {
            get;
            set
           ;
        }
    }
}
