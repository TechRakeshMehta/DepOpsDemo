using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class OrderEventHistoryContract
    {
        public Int32 BOEH_ID { get; set; }
        public String BOEH_CreatedOn { get; set; }
        public String BOEH_OrderEventDetail { get; set; }
        public String BOEH_FullName { get; set; }
        //public String BOEH_FirstName { get; set; }
        //public String BOEH_MiddleName { get; set; }
        //public String BOEH_LastName { get; set; }

    }
}
