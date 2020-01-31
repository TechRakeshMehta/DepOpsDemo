using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class SystemEntityUserPermission
    {
        [DataMember]
        public Int32 SEP_ID { get; set; }
        [DataMember]
        public Int32 SEP_OrganisationUserId { get; set; }
        [DataMember]
        public Int32 SEP_EntityId { get; set; }
        [DataMember]
        public string SEP_PermissionName { get; set; }
        [DataMember]
        public string SEP_PermissionDescription { get; set; }
        [DataMember]
        public string SEP_PermissionCode { get; set; }

        [DataMember]
        public string SE_Name { get; set; }
        [DataMember]
        public string SE_Description { get; set; }
        [DataMember]
        public string SE_CODE { get; set; }

    }
}
