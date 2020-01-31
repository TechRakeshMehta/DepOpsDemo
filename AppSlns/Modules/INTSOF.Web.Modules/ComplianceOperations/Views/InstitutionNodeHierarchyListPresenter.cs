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

#endregion

#endregion



namespace CoreWeb.ComplianceOperations.Views
{
    public class InstitutionNodeHierarchyListPresenter : Presenter<IInstitutionNodeHierarchyListView>
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
            if (View.ScreenName.IsNullOrEmpty())
            {
                //ObjectResult<InstituteHierarchyNodesList> objInstituteHierarchyTree = ComplianceSetupManager.GetInstituteHierarchyNodes(View.SelectedTenant, IsAdminLoggedIn() ? (int?)null : View.CurrentUserId);
                //View.lstTreeHierarchyData = objInstituteHierarchyTree.OrderBy(x => x.DPM_DisplayOrder).ThenBy(x => x.TreeNodeTypeID).Where(x => x.UICode != RuleSetTreeNodeType.CompliancePackage).ToList(); //UAT-3369
                ObjectResult<GetDepartmentTree> objInstituteHierarchyTree = ComplianceSetupManager.GetInstituteHierarchyTreeCommon(View.SelectedTenant, IsAdminLoggedIn() ? (int?)null : View.CurrentUserId, View.IsRequestFromAddRotationScreen);
                View.lstTreeData = objInstituteHierarchyTree.OrderBy(x => x.DPM_DisplayOrder).ThenBy(x => x.TreeNodeTypeID).Where(x => x.UICode != RuleSetTreeNodeType.CompliancePackage).ToList(); // UAT-4056
            }
            else if (View.ScreenName == "CommonScreen")
            {
                ObjectResult<GetDepartmentTree> objInstituteHierarchyTree = ComplianceSetupManager.GetInstituteHierarchyTreeCommon(View.SelectedTenant, IsAdminLoggedIn() ? (int?)null : View.CurrentUserId, View.IsRequestFromAddRotationScreen);
                View.lstTreeData = objInstituteHierarchyTree.OrderBy(x => x.DPM_DisplayOrder).ThenBy(x => x.TreeNodeTypeID).Where(x => x.UICode != RuleSetTreeNodeType.CompliancePackage).ToList();
            }
            else if (View.ScreenName == "OrderQueue")
            {
                ObjectResult<GetInstituteHierarchyOrderTree> objInstituteHierarchyTree = ComplianceSetupManager.GetInstituteHierarchyOrderTree(View.SelectedTenant, IsAdminLoggedIn() ? (int?)null : View.CurrentUserId);
                View.lstOrderTreeData = objInstituteHierarchyTree.OrderBy(x => x.DPM_DisplayOrder).ThenBy(x => x.TreeNodeTypeID).Where(x => x.UICode != RuleSetTreeNodeType.CompliancePackage).ToList();
            }
            else if (View.ScreenName == "BackgroundScreen")
            {
                ObjectResult<GetDepartmentTree> objInstituteHierarchyTree = ComplianceSetupManager.GetInstituteHierarchyTreeForBackgroundHierarchyPermissionType(View.SelectedTenant, IsAdminLoggedIn() ? (int?)null : View.CurrentUserId);
                View.lstTreeData = objInstituteHierarchyTree.OrderBy(x => x.DPM_DisplayOrder).ThenBy(x => x.TreeNodeTypeID).Where(x => x.UICode != RuleSetTreeNodeType.CompliancePackage).ToList();
            }
        }

        /// <summary>
        /// Sets the navigation URL, Color Code and Font Code as per node type.
        /// </summary>
        /// <param name="uiCode">UI Code of a node.</param>
        /// <param name="dataID">DataID of a node.</param>
        /// <param name="parentDataID">Parent Node DataID of a node.</param>
        /// <returns>ComplianceTreeUIContract</returns>
        public ComplianceTreeUIContract GetTreeNodeDetails(String uiCode, Int32 dataID, Int32? parentDataID, String parentNodeID, String nodeID, Int32 mappingID, String treeNodeValue)
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


            return complianceTreeUIContract;
        }

        /// <summary>
        /// To get Unique Key Package ID for Node
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
            if (View.ScreenName != "OrderQueue")
            {
                List<GetDepartmentTree> lstDeptTree;

                if (!View.lstTreeData.IsNullOrEmpty())
                {
                    lstDeptTree = View.lstTreeData;

                }
                else
                {
                    lstDeptTree = View.lstTreeHierarchyData
                        .Select(x => new GetDepartmentTree
                        {
                            NodeID = x.NodeID,
                            ParentNodeID = x.ParentNodeID,
                            UICode = x.UICode,
                            EntityID = x.EntityID
                        }).ToList();
                }

                while (true)
                {
                    var DeptTree = lstDeptTree.FirstOrDefault(x => x.NodeID == nodeID && x.UICode != RuleSetTreeNodeType.CompliancePackage);
                    if (DeptTree.IsNotNull())
                    {
                        uniqueKey += "_" + DeptTree.EntityID.ToString();
                        //uniqueKey += "_" + DeptTree.MappingID.ToString();
                        if (DeptTree.ParentNodeID.IsNull())
                        {
                            break;
                        }
                        nodeID = DeptTree.ParentNodeID;
                    }
                }
                nodeUniqueKeyPackageID.Add(packageID, uniqueKey);
                return nodeUniqueKeyPackageID;
            }
            else
            {
                List<GetInstituteHierarchyOrderTree> lstDeptTree = View.lstOrderTreeData;

                while (true)
                {
                    var DeptTree = lstDeptTree.FirstOrDefault(x => x.NodeID == nodeID && x.UICode != RuleSetTreeNodeType.CompliancePackage);
                    if (DeptTree.IsNotNull())
                    {
                        uniqueKey += "_" + DeptTree.EntityID.ToString();
                        //uniqueKey += "_" + DeptTree.MappingID.ToString();
                        if (DeptTree.ParentNodeID.IsNull())
                        {
                            break;
                        }
                        nodeID = DeptTree.ParentNodeID;
                    }
                }
                nodeUniqueKeyPackageID.Add(packageID, uniqueKey);
                return nodeUniqueKeyPackageID;
            }
        }

        /// <summary>
        /// To get Tree Node Type ID
        /// </summary>
        /// <param name="ParenNodeID"></param>
        /// <returns></returns>
        private Int32 GetTreeNodeTypeID(String parentNodeID)
        {
            if (parentNodeID.IsNotNull())
            {
                return View.lstTreeData.FirstOrDefault(obj => obj.NodeID == parentNodeID).TreeNodeTypeID;
            }
            else
            {
                return 0;
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