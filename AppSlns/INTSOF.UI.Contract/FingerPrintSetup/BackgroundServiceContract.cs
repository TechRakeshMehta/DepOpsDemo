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
    public class BackgroundServiceContract
    {
        [DataMember]
        public Int32 BSE_ID { get; set; }
        [DataMember]
        public String BSE_Name { get; set; }
        [DataMember]
        public String BSE_Description { get; set; }
        [DataMember]
        public Int32 BSE_SvcTypeID { get; set; }
        [DataMember]
        public Boolean BSE_IsEditable { get; set; }
        [DataMember]
        public Boolean BSE_IsDeleted { get; set; }

        [DataMember]
        public string BST_Code { get; set; }
        
    }
}
