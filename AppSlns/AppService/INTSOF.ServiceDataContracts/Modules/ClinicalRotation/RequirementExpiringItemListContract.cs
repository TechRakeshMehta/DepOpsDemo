using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class RequirementExpiringItemListContract
    {
        [DataMember]
        public Int32 ARID_RequirementItemID { get; set; }
        [DataMember]
        public String ItemRequirementStatus { get; set; }
        [DataMember]
        public Boolean ShowUpdateDelete { get; set; }
    }
}
