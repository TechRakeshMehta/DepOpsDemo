#region NameSpace

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Project Specific
using INTSOF.SharedObjects;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using Business.RepoManagers;
#endregion

#endregion

namespace CoreWeb.ClinicalRotation.Views
{
    public class ManageAgencyUploadPresenter : Presenter<IManageAgencyUploadView>
    {

        private ClinicalRotationProxy _clientRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }

        public void SaveUpdateAgencyData()
        {
            if (!View.AgencyXmlData.IsNullOrEmpty())
            {
                List<AgencyDataContract> lstAgencyData = new List<AgencyDataContract>();
                lstAgencyData = ClinicalRotationManager.SaveUpdateAgencyInBulk(View.AgencyXmlData, View.CurrentLoggedInUserId);
                View.LstAgencyData = lstAgencyData.Where(x => x.IsAgencyUploaded == true).ToList();
                View.LstNotUploadedAgencyData = lstAgencyData.Where(x => x.IsAgencyUploaded == false).ToList();
            }
            else
            {
                View.LstAgencyData = new List<AgencyDataContract>();
                View.LstNotUploadedAgencyData = new List<AgencyDataContract>();
            }
        }

    }
}
