using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class ShareHistorySearchContract
    {
        public Int32 TenantId { get; set; }
        public String DPMIds { get; set; }
        public Int32 AgencyID { get; set; }
        public Int32 UserGroupID { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public String ApplicantFirstName { get; set; }
        public String ApplicantLastName { get; set; }
        public String EmailAddress { get; set; }
        public String ApplicantSSN { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public String CustomFields { get; set; }

        public String RotationName { get; set; }
        public String TypeSpecialty { get; set; }
        public String Department { get; set; }
        public String Program { get; set; }
        public String Course { get; set; }
        public String Term { get; set; }
        public String UnitFloorLoc { get; set; }
        public String DaysIdList { get; set; }
        public String StartTime { get; set; }
        public String EndTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public String SelectedClientContacts { get; set; }
        public String RotationCustomAttributes { get; set; }
        public Int32? LoggedInUserId { get; set; }
        //UAT-1895 Add a filter to show all or audit requested shares.
        public Boolean IsAuditRequested { get; set; }
        public String CreateXml()
        {
            var serializer = new XmlSerializer(typeof(ShareHistorySearchContract));
            var sb = new StringBuilder();

            ShareHistorySearchContract xmlData = this;
            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, xmlData);
            }
            return sb.ToString();
        }
    }
}
