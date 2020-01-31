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
    public class ValidateRegexDataContract
    {
        [DataMember]
        public Int32 BSA_ID { get; set; }
        [DataMember]
        public Int32 DataTypeID { get; set; }
        [DataMember]
        public String FieldName { get; set; }
        [DataMember]
        public String ValidateExpression { get; set; }
        [DataMember]
        public String BAGM_Code { get; set; }
        [DataMember]
        public String ValidationMessage { get; set; }
    }
}
