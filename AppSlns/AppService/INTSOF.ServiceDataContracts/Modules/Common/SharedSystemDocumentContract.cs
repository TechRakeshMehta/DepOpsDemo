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
    public class SharedSystemDocumentContract
    {
        [DataMember]
        public Int32 DocumentID { get; set; }
        [DataMember]
        public Int32 TempDocumentID { get; set; }
        [DataMember]
        public String FileName { get; set; }
        [DataMember]
        public Int32 FileSize { get; set; }
        [DataMember]
        public String DocumentPath { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public Int32 DocumentTypeID { get; set; }
        [DataMember]
        public String DocumentTypeName { get; set; }
    }
}
