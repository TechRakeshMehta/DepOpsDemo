using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.TicketsCentre.Views
{
    public class TicketNotesPresenter : Presenter<ITicketNotesView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads   
            //View.lkpTicketStatus = LookupManager.GetLookUpData<lkpTicketStatu>().ToList();
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }      

        public void GetTicketNotesList(Int64 TicketIssueID)
        {
            View.TicketNotesList = TicketsCentreManager.GetTicketNotesList(View.TenantID,TicketIssueID);
        }

        public Boolean SaveTicketNotes(TicketIssuesNote ticketIssuesNote)
        {
            Boolean result = TicketsCentreManager.SaveTicketNotes(View.TenantID,ticketIssuesNote);
            return result;
        }
    }
}



