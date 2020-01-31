using Business.RepoManagers;
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
    public class FingerPrintLocationMapPresenter : Presenter<IFingerPrintLocationMapView>
    {
        //public Boolean SaveLocation()
        //{
        //    return FingerPrintSetUpManager.SaveLocation(View.TenantId, View.LocationDetails, View.CurrentLoggedInUserId);
        //}
        public Int32 SaveFingerPrintLocations()
        {
            return FingerPrintSetUpManager.SaveFingerPrintLocations(View.CurrentLoggedInUserId, View.LocationDetails);
        }

        public void GetSelectedLocationDetails()
        {
            View.LocationDetails = new LocationContract();
            View.LocationDetails = FingerPrintSetUpManager.GetSelectedLocationDetails(View.SelectedLocationId);
        }
        public void GetLocationGroupList()
        {
            View.lstLocationGroup = new List<FingerprintLocationGroupContract>();
            View.lstLocationGroup = FingerPrintSetUpManager.GetLocationGroupCompleteList();
        }

        /// <summary>
        /// Method for saving the location images
        /// </summary>
        /// <returns></returns>
        //public Boolean SaveLocationImages()
        //{
        //    return FingerPrintSetUpManager.SaveLocationImages(View.AddedLocationImagesData, View.SelectedLocationId,View.CurrentLoggedInUserId);
        //}

        public void GetLocationImages()
        {
            View.AddedLocationImagesData = new List<FingerPrintLocationImagesContract>();
            if (!View.SelectedLocationId.IsNullOrEmpty())
                View.AddedLocationImagesData = FingerPrintSetUpManager.GetLocationImages(View.GridCustomPaging, View.SelectedLocationId);
            if (View.AddedLocationImagesData.Count > 0)
                View.VirtualRecordCount = View.AddedLocationImagesData.FirstOrDefault().TotalCount;
            else
                View.VirtualRecordCount = AppConsts.NONE;
        }

        public Boolean DeleteLocationImage(Int32 ImageId)
        {
            if (ImageId > AppConsts.NONE)
                return FingerPrintSetUpManager.DeleteLocationImage(ImageId, View.CurrentLoggedInUserId);
            return false;
        }
    }
}
