#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using Business.RepoManagers;
using INTSOF.SharedObjects;
using System.Linq;
using INTSOF.Utils;


#endregion

#endregion

namespace CoreWeb.ProfileSharing.Views
{
    public class InstructorPreceptorDashboardPresenter : Presenter<IInstructorPreceptorDashboardView>
    {
        public override void OnViewInitialized()
        {
        }

        public override void OnViewLoaded()
        {

        }

        public void GetSharedUserDetails()
        {
            View.SharedUserDetails = ProfileSharingManager.GetSharedUserDashboardDetails(View.UserId);
        }
    }
}
