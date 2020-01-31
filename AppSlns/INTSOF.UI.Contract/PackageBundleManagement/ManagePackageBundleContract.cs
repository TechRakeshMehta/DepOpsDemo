using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.PackageBundleManagement
{
    public class ManagePackageBundleContract
    {
        public Int32 BundleId { get; set; }
        public String BundleName { get; set; }
        public String HierarchyNode { get; set; }
        public String TrackingPackage { get; set; }
        public String AdministrativePackage { get; set; }
        public String ScreeningPackage { get; set; }
        public String PackageBundleLabel { get; set; }
        public String BundleDescription { get; set; }
        public Int32 TrackingPackageID { get; set; }
        public Int32 AdministrativePackageID { get; set; }
        public Int32 ScreeningPackageID { get; set; }
        public bool IsAvailableForOrder { get; set; }
        public Int32 TenantId { get; set; }
        public String HierarchyNodes { get; set; }
    }
}
