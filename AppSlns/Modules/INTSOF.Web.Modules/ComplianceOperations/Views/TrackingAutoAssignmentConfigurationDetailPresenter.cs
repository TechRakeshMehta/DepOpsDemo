using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public class TrackingAutoAssignmentConfigurationDetailPresenter : Presenter<ITrackingAutoAssignmentConfigurationDetailView>
    {
        public void GetUserList()
        {
            View.lstOrganizationUser = SecurityManager.GetOganisationUsersByTanentId(SecurityManager.DefaultTenantID).Select(x => new Entity.OrganizationUser
                                       {
                                           FirstName = x.FirstName + " " + x.LastName,
                                           OrganizationUserID = x.OrganizationUserID
                                       }).ToList();
        }

        public Boolean SaveAdminsConfig(List<TrackingAssignmentConfigurationContract> lstAdminsConfig)
        {
            return SecurityManager.SaveAdminsConfig(lstAdminsConfig, View.CurrentLoggedInUserId);
        }

        public List<TrackingAssignmentConfigurationContract> UserBucketData()
        {
            return SecurityManager.GetAdminTrackingAssignmentConfiguration();
        }

        #region UAT-3075
        public void GetComplianceObjects ()
        {
            View.lstObjects = SecurityManager.GetCompliancePriorityObjects();
        }
        #endregion

    }
}
