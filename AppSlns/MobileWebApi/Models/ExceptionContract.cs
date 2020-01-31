using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MobileWebApi
{
    [DataContract]
    public class ExceptionContract
    {
        [DataMember]
        public String ModuleName { get; set; }
        [DataMember]
        public String ExceptionCode { get; set; }
        [DataMember]
        public String ExceptionMessage { get; set; }
    }
}