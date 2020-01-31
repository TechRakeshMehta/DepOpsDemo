using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.Utils;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{  

    [Serializable]
    [DataContract]
    public class ManageRotationMemberSearchContract : SearchContract
    {
        [DataMember]
        public RotationMemberSearchDetailContract SearchParameters { get; set; }

        [DataMember]
        public CustomPagingArgsContract GridCustomPagingArguments { get; set; }

        [DataMember]
        public Dictionary<Int32, Int32> DicOfSelectedRotation { get; set; }
    }
}
