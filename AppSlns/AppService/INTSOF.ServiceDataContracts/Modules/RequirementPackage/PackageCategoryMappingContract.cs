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
    public class PackageCategoryMappingContract
    {
        [DataMember]
        public Int32 ReqPackageID { get; set; }

        [DataMember]
        public String ReqCategoryName { get; set; }

        [DataMember]
        public Int32 ResultTypeID { get; set; }

        [DataMember]
        public Int32 CategoryDisplayOrder { get; set; }
    }
}
