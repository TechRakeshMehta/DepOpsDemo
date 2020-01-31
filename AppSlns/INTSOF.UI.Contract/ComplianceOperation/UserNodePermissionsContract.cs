using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class UserNodePermissionsContract
    {
        public Int32 DPM_ID { get; set; }
        public Int32 PermissionID { get; set; }
        public Int32 VerificationPermissionID { get; set; }
        public Int32 ProfilePermissionID { get; set; }
        public String PermissionCode { get; set; }
        public String VerificationPermissionCode { get; set; }
        public String ProfilePermissionCode { get; set; }
        public Int32? ParentNodeID { get; set; }
    }
}
