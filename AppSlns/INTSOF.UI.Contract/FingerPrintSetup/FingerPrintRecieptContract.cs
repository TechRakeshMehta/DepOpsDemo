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
    public class FingerPrintRecieptContract
    {
        [DataMember]
        public String ApplicantName { get; set; }
        [DataMember]
        public String OrderNumber { get; set; }
        [DataMember]
        public String PCNNumber { get; set; }
        [DataMember]
        public String UserEmail { get; set; }
        [DataMember]
        public Int32 UserId { get; set; }
        [DataMember]
        public Int32? RecieptId { get; set; }
        [DataMember]
        public Int32 AppointmentId { get; set; }
        [DataMember]
        public Int32 HierarchyNodeID { get; set; }
        [DataMember]
        public Int32 DocumentID { get; set; }
        [DataMember]
        public String DocFileName { get; set; }
        [DataMember]
        public String DocPath { get; set; }
        [DataMember]
        public Int32 DocSize { get; set; }
        [DataMember]
        public Int16? DocTypeId { get; set; }
    }
}
