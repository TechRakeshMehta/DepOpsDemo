using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using Entity.SharedDataEntity;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IManageRotationByAgencyView
    {
        List<lkpArchiveState> lstArchiveState { set; }
        List<String> SelectedArchiveStatusCode { get; set; } 

        List<ClinicalRotationDetailContract> ClinicalRotationData
        {
            get;
            set;
        }    

        List<AgencyDetailContract> lstAgency
        {
            get;
            set;
        }

        Int32 SelectedTenantID
        {
            get;
            set;
        }

        Int32 TenantID
        {
            get;
            set;
        }


        List<TenantDetailContract> lstTenant
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        //Int32 SelectedAgencyID
        //{
        //    get;
        //    set;
        //}

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

        List<ClientContactContract> ClientContactList
        {
            get;
            set;
        }

        List<WeekDayContract> WeekDayList
        {
            get;
            set;
        }

        ClinicalRotationDetailContract SearchContract
        {
            get;
            set;
        }
       
        Int32 SelectedRotationID
        {
            get;
            set;
        }

        #region Custom paging parameters
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        Int32 PageSize
        {
            get;
            set;
        }

        Int32 VirtualRecordCount
        {
            get;
            set
           ;
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }
        #endregion
       
        Int32 CurrentLoggedInUserId
        {
            get;
        }    
        Boolean RebindGrid
        {
            get;
            set;
        }

        String SelectedAgencyIDs
        {
            get;
            set;
        }

    }
}
