using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.RotationPackages
{
    public class RequirementPkgSubscriptionStatusContract
    {
        public Int32 RequirementPackageSubscriptionID { get; set; }       
        public string RequirementCategoryStatusCode { get; set; }      
        public string RotationName { get; set; }      
        public string PackageName { get; set; }      
        public string ApplicantName { get; set; }       
        public string UserName { get; set; }       
        public string Email { get; set; }      
        public Int32 OrganizationUserID { get; set; }
    }
}
