using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.PackageBundleManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.CommonOperations.Views
{
    public interface IManageBundle
    {

        List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }
        Int32 TenantID
        {
            get;
            set;
        }
        Int32 SelectedTenantID
        {
            get;
            set;
        }

        Int32 BundleId { get; set; }

        List<ManagePackageBundleContract> BundleDataList
        {
            get;
            set;
        }

        String BundleName { get; set; }
        String HierarchyNode { get; set; }
        String TrackingPackage { get; set; }
        String AdministrativePackage { get; set; }
        String Screeningpackage { get; set; }
        String BundleLabel { get; set; }
        String BundleDescription { get; set; }
        Int32 TrackingPackageID { get; set; }
        Int32 AdministrativePackageID { get; set; }
        Int32 ScreeningpackageID { get; set; }
        List<Int32> ScreeningpackageIDs { get; set; }
        String ExplanatoryNotes { get; set; }
        bool IsAvailableForOrder { get; set; }
        bool IsDeleted { get; set; }
        Int32 CurrentUserId { get; }

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

        Int32 SelectedTenantIDForAddForm
        {
            get;
            set;
        }

        ManagePackageBundleContract SearchContract
        {
            get;
            set;
        }

        //UAT-3157
        Int32 PreferredSelectedTenantID { get; set; }
    }
}
