using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace INTSOF.Complio.API.Core
{
    public class RequestData
    {
        public Int32 SchoolId { get; set; }

        public String InputData { get; set; }

        public String EntityTypeCode { get; set; }

        public String Format { get; set; }

    }
}