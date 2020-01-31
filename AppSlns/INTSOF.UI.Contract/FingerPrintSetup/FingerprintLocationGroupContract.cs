using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.FingerPrintSetup
{
    [Serializable]
    [DataContract]
    public class FingerprintLocationGroupContract
    {
        [DataMember]
        public Int32 LocationGroupID { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public String NameFilter { get; set; }
        [DataMember]
        public String DescriptionFilter { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }
        [DataMember]
        public Boolean IsAssigned { get; set; }        
    }
}
