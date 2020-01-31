using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.PlacementMatching
{
    public class PlacementSearchContract
    {
        public String DepartmentId { get; set; }
        public String LocationId { get; set; }
        public String StudentTypeIds { get; set; }
        public String SpecialtyId { get; set; }
        public String StatusIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public String Max { get; set; }
        public Int32 TenantId { get; set; }
        public String Shift { get; set; }
        public String Days { get; set; }
        public Int32 AgencyHierarchyID { get; set; }
        public String StatusCode { get; set; }
        public String SharedCustomAttributes { get; set; }
    }
}
