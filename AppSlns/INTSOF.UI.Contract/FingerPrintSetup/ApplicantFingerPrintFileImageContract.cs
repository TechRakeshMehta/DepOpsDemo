using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace INTSOF.UI.Contract.FingerPrintSetup
{
    [Serializable]
    [DataContract]
    public class ApplicantFingerPrintFileImageContract
    {
        [DataMember]
        public Int32 AFFI_ID { get; set; }
        [DataMember]
        public Int32 AFFI_ApplicantAppointmentDetailID { get; set; }
        [DataMember]
        public String AFFI_FileName { get; set; }
        [DataMember]
        public String AFFI_FilePath { get; set; }
        [DataMember]
        public String OriginalFileName { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }
        [DataMember]
        public String LocationImages { get; set; }
    }
}
