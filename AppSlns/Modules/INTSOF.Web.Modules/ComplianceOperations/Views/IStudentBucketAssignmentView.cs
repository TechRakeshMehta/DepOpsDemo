using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Core;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IStudentBucketAssignmentView
    {
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Int32? OrganizationUserID
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

        List<Entity.Tenant> lstTenant
        {
            get;
            set;
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

        List<StudentBucketAssignmentContract> GridSearchData
        {
            get;
            set;
        }

        Dictionary<Int32, Boolean> AssignOrganizationUserIds
        {
            get;
            set;
        }

        Dictionary<Int32, String> SelectedOrgUsersToList { get; set; }

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
        CustomPagingArgsContract GridCustomPaging
        {
            get;
        }

        #endregion

        String SSNPermissionCode { get; set; }

        Boolean IsDOBDisable { get; set; }

        String DPM_IDs { get; set; }

        String CustomFields { get; set; }

        List<Entity.ClientEntity.UserGroup> lstUserGroup { get; set; }

        Int32 FilterUserGroupId { get; set; }

        Int32 MatchUserGroupId { get; set; }

        List<String> LstStudentBucketAssigmentPermissions { get; set; }

        Int32 AgencyID { get; }
        String ComplioID { get; }
        String RotationName { get; }
        String Department { get; }
        String Program { get; }
        String Course { get; }
        String Term { get; }
        String UnitFloorLoc { get; }
        float? RecommendedHours { get; }
        float? Students { get; }
        String Shift { get; }
        TimeSpan? StartTime { get; }
        TimeSpan? EndTime { get; }
        DateTime? StartDate { get; }
        DateTime? EndDate { get; }
        String DaysIdList { get; }
        String ContactIdList { get; }
        String TypeSpecialty { get; }
        String RotationCustomAttributesXML { get; }

        List<AgencyDetailContract> lstAgency
        {
            get;
            set;
        }

        List<ClientContactContract> ClientContactList
        {
            get;
            set;
        }

        List<WeekDayContract> WeekDayList
        {
            get;
            set;
        }

        List<CustomAttribteContract> RotationCustomAttributeList
        {
            get;
            set;
        }

        #region UAT-3010:- Granular Permission for Client Admin Users to Archive.

        String ArchivePermissionCode { get; set; }
        #endregion

    }
}
