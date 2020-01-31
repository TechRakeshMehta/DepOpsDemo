using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.TicketsCentre;
using INTSOF.UI.Contract.Templates;
using Entity.ClientEntity;
using INTSOF.UI.Contract.TicketsCentre;
using INTSOF.UI.Contract.FingerPrintSetup;

namespace CoreWeb.TicketsCentre.Views
{
    public interface ITicketView
    {
        Int64 TicketId { get; set; }
        Int32 ClientId { get; set; }
        Int64 ServiceId { get; set; }
        List<TicketIssueSrvcStepMappingContract> TicketIssueSrvcStepMapping { get; set; }
        List<DocumentsContract> AttachedDocuments { get; set; }
        List<Entity.Tenant> lstClients { get; set; }
        List<ClientSrvcMappingContract> lstServices { get; set; }
        List<TicketIssueSrvcStepMappingContract> lstServiceSteps { get; set; }
        List<ClientUsers> lstAssignToUsers { get; set; }
        List<ClientUsers> lstSendTo { get; set; }
        List<ClientUsers> SelectedSendToList { get; set; }

        List<lkpTicketSeverity> lstSeverity { get; set; }
        List<lkpTicketStatu> lstStatus { get; set; }
        Int32 WorkItemID
        {
            get;
            set;
        }
        Int32 SelectedClientID
        {
            get;
            set;
        }
        Int32 ClientServiceMappingID
        {
            get;
            set;
        }
        Int32 CurrentLoggedInUserId
        {
            get;
        }
        Int32 loggedInUserClientID
        {
            get;
            set;
        }
        Int32 LOBId
        {
            get;
        }
        TicketsContract TicketDetails { get; set; }
        Int64 TicketIssueSrvcMappingID { get; set; }
        Int32 AssignTicket { get; set; }
        string Source { get; set; }

        Int32 TenantID { get; set; }
        String LocationIDs
        {
            get;
            set;
        }
        List<LocationContract> lstAvailableLocations
        {
            get;
            set;
        }

        List<lkpTicketIssueType> lstTicketType { get; set; }

        Int16 TicketTypeID { get; set; }

        Boolean IsEnroller { get; }
        String CurrentLoggedInUser_Guid { get; }
        
    }
}
