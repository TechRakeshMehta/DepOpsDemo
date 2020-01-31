using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace INTSOF.Utils
{
    [Serializable]
    public class ItemVerificationQueueData
    {
        [DataMember]
        public Boolean IsDefaultOrThrdPrty
        {
            get;
            set;
        }

        [DataMember]
        public Int32 SelectedUserGroupId
        {
            get;
            set;
        }

        [DataMember]
        public String ReviewerType
        {
            get;
            set;
        }

        [DataMember]
        public Boolean ShowIncompleteItems
        {
            get;
            set;
        }

        [DataMember]
        public Int32 ReviewerId
        {
            get;
            set;
        }
        [DataMember]
        public Int32 AssignedToUserId
        {
            get;
            set;
        }

        [DataMember]
        public List<StatusCode> lstStatusCode
        {
            get;
            set;
        }

        [DataMember]
        public Int32 selectedPackageId
        {
            get;
            set;
        }
        [DataMember]
        public Int32 CategoryId
        {
            get;
            set;
        }
        [DataMember]
        public String QueueId
        {
            get;
            set;
        }
        [DataMember]
        public Boolean ShowOnlyRushOrder
        {
            get;
            set;
        }

        [DataMember]
        public Int32 CurrentLoggedInUser
        {
            get;
            set;
        }

        [DataMember]
        public Int32 BussinessProcessId
        {
            get;
            set;
        }

        [DataMember]
        public Boolean IsEscalationRecords
        {
            get;
            set;
        }

        public String XML { get { return GetXml(); } }

        private String GetXml()
        {
            var serializer = new XmlSerializer(typeof(ItemVerificationQueueData));
            var sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, this);
            }


            return sb.ToString();
        }
    }

    [Serializable]
    public class StatusCode
    {
        [DataMember]

        public string statusCode { get; set; }
    }

    [Serializable]
    public class QueueRecords
    {
        public List<QueueRecordUsers> QueueRecordUsers { get; set; }

        public string Xml { get { return GetXml(); } }

        private String GetXml()
        {
            var serializer = new XmlSerializer(typeof(QueueRecords));
            var sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, this);
            }


            return sb.ToString();
        }
    }

    [Serializable]
    public class QueueRecordUsers
    {
        public Int32 QueueID { get; set; }
        public Int32 RecordID { get; set; }
        public Int32 AssignToUserID { get; set; }

    }

}
