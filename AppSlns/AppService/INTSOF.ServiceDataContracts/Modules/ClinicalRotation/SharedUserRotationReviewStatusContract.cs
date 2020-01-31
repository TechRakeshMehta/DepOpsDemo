using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class SharedUserRotationReviewStatusContract
    {
        [DataMember]
        public Int32 RotationReviewStatusID { get; set; }
        [DataMember]
        public String Code { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public Boolean IsDeleted { get; set; }
    }
}
