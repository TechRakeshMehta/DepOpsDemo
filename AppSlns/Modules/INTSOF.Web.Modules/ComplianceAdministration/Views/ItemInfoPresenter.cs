using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity.ClientEntity;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ItemInfoPresenter : Presenter<IItemInfoView>
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


        public void GetCurrentItemInfo()
        {
            ComplianceItem cmpItem = ComplianceSetupManager.getCurrentItemInfo(View.CurrentItemId, View.SelectedTenantId);
            View.ViewContract = new ComplianceItemsContract
            {
                Name = cmpItem.Name,
                ItemLabel = cmpItem.ItemLabel,
                ScreenLabel = cmpItem.ScreenLabel,
                IsActive = cmpItem.IsActive,
                EffectiveDate = cmpItem.EffectiveDate,
                Description = cmpItem.Description,
                Details = cmpItem.Details,
                DisplayOrder = cmpItem.ComplianceCategoryItems.Where(cond => cond.CCI_CategoryID == View.CurrentCategoryId && !cond.CCI_IsDeleted && cond.CCI_IsActive).Select(col => col.CCI_DisplayOrder).FirstOrDefault(),
                //UAT-3077
                Amount = cmpItem.Amount,
                IsPaymentType = cmpItem.IsPaymentType.HasValue ? cmpItem.IsPaymentType.Value : false,
            };
            //= ComplianceSetupManager.getCurrentItemInfo(View.CurrentItemId, View.TenantId);
            //UAT-2305
            //View.CategoryItemMappingID = cmpItem.ComplianceCategoryItems.FirstOrDefault(cond => cond.CCI_CategoryID == View.CurrentCategoryId
            //                                                             && !cond.CCI_IsDeleted && cond.CCI_IsActive).CCI_ID;
        }


        public List<ComplianceItemAttribute> GetComplianceItemAttributes(Int32 itemID)
        {
            Boolean getTenantName = View.DefaultTenantId.Equals(View.SelectedTenantId);
            return ComplianceSetupManager.GetComplianceItemAttribute(itemID, View.SelectedTenantId, getTenantName);
        }

        public Boolean DeleteComplianceItemAttribute(Int32 cia_Id, Int32 complianceAttributeID, Int32 itemId, Int32 currentUserID)
        {
            return ComplianceSetupManager.DeleteComplianceItemAttribute(cia_Id, itemId, complianceAttributeID, currentUserID, View.SelectedTenantId);
        }

        public Boolean IfAttributeCanBeRemoved(Int32 complianceAttributeID, Int32 itemId)
        {
            List<lkpObjectType> lkpObjectType = RuleManager.GetObjectTypeList(View.SelectedTenantId);
            Int32 objectTypeIdForItem = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceItem.GetStringValue()).OT_ID;
            Int32 objectTypeIdForAttribute = lkpObjectType.FirstOrDefault(x => x.OT_Code == LCObjectType.ComplianceATR.GetStringValue()).OT_ID;
            //check whether mapping can be deleted or not
            IntegrityCheckResponse response = IntegrityManager.IfItemAttributeMappingCanBeDeleted(complianceAttributeID, itemId, View.SelectedTenantId, objectTypeIdForAttribute, objectTypeIdForItem);
            if (response.CheckStatus == CheckStatus.True)
            {
                ComplianceAttribute cmpAttribute = ComplianceSetupManager.GetComplianceAttribute(complianceAttributeID, View.SelectedTenantId);
                View.ErrorMessage = String.Format(response.UIMessage, cmpAttribute.Name);
                return false;
            }
            return true;
        }

        public void UpdateItem()
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
                    TenantID = View.SelectedTenantId,
                    //UAT-3077
                    Amount = View.ViewContract.Amount,
                    IsPaymentType = View.ViewContract.IsPaymentType
                };
                if (View.ViewContract.ComplianceItemId > 0)
                    complianceItem.ModifiedBy = View.CurrentLoggedInUserId;
                else
                    complianceItem.CreatedBy = View.CurrentLoggedInUserId;

                ComplianceSetupManager.UpdateComplianceItem(complianceItem, View.ViewContract.ExplanatoryNotes);
                //UAT-2305:
                //SaveUniversalItemMapping();
            }
        }

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        public void GetLargeContent()
        {
            LargeContent notesRecord = ComplianceSetupManager.getLargeContentRecord(View.CurrentViewContext.CurrentItemId, LCObjectType.ComplianceItem.GetStringValue(), LCContentType.ExplanatoryNotes.GetStringValue(), View.SelectedTenantId);
            if (notesRecord.IsNotNull())
                View.ViewContract.ExplanatoryNotes = notesRecord.LC_Content;
        }

        public void UpdateComplianceCategoryItemDisplayOrder()
        {
            ComplianceSetupManager.UpdateComplianceCategoryItemDisplayOrder(View.SelectedTenantId, View.CurrentViewContext.CurrentItemId, View.CurrentViewContext.CurrentCategoryId, View.ViewContract.DisplayOrder, View.CurrentLoggedInUserId);
        }

        public String GetItemDissociationStatus()
        {
            return ComplianceSetupManager.GetItemDissociationStatus(View.SelectedTenantId, View.CurrentPackageId, View.CurrentCategoryId, View.CurrentItemId);
        }

        public Int32 DissociateItem()
        {
            return ComplianceSetupManager.DissociateItem(View.SelectedTenantId, View.CurrentPackageId, View.SelectedCategoryIDs, View.CurrentItemId, View.CurrentLoggedInUserId);
        }

        public void GetCategoryItemMappingID()
        {
            Int32 complianceCategoryItemID = 0;
            Int32 complianceItemAttributeID = 0;
            ComplianceSetupManager.GetCategoryItemAttributeMappingID(View.SelectedTenantId, View.CurrentCategoryId, View.CurrentItemId, AppConsts.NONE, ref complianceCategoryItemID, ref complianceItemAttributeID);
            View.ComplianceCategoryItemID = complianceCategoryItemID;
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
        //    }
        //    lstUniversalItems.Insert(0, new Entity.SharedDataEntity.UniversalItem { UI_ID = 0, UI_Name = "--SELECT--" });
        //    View.LstUniversalItem = lstUniversalItems;
        //}

        //public void SaveUniversalItemMapping()
        //{
        //    if (View.SelectedUniversalCatItemID > AppConsts.NONE)
        //    {
        //        UniversalItemMapping uimObj = new UniversalItemMapping();
        //        uimObj.UIM_ID = View.UniversalItemMappingID;
        //        uimObj.UIM_UniversalCategoryItemMappingID = View.SelectedUniversalCatItemID;
        //        uimObj.UIM_UniversalCategoryMappingID = View.MappedUniversalCategoryID;
        //        uimObj.UIM_CategoryItemMappingID = View.CategoryItemMappingID;
        //        uimObj.UIM_CreatedBy = View.CurrentLoggedInUserId;
        //        uimObj.UIM_CreatedOn = DateTime.Now;
        //        if (View.SelectedUniversalCatItemID != View.MappedUniversalCatItemID && View.UniversalItemMappingID > AppConsts.NONE)
        //        {
        //            UniversalMappingDataManager.DeleteItemMappingWithUniversalItem(View.SelectedTenantId, View.UniversalItemMappingID, View.CurrentLoggedInUserId);
        //            uimObj.UIM_ID = AppConsts.NONE;
        //        }
        //        UniversalMappingDataManager.SaveUpdateItemMappingWithUniversalItem(View.SelectedTenantId, uimObj);
        //    }
        //    else if (View.UniversalItemMappingID > AppConsts.NONE && View.SelectedUniversalCatItemID == AppConsts.NONE)
        //    {
        //        UniversalMappingDataManager.DeleteItemMappingWithUniversalItem(View.SelectedTenantId, View.UniversalItemMappingID, View.CurrentLoggedInUserId);
        //    }
        //    MappedUniversalItemData();
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

        //public void MappedUniversalItemData()
        //{
        //    var mappedData = UniversalMappingDataManager.GetMappedUniversalItemDataByID(View.SelectedTenantId, View.MappedUniversalCategoryID, View.CategoryItemMappingID);
        //    if (!mappedData.IsNullOrEmpty())
        //    {
        //        View.UniversalItemMappingID = mappedData.UIM_ID;
        //        View.SelectedUniversalCatItemID = mappedData.UIM_UniversalCategoryItemMappingID;
        //        View.MappedUniversalCatItemID = mappedData.UIM_UniversalCategoryItemMappingID;
        //    }
        //}
        #endregion

        #region UAT-2402:

        public Boolean AddApprovedItemsToCopyDataQueue()
        {
            return UniversalMappingDataManager.AddApprovedItemsToCopyDataQueue(View.SelectedTenantId, View.CurrentItemId, View.CurrentCategoryId, View.CurrentLoggedInUserId);
        }

        public Boolean IsUniversalAttributeMappingExist()
        {
            return UniversalMappingDataManager.IsAnyAttributeMappingExists(View.SelectedTenantId, View.ComplianceCategoryItemID);
        }

        //public Boolean IsUniversalAttributeMappingExist()
        //{
        //    var mappedAttributesList = UniversalMappingDataManager.GetMappedUniversalAttributesByItemMappingID(View.SelectedTenantId, View.UniversalItemMappingID);
        //    if (!mappedAttributesList.IsNullOrEmpty())
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        #endregion

        public void GetComplianceCategoriesAssociatedToItem()
        {
            View.lstComplianceCategory = ComplianceSetupManager.GetComplianceCategoriesAssociatedtoItem(View.SelectedTenantId, View.CurrentItemId, View.CurrentCategoryId);
        }
    }
}




