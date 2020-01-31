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
    public class ImageViewerPresenter : Presenter<IImageViewerView>
    {
        public void GetImageDataList()
        {
            View.lstLocationImagesData = new List<FingerPrintLocationImagesContract>();

            View.lstLocationImagesData = FingerPrintSetUpManager.GetLocationImages(new CustomPagingArgsContract(), View.LocationId);

        }
        
    }
}
