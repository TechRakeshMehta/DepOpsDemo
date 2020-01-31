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

namespace CoreWeb.SearchUI.Views
{
    public interface IAgencyUserPermissionTemplateView
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

        IAgencyUserPermissionTemplateView CurrentViewContext
        {
            get;
        }

        List<AgencyUserPermissionTemplateContract> LstAgencyUsrPerTemplate
        {
            get;
            set;
        }

        List<AgencyUserPermissionTemplateMappingContract> LstAgencyUsrPerTemplateMapping
        {
            get;
            set;
        }

        List<AgencyUserPermissionTemplateNotificationsContract> LstAgencyUsrPerTemplateNotification
        {
            get;
            set;
        }

        List<lkpAgencyUserNotification> lstAgencyUserNotification
        {
            get;
            set;
        }

        Int32 OrganisationUserID
        {
            get;
        }

        Int32 AgencyUserPermissionAccessTypeId { get; set; }

        Int32 AgencyUserPermissionTypeId { get; set; }

        Boolean IsAdminLoggedIn { get; set; }

        Boolean EditFlag { get; set; }

        Int32 AGUPT_ID { get; set; }
        Boolean IsAgencyUserLoggedIn { get; set; }
        AgencyUserPermissionTemplateContract SelectedAgencyUserPerTemplate { get; set; }

        String TemplateName { get; set; }
        
        String TemplateDescription { get; set; }

        #region Custom paging parameters

        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        Int32 PageSize
        {
            get;
            set;
        }

        Int32 VirtualRecordCount
        {
            get;
            set;
        }
        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }
        #endregion

        List<ApplicantInvitationMetaData> LstSharedInfo
        {
            get;
            set;
        }

        List<Entity.SharedDataEntity.lkpInvitationSharedInfoType> LstSharedInfoType
        {
            get;
            set;
        }

        List<lkpAgencyUserReport> lstAgencyUserReports { get; set; }//UAT-3664
        List<AgencyUserPermissionTemplateMapping> lstTemplateReportPermissions { get; set; }  //UAT-3664
    }
}
