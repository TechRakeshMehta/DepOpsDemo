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
    public class InstitutionHierarchyListPresenter : Presenter<IInstitutionHierarchyListView>
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
            ObjectResult<InstituteHierarchyNodesList> objInstituteHierarchyTree = ComplianceSetupManager.GetInstituteHierarchyNodes(View.SelectedTenant, IsAdminLoggedIn() ? (int?)null : View.CurrentUserId); //UAT-3369
            View.lstTreeHierarchyData = objInstituteHierarchyTree.OrderBy(x => x.DPM_DisplayOrder).ThenBy(x => x.TreeNodeTypeID).Where(x => x.UICode != RuleSetTreeNodeType.CompliancePackage).ToList();
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
            //String uniqueKey = nodeID;

            //Int32 packageDataID = nodeUniqueKeyPackageID.FirstOrDefault().Key;
            //Int32 parentNodeTypeID = GetTreeNodeTypeID(parentNodeID);

            //Sets Navigation URL, Value, ColorCode, FontBold and IsExpand properties for a node.
            if (uiCode != RuleSetTreeNodeType.CompliancePackage)
            {
                complianceTreeUIContract.Value = uniqueKey;
                complianceTreeUIContract.ColorCode = "DarkBlue";
                complianceTreeUIContract.FontBold = true;
                complianceTreeUIContract.IsExpand = false;
            }
           
            

            //switch (uiCode)
            //{
            //    //InstituteHierarchy Node 
            //    case RuleSetTreeNodeType.InstituteHierarchyNode:
            //        //complianceTreeUIContract.NavigateURL = "InstituteHierarchyNodePackage.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&NodeID=" + uniqueKey;
            //        return complianceTreeUIContract;

            //    //CompliancePackage
            //    case RuleSetTreeNodeType.CompliancePackage:
            //        //complianceTreeUIContract.NavigateURL = "SubscriptionPackage.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&MappingID=" + mappingID + "&NodeID=" + uniqueKey;
            //        complianceTreeUIContract.ColorCode = "Green";
            //        return complianceTreeUIContract;

            //    //Subscription 
            //    case RuleSetTreeNodeType.Subscription:
            //        //complianceTreeUIContract.NavigateURL = "PackagePrice.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&MappingID=" + mappingID + "&NodeID=" + uniqueKey + "&ParentNodeID=" + parentNodeID + "&TreeNodeType=" + RuleSetTreeNodeType.Subscription + "&TreeNodeValue=" + treeNodeValue;
            //        return complianceTreeUIContract;

            //    //Category 
            //    case RuleSetTreeNodeType.Category:
            //        //complianceTreeUIContract.NavigateURL = "PackagePrice.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&MappingID=" + mappingID + "&NodeID=" + uniqueKey + "&ParentNodeID=" + parentNodeID + "&TreeNodeType=" + RuleSetTreeNodeType.Category + "&TreeNodeValue=" + treeNodeValue;
            //        return complianceTreeUIContract;

            //    //Item 
            //    case RuleSetTreeNodeType.Item:
            //        //complianceTreeUIContract.NavigateURL = "PackagePrice.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&MappingID=" + mappingID + "&NodeID=" + uniqueKey + "&ParentNodeID=" + parentNodeID + "&TreeNodeType=" + RuleSetTreeNodeType.Item + "&TreeNodeValue=" + treeNodeValue;
            //        return complianceTreeUIContract;
            //}
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
            List<InstituteHierarchyNodesList> lstDeptTree = View.lstTreeHierarchyData;
            Dictionary<Int32, String> nodeUniqueKeyPackageID = new Dictionary<Int32, String>();
            String uniqueKey = "Start_" + uiCode;
            Int32 packageID = 0;

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

        /// <summary>
        /// To get Tree Node Type ID
        /// </summary>
        /// <param name="ParenNodeID"></param>
        /// <returns></returns>
        private Int32 GetTreeNodeTypeID(String parentNodeID)
        {
            if (parentNodeID.IsNotNull())
            {
                return View.lstTreeHierarchyData.FirstOrDefault(obj => obj.NodeID == parentNodeID).TreeNodeTypeID;
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

        public void GetHierarchyLabel(Int32 depPrgMappingId)
        {
            DeptProgramMapping depProgramMapping= ComplianceSetupManager.GetDepartmentProgMapping(View.SelectedTenant, depPrgMappingId);
            if (depProgramMapping.IsNotNull())
            {
                View.InstitutionNodeId = depProgramMapping.DPM_InstitutionNodeID;
                View.HierarchyLabel = depProgramMapping.DPM_Label;
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




