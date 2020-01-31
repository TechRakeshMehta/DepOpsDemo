using Entity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IManageOrderFulfillmentQueueView
    {

        Int32 TenantId { get; set; }
        Int32 SelectedTenantID { get; set; }
        List<Tenant> lstTenant { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        String CurrentUserId { get; }
        String TenantIDs { get; set; }
        AppointmentOrderScheduleContract filterContract { get; set; }
        AppointmentOrderScheduleContract fulFillmentDetailContract { get; set; }
        Boolean IsAdminScreen { get; set; }
        List<AppointmentOrderScheduleContract> lstOrderFulFillment { get; set; }
        //Int32 PageSize { get; set; }
        //Int32 CurrentPageIndex { get; set; }
        //Int32 VirtualRecordCount { get; set; }
        //CustomPagingArgsContract GridCustomPaging { get; set; }

        Int32 DefaultTenantId
        {
            get;
            set;
        }
        List<LocationContract> lstAvailableLocations { get; set; }
        String LocationIDs { get; set; }
        List<LookupContract> lstAppointmentStatus { get; set; }
        List<ServiceStatusContract> lstShipmentStatus { get; set; }
        string SelectedAppointmentStatusIds { get; set; }
        string SelectedShipmentStatusIds { get; set; }
        List<AppointmentOrderScheduleContract> lstAppointmentOrderScheduleContract { get; set; }
        #region UAT - 4025
        Boolean IsHrAdmin { get; set; }
        List<String> lstPermittedCBIUniqueIds { get; set; }
        Boolean IsHrAdminEnroller { get; set; }
        String SelectedCbiUniqueIds { get; set; }
        #endregion

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
    }
}
