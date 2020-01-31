#region Namespaces

#region System Defined

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;

#endregion

#region Application Specific

using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public class PackageCopyToOtherClientPresenter : Presenter<IPackageCopyToOtherClientView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        /// <summary>
        /// To get all tenants or institutions.
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String tenantTypeCodeForClient = TenantType.Institution.GetStringValue();
            Int32 defaultTenantId = SecurityManager.DefaultTenantID;
            View.Tenants = SecurityManager.GetTenants(SortByName, false, tenantTypeCodeForClient).Where(obj => obj.TenantID != View.TenantId && obj.TenantID != View.DefaultTenantId).ToList();
        }

        /// <summary>
        /// To copy package structure and data
        /// </summary>
        public void CopyPackageStructure()
        {
            Int32 defaultTenantId = SecurityManager.DefaultTenantID;
            View.ErrorMessage = String.Empty;
            
            //While copy/insert package, packageID should be 0 to verify Package Name exists
            if (ComplianceSetupManager.CheckIfPackageNameAlreadyExist(View.CompliancePackageName, View.SelectedTenantId))
            {
                View.ErrorMessage = "Package Name already exists in this Institution.";
                return;
            }
            ComplianceSetupManager.CopyPackageStructureToOtherClient(View.TenantId, View.CompliancePackageID, View.CompliancePackageName, View.CurrentLoggedInUserId, View.SelectedTenantId);

            //CompliancePackage copiedCompliancePackage = ComplianceSetupManager.GetCopiedCompliancePackage(View.TenantId, View.CompliancePackageID, View.CompliancePackageName);
            //if (!copiedCompliancePackage.IsNullOrEmpty())
            //{
            //    if (!View.CopiedPackageId.IsNullOrEmpty() && !View.CopiedPackageName.IsNullOrEmpty())
            //    {
            //        View.CopiedPackageId = View.CopiedPackageId + "," + copiedCompliancePackage.CompliancePackageID;
            //        View.CopiedPackageName = View.CopiedPackageName + "," + copiedCompliancePackage.PackageName;
            //    }
            //    else
            //    {
            //        View.CopiedPackageId = Convert.ToString(copiedCompliancePackage.CompliancePackageID);
            //        View.CopiedPackageName = copiedCompliancePackage.PackageName;
            //    }

            //}
        }

        public Boolean CheckIfPackAlreadyCopiedInClient(CompliancePackage compliancePackage)
        {
            var compliancePackageClient = ComplianceSetupManager.GetPackageDetailsByCode(View.SelectedTenantId, compliancePackage.Code);
            if (compliancePackageClient.IsNotNull())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
