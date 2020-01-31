using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity.ClientEntity;
using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System.Linq;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class AttributeListPresenter : Presenter<IAttributeListView>
    {

        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public List<ComplianceItemAttribute> GetComplianceItemAttributes(Int32 itemID)
        {
            Boolean getTenantName = View.DefaultTenantId.Equals(View.SelectedTenantId);
            return ComplianceSetupManager.GetComplianceItemAttribute(itemID, View.SelectedTenantId, getTenantName);
        }

        public List<ComplianceAttribute> GetNotMappedComplianceAttributes(Int32 itemId, Int32 SelectedTenantId)
        {
            return ComplianceSetupManager.GetNotMappedComplianceAttributes(itemId, SelectedTenantId);
            //return ComplianceSetupManager.GetComplianceAttributes(View.tenantId);
        }

        public bool IsAllowedOverrideDate(Int32 CategoryId)
        {
            return ComplianceSetupManager.IsAllowedOverrideDate(CategoryId, View.SelectedTenantId);
        }
        public List<lkpComplianceAttributeType> GetComplianceAttributeType()
        {
            return ComplianceSetupManager.GetComplianceAttributeType(View.SelectedTenantId);
        }

        public List<lkpComplianceAttributeDatatype> GetComplianceAttributeDatatype()
        {
            return ComplianceSetupManager.GetComplianceAttributeDatatype(View.SelectedTenantId);
        }

        /// <summary>
        /// To get compliance attribute group list
        /// </summary>
        /// <returns></returns>
        public List<ComplianceAttributeGroup> GetComplianceAttributeGroup()
        {
            var complianceAttributeGroupList = ComplianceSetupManager.GetComplianceAttributeGroup(View.SelectedTenantId);
            complianceAttributeGroupList.Insert(0, new ComplianceAttributeGroup { CAG_ID = 0, CAG_Name = "--SELECT--" });
            return complianceAttributeGroupList;
        }

        public Boolean AddComplianceItemAttributes(ComplianceItemAttributeContract complianceItemAttributeContract, Int32 loggedInUserId)
        {
            if (ComplianceSetupManager.AddComplianceItemAttribute(complianceItemAttributeContract.TranslateToEntity(), loggedInUserId, View.SelectedTenantId))
            {
                View.ErrorMessage = String.Empty;
                return true;
            }
            return false;
        }

        public Boolean AddComplianceAttribute(ComplianceAttributeContract complianceAttributeContract, Int32 loggedInUserId)
        {
            ComplianceAttribute attributeObject = ComplianceSetupManager.AddComplianceAttribute(complianceAttributeContract.TranslateToEntity(), complianceAttributeContract.ExplanatoryNotes);
            ComplianceItemAttribute itemAttributeMapping = new ComplianceItemAttribute();
            itemAttributeMapping.CIA_AttributeID = attributeObject.ComplianceAttributeID;
            itemAttributeMapping.CIA_ItemID = attributeObject.ComplianceItemAttributes.FirstOrNew().CIA_ItemID;
            ComplianceSetupManager.CreateAttributeAssignmentHierarchy(itemAttributeMapping, loggedInUserId, View.SelectedTenantId);
            complianceAttributeContract.ItemAttributeMappingID = ComplianceSetupManager.GetItemAttributeMappingID(itemAttributeMapping, View.SelectedTenantId);
            if (complianceAttributeContract.InstructionText != string.Empty)
            {
                Boolean output = ComplianceSetupManager.SaveInstructionText(complianceAttributeContract, loggedInUserId, View.SelectedTenantId, itemAttributeMapping);
            }

            //UAT-2305:
            Int32 itemAttrMappingId = complianceAttributeContract.ItemAttributeMappingID.IsNullOrEmpty() ? AppConsts.NONE :
                                                                                                           complianceAttributeContract.ItemAttributeMappingID.Value;

            View.AttributeID = itemAttributeMapping.CIA_AttributeID;
            GetCategoryItemAttributeMappingID(View.CategoryId, View.ItemID, View.AttributeID);

            //SaveUniversalAttributeMapping(itemAttrMappingId, itemAttributeMapping.CIA_AttributeID);
            SaveUniversalFieldMapping();

            View.ErrorMessage = String.Empty;
            return true;
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

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentUserId).Organization.TenantID.Value;
        }

        public List<ClientSystemDocument> GetComplianceViewDocumentSysDocs()
        {
            List<ClientSystemDocument> lstComplianceViewDocumentSysDocs = ComplianceSetupManager.GetComplianceViewDocumentSysDocs(View.SelectedTenantId);
            if (lstComplianceViewDocumentSysDocs.IsNull())
            {
                lstComplianceViewDocumentSysDocs = new List<ClientSystemDocument>();
            }
            lstComplianceViewDocumentSysDocs.Insert(0, new ClientSystemDocument { CSD_ID = 0, CSD_FileName = "--SELECT--" });
            return lstComplianceViewDocumentSysDocs;
        }

        /// <summary>
        /// Get the ID of the Screening DocumentType.
        /// </summary>
        /// <returns></returns>
        public Int32 GetScreeningDocumentAttributeDataTypeId()
        {
            var _lstAttributeDataTypes = LookupManager.GetLookUpData<lkpComplianceAttributeDatatype>(View.SelectedTenantId);
            return _lstAttributeDataTypes.First(adt => adt.Code == ComplianceAttributeDatatypes.Screening_Document.GetStringValue()).ComplianceAttributeDatatypeID;
        }

        #region UAT-2985:Universal Mapping Design Changes (1 of 2)

        public ComplianceAttribute GetComplianceAttribute(Int32 complianceAttributeID)
        {
            return ComplianceSetupManager.GetComplianceAttribute(complianceAttributeID, View.SelectedTenantId);
        }

        public void GetCategoryItemAttributeMappingID(Int32 categoryId, Int32 itemId, Int32 attributeID)
        {
            Int32 complianceCategoryItemID = 0;
            Int32 complianceItemAttributeID = 0;
            ComplianceSetupManager.GetCategoryItemAttributeMappingID(View.SelectedTenantId, categoryId, itemId, attributeID, ref complianceCategoryItemID, ref complianceItemAttributeID);
            View.ComplianceCategoryItemID = complianceCategoryItemID;
            View.ComplianceItemAttributeID = complianceItemAttributeID;
        }

        public void GetUniversalField()
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
                View.LstUniversalField = lstUniversalAttributes;
            }
        }

        public List<Entity.SharedDataEntity.UniversalField> GetFilteredFields(String attrDataType, String attrType)
        {
            String uniAttributeDataTypeCode = String.Empty;
            Int32 uniAttributeDataTypeID = 0;
            List<Entity.SharedDataEntity.UniversalField> lstUniversalField = new List<Entity.SharedDataEntity.UniversalField>();
            if (!attrType.IsNullOrEmpty() && String.Compare(attrType, "Calculated", true) != AppConsts.NONE)
            {
                if (!attrDataType.IsNullOrEmpty() && String.Compare(attrDataType, "Options", true) == AppConsts.NONE)
                {
                    uniAttributeDataTypeCode = UniversalAttributeDataTypeEnum.OPTIONS.GetStringValue();
                }
                else if (!attrDataType.IsNullOrEmpty() && String.Compare(attrDataType, "Date", true) == AppConsts.NONE)
                {
                    uniAttributeDataTypeCode = UniversalAttributeDataTypeEnum.DATE.GetStringValue();
                }
                else if (!attrDataType.IsNullOrEmpty() && String.Compare(attrDataType, "file upload", true) == AppConsts.NONE)
                {
                    uniAttributeDataTypeCode = UniversalAttributeDataTypeEnum.UPLOAD_DOCUMENT.GetStringValue();
                }
                else if (!attrDataType.IsNullOrEmpty() && String.Compare(attrDataType, "view document", true) == AppConsts.NONE)
                {
                    uniAttributeDataTypeCode = UniversalAttributeDataTypeEnum.VIEW_DOCUMENT.GetStringValue();
                }

                else if (!attrDataType.IsNullOrEmpty() && String.Compare(attrDataType, "text", true) == AppConsts.NONE)
                {
                    uniAttributeDataTypeCode = UniversalAttributeDataTypeEnum.TEXT.GetStringValue();
                }
                if (!uniAttributeDataTypeCode.IsNullOrEmpty())
                {
                    uniAttributeDataTypeID = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpUniversalAttributeDataType>().FirstOrDefault(x => x.LUADT_Code == uniAttributeDataTypeCode).LUADT_ID;
                    lstUniversalField = View.LstUniversalField.Where(x => x.UF_AttributeDataTypeID == uniAttributeDataTypeID).ToList();
                }
            }
            lstUniversalField.Insert(0, new Entity.SharedDataEntity.UniversalField { UF_ID = 0, UF_Name = "--SELECT--" });
            return lstUniversalField;
        }

        private void MappedUniversalFieldData()
        {
            var mapping = UniversalMappingDataManager.GetComplianceTypeUniversalFieldMapping(View.SelectedTenantId, View.ComplianceCategoryItemID, View.ComplianceItemAttributeID, UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue());

            if (!mapping.IsNullOrEmpty())
            {
                View.MappedUniversalFieldMappingID = mapping.UFM_ID;
                View.SelectedUniversalFieldID = mapping.UFM_UniversalFieldID;
            }
        }

        public void GetUniversalFieldOptions()
        {
            View.lstUniversalFieldOptions = UniversalMappingDataManager.GetUniversalFieldOptionData(View.SelectedUniversalFieldID);
        }

        public void SaveUniversalFieldMapping()
        {
            if (View.SelectedUniversalFieldID > AppConsts.NONE)
            {
                UniversalFieldMapping ufm = new UniversalFieldMapping();
                ufm.UFM_UniversalFieldID = View.SelectedUniversalFieldID;
                ufm.UFM_CategoryItemMappingID = View.ComplianceCategoryItemID;
                ufm.UFM_ItemAttributeMappingID = View.ComplianceItemAttributeID;
                ufm.UFM_IsDeleted = false;
                ufm.UFM_CreatedBy = View.CurrentUserId;
                ufm.UFM_CreatedOn = DateTime.Now;
                ufm.UFM_ModifiedBy = View.CurrentUserId;
                ufm.UFM_ModifiedOn = DateTime.Now;

                UniversalMappingDataManager.SaveUpdateUniversalFieldMapping(View.SelectedTenantId, ufm, UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue());
                MappedUniversalFieldData();
                SaveInputFieldType();
                //UAT-2402:
                SaveFieldOptionMapping();
            }
        }

        public void SaveInputFieldType()
        {
            if (!View.lstSelectedInputFieldData.IsNull())
            {
                UniversalMappingDataManager.UpdateFieldInputMapping(View.SelectedTenantId, View.MappedUniversalFieldMappingID, View.lstSelectedInputFieldData, View.CurrentUserId);
            }
        }

        private void SaveFieldOptionMapping()
        {
            if (!View.lstSelectedAttributeOptionData.IsNull())
            {
                ComplianceAttribute compAttribute = GetComplianceAttribute(View.AttributeID);
                List<ComplianceAttributeOption> lstCompAttrOption = new List<ComplianceAttributeOption>();
                if (!compAttribute.IsNullOrEmpty())
                {
                    compAttribute.ComplianceAttributeOptions.ForEach(attrOpt =>
                    {
                        lstCompAttrOption.Add(attrOpt);
                    });
                }
                SetUniversalFieldOptMapping(lstCompAttrOption);
            }
        }

        private void SetUniversalFieldOptMapping(List<ComplianceAttributeOption> lstCompAttrOption)
        {
            List<UniversalFieldOptionMapping> lstUniFieldOptMapping = new List<UniversalFieldOptionMapping>();
            if (!lstCompAttrOption.IsNullOrEmpty())
            {
                View.lstSelectedAttributeOptionData.ForEach(attrOptData =>
                {
                    UniversalFieldOptionMapping uniFieldOptMapping = new UniversalFieldOptionMapping();
                    uniFieldOptMapping.UFOM_UniversalFieldOptionID = attrOptData.MappedUniversalOptionID;
                    uniFieldOptMapping.UFOM_UniversalFieldMappingID = View.MappedUniversalFieldMappingID;
                    var optData = lstCompAttrOption.FirstOrDefault(x => x.OptionText.Trim().ToLower() == attrOptData.OptionText.Trim().ToLower());
                    uniFieldOptMapping.UFOM_AttributeOptionID = optData.IsNullOrEmpty() ? AppConsts.NONE : optData.ComplianceAttributeOptionID;
                    uniFieldOptMapping.UFOM_CreatedBy = View.CurrentUserId;
                    uniFieldOptMapping.UFOM_CreatedOn = DateTime.Now;
                    lstUniFieldOptMapping.Add(uniFieldOptMapping);
                });
            }
            UniversalMappingDataManager.SaveUniversalFieldOptionMapping(View.SelectedTenantId, lstUniFieldOptMapping, View.MappedUniversalFieldMappingID, View.CurrentUserId);
        }

        #endregion


        //#region UAT-2305:Tracking to Rotation category/item/attribute mapping
        //public void GetUniversalItemAttribute()
        //{
        //    List<Entity.SharedDataEntity.UniversalAttribute> lstUniversalAttributes = new List<Entity.SharedDataEntity.UniversalAttribute>();
        //    var lstUniversalItemAttributes = UniversalMappingDataManager.GetUniversalAttributesByItemID(View.SelectedTenantId, View.SelectedUniversalItemID);

        //    if (!lstUniversalItemAttributes.IsNullOrEmpty())
        //    {
        //        lstUniversalItemAttributes.ForEach(x =>
        //        {
        //            Entity.SharedDataEntity.UniversalAttribute universalAttribute = new Entity.SharedDataEntity.UniversalAttribute();
        //            universalAttribute.UA_ID = x.UIAM_ID;
        //            universalAttribute.UA_Name = x.UniversalAttribute.UA_Name;
        //            universalAttribute.UA_AttributeDataTypeID = x.UniversalAttribute.UA_AttributeDataTypeID;
        //            lstUniversalAttributes.Add(universalAttribute);
        //        });
        //        View.LstUniversalAttribute = lstUniversalAttributes;
        //    }
        //}

        //public void SaveUniversalAttributeMapping(Int32 itemAttributeMappingId,Int32 attributeID)
        //{
        //    if (View.SelectedUniversalItemAttrID > AppConsts.NONE)
        //    {
        //        UniversalAttributeMapping uamObj = new UniversalAttributeMapping();
        //        uamObj.UAM_UniversalItemAttributeMappingID = View.SelectedUniversalItemAttrID;
        //        uamObj.UAM_UniversalItemMappingID = View.MappedUniversalItemID;
        //        uamObj.UAM_ItemAttributeMappingID = itemAttributeMappingId;
        //        //uamObj.UAM_InputPriority = View.InputPriority;
        //        uamObj.UAM_CreatedBy = View.CurrentUserId;
        //        uamObj.UAM_CreatedOn = DateTime.Now;
        //        SetAttributeInputTypes(uamObj);
        //        //UAT-2402:
        //        SetAttrOptionMapping(uamObj, attributeID);
        //        UniversalMappingDataManager.SaveUpdateAttributeMappingWithUniversalAttribute(View.SelectedTenantId, uamObj);
        //    }

        //}

        //public void MappedUniversalItemData(Int32 categoryId, Int32 itemId)
        //{
        //    var mappedCatData = UniversalMappingDataManager.GetMappedUniversalCategoryDataByID(View.SelectedTenantId, categoryId
        //                                                                                       , UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue());
        //    ComplianceItem cmpItem = ComplianceSetupManager.getCurrentItemInfo(itemId, View.SelectedTenantId);
        //    Int32 UCM_ID = 0;
        //    Int32 catItemMappingId = 0;
        //    if (!mappedCatData.IsNullOrEmpty() && !cmpItem.IsNullOrEmpty())
        //    {
        //        UCM_ID = mappedCatData.UCM_ID;
        //        catItemMappingId = cmpItem.ComplianceCategoryItems.FirstOrDefault(cond => cond.CCI_CategoryID == categoryId && !cond.CCI_IsDeleted && cond.CCI_IsActive).CCI_ID;
        //    }
        //    var mappedData = UniversalMappingDataManager.GetMappedUniversalItemDataByID(View.SelectedTenantId, UCM_ID, catItemMappingId);
        //    if (!mappedData.IsNullOrEmpty())
        //    {
        //        View.MappedUniversalItemID = mappedData.UIM_ID;
        //        var uniCatItemMapping = UniversalMappingDataManager.GetUniversalCatItemMappingData(View.SelectedTenantId, mappedData.UIM_UniversalCategoryItemMappingID);
        //        View.SelectedUniversalItemID = uniCatItemMapping.UCIM_ID;

        //    }
        //}

        //public List<Entity.SharedDataEntity.UniversalAttribute> GetFilteredAttribute(String attrDataType,String attrType)
        //{
        //    String uniAttributeDataTypeCode = String.Empty;
        //    Int32 uniAttributeDataTypeID = 0;
        //    List<Entity.SharedDataEntity.UniversalAttribute> lstUniversalAttributes = new List<Entity.SharedDataEntity.UniversalAttribute>();
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
        //            lstUniversalAttributes = View.LstUniversalAttribute.Where(x => x.UA_AttributeDataTypeID == uniAttributeDataTypeID).ToList();
        //        }
        //    }
        //    lstUniversalAttributes.Insert(0, new Entity.SharedDataEntity.UniversalAttribute { UA_ID = 0, UA_Name = "--SELECT--" });
        //    return lstUniversalAttributes;
        //}

        //private void SetAttributeInputTypes(UniversalAttributeMapping uamObj)
        //{
        //    View.lstSelectedInputAttributesData.ForEach(attrInputType => 
        //    {
        //        uamObj.UniversalAttributeInputTypeMappings.Add(attrInputType);
        //    });
        //}
        //#endregion

        //#region UAT-2402:Additional Tracking to Rotation Mapping development and testing
        //public void GetUniversalAttributeOptions()
        //{
        //    View.lstUniversalAttributeOptions = UniversalMappingDataManager.GetUniversalAtrOptionData(View.SelectedUniversalItemAttrID);
        //}

        //private void SetAttrOptionMapping(UniversalAttributeMapping uamObj,Int32 attibuteID)
        //{
        //    if (!View.lstSelectedAttributeOptionData.IsNull())
        //    {
        //        ComplianceAttribute compAttribute = ComplianceSetupManager.GetComplianceAttribute(attibuteID, View.SelectedTenantId);
        //        List<ComplianceAttributeOption> lstCompAttrOption = new List<ComplianceAttributeOption>();
        //        if (!compAttribute.IsNullOrEmpty())
        //        {
        //            compAttribute.ComplianceAttributeOptions.ForEach(attrOpt =>
        //            {
        //                lstCompAttrOption.Add(attrOpt);
        //            });
        //        }
        //        SetUniversalAttrOptMapping(lstCompAttrOption, uamObj);
        //    }
        //}

        //private void SetUniversalAttrOptMapping(List<ComplianceAttributeOption> lstCompAttrOption, UniversalAttributeMapping uamObj)
        //{
        //    if (!lstCompAttrOption.IsNullOrEmpty())
        //    {
        //        View.lstSelectedAttributeOptionData.ForEach(attrOptData =>
        //        {
        //            UniversalAttributeOptionMapping uniAttrOptMapping = new UniversalAttributeOptionMapping();
        //            uniAttrOptMapping.UAOM_UniversalAttributeOptionID = attrOptData.MappedUniversalOptionID;
        //            var optData = lstCompAttrOption.FirstOrDefault(x => x.OptionText.Trim().ToLower() == attrOptData.OptionText.Trim().ToLower());
        //            uniAttrOptMapping.UAOM_AttributeOptionID = optData.IsNullOrEmpty() ? AppConsts.NONE : optData.ComplianceAttributeOptionID;
        //            uniAttrOptMapping.UAOM_CreatedBy = uamObj.UAM_CreatedBy;
        //            uniAttrOptMapping.UAOM_CreatedOn = DateTime.Now;
        //            uamObj.UniversalAttributeOptionMappings.Add(uniAttrOptMapping);
        //        });
        //    }
        //}
        //#endregion

        #region UAT-4558
        public List<Entity.SystemDocument> GetFileUploadAdditionalDocs()//(String attributeDatatypeCode)
        {
            //String docTypeCode = String.Empty;
            List<Entity.SystemDocument> lstComplianceFileUploadDocumentSysDocs = new List<Entity.SystemDocument>();

            List<String> lstSatusCodeToFetch = new List<String>();
            List<Int32> lstDocStatusIDs = new List<Int32>();


            // if (!String.IsNullOrEmpty(attributeDatatypeCode))
            //{
            //   if (String.Compare(attributeDatatypeCode.ToLower(), ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower()) == 1)
            //  {
            lstSatusCodeToFetch.Add(DislkpDocumentType.ADDITIONAL_DOCUMENTS.GetStringValue());
            //Fetch only Additional Documents
            lstDocStatusIDs = SecurityManager.GetDocumentTypes().Where(x => lstSatusCodeToFetch.Contains(x.DT_Code) && x.DT_IsActive).Select(cond => cond.DT_ID).ToList();
            //}
            //}

            if (!lstDocStatusIDs.IsNullOrEmpty() && lstDocStatusIDs.Count > AppConsts.NONE)
                lstComplianceFileUploadDocumentSysDocs = BackgroundSetupManager.GetBothUploadedDisclosureDocuments(lstDocStatusIDs).Where(x => !x.IsOperational.HasValue || (x.IsOperational.HasValue && x.IsOperational == false)).ToList();
            //lstComplianceFileUploadDocumentSysDocs.Insert(0, new Entity.SystemDocument { SystemDocumentID = 0, FileName = "--SELECT--" });
            return lstComplianceFileUploadDocumentSysDocs;
        }
        #endregion
    }
}




