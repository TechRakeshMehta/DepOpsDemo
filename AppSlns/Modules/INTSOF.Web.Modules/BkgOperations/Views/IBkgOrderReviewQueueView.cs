using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public interface IBkgOrderReviewQueueView
    {
        Int32 TenantId { get; set; }
        Int32 SelectedTenantId { get; set; }
        String ApplicantFirstName { get; set; }
        String ApplicantLastName { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32? SelectedReviewCriteriaId { get; set; }
        List<Entity.Tenant> LstTenant { get; set; }
        List<BkgReviewCriteria> LstReviewCriterias { set; }
        List<BkgOrderReviewQueueContract> BkgOrderReviewQueueData { get; set; }
        List<lkpBkgSvcGrpReviewStatusType> LstSvcGrpReviewStatus { set; }
        List<lkpBkgSvcGrpStatusType> LstSvcGrpStatus { set; }
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

        String InfoMessage
        {
            get;
            set;
        }

        String OrderNumber
        {
            get;
            set;
        }

        //Int32? svcgrpreviewstatustypeid
        //{
        //    get;
        //    set;
        //}


        List<Int32?> SvcGrpReviewStatusTypeIDs
        {
            get;
            set;
        }

        Int32? SvcGrpStatusTypeID
        {
            get;
            set;
        }

        DateTime? OrderFromDate
        {
            get;
            set;

        }
        DateTime? OrderToDate
        {
            get;
            set;
        }

        DateTime? SvcGrpUpdatedFromDate
        {
            get;
            set;

        }
        DateTime? SvcGrpUpdatedToDate
        {
            get;
            set;
        }
        //Int32? TargetHierarchyNodeId
        //{
        //    get;
        //}

        String TargetHierarchyNodeIds
        {
            get;
        }

        BkgOrderReviewQueueContract SetBkgOrderReviewQueueContract { set; }

        BkgOrderReviewQueueContract GetBkgOrderReviewQueueContract { get; }

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

        /// <summary>
        /// Indicates wheather Select Client dropdown will be visible or not.
        /// </summary>
        Boolean IsAdminUser
        {
            get;
            set;
        } 

        List<lkpArchiveState> lstArchiveState { set; }
        String SelectedArchiveStateCode { get; set; }

        String SelectedSvcGrpReviewType
        {
            get;
            set;
        }
    }
}
