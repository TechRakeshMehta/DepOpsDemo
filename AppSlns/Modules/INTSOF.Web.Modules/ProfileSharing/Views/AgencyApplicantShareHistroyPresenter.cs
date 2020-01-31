using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ProfileSharing;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ProfileSharing.Views
{
    public class AgencyApplicantShareHistroyPresenter : Presenter<IAgencyApplicantShareHistroy>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        /// <summary>
        /// On View Initialized Event
        /// </summary>
        public override void OnViewInitialized()
        {
        }
        /// <summary>
        /// On View Loaded Event
        /// </summary>
        public override void OnViewLoaded()
        {
        }

        public void GetApplicantProfileSharingHistory()
        {
            List<AgencyApplicantShareHistoryContract> lstAgencyApplicantShareHistoryContractData = new List<AgencyApplicantShareHistoryContract>();
            AgencyApplicantShareHistoryContract objAgencyApplicantStatusContract = new AgencyApplicantShareHistoryContract();
            objAgencyApplicantStatusContract.ApplicantId = View.ApplicantId;
            objAgencyApplicantStatusContract.TenantId = View.TenantId;
            objAgencyApplicantStatusContract.AgencyOrgUserID = View.AgencyOrgUserID;
            lstAgencyApplicantShareHistoryContractData = ProfileSharingManager.GetApplicantProfileSharingHistory(objAgencyApplicantStatusContract, View.GridCustomPaging);

            if (!lstAgencyApplicantShareHistoryContractData.IsNullOrEmpty())
            {
                if (lstAgencyApplicantShareHistoryContractData[0].TotalCount > 0)
                {
                    View.VirtualRecordCount = lstAgencyApplicantShareHistoryContractData[0].TotalCount;
                }
                View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
            }
            else
            {
                View.VirtualRecordCount = 0;
                View.CurrentPageIndex = 1;
            }
            View.lstAgencyApplicantShareHistory = lstAgencyApplicantShareHistoryContractData;
        }
    }
}
