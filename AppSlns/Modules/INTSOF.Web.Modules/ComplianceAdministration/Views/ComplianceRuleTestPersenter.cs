using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ComplianceRuleTestPersenter : Presenter<IComplianceRuleTestView>
    {
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
        }

        public void GetComplianceRuleData()
        {
            View.ComplianceRuleData = ComplianceSetupManager.GetDetailsForComplianceRuleTest(View.RuleMappingID, View.SelectedTenantId);
            List<String> lstStatusCodeToBeExcluded = new List<String>();
            lstStatusCodeToBeExcluded.Add(ApplicantItemComplianceStatus.Pending_Review_For_Client.GetStringValue());
            lstStatusCodeToBeExcluded.Add(ApplicantItemComplianceStatus.Pending_Review_For_Third_Party.GetStringValue());
            lstStatusCodeToBeExcluded.Add(ApplicantItemComplianceStatus.Exception_Rejected.GetStringValue());
            lstStatusCodeToBeExcluded.Add(ApplicantItemComplianceStatus.Exception_Approved.GetStringValue());
            lstStatusCodeToBeExcluded.Add(ApplicantItemComplianceStatus.Approved_With_Exception.GetStringValue());
            lstStatusCodeToBeExcluded.Add(ApplicantItemComplianceStatus.Applied_For_Exception.GetStringValue());
            View.LstItemComplianceStatus = ComplianceDataManager.GetItemComplianceStatus(View.SelectedTenantId)
                                                                .Where(cond => !lstStatusCodeToBeExcluded.Contains(cond.Code)).ToList();
        }

        public String TestComplianceRule( String inputXml)
        {
           return RuleManager.TestComplianceRule(View.SelectedTenantId, inputXml);
        }

        public Boolean IsActionTypeDueDate()
        {
            return RuleManager.IsActionTypeDueDate(View.SelectedTenantId, View.RuleActionTypeID);
        }

        public String CalculateDueDate(String resultXml)
        {
            return RuleManager.CalculateDueDate(View.SelectedTenantId,resultXml);
        }
    }
}
