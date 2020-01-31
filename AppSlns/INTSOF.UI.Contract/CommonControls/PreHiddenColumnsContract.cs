using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.CommonControls
{
    [Serializable]
    public class PreHiddenColumnsContract
    {
        public String GridCode { get; set; }
        public String PredefinedHiddenColumn { get; set; }
    }
}
