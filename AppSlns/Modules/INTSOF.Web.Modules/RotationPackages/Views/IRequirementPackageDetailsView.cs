using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface IRequirementPackageDetailsView
    {
        Int32 CurrentLoggedInUserId { get; }

        Int32 CurrentPackageId { get; set; }

        IRequirementPackageDetailsView CurrentViewContext { get; }

        Dictionary<Int32, String> lstCategoryDetails { get; set; }

        String RequirementPackageName { get; set; }
    }
}
