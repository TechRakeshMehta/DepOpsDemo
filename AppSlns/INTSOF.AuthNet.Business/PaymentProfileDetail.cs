using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.AuthNet.Business
{
    public class PaymentProfileDetail
    {
        public long CustomerPaymentProfileId { get; set; }
        public String CardNumber { get; set; }
        public String CardType { get; set; }
        public String Expirydate { get; set; }
        public String NameOnCard { get; set; }
        public String CardTypeAndNumber { get; set; }
    }
}
