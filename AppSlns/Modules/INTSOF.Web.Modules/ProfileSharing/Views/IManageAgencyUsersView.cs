#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System.Linq;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.UI.Contract.ProfileSharing;
using Entity.SharedDataEntity;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.ProfileSharing.Views
{
    public interface IManageAgencyUsersView
    {
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

        /// <summary>
        /// Id of the Current Loggedin User
        /// </summary>
        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 TenantId
        {
            set;
            get;
        }

        IManageAgencyUsersView CurrentViewContext
        {
            get;
        }

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

        List<AgencyInstitutionsContract> LstAgencyInstitutions
        {
            get;
            set;
        }

        List<Int32> SelectedAgencyID { get; set; }

        List<Entity.SharedDataEntity.lkpInvitationSharedInfoType> LstSharedInfoType
        {
            get;
            set;
        }


        List<AgencyUserContract> LstAgencyUsers { get; set; }

        Int32 AGU_ID { get; set; }

        Boolean EditFlag { get; set; }

        List<Int32> PrevSelectedTenantIDs { get; set; }

        List<Int32> CurrentSelectedTenantIDs { get; set; }

        Boolean IsAgencyUserLoggedIn { get; }

        Guid UserID { get; }

        //UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
        Int32 AgencyUserPermissionAccessTypeId { get; set; }

        //UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
        Int32 AgencyUserPermissionTypeId { get; set; }

        AgencyUserContract SelectedAgencyUser { get; set; }

        //Dictionary<int, List<int>> DicSelectedAgencyInstitutionMapping { get; set; }

        #region UAT-3316

        List<AgencyUserPermissionTemplateContract> lstAgencyUserPerTemplates
        {
            get;
            set;
        }

        List<AgencyUserPermissionTemplateMapping> lstAgencyUserPerTemplatesMappings
        {
            get;
            set;
        }

        List<AgencyUserPerTemplateNotificationMapping> lstAgencyUserPerTemplatesNotificationMappings
        {
            get;
            set;
        }
        List<lkpAgencyUserPermissionType> lstAgencyUserPermisisonType
        {
            get;
            set;
        }

        #endregion
        //UAT-2538 : New email sent to agency users (configurable as to who receives on manage agency users) to inform that a student has been approved or not approved (and by whom) for their rotation package.
        List<Int32> lstSelectedAgencyUserIDs { get; set; }

        //UAT-2640
        Dictionary<Int32, String> lstAgencyHierarchy { get; set; }
        Int32 SelectedAgencyHierarchyID { get; set; }

        //UAT-2641
        List<Int32> lstPrevAgencyHierarchyIds { get; set; }

        String CurrentLoggedInOrgUserID { get; }

        Int32 OrganisationUserID
        {
            get;
        }
        //UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
        List<lkpAgencyUserNotification> lstAgencyUserNotification
        {
            get;
            set;
        }
        List<Entity.SharedDataEntity.AgencyUser> LstAgencyUserByAgency
        {
            get;
            set;
        }

        #region UAT-3360
        Boolean IsAdminLoggedIn { get; set; }
        Entity.OrganizationUser ExistingOrganisationUser { get; set; }
        String SelectedLinkingProfileOrgUsername { get; set; }

        #endregion

        List<Int32> InvitationSharedInfoTypeIDs { get; set; }

        List<Int32?> ApplicantInvMetaDataIDs { get; set; }

        List<lkpAgencyUserReport> lstAgencyUserReports { get; set; }//UAT-3664
        List<AgencyUserReportPermissionContract> lstAgencyUserReportPermission { get; set; } //UAT-3664
        List<AgencyUserPermissionTemplateMapping> lstTemplateReportPermissions { get; set; }  //UAT-3664



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
        //Int32 VirtualPageCount
        //{
        //    set;
        //}

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
    }
}
