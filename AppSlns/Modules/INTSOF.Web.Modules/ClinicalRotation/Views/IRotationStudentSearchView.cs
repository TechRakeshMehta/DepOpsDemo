using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRotationStudentSearchView
    {
        Int32 SelectedTenantID
        {
            get;
            set;
        }

        //UAT-4013
        List<TenantDetailContract> SelectedTenantIDs
        {
            get;
            //set;
        }
        Boolean IsReturntoRotationStudentSearch { get; set; }
        Boolean IsResetClicked { get; set; }

        RotationMemberSearchDetailContract SearchParameterContract { get; set; }

        List<RotationMemberSearchDetailContract> lstRotationMemberSearchData { get; set; }

        List<TenantDetailContract> lstTenant
        {
            get; //UAT-4013 added
            set;
        }
       
        IRotationStudentSearchView CurrentViewContext { get;  }

        List<String> SharedUserTypeCodes { get; }

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
        Int32 OrganizationUserID
        {
            get;                   
        }
        List<AgencyDetailContract> lstAgency
        {
            set;
        }

        String lstSelectedAgencyIds
        {
            get;
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
        Int32 VirtualRecordCount
        {
            get;
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract StudentGridCustomPaging
        {
            get;
            set;
        }

        #endregion
        Boolean IsAdvanceSearchPanelDisplay { get; set; }
        List<WeekDayContract> WeekDayList { set;  }

        //UAT-4013
        Dictionary<Int32, Boolean> CustomMessageOrgUserIds
        {
            get;
            set;
        }
        Int32 CurrentLoggedInUserId
        {
            get;
        }
        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        Dictionary<Int32, Boolean> AssignOrganizationUserIds
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        Dictionary<Int32, Tuple<Boolean, Int32>> RemovedClinicalRotationMemberIds
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the value for approved rotation memebers.
        /// </summary>
        Dictionary<Int32, Boolean> ApprovedClinicalRotationMemberIdsToRemove
        {
            get;
            set;
        }

        Dictionary<Int32, String> lstSelectedOrgUserIDs { get; set; }

        //UAT-4013
        Boolean RebindGrid
        {
            get;
            set;
        }
        String lstTenantIds
        {
            get;
            set;
        }

        List<Int32> lstSelectedTenantIDs
        { get; set; }

    }
}
