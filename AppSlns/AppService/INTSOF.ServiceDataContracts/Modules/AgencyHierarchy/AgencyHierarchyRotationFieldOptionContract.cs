using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.AgencyHierarchy
{
    [Serializable]
    [DataContract]
    public class AgencyHierarchyRotationFieldOptionContract
    {
        [DataMember]
        public Int32 CurrentLoggedInUser { get; set; }

        [DataMember]
        public Int32 AgencyHierarchyID { get; set; }

        [DataMember]
        public Boolean AHRFO_CheckParentSetting { get; set; }

        [DataMember]
        public Boolean IsRootNode { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsRotationName_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsTypeSpecialty_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsDepartment_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsProgram_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsCourse_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsTerm_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsUnitFloorLoc_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsNoOfStudents_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsNoOfHours_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsRotDays_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsRotationShift_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsIP_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsStartTime_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsEndTime_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsSyllabusDocument_Required { get; set; }


        [DataMember]
        public Nullable<Boolean> AHRFO_IsAdditionalDocuments_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsDaysBefore_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsFrequency_Required { get; set; }

        [DataMember]
        public Nullable<Boolean> AHRFO_IsDeadlineDate_Required { get; set; }
    }
}
