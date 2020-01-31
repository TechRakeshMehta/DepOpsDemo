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
    public class OrderDetailContract
    {
        public int DetailExtID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceStatus { get; set; }
        public string OrderNumber { get; set; }
        public bool IsMailing { get; set; }
        public int OrderID { get; set; }
        public string TrackingNumber { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }
        public int? PPQuantity { get; set; }
        public decimal? FCAdditionalPrice { get; set; }
        public decimal? PPAdditionalPrice { get; set; }


    }
}
