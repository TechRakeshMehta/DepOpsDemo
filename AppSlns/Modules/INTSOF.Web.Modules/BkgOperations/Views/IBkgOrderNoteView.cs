using INTSOF.UI.Contract.BkgOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IBkgOrderNoteView
    {
        Int32 OrderID { get; set; }

        Int32 CurrentUserId { get; }

        Int32 TenantID { get; set; }

        String Notes { get; set; }

        List<BkgOrderQueueNotesContract> LstNotes { get; set; }
    }
}
