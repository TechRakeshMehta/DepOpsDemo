using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.Common
{
    /// <summary>
    /// DataContract for 'lkpRequirementItemStatus' entity
    /// </summary>
    [DataContract]
    public class RequirementItemStatusContract
    {
        [DataMember]
        public Int32 ReqItemstatusId { get; set; }

        [DataMember]
        public String ReqItemstatusName { get; set; }

        [DataMember]
        public String ReqItemstatusCode { get; set; }
    }
}
