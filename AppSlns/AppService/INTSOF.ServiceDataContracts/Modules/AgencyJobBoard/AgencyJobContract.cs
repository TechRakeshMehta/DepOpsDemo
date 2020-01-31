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
    public class AgencyJobContract
    {
        [DataMember]
        public Int32 AgencyJobID { get; set; }
        [DataMember]
        public String Company { get; set; }
        [DataMember]
        public String Location { get; set; }
        [DataMember]
        public String JobTitle { get; set; }
        [DataMember]
        public String JobDescription { get; set; }
        [DataMember]
        public String JobTypeName { get; set; }
        [DataMember]
        public Int32 TypeID { get; set; }
        [DataMember]
        public Int32 StatusID { get; set; }
        [DataMember]
        public String StatusCode { get; set; }
        [DataMember]
        public String Status { get; set; }
        [DataMember]
        public String HowToApply { get; set; }
        [DataMember]
        public String Instructions { get; set; }
        [DataMember]
        public Int32 AgencyHierarchyID { get; set; }
        [DataMember]
        public String TemplateName { get; set; }
        [DataMember]
        public String LogoPath { get; set; }
        [DataMember]
        public String AgencyJobTypeCode { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }
        [DataMember]
        public DateTime? PublishDate { get; set; }
        [DataMember]
        public String FieldTypeName{ get; set; }
        [DataMember]
        public Int32 FieldTypeID { get; set; }
       
    }
}
