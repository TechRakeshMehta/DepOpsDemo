using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    [DataContract]
    public class CustomFormDataContract
    {
        [DataMember]
        public String CustomFormName { get; set; }
        [DataMember]
        public Int32 customFormId { get; set; }
        [DataMember]
        public List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }
        [DataMember]
        public Int32 groupId { get; set; }
        [DataMember]
        public Int32 instanceId { get; set; }

    }

    [Serializable]
    public class BkgOrderDetailCustomFormDataContract
    {
        public List<AttributesForCustomFormContract> lstCustomFormAttributes { get; set; }
        public List<BkgOrderDetailCustomFormUserData> lstDataForCustomForm { get; set; }

    }
}
