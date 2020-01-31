using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.RotationPackages
{
    public class RequirementCategoriesBeforeGoingToBeRequiredContract
    {
        public String UserFirstName { get; set; }
        public String UserLastName { get; set; }
        public String PrimaryEmailaddress { get; set; }
        public String RequirementCategoryName { get; set; }
        public DateTime? CategoryRequiredDate { get; set; }
        public Int32 RequirementCategoryID { get; set; }
        public Int32 ApplicantRequirementCategoryID { get; set; }
        public Int32 OrgUserId { get; set; }
        public String RotationHierachyIds { get; set; }
        public String ComplioID { get; set; }
        public String RotationName { get; set; }
    }
}
