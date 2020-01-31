using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.AdminEntryPortal
{
    public class AutoArchivedTimeLineDays
    {
        public int BkgAdminEntryOrderId { get; set; }
        public int Days { get; set; }
        public int OrderStatusId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Int32 BkgOrderId { get; set; }
        public string UserName { get; set; }
        public string OrderCompletedDate { get; set; }
        public string InstitutionHierarchyName { get; set; }

        public int? SelectedNodeID { get; set; }

        public int OrgUserId { get; set; }
        public int OrderId { get; set; }
        public String OrderNumber { get; set; }
    }
}
