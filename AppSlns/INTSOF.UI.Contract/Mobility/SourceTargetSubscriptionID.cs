using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.Mobility
{
    [Serializable]
    public class SourceTargetSubscriptionID
    {

        public Int32 SourceSubscriptionID { get; set; }
        public Int32 TargetSubscriptionID { get; set; }
    }
}
