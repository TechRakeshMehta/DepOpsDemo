using INTSOF.UI.Contract.SystemSetUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.SystemSetUp.Views
{
    public interface IBundleDetailsView
    {
        Int32 PackageHierarchyID
        {
            get;
            set;
        }
        IBundleDetailsView CurrentViewContext { get; }
        String BundleName { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 DeptProgramMappingID { get; set; }
        Int32 ParentID { get; set; }
        Int32 NodeId { get; set; }
        Int32 OrganizationUserID { get; set; }
        String NodeLabel { get; set; }
        String MasterNodeLabel { get; set; }
        Int32 BundlePackageID { get; set; }
        List<InstitutionConfigurationPackageDetails> InstitutionConfigurationPackageDetailsList { get; set; }
    }
}
