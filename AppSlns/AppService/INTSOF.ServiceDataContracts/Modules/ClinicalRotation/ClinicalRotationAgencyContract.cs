using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    public class ClinicalRotationAgencyContract
    {
        public Int32 AgencyID { get; set; }

        public String AgencyName { get; set; }
    }
}
