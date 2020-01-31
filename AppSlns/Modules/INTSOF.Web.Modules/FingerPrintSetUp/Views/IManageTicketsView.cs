using Entity.ClientEntity;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.UI.Contract.TicketsCentre;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.Tickets.Views
{
    public interface IManageTicketsView
    {

        Int64 TicketIssueID { get; set; }
        Int32 LobId { get; set; }
        Int32 ClientId { get; set; }
        string ClientName { get; set; }
        string Service { get; set; }
        string TicketSummary { get; set; }
        string TicketDetail { get; set; }
        Int16 SeverityId { get; set; }
        string SeverityName { get; set; }
        Int16 TicketStatusId { get; set; }
        string TicketStatus { get; set; }
        List<TicketsContract> lstTickets { get; set; }
        List<Entity.Tenant> lstTenant { get; set; }
        //Comments BY [SS]
        //List<CUService> lstServices { get; set; }
        //List<ServiceStep> lstServiceSteps { get; set; }
        List<lkpTicketSeverity> lstSeverity { get; set; }
        List<lkpTicketStatu> lstTicketStatus { get; set; }
        Dictionary<int, string> SendTo { get; set; }
        List<string> AttachedDocuments { get; set; }
        Int32 SelectedTenantID { get; set; }
        string ErrorMessage { get; set; }
        string SuccessMessage { get; set; }
        string InfoMessage { get; set; }
        CustomPagingArgsContract CustomPagingContract
        {
            get;
            set;
        }
        #region Paging Options

        /// <summary>
        /// Get or set current page index of grid
        /// </summary>
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set page size of grid
        /// </summary>
        Int32 PageSize
        {
            get;
            //set;
        }

        /// <summary>
        /// Get or set virtual page count of grid
        /// </summary>
        Int32 VirtualPageCount
        {
            get;
            set;
        }
        //List<ClientUsers> lstAssignToUsers { get; set; }
        #endregion
        Int32 CurrentLoggedInUserId
        {
            get;
        }
        Int32 loggedInUserClientID
        {
            get;
            set;
        }
        Int32 ViewAllTickets { get; set; }

        String QueueCode { get; set; }

        Int64 WorkItemID { get; set; }
        Boolean IsEncryptionApplied { get; set; }
        List<lkpTicketIssueType> lstTicketType { get; set; }
        Int16 TicketTypeID { get; set; }

        TicketSearchContract TicketSearchContract { get; set; }
        String LocationIDs { get; set; }

        List<LocationContract> lstAvailableLocations { get; set; }
        Boolean IsEnroller { get; }
        String CurrentLoggedInUser_Guid { get; }
        
    }
}
