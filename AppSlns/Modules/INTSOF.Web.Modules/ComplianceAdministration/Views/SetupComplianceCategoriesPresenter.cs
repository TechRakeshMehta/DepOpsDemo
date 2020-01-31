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
    public class SetupComplianceCategoriesPresenter : Presenter<ISetupComplianceCategoriesView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void GetTenants()
        {
            View.ListTenants = ComplianceDataManager.GetMasterAndInstitutionTypeTenants(View.DefaultTenantId);
        }

        /// <summary>
        /// Gets all the compliance cotegories for th given tanent ID.
        /// </summary>
        public void GetComplianceCategories()
        {
            Boolean getTenantName = View.DefaultTenantId.Equals(View.SelectedTenantId);
            if (getTenantName)
            {
                View.ComplianceCategories = ComplianceSetupManager.GetComplianceCategories(View.SelectedTenantId, getTenantName);
            }
            else
            {
                View.ComplianceCategories = ComplianceSetupManager.GetComplianceCategoriesForNodes(View.SelectedTenantId,View.DeptProgramMappingID);
            }
            
        }

        /// <summary>
        /// Saves the Compliance Category in th MasterDB followed by ClientDB(in case of client).
        /// </summary>
        public void SaveComplianceCategory()
        {
            if (ComplianceSetupManager.CheckIfCategoryNameAlreadyExist(View.ViewContract.CategoryName, View.ViewContract.ComplianceCategoryId, View.SelectedTenantId))
            {
                View.ErrorMessage = "Category Name can not be duplicate.";
            }
            else
            {
                ComplianceCategory newCategory = new ComplianceCategory
                {
                    CategoryName = View.ViewContract.CategoryName,
                    CategoryLabel = View.ViewContract.CategoryLabel,
                    ScreenLabel = View.ViewContract.ScreenLabel,
                    Description = View.ViewContract.Description,
                    //SampleDocFormURL = View.ViewContract.SampleDocFormURL,
                    TenantID = View.SelectedTenantId,
                    IsActive = View.ViewContract.Active,
                    IsCreatedByAdmin = View.TenantId == SecurityManager.DefaultTenantID ? true : false,
                    SendItemDocOnApproval = View.ViewContract.SendItemDoconApproval ,//UAT-3805
                    //SampleDocFormURLLabel = View.ViewContract.MoreInfoText //UAT-3161
                };

                if (newCategory.TenantID != View.DefaultTenantId)
                {
                    newCategory.TriggerOtherCategoryRules = View.ViewContract.TriggerOtherCategoryRules;
                }

                //Dictionary notesDetail will contain Content type as keys and content value as values. 
                Dictionary<String, String> notesDetail = new Dictionary<String, String>();
                notesDetail.Add(LCContentType.ExplanatoryNotes.GetStringValue(), View.ViewContract.ExplanatoryNotes);
                //Add Compliance Category DocumentUrls
                AddDocumentUrlDetails(newCategory);
                ComplianceSetupManager.SaveCategoryDetail(newCategory, View.CurrentLoggedInUserId, notesDetail);
            }
        }

        private void AddDocumentUrlDetails(ComplianceCategory newCategory)
        {
            if (View.ViewContract.DocumentUrls.IsNotNull())
            {
                foreach (DocumentUrlContract tempComplianceCategoryDocUrl in View.ViewContract.DocumentUrls)
                {
                    ComplianceCategoryDocUrl documentUrl = new ComplianceCategoryDocUrl();

                        documentUrl.SampleDocFormURL = tempComplianceCategoryDocUrl.SampleDocFormURL;
                        documentUrl.SampleDocFormURLLabel = tempComplianceCategoryDocUrl.SampleDocFormURLLabel;
                        documentUrl.ModifiedByID = View.CurrentLoggedInUserId;
                        documentUrl.ModifiedOn = DateTime.Now;
                        documentUrl.CreatedByID = View.CurrentLoggedInUserId;
                        documentUrl.CreatedOn = DateTime.Now;
                        documentUrl.IsDeleted = false;
                        newCategory.ComplianceCategoryDocUrls.Add(documentUrl);
                }
            }
        }

        /// <summary>
        /// Updates the Compliance Category.
        /// </summary>
        public void UpdateComplianceCategory()
        {
            if (ComplianceSetupManager.CheckIfCategoryNameAlreadyExist(View.ViewContract.CategoryName, View.ViewContract.ComplianceCategoryId, View.SelectedTenantId))
            {
                View.ErrorMessage = "Category Name can not be duplicate.";
            }
            else
            {
                ComplianceCategory categoryDetail = new ComplianceCategory();
                categoryDetail.ComplianceCategoryID = View.ViewContract.ComplianceCategoryId;
                categoryDetail.CategoryName = View.ViewContract.CategoryName;
                categoryDetail.CategoryLabel = View.ViewContract.CategoryLabel;
                categoryDetail.ScreenLabel = View.ViewContract.ScreenLabel;
                categoryDetail.Description = View.ViewContract.Description;
                //categoryDetail.SampleDocFormURL = View.ViewContract.SampleDocFormURL;
                categoryDetail.IsActive = View.ViewContract.Active;
                categoryDetail.TenantID = View.SelectedTenantId;
                categoryDetail.SendItemDocOnApproval = View.ViewContract.SendItemDoconApproval; //UAT-3805
                //categoryDetail.ComplianceCategoryDocUrls = new List<ComplianceCategoryDocUrl>();
                if (!View.ViewContract.DocumentUrls.IsNullOrEmpty())
                {
                    foreach (DocumentUrlContract documenturlContract in View.ViewContract.DocumentUrls)
                    {
                        ComplianceCategoryDocUrl documentUrl = new ComplianceCategoryDocUrl();
                        documentUrl.SampleDocFormURL = documenturlContract.SampleDocFormURL;
                        documentUrl.SampleDocFormURLLabel = documenturlContract.SampleDocFormURLLabel;
                        documentUrl.ComplianceCategoryDocUrlID = documenturlContract.ID;
                        categoryDetail.ComplianceCategoryDocUrls.Add(documentUrl);
                    }
                }

                //categoryDetail.SampleDocFormURLLabel = View.ViewContract.MoreInfoText; //UAT-3161
                if (categoryDetail.TenantID != View.DefaultTenantId)
                {
                    categoryDetail.TriggerOtherCategoryRules = View.ViewContract.TriggerOtherCategoryRules;
                }
                //Dictionary notesDetail will contain Content type as keys and content value as values. 
                Dictionary<String, String> notesDetail = new Dictionary<String, String>();
                notesDetail.Add(LCContentType.ExplanatoryNotes.GetStringValue(), View.ViewContract.ExplanatoryNotes);
                ComplianceSetupManager.UpdateCategoryDetail(categoryDetail, View.CurrentLoggedInUserId, notesDetail, true);
            }
        }

        /// <summary>
        /// Deletes the Compliance Category.
        /// </summary>
        public Boolean DeleteComplianceCategory()
        {
            IntegrityCheckResponse response = IntegrityManager.IfCategoryCanBeDeleted(View.ViewContract.ComplianceCategoryId, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                ComplianceCategory cmpCategory = ComplianceSetupManager.getCurrentCategoryInfo(View.ViewContract.ComplianceCategoryId, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, cmpCategory.CategoryName);
                return false;
            }
            else
            {
                return ComplianceSetupManager.DeleteComplianceCategory(View.ViewContract.ComplianceCategoryId, View.CurrentLoggedInUserId, View.SelectedTenantId);
            }
        }

        /// <summary>
        /// Gets the value of large content for the given object id and assigns the value in the corresponding field.
        /// </summary>
        public void GetLargeContent()
        {
            LargeContent notesRecord = ComplianceSetupManager.getLargeContentRecord(View.ViewContract.ComplianceCategoryId, LCObjectType.ComplianceCategory.GetStringValue(), LCContentType.ExplanatoryNotes.GetStringValue(), View.SelectedTenantId);
            if (notesRecord != null)
                View.ViewContract.ExplanatoryNotes = notesRecord.LC_Content;
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
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

        public Boolean IsDefaultTenant()
        {
            //Checked if logged user is admin or not.
            if (View.SelectedTenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if package can be updated or not
        /// </summary>
        /// <returns></returns>
        public Boolean IfCategoryCanBeupdated()
        {
            IntegrityCheckResponse response = IntegrityManager.IfCategoryCanBeUpdated(View.ViewContract.ComplianceCategoryId, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                ComplianceCategory cmpCategory = ComplianceSetupManager.getCurrentCategoryInfo(View.ViewContract.ComplianceCategoryId, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, cmpCategory.CategoryName);
                return false;
            }
            return true;
        }
       
    }
}




