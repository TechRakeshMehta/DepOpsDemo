using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IManageInvitationExpirationView
    {
        IManageInvitationExpirationView CurrentViewContext { get; }
        List<ProfileSharingInvitationSearchContract> lstInvitationQueue { get; set; }
        List<Int32> SelectedInvitationIds { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 TenantId { get; set; }
        List<TenantDetailContract> lstTenants { set; }
        Boolean IsAdminLoggedIn { get; set; }
        String ErrorMessage { get; set; }
        String SuccessMessage { get; set; }
        ProfileSharingInvitationSearchContract SearchContract { get; set; }
        ProfileSharingInvitationSearchContract ExpirationCriteriaDetail { get; set; }

        #region Custom Paging Parameters
        Int32 CurrentPageIndex
        {
            get;
            set;
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

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }
        #endregion
    }
}
