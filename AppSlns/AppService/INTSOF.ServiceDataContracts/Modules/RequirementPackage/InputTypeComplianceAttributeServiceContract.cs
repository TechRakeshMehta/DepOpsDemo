using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class InputTypeComplianceAttributeServiceContract
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public Int32 ID { get; set; }
        [DataMember]
        public Int32 InputPriority { get; set; }
        [DataMember]
        public Boolean Enabled { get; set; }
    }
}
