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
    public class ApplicantDocumentContract
    {
        [DataMember]
        public Int32 ApplicantDocumentId { get; set; }

        [DataMember]
        public String DocumentPath { get; set; }

        [DataMember]
        public String FileName { get; set; }

        [DataMember]
        public String Description { get; set; }

        [DataMember]
        public String DocumentType { get; set; }
        
        [DataMember]
        public Int32? Size { get; set; }

        [DataMember]
        public String OriginalDocMD5Hash { get; set; }

        [DataMember]
        public String DataEntryDocumentStatusCode { get; set; }

        [DataMember]
        public bool IsSignatureRequired { get; set; }

        [DataMember]
        public bool IsRequiredToView { get; set; }

        [DataMember]
        public Int32 ApplicantId { get; set; }

        [DataMember]
        public String ApplicantName { get; set; }

    }
}
