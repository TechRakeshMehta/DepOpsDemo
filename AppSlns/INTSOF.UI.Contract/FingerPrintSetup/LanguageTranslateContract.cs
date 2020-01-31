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
    public class LanguageTranslateContract
    {
        [DataMember]
        public Int32 LanguageId { get; set; }
        [DataMember]
        public Int32 TenantId { get; set; }
        [DataMember]
        public Int32 SystemSpecificLanguageTextId { get; set; }
        [DataMember]
        public Int32 EntityTypeId { get; set; }
        [DataMember]
        public Int32 EntityId { get; set; }
        [DataMember]
        public String EnglishText { get; set; }
        [DataMember]
        public String SpanishText { get; set; }

        public Int32 TotalCount { get; set; }
    }
}
