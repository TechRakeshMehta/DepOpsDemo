using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRotationDetailFormView
    {

        Int32 ClinicalRotationID
        {
            get;
            set;
        }

        ClinicalRotationDetailContract ClinicalRotationDetails
        {
            get;
            set;
        }

        List<ApplicantDataListContract> ApplicantSearchData
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        String ApplicantFirstName
        {
            get;
            set;
        }

        String ApplicantLastName
        {
            get;
            set;
        }

        String EmailAddress
        {
            get;
            set;
        }

        String SSN
        {
            get;
            set;
        }

        DateTime? DateOfBirth
        {
            get;
            set;
        }

        List<UserGroupContract> lstUserGroup
        {
            get;
            set;
        }

        List<RequirementPackageContract> lstTenantRequirementPackage
        {
            get;
            set;
        }

        List<RequirementPackageContract> lstSharedRequirementPackage
        {
            get;
            set;
        }

        List<RequirementPackageContract> lstSharedInstructorRequirementPackages
        {
            get;
            set;
        }

        List<RequirementPackageContract> lstCombinedRequirementPackage
        {
            get;
            set;
        }

        List<RequirementPackageContract> lstCombinedInstructorRequirementPackages
        {
            get;
            set;
        }

        ClinicalRotationRequirementPackageContract RotationRequirementPackage
        {
            get;
            set;
        }

        Int32 RequirementPackageID { get; set; }

        Int32 ReturnedRequirementPackageID { get; set; }

        Int32 FilterUserGroupId
        {
            get;
            set;
        }

        Int32 MatchUserGroupId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        Dictionary<Int32, Boolean> AssignOrganizationUserIds
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        Dictionary<Int32, Tuple<Boolean,Int32>> RemovedClinicalRotationMemberIds
        {
            get;
            set;
        }

        Dictionary<Int32, Boolean> CustomMessageOrgUserIds
        {
            get;
            set;
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

        String InfoMessage
        {
            get;
            set;
        }

        List<RotationMemberDetailContract> RotationMemberDetailList
        {
            get;
            set;
        }

        List<RotationAndTrackingPkgStatusContract> LstStatusMessages
        {
            get;
            set;
        }

        String SSNPermissionCode { get; set; }
        Boolean IsSearchClicked { get; set; }
        Boolean IsRotationPackageEligibleForSharing { get; set; }
        /// <summary>
        /// AgencyID for the Current Rotation
        /// </summary>
        Int32 AgencyId { get; set; }

        Boolean IsFromPackageWizard { get; set; }

        Boolean IsEditMode { get; set; }

        List<RequirementPackageContract> lstInstructorRequirementPackage
        {
            get;
            set;
        }


        ClinicalRotationRequirementPackageContract MappedInstructorRequirementPackage
        {
            get;
            set;
        }

        Int32 InstructorRequirementPackageID { get; set; }

        Int32 ReturnedInstRequirementPackageID { get; set; }
        Int32 InstPercepRequirementPackageID { get; set; } //UAT-3702

        Int32 OldInstRequirementPackageID { get; set; }

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
        Int32 VirtualRecordCount
        {
            get;
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

        //UAT-1629 : As a client admin, I should not be able to edit rotation packages
        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        /// <summary>
        /// Granular Permissions of the logged-in user.
        /// </summary>
        Dictionary<String, String> dicGranularPermissions
        {
            get;
            set;
        }

        String SelectedDPMIds { get; }

        //UAT-2090:Complete Question 4 (C5) from UAT-2052.
        String Notes
        { get; set; }


        Dictionary<Int32, String> lstSelectedOrgUserIDs { get; set; }

        //UAT-2544:
        /// <summary>
        /// Gets or Sets the value for approved rotation memebers.
        /// </summary>
        Dictionary<Int32, Boolean> ApprovedClinicalRotationMemberIdsToRemove
        {
            get;
            set;
        }
        //UAT-4460
        Dictionary<Int32, Boolean> ClinicalRotationMemberIdsToDrop
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or Sets the value for Rotation Start Date.
        /// </summary>
        DateTime? RotationStartDate
        {
            get;

            set;

        }

        /// <summary>
        /// Gets or Sets the value for Is Rotation Start.
        /// </summary>
        Boolean IsRotationStart
        {
            get;
            set;
        }
        String SourceScreen  //UAT-2313
        { get; set; }

        Boolean HighlightRotationFieldUpdatedByAgencies { get; set; } //UAT:2666

        String RotationAgencyIds
        {
            get;
            set;
        }

        String lstSelectedUserIDs { get; set; }

        //UAT-3053 : As an admin, after creating a rotation I should be directed to the rotation details screen of that rotation.
        Boolean IsDisplaySuccessMessage { get; set; }

        Boolean IsEditableByClientAdmin { get; set; } //UAT:3041

        Boolean IsEditableByAgencyUser { get; set; }  //UAT:3041

        Boolean IsApplicantPkgNotAssignedThroughCloning { get; set; }//UAT-3121
        Boolean IsInstructorPkgNotAssignedThroughCloning { get; set; }//UAT-3121

        #region UAT 4398
        List<Entity.SharedDataEntity.AgencyUser> LstAgencyUserByAgency
        {
            get;
            set;
        }

        List<RotationMemberDetailContract> lstSelectedRotationMembers
        {
            get;
            set;
        }
        #endregion
    }
}
