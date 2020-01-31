using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;

namespace CoreWeb.Shell.Views
{
    public class EmploymentDisclosurePresenter : Presenter<IEmploymentDisclosureView>
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
        /// Method to save Employment Disclosure Details
        /// </summary>
        public Boolean SaveEDDetails()
        {
            return SecurityManager.SaveEDDetails(View.OrganizationUserID);
        }

        /// <summary>
        /// Get Announcements
        /// </summary>
        public void GetAnnouncements()
        {
            View.lstAnnouncementID = SecurityManager.GetAnnouncements(View.OrganizationUserID);
        }
    }
}
