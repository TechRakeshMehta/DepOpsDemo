using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Core
{
    [DataContract]
    public class ServiceResponse<T> : BaseServiceResponse
    {
        /// <summary>
        /// Result
        /// </summary>
        [DataMember(Name = "Result")]
        public T Result { get; set; }
    }

    [DataContract]
    public class ServiceResponse<T, K> : BaseServiceResponse
    {
        /// <summary>
        /// Result
        /// </summary>
        [DataMember(Name = "Result1")]
        public T Result1 { get; set; }

        /// <summary>
        /// Result
        /// </summary>
        [DataMember(Name = "Result2")]
        public K Result2 { get; set; }
    }
}
