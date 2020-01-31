using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    [Serializable]
    [DataContract]
    public class CascadingAttributeOptionsContract
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int AttributeId { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string SourceValue { get; set; }
        [DataMember]
        public int DisplaySequence { get; set; }
    }
}
