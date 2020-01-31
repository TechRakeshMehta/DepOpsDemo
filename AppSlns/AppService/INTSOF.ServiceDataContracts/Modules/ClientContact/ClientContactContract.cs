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
    public class ClientContactContract
    {
        [DataMember]
        public Int32 ClientContactID { get; set; }
        [DataMember]
        public Int32 TenantID { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public String Email { get; set; }
        [DataMember]
        public String Phone { get; set; }
        [DataMember]
        public Guid? UserID { get; set; }
        [DataMember]
        public Guid TokenID { get; set; }
        [DataMember]
        public Int32 ClientContactTypeID { get; set; }
        [DataMember]
        public String TenantName { get; set; }
        [DataMember]
        public List<ClientContactAvailibiltyContract> ListClientContactAvailibiltyContract { get; set; }
        [DataMember]
        public Int32? OrgUserId { get; set; }
        //UAT-2447
        [DataMember]
        public Boolean IsInternationalPhone { get; set; }
        //UAT-4153
        [DataMember]
        public String AccountActivated { get; set; }
        //UAT-4160
        [DataMember]
        public Boolean IsRegistered { get; set; }

    }

    [Serializable]
    [DataContract]
    public class ClientContactAvailibiltyContract
    {
        [DataMember]
        public Int32 ClientContactAvailibiltyID { get; set; }
        [DataMember]
        public Int32 ClientContactID { get; set; }
        [DataMember]
        public Int32 WeekDayID { get; set; }
        [DataMember]
        public DateTime? StartTime { get; set; }
        [DataMember]
        public DateTime? EndTime { get; set; }
    }
}
