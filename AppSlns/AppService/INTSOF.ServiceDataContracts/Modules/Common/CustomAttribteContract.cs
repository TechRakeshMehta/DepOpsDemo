using System;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Core
{
    [Serializable]
    [DataContract]
    public class CustomAttribteContract
    {
        [DataMember]
        public Int32 CustomAttributeId
        {
            get;
            set;
        }

        [DataMember]
        public String CustomAttributeName
        {
            get;
            set;
        }
        [DataMember]
        public String CustomAttributeLabel
        {
            get;
            set;
        }

        [DataMember]
        public String CustomAttributeDataTypeCode
        {
            get;
            set;
        }

        [DataMember]
        public Boolean? CustomAttributeIsRequired
        {
            get;
            set;
        }

        [DataMember]
        public Int32? MaxLength
        {
            get;
            set;
        }

        [DataMember]
        public String CustomAttributeValue
        {
            get;
            set;
        }

        [DataMember]
        public Int32 CustomAttrMappingId
        {
            get;
            set;
        }

        [DataMember]
        public Int32? CustomAttrValueId
        {
            get;
            set;
        }
    }
}
