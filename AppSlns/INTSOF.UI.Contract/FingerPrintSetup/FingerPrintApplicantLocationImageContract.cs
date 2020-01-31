using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.FingerPrintSetup
{
    [Serializable]
    [DataContract]
    public class FingerPrintApplicantLocationImageContract
    {
        [DataMember]
        public List<FingerPrintLocationImagesContract> imageDataList { get; set; }
        [DataMember]
        public String LocationName { get; set; }

    }
}
