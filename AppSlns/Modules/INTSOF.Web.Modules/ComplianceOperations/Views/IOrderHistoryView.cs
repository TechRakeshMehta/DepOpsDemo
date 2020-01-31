#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.BkgOperations;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IOrderHistoryView
    {
        #region Variables

        #region Private Variables
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties

        Int32 CurrentUserId
        {
            get;
        }

        List<vwOrderDetail> ListOrderDetail
        {
            get;
            set;
        }

        Int32 CurrentUserTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Get Set Property to show hide link for place rush order on the basis of client setting.
        /// </summary>
        Boolean ShowRushOrder
        {
            get;
            set;
        }

        //Boolean IsAutoRenewalOn
        //{
        //    get;
        //    set;
        //}
        Boolean IsScreeningOnly
        {
            get;
            set;
        }

        Boolean IsBkgOrderWithAppointment
        {
            get;
            set;
        }

        List<OrderPaymentDetail> OrderPaymentDetailList
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Methods

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion

        Dictionary<Int32, Boolean> EDSExistStatus { get; set; }

        List<Tuple<Int32, Boolean>> LstReciptDocumentStatus { get; set; }

        List<ServiceFormContract> lstServiceForm { get; set; }

        Int32 OrgUsrID { get; }

        #region UAT-1648
        List<BkgOrderDetailCustomFormUserData> lstDataForCustomForm { get; set; }
        List<Int32> BopIds { get; set; }
        #endregion
        /// <summary>
        /// UAT-1683 Add the Archive button and Manage Un-Archive to the Screening side
        /// </summary>
        Boolean IsArchivedSubscription { get; set; }

        List<AttributeFieldsOfSelectedPackages> LstInternationCriminalSrchAttributes { get; set; }
        List<AttributeFieldsOfSelectedPackages> lstAttrMVRGrp { get; set; }

        #region CBI CABS
        Boolean IsLocationServiceTenant { get; set; }
        #endregion

        String MenuId { get; set; }
    }
}




