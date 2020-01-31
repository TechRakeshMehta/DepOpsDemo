using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using INTSOF.Utils;


namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class InvitationSearchContract
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public String EmailAddress { get; set; }
        public String Phone { get; set; }
        public Int32 TenantID { get; set; }
        public String TenantName { get; set; }
        public DateTime? ExpirationDateFrom { get; set; }
        public DateTime? ExpirationDateTo { get; set; }
        public DateTime? InvitationDateFrom { get; set; }
        public DateTime? InvitationDateTo { get; set; }
        public DateTime? LastViewedDateFrom { get; set; }
        public DateTime? LastViewedDateTo { get; set; }
        public List<Int32> LstInviteTypeID { get; set; }
        public List<String> LstInviteTypeCode { get; set; }
        public List<String> LstInviteTypeName { get; set; }
        public String Notes { get; set; }
        public Int32 CurrentLoggedInUserID { get; set; }
        public String SelectedReviewStatusCode { get; set; }
        public String SelectedInvitationReviewStatusIds { get; set; }
        public String TenantIDs { get; set; }
        public List<TenantDetailsContract> TenantDetailList { get; set; }

        public CustomPagingArgsContract GridCustomPagingArguments { get; set; }

        public String XML { get { return CreateXml(); } }

        public String CreateXml()
        {
            var serializer = new XmlSerializer(typeof(InvitationSearchContract));
            var sb = new StringBuilder();

            InvitationSearchContract xmlData = this;
            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, xmlData);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Selected Agency Ids
        /// </summary>
        public String AgencyIdList { get; set; }

        public String SelectedInviteeTypeCode { get; set; }

        public String SelectedInvitationArchiveStateCode { get; set; } //UAT-3470
    }

    [Serializable]
    public class InvitationIDsDetailContract
    {
        public Int32 ProfileSharingInvitationID { get; set; }
        public Int32 TenantID { get; set; }
        public String TenantName { get; set; }
        public Dictionary<Int32, String> InvitationIDs { get; set; }
        public Boolean Checked { get; set; }
        public String InvitationSource { get; set; } //UAT:2475
        public Int32 RotationID { get; set; } //UAT:2475
        public String RotationName { get; set; } //UAT-2475
        public Int32 AgencyID { get; set; }
    }
}
