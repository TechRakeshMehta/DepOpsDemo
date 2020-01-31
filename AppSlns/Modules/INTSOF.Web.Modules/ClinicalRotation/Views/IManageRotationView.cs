using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using Entity.ClientEntity;


namespace CoreWeb.ClinicalRotation.Views
{
    public interface IManageRotationView
    {
        List<lkpArchiveState> lstArchiveState { set; }//UAT-2545
        List<String> SelectedArchiveStatusCode { get; set; } //UAT-2545

        List<ClinicalRotationDetailContract> ClinicalRotationData
        {
            get;
            set;
        }

        ///// <summary>
        ///// Gets the view contract.
        ///// </summary>
        ///// <remarks></remarks>
        ClinicalRotationDetailContract ViewContract
        {
            get;
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

        Boolean IsReset
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

        String SelectedAgencyIDs
        {
            get;
            set;
        }

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

        /// <summary>
        /// Get and set custom attribute list of type hierarchy.
        /// </summary>
        List<CustomAttribteContract> GetCustomAttributeList
        {
            get;
            set;
        }

        List<CustomAttribteContract> SaveCustomAttributeList
        {
            get;
            set;
        }

        Int32? RotationID
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

        List<int> SelectedClientContacts { get; set; }

        List<AgencyDetailContract> lstAgencyForAddForm
        {
            get;
            set;
        }


        Int32 SelectedAgencyIDForAddForm
        {
            get;
            set;
        }

        Int32 SelectedTenantIDForAddForm
        {
            get;
            set;
        }

        List<ClientContactContract> ClientContactListForAddForm
        {
            get;
            set;
        }

        List<WeekDayContract> WeekDayListForAddForm
        {
            get;
            set;
        }

        String HierarchyNode
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        /// <summary>
        /// Granular Permissions of the logged-in user.
        /// </summary>
        Dictionary<String, String> dicGranularPermissions
        {
            get;
            set;
        }

        #region UAT-2034:Phase 4 (16): Manage Rotation screen updates
        Dictionary<Int32, Int32> DicOfSelectedRotation
        {
            get;
            set;
        }
        #endregion

        #region UAT-2424
        List<ClinicalRotationDetailContract> lstClinicalRotation
        {
            get;
            set;
        }
        Int32 SelectedRotationIDForCloning
        {
            get;
            set;
        }

        ClinicalRotationDetailContract CloneContract
        {
            get;
            set;
        }

        #endregion

        #region UAT-2666
        List<RotationFieldUpdatedByAgencyContract> lstRotationFieldUpdaeByAgency { get; set; }
        #endregion

        #region UAT-2617
        List<lkpSharedUserRotationReviewStatu> lstRotationReviewStatus
        {
            get;
            set;
        }
        #endregion

        #region UAT-2668
        String SelectedAgencyIDsForAddForm
        {
            get;
            set;
        }

        String AgencyIDs
        {
            get;
            set;
        }

        RotationsMappedToAgenciesContract RotationsMappedToAgenciesData { get; set; }

        #endregion

        //UAT-2696
        Int32 OrganizationUserId { get; }

        ////UAT-2979
        String DeptProgramMappingID { get; }

        //UAT-3032
        Int32 PreferredSelectedTenantID { get; set; }
        
        #region UAT-3121
        Boolean IsApplicantPkgNotAssignedThroughCloning { get; set; }
        Boolean IsInstructorPkgNotAssignedThroughCloning { get; set; }
        #endregion
    }
}
