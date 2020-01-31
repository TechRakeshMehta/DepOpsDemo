using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.Search.Views
{
    public interface IApplicantPortFolioSearchView
    {
        List<CustomComplianceContract> lstPackageSubscription { get; set; } //UAT-4218
        List<CustomComplianceContract> AssignOrganizationUsers { get; set; } //UAT-4218
        String ApplicantName { get; set; } // UAT-4218
        Int32 TenantId { get; set; }
        Int32 SelectedTenantId { get; set; }
        String ApplicantFirstName { get; set; }
        String ApplicantLastName { get; set; }
        DateTime? DateOfBirth { get; set; }
        String EmailAddress { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32? OrganizationUserID { get; set; }
        String SSN { get; set; }
        Int32 MatchUserGroupId { get; set; }
        Int32 FilterUserGroupId { get; set; }
        Int32 DPM_ID { get; set; }
        String DPM_IDs { get; set; }
        String CustomFields { get; set; }
        List<Entity.Tenant> lstTenant { get; set; }
        List<UserGroup> lstUserGroup { get; set; }
        List<ApplicantDataList> ApplicantSearchData { get; set; }
        Int32? NodeId { get; set; }
        Dictionary<Int32, String> AssignOrganizationUserIds { get; set; }
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

        List<Int32> ListSubscriptionIds { get; set; }

        #region UAT-806 Creation of granular permissions for Client Admin users
        String SSNPermissionCode { get; set; }
        Boolean IsDOBDisable { get; set; }
        #endregion

        #region UAT-977
        List<lkpArchiveState> lstArchiveState { set; }
        List<String> SelectedArchiveStateCode { get; set; }
        #endregion

        Dictionary<String, List<Int32>> DicSubscriptionIDs { get; set; }
        //List<Int32> LstMultipleSubscriptionIDs { get; set; }
        //List<Int32> LstSingleSubscriptionIDs { get; set; }

        List<UserNodePermissionsContract> lstUserNodePermissionsContract { get; set; }
        List<ApplicantInstitutionHierarchyMapping> lstApplicantInstitutionHierarchyMapping { get; set; }
        String OrganisationUserIds { get; set; }

        #region UAT-3010:- Granular Permission for Client Admin Users to Archive.

        String ArchivePermissionCode { get; set; }
        #endregion

        Boolean ShowActiveOrdersOnly { get; set; } //UAT-4273

    }
}




