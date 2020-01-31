using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IArchivedOrderView
    {
        #region Variables

        #region Private Variables



        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// returns Current View Context
        /// </summary>
        IArchivedOrderView CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// Sets or gets the Tenant Id for the logged-in user.
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// returns the Current Logged In User Id.
        /// </summary>
        Int32 CurrentLoggedInUserId
        {
            get;
        }

        /// <summary>
        /// get or set previous button value 
        /// </summary>
        Boolean IsPreviousButtonFire { get; set; }

        /// <summary>
        /// return FingerPrint Archived Orders List
        /// </summary>
        List<FingerPrintArchivedOrderContract> FPArchivedOrderList
        {
            get;
            set;
        }

        /// <summary>
        /// returns Is Applicant
        /// </summary>
        Boolean IsApplicant { get; }

        /// <summary>
        /// get or set the Fingerprint Data
        /// </summary>
        FingerPrintAppointmentContract FingerprintData { get; set; }

        #region  Get Custom form data of Archived order
        List<Int32> BopIds { get; set; }

        List<AttributeFieldsOfSelectedPackages> lstAttrMVRGrp { get; set; }
        List<BkgOrderDetailCustomFormUserData> lstDataForCustomForm { get; set; }
        #endregion

        #endregion
    }
}
