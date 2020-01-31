using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
   public class BkgPackageTypeContract
    {
        public Int32 BkgPackageTypeId { get; set; }
        //public Int32 BkgPackageType_TenantId { get; set; }
        public String BkgPackageTypeName { get; set; }
        public String BkgPackageTypeCode { get; set; }
        public String BkgPackageTypeColorCode { get; set; }  
      
    }
}
