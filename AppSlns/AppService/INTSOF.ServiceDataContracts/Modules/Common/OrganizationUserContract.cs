using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.Common
{
    [Serializable]
    [DataContract]
    public class OrganizationUserContract
    {
        [DataMember]
        public Int32 OrganizationUserID { get; set; }
        [DataMember]
        public Guid UserID { get; set; }
        [DataMember]
        public Int32 OrganizationID { get; set; }
        [DataMember]
        public String FirstName { get; set; }
        [DataMember]
        public String MiddleName { get; set; }
        [DataMember]
        public String LastName { get; set; }
        [DataMember]
        public String Email { get; set; }
        [DataMember]
        public Boolean IsOutOfOffice { get; set; }
        [DataMember]
        public Boolean IsNewPassword { get; set; }
        [DataMember]
        public Boolean IgnoreIPRestriction { get; set; }
        [DataMember]
        public Boolean IsMessagingUser { get; set; }
        [DataMember]
        public Boolean IsSystem { get; set; }
        [DataMember]
        public Boolean IsDeleted { get; set; }
        [DataMember]
        public Boolean IsActive { get; set; }
        [DataMember]
        public Int32 CreatedByID { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public Boolean IsSubscribeToEmail { get; set; }
        [DataMember]
        public DateTime? DateOfBirth{ get; set; }
        [DataMember]
        public String UserName { get; set; }
        [DataMember]
        public String Phone { get; set; }
        [DataMember]
        public String Address1 { get; set; }
        [DataMember]
        public String Address2 { get; set; }
        [DataMember]
        public String City { get; set; }
        [DataMember]
        public String State { get; set; }
        [DataMember]
        public String County { get; set; }
        [DataMember]
        public String Country { get; set; }
        [DataMember]
        public String ZipCode { get; set; }
        [DataMember]
        public String SSN { get; set; }
    }
}
