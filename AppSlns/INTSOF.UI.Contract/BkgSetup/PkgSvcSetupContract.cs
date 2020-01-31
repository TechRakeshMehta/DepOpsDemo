using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    [Serializable]
    public class PkgSvcSetupContract
    {
        public Int32 DataId { get; set; }
        public Int32? ParentDataId { get; set; }
        public String Name { get; set; }
        public String NodeId { get; set; }
        public String ParentNodeId { get; set; }
        public String Code { get; set; }
        public Int32 Level { get; set; }
        public Int32 ATTRDisplayOrder { get; set; }
        //public String ColorCode { get; set; }
        
    }
}
