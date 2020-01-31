using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.Common
{

    /// <summary>
    /// Class to represent the Contract for lkpAgencySearchStatus in SharedData
    /// </summary>
    [DataContract]
    public class AgencySearchStatusContract
    {
        [DataMember]
        public String SearchStatusName { get; set; }

        [DataMember]
        public String SearchStatusCode { get; set; }
    }
}
