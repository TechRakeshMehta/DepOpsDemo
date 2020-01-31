using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    [Serializable]
    public class BkgPackageStateSearchContract
    {
        public Boolean IsStateSearchChecked { get; set; }
        public Boolean IsCountySearchChecked { get; set; }
        public Int32 StateID { get; set; }
        public Int32 BkgPackageID { get; set; }
    }
}
