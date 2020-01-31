using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IManageAgencyHierarchyPackageView
    {
        IManageAgencyHierarchyPackageView CurrentViewContext { get; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 AgencyHierarchyId { get; set; }
      
        List<RequirementPackageContract> lstRequirementPackage { get; set; }

        AgencyHierarchyPackageContract agencyHierarchyPackageContract { get; set; }

        List<RequirementPackageContract> lstAgencyHierarchyPackages { get; set; }

        //Dictionary<Int32, Boolean> lstSelectedRotPackage { get; set; }

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

    }
}
