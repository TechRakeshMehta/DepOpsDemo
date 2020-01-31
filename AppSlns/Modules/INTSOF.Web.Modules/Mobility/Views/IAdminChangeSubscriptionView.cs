using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.Mobility;

namespace CoreWeb.Mobility.Views
{
    public interface IAdminChangeSubscriptionView
    {
        Int32 TenantId { get; set; }
        Int32 SelectedTenantId { get; set; }
        String ApplicantFirstName { get; set; }
        String ApplicantLastName { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 MatchUserGroupId { get; set; }
        Int32 FilterUserGroupId { get; set; }
        Int32 DPM_ID { get; set; }
        String CustomFields { get; set; }
        List<Entity.Tenant> lstTenant { get; set; }
        List<UserGroup> lstUserGroup { get; set; }
        List<Entity.ClientEntity.AdminChangeSubscription> ApplicantSearchData { get; set; }
        Dictionary<Int32, Boolean> SelectedVerificationItems { get; set; }
        MobilityProgramChange sessionMobilityProgramChange { get; set; }

        #region Custom Paging Parameters

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 CurrentPageIndex { get; set; }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        Int32 PageSize { get; set; }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 VirtualRecordCount { set; }
        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging { get; set; }

        #endregion

    }
}




