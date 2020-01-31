using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class CategoryInfoPresenter : Presenter<ICategoryInfoView>
    {

        public override void OnViewLoaded()
        {
            //View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void getCurrentCategoryInfo()
        {
            View.complianceCategories = ComplianceSetupManager.getCurrentCategoryInfo(View.currentCategoryId, View.SelectedTenantId);
        }

        public void saveCurrentCategoryInfo()
        {
            if (ComplianceSetupManager.CheckIfCategoryNameAlreadyExist(View.ViewContract.CategoryName, View.ViewContract.ComplianceCategoryId, View.SelectedTenantId))
            {
                View.ErrorMessage = "Category Name can not be duplicate.";
            }
            else
            {
                ComplianceCategory newCategory = new ComplianceCategory
                {
                    ComplianceCategoryID = View.ViewContract.ComplianceCategoryId,
                    CategoryName = View.ViewContract.CategoryName,
                    CategoryLabel = View.ViewContract.CategoryLabel,
                    ScreenLabel = View.ViewContract.ScreenLabel,
                    Description = View.ViewContract.Description,
                    TenantID = View.SelectedTenantId,
                    IsActive = View.ViewContract.Active,
                    TriggerOtherCategoryRules = View.ViewContract.TriggerOtherCategoryRules,
                    SendItemDocOnApproval = View.ViewContract.SendItemDoconApproval //UAT-3805
                };
                //Dictionary notesDetail will contain Content type as keys and content value as values. 
                Dictionary<String, String> notesDetail = new Dictionary<String, String>();
                notesDetail.Add(LCContentType.ExplanatoryNotes.GetStringValue(), View.ViewContract.ExplanatoryNotes);
                ComplianceSetupManager.UpdateCategoryDetail(newCategory, View.CurrentLoggedInUserId, notesDetail);
                //UAT-2305:
                //SaveUniversalCategoryMapping();
            }
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 getTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
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

        public void UpdateCompliancePackageCategoryDisplayOrder()
        {
            CompliancePackageCategory compliancePackageCategory = new CompliancePackageCategory();
            compliancePackageCategory.CPC_PackageID = View.PackageId;
            compliancePackageCategory.CPC_CategoryID = View.ViewContract.ComplianceCategoryId;
            compliancePackageCategory.CPC_ComplianceRequired = View.ViewContract.ComplianceRequired;
            compliancePackageCategory.CPC_ComplianceRqdStartDate = View.ViewContract.CmplncRqdStartDate;
            compliancePackageCategory.CPC_ComplianceRqdEndDate = View.ViewContract.CmplncRqdEndDate;
            compliancePackageCategory.CPC_DisplayOrder = View.ViewContract.DisplayOrder;

            CompliancePackageCategory oldcompliancePackageCategory = new CompliancePackageCategory();
            oldcompliancePackageCategory.CPC_PackageID = View.PackageId;
            oldcompliancePackageCategory.CPC_CategoryID = View.ViewContract.ComplianceCategoryId;
            oldcompliancePackageCategory.CPC_ComplianceRequired = View.PreviousComplianceRequired;
            oldcompliancePackageCategory.CPC_ComplianceRqdStartDate = View.PreviousCmplncRqdStartDate;
            oldcompliancePackageCategory.CPC_ComplianceRqdEndDate = View.PreviousCmplncRqdEndDate;
            Boolean result = ComplianceSetupManager.UpdateCompliancePackageCategoryDisplayOrder(View.SelectedTenantId, View.CurrentLoggedInUserId, compliancePackageCategory, oldcompliancePackageCategory);
            if (result)
            {
                View.PreviousComplianceRequired = View.ViewContract.ComplianceRequired;
                View.PreviousCmplncRqdStartDate = View.ViewContract.CmplncRqdStartDate;
                View.PreviousCmplncRqdEndDate = View.ViewContract.CmplncRqdEndDate;
            }
        }

        public void GetCompliancePackageCategory()
        {
            var CompliancePackageCategory = ComplianceSetupManager.GetCompliancePackageCategory(View.SelectedTenantId, View.ViewContract.ComplianceCategoryId, View.PackageId);
            if (CompliancePackageCategory != null)
            {
                View.ViewContract.CPC_ID = CompliancePackageCategory.CPC_ID;
                View.ViewContract.DisplayOrder = CompliancePackageCategory.CPC_DisplayOrder;
                View.ViewContract.ComplianceRequired = CompliancePackageCategory.CPC_ComplianceRequired;
                View.ViewContract.CmplncRqdStartDate = CompliancePackageCategory.CPC_ComplianceRqdStartDate;
                View.ViewContract.CmplncRqdEndDate = CompliancePackageCategory.CPC_ComplianceRqdEndDate;
            }
        }

        public String GetCategoryDissociationStatus()
        {
            return ComplianceSetupManager.GetCategoryDissociationStatus(View.SelectedTenantId, View.currentCategoryId, View.PackageId);
        }

        public Int32 DissociateCategory()
        {
            return ComplianceSetupManager.DissociateCategory(View.SelectedTenantId, View.currentCategoryId, View.SelectedPackageIDs, View.CurrentLoggedInUserId);
        }

        //#region UAT-2305:Tracking to Rotation category/item/attribute mapping
        //public void GetUniversalCategories()
        //{
        //    List<Entity.SharedDataEntity.UniversalCategory> lstUniversalCategory = new List<Entity.SharedDataEntity.UniversalCategory>();
        //    lstUniversalCategory = UniversalMappingDataManager.GetUniversalCategories(View.SelectedTenantId);
        //    lstUniversalCategory.Insert(0, new Entity.SharedDataEntity.UniversalCategory { UC_ID = 0, UC_Name = "--SELECT--" });
        //    View.LstUniversalCategory = lstUniversalCategory;
        //}

        //public void SaveUniversalCategoryMapping()
        //{
        //    if (View.SelectedUniversalCategoryID > AppConsts.NONE)
        //    {
        //        UniversalCategoryMapping ucmObj = new UniversalCategoryMapping();
        //        ucmObj.UCM_ID = View.UniversalCategoryMappingID;
        //        ucmObj.UCM_UniversalCategoryID = View.SelectedUniversalCategoryID;
        //        ucmObj.UCM_CategoryID = View.ViewContract.ComplianceCategoryId;
        //        ucmObj.UCM_CreatedBy = View.CurrentLoggedInUserId;
        //        ucmObj.UCM_CreatedOn = DateTime.Now;
        //        if (View.SelectedUniversalCategoryID != View.MappedUniversalCategoryID && View.UniversalCategoryMappingID > AppConsts.NONE)
        //        {
        //            UniversalMappingDataManager.DeleteCategoryMappingWithUniversalCategory(View.SelectedTenantId, View.UniversalCategoryMappingID
        //                                                                               , UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue(), View.CurrentLoggedInUserId);
        //            ucmObj.UCM_ID = AppConsts.NONE;
        //        }
        //        UniversalMappingDataManager.SaveUpdateCategoryMappingWithUniversalCat(View.SelectedTenantId, ucmObj, UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue());

        //    }
        //    else if (View.UniversalCategoryMappingID > AppConsts.NONE && View.SelectedUniversalCategoryID == AppConsts.NONE)
        //    {
        //        UniversalMappingDataManager.DeleteCategoryMappingWithUniversalCategory(View.SelectedTenantId, View.UniversalCategoryMappingID
        //                                                                               , UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue(), View.CurrentLoggedInUserId);
        //    }
        //    MappedUniversalCategoryData();
        //}

        //public void MappedUniversalCategoryData()
        //{
        //    var mappedData = UniversalMappingDataManager.GetMappedUniversalCategoryDataByID(View.SelectedTenantId, View.ViewContract.ComplianceCategoryId
        //                                                                                    , UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue());
        //    if (!mappedData.IsNullOrEmpty())
        //    {
        //        View.UniversalCategoryMappingID = mappedData.UCM_ID;
        //        View.SelectedUniversalCategoryID = mappedData.UCM_UniversalCategoryID;
        //        View.MappedUniversalCategoryID = mappedData.UCM_UniversalCategoryID;
        //    }
        //}
        //#endregion

        public void GetCompliancePackagesAssociatedtoCat()
        {
            View.lstCompliancePackage = ComplianceSetupManager.GetCompliancePackagesAssociatedtoCat(View.SelectedTenantId,View.currentCategoryId,View.PackageId);
        }

    }
}




