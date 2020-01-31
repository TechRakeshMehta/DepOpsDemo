using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.Security.Views
{
    public class LocationImagesPresenter : Presenter<ILocationImagesView>
    {
        public FingerPrintApplicantLocationImageContract GetImageDataList(Int32 LocationId)
        {
            return FingerPrintSetUpManager.GetImagesWithLocationId(LocationId);

        }
        public String GetUserPreferedLangCode(Int32 UserId, Int32 TenantId)
        {
            return FingerPrintSetUpManager.getUserPreferLangCode(UserId, TenantId);
        }
    }
}
