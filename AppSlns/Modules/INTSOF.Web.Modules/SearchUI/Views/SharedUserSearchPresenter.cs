using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.UI.Contract.ComplianceManagement;
using Business.RepoManagers;
using INTSOF.Utils;

namespace CoreWeb.SearchUI.Views
{
    public class SharedUserSearchPresenter : Presenter<ISharedUserSearch>
    {
        #region EVENTS
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            //GetTenants();
        }
        #endregion

        #region METHODS

        /// <summary>
        /// Method to Perform search basis on applied filters on the screen
        /// </summary>
        public void PerformSearch()
        {
            SharedUserSearchContract sharedUserSearchContract = new SharedUserSearchContract();
            sharedUserSearchContract.SharedUserID = Convert.ToInt32(View.SharedUserID);
            sharedUserSearchContract.FirstName = View.FirstName;
            sharedUserSearchContract.LastName = View.LastName;
            sharedUserSearchContract.UserName = View.UserName;
            sharedUserSearchContract.EmailAddress = View.EmailAddress;

            View.SharedUserResultContract = ProfileSharingManager.GetSharedUserSearchData(sharedUserSearchContract, View.GridCustomPaging);
            if (!View.SharedUserResultContract.IsNullOrEmpty())
            {
                View.VirtualPageCount = View.SharedUserResultContract.Select(col => col.TotalCount).First();
            }
        }
        #endregion
    }
}
