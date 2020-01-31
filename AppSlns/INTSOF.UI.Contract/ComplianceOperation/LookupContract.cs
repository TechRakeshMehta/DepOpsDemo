using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    [Serializable]
    [DataContract]
    public class LookupContract
    {
        [DataMember]
        public Int32 ID { get; set; }
        [DataMember]
        public String Code { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public Int32 UserID { get; set; }
        [DataMember]
        // public String ExtendedName { get; set; }
        //UAT-2792
        public Boolean IsFirstLogin { get; set; }
        //Globalization
        [DataMember]
        public Int32? DefaultLanguageKeyID { get; set; }
        [DataMember]
        public Int32? LanguageID { get; set; }
        [DataMember]
        public Guid? OrganizationUserId { get; set; }
    }
}
