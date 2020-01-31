using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.TicketsCentre
{
    public class TicketIssueSrvcStepMapping
    {
        public Int32 TicketIssueSrvcStepMappingID { get; set; }
        public Int64 ClientSrvcStepMappingID { get; set; }
        public String ServcieStepName { get; set; }
    }
}
