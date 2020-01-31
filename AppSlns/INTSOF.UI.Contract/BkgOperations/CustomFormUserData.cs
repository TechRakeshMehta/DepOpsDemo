using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    public class CustomFormUserData
    {


    }

    public class BkgOrderDetailCustomFormUserData
    {
        public Int32 BackgroundOrderID { get; set; }
        public Int32 CustomFormID { get; set; }
        public Int32 AttributeGroupMappingID { get; set; }
        public Int32 AttributeGroupID { get; set; }
        public Int32 InstanceID { get; set; }
        public String AttributName { get; set; }
        public String Value { get; set; }
    }
}
