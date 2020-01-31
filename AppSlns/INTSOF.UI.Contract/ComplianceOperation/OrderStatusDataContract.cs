using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    /// <summary>
    /// Class used as a Contract to pass status tyo be updated by UpdateOrderSts method
    /// </summary>
    public class OrderStatusDataContract
    {
        public Int32 StatusId { get; set; }
        public Decimal Amount { get; set; }
        public Int32 PaymentOptionId { get; set; }

        public List<OrderPkgPaymentDetail> lstPackages { get; set; }
    }
}
