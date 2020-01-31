using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class FingerprintLocationDetailsPresenter : Presenter<IFingerprintLocationDetailsView>
    {
        //public Boolean SaveFingerPrintLocations()
        //{
        //    return FingerPrintSetUpManager.SaveFingerPrintLocations(View.TenantId, View.CurrentLoggedInUserID, View.locationContract);
        //}

        //public void GetSelectedLocationDetails()
        //{
        //    View.locationContract = new LocationContract();
        //    View.locationContract = FingerPrintSetUpManager.GetSelectedLocationDetails(View.TenantId, View.SelectedLocationID);

        //}
        #region Commented Code For Payment Options
        // Commented below code for task Payment options should be removed from Location screen for everyone
        /// <summary>
        /// method to get payment options
        /// </summary>
        //public void GetPaymentOptions()
        //{
        //    View.ListPaymentOption = FingerPrintSetUpManager.GetAllPaymentOption().ToList();
        //}

        /// <summary>
        /// To get selected Payment Options
        /// </summary>
        //public void GetSelectedPaymentOptions()
        //{
        //    View.SelectedMappedPaymentOptions = FingerPrintSetUpManager.GetMappedLocationPaymentOption(View.SelectedLocationID)
        //                                                             .Select(x => x.LPO_PaymentOptionID).ToList();

        //}

        //public Boolean SaveMappedPaymentOptions()
        //{
        //  return FingerPrintSetUpManager.SaveMappedPaymentOptions(View.SelectedLocationID, View.SelectedMappedPaymentOptions, View.CurrentLoggedInUserID);
        //}
        #endregion
        public void GetEnrollerPermission()
        {
            View.EnrollerPermission = new ManageEnrollerMappingContract();
            View.EnrollerPermission = FingerPrintSetUpManager.GetEnrollerPermission(View.CurrentLoggedInUserID, View.SelectedLocationID);
        }

        public Int32 SaveTimeFrame(Int32? TimeFrame)
        {
            
            return FingerPrintSetUpManager.SaveFingerPrintLocationTimeFrame(View.CurrentLoggedInUserID, View.SelectedLocationID,TimeFrame);
        }
        public String GetLocationDelayFrame()

        {
            return FingerPrintSetUpManager.GetSelectedLocationDetails(View.SelectedLocationID).TimeFrame.ToString();
        }

    }
}
