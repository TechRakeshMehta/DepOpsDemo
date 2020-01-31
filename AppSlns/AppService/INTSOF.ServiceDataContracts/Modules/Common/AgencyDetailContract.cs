using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.Common
{
    [Serializable]
    [DataContract]
    public class AgencyDetailContract
    {
        [DataMember]
        public Int32 AgencyID { get; set; }
        [DataMember]
        public String AgencyName { get; set; }
        [DataMember]
        public String AgencyDescription { get; set; }
    }
}
