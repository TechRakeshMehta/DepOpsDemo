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
  public  class RequirementAgencyData
    {
        [DataMember]
        public Int32 RequirementPackageAgencyID { get; set; }
        [DataMember]
        public Int32 AgencyID { get; set; }
        [DataMember]
        public String AgencyName { get; set; }
    }
}
