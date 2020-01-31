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
    public class PackageCopyPresenter : Presenter<IPackageCopyView>
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
        /// To copy package structure and data
        /// </summary>
        public void CopyPackageStructure()
        {
            Int32 defaultTenantId = SecurityManager.DefaultTenantID;
            View.ErrorMessage = String.Empty;
            //Copy to Master
            if (View.MenuItemValue.ToLower() == "menuitemcopymaster")
            {
                if (ComplianceSetupManager.CheckIfPackageNameAlreadyExist(View.CompliancePackageName, defaultTenantId))
                {
                    View.ErrorMessage = "Package Name already exists.";
                    return;
                }
                //Call Copy Package Structure Master SP
                ComplianceSetupManager.CopyPackageStructureToMaster(View.CompliancePackageID, View.CompliancePackageName, View.CurrentLoggedInUserId, View.TenantId);
            }
            //Copy to Client
            else if (View.MenuItemValue.ToLower() == "menuitemcopyclient")
            {
                if (ComplianceSetupManager.CheckIfPackageNameAlreadyExist(View.CompliancePackageName, View.SelectedTenantId))
                {
                    View.ErrorMessage = "Package Name already exists.";
                    return;
                }
                var compliancePackage = ComplianceSetupManager.GetCurrentPackageInfo(View.CompliancePackageID, defaultTenantId);
                if (compliancePackage.IsNotNull())
                {
                    //Check if package already copied in client DB
                    if (CheckIfPackAlreadyCopiedInClient(compliancePackage))
                    {
                        View.ErrorMessage = "Package " + compliancePackage.PackageName + " is already copied in this Institution.";
                        return;
                    }
                }
                //Call Copy Package Structure Client SP
                ComplianceSetupManager.CopyPackageStructureToClient(View.CompliancePackageID, View.CompliancePackageName, View.CurrentLoggedInUserId, View.SelectedTenantId);
            }
            //Create a Copy
            else
            {
                //While copy/insert package, packageID should be 0 to verify Package Name exists
                if (ComplianceSetupManager.CheckIfPackageNameAlreadyExist(View.CompliancePackageName, View.TenantId))
                {
                    View.ErrorMessage = "Package Name already exists.";
                    return;
                }
                ComplianceSetupManager.CopyPackageStructure(View.CompliancePackageID, View.CompliancePackageName, View.CurrentLoggedInUserId, View.TenantId,false,AppConsts.NONE,AppConsts.NONE);

                CompliancePackage copiedCompliancePackage = ComplianceSetupManager.GetCopiedCompliancePackage(View.TenantId, View.CompliancePackageID, View.CompliancePackageName);
                if (!copiedCompliancePackage.IsNullOrEmpty())
                {
                    if (!View.CopiedPackageId.IsNullOrEmpty() && !View.CopiedPackageName.IsNullOrEmpty())
                    {
                        View.CopiedPackageId = View.CopiedPackageId + "," + copiedCompliancePackage.CompliancePackageID;
                        View.CopiedPackageName = View.CopiedPackageName + "," + copiedCompliancePackage.PackageName;
                    }
                    else
                    {
                        View.CopiedPackageId = Convert.ToString(copiedCompliancePackage.CompliancePackageID);
                        View.CopiedPackageName = copiedCompliancePackage.PackageName;
                    }
                }
            }
        }

        /// <summary>
        /// To get all Tenants or Institutions
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String tenantTypeCodeForClient = TenantType.Institution.GetStringValue();
            Int32 defaultTenantId = SecurityManager.DefaultTenantID;
            View.Tenants = SecurityManager.GetTenants(SortByName).Where(obj => obj.lkpTenantType.TenantTypeCode == tenantTypeCodeForClient && obj.TenantID != defaultTenantId).ToList();
        }

        /// <summary>
        /// Check if package already copied in client DB
        /// </summary>
        /// <param name="compliancePackage"></param>
        /// <returns></returns>
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




