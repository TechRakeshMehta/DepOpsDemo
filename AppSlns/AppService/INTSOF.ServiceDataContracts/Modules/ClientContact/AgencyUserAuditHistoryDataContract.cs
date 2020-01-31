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
    public class AgencyUserAuditHistoryDataContract
    {
        [DataMember]
        public Int32 RotationID { get; set; }
        [DataMember]
        public Int32 TenantID { get; set; }
        [DataMember]
        public Int32 AgencyID { get; set; }
        [DataMember]
        public Int32 ProfileSharingInvitationID { get; set; }
        [DataMember]
        public List<AgencyUserAuditChangeTypeDataContract> ListChangeTypeData { get; set; }
        [DataMember]
        public Int32 CurrentLoggedInUserID { get; set; }
        [DataMember]
        public Boolean IsRotation { get; set; }
    }

    [Serializable]
    [DataContract]
    public class AgencyUserAuditChangeTypeDataContract
    {
        [DataMember]
        public Int32 ChangeTypeID { get; set; }
        [DataMember]
        public String ChangeTypeCode { get; set; }
        [DataMember]
        public String OldValue { get; set; }
        [DataMember]
        public String NewValue { get; set; }
    }
}
