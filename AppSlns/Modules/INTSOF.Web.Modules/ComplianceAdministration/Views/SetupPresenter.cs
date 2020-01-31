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
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public class SetupPresenter : Presenter<ISetupView>
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
            //UAT-1116: Package selection combo box on package screens
            //ObjectResult<GetRuleSetTree> objRuleSetTree = ComplianceSetupManager.GetRuleSetTree(View.SelectedTenantId);
            // View.lstTreeData = objRuleSetTree.OrderBy(x => x.Value).ThenBy(x => x.TreeNodeTypeID).ToList();

            ObjectResult<GetRuleSetTree> objRuleSetTree = ComplianceSetupManager.GetRuleSetTree(View.SelectedTenantId, View.SelectedPackageIDList);
            View.lstTreeData = objRuleSetTree.ToList();
        }

        public Dictionary<String, Int32> GetUniqueKeyPackageIDForNode(String nodeID, Int32 dataID, String uiCode)
        {
            List<GetRuleSetTree> lstRuleSetTree = View.lstTreeData;
            Dictionary<String, Int32> nodeUniqueKeyPackageID = new Dictionary<String, Int32>();
            String uniqueKey = "Start_" + uiCode;
            Int32 packageID = 0;
            Int32 categoryID = 0;
            Int32 itemID = 0;
            Int32 attributeID = 0;
            while (true)
            {
                var ruleSetTree = lstRuleSetTree.FirstOrDefault(x => x.NodeID == nodeID);
                if (ruleSetTree.IsNotNull())
                {
                    uniqueKey += "_" + ruleSetTree.DataID.ToString();
                    if (ruleSetTree.ParentNodeID.IsNull())
                    {
                        break;
                    }
                    if (ruleSetTree.UICode == RuleSetTreeNodeType.Package)
                    {
                        packageID = ruleSetTree.DataID;
                    }
                    else if (ruleSetTree.UICode == RuleSetTreeNodeType.Category)
                    {
                        categoryID = ruleSetTree.DataID;
                    }
                    else if (ruleSetTree.UICode == RuleSetTreeNodeType.Item)
                    {
                        itemID = ruleSetTree.DataID;
                    }
                    else if (ruleSetTree.UICode == RuleSetTreeNodeType.Attribute)
                    {
                        attributeID = ruleSetTree.DataID;
                    }
                    nodeID = ruleSetTree.ParentNodeID;
                }
            }
            nodeUniqueKeyPackageID.Add(uniqueKey, packageID);
            nodeUniqueKeyPackageID.Add("ParentCategoryId", categoryID);
            nodeUniqueKeyPackageID.Add("ParentItemId", itemID);
            nodeUniqueKeyPackageID.Add("ParentAttributeId", attributeID);
            return nodeUniqueKeyPackageID;
        }

        /// <summary>
        /// Sets the navigation URL, Color Code and Font Code as per node type.
        /// </summary>
        /// <param name="uiCode">UI Code of a node.</param>
        /// <param name="dataID">DataID of a node.</param>
        /// <param name="parentDataID">Parent Node DataID of a node.</param>
        /// <returns>ComplianceTreeUIContract</returns>
        public ComplianceTreeUIContract GetTreeNodeDetails(String uiCode, Int32 dataID, Int32? parentDataID, String ParenNodeID, String nodeID)
        {
            ComplianceTreeUIContract complianceTreeUIContract = new ComplianceTreeUIContract();

            Dictionary<String, Int32> nodeUniqueKeyPackageID = GetUniqueKeyPackageIDForNode(nodeID, dataID, uiCode);
            String uniqueKey = nodeUniqueKeyPackageID.FirstOrDefault().Key;
            Int32 packageDataID = nodeUniqueKeyPackageID.FirstOrDefault().Value;
            Int32 parentNodeTypeID = GetTreeNodeTypeID(ParenNodeID);
            Int32 parentCategoryID = nodeUniqueKeyPackageID.FirstOrDefault(x => x.Key == "ParentCategoryId").Value;
            Int32 parentItemID = nodeUniqueKeyPackageID.FirstOrDefault(x => x.Key == "ParentItemId").Value;
            Int32 parentAttributeID = nodeUniqueKeyPackageID.FirstOrDefault(x => x.Key == "ParentAttributeId").Value;
            //Sets Navigation URL, Value, ColorCode, FontBold and IsExpand properties for a node.
            switch (uiCode)
            {
                //Package Label 
                case RuleSetTreeNodeType.PackageLabel:
                    complianceTreeUIContract.NavigateURL = "PackageList.aspx?NodeID=" + uniqueKey + "&SelectedTenantId=" + View.SelectedTenantId;
                    complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = true;
                    return complianceTreeUIContract;

                //Category Label 
                case RuleSetTreeNodeType.CategoryLabel:
                    complianceTreeUIContract.NavigateURL = "CategoryList.aspx?Id=" + parentDataID + "&NodeID=" + uniqueKey +
                        "&SelectedTenantId=" + View.SelectedTenantId;
                    complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Item Label 
                case RuleSetTreeNodeType.ItemLabel:
                    complianceTreeUIContract.NavigateURL = "ItemList.aspx?Id=" + parentDataID + "&NodeID=" + uniqueKey +
                        "&SelectedTenantId=" + View.SelectedTenantId;
                    complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Attribute Label 
                case RuleSetTreeNodeType.AttributeLabel:
                    complianceTreeUIContract.NavigateURL = "AttributeList.aspx?Id=" + parentDataID + "&NodeID=" + uniqueKey +
                        "&SelectedTenantId=" + View.SelectedTenantId + "&PackageId=" + packageDataID + "&CategoryID=" + parentCategoryID;
                    complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Rule Set Label 
                case RuleSetTreeNodeType.RuleSetLabel:
                    complianceTreeUIContract.NavigateURL = "RulesetList.aspx?Id=" + parentDataID + "&ObjectTypeId=" + parentNodeTypeID + "&NodeID=" +
                        uniqueKey + "&parentPackageID=" + packageDataID + "&parentCategoryID=" + parentCategoryID + "&parentItemID=" + parentItemID
                        + "&SelectedTenantId=" + View.SelectedTenantId; ;
                    complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "Green";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;


                //Rule Set Label 
                case RuleSetTreeNodeType.RuleLabel:
                    complianceTreeUIContract.NavigateURL = "RuleList.aspx?Id=" + parentDataID + "&PackageId=" + packageDataID + "&ObjectTypeId=" + parentNodeTypeID +
                        "&NodeID=" + uniqueKey + "&SelectedTenantId=" + View.SelectedTenantId + "&CurrentCategoryID=" + parentCategoryID + "&CurrentItemID=" + parentItemID + "&CurrentAttributeID=" + parentAttributeID; ;
                    complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "Green";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Package
                case RuleSetTreeNodeType.Package:
                    complianceTreeUIContract.NavigateURL = "PackageInfo.aspx?Id=" + dataID + "&NodeID=" + uniqueKey +
                        "&SelectedTenantId=" + View.SelectedTenantId + "&CompliancePackageID=" + dataID;
                    complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "DarkBlue";
                    complianceTreeUIContract.FontBold = true;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Category 
                case RuleSetTreeNodeType.Category:
                    complianceTreeUIContract.NavigateURL = "CategoryInfo.aspx?Id=" + dataID + "&NodeID=" + uniqueKey +
                        "&SelectedTenantId=" + View.SelectedTenantId + "&PackageId=" + packageDataID;
                    complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "DarkBlue";
                    complianceTreeUIContract.FontBold = true;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Item 
                case RuleSetTreeNodeType.Item:
                    complianceTreeUIContract.NavigateURL = "ItemInfo.aspx?Id=" + dataID + "&NodeID=" + uniqueKey +
                        "&SelectedTenantId=" + View.SelectedTenantId + "&PackageId=" + packageDataID + "&CategoryID=" + parentCategoryID;
                    complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "DarkBlue";
                    complianceTreeUIContract.FontBold = true;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Attribute 
                case RuleSetTreeNodeType.Attribute:
                    complianceTreeUIContract.NavigateURL = "AttributeInfo.aspx?Id=" + dataID + "&NodeID=" + uniqueKey +
                        "&SelectedTenantId=" + View.SelectedTenantId + "&PackageId=" + packageDataID + "&CategoryID=" + parentCategoryID + "&ParentDataId=" + parentDataID;
                    complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "DarkBlue";
                    complianceTreeUIContract.FontBold = true;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Rule Set 
                case RuleSetTreeNodeType.RuleSet:
                    complianceTreeUIContract.NavigateURL = "RulesetInfo.aspx?Id=" + dataID + "&NodeID=" + uniqueKey +
                        "&SelectedTenantId=" + View.SelectedTenantId;
                    complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "DarkBlue";
                    complianceTreeUIContract.FontBold = true;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Rule 
                case RuleSetTreeNodeType.Rule:
                    //complianceTreeUIContract.NavigateURL = "RuleInfo.aspx?Id=" + dataID + "&ParentDataID=" + parentDataID + "&PackageId=" + packageDataID + "&ObjectTypeId=" + 
                    //    parentNodeTypeID + "&NodeID=" + uniqueKey;
                    complianceTreeUIContract.NavigateURL = "RuleInfo.aspx?Id=" + dataID + "&PackageId=" + packageDataID + "&NodeID=" + uniqueKey
                        + "&SelectedTenantId=" + View.SelectedTenantId + "&CurrentCategoryID=" + parentCategoryID;
                    complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "DarkBlue";
                    complianceTreeUIContract.FontBold = true;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;
            }
            return complianceTreeUIContract;
        }

        /// <summary>
        /// Method called when View is initialized.
        /// </summary>
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
            // TODO: Implement code that will be executed the first time the view loads
        }

        private Int32 GetTreeNodeTypeID(String ParenNodeID)
        {
            if (ParenNodeID.IsNotNull())
            {
                return View.lstTreeData.FirstOrDefault(obj => obj.NodeID == ParenNodeID).TreeNodeTypeID;
            }
            else
            {
                return 0;
            }
        }

        public void GetTenants()
        {
            View.ListTenants = ComplianceDataManager.GetMasterAndInstitutionTypeTenants(View.DefaultTenantId);
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

        public List<CompliancePackageCategory> getCompliancePackageCategoryByDisplayOrder(Int32 packageId)
        {
            return ComplianceDataManager.GetCompliancePackageCategoryByDisplayOrder(packageId, View.SelectedTenantId);
        }

        public List<ComplianceCategoryItem> getComplianceCategoryItemByDisplayOrder(Int32 categoryId)
        {
            return ComplianceDataManager.getComplianceCategoryItemByDisplayOrder(categoryId, View.SelectedTenantId);
        }

        public List<ComplianceItemAttribute> getComplianceItemAttributeByDisplayOrder(Int32 itemId)
        {
            return ComplianceDataManager.getComplianceItemAttributeByDisplayOrder(itemId, View.SelectedTenantId);
        }

        public void updateDisplayOrder(Int32 parentId, Int32 childId, Int32 displayOrder, Int32 level)
        {
            switch (level)
            {
                case AppConsts.THREE:
                    ComplianceDataManager.UpdateCategoryDisplayOrder(parentId, childId, displayOrder, View.CurrentUserId, View.SelectedTenantId);
                    break;

                case AppConsts.FIVE:
                    ComplianceDataManager.UpdateItemDisplayOrder(parentId, childId, displayOrder, View.CurrentUserId, View.SelectedTenantId);
                    break;

                case AppConsts.SEVEN:
                    ComplianceDataManager.UpdateAttributeDisplayOrder(parentId, childId, displayOrder, View.CurrentUserId, View.SelectedTenantId);
                    break;

                default:
                    break;
            }

        }

        /// <summary>
        /// To get compliance packages List
        /// </summary>
        public void GetCompliancePackages()
        {
            List<CompliancePackage> tempListPackages = new List<CompliancePackage>();
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                //tempListPackages = ComplianceSetupManager.GetCompliancePackage(View.SelectedTenantId, false).Select(pak => new LookupContract()
                //{
                //    Name = pak.PackageName,
                //    ID = pak.CompliancePackageID
                //}).OrderBy(x => x.Name).ToList();
                tempListPackages = ComplianceSetupManager.GetCompliancePackage(View.SelectedTenantId, false);
            }
            View.ListCompliancePackages = tempListPackages.OrderBy(x=>x.PackageName).ToList();
        }
        #endregion

        #endregion
    }
}




