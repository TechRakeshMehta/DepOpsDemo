using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration
{
    public class ManageUniversalItemPresenter : Presenter<IManageUniversalItemView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetUniversalItemDetailsByID()
        {
            if (View.UniversalCategoryID > AppConsts.NONE && View.UniversalItemID > AppConsts.NONE)
            {
                UniversalItem UI = UniversalMappingDataManager.GetUniversalItemDetailByID(View.UniversalCategoryID, View.UniversalItemID);

                if (!UI.IsNullOrEmpty())
                {
                    View.ItemName = UI.UI_Name;
                }
            }
        }

        public void GetUniversalItemsByCatID()
        {
            if (View.UniversalCategoryID > AppConsts.NONE)
            {
                List<UniversalCategoryItemMapping> data = UniversalMappingDataManager.GetUniversalItemsByCategoryID(0, View.UniversalCategoryID);
                View.lstUniversalItems = new List<UniversalItem>();
                data.ForEach(x =>
                {
                    View.lstUniversalItems.Add(x.UniversalItem);
                });
            }
        }

        public Boolean DeleteUniversalItemByID()
        {
            if (View.UniversalItemID > AppConsts.NONE)
            {
                return UniversalMappingDataManager.DeleteUniversalItemByID(View.UniversalItemID, View.UniversalCategoryID, View.CurrentUserId);
            }
            return false;
        }

        public Boolean DeleteUniversalAttributeByID(Int32 currentAttributeID)
        {
            if (View.UniversalItemID > AppConsts.NONE && currentAttributeID > AppConsts.NONE)
            {
                return UniversalMappingDataManager.DeleteUniversalAttributeByID(View.UniversalItemID, currentAttributeID, View.CurrentUserId);
            }
            return false;
        }

        public Boolean SaveUpdateUniversalItem()
        {
            UniversalItem uniItemData = null;
            UniversalCategoryItemMapping uniCatItmMapping = null;

            if (View.UniversalItemID > AppConsts.NONE && View.UniversalCategoryID > AppConsts.NONE)
            {
                uniItemData = UniversalMappingDataManager.GetUniversalItemDetailByID(View.UniversalCategoryID, View.UniversalItemID);

                uniCatItmMapping = uniItemData.UniversalCategoryItemMappings.Where(cond => !cond.UCIM_IsDeleted).FirstOrDefault();

                uniItemData.UI_Name = View.ItemName;
                uniItemData.UI_ModifiedBy = View.CurrentUserId;
                uniItemData.UI_ModifiedOn = DateTime.Now;

                uniCatItmMapping.UCIM_ModifiedBy = View.CurrentUserId;
                uniCatItmMapping.UCIM_ModifiedOn = DateTime.Now;
            }
            else
            {
                uniItemData = new UniversalItem();
                
                uniCatItmMapping = new UniversalCategoryItemMapping();

                uniItemData.UI_Name = View.ItemName;
                uniItemData.UI_IsDeleted = false;
                uniItemData.UI_CreatedBy = View.CurrentUserId;
                uniItemData.UI_CreatedOn = DateTime.Now;

                uniCatItmMapping.UCIM_IsDeleted = false;
                uniCatItmMapping.UCIM_UniversalCategoryID = View.UniversalCategoryID;
                uniCatItmMapping.UCIM_CreatedBy=View.CurrentUserId;
                uniCatItmMapping.UCIM_CreatedOn = DateTime.Now;

                uniItemData.UniversalCategoryItemMappings.Add(uniCatItmMapping);

            }
            return UniversalMappingDataManager.SaveUpdateUniverItem(uniItemData);
        }

        public Boolean IsValidItemName()
        {
            UniversalItem UI = UniversalMappingDataManager.GetUniversalItemDetailByID(View.UniversalCategoryID, View.UniversalItemID);
            if (!UI.IsNullOrEmpty() && UI.UI_Name == View.ItemName)
            {
                return false;
            }
            return UniversalMappingDataManager.IsValidItemName(View.ItemName);
        }

        public void GetUniversalAttributesDetails()
        {
            List<UniversalItemAttributeMapping> UIAM = UniversalMappingDataManager.GetUniversalAttributesByItemID(0, View.UniversalItemID);

            View.lstUniversalAttribute = new List<UniversalAttribute>();

            UIAM.ForEach(x =>
            {
                if (!x.UniversalAttribute.UA_IsDeleted)
                {
                    View.lstUniversalAttribute.Add(x.UniversalAttribute);
                }
            });
        }

        public Boolean IsValidToDeleteItem()
        {
            List<UniversalItemAttributeMapping> data = UniversalMappingDataManager.GetUniversalAttributesByItemID(0, View.UniversalItemID);

            if (data.IsNullOrEmpty())
                return true;
            return false;
        }
    }
}
