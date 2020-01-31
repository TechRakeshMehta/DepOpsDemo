using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.Mobility;

namespace CoreWeb.Mobility.Views
{
    public interface IRollbackChangeSubscriptionView
    {
        Int32 TenantId { get; set; }
        Int32 SelectedTenantId { get; set; }
        String ApplicantFirstName { get; set; }
        String ApplicantLastName { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 UserGroupId { get; set; }
        Int32 SourceNodeId { get; set; }
        Int32 TargetNodeId { get; set; }
        DateTime? FromDate { get; set; }
        DateTime? ToDate { get; set; } 
        List<Entity.Tenant> lstTenant { get; set; }
        List<UserGroup> lstUserGroup { get; set; }
        List<Entity.ClientEntity.ActiveSubscriptionsForRollback> ApplicantSearchData { get; set; }
        List<Int32> SelectedOrdersIDList { get; set; }

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




