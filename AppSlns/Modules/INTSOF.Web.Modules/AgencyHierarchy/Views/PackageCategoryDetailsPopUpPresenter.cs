using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract;
using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.AgencyHierarchy.Views
{
    public class PackageCategoryDetailsPopUpPresenter: Presenter<IPackageCategoryDetailsPopUpView>
    {
        public List<RequirementCategoryContract> GetRequirementPackageCategories(Int32 packageId)
        {
            try
            {
                return View.lstRequirementCategory = AgencyHierarchyManager.GetRequirementCategory(packageId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
           
        }

    }
}
