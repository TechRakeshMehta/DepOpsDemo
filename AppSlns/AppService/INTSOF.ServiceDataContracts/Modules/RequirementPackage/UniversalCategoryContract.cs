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
    public class UniversalCategoryContract
    {
        [DataMember]
        public Int32 UniversalCategoryID { get; set; }
        [DataMember]
        public String UniversalCategoryName { get; set; }
        [DataMember]
        public Int32 UniCatReqCatMappingID { get; set; }
    }
}
