using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Entity.ClientEntity;
using System.Runtime.Serialization;

namespace INTSOF.UI.Contract.ProfileSharing
{
    /// <summary>
    /// Contract used to display the data in the Agency User grid.
    /// </summary>
    [Serializable]
    public class AgencyUserContract
    {
        public Int32 AGU_ID { get; set; }
        public String AGU_Name { get; set; }
        public String AGU_Email { get; set; }
        public String AGU_Phone { get; set; }
        public String AGU_TenantName { get; set; }
        public Int32? AGU_ComplianceSharedInfoTypeID { get; set; }
        public Int32? AGU_BkgSharedInfoTypeID { get; set; }
        //public Int32 AGU_AgencyID { get; set; }
        public List<Int32> LstAGU_AgencyID { get; set; }
        public List<Int32> LstNotVerifiedAGU_AgencyID { get; set; }
        public String AGU_UserID { get; set; }
        public List<Int32> AGU_TenantIDLst { get; set; }
        public String AGU_ComplianceSharedInfoTypeName { get; set; }
        public String AGU_BkgSharedInfoTypeName { get; set; }
        public List<Int32> lstApplicationInvitationMetaDataID { get; set; }
        public List<Int32> lstSelectedAGI_ID { get; set; }
        //UAT-1213: Updates to Agency User background check permissions.
        public List<Int32> lstInvitationSharedInfoTypeID { get; set; }
        public String SharedInfoTypeName { get; set; }
        public Int32 AGU_ReqRotationSharedInfoTypeID { get; set; }
        public String AGU_ReqRotationSharedInfoTypeName { get; set; }
        //UAT-1345 Add/Update permission to Manage Rotation and MAnafge Agency Users 
        public Boolean AGU_RotationPackagePermission { get; set; }
        public Boolean AGU_AgencyUserPermission { get; set; }
        public Boolean IsCreatedBySharedUser { get; set; }
        //UAT 1458 On the result grid of manage agency users, we should reflect the Agency that the user is associated with.
        public String AgencyName { get; set; }

        //UAT 1529 WB: Need to have active/inactive and locked/unlocked statuses for agency users on the manage agency users screen
        public Boolean IsActive { get; set; }
        public Boolean IsLocked { get; set; }

        //UAT 1616: WB: As an agency user, I should be able to manage my agency's attestation statement. 
        public Boolean AttestationRptPermission { get; set; }
        public Dictionary<Int32, List<Int32>> DicSelectedAgencyInstitutionMapping { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }


        public String AgnecyNames { get; set; }
        public String AgnecyIds { get; set; }
        public String ProfileSharedInformation { get; set; }

        public String TenantNames { get; set; }

        //UAT-1993:Add the user name to the Agency User detail screen from the client user search.
        public String UserName { get; set; }

        //Code commented for UAT-2803
        //UAT-2538
        // public Boolean IsEmailNeedToSend { get; set; }

        //UAT-2447
        public Boolean IsInternationalPhone { get; set; }

        //UAT-2641
        public Int32 CreatedByClientID { get; set; }
        public List<Int32> lstAgencyHierarchyIds { get; set; }
        public Dictionary<Int32, String> SelectedAgencyHieararchyMapping { get; set; }
        //UAT-2663
        public Boolean IsAutoAgencyActivation { get; set; }
        public String Password { get; set; }
        //UAT-2548
        public Boolean AGU_AgencyApplicantStatus { get; set; }

        public Boolean IsCreatedByClientAdmin { get; set; }

        //UAT-2706
        public Boolean RequirementPkgPermission { get; set; }
        //UAT-2427
        public Boolean AllowJobPosting { get; set; }
        public Boolean SSN_Permission { get; set; } // UAT-2510
        public Boolean HideAgencyPortalDetailLink { get; set; } // UAT-3220
        //UAT-2840
        public Boolean DoNotShowNonAgencyShares { get; set; }

        public Int32 AgencyUserTemplateId { get; set; } //UAT-3316

        #region UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
        public Boolean IsRequirementSharingNonRotationNotification { get; set; }
        public Boolean IsRequirementSharingRotationNotification { get; set; }
        public Boolean IsRotationInvitationApprovalRejectionNotification { get; set; }
        public Boolean IsIndividualProfileSharingWithEmailNotification { get; set; }
        public String EmailConfiguration { get; set; }//UAT-2803
        #endregion
        #region UAT-2942
        public Boolean IsProfileSharingWithEmailNotification { get; set; }
        #endregion


        #region UAT-2977
        public Boolean SendOutOfComplianceNotification { get; set; }
        #endregion

        #region UAT-2977
        public Boolean SendUpdatedApplicantRequirementNotification { get; set; }
        #endregion

        #region UAT-3108
        public Boolean SendUpdatedRotationDetailsNotification { get; set; }

        #endregion

        #region UAT-3222
        public Boolean SendStudentDroppedFromRotationNotification { get; set; }

        #endregion


        //#region UAT-3998
        public Boolean SendItSystemAccessFormNotification { get; set; }

        //#endregion

        #region 3294
        public Guid EmailSharedOrgUserId { get; set; }
        #endregion

        #region UAT-4561
        public Boolean SendRotationEndDateChangeNotification { get; set; }
        #endregion
    }

    [Serializable]
    public class AgencyUserTenantCmbContract
    {
        public Int32 AGI_ID { get; set; }
        public String TenantName { get; set; }
        public Int32 Tenant_ID { get; set; }
    }
    [Serializable]
    public class AgencyClientContact
    {
        public Int32 OrganizationUserID { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public String Agency { get; set; }
        public String Institute { get; set; }
        public Int32 TotalCount { get; set; }
        public DateTime DateOfLastShare { get; set; } //UAT-3801

    }
}
