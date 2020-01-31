using Entity.ClientEntity;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IManageScheduleTimeView
    {
        Int32 selectedTenantID { get; set; }
        Int32 SelectedLocationId { get; set; }
        List<OrganizationUser> lstClientAdminUser { get; set; }
        List<ManageScheduleTimeContract> lstSchedlueTimeOff { get; set; }
        ManageScheduleTimeContract ManageScheduleTimeContract { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        bool IsReadOnly { get; set; }
        bool IsEnroller { get; set; }
        #region Custom Args
        Int32 VirtualRecordCount { get; set; }
        Int32 PageSize { get; set; }
        Int32 CurrentPageIndex { get; set; }
        CustomPagingArgsContract GridCustomPaging { get; set; }
        #endregion
    }
}
