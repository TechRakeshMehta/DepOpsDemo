using INTSOF.UI.Contract.PersonalSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.DashBoard.DashBoard.Views
{
    public interface IManageFavouriteFeaturesView
    {
        IManageFavouriteFeaturesView CurrentViewContext { get; }

        int TenantID { get; set; }

        Int32 OrganizationUserId { get; }

        Int32 SysXBlockId { get; }

        Int32 BusinessChannelTypeID { get; }

        List<BookmarkedFeatureContact> lstAccessibleFeatures { get; set; }

        Boolean IsSuperAdmin { get; }
    }
}
