using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ClinicalRotation;

namespace INTSOF.UI.Contract.ProfileSharing
{
    /// <summary>
    /// Contains the Data related to particular Invitation, during CRUD operations by Applicant
    /// </summary>
    [Serializable]
    public class InvitationDetailsContract
    {
        /// <summary>
        /// Name of the Applicant
        /// </summary>
        public String ApplicantName
        {
            get;
            set;
        }

        /// <summary>
        /// Used for resending the Invitation Email .
        /// </summary>
        public Guid? InvitationToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32 ApplicantId { get; set; }

        /// <summary>
        /// This can be null when the invitation is sent. While saving the New invitation, if it exists, then use that id.
        /// </summary>
        public Int32? InviteeOrgUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32 CurrentUserId { get; set; }


        /// <summary>
        /// AgencyId, to be used during the Invitations being sent by Admin or Client admin
        /// </summary>
        public Int32? AgencyId { get; set; }

        /// <summary>
        /// AgencyUserId, to be used during the Invitations being sent by Admin or Client admin
        /// </summary>
        public Int32? AgencyUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32 PSIId { get; set; }

        /// <summary>
        /// ProfileSharingInvitationGroupID
        /// </summary>
        public Int32 PSIGroupId { get; set; }

        public Guid InvitationIdentifier { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32 PreviousPSIId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public String EmailAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Phone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String Agency { get; set; }

        /// <summary>
        /// TenantId
        /// </summary>
        public Int32 TenantID { get; set; }

        /// <summary>
        /// Code field of 'lkpInvitationExpirationType' table
        /// </summary>
        public String ExpirationTypeCode { get; set; }

        /// <summary>
        /// PK field of 'lkpInvitationExpirationType' table
        /// </summary>
        public Int32 ExpirationTypeId { get; set; }

        /// <summary>
        /// PK field of 'lkpinvitationStatus' table
        /// </summary>
        public Int32 InvitationStatusId { get; set; }

        /// <summary>
        /// Code field of 'lkpinvitationStatus' table
        /// </summary>
        public String InvitationStatusCode { get; set; }

        /// <summary>
        /// Code field of 'lkpinvitationsource' table
        /// </summary>
        public String InvitationSourceCode { get; set; }

        /// <summary>
        /// PK field of 'lkpInvitationStatus' table
        /// </summary>
        public Int32 InvitationSourceId { get; set; }

        /// <summary>
        /// Maximum views allowed for invitation viewing
        /// </summary>
        public Int32? MaxViews { get; set; }

        /// <summary>
        /// Expiration date of the invitation
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// List of the Codes of the MetaData shared by the applicant, in the invitation
        /// </summary>
        public List<String> SharedApplicantMetaDataCode { get; set; }

        /// <summary>
        /// List of the ID's of Meta Data shared by the applicant, in the invitation
        /// </summary>
        public List<Int32> SharedApplicantMetaDataIds { get; set; }

        /// <summary>
        /// Message to be sent by the Applicant
        /// </summary>
        public String CustomMessage { get; set; }

        /// <summary>
        /// Current DateTime
        /// </summary>
        public DateTime CurrentDateTime { get; set; }

        /// <summary>
        /// List of the Compliance Data shared. 
        /// </summary> 
        public List<ComplianceInvitationData> lstComplianceData { get; set; }

        /// <summary>
        /// List of the Background Packages shared.
        /// </summary> 
        public List<BkgInvitationData> lstBkgData { get; set; }

        /// <summary>
        /// List of the Requirement Package Data shared.
        /// </summary> 
        public List<RequirementInvitationData> lstRequirementData { get; set; }


        /// <summary>
        /// Decides whether the previous invitation status is to be updated, when new invitation is sent 
        /// during updating the existing invitation.
        /// </summary>
        public Boolean IsStatusUpdateRequired
        {
            get;
            set;
        }

        public Dictionary<String, String> TemplateData { get; set; }

        public Boolean IsEmailSentSuccessfully { get; set; }

        /// <summary>
        /// Identify whether the invitation is being sent through the Rotation sharing
        /// </summary>
        public Boolean IsRotationType { get; set; }

        /// <summary>
        /// Represnts the Invitee UserType ID
        /// </summary>
        public Int32 InviteeUserTypeID { get; set; }

        /// <summary>
        /// Represnts the Invitee UserType Code
        /// </summary>
        public String InviteeUserTypeCode { get; set; }

        public Int32 ClientContactID { get; set; }

        /// <summary>
        /// Invitation Schedule Date, when 'Submit Later' is used
        /// </summary>
        public DateTime? InvitationScheduleDate
        {
            get;
            set;
        }

        public int InviteeViewCount { get; set; }

        public ClinicalRotationMemberDetail RotationDetail { get; set; }

        #region UAT 1882 Phase 3(12): When a student profile shares, they should be presented with a selectable list of agencies, which have been associated with nodes they have orders with
        public Boolean IsAgencyComboboxSelected { get; set; }

        public Int32 SelectedAgencyID { get; set; }

        public String SelectedAgencyName { get; set; }

        public string ExpireOption
        {
            get;
            set;
        }
        #endregion
        //UAT-1895 Added new property to check the Audit Requested Invitation selection
        public Boolean ClearAuditRequestFlag { get; set; }

        //UAT-2447
        public Boolean IsInternationalPhone
        {
            get;
            set;
        }

        #region UAT-3006
        public String SchoolContactName
        {
            get;
            set;
        }
        public String SchoolContactEmailID
        {
            get;
            set;
        }
        #endregion

        #region UAt-3470
        public Int32 InvitationArchiveStateID { get; set; }
        #endregion

        public Boolean PSI_IsInstructorShare { get; set; }
    }

    [Serializable]
    public class ComplianceInvitationData
    {
        public String PkgName { get; set; }

        /// <summary>
        /// PackageSubscriptionId
        /// </summary>
        public Int32 PkgSubId { get; set; }

        /// <summary>
        /// SnapshotId - SnapShotID of particular subscription, when invitation is sent by admin or client admin. 
        /// </summary>
        public Int32? SnapShotId { get; set; }

        public Boolean IsCompletePkgSelected { get; set; }

        public List<Int32> lstCategoryIds { get; set; }

        /// <summary>
        /// Is True if any of the categories are selected.
        /// </summary>
        public Boolean IsAnyCatSelected { get; set; }

        /// <summary>
        /// PK field of 'lkpInvitationSharedInfoType' table
        /// </summary>
        public Int32? ComplianceSharedInfoTypeId { get; set; }

        /// <summary>
        /// Code field of 'lkpInvitationSharedInfoType' table
        /// </summary>
        public String ComplianceSharedInfoTypeCode { get; set; }

        public String CategoryNames { get; set; }

        public Int32 ApplicantUserID { get; set; }

        public Int32 CompPkgID { get; set; }
    }

    [Serializable]
    public class BkgInvitationData
    {
        public String PkgName { get; set; }

        public Int32 BOPId { get; set; }

        public List<Int32> lstSvcGrpIds { get; set; }

        /// <summary>
        /// Is True if any of the Service groups are selected.
        /// </summary>
        public Boolean IsAnySvcGrpSelected { get; set; }

        /// <summary>
        /// PK field of 'lkpInvitationSharedInfoType' table
        /// </summary>
        public Int32? BkgSharedInfoTypeId { get; set; }

        /// <summary>
        /// Code field of 'lkpInvitationSharedInfoType' table
        /// </summary>
        public String BkgSharedInfoTypeCode { get; set; }

        //UAT-1213 - List of Bkg Shared Info Type Codes
        public List<Int32> LstBkgSharedInfoTypeId { get; set; }
        public List<String> LstBkgSharedInfoTypeCode { get; set; }

        public String SvcGroupNames { get; set; }

        public Int32 ApplicantUserID { get; set; }
    }

    [Serializable]
    public class RequirementInvitationData
    {
        public String RequirementPkgName { get; set; }

        /// <summary>
        /// PackageSubscriptionId
        /// </summary>
        public Int32 RequirementPkgSubId { get; set; }

        /// <summary>
        /// SnapshotId - SnapShotID of particular Requirement subscription, when invitation is sent by admin or client admin. 
        /// </summary>
        public Int32? RequirementSnapShotId { get; set; }

        /// <summary>
        /// List of the Redquirement Package CategoryID's
        /// </summary>
        public List<Int32> lstRequirementCategoryIds { get; set; }

        /// <summary>
        /// PK field of 'lkpInvitationSharedInfoType' table
        /// </summary>
        public Int32 RequirementPkgSharedInfoTypeId { get; set; }

        /// <summary>
        /// Code field of 'lkpInvitationSharedInfoType' table
        /// </summary>
        public String RequirementPkgSharedInfoTypeCode { get; set; }

        public Int32 ApplicantUserID { get; set; }
    }

    public class InvitationSharedInfoDetails
    {
        public Int32 SharingInvitationID { get; set; }

        public String ComplianceSharedInfoTypeCode { get; set; }

        public String BkgSharedInfoTypeCode { get; set; }

        public String ReqRotSharedInfoTypeCode { get; set; }

        //UAT-1213
        public List<String> LstBkgSharedInfoTypeCode { get; set; }

        public Guid InvitationIdentifier { get; set; }

        public Int32 InvitationDocumentID { get; set; }

        public String DocumentPath { get; set; }

        public Boolean IsForEveryOneAttestationForm { get; set; }

    }

    /// <summary>
    /// UAT-3715
    /// </summary>
    public class InvitationAttestationDocumentDataWithAgencyUserPermissions
    {
        public Int32 InvitationDocumentID { get; set; }

        public String DocumentPath { get; set; }

        public String AgencyUserPermissionCode { get; set; }
    }

}
