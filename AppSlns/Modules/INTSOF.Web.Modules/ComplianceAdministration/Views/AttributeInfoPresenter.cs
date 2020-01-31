using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity.ClientEntity;
using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Linq;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ComplianceAdministration.Views
{
    public class AttributeInfoPresenter : Presenter<IAttributeInfoView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IComplianceAdministrationController _controller;
        // public AttributeInfoPresenter([CreateNew] IComplianceAdministrationController controller)
        // {
        // 		_controller = controller;
        // }
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        public ComplianceAttribute GetComplianceAttribute(Int32 complianceAttributeID)
        {
            return ComplianceSetupManager.GetComplianceAttribute(complianceAttributeID, View.SelectedTenantId);
        }

        public List<lkpComplianceAttributeType> GetComplianceAttributeType()
        {
            return ComplianceSetupManager.GetComplianceAttributeType(View.SelectedTenantId);
        }

        //public List<lkpComplianceAttributeDatatype> GetComplianceAttributeDatatype(Int32 itemId, String attributeTypeCode)
        //{
        //    String complianceAttributeDataType = ComplianceAttributeDatatypes.FileUpload.GetStringValue();
        //    if (ComplianceSetupManager.IsFileUploadAttributePresent(itemId, View.SelectedTenantId) || (complianceAttributeDataType == attributeTypeCode))
        //        return ComplianceSetupManager.GetComplianceAttributeDatatype(View.SelectedTenantId);
        //    else
        //    {
        //        return ComplianceSetupManager.GetComplianceAttributeDatatype(View.SelectedTenantId).Where(x => x.Code != complianceAttributeDataType).ToList();
        //    }
        //}

        public List<lkpComplianceAttributeDatatype> GetComplianceAttributeDatatype(Int32 itemId, String attributeTypeCode)
        {
            String fileUploadAttributeDataType = ComplianceAttributeDatatypes.FileUpload.GetStringValue();
            String viewDocAttributeDataType = ComplianceAttributeDatatypes.View_Document.GetStringValue();
            List<lkpComplianceAttributeDatatype> lstDataTypes = ComplianceSetupManager.GetComplianceAttributeDatatype(View.SelectedTenantId);

            if (!ComplianceSetupManager.IsFileUploadAttributePresent(itemId, View.SelectedTenantId) && (fileUploadAttributeDataType != attributeTypeCode))
            {
                lstDataTypes = lstDataTypes.Where(x => x.Code != fileUploadAttributeDataType).ToList();
            }
            if (!ComplianceSetupManager.IsViewDocAttributePresent(itemId, View.SelectedTenantId) && (viewDocAttributeDataType != attributeTypeCode))
            {
                lstDataTypes = lstDataTypes.Where(x => x.Code != viewDocAttributeDataType).ToList();
            }
            return lstDataTypes;
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

        public Boolean UpdateComplianceAttribute(ComplianceAttributeContract complianceAttributeContract, Int32 AttrID, Int32 ItemId, Int32 CategoryId, Int32 PackageId)
        {
            Boolean result = ComplianceSetupManager.UpdateComplianceAttribute(complianceAttributeContract.TranslateToEntity(), complianceAttributeContract.ExplanatoryNotes);

            ComplianceItemAttribute itemAttributeMapping = new ComplianceItemAttribute();
            itemAttributeMapping.CIA_AttributeID = AttrID;
            itemAttributeMapping.CIA_ItemID = ItemId;
            complianceAttributeContract.ItemAttributeMappingID = ComplianceSetupManager.GetItemAttributeMappingID(itemAttributeMapping, View.SelectedTenantId);
            complianceAttributeContract.loggedInUserId = View.currentLoggedInUserId;
            //UAT-2305:

            //SaveUniversalAttributeMapping();
            SaveUniversalFieldMapping();
            return ComplianceSetupManager.UpdateInstructionText(complianceAttributeContract, AttrID, ItemId, CategoryId, PackageId);

        }

        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.currentLoggedInUserId).Organization.TenantID.Value;
        }

        public String GetLargeContent(Int32 complianceAttributeID)
        {
            LargeContent notesRecord = ComplianceSetupManager.getLargeContentRecord(complianceAttributeID, LCObjectType.ComplianceATR.GetStringValue(), LCContentType.ExplanatoryNotes.GetStringValue(), View.SelectedTenantId);
            if (notesRecord != null)
                return notesRecord.LC_Content;
            else
                return String.Empty;
        }

        public Boolean checkIfMappingIsDefinedForAttribute(Int32 complianceAttributeID)
        {
            return MobilityManager.checkIfMappingIsDefinedForAttribute(complianceAttributeID, View.SelectedTenantId);
        }

        public Boolean UpdateComplianceItemAttributeDisplayOrder(Int32 displayOrder, Int32 complianceAttributeID, Int32 itemID)
        {
            return ComplianceSetupManager.UpdateComplianceItemAttributeDisplayOrder(View.SelectedTenantId, complianceAttributeID, itemID, displayOrder, View.currentLoggedInUserId);
        }

        public String GetAttributeDissociationStatus(Int32 packageId, Int32 categoryId, Int32 itemId, Int32 attrID)
        {
            return ComplianceSetupManager.GetAttributeDissociationStatus(View.SelectedTenantId, packageId, categoryId, itemId, attrID);
        }
        public void GetComplianceItemAssociatedtoAttributes()
        {
            View.lstComplianceItems = ComplianceSetupManager.GetComplianceItemAssociatedtoAttributes(View.SelectedTenantId, View.ItemId, View.CategoryId, View.AttrID);
        }

        public Int32 DissociateAttribute(Int32 packageId, Int32 categoryId, String itemIds, Int32 attrID)
        {
            return ComplianceSetupManager.DissociateAttribute(View.SelectedTenantId, packageId, categoryId, itemIds, attrID, View.currentLoggedInUserId);
        }
        //UAT-4348

        public bool IsAllowedOverrideDate(Int32 ItemId)
        {
            return ComplianceSetupManager.IsAllowedOverrideDate(ItemId, View.SelectedTenantId);
        }

        // TODO: Handle other view events and set state in the view

        public String GetAttributeInstruction(Int32 AttrID, Int32 ItemId, Int32 CategoryId, Int32 PackageId, ComplianceAttributeContract complianceAttributeContract)
        {
            ComplianceItemAttribute itemAttributeMapping = new ComplianceItemAttribute();
            itemAttributeMapping.CIA_AttributeID = AttrID;
            itemAttributeMapping.CIA_ItemID = ItemId;
            complianceAttributeContract.ItemAttributeMappingID = ComplianceSetupManager.GetItemAttributeMappingID(itemAttributeMapping, View.SelectedTenantId);
            //UAT-2305:
            View.ItemAttributeMappingID = complianceAttributeContract.ItemAttributeMappingID.IsNullOrEmpty() ? AppConsts.NONE :
                                                                                                               complianceAttributeContract.ItemAttributeMappingID.Value;
            //call usp to get Instruction Text 
            return ComplianceSetupManager.GetAttributeInstructionText(complianceAttributeContract, AttrID, ItemId, CategoryId, PackageId);

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

        public void GetUniversalFieldOptions()
        {
            View.lstUniversalFieldOptions = UniversalMappingDataManager.GetUniversalFieldOptionData(View.SelectedUniversalFieldID);
        }

        public void MappedUniversalFieldData()
        {
            var mapping = UniversalMappingDataManager.GetComplianceTypeUniversalFieldMapping(View.SelectedTenantId, View.ComplianceCategoryItemID, View.ComplianceItemAttributeID, UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue());

            if (!mapping.IsNullOrEmpty())
            {
                View.MappedUniversalFieldMappingID = mapping.UFM_ID;
                View.lstMappedInputFieldData = mapping.UniversalFieldInputTypeMappings.Where(cond => !cond.UFITM_IsDeleted).ToList();
                View.SelectedUniversalFieldID = mapping.UFM_UniversalFieldID;
                View.MappedUniversalFieldID = mapping.UFM_UniversalFieldID;
                if (IsMappedUniversalFieldActive())
                {
                    if (!View.lstMappedInputFieldData.IsNullOrEmpty())
                    {
                        View.selectedInputFields = View.lstMappedInputFieldData.Select(x => x.UFITM_UniversalFieldID).ToList();
                    }

                    var optionMappingData = mapping.UniversalFieldOptionMappings.Where(cond => !cond.UFOM_IsDeleted).ToList();
                    List<ComplianceAttributeOptionMappingContract> lstOptionData = new List<ComplianceAttributeOptionMappingContract>();

                    if (!optionMappingData.IsNullOrEmpty())
                    {
                        optionMappingData.ForEach(attrOption =>
                        {
                            ComplianceAttributeOptionMappingContract optionData = new ComplianceAttributeOptionMappingContract();

                            List<ComplianceAttributeOption> lstAttributeOption = BackgroundSetupManager.GetComplianceAttributeOption(View.SelectedTenantId, View.AttrID);

                            var mappedOptionAttribute = lstAttributeOption.FirstOrDefault(x => x.ComplianceAttributeOptionID == attrOption.UFOM_AttributeOptionID);

                            if (!mappedOptionAttribute.IsNullOrEmpty())
                            {
                                optionData.OptionText = mappedOptionAttribute.OptionText;
                                optionData.OptionValue = mappedOptionAttribute.OptionValue;
                            }
                            optionData.MappedUniversalOptionID = attrOption.UFOM_UniversalFieldOptionID;
                            lstOptionData.Add(optionData);
                        });
                    }

                    View.lstMappedFieldOptionData = lstOptionData;
                }
            }
        }

        public void SaveUniversalFieldMapping()
        {
            if (View.SelectedUniversalFieldID > AppConsts.NONE)
            {
                UniversalFieldMapping ufm = new UniversalFieldMapping();
                ufm.UFM_ID = View.MappedUniversalFieldMappingID;
                ufm.UFM_UniversalFieldID = View.SelectedUniversalFieldID;
                ufm.UFM_CategoryItemMappingID = View.ComplianceCategoryItemID;
                ufm.UFM_ItemAttributeMappingID = View.ComplianceItemAttributeID;
                ufm.UFM_IsDeleted = false;
                ufm.UFM_CreatedBy = View.currentLoggedInUserId;
                ufm.UFM_CreatedOn = DateTime.Now;
                ufm.UFM_ModifiedBy = View.currentLoggedInUserId;
                ufm.UFM_ModifiedOn = DateTime.Now;

                if (View.SelectedUniversalFieldID != View.MappedUniversalFieldID && View.MappedUniversalFieldMappingID > AppConsts.NONE)
                {
                    UniversalMappingDataManager.DeleteUniversalFieldMapping(View.SelectedTenantId, View.MappedUniversalFieldMappingID, View.currentLoggedInUserId);
                    ufm.UFM_ID = AppConsts.NONE;
                }

                UniversalMappingDataManager.SaveUpdateUniversalFieldMapping(View.SelectedTenantId, ufm, UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue());
                MappedUniversalFieldData();
                SaveInputFieldType();
                //UAT-2402:
                SaveFieldOptionMapping();
                MappedUniversalFieldData();
            }
            else if (View.MappedUniversalFieldMappingID > AppConsts.NONE && View.SelectedUniversalFieldID == AppConsts.NONE)
            {
                UniversalMappingDataManager.DeleteUniversalFieldMapping(View.SelectedTenantId, View.MappedUniversalFieldMappingID, View.currentLoggedInUserId);
            }
        }

        public void SaveInputFieldType()
        {
            if (!View.lstSelectedInputFieldData.IsNull())
            {
                UniversalMappingDataManager.UpdateFieldInputMapping(View.SelectedTenantId, View.MappedUniversalFieldMappingID, View.lstSelectedInputFieldData, View.currentLoggedInUserId);
            }
        }

        private void SaveFieldOptionMapping()
        {
            if (!View.lstSelectedAttributeOptionData.IsNull())
            {
                ComplianceAttribute compAttribute = GetComplianceAttribute(View.AttrID);
                List<ComplianceAttributeOption> lstCompAttrOption = new List<ComplianceAttributeOption>();
                if (!compAttribute.IsNullOrEmpty())
                {
                    compAttribute.ComplianceAttributeOptions.Where(cond => cond.IsActive && !cond.IsDeleted).ForEach(attrOpt =>
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
                    var optData = lstCompAttrOption.Where(d => !d.IsDeleted && d.IsActive).FirstOrDefault(x => x.OptionText.Trim().ToLower() == attrOptData.OptionText.Trim().ToLower());
                    uniFieldOptMapping.UFOM_AttributeOptionID = optData.IsNullOrEmpty() ? AppConsts.NONE : optData.ComplianceAttributeOptionID;
                    uniFieldOptMapping.UFOM_CreatedBy = View.currentLoggedInUserId;
                    uniFieldOptMapping.UFOM_CreatedOn = DateTime.Now;
                    lstUniFieldOptMapping.Add(uniFieldOptMapping);
                });
            }
            UniversalMappingDataManager.SaveUniversalFieldOptionMapping(View.SelectedTenantId, lstUniFieldOptMapping, View.MappedUniversalFieldMappingID, View.currentLoggedInUserId);
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

        //public void SaveUniversalAttributeMapping()
        //{
        //    if (View.SelectedUniversalItemAttrID > AppConsts.NONE)
        //    {
        //        UniversalAttributeMapping uamObj = new UniversalAttributeMapping();
        //        uamObj.UAM_ID = View.UniversalAttributeMappingID;
        //        uamObj.UAM_UniversalItemAttributeMappingID = View.SelectedUniversalItemAttrID;
        //        uamObj.UAM_UniversalItemMappingID = View.MappedUniversalItemID;
        //        uamObj.UAM_ItemAttributeMappingID = View.ItemAttributeMappingID;
        //        //uamObj.UAM_InputPriority = View.InputPriority;
        //        uamObj.UAM_CreatedBy = View.currentLoggedInUserId;
        //        uamObj.UAM_CreatedOn = DateTime.Now;
        //        if (View.SelectedUniversalItemAttrID != View.MappedUniversalItemAttributeID && View.UniversalAttributeMappingID > AppConsts.NONE)
        //        {
        //            UniversalMappingDataManager.DeleteAttributeMappingWithUniversalAttribute(View.SelectedTenantId, View.UniversalAttributeMappingID, View.currentLoggedInUserId);
        //            uamObj.UAM_ID = AppConsts.NONE;
        //        }
        //        UniversalMappingDataManager.SaveUpdateAttributeMappingWithUniversalAttribute(View.SelectedTenantId, uamObj);
        //        MappedUniversalAttributeData();
        //        SaveInputAttributeType();
        //        //UAT-2402:
        //        SaveAttrOptionMapping();
        //        GetMappedOptionsMapping();
        //    }
        //    else if (View.UniversalAttributeMappingID > AppConsts.NONE && View.SelectedUniversalItemAttrID == AppConsts.NONE)
        //    {
        //        UniversalMappingDataManager.DeleteAttributeMappingWithUniversalAttribute(View.SelectedTenantId, View.UniversalAttributeMappingID, View.currentLoggedInUserId);
        //    }
        //}

        //public void MappedUniversalItemData(Int32 categoryId, Int32 itemId)
        //{
        //    var mappedCatData = UniversalMappingDataManager.GetMappedUniversalCategoryDataByID(View.SelectedTenantId
        //                                                                                       , categoryId, UniversalMappingTypeEnum.COMPLIANCE_TYPE.GetStringValue());
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
        //        View.SelectedUniversalItemID = uniCatItemMapping.UCIM_UniversalItemID;

        //    }
        //}

        //public void MappedUniversalAttributeData()
        //{
        //    var mappedData = UniversalMappingDataManager.GetMappedUniversalAttributeDataByID(View.SelectedTenantId, View.MappedUniversalItemID, View.ItemAttributeMappingID);
        //    if (!mappedData.IsNullOrEmpty())
        //    {
        //        View.UniversalAttributeMappingID = mappedData.UAM_ID;
        //        View.SelectedUniversalItemAttrID = mappedData.UAM_UniversalItemAttributeMappingID;
        //        View.MappedUniversalItemAttributeID = mappedData.UAM_UniversalItemAttributeMappingID;
        //        //View.InputPriority = mappedData.UAM_InputPriority.IsNullOrEmpty() ? AppConsts.NONE : mappedData.UAM_InputPriority;
        //    }
        //}

        //public List<Entity.SharedDataEntity.UniversalAttribute> GetFilteredAttribute(String attrDataType, String attrType)
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

        //public void GetMappedInputAttributes()
        //{
        //    View.lstMappedInputAttributesData = UniversalMappingDataManager.GetMappedUniversalInputAttributeDataByID(View.SelectedTenantId, View.UniversalAttributeMappingID);
        //    if (!View.lstMappedInputAttributesData.IsNullOrEmpty())
        //    {
        //        View.selectedInputAttributes = View.lstMappedInputAttributesData.Select(x => x.UAITM_UniversalItemAttributeMappingID).ToList();
        //    }
        //}

        //public void SaveInputAttributeType()
        //{
        //    if (!View.lstSelectedInputAttributesData.IsNull())
        //    {
        //        UniversalMappingDataManager.UpdateAttributeInputMapping(View.SelectedTenantId, View.UniversalAttributeMappingID, View.lstSelectedInputAttributesData, View.currentLoggedInUserId);
        //    }
        //}
        //#endregion

        //#region UAT-2402:Additional Tracking to Rotation Mapping development and testing
        //public void GetUniversalAttributeOptions()
        //{
        //    View.lstUniversalAttributeOptions = UniversalMappingDataManager.GetUniversalAtrOptionData(View.SelectedUniversalItemAttrID);
        //}

        //public void GetMappedOptionsMapping()
        //{
        //    View.lstMappedAttributeOptionData = new List<ComplianceAttributeOptionMappingContract>();
        //   // View.lstMappedAttributeOptionData
        //      var lstMappedAttributeOptionData  = UniversalMappingDataManager.GetUniversalAttributeOptionMapping(View.SelectedTenantId, View.UniversalAttributeMappingID);

        //      if (!lstMappedAttributeOptionData.IsNullOrEmpty())
        //      {
        //          List<ComplianceAttributeOption> lstAttributeOption = BackgroundSetupManager.GetComplianceAttributeOption(View.SelectedTenantId, View.AttrID);

        //          lstMappedAttributeOptionData.ForEach(attrOption =>
        //          {
        //              ComplianceAttributeOptionMappingContract optionData = new ComplianceAttributeOptionMappingContract();
        //              //var mappedInputAttribute = lstMappedAttributeOptionData.FirstOrDefault(x => x.ComplianceAttributeOption.OptionText.Trim().ToLower() == attrOption.OptionText.Trim().ToLower());
        //              var mappedOptionAttribute = lstAttributeOption.FirstOrDefault(x => x.ComplianceAttributeOptionID == attrOption.UAOM_AttributeOptionID);
        //              if (!mappedOptionAttribute.IsNullOrEmpty())
        //              {
        //                  optionData.OptionText = mappedOptionAttribute.OptionText;
        //                  optionData.OptionValue = mappedOptionAttribute.OptionValue;
        //              }
        //              optionData.MappedUniversalOptionID = attrOption.UAOM_UniversalAttributeOptionID;

        //              View.lstMappedAttributeOptionData.Add(optionData);
        //          });
        //      }

        //}

        //private void SaveAttrOptionMapping()
        //{
        //    if (!View.lstSelectedAttributeOptionData.IsNull())
        //    {
        //        ComplianceAttribute compAttribute = GetComplianceAttribute(View.AttrID);
        //        List<ComplianceAttributeOption> lstCompAttrOption = new List<ComplianceAttributeOption>();
        //        if (!compAttribute.IsNullOrEmpty())
        //        {
        //            compAttribute.ComplianceAttributeOptions.ForEach(attrOpt =>
        //            {
        //                lstCompAttrOption.Add(attrOpt);
        //            });
        //        }
        //        SetUniversalAttrOptMapping(lstCompAttrOption);
        //    }
        //}

        //private void SetUniversalAttrOptMapping(List<ComplianceAttributeOption> lstCompAttrOption)
        //{
        //    List<UniversalAttributeOptionMapping> lstUniAttrOptMapping = new List<UniversalAttributeOptionMapping>();
        //    if (!lstCompAttrOption.IsNullOrEmpty())
        //    {
        //        View.lstSelectedAttributeOptionData.ForEach(attrOptData =>
        //        {
        //            UniversalAttributeOptionMapping uniAttrOptMapping = new UniversalAttributeOptionMapping();
        //            uniAttrOptMapping.UAOM_UniversalAttributeOptionID = attrOptData.MappedUniversalOptionID;
        //            uniAttrOptMapping.UAOM_UniversalAttributeMappingID = View.UniversalAttributeMappingID;
        //            var optData = lstCompAttrOption.FirstOrDefault(x => x.OptionText.Trim().ToLower() == attrOptData.OptionText.Trim().ToLower());
        //            uniAttrOptMapping.UAOM_AttributeOptionID = optData.IsNullOrEmpty() ? AppConsts.NONE : optData.ComplianceAttributeOptionID;
        //            uniAttrOptMapping.UAOM_CreatedBy = View.currentLoggedInUserId;
        //            uniAttrOptMapping.UAOM_CreatedOn = DateTime.Now;
        //            lstUniAttrOptMapping.Add(uniAttrOptMapping);
        //        });
        //    }
        //    UniversalMappingDataManager.SaveUniverAttrOptionMapping(View.SelectedTenantId, lstUniAttrOptMapping, View.UniversalAttributeMappingID, View.currentLoggedInUserId);
        //}
        //#endregion


        public Boolean IsMappedUniversalFieldActive()
        {
            return View.LstUniversalField.Any(any => any.UF_ID == View.MappedUniversalFieldID && !any.UF_IsDeleted);
        }

        #region UAT-4558
        public List<Entity.SystemDocument> GetFileUploadAdditionalDocs()//lkpComplianceAttributeDatatype attributeDatatype)
        {
            //String docTypeCode = String.Empty;
            List<Entity.SystemDocument> lstComplianceFileUploadDocumentSysDocs = new List<Entity.SystemDocument>();

            List<String> lstSatusCodeToFetch = new List<String>();
            List<Int32> lstDocStatusIDs = new List<Int32>();


            //if (!attributeDatatype.IsNullOrEmpty() && !String.IsNullOrEmpty(attributeDatatype.Code))
            //{
            //    if (String.Compare(attributeDatatype.Code.ToLower(), ComplianceAttributeDatatypes.FileUpload.GetStringValue().ToLower()) == 1)
            //    {
            lstSatusCodeToFetch.Add(DislkpDocumentType.ADDITIONAL_DOCUMENTS.GetStringValue());
            //Fetch only Additional Documents
            lstDocStatusIDs = SecurityManager.GetDocumentTypes().Where(x => lstSatusCodeToFetch.Contains(x.DT_Code) && x.DT_IsActive).Select(cond => cond.DT_ID).ToList();
            //    }
            //}

            if (!lstDocStatusIDs.IsNullOrEmpty() && lstDocStatusIDs.Count > AppConsts.NONE)
                lstComplianceFileUploadDocumentSysDocs = BackgroundSetupManager.GetBothUploadedDisclosureDocuments(lstDocStatusIDs).Where(x => !x.IsOperational.HasValue||(x.IsOperational.HasValue && x.IsOperational == false)).ToList();
            //lstComplianceFileUploadDocumentSysDocs.Insert(0, new Entity.SystemDocument { SystemDocumentID = 0, FileName = "--SELECT--" });
            return lstComplianceFileUploadDocumentSysDocs;
        }

        #endregion
    }
}




