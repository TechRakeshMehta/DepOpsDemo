using INTSOF.Utils;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.TicketsCentre;
using Entity.ClientEntity;

namespace DAL.Interfaces
{
    public interface ITicketsCentreRepository
    {
        DataTable GetTickets(int LOBID, CustomPagingArgsContract obj, Int32 ViewAllTicketsPermission, Int32 ClientID, Int32 UserID, Int64 workItemID, Int32 timeZoneOffset,
            TicketSearchContract ticketSearchContract);
        Int32 AddTicket(TicketsContract TicketContract, DataTable ListClientSrvcStepMappingID, DataTable SendToList, DataTable ListTicketDocuments, Int32 LoggedInUserID);
        //List<Entity.Tenant> GetListOfClients();
        //List<ClientUsers> GetClientUsers(Int32 UserId, Int32 LoggedInClientID, Int32? SelectedClientID);
        bool DeleteTicket(Int64 TicketIssueId, Int32 UserId);
        DataSet GetTicketDetailById(Int64 TicketIssueId);
        //Comments By [SS]
        List<ClientSrvcStepMapping> GetServiceStepsList(Int32 ClientServiceMappingID, Int32 ClientID);
        //List<ClientSrvcStepMapping> GetWorkItemStepsList(Int32 ClientServiceMappingID, Int32 ClientID, Int32 WorkItemId);

        #region Ticket Notes
        List<TicketsContract> GetTicketNotesList(Int64 TicketIssueID);
        Boolean SaveTicketNotes(TicketIssuesNote ticketIssuesNote);

        #endregion

        #region Ticket Document
        Boolean DeleteDocument(Int64 ticketID, Int32 documentID, Int16 entityTypeID, Int32 loggedInClientID);
        TicketDocument GetTicketDocument(Int32 documentID, Int32 documentTypeID);
        
        #endregion

        //Boolean UpdateWorkItemStatus(int WorkItemID, Int32 statusID, int loggedInUserID);

        #region  Bug 19211
        //Comments By [SS]
        //List<TicketIssueSrvcMapping> IsTicketCreatedForServices(List<Int64> lstClientSrvcMappingIDs);
        #endregion

        #region Automatic Ticket Creation
        //Comments By [SS]
        //Boolean CreateAutomaticTicket(TicketsContract TicketContract, DataTable ListTicketDocuments, Int32 LoggedInUserID);

        #endregion

        #region Ticket Center Settings
        //Comments By [SS]
        //List<TicketEmailLOBMapping> GetTicketCenterSettings(Int32 LOBID);
        //Boolean SaveTicketEmailLOBMapping(TicketEmailLOBMapping ticketEmailLOBMapping);
        //Boolean UpdateTicketEmailLOBMapping(TicketEmailLOBMapping ticketEmailLOBMapping, Int32 TicketEmailLOBMappingID);
        //Boolean DeleteTicketSetting(Int32 TicketEmailLOBMappingID, Int32 CurrentUserId);
        //Boolean CheckEmailAlreadyExists(String email, Boolean isUpdate, Int32 TicketEmailLOBMappingID);
        List<TicketIssue> GetAllTickets();
        List<TicketIssuesNote> GetAllTicketNotes();
        void SaveTickets(List<TicketIssue> lstTicket, List<TicketIssuesNote> lstNotes);

        #endregion

        #region Servcies and Service Steps
        List<ClientSrvcMapping> GetAllClientServiceMapping(Int32 tenantID, Int32? LOBId);
        #endregion

    }
}
