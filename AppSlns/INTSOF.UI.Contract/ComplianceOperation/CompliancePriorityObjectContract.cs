using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    public class CompliancePriorityObjectContract
    {
        public Int32 CPO_ID { get; set; }
        public String CPO_Name { get; set; }
        public String CPO_Description { get; set; }

        public Int32 CCIPOM_ID { get; set; }
        public Int32 CategoryID { get; set; }
        public String CategoryName { get; set; }
        public String ItemName { get; set; }
        public Int32? ItemID { get; set; }
        //public String TenantName { get; set; }
        //public Int32 TenantID { get; set; }
       // public String TenantName { get; set; }
    }
}
