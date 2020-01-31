using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IInvitationDetailsView
    {
        Int32 CurrentLoggedInUserId { get; }
        Int32 TenantId { get; set; }

        String ErrorMessage { get; set; }
        String SuccessMessage { get; set; }

        Int32 SelectedTenantId { get; set; }
        String SelectedTenantName { get; set; }
        IInvitationDetailsView CurrentViewContext { get; }
        List<Tenant> lstTenant { set; }
        List<LookupContract> lstInviteeType { set; }
        List<InvitationDataContract> lstInvitationQueue { get; set; }
        InvitationSearchContract SetSearchContract { set; }
        Dictionary<Int32, String> SelectedInvitationIds { get; set; }
        List<InvitationDocumentContract> DocumentListToExport { get; set; }
        List<InvitationDocumentContract> PassportReportData { get; set; }
        List<InvitationDocumentContract> AttestationDocumentData { get; set; }
        String InviteeNameSearch { get; }
        String EmailAddressSearch { get; }
        String PhoneNumberSearch { get; }
        String NotesSearch { get; }
        DateTime? ExpirationDateFrom { get; }
        DateTime? ExpirationDateTo { get; }
        DateTime? InvitationDateFrom { get; }
        DateTime? InvitationDateTo { get; }
        DateTime? LastViewedDateFrom { get; }
        DateTime? LastViewedDateTo { get; }
        List<String> SelectedInviteeTypeCode { get; set; }
        String SelectedReviewStatusCode { get; }

        #region Custom Paging Parameters

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 VirtualPageCount
        {
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        #endregion

        Boolean IsControlVisible { get; set; }

        List<SharedUserInvitationReviewStatusContract> lstInvitationReviewStatus
        {
            set;
        }

        Int32 SelectedReviewStatusID { get; set; }
        List<TenantDetailsContract> lstSelectedTenants { get; }
        List<InvitationIDsDetailContract> lstInvitationIDsDetailContract { get; set; }

        #region UAT-1641:As an Agency User, I should be able to be linked to multiple agencies
        String UserID { get; }
       
        List<AgencyDetailContract> lstAgency
        {
            set;
        }

        String lstSelectedAgencyIds
        {
            get;
        }
        #endregion
    }
}
