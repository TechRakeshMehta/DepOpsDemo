using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class SeriesUnMappedAttributesPresenter : Presenter<ISeriesUnMappedAttributesView>
    {
        /// <summary>
        /// Get Series Un-Mapped Attributes
        /// </summary>
        public void GetUnMappedAttributes()
        {
            View.UnMappedAttributesList = ComplianceSetupManager.GetUnMappedAttributes(View.TenantId, View.ItemSeriesId);
        }

        /// <summary>
        /// Save Series Un-Mapped Attributes
        /// </summary>
        /// <param name="lstSeriesAttributeContract"></param>
        /// <returns></returns>
        public Boolean SaveUnMappedAttributes(List<SeriesAttributeContract> lstSeriesAttributeContract)
        {
            return ComplianceSetupManager.SaveUnMappedAttributes(View.TenantId, View.CurrentLoggedInUserId, lstSeriesAttributeContract);
        }
    }
}
