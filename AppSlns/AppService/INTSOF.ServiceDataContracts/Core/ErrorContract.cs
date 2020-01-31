using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Core
{
    [DataContract]
    public class ErrorContract
    { 
        /// <summary>
        /// Error Code
        /// </summary>
        [DataMember(Name = "ErrorCode")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// Source
        /// </summary>
        [DataMember(Name = "Source")]
        public string Source { get; set; }

        /// <summary>
        /// Error Message
        /// </summary>
        [DataMember(Name = "ErrorMessage")]
        public string ErrorMessage { get; set; }
    }
}
