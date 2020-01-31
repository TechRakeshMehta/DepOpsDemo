using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    public class SharedUserDashboardDetailsContract
    {
        public String SharedUserName { get; set; }
        public String AgencyName { get; set; }
        public String SharedUserInstitutions { get; set; }
        public String SharedUserEmail { get; set; }
        public String SharedUserPhone { get; set; }
        public String SharedUserBackgroundPermissions { get; set; }
        public String SharedUserCompliancePremissions { get; set; }
        public String SharedUserRequirementPermissions { get; set; }
        public String TenantName { get; set; }
    }
}
