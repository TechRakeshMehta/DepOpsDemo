using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IPackageDetailsView
    {
        Dictionary<string, Int32> DPPSIds { get; set; }
        Int32 TenantId { get; set; }
        Dictionary<string, DeptProgramPackageSubscription> SelectedPackageDetails { get; set; }
        IPackageDetailsView CurrentViewContext { get; }
        Int32 CurrentLoggedInUserId
        {
            get;
        }
        String DetailViewType
        {
            get;
            set;
        }

        List<BackgroundPackagesContract> lstExternalPackages
        {
            get;
            set;
        }
        Boolean IsLocationServiceTenant { get; set; }
        #region UAT-3601
        String PackageDetailHeaderLabel { set; }
        String ChangePackageSelectionButtonLabel { set; }

        #endregion

        String LanguageCode { get; }

        #region Admin Entry Portal
        Boolean IsAdminEntryTenant { get; set; }
        #endregion
    }
}




