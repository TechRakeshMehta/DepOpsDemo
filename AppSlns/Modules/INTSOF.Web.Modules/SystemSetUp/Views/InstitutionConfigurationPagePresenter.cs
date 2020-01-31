using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace CoreWeb.SystemSetUp.Views
{
    public class InstitutionConfigurationPagePresenter : Presenter<IInstitutionConfigurationPageView>
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
                ObjectResult<GetDepartmentTree> objInstituteHierarchyTree = ComplianceSetupManager.GetInstituteHierarchyTreeForConfiguration(View.SelectedTenant, IsAdminLoggedIn() ? (int?)null : View.CurrentUserId);
                View.lstTreeData = objInstituteHierarchyTree.OrderBy(x => x.DPM_DisplayOrder).ThenBy(x => x.TreeNodeTypeID).ToList();
            }
            else
            {
                View.lstTreeData = new List<GetDepartmentTree>();
            }

        }

        /// <summary>
        /// Sets the navigation URL, Color Code and Font Code as per node type.
        /// </summary>
        /// <param name="uiCode">UI Code of a node.</param>
        /// <param name="dataID">DataID of a node.</param>
        /// <param name="parentDataID">Parent Node DataID of a node.</param>
        /// <returns>ComplianceTreeUIContract</returns>
        public ComplianceTreeUIContract GetTreeNodeDetails(String uiCode, Int32 dataID, Int32? parentDataID, String parentNodeID, String nodeID, Int32 mappingID, String treeNodeValue, Int32 entityId, Boolean? IsAvailableforOrder)
        {
            ComplianceTreeUIContract complianceTreeUIContract = new ComplianceTreeUIContract();

            String nodeUniqueKey = GetUniqueKeyIDForNode(nodeID, dataID, uiCode);
            //Sets Navigation URL, Value, ColorCode, FontBold and IsExpand properties for a node.
            complianceTreeUIContract.Value = nodeUniqueKey;
            complianceTreeUIContract.FontBold = true;
            complianceTreeUIContract.IsExpand = false;
            complianceTreeUIContract.NavigateURL = @"InstitutionConfigurationDetails.aspx?Id=" + dataID + "&ParentID=" + parentDataID + "&NodeID=" + nodeUniqueKey + "&NodeName=" + treeNodeValue;
            complianceTreeUIContract.ColorCode = (IsAvailableforOrder.HasValue && IsAvailableforOrder.Value == false) ? "RED" : "STEELBLUE";
            return complianceTreeUIContract;
        }

        /// <summary>
        /// To get Unique Key Package ID for Node
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="dataID"></param>
        /// <param name="uiCode"></param>
        /// <returns></returns>
        private String GetUniqueKeyIDForNode(String nodeID, Int32 dataID, String uiCode)
        {
            List<GetDepartmentTree> lstDeptTree = View.lstTreeData;
            String uniqueKey = "Start_" + uiCode;
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
                    nodeID = DeptTree.ParentNodeID;
                }
            }
            return uniqueKey;
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

