using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;
using Entity.SharedDataEntity;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IRotationDetailsView
    {
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

        Int32 TenantID
        {
            get;
            set;
        }

        //Int32 SelectedTenantId { get; set; }

        List<TenantDetailContract> lstTenants
        {
            set;
        }

        List<TenantDetailContract> lstSelectedTenants
        {
            get;
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

        List<WeekDayContract> WeekDayList
        {
            set;
        }

        List<SharedUserRotationReviewStatusContract> RotationStatusList
        {
            set;
        }

        ClinicalRotationDetailContract SearchContract
        {
            get;
            set;
        }

        List<String> SharedUserTypeCodes { get; }

        String UserID { get; }

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

        List<SharedUserRotationReviewStatusContract> lstRotationReviewStatus
        {
            set;
        }

        List<SharedUserRotationReviewContract> lstShareduserReviewContract { get; set; }

        Int32 SelectedReviewStatusID { get; set; }

        List<AttestationDocumentContract> LstInvitationDocumentContract { get; set; }

        Boolean IsControlVisible { get; set; }

        Boolean IsInstructor { get; }
        #region UAT-1641:As an Agency User, I should be able to be linked to multiple agencies
        List<AgencyDetailContract> lstAgency
        {
            set;
        }

        String lstSelectedAgencyIds
        {
            get;
        }
        #endregion
    }
}
