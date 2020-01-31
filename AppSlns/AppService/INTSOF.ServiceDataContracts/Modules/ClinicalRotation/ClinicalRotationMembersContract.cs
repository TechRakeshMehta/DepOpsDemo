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
    public class ClinicalRotationMembersContract
    {
        [DataMember]
        public Int32 RotationID { get; set; }

        [DataMember]
        public String RotationName { get; set; }

        [DataMember]
        public String ComplioID { get; set; }

        [DataMember]
        public Boolean IsApplicant { get; set; }

        [DataMember]
        public string ApplicantName { get; set; }

        [DataMember]
        public Boolean IsInstructor { get; set; }

        [DataMember]
        public String InstructorName { get; set; }

        [DataMember]
        public String UserName { get; set; }
    }
}
