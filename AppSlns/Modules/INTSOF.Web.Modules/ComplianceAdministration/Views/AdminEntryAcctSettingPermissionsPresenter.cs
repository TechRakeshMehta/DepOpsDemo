using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class AdminEntryAcctSettingPermissionsPresenter : Presenter<IAdminEntryAcctSettingPermissionsView>
    {
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
            GetAdminEntryAccountSetting();
        }

        public void GetApplicantInviteSubmitStatus()
        {
            List<lkpApplicantInviteSubmitStatusType> listApplicantInviteSubmitStatus = LookupManager.GetLookUpData<lkpApplicantInviteSubmitStatusType>(View.TenantId).Where(x => x.AISST_IsDeleted == false).ToList();
            View.lstApplicantInviteSubmitStatus = listApplicantInviteSubmitStatus;
        }
        public void GetAdminEntryAccountSetting()
        {
            List<lkpAdminEntryAccountSetting> listAdminEntryAccountSetting = LookupManager.GetLookUpData<lkpAdminEntryAccountSetting>(View.TenantId).Where(x => x.AEAS_IsDeleted == false).ToList();
            View.lstAdminEntryAccountSetting = listAdminEntryAccountSetting;
        }

        public List<DeptProgramAdminEntryAcctSetting> GetDeptProgramAdminEntryAcctSettings()
        {
            List<DeptProgramAdminEntryAcctSetting> listDeptProgramAdminEntryAcctSettings = new List<DeptProgramAdminEntryAcctSetting>();
            listDeptProgramAdminEntryAcctSettings = ComplianceSetupManager.GetDeptProgramAdminEntryAcctSettings(View.TenantId, View.DeptProgramMappingID);
            return listDeptProgramAdminEntryAcctSettings;
        }

        public void SaveNodeSettingsForAdminEntry(Dictionary<String, String> dicSettings)
        {
            List<DeptProgramAdminEntryAcctSetting> deptProgramAdminEntryAcctSettingList = new List<DeptProgramAdminEntryAcctSetting>();
            if (!dicSettings.IsNullOrEmpty() && dicSettings.Count > 0)
            {
                foreach (var item in dicSettings)
                {
                    DeptProgramAdminEntryAcctSetting deptProgramAdminEntryAcctSetting = new DeptProgramAdminEntryAcctSetting();
                    deptProgramAdminEntryAcctSetting.DPAEAS_AdminEntryAccountSettingID = View.lstAdminEntryAccountSetting.Where(x => x.AEAS_IsDeleted == false && x.AEAS_Code == item.Key).Select(x => x.AEAS_ID).FirstOrDefault();
                    deptProgramAdminEntryAcctSetting.DPAEAS_DeptProgramMappingID = View.DeptProgramMappingID;
                    deptProgramAdminEntryAcctSetting.DPAEAS_SettingValue = item.Value;
                    deptProgramAdminEntryAcctSetting.DPAEAS_IsDeleted = false;
                    deptProgramAdminEntryAcctSetting.DPAEAS_CreatedBy = View.CurrentLoggedInUserId;
                    deptProgramAdminEntryAcctSettingList.Add(deptProgramAdminEntryAcctSetting);
                }
            }             
            if (!deptProgramAdminEntryAcctSettingList.IsNullOrEmpty() && deptProgramAdminEntryAcctSettingList.Count > 0)
            {
                Boolean isSaved = ComplianceSetupManager.SaveNodeSettingsForAdminEntry(View.TenantId, View.DeptProgramMappingID, deptProgramAdminEntryAcctSettingList);
                if (isSaved)
                {
                    View.SuccessMessage = "Admin Entry Portal Settings saved successfully.";
                }
                else
                {
                    View.ErrorMessage = "Some error occurred. Please try again.";
                }
            }            
        }
    }
}
