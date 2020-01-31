using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IAdminEntryAcctSettingPermissionsView
    {
        Int32 CurrentLoggedInUserId { get; }
        Int32 TenantId { get; set; }
        Int32 DefaultTenantId { get; set; }
        Int32 DeptProgramMappingID { get; set; }              
        List<lkpApplicantInviteSubmitStatusType> lstApplicantInviteSubmitStatus { get; set; }
        List<lkpAdminEntryAccountSetting>  lstAdminEntryAccountSetting { get; set; }
        String ErrorMessage { get; set; }
        String SuccessMessage { get; set; }
        String InfoMessage { get; set; }

    }
}
