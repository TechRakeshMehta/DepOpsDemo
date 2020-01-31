using INTSOF.UI.Contract.PackageBundleManagement;
using System;

namespace CoreWeb.CommonOperations.Views
{
    public interface IBulletinPopupView
    {
        Int32 LoggedInUserID { get; }
        Int32 BulletinID { get; set; }
        BulletinContract ViewContract { get; set; }
    }
}
