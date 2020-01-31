#region Header Comment Master

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  IAppMasterView.cs
// Purpose:
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;

#endregion

#region Application Specific

using Entity;
using System.Web.UI;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.Shell.MasterPages
{
    public interface IAppMasterView : IBaseMasterView
    {
        #region Variables

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current user ID.
        /// </summary>
        /// <remarks></remarks>
        String CurrentUserId
        {
            get;
        }

        /// <summary>
        /// Gets the current Session ID.
        /// </summary>
        /// <remarks></remarks>
        String CurrentSessionId
        {
            get;
        }

        /// <summary>
        /// Sets the assigned blocks.
        /// </summary>
        /// <value>The assigned blocks.</value>
        /// <remarks></remarks>
        List<vw_UserAssignedBlocks> AssignedBlocks
        {
            set;
        }

        /// <summary>
        /// Gets and sets the aspnet_Membership
        /// </summary>
        aspnet_Membership aspnet_Membership
        {
            get;
            set;
        }

        /// <summary>
        /// List of the Website Pages
        /// </summary>
        List<WebSiteWebPage> lstWebsitePages
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current organization user id.
        /// </summary>
        /// <remarks></remarks>
        Int32 CurrentOrgUserId
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        String SelectedTenantId
        {
            get;
            set;
        }

        String SelectedTenantIdForClientAdmin
        {
            get;
            set;
        }

        List<Tenant> lstTenant
        {
            get;
            set;
        }

        Int32 BlockID
        {
            get;
        }

        #region UAT-1218

        Boolean IsSharedUserLoginURL
        {
            get;
            set;
        }

        List<OrganizationUser> LstOrganizationUser { get; set; }
        List<lkpUserTypeSwitchView> lstUserTypeSwitchView { get; set; }
        String UserTypeSwitchViewCode { get; set; }
        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Register postback controls inside an UpdatePanel control as triggers. 
        /// Controls that are registered by using this method update a whole page instead of updating only the UpdatePanel control's content
        /// </summary>
        /// <param name="registerControl"></param>
        void RegisterControlForPostBack(Control registerControl);

        #endregion

        String InstituteLabelText
        {
            set;
        }

        //UAT 1616 WB: As an agency user, I should be able to manage my agency's attestation statement. 
        Int32 AgencyUserPermissionAccessTypeId { get; set; }

        Int32 AgencyUserPermissionTypeId { get; set; }

        //UAT:-3032
        List<Tenant> lstTenants { get; set; }

        Boolean IsUserAllowedPreferredTenant { get; set; }

        Boolean IsLocationServiceTenant { get; }

        //UAT-3664
        List<AgencyUserReportPermissionContract> lstAgencyUserReportPermissions { get; set; }
        List<Entity.SharedDataEntity.lkpAgencyUserReport> lstAgencyUserReports { get; set; }
        Boolean IsAllReportsNotVisible { get; set; }

        IPersistViewState ViewStateProvider { get; }
    }
}