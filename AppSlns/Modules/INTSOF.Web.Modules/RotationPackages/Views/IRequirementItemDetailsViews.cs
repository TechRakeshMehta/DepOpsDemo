using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public interface IRequirementItemDetailsViews
    {
        Int32 CurrentLoggedInUserId { get; }

        Int32 CurrentPackageId { get; set; }

        Int32 CurrentCategoryId { get; set; }

        IRequirementItemDetailsViews CurrentViewContext { get; }

        List<RequirementItemContract> lstItemDetails { get; set; }

        String CategoryDocumentLink { get; set; }
        String CategoryExplanatoryNotes { set; }   //UAT-3296
        List<RequirementCategoryDocUrl> lstReqCatDocUrls { get; set; } //UAT-4254
    }
}
