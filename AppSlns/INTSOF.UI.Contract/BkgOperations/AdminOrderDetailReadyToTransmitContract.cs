using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    public class AdminOrderDetailReadyToTransmitContract
    {
        public Int32 MasterOrderID { get; set; }
        public Int32 BkgOrderID { get; set; }
        public Boolean IsProfileExist { get; set; }
        public Boolean IsPackageSelected { get; set; }
        public Boolean IsALLCustomFormDataExist { get; set; }
        public Boolean IsOrderReadyToTransmit { get; set; } 
    }
}
