using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class SharedInvitationContract
    {
        [DataMember]
        public Int32? CurrentUserID { get; set; }

        [DataMember]
        public Int32? SelectedAgencyID { get; set; }

        [DataMember]
        public Int32? SelectedTenantID { get; set; }

        [DataMember]
        public String AssignedUnits { get; set; }

        [DataMember]
        public String ProgramName { get; set; }

        [DataMember]
        public String SchoolRepresentative { get; set; }

        [DataMember]
        public DateTime? EffectiveDate { get; set; }

        //Code commented for UAT-1749: As an admin, I should be able to search on sent profile shares and rotation shares 
        //[DataMember]
        //public DateTime? AttestationDate { get; set; }

        [DataMember]
        public DateTime? ClinicalFromDate { get; set; }

        [DataMember]
        public DateTime? ClinicalToDate { get; set; }

        [DataMember]
        public Int32? InvitationStatusID { get; set; }

        [DataMember]
        public Int32? InvitationGroupId { get; set; }

        #region UAT-1749: As an admin, I should be able to search on sent profile shares and rotation shares

        [DataMember]
        public Boolean? IsViewed { get; set; }

        [DataMember]
        public DateTime? InvitationToDate { get; set; }

        [DataMember]
        public DateTime? InvitationFromDate { get; set; }
        #endregion

        public String XML
        {
            get
            {
                return CreateXml();
            }
        }

        private String CreateXml()
        {
            var serializer = new XmlSerializer(typeof(SharedInvitationContract));
            var sb = new StringBuilder();
            SharedInvitationContract xmlData = this;

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, xmlData);
            }
            return sb.ToString();
        }
    }
}
