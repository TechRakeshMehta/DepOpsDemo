using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Core
{
    public interface IBaseServiceResponse
    {
        /// <summary>
        /// Status
        /// </summary>
        [DataMember(Name = "Status")]
        ResponseStatus Status { get; set; }

        /// <summary>
        /// Errors
        /// </summary>
        [DataMember(Name = "Errors")]
        List<ErrorContract> Errors { get; set; }
    }
}
