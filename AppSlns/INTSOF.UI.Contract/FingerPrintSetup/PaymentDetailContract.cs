using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.FingerPrintSetup
{
    [Serializable]
    [DataContract]
    public class PaymentDetailContract
    {
        public int OrderPaymentDetailID { get; set; }
        public decimal? OrderPaymentInvoiceItemAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentStatusCode { get; set; }
        public string PaymentType { get; set; }
        public string PaymentOptionCode { get; set; }
    }
}
