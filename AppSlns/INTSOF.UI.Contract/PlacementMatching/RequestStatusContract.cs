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
    public class RequestStatusContract
    {
        [DataMember]
        public int StatusID { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Code { get; set; }
        [DataMember]
        public Int32 Count { get; set; }

    }
}

