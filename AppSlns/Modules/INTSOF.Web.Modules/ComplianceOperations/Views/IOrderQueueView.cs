using System;
using System.Collections.Generic;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IOrderQueueView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        IOrderQueueView CurrentViewContext
        {
            get;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        List<Tenant> lstTenant
        {
            set;
        }

        List<lkpOrderStatu> lstOrderStatus
        {
            set;
        }

        List<lkpPaymentOption> lstPaymentType
        {
            set;
        }

        List<OrderContract> lstOrderQueue
        {
            get;
            set;
        }

        /// <summary>
        /// Error Message
        /// </summary>
        String ErrorMessage
        {
            get;
            set;
        }

        String InfoMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }


        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Boolean ShowClientDropDown
        {
            get;
            set;
        }

        List<String> SelectedOrderStatusCode
        {
            get;
            set;

        }

        List<String> SelectedPaymentTypeCode
        {
            get;
            set;

        }

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

        /// <summary>
        /// Sets and gets option for showing Rush Orders in the Queue. 
        /// </summary>
        Boolean ShowOnlyRushOrders
        {
            get;
        }

        List<DateTime> LstOrderCrtdDate
        {
            get;
        }

        List<DateTime> LstOrderPaidDate
        {
            get;
        }

        String DeptProgramMappingID
        {
            get;
        }

        List<lkpArchiveState> lstArchiveState
        {
            set;
        }

        List<String> SelectedArchiveStateCode
        {
            get;
            set;

        }

        Dictionary<Int32, Boolean> SelectedOrderIds
        {
            get;
            set;

        }

        String ReferenceNumber
        {
            get;
            set;
        }


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
        OrderContract ViewContract
        {
            get;
        }

        Int32 VirtualRecordCount
        {
            get;
            set;
        }

        #endregion

        //SearchItemDataContract SetSearchItemDataContract
        //{
        //    set;
        //}

        OrderApprovalQueueContract SetOrderApprovalQueueContract
        {
            set;
        }

        String ApplicantSSN { get; set; }

        #region UAT-806 Creation of granular permissions for Client Admin users

        String SSNPermissionCode { get; set; }
        Boolean IsDOBDisable { get; set; }
        #endregion

        #region UAT-796
        String ParentPageName { get; set; }

        #endregion

        /// <summary>
        /// Master Order package types list with Compliance Package, Background Package and Compliance and Background Package
        /// </summary>
        List<lkpOrderPackageType> lstOrderPackageType
        {
            set;
        }

        /// <summary>
        /// List of Order package type codes
        /// </summary>
        List<String> lstSelectedOrderPkgType
        {
            get;
            set;
        }

        Boolean IsSSNDisabled { get; set; }
        Boolean isBkgScreen { get; }
    }

    public class ComplianceDetail
    {
        public String Name
        {
            get;
            set;
        }
        public Int32 ID
        {
            get;
            set;
        }
    }



    //public class ScheduledTaskContract
    //{
    //    public Int32 OrderId
    //    {
    //        get;
    //        set;
    //    }
    //    public Int32 PackageId
    //    {
    //        get;
    //        set;
    //    }
    //    public Int32 OrganisationUserId
    //    {
    //        get;
    //        set;
    //    }

    //    public String ReferenceNumber
    //    {
    //        get;
    //        set;
    //    }

    //    public DateTime ExpiryDate
    //    {
    //        get;
    //        set;
    //    }

    //    public String OrderStatusCode
    //    {
    //        get;
    //        set;
    //    }

    //    public Int32 ApprovedBy
    //    {
    //        get;
    //        set;
    //    }

    //    public String ApprovedDate
    //    {
    //        get;
    //        set;
    //    }
    //}
}




