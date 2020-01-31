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
    public class ItemListPresenter : Presenter<IItemListView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceAdministrationController _controller;
        // public ItemListPresenter([CreateNew] IComplianceAdministrationController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public override void OnViewInitialized()
        {
            View.TenantId = SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        // TODO: Handle other view events and set state in the view

        public void GetMasterItems()
        {
            View.CurrentViewContext.lstMasterItems = ComplianceSetupManager.GetAvailableComplianceItems(View.CurrentViewContext.CurrentCategoryId, View.SelectedTenantId);
        }

        public void SaveComplianceItem()
        {
            if (ComplianceSetupManager.CheckIfItemAlreadyExist(View.ViewContract.Name, View.ViewContract.ComplianceItemId, View.SelectedTenantId))
            {
                View.ErrorMessage = String.Format("{0} already exists.", View.ViewContract.Name);
                return;
            }
            else
            {
                ComplianceItem complianceItem = new ComplianceItem
                {
                    Name = View.ViewContract.Name,
                    Description = View.ViewContract.Description,
                    Details = View.ViewContract.Details,
                    ItemLabel = View.ViewContract.ItemLabel,
                    ScreenLabel = View.ViewContract.ScreenLabel,
                    EffectiveDate = View.ViewContract.EffectiveDate,
                    IsActive = View.ViewContract.IsActive,
                    ComplianceItemID = View.ViewContract.ComplianceItemId,
                    CreatedBy = View.ViewContract.CreatedById,
                    TenantID = View.SelectedTenantId,
                    //UAT-3077
                    Amount = View.ViewContract.Amount,
                    IsPaymentType = View.ViewContract.IsPaymentType
                };
                ComplianceCategoryItem complianceCategoryItem = new ComplianceCategoryItem
                {
                    CCI_ItemID = View.ViewContract.CategoryItem.CCI_ItemId,
                    CCI_CategoryID = View.ViewContract.CategoryItem.CCI_CategoryId,
                    CCI_DisplayOrder = View.ViewContract.CategoryItem.CCI_DisplayOrder,
                    CCI_IsDeleted = false,
                    CCI_IsActive = true,
                    CCI_IsCreatedByAdmin = View.TenantId == SecurityManager.DefaultTenantID ? true : false,
                    CCI_CreatedByID = View.CurrentViewContext.CurrentLoggedInUserId,
                    CCI_CreatedOn = DateTime.Now,
                };
                complianceItem.ComplianceCategoryItems.Add(complianceCategoryItem);

                var newComplianceItem = ComplianceSetupManager.SaveComplianceItem(complianceItem, View.ViewContract.ExplanatoryNotes);
                complianceCategoryItem.CCI_ItemID = newComplianceItem.ComplianceItemID;
                //UAT-2305:
                //SaveUniversalItemMapping(complianceCategoryItem.CCI_ID);
                ComplianceSetupManager.CreateItemAssignmentHierarchy(complianceCategoryItem, View.SelectedTenantId);
                View.ErrorMessage = String.Empty;

            }
        }

        public void SaveComplianceCategoryItemMapping()
        {
            ComplianceCategoryItem complianceCategoryItem = new ComplianceCategoryItem
            {
                CCI_ItemID = View.CurrentViewContext.CurrentItemId,
                CCI_CategoryID = View.CurrentViewContext.CurrentCategoryId,
                CCI_DisplayOrder = View.ViewContract.DisplayOrder,
                CCI_IsDeleted = false,
                CCI_IsActive = true,
                CCI_IsCreatedByAdmin = View.TenantId == SecurityManager.DefaultTenantID ? true : false,
                CCI_CreatedByID = View.CurrentViewContext.CurrentLoggedInUserId,
                CCI_CreatedOn = DateTime.Now
                
            };

            ComplianceSetupManager.SaveCategoryItemMapping(complianceCategoryItem, View.SelectedTenantId);

        }

        #region UAT-2305:Tracking to Rotation category/item/attribute mapping
        //public void GetUniversalCategoryItems()
        //{
        //    List<Entity.SharedDataEntity.UniversalItem> lstUniversalItems = new List<Entity.SharedDataEntity.UniversalItem>();
        //    var lstUniversalCategoryItems = UniversalMappingDataManager.GetUniversalItemsByCategoryID(View.SelectedTenantId, View.SelectedUniversalCategoryID);

        //    if (!lstUniversalCategoryItems.IsNullOrEmpty())
        //    {
        //        lstUniversalCategoryItems.ForEach(x =>
        //        {
        //            Entity.SharedDataEntity.UniversalItem universalItem = new Entity.SharedDataEntity.UniversalItem();
        //            universalItem.UI_ID = x.UCIM_ID;
        //            universalItem.UI_Name = x.UniversalItem.UI_Name;
        //            lstUniversalItems.Add(universalItem);
        //        });
        //        lstUniversalItems.Insert(0, new Entity.SharedDataEntity.UniversalItem { UI_ID = 0, UI_Name = "--SELECT--" });
        //        View.LstUniversalItem = lstUniversalItems;
        //    }
        //}

        //public void SaveUniversalItemMapping(Int32 categoryItemMappingID)
        //{
        //    if (View.SelectedUniversalCatItemID > AppConsts.NONE)
        //    {
        //        UniversalItemMapping uimObj = new UniversalItemMapping();
        //        uimObj.UIM_UniversalCategoryItemMappingID = View.SelectedUniversalCatItemID;
        //        uimObj.UIM_UniversalCategoryMappingID = View.MappedUniversalCategoryID;
        //        uimObj.UIM_CategoryItemMappingID = categoryItemMappingID;
        //        uimObj.UIM_CreatedBy = View.CurrentLoggedInUserId;
        //        uimObj.UIM_CreatedOn = DateTime.Now;
        //        UniversalMappingDataManager.SaveUpdateItemMappingWithUniversalItem(View.SelectedTenantId, uimObj);
        //    }
        //}

        //public void MappedUniversalCategoryData()
        //{
        //    var mappedData = UniversalMappingDataManager.GetMappedUniversalCategoryDataByID(View.SelectedTenantId, View.CurrentCategoryId
        //                                                                                    , UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue());
        //    if (!mappedData.IsNullOrEmpty())
        //    {
        //        View.MappedUniversalCategoryID = mappedData.UCM_ID;
        //        View.SelectedUniversalCategoryID = mappedData.UCM_UniversalCategoryID;
        //    }
        //}
        #endregion
    }
}




