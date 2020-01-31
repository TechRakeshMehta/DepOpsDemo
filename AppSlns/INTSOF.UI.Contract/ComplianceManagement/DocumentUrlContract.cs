using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    [Serializable]
    public class DocumentUrlContract
    {
        public Int32 ID { get; set; }
        public String SampleDocFormURL { get; set; }
        public String SampleDocFormURLLabel { get; set; }
        public String SampleDocFormUrlDisplayLabel { get; set; }
        //public bool IsUcOnComplianceCategoryScreen { get; set; }
    }
}
