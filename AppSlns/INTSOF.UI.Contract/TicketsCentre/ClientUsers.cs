using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.TicketsCentre
{
    [Serializable]
    public class ClientUsers
    {
        public Int32 UserId { get; set; }
        public Int64 TicketIssueNotificationUserID { get; set; }
        public String UserName { get; set; }
    }
}
