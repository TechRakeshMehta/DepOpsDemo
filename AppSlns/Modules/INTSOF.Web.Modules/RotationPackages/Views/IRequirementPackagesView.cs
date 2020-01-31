using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface IRequirementPackagesView
    {

        IRequirementPackagesView CurrentViewContext { get; }

       // Int32 TenantID { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        List<RequirementPackageContract> lstRequirementPackage { get; set; }

        String LstSelectedAgencyIDs { get; set; }

        List<Int32> LstAgencyHeirarchyIds { get; set; }

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
