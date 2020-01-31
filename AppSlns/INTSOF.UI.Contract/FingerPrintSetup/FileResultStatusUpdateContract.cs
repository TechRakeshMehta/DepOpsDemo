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
    public class FileResultStatusUpdateContract
    {
        [DataMember]
        public Int32 AppointmentDetailID { get; set; }
        [DataMember]
        public Int32 UserId { get; set; }
        [DataMember]
        public String PCNNumber { get; set; }
        [DataMember]
        public String Result { get; set; }
        [DataMember]
        public String ApplicantName { get; set; }
        [DataMember]
        public String OrderNumber { get; set; }
        [DataMember]
        public Int32 CBIFileID { get; set; }
        [DataMember]
        public String UserEmailId { get; set; }
        [DataMember]
        public Int32 HierarchyNodeId { get; set; }
        [DataMember]
        public Boolean IsOutofStateOrder { get; set; }
        [DataMember]
        public Decimal StateFees { get; set; }
        [DataMember]
        public Decimal FBIFees { get; set; }
        [DataMember]
        public String Extention { get; set; }
        [DataMember]
        public String AppointmentStatus { get; set; } 
        [DataMember]
        public Int32 OrderID { get; set; }
        [DataMember]
        public Boolean isFbiErrSubmit { get; set; }
        [DataMember]
        public Boolean isFbisuccess { get; set; }
        [DataMember]
        public Boolean isCbiSuccess { get; set; }
        [DataMember]
        public String finalresult { get; set; }
        [DataMember]
        public Boolean IsStateFBIApplicable
        {
            get
            {
                return this.FBIFees > 0 && this.StateFees > 0;
            }
        }
        [DataMember]
        public Boolean IsStateApplicable
        {
            get
            {
                return this.StateFees > 0 && this.FBIFees == 0;
            }
        }
        [DataMember]
        public Boolean IsDataError { get; set; }
    }
}
