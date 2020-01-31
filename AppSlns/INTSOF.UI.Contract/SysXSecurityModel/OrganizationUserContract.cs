using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SysXSecurityModel
{
    /// <summary>
    /// Organization user contract class used in ManageUser screen.
    /// UAT 1204: Issue with Manage Users, and Manage Users from Manage institution
    /// </summary>
    public class OrganizationUserContract
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String UserName { get; set; }
        public String MobileAlias { get; set; }
        public String LastActivityDate { get; set; }
        public String OrganizationName { get; set; }
        public Int32 OrganizationID { get; set; }
        public String CreatedByUserName { get; set; }
        public Boolean IsLockedOut { get; set; }
        public Boolean IsActive { get; set; }
        public Guid UserID { get; set; } //todo used in item databound manageuser screen
        public Int32 OrganizationUserID { get; set; } //todo used in item databound manageuser screen
        public String Email { get; set; } //todo used in item databound manageuser screen
        public Int32 CreatedByID { get; set; } //todo used in item databound manageuser screen
        public Boolean IsSystem { get; set; } //todo used in item databound manageuser screen
        public Boolean IsApplicant { get; set; } //todo used in item databound manageuser screen
        public Boolean IsSharedUser { get; set; } //todo used in presenter
        public Int32 TenantID { get; set; } //todo used in item databound manageuser screen
        public String TenantTypeCode { get; set; } //todo used in item databound manageuser screen
        public String SecondaryEmailAddress { get; set; }
        public String Phone { get; set; }
        public String Gender { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String Country { get; set; }
        public String State { get; set; }
        public String City { get; set; }
        public String Zipcode { get; set; }
        public String County { get; set; }
        public String MiddleName { get; set; }
        //UAT-2247
        public Boolean IsInternationalPhoneNumber { get; set; }
    }
}
