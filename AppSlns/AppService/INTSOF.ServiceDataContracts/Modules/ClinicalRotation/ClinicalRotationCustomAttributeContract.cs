using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class ClinicalRotationCustomAttributeContract
    {

        [DataMember]
        public Int32 RotationID { get; set; }
        [DataMember]
        public Int32 TenantID { get; set; }
        [DataMember]
        public String CustomAttributeList { get; set; }
    }
}
