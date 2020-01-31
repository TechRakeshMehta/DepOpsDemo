using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.TicketsCentre
{
    [Serializable]
    public class TicketIssueSrvcStepMappingContract
    {
        public Int64 TicketIssueSrvcStepMappingID { get; set; }
        public Int64 ClientSrvcStepMappingID { get; set; }
        public String ServcieStepName { get; set; }
        public Int64 TicketIssueID { get; set; }
    }
}
