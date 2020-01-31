//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    [Serializable]
    public class UserGroupContract
    {
        public Int32? UserGroupId { get; set; }
        public String UserGroupName { get; set; }
        public String UserGroupDataId { get; set; }
        public String UserGroupType { get; set; }
    }
}
