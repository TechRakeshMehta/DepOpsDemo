using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.SystemSetUp;

namespace CoreWeb.SystemSetUp.Views
{
    public interface ICompliancePkgDetailsView
    {
        /// <summary>
        /// Gets the data for Tree List.
        /// </summary>
        CompliancePkgDetailContract CompliancePkgDetails
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Int32 DeptProgramMappingID
        {
            get;
            set;
        }

        Int32 PackageHierarchyID
        {
            get;
            set;
        }

        Int32 PackageID
        {
            get;
            set;
        }

        String NodeLabel
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
