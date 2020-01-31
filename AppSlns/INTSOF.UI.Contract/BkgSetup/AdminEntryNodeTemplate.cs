using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    [Serializable]
    public class AdminEntryNodeTemplate
    {
        [DataMember]
        public Int32 TemplateId { get; set; }
        [DataMember]
        public String Content { get; set; }
        [DataMember]
        public String TemplateName { get; set; }
        [DataMember]
        public String Subject { get; set; }
    }
}
