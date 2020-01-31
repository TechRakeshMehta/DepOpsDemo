using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;

namespace DAL.Repository
{
    public class RequirementPackageRepository : ClientBaseRepository, IRequirementPackageRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public RequirementPackageRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }

        Boolean IRequirementPackageRepository.AddRequirementPackageToDatabase(RequirementPackage requirementPackage)
        {
            _dbContext.RequirementPackages.AddObject(requirementPackage);
            if (_dbContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// used to get requirement package details including package name,category name,item name and field name in hierarichal way
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        List<RequirementPackageDetailsContract> IRequirementPackageRepository.GetRequirementPackageDetailsByPackageID(Int32 requirementPackageID)
        {
            List<RequirementPackageDetailsContract> requirementPackageDetailsList = new List<RequirementPackageDetailsContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@RequirementPackageID", requirementPackageID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementPackageDetailsByPackageID", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementPackageDetailsContract requirementPackageDetail = new RequirementPackageDetailsContract();
                            requirementPackageDetail.RequirementPackageID = dr["RequirementPackageID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageID"]);
                            requirementPackageDetail.RequirementPackageName = dr["RequirementPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageName"]);
                            requirementPackageDetail.RequirementCategoryID = dr["RequirementCategoryID"] != DBNull.Value ? Convert.ToInt32(dr["RequirementCategoryID"]) : AppConsts.NONE;
                            requirementPackageDetail.RequirementCategoryName = dr["RequirementCategoryName"] != DBNull.Value ? Convert.ToString(dr["RequirementCategoryName"]) : String.Empty;
                            requirementPackageDetail.RequirementItemID = dr["RequirementItemID"] != DBNull.Value ? Convert.ToInt32(dr["RequirementItemID"]) : 0;
                            requirementPackageDetail.RequirementItemName = dr["RequirementItemName"] != DBNull.Value ? Convert.ToString(dr["RequirementItemName"]) : String.Empty;
                            requirementPackageDetail.RequirementFieldID = dr["RequirementFieldID"] != DBNull.Value ? Convert.ToInt32(dr["RequirementFieldID"]) : 0;
                            requirementPackageDetail.RequirementFieldName = dr["RequirementFieldName"] != DBNull.Value ? Convert.ToString(dr["RequirementFieldName"]) : String.Empty;
                            requirementPackageDetail.DefinedRequirementDescription = dr["DefinedRequirementDescription"] != DBNull.Value ? Convert.ToString(dr["DefinedRequirementDescription"]) : String.Empty;
                            requirementPackageDetail.DefinedRequirementID = dr["DefinedRequirementID"] != DBNull.Value ? (Int32?)dr["DefinedRequirementID"] : null;
                            requirementPackageDetailsList.Add(requirementPackageDetail);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return requirementPackageDetailsList;
        }

        /// <summary>
        /// To get Requirement Packages
        /// </summary>
        /// <param name="agencyId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        List<RequirementPackageContract> IRequirementPackageRepository.GetRequirementPackages(String agencyId)
        {
            List<RequirementPackageContract> requirementPackageList = new List<RequirementPackageContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyID", agencyId)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementPackages", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementPackageContract requirementPackageContract = new RequirementPackageContract();
                            requirementPackageContract.RequirementPackageID = dr["RequirementPackageID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RequirementPackageID"]);
                            requirementPackageContract.RequirementPackageName = Convert.ToString(dr["RequirementPackageName"]);
                            requirementPackageContract.RequirementPackageLabel = Convert.ToString(dr["RequirementPackageLabel"]);
                            requirementPackageContract.RequirementPackageCode = dr["RequirementPackageCode"] == DBNull.Value ? Guid.Empty : (Guid)(dr["RequirementPackageCode"]);
                            requirementPackageContract.IsCopied = dr["IsCopied"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsCopied"]);
                            //UAT-2514--Retrieved NewPackage Property
                            requirementPackageContract.IsNewPackage = dr["IsNewPackage"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsNewPackage"]);
                            requirementPackageContract.IsSharedUserPackage = false;
                            requirementPackageContract.IsNewPackage = dr["IsNewPackage"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsNewPackage"]);
                            requirementPackageContract.EffectiveStartDate = dr["RequirementEffectiveStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RequirementEffectiveStartDate"]);
                            requirementPackageContract.EffectiveEndDate = dr["RequirementEffectiveEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RequirementEffectiveEndDate"]);
                            #region UAT-4657
                            requirementPackageContract.RootParentCode = dr["RootParentCode"] == DBNull.Value ? Guid.Empty : (Guid)(dr["RootParentCode"]); ;
                            //requirementPackageContract.RootParentID = dr["RootParentID"] != DBNull.Value ? Convert.ToInt32(dr["RootParentID"]) : AppConsts.NONE; 
                            #endregion
                            requirementPackageList.Add(requirementPackageContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return requirementPackageList.OrderBy(col => col.RequirementPackageName).ToList();
        }

        /// <summary>
        /// used to get all requirement package details including package name and comma separated agencyNames with which they are mapped. It also returns unMapped packages too
        /// </summary>
        /// <returns></returns>
        List<RequirementPackageDetailsContract> IRequirementPackageRepository.GetRequirementPackageDetails(RequirementPackageDetailsContract requirementPackageDetailsContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<RequirementPackageDetailsContract> requirementPackageDetailList = new List<RequirementPackageDetailsContract>();
            String orderBy = "RequirementPackageID";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyIDs", requirementPackageDetailsContract.LstAgencyIDs),
                    new SqlParameter("@PackageName", requirementPackageDetailsContract.RequirementPackageName),
                    new SqlParameter("@RotationPackageTypeID", (requirementPackageDetailsContract.RequirementPkgTypeID == AppConsts.NONE? null : requirementPackageDetailsContract.RequirementPkgTypeID)), //UAT1352
                    new SqlParameter("@PackageActiveStatus", requirementPackageDetailsContract.IsActivePackage),
                    new SqlParameter("@DefinedRequirementID", requirementPackageDetailsContract.DefinedRequirementID),
                    new SqlParameter("@OrderBy", orderBy),
                    new SqlParameter("@OrderDirection", ordDirection),
                    new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                    new SqlParameter("@PageSize", customPagingArgsContract.PageSize)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementPackageDetails", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementPackageDetailsContract requirementPackageDetail = new RequirementPackageDetailsContract();
                            requirementPackageDetail.RequirementPackageID = dr["RequirementPackageID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageID"]);
                            requirementPackageDetail.RequirementPackageName = dr["RequirementPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageName"]);
                            requirementPackageDetail.RequirementPackageLabel = dr["RequirementPackageLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageLabel"]);
                            requirementPackageDetail.IsActivePackage = dr["IsActivePackage"] != DBNull.Value ? Convert.ToBoolean(dr["IsActivePackage"]) : true;
                            requirementPackageDetail.AgencyNames = dr["AgencyNames"] != DBNull.Value ? Convert.ToString(dr["AgencyNames"]) : String.Empty;
                            requirementPackageDetail.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                            requirementPackageDetail.PackageObjectTreeID = dr["PackageObjectTreeID"] != DBNull.Value ? Convert.ToInt32(dr["PackageObjectTreeID"]) : 0;
                            requirementPackageDetail.RequirementPkgTypeName = dr["RequirementPkgTypeName"] != DBNull.Value ? Convert.ToString(dr["RequirementPkgTypeName"]) : String.Empty;
                            requirementPackageDetail.IsSharedUserPackage = false;
                            requirementPackageDetail.DefinedRequirementDescription = dr["DefinedRequirementDescription"] != DBNull.Value ? Convert.ToString(dr["DefinedRequirementDescription"]) : String.Empty;
                            requirementPackageDetail.DefinedRequirementID = dr["DefinedRequirementID"] != DBNull.Value ? (Int32?)(dr["DefinedRequirementID"]) : null;
                            requirementPackageDetailList.Add(requirementPackageDetail);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);

            }
            return requirementPackageDetailList;
        }

        /// <summary>
        /// get complete package details in hierarchal way
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        List<RequirementPackageHierarchicalDetailsContract> IRequirementPackageRepository.GetRequirementPackageHierarchalDetailsByPackageID(Int32 requirementPackageID)
        {
            List<RequirementPackageHierarchicalDetailsContract> requirementPackageHierarchicalDetailList = new List<RequirementPackageHierarchicalDetailsContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@RequirementPackageID", requirementPackageID)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementPackageHierarchyDetails", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementPackageHierarchicalDetailsContract requirementPackageHierarchicalDetail = new RequirementPackageHierarchicalDetailsContract();
                            requirementPackageHierarchicalDetail.RequirementPackageID = dr["RequirementPackageID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageID"]);
                            requirementPackageHierarchicalDetail.RequirementPackageCode = dr["RequirementPackageCode"] == DBNull.Value ? Guid.Empty : (Guid)(dr["RequirementPackageCode"]);
                            requirementPackageHierarchicalDetail.RequirementPackageName = dr["RequirementPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageName"]);
                            requirementPackageHierarchicalDetail.RequirementPackageLabel = dr["RequirementPackageLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageLabel"]);
                            requirementPackageHierarchicalDetail.RequirementPackageRuleTypeCode = dr["RequirementPackageRuleTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageRuleTypeCode"]);
                            requirementPackageHierarchicalDetail.RequirementPackageCategoryID = dr["RequirementPackageCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageCategoryID"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryID = dr["RequirementCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementCategoryID"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryCode = dr["RequirementCategoryCode"] == DBNull.Value ? Guid.Empty : (Guid)(dr["RequirementCategoryCode"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryName = dr["RequirementCategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementCategoryName"]);

                            //UAT-1555 Add Document link to Rotation package student notes (as it is on immunization package student notes)
                            requirementPackageHierarchicalDetail.RequirementDocumentLink = dr["RequirementDocumentLink"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementDocumentLink"]);
                            //UAT-3161
                            requirementPackageHierarchicalDetail.RequirementDocumentLinkLabel = dr["RequirementDocumentLinkLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementDocumentLinkLabel"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryLabel = dr["RequirementCategoryLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementCategoryLabel"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryDescription = dr["RequirementCategoryDescription"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementCategoryDescription"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryRuleTypeCode = dr["RequirementCategoryRuleTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementCategoryRuleTypeCode"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryItemID = dr["RequirementCategoryItemID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementCategoryItemID"]);
                            requirementPackageHierarchicalDetail.RequirementItemID = dr["RequirementItemID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementItemID"]);
                            requirementPackageHierarchicalDetail.RequirementItemCode = dr["RequirementItemCode"] == DBNull.Value ? Guid.Empty : (Guid)(dr["RequirementItemCode"]);
                            requirementPackageHierarchicalDetail.RequirementItemName = dr["RequirementItemName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementItemName"]);
                            requirementPackageHierarchicalDetail.RequirementItemLabel = dr["RequirementItemLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementItemLabel"]);
                            requirementPackageHierarchicalDetail.RequirementItemDescription = dr["RequirementItemDescription"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementItemDescription"]);
                            requirementPackageHierarchicalDetail.RequirementItemRuleTypeCode = dr["RequirementItemRuleTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementItemRuleTypeCode"]);
                            requirementPackageHierarchicalDetail.DateTypeRequirementFieldCodeForExpiration = dr["DateTypeRequirementFieldCodeForExpiration"] == DBNull.Value ? Guid.Empty : (Guid)(dr["DateTypeRequirementFieldCodeForExpiration"]);
                            requirementPackageHierarchicalDetail.DateTypeRequirementFieldIDForExpiration = dr["DateTypeRequirementFieldIDForExpiration"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DateTypeRequirementFieldIDForExpiration"]);
                            requirementPackageHierarchicalDetail.ExpirationValueTypeCode = dr["ExpirationValueTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ExpirationValueTypeCode"]);
                            requirementPackageHierarchicalDetail.ExpirationValueTypeID = dr["ExpirationValueTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ExpirationValueTypeID"]);
                            requirementPackageHierarchicalDetail.ExpirationValue = dr["ExpirationValue"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ExpirationValue"]);
                            requirementPackageHierarchicalDetail.RequirementItemFieldID = dr["RequirementItemFieldID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementItemFieldID"]);
                            requirementPackageHierarchicalDetail.RequirementFieldID = dr["RequirementFieldID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementFieldID"]);
                            requirementPackageHierarchicalDetail.RequirementFieldCode = dr["RequirementFieldCode"] == DBNull.Value ? Guid.Empty : (Guid)(dr["RequirementFieldCode"]);
                            requirementPackageHierarchicalDetail.RequirementFieldName = dr["RequirementFieldName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementFieldName"]);
                            requirementPackageHierarchicalDetail.RequirementFieldLabel = dr["RequirementFieldLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementFieldLabel"]);
                            requirementPackageHierarchicalDetail.RequirementFieldDescription = dr["RequirementFieldDescription"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementFieldDescription"]);
                            requirementPackageHierarchicalDetail.RequirementFieldAttributeTypeID = dr["RequirementFieldAttributeTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementFieldAttributeTypeID"]);
                            requirementPackageHierarchicalDetail.RequirementFieldRuleTypeCode = dr["RequirementFieldRuleTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementFieldRuleTypeCode"]);
                            requirementPackageHierarchicalDetail.RequirementFieldRuleValue = dr["RequirementFieldRuleValue"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementFieldRuleValue"]);
                            requirementPackageHierarchicalDetail.RequirementFieldDataTypeID = dr["RequirementFieldDataTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementFieldDataTypeID"]);
                            requirementPackageHierarchicalDetail.RequirementFieldDataTypeCode = dr["RequirementFieldDataTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementFieldDataTypeCode"]);
                            requirementPackageHierarchicalDetail.RequirementFieldDataTypeName = dr["RequirementFieldDataTypeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementFieldDataTypeName"]);
                            requirementPackageHierarchicalDetail.RequirementFieldVideoID = dr["RequirementFieldVideoID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementFieldVideoID"]);
                            requirementPackageHierarchicalDetail.RequirementFieldVideoName = dr["RequirementFieldVideoName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementFieldVideoName"]);
                            requirementPackageHierarchicalDetail.FieldVideoURL = dr["FieldVideoURL"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FieldVideoURL"]);
                            requirementPackageHierarchicalDetail.RequirementFieldOptionsID = dr["RequirementFieldOptionsID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementFieldOptionsID"]);
                            requirementPackageHierarchicalDetail.OptionText = dr["OptionText"] == DBNull.Value ? String.Empty : Convert.ToString(dr["OptionText"]);
                            requirementPackageHierarchicalDetail.OptionValue = dr["OptionValue"] == DBNull.Value ? String.Empty : Convert.ToString(dr["OptionValue"]);
                            requirementPackageHierarchicalDetail.RequirementFieldDocumentID = dr["RequirementFieldDocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementFieldDocumentID"]);
                            requirementPackageHierarchicalDetail.ClientSystemDocumentID = dr["ClientSystemDocumentID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ClientSystemDocumentID"]);
                            requirementPackageHierarchicalDetail.ClientSystemDocumentFileName = dr["ClientSystemDocumentFileName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ClientSystemDocumentFileName"]);
                            requirementPackageHierarchicalDetail.ClientSystemDocumentSize = dr["ClientSystemDocumentSize"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ClientSystemDocumentSize"]);
                            requirementPackageHierarchicalDetail.ClientSystemDocumentPath = dr["ClientSystemDocumentPath"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ClientSystemDocumentPath"]);
                            requirementPackageHierarchicalDetail.DocumentTypeID = dr["DocumentTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DocumentTypeID"]);
                            requirementPackageHierarchicalDetail.DocumentTypeCode = dr["DocumentTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DocumentTypeCode"]);
                            requirementPackageHierarchicalDetail.RequirementDocumentAcroFieldID = dr["RequirementDocumentAcroFieldID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementDocumentAcroFieldID"]);
                            requirementPackageHierarchicalDetail.DocumentAcroFieldTypeID = dr["DocumentAcroFieldTypeID"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DocumentAcroFieldTypeID"]);
                            requirementPackageHierarchicalDetail.DocumentAcroFieldTypeCode = dr["DocumentAcroFieldTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DocumentAcroFieldTypeCode"]);
                            requirementPackageHierarchicalDetail.DocumentAcroFieldTypeName = dr["DocumentAcroFieldTypeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DocumentAcroFieldTypeName"]);
                            requirementPackageHierarchicalDetail.RequirementPackageAgencyID = dr["RequirementPackageAgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageAgencyID"]);
                            requirementPackageHierarchicalDetail.AgencyID = dr["AgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyID"]);
                            requirementPackageHierarchicalDetail.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            requirementPackageHierarchicalDetail.PackageObjectTreeID = dr["PackageObjectTreeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PackageObjectTreeID"]);
                            requirementPackageHierarchicalDetail.CategoryObjectTreeID = dr["CategoryObjectTreeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CategoryObjectTreeID"]);
                            requirementPackageHierarchicalDetail.ItemObjectTreeID = dr["ItemObjectTreeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ItemObjectTreeID"]);
                            requirementPackageHierarchicalDetail.FieldObjectTreeID = dr["FieldObjectTreeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FieldObjectTreeID"]);
                            requirementPackageHierarchicalDetail.CategoryExplanatoryNotes = dr["CategoryExplanatoryNotes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CategoryExplanatoryNotes"]);
                            //UAT 1352:As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use
                            requirementPackageHierarchicalDetail.RequirementPkgTypeID = dr["RequirementPkgTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPkgTypeID"]);
                            requirementPackageHierarchicalDetail.RequirementPkgTypeCode = dr["RequirementPkgTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPkgTypeCode"]);
                            requirementPackageHierarchicalDetail.IsCopied = dr["IsCopied"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsCopied"]);

                            #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
                            //Fetching ComplinaceRequired Data from the DataTable.
                            requirementPackageHierarchicalDetail.IsComplianceRequired = dr["IsComplianceRequired"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsComplianceRequired"]);
                            requirementPackageHierarchicalDetail.ComplianceReqStartDate = dr["ComplianceReqStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ComplianceReqStartDate"]);
                            requirementPackageHierarchicalDetail.ComplianceReqEndDate = dr["ComplianceReqEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ComplianceReqEndDate"]);
                            requirementPackageHierarchicalDetail.ExpirationCondEndDate = dr["ExpirationCondEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ExpirationCondEndDate"]);
                            requirementPackageHierarchicalDetail.ExpirationCondStartDate = dr["ExpirationCondStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ExpirationCondStartDate"]);
                            #endregion

                            #region UAT-2164 : Agency User - Granular Permissions
                            requirementPackageHierarchicalDetail.IsBackgroundDocument = dr["IsBackgroundDocument"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsBackgroundDocument"]);
                            #endregion

                            requirementPackageHierarchicalDetail.DefinedRequirementDescription = dr["DefinedRequirementDescription"] != DBNull.Value ? Convert.ToString(dr["DefinedRequirementDescription"]) : String.Empty;
                            requirementPackageHierarchicalDetail.DefinedRequirementID = dr["DefinedRequirementID"] != DBNull.Value ? (Int32?)dr["DefinedRequirementID"] : null;

                            //UAT-3805
                            requirementPackageHierarchicalDetail.SendItemDocOnApproval = dr["CategorySendItemDocOnApprovalData"] == DBNull.Value ? false : Convert.ToBoolean(dr["CategorySendItemDocOnApprovalData"]);

                            requirementPackageHierarchicalDetailList.Add(requirementPackageHierarchicalDetail);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return requirementPackageHierarchicalDetailList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <param name="pkgObjectTypeId"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        List<RequirementObjectTreeContract> IRequirementPackageRepository.AddRequirementObjectTree(Int32 requirementPackageID, Int32 pkgObjectTypeId, Int32 currentUserID)
        {
            List<RequirementObjectTreeContract> requirementObjectTreeList = new List<RequirementObjectTreeContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@ParentObjectID", null),
                    new SqlParameter("@ParentObjectTypeID", null),
                    new SqlParameter("@NewObjectID", requirementPackageID),
                    new SqlParameter("@NewObjectTypeID", pkgObjectTypeId),
                    new SqlParameter("@UserID", currentUserID)

                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_AddRequirementObjectTreeNode", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementObjectTreeContract requirementObjectTree = new RequirementObjectTreeContract();
                            requirementObjectTree.RequirementObjectTreeID = dr["ROTID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ROTID"]);
                            requirementObjectTree.ObjectID = dr["ObjectID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ObjectID"]);
                            requirementObjectTree.ObjectTypeID = dr["ObjectTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ObjectTypeID"]);
                            requirementObjectTree.ParentID = dr["ParentID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ParentID"]);
                            requirementObjectTree.ParentObjectID = dr["ParentObjectID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ParentObjectID"]);
                            requirementObjectTree.IsNewRecordInserted = dr["IsNewRecord"] == DBNull.Value ? true : Convert.ToBoolean(dr["IsNewRecord"]);

                            requirementObjectTreeList.Add(requirementObjectTree);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return requirementObjectTreeList;
        }

        Boolean IRequirementPackageRepository.IsPackageVersionNeedToCreate(Int32 requirementPackageID, String reqPkgTypeCode)
        {
            if (reqPkgTypeCode == RequirementPackageType.APPLICANT_ROTATION_PACKAGE.GetStringValue())
            {
                return _dbContext.ClinicalRotationRequirementPackages
                 .Any(cond => cond.CRRP_RequirementPackageID == requirementPackageID && !cond.CRRP_IsDeleted
                        && cond.ClinicalRotation != null && cond.ClinicalRotation.ClinicalRotationMembers.Any(crm => !crm.CRM_IsDeleted)
                        && !cond.ClinicalRotation.CR_IsDeleted
                    );
            }
            else if (reqPkgTypeCode == RequirementPackageType.INSTRUCTOR_PRECEPTOR_ROTATION_PACKAGE.GetStringValue())
            {

                return _dbContext.ClinicalRotationRequirementPackages.Where(crp => crp.CRRP_RequirementPackageID == requirementPackageID && !crp.CRRP_IsDeleted)
                    .Join(_dbContext.ClinicalRotationSubscriptions, crp => crp.CRRP_ID, crs => crs.CRS_ClinicalRotationRequirementPackageID, (crp, crs) => new { RequirementPackageSubscriptionID = crs.CRS_RequirementPackageSubscriptionID, CRS_IsDeleted = crs.CRS_IsDeleted })
                       .Where(cond => !cond.CRS_IsDeleted)
                    .Join(_dbContext.RequirementPackageSubscriptions, crp1 => crp1.RequirementPackageSubscriptionID, rps => rps.RPS_ID, (crp1, rps) => new { RequirementPackageSubscriptionID = rps.RPS_ID, RPS_IsDeleted = rps.RPS_IsDeleted })
                       .Where(cond => !cond.RPS_IsDeleted)
                    .Join(_dbContext.ApplicantRequirementCategoryDatas, rps1 => rps1.RequirementPackageSubscriptionID, acd => acd.ARCD_RequirementPackageSubscriptionID, (rps1, acd) => new { ARCD_IsDeleted = acd.ARCD_IsDeleted })
                    .Where(cond => !cond.ARCD_IsDeleted)
                     .Any();




                return _dbContext.ClinicalRotationRequirementPackages
                                                         .Any(x => !x.CRRP_IsDeleted && x.CRRP_RequirementPackageID == requirementPackageID
                                                          && x.ClinicalRotationSubscriptions
                                                         .Any(cond => !cond.CRS_IsDeleted &&
                                                             !cond.RequirementPackageSubscription.RPS_IsDeleted
                                                             && cond.RequirementPackageSubscription.ApplicantRequirementCategoryDatas.Any(arcd => !arcd.ARCD_IsDeleted))
                                                         );
            }

            return false;
        }

        Boolean IRequirementPackageRepository.IsPackageMappedToRotation(Int32 requirementPackageID)
        {
            return _dbContext.ClinicalRotationRequirementPackages
                 .Any(cond => cond.CRRP_RequirementPackageID == requirementPackageID && !cond.CRRP_IsDeleted);
        }

        RequirementPackage IRequirementPackageRepository.GetRequirementPackageByPackageID(Int32 requirementPackageID)
        {
            return _dbContext.RequirementPackages.Where(cond => !cond.RP_IsDeleted && cond.RP_ID == requirementPackageID).FirstOrDefault();
        }

        RequirementPackage IRequirementPackageRepository.GetRequirementPackageByPackageCode(Guid requirementPackageCode)
        {
            return _dbContext.RequirementPackages.Where(cond => !cond.RP_IsDeleted && cond.RP_Code == requirementPackageCode).FirstOrDefault();
        }


        Boolean IRequirementPackageRepository.SetExistingPackageIsCopiedToTrue(Int32 currentLoggedInUserID, Int32 requirementPackageID)
        {
            //Get existing requirement package(from DB) which is to be updated 
            RequirementPackage existingRequirementPackage = _dbContext.RequirementPackages.Where(cond => !cond.RP_IsDeleted && cond.RP_ID == requirementPackageID).FirstOrDefault();
            if (!existingRequirementPackage.IsNullOrEmpty())
            {
                existingRequirementPackage.RP_IsCopied = true;
                existingRequirementPackage.RP_ModifiedByID = currentLoggedInUserID;
                existingRequirementPackage.RP_ModifiedOn = DateTime.Now;
            }
            if (_dbContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        Boolean IRequirementPackageRepository.SetExistingPackageIsDeletedToTrue(Int32 currentLoggedInUserID, Int32 requirementPackageID)
        {
            //Get existing requirement package(from DB) which is to be updated 
            RequirementPackage existingRequirementPackage = _dbContext.RequirementPackages.Where(cond => !cond.RP_IsDeleted && cond.RP_ID == requirementPackageID).FirstOrDefault();
            if (!existingRequirementPackage.IsNullOrEmpty())
            {
                existingRequirementPackage.RP_IsDeleted = true;
                existingRequirementPackage.RP_ModifiedByID = currentLoggedInUserID;
                existingRequirementPackage.RP_ModifiedOn = DateTime.Now;
            }
            if (_dbContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        Boolean IRequirementPackageRepository.SaveContextIntoDataBase()
        {
            if (_dbContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        List<RequirementObjectTree> IRequirementPackageRepository.GetRequirementObjectTreeList(List<Int32> ReqObjectTreeIds)
        {
            return ClientDBContext.RequirementObjectTrees.Include("RequirementObjectRules")
                                                    .Include("RequirementObjectRules.RequirementObjectRuleDetails")
                                                    .Include("RequirementObjectTreeProperties")
                                                    .Where(cond => ReqObjectTreeIds.Contains(cond.ROT_ID) && !cond.ROT_IsDeleted).ToList();
        }

        RequirementObjectTree IRequirementPackageRepository.GetRequirementObjectTree(Int32 reqObjectTreeId)
        {
            return ClientDBContext.RequirementObjectTrees.Include("RequirementObjectRules")
                                                    .Include("RequirementObjectRules.RequirementObjectRuleDetails")
                                                    .Include("RequirementObjectTreeProperties")
                                                    .Where(cond => cond.ROT_ID == reqObjectTreeId && !cond.ROT_IsDeleted).FirstOrDefault();
        }

        Boolean IRequirementPackageRepository.AddLargeContentToDatabase(List<LargeContent> largeContentList)
        {
            foreach (LargeContent largeContent in largeContentList)
            {
                ClientDBContext.LargeContents.AddObject(largeContent);
            }
            if (ClientDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        Boolean IRequirementPackageRepository.AddLargeContentToContext(LargeContent largeContent)
        {
            ClientDBContext.LargeContents.AddObject(largeContent);
            return true;
        }

        LargeContent IRequirementPackageRepository.GetLargeContentForReqrmntCategory(Int32 reqrmntCategoryID, Int32 objectTypeID, Int32 contentTypeID)
        {
            return ClientDBContext.LargeContents
                    .Where(cond => cond.LC_ObjectID == reqrmntCategoryID && cond.LC_LargeContentTypeID == contentTypeID
                        && cond.LC_ObjectTypeID == objectTypeID && !cond.LC_IsDeleted).FirstOrDefault();

        }

        /// <summary>
        /// To get Requirement Packages
        /// </summary>
        /// <param name="agencyId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        List<RequirementPackageContract> IRequirementPackageRepository.GetInstructorRequirementPackages(String agencyId)
        {
            List<RequirementPackageContract> requirementPackageList = new List<RequirementPackageContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyID", agencyId)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetInstructorRequirementPackages", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementPackageContract requirementPackageContract = new RequirementPackageContract();
                            requirementPackageContract.RequirementPackageID = dr["RequirementPackageID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RequirementPackageID"]);
                            requirementPackageContract.RequirementPackageName = Convert.ToString(dr["RequirementPackageName"]);
                            requirementPackageContract.RequirementPackageLabel = Convert.ToString(dr["RequirementPackageLabel"]);
                            requirementPackageContract.RequirementPackageCode = dr["RequirementPackageCode"] == DBNull.Value ? Guid.Empty : (Guid)(dr["RequirementPackageCode"]);
                            requirementPackageContract.IsCopied = dr["IsCopied"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsCopied"]);
                            requirementPackageContract.IsSharedUserPackage = false;
                            requirementPackageContract.IsNewPackage = dr["IsNewPackage"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsNewPackage"]);
                            requirementPackageContract.EffectiveStartDate = dr["RequirementEffectiveStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RequirementEffectiveStartDate"]);
                            requirementPackageContract.EffectiveEndDate = dr["RequirementEffectiveEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RequirementEffectiveEndDate"]);
                            #region UAT-4657
                            requirementPackageContract.RootParentCode = dr["RootParentCode"] == DBNull.Value ? Guid.Empty : (Guid)(dr["RootParentCode"]); ;
                            //requirementPackageContract.RootParentID = dr["RootParentID"] != DBNull.Value ? Convert.ToInt32(dr["RootParentID"]) : AppConsts.NONE;
                            #endregion
                            requirementPackageList.Add(requirementPackageContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return requirementPackageList.OrderBy(col => col.RequirementPackageName).ToList();
        }

        List<RequirementPackageContract> IRequirementPackageRepository.GetAllRequirementPackages()
        {
            List<RequirementPackageContract> lstRequirementPackages = new List<RequirementPackageContract>();
            List<RequirementPackage> lstRequirementPackage = _dbContext.RequirementPackages.Where(cond => !cond.RP_IsDeleted).ToList();
            lstRequirementPackage.ForEach(col => lstRequirementPackages.Add(new RequirementPackageContract()
            {
                RequirementPackageID = col.RP_ID,
                RequirementPackageName = col.RP_PackageLabel.IsNullOrEmpty() ? col.RP_PackageName : col.RP_PackageLabel
            }));
            return lstRequirementPackages;
        }

        /// <summary>
        /// get categories required for compliance action.
        /// </summary>
        /// <returns></returns>
        DataTable IRequirementPackageRepository.GetRequirementCategoriesRqdForComplianceAction()
        {
            EntityConnection connection = ClientDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("[dbo].[usp_GetRotCategoriesRqdForComplianceAction]", con);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return new DataTable();

        }

        /// <summary>
        /// To process Required Category and insert data in ScheduledAction table
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="categoryId"></param>
        /// <param name="currentUserId"></param>
        public Dictionary<Int32,Boolean> ProcessRotcomplianceRqdChange(Int32 currentUserId, RequirementPackageCategory requirementPackageCategory)
        {
            EntityConnection connection = ClientDBContext.Connection as EntityConnection;
            Dictionary<Int32, Boolean> lstRPSID = new Dictionary<Int32, Boolean>();

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
               {
                    new SqlParameter("@PackageID", requirementPackageCategory.RPC_RequirementPackageID),
                    new SqlParameter("@CategoryID", requirementPackageCategory.RPC_RequirementCategoryID),
                    new SqlParameter("@IsComplianceRqd", requirementPackageCategory.RPC_ComplianceRequired),
                    new SqlParameter("@SystemUserID", currentUserId)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_ProcessRotcomplianceRqdChange", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Int32 key = Convert.ToInt32(dr["RPSID"]);
                            Boolean value = Convert.ToBoolean(dr["IsCatApprMailNeedToSend"]);
                            lstRPSID.Add(key, value);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return lstRPSID;
        }

        #region UAT-2514 Copy Package
        /// <summary>
        /// Add in RequirementObjectTree
        /// </summary>
        /// <param name="requirementCategoryID"></param>
        /// <param name="catObjectTypeId"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        List<RequirementObjectTreeContract> IRequirementPackageRepository.AddRequirementObjectTreeNew(Int32 requirementCategoryID, Int32 catObjectTypeId, Int32 currentUserID)
        {
            List<RequirementObjectTreeContract> requirementObjectTreeList = new List<RequirementObjectTreeContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@ParentObjectID", null),
                    new SqlParameter("@ParentObjectTypeID", null),
                    new SqlParameter("@NewObjectID", requirementCategoryID),
                    new SqlParameter("@NewObjectTypeID", catObjectTypeId),
                    new SqlParameter("@UserID", currentUserID)

                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_AddRequirementObjectTree", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementObjectTreeContract requirementObjectTree = new RequirementObjectTreeContract();
                            requirementObjectTree.RequirementObjectTreeID = dr["ROTID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ROTID"]);
                            requirementObjectTree.ObjectID = dr["ObjectID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ObjectID"]);
                            requirementObjectTree.ObjectTypeID = dr["ObjectTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ObjectTypeID"]);
                            requirementObjectTree.ParentID = dr["ParentID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ParentID"]);
                            requirementObjectTree.ParentObjectID = dr["ParentObjectID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ParentObjectID"]);
                            requirementObjectTree.IsNewRecordInserted = dr["IsNewRecord"] == DBNull.Value ? true : Convert.ToBoolean(dr["IsNewRecord"]);

                            requirementObjectTreeList.Add(requirementObjectTree);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return requirementObjectTreeList;
        }
        /// <summary>
        /// Get Requirement Category If it already exists, check on the bais of CategoryCode
        /// </summary>
        /// <param name="requirementCategoryCode"></param>
        /// <returns></returns>

        RequirementCategory IRequirementPackageRepository.GetRequirementCategoryIfAlreadyExists(Guid requirementCategoryCode)
        {
            return _dbContext.RequirementCategories.Where(cond => cond.RC_Code == requirementCategoryCode && !cond.RC_IsDeleted).FirstOrDefault();
        }

        /// <summary>
        ///  Get Requirement Item If it already exists, check on the bais of CategoryCode
        /// </summary>
        /// <param name="requirementItemCode"></param>
        /// <returns></returns>
        RequirementItem IRequirementPackageRepository.GetRequirementItemIfAlreadyExists(Guid requirementItemCode)
        {
            return _dbContext.RequirementItems.Where(cond => cond.RI_Code == requirementItemCode && !cond.RI_IsDeleted).FirstOrDefault();
        }
        /// <summary>
        /// Check If Category ID Already Exists in Object Tree
        /// </summary>
        /// <param name="requirementCategoryID"></param>
        /// <param name="catObjectTypeId"></param>
        /// <returns></returns>

        Boolean IRequirementPackageRepository.IfCategoryIdAlreadyExistsInObjectTree(Int32 requirementCategoryID, Int32 catObjectTypeId)
        {
            return _dbContext.RequirementObjectTrees.Any(cond => cond.ROT_ObjectID == requirementCategoryID && cond.ROT_ObjectTypeID == catObjectTypeId && cond.ROT_ParentID == null && !cond.ROT_IsDeleted);
        }

        #endregion

        #region UAT-2514
        String IRequirementPackageRepository.RequirementPkgSync(Int32 currentLoggedInUserId, String reqPackageObjectIds)
        {
            string packageSubscriptionIDs = string.Empty;
            EntityConnection connection = ClientDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_SyncDataWithRequirementPackages", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CurrentOrgUserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@SyncRequirementPackageObjectIds", reqPackageObjectIds);
                command.Parameters.Add("@VerificationDetailIds", SqlDbType.VarChar,500).Direction = ParameterDirection.Output;
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();

                packageSubscriptionIDs = Convert.ToString(command.Parameters["@VerificationDetailIds"].Value);

                con.Close();
            }
            return packageSubscriptionIDs;
        }
        #endregion


        /// <summary>
        /// call usp_syncRequirementVerificationDataToFlatTable after requirenment package sync
        /// </summary>
        /// <param name="packageSubscriptionIDs"></param>
        /// <param name="currentLoggedInUserId"></param>
        #region 
        void IRequirementPackageRepository.RequirementVerificationDataToFlatTable(String packageSubscriptionIDs,Int32 currentLoggedInUserId , Int32 taskId)
        {
            EntityConnection connection = ClientDBContext.Connection as EntityConnection;
            if (!packageSubscriptionIDs.IsNullOrWhiteSpace())
            {
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("usp_syncRequirementVerificationDataToFlatTable", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PackageSubscriptionIDs", packageSubscriptionIDs);
                    command.Parameters.AddWithValue("@CurrentLoggedInUSerID", currentLoggedInUserId);
                    command.Parameters.AddWithValue("@ScheduleTaskId", taskId);
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        #endregion

        /// <summary>
        /// In case of usp_syncRequirementVerificationDataToFlatTable throw timeout exception store packageIds  in scheduleTask.
        /// </summary>
        /// <param name="packageSubscriptionIDs"></param>
        /// <param name="currentLoggedInUserId"></param>
        #region
        void IRequirementPackageRepository.StoreRequirenmentPackageIdsInScheduleTask(String packageSubscriptionIDs, Int32 currentLoggedInUserId)
        {
            EntityConnection connection = ClientDBContext.Connection as EntityConnection;
            if (!packageSubscriptionIDs.IsNullOrWhiteSpace())
            {
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("usp_StoreRequirenmentPackageIdsInScheduleTask", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PackageSubscriptionIDs", packageSubscriptionIDs);
                    command.Parameters.AddWithValue("@CurrentLoggedInUSerID", currentLoggedInUserId);
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        #endregion

        List<ScheduledTask> IRequirementPackageRepository.GetRequirenmentPackageIdsInScheduleTask(Int32 tasktypeid , Int32 taskstatustypeIdPending)
        {
           
            List<ScheduledTask> lstScheduledTasks = _dbContext.ScheduledTasks.Where(con => con.ST_TaskTypeID == tasktypeid
             && con.ST_TaskStatusID == taskstatustypeIdPending && !con.ST_IsDeleted).ToList();

            return lstScheduledTasks;
        }

        /// <summary>
        /// UAT-2423 Get the Rotation Package Category name and ExplanatoryNotes using rotation ID 
        /// </summary>
        /// <param name="rotationID"></param>
        /// <returns></returns>
        List<RequirementCategoryContract> IRequirementPackageRepository.GetRotationPackageCategoryDetailByRotationID(Int32 rotationID, Boolean IsStudentPackage)
        {
            List<RequirementCategoryContract> requirementCategoryDetails = new List<RequirementCategoryContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@ClinicalRotationID", rotationID),
                    new SqlParameter("@IsStudentPackage",IsStudentPackage)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRotationPackageCategoryDetail", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementCategoryContract requirementPackageContract = new RequirementCategoryContract();
                            requirementPackageContract.RequirementCategoryName = dr["CategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CategoryName"]);
                            requirementPackageContract.ExplanatoryNotes = dr["ExplanatoryNotes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ExplanatoryNotes"]);
                            requirementCategoryDetails.Add(requirementPackageContract);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return requirementCategoryDetails;
        }


        #region UAT-2973

        /// <summary>
        /// To get Requirement Packages
        /// </summary>
        /// <param name="agencyId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        List<RequirementPackageContract> IRequirementPackageRepository.GetRequirementPackagesFromAgencyIds(String agencyId, String reqPkgTypeCode = null)
        {
            List<RequirementPackageContract> requirementPackageList = new List<RequirementPackageContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyID", agencyId),
                    new SqlParameter("@RotationPackageTypeCode",(reqPkgTypeCode.IsNullOrEmpty()? null : reqPkgTypeCode))
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementPackagesByAgencyIds", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementPackageContract requirementPackageContract = new RequirementPackageContract();
                            requirementPackageContract.RequirementPackageID = dr["RequirementPackageID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RequirementPackageID"]);
                            requirementPackageContract.RequirementPackageName = Convert.ToString(dr["RequirementPackageName"]);
                            requirementPackageContract.RequirementPackageCode = dr["RequirementPackageCode"] == DBNull.Value ? Guid.Empty : (Guid)(dr["RequirementPackageCode"]);
                            requirementPackageContract.IsCopied = dr["IsCopied"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsCopied"]);
                            //UAT-2514--Retrieved NewPackage Property
                            requirementPackageContract.IsNewPackage = dr["IsNewPackage"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsNewPackage"]);
                            requirementPackageContract.IsSharedUserPackage = false;
                            requirementPackageContract.IsNewPackage = dr["IsNewPackage"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsNewPackage"]);
                            requirementPackageContract.EffectiveStartDate = dr["RequirementEffectiveStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RequirementEffectiveStartDate"]);
                            requirementPackageContract.EffectiveEndDate = dr["RequirementEffectiveEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RequirementEffectiveEndDate"]);
                            #region UAT-4657
                            requirementPackageContract.RootParentCode = dr["RootParentCode"] == DBNull.Value ? Guid.Empty : (Guid)(dr["RootParentCode"]); ;
                           // requirementPackageContract.RootParentID = dr["RootParentID"] != DBNull.Value ? Convert.ToInt32(dr["RootParentID"]) : AppConsts.NONE;
                            #endregion
                            requirementPackageList.Add(requirementPackageContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return requirementPackageList.OrderBy(col => col.RequirementPackageName).ToList();
        }

        Boolean IRequirementPackageRepository.UpdateRequirementPackageAgencyMappings(List<Int32> lstNewRequirementPkgAgencies, Int32 currentLoggedInUserID, Int32 requirementPackageId)
        {

            foreach (Int32 agencyID in lstNewRequirementPkgAgencies)
            {
                RequirementPackageAgency requirementPackageAgency = new RequirementPackageAgency()
                {
                    RPA_AgencyID = agencyID,
                    RPA_IsDeleted = false,
                    RPA_CreatedByID = currentLoggedInUserID,
                    RPA_CreatedOn = DateTime.Now,
                    RPA_RequirementPackageID = requirementPackageId,
                };
                _dbContext.RequirementPackageAgencies.AddObject(requirementPackageAgency);
            }
            if (_dbContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;

        }
        #endregion

        /// <summary>
        /// UAT 3080
        /// </summary>
        /// <param name="packageSubscriptionId"></param>
        /// <returns></returns>
        String IRequirementPackageRepository.GetReqPackageSubsStatusBySubscriptionID(Int32 packageSubscriptionId)
        {
            return _dbContext.RequirementPackageSubscriptions.Where(cond => cond.RPS_ID == packageSubscriptionId && !cond.RPS_IsDeleted).Select(sel => sel.lkpRequirementPackageStatu.RPS_Code).FirstOrDefault();
        }

        #region UAT 3120
        /// <summary>
        /// Get ',' seperated Rotation Institute Hierarchy ID based on Package Subscription ID
        /// </summary>
        /// <param name="packageSubscriptionID"></param>
        /// <returns></returns>
        String IRequirementPackageRepository.GetRotationHierarchyIdsBasedOnSubscriptionID(Int32 packageSubscriptionID)
        {
            String HierarchyIds = String.Empty;
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        { 
                           new SqlParameter("@PackageSubscriptionID", packageSubscriptionID)
                        };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRotationHierarchyIDBasedOnPackageSubscriptionID", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            HierarchyIds = dr["HierarchyNodeIds"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodeIds"]);
                        }
                    }
                }
            }
            return HierarchyIds;
        }
        #endregion
    }
}

