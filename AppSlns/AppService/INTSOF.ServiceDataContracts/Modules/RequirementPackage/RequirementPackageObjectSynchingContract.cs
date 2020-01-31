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
    public class RequirementPackageObjectSynchingContract
    {
        [DataMember]
        public Int32 OldObjectId { get; set; }
        [DataMember]
        public Int32 NewObjectId { get; set; }

        [DataMember]
        public String ObjectTypeCode { get; set; }
        [DataMember]
        public String ActionTypeCode { get; set; }

        [DataMember]
        public Int32 PackageTypeId { get; set; }

        [DataMember]
        public Int32 CategoryID { get; set; }
        [DataMember]
        public Boolean IsAdded { get; set; }
        [DataMember]
        public Boolean IsEdit { get; set; }
        [DataMember]
        public Boolean IsRemoved { get; set; }
    }
}
