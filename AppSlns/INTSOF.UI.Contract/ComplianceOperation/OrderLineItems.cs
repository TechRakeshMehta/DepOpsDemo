using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    [DataContract]
    public class OrderLineItem
    {
        [DataMember]
        public String OrderName { get; set; }

        [DataMember]
        public Int32? Quantity { get; set; }

        [DataMember]
        public Decimal? Price { get; set; }

        [DataMember]
        public Decimal? Amount { get; set; }

        [DataMember]
        public Int32? PPQuantity { get; set; }

        [DataMember]
        public Decimal? FCAdditionalPrice { get; set; }

        [DataMember]
        public Decimal? PPAdditionalPrice { get; set; }
        public String ServiceCode {get;set;}

    }      
}

