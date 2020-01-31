using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace INTSOF.UI.Contract.FingerPrintSetup
{
    [Serializable]
    [DataContract]
    public class CDRFileDetailContract
    {
        [DataMember]
        public Int32 FileId { get; set; }
        [DataMember]
        public String FilePath { get; set; }
        [DataMember]
        public String FileName { get; set; }
        [DataMember]
        public DateTime? FileCreatedDate { get; set; }
        [DataMember]
        public Int64? FileFromID { get; set; }
        [DataMember]
        public Int64? FileToID { get; set; }     
    }
}
