using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IAgencyHierarchyUserPermissionView
    {
        IAgencyHierarchyUserPermissionView CurrentViewContext { get; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 AgencyHierarchyId { get; set; }

        List<AgencyHierarchyUserContract> lstAgencyHierarchyUsers { get; set; }

        AgencyHierarchyUserContract agencyHierarchyUserContract { get; set; }

        List<AgencyHierarchyUserContract> lstAgencyUsers { get; set; }

        List<Entity.SharedDataEntity.lkpInvitationSharedInfoType> LstSharedInfoType
        {
            get;
            set;
        }

        List<Entity.SharedDataEntity.ApplicantInvitationMetaData> LstSharedInfo
        {
            get;
            set;
        }
        //UAT-2803:Enhance the agency user settings so that we can individually choose what notifications an agency user receives
        List<lkpAgencyUserNotification> lstAgencyUserNotification
        {
            get;
            set;
        }
        List<AgencyUserPermissionTemplateContract> lstAgencyUserPerTemplates
        {
            get;
            set;
        }

        #region UAT-3316

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
        List<Int32> InvitationSharedInfoTypeIDs { get; set; }

        List<Int32?> ApplicantInvMetaDataIDs { get; set; }
        #endregion

        List<lkpAgencyUserReport> lstAgencyUserReports { get; set; }//UAT-3664
        List<AgencyUserReportPermissionContract> lstAgencyUserReportPermission { get; set; } //UAT-3664
        List<AgencyUserPermissionTemplateMapping> lstTemplateReportPermissions { get; set; }  //UAT-3664
    }
}
