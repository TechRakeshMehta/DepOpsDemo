using System;
using System.Collections.Generic;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IShareHistory
    {
        Int32 SelectedTenantID { get; set; }
        List<Entity.Tenant> LstTenant { get; set; }
        Boolean IsAdminLoggedIn { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 ClientTenantID { get; set; }
        List<Entity.ClientEntity.UserGroup> LstUserGroup { get; set; }
        List<Agency> LstAgency { get; set; }
        List<ShareHistoryDataContract> ShareHistoryData { get; set; }
        Dictionary<Int32, Boolean> SelectedInvitationIds { get; set; }
        Int32 InvitationIdToDownloadReport { get; set; }
        List<ClientContactContract> ClientContactList { get; set; }
        List<WeekDayContract> WeekDayList { get; set; }
        Boolean IsReset { get; set; } //UAT-4214
        List<CustomAttribteContract> GetCustomAttributeList
        {
            get;
            set;
        }

        //Serach Filters
        String SelectedHierarchyIDs { get; set; }
        Int32 SelectedAgencyID { get; set; }
        Int32 SelectedUserGroupID { get; set; }
        Int32 OrganizationUserID { get; }
        String ApplicantFirstName { get; }
        String ApplicantLastName { get; }
        String EmailAddress { get; }
        String SSN { get; }
        DateTime? DOB { get; }
        String CustomFields { get; set; }
        CustomPagingArgsContract DataGridCustomPaging { get; }

        String RotationName { get; }
        String TypeSpecialty { get; }
        String Department { get; }
        String Program { get; }
        String Course { get; }
        String Term { get; }
        String UnitFloorLoc { get; }
        String DaysIdList { get; }
        TimeSpan? StartTime { get; }
        TimeSpan? EndTime { get; }
        DateTime? StartDate { get; }
        DateTime? EndDate { get; }
        String SelectedClientContacts { get; }
        String RotationCustomAttributes { get; set; }

        List<AttestationDocumentContract> LstInvitationDocumentContract { get; set; }
        //UAT-1895 To check wheather all invitation should be Shown in grid or only audit requested .
        Boolean IsAuditRequested { get;}

        #region Custom Paging Parameters

        Int32 CurrentPageIndex
        {
            get;
            set;
        }
        Int32 PageSize
        {
            get;
        }
        Int32 VirtualRecordCount
        {
            set;
        }

        #endregion
    }
}
