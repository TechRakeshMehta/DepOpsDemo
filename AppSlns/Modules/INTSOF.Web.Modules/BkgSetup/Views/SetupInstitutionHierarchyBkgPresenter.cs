using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public class SetupInstitutionHierarchyBkgPresenter : Presenter<ISetupInstitutionHierarchyBkgView>
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
            if (View.SelectedTenant.IsNotNull() && View.SelectedTenant != AppConsts.NONE)
            {
                List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> objInstituteHierarchyTree = BackgroundSetupManager.GetBackgroundInstituteHierarchyTree(IsAdminLoggedIn() ? (int?)null : View.CurrentUserId, View.SelectedTenant);
                View.lstTreeData = objInstituteHierarchyTree.OrderBy(x => x.DPM_DisplayOrder).ThenBy(x => x.TreeNodeTypeID).ToList();
            }
            else
            {
                View.lstTreeData = new List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract>();
            }

        }

        /// <summary>
        /// Sets the navigation URL, Color Code and Font Code as per node type.
        /// </summary>
        /// <param name="uiCode">UI Code of a node.</param>
        /// <param name="dataID">DataID of a node.</param>
        /// <param name="parentDataID">Parent Node DataID of a node.</param>
        /// <returns>ComplianceTreeUIContract</returns>
        public ComplianceTreeUIContract GetTreeNodeDetails(String uiCode, Int32 dataID, Int32? parentDataID, String parentNodeID, String nodeID, Int32 mappingID, String treeNodeValue, Int32 entityId, Boolean? IsAvailableforOrder, Boolean? IsPackageBundleAvailableforOrder, String packageColorCode)
        {
            ComplianceTreeUIContract complianceTreeUIContract = new ComplianceTreeUIContract();

            Dictionary<Int32, String> nodeUniqueKeyPackageID = GetUniqueKeyPackageIDForNode(nodeID, dataID, uiCode);
            String uniqueKey = nodeUniqueKeyPackageID.FirstOrDefault().Value;
            //String uniqueKey = nodeID;
            Int32 packageDataID = nodeUniqueKeyPackageID.FirstOrDefault().Key;
            //Sets Navigation URL, Value, ColorCode, FontBold and IsExpand properties for a node.
            complianceTreeUIContract.Value = uniqueKey;
            complianceTreeUIContract.FontBold = true;
            complianceTreeUIContract.IsExpand = false;
            switch (uiCode)
            {
                //InstituteHierarchy Node 
                case RuleSetTreeNodeType.InstituteHierarchyNode:
                    complianceTreeUIContract.NavigateURL = "InstituteHierarchyNodePackageBkg.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&NodeID=" + uniqueKey + "&NodeName=" + treeNodeValue + "&RecordTypeCode=" + RecordType.Institution_Node.GetStringValue();
                    return complianceTreeUIContract;

                //CompliancePackage
                case RuleSetTreeNodeType.CompliancePackage:
                    complianceTreeUIContract.NavigateURL = "BkgPackagePriceSetUp.aspx?BackGroundPackageId=" + dataID + "&ParentID=" + parentDataID + "&NodeID=" + uniqueKey + "&RecordTypeCode=" + RecordType.Background_Package.GetStringValue() + "&BPAId=" + entityId;
                    complianceTreeUIContract.ColorCode = packageColorCode;
                    return complianceTreeUIContract;

                //Contact 
                case RuleSetTreeNodeType.Contact:
                    complianceTreeUIContract.NavigateURL = "ContactDetaill.aspx?ContactId=" + dataID + "&ParentID=" + parentDataID + "&NodeID=" + uniqueKey;
                    return complianceTreeUIContract;

                //Service 
                case RuleSetTreeNodeType.Service:
                    complianceTreeUIContract.NavigateURL = "ManageServiceDetail.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&NodeID=" + uniqueKey + "&SrvcId=" + entityId;
                    return complianceTreeUIContract;

                //Service Item
                case RuleSetTreeNodeType.ServiceItem:
                    complianceTreeUIContract.NavigateURL = "ManageServiceItemDetail.aspx?Id=" + dataID + "&NodeID=" + uniqueKey;
                    return complianceTreeUIContract;

                //Rule Set
                case RuleSetTreeNodeType.RuleSet:
                    complianceTreeUIContract.NavigateURL = "ManageRulesetBkg.aspx?Id=" + dataID + "&NodeID=" + uniqueKey + "&PackageId=" + packageDataID;
                    return complianceTreeUIContract;

                //Rule
                case RuleSetTreeNodeType.Rule:
                    complianceTreeUIContract.NavigateURL = "ManageRulesBkg.aspx?Id=" + dataID + "&NodeID=" + uniqueKey + "&PackageId=" + packageDataID;
                    return complianceTreeUIContract;

                //ServiceFeeItem
                case RuleSetTreeNodeType.ServiceFeeItem:
                    complianceTreeUIContract.NavigateURL = "ManageFeeItemDetail.aspx?Id=" + dataID + "&NodeID=" + uniqueKey;
                    return complianceTreeUIContract;
                case RuleSetTreeNodeType.PackageBundle:
                    complianceTreeUIContract.NavigateURL = "PackageBundleBkg.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&MappingID=" + mappingID + "&NodeID=" + uniqueKey + "&ParentNodeID=" + parentNodeID + "&TreeNodeType=" + RuleSetTreeNodeType.PackageBundle + "&TreeNodeValue=" + treeNodeValue;
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
            List<INTSOF.UI.Contract.BkgSetup.InstituteHierarchyBkgTreeDataContract> lstDeptTree = View.lstTreeData;
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
                    if (DeptTree.UICode == RuleSetTreeNodeType.CompliancePackage)
                    {
                        packageID = DeptTree.EntityID;
                    }
                    nodeID = DeptTree.ParentNodeID;
                }
            }
            nodeUniqueKeyPackageID.Add(packageID, uniqueKey);
            return nodeUniqueKeyPackageID;
        }
        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
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

        #endregion
    }
}
