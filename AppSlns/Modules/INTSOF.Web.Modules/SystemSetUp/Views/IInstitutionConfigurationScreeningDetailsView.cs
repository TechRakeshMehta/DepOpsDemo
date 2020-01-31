using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.SystemSetUp;

namespace CoreWeb.SystemSetUp.Views
{
    public interface IInstitutionConfigurationScreeningDetailsView
    {
        IInstitutionConfigurationScreeningDetailsView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedTenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 DeptProgramMappingID { get; set; }
        Int32 PackageID { get; set; }
        String NodeLabel { get; set; }
        ScreeningDetailsForConfigurationContract ScreeningDetailsForConfigurationContract { get; set; }
        List<BackgroundPackageDetailsForConfigurationContract> BackgroundPackageDetailsForConfigurationList { get; set; }
        List<ServiceFormDetailsForConfigurationContract> ServiceFormDetailsForConfigurationList { get; set; }
        List<ServiceItemFeeDetailsForConfigurationContract> ServiceItemFeeDetailsList { get; set; }
        Int32 PackageHierarchyID
        {
            get;
            set;
        }
        #region UAT:2411
        String ParentScreenName
        {
            get;
            set;
        }
        Int32 BundlePackageID
        {
            get;
            set;
        }
        String MasterNodeLabel
        {
            get;
            set;
        }
        String BundleName
        {
            get;
            set;
        }
        #endregion
    }
}


