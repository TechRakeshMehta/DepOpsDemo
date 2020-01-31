using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace INTSOF.Complio.API.Core
{
    /// <summary>
    /// Wrapper class to include Root element when Json result is returned from API
    /// </summary>
    public class ServiceResponseWrapper
    {
        [DataMember]
        public ServiceResponse ServiceResponse { get; set; }
    }
    
    public class ServiceResponse
    {
        [DataMember(Order = 0)]
        public ServiceStatus Status { get; set; }

        [DataMember(Order = 1)]
        public System.Xml.XmlDocument Result { get; set; }
    }

    public class ServiceResponseJsonWrapper
    {
        [DataMember]
        public ServiceResponseJson ServiceResponseJson { get; set; }
    }

    public class ServiceResponseJson
    {
        [DataMember(Order = 0)]
        public ServiceStatus Status { get; set; }

        [DataMember(Order = 1)]
        public String Result { get; set; }
    }
   

    /// <summary>
    /// Contract for Status of the Request, to be sent in Response
    /// </summary>
    [DataContract]
    public class ServiceStatus
    {
        [DataMember(Order = 0)]
        public String Type { get; set; }

        [DataMember(Order = 1)]
        public Int32 Code { get; set; }

        [DataMember(Order = 2)]
        public String Message { get; set; }
    }
}