using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class ManageUniversalCategoryPresenter : Presenter<IManageUniversalCategoryView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public void GetUniversalCategoryDetails()
        {
           View.lstUniversalCategory = UniversalMappingDataManager.GetUniversalCategories(0);
        }

        public Boolean SaveUpdateUniversalCategory()
        {
            UniversalCategory uniCategoryData = null;

            if (View.UniversalCategoryID > AppConsts.NONE)
            {
                uniCategoryData = UniversalMappingDataManager.GetUniversalCategoryByID(View.UniversalCategoryID);

                //uniCategoryData.UC_Description = View.Description;
                uniCategoryData.UC_Name = View.CategoryName;
                //uniCategoryData.UC_Label = View.CategoryLabel;
                uniCategoryData.UC_ModifiedBy = View.CurrentUserId;
                uniCategoryData.UC_ModifiedOn = DateTime.Now;
                uniCategoryData.UC_IsDeleted = false;
            }
            else
            {
                uniCategoryData = new UniversalCategory();

                //uniCategoryData.UC_Description = View.Description;
                uniCategoryData.UC_Name = View.CategoryName;
                //uniCategoryData.UC_Label = View.CategoryLabel;
                uniCategoryData.UC_CreatedBy = View.CurrentUserId;
                uniCategoryData.UC_CreatedOn = DateTime.Now;
                uniCategoryData.UC_IsDeleted = false;
                
            }
            return UniversalMappingDataManager.SaveUpdateUniversalcategoryData(uniCategoryData);
        }

        public Boolean DeleteUniversalCategorydata()
        {
            if (View.UniversalCategoryID > AppConsts.NONE)
            {
                return UniversalMappingDataManager.DeleteUniversalCategorydata(View.UniversalCategoryID, View.CurrentUserId);
            }
            return false;
        }
        public void GetUniversalCategoryDataByID()
        {
            if (View.UniversalCategoryID > AppConsts.NONE)
            {
                UniversalCategory categoryData = UniversalMappingDataManager.GetUniversalCategoryByID(View.UniversalCategoryID);

                if (!categoryData.IsNullOrEmpty())
                {
                    View.CategoryName = categoryData.UC_Name;
                    //View.CategoryLabel = categoryData.UC_Label;
                    //View.Description = categoryData.UC_Description;
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

        public Boolean DeleteUniversalItemByID(Int32 uniItemID)
        {
            if (uniItemID > AppConsts.NONE)
            {
                return UniversalMappingDataManager.DeleteUniversalItemByID(uniItemID, View.UniversalCategoryID,View.CurrentUserId);
            }
            return false;
        }

        public Boolean IsValidCategoryName()
        {
            UniversalCategory UC = UniversalMappingDataManager.GetUniversalCategoryByID(View.UniversalCategoryID);
            if (!UC.IsNullOrEmpty() && UC.UC_Name == View.CategoryName)
            {
                return false;
            }
            return UniversalMappingDataManager.IsValidCategoryName(View.CategoryName);
        }

        public Boolean IsValidToDeleteCategory()
        {
            List<UniversalCategoryItemMapping> data = UniversalMappingDataManager.GetUniversalItemsByCategoryID(0, View.UniversalCategoryID);

            if (data.IsNullOrEmpty())
                return true;
            return false;
        }

        public Boolean IsValidToDeleteItem(Int32 uniItemID)
        {
            List<UniversalItemAttributeMapping> data = UniversalMappingDataManager.GetUniversalAttributesByItemID(0, uniItemID);

            if (data.IsNullOrEmpty())
                return true;
            return false;
        }
    }
}
