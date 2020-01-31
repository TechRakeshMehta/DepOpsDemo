using Business.RepoManagers;
using Entity.ClientEntity;
using Entity.SharedDataEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.ClinicalRotation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ComplianceOperations.Views
{
    public class UniversalComplianceMappingViewPresenter : Presenter<IUniversalComplianceMappingViewView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (View.DefaultTenantId == View.TenantId);
        }

        public void GetTenants()
        {
            View.ListTenants = ComplianceDataManager.GetMasterAndInstitutionTypeTenants(View.DefaultTenantId);
        }

        public void GetCompliancePackages()
        {
            List<CompliancePackage> tempListPackages = new List<CompliancePackage>();
            if (View.SelectedTenantId > AppConsts.NONE)
            {
                tempListPackages = ComplianceSetupManager.GetCompliancePackage(View.SelectedTenantId, false);
            }
            View.ListCompliancePackages = tempListPackages.OrderBy(x => x.PackageName).ToList();
        }

        public void GetUniversalComplianceMappingView()
        {
            if (View.CompliancePackageID.IsNotNull() && View.CompliancePackageID > AppConsts.NONE)
            {
                View.lstUniversalComplianceMappingViewContract = UniversalMappingDataManager.GetUniversalComplianceMappingView(View.SelectedTenantId, View.CompliancePackageID);
            }
            else
            {
                View.lstUniversalComplianceMappingViewContract = new List<UniversalComplianceMappingViewContract>();
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
                UA_ID = x.UIAM_ID,
                UA_AttributeDataTypeID = x.UniversalAttribute.UA_AttributeDataTypeID
            }).ToList();
        }

        public void FilterUniversalAttrByCompAttrID()
        {
            String uniAttrDataTypeCode = String.Empty;
            String compFieldDataTypeCode = ComplianceSetupManager.GetComplianceAttributeDatatypeByAttributeID(View.SelectedTenantId, View.ComplianceFieldID);

            if (!compFieldDataTypeCode.IsNullOrEmpty() && compFieldDataTypeCode.Equals(ComplianceAttributeDatatypes.Options.GetStringValue()))
            {
                uniAttrDataTypeCode = UniversalAttributeDataTypeEnum.OPTIONS.GetStringValue();
            }
            else if (!compFieldDataTypeCode.IsNullOrEmpty() && compFieldDataTypeCode.Equals(ComplianceAttributeDatatypes.Date.GetStringValue()))
            {
                uniAttrDataTypeCode = UniversalAttributeDataTypeEnum.DATE.GetStringValue();
            }
            else if (!compFieldDataTypeCode.IsNullOrEmpty() && compFieldDataTypeCode.Equals(ComplianceAttributeDatatypes.FileUpload.GetStringValue()))
            {
                uniAttrDataTypeCode = UniversalAttributeDataTypeEnum.UPLOAD_DOCUMENT.GetStringValue();
            }
            else if (!compFieldDataTypeCode.IsNullOrEmpty() && compFieldDataTypeCode.Equals(ComplianceAttributeDatatypes.View_Document.GetStringValue()))
            {
                uniAttrDataTypeCode = UniversalAttributeDataTypeEnum.VIEW_DOCUMENT.GetStringValue();
            }
            else if (!compFieldDataTypeCode.IsNullOrEmpty() && compFieldDataTypeCode.Equals(ComplianceAttributeDatatypes.Text.GetStringValue()))
            {
                uniAttrDataTypeCode = UniversalAttributeDataTypeEnum.TEXT.GetStringValue();
            }
            if (!uniAttrDataTypeCode.IsNullOrEmpty())
            {
                Int32 uniAttrDataTypeID = UniversalMappingDataManager.GetLkpAttributeDataTypeIDByCode(uniAttrDataTypeCode);
                View.lstUniversalAttribute = GetUniversalField().Where(x => x.UF_AttributeDataTypeID == uniAttrDataTypeID).ToList();
            }
        }

        public List<Entity.SharedDataEntity.UniversalField> GetUniversalField()
        {
            List<Entity.SharedDataEntity.UniversalField> lstUniversalAttributes = new List<Entity.SharedDataEntity.UniversalField>();
            var lstUniversalItemAttributes = UniversalMappingDataManager.GetUniversalAttributeField(View.SelectedTenantId);

            if (!lstUniversalItemAttributes.IsNullOrEmpty())
            {
                lstUniversalItemAttributes.ForEach(x =>
                {
                    Entity.SharedDataEntity.UniversalField universalAttribute = new Entity.SharedDataEntity.UniversalField();
                    universalAttribute.UF_ID = x.UF_ID;
                    universalAttribute.UF_Name = x.UF_Name;
                    universalAttribute.UF_AttributeDataTypeID = x.UF_AttributeDataTypeID;
                    lstUniversalAttributes.Add(universalAttribute);
                });

                return lstUniversalAttributes;
            }

            return new List<Entity.SharedDataEntity.UniversalField>();
        }

        //public List<Entity.SharedDataEntity.UniversalField> GetFilteredFields(String attrDataType, String attrType)
        //{
        //    String uniAttributeDataTypeCode = String.Empty;
        //    Int32 uniAttributeDataTypeID = 0;
        //    List<Entity.SharedDataEntity.UniversalField> lstUniversalField = new List<Entity.SharedDataEntity.UniversalField>();
        //    if (!attrType.IsNullOrEmpty() && String.Compare(attrType, "Calculated", true) != AppConsts.NONE)
        //    {
        //        if (!attrDataType.IsNullOrEmpty() && String.Compare(attrDataType, "Options", true) == AppConsts.NONE)
        //        {
        //            uniAttributeDataTypeCode = UniversalAttributeDataTypeEnum.OPTIONS.GetStringValue();
        //        }
        //        else if (!attrDataType.IsNullOrEmpty() && String.Compare(attrDataType, "Date", true) == AppConsts.NONE)
        //        {
        //            uniAttributeDataTypeCode = UniversalAttributeDataTypeEnum.DATE.GetStringValue();
        //        }
        //        else if (!attrDataType.IsNullOrEmpty() && String.Compare(attrDataType, "file upload", true) == AppConsts.NONE)
        //        {
        //            uniAttributeDataTypeCode = UniversalAttributeDataTypeEnum.UPLOAD_DOCUMENT.GetStringValue();
        //        }
        //        else if (!attrDataType.IsNullOrEmpty() && String.Compare(attrDataType, "view document", true) == AppConsts.NONE)
        //        {
        //            uniAttributeDataTypeCode = UniversalAttributeDataTypeEnum.VIEW_DOCUMENT.GetStringValue();
        //        }

        //        else if (!attrDataType.IsNullOrEmpty() && String.Compare(attrDataType, "text", true) == AppConsts.NONE)
        //        {
        //            uniAttributeDataTypeCode = UniversalAttributeDataTypeEnum.TEXT.GetStringValue();
        //        }
        //        if (!uniAttributeDataTypeCode.IsNullOrEmpty())
        //        {
        //            uniAttributeDataTypeID = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpUniversalAttributeDataType>().FirstOrDefault(x => x.LUADT_Code == uniAttributeDataTypeCode).LUADT_ID;
        //            lstUniversalField = View.LstUniversalField.Where(x => x.UF_AttributeDataTypeID == uniAttributeDataTypeID).ToList();
        //        }
        //    }
        //    lstUniversalField.Insert(0, new Entity.SharedDataEntity.UniversalField { UF_ID = 0, UF_Name = "--SELECT--" });
        //    return lstUniversalField;
        //}
        public void SaveUniversalCategoryMappingData()
        {

            if (View.UpdateContract.IsNotNull() && View.UpdateContract.UniversalCategoryID > AppConsts.NONE)
            {
                UniversalCategoryMapping ucmObj = new UniversalCategoryMapping();
                ucmObj.UCM_ID = View.UpdateContract.UniversalCatItemMappingID; // View.UniversalCategoryMappingID;
                ucmObj.UCM_UniversalCategoryID = View.UpdateContract.UniversalCategoryID; //View.SelectedUniversalCategoryID;
                ucmObj.UCM_CategoryID = View.UpdateContract.ComplianceCategoryID;
                ucmObj.UCM_CreatedBy = View.CurrentLoggedInUserId;
                ucmObj.UCM_CreatedOn = DateTime.Now;
                if (UniversalMappingDataManager.SaveUpdateCategoryMappingWithUniversalCat(View.SelectedTenantId, ucmObj, UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue())
                    && View.UniversalCategoryID != View.UpdateContract.MappedUniversalCategoryID && View.UpdateContract.UniversalCatMappingID > AppConsts.NONE)
                {
                    UniversalMappingDataManager.DeleteCategoryMappingWithUniversalCategory(View.SelectedTenantId, View.UpdateContract.UniversalCatMappingID
                                                                                       , UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue(), View.CurrentLoggedInUserId);
                }
            }
            else if (View.UpdateContract.IsNotNull() && View.UpdateContract.UniversalCatMappingID > AppConsts.NONE && View.UpdateContract.UniversalCategoryID == AppConsts.NONE)
            {
                UniversalMappingDataManager.DeleteCategoryMappingWithUniversalCategory(View.SelectedTenantId, View.UpdateContract.UniversalCatMappingID
                                                                                       , UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue(), View.CurrentLoggedInUserId);
            }

        }

        public void SaveUniversalComplianceItemMappingData()
        {
            if (View.UpdateContract.IsNotNull() && View.UpdateContract.UniversalCatItemMappingID > AppConsts.NONE)
            {
                UniversalItemMapping uimObj = new UniversalItemMapping();
                uimObj.UIM_ID = View.UpdateContract.UniversalItemMappingID; //View.UniversalItemMappingID;
                uimObj.UIM_UniversalCategoryItemMappingID = View.UpdateContract.UniversalCatItemMappingID; //View.SelectedUniversalCatItemID;
                uimObj.UIM_UniversalCategoryMappingID = View.UpdateContract.UniversalCatMappingID; //View.MappedUniversalCategoryID;
                uimObj.UIM_CategoryItemMappingID = View.UpdateContract.ComplianceCategoryItemID; //View.CategoryItemMappingID;
                uimObj.UIM_CreatedBy = View.CurrentLoggedInUserId;
                uimObj.UIM_CreatedOn = DateTime.Now;
                if (UniversalMappingDataManager.SaveUpdateItemMappingWithUniversalItem(View.SelectedTenantId, uimObj)
                    && View.UpdateContract.UniversalCatItemMappingID != View.UpdateContract.MappedUniversalCatItemID && View.UpdateContract.UniversalItemMappingID > AppConsts.NONE)
                {
                    UniversalMappingDataManager.DeleteItemMappingWithUniversalItem(View.SelectedTenantId, View.UpdateContract.UniversalItemMappingID, View.CurrentLoggedInUserId);
                }
            }
            else if (View.UpdateContract.IsNotNull() && View.UpdateContract.UniversalItemMappingID > AppConsts.NONE && View.UpdateContract.UniversalCatItemMappingID == AppConsts.NONE)
            {
                UniversalMappingDataManager.DeleteItemMappingWithUniversalItem(View.SelectedTenantId, View.UpdateContract.UniversalItemMappingID, View.CurrentLoggedInUserId);
            }
        }

        public void SaveUniversalComplianceAttributeMappingData()
        {
            if (View.UpdateContract.IsNotNull())
            {
                Entity.ClientEntity.UniversalFieldMapping uamObj = new Entity.ClientEntity.UniversalFieldMapping();
                uamObj.UFM_ID = View.UpdateContract.UniversalFieldMappingID;
                uamObj.UFM_CategoryItemMappingID = View.UpdateContract.ComplianceCategoryItemID; //View.SelectedUniversalItemAttrID;
                uamObj.UFM_ItemAttributeMappingID = View.UpdateContract.ComplianceItemAttributeID; //View.MappedUniversalItemID;
                uamObj.UFM_UniversalFieldID = View.UpdateContract.ComplianceItemAttributeID; //itemAttributeMappingId;
                uamObj.UFM_UniversalFieldID = View.UpdateContract.UniversalFieldID;
 
                SetAttributeInputTypes(uamObj);
                SetAttributeOptionMapping(uamObj);
                UniversalMappingDataManager.SaveUpdateAttributeMappingWithUniversalAttributeFromView(View.SelectedTenantId, uamObj, View.CurrentLoggedInUserId);
            }
        }

        private void SetAttributeInputTypes(Entity.ClientEntity.UniversalFieldMapping uamObj)
        {
            View.UpdateContract.lstUniAttrInputMapping.ForEach(attrInputType =>
            {
                uamObj.UniversalFieldInputTypeMappings.Add(attrInputType);
            });
        }

        private void SetAttributeOptionMapping(Entity.ClientEntity.UniversalFieldMapping uamObj)
        {
            View.UpdateContract.lstUniversalAttributeOptionMapping.Where(x => x.UFOM_UniversalFieldOptionID > AppConsts.NONE).ForEach(attrOption =>
            {
                uamObj.UniversalFieldOptionMappings.Add(attrOption);
            });
        }

        public void GetUniversalAttributeInputTypeMapping()
        {
            View.lstInputTypeComplianceAttributeContract = UniversalMappingDataManager.GetUniversalComplianceAttributeInputTypeMapping(View.SelectedTenantId, View.UniversalFieldMappingID);
        }

        //public void GetUniversalAttributeOptionsByID()
        //{
        //    View.lstUniversalAttributeOptions = UniversalMappingDataManager.GetUniversalAttributeOptionsByID(View.UniversalItemAttrMappingID);
        //}

        public void GetUniversalFieldOptions()
        {
            View.lstUniversalAttributeOptions = UniversalMappingDataManager.GetUniversalFieldOptionData(View.UniversalFieldID);
        }

        public void GetUniversalFieldOptionsByUniFieldId(Int32 unifiedId)
        {
            View.lstUniversalAttributeOptions = UniversalMappingDataManager.GetUniversalFieldOptionData(unifiedId);
        }
    }
}
