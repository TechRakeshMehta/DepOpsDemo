using System;
using System.Runtime.Serialization;

namespace INTSOF.UI.Contract.SystemSetUp
{
    [Serializable]
    [DataContract]
    public class ClientSettingCustomAttributeContract
    {
        [DataMember]
        public Int32 CustomAttributeID { get; set; }
        [DataMember]
        public String SettingName { get; set; }
        [DataMember]
        public String SettingOverrideText { get; set; }
        [DataMember]
        public Boolean SettingValue { get; set; }
        [DataMember]
        public String CustomAttributeDatatypeCode { get; set; }
        [DataMember]
        public Int32 CustomAttributeClientSettingMappingID { get; set; }
        [DataMember]
        public Int32 SettingID { get; set; }
        [DataMember]
        public String ValidateExpression { get; set; }
        [DataMember]
        public String ValidationMessage { get; set; }
        [DataMember]
        public Boolean IsRequired { get; set;  }
    }
}
