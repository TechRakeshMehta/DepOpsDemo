using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.Templates;
using INTSOF.UI.Contract.TicketsCentre;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.TicketsCentre
{
    [Serializable]
    public class TicketsContract
    {
        public Int64 TicketIssueID { get; set; }
        public Int32 ClientId { get; set; }
        public string ClientName { get; set; }
        public string Service { get; set; }
        public Int64 ServiceId { get; set; }
        public string TicketSummary { get; set; }
        public string TicketSummaryEncypted { get; set; }
        public string TicketDetailEncypted { get; set; }
        public string TicketDetail { get; set; }
        public string SeverityName { get; set; }
        public int SeverityId { get; set; }
        public string SeverityCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? CreatedOnDatePart { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public int TicketStatusId { get; set; }
        public string TicketStatus { get; set; }
        public string TicketStatusCode { get; set; }
        public string TicketNotes { get; set; }
        public string AssignedTo { get; set; }
        //public Dictionary<int, string> SendTo { get; set; }
        public List<DocumentsContract> AttachedDocuments { get; set; }
        public Int32 CreatedBy { get; set; }
        public Int32 ModifiedBy { get; set; }
        public string ModifiedByUserName { get; set; }
        public DateTime? ModifiedOnDatePart { get; set; }
        public List<TicketIssueSrvcStepMappingContract> TicketIssueSrvcStepMapping { get; set; }
        public List<ClientUsers> SendToList { get; set; }
        public Int32 AssignToUserID { get; set; }
        public Int64 TicketIssueSrvcMappingID { get; set; }
        public Int32 TicketIssueSrvcStepMappingID { get; set; }
        public Int32 TicketIssueTypeID { get; set; }
        public string TicketIssueTypeCode { get; set; }
        public Int32? WorkItemID { get; set; }
        public List<TicketIssueSrvcStepMappingContract> lstServiceSteps { get; set; }
        public Int32 ClientSrvcMappingID { get; set; }
        public Int32 CurrentLoggedInUserId { get; set; }


        public String UserName { get; set; }
        public CustomPagingArgsContract GridCustomPagingArguments { get; set; }

        public String EmailConversationId { get; set; }
        public String RepliedEmailContent { get; set; }

        public string CreatedByUserName { get; set; }
        public Int32 TicketEmailLOBMappingID { get; set; }

        public String TicketType { get; set; }
        public String LocationIds { get; set; }
        public String LocationName { get; set; }
        public Boolean IsTicketNotesSaved { get; set; }

    }
}
