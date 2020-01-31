using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.QueueManagement
{
   public  class QueueAuditRecordContract
    {
       public Int32 RecordID { get; set; }
       public Int32 QueueRecordAuditDataID { get; set; }
       public Int32 QueueID { get; set; }
       public String UserName { get; set; }
       public DateTime? CreatedOn { get; set; }
       public String QueueRecord { get; set; }
       public String PrevLifecycleFieldValue { get; set; }
       public String AttemptedLifecycleFieldValue { get; set; }
       public String ActualLifecycleFieldValue { get; set; }
       public Int32 TotalCount { get; set; }
       public String IsActive { get; set; }
       public String QueueName { get; set; }
    }
}
