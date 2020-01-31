using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.FingerPrintSetup;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public class ManageHrAdminPermissionPresenter : Presenter<IManageHrAdminPermissionView>
    {

        public void GetHrAdminpermissions()
        {
            View.lstPermittedHrAdmins = new List<INTSOF.UI.Contract.FingerPrintSetup.HrAdminPermissionContract>();
            View.lstPermittedHrAdmins = FingerPrintSetUpManager.GetAllHrAdmins(View.GridCustomPaging, View.FilterContract);
            View.VirtualRecordCount = View.lstPermittedHrAdmins.Count;
        }

        public void GetCABSUsers()
        {
            View.lstCABSUsers = FingerPrintSetUpManager.GetCABSUsers();
        }

        public Boolean SaveHrAdminPermission(HrAdminPermissionContract PermissionContract)
        {
            return FingerPrintSetUpManager.SaveHrAdminPermission(PermissionContract, View.CurrentLoggedInUserID);
        }


        public Boolean DeleteHrAdminPermission(Int32 selectedUserID)
        {
            return FingerPrintSetUpManager.DeleteHrAdminPermission(View.CurrentLoggedInUserID, selectedUserID);
        }

    }
}
