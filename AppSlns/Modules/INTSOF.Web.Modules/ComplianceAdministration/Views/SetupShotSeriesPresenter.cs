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
    public class SetupShotSeriesPresenter : Presenter<ISetupShotSeriesView>
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
            if (View.SelectedTenantId > AppConsts.NONE && View.SelectedTenantId != View.DefaultTenantId)
            {
                View.lstTreeData = ComplianceSetupManager.GetShotSeriesTreeData(View.SelectedTenantId);
            }
            else
            {
                View.lstTreeData = new List<GetShotSeriesTree>();
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

            //Sets Navigation URL, Value, ColorCode, FontBold and IsExpand properties for a node.
            switch (uiCode)
            {
                //Package Label 
                case ShotSeriesTreeNodeType.Category:
                    complianceTreeUIContract.NavigateURL = "ManageShotSeries.aspx?Id=" + dataID + "&SelectedTenantId=" + View.SelectedTenantId;
                    //complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Category Label 
                case ShotSeriesTreeNodeType.Series:
                    complianceTreeUIContract.NavigateURL = "ShotSeriesInfo.aspx?Id=" + dataID + "&SelectedTenantId=" + View.SelectedTenantId + "&CatId=" + parentDataID;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Item Label 
                case ShotSeriesTreeNodeType.Item:
                    complianceTreeUIContract.NavigateURL = "ManageSeriesDose.aspx?SeriesId=" + dataID + "&TenantId=" + View.SelectedTenantId + "&CatId=" + parentDataID;
                    //complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Rule Label 
                case ShotSeriesTreeNodeType.RuleLabel:
                    complianceTreeUIContract.NavigateURL = "ShotSeriesRuleList.aspx?Id=" + dataID + "&SelectedTenantId=" + View.SelectedTenantId + "&CategoryId=" + parentDataID;
                    //complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
                    complianceTreeUIContract.IsExpand = false;
                    return complianceTreeUIContract;

                //Rules
                case ShotSeriesTreeNodeType.Rule:
                    String seriesId = ParenNodeID.Substring(ParenNodeID.IndexOf("_") + 1);
                    complianceTreeUIContract.NavigateURL = "ShotSeriesRuleInfo.aspx?Id=" + dataID + "&SelectedTenantId=" + View.SelectedTenantId + "&SeriesId=" + seriesId + "&CategoryId=" + parentDataID;
                    //complianceTreeUIContract.Value = uniqueKey;
                    complianceTreeUIContract.ColorCode = "Red";
                    complianceTreeUIContract.FontBold = false;
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


        public void GetTenants()
        {
            View.ListTenants = ComplianceDataManager.GetTenantList(View.DefaultTenantId);
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

        #endregion

        #endregion
    }
}




