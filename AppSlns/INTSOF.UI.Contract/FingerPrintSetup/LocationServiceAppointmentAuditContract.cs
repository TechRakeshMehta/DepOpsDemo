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
    public class LocationServiceAppointmentAuditContract
    {
        [DataMember]
        public Int32 AppointmentAuditId { get; set; }
        [DataMember]
        public Int32 AppointmentId { get; set; }
        [DataMember]
        public Int32 ChangeTypeId { get; set; }
        [DataMember]
        public String OldValue { get; set; }
        [DataMember]
        public String NewValue { get; set; }
        [DataMember]
        public DateTime UpdationDate { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public Int32 CurrentPageIndex { get; set; }
        [DataMember]
        public Int32 CurrentPageSize { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }
        [DataMember]
        public String ChangeType { get; set; }
        [DataMember]
        public String UpdatedBy { get; set; }
        [DataMember]
        public Int32 TenantId { get; set; }
        [DataMember]
        public String EventName { get; set; }
        [DataMember]
        public String ApplicantName { get; set; }
        [DataMember]
        public String LocationName { get; set; }
        [DataMember]
        public String LocationAddress { get; set; }
        [DataMember]
        public String OrderNumber { get; set; }

        #region Filter Parameters
        [DataMember]
        public DateTime? AppointmentAuditHistoryFrom { get; set; }
        [DataMember]
        public DateTime? AppointmentAuditHistoryTo { get; set; }
        [DataMember]
        public Boolean IsEventFilter { get; set; }
        [DataMember]
        public String EventNameFilter { get; set; }
        [DataMember]
        public String LocationIds { get; set; }
        [DataMember]
        public Boolean IsAllAppointment { get; set; }
        [DataMember]
        public string ApplicantnameFilter { get; set; }
        [DataMember]
        public Boolean IsOutOfState { get; set;}
        [DataMember]
        public string OrderNumberFilter { get; set; }

        #endregion

    }
}
