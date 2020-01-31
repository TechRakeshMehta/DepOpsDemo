using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class PriceContract
    {
        public Int32 ID { get; set; }
        public Int32 PriceAdjustmentID { get; set; }
        public String PriceAdjustmentLabel { get; set; }
        public Decimal PriceAdjustmentValue { get; set; }

        public Decimal Price { get; set; }
        public Decimal TotalPrice { get; set; }
        public Decimal? RushOrderAdditionalPrice { get; set; }
        public Boolean IsPriceNull { get; set; }

        public Int32 NewID { get; set; }
        public Int32 NewParentID { get; set; }
    }
}
