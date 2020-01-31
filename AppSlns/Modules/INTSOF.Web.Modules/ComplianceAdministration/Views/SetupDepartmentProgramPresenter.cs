#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;
using INTSOF.UI.Contract.SystemSetUp;
#endregion

#region UserDefined

using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using Entity.ClientEntity;

#endregion

#endregion
namespace CoreWeb.ComplianceAdministration.Views
{
    public class SetupDepartmentProgramPresenter : Presenter<ISetupDepartmentProgramView>
    {
        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties


        #endregion

        #endregion

        #region Methods

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed the every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.ListTenants = ComplianceDataManager.getClientTenant();
        }

        public void GetTreeData()
        {
            //ObjectResult<GetDepartmentTree> objDepartmentTree = ComplianceSetupManager.GetDepartmentTree(View.SelectedTenant, View.DepartmentId);
            List<InstituteHierarchyTreeDataContract> objInstituteHierarchyTree = ComplianceSetupManager.GetInstituteHierarchyTree(View.SelectedTenant, IsAdminLoggedIn() ? (int?)null : View.CurrentUserId);
            View.lstTreeData = objInstituteHierarchyTree.OrderBy(x => x.DPM_DisplayOrder).ThenBy(x => x.TreeNodeTypeID).ToList();
        }

        /// <summary>
        /// Sets the navigation URL, Color Code and Font Code as per node type.
        /// </summary>
        /// <param name="uiCode">UI Code of a node.</param>
        /// <param name="dataID">DataID of a node.</param>
        /// <param name="parentDataID">Parent Node DataID of a node.</param>
        /// <returns>ComplianceTreeUIContract</returns>
        public ComplianceTreeUIContract GetTreeNodeDetails(String uiCode, Int32 dataID, Int32? parentDataID, String parentNodeID, String nodeID, Int32 mappingID, String treeNodeValue, Int32 EntityID, Boolean? IsAvailableforOrder, Boolean? IsPackageBundleAvailableforOrder)
        {
            ComplianceTreeUIContract complianceTreeUIContract = new ComplianceTreeUIContract();

            Dictionary<Int32, String> nodeUniqueKeyPackageID = GetUniqueKeyPackageIDForNode(nodeID, dataID, uiCode);
            String uniqueKey = nodeUniqueKeyPackageID.FirstOrDefault().Value;
            //String uniqueKey = nodeID;

            //Int32 packageDataID = nodeUniqueKeyPackageID.FirstOrDefault().Key;
            //Int32 parentNodeTypeID = GetTreeNodeTypeID(parentNodeID);

            //Sets Navigation URL, Value, ColorCode, FontBold and IsExpand properties for a node.
            complianceTreeUIContract.Value = uniqueKey;
            complianceTreeUIContract.ColorCode = "DarkBlue";
            complianceTreeUIContract.FontBold = true;
            complianceTreeUIContract.IsExpand = false;
            switch (uiCode)
            {
                //InstituteHierarchy Node 
                case RuleSetTreeNodeType.InstituteHierarchyNode:
                    complianceTreeUIContract.NavigateURL = "InstituteHierarchyNodePackage.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&NodeID=" + uniqueKey + "&RecordTypeCode=" + RecordType.Institution_Node.GetStringValue(); //Node
                    return complianceTreeUIContract;

                //CompliancePackage
                case RuleSetTreeNodeType.CompliancePackage:
                    complianceTreeUIContract.NavigateURL = "SubscriptionPackage.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&MappingID=" + mappingID + "&NodeID=" + uniqueKey + "&RecordTypeCode=" + RecordType.Compliance_Package.GetStringValue() + "&CompliancePkgId=" + EntityID; //Compliance package
                    complianceTreeUIContract.ColorCode = (IsAvailableforOrder.HasValue && IsAvailableforOrder.Value == false) ? "Red" : "Green";
                    return complianceTreeUIContract;

                //Subscription 
                case RuleSetTreeNodeType.Subscription:
                    complianceTreeUIContract.NavigateURL = "PackagePrice.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&MappingID=" + mappingID + "&NodeID=" + uniqueKey + "&ParentNodeID=" + parentNodeID + "&TreeNodeType=" + RuleSetTreeNodeType.Subscription + "&TreeNodeValue=" + treeNodeValue;
                    return complianceTreeUIContract;

                //Category 
                case RuleSetTreeNodeType.Category:
                    complianceTreeUIContract.NavigateURL = "PackagePrice.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&MappingID=" + mappingID + "&NodeID=" + uniqueKey + "&ParentNodeID=" + parentNodeID + "&TreeNodeType=" + RuleSetTreeNodeType.Category + "&TreeNodeValue=" + treeNodeValue;
                    return complianceTreeUIContract;

                //Item 
                case RuleSetTreeNodeType.Item:
                    complianceTreeUIContract.NavigateURL = "PackagePrice.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&MappingID=" + mappingID + "&NodeID=" + uniqueKey + "&ParentNodeID=" + parentNodeID + "&TreeNodeType=" + RuleSetTreeNodeType.Item + "&TreeNodeValue=" + treeNodeValue;
                    return complianceTreeUIContract;
                case RuleSetTreeNodeType.PackageBundle:
                    complianceTreeUIContract.NavigateURL = "PackageBundle.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&MappingID=" + mappingID + "&NodeID=" + uniqueKey + "&ParentNodeID=" + parentNodeID + "&TreeNodeType=" + RuleSetTreeNodeType.PackageBundle + "&TreeNodeValue=" + treeNodeValue;
                    complianceTreeUIContract.ColorCode = (IsPackageBundleAvailableforOrder.HasValue && IsPackageBundleAvailableforOrder.Value == true) ? "DarkOrange" : "Red";
                    return complianceTreeUIContract;
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
            List<InstituteHierarchyTreeDataContract> lstDeptTree = View.lstTreeData;
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
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
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

        #endregion
    }
}




