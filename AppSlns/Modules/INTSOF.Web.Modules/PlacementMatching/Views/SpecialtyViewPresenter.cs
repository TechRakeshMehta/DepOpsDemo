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
   public class SpecialtyViewPresenter: Presenter<ISpecialtyView>
    {

       public void GetSpecialties()
       {
           View.lstSpecialties = PlacementMatchingSetupManager.GetPlacementSpecialties();
       }
  

       public bool UpdateSpecialty()
       {
           try
           {
               return PlacementMatchingSetupManager.UpdatePlacementSpecialty(View.specialtyContract, View.CurrentUserId);
           }
           catch (Exception ex)
           {
               return false;
           }
       }
       public void InsertSpecialty()
       {
           try
           {
               if (!PlacementMatchingSetupManager.GetPlacementSpecialties().Where(spec => spec.Name.ToLower() == View.specialtyContract.Name.ToLower()).Any())
                   if (PlacementMatchingSetupManager.InsertPlacementSpecialty(View.specialtyContract, View.CurrentUserId))
                       View.SuccessMsg = "Specialty added successfully.";
                   else
                       View.ErrorMsg = "Some error occurred.Please try again.";
               else
                   View.InfoMsg = "Specialty with same name already exists.";

           }
           catch (Exception ex)
           {

           }
       }

       public bool DeleteSpecialty(Int32 specialtyID)
       {
           try
           {
               return PlacementMatchingSetupManager.DeletePlacementSpecialty(specialtyID, View.CurrentUserId);
           }
           catch (Exception ex)
           {
               return false;
           }
       }
    }
}
