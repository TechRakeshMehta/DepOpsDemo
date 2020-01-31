using INTSOF.UI.Contract.PackageBundleManagement;
using System;

namespace CoreWeb.CommonOperations.Views
{
    public interface IAnnouncementPopupView
    {
        Int32 LoggedInUserID { get; }
        Int32 AnnouncementID { get; set; }
        AnnouncementContract ViewContract { get; set; }
    }
}
