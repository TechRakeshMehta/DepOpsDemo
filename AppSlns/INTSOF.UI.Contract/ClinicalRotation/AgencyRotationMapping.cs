using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ClinicalRotation
{
    public class AgencyRotationMapping
    {
        public int AgencyID { get; set; }

        public int TenantID { get; set; }

        public string RotationIds { get; set; }

        public string AgencyName { get; set; }

        public string TenantName { get; set; }
    }
}
