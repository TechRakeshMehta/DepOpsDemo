using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.SharedObjects;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class SeriesDoseRowControlPresenter : Presenter<ISeriesDoseRowControlView>
    {
        /// <summary>
        /// Add New ItemSeriesItem, on table mapping new click
        /// </summary>
        public void SaveItemSeriesItem()
        {
            ComplianceSetupManager.SaveItemSeriesItem(View.SeriesId, Convert.ToInt32(View.CIId), View.CurrentUserId, View.TenantId);
        }

        /// <summary>
        /// Remove Item from ItemSeriesItem, on table mapping Remove click
        /// </summary>
        public void RemoveItemSeriesItem()
        {
            ComplianceSetupManager.RemoveItemSeriesItem(Convert.ToInt32(View.ItemSeriesItemId), View.CurrentUserId, View.TenantId);
        }
    }
}
