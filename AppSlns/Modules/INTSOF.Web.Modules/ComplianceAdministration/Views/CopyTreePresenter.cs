using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using System.Data.Entity.Core.Objects;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System.Linq;
using Business.RepoManagers;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class CopyTreePresenter : Presenter<ICopyTreeView>
    {
        #region Public Methods

        /// <summary>
        /// Sets the TenantID of Current LoggedIn User in property TenantId.
        /// </summary>
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            View.CurrentViewContext.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Called when viwe is initialized.
        /// </summary>
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Gets the Current LoggedIn User TenantID.
        /// </summary>
        /// <returns>Tenant ID</returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Gets the assigned tree data from database and sets it in property AssignedTreeData.
        /// </summary>
        public void GetTreeData()
        {
            List<GetRuleSetTree> lstRuleSetTree = ComplianceSetupManager.GetComplianceTree(View.CurrentViewContext.TenantId).ToList();
            List<ComplianceAssociations> lstComplianceAssociations = ComplianceSetupManager.GetComplianceAssociations(View.ManageTenantId).ToList();

            //Finds which nodes are associated and set associated property true for them.
            foreach (GetRuleSetTree ruleSetTree in lstRuleSetTree)
            {
                //Checks which node is present in lstComplianceAssociations.
                if (lstComplianceAssociations.Any(x => x.UICode == ruleSetTree.UICode && x.NodeCode == ruleSetTree.NodeCode && x.ParentNodeCode == ruleSetTree.ParentNodeCode))
                {
                    ruleSetTree.Associated = true;
                }
            }
            View.AssignedTreeData = lstRuleSetTree;
        }

        /// <summary>
        /// Copy the assigned nodes to client or remove unassigned nodes from Client.
        /// </summary>
        public void CopyToClient()
        {
            View.InfoMessage = String.Empty;
            List<IntegrityCheckResponse> lstResponse = new List<IntegrityCheckResponse>();
            List<GetRuleSetTree> lstElementsToAdd = new List<GetRuleSetTree>();
            List<GetRuleSetTree> lstElementsToRemove = new List<GetRuleSetTree>();
            List<GetRuleSetTree> lstPreviousRuleSetTree = View.AssignedTreeData;
            List<GetRuleSetTree> lstCurrentRuleSetTree = lstPreviousRuleSetTree.Clone();
            lstCurrentRuleSetTree = ManageParentChild(lstCurrentRuleSetTree, View.TreeListPackagesUIState);

            //Checks the which node is to be assigned or unassigned to Client.
            foreach (GetRuleSetTree ruleSetTree in lstPreviousRuleSetTree)
            {
                //Checks if differnce in assigned nodes vin database and current list.
                if (lstCurrentRuleSetTree.Any(x => x.NodeID == ruleSetTree.NodeID && x.Associated != ruleSetTree.Associated))
                {
                    //Add unassigned node to Remove Elements list.
                    if (ruleSetTree.Associated == true)
                    {
                        IntegrityCheckResponse response = CheckIfObjectCanBeRemoved(ruleSetTree);
                        if (response.CheckStatus == CheckStatus.False)
                            lstElementsToRemove.Add(ruleSetTree);
                        else
                            lstResponse.Add(response);
                    }
                    //Add assigned node to Add Elements list.
                    else
                    {
                        lstElementsToAdd.Add(ruleSetTree);
                    }
                }
            }

            //Checks if any element is to be assigned or unassigned.
            if (lstElementsToAdd.Count() > 0 || lstElementsToRemove.Count() > 0)
            {
                ComplianceSetupManager.CopyToClient(lstElementsToAdd.OrderBy(x=>x.TreeNodeTypeID).ToList(), lstElementsToRemove, View.CurrentViewContext.CurrentLoggedInUserId, View.CurrentViewContext.ManageTenantId, View.CurrentViewContext.TenantId);
            }
            if (lstElementsToRemove.Count() == 0)
            {
                foreach (IntegrityCheckResponse response in lstResponse)
                    View.InfoMessage += response.UIMessage + "<br />";
            }
        }

        /// <summary>
        /// Manage parent and child as per node is assigned or unassigned.
        /// </summary>
        /// <param name="lstRuleSetTree">List of rules set tree as per data in database.</param>
        /// <param name="lstRuleSetTreeFromUI">List of rule set tree as per data recieved from UI.</param>
        /// <returns>List<GetRuleSetTree></returns>
        // For example: Data is as follows: 
        // NodeID    ParentNodeID    UICode    DataID    ParentDataID
        //   1            NULL        PAK        P1          NULL
        //   2            1           CAT        C1          P1
        //   3            2           ITM        I1          C1
        //   4            3           ATR        A1          I1
        //   5            NULL        PAK        P2          NULL
        //   6            5           CAT        C1          P2
        //   7            6           ITM        I1          C1
        //   8            7           ATR        A1          I1
        //   9            5           CAT        C2          P2
        //   10           9           ITM        I2          C2
        //   11           10          ATR        A2          I2
        // If user selects NodeID(4) then, it selects NodeID(1, 2, 3) that is selects all parents of NodeID(4).
        // If user unselects NodeID(1) then, it unselects NodeID(2, 3, 4) that is unselects all children of NodeID(1).
        public List<GetRuleSetTree> ManageParentChild(List<GetRuleSetTree> lstRuleSetTree, List<GetRuleSetTree> lstRuleSetTreeFromUI)
        {
            var tempList = lstRuleSetTree.Clone();

            //Checks if a node is  assigned or unassigned and accordingly assigned or unassigned the parent and/ or child nodes.
            foreach (GetRuleSetTree ruleSetTree in tempList)
            {
                if (lstRuleSetTreeFromUI.IsNotNull() && lstRuleSetTreeFromUI.Any(x => x.NodeID == ruleSetTree.NodeID))
                {
                    //Set the checbox for a node as true if list from UI has associated property as true.
                    ruleSetTree.Associated = lstRuleSetTreeFromUI.FirstOrDefault(x => x.NodeID == ruleSetTree.NodeID).Associated;

                    //Assigned all parent nodes if child node is checked.
                    if (ruleSetTree.Associated == true && ruleSetTree.ParentNodeID.IsNotNull())
                    {
                        String parentNodeID = ruleSetTree.ParentNodeID;

                        //Assigned parent node of a assigned node till top most parent.
                        while (parentNodeID.IsNotNull())
                        {
                            var ruleSetParentNode = tempList.FirstOrDefault(x => x.NodeID == parentNodeID);
                            tempList.FirstOrDefault(x => x.NodeID == parentNodeID).Associated = true;
                            parentNodeID = ruleSetParentNode.ParentNodeID;
                        }
                    }
                    //Unassigned all child nodes if parent node is unchecked.
                    else if (ruleSetTree.Associated == false)
                    {
                        ManageChildren(lstRuleSetTreeFromUI, tempList, ruleSetTree, false);
                    }
                }
            }
            return tempList;
        }

        public void ManageChildren(List<GetRuleSetTree> lstRuleSetTreeFromUI, List<GetRuleSetTree> tempList, GetRuleSetTree ruleSetTree, Boolean checkChild)
        {
            List<String> childNodeIDs = new List<String>();
            childNodeIDs.Add(ruleSetTree.NodeID);

            //Unassigned child node of an unassigned node till its last child node.
            while (childNodeIDs.Count > 0)
            {
                var lstRuleSetChildNodes = tempList.Where(x => childNodeIDs.Contains(x.ParentNodeID)).ToList();

                //Checks if child node is found or not.
                if (lstRuleSetChildNodes.Count > 0)
                {
                    tempList.Where(x => childNodeIDs.Contains(x.ParentNodeID)).ForEach(y => y.Associated = checkChild);
                    childNodeIDs.Clear();
                    childNodeIDs = lstRuleSetChildNodes.Select(x => x.NodeID).ToList();
                    lstRuleSetTreeFromUI.Where(x => childNodeIDs.Contains(x.NodeID)).ForEach(y => y.Associated = checkChild);
                }
                else
                {
                    childNodeIDs.Clear();
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks if the object is associated with other objects befor adding it to removal list.
        /// </summary>
        /// <param name="ruleSetTree">RuleSetTree Object</param>
        /// <returns>Integrity Check Response</returns>
        private IntegrityCheckResponse CheckIfObjectCanBeRemoved(GetRuleSetTree ruleSetTree)
        {
            IntegrityCheckResponse response;
            response.CheckStatus = CheckStatus.False;
            response.UIMessage = String.Empty;
            switch (ruleSetTree.UICode)
            {
                case RuleSetTreeNodeType.Package:
                    CompliancePackage compliancePackage = ComplianceSetupManager.GetPackageDetailsByCode(View.CurrentViewContext.ManageTenantId, ruleSetTree.NodeCode);
                    if (compliancePackage.IsNotNull())
                    {
                        response = IntegrityManager.IfPackageCanBeUnassigned(compliancePackage.CompliancePackageID, compliancePackage.PackageName, View.CurrentViewContext.ManageTenantId);
                    }
                    break;

                case RuleSetTreeNodeType.Category:
                    ComplianceCategory complianceCategory = ComplianceSetupManager.GetCategoryDetailsByCode(View.CurrentViewContext.ManageTenantId, ruleSetTree.NodeCode);
                    CompliancePackage parentPackage = ComplianceSetupManager.GetPackageDetailsByCode(View.CurrentViewContext.ManageTenantId, ruleSetTree.ParentNodeCode);
                    if (complianceCategory.IsNotNull() && parentPackage.IsNotNull())
                    {
                        response = IntegrityManager.IfCategoryCanBeUnassigned(parentPackage.CompliancePackageID, complianceCategory.CategoryName, View.CurrentViewContext.ManageTenantId);
                    }
                    break;

                case RuleSetTreeNodeType.Item:
                    ComplianceItem complianceItem = ComplianceSetupManager.GetItemDetailsByCode(View.CurrentViewContext.ManageTenantId, ruleSetTree.NodeCode);
                    ComplianceCategory parentCategory = ComplianceSetupManager.GetCategoryDetailsByCode(View.CurrentViewContext.ManageTenantId, ruleSetTree.ParentNodeCode);
                    if (complianceItem.IsNotNull() && parentCategory.IsNotNull())
                    {
                        response = IntegrityManager.IfItemCanBeUnassigned(parentCategory.ComplianceCategoryID, complianceItem.Name, View.CurrentViewContext.ManageTenantId);
                    }
                    break;

                case RuleSetTreeNodeType.Attribute:
                    ComplianceAttribute complianceAttribute = ComplianceSetupManager.GetAttributeDetailsByCode(View.CurrentViewContext.ManageTenantId, ruleSetTree.NodeCode);
                    ComplianceItem parentItem = ComplianceSetupManager.GetItemDetailsByCode(View.CurrentViewContext.ManageTenantId, ruleSetTree.ParentNodeCode);
                    if (complianceAttribute.IsNotNull() && parentItem.IsNotNull())
                    {
                        response = IntegrityManager.IfAttributeCanBeUnassigned(parentItem.ComplianceItemID, complianceAttribute.Name, View.CurrentViewContext.ManageTenantId);
                    }
                    break;

                default: break;
            }
            return response;
        }

        #endregion
    }
}




