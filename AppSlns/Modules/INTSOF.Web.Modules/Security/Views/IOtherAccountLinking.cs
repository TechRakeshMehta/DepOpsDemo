using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.IntsofSecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public interface IOtherAccountLinking
    {
        Int32 CurrentLoggedinOrgUserID { get; }
        String EmailID { get; set; }
        Tuple<LookupContract, Boolean> ExistingUser { get; set; }
        String UserName { get; set; }
        String Password { get; set; }
        Int32 SourceOrgUserId { get; }
        String SourceUserId { get; }
        String  TargetUserID { get; set; } 
        List<ManageUsersContract> lstUserContract { get; set; }
        Boolean IsApplicantsSharedUser { get; set; }
        String InstructorPageType { get; set; }
    }
}
