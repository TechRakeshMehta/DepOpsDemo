using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Entity;

namespace CoreWeb.CommonOperations.Views
{
    public class SavedReportsPagePresenter : Presenter<ISavedReportsPageView>
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
        }

        public void GetTreeData()
        {

            View.LstReportFavouriteParameter = ReportManager.GetReportFavouriteParametersByUserID(View.CurrentUserId).OrderBy(cond => cond.RFP_Name).ToList();
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

        public Boolean DeleteFavParamReportParamMapping(string RPF_ids)
        {
            return ReportManager.DeleteFavParamReportParamMapping(RPF_ids, View.CurrentUserId);
           
        }

        #endregion
    }
}

