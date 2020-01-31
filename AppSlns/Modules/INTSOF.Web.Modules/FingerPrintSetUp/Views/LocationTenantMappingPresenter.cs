using Business.RepoManagers;
using DAL.Repository;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class LocationTenantMappingPresenter : Presenter<ILocationTenantMappingView>
    {
        public List<LocationContract> GetTenantLocationData()
        {
            View.lstMappedLocations = new List<LocationContract>();
            View.lstMappedLocations = FingerPrintSetUpManager.GetLocationAvailable(View.TenantId);
            return View.lstMappedLocations;
        }
        public List<LocationContract> GetDPMLocationIDs(int SelectedDPMID)
        {
            View.lstMappedLocations = new List<LocationContract>();
            if (View.SelectedDPMID > AppConsts.NONE)
            {
                View.lstMappedLocations = FingerPrintSetUpManager.GetLocationAvailable(View.TenantId);

                List<Int32> DPMLocations = FingerPrintDataManager.GetDPMLocations(View.TenantId, View.SelectedDPMID).ToList();
                View.DPMMappedLocations = new List<LocationContract>();
                if (!DPMLocations.IsNullOrEmpty() && !View.lstMappedLocations.IsNullOrEmpty())
                {
                    View.DPMMappedLocations = View.lstMappedLocations.Where(a => DPMLocations.Contains(a.LocationID)).ToList();
                }

                foreach (LocationContract locContract in View.DPMMappedLocations)
                {
                    Boolean isLocationInUse = false;
                    isLocationInUse = FingerPrintSetUpManager.IsAnyScheduleForLocation(locContract.LocationID);
                    locContract.IsInUse = isLocationInUse;
                }
            }
            return View.DPMMappedLocations;
        }
        public bool SaveTenantLocationMapping()
        {
            return FingerPrintDataManager.SaveTenantLocationMapping(View.SelectedLocationID, View.TenantId, View.SelectedDPMID, View.CurrentLoggedInUserID);
        }




        public bool DeleteTenantLocationMapping(int selectedLocationId)
        {
            return FingerPrintDataManager.DeleteTenantLocationMapping(View.TenantId, View.SelectedDPMID, selectedLocationId, View.CurrentLoggedInUserID);
        }

        public List<LocationContract> GetDropDownLocationTenantData()
        {
            List<Int32> lstlocationIDs = View.lstMappedLocations.Select(Sel => Sel.LocationID).ToList();
            List<LocationContract> lstAlllocations = new List<LocationContract>();
            lstAlllocations = FingerPrintSetUpManager.GetFingerprintLocations(View.CurrentLoggedInUserID, false);

            View.lstAvailableLocations = new List<LocationContract>(lstAlllocations);
            if (!View.lstAvailableLocations.IsNullOrEmpty() && View.lstAvailableLocations.Count > AppConsts.NONE)
            {
                View.lstAvailableLocations.RemoveAll(x => lstlocationIDs.Contains(x.LocationID));
            }

            return View.lstAvailableLocations;
        }
    }
}
