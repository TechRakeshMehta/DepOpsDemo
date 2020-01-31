using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.RotationPackages
{
    public class RequirementItemsAboutToExpireContract
    {
        public String UserFirstName { get; set; }
        public String UserLastName { get; set; }
        public String PrimaryEmailaddress { get; set; }
        public String RequirementItemName { get; set; }
        public DateTime? ItemExpirationDate { get; set; }
        public Int32 RequirementItemID { get; set; }
        public Int32 ApplicantRequirementItemID { get; set; }
        public Int32 OrgUserId { get; set; }
        public String RotationHierachyIds { get; set; }
        public String ComplioID { get; set; }  //UAT:4619
    }
}
