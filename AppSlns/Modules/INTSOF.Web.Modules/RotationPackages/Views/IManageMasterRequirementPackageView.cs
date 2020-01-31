using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface IManageMasterRequirementPackageView
    {
        IManageMasterRequirementPackageView CurrentViewContext { get; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 TenantId { get; set; }

        List<AgencyDetailContract> lstAgency { get; set; }

        List<RequirementPackageContract> lstRequirementPackage { get; set; }

        List<RequirementPackageTypeContract> lstRequirementPackageType { get; set; }

        List<RequirementCategoryContract> lstCategory { get; set; }

        String PackageName { get; set; }

        String PackageLabel { get; set; }

        String lstSelectedAgencyIds { get; set; }

        Int32 SelectedReqPackageTypeID { get; set; }

        DateTime? EffectiveStartDate { get; set; }

        DateTime? EffectiveEndDate { get; set; }

        DateTime? PackageCreatedDate { get; set; }

        DateTime? RotationEndDate { get; set; }

        RequirementPackageContract RequirementPackage { get; set; }

        Dictionary<Int32, Boolean> lstSelectedRotPackage { get; set; }

        List<DefinedRequirementContract> lstDefinedRequirement { get; set; }

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
            set;
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

        Int32[] PackageIds { get; set; }

        Int32 ? PackageOptions { get; set; }

        //UAT-2649
        List<Int32> LstSelectedAgencyHierarchyIds { get; set; }
        List<Int32> lstPrevAgencyHierarchyIds { get; set; }

        List<RequirementReviewByContract> lstRequirementReviewBy { get; set; }

    }
}
