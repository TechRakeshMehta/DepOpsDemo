using Business.RepoManagers;
using Entity.SharedDataEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ClinicalRotation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.RotationPackages.Views
{
    public class UniversalRotationMappingViewPresenter : Presenter<IUniversalRotationMappingViewView>
    {
        /// <summary>
        /// To check if Tenant is Admin/DefaultTenant
        /// UAT-1629 : As a client admin, I should not be able to edit rotation packages
        /// </summary>
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantId);
        }

        public void GetUniversalRotationMappingView()
        {
            if (View.RequirementPackageID.IsNotNull() && View.RequirementPackageID > AppConsts.NONE)
            {
                View.lstUniversalRotationMappingViewContract = UniversalMappingDataManager.GetUniversalRotationMappingView(View.RequirementPackageID);
                if (!View.lstUniversalRotationMappingViewContract.IsNullOrEmpty())
                {
                    View.IsPackageDisabled = View.lstUniversalRotationMappingViewContract.FirstOrDefault().IsPackageDisabled;
                }
            }
            else
            {
                View.lstUniversalRotationMappingViewContract = new List<UniversalRotationMappingViewContract>();
            }
        }

        public void GetUniversalCategory()
        {
            View.lstUniversalCategory = UniversalMappingDataManager.GetUniversalCategories(AppConsts.ONE); //Shared DB
        }

        public void GetUniversalItemsByCategoryID()
        {
            List<UniversalCategoryItemMapping> lstUniversalCategoryItemMapping = UniversalMappingDataManager.GetUniversalItemsByCategoryID(AppConsts.ONE, View.UniversalCategoryID); //Shared DB
            View.lstUniversalItem = lstUniversalCategoryItemMapping.Select(x => new UniversalItem
            {
                UI_ID = x.UCIM_ID, //UniversalCatItemMappingID 
                UI_Name = x.UniversalItem.UI_Name
            }).ToList();
        }

        public List<UniversalAttribute> GetUniversalAttributesByItemID()
        {
            List<UniversalItemAttributeMapping> lstUniversalCategoryItemMapping = UniversalMappingDataManager.GetUniversalAttributesByItemID(AppConsts.ONE, View.UniversalItemID); //Shared DB

            return lstUniversalCategoryItemMapping.Select(x => new UniversalAttribute
            {
                UA_Name = x.UniversalAttribute.UA_Name,
                UA_ID = x.UIAM_ID, //UniversalItemAttributeMappingID
                UA_AttributeDataTypeID = x.UniversalAttribute.UA_AttributeDataTypeID
            }).ToList();
        }

        public void SaveUniversalRequirmentCategoryMappingData()
        {
            View.Status = UniversalMappingDataManager.SaveUniversalRequirmentCategoryMappingData(View.UpdateContract, View.CurrentLoggedInUserId);
        }

        public void SaveUniversalRequirmentItemMappingData()
        {
            View.Status = UniversalMappingDataManager.SaveUniversalRequirmentItemMappingData(View.UpdateContract, View.CurrentLoggedInUserId);
        }

        public void SaveUniversalRequirmentAttributeMappingData()
        {
            View.Status = UniversalMappingDataManager.SaveUniversalRequirmentAttributeMappingData(View.UpdateContract, View.CurrentLoggedInUserId);
        }

        public void FilterUniversalAttrByReqFieldDataTypeID()
        {
            String uniAttrDataTypeCode = String.Empty;
            String reqFieldDataTypeCode = SharedRequirementPackageManager.GetRequirementFieldByFieldID(View.RequirementFieldID);

            if (!reqFieldDataTypeCode.IsNullOrEmpty() && reqFieldDataTypeCode.Equals(RequirementFieldDataType.OPTIONS.GetStringValue()))
            {
                uniAttrDataTypeCode = UniversalAttributeDataTypeEnum.OPTIONS.GetStringValue();
            }
            else if (!reqFieldDataTypeCode.IsNullOrEmpty() && reqFieldDataTypeCode.Equals(RequirementFieldDataType.DATE.GetStringValue()))
            {
                uniAttrDataTypeCode = UniversalAttributeDataTypeEnum.DATE.GetStringValue();
            }
            else if (!reqFieldDataTypeCode.IsNullOrEmpty() && reqFieldDataTypeCode.Equals(RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue()))
            {
                uniAttrDataTypeCode = UniversalAttributeDataTypeEnum.UPLOAD_DOCUMENT.GetStringValue();
            }
            else if (!reqFieldDataTypeCode.IsNullOrEmpty() && reqFieldDataTypeCode.Equals(RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue()))
            {
                uniAttrDataTypeCode = UniversalAttributeDataTypeEnum.VIEW_DOCUMENT.GetStringValue();
            }

            else if (!reqFieldDataTypeCode.IsNullOrEmpty() && reqFieldDataTypeCode.Equals(RequirementFieldDataType.TEXT.GetStringValue()))
            {
                uniAttrDataTypeCode = UniversalAttributeDataTypeEnum.TEXT.GetStringValue();
            }

            if (!uniAttrDataTypeCode.IsNullOrEmpty())
            {
                Int32 uniAttrDataTypeID = UniversalMappingDataManager.GetLkpAttributeDataTypeIDByCode(uniAttrDataTypeCode);
                View.lstUniversalAttribute = UniversalMappingDataManager.GetUniversalFieldByAttributeDataTypeID(AppConsts.ONE, uniAttrDataTypeID);
            }
        }
       

        

        public void BindSharedRequirementPackages()
        {
            View.lstRequirementPackages = SharedRequirementPackageManager.GetMasterRequirementPackages();
        }

        public void GetUniversalRequirementAttributeInputTypeMapping()
        {
            // View.lstInputTypeRotationAttributeContract = UniversalMappingDataManager.GetUniversalRequirementAttributeInputTypeMapping(View.UniversalReqAttrMappingID);

            View.lstInputTypeRotationAttributeContract= UniversalMappingDataManager.GetUniversalFieldInputTypeMappings(View.UniversalFieldMappingID);
        }

        public void GetUniversalAttributeOptionsByID()
        {
            View.lstUniversalAttributeOptions = UniversalMappingDataManager.GetUniversalFieldeOptionsByID(View.UniversalFieldID);            
        }

        public void GetUniversalAttributeOptionsByUniFieldID(Int32 universalFieldID)
        {
            View.lstUniversalAttributeOptions = UniversalMappingDataManager.GetUniversalFieldeOptionsByID(universalFieldID);
        }
    }
}
