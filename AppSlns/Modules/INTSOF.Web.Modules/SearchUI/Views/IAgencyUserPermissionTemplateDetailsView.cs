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
    public interface IAgencyUserPermissionTemplateDetailsView
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


        Int32 CurrentTemplateID
        {
            get;
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

        IAgencyUserPermissionTemplateDetailsView CurrentViewContext
        {
            get;
        }
        String TemplateName { get; set; }

        String TemplateDescription { get; set; }
        Int32 AGUPT_ID { get; set; }
        //Boolean IsAgencyUserLoggedIn { get; set; }
        //Int32 AgencyUserPermissionAccessTypeId { get; set; }

        //Int32 AgencyUserPermissionTypeId { get; set; }

       // Boolean IsAdminLoggedIn { get; set; }

     //   Boolean EditFlag { get; set; }


        AgencyUserPermissionTemplate AgencyUsrPerTemplate
        {
            get;
            set;
        }

        
        List<lkpAgencyUserNotification> lstAgencyUserNotification
        {
            get;
            set;
        }

        List<Int32> InvitationSharedInfoTypeIDs { get; set; }

        List<Int32?> ApplicantInvMetaDataIDs { get; set; }

        List<Entity.SharedDataEntity.lkpInvitationSharedInfoType> LstSharedInfoType
        {
            get;
            set;
        }

        List<ApplicantInvitationMetaData> LstSharedInfo
        {
            get;
            set;
        }

        List<lkpAgencyUserReport> lstAgencyUserReports { get; set; }//UAT-3664
        List<AgencyUserPermissionTemplateMapping> lstTemplateReportPermissions { get; set; }  //UAT-3664
    }
}
