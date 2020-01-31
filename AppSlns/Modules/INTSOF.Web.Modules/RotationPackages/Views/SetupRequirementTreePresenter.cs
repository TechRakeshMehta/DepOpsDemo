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
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

#endregion

#endregion

namespace CoreWeb.RotationPackages.Views
{
    public class SetupRequirementTreePresenter : Presenter<ISetupRequirementTreeView>
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

        /// <summary>
        /// Method called when View is initialized.
        /// </summary>
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
            // TODO: Implement code that will be executed the first time the view loads
        }

        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        public void GetTreeData()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.SelectedPackageID;
            ServiceResponse<List<RequirementTreeContract>> _serviceResponse = _requirementPackageProxy.GetRequirementTree(serviceRequest);
            View.lstTreeDataContract = _serviceResponse.Result;
            // View.lstTreeData = SharedRequirementPackageManager.GetRequirementTree(View.SelectedPackageID);


            // List<RequirementTreeContract> lstRequirementTree = new List<RequirementTreeContract>();

            // foreach (var objectTree in View.lstTreeData)
            // {
            //     RequirementTreeContract requirementObjectTree = new RequirementTreeContract();
            //     requirementObjectTree.TreeNodeTypeID = objectTree.TreeNodeTypeID;
            //     requirementObjectTree.DataID = objectTree.DataID;
            //     requirementObjectTree.ParentDataID = objectTree.ParentDataID.HasValue ? objectTree.ParentDataID.Value : (Int32?)null;
            //     requirementObjectTree.HID = objectTree.HID;
            //     requirementObjectTree.NodeID = objectTree.NodeID;
            //     requirementObjectTree.ParentNodeID = objectTree.ParentNodeID.IsNull() ? null : objectTree.ParentNodeID;
            //     requirementObjectTree.Value = objectTree.Value;
            //     requirementObjectTree.UICode = objectTree.UICode;
            //     lstRequirementTree.Add(requirementObjectTree);
            // }

            //View.lstTreeDataContract = lstRequirementTree;

        }

        /// <summary>
        /// Sets the navigation URL, Color Code and Font Code as per node type.
        /// </summary>
        /// <param name="uiCode">UI Code of a node.</param>
        /// <param name="dataID">DataID of a node.</param>
        /// <param name="parentDataID">Parent Node DataID of a node.</param>
        /// <returns>ComplianceTreeUIContract</returns>
        public ComplianceTreeUIContract GetTreeNodeDetails(String uiCode, Int32 dataID, Int32? parentDataID, String ParenNodeID, String nodeID, String hid)
        {
            ComplianceTreeUIContract complianceTreeUIContract = new ComplianceTreeUIContract();
            Boolean isNewPackage = View.SelectedPackageID == AppConsts.NONE;
            //Sets Navigation URL, Value, ColorCode, FontBold and IsExpand properties for a node.
            switch (uiCode)
            {
                //Package Label 
                case RequirementTreeNodeType.PACKAGE:
                    complianceTreeUIContract.NavigateURL = "SetupRequirementPackage.aspx?Id=" + dataID + "&IsPackageUsed=" + View.IsPackageUsed + "&IsNewPackage=" + isNewPackage + "&IsViewOnly=" + View.IsViewOnly;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                case RequirementTreeNodeType.PACKAGELABEL:
                    complianceTreeUIContract.NavigateURL = "SetupRequirementPackage.aspx?Id=" + AppConsts.NONE + "&IsPackageUsed=" + View.IsPackageUsed + "&IsNewPackage=" + isNewPackage + "&IsViewOnly=" + View.IsViewOnly;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Category Label 
                case RequirementTreeNodeType.CATEGORYLABEL:
                    complianceTreeUIContract.NavigateURL = "SetupRequirementCategory.aspx?RequirementPackageID=" + dataID + "&RequirementCategoryID=" + AppConsts.NONE + "&IsPackageUsed=" + View.IsPackageUsed + "&IsViewOnly=" + View.IsViewOnly;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Category Label 
                case RequirementTreeNodeType.CATEGORY:
                    complianceTreeUIContract.NavigateURL = "SetupRequirementCategory.aspx?RequirementCategoryID=" + dataID + "&IsPackageUsed=" + View.IsPackageUsed + "&IsViewOnly=" + View.IsViewOnly + "&RequirementPackageID=" + parentDataID;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Item Label 
                case RequirementTreeNodeType.ITEMLABEL:
                    complianceTreeUIContract.NavigateURL = "SetupRequirementItem.aspx?RequirementItemID=" + AppConsts.ZERO + "&IsPackageUsed=" + View.IsPackageUsed + "&SelectedPackageID=" + View.SelectedPackageID + "&IsViewOnly=" + View.IsViewOnly + "&RequirementCategoryID=" + dataID;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;
                //Item  
                case RequirementTreeNodeType.ITEM:
                    complianceTreeUIContract.NavigateURL = "SetupRequirementItem.aspx?RequirementItemID=" + dataID + "&IsPackageUsed=" + View.IsPackageUsed + "&SelectedPackageID=" + View.SelectedPackageID + "&IsViewOnly=" + View.IsViewOnly + "&ItemHId=" + hid + "&RequirementCategoryID=" + parentDataID;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Field Label 
                case RequirementTreeNodeType.FIELDLABEL:
                    complianceTreeUIContract.NavigateURL = "SetupRequirementField.aspx?RequirementFieldID=" + AppConsts.ZERO + "&IsPackageUsed=" + View.IsPackageUsed + "&SelectedPackageID=" + View.SelectedPackageID + "&IsViewOnly=" + View.IsViewOnly + "&ItemHId=" + hid + "&RequirementItemID=" + dataID;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;
                //Field  
                case RequirementTreeNodeType.FIELD:
                    complianceTreeUIContract.NavigateURL = "SetupRequirementField.aspx?RequirementFieldID=" + dataID + "&IsPackageUsed=" + View.IsPackageUsed + "&SelectedPackageID=" + View.SelectedPackageID + "&IsViewOnly=" + View.IsViewOnly + "&RequirementItemID=" + parentDataID;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Rules
                case RequirementTreeNodeType.RULELABEL:
                    complianceTreeUIContract.NavigateURL = "SetUpRequirementRule.aspx?Id=" + dataID + "&IsPackageUsed=" + View.IsPackageUsed + "&IsViewOnly=" + View.IsViewOnly + "&RequirementCategoryID=" + parentDataID + "&HId=" + hid;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;
            }
            return complianceTreeUIContract;
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


        #region UAT-3078

        public List<RequirementItemContract> GetRequirementItems(Int32 RequirementCategoryID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = RequirementCategoryID;
            var _serviceResponse = _requirementPackageProxy.GetRequirementItemDetailsByCategoryId(serviceRequest);
            if (_serviceResponse.Result.IsNotNull())
            {
                return _serviceResponse.Result;
            }
            return new List<RequirementItemContract>();
        }
        public List<RequirementFieldContract> GetRequirementFields(Int32 RequirementItemID)
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = RequirementItemID;
            var _serviceResponse = _requirementPackageProxy.GetRequirementFieldsByItemID(serviceRequest);
            if (_serviceResponse.Result.IsNotNull())
            {
                return _serviceResponse.Result;
            }
            return new List<RequirementFieldContract>();
        }

        public void updateDisplayOrder(Int32 parentId, Int32 childId, Int32 displayOrder, Int32 level,Boolean isNewPackage)
        {
            ServiceRequest<Int32, Int32, Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32, Int32, Int32>();
            serviceRequest.Parameter1 = childId;
            serviceRequest.Parameter2 = parentId;
            serviceRequest.Parameter3 = displayOrder;
            serviceRequest.Parameter4 = View.CurrentUserId;
            ServiceResponse<Boolean> _serviceResponse = null;
            switch (level)
            {
                case AppConsts.FIVE:
                    _serviceResponse = _requirementPackageProxy.updateRequirementItemDisplayOrder(serviceRequest, isNewPackage);
                    break;
                case AppConsts.SEVEN:
                    _serviceResponse = _requirementPackageProxy.updateRequirementFieldDisplayOrder(serviceRequest, isNewPackage);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #endregion

        #endregion
    }
}




