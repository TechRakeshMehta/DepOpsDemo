using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    public class RotationsMappedToAgenciesContract
    {
        public Boolean IsRotationAgencyCountMatched { get; set; }

        public String AgencyIDs { get; set; }
    }
}
