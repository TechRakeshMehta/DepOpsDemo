#region Namespaces

#region SystemDefined
using Microsoft.Practices.ObjectBuilder;
using System.Linq;
using System.Data.Entity.Core.Objects;
using System;
using System.Collections.Generic;
using System.Text;
#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceProxy.Modules.AgencyHierarchy;
using INTSOF.SharedObjects;
#endregion

#endregion

namespace CoreWeb.CommonOperations.Views
{
    public class InstituteHierarchyPackageListPresenter: Presenter<IInstituteHierarchyPackageListView>
    {
        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            //View.SelectedTenant = GetTenantId();

            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void GetTreeData()
        {
            //ObjectResult<GetDepartmentTree> objDepartmentTree = ComplianceSetupManager.GetDepartmentTree(View.SelectedTenant, View.DepartmentId);
            List<GetDepartmentTree> objInstituteHierarchyTree = ComplianceSetupManager.GetInstituteHierarchyPackageTree(View.SelectedTenant, IsAdminLoggedIn() ? (int?)null : View.CurrentUserId,View.CompliancePackageTypeCode, View.IsCompliancePackage);
            View.lstTreeData = objInstituteHierarchyTree.OrderBy(x => x.DPM_DisplayOrder).ThenBy(x => x.TreeNodeTypeID).ToList();
        }
        public Dictionary<Int32, String> GetUniqueKeyPackageIDForNode(String nodeID, Int32 dataID, String uiCode)
        {
            List<GetDepartmentTree> lstDeptTree = View.lstTreeData;
            Dictionary<Int32, String> nodeUniqueKeyPackageID = new Dictionary<Int32, String>();
            String uniqueKey = "Start_" + uiCode;
            Int32 packageID = 0;

            while (true)
            {
                var DeptTree = lstDeptTree.FirstOrDefault(x => x.NodeID == nodeID);
                if (DeptTree.IsNotNull())
                {
                    uniqueKey += "_" + DeptTree.EntityID.ToString();
                    if (DeptTree.ParentNodeID.IsNull())
                    {
                        break;
                    }
                    //if (DeptTree.UICode == RuleSetTreeNodeType.Package)
                    //{
                    //    packageID = DeptTree.DataID;
                    //}
                    nodeID = DeptTree.ParentNodeID;
                }
            }
            nodeUniqueKeyPackageID.Add(packageID, uniqueKey);
            return nodeUniqueKeyPackageID;
        }
    }
}
