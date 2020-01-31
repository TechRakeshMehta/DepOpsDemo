using INTSOF.SharedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using INTSOF.Utils;
using System.Configuration;
using Entity.ClientEntity;
using INTSOF.UI.Contract.SystemSetUp;

namespace CoreWeb.Shell.Views
{
    public class AdditionalAccountVerificationPresenter : Presenter<IAdditionalAccountVerification>
    {

        public void GetOrganizationUserByVerificationCode()
        {
            View.OrganizationUser = SecurityManager.GetOrganizationUserByVerificationCode(View.UsrVerCode);
            if (View.OrganizationUser.IsNotNull())
            {
                View.OrganizationUser.SSN = SecurityManager.GetFormattedString(View.OrganizationUser.OrganizationUserID, false);
                View.OrganizationUserID = View.OrganizationUser.OrganizationUserID;
            }
        }

        public void GetAccountVerificationSettings()
        {
            List<ClientSetting> lstAllClientSettings = GetAllClientSettings();
            ClientSetting accVerificationProcessMainSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_MAIN.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessMainSetting.IsNullOrEmpty())
            {
                View.AccountVerificationMainSetting = accVerificationProcessMainSetting.CS_SettingValue == "1" ? true : false;
            }
            else
            {
                View.AccountVerificationMainSetting = false;
            }

            ClientSetting accVerificationProcessResponseReqdSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_RESPONSE_REQD.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessResponseReqdSetting.IsNullOrEmpty())
            {
                View.AccVerificationProcessResponseReqdSetting = accVerificationProcessResponseReqdSetting.CS_SettingValue == "1" ? true : false;
            }
            else
            {
                View.AccVerificationProcessResponseReqdSetting = false;
            }

            ClientSetting accVerificationProcessDOBSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_DOB_PERMISSION.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessDOBSetting.IsNullOrEmpty())
            {
                View.AccVerificationProcessDOBSetting = accVerificationProcessDOBSetting.CS_SettingValue == "1" ? true : false;
            }
            else
            {
                View.AccVerificationProcessDOBSetting = false;
            }

            ClientSetting accVerificationProcessSSNSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_SSN_PERMISSION.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessSSNSetting.IsNullOrEmpty())
            {
                View.AccVerificationProcessSSNSetting = accVerificationProcessSSNSetting.CS_SettingValue == "1" ? true : false;
            }
            else
            {
                View.AccVerificationProcessSSNSetting = false;
            }

            ClientSetting accVerificationProcessLSSNSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_LSSN_PERMISSION.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessLSSNSetting.IsNullOrEmpty())
            {
                View.AccVerificationProcessLSSNSetting = accVerificationProcessLSSNSetting.CS_SettingValue == "1" ? true : false;
            }
            else
            {
                View.AccVerificationProcessLSSNSetting = false;
            }

            ClientSetting accVerificationProcessProfCustAttrSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_PROF_CUST_ATTR_PERMISSION.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessProfCustAttrSetting.IsNullOrEmpty())
            {
                View.AccVerificationProcessProfCustAttrSetting = accVerificationProcessProfCustAttrSetting.CS_SettingValue == "1" ? true : false;
            }
            else
            {
                View.AccVerificationProcessProfCustAttrSetting = false;
            }

            ClientSetting accVerificationProcessDOBTextSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_DOB_OVERRIDE_TEXT.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessDOBTextSetting.IsNullOrEmpty())
            {
                View.AccVerificationProcessDOBTextSetting = accVerificationProcessDOBTextSetting.CS_SettingValue;
            }
            else
            {
                View.AccVerificationProcessDOBTextSetting = String.Empty;
            }

            ClientSetting accVerificationProcessSSNTextSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_SSN_OVERRIDE_TEXT.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessSSNTextSetting.IsNullOrEmpty())
            {
                View.AccVerificationProcessSSNTextSetting = accVerificationProcessSSNTextSetting.CS_SettingValue;
            }
            else
            {
                View.AccVerificationProcessSSNTextSetting = String.Empty;
            }

            ClientSetting accVerificationProcessLSSNTextSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_LSSN_OVERRIDE_TEXT.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessLSSNTextSetting.IsNullOrEmpty())
            {
                View.AccVerificationProcessLSSNTextSetting = accVerificationProcessLSSNTextSetting.CS_SettingValue;
            }
            else
            {
                View.AccVerificationProcessLSSNTextSetting = String.Empty;
            }

            ClientSetting accVerificationProcessProfCustAttrTextSetting = lstAllClientSettings.Where(x => x.lkpSetting.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_PROF_CUST_ATTR_OVERRIDE_TEXT.GetStringValue()).FirstOrDefault();
            if (!accVerificationProcessProfCustAttrTextSetting.IsNullOrEmpty())
            {
                View.AccVerificationProcessProfCustAttrTextSetting = accVerificationProcessProfCustAttrTextSetting.CS_SettingValue;
            }
            else
            {
                View.AccVerificationProcessProfCustAttrTextSetting = String.Empty;
            }
        }

        public List<ClientSetting> GetAllClientSettings()
        {
            return ComplianceDataManager.GetClientSetting(View.TenantID);
        }

        public void GetLkpSettings()
        {
            List<String> lstCodes = new List<String>();
            List<ClientSettingCustomAttributeContract> lstClientSettingustAttributes = new List<ClientSettingCustomAttributeContract>();

            if (View.AccVerificationProcessDOBSetting)
            {
                lstCodes.Add(Setting.ACCOUNT_VERIFICATION_PROCESS_DOB_PERMISSION.GetStringValue());
            }
            if (View.AccVerificationProcessSSNSetting)
            {
                lstCodes.Add(Setting.ACCOUNT_VERIFICATION_PROCESS_SSN_PERMISSION.GetStringValue());
            }
            if (View.AccVerificationProcessLSSNSetting)
            {
                lstCodes.Add(Setting.ACCOUNT_VERIFICATION_PROCESS_LSSN_PERMISSION.GetStringValue());
            }
            if (View.AccVerificationProcessProfCustAttrSetting)
            {
                lstClientSettingustAttributes = GetClientSettingCustomAttribute();
            }
            List<lkpSetting> lstQuestions = ComplianceDataManager.GetLkpSetting(View.TenantID).Where(x => lstCodes.Contains(x.Code) && !x.IsDeleted).ToList();

            if (!lstClientSettingustAttributes.IsNullOrEmpty())
            {
                foreach (ClientSettingCustomAttributeContract custAttr in lstClientSettingustAttributes)
                {
                    lkpSetting setting = new lkpSetting();
                    if (custAttr.SettingValue)
                    {
                        setting.Name = custAttr.SettingOverrideText.IsNullOrEmpty() ? custAttr.SettingName : custAttr.SettingOverrideText;
                        setting.Code = Convert.ToString(custAttr.CustomAttributeID); //custAttr.CustomAttributeDatatypeCode;
                        lstQuestions.Add(setting);
                    }
                }
            }

            foreach (lkpSetting item in lstQuestions)
            {
                if (item.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_DOB_PERMISSION.GetStringValue())
                {
                    item.Name = View.AccVerificationProcessDOBTextSetting.IsNullOrEmpty() ? "Date of Birth" : View.AccVerificationProcessDOBTextSetting;
                }
                if (item.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_SSN_PERMISSION.GetStringValue())
                {
                    item.Name = View.AccVerificationProcessSSNTextSetting.IsNullOrEmpty() ? "SSN" : View.AccVerificationProcessSSNTextSetting;
                }
                if (item.Code == Setting.ACCOUNT_VERIFICATION_PROCESS_LSSN_PERMISSION.GetStringValue())
                {
                    item.Name = View.AccVerificationProcessLSSNTextSetting.IsNullOrEmpty() ? "Last four SSN" : View.AccVerificationProcessLSSNTextSetting;
                }
            }

            View.LstSettings = lstQuestions;
        }

        public List<ClientSettingCustomAttributeContract> GetClientSettingCustomAttribute()
        {
            return ComplianceDataManager.GetClientSettingCustomAttribute(View.TenantID).ToList();
        }

        public void ActivateUser()
        {
            Entity.OrganizationUser orgUser = SecurityManager.GetOrganizationUserByVerificationCode(View.UsrVerCode);
            orgUser.IsActive = true;
            if (orgUser.ActiveDate == null && orgUser.IsApplicant == true)
                orgUser.ActiveDate = DateTime.Now;

            SecurityManager.UpdateOrganizationUser(orgUser);
        }

        public void GetProfileCustomAttributeByOrgUserID()
        {
            View.lstCustomAttrUserData = ComplianceDataManager.GetCustomAttributes(AppConsts.NONE, AppConsts.NONE, CustomAttributeUseTypeContext.Profile.GetStringValue(), View.OrganizationUserID, View.TenantID);
        }


    }
}
