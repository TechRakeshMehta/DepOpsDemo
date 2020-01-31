using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    public class AgencyUserPermissionContract
    {
        public String UserID { get; set; }

        public Boolean IsActive { get; set; }

        public Boolean IsLocked { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }
    }
}
