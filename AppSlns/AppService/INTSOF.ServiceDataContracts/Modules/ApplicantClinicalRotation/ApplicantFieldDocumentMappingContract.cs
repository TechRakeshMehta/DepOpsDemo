using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation
{
    [Serializable]
    [DataContract]
    public class ApplicantFieldDocumentMappingContract
    {
        [DataMember]
        public Int32 ApplicantRequirementDocumentMapId { get; set; }

        [DataMember]
        public Int32 ApplicantDocumentId { get; set; }

        [DataMember]
        public Int32 ApplicantReqFieldDataId { get; set; }

        [DataMember]
        public String DocumentPath { get; set; }

        [DataMember]
        public String FileName { get; set; }

        [DataMember]
        public String Description { get; set; }

        [DataMember]
        public String DocumentType { get; set; }

        //Start UAT-4900
        [DataMember]
        public Boolean IsDisabled { get; set; }
        //End UAT-4900
    }
}
