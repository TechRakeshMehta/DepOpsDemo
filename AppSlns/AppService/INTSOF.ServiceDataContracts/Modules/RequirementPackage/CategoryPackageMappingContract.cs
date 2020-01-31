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
    public class CategoryPackageMappingContract
    {
        [DataMember]
        public Int32 ReqCategoryID { get; set; }

        [DataMember]
        public String ReqPackageName { get; set; }

        [DataMember]
        public DateTime? EffectiveStartDate { get; set; }

        [DataMember]
        public DateTime? EffectiveEndDate { get; set; }

        [DataMember]
        public Int32 ResultTypeID { get; set; }
    }
}
