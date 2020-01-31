using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class AgencyUserDashboardDetailsContract
    {
        public String AgencyUserName { get; set; }
        public String AgencyUserEmail { get; set; }
        public String AgencyUserPhone { get; set; }
        public String AgencyUserBackgroundPermissions { get; set; }
        public String AgencyUserCompliancePremissions { get; set; }
        public String AgencyUserRequirementPermissions { get; set; }
        public List<AgencyUserAgencyInstitutionContract> LstAgencyUserAgencyInstitutionContract { get; set; }
        public String AgencyHierarchyRootNode { get; set; }
    }
     [Serializable]
    public class AgencyUserAgencyInstitutionContract
    {
        public Int32 AgencyId { get; set; }
        public String AgencyName { get; set; }
        public String MappedInstitutions { get; set; }
    }

}
