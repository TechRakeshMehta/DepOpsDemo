using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    public class CustomFormContract
    {
        public Int32 CustomFormID { get; set; }
        public String CustomFormTitle{ get; set; }
        public String CustomFormName { get; set; }
        public String CustomFormDesc { get; set; }
        public Int32 CustomFormSequence { get; set; }
        public Int32 SelectedCustomFormTypeID { get; set; }
    }
}
