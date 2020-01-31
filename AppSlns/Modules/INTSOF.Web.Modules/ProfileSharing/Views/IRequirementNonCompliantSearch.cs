using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using System;
using System.Collections.Generic;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IRequirementNonCompliantSearch
    {
        List<RequirementSharesDataContract> RotationAndInvitationData
        {
            get;
            set;
        }

        //UAT-3425
        List<RequirementSharesDataContract> SelectedInvitations
        {
            get;
            set;
        }



        Int32 TenantID
        {
            get;
            set;
        }

        List<TenantDetailContract> lstTenants
        {
            get;
            set;
        }

        List<TenantDetailContract> lstSelectedTenants
        {
            get;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }

        List<WeekDayContract> WeekDayList
        {
            set;
        }

        List<SharedUserRotationReviewStatusContract> RotationStatusList
        {
            set;
        }

        ClinicalRotationDetailContract ClinicalRotationSearchContract
        {
            get;
            set;
        }

        InvitationSearchContract InvitationSearchContract
        {
            get;
            set;
        }

        List<String> SharedUserTypeCodes { get; }

        String UserID { get; }

        #region Custom paging parameters
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

        Int32 VirtualPageCount
        {
            get;
            set;
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

        List<SharedUserRotationReviewStatusContract> lstRotationReviewStatus
        {
            set;
        }

        Int32 SelectedReviewStatusID { get; set; }

        List<AttestationDocumentContract> LstInvitationDocumentContract { get; set; }

        Boolean IsControlVisible { get; set; }

        Boolean IsInstructor { get; }

        List<AgencyDetailContract> lstAgency
        {
            set;
        }

        String lstSelectedAgencyIds
        {
            get;
        }

        List<LookupContract> lstInviteeType { set; }

        List<String> SelectedInviteeTypeCode { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        Dictionary<Int32, String> SelectedInvitationIds { get; set; }
        //UAT-2035
        List<Int32> SelectedRotationIds { get; set; }
        List<InvitationIDsDetailContract> lstInvitationIDsDetailContract { get; set; }

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        List<InvitationDocumentContract> DocumentListToExport { get; set; }
        List<InvitationDocumentContract> PassportReportData { get; set; }

        List<InvitationDocumentContract> AttestationDocumentData { get; set; }

        List<SharedUserInvitationReviewStatusContract> lstInvitationReviewStatus
        {
            set;
        }

        // List<Tuple<Int32, Int32, String>> DicRotationTenantIDs { get; set; } //Replacing tuple by contract as per the requirement of UAT-2475

        //ClinicalRotationDetailContract ViewContract
        //{
        //    get;
        //}

        //UAT-2183
        Boolean IsReturnFromStudentDetailScren { get; set; }

        List<InvitationIDsDetailContract> lstRotationIdsDetailContract { get; set; }  //UAT:2475

        //UAT-2538
        String CurrentUserFirstName
        {
            get;
        }
        String CurrentUserLastName
        {
            get;
        }
        ////UAT-2519
        String SearchApplicantName
        {
            get;
            set;
        }

        Int32 OrganizationUserId { get; }

        String StrSelectedTenantIds
        {
            get;
            set;
        }

        //UAT-3211
        Boolean IsAdvanceSearchPanelDisplay { get; set; }

        Boolean HideDetailLink { get; } //UAT-3220

        #region UAT-3470
        List<Entity.SharedDataEntity.lkpInvitationArchiveState> lstInvitationArchiveState { set; }
        List<String> SelectedInvitationArchiveStatusCode { get; set; }
        #endregion

        Dictionary<Int32, String> SelectedUsersForEmail { get; set; }
    }
}
