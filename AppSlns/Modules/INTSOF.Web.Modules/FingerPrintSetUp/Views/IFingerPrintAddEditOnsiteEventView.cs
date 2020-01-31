using System;
using INTSOF.UI.Contract.FingerPrintSetup;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.FingerPrintSetUp.Views
{
   public interface IFingerPrintAddEditOnsiteEventView
    {
        ManageOnsiteEventsContract OnsiteEventDetail { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Boolean IsEditMode { get; set; }
        Boolean IsSaveMode { get; set; }
        Int32 SelectedLocationId { get; set; }
        Int32 SelectedEventId { get; set; }
        Boolean IsEditClicked { get; set; }
        List<FingerPrintEventSlotContract> lstEventSlots { get; set; }
        bool IsEnroller { get; set; }
        bool IsPublished { get; set; }
        bool IsReadOnly { get; set; }
        Int32 PageSize { get; set; }
        Int32 CurrentPageIndex { get; set; }
        Int32 VirtualRecordCount { get; set; }    
        CustomPagingArgsContract GridCustomPaging { get; set; }
    }
}
