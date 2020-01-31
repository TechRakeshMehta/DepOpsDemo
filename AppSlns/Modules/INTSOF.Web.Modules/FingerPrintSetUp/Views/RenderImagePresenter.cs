using Business.RepoManagers;
using INTSOF.SharedObjects;
using System;

namespace CoreWeb.FingerPrintSetUp.Views
{
   public class RenderImagePresenter:Presenter<IRenderImageView>
    {

        public void GetFingerPrintImageData(Int32 entityID,Int32 ApplicantAppointmentDetailID)
        {

            View.ApplicantFingerPrintImagesData = FingerPrintDataManager.GetFingerPrintImageData(entityID,ApplicantAppointmentDetailID);

        }
    }
}
