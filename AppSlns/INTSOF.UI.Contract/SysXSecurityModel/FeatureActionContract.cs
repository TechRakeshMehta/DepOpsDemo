using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SysXSecurityModel
{
    public class FeatureActionContract
    {
        public String ParentId { get; set; }
        public String NodeId { get; set; }
        public String Name { get; set; } // treated as value
        public String Code{ get; set; }
        public String SelectedPermission { get; set; } //treated as permission
        public Int32 RolePermissionProductFeatueID { get;set; }

        public String ParentNodeID { get; set; }
        public String ParentDataID { get; set; }
        public String DataID { get; set; }
    }
}
