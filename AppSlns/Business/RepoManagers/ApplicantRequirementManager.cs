using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;

namespace Business.RepoManagers
{
    public class ApplicantRequirementManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static ApplicantRequirementManager()
        {
            BALUtils.ClassModule = "Applicant Requirement Manager";
        }
        #endregion

        public static RequirementPackageSubscriptionContract GetRequirementPackageSubscription(Int32 requirementPackageSubscriptionID, Int32 tenantID)
        {
            try
            {
                RequirementPackageSubscription requirementPackageSubscription = BALUtils.GetApplicantRequirementRepoInstance(tenantID).GetRequirementPackageSubscription(requirementPackageSubscriptionID);
                RequirementPackageSubscriptionContract requirementPackageSubscriptionContract = ConvertSubscriptionDataToContract(requirementPackageSubscription);
                return requirementPackageSubscriptionContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static RequirementPackageContract GetRequirementPackageDetail(Int32 rotationRequirementPackageID, Int32 tenantID)
        {
            try
            {
                RequirementPackage requirementPackage = BALUtils.GetApplicantRequirementRepoInstance(tenantID).GetRequirementPackageDetail(rotationRequirementPackageID);
                RequirementPackageContract requirementPackageContract = ConvertRequirementPackageDataTocontract(requirementPackage);
                return requirementPackageContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static RequirementPackageSubscriptionContract ConvertSubscriptionDataToContract(RequirementPackageSubscription requirementPackageSubscription)
        {
            if (requirementPackageSubscription.IsNotNull())
            {
                RequirementPackageSubscriptionContract requirementPackageSubscriptionContract = new RequirementPackageSubscriptionContract();
                requirementPackageSubscriptionContract.ApplicantOrgUserID = requirementPackageSubscription.RPS_ApplicantOrgUserID;
                requirementPackageSubscriptionContract.RequirementPackageSubscriptionID = requirementPackageSubscription.RPS_ID;
                //UAT-1523: Addition a notes box for each rotation for the student to input information
                requirementPackageSubscriptionContract.Notes = requirementPackageSubscription.RPS_Notes;

                // DB Updated
                //requirementPackageSubscriptionContract.RequirementPackageID = requirementPackageSubscription.RPS_RequirementPackageID.Value;
                requirementPackageSubscriptionContract.RequirementPackageID = requirementPackageSubscription.ClinicalRotationSubscriptions.FirstOrDefault()
                                                                             .ClinicalRotationRequirementPackage.CRRP_RequirementPackageID;
                requirementPackageSubscriptionContract.HierarchyID = requirementPackageSubscription.ClinicalRotationSubscriptions.FirstOrDefault()
                    .ClinicalRotationRequirementPackage.ClinicalRotation.ClinicalRotationHierarchyMappings.Select(sel => sel.CRHM_HierarchyNodeID).FirstOrDefault().IsNullOrEmpty() ? -1 : requirementPackageSubscription.ClinicalRotationSubscriptions.FirstOrDefault()
                                                                             .ClinicalRotationRequirementPackage.ClinicalRotation.ClinicalRotationHierarchyMappings.Select(sel => sel.CRHM_HierarchyNodeID).FirstOrDefault();
                requirementPackageSubscriptionContract.RequirementPackageName = (requirementPackageSubscription.ClinicalRotationSubscriptions.FirstOrDefault()
                                                                                 .ClinicalRotationRequirementPackage.RequirementPackage.RP_PackageLabel.IsNullOrEmpty() ?
                                                                            requirementPackageSubscription.ClinicalRotationSubscriptions.FirstOrDefault()
                                                                            .ClinicalRotationRequirementPackage.RequirementPackage.RP_PackageName
                                                                           : requirementPackageSubscription.ClinicalRotationSubscriptions.FirstOrDefault()
                                                                            .ClinicalRotationRequirementPackage.RequirementPackage.RP_PackageLabel
                                                                        );

                requirementPackageSubscriptionContract.RequirementPackageSubscriptionStatusId = requirementPackageSubscription.RPS_RequirementPackageStatusID.IsNotNull() ? requirementPackageSubscription.RPS_RequirementPackageStatusID.Value : AppConsts.ONE;
                requirementPackageSubscriptionContract.RequirementPackageSubscriptionStatusCode = requirementPackageSubscription.lkpRequirementPackageStatu.IsNotNull() ? requirementPackageSubscription.lkpRequirementPackageStatu.RPS_Code
                                                                                                                     : String.Empty;
                //UAT-5040
                if (requirementPackageSubscription.ClinicalRotationSubscriptions.Count() > AppConsts.NONE
                    && !requirementPackageSubscription.ClinicalRotationSubscriptions.FirstOrDefault().ClinicalRotationRequirementPackage.IsNullOrEmpty())
                {
                    requirementPackageSubscriptionContract.RotationEndDate = requirementPackageSubscription.ClinicalRotationSubscriptions.FirstOrDefault()
                                                                             .ClinicalRotationRequirementPackage.ClinicalRotation.CR_EndDate;
                }
                else
                {
                    requirementPackageSubscriptionContract.RotationEndDate = null;
                }
                #region UAT-3122
                if (!requirementPackageSubscription.ClinicalRotationSubscriptions.FirstOrDefault().IsNullOrEmpty()
                    && !requirementPackageSubscription.ClinicalRotationSubscriptions.FirstOrDefault().ClinicalRotationRequirementPackage.IsNullOrEmpty()
                    && !requirementPackageSubscription.ClinicalRotationSubscriptions.FirstOrDefault().ClinicalRotationRequirementPackage.ClinicalRotation.IsNullOrEmpty())
                {
                    requirementPackageSubscriptionContract.ComplioID = requirementPackageSubscription.ClinicalRotationSubscriptions.FirstOrDefault().ClinicalRotationRequirementPackage.ClinicalRotation.CR_ComplioID;
                }
                #endregion

                List<ApplicantRequirementCategoryData> applicantRequirementCategoryDataList = requirementPackageSubscription.ApplicantRequirementCategoryDatas.Where(cond =>
                                                                                              !cond.ARCD_IsDeleted).ToList();
                if (applicantRequirementCategoryDataList.IsNotNull())
                {
                    applicantRequirementCategoryDataList = applicantRequirementCategoryDataList.OrderBy(ord => ord.RequirementCategory.RC_CreatedOn).ToList();
                }
                foreach (ApplicantRequirementCategoryData applicantRequirementCategoryData in applicantRequirementCategoryDataList)
                {
                    ApplicantRequirementCategoryDataContract requirementCategoryDataContract = new ApplicantRequirementCategoryDataContract();
                    requirementCategoryDataContract.RequirementCategoryID = applicantRequirementCategoryData.ARCD_RequirementCategoryID;
                    requirementCategoryDataContract.RequirementCategoryDataID = applicantRequirementCategoryData.ARCD_ID;
                    requirementCategoryDataContract.RequirementCategoryStatusID = applicantRequirementCategoryData.ARCD_RequirementCategoryStatusID;
                    requirementCategoryDataContract.RequirementCategoryStatus = applicantRequirementCategoryData.lkpRequirementCategoryStatu.IsNotNull() ? applicantRequirementCategoryData.lkpRequirementCategoryStatu.RCS_Name :
                                                                                                                                                    String.Empty;
                    requirementCategoryDataContract.CategoryRuleStatusID = Convert.ToString(applicantRequirementCategoryData.ARCD_RuleStatusID); //UAT 3106
                    requirementCategoryDataContract.RequirementCategoryStatusCode = applicantRequirementCategoryData.lkpRequirementCategoryStatu.RCS_Code;
                    //UAT-3122
                    if (!applicantRequirementCategoryData.RequirementCategory.IsNullOrEmpty())
                    {
                        requirementCategoryDataContract.RequirementCategoryName = !applicantRequirementCategoryData.RequirementCategory.RC_CategoryLabel.IsNullOrEmpty() ? applicantRequirementCategoryData.RequirementCategory.RC_CategoryLabel : applicantRequirementCategoryData.RequirementCategory.RC_CategoryName;
                    }
                    //requirementCategoryDataContract.
                    List<ApplicantRequirementItemData> applicantRequirementItemDataList = applicantRequirementCategoryData.ApplicantRequirementItemDatas.Where(cond => !cond.ARID_IsDeleted).ToList();
                    if (applicantRequirementItemDataList.IsNotNull())
                    {
                        applicantRequirementItemDataList = applicantRequirementItemDataList.OrderBy(ord => ord.RequirementItem.RI_CreatedOn).ToList();
                    }
                    foreach (ApplicantRequirementItemData applicantRequirementItemData in applicantRequirementItemDataList)
                    {
                        ApplicantRequirementItemDataContract requirementItemDataContract = new ApplicantRequirementItemDataContract();
                        requirementItemDataContract = ConvertApplicantItemDataTocontract(applicantRequirementItemData);
                        //requirementItemDataContract.RequirementItemDataID = applicantRequirementItemData.ARID_ID;
                        //requirementItemDataContract.RequirementCategoryDataID = applicantRequirementItemData.ARID_RequirementCategoryDataID;
                        //requirementItemDataContract.RequirementItemID = applicantRequirementItemData.ARID_RequirementItemID;
                        //requirementItemDataContract.RequirementItemStatusID = applicantRequirementItemData.ARID_RequirementItemStatusID;
                        //requirementItemDataContract.RequirementItemStatus = applicantRequirementItemData.lkpRequirementItemStatu.IsNotNull() ? applicantRequirementItemData.lkpRequirementItemStatu.RIS_Name : String.Empty;
                        //requirementItemDataContract.RequirementItemStatusCode = applicantRequirementItemData.lkpRequirementItemStatu.IsNotNull() ? applicantRequirementItemData.lkpRequirementItemStatu.RIS_Code : String.Empty;
                        if (requirementCategoryDataContract.ApplicantRequirementItemData.IsNull())
                            requirementCategoryDataContract.ApplicantRequirementItemData = new List<ApplicantRequirementItemDataContract>();
                        requirementCategoryDataContract.ApplicantRequirementItemData.Add(requirementItemDataContract);
                    }
                    if (requirementPackageSubscriptionContract.ApplicantRequirementCategoryData == null)
                        requirementPackageSubscriptionContract.ApplicantRequirementCategoryData = new List<ApplicantRequirementCategoryDataContract>();


                    requirementPackageSubscriptionContract.ApplicantRequirementCategoryData.Add(requirementCategoryDataContract);
                }
                if (!requirementPackageSubscription.IsNullOrEmpty() && requirementPackageSubscription.ClinicalRotationSubscriptions.Count() > AppConsts.NONE && !requirementPackageSubscription.ClinicalRotationSubscriptions.FirstOrDefault().ClinicalRotationRequirementPackage.IsNullOrEmpty())
                    requirementPackageSubscriptionContract.RotationID = requirementPackageSubscription.ClinicalRotationSubscriptions.FirstOrDefault().ClinicalRotationRequirementPackage.ClinicalRotation.CR_ID;
                else
                    requirementPackageSubscriptionContract.RotationID = AppConsts.NONE;
                return requirementPackageSubscriptionContract;
            }
            return null;
        }

        private static RequirementPackageContract ConvertRequirementPackageDataTocontract(RequirementPackage requirementPackage)
        {
            if (requirementPackage.IsNotNull())
            {
                RequirementPackageContract requirementPackageContract = new RequirementPackageContract();
                requirementPackageContract.RequirementPackageID = requirementPackage.RP_ID;
                requirementPackageContract.RequirementPackageName = requirementPackage.RP_PackageName;
                requirementPackageContract.RequirementPackageLabel = requirementPackage.RP_PackageLabel;
                requirementPackageContract.IsNewPackage = requirementPackage.RP_IsNewPackage;

                //requirementPackageContract.RequirementPackageDescription = requirementPackage.RP_Description;

                List<RequirementCategory> requirementCategoryList = requirementPackage.RequirementPackageCategories.Where(cond => !cond.RPC_IsDeleted).OrderBy(ord => ord.RPC_CDisplayOrder)
                                                                                                     .Select(sel => sel.RequirementCategory)
                                                                                                     .ToList();
                foreach (RequirementCategory requirementCategory in requirementCategoryList)
                {
                    RequirementCategoryContract requirementCategoryContract = new RequirementCategoryContract();
                    requirementCategoryContract.RequirementCategoryID = requirementCategory.RC_ID;
                    requirementCategoryContract.RequirementCategoryName = requirementCategory.RC_CategoryLabel.IsNullOrEmpty() ? requirementCategory.RC_CategoryName : requirementCategory.RC_CategoryLabel;
                    requirementCategoryContract.RequirementDocumentLink = requirementCategory.RC_SampleDocFormURL;
                    requirementCategoryContract.RequirementCategoryLabel = requirementCategory.RC_CategoryLabel.IsNullOrEmpty() ? requirementCategory.RC_CategoryName : requirementCategory.RC_CategoryLabel;
                    requirementCategoryContract.CategoryDisplayOrder = requirementCategory.RequirementPackageCategories.Where(cond => cond.RPC_RequirementPackageID == requirementPackage.RP_ID && !cond.RPC_IsDeleted).Select(sel => sel.RPC_CDisplayOrder).FirstOrDefault();
                    //requirementCategoryContract.RequirementCategoryDescription = requirementCategory.RC_Description.IsNullOrEmpty() ? String.Empty : requirementCategory.RC_Description;
                    //UAT-3161
                    requirementCategoryContract.RequirementDocumentLinkLabel = requirementCategory.RC_SampleDocFormUrlLabel;
                    var isCatEditableByApplicant = requirementCategory.RequirementObjectProperties.Where(cond => cond.ROTP_CategoryItemID == null && !cond.ROTP_IsDeleted && cond.ROTP_ItemFieldID == null).IsNullOrEmpty() ? (bool?)null : requirementCategory.RequirementObjectProperties.Where(cond => cond.ROTP_CategoryItemID == null && !cond.ROTP_IsDeleted && cond.ROTP_ItemFieldID == null).Select(cond => cond.ROTP_IsEditableByApplicant).FirstOrDefault();
                    if (isCatEditableByApplicant.IsNotNull())
                    {
                        requirementCategoryContract.IsEditableByApplicant = isCatEditableByApplicant;
                    }
                    else
                    {
                        requirementCategoryContract.IsEditableByApplicant = true;
                    }
                    List<RequirementItem> requirementItemList = requirementCategory.RequirementCategoryItems.Where(cond => !cond.RCI_IsDeleted).OrderBy(or => or.RCI_DisplayOrder) //UAT-3078
                                                                                                    .Select(sel => sel.RequirementItem)
                                                                                                    .ToList();
                    foreach (RequirementItem requirementItem in requirementItemList)
                    {
                        RequirementItemContract requirementItemContract = new RequirementItemContract();
                        requirementItemContract = ConvertRequirementItemDataTocontract(requirementItem, requirementCategoryContract.RequirementCategoryID);
                        //requirementItemContract.RequirementItemID = requirementItem.RI_ID;
                        //requirementItemContract.RequirementItemName = requirementItem.RI_ItemName;
                        //requirementItemContract.RequirementItemLabel = requirementItem.RI_ItemLabel.IsNullOrEmpty() ? requirementItem.RI_ItemName : requirementItem.RI_ItemLabel;
                        //requirementItemContract.RequirementItemDescription = requirementItem.RI_Description.IsNullOrEmpty() ? String.Empty : requirementItem.RI_Description;
                        if (requirementCategoryContract.LstRequirementItem.IsNull())
                            requirementCategoryContract.LstRequirementItem = new List<RequirementItemContract>();
                        requirementCategoryContract.LstRequirementItem.Add(requirementItemContract);
                    }
                    if (requirementPackageContract.LstRequirementCategory.IsNull())
                        requirementPackageContract.LstRequirementCategory = new List<RequirementCategoryContract>();
                    requirementPackageContract.LstRequirementCategory.Add(requirementCategoryContract);
                }
                return requirementPackageContract;
            }
            return null;
        }

        #region UAT-1316
        public static RequirementItemContract GetDataEntryRequirementItem(Int32 requirementItemId, Int32 tenantID, Int32 RequirementCategoryID)
        {
            try
            {
                RequirementItem requirementItem = BALUtils.GetApplicantRequirementRepoInstance(tenantID).GetDataEntryRequirementItem(requirementItemId);
                //RequirementItem requirementItem1 = requirementItem.RequirementCategoryItems.OrderBy(o => o.RCI_DisplayOrder).ToList();
                RequirementItemContract requirementItemContract = ConvertRequirementItemDataTocontract(requirementItem, RequirementCategoryID);
                return requirementItemContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        private static RequirementItemContract ConvertRequirementItemDataTocontract(RequirementItem requirementItem, Int32 RequirementCategoryID)
        {
            if (requirementItem.IsNotNull())
            {
                RequirementItemContract requirementItemContract = new RequirementItemContract();
                Int32? categoryItemDisplayOrder = requirementItem.RequirementCategoryItems.Where(cn => cn.RCI_RequirementItemID == requirementItem.RI_ID && !cn.RCI_IsDeleted).Select(sl => sl.RCI_DisplayOrder).FirstOrDefault();//UAT-3078
                requirementItemContract.RequirementItemID = requirementItem.RI_ID;
                requirementItemContract.RequirementItemName = requirementItem.RI_ItemLabel.IsNullOrEmpty() ? requirementItem.RI_ItemName : requirementItem.RI_ItemLabel;
                requirementItemContract.RequirementItemLabel = requirementItem.RI_ItemLabel.IsNullOrEmpty() ? requirementItem.RI_ItemName : requirementItem.RI_ItemLabel;
                requirementItemContract.RequirementItemDisplayOrder = categoryItemDisplayOrder == null ? 0 : Convert.ToInt32(categoryItemDisplayOrder);
                requirementItemContract.AllowItemDataEntry = requirementItem.RI_AllowItemDataEntry.HasValue ? requirementItem.RI_AllowItemDataEntry.Value : true;

                var requirementCategoryItemData = requirementItem.RequirementCategoryItems.Where(cn => cn.RCI_RequirementItemID == requirementItem.RI_ID && !cn.RCI_IsDeleted && cn.RCI_RequirementCategoryID == RequirementCategoryID).FirstOrDefault();
                var isCustomSettings = requirementCategoryItemData.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted && cond.ROTP_ItemFieldID == null).IsNullOrEmpty() ? (bool?)null : requirementCategoryItemData.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted && cond.ROTP_ItemFieldID == null).Select(cond => cond.ROTP_IsCustomSettings).FirstOrDefault();
                if (!isCustomSettings.IsNullOrEmpty() && Convert.ToBoolean(isCustomSettings))
                {
                    requirementItemContract.IsEditableByApplicant = requirementCategoryItemData.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted && cond.ROTP_ItemFieldID == null).IsNullOrEmpty() ? (bool?)null : requirementCategoryItemData.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted && cond.ROTP_ItemFieldID == null).Select(cond => cond.ROTP_IsEditableByApplicant).FirstOrDefault();
                }
                else
                {
                    var isEditableByApplicant = requirementCategoryItemData.RequirementCategory.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted && cond.ROTP_ItemFieldID == null && cond.ROTP_CategoryItemID == null).IsNullOrEmpty() ? (bool?)null : requirementCategoryItemData.RequirementCategory.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted && cond.ROTP_ItemFieldID == null && cond.ROTP_CategoryItemID == null).Select(cond => cond.ROTP_IsEditableByApplicant).FirstOrDefault();
                    if (isEditableByApplicant.IsNotNull())
                    {
                        requirementItemContract.IsEditableByApplicant = Convert.ToBoolean(isEditableByApplicant);
                    }
                    else
                    {
                        requirementItemContract.IsEditableByApplicant = true;
                    }
                    //   requirementItemContract.IsEditableByApplicant = RequirementCategory.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted).Select(cond => cond.ROTP_IsEditableByApplicant).FirstOrDefault();                  
                }
                //requirementItemContract.RequirementItemDescription = requirementItem.RI_Description.IsNullOrEmpty() ? String.Empty : requirementItem.RI_Description;

                #region UAT-3077
                requirementItemContract.Amount = requirementItem.RI_Amount.HasValue ? requirementItem.RI_Amount.Value : AppConsts.NONE;
                requirementItemContract.IsPaymentType = requirementItem.RI_IsPaymentType.HasValue ? requirementItem.RI_IsPaymentType.Value : false;
                #endregion

                requirementItemContract.RequirementItemSampleDocumentFormURL = requirementItem.RI_SampleDocFormURL; //UAT-3309

                List<Entity.SharedDataEntity.lkpComplianceAttributeType> lstlkpComplianceAttrType = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpComplianceAttributeType>();

                List<RequirementField> requirementFieldList = requirementItem.RequirementItemFields.Where(cond => !cond.RIF_IsDeleted).OrderBy(or => or.RIF_DisplayOrder) //UAT-3078
                                                                                                     .Select(sel => sel.RequirementField).ToList(); //OrderBy(ord => ord.RF_CreatedOn).
                foreach (RequirementField requirementField in requirementFieldList)
                {
                    Int32 requirementItemFieldId = requirementItem.RequirementItemFields.FirstOrDefault(cnd => cnd.RIF_RequirementFieldID == requirementField.RF_ID
                                                                                                        && !cnd.RIF_IsDeleted).RIF_ID;
                    Int32? displayOrder = requirementField.RequirementItemFields.Where(cnd => cnd.RIF_RequirementFieldID == requirementField.RF_ID).Select(sel => sel.RIF_DisplayOrder).FirstOrDefault();

                    RequirementFieldContract requirementFieldContract = new RequirementFieldContract();
                    requirementFieldContract.RequirementFieldID = requirementField.RF_ID;
                    requirementFieldContract.RequirementFieldName = requirementField.RF_FieldLabel.IsNullOrEmpty() ? requirementField.RF_FieldName : requirementField.RF_FieldLabel;
                    requirementFieldContract.RequirementFieldLabel = requirementField.RF_FieldLabel.IsNullOrEmpty() ? requirementField.RF_FieldName : requirementField.RF_FieldLabel;
                    requirementFieldContract.RequirementFieldDescription = requirementField.RF_Description;
                    requirementFieldContract.RequirementItemID = requirementItemContract.RequirementItemID;
                    requirementFieldContract.RequirementItemFieldID = requirementItemFieldId;
                    requirementFieldContract.RequirementFieldMaxLength = requirementField.RF_MaximumCharacters;
                    requirementFieldContract.AttributeTypeCode = lstlkpComplianceAttrType.Where(x => x.ComplianceAttributeTypeID == requirementField.RF_AttributeTypeID).Any() ? lstlkpComplianceAttrType.Where(x => x.ComplianceAttributeTypeID == requirementField.RF_AttributeTypeID).FirstOrDefault().Code : AppConsts.REQUIREMENT_FIELD_ATTRIBUTE_TYPE_MANUAL_CODE;
                    requirementFieldContract.RequirementFieldDataTypeCode = requirementField.lkpRequirementFieldDataType.RFDT_Code;
                    requirementFieldContract.RequirementFieldDisplayOrder = displayOrder == null ? 0 : Convert.ToInt32(displayOrder); //UAT-3078

                    List<RequirementFieldOption> requirementOptionList = requirementField.RequirementFieldOptions.Where(cnd => !cnd.RFO_IsDeleted).ToList();
                    RequirementFieldVideo requirementFieldVideo = requirementField.RequirementFieldVideos.FirstOrDefault(cond => !cond.RFV_IsDeleted);
                    RequirementFieldDocument requirementFieldDocument = requirementField.RequirementFieldDocuments.FirstOrDefault(cnd => !cnd.RFD_IsDeleted
                                                                                                                   && !cnd.ClientSystemDocument.CSD_IsDeleted);
                    var requirementItemFieldData = requirementField.RequirementItemFields.Where(cn => cn.RIF_RequirementFieldID == requirementField.RF_ID && !cn.RIF_IsDeleted).FirstOrDefault();
                    //UAT-4380
                    var isCustomSettings2 = requirementItemFieldData.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted).IsNullOrEmpty() ? (bool?)null : requirementItemFieldData.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted).Select(cond => cond.ROTP_IsCustomSettings).FirstOrDefault();
                    if (!isCustomSettings2.IsNullOrEmpty() && Convert.ToBoolean(isCustomSettings2))
                    {
                        requirementFieldContract.IsEditableByApplicant = requirementItemFieldData.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted
                        && cond.ROTP_CategoryID == requirementCategoryItemData.RCI_RequirementCategoryID && cond.ROTP_CategoryItemID == requirementCategoryItemData.RCI_ID).IsNullOrEmpty() ?
                            (bool?)null :
                            requirementItemFieldData.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted
                            && cond.ROTP_CategoryID == requirementCategoryItemData.RCI_RequirementCategoryID && cond.ROTP_CategoryItemID == requirementCategoryItemData.RCI_ID).Select(cond => cond.ROTP_IsEditableByApplicant).FirstOrDefault();

                        if (!requirementItemContract.IsEditableByApplicant.Value && requirementFieldContract.IsEditableByApplicant.HasValue && requirementFieldContract.IsEditableByApplicant.Value)
                            requirementItemContract.IsEditableByApplicant = true;
                    }
                    else
                    {
                        var isEditableByApplicant = requirementCategoryItemData.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted && cond.ROTP_ItemFieldID == null).IsNullOrEmpty() ? (bool?)null : requirementCategoryItemData.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted && cond.ROTP_ItemFieldID == null).Select(cond => cond.ROTP_IsEditableByApplicant).FirstOrDefault();
                        if (isEditableByApplicant.IsNotNull())
                        {
                            requirementFieldContract.IsEditableByApplicant = Convert.ToBoolean(isEditableByApplicant);
                            if (!requirementItemContract.IsEditableByApplicant.Value && requirementFieldContract.IsEditableByApplicant.HasValue && requirementFieldContract.IsEditableByApplicant.Value)
                                requirementItemContract.IsEditableByApplicant = true;
                        }
                        else
                        {
                            var isEditableByApplicant2 = requirementCategoryItemData.RequirementCategory.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted && cond.ROTP_ItemFieldID == null && cond.ROTP_CategoryItemID == null).IsNullOrEmpty() ? (bool?)null : requirementCategoryItemData.RequirementCategory.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted && cond.ROTP_ItemFieldID == null && cond.ROTP_CategoryItemID == null).Select(cond => cond.ROTP_IsEditableByApplicant).FirstOrDefault();
                            if (isEditableByApplicant2.IsNotNull())
                            {
                                requirementFieldContract.IsEditableByApplicant = Convert.ToBoolean(isEditableByApplicant2);
                                if (!requirementItemContract.IsEditableByApplicant.Value && requirementFieldContract.IsEditableByApplicant.HasValue && requirementFieldContract.IsEditableByApplicant.Value)
                                    requirementItemContract.IsEditableByApplicant = true;
                            }
                            else
                            {
                                requirementFieldContract.IsEditableByApplicant = true;
                            }
                            //   requirementItemContract.IsEditableByApplicant = RequirementCategory.RequirementObjectProperties.Where(cond => !cond.ROTP_IsDeleted).Select(cond => cond.ROTP_IsEditableByApplicant).FirstOrDefault();                  
                        }
                    }
                    if (requirementFieldContract.AttributeTypeCode == ComplianceAttributeType.Calculated.GetStringValue())
                        requirementFieldContract.IsEditableByApplicant = false;
                    //END UAT
                    requirementFieldContract.RequirementFieldData = new RequirementFieldDataContract();

                    //Assign DataType code and Data Type id
                    requirementFieldContract.RequirementFieldData.RequirementFieldDataTypeCode = requirementField.lkpRequirementFieldDataType.RFDT_Code;
                    requirementFieldContract.RequirementFieldData.RequirementFieldDataTypeID = requirementField.lkpRequirementFieldDataType.RFDT_ID;

                    foreach (RequirementFieldOption requirementFieldOption in requirementOptionList)
                    {
                        RequirementFieldOptionsData requirementFieldOptionData = new RequirementFieldOptionsData();
                        requirementFieldOptionData.RequirementFieldOptionsID = requirementFieldOption.RFO_ID;
                        requirementFieldOptionData.OptionValue = requirementFieldOption.RFO_OptionValue;
                        requirementFieldOptionData.OptionText = requirementFieldOption.RFO_OptionText;
                        requirementFieldOptionData.RequirementFieldID = requirementField.RF_ID;

                        if (requirementFieldContract.RequirementFieldData.LstRequirementFieldOptions.IsNull())
                        {
                            requirementFieldContract.RequirementFieldData.LstRequirementFieldOptions = new List<RequirementFieldOptionsData>();
                        }
                        requirementFieldContract.RequirementFieldData.LstRequirementFieldOptions.Add(requirementFieldOptionData);
                    }

                    if (requirementFieldVideo.IsNotNull())
                    {
                        RequirementFieldVideoData requirementFieldVideoData = new RequirementFieldVideoData();
                        requirementFieldVideoData.RequirementFieldVideoID = requirementFieldVideo.RFV_ID;
                        requirementFieldVideoData.VideoName = requirementFieldVideo.RFV_VideoName;
                        //requirementFieldVideoData.IsVideoRequiredToBeOpened = requirementFieldVideo.RFV_IsVideoOpenRequired;
                        requirementFieldVideoData.VideoURL = requirementFieldVideo.RFV_VideoURL;
                        //requirementFieldVideoData.VideoOpenTimeDuration = requirementFieldVideo.RFV_VideoOpenTimeDuration.IsNotNull() ? 
                        //                                                  requirementFieldVideo.RFV_VideoOpenTimeDuration.Value : AppConsts.NONE;
                        if (requirementFieldContract.RequirementFieldData.VideoFieldData.IsNull())
                        {
                            requirementFieldContract.RequirementFieldData.VideoFieldData = new RequirementFieldVideoData();
                        }
                        requirementFieldContract.RequirementFieldData.VideoFieldData = requirementFieldVideoData;
                    }

                    if (requirementFieldDocument.IsNotNull())
                    {
                        RequirementFieldViewDocumentData requirementFieldViewDocData = new RequirementFieldViewDocumentData();
                        requirementFieldViewDocData.ClientSystemDocumentID = requirementFieldDocument.RFD_ClientSystemDocumentID.IsNotNull() ?
                                                                             requirementFieldDocument.RFD_ClientSystemDocumentID.Value : AppConsts.NONE;
                        requirementFieldViewDocData.DocumentDescription = requirementFieldDocument.ClientSystemDocument.CSD_Description;
                        requirementFieldViewDocData.DocumentFileName = requirementFieldDocument.ClientSystemDocument.CSD_FileName;
                        requirementFieldViewDocData.DocumentPath = requirementFieldDocument.ClientSystemDocument.CSD_DocumentPath;
                        requirementFieldViewDocData.DocumentSize = requirementFieldDocument.ClientSystemDocument.CSD_Size.IsNotNull() ?
                                                                    requirementFieldDocument.ClientSystemDocument.CSD_Size.Value : AppConsts.NONE;
                        if (requirementFieldContract.RequirementFieldData.FieldViewDocumentData.IsNull())
                        {
                            requirementFieldContract.RequirementFieldData.FieldViewDocumentData = new RequirementFieldViewDocumentData();
                        }
                        requirementFieldContract.RequirementFieldData.FieldViewDocumentData = requirementFieldViewDocData;
                    }

                    //Add requirement field list in requirement item contract.
                    if (requirementItemContract.LstRequirementField.IsNull())
                    {
                        requirementItemContract.LstRequirementField = new List<RequirementFieldContract>();
                    }
                    requirementItemContract.LstRequirementField.Add(requirementFieldContract);
                }
                return requirementItemContract;
            }
            return null;
        }

        public static List<ApplicantDocumentContract> GetApplicantDocument(Int32 currentLoggedInUserID, Int32 tenantID)
        {
            try
            {
                List<lkpDocumentType> docType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(tenantID).Where(x => x.DMT_IsDeleted == false).ToList();
                Int32 reqFieldUploadedDocTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.REQUIREMENT_FIELD_UPLOAD_DOCUMENT.GetStringValue())).FirstOrDefault().DMT_ID;
                Int32 complianceViewDocumentTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.COMPLIANCE_VIEW_DOCUMENT.GetStringValue())).FirstOrDefault().DMT_ID;
                Int32 additionalDocumentTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.ADDITIONAL_DOCUMENTS.GetStringValue())).FirstOrDefault().DMT_ID;
                Int32 screeningDocumentAtrTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.SCREENING_DOCUMENT_ATTRIBUTE_TYPE_DOCUMENT.GetStringValue())).FirstOrDefault().DMT_ID;
                Int32 personalDocumentAtrTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.PERSONAL_DOCUMENT.GetStringValue())).FirstOrDefault().DMT_ID;
                //Start UAT-4900
                Int32 requirementViewDocumentTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.REQUIREMENT_FIELD_VIEW_DOCUMENT.GetStringValue())).FirstOrDefault().DMT_ID;
                //End UAT-4900
                List<Int32?> lstDocType = new List<int?>();
                lstDocType.Add(reqFieldUploadedDocTypeId);
                lstDocType.Add(complianceViewDocumentTypeId);
                lstDocType.Add(additionalDocumentTypeId);
                lstDocType.Add(screeningDocumentAtrTypeId);
                //Start UAT-4900
                lstDocType.Add(requirementViewDocumentTypeId);
                //End UAT-4900
                lstDocType.Add(null); //Document upload by applicant is of NULL type 

                List<ApplicantDocument> applicantDocList = BALUtils.GetApplicantRequirementRepoInstance(tenantID).GetApplicantDocument(currentLoggedInUserID, lstDocType);
                if (!applicantDocList.IsNullOrEmpty())
                {
                    applicantDocList = applicantDocList.Where(cond => cond.DocumentType.IsNull() || (cond.DocumentType != personalDocumentAtrTypeId)).ToList();
                }
                List<ApplicantDocumentContract> applicantDocumnetContract = ConvertApplicantDocumnetTocontract(applicantDocList);
                return applicantDocumnetContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static List<ApplicantDocument> FilterApplcantDocument(List<ApplicantDocument> appliDocList, Int32 tenantId)
        {
            try
            {
                List<ApplicantDocument> applicantDocuments = new List<ApplicantDocument>();
                if (appliDocList.IsNotNull())
                {
                    List<lkpDocumentType> docType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(tenantId).Where(x => x.DMT_IsDeleted == false).ToList();
                    Int32 disclaimerId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.DisclaimerDocument.GetStringValue())).FirstOrDefault().DMT_ID;
                    Int32 disclosureId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.DisclosureDocument.GetStringValue())).FirstOrDefault().DMT_ID;
                    Int32 edsDocTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.EDS_AuthorizationForm.GetStringValue())).FirstOrDefault().DMT_ID;
                    Int32 dnrDocTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.Disclosure_n_Release.GetStringValue())).FirstOrDefault().DMT_ID;
                    //UAT-1035 WB: Capture and store order summary and add "Print Receipt" button to Order history for each order's order history. 
                    Int32 rcptDocTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.Reciept_Document.GetStringValue())).FirstOrDefault().DMT_ID;

                    //UAT-1316 WB:Clinical Rotation Details screen for student.
                    Int32 reqUpldDocTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.REQUIREMENT_FIELD_UPLOAD_DOCUMENT.GetStringValue())).FirstOrDefault().DMT_ID;
                    Int32 reqViewDocTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.REQUIREMENT_FIELD_VIEW_DOCUMENT.GetStringValue())).FirstOrDefault().DMT_ID;
                    Int32 rotSyllabusDocTypeId = docType.Where(condition => condition.DMT_Code.Equals(DocumentType.ROTATION_SYLLABUS.GetStringValue())).FirstOrDefault().DMT_ID;

                    applicantDocuments = appliDocList.Where(condition => (condition.DocumentType.IsNull() || (condition.DocumentType != disclaimerId
                       && condition.DocumentType != disclosureId && condition.DocumentType != edsDocTypeId && condition.DocumentType != dnrDocTypeId && condition.DocumentType != rcptDocTypeId
                       && condition.DocumentType != reqUpldDocTypeId && condition.DocumentType != reqViewDocTypeId && condition.DocumentType != rotSyllabusDocTypeId
                       && (condition.IsSearchableOnly == null || condition.IsSearchableOnly == false)//UAT-1560:WB: We should be able to add documents that need to be signed to the order process
                       ))).ToList();


                }
                return applicantDocuments;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }

        }

        private static List<ApplicantDocumentContract> ConvertApplicantDocumnetTocontract(List<ApplicantDocument> applicantDocumentList)
        {
            return applicantDocumentList.Select(doc => new ApplicantDocumentContract
            {
                ApplicantDocumentId = doc.ApplicantDocumentID,
                Description = doc.Description,
                DocumentPath = doc.DocumentPath,
                FileName = doc.FileName
            }).ToList();

        }

        public static ApplicantRequirementItemDataContract GetApplicantRequirementItemData(ApplicantRequirementParameterContract requirementParameters, Int32 currentLoggedInUserId)
        {
            try
            {
                ApplicantRequirementItemData appItemData = BALUtils.GetApplicantRequirementRepoInstance(requirementParameters.TenantId).GetApplicantRequirementItemData(
                                                           requirementParameters.RequirementPkgSubscriptionId, requirementParameters.RequirementItemId,
                                                           requirementParameters.RequirementCategoryId, currentLoggedInUserId);
                ApplicantRequirementItemDataContract appItemDataContract = ConvertApplicantItemDataTocontract(appItemData);
                //START UAT-4468
                if (!appItemData.IsNullOrEmpty())
                    appItemDataContract.ApplicantName = appItemData.ApplicantRequirementCategoryData.RequirementPackageSubscription.OrganizationUser.FirstName + " " + appItemData.ApplicantRequirementCategoryData.RequirementPackageSubscription.OrganizationUser.LastName;
                //END UAT-4468
                return appItemDataContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static ApplicantRequirementItemDataContract ConvertApplicantItemDataTocontract(ApplicantRequirementItemData appItemData)
        {

            ApplicantRequirementItemDataContract applicantItemDataContract = new ApplicantRequirementItemDataContract();
            bool IsItemExpired = false;
            if (appItemData.IsNotNull())
            {
                applicantItemDataContract.RequirementItemDataID = appItemData.ARID_ID;
                applicantItemDataContract.RequirementCategoryDataID = appItemData.ARID_RequirementCategoryDataID;
                applicantItemDataContract.RequirementItemID = appItemData.ARID_RequirementItemID;
                applicantItemDataContract.RequirementItemStatusID = appItemData.ARID_RequirementItemStatusID;
                applicantItemDataContract.RequirementItemStatus = appItemData.lkpRequirementItemStatu.IsNotNull() ?
                                                                  appItemData.lkpRequirementItemStatu.RIS_Name : String.Empty;
                applicantItemDataContract.RequirementItemStatusCode = appItemData.lkpRequirementItemStatu.IsNotNull() ?
                                                                      appItemData.lkpRequirementItemStatu.RIS_Code : String.Empty;

                if (applicantItemDataContract.RequirementItemStatusCode == "AAAE")
                {
                    IsItemExpired = true;
                }

                //applicantItemDataContract.RequirementItemName = appItemData.RequirementItem.RI_ItemName;
                applicantItemDataContract.RequirementItemName = appItemData.RequirementItem.RI_ItemLabel.IsNullOrEmpty() ? appItemData.RequirementItem.RI_ItemName : appItemData.RequirementItem.RI_ItemLabel;

                //UAT-2226
                applicantItemDataContract.RejectionReason = appItemData.ARID_RejectionReason;

                //UAT - 3299
                if (!appItemData.ARID_ItemMovementTypeID.IsNullOrEmpty())
                {
                    applicantItemDataContract.ItemMovementTypeCode = appItemData.lkpItemMovementType.Code;
                }
                List<ApplicantRequirementFieldData> appFieldDataList = appItemData.ApplicantRequirementFieldDatas.Where(cnd => !cnd.ARFD_IsDeleted).ToList();
                if (appFieldDataList.IsNotNull())
                {
                    appFieldDataList = appFieldDataList.OrderBy(ord => ord.RequirementField.RF_CreatedOn).ToList();
                }

                //Start UAT-4900
                String viewDocTypeID = RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue();
                var viewDocMapData = appFieldDataList.Where(x => x.RequirementField.lkpRequirementFieldDataType != null && x.RequirementField.lkpRequirementFieldDataType.RFDT_Code == viewDocTypeID).Select(x => x.ApplicantRequirementDocumentMaps).FirstOrDefault();
                //End UAT-4900

                foreach (ApplicantRequirementFieldData appFieldData in appFieldDataList)
                {
                    if (appItemData.RequirementItem.RequirementItemFields.Where(cond => cond.RIF_RequirementFieldID == appFieldData.ARFD_RequirementFieldID && !cond.RIF_IsDeleted).Any())
                    {
                        ApplicantRequirementFieldDataContract appFieldDataContract = new ApplicantRequirementFieldDataContract();
                        appFieldDataContract.RequirementItemDataID = appFieldData.ARFD_RequirementItemDataID;
                        appFieldDataContract.RequirementFieldID = appFieldData.ARFD_RequirementFieldID;
                        appFieldDataContract.FieldValue = appFieldData.ARFD_FieldValue;



                        appFieldDataContract.ApplicantReqFieldDataID = appFieldData.ARFD_ID;

                        if (!appFieldData.RequirementFieldDataLargeContents.IsNullOrEmpty())
                        {
                            appFieldDataContract.Signature = appFieldData.RequirementFieldDataLargeContents
                                                                .Where(cond => cond.RFDLC_ApplicantRequirementFieldDataID == appFieldData.ARFD_ID)
                                                                .FirstOrDefault().RFDLC_Signature;
                        }

                        //UAT-3078
                        var reqItemFieldData = appItemData.RequirementItem.RequirementItemFields.
                                         Where(cond => cond.RIF_RequirementFieldID == appFieldData.ARFD_RequirementFieldID && !cond.RIF_IsDeleted).
                                         FirstOrDefault();
                        if (!reqItemFieldData.IsNullOrEmpty())
                            appFieldDataContract.RequirementFieldDisplayOrder = reqItemFieldData.RIF_DisplayOrder.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(reqItemFieldData.RIF_DisplayOrder);

                        if (!appFieldData.RequirementField.RF_MaximumCharacters.IsNullOrEmpty())
                        {
                            if (appFieldData.RequirementField.RF_MaximumCharacters > AppConsts.NONE)
                            {
                                appFieldDataContract.FieldMaxLength = appFieldData.RequirementField.RF_MaximumCharacters;
                            }
                        }
                        appFieldDataContract.FieldDataTypeCode = appFieldData.RequirementField.lkpRequirementFieldDataType.IsNotNull() ?
                                                                 appFieldData.RequirementField.lkpRequirementFieldDataType.RFDT_Code : String.Empty;
                        if (appFieldDataContract.FieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue()
                            || appFieldDataContract.FieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
                        {
                            Int32 IsSuccessParseInt = 0;
                            bool Issuccess = Int32.TryParse(appFieldData.ARFD_FieldValue, out IsSuccessParseInt);

                            if (IsItemExpired && Issuccess && IsSuccessParseInt == AppConsts.NONE)
                                appFieldDataContract.FieldValueViewDoc = "Expired";
                            List<ApplicantRequirementDocumentMap> appDocumentMappingList = appFieldData.ApplicantRequirementDocumentMaps
                                                                                           .Where(cond => !cond.ARDM_IsDeleted).ToList();
                            foreach (ApplicantRequirementDocumentMap appDocumentMapping in appDocumentMappingList)
                            {
                                ApplicantFieldDocumentMappingContract appFieldDocMapContract = new ApplicantFieldDocumentMappingContract();
                                appFieldDocMapContract.ApplicantDocumentId = appDocumentMapping.ARDM_ApplicantDocumentID;
                                appFieldDocMapContract.ApplicantReqFieldDataId = appDocumentMapping.ARDM_ApplicantRequirementFieldDataID;
                                appFieldDocMapContract.DocumentPath = appDocumentMapping.ApplicantDocument.DocumentPath;
                                appFieldDocMapContract.Description = appDocumentMapping.ApplicantDocument.Description;
                                appFieldDocMapContract.DocumentType = appDocumentMapping.ApplicantDocument.lkpDocumentType.IsNotNull() ?
                                                                      appDocumentMapping.ApplicantDocument.lkpDocumentType.DMT_Code : String.Empty;
                                appFieldDocMapContract.FileName = appDocumentMapping.ApplicantDocument.FileName;
                                appFieldDocMapContract.ApplicantRequirementDocumentMapId = appDocumentMapping.ARDM_ID;

                                //Start UAT-4900
                                if (appFieldDataContract.FieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue() && !viewDocMapData.IsNullOrEmpty())
                                {
                                    appFieldDocMapContract.IsDisabled = viewDocMapData.Any(x => x.ARDM_ApplicantDocumentID == appDocumentMapping.ARDM_ApplicantDocumentID && !x.ARDM_IsDeleted);
                                }
                                //End UAT-4900

                                if (appFieldDataContract.LstApplicantFieldDocumentMapping.IsNull())
                                {
                                    appFieldDataContract.LstApplicantFieldDocumentMapping = new List<ApplicantFieldDocumentMappingContract>();
                                }
                                appFieldDataContract.LstApplicantFieldDocumentMapping.Add(appFieldDocMapContract);
                            }
                        }
                        if (applicantItemDataContract.ApplicantRequirementFieldData.IsNull())
                        {
                            applicantItemDataContract.ApplicantRequirementFieldData = new List<ApplicantRequirementFieldDataContract>();
                        }
                        applicantItemDataContract.ApplicantRequirementFieldData.Add(appFieldDataContract);
                    }
                }
            }
            return applicantItemDataContract;
        }

        public static RequirementFieldVideoData GetRequirementFieldVideoData(Int32 rfVideoId, Int32 tenantID)
        {
            try
            {
                RequirementFieldVideo requirementVideoData = BALUtils.GetApplicantRequirementRepoInstance(tenantID).GetRequirementFieldVideoData(rfVideoId);
                RequirementFieldVideoData requirementVideoDataContract = ConvertRequirementFieldVideoDataToContract(requirementVideoData);
                return requirementVideoDataContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static RequirementFieldVideoData ConvertRequirementFieldVideoDataToContract(RequirementFieldVideo rfVideoItem)
        {
            RequirementFieldVideoData rfVideoItemContract = new RequirementFieldVideoData();
            if (rfVideoItem.IsNotNull())
            {
                rfVideoItemContract.RequirementFieldVideoID = rfVideoItem.RFV_ID;
                rfVideoItemContract.VideoName = rfVideoItem.RFV_VideoName;
                rfVideoItemContract.VideoURL = rfVideoItem.RFV_VideoURL;
            }
            return rfVideoItemContract;
        }

        public static RequirementObjectTreeContract GetObjectTreeProperty(Int32 rotId, Int32 tenantId)
        {
            try
            {
                String attrObjTypeCode = ObjectAttribute.BOX_STAYS_OPEN_TIME.GetStringValue();
                lkpObjectAttribute attrObjectType = LookupManager.GetLookUpData<lkpObjectAttribute>(tenantId).FirstOrDefault(cond => cond.OA_Code == attrObjTypeCode);
                Int32 lkpObjectTypeId = attrObjectType.IsNull() ? 0 : attrObjectType.OA_ID;
                RequirementObjectTreeProperty requirmentObjectTreeProperty = BALUtils.GetApplicantRequirementRepoInstance(tenantId).GetObjectTreeProperty(rotId, lkpObjectTypeId);
                RequirementObjectTreeContract requirmentObjectTreeContract = ConvertGetObjectTreePropertyDataToContract(requirmentObjectTreeProperty);
                return requirmentObjectTreeContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static RequirementObjectTreeContract ConvertGetObjectTreePropertyDataToContract(RequirementObjectTreeProperty requirmentObjectTreeProperty)
        {
            RequirementObjectTreeContract requirmentObjectTreeContract = new RequirementObjectTreeContract();
            if (!requirmentObjectTreeProperty.IsNull())
            {
                requirmentObjectTreeContract.ObjectID = requirmentObjectTreeProperty.ROTP_ObjectTreeID;
                requirmentObjectTreeContract.ObjectAttributeTypeId = requirmentObjectTreeProperty.ROTP_ObjectAttributeID.IsNull() ? 0 : requirmentObjectTreeProperty.ROTP_ObjectAttributeID.Value;
                requirmentObjectTreeContract.ObjectAttributeValue = requirmentObjectTreeProperty.ROTP_ObjectAttributeValue.IsNullOrEmpty() ? "0" : requirmentObjectTreeProperty.ROTP_ObjectAttributeValue;
            }
            return requirmentObjectTreeContract;
        }

        public static ApplicantDocumentContract GetClientSystemDocument(Int32 clientSystemDocId, Int32 tenantId)
        {
            try
            {
                ClientSystemDocument clientSystemDocument = BALUtils.GetApplicantRequirementRepoInstance(tenantId).GetClientSystemDocument(clientSystemDocId);
                ApplicantDocumentContract clientSystemDocContract = ConvertGetClientSystemDocumentDataToContract(clientSystemDocument);
                return clientSystemDocContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        private static ApplicantDocumentContract ConvertGetClientSystemDocumentDataToContract(ClientSystemDocument clientSystemDocument)
        {
            ApplicantDocumentContract clientSystemDocContract = new ApplicantDocumentContract();
            if (!clientSystemDocument.IsNull())
            {
                clientSystemDocContract.FileName = clientSystemDocument.CSD_FileName;
                clientSystemDocContract.DocumentPath = clientSystemDocument.CSD_DocumentPath;
                clientSystemDocContract.Description = clientSystemDocContract.Description;
            }
            return clientSystemDocContract;
        }

        #region Applicant Requirement Data
        public static Dictionary<Boolean, String> SaveApplicantRequirementData(ApplicantRequirementParameterContract requirementParameters, Int32 currentLoggedInUserId, Int32 orgUsrID)
        {
            try
            {
                List<lkpRequirementCategoryStatu> LstReqCategoryStatus = LookupManager.GetLookUpData<lkpRequirementCategoryStatu>(requirementParameters.TenantId)
                                                                        .Where(cnd => !cnd.RCS_IsDeleted).ToList();
                List<lkpRequirementItemStatu> LstReqItemStatus = LookupManager.GetLookUpData<lkpRequirementItemStatu>(requirementParameters.TenantId)
                                                                 .Where(cnd => !cnd.RIS_IsDeleted).ToList();
                List<lkpRequirementFieldDataType> LstReqFieldDataType = LookupManager.GetLookUpData<lkpRequirementFieldDataType>(requirementParameters.TenantId)
                                                                 .Where(cnd => !cnd.RFDT_IsDeleted).ToList();
                List<lkpDataEntryDocumentStatu> LstDataEntryDocStatus = LookupManager.GetLookUpData<lkpDataEntryDocumentStatu>(requirementParameters.TenantId)
                                                                 .Where(cnd => !cnd.LDEDS_IsDeleted).ToList();
                List<lkpDocumentType> LstDocumentType = LookupManager.GetLookUpData<lkpDocumentType>(requirementParameters.TenantId)
                                                                 .Where(cnd => !cnd.DMT_IsDeleted).ToList();

                Dictionary<Int32, ApplicantDocument> signedAppDoc = new Dictionary<int, ApplicantDocument>();

                ApplicantRequirementCategoryData categoryData = new ApplicantRequirementCategoryData
                {
                    ARCD_ID = requirementParameters.AppRequirementCategoryData.RequirementCategoryDataID > 0 ?
                                                    requirementParameters.AppRequirementCategoryData.RequirementCategoryDataID : 0,
                    ARCD_RequirementPackageSubscriptionID = requirementParameters.RequirementPkgSubscriptionId,
                    ARCD_RequirementCategoryID = requirementParameters.AppRequirementCategoryData.RequirementCategoryID,
                    ARCD_RequirementCategoryStatusID = LstReqCategoryStatus.FirstOrDefault(cnd => cnd.RCS_Code ==
                                                       requirementParameters.AppRequirementCategoryData.RequirementCategoryStatusCode).RCS_ID,
                };

                ApplicantRequirementItemData itemData = new ApplicantRequirementItemData
                {
                    ARID_ID = requirementParameters.AppRequirementItemData.RequirementItemDataID > 0 ?
                                                requirementParameters.AppRequirementItemData.RequirementItemDataID : 0,
                    ARID_RequirementItemID = requirementParameters.AppRequirementItemData.RequirementItemID,
                    ARID_RequirementItemStatusID = LstReqItemStatus.FirstOrDefault(cnd => cnd.RIS_Code ==
                                                       requirementParameters.AppRequirementItemData.RequirementItemStatusCode).RIS_ID,

                };

                List<ApplicantRequirementFieldData> applicantFieldData = new List<ApplicantRequirementFieldData>();

                foreach (var fieldData in requirementParameters.AppRequirementFieldDataList)
                {
                    ApplicantRequirementFieldData applicantRequirementFieldData = new ApplicantRequirementFieldData();
                    applicantRequirementFieldData.ARFD_ID = fieldData.ApplicantReqFieldDataID > 0 ? fieldData.ApplicantReqFieldDataID : 0;
                    applicantRequirementFieldData.ARFD_RequirementItemDataID = fieldData.RequirementItemDataID;
                    applicantRequirementFieldData.ARFD_FieldValue = fieldData.FieldValue;
                    applicantRequirementFieldData.ARFD_RequirementFieldID = fieldData.RequirementFieldID;

                    if (!fieldData.Signature.IsNullOrEmpty())
                    {
                        RequirementFieldDataLargeContent requirementFieldDataLargeContent = new RequirementFieldDataLargeContent();
                        if (fieldData.ApplicantReqFieldDataID > 0)
                        {
                            requirementFieldDataLargeContent.RFDLC_ModifiedBy = currentLoggedInUserId;
                            requirementFieldDataLargeContent.RFDLC_ModifiedOn = DateTime.Now;
                        }
                        else
                        {
                            requirementFieldDataLargeContent.RFDLC_CreatedBy = currentLoggedInUserId;
                            requirementFieldDataLargeContent.RFDLC_CreatedOn = DateTime.Now;
                        }
                        requirementFieldDataLargeContent.RFDLC_Signature = fieldData.Signature;
                        requirementFieldDataLargeContent.RFDLC_IsDeleted = false;
                        applicantRequirementFieldData.RequirementFieldDataLargeContents.Add(requirementFieldDataLargeContent);
                    }
                    //Start UAT-5062
                    //Start UAT-4900
                    String dataTypeCode = fieldData.FieldDataTypeCode;
                    if (dataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue().Trim()
                                && !requirementParameters.SignedApplicantDocuments.IsNullOrEmpty() && requirementParameters.SignedApplicantDocuments.Count() > AppConsts.NONE && requirementParameters.IsUploadDocUpdated)
                    {
                        applicantRequirementFieldData.ARFD_FieldValue = !applicantRequirementFieldData.ARFD_FieldValue.IsNullOrEmpty() ? (requirementParameters.SignedApplicantDocuments.Count() + Convert.ToInt32(applicantRequirementFieldData.ARFD_FieldValue)).ToString() : applicantRequirementFieldData.ARFD_FieldValue;
                    }
                    //End UAT-4900
                    //End UAT-5062
                    applicantFieldData.Add(applicantRequirementFieldData);
                }

                //Convert Signed document from Contract to entity
                if (requirementParameters.SignedApplicantDocuments.IsNotNull())
                {
                    requirementParameters.SignedApplicantDocuments.ForEach(signedDoc =>
                    {
                        ApplicantDocument signedDocument = new ApplicantDocument();
                        signedDocument.DocumentPath = signedDoc.Value.DocumentPath;
                        signedDocument.Size = signedDoc.Value.Size;
                        signedDocument.FileName = signedDoc.Value.FileName;
                        signedDocument.DataEntryDocumentStatusID = LstDataEntryDocStatus.FirstOrDefault(x => x.LDEDS_Code == signedDoc.Value.DataEntryDocumentStatusCode
                                                                 && x.LDEDS_IsDeleted == false).LDEDS_ID;
                        signedDocument.DocumentType = LstDocumentType.FirstOrDefault(cnd => cnd.DMT_Code == signedDoc.Value.DocumentType && !cnd.DMT_IsDeleted).DMT_ID;
                        signedDocument.PdfDocPath = signedDoc.Value.DocumentPath;
                        signedDocument.PdfFileName = signedDoc.Value.FileName;
                        signedDocument.OrganizationUserID = currentLoggedInUserId;
                        signedDocument.IsDeleted = false;
                        signedDocument.CreatedByID = orgUsrID;
                        signedDocument.CreatedOn = DateTime.Now;
                        signedAppDoc.Add(signedDoc.Key, signedDocument);

                    });

                }
                var lkpObjectType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpObjectType>(requirementParameters.TenantId).Where(x => x.OT_IsDeleted == false).ToList();
                return BALUtils.GetApplicantRequirementRepoInstance(requirementParameters.TenantId).SaveApplicantRequirementData(categoryData, itemData, applicantFieldData,
                                                                    currentLoggedInUserId, requirementParameters.AppFieldDocuments, requirementParameters.RequirementPackageId,
                                                                    requirementParameters.RequirementPkgSubscriptionId, lkpObjectType, signedAppDoc, orgUsrID, requirementParameters.IsNewPackage, requirementParameters.IsUploadDocUpdated); //UAT-5062 IsUploadDocUpdated
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static DataTable GetPackageSubscriptionCategoryStatus(Int32 tenantID, string requirementPackageSubscriptionID)
        {
            try
            {
                return BALUtils.GetApplicantRequirementRepoInstance(tenantID).GetPackageSubscriptionCategoryStatus(requirementPackageSubscriptionID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }


        public static List<ApplicantDocumentContract> SaveApplicantUploadDocument(List<ApplicantDocumentContract> appUploadedDocumentList, Int32 currentloggedInUserId, Int32 tenantId, Int32 orgUsrID)
        {
            try
            {
                List<lkpDocumentType> LstDocumentType = LookupManager.GetLookUpData<lkpDocumentType>(tenantId)
                                                                        .Where(cnd => !cnd.DMT_IsDeleted).ToList();
                List<lkpDataEntryDocumentStatu> LstDataEntryDocStatus = LookupManager.GetLookUpData<lkpDataEntryDocumentStatu>(tenantId)
                                                                 .Where(cnd => !cnd.LDEDS_IsDeleted).ToList();

                List<ApplicantDocument> applicantDocument = new List<ApplicantDocument>();
                List<ApplicantDocument> SavedApplicantDocumentList = new List<ApplicantDocument>();
                List<ApplicantDocumentContract> savedAppDocumnetResponse = new List<ApplicantDocumentContract>();

                applicantDocument = appUploadedDocumentList.Select(appDoc => new ApplicantDocument
                {
                    OrganizationUserID = currentloggedInUserId,
                    Size = appDoc.Size,
                    DocumentPath = appDoc.DocumentPath,
                    FileName = appDoc.FileName,
                    CreatedByID = orgUsrID,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    DocumentType = LstDocumentType.FirstOrDefault(cnd => cnd.DMT_Code == appDoc.DocumentType).DMT_ID,
                    DataEntryDocumentStatusID = LstDataEntryDocStatus.FirstOrDefault(x => x.LDEDS_Code == appDoc.DataEntryDocumentStatusCode).LDEDS_ID,
                    Code = Guid.NewGuid(),

                }).ToList();


                SavedApplicantDocumentList = BALUtils.GetApplicantRequirementRepoInstance(tenantId).SaveApplicantUploadDocument(applicantDocument);

                SavedApplicantDocumentList.ForEach(cnd =>
                {
                    ApplicantDocumentContract savedApplicantDocument = new ApplicantDocumentContract();
                    savedApplicantDocument.ApplicantDocumentId = cnd.ApplicantDocumentID;
                    savedApplicantDocument.DocumentPath = cnd.DocumentPath;
                    savedApplicantDocument.Size = cnd.Size;
                    savedApplicantDocument.FileName = cnd.FileName;
                    savedApplicantDocument.Description = cnd.Description;
                    savedAppDocumnetResponse.Add(savedApplicantDocument);
                });
                return savedAppDocumnetResponse;

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean DeleteAppRequirementItemFieldData(Int32 applicantReqItemDataId, Int32 currentLoggedInUserId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetApplicantRequirementRepoInstance(tenantId).DeleteAppRequirementItemFieldData(applicantReqItemDataId, currentLoggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean IsDocumentAlreadyUploaded(String documentName, Int32 documentSize, Int32 organizationUserId, Int32 tenantId)
        {
            try
            {
                List<lkpDocumentType> docType = LookupManager.GetLookUpData<Entity.ClientEntity.lkpDocumentType>(tenantId).Where(x => x.DMT_IsDeleted == false).ToList();
                return BALUtils.GetApplicantRequirementRepoInstance(tenantId).IsDocumentAlreadyUploaded(documentName, documentSize, organizationUserId, docType);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<RequirementObjectTreeContract> GetAttributeObjectTreeProperties(ApplicantRequirementParameterContract requirementParameters, Int32 currentLoggedInUserId)
        {
            try
            {
                List<RequirementObjectTreeContract> attObjectPropertiesList = BALUtils.GetApplicantRequirementRepoInstance(requirementParameters.TenantId).GetAttributeObjectTreeProperties(
                                                           requirementParameters.RequirementPackageId, requirementParameters.RequirementItemId,
                                                           requirementParameters.RequirementCategoryId, currentLoggedInUserId);
                return attObjectPropertiesList;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-2224: Admin access to upload/associate documents on rotation package items.

        /// <summary>
        /// Get Applicant Requirement Item Data By ID
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="applicantComplianceItemID"></param>
        /// <returns></returns>
        public static ApplicantRequirementItemData GetApplicantRequirementItemDataByID(Int32 tenantId, Int32 applicantComplianceItemID)
        {
            try
            {
                return BALUtils.GetApplicantRequirementRepoInstance(tenantId).GetApplicantRequirementItemDataByID(applicantComplianceItemID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Add/Update applicant Requirement document mapping data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="applicantUploadedDocuments"></param>
        /// <param name="applicantRequirementItemDataId"></param>
        /// <param name="requirementFieldId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static Boolean AddUpdateApplicantRequirementDocumentMappingData(Int32 tenantId, List<ApplicantDocumentContract> applicantUploadedDocuments, Int32 applicantRequirementItemDataId, Int32 requirementFieldId, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetApplicantRequirementRepoInstance(tenantId).AddUpdateApplicantRequirementDocumentMappingData(applicantUploadedDocuments, applicantRequirementItemDataId, requirementFieldId, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Add Incomplete Applicant Requirement Document Mapping Data
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="applicantUploadedDocuments"></param>
        /// <param name="categoryData"></param>
        /// <param name="itemData"></param>
        /// <param name="attributeData"></param>
        /// <param name="requirementPackageSubscriptionId"></param>
        /// <param name="applicantId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="itemDataId"></param>
        /// <returns></returns>
        public static Boolean AddIncompleteApplicantRequirementDocumentMappingData(Int32 tenantId, List<ApplicantDocumentContract> applicantUploadedDocuments, ApplicantRequirementCategoryData categoryData,
              ApplicantRequirementItemData itemData, ApplicantRequirementFieldData fieldData, Int32 requirementPackageSubscriptionId, Int32 applicantId, Int32 currentUserId, out Int32 itemDataId)
        {
            try
            {
                return BALUtils.GetApplicantRequirementRepoInstance(tenantId).AddIncompleteApplicantRequirementDocumentMappingData(applicantUploadedDocuments, categoryData, itemData, fieldData,
                        requirementPackageSubscriptionId, applicantId, currentUserId, out itemDataId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Assign/UnAssign Requirement Item documents
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="toAddDocumentMapList"></param>
        /// <param name="toDeleteApplicantRequirementDocumentMapIDs"></param>
        /// <param name="requirementItemDataId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static Boolean AssignUnAssignRequirementItemDocuments(Int32 tenantId, List<ApplicantDocumentContract> toAddDocumentMapList, List<Int32> toDeleteApplicantRequirementDocumentMapIDs, Int32 requirementItemDataId, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetApplicantRequirementRepoInstance(tenantId).AssignUnAssignRequirementItemDocuments(toAddDocumentMapList, toDeleteApplicantRequirementDocumentMapIDs, requirementItemDataId, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Assign/UnAssign Incomplete Requirement Item documents
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="toAddDocumentMapList"></param>
        /// <param name="toDeleteApplicantRequirementDocumentMapIDs"></param>
        /// <param name="categoryData"></param>
        /// <param name="itemData"></param>
        /// <param name="fieldData"></param>
        /// <param name="requirementPackageSubscriptionId"></param>
        /// <param name="applicantId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="itemDataId"></param>
        /// <returns></returns>
        public static Boolean AssignUnAssignIncompleteRequirementItemDocuments(Int32 tenantId, List<ApplicantDocumentContract> toAddDocumentMapList, List<Int32> toDeleteApplicantRequirementDocumentMapIDs, ApplicantRequirementCategoryData categoryData,
              ApplicantRequirementItemData itemData, ApplicantRequirementFieldData fieldData, Int32 requirementPackageSubscriptionId, Int32 applicantId, Int32 currentUserId, out Int32 itemDataId)
        {
            try
            {
                return BALUtils.GetApplicantRequirementRepoInstance(tenantId).AssignUnAssignIncompleteRequirementItemDocuments(toAddDocumentMapList,
                    toDeleteApplicantRequirementDocumentMapIDs, categoryData, itemData, fieldData,
                        requirementPackageSubscriptionId, applicantId, currentUserId, out itemDataId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Remove Applicant Requirement Document Mapping
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="applicantRequirementDocumentMapId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public static Boolean RemoveMapping(Int32 tenantId, Int32 applicantRequirementDocumentMapId, Int32 currentUserId)
        {
            try
            {
                return BALUtils.GetApplicantRequirementRepoInstance(tenantId).RemoveMapping(applicantRequirementDocumentMapId, currentUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #endregion
        #endregion

        #region Rule's Execution.
        public static void ExecuteRequirementObjectBuisnessRules(List<RequirementRuleObject> ruleObjectMapping, Int32 reqSubscriptionId, Int32 systemUserId, Int32 tenantId)
        {
            try
            {
                String ruleObjectXml = GenarateRuleObjectXml(ruleObjectMapping, tenantId);
                BALUtils.GetApplicantRequirementRepoInstance(tenantId).ExecuteRequirementObjectBuisnessRules(ruleObjectXml, reqSubscriptionId, systemUserId);

            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                // throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Method for generating object mapping Xml
        /// </summary>
        /// <param name="ruleObjectMappingList">object list of RuleObjectMapping </param>
        /// <returns></returns>
        public static String GenarateRuleObjectXml(List<RequirementRuleObject> ruleObjectMappingList, Int32 tenantID)
        {
            List<lkpObjectType> lstlkpObjectType = LookupManager.GetLookUpData<lkpObjectType>(tenantID).Where(cond => !cond.OT_IsDeleted).ToList();
            XmlDocument doc = new XmlDocument();
            XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("RuleObjects"));
            foreach (RequirementRuleObject ruleObjectMapping in ruleObjectMappingList)
            {
                var lkpObjectType = lstlkpObjectType.Where(sel => sel.OT_Code == ruleObjectMapping.RuleObjectTypeCode).FirstOrDefault();
                XmlNode exp = el.AppendChild(doc.CreateElement("RuleObject"));
                exp.AppendChild(doc.CreateElement("TypeId")).InnerText = lkpObjectType.IsNotNull() ? lkpObjectType.OT_ID.ToString() : String.Empty;
                exp.AppendChild(doc.CreateElement("Id")).InnerText = ruleObjectMapping.RuleObjectId;
                exp.AppendChild(doc.CreateElement("ParentId")).InnerText = ruleObjectMapping.RuleObjectParentId;
            }

            return doc.OuterXml.ToString();
        }
        #endregion

        #region View Video/Doc Applicant side

        public static ObjectAttributeContract GetObjectTreeProperties(Int32 rotId, Int32 tenantId)
        {
            try
            {
                List<lkpObjectAttribute> attrObjectTypes = LookupManager.GetLookUpData<lkpObjectAttribute>(tenantId).ToList();
                Int32 attrSignatureRequired = attrObjectTypes.FirstOrDefault(attr => attr.OA_Code == ObjectAttribute.SIGNATURE_REQUIRED.GetStringValue()).OA_ID;
                Int32 attrIsRequiredToView = attrObjectTypes.FirstOrDefault(attr => attr.OA_Code == ObjectAttribute.REQUIRED_TO_VIEW.GetStringValue()).OA_ID;
                List<RequirementObjectTreeProperty> requirmentObjectTreeProperty = BALUtils.GetApplicantRequirementRepoInstance(tenantId).GetObjectTreeProperties(rotId);
                ObjectAttributeContract objAttributeContract = ConvertObjectTreeProperyDataToContract(requirmentObjectTreeProperty, attrObjectTypes);
                return objAttributeContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static ObjectAttributeContract ConvertObjectTreeProperyDataToContract(List<RequirementObjectTreeProperty> requirmentObjectTreeProperty, List<lkpObjectAttribute> attrObjectTypes)
        {
            ObjectAttributeContract objAttributeContract = new ObjectAttributeContract();
            if (requirmentObjectTreeProperty.Count > 1)
            {
                Int32 attrSignatureRequired = attrObjectTypes.FirstOrDefault(attr => attr.OA_Code == ObjectAttribute.SIGNATURE_REQUIRED.GetStringValue()).OA_ID;
                Int32 attrIsRequiredToView = attrObjectTypes.FirstOrDefault(attr => attr.OA_Code == ObjectAttribute.REQUIRED_TO_VIEW.GetStringValue()).OA_ID;
                Int32 attrBoxOpenTime = attrObjectTypes.FirstOrDefault(attr => attr.OA_Code == ObjectAttribute.BOX_STAYS_OPEN_TIME.GetStringValue()).OA_ID;
                Int32 attrIsRequiredToOpen = attrObjectTypes.FirstOrDefault(attr => attr.OA_Code == ObjectAttribute.REQUIRED_TO_OPEN.GetStringValue()).OA_ID;

                var signObject = requirmentObjectTreeProperty.FirstOrDefault(cond => cond.ROTP_ObjectAttributeID == attrSignatureRequired);
                objAttributeContract.IsSignatureRequired = signObject.IsNotNull() ? signObject.ROTP_ObjectAttributeValue == "True" ? true : false : false;
                var requiredViewObject = requirmentObjectTreeProperty.FirstOrDefault(cond => cond.ROTP_ObjectAttributeID == attrIsRequiredToView);
                objAttributeContract.IsRequiredToView = requiredViewObject.IsNotNull() ? requiredViewObject.ROTP_ObjectAttributeValue == "True" ? true : false : false;

                var boxOpenTimeObject = requirmentObjectTreeProperty.FirstOrDefault(cond => cond.ROTP_ObjectAttributeID == attrBoxOpenTime);
                objAttributeContract.BoxOpenTime = boxOpenTimeObject.IsNotNull() ? boxOpenTimeObject.ROTP_ObjectAttributeValue : String.Empty;
                var requiredToOpenObject = requirmentObjectTreeProperty.FirstOrDefault(cond => cond.ROTP_ObjectAttributeID == attrIsRequiredToOpen);
                objAttributeContract.IsRequiredToOpen = requiredToOpenObject.IsNotNull() ? requiredToOpenObject.ROTP_ObjectAttributeValue == "True" ? true : false : false;
            }
            return objAttributeContract;
        }

        public static ViewDocumentContract GetViewDocumentData(Int32 applicantDocId, Int32 clientSysDocId, Int32 organizationUserId, Int32 reqFieldId, Int32 tenantId)
        {
            try
            {

                ViewDocumentContract viewDocContract = BALUtils.GetApplicantRequirementRepoInstance(tenantId).GetViewDocumentData(applicantDocId, clientSysDocId, organizationUserId, reqFieldId, DocumentAcroFieldType.SIGNATURE.GetStringValue());
                return viewDocContract;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region GET EXPLANATORY NOTES
        public static String GetExplanatoryNotes(Int32 objectId, String objectTypeCode, String contentTypeCode, Int32 tenantId)
        {
            try
            {
                Int32 contentTypeId = LookupManager.GetLookUpData<lkpLargeContentType>(tenantId).Where(obj => obj.LCT_Code == contentTypeCode
                                                                                                       && obj.LCT_IsDeleted == false).FirstOrDefault().LCT_ID;
                Int32 objectTypeId = LookupManager.GetLookUpData<lkpObjectType>(tenantId).Where(obj => obj.OT_Code == objectTypeCode
                                                                                                && obj.OT_IsDeleted == false).FirstOrDefault().OT_ID;
                return BALUtils.GetApplicantRequirementRepoInstance(tenantId).GetExplanatoryNotes(objectId, objectTypeId, contentTypeId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        //UAT-1523 Addition a notes box for each rotation for the student to input information
        public static Boolean UpdateRequirementPackageSubscriptionNotes(String notes, Int32 requirementPackageSubscriptionId, Int32 tenantId, Int32 currentUserId)
        {
            try
            {
                RequirementPackageSubscription requirementPackageSubscription = BALUtils.GetApplicantRequirementRepoInstance(tenantId).GetRequirementPackageSubscription(requirementPackageSubscriptionId);
                if (!requirementPackageSubscription.IsNullOrEmpty())
                {
                    requirementPackageSubscription.RPS_Notes = notes;
                    requirementPackageSubscription.RPS_ModifiedOn = DateTime.Now;
                    requirementPackageSubscription.RPS_ModifiedByID = currentUserId;
                    return BALUtils.GetApplicantRequirementRepoInstance(tenantId).SaveChanges();
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<ApplicantDocument> SaveApplicantUploadDocument(List<ApplicantDocument> lstApplicantDocument, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetApplicantRequirementRepoInstance(tenantId).SaveApplicantUploadDocument(lstApplicantDocument);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean CanDeleteRqmtFieldUploadDoc(Int32 applicantUploadedDocumentID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetApplicantRequirementRepoInstance(tenantID).CanDeleteRqmtFieldUploadDoc(applicantUploadedDocumentID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-2905

        public static DataTable GetMailDataForItemSubmitted(String RpsIds, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetApplicantRequirementRepoInstance(tenantID).GetMailDataForItemSubmitted(RpsIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion


        #region UAT-3160
        public static String GetRotationHierarchyIdsBasedOnSubscriptionID(Int32 selectedTenantId, Int32 requirementPackageSubscriptionID)
        {
            try
            {
                return BALUtils.GetRequirementPackageRepoInstance(selectedTenantId).GetRotationHierarchyIdsBasedOnSubscriptionID(requirementPackageSubscriptionID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(ex);
                throw (new SysXException(ex.Message, ex));
            }

        }
        #endregion


        public static DataTable GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(Int32 tenantID, String requirementPackageSubscriptionIDs)
        {
            try
            {
                return BALUtils.GetApplicantRequirementRepoInstance(tenantID).GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(requirementPackageSubscriptionIDs);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static DataTable GetApprovedSubscrptionByRequirementPackageIDs(Int32 tenantId, String requirementPackageId)
        {
            try
            {
                String RequirementPackageSubscriptionIds = BALUtils.GetApplicantRequirementRepoInstance(tenantId).GetRequirementPackageSubscriptionIdsByPackageID(requirementPackageId);
                return BALUtils.GetApplicantRequirementRepoInstance(tenantId).GetApprovedSubscrptionByRequirementPackageSubscriptionIDs(RequirementPackageSubscriptionIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-4015 : Get RPS Data for inst/Preceptor and Send Mail.

        public static List<RequirementPackageSubscriptionStatusContract> GetInstPrecepRPSData(Int32 tenantId, Int32 rpsId)
        {
            try
            {
                return BALUtils.GetApplicantRequirementRepoInstance(tenantId).GetInstPrecepRPSData(rpsId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean MapInstPrecDataAndSendMail(Int32 tenantId, List<RequirementPackageSubscriptionStatusContract> lstrpsData, Int32 loggedInUserId)
        {
            try
            {
                if (!lstrpsData.IsNullOrEmpty())
                {
                    //Bind dictMailData and mockData Here.

                    List<NotificationDelivery> lstNotificationDelivery = new List<NotificationDelivery>();
                    String subEventTypeCode = CommunicationSubEvents.NOTIFICATION_TO_INSTRUCTOR_FOR_REQUIREMENT_PACKAGE_SUBSCRIPTION_TO_COMPLIANT.GetStringValue();
                    List<Entity.lkpCommunicationSubEvent> _lstSubEventTypes = LookupManager.GetMessagingLookUpData<Entity.lkpCommunicationSubEvent>();
                    Int32 subEventId = _lstSubEventTypes.Where(nt => nt.Code == subEventTypeCode).FirstOrDefault().CommunicationSubEventID;

                    foreach (var rpsData in lstrpsData)
                    {
                        Dictionary<String, object> dictMailData = new Dictionary<string, object>();
                        dictMailData.Add(EmailFieldConstants.USER_FULL_NAME, rpsData.ApplicantName);
                        dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, rpsData.PackageName);
                        dictMailData.Add(EmailFieldConstants.AGENCY_NAME, rpsData.AgencyName);

                        Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
                        mockData.UserName = rpsData.UserName;
                        mockData.EmailID = rpsData.Email;
                        mockData.ReceiverOrganizationUserID = rpsData.OrganizationUserID;

                        Int32? systemCommunicationID = CommunicationManager.SendEmailToInstructorPrecPnComplaintStaus(CommunicationSubEvents.NOTIFICATION_TO_INSTRUCTOR_FOR_REQUIREMENT_PACKAGE_SUBSCRIPTION_TO_COMPLIANT, dictMailData, mockData, tenantId, -1, ignoreSubscriptionSeting: true);

                        if (!systemCommunicationID.IsNullOrEmpty() && systemCommunicationID > AppConsts.NONE)
                        {
                            NotificationDelivery notificationDelivery = new NotificationDelivery();
                            notificationDelivery.ND_OrganizationUserID = rpsData.OrganizationUserID;
                            notificationDelivery.ND_SubEventTypeID = subEventId;
                            notificationDelivery.ND_EntityId = rpsData.ClinicalRotationSubscriptionID;
                            notificationDelivery.ND_EntityName = "ClinicalRotationSubscription";
                            notificationDelivery.ND_IsDeleted = false;
                            notificationDelivery.ND_CreatedBy = loggedInUserId;
                            notificationDelivery.ND_CreatedOn = DateTime.Now;
                            lstNotificationDelivery.Add(notificationDelivery);
                        }
                    }
                    if (!lstNotificationDelivery.IsNullOrEmpty())
                        return BALUtils.GetComplianceDataRepoInstance(tenantId).AddNotificationDeliveryList(lstNotificationDelivery);
                }
                return false;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean SendMailToInstPrecpReqPKgCompliantStatus(Int32 tenantId, Int32 rpsId, Int32 loggedInUserId)
        {
            try
            {
                //Step 1 to get data needed.
                List<RequirementPackageSubscriptionStatusContract> requirementPackageSubscriptionStatusContract = GetInstPrecepRPSData(tenantId, rpsId);
                //Step 2 Mapp data in this Method and send Mail.
                return MapInstPrecDataAndSendMail(tenantId, requirementPackageSubscriptionStatusContract, loggedInUserId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        public static Int32 GetApplicantRequirementFieldData(Int32 RequirementItemDataID, Int32 RequirementFieldID, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetApplicantRequirementRepoInstance(tenantId).GetApplicantRequirementFieldData(RequirementItemDataID, RequirementFieldID);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #region UAT-4254


        public static List<RequirementCategoryDocUrl> GetRequirementCatDocUrls(Int32 requirementCategoryId, Int32 tenantId)
        {
            try
            {
                List<RequirementCategoryDocUrl> lstReqCatDocUrls = new List<RequirementCategoryDocUrl>();
                List<RequirementCategoryDocLink> lstCatDocLinks = BALUtils.GetApplicantRequirementRepoInstance(tenantId).GetRequirementCatDocUrls(requirementCategoryId);

                lstCatDocLinks.ForEach(x =>
                {
                    RequirementCategoryDocUrl reqCatDocUrl = new RequirementCategoryDocUrl();

                    reqCatDocUrl.RequirementCatDocUrlLabel = x.RCDL_SampleDocFormUrlLabel;
                    reqCatDocUrl.RequirementCatDocLinkID = x.RCDL_ID;
                    reqCatDocUrl.RequirementCatID = x.RCDL_RequirementCategoryID;
                    reqCatDocUrl.RequirementCatDocUrl = x.RCDL_SampleDocFormURL;

                    lstReqCatDocUrls.Add(reqCatDocUrl);
                });

                return lstReqCatDocUrls;
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion
    }
}
