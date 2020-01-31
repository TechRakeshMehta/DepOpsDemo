using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.MobileAPI
{
    [DataContract]
    public class OrderDocumentContract
    {
        [DataMember]
        public byte[] DocumentByte { get; set; }

        [DataMember]
        public String FileName { get; set; }
    }
}
