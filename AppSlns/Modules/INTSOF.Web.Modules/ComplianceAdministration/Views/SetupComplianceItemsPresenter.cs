using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class SetupComplianceItemsPresenter : Presenter<ISetupComplianceItemsView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetTenants()
        {
            View.ListTenants = ComplianceDataManager.GetMasterAndInstitutionTypeTenants(View.DefaultTenantId);
        }

        public void GetComplianceIteme()
        {
            Boolean getTenantName = View.DefaultTenantId.Equals(View.SelectedTenantId);
            if (getTenantName)
            {
                View.lstComplianceItems = ComplianceSetupManager.GetComplianceItems(View.SelectedTenantId, getTenantName);
            }
            else
            {
                View.lstComplianceItems = ComplianceSetupManager.GetComplianceItemsForNodes(View.SelectedTenantId, View.DeptProgramMappingID);
            }
        }

        public void DeleteComplianceItem()
        {
            IntegrityCheckResponse response = IntegrityManager.IfItemCanBeDeleted(View.ViewContract.ComplianceItemId, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                ComplianceItem cmpItem = ComplianceSetupManager.getCurrentItemInfo(View.ViewContract.ComplianceItemId, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, cmpItem.Name);
                View.CurrentViewContext.IsOperationSuccessful = false;
            }
            else
            {
                View.CurrentViewContext.IsOperationSuccessful = ComplianceSetupManager.DeleteComplianceItem(View.ViewContract.ComplianceItemId, View.CurrentLoggedInUserId, View.SelectedTenantId);
            }
        }

        public void SaveComplianceItem()
        {
            if (ComplianceSetupManager.CheckIfItemAlreadyExist(View.ViewContract.Name, View.ViewContract.ComplianceItemId, View.SelectedTenantId))
            {
                View.ErrorMessage = "Item Name can not be duplicate.";
            }
            else
            {
                ComplianceItem complianceItem = new ComplianceItem
                {
                    Name = View.ViewContract.Name,
                    Description = View.ViewContract.Description,
                    Details=View.ViewContract.Details,
                    ItemLabel = View.ViewContract.ItemLabel,
                    ScreenLabel = View.ViewContract.ScreenLabel,
                    EffectiveDate = View.ViewContract.EffectiveDate,
                    IsActive = View.ViewContract.IsActive,
                    SampleDocFormURL = View.ViewContract.SampleDocFormURL,
                    ComplianceItemID = View.ViewContract.ComplianceItemId,
                    TenantID = View.SelectedTenantId,
                    IsCreatedByAdmin = View.TenantId == SecurityManager.DefaultTenantID ? true : false,
                     //UAT-3077
                    Amount = View.ViewContract.Amount,
                    IsPaymentType = View.ViewContract.IsPaymentType
                };
                AddDocumentUrlDetails(complianceItem);
                if (View.ViewContract.ComplianceItemId > 0)
                {
                    complianceItem.ModifiedBy = View.CurrentLoggedInUserId;
                    View.CurrentViewContext.IsOperationSuccessful = ComplianceSetupManager.UpdateComplianceItem(complianceItem, View.ViewContract.ExplanatoryNotes, true);
                }
                else
                {
                    complianceItem.CreatedBy = View.CurrentLoggedInUserId;
                    ComplianceSetupManager.SaveComplianceItem(complianceItem, View.ViewContract.ExplanatoryNotes);
                }
            }
        }

        private void AddDocumentUrlDetails(ComplianceItem newItem)
        {
            if (View.ViewContract.DocumentUrls.IsNotNull())
            {
                foreach (DocumentUrlContract tempComplianceItemDocUrl in View.ViewContract.DocumentUrls)
                {
                    ComplianceItemDocUrl documentUrl = new ComplianceItemDocUrl();
                    documentUrl.ComplianceItemDocUrlID = tempComplianceItemDocUrl.ID;
                    documentUrl.SampleDocFormURL = tempComplianceItemDocUrl.SampleDocFormURL;
                    documentUrl.SampleDocFormDisplayURLLabel = tempComplianceItemDocUrl.SampleDocFormUrlDisplayLabel;
                    documentUrl.ModifiedByID = View.CurrentLoggedInUserId;
                    documentUrl.ModifiedOn = DateTime.Now;
                    documentUrl.CreatedByID = View.CurrentLoggedInUserId;
                    documentUrl.CreatedOn = DateTime.Now;
                    documentUrl.IsDeleted = false;
                    newItem.ComplianceItemDocUrls.Add(documentUrl);
                }
            }
        }

        public void GetLargeContent()
        {
            LargeContent notesRecord = ComplianceSetupManager.getLargeContentRecord(View.ViewContract.ComplianceItemId, LCObjectType.ComplianceItem.GetStringValue(), LCContentType.ExplanatoryNotes.GetStringValue(), View.SelectedTenantId);
            if (notesRecord != null)
                View.ViewContract.ExplanatoryNotes = notesRecord.LC_Content;
        }

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }

        /// <summary>
        /// Check if item can be updated or not
        /// </summary>
        /// <returns></returns>
        public Boolean IfItemCanBeupdated()
        {
            IntegrityCheckResponse response = IntegrityManager.IfItemCanBeUpdated(View.ViewContract.ComplianceItemId, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                ComplianceItem cmpItem = ComplianceSetupManager.getCurrentItemInfo(View.ViewContract.ComplianceItemId, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, cmpItem.Name);
                return false;
            }
            return true;
        }
    }
}




