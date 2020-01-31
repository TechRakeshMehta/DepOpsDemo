using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;
using System;
using System.Collections.Generic;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRotationVerificationUserWorkQueue
    {
        #region Selected Filters

        List<Int32> SelectedTenantIDs { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        String ApplicantFirstName { get; set; }

        String ApplicantLastName { get; set; }

        String PackageName { get; set; }

        Int32 SelectedAgencyID { get; set; }

        DateTime? RotationStartDate { get; set; }

        DateTime? RotationEndDate { get; set; }

        DateTime? SubmissionDate { get; set; }

        Boolean IsCurrent { get; set; }

        String ComplioID { get; set; }

        String RequirementPackageTypes { get; set; } 

        #endregion

        #region Filter Data Source

        List<TenantDetailContract> lstTenant { get; set; }

        List<Agency> lstAgency { get; set; }

        List<RequirementPackageTypeContract> lstRequirementPackageType { get; set; }

        #endregion

        List<RequirementVerificationQueueContract> ApplicantSearchData { get; set; } 

        String ErrorMessage { get; set; }

        String SuccessMessage { get; set; } 

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

        Boolean IsAdminLoggedIn { get; set; }
        Int32 TenantId { get; set; }

        //UAT-3245
        //Int32 SelectedTenantID { get; set; }
        //String SelectedAgencyIDs { get; set; }
        String DeptProgramMappingID { get; }
        /// <summary>
        /// View Contract
        /// </summary>
        RequirementVerificationFilterDataContract VerificationViewContract
        {
            get;
        }
    }
}
