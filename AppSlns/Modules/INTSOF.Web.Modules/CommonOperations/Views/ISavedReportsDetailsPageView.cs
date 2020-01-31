using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.SystemSetUp;

namespace CoreWeb.CommonOperations.Views
{
    public interface ISavedReportsDetailsPageView
    {
        ISavedReportsDetailsPageView CurrentViewContext { get; }
        String ErrorMessage { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 SelectedFavParamID { get; set; }
        Int32 SelectedTenantID { get;  }
        String SelectedTenantIDs { get; }
        ReportFavouriteParameter SelectedFavouriteParameter { get; set; }
        String SelectedNodeIds { get; }
        String SelectedCategoryIds { get; }
        Int32 SelectedUserID { get; }
    }
}

