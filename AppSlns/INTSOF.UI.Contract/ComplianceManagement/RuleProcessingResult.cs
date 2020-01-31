using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    [System.Xml.Serialization.XmlRootAttribute("RuleProcessingResult")]
 
    public class RuleProcessingResult
    {
        [XmlElement("Status")]
        public Int32 Status { get; set; }

        [XmlElement("Action")]
        public String Action { get; set; }

        [XmlElement("Result")]
        public String Result { get; set; }

        [XmlElement("UIExpressionLabel")]
        public String UIExpressionLabel { get; set; }

        [XmlElement("SuccessMessage")]
        public String SuccessMessage { get; set; }

        [XmlElement("ErrorMessage")]
        public String ErrorMessage { get; set; }
    }
}
