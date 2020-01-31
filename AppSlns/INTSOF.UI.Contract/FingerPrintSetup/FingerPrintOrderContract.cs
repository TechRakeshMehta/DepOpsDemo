using Entity.ClientEntity;
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
    public class FingerPrintOrderContract
    {
        [DataMember]
        public Int32 OrderId { get; set; }
        [DataMember]
        public String OrderNumber { get; set; }
        [DataMember]
        public String OrderResultXML { get; set; }
        [DataMember]
        public Int32 ApplicantOrgUserID { get; set; }
        [DataMember]
        public Int32 BkgOrderId { get; set; }
        [DataMember]
        public Int32 BkgOrderStatusId { get; set; }
        [DataMember]
        public String BkgOrderStatusCode { get; set; }
        [DataMember]
        public ApplicantDocument ApplicantDocument { get; set; }


        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public String Extension { get; set; }
        [DataMember]
        public String Result { get; set; }
        [DataMember]
        public string ResultStatus { get; set; }
        [DataMember]
        public string FileContent { get; set; }
        [DataMember]
        public string PCNNumber { get; set; }
        [DataMember]
        public int AADID { get; set; }
        [DataMember]
        public String RejectionReason { get; set; }
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public int ModifiedBy { get; set; }
        [DataMember]
        public int CBI_TentantId { get; set; }
        [DataMember]
        public Boolean SkipABIReview { get; set; }
        [DataMember]
        public List<ApplicantFingerPrintFileImageContract> ApplicantFingerPrintFileImages { get; set; }
    }
}
