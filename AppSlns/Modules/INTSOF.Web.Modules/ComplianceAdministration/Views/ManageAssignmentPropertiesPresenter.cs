#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Linq;
using System.Data.Entity.Core.Objects;

using System;

#endregion

#region UserDefined

using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;

#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ManageAssignmentPropertiesPresenter : Presenter<IManageAssignmentPropertiesView>
    {
        #region Variables

        #region Private Variables
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties
        #endregion

        #region Public Properties
        #endregion

        #endregion

        #region Methods

        #region Public Methods

        public override void OnViewLoaded()
        {

            View.ListTenants = ComplianceDataManager.getClientTenant();
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetAssignmentPropertiesTreeData()
        {
            ObjectResult<GetRuleSetTree> objRuleSetTree = ComplianceSetupManager.GetComplianceTree(View.SelectedTenant);
            //View.ListAssignmentPropertiesTreeData = objRuleSetTree.Where(x => x.UICode != RuleSetTreeNodeType.Attribute).ToList();
            //Changes for Editable By for ATR
            //View.ListAssignmentPropertiesTreeData = objRuleSetTree.ToList();
            View.LstAssignmentPropertiesTreeData = objRuleSetTree.ToList();
        }

        /// <summary>
        /// Sets the navigation URL, Color Code and Font Code as per node type.
        /// </summary>
        /// <param name="uiCode">UI Code of a node.</param>
        /// <param name="dataID">DataID of a node.</param>
        /// <param name="parentDataID">Parent Node DataID of a node.</param>
        /// <returns>ComplianceTreeUIContract</returns>
        public ComplianceTreeUIContract GetTreeNodeDetails(String uiCode, Int32 dataID, Int32? parentDataID, String parenNodeID)
        {
            ComplianceTreeUIContract complianceTreeUIContract = new ComplianceTreeUIContract();

            //Sets Navigation URL for a node.
            switch (uiCode)
            {
                //Package
                case RuleSetTreeNodeType.Package:
                    complianceTreeUIContract.NavigateURL = "AssignmentPropertiesDetail.aspx?tenantId=" + View.SelectedTenant + "&CurrentDataId=" + dataID +
                        "&RuleSetTreeTypeCode=" + uiCode;
                    break;

                //Category 
                case RuleSetTreeNodeType.Category:
                    complianceTreeUIContract.NavigateURL = "AssignmentPropertiesDetail.aspx?tenantId=" + View.SelectedTenant + "&PackageId=" + parentDataID +
                        "&CurrentDataId=" + dataID + "&RuleSetTreeTypeCode=" + uiCode;
                    break;

                //Item 
                case RuleSetTreeNodeType.Item:
                    Int32? packageDataID = GetNodePackageID(parenNodeID);
                    complianceTreeUIContract.NavigateURL = "AssignmentPropertiesDetail.aspx?tenantId=" + View.SelectedTenant + "&PackageId=" + packageDataID +
                        "&CategoryId=" + parentDataID + "&CurrentDataId=" + dataID + "&RuleSetTreeTypeCode=" + uiCode;
                    break;

                //Attribute 
                case RuleSetTreeNodeType.Attribute:
                    //Changes for Editable By for ATR
                    Int32? packageId = GetAttNodePackageID(parenNodeID);
                    Int32? categoryId = GetNodeCategoryID(parenNodeID);
                    complianceTreeUIContract.NavigateURL = "AssignmentPropertiesDetail.aspx?tenantId=" + View.SelectedTenant + "&PackageId=" + packageId +
                        "&CategoryId=" + categoryId + "&ItemId=" + parentDataID + "&CurrentDataId=" + dataID + "&RuleSetTreeTypeCode=" + uiCode;
                    break;
            }
            return complianceTreeUIContract;
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public Boolean IsAdminLoggedIn()
        {
            View.SelectedTenant = GetTenantId();
            //Checked if logged user is admin or not.
            if (View.SelectedTenant == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //UAT-2717
        public void GetCompliancePackages()
        {
            List<CompliancePackage> tempListPackages = new List<CompliancePackage>();
            if (View.SelectedTenant > AppConsts.NONE)
            {
                //tempListPackages = ComplianceSetupManager.GetCompliancePackage(View.SelectedTenantId, false).Select(pak => new LookupContract()
                //{
                //    Name = pak.PackageName,
                //    ID = pak.CompliancePackageID
                //}).OrderBy(x => x.Name).ToList();
                tempListPackages = ComplianceSetupManager.GetTenantCompliancePackage(View.SelectedTenant);
            }
            View.ListCompliancePackages = tempListPackages.OrderBy(x => x.PackageName).ToList();
        }

        public void GetTreeData()
        {
            //UAT-1116: Package selection combo box on package screens
            //ObjectResult<GetRuleSetTree> objRuleSetTree = ComplianceSetupManager.GetRuleSetTree(View.SelectedTenantId);
            // View.lstTreeData = objRuleSetTree.OrderBy(x => x.Value).ThenBy(x => x.TreeNodeTypeID).ToList();

            ObjectResult<GetRuleSetTree> objRuleSetTree = ComplianceSetupManager.GetRuleSetTree(View.SelectedTenant, View.LstSelectedPackageIDs);
            View.LstAssignmentPropertiesTreeData = objRuleSetTree.ToList();
        }
        #endregion

        #region Private Methods

        private Int32? GetNodePackageID(String parentNodeID)
        {
            List<GetRuleSetTree> lstRuleSetTree = View.LstAssignmentPropertiesTreeData; //View.ListAssignmentPropertiesTreeData;
            var associatedPropertyParentNode = lstRuleSetTree.FirstOrDefault(x => x.NodeID == parentNodeID);
            if (associatedPropertyParentNode.IsNotNull())
            {
                return associatedPropertyParentNode.ParentDataID;
            }
            return 0;
        }

        private Int32? GetAttNodePackageID(String parentNodeID)
        {
            List<GetRuleSetTree> lstRuleSetTree = View.LstAssignmentPropertiesTreeData;//View.ListAssignmentPropertiesTreeData;
            var associatedPropertyParentNode = lstRuleSetTree.FirstOrDefault(x => x.NodeID == parentNodeID);
            if (associatedPropertyParentNode.IsNotNull())
            {
                return GetNodePackageID(associatedPropertyParentNode.ParentNodeID);
            }
            return 0;
        }

        private Int32? GetNodeCategoryID(String parentNodeID)
        {
            List<GetRuleSetTree> lstRuleSetTree = View.LstAssignmentPropertiesTreeData;//View.ListAssignmentPropertiesTreeData;
            var associatedPropertyParentNode = lstRuleSetTree.FirstOrDefault(x => x.NodeID == parentNodeID);
            if (associatedPropertyParentNode.IsNotNull())
            {
                return associatedPropertyParentNode.ParentDataID;
            }
            return 0;
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        private Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentUserId).Organization.TenantID.Value;
        }

        #endregion

        #endregion
    }
}




