using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.Utils
{
    [Serializable]
    public class ApplicantSMSNotificationDataContract
    {
        [DataMember]
        public Boolean IsReceiveTextNotification { get; set; }
        [DataMember]
        public String PhoneNumber { get; set; }
        [DataMember]
        public Boolean IsComfirmMessageVisible { get; set; } 
        [DataMember]
        public String ConfirmationStatus { get; set; }
    }
}
