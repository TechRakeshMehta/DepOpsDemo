using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.PlacementMatching
{
     [Serializable]
     [DataContract]
    public class InstitutionRequestPieChartContract
    {
         [DataMember]
         public Int32 TenantID { get; set; }
         [DataMember]
         public String TenantName { get; set; }
         [DataMember]
         public Int32 RecordsPercentage { get; set; }
         [DataMember]
         public String Color { get; set; }
    }
}
