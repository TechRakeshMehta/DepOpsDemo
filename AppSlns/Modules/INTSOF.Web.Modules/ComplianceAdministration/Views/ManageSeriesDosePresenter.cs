using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ManageSeriesDosePresenter : Presenter<IManageSeriesDoseView>
    {
        /// <summary>
        /// Get the ComplianceItems of selected Category
        /// </summary>
        public void GetComplianceItems()
        {
            View.lstComplianceItems = ComplianceSetupManager.GetComplianceItemsByCategory(View.SelectedCategoryId, View.TenantId);
        }

        /// <summary>
        /// Get the ComplianceItems of selected Category
        /// </summary>
        public void GetComplianceAttributes()
        {
            View.lstComplianceAttributes = ComplianceSetupManager.GetComplianceAttributesByItemIds(View.lstSelectedComplianceItem, View.TenantId);
        }

        /// <summary>
        /// Saves the Items and Attributes selected in the Series
        /// </summary>
        public void SaveSeriesData()
        {
            ComplianceSetupManager.SaveSeriesData(View.SelectedSeriesId, View.lstSelectedComplianceItem, View.dicAttributes, View.CurrentUserId, View.TenantId);
        }

        public void CheckComplinaceItemForEditMode()
        {
            View.lstSelectedComplianceItem = ComplianceSetupManager.GetItemSeriesItemsBySeriesId(View.SelectedSeriesId, View.TenantId);
            if (!View.lstSelectedComplianceItem.IsNullOrEmpty())
            {
                View.IsEditMode = true;
            }
            else
            {
                View.IsEditMode = false;
            }
        }

        public void ChcekComplianceAttributeForEditMode()
        {
            View.lstSelectedAttributes = ComplianceSetupManager.GetItemSeriesAttributeBySeriesId(View.SelectedSeriesId, View.TenantId);
            View.SelectedKeyAttribute = ComplianceSetupManager.GetItemSeriesKeyAttributeBySeriesId(View.SelectedSeriesId, View.TenantId);
        }
    }
}
