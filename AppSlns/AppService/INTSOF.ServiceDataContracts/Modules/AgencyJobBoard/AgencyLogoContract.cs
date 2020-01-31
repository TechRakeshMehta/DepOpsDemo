using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.AgencyJobBoard
{
    [Serializable]
    [DataContract]
    public class AgencyLogoContract
    {
        [DataMember]
        public Int32 AgencyLogoID { get; set; }

        [DataMember]
        public Int32 AgencyHierarchyID { get; set; }

        [DataMember]
        public String LogoPath { get; set; }
    }
}
