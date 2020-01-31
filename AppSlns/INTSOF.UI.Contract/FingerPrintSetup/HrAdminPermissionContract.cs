using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.FingerPrintSetup
{
    [Serializable]
    [DataContract]
    public class HrAdminPermissionContract
    {
        [DataMember]
        public Int32 PermissionId { get; set; }
        [DataMember]
        public String FirstName { get; set; }
        [DataMember]
        public String LastName { get; set; }
        [DataMember]
        public Int32 OrganizationUserID { get; set; }
        [DataMember]
        public String CabsPermissionType { get; set; }
        [DataMember]
        public String CabsPermissionValue { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }

        #region Filter Parameters
        [DataMember]
        public String FirstNameFilter { get; set; }
        [DataMember]
        public String LastNameFilter { get; set; }
        #endregion
    }
}
