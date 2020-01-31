using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class AgencyInstitutionsContract
    {
        public Int32 AgencyID { get; set; }
        public String AgencyName { get; set; }
        public List<AgencyUserTenantCmbContract> InstituteList { get; set; }
        public bool IsVerified { get; set; }
    }
}
