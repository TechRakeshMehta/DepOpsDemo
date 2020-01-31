
using INTSOF.SharedObjects;
using Business.RepoManagers;
namespace CoreWeb.FingerPrintSetUp.Views
{
    public class ManageHrAdminAppointmentOrderPresenter : Presenter<IManageHrAdminAppointmentOrderView>
    {

        public override void OnViewLoaded()
        {
            //CheckIfUserIsEnroller();
           
        }

        //public void CheckIfUserIsEnroller()
        //{
        //    View.IsEnroller = SecurityManager.CheckIfUserIsEnroller(View.CurrentLoggedInUser_Guid);
        //}

    }
}
