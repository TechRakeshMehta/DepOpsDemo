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
    public class UniversalItemContract
    {
        [DataMember]
        public Int32 UniversalItemID { get; set; }
        [DataMember]
        public String UniversalItemName { get; set; }
        [DataMember]
        public Int32 UniReqItmMappingID { get; set; }
        [DataMember]
        public Int32 UniReqCatMappingID { get; set; }
        [DataMember]
        public Int32 UniCatItmMappingID { get; set; }
        [DataMember]
        public Int32 ReqCategoryID { get; set; }
        [DataMember]
        public Int32 ReqItemID { get; set; }
        [DataMember]
        public Int32 ReqCatIteMappingmID { get; set; }
    }
}
