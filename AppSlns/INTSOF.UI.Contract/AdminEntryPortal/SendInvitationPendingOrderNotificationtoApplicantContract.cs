using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.AdminEntryPortal
{
    public class SendInvitationPendingOrderNotificationtoApplicantContract
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string InstituteURL { get; set; }
        public string InstituteName { get; set; }
        public string hierarchyNodeName { get; set; }
        public string Email { get; set; }
        public int? SelectedNodeID { get; set; }
        public int OrgUserId { get; set; }
        public int BkgOrderID { get; set; }
        public DateTime CreatedDate { get; set; }
        public String AdminEntryInvitationToken { get; set; }

    }

}
