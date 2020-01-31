using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class AddPrintScanLocationPresenter : Presenter<IAddPrintScanLocationView>
    {

        public override void OnViewInitialized()
        {

        }

        /// <summary>
        /// To get all the locations details.
        /// </summary>
        public void GetFingerprintLocations()
        {
            View.lstFingerprintLocations = new List<LocationContract>();
            if (View.CurrentLoggedInUserID > AppConsts.NONE)
                View.lstFingerprintLocations = FingerPrintSetUpManager.GetFingerprintLocations(View.CurrentLoggedInUserID, View.IsEnroller);
        }

        /// <summary>
        /// To delete the location and enroller location permission mapping.
        /// </summary>
        /// <param name="selectedLocationID"></param>
        /// <returns></returns>
        public Boolean DeleteFingerPrintLocations(Int32 selectedLocationID)
        {
            return FingerPrintSetUpManager.DeleteFingerPrintLocations(View.CurrentLoggedInUserID, selectedLocationID);
        }

        public Boolean IsAnyScheduleForLocation(Int32 selectedLocationID)
        {
            return FingerPrintSetUpManager.IsAnyScheduleForLocation(selectedLocationID);
        }
    }
}
