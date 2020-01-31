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
    public class ApplicantRequirementFieldDataContract
    {
        [DataMember]
        public Int32 RequirementItemDataID { get; set; }
        [DataMember]
        public Int32 RequirementFieldID { get; set; }
        [DataMember]
        public String FieldValue { get; set; }
        [DataMember]
        public String FieldValueViewDoc { get; set; }
        [DataMember]
        public Nullable<Int32> FieldMaxLength { get; set; }
        [DataMember]
        public Int32 ApplicantReqFieldDataID { get; set; }
        [DataMember]
        public String FieldDataTypeCode { get; set; }

        [DataMember]
        public Int32 RequirementFieldDisplayOrder { get; set; }

        [DataMember]
        public List<ApplicantFieldDocumentMappingContract> LstApplicantFieldDocumentMapping { get; set; }

        [DataMember]
        public byte[] Signature { get; set; }
    }
}
