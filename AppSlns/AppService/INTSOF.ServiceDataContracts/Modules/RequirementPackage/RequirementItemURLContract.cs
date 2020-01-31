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
    public class RequirementItemURLContract
    {

        [DataMember]
        public Int32 RItemURLID { get; set; }
        [DataMember]
        public String RItemURLLabel { get; set; }
        [DataMember]
        public String RItemURLSampleDocURL { get; set; }
        //[DataMember]
        //public int RIU_RequirementItemId { get; set; }
        //[DataMember]
        //public String RIU_Label { get; set; }
        //[DataMember]
        //public String RIU_SampleDocURL { get; set; }
        //[DataMember]
        //public int RIU_CreatedById { get; set; }
        //[DataMember]
        //public DateTime RIU_CreatedOn { get; set; }
        //[DataMember]
        //public bool RIU_IsDeleted { get; set; }



    }
}
