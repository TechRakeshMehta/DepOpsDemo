using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    [DataContract]
    public class CustomComplianceContract
    {
        [DataMember]
        public Int32 OrganizationUserID { get; set; }
        [DataMember]
        public Int32 PackageSubscriptionID { get; set; }
        [DataMember]
        public String ApplicantName { get; set; }
    }
}
