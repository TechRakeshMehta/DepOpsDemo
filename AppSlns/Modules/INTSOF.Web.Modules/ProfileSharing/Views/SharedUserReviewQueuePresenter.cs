using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;

namespace CoreWeb.ProfileSharing.Views
{
    public class SharedUserReviewQueuePresenter : Presenter<ISharedUserReviewQueueView>
    {
        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {
            GetTenants();
        }

        /// <summary>
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {

        }

        /// <summary>
        /// Method To get all Tenants
        /// </summary>
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetTenants(SortByName, false, clientCode);
        }
        
        /// <summary>
        /// Check Admin is Logged or not
        /// </summary>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantID == SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }

        /// <summary>
        /// Method to Get agencies of an institution
        /// </summary>
        public void GetAllAgency()
        {

            if (View.SelectedTenantID == 0)
                View.lstAgency = new List<Agency>();
            else
            {
                //UAT-1881
                if (View.IsAdminLoggedIn)
                {
                    View.lstAgency = ProfileSharingManager.GetAllAgency(View.SelectedTenantID).OrderBy(x => x.AG_Name).ToList();
                }
                else
                {
                    View.lstAgency = ProfileSharingManager.GetAllAgencyForOrgUser(View.SelectedTenantID, View.CurrentLoggedInUserId).OrderBy(x => x.AG_Name).ToList();
                }
            }
        }

        public void GetSharedUserReiewQueueData()
        {
            List<SharedUserReiewQueueContract> sharedUserReiewQueueContractData;
            if (View.SelectedTenantID == 0)
            {
                sharedUserReiewQueueContractData = new List<SharedUserReiewQueueContract>();
            }

            else
            {
                View.SearchContract.TenantID = View.SelectedTenantID;
                var data = ProfileSharingManager.GetSharedUserReviewQueueData(View.SearchContract, View.GridCustomPaging);
                sharedUserReiewQueueContractData = data.Item1;
                View.CurrentPageIndex = data.Item2;
            }
            View.SharedUserReiewQueueContractData = sharedUserReiewQueueContractData;
            if (sharedUserReiewQueueContractData.IsNullOrEmpty())
            {
                View.VirtualRecordCount = AppConsts.NONE;
            }
            else
            {
                View.VirtualRecordCount = sharedUserReiewQueueContractData[0].TotalCount;
            }
        }

        public void DeleteSharedUserReiewQueueRecord()
        {
            ProfileSharingManager.DeleteSharedUserReviewQueueRecord(View.QueueRecordId, View.CurrentLoggedInUserId);
        }

        public void UpdateSharedUserReviewQueueStatus()
        {
            ProfileSharingManager.UpdateSharedUserReviewQueueStatus(View.lstQueueRecords, View.CurrentLoggedInUserId);
        }

        //public bool IsAdminLoggedIn()
        //{
        //   return SecurityManager.DefaultTenantID == View.TenantID;
        //}
    }
}
