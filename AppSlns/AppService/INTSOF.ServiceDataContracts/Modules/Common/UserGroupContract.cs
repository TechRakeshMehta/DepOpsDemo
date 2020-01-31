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
    public class UserGroupContract
    {
        [DataMember]
        public Int32 UG_ID { get; set; }
        [DataMember]
        public String UG_Name { get; set; }
        [DataMember]
        public String UG_Description { get; set; }
    }
}
