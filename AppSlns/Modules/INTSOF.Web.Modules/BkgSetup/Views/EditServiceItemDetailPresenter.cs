#region NameSpaces

#region system defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region Project Specific
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
#endregion

#endregion


namespace CoreWeb.BkgSetup.Views
{
    public class EditServiceItemDetailPresenter : Presenter<IEditServiceItemDetailView>
    {
        public void OnViewInitialized()
        {
            GetGlobalFeeItemList();
            GetServiceItemTypeList();
            BindServiceItemPriceTypes();
            GetServiceItemData();
        }

        /// <summary>
        /// Method to get Parent service item list.
        /// </summary>
        public void GetParentPackageServiceItemList(Int32 BkgPackageHierarchyMappingId)
        {
            List<PackageServiceItem> tempSvcParentItemList = new List<PackageServiceItem>();
            if (View.TenantId > 0)
            {
                tempSvcParentItemList = BackgroundPricingManager.GetParentPackageServiceItemList(View.TenantId, View.BkgPackageSvcId, BkgPackageHierarchyMappingId);
                tempSvcParentItemList = tempSvcParentItemList.Where(cond => cond.PSI_ID != View.PSI_ID).ToList();
            }
            tempSvcParentItemList.Insert(0, new PackageServiceItem { PSI_ID = -1, PSI_ServiceItemName = "--SELECT--" });
            View.ListParentServiceItem = tempSvcParentItemList;
        }

        /// <summary>
        /// Method to get Global Fee item List.
        /// </summary>
        public void GetGlobalFeeItemList()
        {
            List<Entity.PackageServiceItemFee> tempListServiceItemFee = new List<Entity.PackageServiceItemFee>();
            if (View.TenantId > 0)
                tempListServiceItemFee = BackgroundPricingManager.GetPackageServiceFeeItemGlobal(View.TenantId);
            tempListServiceItemFee.Insert(0, new Entity.PackageServiceItemFee { PSIF_ID = -1, PSIF_Name = "--SELECT--" });
            View.GlobalPackageServiceFeeItemList = tempListServiceItemFee;
        }

        /// <summary>
        /// Method to get attribute group associated to service.
        /// </summary>
        public void GetAttributeGroupList()
        {
            List<BkgSvcAttributeGroup> tempSvcAttributeListList = new List<BkgSvcAttributeGroup>();
            if (View.TenantId > 0)
                tempSvcAttributeListList = BackgroundPricingManager.GetBkgSvcAttributeGroupById(View.TenantId, View.BkgPackageSvcId);
            tempSvcAttributeListList.Insert(0, new BkgSvcAttributeGroup { BSAD_ID = -1, BSAD_Name = "--SELECT--" });
            View.ListAttributeGroup = tempSvcAttributeListList;
        }

        /// <summary>
        /// Get List of Service Item type.
        /// </summary>
        public void GetServiceItemTypeList()
        {
            List<lkpServiceItemType> tempListServiceItemType = new List<lkpServiceItemType>();
            if (View.TenantId > 0)
                tempListServiceItemType = BackgroundPricingManager.GetServiceItemTypeList(View.TenantId);
            tempListServiceItemType.Insert(0, new lkpServiceItemType { SIT_ID = 0, SIT_Name = "--SELECT--", SIT_Code = "SLCT"});
            View.ListServiceItemType = tempListServiceItemType;
        }

        /// <summary>
        /// Method to update the service item data .
        /// </summary>
        /// <returns></returns>
        public Boolean UpdateServiceItemData()
        {
            //if (BackgroundPricingManager.IsServiceItemNameExist(View.TenantId, View.ServiceItemName, View.PSI_ID, View.BkgPackageSvcId))
            //{
            //    View.InfoMessage = "Service Item Name exist.Please choose another Service Item Name";
            //    View.SuccessMessage = String.Empty;
            //    View.ErrorMessage = String.Empty;
            //    return false;
            //}
            PackageServiceItem packageServiceItemData = BackgroundPricingManager.GetPackageServiceItemData(View.TenantId, View.PSI_ID);
            if (!packageServiceItemData.IsNullOrEmpty())
            {
                packageServiceItemData.PSI_ServiceItemName = View.ServiceItemName;
                packageServiceItemData.PSI_ServiceItemDecsription = View.ServiceItemDescription;
                //packageServiceItemData.PSI_ServiceItemLabel = View.ServiceItemLabel;
                packageServiceItemData.PSI_ServiceItemType = View.ServiceItemTypeId;
                packageServiceItemData.PSI_ParentServiceItemId = View.ParentServiceItemId;
                packageServiceItemData.PSI_IsRequired = View.IsRequired;
                packageServiceItemData.PSI_IsSupplemental = View.IsSupplemental;
                packageServiceItemData.PSI_AttributeGroupId = View.AttributeGroupId;
                packageServiceItemData.PSI_ModifiedByID = View.CurrentLoggedInUserId;
                packageServiceItemData.PSI_ModifiedOn = DateTime.Now;
                packageServiceItemData.PSI_QuantityIncluded = View.QuantityIncluded;

                if (View.QuantityGroupId == -1)
                    packageServiceItemData.PSI_QuantityGroup = packageServiceItemData.PSI_ID;

                if (View.QuantityGroupId > AppConsts.NONE)
                    packageServiceItemData.PSI_QuantityGroup = View.QuantityGroupId;

                if (View.QuantityGroupId == AppConsts.NONE)
                    packageServiceItemData.PSI_QuantityGroup = null;

                packageServiceItemData.PSI_MinOccurrences = View.MinOccurrences;
                packageServiceItemData.PSI_MaxOccurrences = View.MaxOccurrences;
                //Update Additional occurence price
                PackageServiceItemPrice packageServiceItemPriceData = packageServiceItemData.PackageServiceItemPrices.FirstOrDefault(cond => cond.PSIP_PackageServiceItemID == packageServiceItemData.PSI_ID && cond.PSIP_IsDeleted == false);
                if (!packageServiceItemPriceData.IsNullOrEmpty())
                {
                    if (!View.AdditinalOccurencePrice.IsNullOrEmpty())
                    {
                        packageServiceItemPriceData.PSIP_Amount = View.AdditinalOccurencePrice.Value;
                        packageServiceItemPriceData.PSIP_ModifiedID = View.CurrentLoggedInUserId;
                        packageServiceItemPriceData.PSIP_ModifiedOn = DateTime.Now;
                        packageServiceItemPriceData.PSIP_Description = "Additional occurence price for " + View.ServiceItemName;
                        packageServiceItemPriceData.PSIP_ServiceItemPriceType = View.ServiceItemPriceTypeId;
                    }
                    else
                    {
                        packageServiceItemPriceData.PSIP_IsDeleted = true;
                        packageServiceItemPriceData.PSIP_ModifiedID = View.CurrentLoggedInUserId;
                        packageServiceItemPriceData.PSIP_ModifiedOn = DateTime.Now;
                    }
                }
                else
                {
                    if (!View.AdditinalOccurencePrice.IsNullOrEmpty())
                    {
                        PackageServiceItemPrice packageServiceItemPriceNewData = new PackageServiceItemPrice();
                        packageServiceItemPriceNewData.PSIP_Amount = View.AdditinalOccurencePrice.Value;
                        packageServiceItemPriceNewData.PSIP_Description = "Additional occurence price for " + View.ServiceItemName;
                        packageServiceItemPriceNewData.PSIP_Label = "Additional occurence";
                        packageServiceItemPriceNewData.PSIP_Name = "Additional occurence";
                        packageServiceItemPriceNewData.PSIP_CreatedByID = View.CurrentLoggedInUserId;
                        packageServiceItemPriceNewData.PSIP_CreadtedOn = DateTime.Now;
                        packageServiceItemPriceNewData.PSIP_IsDeleted = false;
                        packageServiceItemPriceNewData.PSIP_PackageServiceItemID = packageServiceItemData.PSI_ID;
                        //packageServiceItemPriceNewData.PSIP_ServiceItemPriceType = BackgroundPricingManager.GetServiceItemPriceTypeByCode(View.TenantId, "AAAA").SIPT_ID;
                        packageServiceItemPriceNewData.PSIP_ServiceItemPriceType = View.ServiceItemPriceTypeId;

                        BackgroundPricingManager.SavePackageServiceItemPrice(View.TenantId, packageServiceItemPriceNewData);
                    }
                }
                //Update Fee Item Mapping
                PackageServiceFeeMapping packageServiceFeeMappingData = packageServiceItemData.PackageServiceFeeMappings.FirstOrDefault(cnd => cnd.PSFM_PackageServiceItemID == packageServiceItemData.PSI_ID && cnd.PSFM_IsDeleted == false && cnd.PSFM_IsGlobal == true);
                if (!packageServiceFeeMappingData.IsNullOrEmpty())
                {
                    if (View.GlobalFeeItemId != AppConsts.MINUS_ONE)
                    {
                        packageServiceFeeMappingData.PSFM_FeeItemID = View.GlobalFeeItemId;
                        packageServiceFeeMappingData.PSFM_ModifiedByID = View.CurrentLoggedInUserId;
                        packageServiceFeeMappingData.PSFM_ModifiedOn = DateTime.Now;
                    }
                    else
                    {
                        packageServiceFeeMappingData.PSFM_IsDeleted = true;
                        packageServiceFeeMappingData.PSFM_ModifiedByID = View.CurrentLoggedInUserId;
                        packageServiceFeeMappingData.PSFM_ModifiedOn = DateTime.Now;
                    }
                }
                else
                {
                    if (View.GlobalFeeItemId != AppConsts.MINUS_ONE)
                    {
                        PackageServiceFeeMapping packageServiceFeeMappingNewRecord = new PackageServiceFeeMapping();
                        packageServiceFeeMappingNewRecord.PSFM_FeeItemID = View.GlobalFeeItemId;
                        packageServiceFeeMappingNewRecord.PSFM_IsDeleted = false;
                        packageServiceFeeMappingNewRecord.PSFM_IsGlobal = true;
                        packageServiceFeeMappingNewRecord.PSFM_PackageServiceItemID = packageServiceItemData.PSI_ID;
                        packageServiceFeeMappingNewRecord.PSFM_CreatedByID = View.CurrentLoggedInUserId;
                        packageServiceFeeMappingNewRecord.PSFM_CreatedOn = DateTime.Now;
                        BackgroundPricingManager.SavePackageServiceItemFeeMapping(View.TenantId, packageServiceFeeMappingNewRecord);
                    }
                }
                if (BackgroundPricingManager.SaveClientChanges(View.TenantId))
                {
                    View.SuccessMessage = "Service Item updated successfully.";
                    View.InfoMessage = String.Empty;
                    View.ErrorMessage = String.Empty;
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error has occurred.Please contact administrator.";
                    View.SuccessMessage = String.Empty;
                    View.InfoMessage = String.Empty;
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Method to get the data of service item for bind the controls.
        /// </summary>
        public void GetServiceItemData()
        {
            PackageServiceItem packageServiceItemData = BackgroundPricingManager.GetPackageServiceItemData(View.TenantId, View.PSI_ID);
            if (!packageServiceItemData.IsNullOrEmpty())
            {
                //Set Package Service Id
                View.BkgPackageSvcId = packageServiceItemData.PSI_PackageServiceID;
                GetParentPackageServiceItemList(packageServiceItemData.PSI_BkgPackageHierarchyMappingId.Value);
                GetAttributeGroupList();
                View.ServiceItemName = packageServiceItemData.PSI_ServiceItemName;
                View.ServiceItemDescription = packageServiceItemData.PSI_ServiceItemDecsription;
                //View.ServiceItemLabel = packageServiceItemData.PSI_ServiceItemLabel;
                View.ServiceItemTypeId = packageServiceItemData.PSI_ServiceItemType;
                View.ParentServiceItemId = packageServiceItemData.PSI_ParentServiceItemId;
                View.IsRequired = packageServiceItemData.PSI_IsRequired == null ? false : packageServiceItemData.PSI_IsRequired.Value;
                View.IsSupplemental = packageServiceItemData.PSI_IsSupplemental == null ? false : packageServiceItemData.PSI_IsSupplemental.Value;
                View.AttributeGroupId = packageServiceItemData.PSI_AttributeGroupId.Value;
                View.BPHM_Id = packageServiceItemData.PSI_BkgPackageHierarchyMappingId.HasValue ? packageServiceItemData.PSI_BkgPackageHierarchyMappingId.Value : 0;
                GetQuantityGroups(View.AttributeGroupId, View.BPHM_Id, View.TenantId);
                if (packageServiceItemData.PSI_QuantityGroup.HasValue)
                    View.QuantityGroupId = packageServiceItemData.PSI_QuantityGroup.Value;
                else
                    View.QuantityGroupId = null;
                View.QuantityIncluded = Convert.ToInt32(packageServiceItemData.PSI_QuantityIncluded);
                View.MinOccurrences = packageServiceItemData.PSI_MinOccurrences;
                View.MaxOccurrences = packageServiceItemData.PSI_MaxOccurrences;
                View.BackgroundServiceId = packageServiceItemData.BkgPackageSvc.BPS_BackgroundServiceID;

                // to check if quantity grp editable
                View.ifQuantityGrpEditable = packageServiceItemData.PackageServiceItem11.Count == 0 ? true : !(packageServiceItemData.PackageServiceItem11.Any(x => x.PSI_QuantityGroup != x.PSI_ID && x.PSI_IsDeleted == false));
                //Set Additional occurence price
                PackageServiceItemPrice packageServiceItemPriceData = packageServiceItemData.PackageServiceItemPrices.FirstOrDefault(cond => cond.PSIP_PackageServiceItemID == packageServiceItemData.PSI_ID && cond.PSIP_IsDeleted == false);
                if (!packageServiceItemPriceData.IsNullOrEmpty())
                {
                    View.AdditinalOccurencePrice = packageServiceItemPriceData.PSIP_Amount;
                    View.ServiceItemPriceTypeId = packageServiceItemPriceData.PSIP_ServiceItemPriceType;
                }
                //Set Fee Item Mapping
                PackageServiceFeeMapping packageServiceFeeMappingData = packageServiceItemData.PackageServiceFeeMappings.FirstOrDefault(cnd => cnd.PSFM_PackageServiceItemID == packageServiceItemData.PSI_ID && cnd.PSFM_IsDeleted == false && cnd.PSFM_IsGlobal == true);
                if (!packageServiceFeeMappingData.IsNullOrEmpty())
                    View.GlobalFeeItemId = packageServiceFeeMappingData.PSFM_FeeItemID;
            }
        }

        /// <summary>
        /// Get the Service Item Price types i.e. ams.lkpServiceItemPriceType data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public void BindServiceItemPriceTypes()
        {
            List<lkpServiceItemPriceType> _lst = LookupManager.GetLookUpData<lkpServiceItemPriceType>(View.TenantId);
            _lst.Insert(AppConsts.NONE, new lkpServiceItemPriceType { SIPT_ID = AppConsts.NONE, SIPT_Name = AppConsts.COMBOBOX_ITEM_SELECT });
            View.ServiceItemPriceTypes = _lst;
        }


        /// <summary>
        /// Get Parent package service item list of service.
        /// </summary>
        public void GetQuantityGroups(Int32 selectedAttributeGrpId, Int32 parentNodeId, Int32 tenantId)
        {
            View.QuantityGroups = BackgroundPricingManager.GetServiceItemListAssociatedWithPackage(tenantId, parentNodeId, selectedAttributeGrpId, View.PSI_ID);
        }

        public Entity.ApplicableServiceSetting GetServiceSettings()
        {
            return BackgroundSetupManager.GetServiceSetting(View.TenantId, View.BackgroundServiceId);
        }

        public Boolean ifAttribteGrpEditable()
        {
            return BackgroundSetupManager.IsPackageServiceItemEditable(View.PSI_ID, View.TenantId);
        }

        public void IsStateSearchRuleExists()
        {
           View.IsStateSearchRuleExists =  BackgroundSetupManager.IsStateSearchRuleExists(View.PSI_ID, View.TenantId);
        }

        public String CreateAutomaticSrchRule()
        {
            return BackgroundPricingManager.CreateAutomaticSrchRule(View.TenantId, View.CurrentLoggedInUserId, View.PSI_ID, View.ServiceItemName, View.ServiceItemTypeId);
        }
    }
}
