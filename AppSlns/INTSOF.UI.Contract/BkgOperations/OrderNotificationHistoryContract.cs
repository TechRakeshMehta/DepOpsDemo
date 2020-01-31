using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class OrderNotificationHistoryContract
    {
        public List<OrderNotificationDetail> OrderNotificationDetailList { get; set; }
        public List<LookupContract> StatusList { get; set; }
        public List<LookupContract> NotificationHistoryList { get; set; }
    }
    public class OrderNotificationDetail
    {
        public Int32 NotificationId { get; set; }
        public Int32 SystemCommunicationId { get; set; }
        public Int32 ServiceFormId { get; set; }
        public String NotificationType { get; set; }
        public Boolean NotificationAuto { get; set; }
        public DateTime CreatedDate { get; set; }
        public String SentBy { get; set; }
        public Int32 StatusId { get; set; }
        public Int32 SystemDocumentId { get; set; }
        public String NotificationTypeCode { get; set; }
        public Int32 OrderId { get; set; }
        public Int32 HierarchyNodeID { get; set; }
        public Int32 SvcGroupID { get; set; }
        public Int32 BkgPackageSvcGroupID { get; set; }
        public String NotificationDetail { get; set; }
        public Int32 OldStatusId { get; set; }
        public Int32 NewStatusId { get; set; }
        public String SvcGrpName { get; set; }
        public String OrderNumber { get; set; }
        //UAT-2156 :New Notification for students with Comm Copy setting for Form Dispatched (Manual Service Forms) .
        public String PackageName { get; set; }
        public String ServiceName { get; set; }
    }
}
