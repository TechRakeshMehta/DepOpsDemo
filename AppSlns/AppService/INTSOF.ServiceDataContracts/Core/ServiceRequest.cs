using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Core
{
    [DataContract]
    public class ServiceRequest<T> : BaseServiceRequest
    {
        /// <summary>
        /// T Parameter
        /// </summary>
        [DataMember]
        public T Parameter { get; set; }
    }

    [DataContract]
    public class ServiceRequest<T, K> : BaseServiceRequest
    {
        /// <summary>
        /// T Parameter
        /// </summary>
        [DataMember]
        public T Parameter1 { get; set; }

        /// <summary>
        /// K Parameter
        /// </summary>
        [DataMember]
        public K Parameter2 { get; set; }
    }

    [DataContract]
    public class ServiceRequest<T, K, M> : BaseServiceRequest
    {
        /// <summary>
        /// T Parameter
        /// </summary>
        [DataMember]
        public T Parameter1 { get; set; }

        /// <summary>
        /// K Parameter
        /// </summary>
        [DataMember]
        public K Parameter2 { get; set; }

        /// <summary>
        /// M Parameter
        /// </summary>
        [DataMember]
        public M Parameter3 { get; set; }
    }

    [DataContract]
    public class ServiceRequest<T, K, M, N> : BaseServiceRequest
    {
        /// <summary>
        /// T Parameter
        /// </summary>
        [DataMember]
        public T Parameter1 { get; set; }

        /// <summary>
        /// K Parameter
        /// </summary>
        [DataMember]
        public K Parameter2 { get; set; }

        /// <summary>
        /// M Parameter
        /// </summary>
        [DataMember]
        public M Parameter3 { get; set; }

        /// <summary>
        /// N Parameter
        /// </summary>
        [DataMember]
        public N Parameter4 { get; set; }
    }
}
