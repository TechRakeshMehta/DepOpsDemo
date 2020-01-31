using Entity;
using INTSOF.UI.Contract.TicketsCentre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.TicketsCentre.Views
{
    public interface ITicketNotesView
    {      
        List<TicketsContract> TicketNotesList { get; set; }
        Int64 TicketIssueID { get; set; }

        Int32 TenantID { get; set; }

        
    }
}


