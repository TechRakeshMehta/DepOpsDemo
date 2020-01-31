using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class BatchRotationUploadContract
    {
        [DataMember]
        public Int32 InstitutionNodeID { get; set; }

        [DataMember]
        public String Agency { get; set; }

        [DataMember]
        public String Rotation_Name { get; set; }

        [DataMember]
        public String Rotation_Review_Status { get; set; }

        [DataMember]
        public String Type_Specialty { get; set; }

        [DataMember]
        public String Department { get; set; }

        [DataMember]
        public String Program { get; set; }

        [DataMember]
        public String Course { get; set; }

        [DataMember]
        public String Term { get; set; }

        [DataMember]
        public String Unit_Floor { get; set; }

        [DataMember]
        public String Students { get; set; }

        [DataMember]
        public String Recommended_Hours { get; set; }

        [DataMember]
        public String Days { get; set; }

        [DataMember]
        public String Shift { get; set; }

        [DataMember]
        public String Time { get; set; }

        [DataMember]
        public DateTime? StartDate { get; set; }

        [DataMember]
        public DateTime? EndDate { get; set; }

        [DataMember]
        public String Instructor_Preceptor { get; set; }

        [DataMember]
        public String Upload_Status { get; set; }

        [DataMember]
        public Int32 TotalCount { get; set; }

        [DataMember]
        public String BatchRotationErrorMessage { get; set; }

        [DataMember]
        public DateTime ? CreatedOn { get; set; }

    }
}
