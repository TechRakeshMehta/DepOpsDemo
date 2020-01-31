using System;
using System.Collections.Generic;
using System.Linq;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using Entity.ClientEntity;
using System.Data;
using System.Text;
using INTSOF.ServiceDataContracts.Modules.Common;
using System.Xml;
using System.Xml.Linq;

namespace Business.RepoManagers
{
    public class RequirementPackageManager
    {
        #region Constructor

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        static RequirementPackageManager()
        {
            BALUtils.ClassModule = "Requirement Package Manager";
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Method to get all possible data types of rotation fields
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static List<RotationFieldDataTypeContract> GetRotationFieldDataTypes(Int32 tenantID)
        {
            try
            {
                List<Entity.ClientEntity.lkpRequirementFieldDataType> dataTypeList = LookupManager.GetLookUpData<Entity.ClientEntity.lkpRequirementFieldDataType>(tenantID);
                return dataTypeList.Select(col => new RotationFieldDataTypeContract
                {
                    DataTypeCode = col.RFDT_Code,
                    DataTypeName = col.RFDT_Name,
                    DataTypeID = col.RFDT_ID
                }).ToList();
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

        public static Int32 CopyPackageToClient(RequirementPackageContract requirementPackageContract, Int32 selectedTenantID, Int32 currentLoggedInUserID)
        {
            RequirementPackage existingRequirementPackage = BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).GetRequirementPackageByPackageCode(requirementPackageContract.RequirementPackageCode);
            if (!existingRequirementPackage.IsNullOrEmpty())
            {
                return existingRequirementPackage.RP_ID;
            }
            else
            {
                return SaveRequirementPackage(requirementPackageContract, selectedTenantID, currentLoggedInUserID);
            }
        }




        public static Int32 CopyClientRqrmntPkgToShared(RequirementPackageContract requirementPackageContract, Int32 selectedTenantID, Int32 currentLoggedInUserID, Boolean isPackageMappedToRotation)
        {
            return SharedRequirementPackageManager.SaveRequirementPackage(requirementPackageContract, currentLoggedInUserID, isPackageMappedToRotation, true);
        }

        public static Int32 SaveRequirementPackage(RequirementPackageContract requirementPackageContract, Int32 selectedTenantID, Int32 currentLoggedInUserID)
        {
            try
            {
                if (requirementPackageContract.IsSharedUserPackage)
                {
                    return AddNewRequirementPackage(requirementPackageContract, selectedTenantID, currentLoggedInUserID, false);
                }
                else
                {
                    Boolean isNewPackage = requirementPackageContract.RequirementPackageID == AppConsts.NONE;
                    Boolean isPackageVersionNeedToCreate = IsPackageVersionNeedToCreate(requirementPackageContract, selectedTenantID);

                    if (isNewPackage || isPackageVersionNeedToCreate)
                    {
                        //UAT-1828
                        Int32 requirementPkgID = AddNewRequirementPackage(requirementPackageContract, selectedTenantID, currentLoggedInUserID, isPackageVersionNeedToCreate);
                        if (isPackageVersionNeedToCreate)
                        {
                            SendRotationPackageVersioningNotification(requirementPackageContract, selectedTenantID);
                        }
                        return requirementPkgID;
                    }
                    else
                    {
                        //call method to update existing package package by passing contract to it
                        return UpdateRequirementPackage(requirementPackageContract, selectedTenantID, currentLoggedInUserID);
                    }
                }
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



        public static Int32 DeleteRequirementPackage(RequirementPackageContract requirementPackageContract, Int32 selectedTenantID, Int32 currentLoggedInUserID)
        {
            try
            {
                if (requirementPackageContract.IsDeleted)
                {
                    //check if package can be deleted
                    if (IsPackageMappedToRotation(requirementPackageContract.RequirementPackageID, selectedTenantID))
                    {
                        return AppConsts.NONE;
                    }
                    else
                    {
                        return DeleteRequirementPackageEntities(requirementPackageContract, selectedTenantID, currentLoggedInUserID);
                    }

                }
                else
                {
                    Boolean isPackageVersionNeedToCreate = IsPackageVersionNeedToCreate(requirementPackageContract, selectedTenantID);

                    if (isPackageVersionNeedToCreate)
                    {

                        return AddNewRequirementPackage(requirementPackageContract, selectedTenantID, currentLoggedInUserID, isPackageVersionNeedToCreate);
                    }
                    else
                    {
                        //call method to update existing package package by passing contract to it
                        return DeleteRequirementPackageEntities(requirementPackageContract, selectedTenantID, currentLoggedInUserID);
                    }
                }
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

        private static Int32 DeleteRequirementPackageEntities(RequirementPackageContract requirementPackageContract, Int32 selectedTenantID, Int32 currentLoggedInUserID)
        {
            List<RequirementFieldContract> requirementFieldContractList = new List<RequirementFieldContract>();
            List<RequirementItemContract> requirementItemContractList = new List<RequirementItemContract>();

            Int32 objectTreeIDToBeDeleted = 0;

            //Get existing requirement package(from DB) which is to be deleted 
            RequirementPackage existingRequirementPackage = BALUtils.GetRequirementPackageRepoInstance(selectedTenantID)
                                                                    .GetRequirementPackageByPackageID(requirementPackageContract.RequirementPackageID);

            //if package does not exists
            if (existingRequirementPackage.IsNullOrEmpty())
            {
                return AppConsts.NONE;
            }
            else if (requirementPackageContract.IsDeleted)
            {
                existingRequirementPackage.RP_IsDeleted = true;
                existingRequirementPackage.RP_ModifiedByID = currentLoggedInUserID;
                existingRequirementPackage.RP_ModifiedOn = DateTime.Now;
                objectTreeIDToBeDeleted = requirementPackageContract.PackageObjectTreeID;
            }
            else
            {
                foreach (RequirementCategoryContract categoryContract in requirementPackageContract.LstRequirementCategory)
                {
                    RequirementPackageCategory existingRequrmntPkgCategory = existingRequirementPackage.RequirementPackageCategories
                                                                                         .Where(cond => cond.RPC_ID == categoryContract.RequirementPackageCategoryID && !cond.RPC_IsDeleted).FirstOrDefault();
                    //if category is deleted
                    if (categoryContract.IsDeleted)
                    {
                        existingRequrmntPkgCategory.RPC_IsDeleted = true;
                        existingRequrmntPkgCategory.RPC_ModifiedByID = currentLoggedInUserID;
                        existingRequrmntPkgCategory.RPC_ModifiedOn = DateTime.Now;
                        objectTreeIDToBeDeleted = categoryContract.CategoryObjectTreeID;
                        break;
                    }
                    else
                    {
                        foreach (RequirementItemContract itemContract in categoryContract.LstRequirementItem)
                        {
                            requirementItemContractList.Add(itemContract);
                            RequirementCategoryItem existingRequrmntCategoryItem = existingRequrmntPkgCategory.RequirementCategory.RequirementCategoryItems
                                                                                               .Where(cond => cond.RCI_ID == itemContract.RequirementCategoryItemID && !cond.RCI_IsDeleted).FirstOrDefault();

                            if (itemContract.IsDeleted)
                            {
                                existingRequrmntCategoryItem.RCI_IsDeleted = true;
                                existingRequrmntCategoryItem.RCI_ModifiedByID = currentLoggedInUserID;
                                existingRequrmntCategoryItem.RCI_ModifiedOn = DateTime.Now;
                                objectTreeIDToBeDeleted = itemContract.ItemObjectTreeID;
                                break;
                            }
                            else
                            {
                                foreach (RequirementFieldContract fieldContract in itemContract.LstRequirementField)
                                {
                                    if (fieldContract.IsDeleted)
                                    {
                                        RequirementItemField existingReqrmntItemField = existingRequrmntCategoryItem.RequirementItem.RequirementItemFields
                                                                                                         .Where(cond => cond.RIF_ID == fieldContract.RequirementItemFieldID && !cond.RIF_IsDeleted)
                                                                                                          .FirstOrDefault();
                                        existingReqrmntItemField.RIF_IsDeleted = true;
                                        existingReqrmntItemField.RIF_ModifiedByID = currentLoggedInUserID;
                                        existingReqrmntItemField.RIF_ModifiedOn = DateTime.Now;
                                        objectTreeIDToBeDeleted = fieldContract.FieldObjectTreeID;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (objectTreeIDToBeDeleted > 0)
            {
                DeleteRequirementObjectTree(objectTreeIDToBeDeleted, selectedTenantID, currentLoggedInUserID);
            }

            BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).SaveContextIntoDataBase();

            return existingRequirementPackage.RP_ID;
        }

        /// <summary>
        /// used to get requirement package details including package name,category name,item name and field name in hierarichal way
        /// </summary>
        /// <returns></returns>
        public static List<RequirementPackageDetailsContract> GetRequirementPackageDetailsByPackageID(Int32 requirementPackageID, Int32 tenantID)
        {
            try
            {
                return BALUtils.GetRequirementPackageRepoInstance(tenantID).GetRequirementPackageDetailsByPackageID(requirementPackageID);
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
        /// used to get all requirement package details including package name and comma separated agencyNames with which they are mapped. It also returns unMapped packages too
        /// </summary>
        /// <returns></returns>
        public static List<RequirementPackageDetailsContract> GetRequirementPackageDetails(RequirementPackageDetailsContract requirementPackageDetailsContract
                                                                                           , CustomPagingArgsContract customPagingArgsContract)
        {
            try
            {
                return BALUtils.GetRequirementPackageRepoInstance(requirementPackageDetailsContract.SelectedTenantID)
                           .GetRequirementPackageDetails(requirementPackageDetailsContract, customPagingArgsContract);

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
        /// To get Requirement Packages
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="agencyId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public static List<RequirementPackageContract> GetRequirementPackages(Int32 tenantId, String agencyId)
        {
            try
            {
                return BALUtils.GetRequirementPackageRepoInstance(tenantId).GetRequirementPackages(agencyId);
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
        /// get complete package details in hierarchal way
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        public static RequirementPackageContract GetRequirementPackageHierarchalDetailsByPackageID(Int32 requirementPackageID, Int32 tenantID)
        {
            try
            {
                List<RequirementPackageHierarchicalDetailsContract> requirmntPkgHrchicalContract = BALUtils.GetRequirementPackageRepoInstance(tenantID)
                                                                                                           .GetRequirementPackageHierarchalDetailsByPackageID(requirementPackageID);

                RequirementPackageContract requirementPackageDetailsContract = AssignPackageHierarchalDetailsToSessionContract(requirmntPkgHrchicalContract);
                return requirementPackageDetailsContract;

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
        /// method used to return lkpConstantType values
        /// </summary>
        /// <param name="tenantParameter"></param>
        /// <returns></returns>
        public static List<RulesConstantTypeContract> GetRulesConstantTypes(Int32 tenantID)
        {
            try
            {
                List<Entity.ClientEntity.lkpConstantType> constantTypeList = LookupManager.GetLookUpData<Entity.ClientEntity.lkpConstantType>(tenantID);
                return constantTypeList.Select(col => new RulesConstantTypeContract
                {
                    ConstantTypeID = col.ID,
                    ConstantTypeName = col.Name,
                    ConstantTypeCode = col.Code,
                    ConstantTypeGroup = col.Group
                }).ToList();
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
        /// To get Instructor/Preceptor Requirement Packages
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="agencyId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public static List<RequirementPackageContract> GetInstructorRequirementPackages(Int32 tenantId, String agencyId)
        {
            try
            {
                return BALUtils.GetRequirementPackageRepoInstance(tenantId).GetInstructorRequirementPackages(agencyId);
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

        #region Private Methods

        private static List<RequirementPackageHierarchicalDetailsContract> AssignValuesToPackageHierarchicalDetailsContract(DataTable packageHierarchalDetailsDataTable)
        {
            try
            {
                IEnumerable<DataRow> packageDetailsDataRows = packageHierarchalDetailsDataTable.AsEnumerable();
                return packageDetailsDataRows.Select(row => new RequirementPackageHierarchicalDetailsContract
                {
                    RequirementPackageID = row["RequirementPackageID"] == DBNull.Value ? 0 : Convert.ToInt32(row["RequirementPackageID"]),
                    RequirementPackageCode = row["RequirementPackageCode"] == DBNull.Value ? Guid.Empty : (Guid)(row["RequirementPackageCode"]),
                    RequirementPackageName = row["RequirementPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementPackageName"]),
                    RequirementPackageLabel = row["RequirementPackageLabel"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementPackageLabel"]),
                    RequirementPackageRuleTypeCode = row["RequirementPackageRuleTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementPackageRuleTypeCode"]),
                    //RequirementPackageLabel = row["RequirementPackageLabel"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementPackageLabel"]),
                    //RequirementPackageDescription = row["RequirementPackageDescription"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementPackageDescription"]),
                    RequirementPackageCategoryID = row["RequirementPackageCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(row["RequirementPackageCategoryID"]),
                    RequirementCategoryID = row["RequirementCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(row["RequirementCategoryID"]),
                    RequirementCategoryCode = row["RequirementCategoryCode"] == DBNull.Value ? Guid.Empty : (Guid)(row["RequirementCategoryCode"]),
                    RequirementCategoryName = row["RequirementCategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementCategoryName"]),
                    RequirementCategoryLabel = row["RequirementCategoryLabel"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementCategoryLabel"]),
                    RequirementCategoryDescription = row["RequirementCategoryDescription"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementCategoryDescription"]),
                    RequirementCategoryRuleTypeCode = row["RequirementCategoryRuleTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementCategoryRuleTypeCode"]),
                    RequirementCategoryItemID = row["RequirementCategoryItemID"] == DBNull.Value ? 0 : Convert.ToInt32(row["RequirementCategoryItemID"]),
                    RequirementItemID = row["RequirementItemID"] == DBNull.Value ? 0 : Convert.ToInt32(row["RequirementItemID"]),
                    RequirementItemCode = row["RequirementItemCode"] == DBNull.Value ? Guid.Empty : (Guid)(row["RequirementItemCode"]),
                    RequirementItemName = row["RequirementItemName"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementItemName"]),
                    RequirementItemLabel = row["RequirementItemLabel"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementItemLabel"]),
                    RequirementItemDescription = row["RequirementItemDescription"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementItemDescription"]),
                    RequirementItemRuleTypeCode = row["RequirementItemRuleTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementItemRuleTypeCode"]),
                    DateTypeRequirementFieldCodeForExpiration = row["DateTypeRequirementFieldCodeForExpiration"] == DBNull.Value ? Guid.Empty : (Guid)(row["DateTypeRequirementFieldCodeForExpiration"]),
                    DateTypeRequirementFieldIDForExpiration = row["DateTypeRequirementFieldIDForExpiration"] == DBNull.Value ? 0 : Convert.ToInt32(row["DateTypeRequirementFieldIDForExpiration"]),
                    ExpirationValueTypeCode = row["ExpirationValueTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(row["ExpirationValueTypeCode"]),
                    ExpirationValueTypeID = row["ExpirationValueTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ExpirationValueTypeID"]),
                    ExpirationValue = row["ExpirationValue"] == DBNull.Value ? String.Empty : Convert.ToString(row["ExpirationValue"]),
                    RequirementItemFieldID = row["RequirementItemFieldID"] == DBNull.Value ? 0 : Convert.ToInt32(row["RequirementItemFieldID"]),
                    RequirementFieldID = row["RequirementFieldID"] == DBNull.Value ? 0 : Convert.ToInt32(row["RequirementFieldID"]),
                    RequirementFieldCode = row["RequirementFieldCode"] == DBNull.Value ? Guid.Empty : (Guid)(row["RequirementFieldCode"]),
                    RequirementFieldName = row["RequirementFieldName"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementFieldName"]),
                    RequirementFieldLabel = row["RequirementFieldLabel"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementFieldLabel"]),
                    RequirementFieldDescription = row["RequirementFieldDescription"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementFieldDescription"]),
                    RequirementFieldRuleTypeCode = row["RequirementFieldRuleTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementFieldRuleTypeCode"]),
                    RequirementFieldRuleValue = row["RequirementFieldRuleValue"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementFieldRuleValue"]),
                    RequirementFieldDataTypeID = row["RequirementFieldDataTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(row["RequirementFieldDataTypeID"]),
                    RequirementFieldDataTypeCode = row["RequirementFieldDataTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementFieldDataTypeCode"]),
                    RequirementFieldDataTypeName = row["RequirementFieldDataTypeName"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementFieldDataTypeName"]),
                    RequirementFieldVideoID = row["RequirementFieldVideoID"] == DBNull.Value ? 0 : Convert.ToInt32(row["RequirementFieldVideoID"]),
                    RequirementFieldVideoName = row["RequirementFieldVideoName"] == DBNull.Value ? String.Empty : Convert.ToString(row["RequirementFieldVideoName"]),
                    FieldVideoURL = row["FieldVideoURL"] == DBNull.Value ? String.Empty : Convert.ToString(row["FieldVideoURL"]),
                    //IsVideoOpenRequired = row["IsVideoOpenRequired"] == DBNull.Value ? false : Convert.ToBoolean(row["IsVideoOpenRequired"]),
                    //VideoOpenTimeDuration = row["VideoOpenTimeDuration"] == DBNull.Value ? 0 : Convert.ToInt32(row["VideoOpenTimeDuration"]),
                    RequirementFieldOptionsID = row["RequirementFieldOptionsID"] == DBNull.Value ? 0 : Convert.ToInt32(row["RequirementFieldOptionsID"]),
                    OptionText = row["OptionText"] == DBNull.Value ? String.Empty : Convert.ToString(row["OptionText"]),
                    OptionValue = row["OptionValue"] == DBNull.Value ? String.Empty : Convert.ToString(row["OptionValue"]),
                    RequirementFieldDocumentID = row["RequirementFieldDocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(row["RequirementFieldDocumentID"]),
                    ClientSystemDocumentID = row["ClientSystemDocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ClientSystemDocumentID"]),
                    ClientSystemDocumentFileName = row["ClientSystemDocumentFileName"] == DBNull.Value ? String.Empty : Convert.ToString(row["ClientSystemDocumentFileName"]),
                    ClientSystemDocumentSize = row["ClientSystemDocumentSize"] == DBNull.Value ? 0 : Convert.ToInt32(row["ClientSystemDocumentSize"]),
                    ClientSystemDocumentPath = row["ClientSystemDocumentPath"] == DBNull.Value ? String.Empty : Convert.ToString(row["ClientSystemDocumentPath"]),
                    DocumentTypeID = row["DocumentTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(row["DocumentTypeID"]),
                    DocumentTypeCode = row["DocumentTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(row["DocumentTypeCode"]),
                    RequirementDocumentAcroFieldID = row["RequirementDocumentAcroFieldID"] == DBNull.Value ? 0 : Convert.ToInt32(row["RequirementDocumentAcroFieldID"]),
                    DocumentAcroFieldTypeID = row["DocumentAcroFieldTypeID"] == DBNull.Value ? String.Empty : Convert.ToString(row["DocumentAcroFieldTypeID"]),
                    DocumentAcroFieldTypeCode = row["DocumentAcroFieldTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(row["DocumentAcroFieldTypeCode"]),
                    DocumentAcroFieldTypeName = row["DocumentAcroFieldTypeName"] == DBNull.Value ? String.Empty : Convert.ToString(row["DocumentAcroFieldTypeName"]),
                    RequirementPackageAgencyID = row["RequirementPackageAgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(row["RequirementPackageAgencyID"]),
                    AgencyID = row["AgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(row["AgencyID"]),
                }).ToList();
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

        private static RequirementPackageContract AssignPackageHierarchalDetailsToSessionContract(List<RequirementPackageHierarchicalDetailsContract> LstRequirementPackageHierarchicalDetailsContract)
        {
            RequirementPackageContract requirementPackageContract = new RequirementPackageContract();

            if (!LstRequirementPackageHierarchicalDetailsContract.IsNullOrEmpty())
            {

                //Add data into top level contract - RequirementPackageContract
                RequirementPackageHierarchicalDetailsContract packageHierarchicalDetailsContract = LstRequirementPackageHierarchicalDetailsContract.FirstOrDefault();
                requirementPackageContract.RequirementPackageID = packageHierarchicalDetailsContract.RequirementPackageID;
                requirementPackageContract.RequirementPackageName = packageHierarchicalDetailsContract.RequirementPackageName;
                requirementPackageContract.RequirementPackageLabel = packageHierarchicalDetailsContract.RequirementPackageLabel;
                requirementPackageContract.DefinedRequirementID = packageHierarchicalDetailsContract.DefinedRequirementID;

                //requirementPackageContract.RequirementPackageDescription = packageHierarchicalDetailsContract.RequirementPackageDescription;
                requirementPackageContract.LstAgencyIDs = LstRequirementPackageHierarchicalDetailsContract.Where(cond => cond.RequirementPackageAgencyID > 0)
                                                        .Select(col => col.AgencyID).Distinct().ToList();
                requirementPackageContract.LstAgencyNames = LstRequirementPackageHierarchicalDetailsContract.Where(cond => cond.RequirementPackageAgencyID > 0)
                                                        .DistinctBy(col => col.AgencyID).Select(col => col.AgencyName).ToList();
                requirementPackageContract.RequirementPackageCode = packageHierarchicalDetailsContract.RequirementPackageCode;
                requirementPackageContract.PackageRuleTypeCode = packageHierarchicalDetailsContract.RequirementPackageRuleTypeCode;
                requirementPackageContract.PackageObjectTreeID = packageHierarchicalDetailsContract.PackageObjectTreeID;
                requirementPackageContract.IsSharedUserLoggedIn = false;
                requirementPackageContract.IsSharedUserPackage = false;

                //UAT 1352:As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use
                requirementPackageContract.RequirementPkgTypeID = packageHierarchicalDetailsContract.RequirementPkgTypeID;
                requirementPackageContract.RequirementPkgTypeCode = packageHierarchicalDetailsContract.RequirementPkgTypeCode;
                //requirementPackageContract.IsSharedUserPackage = packageHierarchicalDetailsContract

                //Add data into Category contract - RequirementCategoryContract

                requirementPackageContract.LstRequirementCategory = new List<RequirementCategoryContract>();

                List<RequirementPackageHierarchicalDetailsContract> lstPackagesDistinctByCategory = LstRequirementPackageHierarchicalDetailsContract
                                                                                                    .Where(cond => cond.RequirementCategoryID != AppConsts.NONE)
                                                                                                    .DistinctBy(col => col.RequirementPackageCategoryID).ToList();

                foreach (RequirementPackageHierarchicalDetailsContract packageDetailDistinctByCategory in lstPackagesDistinctByCategory)
                {
                    RequirementCategoryContract categoryData = new RequirementCategoryContract();
                    categoryData.RequirementCategoryID = packageDetailDistinctByCategory.RequirementCategoryID;
                    categoryData.RequirementPackageCategoryID = packageDetailDistinctByCategory.RequirementPackageCategoryID;
                    categoryData.RequirementCategoryName = packageDetailDistinctByCategory.RequirementCategoryName;

                    //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes)
                    categoryData.RequirementDocumentLink = packageDetailDistinctByCategory.RequirementDocumentLink;
                    //UAT-3161
                    categoryData.RequirementDocumentLinkLabel = packageDetailDistinctByCategory.RequirementDocumentLinkLabel;
                    //categoryData.RequirementCategoryLabel = packageDetailDistinctByCategory.RequirementCategoryLabel;
                    //categoryData.RequirementCategoryDescription = packageDetailDistinctByCategory.RequirementCategoryDescription;
                    categoryData.RequirementCategoryCode = packageDetailDistinctByCategory.RequirementCategoryCode;
                    categoryData.CategoryRuleTypeCode = packageDetailDistinctByCategory.RequirementCategoryRuleTypeCode;
                    categoryData.CategoryObjectTreeID = packageDetailDistinctByCategory.CategoryObjectTreeID;
                    categoryData.ExplanatoryNotes = packageDetailDistinctByCategory.CategoryExplanatoryNotes;

                    //UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
                    //Filling the values of Complinacerequired into RequirementPackageContract.
                    categoryData.IsComplianceRequired = packageDetailDistinctByCategory.IsComplianceRequired;
                    categoryData.ComplianceReqStartDate = packageDetailDistinctByCategory.ComplianceReqStartDate.HasValue ? packageDetailDistinctByCategory.ComplianceReqStartDate : null;
                    categoryData.ComplianceReqEndDate = packageDetailDistinctByCategory.ComplianceReqEndDate.HasValue ? packageDetailDistinctByCategory.ComplianceReqEndDate : null;

                    //UAT-3805
                    categoryData.SendItemDoconApproval = packageDetailDistinctByCategory.SendItemDocOnApproval;

                    //Add data into Item contract - RequirementItemContract
                    List<RequirementPackageHierarchicalDetailsContract> LstPackagesDistinctByItem = LstRequirementPackageHierarchicalDetailsContract
                                                                .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                       && cond.RequirementItemID != AppConsts.NONE)
                                                                .DistinctBy(col => col.RequirementItemID).ToList();
                    categoryData.LstRequirementItem = new List<RequirementItemContract>();

                    foreach (RequirementPackageHierarchicalDetailsContract packageDetailsDistinctByItem in LstPackagesDistinctByItem)
                    {
                        RequirementItemContract itemData = new RequirementItemContract();
                        itemData.RequirementCategoryItemID = packageDetailsDistinctByItem.RequirementCategoryItemID;
                        itemData.RequirementItemID = packageDetailsDistinctByItem.RequirementItemID;
                        itemData.RequirementItemName = packageDetailsDistinctByItem.RequirementItemName;
                        //itemData.RequirementItemLabel = packageDetailsDistinctByItem.RequirementItemLabel;
                        itemData.RequirementItemCode = packageDetailsDistinctByItem.RequirementItemCode;
                        itemData.ItemObjectTreeID = packageDetailsDistinctByItem.ItemObjectTreeID;
                        itemData.ExplanatoryNotes = packageDetailsDistinctByItem.RequirementItemDescription;
                        itemData.RequirementItemExpiration = new RequirementItemExpirationContract();

                        List<RequirementPackageHierarchicalDetailsContract> lstCurrentItemRows = LstRequirementPackageHierarchicalDetailsContract
                                                                    .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                             && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                                             && cond.RequirementItemID != AppConsts.NONE).ToList();

                        if (lstCurrentItemRows.Any(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue()))
                        {
                            itemData.IsRequirementItemNeededExpiration = true;
                            itemData.RequirementItemExpiration.RequirementItemExpirationTypeCode = RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue();
                            RequirementPackageHierarchicalDetailsContract currentItem = lstCurrentItemRows.
                                                    Where(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue())
                                                    .FirstOrDefault();
                            itemData.RequirementItemExpiration.ExpirationValueTypeCode = currentItem.ExpirationValueTypeCode;
                            itemData.RequirementItemExpiration.ExpirationValueTypeID = currentItem.ExpirationValueTypeID;
                            itemData.RequirementItemExpiration.ExpirationDate = currentItem.ExpirationValue;
                        }
                        else if (lstCurrentItemRows.Any(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue()))
                        {
                            //UAT-2165
                            itemData.IsRequirementItemNeededExpiration = true;
                            itemData.RequirementItemExpiration.RequirementItemExpirationTypeCode = RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue();
                            RequirementPackageHierarchicalDetailsContract currentItem = lstCurrentItemRows.
                                                    Where(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue())
                                                    .FirstOrDefault();
                            itemData.RequirementItemExpiration.ExpirationValueTypeCode = currentItem.ExpirationValueTypeCode;
                            itemData.RequirementItemExpiration.ExpirationValueTypeID = currentItem.ExpirationValueTypeID;
                            itemData.RequirementItemExpiration.ExpirationDate = currentItem.ExpirationValue;
                            itemData.RequirementItemExpiration.ExpirationCondStartDate = currentItem.ExpirationCondStartDate;
                            itemData.RequirementItemExpiration.ExpirationCondEndDate = currentItem.ExpirationCondEndDate;
                            itemData.RequirementItemExpiration.SelectedDateTypeFieldID = currentItem.DateTypeRequirementFieldIDForExpiration;
                            itemData.RequirementItemExpiration.SelectedDateTypeFieldCode = currentItem.DateTypeRequirementFieldCodeForExpiration;
                        }
                        else if (lstCurrentItemRows.Any(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()))
                        {
                            itemData.IsRequirementItemNeededExpiration = true;
                            itemData.RequirementItemExpiration.RequirementItemExpirationTypeCode = RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue();
                            RequirementPackageHierarchicalDetailsContract currentItemWithDateAttribute = lstCurrentItemRows.
                                                                            Where(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()
                                                                            && cond.DateTypeRequirementFieldIDForExpiration != 0 && cond.ExpirationValue == String.Empty)
                                                                            .FirstOrDefault();
                            RequirementPackageHierarchicalDetailsContract currentItemWithDateAttributeValue = lstCurrentItemRows.
                                                                            Where(cond => cond.RequirementItemRuleTypeCode == RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()
                                                                            && cond.DateTypeRequirementFieldIDForExpiration == 0)
                                                                            .FirstOrDefault();
                            itemData.RequirementItemExpiration.ExpirationValueTypeCode = currentItemWithDateAttributeValue.ExpirationValueTypeCode;
                            itemData.RequirementItemExpiration.ExpirationValueTypeID = currentItemWithDateAttributeValue.ExpirationValueTypeID;
                            itemData.RequirementItemExpiration.SelectedDateTypeFieldCode = currentItemWithDateAttribute.DateTypeRequirementFieldCodeForExpiration;
                            itemData.RequirementItemExpiration.SelectedDateTypeFieldID = currentItemWithDateAttribute.DateTypeRequirementFieldIDForExpiration;
                            itemData.RequirementItemExpiration.ExpirationValue = Convert.ToInt32(currentItemWithDateAttributeValue.ExpirationValue);
                        }
                        else
                        {
                            itemData.IsRequirementItemNeededExpiration = false;
                            itemData.RequirementItemExpiration = null;
                        }

                        itemData.RequirementItemRuleTypeCode = lstCurrentItemRows.
                                                            Where(cond => cond.RequirementItemRuleTypeCode != RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue()
                                                                      && cond.RequirementItemRuleTypeCode != RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue()
                                                                      && cond.RequirementItemRuleTypeCode != RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue()
                                                                                ).FirstOrDefault().RequirementItemRuleTypeCode;

                        List<RequirementPackageHierarchicalDetailsContract> LstPackagesDistinctByField = LstRequirementPackageHierarchicalDetailsContract
                                                                    .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                             && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                                              && cond.RequirementFieldID != AppConsts.NONE)
                                                                    .DistinctBy(col => col.RequirementFieldID).ToList();
                        itemData.LstRequirementField = new List<RequirementFieldContract>();
                        //Add data into field contract - RequirementFieldContract
                        foreach (RequirementPackageHierarchicalDetailsContract packagedetailsDistinctByField in LstPackagesDistinctByField)
                        {
                            RequirementFieldContract fieldContract = new RequirementFieldContract();
                            fieldContract.RequirementFieldID = packagedetailsDistinctByField.RequirementFieldID;
                            fieldContract.RequirementItemFieldID = packagedetailsDistinctByField.RequirementItemFieldID;
                            fieldContract.RequirementFieldName = packagedetailsDistinctByField.RequirementFieldName;
                            //fieldContract.RequirementFieldLabel = packagedetailsDistinctByField.RequirementFieldLabel;
                            //fieldContract.RequirementFieldDescription = packagedetailsDistinctByField.RequirementFieldDescription;
                            fieldContract.RequirementFieldCode = packagedetailsDistinctByField.RequirementFieldCode;
                            fieldContract.FieldObjectTreeID = packagedetailsDistinctByField.FieldObjectTreeID;

                            //UAT-2164
                            fieldContract.IsBackgroundDocument = packagedetailsDistinctByField.IsBackgroundDocument;

                            List<RequirementPackageHierarchicalDetailsContract> currentFieldRows = LstRequirementPackageHierarchicalDetailsContract
                                                .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                      && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                      && cond.RequirementFieldID == packagedetailsDistinctByField.RequirementFieldID && cond.RequirementFieldID != AppConsts.NONE)
                                                         .ToList();

                            fieldContract.IsFieldRequired = Convert.ToBoolean(currentFieldRows.Where(cond => cond.RequirementFieldRuleTypeCode == ObjectAttribute.REQUIRED.GetStringValue())
                                                                            .FirstOrDefault().RequirementFieldRuleValue);

                            //Add data into Field data contract - RequirementFieldDataContract

                            RequirementFieldDataContract fieldDataContract = new RequirementFieldDataContract();
                            fieldDataContract.RequirementFieldDataTypeCode = packagedetailsDistinctByField.RequirementFieldDataTypeCode;
                            fieldDataContract.RequirementFieldDataTypeID = packagedetailsDistinctByField.RequirementFieldDataTypeID;
                            fieldDataContract.RequirementFieldDataTypeName = packagedetailsDistinctByField.RequirementFieldDataTypeName;

                            //Save data into video contract - RequirementFieldVideoData
                            if (packagedetailsDistinctByField.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
                            {
                                RequirementFieldVideoData videoData = new RequirementFieldVideoData();
                                videoData.RequirementFieldVideoID = packagedetailsDistinctByField.RequirementFieldVideoID;
                                RequirementPackageHierarchicalDetailsContract videoFieldRules_OpenRequired = currentFieldRows
                                    .Where(cond => cond.RequirementFieldRuleTypeCode == ObjectAttribute.REQUIRED_TO_OPEN.GetStringValue()).FirstOrDefault();
                                if (!videoFieldRules_OpenRequired.IsNullOrEmpty())
                                {
                                    videoData.IsVideoRequiredToBeOpened = Convert.ToBoolean(videoFieldRules_OpenRequired.RequirementFieldRuleValue);
                                }
                                if (videoData.IsVideoRequiredToBeOpened)
                                {
                                    videoData.VideoOpenTimeDuration = Convert.ToInt32(currentFieldRows
                                        .Where(cond => cond.RequirementFieldRuleTypeCode == ObjectAttribute.BOX_STAYS_OPEN_TIME.GetStringValue())
                                        .FirstOrDefault().RequirementFieldRuleValue);
                                    videoData.VideoOpenedMinutes = videoData.VideoOpenTimeDuration / 60;
                                    videoData.VideoOpenedSeconds = videoData.VideoOpenTimeDuration % 60;
                                }

                                videoData.VideoURL = packagedetailsDistinctByField.FieldVideoURL;
                                videoData.VideoName = packagedetailsDistinctByField.RequirementFieldVideoName;
                                fieldDataContract.VideoFieldData = videoData;
                            }

                            //Save data into options contract - RequirementFieldOptionsData
                            if (packagedetailsDistinctByField.RequirementFieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue())
                            {
                                fieldDataContract.LstRequirementFieldOptions = new List<RequirementFieldOptionsData>();
                                List<RequirementPackageHierarchicalDetailsContract> LstPackagesDistinctByField_OptionData = LstRequirementPackageHierarchicalDetailsContract
                                                                   .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                            && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                                            && cond.RequirementFieldID == packagedetailsDistinctByField.RequirementFieldID
                                                                            ).DistinctBy(col => col.RequirementFieldOptionsID).ToList();
                                StringBuilder formatOptions = new StringBuilder();

                                foreach (RequirementPackageHierarchicalDetailsContract packageFieldOptionData in LstPackagesDistinctByField_OptionData)
                                {
                                    RequirementFieldOptionsData optionData = new RequirementFieldOptionsData();
                                    optionData.RequirementFieldOptionsID = packageFieldOptionData.RequirementFieldOptionsID;
                                    optionData.OptionText = packageFieldOptionData.OptionText;
                                    optionData.OptionValue = packageFieldOptionData.OptionValue;
                                    fieldDataContract.LstRequirementFieldOptions.Add(optionData);

                                    //code to generate FormatOptions to be displayed during edit mode
                                    formatOptions.AppendFormat("{0}={1}|", packageFieldOptionData.OptionText, packageFieldOptionData.OptionValue);
                                }

                                //code to generate FormatOptions to be displayed during edit mode
                                Int32 index = formatOptions.ToString().LastIndexOf('|');
                                if (index >= 0)
                                    formatOptions.Remove(index, 1);

                                fieldDataContract.RequiredFieldOptionsFormattedValue = Convert.ToString(formatOptions);
                            }

                            if (packagedetailsDistinctByField.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
                            {
                                RequirementFieldViewDocumentData documentData = new RequirementFieldViewDocumentData();
                                documentData.ClientSystemDocumentID = packagedetailsDistinctByField.ClientSystemDocumentID;
                                documentData.DocumentFileName = packagedetailsDistinctByField.ClientSystemDocumentFileName;
                                documentData.DocumentPath = packagedetailsDistinctByField.ClientSystemDocumentPath;
                                documentData.DocumentSize = packagedetailsDistinctByField.ClientSystemDocumentSize;

                                List<RequirementPackageHierarchicalDetailsContract> LstPackagesDistinctByField_DocFieldData = LstRequirementPackageHierarchicalDetailsContract
                                                                   .Where(cond => cond.RequirementCategoryID == packageDetailDistinctByCategory.RequirementCategoryID
                                                                            && cond.RequirementItemID == packageDetailsDistinctByItem.RequirementItemID
                                                                            && cond.RequirementFieldID == packagedetailsDistinctByField.RequirementFieldID
                                                                            ).DistinctBy(col => col.DocumentAcroFieldTypeID).ToList();
                                documentData.LstDocumentAcroFieldTypeCodes = new List<String>();
                                documentData.LstRequirementDocumentAcroFieldIDs = new List<Int32>();

                                foreach (RequirementPackageHierarchicalDetailsContract docFieldData in LstPackagesDistinctByField_DocFieldData)
                                {
                                    if (!docFieldData.DocumentAcroFieldTypeCode.IsNullOrEmpty())
                                    {
                                        documentData.LstDocumentAcroFieldTypeCodes.Add(docFieldData.DocumentAcroFieldTypeCode);
                                        documentData.LstRequirementDocumentAcroFieldIDs.Add(docFieldData.RequirementDocumentAcroFieldID);
                                    }
                                }
                                fieldDataContract.FieldViewDocumentData = documentData;
                            }

                            fieldContract.RequirementFieldData = fieldDataContract;
                            itemData.LstRequirementField.Add(fieldContract);
                        }
                        categoryData.LstRequirementItem.Add(itemData);
                    }

                    requirementPackageContract.LstRequirementCategory.Add(categoryData);
                }
            }

            return requirementPackageContract;
        }

        #region RequirementObjectTree related methods

        private static void InsertDataIntoRequirementObjectTree(RequirementPackageContract requirementPackageContract, Int32 selectedTenantID, Int32 currentLoggedInUserID, RequirementPackage requirementPackage, List<RequirementFieldContract> requirementFieldContractList, List<RequirementItemContract> requirementItemContractList, Boolean isPackageVersionNeedToCreate)
        {
            //Add entries into requirement object tree.
            List<RequirementObjectTreeContract> requirmntObjTreeCntrct = AddRequirementObjectTree(requirementPackage.RP_ID, currentLoggedInUserID, selectedTenantID);
            if (!requirmntObjTreeCntrct.IsNullOrEmpty())
            {
                List<Int32> reqObjectTreeIds = requirmntObjTreeCntrct.Select(sel => sel.RequirementObjectTreeID).ToList();
                List<RequirementObjectTree> lstRequirmntObjTreeFromDb = BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).GetRequirementObjectTreeList(reqObjectTreeIds);

                #region Get required Lkp values
                List<lkpObjectType> lstlkpObjectType = LookupManager.GetLookUpData<lkpObjectType>(selectedTenantID).ToList();
                List<lkpObjectAttribute> lstlkpObjectAttribue = LookupManager.GetLookUpData<lkpObjectAttribute>(selectedTenantID).
                                                                                            Where(cond => !cond.OA_IsDeleted).ToList();
                List<lkpFixedRuleType> lstFixedRuleType = LookupManager.GetLookUpData<lkpFixedRuleType>(selectedTenantID).
                                                                                            Where(cond => !cond.FRLT_IsDeleted).ToList();
                List<lkpRuleActionType> lstRuleActionType = LookupManager.GetLookUpData<lkpRuleActionType>(selectedTenantID).
                                                                                              Where(cond => !cond.ACT_IsDeleted).ToList();

                List<lkpRuleObjectMappingType> lstRuleObjectMappingType = LookupManager.GetLookUpData<lkpRuleObjectMappingType>(selectedTenantID).
                                                                                             Where(cond => !cond.RMT_IsDeleted).ToList();


                String requiredObjectAttributeCode = ObjectAttribute.REQUIRED.GetStringValue();
                String signatureRequiredObjectAttributeCode = ObjectAttribute.SIGNATURE_REQUIRED.GetStringValue();
                String requiredToViewObjectAttributeCode = ObjectAttribute.REQUIRED_TO_VIEW.GetStringValue();
                String requiredToOpeAttributeCode = ObjectAttribute.REQUIRED_TO_OPEN.GetStringValue();
                String boxStayOpenObjectAttributeCode = ObjectAttribute.BOX_STAYS_OPEN_TIME.GetStringValue();
                lkpObjectAttribute requiredObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == requiredObjectAttributeCode);
                lkpObjectAttribute signatureRequiredObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == signatureRequiredObjectAttributeCode);
                lkpObjectAttribute requiredToViewObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == requiredToViewObjectAttributeCode);
                lkpObjectAttribute requiredToOpenObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == requiredToOpeAttributeCode);
                lkpObjectAttribute boxStayOpenObjectAttribute = lstlkpObjectAttribue.FirstOrDefault(cond => cond.OA_Code == boxStayOpenObjectAttributeCode);

                String attrObjTypeCode = ObjectType.Compliance_ATR.GetStringValue();
                String itmObjTypeCode = ObjectType.Compliance_Item.GetStringValue();
                String catObjTypeCode = ObjectType.Compliance_Category.GetStringValue();
                String pkgObjTypeCode = ObjectType.Compliance_Package.GetStringValue();
                lkpObjectType attrObjectType = lstlkpObjectType.FirstOrDefault(cond => cond.OT_Code == attrObjTypeCode);
                lkpObjectType itmObjectType = lstlkpObjectType.FirstOrDefault(cond => cond.OT_Code == itmObjTypeCode);
                lkpObjectType catObjectType = lstlkpObjectType.FirstOrDefault(cond => cond.OT_Code == catObjTypeCode);
                lkpObjectType pkgObjectType = lstlkpObjectType.FirstOrDefault(cond => cond.OT_Code == pkgObjTypeCode);

                lkpRuleType buisnessRuleType = LookupManager.GetLookUpData<lkpRuleType>(selectedTenantID).FirstOrDefault(cond => cond.RLT_Code ==
                                                                                                        ComplianceRuleType.BuisnessRules.GetStringValue());

                lkpRuleType uiRuleType = LookupManager.GetLookUpData<lkpRuleType>(selectedTenantID).FirstOrDefault(cond => cond.RLT_Code ==
                                                                                                       ComplianceRuleType.UIRules.GetStringValue());

                lkpRuleActionType defaultActionType = lstRuleActionType.FirstOrDefault(cond => cond.ACT_Code == "DFLT");
                lkpRuleActionType itemExpiratnActionType = lstRuleActionType.FirstOrDefault(cond => cond.ACT_Code == "SEXP");

                #endregion

                RequirementObjectTreeContract pkgRequirmntObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == pkgObjectType.OT_ID
                                                                                                            && cond.ObjectID == requirementPackage.RP_ID
                                                                                                            && cond.ParentObjectID == null);
                if (!pkgRequirmntObjTree.IsNullOrEmpty())
                {
                    RequirementObjectTree pkgRequirmntObjTreeFromDb = lstRequirmntObjTreeFromDb
                                                                        .FirstOrDefault(cond => cond.ROT_ID == pkgRequirmntObjTree.RequirementObjectTreeID);
                    CreateePackageObjectRule(requirementPackageContract, currentLoggedInUserID, requirementPackage, pkgRequirmntObjTree
                                            , pkgRequirmntObjTreeFromDb, lstFixedRuleType, pkgObjectType, buisnessRuleType, defaultActionType);
                }

                IEnumerable<RequirementPackageCategory> requirementPackageCategories = requirementPackage.RequirementPackageCategories;

                requirementPackageCategories.ForEach(rpc =>
                {
                    RequirementCategory requirementCatgry = rpc.RequirementCategory;
                    if (requirementCatgry.IsNotNull())
                    {
                        RequirementCategoryContract requirmntCatContractObj = requirementPackageContract.LstRequirementCategory
                                                                                                                .Where(cond => cond.RequirementCategoryCode == requirementCatgry.RC_Code)
                                                                                                                .FirstOrDefault();
                        if (!requirmntCatContractObj.IsNullOrEmpty() && (requirmntCatContractObj.IsNewCategory || requirmntCatContractObj.IsUpdated || isPackageVersionNeedToCreate || requirementPackageContract.IsSharedUserPackage))
                        {
                            RequirementObjectTreeContract catRequirmntObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == catObjectType.OT_ID
                                                                                                  && cond.ObjectID == requirementCatgry.RC_ID
                                                                                                  && cond.ParentObjectID == requirementPackage.RP_ID);
                            if (!catRequirmntObjTree.IsNullOrEmpty())
                            {
                                RequirementObjectTree catRequirmntObjTreeFromDb = lstRequirmntObjTreeFromDb
                                                                                    .FirstOrDefault(cond => cond.ROT_ID == catRequirmntObjTree.RequirementObjectTreeID);

                                //Updating SQL expression for category Custom Rule

                                foreach (var reqItem in requirmntCatContractObj.LstRequirementItem)
                                {
                                    int reqItemId = reqItem.RequirementItemID;
                                    string reqItemCode = reqItem.RequirementItemCode.ToString();

                                    foreach (var reqcatItem in requirementCatgry.RequirementCategoryItems)
                                    {
                                        RequirementItem reqItemFromDB = reqcatItem.RequirementItem;

                                        if (reqItemFromDB.RI_Code.ToString() == reqItemCode)
                                        {
                                            int reqItemIdFromDB = reqItemFromDB.RI_ID;

                                            if (!requirmntCatContractObj.RuleSqlExpression.IsNullOrEmpty())
                                                requirmntCatContractObj.RuleSqlExpression = requirmntCatContractObj.RuleSqlExpression.Replace(reqItemId.ToString(), reqItemIdFromDB.ToString());

                                            if (!requirmntCatContractObj.RuleUIExpression.IsNullOrEmpty())
                                                requirmntCatContractObj.RuleUIExpression = requirmntCatContractObj.RuleUIExpression.Replace(reqItemId.ToString(), reqItemIdFromDB.ToString());
                                        }
                                    }
                                }

                                CreateCategoryRequirementObjectRule(currentLoggedInUserID, catRequirmntObjTree, requirmntCatContractObj, catRequirmntObjTreeFromDb
                                                                    , lstFixedRuleType, buisnessRuleType, defaultActionType);
                            }
                        }

                        IEnumerable<RequirementCategoryItem> reqrmntCatgryItems = requirementCatgry.RequirementCategoryItems;
                        reqrmntCatgryItems.ForEach(rci =>
                        {
                            RequirementItem requirmntItem = rci.RequirementItem;
                            IEnumerable<RequirementItemField> requirmntItmFields = requirmntItem.RequirementItemFields;
                            if (requirmntItem.IsNotNull())
                            {
                                RequirementItemContract requirementItemContractObj = requirementItemContractList.Where(cond => cond.RequirementItemCode
                                                                                                             == requirmntItem.RI_Code)
                                                                                                            .FirstOrDefault();
                                if (!requirementItemContractObj.IsNullOrEmpty() && (requirementItemContractObj.IsNewItem || requirementItemContractObj.IsUpdated || isPackageVersionNeedToCreate || requirementPackageContract.IsSharedUserPackage))
                                {
                                    RequirementObjectTreeContract itmRequirmntObjTree = requirmntObjTreeCntrct
                                                                                       .FirstOrDefault(cond => cond.ObjectTypeID == itmObjectType.OT_ID
                                                                                           && cond.ObjectID == requirmntItem.RI_ID
                                                                                           && cond.ParentObjectID == rci.RCI_RequirementCategoryID);
                                    if (!itmRequirmntObjTree.IsNullOrEmpty())
                                    {
                                        RequirementObjectTree itmRequirmntObjTreeFromDb = lstRequirmntObjTreeFromDb
                                                                                          .FirstOrDefault(cond => cond.ROT_ID == itmRequirmntObjTree.RequirementObjectTreeID);

                                        CreateItemApprovalObjectRule(currentLoggedInUserID, itmRequirmntObjTree, lstFixedRuleType, buisnessRuleType
                                                                    , defaultActionType, requirementItemContractObj, itmRequirmntObjTreeFromDb);
                                        //expiration based rule.
                                        CreateItemExpirationObjectRule(currentLoggedInUserID, requirmntObjTreeCntrct, lstRuleObjectMappingType, lstFixedRuleType
                                                                       , attrObjectType, buisnessRuleType, itemExpiratnActionType
                                                                       , requirmntItem, requirmntItmFields, requirementItemContractObj, itmRequirmntObjTree
                                                                       , itmRequirmntObjTreeFromDb, selectedTenantID);
                                    }
                                }

                                requirmntItmFields.ForEach(rfi =>
                                {
                                    RequirementField requiremntField = rfi.RequirementField;
                                    if (requiremntField.IsNotNull())
                                    {
                                        RequirementFieldContract requirementFieldContractObj = requirementFieldContractList
                                                                                              .Where(cond => cond.RequirementFieldCode
                                                                                                             == requiremntField.RF_Code)
                                                                                              .FirstOrDefault();
                                        if (!requirementFieldContractObj.IsNullOrEmpty() && (requirementFieldContractObj.IsNewField || requirementFieldContractObj.IsUpdated || isPackageVersionNeedToCreate || requirementPackageContract.IsSharedUserPackage))
                                        {
                                            RequirementObjectTreeContract attrRequirmntObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == attrObjectType.OT_ID
                                                                                                                        && cond.ObjectID == requiremntField.RF_ID
                                                                                                                        && cond.ParentObjectID == rfi.RIF_RequirementItemID);
                                            if (!attrRequirmntObjTree.IsNullOrEmpty())
                                            {
                                                RequirementObjectTree attrRequirmntObjTreeFromDb = lstRequirmntObjTreeFromDb.FirstOrDefault(cond => cond.ROT_ID == attrRequirmntObjTree.RequirementObjectTreeID);
                                                if (requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.DATE.GetStringValue()
                                                   || requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue()
                                                   || requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue()
                                                   || requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue()
                                                    || requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
                                                {
                                                    InsertUpdateRequiredObjectProperty(currentLoggedInUserID, requiredObjectAttribute, requirementFieldContractObj, attrRequirmntObjTree, attrRequirmntObjTreeFromDb);
                                                }

                                                //if (requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
                                                //{
                                                //    if (attrRequirmntObjTree.IsNewRecordInserted)
                                                //    {
                                                //        InsertUpdateSignatureRequiredObjectProperty(currentLoggedInUserID, signatureRequiredObjectAttribute, requirementFieldContractObj, attrRequirmntObjTree, attrRequirmntObjTreeFromDb);

                                                //        InsertUpdateRequiredToViewObjectProperty(currentLoggedInUserID, requiredToViewObjectAttribute, requirementFieldContractObj, attrRequirmntObjTree, attrRequirmntObjTreeFromDb);
                                                //    }
                                                //}

                                                if (requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
                                                {
                                                    RequirementFieldVideoData VideoFieldData = requirementFieldContractObj.RequirementFieldData.VideoFieldData;
                                                    if (!VideoFieldData.IsNullOrEmpty())
                                                    {
                                                        InsertUpdateRequiredToOpenObjectProperty(currentLoggedInUserID, requiredToOpenObjectAttribute, requirementFieldContractObj, attrRequirmntObjTree, attrRequirmntObjTreeFromDb, VideoFieldData);
                                                        if (!VideoFieldData.VideoOpenTimeDuration.IsNullOrEmpty())// && VideoFieldData.VideoOpenTimeDuration == AppConsts.NONE)
                                                        {
                                                            InsertUpdateBoxStayOpenObjectProperty(currentLoggedInUserID, boxStayOpenObjectAttribute, attrRequirmntObjTree, attrRequirmntObjTreeFromDb, VideoFieldData);
                                                        }
                                                    }
                                                }

                                                if (requirementFieldContractObj.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.DATE.GetStringValue()
                                                    && (requirementFieldContractObj.RequirementFieldUIRuleTypeCode == RequirementFixedRuleType.GREATER_THAN.GetStringValue()
                                                    || requirementFieldContractObj.RequirementFieldUIRuleTypeCode == RequirementFixedRuleType.LESS_THAN.GetStringValue()))
                                                {
                                                    var itemUsedInRule = requirmntCatContractObj.LstRequirementItem.FirstOrDefault(sel => sel.RequirementItemID == requirementFieldContractObj.UiRequirementItemID);

                                                    if (!itemUsedInRule.IsNullOrEmpty())
                                                    {
                                                        var field = itemUsedInRule.LstRequirementField.FirstOrDefault(sel => sel.RequirementFieldID == requirementFieldContractObj.UiRequirementFieldID);

                                                        Int32 NewUiDataFieldId = AppConsts.NONE;
                                                        foreach (var reqcatItem in requirementCatgry.RequirementCategoryItems)
                                                        {
                                                            RequirementItem reqItemFromDB = reqcatItem.RequirementItem;

                                                            if (reqItemFromDB.RI_Code.ToString() == itemUsedInRule.RequirementItemCode.ToString())
                                                            {
                                                                foreach (var reqField in reqItemFromDB.RequirementItemFields)
                                                                {
                                                                    RequirementField reqFieldFromDB = reqField.RequirementField;
                                                                    if (reqFieldFromDB.RF_Code.ToString() == field.RequirementFieldCode.ToString())
                                                                    {
                                                                        NewUiDataFieldId = reqFieldFromDB.RF_ID;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        RequirementObjectTreeContract uiRuleDateObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == attrObjectType.OT_ID
                                                                                                                                          && cond.ObjectID == NewUiDataFieldId);
                                                        String ruleName = requirementFieldContractObj.RequirementFieldName + "_FieldUIRule";
                                                        //UAT-2366: Add the ability for "Item 1 must be dated before/after item 2" ui rules in rotation packages.
                                                        InsertUpdateFieldUiRule(currentLoggedInUserID, lstRuleObjectMappingType, lstFixedRuleType
                                                                           , uiRuleType, defaultActionType, ruleName, attrRequirmntObjTreeFromDb
                                                                            , requirementFieldContractObj, attrRequirmntObjTree, uiRuleDateObjTree);
                                                    }
                                                }

                                            }
                                        }
                                    }
                                });
                            }
                        });
                    }
                });
                //if (!requirementObjectTreePropertyList.IsNullOrEmpty())
                //    BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).AddRequirementObjectTreePropertyToContext(requirementObjectTreePropertyList);
                //BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).AddRequirementObjectRuleToContext(requirementObjectRuleList);
                BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).SaveContextIntoDataBase();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <param name="currentUserID"></param>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        private static List<RequirementObjectTreeContract> AddRequirementObjectTree(Int32 requirementPackageID, Int32 currentUserID, Int32 tenantID)
        {
            try
            {

                String rpkgObjectTypeCode = ObjectType.Compliance_Package.GetStringValue();
                Int32 pkgObjectTypeId = LookupManager.GetLookUpData<lkpObjectType>(tenantID)
                                                            .Where(cond => cond.OT_Code == rpkgObjectTypeCode).FirstOrDefault().OT_ID;

                return BALUtils.GetRequirementPackageRepoInstance(tenantID).AddRequirementObjectTree(requirementPackageID, pkgObjectTypeId, currentUserID);
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


        private static void CreateePackageObjectRule(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID, RequirementPackage requirementPackage,
                                                                                   RequirementObjectTreeContract pkgRequirmntObjTree, RequirementObjectTree pkgRequirmntObjTreeFromDb, List<lkpFixedRuleType> lstFixedRuleType,
                                                                                   lkpObjectType pkgObjectType, lkpRuleType buisnessRuleType, lkpRuleActionType defaultActionType)
        {
            //Package compliance rule.
            if (pkgRequirmntObjTree.IsNewRecordInserted)
            {
                //insert new rule.
                RequirementObjectRule newObjectRule = new RequirementObjectRule();
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_Code = Guid.NewGuid();
                newObjectRule.ROR_Name = requirementPackage.RP_PackageName + "_PackageComplianceRule";
                newObjectRule.ROR_ObjectTreeId = pkgRequirmntObjTree.RequirementObjectTreeID;
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementPackageContract.PackageRuleTypeCode).FRLT_ID;
                newObjectRule.ROR_RuleType = buisnessRuleType.RLT_ID;
                newObjectRule.ROR_ActionType = defaultActionType.ACT_ID;
                newObjectRule.ROR_FirstVersionID = null;
                newObjectRule.ROR_IsCurrent = true;
                newObjectRule.ROR_IsActive = true;
                newObjectRule.ROR_IsDeleted = false;
                newObjectRule.ROR_CreatedByID = currentLoggedInUserID;
                newObjectRule.ROR_CreatedOn = DateTime.Now;
                pkgRequirmntObjTreeFromDb.RequirementObjectRules.Add(newObjectRule);
            }
            else
            {
                if (pkgRequirmntObjTreeFromDb.RequirementObjectRules.IsNotNull())
                {
                    //update existing rule
                    RequirementObjectRule existingObjectRule = pkgRequirmntObjTreeFromDb.RequirementObjectRules.FirstOrDefault(cond => !cond.ROR_IsDeleted);
                    if (existingObjectRule.IsNotNull())
                    {
                        existingObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementPackageContract.PackageRuleTypeCode).FRLT_ID;
                        existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                        existingObjectRule.ROR_ModifiedOn = DateTime.Now;
                    }
                }
            }
        }

        private static void CreateCategoryRequirementObjectRule(Int32 currentLoggedInUserID, RequirementObjectTreeContract catRequirmntObjTree
                                                                , RequirementCategoryContract requirementcategoryContractObj
                                                                , RequirementObjectTree catRequirmntObjTreeFromDb
                                                                , List<lkpFixedRuleType> lstFixedRuleType, lkpRuleType buisnessRuleType
                                                                , lkpRuleActionType defaultActionType)
        {
            //Category Approval rule.
            if (catRequirmntObjTree.IsNewRecordInserted)
            {
                RequirementObjectRule newObjectRule = new RequirementObjectRule();
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_Code = Guid.NewGuid();
                newObjectRule.ROR_Name = requirementcategoryContractObj.RequirementCategoryName + "_CategoryApprovalRule";
                newObjectRule.ROR_ObjectTreeId = catRequirmntObjTree.RequirementObjectTreeID;
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementcategoryContractObj.CategoryRuleTypeCode).FRLT_ID;
                newObjectRule.ROR_RuleType = buisnessRuleType.RLT_ID;
                newObjectRule.ROR_ActionType = defaultActionType.ACT_ID;
                newObjectRule.ROR_FirstVersionID = null;
                newObjectRule.ROR_IsCurrent = true;
                newObjectRule.ROR_IsActive = true;
                newObjectRule.ROR_IsDeleted = false;
                newObjectRule.ROR_CreatedByID = currentLoggedInUserID;
                newObjectRule.ROR_CreatedOn = DateTime.Now;
                newObjectRule.ROR_SqlExpression = requirementcategoryContractObj.RuleSqlExpression;
                newObjectRule.ROR_UIExpression = requirementcategoryContractObj.RuleUIExpression;
                catRequirmntObjTreeFromDb.RequirementObjectRules.Add(newObjectRule);
            }
            else
            {
                //update existing rule
                if (catRequirmntObjTreeFromDb.RequirementObjectRules.IsNotNull())
                {
                    RequirementObjectRule existingObjectRule = catRequirmntObjTreeFromDb.RequirementObjectRules.FirstOrDefault(cond => !cond.ROR_IsDeleted);
                    if (existingObjectRule.IsNotNull())
                    {
                        existingObjectRule.ROR_FixedRuleType = lstFixedRuleType
                                                               .FirstOrDefault(cond => cond.FRLT_Code == requirementcategoryContractObj.CategoryRuleTypeCode).FRLT_ID;
                        existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                        existingObjectRule.ROR_ModifiedOn = DateTime.Now;
                        existingObjectRule.ROR_Name = requirementcategoryContractObj.RequirementCategoryName + "_CategoryApprovalRule";
                        existingObjectRule.ROR_SqlExpression = requirementcategoryContractObj.RuleSqlExpression;
                        existingObjectRule.ROR_UIExpression = requirementcategoryContractObj.RuleUIExpression;
                    }
                }
            }
        }

        private static void CreateItemApprovalObjectRule(Int32 currentLoggedInUserID, RequirementObjectTreeContract itmRequirmntObjTree
                                                        , List<lkpFixedRuleType> lstFixedRuleType
                                                        , lkpRuleType buisnessRuleType
                                                        , lkpRuleActionType defaultActionType
                                                        , RequirementItemContract requirementItemContractObj, RequirementObjectTree itmRequirmntObjTreeFromDb)
        {
            //Approval required rule.
            if (itmRequirmntObjTree.IsNewRecordInserted)
            {
                RequirementObjectRule newObjectRule = new RequirementObjectRule();
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_Code = Guid.NewGuid();
                newObjectRule.ROR_Name = requirementItemContractObj.RequirementItemName + "_ItemApprovalRule";
                newObjectRule.ROR_ObjectTreeId = itmRequirmntObjTree.RequirementObjectTreeID;
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementItemContractObj.RequirementItemRuleTypeCode).FRLT_ID;
                newObjectRule.ROR_RuleType = buisnessRuleType.RLT_ID;
                newObjectRule.ROR_ActionType = defaultActionType.ACT_ID;
                newObjectRule.ROR_FirstVersionID = null;
                newObjectRule.ROR_IsCurrent = true;
                newObjectRule.ROR_IsActive = true;
                newObjectRule.ROR_IsDeleted = false;
                newObjectRule.ROR_CreatedByID = currentLoggedInUserID;
                newObjectRule.ROR_CreatedOn = DateTime.Now;
                itmRequirmntObjTreeFromDb.RequirementObjectRules.Add(newObjectRule);
            }
            else
            {
                //update existing rule
                if (itmRequirmntObjTreeFromDb.RequirementObjectRules.IsNotNull())
                {
                    List<String> lstItemRuleTypeCodes = new List<String>();
                    lstItemRuleTypeCodes.Add(RequirementFixedRuleType.ENTERED.GetStringValue());
                    lstItemRuleTypeCodes.Add(RequirementFixedRuleType.ENTERED_AND_APPROVED.GetStringValue());
                    RequirementObjectRule existingObjectRule = itmRequirmntObjTreeFromDb.RequirementObjectRules
                                                                                        .FirstOrDefault(cond => !cond.ROR_IsDeleted
                                                                                            && lstItemRuleTypeCodes.Contains(cond.lkpFixedRuleType.FRLT_Code));
                    //&& cond.RequirementObjectRuleDetails == null);
                    if (existingObjectRule.IsNotNull())
                    {
                        existingObjectRule.ROR_FixedRuleType = lstFixedRuleType
                                                               .FirstOrDefault(cond => cond.FRLT_Code == requirementItemContractObj.RequirementItemRuleTypeCode).FRLT_ID;
                        existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                        existingObjectRule.ROR_ModifiedOn = DateTime.Now;
                        existingObjectRule.ROR_Name = requirementItemContractObj.RequirementItemName + "_ItemApprovalRule";
                    }
                }
            }
        }

        private static void CreateItemExpirationObjectRule(Int32 currentLoggedInUserID, List<RequirementObjectTreeContract> requirmntObjTreeCntrct, List<lkpRuleObjectMappingType> lstRuleObjectMappingType,
                                                                                        List<lkpFixedRuleType> lstFixedRuleType, lkpObjectType attrObjectType, lkpRuleType buisnessRuleType, lkpRuleActionType itemExpiratnActionType,
                                                                                        RequirementItem requirmntItem, IEnumerable<RequirementItemField> requirmntItmFields, RequirementItemContract requirementItemContractObj,
                                                                                        RequirementObjectTreeContract itmRequirmntObjTree, RequirementObjectTree itmRequirmntObjTreeFromDb, Int32 selectedTenantID)
        {
            lkpRuleObjectMappingType constMappingType = lstRuleObjectMappingType.FirstOrDefault(cond => cond.RMT_Code == "CONST");
            lkpRuleObjectMappingType dataValueMappingType = lstRuleObjectMappingType.FirstOrDefault(cond => cond.RMT_Code == "DVAL");

            RequirementObjectRule existingObjectRule = itmRequirmntObjTreeFromDb.RequirementObjectRules.FirstOrDefault(cond => !cond.ROR_IsDeleted
                                                                                                       && cond.ROR_ActionType == itemExpiratnActionType.ACT_ID);
            String dateConstantTypeCode = ConstantType.Date.GetStringValue();
            lkpConstantType dateConstantType = LookupManager.GetLookUpData<lkpConstantType>(selectedTenantID).
                                                                                            Where(cond => !cond.IsDeleted).FirstOrDefault();
            if (requirementItemContractObj.IsRequirementItemNeededExpiration && !requirementItemContractObj.RequirementItemExpiration.IsNullOrEmpty())
            {
                if (itmRequirmntObjTree.IsNewRecordInserted || existingObjectRule.IsNullOrEmpty())
                {
                    InsertItemExpirationObjectRule(currentLoggedInUserID, requirmntObjTreeCntrct, constMappingType, lstFixedRuleType, attrObjectType, buisnessRuleType,
                        itemExpiratnActionType, requirmntItem, requirmntItmFields, requirementItemContractObj, itmRequirmntObjTree, itmRequirmntObjTreeFromDb, dateConstantType, dataValueMappingType);
                }
                else
                {
                    String exitingFixedRuleTypeCode = existingObjectRule.lkpFixedRuleType.FRLT_Code;

                    existingObjectRule.ROR_Name = requirementItemContractObj.RequirementItemName + "_ItemExpirationRule";
                    existingObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode).FRLT_ID;
                    existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                    existingObjectRule.ROR_ModifiedOn = DateTime.Now;

                    List<RequirementObjectRuleDetail> requirementObjectRuleDetailList = existingObjectRule.RequirementObjectRuleDetails.Where(cond => !cond.RORD_IsDeleted).ToList();
                    if (requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode
                        == RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue())
                    {
                        RequirementObjectRuleDetail requirementObjectRuleDetail = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.ONE);
                        requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = constMappingType.RMT_ID;
                        requirementObjectRuleDetail.RORD_ConstantType = dateConstantType.ID;
                        requirementObjectRuleDetail.RORD_ConstantValue = requirementItemContractObj.RequirementItemExpiration.ExpirationDate;
                        requirementObjectRuleDetail.RORD_ObjectTreeID = null;
                        requirementObjectRuleDetail.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetail.RORD_ModifiedOn = DateTime.Now;
                        if (exitingFixedRuleTypeCode != requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode)
                        {
                            RequirementObjectRuleDetail requirementObjectRuleDetailToBeDeleted = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.TWO);
                            if (!requirementObjectRuleDetailToBeDeleted.IsNullOrEmpty())
                            {
                                requirementObjectRuleDetailToBeDeleted.RORD_IsDeleted = true;
                                requirementObjectRuleDetailToBeDeleted.RORD_ModifiedByID = currentLoggedInUserID;
                                requirementObjectRuleDetailToBeDeleted.RORD_ModifiedOn = DateTime.Now;
                            }
                        }
                    }
                    else if (requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode
                        == RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue())
                    {
                        //UAT-2165
                        RequirementObjectRuleDetail requirementObjectRuleDetail = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.ONE);
                        requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                        requirementObjectRuleDetail.RORD_ConstantType = dateConstantType.ID;
                        requirementObjectRuleDetail.RORD_ConstantValue = requirementItemContractObj.RequirementItemExpiration.ExpirationDate;
                        requirementObjectRuleDetail.RORD_ExpirationCondStartDate = requirementItemContractObj.RequirementItemExpiration.ExpirationCondStartDate;
                        requirementObjectRuleDetail.RORD_ExpirationCondEndDate = requirementItemContractObj.RequirementItemExpiration.ExpirationCondEndDate;
                        RequirementField exprationDateField = requirmntItmFields.Where(cond => cond.RequirementField.RF_Code == requirementItemContractObj.RequirementItemExpiration.SelectedDateTypeFieldCode)
                                                                                                                                                                    .FirstOrDefault().RequirementField;
                        if (exprationDateField.IsNotNull())
                        {
                            RequirementObjectTreeContract exprationDateObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == attrObjectType.OT_ID
                                                                                                                          && cond.ObjectID == exprationDateField.RF_ID
                                                                                                                          && cond.ParentObjectID == requirmntItem.RI_ID);
                            requirementObjectRuleDetail.RORD_ObjectTreeID = exprationDateObjTree.RequirementObjectTreeID;
                        }
                        requirementObjectRuleDetail.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetail.RORD_ModifiedOn = DateTime.Now;
                        if (exitingFixedRuleTypeCode != requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode)
                        {
                            RequirementObjectRuleDetail requirementObjectRuleDetailToBeDeleted = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.TWO);
                            if (!requirementObjectRuleDetailToBeDeleted.IsNullOrEmpty())
                            {
                                requirementObjectRuleDetailToBeDeleted.RORD_IsDeleted = true;
                                requirementObjectRuleDetailToBeDeleted.RORD_ModifiedByID = currentLoggedInUserID;
                                requirementObjectRuleDetailToBeDeleted.RORD_ModifiedOn = DateTime.Now;
                            }
                        }
                    }
                    else
                    {
                        RequirementObjectRuleDetail requirementObjectRuleDetailForDataValue = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.ONE);
                        requirementObjectRuleDetailForDataValue.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                        requirementObjectRuleDetailForDataValue.RORD_ConstantType = null;
                        requirementObjectRuleDetailForDataValue.RORD_ConstantValue = null;
                        requirementObjectRuleDetailForDataValue.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetailForDataValue.RORD_ModifiedOn = DateTime.Now;
                        RequirementField exprationDateField = requirmntItmFields.Where(cond => cond.RequirementField.RF_Code == requirementItemContractObj.RequirementItemExpiration.SelectedDateTypeFieldCode)
                                                                                                                                                                    .FirstOrDefault().RequirementField;
                        if (exprationDateField.IsNotNull())
                        {
                            RequirementObjectTreeContract exprationDateObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == attrObjectType.OT_ID
                                                                                                                          && cond.ObjectID == exprationDateField.RF_ID
                                                                                                                          && cond.ParentObjectID == requirmntItem.RI_ID);
                            requirementObjectRuleDetailForDataValue.RORD_ObjectTreeID = exprationDateObjTree.RequirementObjectTreeID;
                        }
                        RequirementObjectRuleDetail requirementObjectRuleDetailForContantValue;
                        if (exitingFixedRuleTypeCode != requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode)
                        {
                            requirementObjectRuleDetailForContantValue = new RequirementObjectRuleDetail();
                            requirementObjectRuleDetailForContantValue.RORD_ParameterOrder = AppConsts.TWO;
                            requirementObjectRuleDetailForContantValue.RORD_RuleObjectMappingTypeID = constMappingType.RMT_ID;
                            requirementObjectRuleDetailForContantValue.RORD_ConstantType = requirementItemContractObj.RequirementItemExpiration.ExpirationValueTypeID;
                            requirementObjectRuleDetailForContantValue.RORD_ConstantValue = requirementItemContractObj.RequirementItemExpiration.ExpirationValue.ToString();
                            requirementObjectRuleDetailForContantValue.RORD_IsDeleted = false;
                            requirementObjectRuleDetailForContantValue.RORD_CreatedByID = currentLoggedInUserID;
                            requirementObjectRuleDetailForContantValue.RORD_CreatedOn = DateTime.Now;
                            existingObjectRule.RequirementObjectRuleDetails.Add(requirementObjectRuleDetailForContantValue);
                        }
                        else
                        {
                            requirementObjectRuleDetailForContantValue = requirementObjectRuleDetailList.FirstOrDefault(cond => cond.RORD_ParameterOrder == AppConsts.TWO);
                            requirementObjectRuleDetailForContantValue.RORD_ParameterOrder = AppConsts.TWO;
                            requirementObjectRuleDetailForContantValue.RORD_ModifiedByID = currentLoggedInUserID;
                            requirementObjectRuleDetailForContantValue.RORD_ModifiedOn = DateTime.Now;
                            requirementObjectRuleDetailForContantValue.RORD_RuleObjectMappingTypeID = constMappingType.RMT_ID;
                            requirementObjectRuleDetailForContantValue.RORD_ConstantType = requirementItemContractObj.RequirementItemExpiration.ExpirationValueTypeID;
                            requirementObjectRuleDetailForContantValue.RORD_ConstantValue = requirementItemContractObj.RequirementItemExpiration.ExpirationValue.ToString();
                        }
                    }
                }
            }
            if (!requirementItemContractObj.IsRequirementItemNeededExpiration && !existingObjectRule.IsNullOrEmpty())
            {
                //if expiration is not needed and rule already exist for expiration.
                existingObjectRule.ROR_IsDeleted = true;
                existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                existingObjectRule.ROR_ModifiedOn = DateTime.Now;
                List<RequirementObjectRuleDetail> requirementObjectRuleDetailList = existingObjectRule.RequirementObjectRuleDetails.Where(cond => !cond.RORD_IsDeleted)
                                                                                                                                               .ToList();
                foreach (RequirementObjectRuleDetail requirementObjectRuleDetail in requirementObjectRuleDetailList)
                {
                    requirementObjectRuleDetail.RORD_IsDeleted = true;
                    requirementObjectRuleDetail.RORD_ModifiedByID = currentLoggedInUserID;
                    requirementObjectRuleDetail.RORD_ModifiedOn = DateTime.Now;
                }
            }

        }

        private static void InsertItemExpirationObjectRule(Int32 currentLoggedInUserID, List<RequirementObjectTreeContract> requirmntObjTreeCntrct, lkpRuleObjectMappingType constMappingType, List<lkpFixedRuleType> lstFixedRuleType,
                                                                                        lkpObjectType attrObjectType, lkpRuleType buisnessRuleType, lkpRuleActionType itemExpiratnActionType, RequirementItem requirmntItem,
                                                                                        IEnumerable<RequirementItemField> requirmntItmFields, RequirementItemContract requirementItemContractObj,
                                                                                        RequirementObjectTreeContract itmRequirmntObjTree, RequirementObjectTree itmRequirmntObjTreeFromDb, lkpConstantType dateConstantType
                                                                                        , lkpRuleObjectMappingType dataValueMappingType)
        {
            RequirementObjectRule newExpirationObjectRule = new RequirementObjectRule();
            newExpirationObjectRule.ROR_RuleTemplateID = null;
            newExpirationObjectRule.ROR_Code = Guid.NewGuid();
            newExpirationObjectRule.ROR_Name = requirementItemContractObj.RequirementItemName + "_ItemExpirationRule";
            newExpirationObjectRule.ROR_ObjectTreeId = itmRequirmntObjTree.RequirementObjectTreeID;
            newExpirationObjectRule.ROR_RuleTemplateID = null;
            newExpirationObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode).FRLT_ID;
            newExpirationObjectRule.ROR_RuleType = buisnessRuleType.RLT_ID;
            newExpirationObjectRule.ROR_ActionType = itemExpiratnActionType.ACT_ID;
            newExpirationObjectRule.ROR_FirstVersionID = null;
            newExpirationObjectRule.ROR_IsCurrent = true;
            newExpirationObjectRule.ROR_IsActive = true;
            newExpirationObjectRule.ROR_IsDeleted = false;
            newExpirationObjectRule.ROR_CreatedByID = currentLoggedInUserID;
            newExpirationObjectRule.ROR_CreatedOn = DateTime.Now;

            RequirementObjectRuleDetail requirementObjectRuleDetail = new RequirementObjectRuleDetail();
            requirementObjectRuleDetail.RORD_PlaceHolderName = null;
            requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = constMappingType.RMT_ID;
            requirementObjectRuleDetail.RORD_ObjectTreeID = null;
            if (requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode
                           == RequirementFixedRuleType.FIXED_EXPIRATION.GetStringValue())
            {
                requirementObjectRuleDetail.RORD_ParameterOrder = 1;
                requirementObjectRuleDetail.RORD_ConstantType = dateConstantType.ID;
                requirementObjectRuleDetail.RORD_ConstantValue = requirementItemContractObj.RequirementItemExpiration.ExpirationDate;
            }
            else if (requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode
                           == RequirementFixedRuleType.EXPIRES_CONDITIONALLY.GetStringValue())
            {
                //UAT-2165
                requirementObjectRuleDetail.RORD_ParameterOrder = 1;
                requirementObjectRuleDetail.RORD_ConstantType = dateConstantType.ID;
                requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                requirementObjectRuleDetail.RORD_ConstantValue = requirementItemContractObj.RequirementItemExpiration.ExpirationDate;
                requirementObjectRuleDetail.RORD_ExpirationCondStartDate = requirementItemContractObj.RequirementItemExpiration.ExpirationCondStartDate;
                requirementObjectRuleDetail.RORD_ExpirationCondEndDate = requirementItemContractObj.RequirementItemExpiration.ExpirationCondEndDate;
                RequirementField exprationDateField = requirmntItmFields.Where(cond => cond.RequirementField.RF_Code == requirementItemContractObj.RequirementItemExpiration.SelectedDateTypeFieldCode)
                                                                                                                                                        .FirstOrDefault().RequirementField;
                if (exprationDateField.IsNotNull())
                {
                    RequirementObjectTreeContract exprationDateObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == attrObjectType.OT_ID
                                                                                                                                           && cond.ObjectID == exprationDateField.RF_ID
                                                                                                                                           && cond.ParentObjectID == requirmntItem.RI_ID);
                    requirementObjectRuleDetail.RORD_ObjectTreeID = exprationDateObjTree.RequirementObjectTreeID;
                }
            }
            else
            {
                requirementObjectRuleDetail.RORD_ParameterOrder = 2;
                requirementObjectRuleDetail.RORD_ConstantType = requirementItemContractObj.RequirementItemExpiration.ExpirationValueTypeID;
                requirementObjectRuleDetail.RORD_ConstantValue = requirementItemContractObj.RequirementItemExpiration.ExpirationValue.ToString();
            }
            requirementObjectRuleDetail.RORD_IsDeleted = false;
            requirementObjectRuleDetail.RORD_CreatedByID = currentLoggedInUserID;
            requirementObjectRuleDetail.RORD_CreatedOn = DateTime.Now;
            newExpirationObjectRule.RequirementObjectRuleDetails.Add(requirementObjectRuleDetail);

            if (requirementItemContractObj.RequirementItemExpiration.RequirementItemExpirationTypeCode
               == RequirementFixedRuleType.ENTERED_DATE_BASED_EXPIRATION.GetStringValue())
            {
                requirementObjectRuleDetail = new RequirementObjectRuleDetail();
                requirementObjectRuleDetail.RORD_PlaceHolderName = null;
                requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                requirementObjectRuleDetail.RORD_ParameterOrder = 1;
                requirementObjectRuleDetail.RORD_ConstantType = null;
                requirementObjectRuleDetail.RORD_ConstantValue = null;
                requirementObjectRuleDetail.RORD_ObjectTreeID = null;
                requirementObjectRuleDetail.RORD_IsDeleted = false;
                requirementObjectRuleDetail.RORD_CreatedByID = currentLoggedInUserID;
                requirementObjectRuleDetail.RORD_CreatedOn = DateTime.Now;

                RequirementField exprationDateField = requirmntItmFields.Where(cond => cond.RequirementField.RF_Code == requirementItemContractObj.RequirementItemExpiration.SelectedDateTypeFieldCode)
                .FirstOrDefault().RequirementField;
                if (exprationDateField.IsNotNull())
                {
                    RequirementObjectTreeContract exprationDateObjTree = requirmntObjTreeCntrct.FirstOrDefault(cond => cond.ObjectTypeID == attrObjectType.OT_ID
                                                                                                                                           && cond.ObjectID == exprationDateField.RF_ID
                                                                                                                                           && cond.ParentObjectID == requirmntItem.RI_ID);
                    requirementObjectRuleDetail.RORD_ObjectTreeID = exprationDateObjTree.RequirementObjectTreeID;
                }
                newExpirationObjectRule.RequirementObjectRuleDetails.Add(requirementObjectRuleDetail);
            }
            itmRequirmntObjTreeFromDb.RequirementObjectRules.Add(newExpirationObjectRule);
        }

        private static void InsertUpdateRequiredObjectProperty(Int32 currentLoggedInUserID, lkpObjectAttribute requiredObjectAttribute, RequirementFieldContract requirementFieldContractObj, RequirementObjectTreeContract attrRequirmntObjTree, RequirementObjectTree attrRequirmntObjTreeFromDb)
        {
            if (attrRequirmntObjTree.IsNewRecordInserted)
            {
                RequirementObjectTreeProperty newRequirementObjectTreeProperty = new RequirementObjectTreeProperty();
                newRequirementObjectTreeProperty.ROTP_ObjectTreeID = attrRequirmntObjTree.RequirementObjectTreeID;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeID = requiredObjectAttribute.IsNotNull() ? requiredObjectAttribute.OA_ID : AppConsts.NONE;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.IsFieldRequired.ToString();
                newRequirementObjectTreeProperty.ROTP_IsDeleted = false;
                newRequirementObjectTreeProperty.ROTP_CreatedByID = currentLoggedInUserID;
                newRequirementObjectTreeProperty.ROTP_CreatedOn = DateTime.Now;
                attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.Add(newRequirementObjectTreeProperty);
            }
            else
            {
                RequirementObjectTreeProperty existingRequirementObjectTreeProperty = attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.
                                                                                                        FirstOrDefault(cond => !cond.ROTP_IsDeleted);
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeID = requiredObjectAttribute.IsNotNull() ? requiredObjectAttribute.OA_ID : AppConsts.NONE;
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.IsFieldRequired.ToString();
                existingRequirementObjectTreeProperty.ROTP_ModifiedByID = currentLoggedInUserID;
                existingRequirementObjectTreeProperty.ROTP_ModifiedOn = DateTime.Now;
            }
        }

        private static void InsertUpdateSignatureRequiredObjectProperty(Int32 currentLoggedInUserID, lkpObjectAttribute signatureRequiredObjectAttribute, RequirementFieldContract requirementFieldContractObj, RequirementObjectTreeContract attrRequirmntObjTree, RequirementObjectTree attrRequirmntObjTreeFromDb)
        {
            if (requirementFieldContractObj.RequirementFieldData.FieldViewDocumentData.LstDocumentAcroFieldTypeCodes.IsNotNull()
               && requirementFieldContractObj.RequirementFieldData.FieldViewDocumentData.LstDocumentAcroFieldTypeCodes.
                                                                                Contains(DocumentAcroFieldType.SIGNATURE.GetStringValue()))
            {
                if (attrRequirmntObjTree.IsNewRecordInserted)
                {
                    RequirementObjectTreeProperty newRequirementObjectTreeProperty = new RequirementObjectTreeProperty();
                    newRequirementObjectTreeProperty.ROTP_ObjectTreeID = attrRequirmntObjTree.RequirementObjectTreeID;
                    newRequirementObjectTreeProperty.ROTP_IsDeleted = false;
                    newRequirementObjectTreeProperty.ROTP_CreatedByID = currentLoggedInUserID;
                    newRequirementObjectTreeProperty.ROTP_CreatedOn = DateTime.Now;
                    newRequirementObjectTreeProperty.ROTP_ObjectAttributeID = signatureRequiredObjectAttribute.IsNotNull() ?
                                                                            signatureRequiredObjectAttribute.OA_ID : AppConsts.NONE;
                    newRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.IsFieldRequired.ToString();
                    attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.Add(newRequirementObjectTreeProperty);
                }
                else
                {
                    RequirementObjectTreeProperty existingRequirementObjectTreeProperty = attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.FirstOrDefault(cond => !cond.ROTP_IsDeleted
                                                                                                            && cond.ROTP_ObjectAttributeID == signatureRequiredObjectAttribute.OA_ID);
                    existingRequirementObjectTreeProperty.ROTP_ObjectAttributeID = signatureRequiredObjectAttribute.IsNotNull() ?
                                                                            signatureRequiredObjectAttribute.OA_ID : AppConsts.NONE;
                    existingRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.IsFieldRequired.ToString();
                    existingRequirementObjectTreeProperty.ROTP_ModifiedByID = currentLoggedInUserID;
                    existingRequirementObjectTreeProperty.ROTP_ModifiedOn = DateTime.Now;
                }
            }
        }

        private static void InsertUpdateRequiredToViewObjectProperty(Int32 currentLoggedInUserID, lkpObjectAttribute requiredToViewObjectAttribute, RequirementFieldContract requirementFieldContractObj, RequirementObjectTreeContract attrRequirmntObjTree, RequirementObjectTree attrRequirmntObjTreeFromDb)
        {
            if (attrRequirmntObjTree.IsNewRecordInserted)
            {
                RequirementObjectTreeProperty requiredToViewObjectTreeProperty = new RequirementObjectTreeProperty();
                requiredToViewObjectTreeProperty.ROTP_ObjectTreeID = attrRequirmntObjTree.RequirementObjectTreeID;
                requiredToViewObjectTreeProperty.ROTP_IsDeleted = false;
                requiredToViewObjectTreeProperty.ROTP_CreatedByID = currentLoggedInUserID;
                requiredToViewObjectTreeProperty.ROTP_CreatedOn = DateTime.Now;
                requiredToViewObjectTreeProperty.ROTP_ObjectAttributeID = requiredToViewObjectAttribute.IsNotNull() ?
                                                                           requiredToViewObjectAttribute.OA_ID : AppConsts.NONE;
                requiredToViewObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.IsFieldRequired.ToString();
                attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.Add(requiredToViewObjectTreeProperty);
            }
            else
            {
                RequirementObjectTreeProperty existingRequirementObjectTreeProperty = attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.FirstOrDefault(cond => !cond.ROTP_IsDeleted
                                                                                                        && cond.ROTP_ObjectAttributeID == requiredToViewObjectAttribute.OA_ID);
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeID = requiredToViewObjectAttribute.IsNotNull() ?
                                                                           requiredToViewObjectAttribute.OA_ID : AppConsts.NONE;
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = requirementFieldContractObj.IsFieldRequired.ToString();
                existingRequirementObjectTreeProperty.ROTP_ModifiedByID = currentLoggedInUserID;
                existingRequirementObjectTreeProperty.ROTP_ModifiedOn = DateTime.Now;
            }
        }

        private static void InsertUpdateBoxStayOpenObjectProperty(Int32 currentLoggedInUserID, lkpObjectAttribute boxStayOpenObjectAttribute, RequirementObjectTreeContract attrRequirmntObjTree, RequirementObjectTree attrRequirmntObjTreeFromDb, RequirementFieldVideoData VideoFieldData)
        {
            if (attrRequirmntObjTree.IsNewRecordInserted)
            {
                RequirementObjectTreeProperty newRequirementObjectTreeProperty = new RequirementObjectTreeProperty();
                newRequirementObjectTreeProperty.ROTP_ObjectTreeID = attrRequirmntObjTree.RequirementObjectTreeID;
                newRequirementObjectTreeProperty.ROTP_IsDeleted = false;
                newRequirementObjectTreeProperty.ROTP_CreatedByID = currentLoggedInUserID;
                newRequirementObjectTreeProperty.ROTP_CreatedOn = DateTime.Now;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeID = boxStayOpenObjectAttribute.IsNotNull() ?
                                                                           boxStayOpenObjectAttribute.OA_ID : AppConsts.NONE;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = VideoFieldData.VideoOpenTimeDuration.ToString();
                attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.Add(newRequirementObjectTreeProperty);
            }
            else
            {
                RequirementObjectTreeProperty existingRequirementObjectTreeProperty = attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.FirstOrDefault(cond => !cond.ROTP_IsDeleted
                                                                                                        && cond.ROTP_ObjectAttributeID == boxStayOpenObjectAttribute.OA_ID);
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeID = boxStayOpenObjectAttribute.IsNotNull() ?
                                                                       boxStayOpenObjectAttribute.OA_ID : AppConsts.NONE;
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = VideoFieldData.VideoOpenTimeDuration.ToString();
                existingRequirementObjectTreeProperty.ROTP_ModifiedByID = currentLoggedInUserID;
                existingRequirementObjectTreeProperty.ROTP_ModifiedOn = DateTime.Now;
            }
        }

        private static void InsertUpdateRequiredToOpenObjectProperty(Int32 currentLoggedInUserID, lkpObjectAttribute requiredToOpenObjectAttribute, RequirementFieldContract requirementFieldContractObj, RequirementObjectTreeContract attrRequirmntObjTree, RequirementObjectTree attrRequirmntObjTreeFromDb, RequirementFieldVideoData VideoFieldData)
        {
            if (attrRequirmntObjTree.IsNewRecordInserted)
            {
                RequirementObjectTreeProperty newRequirementObjectTreeProperty = new RequirementObjectTreeProperty();
                newRequirementObjectTreeProperty.ROTP_ObjectTreeID = attrRequirmntObjTree.RequirementObjectTreeID;
                newRequirementObjectTreeProperty.ROTP_IsDeleted = false;
                newRequirementObjectTreeProperty.ROTP_CreatedByID = currentLoggedInUserID;
                newRequirementObjectTreeProperty.ROTP_CreatedOn = DateTime.Now;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeID = requiredToOpenObjectAttribute.IsNotNull() ? requiredToOpenObjectAttribute.OA_ID : AppConsts.NONE;
                newRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = VideoFieldData.IsVideoRequiredToBeOpened.ToString();
                attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.Add(newRequirementObjectTreeProperty);
            }
            else
            {
                RequirementObjectTreeProperty existingRequirementObjectTreeProperty = attrRequirmntObjTreeFromDb.RequirementObjectTreeProperties.FirstOrDefault(cond => !cond.ROTP_IsDeleted
                                                                                                        && cond.ROTP_ObjectAttributeID == requiredToOpenObjectAttribute.OA_ID);
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeID = requiredToOpenObjectAttribute.IsNotNull() ?
                                                                           requiredToOpenObjectAttribute.OA_ID : AppConsts.NONE;
                existingRequirementObjectTreeProperty.ROTP_ObjectAttributeValue = VideoFieldData.IsVideoRequiredToBeOpened.ToString();
                existingRequirementObjectTreeProperty.ROTP_ModifiedByID = currentLoggedInUserID;
                existingRequirementObjectTreeProperty.ROTP_ModifiedOn = DateTime.Now;
            }
        }

        private static void InsertUpdateFieldUiRule(Int32 currentLoggedInUserID, List<lkpRuleObjectMappingType> lstRuleObjectMappingType,
                                                List<lkpFixedRuleType> lstFixedRuleType, lkpRuleType uiRuleType, lkpRuleActionType defaultActionType,
                                                String ruleName, RequirementObjectTree requirmntObjTreeFromDb, RequirementFieldContract requirementFieldContractObj
                                                 , RequirementObjectTreeContract attrRequirmntObjTree, RequirementObjectTreeContract uiRuleDateObjTree)
        {
            lkpRuleObjectMappingType dataValueMappingType = lstRuleObjectMappingType.FirstOrDefault(cond => cond.RMT_Code == "DVAL");
            if (attrRequirmntObjTree.IsNewRecordInserted)
            {
                //insert new rule.
                RequirementObjectRule newObjectRule = new RequirementObjectRule();
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_Code = Guid.NewGuid();
                newObjectRule.ROR_Name = ruleName;
                newObjectRule.ROR_ObjectTreeId = attrRequirmntObjTree.RequirementObjectTreeID;
                newObjectRule.ROR_RuleTemplateID = null;
                newObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementFieldContractObj.RequirementFieldUIRuleTypeCode).FRLT_ID;
                newObjectRule.ROR_RuleType = uiRuleType.RLT_ID;
                newObjectRule.ROR_ActionType = defaultActionType.ACT_ID;
                newObjectRule.ROR_FirstVersionID = null;
                newObjectRule.ROR_IsCurrent = true;
                newObjectRule.ROR_IsActive = true;
                newObjectRule.ROR_IsDeleted = false;
                newObjectRule.ROR_CreatedByID = currentLoggedInUserID;
                newObjectRule.ROR_CreatedOn = DateTime.Now;
                newObjectRule.ROR_ErroMessage = requirementFieldContractObj.UiRuleErrorMessage;

                RequirementObjectRuleDetail requirementObjectRuleDetail = new RequirementObjectRuleDetail();
                requirementObjectRuleDetail.RORD_PlaceHolderName = null;
                requirementObjectRuleDetail.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                requirementObjectRuleDetail.RORD_ObjectTreeID = uiRuleDateObjTree.RequirementObjectTreeID;
                requirementObjectRuleDetail.RORD_IsDeleted = false;
                requirementObjectRuleDetail.RORD_CreatedByID = currentLoggedInUserID;
                requirementObjectRuleDetail.RORD_CreatedOn = DateTime.Now;
                newObjectRule.RequirementObjectRuleDetails.Add(requirementObjectRuleDetail);
                requirmntObjTreeFromDb.RequirementObjectRules.Add(newObjectRule);
            }
            else
            {
                if (requirmntObjTreeFromDb.RequirementObjectRules.IsNotNull())
                {
                    //update existing rule
                    RequirementObjectRule existingObjectRule = requirmntObjTreeFromDb.RequirementObjectRules.FirstOrDefault(cond => !cond.ROR_IsDeleted
                                                                                            && (cond.lkpFixedRuleType.FRLT_Code == RequirementFixedRuleType.LESS_THAN.GetStringValue()
                                                                                            || cond.lkpFixedRuleType.FRLT_Code == RequirementFixedRuleType.GREATER_THAN.GetStringValue()));
                    if (existingObjectRule.IsNotNull())
                    {
                        existingObjectRule.ROR_FixedRuleType = lstFixedRuleType.FirstOrDefault(cond => cond.FRLT_Code == requirementFieldContractObj.RequirementFieldUIRuleTypeCode).FRLT_ID;
                        existingObjectRule.ROR_ModifiedByID = currentLoggedInUserID;
                        existingObjectRule.ROR_ModifiedOn = DateTime.Now;
                        existingObjectRule.ROR_ErroMessage = requirementFieldContractObj.UiRuleErrorMessage;
                        RequirementObjectRuleDetail requirementObjectRuleDetailInDb = existingObjectRule.RequirementObjectRuleDetails.Where(cond => !cond.RORD_IsDeleted).FirstOrDefault();
                        requirementObjectRuleDetailInDb.RORD_PlaceHolderName = null;
                        requirementObjectRuleDetailInDb.RORD_RuleObjectMappingTypeID = dataValueMappingType.RMT_ID;
                        requirementObjectRuleDetailInDb.RORD_ObjectTreeID = uiRuleDateObjTree.RequirementObjectTreeID;
                        requirementObjectRuleDetailInDb.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetailInDb.RORD_ModifiedOn = DateTime.Now;
                    }
                }
            }
        }
        #endregion

        private static Boolean IsPackageVersionNeedToCreate(RequirementPackageContract requirementPackageContract, Int32 tenantID)
        {
            if (requirementPackageContract.RequirementPackageID == AppConsts.NONE)
            {
                return false;
            }
            return BALUtils.GetRequirementPackageRepoInstance(tenantID).IsPackageVersionNeedToCreate(requirementPackageContract.RequirementPackageID, requirementPackageContract.RequirementPkgTypeCode);
        }

        public static Boolean IsPackageMappedToRotation(Int32 requirementPackageID, Int32 tenantID)
        {
            return BALUtils.GetRequirementPackageRepoInstance(tenantID).IsPackageMappedToRotation(requirementPackageID);
        }

        private static Int32 AddNewRequirementPackage(RequirementPackageContract requirementPackageContract, Int32 selectedTenantID
                                                       , Int32 currentLoggedInUserID, Boolean isPackageVersionNeedToCreate)
        {
            #region Add data into RequirementPackage table

            RequirementPackage requirementPackage = new RequirementPackage();
            CreateRequirementPackage(requirementPackageContract, selectedTenantID, currentLoggedInUserID, isPackageVersionNeedToCreate, requirementPackage);

            //Add entries in table RequirementPackageAgency. N rows will be inserted for N selected agencies
            CreateRqrmntPkgAgency(requirementPackageContract, currentLoggedInUserID, requirementPackage);

            #endregion

            #region Add data into mapping,detail tables table

            List<RequirementFieldContract> requirementFieldContractList = new List<RequirementFieldContract>();
            List<RequirementItemContract> requirementItemContractList = new List<RequirementItemContract>();

            foreach (RequirementCategoryContract categoryContract in requirementPackageContract.LstRequirementCategory.Where(cond => !cond.IsDeleted))
            {
                RequirementPackageCategory reqrmntPackageCategory = GetRequirementPackageCategory(selectedTenantID, currentLoggedInUserID, requirementPackage
                                                                                                 , requirementFieldContractList, requirementItemContractList
                                                                                                 , categoryContract);
                requirementPackage.RequirementPackageCategories.Add(reqrmntPackageCategory);
            }

            //Code to SAVE RequirementPackageCategory AND RequirementPackage into DB
            BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).AddRequirementPackageToDatabase(requirementPackage);

            #endregion

            #region Add category explanatory notes

            if (requirementPackage.RP_ID > AppConsts.NONE)
            {
                List<RequirementCategory> addedreqrmntCatgoriesList = requirementPackage.RequirementPackageCategories.Select(col => col.RequirementCategory).ToList();

                List<LargeContent> largeContentList = new List<LargeContent>();

                foreach (RequirementCategory requirementcategory in addedreqrmntCatgoriesList)
                {
                    String explanatoryNotes = requirementPackageContract.LstRequirementCategory
                                                .Where(cond => cond.RequirementCategoryCode == requirementcategory.RC_Code).FirstOrDefault().ExplanatoryNotes;
                    if (!explanatoryNotes.IsNullOrEmpty())
                    {
                        LargeContent largeContent = CreateLargeContentForReqrmntCategory(requirementcategory.RC_ID, explanatoryNotes, currentLoggedInUserID, selectedTenantID);
                        largeContentList.Add(largeContent);
                    }
                }

                if (!largeContentList.IsNullOrEmpty())
                {
                    BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).AddLargeContentToDatabase(largeContentList);
                }
            }

            #endregion


            if (requirementPackage.RP_ID > AppConsts.NONE)
            {
                InsertDataIntoRequirementObjectTree(requirementPackageContract, selectedTenantID, currentLoggedInUserID, requirementPackage, requirementFieldContractList, requirementItemContractList, isPackageVersionNeedToCreate);
            }

            return requirementPackage.RP_ID;
        }


        private static RequirementPackageCategory GetRequirementPackageCategory(Int32 selectedTenantID, Int32 currentLoggedInUserID
                                                                                , RequirementPackage requirementPackage
                                                                                , List<RequirementFieldContract> requirementFieldContractList
                                                                                , List<RequirementItemContract> requirementItemContractList
                                                                                , RequirementCategoryContract categoryContract)
        {
            //Add data into category and package category mapping tables
            //Add new category
            RequirementCategory requirementCategory = CreateRequrmntCategory(currentLoggedInUserID, categoryContract);

            foreach (RequirementItemContract itemContract in categoryContract.LstRequirementItem.Where(cond => !cond.IsDeleted))
            {
                requirementItemContractList.Add(itemContract);
                RequirementCategoryItem requrmntCategoryItem = GetRequirementCategoryItem(selectedTenantID, currentLoggedInUserID
                                                                                           , requirementFieldContractList
                                                                                           , requirementCategory, itemContract);

                requirementCategory.RequirementCategoryItems.Add(requrmntCategoryItem);
            }

            //Add new package category mapping
            RequirementPackageCategory reqrmntPackageCategory = CreateRequrmntPackageCategory(currentLoggedInUserID, requirementPackage, requirementCategory);
            #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
            //Add ComplinaceRequired setting in RequirementPackageCategory table
            reqrmntPackageCategory.RPC_ComplianceRequired = categoryContract.IsComplianceRequired;
            reqrmntPackageCategory.RPC_ComplianceRqdStartDate = categoryContract.ComplianceReqStartDate;
            reqrmntPackageCategory.RPC_ComplianceRqdEndDate = categoryContract.ComplianceReqEndDate;
            #endregion
            return reqrmntPackageCategory;
        }


        private static RequirementCategoryItem GetRequirementCategoryItem(Int32 selectedTenantID, Int32 currentLoggedInUserID
                                                                          , List<RequirementFieldContract> requirementFieldContractList
                                                                          , RequirementCategory requirementCategory, RequirementItemContract itemContract)
        {
            //Add new Item
            RequirementItem requirementItem = CreateRequirementItem(currentLoggedInUserID, itemContract);

            #region Define common variables and get lkp values

            List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType = LookupManager.GetLookUpData<lkpDocumentAcroFieldType>(selectedTenantID).ToList();

            String requirementFieldViewDocumentTypeCode = DocumentType.REQUIREMENT_FIELD_VIEW_DOCUMENT.GetStringValue();
            Int32 requirementFieldViewDocumentTypeID = LookupManager.GetLookUpData<lkpDocumentType>(selectedTenantID)
                                                        .Where(cond => cond.DMT_Code == requirementFieldViewDocumentTypeCode).FirstOrDefault().DMT_ID;

            List<lkpRequirementFieldDataType> lstLkpRequirementFieldDataType = LookupManager.GetLookUpData<lkpRequirementFieldDataType>(selectedTenantID).ToList();

            #endregion

            //Add data into field and item field mapping tables
            foreach (RequirementFieldContract fieldContract in itemContract.LstRequirementField.Where(cond => !cond.IsDeleted))
            {
                RequirementItemField requrmntItemField = GetRequirementItemField(currentLoggedInUserID, requirementFieldContractList
                                                                                , requirementItem, lstLkpDocumentAcroFieldType, requirementFieldViewDocumentTypeID
                                                                                , lstLkpRequirementFieldDataType, fieldContract);
                requirementItem.RequirementItemFields.Add(requrmntItemField);
            }

            //Add new category item mapping
            RequirementCategoryItem requrmntCategoryItem = CreateRequrmntCategoryItem(currentLoggedInUserID, requirementCategory, requirementItem, itemContract.RequirementItemDisplayOrder);
            return requrmntCategoryItem;
        }


        private static RequirementItemField GetRequirementItemField(Int32 currentLoggedInUserID, List<RequirementFieldContract> requirementFieldContractList, RequirementItem requirementItem, List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType, Int32 requirementFieldViewDocumentTypeID, List<lkpRequirementFieldDataType> lstLkpRequirementFieldDataType, RequirementFieldContract fieldContract)
        {
            requirementFieldContractList.Add(fieldContract);

            //Add new field
            RequirementField requirementField = CreateRequirementField(currentLoggedInUserID, requirementFieldViewDocumentTypeID,
                                                                       lstLkpRequirementFieldDataType, lstLkpDocumentAcroFieldType, fieldContract);

            //Add new item field mapping
            RequirementItemField requrmntItemField = CreateRequirementItemField(currentLoggedInUserID, requirementItem, requirementField, fieldContract.IsBackgroundDocument, fieldContract.RequirementFieldDisplayOrder);
            return requrmntItemField;
        }

        private static RequirementField CreateRequirementField(Int32 currentLoggedInUserID, Int32 requirementFieldViewDocumentTypeID
                                                               , List<lkpRequirementFieldDataType> lstLkpRequirementFieldDataType
                                                               , List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType, RequirementFieldContract fieldContract)
        {
            RequirementField requirementField = new RequirementField();

            requirementField.RF_FieldName = fieldContract.RequirementFieldName;
            requirementField.RF_FieldLabel = fieldContract.RequirementFieldLabel;
            requirementField.RF_AttributeTypeID = fieldContract.AttributeTypeID;
            requirementField.RF_CreatedByID = currentLoggedInUserID;
            requirementField.RF_CreatedOn = DateTime.Now;
            requirementField.RF_IsDeleted = false;
            requirementField.RF_FieldDataTypeID = lstLkpRequirementFieldDataType
                                                    .Where(cond => cond.RFDT_Code == fieldContract.RequirementFieldData.RequirementFieldDataTypeCode)
                                                    .FirstOrDefault().RFDT_ID;

            requirementField.RF_Code = fieldContract.RequirementFieldCode;

            //code to save field data values
            if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
            {
                RequirementFieldVideo requirementFieldVideo = CreateReqirmntFieldVideo(currentLoggedInUserID, fieldContract);
                requirementField.RequirementFieldVideos.Add(requirementFieldVideo);
            }
            else if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue())
            {
                AddReqrmntFieldOptionsForReqrmntField(currentLoggedInUserID, fieldContract, requirementField);
            }
            else if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
            {
                RequirementFieldDocument requrmntFieldDocument = CreateRequirementFieldDocumentType(currentLoggedInUserID, requirementFieldViewDocumentTypeID
                                                                                                     , lstLkpDocumentAcroFieldType, fieldContract);
                requirementField.RequirementFieldDocuments.Add(requrmntFieldDocument);
            }
            else if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.TEXT.GetStringValue())
            {
                requirementField.RF_MaximumCharacters = fieldContract.RequirementFieldMaxLength;
            }
            //
            return requirementField;
        }

        private static Int32 UpdateRequirementPackage(RequirementPackageContract requirementPackageContract, Int32 selectedTenantID, Int32 currentLoggedInUserID)
        {
            List<RequirementFieldContract> requirementFieldContractList = new List<RequirementFieldContract>();
            List<RequirementItemContract> requirementItemContractList = new List<RequirementItemContract>();

            //Get existing requirement package(from DB) which is to be updated 
            RequirementPackage existingRequirementPackage = BALUtils.GetRequirementPackageRepoInstance(selectedTenantID)
                                                                    .GetRequirementPackageByPackageID(requirementPackageContract.RequirementPackageID);

            //if package does not exists
            if (existingRequirementPackage.IsNullOrEmpty())
            {
                return 0;
            }
            else if (requirementPackageContract.IsUpdated)
            {
                existingRequirementPackage.RP_PackageName = requirementPackageContract.RequirementPackageName;
                existingRequirementPackage.RP_PackageLabel = requirementPackageContract.RequirementPackageLabel;
                existingRequirementPackage.RP_DefinedRequirementID = requirementPackageContract.DefinedRequirementID;
                existingRequirementPackage.RP_ReqReviewByID = requirementPackageContract.ReqReviewByID;

                UpdateReqrmntPackageAgency(requirementPackageContract, currentLoggedInUserID, existingRequirementPackage);
                BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).SaveContextIntoDataBase();
            }
            else
            {
                #region Define common variables and get lkp values

                String requirementFieldViewDocumentTypeCode = DocumentType.REQUIREMENT_FIELD_VIEW_DOCUMENT.GetStringValue();

                Int32 requirementFieldViewDocumentTypeID = LookupManager.GetLookUpData<lkpDocumentType>(selectedTenantID)
                                                            .Where(cond => cond.DMT_Code == requirementFieldViewDocumentTypeCode).FirstOrDefault().DMT_ID;

                List<lkpRequirementFieldDataType> lstLkpRequirementFieldDataType = LookupManager.GetLookUpData<lkpRequirementFieldDataType>(selectedTenantID).ToList();

                List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType = LookupManager.GetLookUpData<lkpDocumentAcroFieldType>(selectedTenantID).ToList();

                #endregion

                foreach (RequirementCategoryContract categoryContract in requirementPackageContract.LstRequirementCategory)
                {
                    //if category is newly added
                    if (categoryContract.IsNewCategory)
                    {
                        RequirementPackageCategory reqrmntPackageCategory = GetRequirementPackageCategory(selectedTenantID, currentLoggedInUserID, existingRequirementPackage
                                                                                                     , requirementFieldContractList, requirementItemContractList
                                                                                                     , categoryContract);

                        existingRequirementPackage.RequirementPackageCategories.Add(reqrmntPackageCategory);
                    }
                    else
                    {
                        RequirementCategory existingRequrmntCategory = existingRequirementPackage.RequirementPackageCategories
                                                                                         .Where(cond => cond.RPC_ID == categoryContract.RequirementPackageCategoryID && !cond.RPC_IsDeleted)
                                                                                         .Select(col => col.RequirementCategory).FirstOrDefault();

                        if (categoryContract.IsUpdated)
                        {
                            //code to update category table objecte
                            existingRequrmntCategory.RC_CategoryName = categoryContract.RequirementCategoryName;
                            //existingRequrmntCategory.RC_CategoryLabel = categoryContract.RequirementCategoryLabel;
                            //existingRequrmntCategory.RC_Description = categoryContract.RequirementCategoryDescription;
                            existingRequrmntCategory.RC_ModifiedByID = currentLoggedInUserID;
                            existingRequrmntCategory.RC_ModifiedOn = DateTime.Now;
                            //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes)
                            if (!categoryContract.RequirementDocumentLink.IsNullOrEmpty())
                            {
                                existingRequrmntCategory.RC_SampleDocFormURL = categoryContract.RequirementDocumentLink;
                            }
                            else
                            {
                                existingRequrmntCategory.RC_SampleDocFormURL = null;
                            }
                            //UAT-3161
                            if (!categoryContract.RequirementDocumentLinkLabel.IsNullOrEmpty())
                            {
                                existingRequrmntCategory.RC_SampleDocFormUrlLabel = categoryContract.RequirementDocumentLinkLabel;
                            }
                            else
                            {
                                existingRequrmntCategory.RC_SampleDocFormUrlLabel = null;
                            }
                            UpdateLargeContentForReqrmntCategory(existingRequrmntCategory.RC_ID, categoryContract.ExplanatoryNotes, currentLoggedInUserID, selectedTenantID);

                            #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
                            //Update the RequirementPackagecategory for ComplianceRequired Setting.
                            RequirementPackageCategory requirementPackageCategoryToUpdate = existingRequrmntCategory.RequirementPackageCategories.Where(cond => cond.RPC_RequirementCategoryID == categoryContract.RequirementCategoryID
                                                                                                                                                        && cond.RPC_RequirementPackageID == requirementPackageContract.RequirementPackageID
                                                                                                                                                        && !cond.RPC_IsDeleted).FirstOrDefault();
                            if (!requirementPackageCategoryToUpdate.IsNullOrEmpty())
                            {
                                requirementPackageCategoryToUpdate.RPC_ComplianceRequired = categoryContract.IsComplianceRequired;
                                requirementPackageCategoryToUpdate.RPC_ComplianceRqdStartDate = categoryContract.ComplianceReqStartDate;
                                requirementPackageCategoryToUpdate.RPC_ComplianceRqdEndDate = categoryContract.ComplianceReqEndDate;
                                requirementPackageCategoryToUpdate.RPC_ModifiedByID = currentLoggedInUserID;
                                requirementPackageCategoryToUpdate.RPC_ModifiedOn = DateTime.Now;
                            }
                            #endregion

                            break;
                        }
                        else
                        {
                            foreach (RequirementItemContract itemContract in categoryContract.LstRequirementItem)
                            {
                                //code to add items,its fields and fields data into database
                                requirementItemContractList.Add(itemContract);

                                //if item is newly added 
                                if (itemContract.IsNewItem)
                                {
                                    RequirementCategoryItem requrmntCategoryItem = GetRequirementCategoryItem(selectedTenantID, currentLoggedInUserID
                                                                                                               , requirementFieldContractList
                                                                                                               , existingRequrmntCategory, itemContract);
                                    existingRequrmntCategory.RequirementCategoryItems.Add(requrmntCategoryItem);
                                }
                                else
                                {
                                    RequirementItem existingRequrmntItem = existingRequrmntCategory.RequirementCategoryItems
                                                                                                   .Where(cond => cond.RCI_ID == itemContract.RequirementCategoryItemID && !cond.RCI_IsDeleted)
                                                                                                   .Select(col => col.RequirementItem).FirstOrDefault();
                                    //if item is updated
                                    if (itemContract.IsUpdated)
                                    {
                                        existingRequrmntItem.RI_ItemName = itemContract.RequirementItemName;
                                        existingRequrmntItem.RI_Description = itemContract.ExplanatoryNotes;
                                        //existingRequrmntItem.RI_ItemLabel = itemContract.RequirementItemLabel;
                                        //UAT-3077
                                        existingRequrmntItem.RI_IsPaymentType = itemContract.IsPaymentType;
                                        existingRequrmntItem.RI_Amount = itemContract.Amount;

                                        existingRequrmntItem.RI_ModifiedByID = currentLoggedInUserID;
                                        existingRequrmntItem.RI_ModifiedOn = DateTime.Now;
                                        break;
                                    }
                                    else
                                    {
                                        foreach (RequirementFieldContract fieldContract in itemContract.LstRequirementField)
                                        {
                                            //if field is newly added
                                            if (fieldContract.IsNewField)
                                            {
                                                //code to add fields and its data into database
                                                RequirementItemField requrmntItemField = GetRequirementItemField(currentLoggedInUserID, requirementFieldContractList
                                                                                            , existingRequrmntItem, lstLkpDocumentAcroFieldType, requirementFieldViewDocumentTypeID
                                                                                            , lstLkpRequirementFieldDataType, fieldContract);
                                                existingRequrmntItem.RequirementItemFields.Add(requrmntItemField);
                                            }
                                            else
                                            {
                                                requirementFieldContractList.Add(fieldContract);
                                                RequirementField existingReqrmntField = existingRequrmntItem.RequirementItemFields
                                                                                                         .Where(cond => cond.RIF_ID == fieldContract.RequirementItemFieldID && !cond.RIF_IsDeleted)
                                                                                                          .Select(col => col.RequirementField)
                                                                                                          .FirstOrDefault();
                                                if (fieldContract.IsUpdated)
                                                {
                                                    //code to update field and field data table object
                                                    existingReqrmntField.RF_FieldName = fieldContract.RequirementFieldName;
                                                    //existingReqrmntField.RF_FieldLabel = fieldContract.RequirementFieldLabel;
                                                    //existingReqrmntField.RF_Description = fieldContract.RequirementFieldDescription;
                                                    existingReqrmntField.RF_ModifiedByID = currentLoggedInUserID;
                                                    existingReqrmntField.RF_ModifiedOn = DateTime.Now;
                                                    if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
                                                    {
                                                        UpdateRequirementFieldVideo(currentLoggedInUserID, fieldContract, existingReqrmntField);
                                                    }
                                                    else if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.OPTIONS.GetStringValue())
                                                    {
                                                        UpdateRequirementFieldOptions(currentLoggedInUserID, fieldContract, existingReqrmntField);
                                                    }
                                                    else if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue()
                                                        && fieldContract.RequirementFieldData.FieldViewDocumentData.IsDocumentUpdated)
                                                    {
                                                        UpdateRequirementFieldDocumentData(currentLoggedInUserID, requirementFieldViewDocumentTypeID, lstLkpDocumentAcroFieldType, fieldContract, existingReqrmntField);
                                                    }
                                                    //UAT-2164
                                                    else if (fieldContract.RequirementFieldData.RequirementFieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue())
                                                    {
                                                        RequirementItemField existingReqrmntItemFieldMapping = existingReqrmntField.RequirementItemFields.Where(x => x.RIF_RequirementFieldID == existingReqrmntField.RF_ID && !x.RIF_IsDeleted).FirstOrDefault();
                                                        if (existingReqrmntItemFieldMapping.IsNotNull())
                                                        {
                                                            existingReqrmntItemFieldMapping.RIF_IsBackgroundDocument = fieldContract.IsBackgroundDocument;
                                                            existingReqrmntItemFieldMapping.RIF_ModifiedByID = currentLoggedInUserID;
                                                            existingReqrmntItemFieldMapping.RIF_ModifiedOn = DateTime.Now;
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).SaveContextIntoDataBase();

                #region Add category explanatory notes

                List<LargeContent> largeContentList = new List<LargeContent>();

                foreach (RequirementCategoryContract categoryContract in requirementPackageContract.LstRequirementCategory.Where(cond => cond.IsNewCategory
                                                                            && !cond.ExplanatoryNotes.IsNullOrEmpty()))
                {
                    RequirementCategory addedCategory = existingRequirementPackage.RequirementPackageCategories
                                                .Where(cond => cond.RequirementCategory.RC_Code == categoryContract.RequirementCategoryCode)
                                                .Select(col => col.RequirementCategory).FirstOrDefault();

                    LargeContent largeContent = CreateLargeContentForReqrmntCategory(addedCategory.RC_ID, categoryContract.ExplanatoryNotes, currentLoggedInUserID, selectedTenantID);
                    largeContentList.Add(largeContent);

                }

                if (!largeContentList.IsNullOrEmpty())
                {
                    BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).AddLargeContentToDatabase(largeContentList);
                }

                #endregion
            }

            if (existingRequirementPackage.RP_ID > AppConsts.NONE)
            {
                InsertDataIntoRequirementObjectTree(requirementPackageContract, selectedTenantID, currentLoggedInUserID, existingRequirementPackage, requirementFieldContractList, requirementItemContractList, false);
            }
            return existingRequirementPackage.RP_ID;
        }

        private static void UpdateReqrmntPackageAgency(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID, RequirementPackage existingRequirementPackage)
        {
            IEnumerable<RequirementPackageAgency> existingRequirementPackageAgencies = existingRequirementPackage.RequirementPackageAgencies
                                                                                                                 .Where(cond => !cond.RPA_IsDeleted);
            foreach (RequirementPackageAgency existingRequirementPackageAgency in existingRequirementPackageAgencies)
            {
                if (!requirementPackageContract.LstAgencyIDs.Contains(existingRequirementPackageAgency.RPA_AgencyID))
                {
                    existingRequirementPackageAgency.RPA_IsDeleted = true;
                    existingRequirementPackageAgency.RPA_ModifiedByID = currentLoggedInUserID;
                    existingRequirementPackageAgency.RPA_ModifiedOn = DateTime.Now;
                }
                else
                {
                    requirementPackageContract.LstAgencyIDs.Remove(existingRequirementPackageAgency.RPA_AgencyID);
                }
            }

            foreach (Int32 agencyID in requirementPackageContract.LstAgencyIDs)
            {
                RequirementPackageAgency newRequirementPackageAgency = new RequirementPackageAgency()
                {
                    RPA_AgencyID = agencyID,
                    RPA_IsDeleted = false,
                    RPA_CreatedByID = currentLoggedInUserID,
                    RPA_CreatedOn = DateTime.Now,
                };
                existingRequirementPackage.RequirementPackageAgencies.Add(newRequirementPackageAgency);
            }
        }

        private static void UpdateRequirementFieldDocumentData(Int32 currentLoggedInUserID, Int32 requirementFieldViewDocumentTypeID, List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType, RequirementFieldContract fieldContract, RequirementField existingReqrmntField)
        {
            existingReqrmntField.RequirementFieldDocuments.ForEach(cond =>
            {
                cond.RFD_IsDeleted = true;
                cond.RFD_ModifiedByID = currentLoggedInUserID;
                cond.RFD_ModifiedOn = DateTime.Now;
                cond.RequirementDocumentAcroFields.ForEach(raf =>
                {
                    raf.RDAF_IsDeleted = true;
                    raf.RDAF_ModifiedByID = currentLoggedInUserID;
                    raf.RDAF_ModifiedOn = DateTime.Now;

                });
                cond.ClientSystemDocument.CSD_IsDeleted = true;
                cond.RFD_ModifiedByID = currentLoggedInUserID;
                cond.RFD_ModifiedOn = DateTime.Now;
            });

            RequirementFieldDocument requrmntFieldDocument = CreateRequirementFieldDocumentType(currentLoggedInUserID, requirementFieldViewDocumentTypeID
                                                            , lstLkpDocumentAcroFieldType, fieldContract);
            existingReqrmntField.RequirementFieldDocuments.Add(requrmntFieldDocument);
        }

        private static void UpdateRequirementFieldVideo(Int32 currentLoggedInUserID, RequirementFieldContract fieldContract, RequirementField existingReqrmntField)
        {
            RequirementFieldVideo existingRequirementFieldVideo = existingReqrmntField.RequirementFieldVideos
                                                                     .Where(cond => !cond.RFV_IsDeleted)
                                                                    .FirstOrDefault();
            existingRequirementFieldVideo.RFV_VideoURL = fieldContract.RequirementFieldData.VideoFieldData.VideoURL;
            existingRequirementFieldVideo.RFV_VideoName = fieldContract.RequirementFieldData.VideoFieldData.VideoName;
            existingRequirementFieldVideo.RFV_ModifiedByID = currentLoggedInUserID;
            existingRequirementFieldVideo.RFV_ModifiedOn = DateTime.Now;
        }

        private static void UpdateRequirementFieldOptions(Int32 currentLoggedInUserID, RequirementFieldContract fieldContract, RequirementField existingReqrmntField)
        {
            List<RequirementFieldOptionsData> lstUpdatedOptionDataContract = fieldContract.RequirementFieldData.LstRequirementFieldOptions;
            List<RequirementFieldOption> lstFieldOptionsInDb = existingReqrmntField.RequirementFieldOptions.Where(cond => !cond.RFO_IsDeleted).ToList();
            foreach (RequirementFieldOption fieldOptionInDb in lstFieldOptionsInDb)
            {
                if (lstUpdatedOptionDataContract.Any(x => x.OptionText == fieldOptionInDb.RFO_OptionText
                    && x.OptionValue == fieldOptionInDb.RFO_OptionValue))
                    continue;

                fieldOptionInDb.RFO_IsDeleted = true;
                fieldOptionInDb.RFO_ModifiedOn = DateTime.Now;
                fieldOptionInDb.RFO_ModifiedByID = currentLoggedInUserID;
            }

            foreach (RequirementFieldOptionsData optionContract in lstUpdatedOptionDataContract)
            {
                if (lstFieldOptionsInDb.Any(x => x.RFO_OptionText == optionContract.OptionText
                    && x.RFO_OptionValue == optionContract.OptionValue))
                    continue;

                existingReqrmntField.RequirementFieldOptions.Add(new RequirementFieldOption()
                {
                    RFO_CreatedByID = currentLoggedInUserID,
                    RFO_CreatedOn = DateTime.Now,
                    RFO_IsDeleted = false,
                    RFO_OptionText = optionContract.OptionText,
                    RFO_OptionValue = optionContract.OptionValue,

                });
            }
        }

        private static Boolean UpdateLargeContentForReqrmntCategory(Int32 reqrmntCategoryID, String explanatoryNotes,
                                                                            Int32 currentLoggedInUserID, Int32 selectedTenantID)
        {
            String explanatoryNotesContentTypeCode = LCContentType.ExplanatoryNotes.GetStringValue();
            String reqrmnCategoryObjectTypeCode = LCObjectType.RequirementCategory.GetStringValue();
            Int32 contentTypeId = LookupManager.GetLookUpData<lkpLargeContentType>(selectedTenantID)
                                            .Where(obj => obj.LCT_Code == explanatoryNotesContentTypeCode && !obj.LCT_IsDeleted).FirstOrDefault().LCT_ID;
            Int32 objectTypeId = LookupManager.GetLookUpData<lkpObjectType>(selectedTenantID)
                                            .Where(obj => obj.OT_Code == reqrmnCategoryObjectTypeCode && obj.OT_IsDeleted == false).FirstOrDefault().OT_ID;
            LargeContent existingLargeContent = BALUtils.GetRequirementPackageRepoInstance(selectedTenantID)
                                                        .GetLargeContentForReqrmntCategory(reqrmntCategoryID, objectTypeId, contentTypeId);
            if (!existingLargeContent.IsNullOrEmpty())
            {
                existingLargeContent.LC_Content = explanatoryNotes;
                existingLargeContent.LC_ModifiedByID = currentLoggedInUserID;
                existingLargeContent.LC_ModifiedOn = DateTime.Now;
            }
            else if (!explanatoryNotes.IsNullOrEmpty())
            {
                LargeContent largeContent = new LargeContent()
                {
                    LC_ObjectID = reqrmntCategoryID,
                    LC_ObjectTypeID = objectTypeId,
                    LC_LargeContentTypeID = contentTypeId,
                    LC_Content = explanatoryNotes,
                    LC_IsDeleted = false,
                    LC_CreatedByID = currentLoggedInUserID,
                    LC_CreatedOn = DateTime.Now,
                };
                BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).AddLargeContentToContext(largeContent);
            }
            return true;
        }

        private static void CreateRequirementPackage(RequirementPackageContract requirementPackageContract, Int32 selectedTenantID, Int32 currentLoggedInUserID
                                                   , Boolean isPackageVersionNeedToCreate, RequirementPackage requirementPackage)
        {
            //set package type to "roatation package type"
            //String rotationPackageTypeCode = RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue();
            //requirementPackage.RP_RequirementPackageTypeID = LookupManager.GetLookUpData<lkpRequirementPackageType>(selectedTenantID)
            //                                                    .Where(col => col.RPT_Code == rotationPackageTypeCode).FirstOrDefault().RPT_ID;

            //UAT 1352:As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use

            requirementPackage.RP_RequirementPackageTypeID = requirementPackageContract.RequirementPkgTypeID;
            requirementPackage.RP_PackageName = requirementPackageContract.RequirementPackageName;

            requirementPackage.RP_PackageLabel = requirementPackageContract.RequirementPackageLabel;
            requirementPackage.RP_IsNewPackage = requirementPackageContract.IsNewPackage;
            requirementPackage.RP_IsArchived = requirementPackageContract.IsArchivedPackage;
            requirementPackage.RP_EffectiveStartDate = requirementPackageContract.EffectiveStartDate;
            requirementPackage.RP_EffectiveEndDate = requirementPackageContract.EffectiveEndDate;

            requirementPackage.RP_IsDeleted = false;
            requirementPackage.RP_IsActive = true;
            requirementPackage.RP_IsUsed = null;
            requirementPackage.RP_DefinedRequirementID = requirementPackageContract.DefinedRequirementID;
            requirementPackage.RP_ReqReviewByID = requirementPackageContract.ReqReviewByID;
            requirementPackage.RP_ParentPackageCode = requirementPackageContract.ParentPackageCode;//UAT-4657

            if (requirementPackageContract.IsSharedUserPackage)
            {
                requirementPackage.RP_IsCopied = true;
                requirementPackage.RP_Code = requirementPackageContract.RequirementPackageCode;
            }
            else
            {
                if (isPackageVersionNeedToCreate)
                {
                    requirementPackage.RP_Code = Guid.NewGuid();
                    requirementPackage.RP_FirstVersionID = requirementPackageContract.RequirementPackageID;
                    //code to update RP_IsActive column of existing package to false
                    SetExistingPackageIsActiveToFalse(selectedTenantID, currentLoggedInUserID, requirementPackageContract.RequirementPackageID);
                }
                else
                {
                    requirementPackage.RP_Code = requirementPackageContract.RequirementPackageCode;
                }
                requirementPackage.RP_IsCopied = false;
            }
            requirementPackage.RP_CreatedByID = currentLoggedInUserID;
            requirementPackage.RP_CreatedOn = DateTime.Now;
        }





        private static void CreateRqrmntPkgAgency(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID, RequirementPackage requirementPackage)
        {
            foreach (Int32 agencyID in requirementPackageContract.LstAgencyIDs)
            {
                RequirementPackageAgency requirementPackageAgency = new RequirementPackageAgency()
                {
                    RPA_AgencyID = agencyID,
                    RPA_IsDeleted = false,
                    RPA_CreatedByID = currentLoggedInUserID,
                    RPA_CreatedOn = DateTime.Now,
                };
                requirementPackage.RequirementPackageAgencies.Add(requirementPackageAgency);
            }
        }

        private static RequirementCategory CreateRequrmntCategory(Int32 currentLoggedInUserID, RequirementCategoryContract categoryContract)
        {
            RequirementCategory requirementCategory = new RequirementCategory();
            requirementCategory.RC_CategoryName = categoryContract.RequirementCategoryName;

            requirementCategory.RC_CategoryLabel = categoryContract.RequirementCategoryLabel;

            //if (!categoryContract.RequirementCategoryDescription.IsNullOrEmpty())
            //{
            //    requirementCategory.RC_Description = categoryContract.RequirementCategoryDescription;
            //}
            requirementCategory.RC_Code = categoryContract.RequirementCategoryCode;
            requirementCategory.RC_CreatedByID = currentLoggedInUserID;
            requirementCategory.RC_CreatedOn = DateTime.Now;
            requirementCategory.RC_IsDeleted = false;
            //UAT-2213
            requirementCategory.RC_ComplianceRqdStartDate = categoryContract.ComplianceReqStartDate;
            requirementCategory.RC_ComplianceRqdEndDate = categoryContract.ComplianceReqEndDate;
            requirementCategory.RC_IsNewCategory = categoryContract.IsNewCategory;
            //requirementCategory.RC_CategoryLabel = categoryContract.CategoryLabel;
            //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes)
            if (!categoryContract.RequirementDocumentLink.IsNullOrEmpty())
            {
                requirementCategory.RC_SampleDocFormURL = categoryContract.RequirementDocumentLink;
            }
            else
            {
                requirementCategory.RC_SampleDocFormURL = null;
            }
            requirementCategory.RC_AllowDataMovement = categoryContract.AllowDataMovement;
            //UAT-3161:- Make "More information" category field label editable by category.
            requirementCategory.RC_SampleDocFormUrlLabel = categoryContract.RequirementDocumentLinkLabel;
            //UAT-3805
            requirementCategory.RC_SendItemDocOnApproval = categoryContract.SendItemDoconApproval;

            #region UAT-4254
            List<RequirementCategoryDocLink> lstCatUrls = CreateReqCatDocUrls(currentLoggedInUserID, categoryContract.lstReqCatDocUrls);
            if (!lstCatUrls.IsNullOrEmpty() && lstCatUrls.Count > AppConsts.NONE)
                lstCatUrls.ForEach(docUrl => { requirementCategory.RequirementCategoryDocLinks.Add(docUrl); });
            //requirementCategory.RequirementCategoryDocLinks.AddRange(lstCatUrls);
            #endregion

            return requirementCategory;
        }


        private static List<RequirementItemURL> CreateRequirementItemURL(int currentLoggedInUserID, List<RequirementItemURLContract> requirementItemURLContract)
        {
            List<RequirementItemURL> listRequirementItemURL = new List<RequirementItemURL>();
            if (requirementItemURLContract != null && requirementItemURLContract.Count > AppConsts.NONE)
            {
                foreach (RequirementItemURLContract item in requirementItemURLContract)
                {
                    RequirementItemURL requirementURL = new RequirementItemURL();
                    requirementURL.RIU_Label = item.RItemURLLabel;
                    requirementURL.RIU_SampleDocURL = item.RItemURLSampleDocURL;
                    requirementURL.RIU_CreatedOn = DateTime.Now;
                    requirementURL.RIU_CreatedById = currentLoggedInUserID;
                    requirementURL.RIU_IsDeleted = false;
                    listRequirementItemURL.Add(requirementURL);
                }

            }
            return listRequirementItemURL;
        }
        private static RequirementItem CreateRequirementItem(Int32 currentLoggedInUserID, RequirementItemContract itemContract)
        {
            RequirementItem requirementItem = new RequirementItem();
            requirementItem.RI_ItemName = itemContract.RequirementItemName;
            //if (!itemContract.RequirementItemLabel.IsNullOrEmpty())
            //{
            //    requirementItem.RI_ItemLabel = itemContract.RequirementItemLabel;
            //}
            //if (!itemContract.RequirementItemDescription.IsNullOrEmpty())
            //{
            //    requirementItem.RI_Description = itemContract.RequirementItemDescription;
            //}

            requirementItem.RI_Description = itemContract.ExplanatoryNotes; //UAT-2676
            //UAT-2213
            requirementItem.RI_ItemLabel = itemContract.RequirementItemLabel;
            requirementItem.RI_Code = itemContract.RequirementItemCode;
            requirementItem.RI_CreatedByID = currentLoggedInUserID;
            requirementItem.RI_CreatedOn = DateTime.Now;
            requirementItem.RI_IsDeleted = false;
            requirementItem.RI_AllowDataMovement = itemContract.AllowDataMovement;
            //UAT-3077
            requirementItem.RI_IsPaymentType = itemContract.IsPaymentType;
            requirementItem.RI_Amount = itemContract.Amount;
            requirementItem.RI_SampleDocFormURL = itemContract.RequirementItemSampleDocumentFormURL;//UAT-3309
            return requirementItem;
        }

        private static RequirementFieldVideo CreateReqirmntFieldVideo(Int32 currentLoggedInUserID, RequirementFieldContract fieldContract)
        {
            RequirementFieldVideo fieldVideo = new RequirementFieldVideo();
            //fieldVideo.RFV_IsVideoOpenRequired = fieldContract.RequirementFieldData.VideoFieldData.IsVideoRequiredToBeOpened;
            fieldVideo.RFV_VideoName = fieldContract.RequirementFieldData.VideoFieldData.VideoName;
            fieldVideo.RFV_VideoURL = fieldContract.RequirementFieldData.VideoFieldData.VideoURL;
            //if (fieldContract.RequirementFieldData.VideoFieldData.IsVideoRequiredToBeOpened)
            //{
            //    Int32 openMinutes = fieldContract.RequirementFieldData.VideoFieldData.VideoOpenedMinutes.IsNullOrEmpty() ? 0 : fieldContract.RequirementFieldData.VideoFieldData.VideoOpenedMinutes;
            //    openMinutes += fieldContract.RequirementFieldData.VideoFieldData.VideoOpenedHours.IsNullOrEmpty() ? 0 : fieldContract.RequirementFieldData.VideoFieldData.VideoOpenedHours * 60;
            //    fieldVideo.RFV_VideoOpenTimeDuration = openMinutes;
            //}
            fieldVideo.RFV_IsDeleted = false;
            fieldVideo.RFV_CreatedByID = currentLoggedInUserID;
            fieldVideo.RFV_CreatedOn = DateTime.Now;

            return fieldVideo;
        }

        private static void AddReqrmntFieldOptionsForReqrmntField(Int32 currentLoggedInUserID, RequirementFieldContract fieldContract
                                                                                          , RequirementField reqirmntField)
        {
            if (fieldContract.RequirementFieldData.IsNotNull())
            {
                foreach (RequirementFieldOptionsData optionData in fieldContract.RequirementFieldData.LstRequirementFieldOptions)
                {
                    RequirementFieldOption requirementFieldOption = CreateReqrmntFieldOptions(currentLoggedInUserID, optionData);
                    reqirmntField.RequirementFieldOptions.Add(requirementFieldOption);
                }
            }
        }

        private static RequirementFieldOption CreateReqrmntFieldOptions(Int32 currentLoggedInUserID, RequirementFieldOptionsData optionData)
        {
            RequirementFieldOption requirementFieldOption = new RequirementFieldOption()
            {
                RFO_OptionText = optionData.OptionText,
                RFO_OptionValue = optionData.OptionValue,
                RFO_CreatedByID = currentLoggedInUserID,
                RFO_CreatedOn = DateTime.Now,
                RFO_IsDeleted = false,
            };

            return requirementFieldOption;
        }

        private static RequirementFieldDocument CreateRequirementFieldDocumentType(Int32 currentLoggedInUserID, Int32 requirementFieldViewDocumentTypeID
                                                                                   , List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType
                                                                                   , RequirementFieldContract fieldContract)
        {
            RequirementFieldDocument requirementFieldDocument = CreateRequrmntFieldDocument(currentLoggedInUserID);

            ClientSystemDocument clientSystemDocument = CreateClientSysDocument(currentLoggedInUserID, requirementFieldViewDocumentTypeID, fieldContract);

            requirementFieldDocument.ClientSystemDocument = clientSystemDocument;

            foreach (String acroFieldTypeCode in fieldContract.RequirementFieldData.FieldViewDocumentData.LstDocumentAcroFieldTypeCodes)
            {
                RequirementDocumentAcroField requirementDocumentAcroField = CreateRequrmntDocumentAcroField(currentLoggedInUserID, lstLkpDocumentAcroFieldType, acroFieldTypeCode);
                requirementFieldDocument.RequirementDocumentAcroFields.Add(requirementDocumentAcroField);
            }
            return requirementFieldDocument;
        }

        private static RequirementDocumentAcroField CreateRequrmntDocumentAcroField(Int32 currentLoggedInUserID, List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType, String acroFieldTypeCode)
        {
            RequirementDocumentAcroField requirementDocumentAcroField = new RequirementDocumentAcroField()
            {
                RDAF_IsDeleted = false,
                RDAF_CreatedByID = currentLoggedInUserID,
                RDAF_CreatedOn = DateTime.Now,
                RDAF_DocumentAcroFieldTypeID = lstLkpDocumentAcroFieldType
                                              .Where(cond => cond.DAFT_Code == acroFieldTypeCode).FirstOrDefault().DAFT_ID

            };
            return requirementDocumentAcroField;
        }

        private static RequirementFieldDocument CreateRequrmntFieldDocument(Int32 currentLoggedInUserID)
        {
            RequirementFieldDocument requirementFieldDocument = new RequirementFieldDocument()
            {
                RFD_IsDeleted = false,
                RFD_CreatedByID = currentLoggedInUserID,
                RFD_CreatedOn = DateTime.Now
            };
            return requirementFieldDocument;
        }

        private static ClientSystemDocument CreateClientSysDocument(Int32 currentLoggedInUserID, Int32 requirementFieldViewDocumentTypeID, RequirementFieldContract fieldContract)
        {
            ClientSystemDocument clientSystemDocument = new ClientSystemDocument()
            {
                CSD_CreatedByID = currentLoggedInUserID,
                CSD_CreatedOn = DateTime.Now,
                CSD_FileName = fieldContract.RequirementFieldData.FieldViewDocumentData.DocumentFileName,
                CSD_DocumentPath = fieldContract.RequirementFieldData.FieldViewDocumentData.DocumentPath,
                CSD_Size = fieldContract.RequirementFieldData.FieldViewDocumentData.DocumentSize,
                CSD_DocumentTypeID = requirementFieldViewDocumentTypeID,
                CSD_IsDeleted = false
            };
            return clientSystemDocument;
        }

        /// <summary>
        /// UAT-2164,AgencyUserGranularPermission.  Added IsBackgroundDocument parameter.
        /// </summary>
        /// <param name="currentLoggedInUserID"></param>
        /// <param name="requirementItem"></param>
        /// <param name="requirementField"></param>
        /// <param name="isBackgroundDocument"></param>
        /// <returns></returns>
        private static RequirementItemField CreateRequirementItemField(Int32 currentLoggedInUserID, RequirementItem requirementItem, RequirementField requirementField, Boolean isBackgroundDocument, Int32 requirementItemFieldDisplayOrder)
        {
            RequirementItemField requrmntItemField = new RequirementItemField();
            requrmntItemField.RIF_CreatedByID = currentLoggedInUserID;
            requrmntItemField.RIF_CreatedOn = DateTime.Now;
            requrmntItemField.RIF_IsDeleted = false;
            requrmntItemField.RequirementItem = requirementItem;
            requrmntItemField.RequirementField = requirementField;
            requrmntItemField.RIF_DisplayOrder = requirementItemFieldDisplayOrder; //UAT-3078

            //UAT-2164 
            requrmntItemField.RIF_IsBackgroundDocument = isBackgroundDocument;
            return requrmntItemField;
        }
        //UAT:4121 
        private static void CreateRequrmntItemURL(RequirementItem requirementitem, Int32 currentLoggedInUserID, List<RequirementItemURL> ListrequirementItemURL)
        {

            foreach (var item in ListrequirementItemURL)
            {
                requirementitem.RequirementItemURLs.Add(item);
            }


        }

        private static RequirementCategoryItem CreateRequrmntCategoryItem(Int32 currentLoggedInUserID, RequirementCategory requirementCategory
                                                                                 , RequirementItem requirementItem, Int32? requirementCategoryItemDisplayOrder)
        {
            RequirementCategoryItem requrmntCategoryItem = new RequirementCategoryItem();
            requrmntCategoryItem.RCI_CreatedByID = currentLoggedInUserID;
            requrmntCategoryItem.RCI_CreatedOn = DateTime.Now;
            requrmntCategoryItem.RCI_IsDeleted = false;
            requrmntCategoryItem.RequirementCategory = requirementCategory;
            requrmntCategoryItem.RequirementItem = requirementItem;
            requrmntCategoryItem.RCI_DisplayOrder = requirementCategoryItemDisplayOrder;
            return requrmntCategoryItem;
        }


        private static RequirementObjectProperty CreateRequrmntObjectProperties(Int32 currentLoggedInUserID, RequirementObjectPropertiesContract requirementObjectPropertyContract, RequirementCategory requirementCategory, RequirementCategoryItem requirementCategoryItem)
        {
            RequirementObjectProperty requrmntObjProp = new RequirementObjectProperty();
            requrmntObjProp.ROTP_CreatedBy = currentLoggedInUserID;
            requrmntObjProp.ROTP_CreatedOn = DateTime.Now;
            requrmntObjProp.ROTP_IsDeleted = false;
            requrmntObjProp.RequirementCategory = requirementCategory;
            requrmntObjProp.RequirementCategoryItem = requirementCategoryItem;
            requrmntObjProp.ROTP_IsCustomSettings = requirementObjectPropertyContract.RequirementObjPropIsCustomSettings;
            requrmntObjProp.ROTP_IsEditableByAdmin = requirementObjectPropertyContract.RequirementObjPropIsEditableByAdmin;
            requrmntObjProp.ROTP_IsEditableByApplicant = requirementObjectPropertyContract.RequirementObjPropIsEditableByApplicant;
            requrmntObjProp.ROTP_IsEditableByClientAdmin = requirementObjectPropertyContract.RequirementObjPropIsEditableByClientAdmin;
            return requrmntObjProp;
        }
        private static RequirementObjectProperty CreateRequrmntObjectProperties(Int32 currentLoggedInUserID, RequirementObjectPropertiesContract requirementObjectPropertyContract, RequirementCategory requirementCategory, RequirementCategoryItem requirementCategoryItem, RequirementItemField requirementItemField)
        {
            RequirementObjectProperty requrmntObjProp = new RequirementObjectProperty();
            requrmntObjProp.ROTP_CreatedBy = currentLoggedInUserID;
            requrmntObjProp.ROTP_CreatedOn = DateTime.Now;
            requrmntObjProp.ROTP_IsDeleted = false;
            requrmntObjProp.RequirementCategory = requirementCategory;
            requrmntObjProp.RequirementCategoryItem = requirementCategoryItem;
            requrmntObjProp.RequirementItemField = requirementItemField;
            requrmntObjProp.ROTP_IsCustomSettings = requirementObjectPropertyContract.RequirementObjPropIsCustomSettings;
            requrmntObjProp.ROTP_IsEditableByAdmin = requirementObjectPropertyContract.RequirementObjPropIsEditableByAdmin;
            requrmntObjProp.ROTP_IsEditableByApplicant = requirementObjectPropertyContract.RequirementObjPropIsEditableByApplicant;
            requrmntObjProp.ROTP_IsEditableByClientAdmin = requirementObjectPropertyContract.RequirementObjPropIsEditableByClientAdmin;
            return requrmntObjProp;
        }

        private static RequirementCategoryItem CreateRequrmntCategoryItemNew(Int32 currentLoggedInUserID, RequirementCategory requirementCategory
                                                                                 , RequirementItem requirementItem, Int32 requirementItemID)
        {
            RequirementCategoryItem requrmntCategoryItem = new RequirementCategoryItem();
            requrmntCategoryItem.RCI_CreatedByID = currentLoggedInUserID;
            requrmntCategoryItem.RCI_CreatedOn = DateTime.Now;
            requrmntCategoryItem.RCI_IsDeleted = false;
            requrmntCategoryItem.RequirementCategory = requirementCategory;
            if (requirementItemID > AppConsts.NONE)
            {
                requrmntCategoryItem.RCI_RequirementItemID = requirementItemID;
            }
            else
            {
                requrmntCategoryItem.RequirementItem = requirementItem;
            }
            return requrmntCategoryItem;
        }
        private static RequirementPackageCategory CreateRequrmntPackageCategory(Int32 currentLoggedInUserID, RequirementPackage requirementPackage
                                                                                      , RequirementCategory requirementCategory)
        {
            RequirementPackageCategory reqrmntPackageCategory = new RequirementPackageCategory();
            reqrmntPackageCategory.RPC_CreatedByID = currentLoggedInUserID;
            reqrmntPackageCategory.RPC_CreatedOn = DateTime.Now;
            reqrmntPackageCategory.RPC_IsDeleted = false;
            reqrmntPackageCategory.RequirementPackage = requirementPackage;
            reqrmntPackageCategory.RequirementCategory = requirementCategory;
            return reqrmntPackageCategory;
        }

        private static LargeContent CreateLargeContentForReqrmntCategory(Int32 reqrmntCategoryID, String explanatoryNotes,
                                                                            Int32 currentLoggedInUserID, Int32 selectedTenantID)
        {
            String explanatoryNotesContentTypeCode = LCContentType.ExplanatoryNotes.GetStringValue();
            String reqrmnCategoryObjectTypeCode = LCObjectType.RequirementCategory.GetStringValue();
            Int32 contentTypeId = LookupManager.GetLookUpData<lkpLargeContentType>(selectedTenantID)
                                            .Where(obj => obj.LCT_Code == explanatoryNotesContentTypeCode && !obj.LCT_IsDeleted).FirstOrDefault().LCT_ID;
            Int32 objectTypeId = LookupManager.GetLookUpData<lkpObjectType>(selectedTenantID)
                                            .Where(obj => obj.OT_Code == reqrmnCategoryObjectTypeCode && obj.OT_IsDeleted == false).FirstOrDefault().OT_ID;
            return new LargeContent()
            {
                LC_ObjectID = reqrmntCategoryID,
                LC_ObjectTypeID = objectTypeId,
                LC_LargeContentTypeID = contentTypeId,
                LC_Content = explanatoryNotes,
                LC_IsDeleted = false,
                LC_CreatedByID = currentLoggedInUserID,
                LC_CreatedOn = DateTime.Now,
            };

        }

        private static void SetExistingPackageIsActiveToFalse(Int32 selectedTenantID, Int32 currentLoggedInUserID, Int32 requirementPackageID)
        {
            //Get existing requirement package(from DB) which is to be updated 
            RequirementPackage existingRequirementPackage = BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).GetRequirementPackageByPackageID(requirementPackageID);
            if (!existingRequirementPackage.IsNullOrEmpty())
            {
                existingRequirementPackage.RP_IsActive = false;
                existingRequirementPackage.RP_ModifiedByID = currentLoggedInUserID;
                existingRequirementPackage.RP_ModifiedOn = DateTime.Now;
            }
        }


        private static void DeleteRequirementObjectTree(Int32 reqObjectTreeId, Int32 selectedTenantID, Int32 currentLoggedInUserID)
        {
            RequirementObjectTree existingRequirementObjectTree = BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).GetRequirementObjectTree(reqObjectTreeId);
            if (!existingRequirementObjectTree.IsNullOrEmpty())
            {
                RequirementObjectTreeProperty requirementObjectTreePropertyInDb = existingRequirementObjectTree.RequirementObjectTreeProperties.Where(cond => !cond.ROTP_IsDeleted)
                                                                                                                                            .FirstOrDefault();
                if (!requirementObjectTreePropertyInDb.IsNullOrEmpty())
                {
                    requirementObjectTreePropertyInDb.ROTP_IsDeleted = false;
                    requirementObjectTreePropertyInDb.ROTP_ModifiedByID = currentLoggedInUserID;
                    requirementObjectTreePropertyInDb.ROTP_ModifiedOn = DateTime.Now;
                }

                List<RequirementObjectRule> requirementObjectRuleListInDb = existingRequirementObjectTree.RequirementObjectRules.Where(cond => !cond.ROR_IsDeleted)
                                                                                                                                             .ToList();
                foreach (RequirementObjectRule requirementObjectRuleInDb in requirementObjectRuleListInDb)
                {
                    requirementObjectRuleInDb.ROR_IsDeleted = false;
                    requirementObjectRuleInDb.ROR_ModifiedByID = currentLoggedInUserID;
                    requirementObjectRuleInDb.ROR_ModifiedOn = DateTime.Now;
                    List<RequirementObjectRuleDetail> requirementObjectRuleDetailListInDb = requirementObjectRuleInDb.RequirementObjectRuleDetails.Where(cond => !cond.RORD_IsDeleted).ToList();
                    foreach (RequirementObjectRuleDetail requirementObjectRuleDetailInDb in requirementObjectRuleDetailListInDb)
                    {
                        requirementObjectRuleDetailInDb.RORD_IsDeleted = false;
                        requirementObjectRuleDetailInDb.RORD_ModifiedByID = currentLoggedInUserID;
                        requirementObjectRuleDetailInDb.RORD_ModifiedOn = DateTime.Now;
                    }
                }
            }
        }

        public static void SetExistingPackageIsCopiedToTrue(Int32 selectedTenantID, Int32 currentLoggedInUserID, Int32 requirementPackageID)
        {
            BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).SetExistingPackageIsCopiedToTrue(currentLoggedInUserID, requirementPackageID);
        }

        public static void SetExistingPackageIsDeletedToTrue(Int32 selectedTenantID, Int32 currentLoggedInUserID, Int32 requirementPackageID)
        {
            BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).SetExistingPackageIsDeletedToTrue(currentLoggedInUserID, requirementPackageID);
        }


        #endregion

        #endregion

        #region UAT 1352 As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use.
        public static List<RequirementPackageTypeContract> GetRequirementPackageType(Int32 tenantID)
        {
            try
            {
                List<lkpRequirementPackageType> dataTypeList = LookupManager.GetLookUpData<lkpRequirementPackageType>(tenantID).Where(x => !x.RPT_IsDeleted).ToList();
                return dataTypeList.Select(con => new RequirementPackageTypeContract
                {
                    ID = con.RPT_ID,
                    Code = con.RPT_Code,
                    Name = con.RPT_Name
                }).ToList();
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

        /// <summary>
        /// To get Requirement Packages
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="agencyId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        public static List<RequirementPackageContract> GetAllRequirementPackages(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetRequirementPackageRepoInstance(tenantId).GetAllRequirementPackages();
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

        #region UAT-1828
        private static void SendRotationPackageVersioningNotification(RequirementPackageContract RequirementPkgContract, Int32 tenantId)
        {
            //Create Dictionary
            Dictionary<String, object> dictMailData = new Dictionary<string, object>();
            dictMailData.Add(EmailFieldConstants.PACKAGE_NAME, RequirementPkgContract.RequirementPackageName);

            Entity.CommunicationMockUpData mockData = new Entity.CommunicationMockUpData();
            mockData.UserName = "Admin";
            mockData.EmailID = AppConsts.BACKGROUND_PROCESS_USER_EMAIL;
            mockData.ReceiverOrganizationUserID = AppConsts.BACKGROUND_PROCESS_USER_VALUE;

            //Send mail
            CommunicationManager.SendPackageNotificationMail(CommunicationSubEvents.ROTATION_PACKAGE_VERSIONING_NOTIFICATION, dictMailData, mockData, tenantId, -1, null, null, true);
        }
        #endregion

        public static List<RequirementPackageCategory> GetRequirementCategoriesRqdForComplianceAction(Int32 tenantId)
        {
            try
            {
                DataTable categoriesRqdForComplianceAction = BALUtils.GetRequirementPackageRepoInstance(tenantId).GetRequirementCategoriesRqdForComplianceAction();

                IEnumerable<DataRow> rows = categoriesRqdForComplianceAction.AsEnumerable();
                return rows.Select(col => new RequirementPackageCategory
                {
                    RPC_ID = col["RPC_ID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RPC_ID"]),
                    RPC_ComplianceRequired = col["RPC_ComplianceRequired"] == DBNull.Value ? false : Convert.ToBoolean(col["RPC_ComplianceRequired"]),
                    RPC_RequirementPackageID = col["RPC_RequirementPackageID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RPC_RequirementPackageID"]),
                    RPC_RequirementCategoryID = col["RPC_RequirementCategoryID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(col["RPC_RequirementCategoryID"])
                }).ToList();
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

        public static Dictionary<Int32, Boolean> ProcessRotcomplianceRqdChange(Int32 currentUserId, Int32 tenantId, RequirementPackageCategory requirementPackageCategory)
        {
            try
            {
                return BALUtils.GetRequirementPackageRepoInstance(tenantId).ProcessRotcomplianceRqdChange(currentUserId, requirementPackageCategory);
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


        #region UAT-2332
        public static List<DefinedRequirementContract> GetDefinedRequirement(Int32 tenantID)
        {
            try
            {
                List<lkpDefinedRequirement> dataTypeList = LookupManager.GetLookUpData<lkpDefinedRequirement>(tenantID).Where(x => !x.DR_IsDeleted && x.DR_IsActive).ToList();
                return dataTypeList.Select(con => new DefinedRequirementContract
                {
                    ID = con.DR_ID,
                    Code = con.DR_Code,
                    Description = con.DR_Description
                }).ToList();
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

        #region UAT-2514 Copy Package
        //Step 1
        /// <summary>
        /// Copy Package from Shared To Tenant
        /// </summary>
        /// <param name="requirementPackageContract"></param>
        /// <param name="selectedTenantID"></param>
        /// <param name="currentLoggedInUserID"></param>
        /// <returns></returns>
        public static Int32 CopySharedPackageToClientNew(RequirementPackageContract requirementPackageContract, Int32 selectedTenantID, Int32 currentLoggedInUserID)
        {
            RequirementPackage existingRequirementPackage = BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).GetRequirementPackageByPackageCode(requirementPackageContract.RequirementPackageCode);
            if (existingRequirementPackage.IsNullOrEmpty())
            {
                return SaveRequirementPackageNew(requirementPackageContract, selectedTenantID, currentLoggedInUserID);
            }
            List<Int32> lstAlreadyMappedAgencies = existingRequirementPackage.RequirementPackageAgencies.Where(con => !con.RPA_IsDeleted).Select(sel => sel.RPA_AgencyID).ToList();
            List<Int32> lstNewRequirementPkgAgencies = requirementPackageContract.LstAgencyIDs.ToList();

            List<Int32> lstAgenciesNeedTobeMapped = lstNewRequirementPkgAgencies.Except(lstAlreadyMappedAgencies).ToList();

            if (!lstAgenciesNeedTobeMapped.IsNullOrEmpty())//create mapping
            {
                UpdateRequirementPackageAgencyMappings(lstAgenciesNeedTobeMapped, currentLoggedInUserID, selectedTenantID, existingRequirementPackage.RP_ID);
            }
            return existingRequirementPackage.RP_ID;
        }
        //Step 2
        /// <summary>
        /// Saves Requirement Package
        /// </summary>
        /// <param name="requirementPackageContract"></param>
        /// <param name="selectedTenantID"></param>
        /// <param name="currentLoggedInUserID"></param>
        /// <returns></returns>
        public static Int32 SaveRequirementPackageNew(RequirementPackageContract requirementPackageContract, Int32 selectedTenantID, Int32 currentLoggedInUserID)
        {
            try
            {
                if (requirementPackageContract.IsSharedUserPackage)
                {
                    return AddNewRequirementPackageToClient(requirementPackageContract, selectedTenantID, currentLoggedInUserID, false);
                }
                //Else Scenario is working as was in case of Old Packages, No change in It
                else
                {
                    Boolean isNewPackage = requirementPackageContract.RequirementPackageID == AppConsts.NONE;
                    Boolean isPackageVersionNeedToCreate = IsPackageVersionNeedToCreate(requirementPackageContract, selectedTenantID);

                    if (isNewPackage || isPackageVersionNeedToCreate)
                    {
                        //UAT-1828
                        Int32 requirementPkgID = AddNewRequirementPackageToClient(requirementPackageContract, selectedTenantID, currentLoggedInUserID, isPackageVersionNeedToCreate);
                        if (isPackageVersionNeedToCreate)
                        {
                            SendRotationPackageVersioningNotification(requirementPackageContract, selectedTenantID);
                        }
                        return requirementPkgID;
                    }
                    else
                    {
                        //call method to update existing package package by passing contract to it
                        return UpdateRequirementPackage(requirementPackageContract, selectedTenantID, currentLoggedInUserID);
                    }
                }
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
        //Step 3
        /// <summary>
        /// Add complete package with all mappings and rules to tenant database
        /// </summary>
        /// <param name="sharedRequirementPackageContract"></param>
        /// <param name="selectedTenantID"></param>
        /// <param name="currentLoggedInUserID"></param>
        /// <param name="isPackageVersionNeedToCreate"></param>
        /// <returns></returns>
        private static Int32 AddNewRequirementPackageToClient(RequirementPackageContract sharedRequirementPackageContract, Int32 selectedTenantID
                                                  , Int32 currentLoggedInUserID, Boolean isPackageVersionNeedToCreate)
        {
            #region Add data into RequirementPackage table

            RequirementPackage tenantRequirementPackage = new RequirementPackage();
            CreateClientRequirementPackage(sharedRequirementPackageContract, selectedTenantID, currentLoggedInUserID, isPackageVersionNeedToCreate, tenantRequirementPackage);

            //Add entries in table RequirementPackageAgency. N rows will be inserted for N selected agencies
            CreateRqrmntPkgAgency(sharedRequirementPackageContract, currentLoggedInUserID, tenantRequirementPackage);

            #endregion

            #region Add data into mapping,detail tables table

            List<RequirementFieldContract> requirementFieldContractList = new List<RequirementFieldContract>();
            List<RequirementItemContract> requirementItemContractList = new List<RequirementItemContract>();
            //UAT:4121
            List<RequirementItemURL> requirementItemURL = new List<RequirementItemURL>();



            List<Int32> lstExistingCategoryIDs = new List<Int32>();
            foreach (RequirementCategoryContract categoryContract in sharedRequirementPackageContract.LstRequirementCategory.Where(cond => !cond.IsDeleted))
            {
                BindDataForRequirementItemURL(sharedRequirementPackageContract, categoryContract);

                RequirementCategory requirementCategory = GetRequirementCategoryIfAlreadyExists(selectedTenantID, categoryContract.RequirementCategoryCode);
                if (requirementCategory.IsNotNull())
                {
                    lstExistingCategoryIDs.Add(requirementCategory.RC_ID);
                }
                RequirementPackageCategory reqrmntPackageCategory = GetRequirementPackageCategoryNew(requirementCategory, selectedTenantID, currentLoggedInUserID, tenantRequirementPackage
                                                                                                 , requirementFieldContractList, requirementItemContractList
                                                                                                 , categoryContract);
                tenantRequirementPackage.RequirementPackageCategories.Add(reqrmntPackageCategory);

            }

            // SAVE RequirementPackageCategory AND RequirementPackage into DB
            BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).AddRequirementPackageToDatabase(tenantRequirementPackage);

            #endregion

            #region Add category explanatory notes

            if (tenantRequirementPackage.RP_ID > AppConsts.NONE)
            {
                List<RequirementCategory> addedRequirementCatgoriesList = tenantRequirementPackage.RequirementPackageCategories.Select(col => col.RequirementCategory).ToList();

                addedRequirementCatgoriesList = addedRequirementCatgoriesList.Where(con => !lstExistingCategoryIDs.Contains(con.RC_ID)).ToList();

                if (!addedRequirementCatgoriesList.IsNullOrEmpty() && addedRequirementCatgoriesList.Count() > AppConsts.NONE)
                {
                    List<LargeContent> largeContentList = new List<LargeContent>();

                    String reqrmnCategoryObjectTypeCode = LCObjectType.RequirementCategory.GetStringValue();
                    Int32 objectTypeId = LookupManager.GetLookUpData<lkpObjectType>(selectedTenantID).Where(obj => obj.OT_Code == reqrmnCategoryObjectTypeCode && obj.OT_IsDeleted == false).FirstOrDefault().OT_ID;


                    foreach (RequirementCategory requirementcategory in addedRequirementCatgoriesList)
                    {
                        String explanatoryNotes = sharedRequirementPackageContract.LstRequirementCategory
                                                    .Where(cond => cond.RequirementCategoryCode == requirementcategory.RC_Code).FirstOrDefault().ExplanatoryNotes;
                        if (!explanatoryNotes.IsNullOrEmpty())
                        {
                            LargeContent largeContent = CreateLargeContentForReqrmntCategory(requirementcategory.RC_ID, explanatoryNotes, currentLoggedInUserID, selectedTenantID);
                            largeContentList.Add(largeContent);
                        }
                    }

                    if (!largeContentList.IsNullOrEmpty())
                    {
                        BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).AddLargeContentToDatabase(largeContentList);
                    }
                }
            }

            #endregion
            //Entry in RequirementObjectTree and Rules Execution 
            if (tenantRequirementPackage.RP_ID > AppConsts.NONE)
            {
                InsertDataIntoRequirementObjectTreeNew(sharedRequirementPackageContract, selectedTenantID, currentLoggedInUserID, tenantRequirementPackage);

            }

            return tenantRequirementPackage.RP_ID;
        }

        private static void BindDataForRequirementItemURL(RequirementPackageContract sharedRequirementPackageContract, RequirementCategoryContract categoryContract)
        {
            categoryContract.RItemURLLabel = sharedRequirementPackageContract.RItemURLLabel;
            categoryContract.RItemURLSampleDocURL = sharedRequirementPackageContract.RItemURLSampleDocURL;
        }

        /// <summary>
        /// Creates Tenant Requirement Package
        /// </summary>
        /// <param name="requirementPackageContract"></param>
        /// <param name="selectedTenantID"></param>
        /// <param name="currentLoggedInUserID"></param>
        /// <param name="isPackageVersionNeedToCreate"></param>
        /// <param name="requirementPackage"></param>
        private static void CreateClientRequirementPackage(RequirementPackageContract requirementPackageContract, Int32 selectedTenantID, Int32 currentLoggedInUserID
                                                 , Boolean isPackageVersionNeedToCreate, RequirementPackage requirementPackage)
        {


            //UAT 1352:As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use

            requirementPackage.RP_RequirementPackageTypeID = requirementPackageContract.RequirementPkgTypeID;
            requirementPackage.RP_PackageName = requirementPackageContract.RequirementPackageName;
            if (!requirementPackageContract.RequirementPackageLabel.IsNullOrEmpty())
            {
                requirementPackage.RP_PackageLabel = requirementPackageContract.RequirementPackageLabel;
            }

            requirementPackage.RP_IsDeleted = false;
            requirementPackage.RP_IsActive = true;
            requirementPackage.RP_IsUsed = null;
            requirementPackage.RP_DefinedRequirementID = requirementPackageContract.DefinedRequirementID;
            requirementPackage.RP_ReqReviewByID = requirementPackageContract.ReqReviewByID;
            //Effective Date,IsNewPackage--SS
            requirementPackage.RP_EffectiveStartDate = requirementPackageContract.EffectiveStartDate;
            requirementPackage.RP_EffectiveEndDate = requirementPackageContract.EffectiveEndDate;
            requirementPackage.RP_IsNewPackage = requirementPackageContract.IsNewPackage;
            requirementPackage.RP_IsArchived = requirementPackageContract.IsArchivedPackage;
            requirementPackage.RP_ParentPackageCode = requirementPackageContract.ParentPackageCode;//UAT-4657

            if (requirementPackageContract.IsSharedUserPackage)
            {
                requirementPackage.RP_IsCopied = true;
                requirementPackage.RP_Code = requirementPackageContract.RequirementPackageCode;
            }
            //Package Version Scenario is working as was in case of Old Packages, No change in It
            else
            {
                if (isPackageVersionNeedToCreate)
                {
                    requirementPackage.RP_Code = Guid.NewGuid();
                    requirementPackage.RP_FirstVersionID = requirementPackageContract.RequirementPackageID;
                    //code to update RP_IsActive column of existing package to false
                    SetExistingPackageIsActiveToFalse(selectedTenantID, currentLoggedInUserID, requirementPackageContract.RequirementPackageID);
                }
                else
                {
                    requirementPackage.RP_Code = requirementPackageContract.RequirementPackageCode;
                }
                requirementPackage.RP_IsCopied = false;
            }
            requirementPackage.RP_CreatedByID = currentLoggedInUserID;
            requirementPackage.RP_CreatedOn = DateTime.Now;
        }
        //Step 4
        /// <summary>
        /// RequirementPackageCategory is returned that is having RequirementCategory and RequirementCategory is further having Mapping of category and item , further level of item and field mapping is also included.
        /// </summary>
        /// <param name="selectedTenantID"></param>
        /// <param name="currentLoggedInUserID"></param>
        /// <param name="requirementPackage"></param>
        /// <param name="requirementFieldContractList"></param>
        /// <param name="requirementItemContractList"></param>
        /// <param name="categoryContract"></param>
        /// <returns></returns>
        private static RequirementPackageCategory GetRequirementPackageCategoryNew(RequirementCategory requirementCategory, Int32 selectedTenantID, Int32 currentLoggedInUserID
                                                                                , RequirementPackage requirementPackage
                                                                                , List<RequirementFieldContract> requirementFieldContractList
                                                                                , List<RequirementItemContract> requirementItemContractList
                                                                                , RequirementCategoryContract categoryContract)
        {
            //Add data into category and package category mapping tables
            //Add new category
            //RequirementCategory requirementCategory = GetRequirementCategoryIfAlreadyExists(selectedTenantID, categoryContract.RequirementCategoryCode);
            if (requirementCategory.IsNull())
            {
                requirementCategory = CreateRequrmntCategory(currentLoggedInUserID, categoryContract);
                //if (requirementCategory.RequirementObjectProperties != null && requirementCategory.RequirementObjectProperties.Count > 0)   //Code commentted to Fixed an existing issue As Editable by settings is not saved in database for first time manual assignment of rotation package.
                if (categoryContract.RequirementObjectProperties != null)
                {
                    RequirementObjectProperty requirementObjectProperty = new RequirementObjectProperty();
                    requirementObjectProperty = CreateRequrmntObjectProperties(currentLoggedInUserID, categoryContract.RequirementObjectProperties, requirementCategory, null);
                    requirementCategory.RequirementObjectProperties.Add(requirementObjectProperty);
                }


                foreach (RequirementItemContract itemContract in categoryContract.LstRequirementItem.Where(cond => !cond.IsDeleted))
                {

                    itemContract.RItemURLSampleDocURL = categoryContract.RItemURLSampleDocURL;
                    itemContract.RItemURLLabel = categoryContract.RItemURLLabel;
                    requirementItemContractList.Add(itemContract);
                    RequirementCategoryItem requrmntCategoryItem = GetRequirementCategoryItemNew(selectedTenantID, currentLoggedInUserID
                                                                                               , requirementFieldContractList
                                                                                               , requirementCategory, itemContract);
                    if (requrmntCategoryItem != null)
                    {

                        requirementCategory.RequirementCategoryItems.Add(requrmntCategoryItem);
                    }

                }

            }

            //Add new package category mapping
            RequirementPackageCategory reqrmntPackageCategory = CreateRequrmntPackageCategory(currentLoggedInUserID, requirementPackage, requirementCategory);
            #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
            //Add ComplinaceRequired setting in RequirementPackageCategory table
            reqrmntPackageCategory.RPC_ComplianceRequired = categoryContract.IsComplianceRequired;
            reqrmntPackageCategory.RPC_ComplianceRqdStartDate = categoryContract.ComplianceReqStartDate;
            reqrmntPackageCategory.RPC_ComplianceRqdEndDate = categoryContract.ComplianceReqEndDate;
            reqrmntPackageCategory.RPC_CDisplayOrder = categoryContract.CategoryDisplayOrder;
            #endregion


            return reqrmntPackageCategory;
        }

        //Step 5
        /// <summary>
        /// Returns RequirementCategoryItem mapping object
        /// </summary>
        /// <param name="selectedTenantID"></param>
        /// <param name="currentLoggedInUserID"></param>
        /// <param name="requirementFieldContractList"></param>
        /// <param name="requirementCategory"></param>
        /// <param name="itemContract"></param>
        /// <returns></returns>
        private static RequirementCategoryItem GetRequirementCategoryItemNew(Int32 selectedTenantID, Int32 currentLoggedInUserID
                                                                          , List<RequirementFieldContract> requirementFieldContractList
                                                                          , RequirementCategory requirementCategory, RequirementItemContract itemContract)
        {
            //Add new Item
            RequirementItem requirementItem = GetRequirementItemIfAlreadyExists(selectedTenantID, itemContract.RequirementItemCode);
            RequirementCategoryItem requrmntCategoryItem = new RequirementCategoryItem();
            if (requirementItem.IsNotNull())
            {
                requrmntCategoryItem = CreateRequrmntCategoryItemNew(currentLoggedInUserID, requirementCategory, requirementItem, requirementItem.RI_ID);
            }

            if (requirementItem == null)
            {
                requirementItem = CreateRequirementItem(currentLoggedInUserID, itemContract);

                //UAT 4121
                if (itemContract.listRequirementItemURLContract != null && itemContract.listRequirementItemURLContract.Count > 0)
                {
                    List<RequirementItemURL> requirementitemurl = new List<RequirementItemURL>();
                    requirementitemurl = CreateRequirementItemURL(currentLoggedInUserID, itemContract.listRequirementItemURLContract);
                    //Add new RequirementitemURL
                    CreateRequrmntItemURL(requirementItem, currentLoggedInUserID, requirementitemurl);

                }

                //Add new category item mapping
                requrmntCategoryItem = CreateRequrmntCategoryItem(currentLoggedInUserID, requirementCategory, requirementItem, itemContract.RequirementItemDisplayOrder);
                if (itemContract.RequirementObjectProperties != null)
                {
                    RequirementObjectProperty requirementObjectProperty = new RequirementObjectProperty();
                    requirementObjectProperty = CreateRequrmntObjectProperties(currentLoggedInUserID, itemContract.RequirementObjectProperties, requirementCategory, requrmntCategoryItem);
                    requrmntCategoryItem.RequirementObjectProperties.Add(requirementObjectProperty);
                }

                #region Define common variables and get lkp values

                List<lkpDocumentAcroFieldType> lstLkpDocumentAcroFieldType = LookupManager.GetLookUpData<lkpDocumentAcroFieldType>(selectedTenantID).ToList();

                String requirementFieldViewDocumentTypeCode = DocumentType.REQUIREMENT_FIELD_VIEW_DOCUMENT.GetStringValue();
                Int32 requirementFieldViewDocumentTypeID = LookupManager.GetLookUpData<lkpDocumentType>(selectedTenantID)
                                                            .Where(cond => cond.DMT_Code == requirementFieldViewDocumentTypeCode).FirstOrDefault().DMT_ID;

                List<lkpRequirementFieldDataType> lstLkpRequirementFieldDataType = LookupManager.GetLookUpData<lkpRequirementFieldDataType>(selectedTenantID).ToList();

                #endregion

                //Add data into field and item field mapping tables
                foreach (RequirementFieldContract fieldContract in itemContract.LstRequirementField.Where(cond => !cond.IsDeleted))
                {
                    RequirementItemField requrmntItemField = GetRequirementItemField(currentLoggedInUserID, requirementFieldContractList
                                                                                    , requirementItem, lstLkpDocumentAcroFieldType, requirementFieldViewDocumentTypeID
                                                                                    , lstLkpRequirementFieldDataType, fieldContract);
                    //UAT 4380
                    if (fieldContract.RequirementObjectProperties != null)
                    {
                        RequirementObjectProperty requirementObjectProperty = new RequirementObjectProperty();
                        requirementObjectProperty = CreateRequrmntObjectProperties(currentLoggedInUserID, fieldContract.RequirementObjectProperties, requirementCategory, requrmntCategoryItem, requrmntItemField);
                        requrmntItemField.RequirementObjectProperties.Add(requirementObjectProperty);
                    }
                    requirementItem.RequirementItemFields.Add(requrmntItemField);
                }

            }

            return requrmntCategoryItem;
        }
        /// <summary>
        /// Entry in RequirementObjectTree and Copy Rules Functionality
        /// </summary>
        /// <param name="requirementSharedPackageContract"></param>
        /// <param name="selectedTenantID"></param>
        /// <param name="currentLoggedInUserID"></param>
        /// <param name="tenantRequirementPackage"></param>       
        private static void InsertDataIntoRequirementObjectTreeNew(RequirementPackageContract requirementSharedPackageContract, Int32 selectedTenantID, Int32 currentLoggedInUserID, RequirementPackage tenantRequirementPackage)
        {
            List<RequirementCategory> addedRequiremntCatgoriesList = tenantRequirementPackage.RequirementPackageCategories.Select(col => col.RequirementCategory).ToList();
            List<RequirementObjectTreeContract> requirmntObjTreeCntrct = new List<RequirementObjectTreeContract>();
            String catObjectTypeCode = ObjectType.Compliance_Category.GetStringValue();
            Int32 catObjectTypeId = LookupManager.GetLookUpData<lkpObjectType>(selectedTenantID).Where(cond => cond.OT_Code == catObjectTypeCode && !cond.OT_IsDeleted).FirstOrDefault().OT_ID;

            foreach (RequirementCategory requirementCategory in addedRequiremntCatgoriesList)
            {
                if (requirementCategory.RC_ID > AppConsts.NONE && !IfCategoryIdAlreadyExistsInObjectTree(selectedTenantID, requirementCategory.RC_ID, catObjectTypeId))
                {
                    RequirementCategoryContract sharedRequirementCategory = requirementSharedPackageContract.LstRequirementCategory
                                                .Where(cond => cond.RequirementCategoryCode == requirementCategory.RC_Code).FirstOrDefault();
                    //Entry in RequirementObjectTree
                    requirmntObjTreeCntrct = AddRequirementObjectTreeNew(selectedTenantID, requirementCategory.RC_ID, currentLoggedInUserID, catObjectTypeId);
                    //Copy RUles Functionality
                    RequirementRuleManager.CopyRulesFromSharedToTenant(selectedTenantID, sharedRequirementCategory.RequirementCategoryID, requirementCategory.RC_ID, currentLoggedInUserID);
                }
            }
        }

        /// <summary>
        /// Adds in Requirement Object Tree
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="requirementCategoryID"></param>
        /// <param name="currentUserID"></param>
        /// <param name="catObjectTypeId"></param>
        /// <returns></returns>
        private static List<RequirementObjectTreeContract> AddRequirementObjectTreeNew(Int32 tenantID, Int32 requirementCategoryID, Int32 currentUserID, Int32 catObjectTypeId)
        {
            try
            {
                return BALUtils.GetRequirementPackageRepoInstance(tenantID).AddRequirementObjectTreeNew(requirementCategoryID, catObjectTypeId, currentUserID);
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
        /// Checks if CategoryID already exists in RequirementObjectTree
        /// </summary>
        /// <param name="selectedTenantID"></param>
        /// <param name="requirementCategoryID"></param>
        /// <param name="catObjectTypeId"></param>
        /// <returns></returns>
        private static Boolean IfCategoryIdAlreadyExistsInObjectTree(Int32 selectedTenantID, Int32 requirementCategoryID, Int32 catObjectTypeId)
        {
            return BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).IfCategoryIdAlreadyExistsInObjectTree(requirementCategoryID, catObjectTypeId);
        }
        /// <summary>
        /// Get RequirementCategory if it already exists, checks it on the basis of CategoryCode
        /// </summary>
        /// <param name="selectedTenantID"></param>
        /// <param name="requirementCategoryCode"></param>
        /// <returns></returns>
        private static RequirementCategory GetRequirementCategoryIfAlreadyExists(Int32 selectedTenantID, Guid requirementCategoryCode)
        {
            return BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).GetRequirementCategoryIfAlreadyExists(requirementCategoryCode);
        }
        /// <summary>
        /// Get RequirementItem if it already exists, checks it on the basis of ItemCode
        /// </summary>
        /// <param name="selectedTenantID"></param>
        /// <param name="requirementItemcode"></param>
        /// <returns></returns>
        private static RequirementItem GetRequirementItemIfAlreadyExists(Int32 selectedTenantID, Guid requirementItemcode)
        {
            return BALUtils.GetRequirementPackageRepoInstance(selectedTenantID).GetRequirementItemIfAlreadyExists(requirementItemcode);
        }

        #endregion

        #region UAT-2514
        public static void RequirementPkgSync(Int32 tenantId, Int32 currentLoggedInUserId, String reqPackageObjectIds)
        {
            try
            {
                var packageSubscriptionIDs = BALUtils.GetRequirementPackageRepoInstance(tenantId).RequirementPkgSync(currentLoggedInUserId, reqPackageObjectIds);
                try
                {
                     BALUtils.GetRequirementPackageRepoInstance(tenantId).RequirementVerificationDataToFlatTable(packageSubscriptionIDs, currentLoggedInUserId,AppConsts.NONE);
                }
                catch
                {
                    if (!packageSubscriptionIDs.IsNullOrWhiteSpace())
                    {
                        Int32 countLength = packageSubscriptionIDs.Split(',').Length;
                        for (int i = 0; i < (countLength % 100); i++)
                        {
                            String joined = String.Join(",", packageSubscriptionIDs.Split(',').Skip(i * 100).Take(100).ToList());
                            if (joined.IsNullOrWhiteSpace())
                                break;
                            BALUtils.GetRequirementPackageRepoInstance(tenantId).StoreRequirenmentPackageIdsInScheduleTask(joined, currentLoggedInUserId);
                        }
                    }

                }
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

        /// <summary>
        /// Get Executiontimeout Package Id 
        /// </summary>
        /// <param name="tenantID"></param>

        #region 
        public static void GetRequirenmentPackageIdsInScheduleTask(Int32 tenantId, Int32 currentLoggedInUserId)
        {
            try
            {
                var tasktypeId = LookupManager.GetLookUpData<lkpTaskType>(tenantId).FirstOrDefault
                    (condition => condition.TaskTypeCode.Trim() == TaskType.sync_Requirement_Verification_DataTo_FlatTable.GetStringValue()
                    && condition.IsActive == true && condition.IsDeleted == false).TaskTypeID;

                var taskstatustypeIdPending = LookupManager.GetLookUpData<lkpTaskStatusType>(tenantId).FirstOrDefault
                    (condition => condition.TaskStatusTypeCode == TaskStatusType.PENDING.GetStringValue() && condition.IsActive == true
                    && condition.IsDeleted == false).TaskStatusTypeID;

                List<ScheduledTask> lstScheduledTasks =  BALUtils.GetRequirementPackageRepoInstance(tenantId).GetRequirenmentPackageIdsInScheduleTask(tasktypeId, taskstatustypeIdPending);

                if(lstScheduledTasks.Count > 0)
                {
                    foreach (var item in lstScheduledTasks)
                    {
                        if (!item.ST_Parameters.IsNullOrWhiteSpace())
                        {
                            XDocument xdocument = XDocument.Parse(item.ST_Parameters);
                            String packageIds = xdocument.Root.Elements("PackageIds").FirstOrDefault().FirstNode.ToString();
                            BALUtils.GetRequirementPackageRepoInstance(tenantId).RequirementVerificationDataToFlatTable(packageIds, currentLoggedInUserId, item.ST_ID);
                        }
                    }
                }
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

        /// <summary>
        /// UAT-2423 Get the Rotation Package Category name and ExplanatoryNotes using rotation ID 
        /// </summary>
        /// <param name="tenantID"></param>
        /// <param name="rotationID"></param>
        /// <returns></returns>
        public static List<RequirementCategoryContract> GetRotationPackageCategoryDetailByRotationID(Int32 tenantID, Int32 rotationID, Boolean IsStudentPackage)
        {
            try
            {
                return BALUtils.GetRequirementPackageRepoInstance(tenantID).GetRotationPackageCategoryDetailByRotationID(rotationID, IsStudentPackage);
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

        #region UAT-2706
        public static Dictionary<Int32, String> GetRequirementCategoryDataBypackageId(Int32 ReqPackageId)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementCategoryDataBypackageId(ReqPackageId);
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

        public static List<RequirementItemContract> GetRequirementItemDetailsByCategoryId(Int32 ReqCategoryId)
        {
            try
            {
                List<RequirementItemContract> lstReqItemDetails = new List<RequirementItemContract>();
                List<Entity.SharedDataEntity.RequirementItem> lstReqItem = BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementItemByCategoryId(ReqCategoryId);

                RequirementItemContract reqItemContract = null;
                List<RequirementFieldContract> lstReqFieldContract = null;
                RequirementFieldContract reqFieldContract = null;

                lstReqItem.Where(cond => !cond.RI_IsDeleted).ForEach(x =>
                    {
                        reqItemContract = new RequirementItemContract();
                        reqItemContract.RequirementItemID = x.RI_ID;
                        reqItemContract.RequirementItemName = x.RI_ItemName;
                        reqItemContract.RequirementItemLabel = x.RI_ItemLabel;
                        //UAT-3078
                        Int32? displayOrder = x.RequirementCategoryItems.Where(cond => cond.RCI_RequirementCategoryID == ReqCategoryId).FirstOrDefault().RCI_DisplayOrder;
                        reqItemContract.RequirementItemDisplayOrder = displayOrder.HasValue ? displayOrder.Value : AppConsts.NONE;

                        lstReqFieldContract = new List<RequirementFieldContract>();
                        x.RequirementItemFields.Where(cond => !cond.RIF_IsDeleted).Select(sel => sel.RequirementField).Where(cond => !cond.RF_IsDeleted).ForEach(f =>
                        {
                            reqFieldContract = new RequirementFieldContract();
                            reqFieldContract.RequirementFieldID = f.RF_ID;
                            reqFieldContract.RequirementFieldName = f.RF_FieldName;
                            reqFieldContract.RequirementFieldLabel = f.RF_FieldLabel;
                            reqFieldContract.RequirementFieldData = new RequirementFieldDataContract();
                            reqFieldContract.RequirementFieldData.RequirementFieldDataTypeCode = f.lkpRequirementFieldDataType.RFDT_Code;
                            reqFieldContract.RequirementFieldData.RequirementFieldDataTypeID = f.RF_FieldDataTypeID;
                            reqFieldContract.RequirementFieldData.RequirementFieldDataTypeName = f.lkpRequirementFieldDataType.RFDT_Name;

                            if (f.lkpRequirementFieldDataType.RFDT_Code == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue())
                            {
                                reqFieldContract.RequirementFieldData.FieldViewDocumentData = new RequirementFieldViewDocumentData();

                                reqFieldContract.RequirementFieldData.FieldViewDocumentData.ClientSystemDocumentID = f.RequirementFieldDocuments.Where(cond => !cond.RFD_IsDeleted).FirstOrDefault().ClientSystemDocument.CSD_ID;
                                reqFieldContract.RequirementFieldData.FieldViewDocumentData.DocumentFileName = f.RequirementFieldDocuments.Where(cond => !cond.RFD_IsDeleted).FirstOrDefault().ClientSystemDocument.CSD_FileName;
                            }
                            if (f.lkpRequirementFieldDataType.RFDT_Code == RequirementFieldDataType.VIEW_VIDEO.GetStringValue())
                            {
                                reqFieldContract.RequirementFieldData.VideoFieldData = new RequirementFieldVideoData();
                                //reqFieldContract.RequirementFieldData.FieldViewDocumentData = new RequirementFieldViewDocumentData();
                                reqFieldContract.RequirementFieldData.VideoFieldData.VideoName = f.RequirementFieldVideos.FirstOrDefault().RFV_VideoName;
                                reqFieldContract.RequirementFieldData.VideoFieldData.VideoURL = f.RequirementFieldVideos.FirstOrDefault().RFV_VideoURL;

                            }
                            else if (f.lkpRequirementFieldDataType.RFDT_Code == RequirementFieldDataType.OPTIONS.GetStringValue())
                            {
                                reqFieldContract.RequirementFieldData.LstRequirementFieldOptions = new List<RequirementFieldOptionsData>();
                                f.RequirementFieldOptions.ForEach(o =>
                                {
                                    RequirementFieldOptionsData reqFieldOptionData = new RequirementFieldOptionsData();
                                    reqFieldOptionData.RequirementFieldID = o.RFO_RequirementFieldID;
                                    reqFieldOptionData.RequirementFieldOptionsID = o.RFO_ID;
                                    reqFieldOptionData.OptionText = o.RFO_OptionText;
                                    reqFieldOptionData.OptionValue = o.RFO_OptionValue;

                                    reqFieldContract.RequirementFieldData.LstRequirementFieldOptions.Add(reqFieldOptionData);
                                });
                            }

                            lstReqFieldContract.Add(reqFieldContract);
                        });

                        reqItemContract.LstRequirementField = lstReqFieldContract;

                        lstReqItemDetails.Add(reqItemContract);
                    });

                return lstReqItemDetails;
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

        public static List<RequirementPackageContract> GetRequirementPackagesByHierarcyIds(List<Int32> lstAgencyHierarchyIds, Int32 currentLoggedInUserId, CustomPagingArgsContract gridCustomPaging)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetRequirementPackagesByHierarcyIds(lstAgencyHierarchyIds, currentLoggedInUserId, gridCustomPaging);
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

        //UAT-2795//

        public static String GetCategoryDocumentLink(Int32 ReqCategoryId)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetCategoryDocumentLink(ReqCategoryId);
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

        public static void UpdateRequirementPackageAgencyMappings(List<Int32> lstNewRequirementPkgAgencies, Int32 currentLoggedInUserID, Int32 tenantID, Int32 requirementPackageId)
        {
            try
            {
                BALUtils.GetRequirementPackageRepoInstance(tenantID).UpdateRequirementPackageAgencyMappings(lstNewRequirementPkgAgencies, currentLoggedInUserID, requirementPackageId);
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

        //UAT-3296//

        public static String GetCategoryExplanatoryNotes(Int32 ReqCategoryId)
        {
            try
            {
                return BALUtils.GetSharedRequirementPackageRepoInstance().GetCategoryExplanatoryNotes(ReqCategoryId);
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

        #region UAT-4015

        public static String GetReqPackageSubsStatusBySubscriptionID(Int32 tenantId, Int32 rpsId)
        {
            try
            {
                return BALUtils.GetRequirementPackageRepoInstance(tenantId).GetReqPackageSubsStatusBySubscriptionID(rpsId);
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


        /// <summary>
        /// Method to get all possible data types of rotation fields
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        public static List<RequirementDocumentAcroFieldType> GetDocumentAcroFieldType()
        {
            try
            {
                List<Entity.SharedDataEntity.lkpDocumentAcroFieldType> dataTypeList = LookupManager.GetSharedDBLookUpData<Entity.SharedDataEntity.lkpDocumentAcroFieldType>().ToList();
                return dataTypeList.Select(col => new RequirementDocumentAcroFieldType
                {
                    Name = col.DAFT_Name,
                    Code = col.DAFT_Code,
                    DocumentAcroFieldTypeID = col.DAFT_ID
                }).ToList();
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
        public static List<RequirementCategoryDocLink> CreateReqCatDocUrls(Int32 currentLoggedInUserID, List<RequirementCategoryDocUrl> lstCatDocUrls)
        {
            try
            {
                List<RequirementCategoryDocLink> lstCatUrls = new List<RequirementCategoryDocLink>();

                if (!lstCatDocUrls.IsNullOrEmpty() && lstCatDocUrls.Count > AppConsts.NONE)
                {
                    lstCatDocUrls.ForEach(x =>
                    {
                        RequirementCategoryDocLink reqCatUrl = new RequirementCategoryDocLink();
                        reqCatUrl.RCDL_SampleDocFormUrlLabel = x.RequirementCatDocUrlLabel;
                        reqCatUrl.RCDL_SampleDocFormURL = x.RequirementCatDocUrl;
                        reqCatUrl.RCDL_IsDeleted = false;
                        reqCatUrl.RCDL_CreatedBy = currentLoggedInUserID;
                        reqCatUrl.RCDL_CreatedOn = DateTime.Now;
                        lstCatUrls.Add(reqCatUrl);
                    });
                }
                return lstCatUrls;
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

