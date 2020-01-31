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
    public class ManageEnrollerMappingContract
    {
        [DataMember]
        public Int32 EnrollerMappingID { get; set; }
        [DataMember]
        public Int32 LocationId { get; set; }
        [DataMember]
        public Int32 OrganizationUserID { get; set; }
        [DataMember]
        public Int32 OrganizationID { get; set; }    
        [DataMember]
        public String FirstName { get; set; }
        [DataMember]
        public String LastName { get; set; }
        [DataMember]
        public Int32 PermissionId { get; set; }
        [DataMember]
        public String PermissionCode { get; set; }
        [DataMember]
        public String Permission { get; set; }
        [DataMember]
        public Int32 TotalCount { get; set; }
    }
}
