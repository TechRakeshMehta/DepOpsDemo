using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class SetupUniversalMappingPresenter : Presenter<ISetupUniversalMappingView>
    {
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
            View.lstTreeData = UniversalMappingDataManager.GetUniversalMappingTreeData();
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

            //Sets Navigation URL, Value, ColorCode, FontBold and IsExpand properties for a node.
            switch (uiCode)
            {
                //Category 
                case ComplainceObjectType.Category:
                    complianceTreeUIContract.NavigateURL = "ManageUniversalCategory.aspx?IsAddMode=false&UniversalCategoryID=" + dataID;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Item  
                case ComplainceObjectType.Item:
                    complianceTreeUIContract.NavigateURL = "ManageUniversalItem.aspx?IsAddMode=false&UniversalCategoryID=" + parentDataID + "&UniversalItemID=" + dataID;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;


                //Attribute
                case ComplainceObjectType.Attribute:
                    String seriesId = ParenNodeID.Substring(ParenNodeID.IndexOf("_") + 1);
                    complianceTreeUIContract.NavigateURL = "ManageUniversalAttribute.aspx?IsAddMode=false&UniversalAttributeID=" + dataID + "&UniversalItemID=" + parentDataID;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Category Label
                case "LAB_CAT":
                    complianceTreeUIContract.NavigateURL = "ManageUniversalCategory.aspx?IsAddMode=true";
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Item Label
                case "LAB_ITM":
                    complianceTreeUIContract.NavigateURL = "ManageUniversalItem.aspx?IsAddMode=true&UniversalCategoryID=" + dataID;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Attribute Label
                case "LAB_ATR":
                    complianceTreeUIContract.NavigateURL = "ManageUniversalAttribute.aspx?IsAddMode=true&UniversalItemID=" + dataID;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;
            }
            return complianceTreeUIContract;
        }
    }
}
