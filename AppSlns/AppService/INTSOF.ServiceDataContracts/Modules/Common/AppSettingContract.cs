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
    public class AppSettingContract
    {
        [DataMember]
        public String ClientContactInvitationURL { get; set; }
        [DataMember]
        public String OrganizationUserType { get; set; }
        [DataMember]
        public String SenderEmailID { get; set; }

    }
}
