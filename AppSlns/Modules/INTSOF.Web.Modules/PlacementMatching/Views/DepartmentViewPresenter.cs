using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.PlacementMatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.PlacementMatching.Views
{
    public class DepartmentViewPresenter : Presenter<IDepartmentView>
    {

        public void GetDepartments()
        {
            View.lstDepartments = PlacementMatchingSetupManager.GetPlacementDepartments();
        }

        public bool UpdateDepartment()
        {
            try
            {
                return PlacementMatchingSetupManager.UpdatePlacementDepartment(View.departmentContract,View.CurrentUserId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void InsertDepartment()
        {
            
                if (!(PlacementMatchingSetupManager.GetPlacementDepartments().Where(dept => dept.Name.ToLower().Equals(View.departmentContract.Name.ToLower())).Any()))
                    if (PlacementMatchingSetupManager.InsertPlacementDepartment(View.departmentContract, View.CurrentUserId))
                        View.SuccessMsg = "Department added successfully.";
                    else
                        View.ErrorMsg = "Some error occurred.Please try again.";
                else
                    View.InfoMsg = "Department with same name already exists.";

            
           
        }

        public bool DeleteDepartment(Int32 departmentID)
        {
            try
            {
                return PlacementMatchingSetupManager.DeletePlacementDepartment(departmentID, View.CurrentUserId);
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
