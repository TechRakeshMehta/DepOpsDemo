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
    public interface IManageHrAdminPermissionView
    {
        Int32 CurrentLoggedInUserID { get; }
        List<HrAdminPermissionContract> lstPermittedHrAdmins { get; set; }
        Int32 PageSize { get; set; }
        Int32 CurrentPageIndex { get; set; }
        Int32 VirtualRecordCount { get; set; }
        CustomPagingArgsContract GridCustomPaging { get; set; }
        HrAdminPermissionContract FilterContract { get; set; }        
        Dictionary<Int32, String> lstCABSUsers { get; set; }

    }
}
