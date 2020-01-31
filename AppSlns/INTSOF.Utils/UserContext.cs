using System;
using System.Runtime.Serialization;

namespace INTSOF.Utils
{
    [Serializable]
    [DataContract]
    public class UserContext
    {
        [DataMember]
        public String UserID { get; set; }

        [DataMember]
        public Boolean IsSysXAdmin
        {
            get;
            set;
        }

        [DataMember]
        public Int32 OrganizationUserId
        {
            get;
            set;
        }

        [DataMember]
        public Int32 SysXBlockId
        {
            get;
            set;
        }

        [DataMember]
        public String SysXBlockName
        {
            get;
            set;
        }

        [DataMember]
        public Int32 BusinessChannelTypeID
        {
            get;
            set;
        }
    }
}
