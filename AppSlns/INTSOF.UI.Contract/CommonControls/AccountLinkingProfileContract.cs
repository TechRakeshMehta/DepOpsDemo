using Entity.SharedDataEntity;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTSOF.UI.Contract.ProfileSharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.CommonControls
{
    [Serializable]
    [DataContract]
    public class AccountLinkingProfileContract
    {
        [DataMember]
        public AgencyUserContract AgencyUserContract { get; set; }
        [DataMember]
        public List<AgencyUserPermission> lstAgencyUserPermission { get; set; }
        [DataMember]
        public Dictionary<Int32, Boolean> dicNotificationData { get; set; }
        [DataMember]
        public String SelectedLinkingProfileOrgUsername { get; set; }
        [DataMember]
        public String Username { get; set; }
        [DataMember]
        public String UserEmail { get; set; }
        [DataMember]
        public Boolean IsProfileLinked { get; set; }
        [DataMember]
        public String ProfileLinkingResponse { get; set; }
        [DataMember]
        public Boolean IsCancelButtonClicked { get; set; }
        [DataMember]
        public ManageUsersContract ManageUsersContract { get; set; }
        [DataMember]
        public Int32 CopyFromClientAdminOrgID { get; set; }
        [DataMember]
        public Guid CopyFromClientAdminUserID { get; set; } 
    }
}
