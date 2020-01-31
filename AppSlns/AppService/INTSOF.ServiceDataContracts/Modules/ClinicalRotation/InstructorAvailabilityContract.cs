using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
// UAT: 4437
    [Serializable]
    [DataContract]
    public class InstructorAvailabilityContract
    {
        [DataMember]
        public Boolean InsAvailibility { get; set; }
        [DataMember]
        public Int32 RotationID { get; set; }
        [DataMember]
        public Boolean IsSchoolSendingInstructor { get; set; }

        [DataMember]
        public String ComplioID { get; set; }
        
    }
}
