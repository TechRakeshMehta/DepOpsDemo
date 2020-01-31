using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ScheduleTask
{
    public class ScheduleTaskContract
    {
        public Int32 ScheduleTaskID { get; set; }
        public Int32 TaskTypeID { get; set; }
        public Int32 TaskStatusID { get; set; }
        public Int32 RecordID { get; set; }
        public String TaskXMLParameters { get; set; }
        public String TaskTypeCode { get; set; }
        public Boolean IsRecurring { get; set; }
        public String TaskGroup { get; set; }
        public String StatusTypeCode { get; set; }
        public Boolean IsApprovalEmailSent { get; set; }
        
    }
}
