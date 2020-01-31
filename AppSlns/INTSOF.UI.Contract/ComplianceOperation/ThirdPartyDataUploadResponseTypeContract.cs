using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
   public class ThirdPartyDataUploadResponseTypeContract
    {
        public Int32 ThirdPartyDataUploadResponseID { get; set; }
        public String Regex { get; set; }
        public String ThirdPartyUploadOutputTypeCode { get; set; }

    }
}
