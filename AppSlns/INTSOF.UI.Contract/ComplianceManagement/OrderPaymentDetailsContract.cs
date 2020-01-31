using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class OrderPaymentDetailsContract
    {
        public Int32 OPD_ID { get; set; }
        public Int32? OPD_OrderID { get; set; }
        public Int32? OPD_OnlinePaymentTransactionID { get; set; }
        public String OPD_MoneyOrderNumber { get; set; }
        public String OPD_BankName { get; set; }
        public String OPD_RoutingInfo { get; set; }
        public String OPD_AccountNumber { get; set; }
        public String OPD_ThirdPartyName { get; set; }
    }
}
