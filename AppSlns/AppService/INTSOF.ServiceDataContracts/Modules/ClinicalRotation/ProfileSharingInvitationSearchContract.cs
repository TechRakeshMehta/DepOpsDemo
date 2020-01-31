using System;
using System.Runtime.Serialization;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.Utils;
using System.IO;
using System.Xml.Serialization;
using System.Text;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class ProfileSharingInvitationSearchContract : SearchContract
    {
        [DataMember]
        public Int32? OrganizationUserId { get; set; }

        [DataMember]
        public String TenantName { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public Int32? InvitationID { get; set; }

        [DataMember]
        public Int32? LoggedInUserId { get; set; }

        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        [DataMember]
        public DateTime? ExpirationDateFrom { get; set; }

        [DataMember]
        public DateTime? ExpirationDateTo { get; set; }

        [DataMember]
        public DateTime? InvitationDate { get; set; }

        [DataMember]
        public DateTime? InvitationDateFrom { get; set; }

        [DataMember]
        public DateTime? InvitationDateTo { get; set; }

        [DataMember]
        public Int32? ViewsRemaining { get; set; }

        [DataMember]
        public Int32 TotalRecordCount { get; set; }

        [DataMember]
        public Int32? MaxViews { get; set; }

        [DataMember]
        public Int32? ExpirationTypeId { get; set; }

        [DataMember]
        public String ExpirationTypeCode { get; set; }

        [DataMember]
        public CustomPagingArgsContract GridCustomPagingArguments { get; set; }

        public String XML
        {
            get
            {
                return CreateXml();
            }
        }

        private String CreateXml()
        {
            var serializer = new XmlSerializer(typeof(ProfileSharingInvitationSearchContract));
            var sb = new StringBuilder();
            ProfileSharingInvitationSearchContract xmlData = this;

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, xmlData);
            }
            return sb.ToString();
        }

        [DataMember]
        public Int32 InviteeViewCount { get; set; }
    }
}
