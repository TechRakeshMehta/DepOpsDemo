using Entity.SharedDataEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using CoreWeb.CommonControls.Views;
using CoreWeb.IntsofSecurityModel;
using INTSOF.Utils.CommonPocoClasses;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IManageAgencySharing
    {
        Int32 SelectedTenantID { get; set; }
        Int32 ClientTenantID { get; set; }
        Int32 CurrentUserID { get; }
        SysXMembershipUser CurrentUser { get; }
        Int32 SelectedHierarchyID { get; set; }
        String SelectedHierarchyIDs { get; set; } //UAT-1055
        Int32 SelectedAgencyID { get; set; }
        Int32 SelectedUserGroupID { get; set; }
        Int32 OrganizationUserID { get; }
        String ApplicantFirstName { get; }
        String ApplicantLastName { get; }
        String EmailAddress { get; }
        String SSN { get; set; }
        String ErrorMessage { get; set; }
        String SuccessMessage { get; set; }
        String InfoMessage { get; set; }
        String ProfileSharingURL { get; }
        DateTime? DOB { get; }
        DateTime? OrderPaidFromDate { get; }
        DateTime? OrderPaidToDate { get; }
        Boolean IsAdminLoggedIn { get; set; }
        Dictionary<Int32, Boolean> AssignOrganizationUserIds { get; set; }
        List<Entity.Tenant> lstTenant { get; set; }
        List<Entity.ClientEntity.UserGroup> lstUserGroup { get; set; }
        List<Agency> lstAgency { get; set; }
        List<AgencySharingDataContract> AgencySharingData { get; set; }
        SearchItemDataContract SearchItemDataContract { get; set; }

        #region INSTITUTION HIERARCHY
        String CustomFields { get; set; }
        #endregion

        #region FOR GRANULAR PERMISSION
        Boolean IsDOBDisable { get; set; }
        String SSNPermissionCode { get; set; }
        #endregion

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
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 VirtualRecordCount
        {
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract AgencyDataGridCustomPaging
        {
            get;
        }

        #endregion

        #region ATTESTATION FORM PROPERTIES
        String SchoolName { get; }
        String CurrentAdminName { get; }
        DateTime? AttestationDate { get; }
        String ProgramName { get; }
        String AssignedUnits { get; }
        DateTime? ClinicalDateFrom { get; }
        DateTime? ClinicalDateTo { get; }
        byte[] Signature { get; }
        //UAT:1219: Display and make editable attestation text on the Manage Agency Sharing screen.
        Dictionary<Int32, String> AttestationReportText { get; }
        #endregion

        /// <summary>
        /// Url of the Central Login
        /// </summary>
        String CentralLoginUrl
        {
            get;
        }

        /// <summary>
        /// Name of the Selected Tenant
        /// </summary>
        String InstitutionName
        {
            get;
        }

        //UAT-1201 Invitation Group
        List<ProfileSharingInvitationGroupContract> LstInvitationGroup { get; set; }
        Int32 SelectedInvitationGroupID { get; set; }
        List<InvitationDocument> LstInvitationDocument { get; set; }

        /// <summary>
        /// Id of the Rotation, for which Profile sharing is being done - Used for Clinical Rotation integration
        /// </summary>
        Int32 RotationId
        {
            get;
            set;
        }

        /// <summary>
        /// Identify from which screen the Profile Sharing is being opened. Will be NULL for Normal Profile Sharing
        /// </summary>
        String SrcScreen
        {
            get;
            set;
        }

        /// <summary>
        /// Returns whether the current Profile Sharing is through the Rotation module.
        /// </summary>
        Boolean IsRotationSharing
        {
            get;
        }

        /// <summary>
        /// Name of the Agency selected
        /// </summary>
        String SelectedAgencyName
        {
            get;
        }

        /// <summary>
        /// CSV List of the RotationMemberIDs which were added in Rotation and selected for Profile sharing, From rotation details.
        /// </summary>
        String RotationMemberIds
        {
            get;
        }

        ClinicalRotationDetailContract ClinicalRotationDetail
        {
            get;
            set;
        }

        /// <summary>
        /// Will be 'True' if 'Submit Now' is used else 'False'
        /// </summary>
        Boolean IsNonScheduledInvitation
        {
            get;
            set;
        }

        /// <summary>
        /// Invitation Schedule Date, when 'Submit Later' is used
        /// </summary>
        DateTime? InvitationSchedlueDate
        {
            get;
            set;
        }

        IProfileExpirationCriteriaView ProfileExpirationCriteria
        {
            get;
            set;
        }

        /// <summary>
        /// List of Packages and thei selected categories/Service groups 
        /// </summary>
        List<SharingPackageDataContract> lstSharedPkgData
        {
            get;
            set;
        }

        /// <summary>
        /// UAT 1530: WB: If sharing with an agency that does not have any users, 
        /// client admin should have to fill out a form displaying the information of the person they would like to add. 
        /// </summary>
        Boolean IsAgencyUserExistInAgency { get; set; }

        /// <summary>
        /// UAT 1522
        /// </summary>
        // List<Tuple<Int32, String, String>> AttestationReportTextForAgency { get; set; }

        /// <summary>
        /// UAT-2529 - Check Agency ProfileSharing Setting
        /// </summary>
        Boolean IsAdminProfileSharingPermission { get; set; }

        Boolean ChildHighlightRotationFieldUpdatedByAgencies { get; set; } //UAT:2666

        String AgencyIDs { get; set; }

        Boolean SendNotificationToSchoolAdmin
        {
            get;
            set;
        }

        List<AgencyAttestationDetailContract> LstAttestationReportTextForAgency { get; set; }
        Dictionary<Int32, String> AttestationFormDocument { get; }
        Boolean IsPermissionTrueForAllAgency { get; set; }
        List<ApplicantDocumentPocoClass> AttestationDocument { get; set; }

        //UAT-3977
        /// <summary>
        /// CSV List of the InstructorPreceptorOrgUserIds which were added in Rotation and selected for Profile sharing, From rotation details.
        /// </summary>
        String InstructorPreceptorOrgUserIds
        {
            get;
        }


    }
}
