using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IBkgOrderSearchQueueView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        IBkgOrderSearchQueueView CurrentViewContext
        {
            get;
        }

        Int32 HierarchyNodeID
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        List<Entity.ClientEntity.Tenant> lstTenant
        {
            set;
        }

        List<Entity.ClientEntity.lkpOrderStatu> lstPaymentStatus
        {
            set;
        }

        //List<Entity.ClientEntity.lkpPaymentOption> lstPaymentType
        //{
        //    set;
        //}

        List<Entity.ClientEntity.lkpOrderStatusType> lstOrderStatusType
        {
            set;
        }
        List<Entity.ClientEntity.BackgroundService> lstBackroundServices
        {
            set;
        }
        List<Entity.ClientEntity.BkgOrderClientStatu> lstOrderClientStatus
        {
            //get;
            set;
        }

        BkgOrderSearchContract SetBkgOrderSearchContract
        {
            set;
        }
        List<Entity.ClientEntity.BackroundOrderSearch> lstBackroundOrderSearch
        {
            get;
            set;
        }
        List<BackroundOrderContract> lstBackroundOrder
        {
            get;
            set;
        }
        List<BackroundServiceGroupContract> lstBackroundServiceGroup
        {
            get;
            set;
        }
        BackroundOrderSearchContract lstBackroundOrderSearchContract
        {
            get;
            set;
        }
        List<BackroundServicesContract> lstBackroundServicesContract
        {
            get;
            set;
        }
        List<Entity.ClientEntity.BkgSvcGroup> lstBkgSvcGroup
        {
            set;
        }
        List<Entity.ClientEntity.lkpServiceFormStatu> lstServiceFormStatus
        {
            set;
        }
        //List<OrderContract> lstOrderQueue
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// Error Message
        /// </summary>
        String ErrorMessage
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Boolean IsAdminUser
        {
            get;
            set;
        }
        //Boolean? IsFlagged
        //{
        //    get;
        //    set;
        //}

        String SelectedFlagged { get; set; }

        List<Entity.ClientEntity.InstitutionOrderFlag> InstitutionOrderFlagList
        {
            get;
            set;
        }
        Boolean IsPaymentStatusChecked
        {
            get;

        }
        Boolean IsClientStatusChecked
        {
            get;

        }
        //Boolean IsFlaggedChecked
        //{
        //    get;

        //}
        Boolean IsArchiveChecked
        {
            get;
        }
        //List<String> SelectedOrderStatusCode
        //{
        //    get;
        //    set;

        //}

        //List<String> SelectedPaymentTypeCode
        //{
        //    get;
        //    set;

        //}

        String FirstNameSearch
        {
            get;
        }

        String LastNameSearch
        {
            get;
        }

        String OrderNumberSearch
        {
            get;
        }
        Int32? ServiceGroupId
        {
            get;
            set;
        }
        Int32? OrderClientStatusID
        {
            get;
            set;
        }

        Int32? ServiceFormStatusID
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
        /// <summary>
        /// Sets and gets option for showing Rush Orders in the Queue. 
        /// </summary>
        //Boolean ShowOnlyRushOrders
        //{
        //    get;
        //}

        //List<DateTime> LstOrderCrtdDate
        //{
        //    get;
        //}

        //List<DateTime> LstOrderPaidDate
        //{
        //    get;
        //}
        Int32? OrderPaymentStatusID
        {
            get;
            set;

        }
        Int32? OrderStatusTypeID
        {
            get;
            set;

        }
        Int32? ServiceID
        {
            get;
            set;
        }
        Int32? InstitutionStatusColorID
        {
            get;
            set;
        }
        DateTime? OrderFromDate
        {
            get;

        }
        DateTime? OrderToDate
        {
            get;


        }
        DateTime? PaidFromDate
        {
            get;


        }
        DateTime? PaidToDate
        {
            get;


        }
        DateTime? OrderCompletedFromDate
        {
            get;


        }
        DateTime? OrderCompletedToDate
        {
            get;


        }
        String SSN
        {
            get;
            set;

        }
        DateTime? DOB
        {
            get;
            set;
        }
        Boolean? IsArchive
        {
            get;
            set;
        }
        //List<lkpArchiveState> lstArchiveState
        //{
        //    set;
        //}

        //List<String> SelectedArchiveStateCode
        //{
        //    get;
        //    set;

        //}

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

        ///// <summary>
        ///// PageSize</summary>
        ///// <value>
        ///// Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
            set;
        }

        ///// <summary>
        ///// VirtualPageCount</summary>
        ///// <value>
        ///// Sets the value for VirtualPageCount.</value>
        Int32 VirtualPageCount
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

        /// <summary>
        /// View Contract
        /// </summary>
        //OrderContract ViewContract
        //{
        //    get;
        //}

        #endregion

        #region UAT-774

        Dictionary<Int32, String> AssignOrganisationUserIDs
        {
            get;
            set;
        }

        #endregion

        #region UAT-806 Creation of granular permissions for Client Admin users

        String SSNPermissionCode { get; set; }
        Boolean IsDOBDisable { get; set; }
        #endregion

        #region UAT-1075 WB:Admin Granular permissions for color flag and Result PDF

        Boolean IsBkgColorFlagDisable { get; set; }
        //Boolean IsBkgResultReportDisable { get; set; }

        #endregion

        Boolean DisplayOrderArchiveStatus
        {
            get;
            set;
        }

        Boolean DisplayOrderClientStatus
        {
            get;
            set;
        }

        List<String> LstBkgOrderResultPermissions
        {
            get;
            set;
        }

        //UAT 1417
        List<Entity.ClientEntity.UserGroup> lstUserGroup
        {
            set;
        }

        Int32? UserGroupID
        {
            get;
            set;
        }

        String SelectedOrderIds { get; set; }

        List<GranularPermission> GranularPermission
        {
            get;
            set;
        }

        Boolean IsBkgOrderNoteEnabled { get; set; }

        List<Entity.ClientEntity.lkpArchiveState> lstArchiveState { set; }

        String SelectedArchiveStateCode { get; set; }

        String DPM_IDs { get; set; }

        String CustomFields { get; set; }

        String NodeLabel { get; set; }

        String NodeIds { get; set; }

        #region UAT-1795 : Add D&A download button on Background Order Queue search.
        List<Int32> lstSeletedOrderIds { get; set; }

        List<BkgOrderSearchQueueContract> DocumentListToExport { get; set; }
        #endregion

        #region UAT-1996: Setting to allow Client admins the ability to edit color flags
        Boolean IsBkgColorFlagFullPermission
        {
            get;
            set;
        }
        #endregion

        //UAT-2178: Color flag column should only show when color flag is enabled for a tenant.
        Boolean IsInstitutionHasOrderFlag { get; set; }

        #region UAT-3010:- Granular Permission for Client Admin Users to Archive.

        String ArchivePermissionCode { get; set; }
        #endregion
    }
}
