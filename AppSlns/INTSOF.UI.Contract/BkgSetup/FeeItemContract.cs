using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    public class FeeItemContract
    {
        public String FeeItemName { get; set; }
        public String FeeItemDescription { get; set; }
        public String FeeItemLabel { get; set; }
        public Int32 FeeItemTypeId { get; set; }
        public Decimal? FixedFeeAmount { get; set; }


    }
}
