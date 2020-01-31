using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceManagement;
using Business.RepoManagers;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class SeriesDosePresenter : Presenter<ISeriesDoseView>
    {
        public void GetSeriesDetails()
        {
            Tuple<List<SeriesItemContract>, List<SeriesAttributeContract>> seriesData = ComplianceSetupManager.GetSeriesDetails(View.SeriesId, View.SelectedTenantId);
            View.lstSeriesItemContract = seriesData.Item1;
            View.lstSeriesAttributeContract = seriesData.Item2;
        }

        /// <summary>
        /// Get List of Possible status types after shuffling of items
        /// </summary>
        public void GetPostShuffleStatusList()
        {
            View.lstStatusTypes = LookupManager.GetLookUpData<lkpItemStatusPostDataShuffle>(View.SelectedTenantId);
        }


        /// <summary>
        /// Save-Update the mapping of the Item Attributes with the Series Attributes.
        /// </summary>
        public void SaveUpdateSeriesMapping()
        {
            ComplianceSetupManager.SaveUpdateSeriesMapping(View.lstSeriesItemContractSaveUpdate, View.CurrentUserId, View.SelectedTenantId);
        }

        /// <summary>
        /// Check if Series Mapped Attribute exist
        /// </summary>
        /// <returns></returns>
        public Boolean CheckIfSeriesMappedAttrExist()
        {
            return ComplianceSetupManager.CheckIfSeriesMappedAttrExist(View.SelectedTenantId, View.SeriesId);
        }
    }
}
