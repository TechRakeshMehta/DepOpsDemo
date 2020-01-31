using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.Common
{
    [Serializable]
    [DataContract]
   public class MultipleAdditionalDocumentsContract
    {
        [DataMember]
        public String AdditionalDocumentFileName { get; set; }

        [DataMember]
        public String AdditionalDocumentFilePath { get; set; }

        [DataMember]
        public Int32? AdditionalDocumentFileSize { get; set; }

        [DataMember]
        public Boolean IfAdditionalDocumentFileRemoved { get; set; }
        [DataMember]
        public int AdditionalDocumentID { get; set; }

       
 
    }
}
