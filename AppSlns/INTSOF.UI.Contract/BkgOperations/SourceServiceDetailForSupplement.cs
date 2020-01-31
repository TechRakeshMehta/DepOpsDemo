using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class SourceServiceDetailForSupplement
    {
        public Boolean IfSSNServiceExist { get; set; }
        public Boolean IfNationalCriminalServiceExist { get; set; }
        public String SSNServiceResult { get; set; }
        public String NationalCriminalServiceResult { get; set; }
    }
}
