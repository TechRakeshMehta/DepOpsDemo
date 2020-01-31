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
    public interface IManageMasterRotationPackagesView
    {
        IManageMasterRotationPackagesView CurrentViewContext { get; }

        List<RequirementPackageDetailsContract> LstRequirementPackageDetailsContract
        {
            get;
            set;
        }

        String PackageName { get; set; }

        String RequirementPackageStatus { get; set; }

        List<AgencyDetailContract> LstAgency
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserID { get; }


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

        Guid CurrentUserID { get; }

        Int32 SelectedReqPackageTypeID
        {
            get;
            set;
        }

        Int32 AgencyId { get; set; }


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

        Int32 TenantId { get; set; }

        RequirementPackageContract RequirementPackageContractSessionData { get; set; }

        List<DefinedRequirementContract> lstDefinedRequirement { get; set; }

        Int32 SelectedDefinedRequirementID { get; set; }

        String LstSelectedAgencyHierarchyIDs { get; set; }
    }
}
