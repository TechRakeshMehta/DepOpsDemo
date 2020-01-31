using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using System.Data.Entity.Core.Objects;
using INTSOF.Utils;
using System.Linq;

namespace CoreWeb.ComplianceOperations.Views
{
    public class VerificationItemDataPanelPresenter : Presenter<IVerificationItemDataPanelView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceOperationsController _controller;
        // public VerificationItemDataPanelPresenter([CreateNew] IComplianceOperationsController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetComplianceItemData()
        {
            View.lst = ComplianceDataManager.GetApplicantDataForVerification(View.SelectedComplianceCategoryId_Global, View.CurrentPackageSubscriptionID_Global, View.SelectedTenantId_Global);
        }
        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.CurrentTenantId_Global == SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }
        ///// <summary>
        ///// gets all the applicant document.
        ///// </summary>
        //public void GetApplicantDocuments()
        //{
        //    //if (View.OrganizationUserData != null)
        //    if (!View.SelectedApplicantId_Global.IsNullOrEmpty() && View.SelectedApplicantId_Global > AppConsts.NONE)
        //        View.lstApplicantDocument = ComplianceDataManager.GetApplicantDocuments(View.SelectedApplicantId_Global, View.SelectedTenantId_Global);
        //}

        public void UpdateApplicantCategoryNotes(String notes, Int32 applicantCategoryDataId)
        {
            ComplianceDataManager.UpdateApplicantCategoryNotes(applicantCategoryDataId, notes, View.CurrentLoggedInUserId, View.SelectedTenantId_Global);
        }
        public ComplianceCategory GetCategoryData()
        {
            return ComplianceDataManager.GetComplianceCategoryDetails(View.SelectedComplianceCategoryId_Global, View.SelectedTenantId_Global);
        }
        //UAT-3566
        public String GetCategoryRuleExecutionDate()
        {
            Int32? associationHierarchyId = null;
            String date = String.Empty;
            associationHierarchyId = ComplianceSetupManager.getAssociationHierarchyIdForObject(View.CurrentLoggedInUserId, View.SelectedTenantId_Global, View.SelectedCompliancePackageId_Global, View.SelectedComplianceCategoryId_Global);
            if (associationHierarchyId != null)
                date = Convert.ToString(ComplianceSetupManager.GetLastRuleAppliedDate(View.SelectedTenantId_Global,associationHierarchyId.Value));
            return date;
        }
        /// <summary>
        /// return the info about last updated the category.
        /// </summary>
        /// <param name="CategoryID">CategoryID</param>
        /// <returns></returns>
        public List<CatUpdatedByLatestInfo> GetCatUpdatedByLatestInfo(Int32 CategoryID)
        {
            return ComplianceDataManager.GetCatUpdatedByLatestInfo(CategoryID, View.CurrentPackageSubscriptionID_Global, View.SelectedTenantId_Global);
        }

        public Boolean SaveUpdateUtilityFeatureUsage(Boolean isButtonClicked, String utilityFeatureCode)
        {
            return ComplianceDataManager.SaveUpdateUtilityFeatureUsage(View.CurrentLoggedInUserId, !isButtonClicked, utilityFeatureCode);
        }

        public Entity.UtilityFeatureUsage GetIgnoreAlertStatus()
        {
            return ComplianceDataManager.GetIgnoreAlertStatus(View.CurrentLoggedInUserId, UtilityFeatures.Dock_UnDock.GetStringValue());
        }

        //UAT-613 show Explanatory Note on the basis of last State.
        public String GetExplanatoryNoteState(String userId)
        {
            Guid userIdTemp = new Guid(userId);
            String explanatoryNoteState = AppConsts.ADMIN_EXPLANATORY_STATE;
            Entity.aspnet_PersonalizationPerUser objPersonalizedPerUser = ComplianceDataManager.GetExplanatoryState(userIdTemp);
            if (!objPersonalizedPerUser.IsNullOrEmpty() && objPersonalizedPerUser.ExplanationTabState.IsNotNull())
            {
                if (objPersonalizedPerUser.ExplanationTabState.Trim().ToLower().Equals(AppConsts.ADMIN_EXPLANATORY_STATE.Trim().ToLower()))
                {
                   explanatoryNoteState = "spnAdminExplanation";
                    // UAT-4436 : Update default tab on verification details screen for Category Information to be "Applicant's Explanation" (current label), and update tab labels
                    // To show Applicant tab first in tri-panel screens.
                    //explanatoryNoteState = "spnApplicantExplanation"; UAT:4711
                }
                else if (objPersonalizedPerUser.ExplanationTabState.Trim().ToLower().Equals(AppConsts.APPLICANT_EXPLANATORY_STATE.Trim().ToLower()))
                {
                    explanatoryNoteState = "spnApplicantExplanation";
                }
                else if (objPersonalizedPerUser.ExplanationTabState.Trim().ToLower().Equals(AppConsts.CLOSED_EXPLANATORY_STATE.Trim().ToLower()))
                {
                    explanatoryNoteState = "closedState";
                }
            }
            return explanatoryNoteState;
        }

        #region UAT:719 Check Exceptions turned off for a Category/Item
        /// <summary>
        /// To check wheather exception is allowed turned off for a Category/item
        /// </summary>
        /// <returns>Boolean</returns>
        public Boolean IsAllowExceptionOnCategory()
        {
            return ComplianceDataManager.IsAllowExceptionOnCategory(View.SelectedTenantId_Global, View.SelectedCompliancePackageId_Global, View.SelectedComplianceCategoryId_Global);
        }
        #endregion

        //UAT-3599
        public Boolean IsStudentDataEntryEnable()
        {
            List<ListItemEditableBies> lstSDE = ComplianceSetupManager.GetEditableBiesByCategoryId(View.SelectedCompliancePackageId_Global, View.SelectedComplianceCategoryId_Global, View.SelectedTenantId_Global);
            Boolean isNotEditableByAdmin = lstSDE.Any(cond => cond.EditableByCode == "EDTAPCT") ? false : true;
            return isNotEditableByAdmin;
        }

    }
}




