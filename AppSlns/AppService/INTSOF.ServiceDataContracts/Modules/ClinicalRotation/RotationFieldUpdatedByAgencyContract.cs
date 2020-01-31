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
    public class RotationFieldUpdatedByAgencyContract
    {
        [DataMember]
        public Int32 ClinicalRotationID { get; set; }
        [DataMember]
        public Boolean IsStartDateUpdated { get; set; }
        [DataMember]
        public Boolean IsEndDateUpdated { get; set; }
        [DataMember]
        public Boolean IsDepartmentUpdated { get; set; }

        [DataMember]
        public Boolean IsProgramUpdated { get; set; }

        [DataMember]
        public Boolean IsCourseUpdated { get; set; }

        [DataMember]
        public Boolean IsUnitFloorLocUpdated { get; set; }

        [DataMember]
        public Boolean IsNoOfHoursUpdated { get; set; }

        [DataMember]
        public Boolean IsRotationShiftUpdated { get; set; }

        [DataMember]
        public Boolean IsRotationNameUpadted { get; set; }

        [DataMember]
        public Boolean IsTermUpdated { get; set; }

        [DataMember]
        public Boolean IsTypeSpecialtyUpdated { get; set; }

        [DataMember]
        public Boolean IsNoOfStudentsUpdated { get; set; }
    }
}
