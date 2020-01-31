using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public class LocationDepartmentPresenter: Presenter<ILocationDepartmentView>
    {
        public void GetDepartments()
        {
            View.lstDepartment = new List<Department>();
            View.lstDepartment = PlacementMatchingSetupManager.GetDepartments();
        }

        public void GetStudentTypes()
        {
            View.lstStudentType = new List<StudentType>();
            View.lstStudentType = PlacementMatchingSetupManager.GetStudentTypes();
        }

        public void GetAgencyLocationDepartment()
        {
            View.lstAgencyLocationDepartment = new List<AgencyLocationDepartmentContract>();
            View.lstAgencyLocationDepartment = PlacementMatchingSetupManager.GetAgencyLocationDepartment(View.AgencyLocationID);
        }

        public Boolean SaveAgencyLocationDepartment()
        {
            return PlacementMatchingSetupManager.SaveAgencyLocationDepartment(View.LocationDepartment, View.CurrentLoggedInUserId);
        }

        public Boolean DeleteAgencyLocationDepartment()
        {
            return PlacementMatchingSetupManager.DeleteAgencyLocationDepartment(View.LocationDepartment.AgencyLocationDepartmentID, View.CurrentLoggedInUserId);
        }
    }
}
