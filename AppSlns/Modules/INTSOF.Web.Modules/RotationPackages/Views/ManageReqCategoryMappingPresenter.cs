using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceProxy.Modules.RequirementPackage;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public class ManageReqCategoryMappingPresenter : Presenter<IManageReqCategoryMappingView>
    {
        private RequirementPackageProxy _requirementPackageProxy
        {
            get
            {
                return new RequirementPackageProxy();
            }
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetTreeData()
        {
            ServiceRequest<Int32> serviceRequest = new ServiceRequest<Int32>();
            serviceRequest.Parameter = View.CurrentCategoryID;
            var _serviceResponse = _requirementPackageProxy.GetRotationMappingTreeData(serviceRequest);
            if (_serviceResponse.Result.IsNotNull())
            {
                View.lstRotationMappingTreeData = _serviceResponse.Result;
            }
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
            Int32 CurrentCategoryID = View.CurrentCategoryID;
            //Sets Navigation URL, Value, ColorCode, FontBold and IsExpand properties for a node.
            switch (uiCode)
            {
                //Category 
                case ComplainceObjectType.Category:
                    complianceTreeUIContract.NavigateURL = "EditMasterRotationCategory.aspx?IsNewPackage=True&RequirementCategoryID=" + CurrentCategoryID + "&IsDetailsEditable=" + View.IsDetailsEditable;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Item  
                case ComplainceObjectType.Item:
                    complianceTreeUIContract.NavigateURL = "SetupRequirementItem.aspx?RequirementItemID=" + dataID + "&IsNewPackage=True&RequirementCategoryID=" + CurrentCategoryID + "&IsDetailsEditable=" + View.IsDetailsEditable; 
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;


                //Attribute
                case "FLD":
                    String seriesId = ParenNodeID.Substring(ParenNodeID.IndexOf("_") + 1);
                    complianceTreeUIContract.NavigateURL = "SetupRequirementField.aspx?RequirementFieldID=" + dataID + "&IsNewPackage=True&RequirementCategoryID=" + CurrentCategoryID + "&RequirementItemID=" + parentDataID + "&IsDetailsEditable=" + View.IsDetailsEditable; 
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Category Label
                case "LAB_CAT":
                    //complianceTreeUIContract.NavigateURL = "ManageUniversalCategory.aspx?IsAddMode=true";
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Item Label
                case "LAB_ITM":
                    complianceTreeUIContract.NavigateURL = "SetupRequirementItem.aspx?IsNewPackage=True&RequirementItemID=" + AppConsts.ZERO + "&IsAddMode=true&RequirementCategoryID=" + CurrentCategoryID + "&IsDetailsEditable=" + View.IsDetailsEditable;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Attribute Label
                case "LAB_FLD":
                    complianceTreeUIContract.NavigateURL = "SetupRequirementField.aspx?IsNewPackage=True&RequirementFieldID=" + AppConsts.ZERO + "&RequirementItemID=" + dataID + "&RequirementCategoryID=" + CurrentCategoryID + "&IsDetailsEditable=" + View.IsDetailsEditable;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Rules
                case "LRULE":
                    complianceTreeUIContract.NavigateURL = "RequirementRuleSetUp.aspx?IsNewPackage=True&IsEditMode=False&ObjectTreeID=" + dataID + "&RequirementCategoryID=" + CurrentCategoryID + "&IsDetailsEditable=" + View.IsDetailsEditable;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;
                
                //Rules
                case "RULE":
                    complianceTreeUIContract.NavigateURL = "RequirementRuleSetUp.aspx?IsNewPackage=True&IsEditMode=True&ObjectRuleID=" + dataID + "&ObjectTreeID=" + parentDataID + "&RequirementCategoryID=" + CurrentCategoryID + "&IsDetailsEditable=" + View.IsDetailsEditable;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;
            }
            return complianceTreeUIContract;
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

        public void updateDisplayOrder(Int32 parentId, Int32 childId, Int32 displayOrder, Int32 level, Boolean isNewPackage)
        {
            ServiceRequest<Int32, Int32, Int32, Int32> serviceRequest = new ServiceRequest<Int32, Int32, Int32, Int32>();
            serviceRequest.Parameter1 = childId;
            serviceRequest.Parameter2 = parentId;
            serviceRequest.Parameter3 = displayOrder;
            serviceRequest.Parameter4 = View.CurrentUserId;
            ServiceResponse<Boolean> _serviceResponse = null;
            switch (level)
            {
                case AppConsts.THREE:
                    _serviceResponse = _requirementPackageProxy.updateRequirementItemDisplayOrder(serviceRequest,isNewPackage);
                    break;
                case AppConsts.FIVE:
                    _serviceResponse = _requirementPackageProxy.updateRequirementFieldDisplayOrder(serviceRequest, isNewPackage);
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
