using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.FingerPrintSetup
{
    [Serializable]
    [DataContract]
    public class ServiceStatusContract
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public string StatusCode { get; set; }
        public int? OrderBy { get; set; }
    }
}
