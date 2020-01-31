using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IApplicantCommunicationGridControlView
    {
        Int32 UserId { get; }
        Int32 CurrentPageIndex { get; set; }
        Int32 VirtualPageCount { set; }
        Int32 PageSize { get; }
        String DefaultSortExpression { get; set; }
        Boolean IsSortDirectionDescending { get; set; }
        List<MessageDetail> lstUserMessageDetailList { get; set; }
        Int32 ApplicantDashboard { get; set; }
        #region UAT-3261: Badge Form Enhancements
        List<EmailDetails> lstUserEmailDetailList { get; set; }
        String DefaultEmailSortExpression { get; set; }
        Int32 SystemCommunicationId { get; set; }
        List<Int32> SystemCommunicationDeliveryIds { get; set; }
        #endregion
    }
}
