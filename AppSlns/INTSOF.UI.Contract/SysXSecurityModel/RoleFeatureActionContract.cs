using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SysXSecurityModel
{
    [Serializable]
    public class RoleFeatureActionContract
    {
        public Int32 SysXBlockFeatureID
        {
            get;
            set;
        }

        public Int64 FeatureActionID
        {
            get;
            set;
        }

        public Int32 PermissionID
        {
            get;
            set;
        }

    }
}
