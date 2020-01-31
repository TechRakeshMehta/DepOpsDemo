using INTSOF.UI.Contract.BkgOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IBkgNoteView
    {
        Int32 SelectedTenantId { get; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 OrderID { get; }
        String NewNote { get; set; }
        
        List<BkgOrderQueueNotesContract> LstNotes { get; set; }
    }
}
