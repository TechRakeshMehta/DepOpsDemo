using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class TrackingAssignmentConfigurationContract
    {
        public Int32 TAC_ID { get; set; }
        public Int32 AdminId { get; set; }
        public String AdminFirstName { get; set; }
        public String AdminLastName { get; set; }
        public Int32 AssignmentCount { get; set; }
        public DateTime? DateFrom { get; set; } // Changed to null UAT-3075
        public DateTime? DateTo { get; set; } // Changed to null UAT-3075
        //UAT-3075
        public Int32? DaysFrom { get; set; } // Needed to change from nullable to Not nullable
        public Int32? DaysTo { get; set; }
        public List<TrackingConfigObjectMappingContract> lstConfigObjMapping { get; set; }
        public String allObjectsName { get; set; }
    }
}
