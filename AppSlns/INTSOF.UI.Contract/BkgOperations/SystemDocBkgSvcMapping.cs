using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    [DataContract]
    public class SystemDocBkgSvcMapping
    {
        [DataMember]
        public Int32 SystemDocumentID { get; set; }
        [DataMember]
        public Int32 BkgServiceID { get; set; }
        [DataMember]
        public Int32 ExtServiceID { get; set; }
        [DataMember]
        public Int32 BackgroundPackageID { get; set; }
        [DataMember]
        public Int32 ApplicantDocumentID { get; set; }
        [DataMember]
        public Int16 RecordTypeID { get; set; }
        [DataMember]
        public String BkgServiceName { get; set; }
        [DataMember]
        public String FileName { get; set; }
        [DataMember]
        public String Description { get; set; }
    }
}
