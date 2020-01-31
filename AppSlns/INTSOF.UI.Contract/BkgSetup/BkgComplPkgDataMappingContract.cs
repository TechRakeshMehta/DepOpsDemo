using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    [Serializable]
    public class BkgComplPkgDataMappingContract
    {
        public String DataPointCode { get; set; }
        public Int32? BkgPackageID { get; set; }
        public Int32? ComplPackageID { get; set; }
        public Int32? ServiceGroupID { get; set; }
        public Int32? ServiceID { get; set; }
        public Int32? CatagoryID { get; set; }
        public Int32? ItemID { get; set; }
        public Int32? AttributeID { get; set; }
        public String FlaggedValue { get; set; }
        public String NonFlaggedValue { get; set; }
    }
}