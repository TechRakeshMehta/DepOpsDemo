using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.Search.Views
{
    public class ApplicantPortfolioIntegrationPresenter : Presenter<IApplicantPortfolioIntegrationView>
    {
        #region UAT-2918: Integration Account Linking Exposure
        public System.Data.DataTable GetIntegrationList()
        {
            return SecurityManager.GetIntegrationList(View.OrganizationUserId);
        }
        public Boolean RemoveIntegrationClientOrganizationUserMapping(Int32 IntegrationClientOrganizationUserMapID)
        {
            return SecurityManager.RemoveIntegrationClientOrganizationUserMapping(IntegrationClientOrganizationUserMapID,View.CurrentLoggedInUserId);
        }
     
        #endregion
    }
}
