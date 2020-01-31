using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.QueueManagement
{
    public class ItemReconciliationAvailiblityContract
    {
        public Int32 ItemID { get; set; }
        public Boolean IsSelected { get; set; }
        public Int32 ReviewerCount { get; set; }
    }
}

