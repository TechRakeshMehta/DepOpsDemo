using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.QueueManagement
{
    [Serializable]
    public class ManageRandomReviewsContract
    {
        public Int32 ReconciliationQueueConfigurationID { get; set; }

        public Int32 TenantID { get; set; }

        public Int32 HierarchyNodeID { get; set; }

        public String InstitutionHierarchy { get; set; }

        public String TenantName { get; set; }

        public Int32 Reviews { get; set; }

        public Decimal? Percentage { get; set; }
        
        public String DisplayPercentage { get; set; }

        public String Description { get; set; }
    }
}
