using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.AdminEntryPortal
{
    public class SendDraftOrderNotificationtoAdminContract
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string InstituteName { get; set; }
        public int BkgOrderID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? SelectedNodeID { get; set; }
        public int OrgUserId { get; set; }
        public int MasterOrderID { get; set; }
        public String OrderNumber { get; set; }


    }
}
