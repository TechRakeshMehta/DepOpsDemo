using Entity.ClientEntity;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.UI.Contract.ClinicalRotation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUniversalMappingDataRepository
    {

        List<UniversalCategory> GetUniversalCategories();

        List<UniversalCategoryItemMapping> GetUniversalItemsByCategoryID(Int32 universalCatgeoryId);

        List<UniversalItemAttributeMapping> GetUniversalAttributesByItemID(Int32 universalItemId);

        List<UniversalField> GetUniversalFieldByAttributeDataTypeID(Int32 attributeDataTypeId);

        #region Universal Mapping Setup Screen
        DataTable GetUniversalMappingTreeData();
        Boolean SaveUpdateUniversalcategoryData(UniversalCategory uniCatData);
        Boolean DeleteUniversalCategorydata(Int32 UniCatID, Int32 currentLoggedInUserID);
        UniversalCategory GetUniversalCategoryByID(Int32 CategoryID);
        Boolean DeleteUniversalItemByID(Int32 uniItemID, Int32 uniCatID, Int32 currentLoggedInUserID);
        Boolean SaveUpdateUniverItem(UniversalItem uniItemData);
        Boolean DeleteUniversalAttributeByID(Int32 uniItemID, Int32 uniAttributeID, Int32 cuurentLoggedInUserID);
        UniversalItem GetUniversalItemDetailByID(Int32 uniCatID, Int32 uniItemID);
        Boolean SaveUpdateAttributeDetails(UniversalAttribute uniAttributeDetails);
        UniversalAttribute GetUniversalAttributeDataByID(Int32 uniItemID, Int32 uniAttributeID);
        Boolean IsValidAttributeName(String AttributeName);
        Boolean IsValidItemName(String ItemName);
        Boolean IsValidCategoryName(String CategoryName);
        #endregion

        #region compliance mapping with universal mapping regarding UAT-2305
        Boolean SaveUpdateCategoryMappingWithUniversalCat(UniversalCategoryMapping ucmData);
        UniversalCategoryMapping GetMappedUniversalCategoryDataByID(Int32 categoryId, Int32 mappingTypeId);
        Boolean DeleteCategoryMappingWithUniversalCategory(Int32 categoryMappingID, Int32 mappingTypeID, Int32 currentLoggedInuserId);
        Boolean SaveUpdateItemMappingWithUniversalItem(UniversalItemMapping uimData);
        UniversalItemMapping GetMappedUniversalItemDataByID(Int32 uniCateMapId, Int32 catItemMappingId);
        Boolean DeleteItemMappingWithUniversalItem(Int32 itemMappedID, Int32 currentLoggedInuserId);
        Boolean SaveUpdateAttributeMappingWithUniversalAttribute(UniversalAttributeMapping uamData);
        UniversalAttributeMapping GetMappedUniversalAttributeDataByID(Int32 uniItemMapId, Int32 itemAttrMappingId);
        Boolean DeleteAttributeMappingWithUniversalAttribute(Int32 attrMappingID, Int32 currentLoggedInuserId);
        UniversalCategoryItemMapping GetUniversalCatItemMappingData(Int32 UCIM_Id);
        List<UniversalAttributeInputTypeMapping> GetMappedUniversalInputAttributeDataByID(Int32 uniAttributeMapId);
        Boolean UpdateAttributeInputMapping(Int32 uniAttributeMapId, List<UniversalAttributeInputTypeMapping> lstInputMapping, Int32 currentLoggedInUserId);

        #endregion

        #region Universal Rotation Mapping View
        List<UniversalRotationMappingViewContract> GetUniversalRotationMappingView(Int32 requirementPackageID);
        Boolean SaveUniversalRequirmentCategoryMappingData(UniversalRotationMappingViewContract updateContract, Int32 loggedInUserID);
        Boolean SaveUniversalRequirmentItemMappingData(UniversalRotationMappingViewContract updateContract, Int32 loggedInUserID);
        Boolean SaveUniversalRequirmentAttributeMappingData(UniversalRotationMappingViewContract updateContract, Int32 loggedInUserID);
        Boolean CopySharedToTenantRequirementUniversalMapping(Int32 sharedRotationPackageId, Int32 tenantRotPackageId, Int32 currentLoggedInUserId);
        Boolean CopyTenantToSharedRequirementUniversalMapping(Int32 sharedCopiedRotationPackageId, Int32 tenantRotPackageId
                                                                                              , Int32 currentLoggedInUserId);
        Boolean CopySharedToSharedReqUniversalMappingForPkg(Int32 sharedRotationPackageId, Int32 sharedCopiedRotationPackageId
                                                                                              , Int32 currentLoggedInUserId);
        List<UniversalRequirementAttributeInputTypeMapping> GetUniversalRequirementAttributeInputTypeMapping(Int32 universalReqAttrMappingID);
        List<Entity.SharedDataEntity.UniversalFieldInputTypeMapping> GetUniversalFieldInputTypeMappings(Int32 universalFieldMappingID);
        #endregion

        #region Master Rotation Package
        UniversalRequirementCategoryMapping GetUniversalCategoryByReqCatID(Int32 ReqCatID);
        UniversalRequirementItemMapping GetUniversalItemsByUniReqCatItmID(Int32 ReqCatItmID, Int32 ReqItmID, Int32 ReqCatID);
        UniversalItem GetUniversalItemDetailsByReqItemID(Int32 ReqItmID, Int32 ReqCatID = 0);
        UniversalRequirementAttributeMapping GetUniversalattributeDetailsByItmFieldMappingID(Int32 UniReqItemMappingID, Int32 ReqItmFieldID);
        List<UniversalRequirementAttributeInputTypeMapping> GetAtrInputPriorityByID(Int32 uniReqAtrMappingID);
        Int32 GetUniversalReqAtrMappingID(Int32 uniReqItmID, Int32 uniItmAtrID);
        Boolean SaveChangesAttributeInputPriorty(List<UniversalRequirementAttributeInputTypeMapping> DataToSave);
        Int32 GetRequirementItmFieldIDByReqFldID(Int32 ReqFieldID, Int32 ReqItmID);
        #endregion

        #region Universal Compliance Mapping View
        List<UniversalComplianceMappingViewContract> GetUniversalComplianceMappingView(Int32 compliancePkgID);

        List<Entity.ClientEntity.UniversalFieldInputTypeMapping> GetUniversalAttributeInputTypeMapping(Int32 UniversalFieldMappingID);

        Boolean SaveUpdateAttributeMappingWithUniversalAttributeFromView(Entity.ClientEntity.UniversalFieldMapping uamData, Int32 loggedInUserID);
        #endregion

        #region UAT-2402
        List<UniversalAttributeOption> GetUniversalAtrOptionData(Int32 uniItmAtrMappingID);
        List<UniversalRequirementAttributeOptionMapping> GetUniversalAtrOptionSelected(Int32 reqFieldOptionID, Int32 uniReqAtrMappingID);
        List<UniversalAttributeOptionMapping> GetUniversalAttributeOptionMapping(Int32 uniAttributeMappingId);

        #endregion
        #region UAT-2402

        List<UniversalAttributeOption> GetUniversalAttributeOptionsByID(Int32 universalItemAttrMappingID);
        List<Entity.SharedDataEntity.UniversalFieldOption> GetUniversalFieldeOptionsByID(int universalFieldID);
        Boolean SaveUniverAttrOptionMapping(List<UniversalAttributeOptionMapping> lstUniAttrOptMapping, Int32 uniAttributeMapId, Int32 currentLoggedInUserId);
        Boolean AddApprovedItemsToCopyDataQueue(Int32 complianceItemId, Int32 complianceCategoryId, Int32 currentLoggedInUserId);

        //UAT-3716
        Boolean AddApprovedPkgsToCopyDataQueue(Int32 CurrentPackageId, Int32 currentLoggedInUserId);
        //END UAT-3716

        List<UniversalAttributeMapping> GetMappedUniversalAttributesByItemMappingID(Int32 uniItemMapId);
        #endregion

        #region Category Copy
        Boolean CopyUniversalDataByCategoryIds(Int32 currentAddedCategoryID, Int32 requirementCatyegoryID, Int32 currentLoggedInUserID);
        #endregion

        List<UniversalField> GetUniversalAttributeField();
        Boolean DeleteUniversalFieldByID(Int32 uniFieldID, Int32 cuurentLoggedInUserID);
        Boolean SaveUpdateUniversalField(UniversalField universalField, Int32 cuurentLoggedInUserID);

        UniversalField GetUniversalFieldById(Int32 uniFieldId);

        List<Entity.SharedDataEntity.UniversalFieldInputTypeMapping> GetUniversalAtrInputPriorityByID(Int32 uniFieldMappingID);

        Entity.SharedDataEntity.UniversalFieldMapping GetUniversalAttributeMappingByReqCatItemFieldID(Int32 ReqItmID, Int32 ReqFieldId, Int32 ReqCatID = 0);

        List<UniversalFieldOption> GetUniversalAttributeOptionData(Int32 universalFieldMappingID);

        List<Entity.SharedDataEntity.UniversalFieldInputTypeMapping> GetUniversalFieldAtrInputPriorityByID(Int32 uniFieldMappingID);

        List<UniversalFieldOption> GetUniversalFieldAtrOptionData(Int32 universalFieldID);

        List<Entity.SharedDataEntity.UniversalFieldOptionMapping> GetUniversalFieldAtrOptionSelected(Int32 uniFieldOptionID, Int32 uniFieldMappingID);

        Entity.ClientEntity.UniversalFieldMapping GetComplianceTypeUniversalFieldMapping(Int32 complianceCategoryItemID, Int32 complianceItemAttributeID, Int32 complianceMappingTypeID);

        List<UniversalFieldOption> GetUniversalFieldOptionData(Int32 universalFieldID);

        Boolean SaveUpdateUniversalFieldMapping(Entity.ClientEntity.UniversalFieldMapping ufmData);

        Boolean DeleteUniversalFieldMapping(Int32 universalFieldMappingID, Int32 currentLoggedInuserId);

        Boolean SaveUniversalFieldOptionMapping(List<Entity.ClientEntity.UniversalFieldOptionMapping> lstUniFieldOptMapping, Int32 uniFieldMappingId, Int32 currentLoggedInUserId);

        Boolean UpdateFieldInputMapping(Int32 uniFieldMappingId, List<Entity.ClientEntity.UniversalFieldInputTypeMapping> lstInputMapping, Int32 currentLoggedInUserId);

        Boolean IsAnyAttributeMappingExists(Int32 categoryItemMappingID);

        Boolean IsUniversalFieldNameExists(String universalFieldName);
    }
}
