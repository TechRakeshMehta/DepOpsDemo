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
   public class CBIBillingStatusContract
   {


       [DataMember]
       public Int32 TenantId { get; set; }
      
        [DataMember]
        public Int32 CurrentPageIndex { get; set; }
        [DataMember]
        public Int32 CurrentPageSize { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }
        [DataMember]
        public string CBIUniqueID { get; set; }
        [DataMember]
        public string BillingCode { get; set; }
        [DataMember]
        public Boolean IsEnabled { get; set; }
        [DataMember]
        public string AccountAddress { get; set; }
        [DataMember]
        public string AccountCity { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public string AccountState { get; set; }
        [DataMember]
        public string AccountZIP { get; set; }
       //UAT-3850
        [DataMember]
        public Decimal Amount { get; set; }

   }
}
