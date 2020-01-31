using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RequirementPackageSubscriptionApprovedContract
    {

        [DataMember]
        public Int32 CR_ID { get; set; }

        [DataMember]
        public Int32 CRA_AgencyID { get; set; }

        [DataMember]
        public string HierarchyNodeIDs { get; set; }

    }
}
