using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class TenantDetailsContract
    {
        public Int32 TenantID { get; set; }
        public String TenantName { get; set; }
        public Int32? TenantTypeID { get; set; }
    }
}