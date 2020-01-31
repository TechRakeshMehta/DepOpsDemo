using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    [Serializable]
    public class ServiceFeeItemRecordContract
    {
        public Int32 SIFR_ID{get;set;}
        public Int32 SIFR_FeeItemID { get; set; }
        public String PSIF_Name { get; set; }
        public String FeeItemTypeCode { get; set; }
        public String FieldID { get; set; }
        public String FieldValue { get; set; }
        public Decimal? SIFR_Amount { get; set; }
        //UAT-3011
        public String State { get; set; }
        public string DisplayText { get { return FieldValue + '(' + SIFR_Amount + ')'; } }
    }
}
