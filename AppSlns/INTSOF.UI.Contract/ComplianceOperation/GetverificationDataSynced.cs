using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
   public class GetverificationDataSynced
    {
       public Int32 TenantId { get; set; }
       public ManualResetEvent FinishManualResetEvent { get; set; }
       public Semaphore Semaphore { get; set; }
    }
}
