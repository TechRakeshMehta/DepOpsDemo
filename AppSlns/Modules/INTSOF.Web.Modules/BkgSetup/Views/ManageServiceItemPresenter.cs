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
    public class ManageServiceItemPresenter : Presenter<IManageServiceItemView>
    {
        #region Methods

        /// <summary>
        /// Get List of Package service item.
        /// </summary>
        public void GetPackageServiceItemList()
        {
            if (View.TenantId > 0)
                View.ListPackageServiceItem = BackgroundPricingManager.GetPackageServiceItemList(View.TenantId, View.BkgPackageSvcId, View.ParentNodeId);
            else
                View.ListPackageServiceItem = new List<PackageServiceItem>();

        }

        /// <summary>
        /// Method to load All dropdown data.
        /// </summary>
        public void LoadDropDownData()
        {
            GetParentPackageServiceItemList();
            GetGlobalFeeItemList();
            GetAttributeGroupList();
            GetServiceItemTypeList();
        }

        /// <summary>
        /// Get Parent package service item list of service.
        /// </summary>
        public void GetParentPackageServiceItemList()
        {
            List<PackageServiceItem> tempSvcParentItemList = new List<PackageServiceItem>();
            if (View.TenantId > 0)
                tempSvcParentItemList = BackgroundPricingManager.GetParentPackageServiceItemList(View.TenantId, View.BkgPackageSvcId, View.ParentNodeId);
            tempSvcParentItemList.Insert(0, new PackageServiceItem { PSI_ID = -1, PSI_ServiceItemName = "--SELECT--" });
            View.ListParentServiceItem = tempSvcParentItemList;
        }

        /// <summary>
        /// Get Global fee item list.
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
        /// Method to get attribute group list of service.
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
            tempListServiceItemType.Insert(0, new lkpServiceItemType { SIT_ID = 0, SIT_Name = "--SELECT--" });
            View.ListServiceItemType = tempListServiceItemType;
        }

        /// <summary>
        /// Delete the Package Service Item.
        /// </summary>
        public Boolean DeletePackageServiceItem(Int32 PSI_BkgPackageHierarchyMappingId)
        {
            if (BackgroundPricingManager.IsPackageNodeMapped(View.TenantId, PSI_BkgPackageHierarchyMappingId))
            {
                View.InfoMessage = "Service Item can't be deleted because it is in used.";
                return false;
            }
            else
            {
                PackageServiceItem packageServiceItemData = BackgroundPricingManager.GetPackageServiceItemData(View.TenantId, View.PSI_ID);
                if (!packageServiceItemData.IsNullOrEmpty())
                {
                    packageServiceItemData.PSI_IsDeleted = true;
                    packageServiceItemData.PSI_ModifiedByID = View.CurrentLoggedInUserId;
                    packageServiceItemData.PSI_ModifiedOn = DateTime.Now;

                    //Deleted Fee mapping.
                    packageServiceItemData.PackageServiceFeeMappings.Where(cond => cond.PSFM_PackageServiceItemID == View.PSI_ID).ForEach
                        (updt =>
                        {
                            updt.PSFM_IsDeleted = true;
                            updt.PSFM_ModifiedByID = View.CurrentLoggedInUserId;
                            updt.PSFM_ModifiedOn = DateTime.Now;
                        }
                    );

                    //Delete aditional occurence price
                    packageServiceItemData.PackageServiceItemPrices.Where(cond => cond.PSIP_PackageServiceItemID == View.PSI_ID).ForEach
                        (updt =>
                        {
                            updt.PSIP_IsDeleted = true;
                            updt.PSIP_ModifiedID = View.CurrentLoggedInUserId;
                            updt.PSIP_ModifiedOn = DateTime.Now;
                        }
                    );
                    if (BackgroundPricingManager.SaveClientChanges(View.TenantId))
                    {
                        View.SuccessMessage = "Service Item deleted successfully.";
                        return true;
                    }
                    else
                    {
                        View.ErrorMessage = "Some error has occurred.Please contact administrator.";
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Save update package service item data.
        /// </summary>
        public Boolean SaveUpdatePackageServiceItem()
        {
            //if (BackgroundPricingManager.IsServiceItemNameExist(View.TenantId, View.ServiceItemName, null, View.BkgPackageSvcId))
            //{
            //    View.InfoMessage = "Service Item Name exist.Please choose another Service Item Name";
            //    View.SuccessMessage = String.Empty;
            //    View.ErrorMessage = String.Empty;
            //    return false;
            //}
            PackageServiceItem packageServiceItemData = new PackageServiceItem();
            packageServiceItemData.PSI_ServiceItemName = View.ServiceItemName;
            packageServiceItemData.PSI_ServiceItemDecsription = View.ServiceItemDescription;
            //packageServiceItemData.PSI_ServiceItemLabel = View.ServiceItemLabel;
            packageServiceItemData.PSI_ServiceItemType = View.ServiceItemTypeId;
            packageServiceItemData.PSI_ParentServiceItemId = View.ParentServiceItemId;
            packageServiceItemData.PSI_PackageServiceID = View.BkgPackageSvcId;
            packageServiceItemData.PSI_IsRequired = View.IsRequired;
            packageServiceItemData.PSI_AttributeGroupId = View.AttributeGroupId;
            packageServiceItemData.PSI_BkgPackageHierarchyMappingId = View.ParentNodeId;
            packageServiceItemData.PSI_IsDeleted = false;
            packageServiceItemData.PSI_CreatedByID = View.CurrentLoggedInUserId;
            packageServiceItemData.PSI_CreatedOn = DateTime.Now;
            packageServiceItemData.PSI_IsSupplemental = View.IsSupplemental;
            packageServiceItemData.PSI_MinOccurrences = View.MinOccurrences;
            packageServiceItemData.PSI_MaxOccurrences = View.MaxOccurrences;
            packageServiceItemData.PSI_QuantityIncluded = View.QuantityIncluded;
            //if (View.QuantityGroup == -1)
            //    packageServiceItemData.PackageServiceItem3 = packageServiceItemData;

            //if (View.QuantityGroup > AppConsts.NONE)
            //    packageServiceItemData.PSI_QuantityGroup = View.QuantityGroup;

            //if (View.QuantityGroup == AppConsts.NONE)
            //    packageServiceItemData.PSI_QuantityGroup = null;

            //Save Additional occurence price

            PackageServiceItemPrice packageServiceItemPriceData = new PackageServiceItemPrice();
            if (!View.AdditinalOccurencePrice.IsNullOrEmpty())
            {
                packageServiceItemPriceData.PSIP_Amount = View.AdditinalOccurencePrice.Value;
                packageServiceItemPriceData.PSIP_Description = "Additional occurence price for " + View.ServiceItemName;
                packageServiceItemPriceData.PSIP_Label = "Additional occurence";
                packageServiceItemPriceData.PSIP_Name = "Additional occurence";
                packageServiceItemPriceData.PSIP_CreatedByID = View.CurrentLoggedInUserId;
                packageServiceItemPriceData.PSIP_CreadtedOn = DateTime.Now;
                packageServiceItemPriceData.PSIP_IsDeleted = false;
                //packageServiceItemPriceData.PSIP_ServiceItemPriceType = BackgroundPricingManager.GetServiceItemPriceTypeByCode(View.TenantId, "AAAA").SIPT_ID;
                packageServiceItemPriceData.PSIP_ServiceItemPriceType = View.ServiceItemPriceTypeId;
            }

            //Save Fee Item Mapping
            /*Not add global fee item in mapping table if selected value is minus one. 
             * -[UAT-831]: WB: Whenever a new Service Item is created without any “Global Fee Item” filled in, by default the value “All County Fee” is saved in the application.*/
            if (!View.GlobalFeeItemId.IsNullOrEmpty() && View.GlobalFeeItemId != AppConsts.MINUS_ONE)
            {
                PackageServiceFeeMapping packageServiceFeeMappingData = new PackageServiceFeeMapping();
                packageServiceFeeMappingData.PSFM_FeeItemID = View.GlobalFeeItemId;
                packageServiceFeeMappingData.PSFM_IsDeleted = false;
                packageServiceFeeMappingData.PSFM_IsGlobal = true;
                packageServiceFeeMappingData.PSFM_CreatedByID = View.CurrentLoggedInUserId;
                packageServiceFeeMappingData.PSFM_CreatedOn = DateTime.Now;
                packageServiceItemData.PackageServiceFeeMappings.Add(packageServiceFeeMappingData);
            }
            if (!View.AdditinalOccurencePrice.IsNullOrEmpty())
            {
                packageServiceItemData.PackageServiceItemPrices.Add(packageServiceItemPriceData);
            }
            if (BackgroundPricingManager.SavePackageServiceItemData(View.TenantId, packageServiceItemData, View.QuantityGroup))
            {
                View.SuccessMessage = "Service Item saved successfully.";
                return true;
            }
            else
            {
                View.ErrorMessage = "Some error has occured.Please contact administrator.";
                return false;
            }
        }

        /// <summary>
        /// Get the Service Item Price types i.e. ams.lkpServiceItemPriceType data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public List<lkpServiceItemPriceType> GetPriceTypes(Int32 tenantId)
        {
            return LookupManager.GetLookUpData<lkpServiceItemPriceType>(tenantId);
        }

        /// <summary>
        /// Get Parent package service item list of service.
        /// </summary>
        public List<PackageServiceItem> GetQuantityGroups(Int32 selectedAttributeGrpId, Int32 parentNodeId, Int32 tenantId)
        {
            return BackgroundPricingManager.GetServiceItemListAssociatedWithPackage(tenantId, parentNodeId, selectedAttributeGrpId);
        }

        public Entity.ApplicableServiceSetting GetServiceSettings()
        {
            return BackgroundSetupManager.GetServiceSetting(View.TenantId, View.BackgroundServiceId);
        }

        #endregion
    }
}
