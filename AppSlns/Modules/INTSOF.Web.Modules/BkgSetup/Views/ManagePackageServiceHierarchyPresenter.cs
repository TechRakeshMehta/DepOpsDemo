#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;

#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;
using System.Xml;

#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public class ManagePackageServiceHierarchyPresenter : Presenter<IManagePackageServiceHierarchyView>
    {
        #region Properties

        #region Private Properties



        #endregion

        #region Public Properties



        #endregion

        #endregion

        #region Methods

        #region Private Methods

        #endregion

        #region Public Methods

        /// <summary>
        /// Method called when SetUp page view is loaded.
        /// </summary>
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads            
        }

        public void GetTreeData()
        {
            if (View.SelectedTenantId > 0)
            {
                String packageIds = GetPackageIds(View.SelectedBkgPackageIdList);
                View.lstTreeData = ComplianceDataManager.GetPackageTreeForServiceMapping(View.SelectedTenantId, packageIds);
                PkgSvcSetupContract pkgSvcSetupContract = View.lstTreeData.FirstOrDefault(x => x.ParentNodeId == "");
                pkgSvcSetupContract.ParentNodeId = null;
                var rootNode = pkgSvcSetupContract;
                View.lstTreeData.Remove(pkgSvcSetupContract);
                View.lstTreeData.Add(rootNode);
                List<PkgSvcSetupContract> lstTempTreeData = View.lstTreeData.OrderBy(cnd => cnd.ATTRDisplayOrder).ToList();
                View.lstTreeData = lstTempTreeData;
            }
        }

        public String GetNodeDetails(PkgSvcSetupContract pkgSvcSetupContract)
        {
            String url = String.Empty;
            String nodeId = pkgSvcSetupContract.NodeId;
            String code = pkgSvcSetupContract.Code;
            switch (code)
            {
                case "LPKG":
                    url = "SetupPackagesForInsitiute.aspx?nodeId=" + nodeId + "&tenantId=" + View.SelectedTenantId;
                    break;
                case "PKG":
                    url = "SetUpServiceGroupsForPackage.aspx?nodeId=" + nodeId + "&tenantId=" + View.SelectedTenantId + "&currentUserID=" + View.CurrentUserId;
                    break;
                case "SVCG":
                    url = "SetUpServiceForServicegroup.aspx?nodeId=" + nodeId + "&tenantId=" + View.SelectedTenantId;
                    break;
                case "SVC":
                    url = "SetUpAttributeGroupForServices.aspx?nodeId=" + nodeId + "&tenantId=" + View.SelectedTenantId;
                    break;
                case "SVCC":
                    url = "SetUpAttributeGroupForServices.aspx?nodeId=" + nodeId + "&tenantId=" + View.SelectedTenantId;
                    break;
                case "ATTG":
                    url = "SetUpAttributeForAttributeGroup.aspx?nodeId=" + nodeId + "&tenantId=" + View.SelectedTenantId;
                    break;
                case "ATT":
                    url = "EditAttribute.aspx?nodeId=" + nodeId + "&tenantId=" + View.SelectedTenantId;
                    break;
                default:
                    url = "ManagePackageServiceHierarchy.aspx?nodeId=" + nodeId + ",tenantId=" + View.SelectedTenantId;
                    break;
            }
            return url;
        }

        /// <summary>
        /// Method called when View is initialized.
        /// </summary>
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetTenants()
        {
            View.ListTenants = ComplianceDataManager.GetTenantList(View.DefaultTenantId);
        }

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }
        #region Package DropDown Changes :UAT-1116

        private String GetPackageIds(List<Int32> lstPackageIds)
        {
            StringBuilder sbPackageIds = new StringBuilder();
            String packageIds = null;
            foreach (Int32 id in lstPackageIds)
            {
                sbPackageIds.Append(Convert.ToString(id) + ",");
            }

            packageIds = sbPackageIds.ToString().Remove(sbPackageIds.Length - 1).TrimEnd();
            return packageIds;
        }

        public void GetBkgPackages()
        {
            List<BackgroundPackage> tempBkgPackages = new List<BackgroundPackage>();
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                tempBkgPackages = BackgroundSetupManager.GetPermittedBackgroundPackagesByUserID(View.SelectedTenantId);
            }
            if (!tempBkgPackages.IsNullOrEmpty())
            {
                tempBkgPackages = tempBkgPackages.OrderBy(col=>col.BPA_Name).ToList();
            }
            View.lstBackgroundPackage = tempBkgPackages;
        }
        #endregion
        #endregion

        #endregion
    }
}
