using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public class ADBDatabaseDetail
    {
        public enum ADBDatabaseType
        {
            [StringValue("SECURITY_DB")]
            SECURITY_DB,
            [StringValue("MESSAGING_DB")]
            MESSAGING_DB,
            [StringValue("SHAREDDATA_DB")]
            SHAREDDATA_DB,
            [StringValue("TENANT_DB")]
            TENANT_DB       
        }        
    }
}
