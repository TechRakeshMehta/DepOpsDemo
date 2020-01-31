using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface ICategoryPackageMappingView
    {
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        String ReqPackageName { get; set; }
        Int32 ReqCategoryID { get; set; }
        DateTime? EffStartDate { get; set; }
        DateTime? EffEndDate { get; set; }
        Int32 ResultTypeID { get; set; }
        List<RequirementPackageContract> LstRequirementPackages { get; set; }
        List<Int32> LstPackageIdsMappedWithCategory { get; set; }
        List<Int32> LstUnMappedPackageIds { get; set; }
        String MappedPkgIds { get; set; }

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
