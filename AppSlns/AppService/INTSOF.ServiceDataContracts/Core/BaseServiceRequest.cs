using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace INTSOF.ServiceDataContracts.Core
{
    /// <summary>
    /// Represents the Base class for the ServiceRequest class.
    /// </summary>
    [DataContract]
    public class BaseServiceRequest
    {
        /// <summary>
        /// Return the DefaultTenantId
        /// </summary>
        [DataMember]
        public Int32 DefaultTenantId
        {
            get
            {
                return AppConsts.ONE;
            }
            internal set
            {
            }
        }

        /// <summary>
        /// Get/Set the SelectedTenantId 
        /// </summary>
        [DataMember]
        public Int32 SelectedTenantId
        {
            get;
            set;
        }
    }
}
