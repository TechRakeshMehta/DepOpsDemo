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
    public class StudentTypeViewPresenter : Presenter<IStudentTypeView>
    {

        public void GetStudentTypes()
        {
            View.lstStudentTypes = PlacementMatchingSetupManager.GetPlacementStudentTypes();
        }
        public bool UpdateStudentType()
        {
            try
            {
                return PlacementMatchingSetupManager.UpdatePlacementStudentType(View.studentTypeContract, View.CurrentUserId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void InsertStudentType()
        {
            try
            {
                if (!PlacementMatchingSetupManager.GetPlacementStudentTypes().Where(stype => stype.Name.ToLower() == View.studentTypeContract.Name.ToLower()).Any())
                    if (PlacementMatchingSetupManager.InsertPlacementStudentType(View.studentTypeContract, View.CurrentUserId))
                        View.SuccessMsg = "Student Type added successfully.";
                    else
                        View.ErrorMsg = "Some error occurred.Please try again.";
                else
                    View.InfoMsg = "Student Type with same name already exists.";

            }
            catch (Exception ex)
            {

            }
        }

        public bool DeleteStudentType(Int32 studentTypeID)
        {
            try
            {
                return PlacementMatchingSetupManager.DeletePlacementStudentType(studentTypeID, View.CurrentUserId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
