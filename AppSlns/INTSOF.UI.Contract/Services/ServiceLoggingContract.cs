using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.Services
{
    public class ServiceLoggingContract
    {
        public String ServiceName { get; set; }
        public String JobName { get; set; }
        public Int32 TenantID { get; set; }
        public Int32 ClientDataUploadId { get; set; }
        public DateTime JobStartTime { get; set; }
        public DateTime JobEndTime { get; set; }
        public String Comments { get; set; }
        public Boolean IsDeleted { get; set; }
        public Int32 CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
