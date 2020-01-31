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
    public class BulkPackageCopyContract
    {
        [DataMember]
        public Int32 SourcePackageID { get; set; }
        [DataMember]
        public String TargetPackageName { get; set; }
        [DataMember]
        public String TargetPackageLabel { get; set; }
        [DataMember]
        public DateTime TargetPackageEffectiveStartDate { get; set; }
        [DataMember]
        public String TargetPackageAgencyIds { get; set; }
        [DataMember]
        public String TargetAgencyHierarchyIds { get; set; }
    }
}
