using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class AssignmentPropertiesDetailPresenter : Presenter<IAssignmentPropertiesDetailView>
    {

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public void GetAssignmentPropertyDetails()
        {
            View.AssignmentPropertyDetails = ComplianceSetupManager.GetAssignmentPropertyDetails(View.CurrentDataID, View.ParentCategoryDataID, View.ParentPackageDataID, View.ParentItemDataID, View.TenantId,
                View.CurrentRuleSetTreeTypeCode);
        }

        public void BindEditableBy()
        {
            var editableByList = ComplianceSetupManager.GetComplianceEditableBy(View.TenantId);
            View.ApplicantEditableByID = editableByList.FirstOrDefault(x => x.Code == LkpEditableBy.Applicant).ComplianceItemEditableByID;

            //UAT-3806
            //Applicant Editable By should not be shown for Attribute
            //if (View.CurrentRuleSetTreeTypeCode == RuleSetTreeNodeType.Attribute)
            //{
            //    editableByList = editableByList.Where(x => x.Code != LkpEditableBy.Applicant).ToList();
            //}
            //View.lstEditableBy = editableByList;
            View.lstEditableBy = editableByList;
        }

        public void BindReviwedBy()
        {
            View.lstReviewerType = ComplianceSetupManager.GetComplianceReviwedBy(View.TenantId);
        }

        public void BindThirdPartyReviewer()
        {
            View.LstThirdPartyReviewer = ComplianceSetupManager.GetThirdPartyReviewers(View.TenantId, View.CurrentDataID);
        }

        public void UpdateAssignmentProperties(AssignmentProperty assignmentProperty)
        {
            ComplianceSetupManager.UpdateAssignmentProperties(assignmentProperty, View.CurrentDataID, View.ParentPackageDataID, View.ParentCategoryDataID, View.ParentItemDataID, View.CurrentRuleSetTreeTypeCode, View.CurrentLoggedInUserId, View.TenantId);
        }

        public void BindThirdPartyUser()
        {
            View.LstThirdPartyUser = SecurityManager.GetOganisationUsersByTanentId(View.selectedReviewerId);
        }

        /// <summary>
        /// UAT-3240
        /// </summary>
        /// <returns></returns>
        public Boolean IsDisabledBothCategoryAndItemExceptionsForTenant()
        {
            Entity.ClientEntity.ClientSetting clientsettings = ComplianceDataManager.GetClientSetting(View.TenantId, Setting.DISABLE_CATEGORY_AND_ITEM_EXCEPTIONS.GetStringValue());
            if (clientsettings.IsNotNull())
            {
                return (!String.IsNullOrEmpty(clientsettings.CS_SettingValue) && clientsettings.CS_SettingValue == AppConsts.STR_ONE) ? true : false;
            }
            else
            {
                return false;
            }
        }
    }
}




