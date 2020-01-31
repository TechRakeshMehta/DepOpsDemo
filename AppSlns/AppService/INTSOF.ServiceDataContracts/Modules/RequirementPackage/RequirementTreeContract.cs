using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RequirementTreeContract
    {
        [DataMember]
        public Int32 TreeNodeTypeID
        {
            get;
            set;
        }

        [DataMember]
        public String NodeID
        {
            get;
            set;
        }

        [DataMember]
        public String ParentNodeID
        {
            get;
            set;
        }

        [DataMember]
        public Int32 DataID
        {
            get;
            set;
        }

        [DataMember]
        public Int32? ParentDataID
        {
            get;
            set;
        }

        [DataMember]
        public String Value
        {
            get;
            set;
        }

        [DataMember]
        public String UICode
        {
            get;
            set;
        }

        [DataMember]
        public String HID
        {
            get;
            set;
        }

    }
}
