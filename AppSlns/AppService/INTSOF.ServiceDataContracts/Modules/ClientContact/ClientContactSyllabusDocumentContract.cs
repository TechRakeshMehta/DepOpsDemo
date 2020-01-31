using INTSOF.ServiceDataContracts.Modules.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClientContact
{
    [Serializable]
    [DataContract]
    public class ClientContactSyllabusDocumentContract
    {
        [DataMember]
        public Int32 DocumentID { get; set; }
        [DataMember]
        public String ComplioID { get; set; }
        [DataMember]
        public String RotationName { get; set; }
        [DataMember]
        public String Department { get; set; }
        [DataMember]
        public String Program { get; set; }
        [DataMember]
        public String Course { get; set; }
        [DataMember]
        public String FileName { get; set; }
        [DataMember]
        public Int32 ClinicalRotationID { get; set; }

        [DataMember]
        public List<MultipleAdditionalDocumentsContract> listMultipleDocuments { get; set; }

    }
}
