using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;

namespace CoreWeb.RotationPackages.Views
{
    public interface IManageRotationPackageView
    {
        IManageRotationPackageView CurrentViewContext { get; }

        List<RequirementPackageDetailsContract> LstRequirementPackageDetailsContract
        {
            get;
            set;
        }

        Boolean IsReset { get; set; }

        String PackageName { get; set; }

        String RequirementPackageStatus { get; set; }

        List<AgencyDetailContract> LstAgency
        {
            get;
            set;
        }

        Int32 SelectedTenantID
        {
            get;
            set;
        }

        //UAT-3875
        Boolean IsActive
        {
            get;
            set;
        }

        Int32 TenantID
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserID { get; }


        List<TenantDetailContract> LstTenant
        {
            get;
            set;
        }

        List<RequirementPackageTypeContract> LstRequirementPackageType
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        String LstSelectedAgencyIDs
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

        Boolean IsSharedUser { get; }

        Guid CurrentUserID { get; }

        Int32 SelectedReqPackageTypeID
        {
            get;
            set;
        }

        RequirementPackageContract RequirementPackageContractSessionData { get; set; }

        String LstSelectedTenantIDs
        {
            get;
            set;
        }

        Int32 AgencyId { get; set; }

        /// <summary>
        /// Granular Permissions of the logged-in user.
        /// </summary>
        Dictionary<String, String> dicGranularPermissions
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

        List<DefinedRequirementContract> lstDefinedRequirement { get; set; }

        List<Int32> LstAgencyHeirarchyIds { get; set; }

        Int32 SelectedDefinedRequirementID { get; set; }
        
        //UAT-2551
        Int32 OrganisationUserID { get; }
    }
}
