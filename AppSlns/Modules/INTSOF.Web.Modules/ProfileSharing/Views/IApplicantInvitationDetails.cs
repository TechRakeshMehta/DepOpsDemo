using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ProfileSharing;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IApplicantInvitationDetails
    {

        IApplicantInvitationDetails CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// OrganizationUserId of the Current Loggedin Applicant
        /// </summary>
        Int32 OrgUserId
        {
            get;
        }

        /// <summary>
        /// Name of the Current Logged-in Applicant
        /// </summary>
        String ApplicantName
        {
            get;
        }

        /// <summary>
        /// Id of Tenant of the Applicant
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Id of Invitation being edited
        /// </summary>
        Int32 InvitationId
        {
            get;
            set;
        }

        /// <summary>
        /// List of Master expiration types i.e. lkpInvitationExpirationType
        /// </summary>
        List<lkpInvitationExpirationType> lstExpirationTypes
        {
            set;
        }

        /// <summary>
        /// List of applicant metadata which can be shared with the Shared user.
        /// </summary>
        List<ApplicantInvitationMetaData> lstMetaData
        {
            set;
        }

        /// <summary>
        /// List of Lookup for lkpInvitationSharedInfoType
        /// </summary>
        List<Entity.ClientEntity.lkpInvitationSharedInfoType> lstShardInfoType
        {
            get;
            set;
        }

        /// <summary>
        /// List of Sharing Packages. Includes both Complianvce and Background packages.
        /// </summary>
        List<ProfileSharingPackages> lstSharingPackages
        {
            set;
            get;
        }

        /// <summary>
        /// List of Sharing Packages. Includes both Complianvce and Background packages.
        /// </summary>
        List<InvitationDataContract> lstInvitationsSent
        {
            set;
            get;
        }

        /// <summary>
        /// Data contract to manage the CRUD operations
        /// </summary>
        InvitationDetailsContract InvitationData
        {
            get;
            set;
        }

        /// <summary>
        /// Url of the Profile sharing site
        /// </summary>
        String ProfileSharingUrl
        {
            get;
        }

        /// <summary>
        /// Url of the Central Login
        /// </summary>
        String CentralLoginUrl
        {
            get;
        }

        Int32 CurrentAgencyID
        {
            get;
            set;
        }

        /// <summary>
        /// Decides whether new invitation is required while updating any existing.
        /// </summary>
        Boolean IsNewInvitationRequired
        {
            get;
            set;
        }
         
        /// <summary>
        /// PreviousInvitationId
        /// </summary>
        Int32 PreviousPSIId
        {
            get;
            set;
        }

        /// <summary>
        /// PreviousEmailAddress
        /// </summary>
        String PreviousEmailAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Stores whether a Valid shared user email address was entered
        /// </summary>
        Boolean IsSharedUserInvited
        {
            get;
            set;
        }

        /// <summary>
        /// Whether the invitation is sent successfully.
        /// </summary>
        Boolean IsInvitationSent
        {
            get;
            set;
        }

        /// <summary>
        /// Check and return OrganizationUserID if invitee already exists as an applicant or client admin
        /// </summary>
        Int32 ExistingUserOrgUserID
        {
            get;
            set;
        }

        Int32 AppViewOrgUsrID { get; }

        //UAT-1883: Phase 3(13): Capture of additional fields
        List<WeekDayContract> WeekDayList { get; set; }


        List<AgencyContract> lstAgencies { get; set; }

        List<AttestationDocumentContract> LstInvitationDocumentContract { get; set; }

        List<RotationAndTrackingPkgStatusContract> LstErrorMessages { get; set; }
        //UAT-4472
        AgencyHierarchyRotationFieldOptionContract agencyHierarchyRotationFieldOptionContract { get; set; }
        Boolean ShowAgencySuggestion { get; set; }
        //UAT-2466
        Boolean ShowRotationStartDateForIndividualShares { get; set; }
        //UAT-2466
        Boolean ShowRotationEndDateForIndividualShares { get; set; }

        #region UAT-2529
        Boolean IsStudentProfileSharingPermission { get; set; }
        String AgencyUserEmail { get; set; }
        String AgencyEmail { get; set; }
        #endregion
        
    }
}
