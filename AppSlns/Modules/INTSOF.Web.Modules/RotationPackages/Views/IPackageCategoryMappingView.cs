using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface IPackageCategoryMappingView
    {
        Int32 AgencyHierarchyId { get; set; } //UAT-2634
        Int32 SelectedRootNodeID { get; set; }  //UAT-2634
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        String ReqCategoryName { get; set; }
        Int32 ReqPackageID { get; set; }
        Int32 ResultTypeID { get; set; }
        List<RequirementCategoryContract> LstRequirementCategory { get; set; }
        List<Int32> LstCategoryIdsMappedWithPackage { get; set; }
        List<Int32> LstUnMappedCategoryIds { get; set; }
        String MappedCategoryIds { get; set; }

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
