using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.TicketsCentre
{
    [Serializable]
    public class TicketSearchContract
    {
        public Int16 TicketTypeID { get; set; }
        public Int16 TicketStatusID { get; set; }
        public Int16 TicketSeverityID { get; set; }
        public String LocationIDs { get; set; }
        public Boolean IsEnroller { get; set; }
    }
}
