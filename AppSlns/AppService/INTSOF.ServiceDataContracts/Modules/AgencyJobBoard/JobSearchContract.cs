using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.AgencyJobBoard
{
    [Serializable]
    [DataContract]
    public class JobSearchContract
    {
        [DataMember]
        public Int32 AgencyJobID { get; set; }
        [DataMember]
        public String JobTitle { get; set; }
        [DataMember]
        public String Location { get; set; }
        [DataMember]
        public String Company { get; set; }
        [DataMember]
        public String JobTypeCode { get; set; }
        [DataMember]
        public String TenantId { get; set; }
        [DataMember]
        public Int32 OrganizationUserId { get; set; }

        [DataMember]
        public List<Int32> lstSelectedTenantIds { get; set; }
        [DataMember]
        public Int32 JobFieldTypeID { get; set; }

    }
}
