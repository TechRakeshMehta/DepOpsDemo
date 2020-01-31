using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceOperations.Views
{
    public class ManageTrackingAutoAssignmentConfigurationPresenter : Presenter<IManageTrackingAutoAssignmentConfigurationView>
    {

        public void GetAdminTrackingAssignmentConfiguration()
        {
            View.lstAdminsConfiguration = new List<TrackingAssignmentConfigurationContract>();
            View.lstAdminsConfiguration = SecurityManager.GetAdminTrackingAssignmentConfiguration();
            foreach (var item in View.lstAdminsConfiguration)
            {
                if (!item.lstConfigObjMapping.IsNullOrEmpty())
                {
                    //New parameter append in comma seperated
                    item.allObjectsName = String.Join(",", item.lstConfigObjMapping.Select(sel => sel.ObjectName));
                }
            }
        }

        public Boolean UpdateConfiguration(TrackingAssignmentConfigurationContract trackingConfigurationContract, List<TrackingConfigObjectMappingContract> lstTrackObjMappingToDelete)
        {
            return SecurityManager.UpdateConfiguration(trackingConfigurationContract, View.CurrentLoggedInUserId, lstTrackObjMappingToDelete);
        }

        public Boolean DeleteConfiguration(Int32 TAC_ID)
        {
            return SecurityManager.DeleteConfiguration(TAC_ID, View.CurrentLoggedInUserId);
        }

        #region UAT-3075
        public void GetComplianceObjects()
        {
            View.lstObjects = SecurityManager.GetCompliancePriorityObjects();
        }

        public void GetTrackConfigObjectMapped()
        {
            View.lstTrackConfigObjectsMapped = SecurityManager.GetTrackConfigObjectMapped(View.selectedTrackConfigID);
            foreach (var item in View.lstTrackConfigObjectsMapped)
            {
                if (!View.lstObjects.IsNullOrEmpty())
                {
                    item.ObjectName = View.lstObjects.Where(cond => cond.CPO_ID == item.TCOM_ComplianceObjectID).Select(sel => sel.CPO_Name).FirstOrDefault();
                }
            }
        }
        #endregion
    }
}
