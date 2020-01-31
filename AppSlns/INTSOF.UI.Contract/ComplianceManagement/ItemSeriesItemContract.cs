using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ItemSeriesItemContract
    {
        public Int32 ComplianceCategoryID { get; set; }
        public Int32 ItemSeriesID { get; set; }
        public List<Int32> ComplianceItemID { get; set; }
        public Boolean IsSeriesAvailablePostApproval{ get; set; }
    }
}
