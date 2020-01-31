using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace CoreWeb.AgencyHierarchy.Views
{
    public interface IPackageCategoryDetailsPopUpView
    {

        List<RequirementCategoryContract> lstRequirementCategory
        {
            get;
            set;
        }
        IPackageCategoryDetailsPopUpView CurrentViewContext
        {
            get;
        }
    }
}
