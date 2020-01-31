using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    public class AgencyUserInfoContract
    {
        public int AgencyID { get; set; }
        public int AgencyUserID { get; set; }
        public int OrgUserID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String FullName { get; set; }
        public String PrimaryEmailAddress { get; set; }
    }
}
