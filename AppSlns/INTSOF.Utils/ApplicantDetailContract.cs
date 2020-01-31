using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.Utils
{
    [Serializable]
    public class ApplicantDetailContract
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime? DOB { get; set; }
        public String Email { get; set; }
        public Int32? PackageID { get; set; }
        public Int32? OrderNodeID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Int32? Interval { get; set; }

    }
}
