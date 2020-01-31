using Entity;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IManageHrAdminPermissionDetailsView
    {
        Int32 CurrentLoggedInUserID { get; }
        Int32 SelectedOrganizationUserID { get; set; }
        String CBIUniqueIds { get; set; }
        String AccountNames { get; set; } 
        String ErrorMessage { get; set; }
        Int32 SelectedPermissionId { get; set; }
        List<Entity.LocationEntity.UserCABSPermissionMapping> lstCBIUniqueIDs { get; set; }
        List<Entity.LocationEntity.UserCABSPermissionMapping> lstAccountNames { get; set; } 
    }
}
