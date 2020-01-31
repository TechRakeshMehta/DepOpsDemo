using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRequirementVerificationQueueView
    {
        Int32 SelectedTenantId { get; set; }

        Int32 TenantId { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        List<TenantDetailContract> lstTenant { get; set; }

        Boolean IsAdminLoggedIn { get; set; }

        String ApplicantFirstName { get; set; }

        String ApplicantLastName { get; set; }

        Int32 SelectedAgencyID { get; set; }

        DateTime? RotationStartDate { get; set; }

        DateTime? RotationEndDate { get; set; }

        DateTime? SubmissionDate { get; set; }

        String RequirementPackageTypes { get; set; }

        String CategoryId { get; set; }

        String ReqItemId { get; set; }

       

        List<RequirementCategoryContract> LstRequirementCategory
        {
            set;
        }

        List<RequirementItemContract> LstRequirementItems
        {
            set;
        }


        List<AgencyDetailContract> lstAgency { get; set; }

        List<RequirementPackageTypeContract> lstRequirementPackageType { get; set; }

        List<RequirementVerificationQueueContract> ApplicantSearchData { get; set; }

        Boolean IsSearchClicked { get; set; }

        String ErrorMessage { get; set; }

        String SuccessMessage { get; set; }

        String InfoMessage { get; set; }

        List<RequirementPackageContract> LstRequirementPackage
        {
            get;
            set;
        }

        Boolean IsRotationPackageVerificationQueue { get; set; }

        String SelectedPackageID { get; set; }

        List<Int32> SelectedPackageIds
        {
            get;
            set;
        }

       

        Boolean IsCurrentRotation { get; set; }

        //UAT-4014
        Dictionary<String, String> dicUserTypes { get; set; }
        String SelectedUserTypeIds { get; }

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
    }
}
