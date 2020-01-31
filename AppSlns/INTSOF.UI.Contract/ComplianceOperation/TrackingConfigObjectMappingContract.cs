using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class TrackingConfigObjectMappingContract
    {
        public Int32 TCOM_ID { get; set; }
        public Int32 TCOM_ConfigurationID { get; set; }
        public Int32 TCOM_ComplianceObjectID { get; set; }
        public Int32? TCOM_Priority { get; set; }
        public String ObjectName { get; set; }
    }
}
