using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class NodeNotificationSettingsContract
    {
        public Int32 NodeDeadlineID { get; set; }
        public String NodeDeadlineName { get; set; }
        public String NodeDeadlineDescription { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public Int32? Frequency { get; set; }
        public Int32? DaysBeforeDeadline { get; set; }
        public NodeNotificationMapping nodeNotificationMapping { get; set; }
    }
}
