using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Core
{
    [DataContract]
    public class BaseServiceResponse : IBaseServiceResponse
    {
        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        public ResponseStatus Status { get; set; }

        /// <summary>
        /// Errors
        /// </summary>
        [DataMember(Name = "Errors")]
        public List<ErrorContract> Errors { get; set; }


        public BaseServiceResponse()
        {
            this.Errors = new List<ErrorContract>();
            this.Status = ResponseStatus.Success;
        }
    }
}
