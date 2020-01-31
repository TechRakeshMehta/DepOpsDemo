using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClientContact
{
    [Serializable]
    [DataContract]
    public class ClientContactTypeContract
    {
        [DataMember]
        public Int32 ClientContactTypeID { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Code { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public Boolean IsDeleted { get; set; }
    }
}
