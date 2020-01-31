using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.BkgSetup
{
    public class CustomFormConfigurationContract
    {
        public Int32 CustomFormConfigID { get; set; }
        public String CustomFormConfigSectionTitle { get; set; }
        public String CustomFormConfigCustomHTML { get; set; }
        public Int32 CustomFormConfigSelectedCustomFormID { get; set; }
        public Int32 CustomFormConfigSequence { get; set; }
        public Int32 CustomFormConfigSelectedAttrGroup { get; set; }
        public DisplayColumn CustomFormConfigDisplayColumn { get; set; }
        public Int32 CustomFormConfigOccurrence{ get; set; }
    }
}
