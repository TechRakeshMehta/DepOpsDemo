using System;
using System.Collections.Generic;
using System.Text;
#region Namespaces

#region SystemDefined
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
using INTSOF.UI.Contract.ComplianceOperation;

#endregion
#endregion


namespace CoreWeb.ComplianceOperations.Views
{
    public class InstitutionNodeHierarchyWithPermissionsPresenter : Presenter<IInstitutionNodeHierarchyWithPermissionsView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }
        public void GetTreeData()
        {
            if (View.ScreenName == "CommonScreen")
            {
                ObjectResult<GetDepartmentTree> objInstitutionHierarchyTree = ComplianceSetupManager.GetInstituteHierarchyTreewithPermissions(View.SelectedTenant, IsAdminLoggedIn() ? (int?)null : View.CurrentUserId);
                View.lstTreeData = objInstitutionHierarchyTree.OrderBy(x => x.DPM_DisplayOrder).ThenBy(x => x.TreeNodeTypeID).Where(x => x.UICode != RuleSetTreeNodeType.CompliancePackage).ToList();
            }
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            //View.SelectedTenant = GetTenantId();

            //Checked if logged user is admin or not.
            if (GetTenantId() == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentUserId).Organization.TenantID.Value;
        }

        public void GetHierarchyLabel(String delimittedDepPrgMappingId)
        {
            List<DeptProgramMapping> depProgramMappingLst = ComplianceSetupManager.GetDepartmentProgMappingList(View.SelectedTenant, delimittedDepPrgMappingId);

            if (depProgramMappingLst.IsNotNull())
            {
                foreach (DeptProgramMapping depProgramMapping in depProgramMappingLst)
                {
                    View.InstitutionNodeId += depProgramMapping.DPM_InstitutionNodeID + ",";
                    View.HierarchyLabel += depProgramMapping.DPM_Label + ", ";
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiCode"></param>
        /// <param name="dataID"></param>
        /// <param name="parentDataID"></param>
        /// <param name="parentNodeID"></param>
        /// <param name="nodeID"></param>
        /// <param name="mappingID"></param>
        /// <param name="treeNodeValue"></param>
        /// <returns></returns>
        public ComplianceTreeUIContract GetTreeNodeDetails(String uiCode, Int32 dataID, Int32? parentDataID, String parentNodeID, String nodeID, Int32 mappingID, String treeNodeValue, String PermissionCode)
        {
            ComplianceTreeUIContract complianceTreeUIContract = new ComplianceTreeUIContract();

            Dictionary<Int32, String> nodeUniqueKeyPackageID = GetUniqueKeyPackageIDForNode(nodeID, dataID, uiCode);
            String uniqueKey = nodeUniqueKeyPackageID.FirstOrDefault().Value;

            //Sets Navigation URL, Value, ColorCode, FontBold and IsExpand properties for a node.
            if (uiCode != RuleSetTreeNodeType.CompliancePackage)
            {
                complianceTreeUIContract.Value = uniqueKey;
                complianceTreeUIContract.ColorCode = "DarkBlue";
                complianceTreeUIContract.FontBold = true;
                complianceTreeUIContract.IsExpand = false;
            }
            //UAt-2339
            if (!PermissionCode.IsNullOrEmpty())
            {
                complianceTreeUIContract.PermissionCode = PermissionCode;
            }

            return complianceTreeUIContract;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="dataID"></param>
        /// <param name="uiCode"></param>
        /// <returns></returns>
        private Dictionary<Int32, String> GetUniqueKeyPackageIDForNode(String nodeID, Int32 dataID, String uiCode)
        {
            Dictionary<Int32, String> nodeUniqueKeyPackageID = new Dictionary<Int32, String>();
            String uniqueKey = "Start_" + uiCode;
            Int32 packageID = 0;
            //UAT-1181: Ability to restrict additional nodes to the order queue
            //if (View.ScreenName.IsNullOrEmpty() || View.ScreenName == "BackgroundScreen")
            if (View.ScreenName == "CommonScreen")
            {
                List<GetDepartmentTree> lstDeptTree = View.lstTreeData;

                while (true)
                {
                    var DeptTree = lstDeptTree.FirstOrDefault(x => x.NodeID == nodeID && x.UICode != RuleSetTreeNodeType.CompliancePackage);
                    if (DeptTree.IsNotNull())
                    {
                        uniqueKey += "_" + DeptTree.EntityID.ToString();

                        if (DeptTree.ParentNodeID.IsNull())
                        {
                            break;
                        }
                        nodeID = DeptTree.ParentNodeID;
                    }
                }
                nodeUniqueKeyPackageID.Add(packageID, uniqueKey);

            }
            return nodeUniqueKeyPackageID;

        }

        //UAT-3952
        public void BindPageControls()
        {
            List<String> _lstCodeForColumnConfig = new List<String>();
            _lstCodeForColumnConfig.Add((Screen.trlInstituteHierarchy).GetStringValue());
            var lstScreenColumnData = SecurityManager.GetScreenColumns(_lstCodeForColumnConfig, View.CurrentUserId);
            View.isHierarchyCollapsed = !lstScreenColumnData.Select(sel => sel.IsColumnVisible).FirstOrDefault();
            View.screenColumnID = lstScreenColumnData.Select(sel => sel.ColumnID).FirstOrDefault();
        }

        public Boolean SaveUserScreenColumnMapping(Dictionary<Int32, Boolean> columnVisibility)
        {
            return SecurityManager.SaveUserScreenColumnMapping(columnVisibility, View.CurrentUserId, View.CurrentUserId);
        }
    }
}
