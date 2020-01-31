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
    public class FingerprintLocationGroupPresenter : Presenter<IFingerprintLocationGroupView>
    {
        public void GetLocationGroupList()
        {
            View.LocationGroupLst = new List<FingerprintLocationGroupContract>();
            View.LocationGroupLst = FingerPrintSetUpManager.GetLocationGroupList(View.GridCustomPaging, View.filterContract);
            if (View.LocationGroupLst.Count > AppConsts.NONE)
                View.VirtualRecordCount = View.LocationGroupLst.FirstOrDefault().TotalCount;
            else
                View.VirtualRecordCount = AppConsts.NONE;
        }
        public Boolean SaveUpdateLocationGroup()
        {
            return FingerPrintSetUpManager.SaveUpdateLocationGroup(View.filterContract, View.CurrentLoggedInUserID);
        }
        public Boolean DeleteLocationGroup(Int32 LocationGroupID)
        {
            return FingerPrintSetUpManager.DeleteLocationGroup(LocationGroupID, View.CurrentLoggedInUserID);
        }
    }
}
