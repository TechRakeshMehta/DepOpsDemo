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
    public class WeekDayContract
    {
        [DataMember]
        public Int32 WeekDayID { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Code { get; set; }
        [DataMember]
        public String Description { get; set; }
    }
}
