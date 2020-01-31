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
    public interface IManageOnsiteEventsView
    {
        Int32 selectedTenantID { get; set; }
        Int32 EventId { get; set; }        
        Int32 LocationId { get; set; }       
        List<ManageOnsiteEventsContract> lstEvents { get; set; }
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
