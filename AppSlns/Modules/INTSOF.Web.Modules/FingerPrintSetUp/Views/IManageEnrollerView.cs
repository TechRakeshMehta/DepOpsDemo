using Entity;
using System;
using INTSOF.UI.Contract.FingerPrintSetup;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IManageEnrollerView
    {
        Int32 selectedTenantID { get; set; }
        Int32 SelectedLocationId { get; set; }
        List<OrganizationUser> lstClientAdminUser { get; set; }
        List<ManageEnrollerMappingContract> lstEnrollerMappings { get; set; }
        Int32 CurrentLoggedInUserID { get; }

        #region Custom Args
        Int32 VirtualRecordCount { get; set; }
        Int32 PageSize { get; set; }
        Int32 CurrentPageIndex { get; set; }
        CustomPagingArgsContract GridCustomPaging { get; set; }
        #endregion
    }
}
