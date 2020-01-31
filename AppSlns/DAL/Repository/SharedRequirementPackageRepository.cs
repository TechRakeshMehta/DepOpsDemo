using CoreWeb.IntsofLoggerModel.Interface;
using DAL.Interfaces;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.Repository
{
    public class SharedRequirementPackageRepository : BaseRepository, ISharedRequirementPackageRepository
    {
        #region Variables

        #region public Variables

        #endregion

        #region Private Variables

        private ADB_SharedDataEntities _sharedDataDBContext;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor to initialize the context
        /// </summary>
        public SharedRequirementPackageRepository()
        {
            _sharedDataDBContext = base.SharedDataDBContext;
        }

        #endregion

        #region Methods
        Boolean ISharedRequirementPackageRepository.AddRequirementPackageToDatabase(RequirementPackage requirementPackage)
        {
            _sharedDataDBContext.RequirementPackages.AddObject(requirementPackage);
            if (_sharedDataDBContext.SaveChanges() > 0)
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
        List<RequirementPackageDetailsContract> ISharedRequirementPackageRepository.GetRequirementPackageDetailsByPackageID(Int32 requirementPackageID)
        {
            List<RequirementPackageDetailsContract> requirementPackageDetailsList = new List<RequirementPackageDetailsContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
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
                            //requirementPackageDetail.RequirementPackageLabel = dr["RequirementPackageLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageLabel"]);
                            requirementPackageDetail.RequirementCategoryID = dr["RequirementCategoryID"] != DBNull.Value ? Convert.ToInt32(dr["RequirementCategoryID"]) : AppConsts.NONE;
                            requirementPackageDetail.RequirementCategoryName = dr["RequirementCategoryName"] != DBNull.Value ? Convert.ToString(dr["RequirementCategoryName"]) : String.Empty;
                            requirementPackageDetail.RequirementItemID = dr["RequirementItemID"] != DBNull.Value ? Convert.ToInt32(dr["RequirementItemID"]) : 0;
                            requirementPackageDetail.RequirementItemName = dr["RequirementItemName"] != DBNull.Value ? Convert.ToString(dr["RequirementItemName"]) : String.Empty;
                            requirementPackageDetail.RequirementFieldID = dr["RequirementFieldID"] != DBNull.Value ? Convert.ToInt32(dr["RequirementFieldID"]) : 0;
                            requirementPackageDetail.RequirementFieldName = dr["RequirementFieldName"] != DBNull.Value ? Convert.ToString(dr["RequirementFieldName"]) : String.Empty;

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
        List<RequirementPackageContract> ISharedRequirementPackageRepository.GetRequirementPackages(Int32 selectedTenantID, String agencyID)
        {
            List<RequirementPackageContract> requirementPackageList = new List<RequirementPackageContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyID", agencyID),
                    new SqlParameter("@SelectedTenantID", selectedTenantID)
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
                            //requirementPackageContract.RequirementPackageLabel = dr["RequirementPackageLabel"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RequirementPackageLabel"]);
                            requirementPackageContract.RequirementPackageCode = (Guid)(dr["RequirementPackageCode"]);
                            requirementPackageContract.IsUsed = dr["IsUsed"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsUsed"]);
                            requirementPackageContract.IsSharedUserPackage = true;

                            //UAT-2514 Retrieved IsNewPackage Column                           
                            requirementPackageContract.IsNewPackage = dr["IsNewPackage"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsNewPackage"]);
                            requirementPackageContract.EffectiveStartDate = dr["RequirementEffectiveStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RequirementEffectiveStartDate"]);
                            requirementPackageContract.EffectiveEndDate = dr["RequirementEffectiveEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RequirementEffectiveEndDate"]);
                            #region UAT-4657
                            requirementPackageContract.RootParentCode = dr["RootParentCode"] == DBNull.Value ? Guid.Empty : (Guid)(dr["RootParentCode"]);
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
        List<RequirementPackageDetailsContract> ISharedRequirementPackageRepository.GetRequirementPackageDetails(RequirementPackageDetailsContract requirementPackageDetailsContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<RequirementPackageDetailsContract> requirementPackageDetailList = new List<RequirementPackageDetailsContract>();
            String orderBy = "RequirementPackageID";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyIDs", requirementPackageDetailsContract.LstAgencyIDs.IsNullOrEmpty()?null:requirementPackageDetailsContract.LstAgencyIDs),
                    new SqlParameter("@PackageName", requirementPackageDetailsContract.RequirementPackageName),
                    new SqlParameter("@RotationPackageTypeID", (requirementPackageDetailsContract.RequirementPkgTypeID == AppConsts.NONE? null : requirementPackageDetailsContract.RequirementPkgTypeID)), //UAT1352
                    new SqlParameter("@PackageActiveStatus", requirementPackageDetailsContract.IsActivePackage),
                    new SqlParameter("@DefinedRequirementID", requirementPackageDetailsContract.DefinedRequirementID),
                    new SqlParameter("@OrderBy", orderBy),
                    new SqlParameter("@OrderDirection", ordDirection),
                    new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                    new SqlParameter("@PageSize", customPagingArgsContract.PageSize),
                    new SqlParameter("@CurrentUserID",(requirementPackageDetailsContract.CurrentUserID)),
                    new SqlParameter("@ShowAllAgencies",(requirementPackageDetailsContract.ShowAllAgencies))
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
                            requirementPackageDetail.IsPackageUsed = dr["IsPackageUsed"] != DBNull.Value ? Convert.ToBoolean(dr["IsPackageUsed"]) : false;
                            requirementPackageDetail.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                            requirementPackageDetail.PackageObjectTreeID = dr["PackageObjectTreeID"] != DBNull.Value ? Convert.ToInt32(dr["PackageObjectTreeID"]) : 0;
                            requirementPackageDetail.RequirementPkgTypeName = dr["RequirementPkgTypeName"] != DBNull.Value ? Convert.ToString(dr["RequirementPkgTypeName"]) : String.Empty;
                            requirementPackageDetail.AgencyNames = dr["Agencies"] != DBNull.Value ? Convert.ToString(dr["Agencies"]) : String.Empty;
                            requirementPackageDetail.IsSharedUserPackage = true;
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
        List<RequirementPackageHierarchicalDetailsContract> ISharedRequirementPackageRepository.GetRequirementPackageHierarchalDetailsByPackageID(Int32 requirementPackageID)
        {
            List<RequirementPackageHierarchicalDetailsContract> requirementPackageHierarchicalDetailList = new List<RequirementPackageHierarchicalDetailsContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
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
                            requirementPackageHierarchicalDetail.RequirementFieldAttributeTypeID = dr["RequirementFieldAttributeTypeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementFieldAttributeTypeID"]);
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
                            requirementPackageHierarchicalDetail.RequirementPackageInstitutionID = dr["RequirementPackageInstitutionID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageInstitutionID"]);
                            requirementPackageHierarchicalDetail.MappedTenantID = dr["MappedTenantID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MappedTenantID"]);
                            requirementPackageHierarchicalDetail.MappedTenantName = dr["MappedTenantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["MappedTenantName"]);
                            requirementPackageHierarchicalDetail.IsUsed = dr["IsUsed"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsUsed"]);
                            requirementPackageHierarchicalDetail.RequirementCategorySQLExpression = dr["RequirementCategorySQLExpression"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementCategorySQLExpression"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryUIExpression = dr["RequirementCategoryUIExpression"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementCategoryUIExpression"]);

                            //UAT-2165
                            requirementPackageHierarchicalDetail.IsComplianceRequired = dr["IsComplianceRequired"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsComplianceRequired"]);
                            requirementPackageHierarchicalDetail.ComplianceReqStartDate = dr["ComplianceReqStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ComplianceReqStartDate"]);
                            requirementPackageHierarchicalDetail.ComplianceReqEndDate = dr["ComplianceReqEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ComplianceReqEndDate"]);
                            requirementPackageHierarchicalDetail.ExpirationCondEndDate = dr["ExpirationCondEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ExpirationCondEndDate"]);
                            requirementPackageHierarchicalDetail.ExpirationCondStartDate = dr["ExpirationCondStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ExpirationCondStartDate"]);

                            #region UAT-2164 : Agency User - Granular Permissions
                            requirementPackageHierarchicalDetail.IsBackgroundDocument = dr["IsBackgroundDocument"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsBackgroundDocument"]);
                            #endregion

                            //UAT-2366
                            requirementPackageHierarchicalDetail.UiRequirementItemID = dr["UiRequirementItemID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["UiRequirementItemID"]);
                            requirementPackageHierarchicalDetail.UiRequirementFieldID = dr["UiRequirementFieldID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["UiRequirementFieldID"]);
                            requirementPackageHierarchicalDetail.RequirementFieldFixedRuleTypeCode = dr["RequirementFieldFixedRuleTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementFieldFixedRuleTypeCode"]);
                            requirementPackageHierarchicalDetail.UiRuleErrorMessage = dr["ErrorMessage"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ErrorMessage"]);

                            //UAT-2332
                            requirementPackageHierarchicalDetail.DefinedRequirementDescription = dr["DefinedRequirementDescription"] != DBNull.Value ? Convert.ToString(dr["DefinedRequirementDescription"]) : String.Empty;
                            requirementPackageHierarchicalDetail.DefinedRequirementID = dr["DefinedRequirementID"] != DBNull.Value ? (Int32?)(dr["DefinedRequirementID"]) : null;

                            //UAT-3078
                            requirementPackageHierarchicalDetail.RequirementCategoryItemDisplayOrder = dr["RequirementCategoryItemDisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementCategoryItemDisplayOrder"]);
                            requirementPackageHierarchicalDetail.RequirementItemFieldDisplayOrder = dr["RequirementItemFieldDisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementItemFieldDisplayOrder"]);
                            //UAT-3176
                            requirementPackageHierarchicalDetail.RequirementAttributeGroupID = dr["RequirementAttributeGroupID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementAttributeGroupID"]);
                            //UAT-3309
                            requirementPackageHierarchicalDetail.RequirementItemSampleDocumentFormURL = dr["ReqItemSampleDocURL"] != DBNull.Value ? Convert.ToString(dr["ReqItemSampleDocURL"]) : String.Empty;

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
        List<RequirementObjectTreeContract> ISharedRequirementPackageRepository.AddRequirementObjectTree(Int32 requirementPackageID, Int32 pkgObjectTypeId, Int32 currentUserID)
        {
            List<RequirementObjectTreeContract> requirementObjectTreeList = new List<RequirementObjectTreeContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
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



        Boolean ISharedRequirementPackageRepository.IsPackageVersionNeedToCreate(Int32 requirementPackageID)
        {
            RequirementPackage packageInBD = _sharedDataDBContext.RequirementPackages
                            .Where(cond => cond.RP_ID == requirementPackageID && !cond.RP_IsDeleted).FirstOrDefault();
            return packageInBD.IsNullOrEmpty() ? false : (packageInBD.RP_IsUsed.HasValue ? packageInBD.RP_IsUsed.Value : false);
        }

        //Boolean ISharedRequirementPackageRepository.IsPackageMappedToRotation(Int32 requirementPackageID)
        //{
        //    return _sharedDataDBContext.ClinicalRotationRequirementPackages
        //         .Any(cond => cond.CRRP_RequirementPackageID == requirementPackageID && !cond.CRRP_IsDeleted);
        //}

        RequirementPackage ISharedRequirementPackageRepository.GetRequirementPackageByPackageID(Int32 requirementPackageID)
        {
            return _sharedDataDBContext.RequirementPackages.Where(cond => !cond.RP_IsDeleted && cond.RP_ID == requirementPackageID).FirstOrDefault();
        }


        Boolean ISharedRequirementPackageRepository.SaveContextIntoDataBase()
        {
            if (_sharedDataDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        List<RequirementObjectTree> ISharedRequirementPackageRepository.GetRequirementObjectTreeList(List<Int32> ReqObjectTreeIds)
        {
            return _sharedDataDBContext.RequirementObjectTrees.Include("RequirementObjectRules")
                                                    .Include("RequirementObjectRules.RequirementObjectRuleDetails")
                                                    .Include("RequirementObjectTreeProperties")
                                                    .Where(cond => ReqObjectTreeIds.Contains(cond.ROT_ID) && !cond.ROT_IsDeleted).ToList();
        }

        RequirementObjectTree ISharedRequirementPackageRepository.GetRequirementObjectTree(Int32 reqObjectTreeId)
        {
            return _sharedDataDBContext.RequirementObjectTrees.Include("RequirementObjectRules")
                                                    .Include("RequirementObjectRules.RequirementObjectRuleDetails")
                                                    .Include("RequirementObjectTreeProperties")
                                                    .Where(cond => cond.ROT_ID == reqObjectTreeId && !cond.ROT_IsDeleted).FirstOrDefault();
        }

        Boolean ISharedRequirementPackageRepository.AddLargeContentToDatabase(List<LargeContent> largeContentList)
        {
            foreach (LargeContent largeContent in largeContentList)
            {
                _sharedDataDBContext.LargeContents.AddObject(largeContent);
            }
            if (_sharedDataDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }

        Boolean ISharedRequirementPackageRepository.AddLargeContentToContext(LargeContent largeContent)
        {
            _sharedDataDBContext.LargeContents.AddObject(largeContent);
            return true;
        }

        LargeContent ISharedRequirementPackageRepository.GetLargeContentForReqrmntCategory(Int32 reqrmntCategoryID, Int32 objectTypeID, Int32 contentTypeID)
        {
            return _sharedDataDBContext.LargeContents
                    .Where(cond => cond.LC_ObjectID == reqrmntCategoryID && cond.LC_LargeContentTypeID == contentTypeID
                        && cond.LC_ObjectTypeID == objectTypeID && !cond.LC_IsDeleted).FirstOrDefault();

        }

        //Dictionary<Int32, List<Int32>> ISharedRequirementPackageRepository.GetTenantIDsMappedForAgencyUser(Guid userID)
        //{
        //    Dictionary<Int32, List<Int32>> responseDictionary = new Dictionary<Int32, List<Int32>>();
        //    List<Int32> lstTenantIDs = new List<Int32>();
        //    AgencyUser agencyUser = _sharedDataDBContext.AgencyUsers.Where(au => au.AGU_UserID == userID && !au.AGU_IsDeleted)
        //                               .FirstOrDefault();
        //    #region //rachit-1641
        //    //List<AgencyUserInstitution> lstAgencyUserInstitution = agencyUser.AgencyUserInstitutions.Where(cond => !cond.AGUI_IsDeleted).ToList();
        //    //if (!lstAgencyUserInstitution.IsNullOrEmpty())
        //    //{
        //    //    List<AgencyInstitution> lstAgencyInstitution = lstAgencyUserInstitution.Select(col => col.AgencyInstitution).ToList();
        //    //    if (!lstAgencyInstitution.IsNullOrEmpty())
        //    //    {
        //    //        lstTenantIDs = lstAgencyInstitution.Select(col => col.AGI_TenantID.HasValue ? col.AGI_TenantID.Value : 0).ToList();
        //    //    }
        //    //}

        //    //responseDictionary.Add(agencyUser.AGU_AgencyID, lstTenantIDs);

        //    #endregion
        //    return responseDictionary;

        //}

        /// <summary>
        /// To get Requirement Packages
        /// </summary>
        /// <param name="agencyId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        List<RequirementPackageContract> ISharedRequirementPackageRepository.GetInstructorRequirementPackages(Int32 selectedTenantID, String agencyId)
        {
            List<RequirementPackageContract> requirementPackageList = new List<RequirementPackageContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyID", agencyId),
                     new SqlParameter("@SelectedTenantID", selectedTenantID)
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
                            requirementPackageContract.RequirementPackageCode = (Guid)(dr["RequirementPackageCode"]);
                            requirementPackageContract.IsUsed = dr["IsUsed"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsUsed"]);
                            requirementPackageContract.IsSharedUserPackage = true;
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
        #endregion

        Int32 ISharedRequirementPackageRepository.SaveRequirementPackageData(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID)
        {
            RequirementPackage rqmntPkg;
            if (requirementPackageContract.RequirementPackageID > AppConsts.NONE)
            {
                rqmntPkg = _sharedDataDBContext.RequirementPackages.FirstOrDefault(cond => cond.RP_ID == requirementPackageContract.RequirementPackageID);
                rqmntPkg.RP_PackageName = requirementPackageContract.RequirementPackageName;
                rqmntPkg.RP_PackageLabel = requirementPackageContract.RequirementPackageLabel;
                rqmntPkg.RP_ModifiedByID = currentLoggedInUserID;
                rqmntPkg.RP_ModifiedOn = DateTime.Now;
                rqmntPkg.RP_RequirementPackageTypeID = requirementPackageContract.RequirementPkgTypeID;
                rqmntPkg.RP_DefinedRequirementID = requirementPackageContract.DefinedRequirementID;
                rqmntPkg.RP_ReqReviewByID = requirementPackageContract.ReqReviewByID;

                //if (!rqmntPkg.IsNullOrEmpty())
                //{
                //    IEnumerable<RequirementPackageAgency> existingRequirementPackageAgencies = rqmntPkg.RequirementPackageAgencies
                //                                                                                                 .Where(cond => !cond.RPA_IsDeleted);
                //    foreach (RequirementPackageAgency existingRequirementPackageAgency in existingRequirementPackageAgencies)
                //    {
                //        if (!requirementPackageContract.LstAgencyIDs.Contains(existingRequirementPackageAgency.RPA_AgencyID))
                //        {
                //            existingRequirementPackageAgency.RPA_IsDeleted = true;
                //            existingRequirementPackageAgency.RPA_ModifiedByID = currentLoggedInUserID;
                //            existingRequirementPackageAgency.RPA_ModifiedOn = DateTime.Now;
                //        }
                //        else
                //        {
                //            requirementPackageContract.LstAgencyIDs.Remove(existingRequirementPackageAgency.RPA_AgencyID);
                //        }
                //    }
                //    foreach (Int32 agencyID in requirementPackageContract.LstAgencyIDs)
                //    {
                //        RequirementPackageAgency newRequirementPackageAgency = new RequirementPackageAgency()
                //        {
                //            RPA_AgencyID = agencyID,
                //            RPA_IsDeleted = false,
                //            RPA_CreatedByID = currentLoggedInUserID,
                //            RPA_CreatedOn = DateTime.Now,
                //        };
                //        rqmntPkg.RequirementPackageAgencies.Add(newRequirementPackageAgency);
                //    }
                //}
            }
            else
            {
                rqmntPkg = new RequirementPackage();
                rqmntPkg.RP_PackageName = requirementPackageContract.RequirementPackageName;
                rqmntPkg.RP_PackageLabel = requirementPackageContract.RequirementPackageLabel;
                rqmntPkg.RP_CreatedByID = currentLoggedInUserID;
                rqmntPkg.RP_CreatedOn = DateTime.Now;
                rqmntPkg.RP_IsDeleted = false;
                rqmntPkg.RP_IsActive = true;
                rqmntPkg.RP_IsUsed = false;
                rqmntPkg.RP_IsCopied = false;
                rqmntPkg.RP_Code = requirementPackageContract.RequirementPackageCode;
                rqmntPkg.RP_RequirementPackageTypeID = requirementPackageContract.RequirementPkgTypeID;
                rqmntPkg.RP_DefinedRequirementID = requirementPackageContract.DefinedRequirementID;
                rqmntPkg.RP_ReqReviewByID = requirementPackageContract.ReqReviewByID;

                //foreach (Int32 agencyID in requirementPackageContract.LstAgencyIDs)
                //{
                //    RequirementPackageAgency newRequirementPackageAgency = new RequirementPackageAgency()
                //    {
                //        RPA_AgencyID = agencyID,
                //        RPA_IsDeleted = false,
                //        RPA_CreatedByID = currentLoggedInUserID,
                //        RPA_CreatedOn = DateTime.Now,
                //    };
                //    rqmntPkg.RequirementPackageAgencies.Add(newRequirementPackageAgency);
                //}
                _sharedDataDBContext.RequirementPackages.AddObject(rqmntPkg);
            }
            SaveUpdateAgencyHierarchyPackage(requirementPackageContract.LstAgencyHierarchyIDs, rqmntPkg, currentLoggedInUserID); //UAT-2648
            _sharedDataDBContext.SaveChanges();
            return rqmntPkg.RP_ID;
        }

        #region UAT-1837:ADB Admin streamlined create and edit rotation packages
        /// <summary>
        /// used to get requirement Item detail
        /// </summary>
        /// <returns></returns>
        RequirementItemContract ISharedRequirementPackageRepository.GetRequirementItemDetailsByItemID(Int32 requirementItemID, Int32? requirementCategoryID = null)
        {
            RequirementItemContract reqItemContract = new RequirementItemContract();
            List<RequirementFieldContract> reqFieldContractList = new List<RequirementFieldContract>();
            RequirementItem reqItem = _sharedDataDBContext.RequirementItems.FirstOrDefault(item => item.RI_ID == requirementItemID && !item.RI_IsDeleted);
            if (!reqItem.IsNullOrEmpty())
            {
                reqItemContract.RequirementItemID = reqItem.RI_ID;
                reqItemContract.RequirementItemName = reqItem.RI_ItemName;
                reqItemContract.RequirementItemLabel = reqItem.RI_ItemLabel;
                reqItemContract.RequirementItemNotes = reqItem.RI_Description; //UAT-2676
                reqItemContract.AllowDataMovement = reqItem.RI_AllowDataMovement;
                reqItemContract.RequirementItemSampleDocumentFormURL = reqItem.RI_SampleDocFormURL; //UAT-3309
                #region UAT-3078
                RequirementCategoryItem reqCatItem;
                if (!requirementCategoryID.Value.IsNullOrEmpty() && Convert.ToInt32(requirementCategoryID.Value) > 0) //Issue 1 Release 184 (BugID: 25118)
                    reqCatItem = reqItem.RequirementCategoryItems.FirstOrDefault(item => item.RCI_RequirementItemID == requirementItemID && !item.RCI_IsDeleted && item.RCI_RequirementCategoryID == requirementCategoryID.Value);
                else
                    reqCatItem = reqItem.RequirementCategoryItems.FirstOrDefault(item => item.RCI_RequirementItemID == requirementItemID && !item.RCI_IsDeleted);
                reqItemContract.RequirementItemDisplayOrder = reqCatItem.RCI_DisplayOrder.HasValue ? reqCatItem.RCI_DisplayOrder.Value : AppConsts.NONE; //UAT-3078
                #endregion
                //UAT-3077
                reqItemContract.IsPaymentType = reqItem.RI_IsPaymentType.IsNullOrEmpty() ? false : reqItem.RI_IsPaymentType.Value;
                reqItemContract.Amount = reqItem.RI_Amount;
                //UAT 3792
                reqItemContract.AllowItemDataEntry = reqItem.RI_AllowItemDataEntry.HasValue ? reqItem.RI_AllowItemDataEntry.Value : true;
                var ListRequirementItemURLs = _sharedDataDBContext.RequirementItemURLs.Where(item => item.RIU_RequirementItemId == requirementItemID && !item.RIU_IsDeleted).ToList();

                //UAT:4121
                if (ListRequirementItemURLs != null && ListRequirementItemURLs.Count > AppConsts.NONE)
                {
                    reqItemContract.listRequirementItemURLContract = new List<RequirementItemURLContract>();
                    ListRequirementItemURLs.ForEach(itemURL =>
                    {
                        RequirementItemURLContract ObjRequirementItemURLContract = new RequirementItemURLContract();
                        ObjRequirementItemURLContract.RItemURLID = itemURL.RIU_ID;
                        ObjRequirementItemURLContract.RItemURLLabel = itemURL.RIU_Label;
                        ObjRequirementItemURLContract.RItemURLSampleDocURL = itemURL.RIU_SampleDocURL;
                        reqItemContract.listRequirementItemURLContract.Add(ObjRequirementItemURLContract);
                    });
                }


                var itemFields = reqItem.RequirementItemFields.Where(cnd => !cnd.RIF_IsDeleted).ToList();
                itemFields.ForEach(itmFld =>
                {
                    RequirementFieldContract reqFieldContract = new RequirementFieldContract();
                    reqFieldContract.RequirementFieldID = itmFld.RIF_RequirementFieldID;
                    reqFieldContract.RequirementItemID = itmFld.RIF_RequirementItemID;
                    reqFieldContract.RequirementItemFieldID = itmFld.RIF_ID;

                    RequirementFieldDataContract reqFieldDataContract = new RequirementFieldDataContract();
                    reqFieldDataContract.RequirementFieldDataTypeCode = itmFld.RequirementField.lkpRequirementFieldDataType.RFDT_Code;
                    reqFieldDataContract.RequirementFieldDataTypeID = itmFld.RequirementField.lkpRequirementFieldDataType.RFDT_ID;
                    reqFieldDataContract.RequirementFieldDataTypeName = itmFld.RequirementField.lkpRequirementFieldDataType.RFDT_Name;
                    reqFieldContract.RequirementFieldData = reqFieldDataContract;
                    reqFieldContractList.Add(reqFieldContract);
                });
                reqItemContract.LstRequirementField = reqFieldContractList;

                RequirementObjectProperty requirementObjProp = _sharedDataDBContext.RequirementObjectProperties
                                                            .Where(cond => cond.ROTP_CategoryItemID == reqCatItem.RCI_ID && cond.ROTP_CategoryID == requirementCategoryID && !cond.ROTP_IsDeleted && cond.ROTP_ItemFieldID == null)
                                                            .FirstOrDefault();

                if (!requirementObjProp.IsNullOrEmpty())
                {
                    Dictionary<String, Boolean> dicData = new Dictionary<String, Boolean>();

                    if (!requirementObjProp.ROTP_IsCustomSettings.IsNullOrEmpty() && Convert.ToBoolean(requirementObjProp.ROTP_IsCustomSettings))
                    {
                        dicData.Add(RotationDataEditableBy.ADBAdmin.GetStringValue(), requirementObjProp.ROTP_IsEditableByAdmin);
                        dicData.Add(RotationDataEditableBy.ClientAdmin.GetStringValue(), requirementObjProp.ROTP_IsEditableByClientAdmin);
                        dicData.Add(RotationDataEditableBy.Applicant.GetStringValue(), requirementObjProp.ROTP_IsEditableByApplicant);
                        reqItemContract.IsCustomSetting = true;
                    }
                    else
                    {
                        reqItemContract.IsCustomSetting = false;
                        RequirementObjectProperty requirementObjPropCatData = _sharedDataDBContext.RequirementObjectProperties
                                                        .Where(cond => cond.ROTP_CategoryID == reqCatItem.RCI_RequirementCategoryID && !cond.ROTP_IsDeleted && cond.ROTP_CategoryItemID == null)
                                                        .FirstOrDefault();

                        if (!requirementObjPropCatData.IsNullOrEmpty())
                        {

                            dicData.Add(RotationDataEditableBy.ADBAdmin.GetStringValue(), requirementObjPropCatData.ROTP_IsEditableByAdmin);
                            dicData.Add(RotationDataEditableBy.ClientAdmin.GetStringValue(), requirementObjPropCatData.ROTP_IsEditableByClientAdmin);
                            dicData.Add(RotationDataEditableBy.Applicant.GetStringValue(), requirementObjPropCatData.ROTP_IsEditableByApplicant);
                        }
                        else
                        {
                            dicData.Add(RotationDataEditableBy.ADBAdmin.GetStringValue(), false);
                            dicData.Add(RotationDataEditableBy.ClientAdmin.GetStringValue(), false);
                            dicData.Add(RotationDataEditableBy.Applicant.GetStringValue(), false);
                        }
                    }

                    reqItemContract.SelectedEditableBy = dicData;
                }
                else
                {
                    Dictionary<String, Boolean> dicData = new Dictionary<String, Boolean>();
                    reqItemContract.IsCustomSetting = false;
                    RequirementObjectProperty requirementObjPropCatData = _sharedDataDBContext.RequirementObjectProperties
                                                    .Where(cond => cond.ROTP_CategoryID == reqCatItem.RCI_RequirementCategoryID && !cond.ROTP_IsDeleted && cond.ROTP_CategoryItemID == null && cond.ROTP_ItemFieldID == null)
                                                    .FirstOrDefault();

                    if (!requirementObjPropCatData.IsNullOrEmpty())
                    {

                        dicData.Add(RotationDataEditableBy.ADBAdmin.GetStringValue(), requirementObjPropCatData.ROTP_IsEditableByAdmin);
                        dicData.Add(RotationDataEditableBy.ClientAdmin.GetStringValue(), requirementObjPropCatData.ROTP_IsEditableByClientAdmin);
                        dicData.Add(RotationDataEditableBy.Applicant.GetStringValue(), requirementObjPropCatData.ROTP_IsEditableByApplicant);
                    }
                    else
                    {
                        dicData.Add(RotationDataEditableBy.ADBAdmin.GetStringValue(), false);
                        dicData.Add(RotationDataEditableBy.ClientAdmin.GetStringValue(), false);
                        dicData.Add(RotationDataEditableBy.Applicant.GetStringValue(), false);
                    }
                    reqItemContract.SelectedEditableBy = dicData;
                }

            }
            return reqItemContract;
        }

        /// <summary>
        /// used to get requirement Item detail
        /// </summary>
        /// <returns></returns>
        Boolean ISharedRequirementPackageRepository.SaveUpdateRequirementItemData(RequirementItemContract reqItemContract, Int32 currentLoggedInUserId)
        {
            RequirementItem reqItemObject = new RequirementItem();
            RequirementCategoryItem reqCategoryItem = new RequirementCategoryItem(); //UAT-3078
            String ActionTypeCode = String.Empty;
            String ActionTypeCodeAddForItemUrl = String.Empty;
            String ActionTypeCodeUpdateForItemUrl = String.Empty;
            String ActionTypeCodeRemoveForItemUrl = String.Empty;
            //Update Requirement Item Data 
            if (reqItemContract.RequirementItemID > AppConsts.NONE)
            {
                List<RequirementItemURL> ListOfRequirementURLs = _sharedDataDBContext.RequirementItemURLs.Where(cond => cond.RIU_RequirementItemId == reqItemContract.RequirementItemID && !cond.RIU_IsDeleted).ToList();

                //Delete Records Here...RequirementItemURL on update condition.
                foreach (RequirementItemURL requirementItemURL in ListOfRequirementURLs)
                {
                    if (reqItemContract.listRequirementItemURLContract == null || (reqItemContract.listRequirementItemURLContract != null &&
                        !reqItemContract.listRequirementItemURLContract.Any(x => x.RItemURLID == requirementItemURL.RIU_ID && x.RItemURLID > AppConsts.NONE)))
                    {
                        requirementItemURL.RIU_IsDeleted = true;
                        requirementItemURL.RIU_ModifiedById = currentLoggedInUserId;
                        requirementItemURL.RIU_ModifiedOn = DateTime.Now;
                        ActionTypeCodeRemoveForItemUrl = RequirementPackageObjectActionTypeEnum.REMOVED.GetStringValue();
                    }

                }

                if (reqItemContract.listRequirementItemURLContract != null)
                {
                    //New Records Add here.....
                    foreach (RequirementItemURLContract requirementItemURLContract in reqItemContract.listRequirementItemURLContract)
                    {
                        if (requirementItemURLContract.RItemURLID == AppConsts.NONE)
                        {
                            RequirementItemURL objRequirementItemURL = new RequirementItemURL();
                            objRequirementItemURL.RIU_SampleDocURL = requirementItemURLContract.RItemURLSampleDocURL;
                            objRequirementItemURL.RIU_Label = requirementItemURLContract.RItemURLLabel;
                            objRequirementItemURL.RIU_RequirementItemId = reqItemContract.RequirementItemID;
                            objRequirementItemURL.RIU_CreatedById = currentLoggedInUserId;
                            objRequirementItemURL.RIU_CreatedOn = DateTime.Now;
                            ActionTypeCodeAddForItemUrl = RequirementPackageObjectActionTypeEnum.ADDED.GetStringValue();
                            _sharedDataDBContext.RequirementItemURLs.AddObject(objRequirementItemURL);
                        }
                    }
                }
                if (ListOfRequirementURLs != null && ListOfRequirementURLs.Count > AppConsts.NONE)
                {
                    foreach (RequirementItemURL requirementItemURL in ListOfRequirementURLs)
                    {
                        //Previous Records Found and update
                        if (reqItemContract.listRequirementItemURLContract != null &&
                            reqItemContract.listRequirementItemURLContract.Count > AppConsts.NONE
                            && reqItemContract.listRequirementItemURLContract.Any(x => x.RItemURLID == requirementItemURL.RIU_ID))
                        {
                            foreach (RequirementItemURLContract requirementItemURLContract in reqItemContract.listRequirementItemURLContract)
                            {
                                if (requirementItemURLContract.RItemURLID == requirementItemURL.RIU_ID && (requirementItemURL.RIU_Label != requirementItemURLContract.RItemURLLabel || requirementItemURL.RIU_SampleDocURL != requirementItemURLContract.RItemURLSampleDocURL))
                                {
                                    requirementItemURL.RIU_Label = requirementItemURLContract.RItemURLLabel;
                                    requirementItemURL.RIU_SampleDocURL = requirementItemURLContract.RItemURLSampleDocURL;
                                    requirementItemURL.RIU_ModifiedById = currentLoggedInUserId;
                                    requirementItemURL.RIU_ModifiedOn = DateTime.Now;
                                    ActionTypeCodeUpdateForItemUrl = RequirementPackageObjectActionTypeEnum.EDITED.GetStringValue();

                                }
                            }
                        }
                    }
                }

                reqItemObject = _sharedDataDBContext.RequirementItems.FirstOrDefault(cond => cond.RI_ID == reqItemContract.RequirementItemID && !cond.RI_IsDeleted);
                reqCategoryItem = _sharedDataDBContext.RequirementCategoryItems.FirstOrDefault(cond => cond.RCI_RequirementItemID == reqItemContract.RequirementItemID && cond.RCI_RequirementCategoryID == reqItemContract.RequirementCategoryID && !cond.RCI_IsDeleted);//UAT-3078
                reqItemObject.RI_ItemName = reqItemContract.RequirementItemName;
                reqItemObject.RI_ItemLabel = reqItemContract.RequirementItemLabel;
                reqItemObject.RI_ModifiedByID = currentLoggedInUserId;
                reqItemObject.RI_ModifiedOn = DateTime.Now;
                reqItemObject.RI_Description = reqItemContract.RequirementItemNotes; //UAT-2676
                reqItemObject.RI_AllowDataMovement = reqItemContract.AllowDataMovement;   //UAT-2603
                reqCategoryItem.RCI_DisplayOrder = reqItemContract.RequirementItemDisplayOrder; //UAT-3078
                //reqItemObject.RI_SampleDocFormURL = reqItemContract.RequirementItemSampleDocumentFormURL; //UAT-3309
                ActionTypeCode = RequirementPackageObjectActionTypeEnum.EDITED.GetStringValue();
                //UAT-3077
                reqItemObject.RI_IsPaymentType = reqItemContract.IsPaymentType;
                reqItemObject.RI_Amount = reqItemContract.Amount;
                //UAT 3792
                reqItemObject.RI_AllowItemDataEntry = reqItemContract.AllowItemDataEntry;

            }
            //Insert Requirement Item Data
            else
            {
                reqCategoryItem.RCI_RequirementCategoryID = reqItemContract.RequirementCategoryID;
                reqCategoryItem.RCI_IsDeleted = false;
                reqCategoryItem.RCI_CreatedOn = DateTime.Now;
                reqCategoryItem.RCI_CreatedByID = currentLoggedInUserId;
                reqCategoryItem.RCI_DisplayOrder = reqItemContract.RequirementItemDisplayOrder;
                reqItemObject.RI_ItemName = reqItemContract.RequirementItemName;
                reqItemObject.RI_ItemLabel = reqItemContract.RequirementItemLabel;
                reqItemObject.RI_Description = reqItemContract.RequirementItemNotes; //UAT-2676
                reqItemObject.RI_IsDeleted = false;
                reqItemObject.RI_Code = Guid.NewGuid();
                reqItemObject.RI_CreatedByID = currentLoggedInUserId;
                reqItemObject.RI_CreatedOn = DateTime.Now;
                reqItemObject.RI_AllowDataMovement = reqItemContract.AllowDataMovement;   //UAT-2603
                //reqItemObject.RI_SampleDocFormURL = reqItemContract.RequirementItemSampleDocumentFormURL; //UAT-3309
                //UAT-3077
                reqItemObject.RI_IsPaymentType = reqItemContract.IsPaymentType;
                reqItemObject.RI_Amount = reqItemContract.Amount;
                //UAT 3792
                reqItemObject.RI_AllowItemDataEntry = reqItemContract.AllowItemDataEntry;
                reqItemObject.RequirementCategoryItems.Add(reqCategoryItem);
                _sharedDataDBContext.RequirementItems.AddObject(reqItemObject);
                ActionTypeCode = RequirementPackageObjectActionTypeEnum.ADDED.GetStringValue();
            }

            if (_sharedDataDBContext.SaveChanges() > 0)
            {
                //UAT-2305
                Int32 RequirementItemID = reqItemObject.RI_ID;
                if (reqItemContract.listRequirementItemURLContract != null && reqItemContract.RequirementItemID == AppConsts.NONE)
                {
                    foreach (RequirementItemURLContract requirementItemURLContract in reqItemContract.listRequirementItemURLContract)
                    {
                        if (requirementItemURLContract.RItemURLID == AppConsts.NONE)
                        {
                            RequirementItemURL objRequirementItemURL = new RequirementItemURL();
                            objRequirementItemURL.RIU_SampleDocURL = requirementItemURLContract.RItemURLSampleDocURL;
                            objRequirementItemURL.RIU_Label = requirementItemURLContract.RItemURLLabel;
                            objRequirementItemURL.RIU_RequirementItemId = RequirementItemID;
                            objRequirementItemURL.RIU_CreatedById = currentLoggedInUserId;
                            objRequirementItemURL.RIU_CreatedOn = DateTime.Now;
                            ActionTypeCodeAddForItemUrl = RequirementPackageObjectActionTypeEnum.ADDED.GetStringValue();
                            _sharedDataDBContext.RequirementItemURLs.AddObject(objRequirementItemURL);
                        }
                    }
                    _sharedDataDBContext.SaveChanges();
                }

                reqItemContract.RequirementCategoryItemID = reqItemObject.RequirementCategoryItems.Where(cond => cond.RCI_RequirementCategoryID == reqItemContract.RequirementCategoryID).FirstOrDefault().RCI_ID;

                if (reqItemContract.IsNewPackage || reqItemContract.listRequirementItemURLContract != null)
                {
                    Int32 objectId = 0;
                    #region Rot Pkg Object Sync Data
                    List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();


                    if (reqItemContract.IsNewPackage)
                    {
                        RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                        objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_ITEM.GetStringValue();
                        objectData.NewObjectId = reqItemObject.RI_ID;
                        objectData.ActionTypeCode = ActionTypeCode;
                        lstPackageObjectSynchingData.Add(objectData);
                    }

                    if (!ActionTypeCodeUpdateForItemUrl.IsNullOrEmpty())
                    {
                        RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                        objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_ITEM_URL.GetStringValue();
                        objectData.NewObjectId = reqItemObject.RI_ID;
                        objectData.ActionTypeCode = ActionTypeCodeUpdateForItemUrl;
                        lstPackageObjectSynchingData.Add(objectData);
                    }
                    if (!ActionTypeCodeAddForItemUrl.IsNullOrEmpty())
                    {
                        RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                        objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_ITEM_URL.GetStringValue();
                        objectData.NewObjectId = reqItemObject.RI_ID;
                        objectData.ActionTypeCode = ActionTypeCodeAddForItemUrl;
                        lstPackageObjectSynchingData.Add(objectData);
                    }
                    if (!ActionTypeCodeRemoveForItemUrl.IsNullOrEmpty())
                    {
                        RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                        objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_ITEM_URL.GetStringValue();
                        objectData.NewObjectId = reqItemObject.RI_ID;
                        objectData.ActionTypeCode = ActionTypeCodeRemoveForItemUrl;
                        lstPackageObjectSynchingData.Add(objectData);
                    }




                    String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                    SaveRequirementPackageObjectForSync(requestXML, currentLoggedInUserId, out objectId);
                    #endregion
                }

                #region UAT-4165
                RequirementObjectProperty reqObjProp = _sharedDataDBContext.RequirementObjectProperties.Where(a => a.ROTP_CategoryItemID == reqCategoryItem.RCI_ID && !a.ROTP_IsDeleted && a.ROTP_CategoryID == reqCategoryItem.RCI_RequirementCategoryID && a.ROTP_ItemFieldID == null).FirstOrDefault();

                if (!reqObjProp.IsNullOrEmpty())
                {
                    if (reqItemContract.IsCustomSetting)
                    {
                        reqObjProp.ROTP_IsEditableByAdmin = Convert.ToBoolean(reqItemContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.ADBAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                        reqObjProp.ROTP_IsEditableByClientAdmin = Convert.ToBoolean(reqItemContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.ClientAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                        reqObjProp.ROTP_IsEditableByApplicant = Convert.ToBoolean(reqItemContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.Applicant.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                        reqObjProp.ROTP_IsCustomSettings = reqItemContract.IsCustomSetting;
                    }
                    else
                    {
                        reqObjProp.ROTP_IsDeleted = true;
                    }

                    reqObjProp.ROTP_ModifiedBy = currentLoggedInUserId;
                    reqObjProp.ROTP_ModifiedOn = DateTime.Now;
                }
                else
                {
                    if (!reqItemContract.SelectedEditableBy.IsNullOrEmpty() && reqItemContract.IsCustomSetting)
                    {
                        RequirementObjectProperty reqObjPropData = new RequirementObjectProperty();
                        reqObjPropData.ROTP_CategoryID = reqCategoryItem.RCI_RequirementCategoryID;
                        reqObjPropData.ROTP_CategoryItemID = reqCategoryItem.RCI_ID;
                        reqObjPropData.ROTP_IsDeleted = false;
                        reqObjPropData.ROTP_IsCustomSettings = reqItemContract.IsCustomSetting;

                        reqObjPropData.ROTP_IsEditableByAdmin = Convert.ToBoolean(reqItemContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.ADBAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                        reqObjPropData.ROTP_IsEditableByClientAdmin = Convert.ToBoolean(reqItemContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.ClientAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                        reqObjPropData.ROTP_IsEditableByApplicant = Convert.ToBoolean(reqItemContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.Applicant.GetStringValue()).Select(a => a.Value).FirstOrDefault());

                        //else
                        //{
                        //    reqObjPropData.ROTP_IsEditableByAdmin = false;
                        //    reqObjPropData.ROTP_IsEditableByClientAdmin = false;
                        //    reqObjPropData.ROTP_IsEditableByApplicant = false;
                        //}
                        reqObjPropData.ROTP_CreatedBy = currentLoggedInUserId;
                        reqObjPropData.ROTP_CreatedOn = DateTime.Now;
                        _sharedDataDBContext.RequirementObjectProperties.AddObject(reqObjPropData);
                    }
                }
                _sharedDataDBContext.SaveChanges();
                #endregion

                //#region UAT-4165
                //if (!reqItemContract.SelectedEditableBy.IsNullOrEmpty())
                //{
                //    RequirementObjectProperty reqObjProp = new RequirementObjectProperty();
                //    reqObjProp.ROTP_CategoryID = reqCategoryItem.RCI_RequirementCategoryID;
                //    reqObjProp.ROTP_CategoryItemID = reqCategoryItem.RCI_ID;
                //    reqObjProp.ROTP_IsDeleted = false;
                //    reqObjProp.ROTP_IsCustomSettings = reqItemContract.IsCustomSetting;
                //    if (reqItemContract.IsCustomSetting)
                //    {
                //        reqObjProp.ROTP_IsEditableByAdmin = Convert.ToBoolean(reqItemContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.ADBAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                //        reqObjProp.ROTP_IsEditableByClientAdmin = Convert.ToBoolean(reqItemContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.ClientAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                //        reqObjProp.ROTP_IsEditableByApplicant = Convert.ToBoolean(reqItemContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.Applicant.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                //    }
                //    else
                //    {
                //        reqObjProp.ROTP_IsEditableByAdmin = false;
                //        reqObjProp.ROTP_IsEditableByClientAdmin = false;
                //        reqObjProp.ROTP_IsEditableByApplicant = false;
                //    }

                //    reqObjProp.ROTP_CreatedBy = currentLoggedInUserId;
                //    reqObjProp.ROTP_CreatedOn = DateTime.Now;
                //    _sharedDataDBContext.RequirementObjectProperties.AddObject(reqObjProp);
                //    _sharedDataDBContext.SaveChanges();
                //}

                //#endregion
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to get All requirement items of category
        /// </summary>
        /// <param name="requirementCatID"></param>
        /// <returns></returns>
        List<RequirementItemContract> ISharedRequirementPackageRepository.GetRequirementItemsByCategoryID(Int32 requirementCatID)
        {
            List<RequirementItemContract> lstReqCategoryItemListContract = new List<RequirementItemContract>();
            List<RequirementCategoryItem> reqCategoryItemsList = _sharedDataDBContext.RequirementCategoryItems.Where(cnd => cnd.RCI_RequirementCategoryID == requirementCatID
                                                                 && !cnd.RCI_IsDeleted).ToList();
            if (!reqCategoryItemsList.IsNullOrEmpty())
            {
                reqCategoryItemsList.ForEach(catItem =>
                {
                    RequirementItemContract reqItemContract = new RequirementItemContract();
                    reqItemContract.RequirementCategoryID = catItem.RCI_RequirementCategoryID;
                    reqItemContract.RequirementItemName = catItem.RequirementItem.RI_ItemName;
                    reqItemContract.RequirementCategoryItemID = catItem.RCI_ID;
                    reqItemContract.RequirementItemID = catItem.RCI_RequirementItemID;
                    reqItemContract.RequirementItemDisplayOrder = catItem.RCI_DisplayOrder == null ? 0 : Convert.ToInt32(catItem.RCI_DisplayOrder); //UAT-3078
                    reqItemContract.RequirementItemSampleDocumentFormURL = string.Join(", ", _sharedDataDBContext.RequirementItemURLs.Where(item => item.RIU_RequirementItemId == catItem.RCI_RequirementItemID && !item.RIU_IsDeleted).Select(item => item.RIU_SampleDocURL).ToArray());// catItem.RequirementItem.RI_SampleDocFormURL; //Uat-3309
                    lstReqCategoryItemListContract.Add(reqItemContract);
                });
            }
            return lstReqCategoryItemListContract;
        }

        /// <summary>
        /// Method to delete Requirement category and item mapping
        /// </summary>
        /// <param name="requirementCatItemID"></param>
        /// <returns></returns>
        String ISharedRequirementPackageRepository.DeleteReqCategoryItemMapping(Int32 requirementCatItemID, Int32 currentLoggedInUserId, Int32 reqPkgId, Boolean isNewPackage = false)
        {
            RequirementCategoryItem reqCategoryItem = _sharedDataDBContext.RequirementCategoryItems.FirstOrDefault(cnd => cnd.RCI_ID == requirementCatItemID
                                                                 && !cnd.RCI_IsDeleted);
            if (!reqCategoryItem.IsNullOrEmpty())
            {
                if (!isNewPackage)
                {
                    RequirementObjectTree objTreeForPkg = _sharedDataDBContext.RequirementObjectTrees.FirstOrDefault(cond => cond.ROT_ObjectTypeID == 1
                                                                                                                    && cond.ROT_ObjectID == reqPkgId);
                    RequirementObjectTree objTreeForCat = _sharedDataDBContext.RequirementObjectTrees.FirstOrDefault(cond => cond.ROT_ObjectTypeID == 2
                                                                               && cond.ROT_ObjectID == reqCategoryItem.RCI_RequirementCategoryID
                                                                               && cond.ROT_ParentID == objTreeForPkg.ROT_ID);

                    RequirementObjectRule rule = _sharedDataDBContext.RequirementObjectRules.Where(cond => cond.ROR_ObjectTreeId == objTreeForCat.ROT_ID
                                                                                                && !cond.ROR_IsDeleted).FirstOrDefault();

                    if (!rule.IsNullOrEmpty() && !rule.ROR_UIExpression.IsNullOrEmpty())
                    {
                        String sqlExpression = rule.ROR_SqlExpression;
                        string itemString = String.Concat("$" + reqCategoryItem.RCI_RequirementItemID + "#");
                        Boolean ifItemIsusedInRule = sqlExpression.Contains(itemString);
                        if (ifItemIsusedInRule)
                            return "Requirement Item can not be deleted as it is mapped with category rule.";

                    }
                    reqCategoryItem.RCI_IsDeleted = true;
                    reqCategoryItem.RCI_ModifiedByID = currentLoggedInUserId;
                    reqCategoryItem.RCI_ModifiedOn = DateTime.Now;
                    DeleteRecordsofRequirementItemURL(currentLoggedInUserId, reqCategoryItem);
                    DeleteRecordsofRequirementObjectProperties(currentLoggedInUserId, reqCategoryItem);
                }
                else
                {
                    //UAT-2514
                    String result = String.Empty;
                    EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
                    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                    {
                        SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@ParentObjectID", "2-"+reqCategoryItem.RCI_RequirementCategoryID),
                              new SqlParameter("@PranetObjectTypeID", 2),
                              new SqlParameter("@ObjectID","2-" + reqCategoryItem.RCI_RequirementCategoryID + "|3-" + reqCategoryItem.RCI_RequirementItemID),
                              new SqlParameter("@ObjectTypeID", 3),
                              new SqlParameter("@CurrentLoggedInUserID",currentLoggedInUserId)
                        };

                        base.OpenSQLDataReaderConnection(con);

                        using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_CheckChildRelatedParentRuleDetails", sqlParameterCollection))
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    result = dr["Result"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Result"]);
                                }
                            }
                        }
                    }
                    if (!result.IsNullOrEmpty() && result.Equals("Rule Exists"))
                    {
                        return "Requirement Item can not be deleted as it is mapped with category rule.";
                    }
                    reqCategoryItem.RCI_IsDeleted = true;
                    reqCategoryItem.RCI_ModifiedByID = currentLoggedInUserId;
                    reqCategoryItem.RCI_ModifiedOn = DateTime.Now;
                    //UAT-4121
                    DeleteRecordsofRequirementItemURL(currentLoggedInUserId, reqCategoryItem);
                    DeleteRecordsofRequirementObjectProperties(currentLoggedInUserId, reqCategoryItem);
                }
            }

            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                if (isNewPackage && !reqCategoryItem.IsNullOrEmpty())
                {
                    Int32 objectId = 0;
                    #region Rot Pkg Object Sync Data
                    List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
                    RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                    objectData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.REMOVED.GetStringValue(); ;
                    objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_ITEM.GetStringValue();
                    objectData.NewObjectId = reqCategoryItem.RCI_RequirementItemID;
                    lstPackageObjectSynchingData.Add(objectData);
                    String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                    SaveRequirementPackageObjectForSync(requestXML, currentLoggedInUserId, out objectId);
                    #endregion
                }
                return String.Empty;
            }


            return "Some error while deleting RequirementItem. Please try again.";
        }

        //UAT-4121
        private void DeleteRecordsofRequirementItemURL(Int32 currentLoggedInUserId, RequirementCategoryItem reqCategoryItem)
        {

            _sharedDataDBContext.RequirementItemURLs.Where(item => item.RIU_RequirementItemId == reqCategoryItem.RCI_RequirementItemID && !item.RIU_IsDeleted)
             .ForEach(item =>
             {
                 item.RIU_IsDeleted = true;
                 item.RIU_ModifiedById = currentLoggedInUserId;
                 item.RIU_ModifiedOn = DateTime.Now;
             });
        }

        #region UAT-4165
        private void DeleteRecordsofRequirementObjectProperties(Int32 currentLoggedInUserId, RequirementCategoryItem reqCategoryItem)
        {

            _sharedDataDBContext.RequirementObjectProperties.Where(item => item.ROTP_CategoryItemID == reqCategoryItem.RCI_ID && !item.ROTP_IsDeleted)
             .ForEach(item =>
             {
                 item.ROTP_IsDeleted = true;
                 item.ROTP_ModifiedBy = currentLoggedInUserId;
                 item.ROTP_ModifiedOn = DateTime.Now;
             });
        }
        #endregion
        /// <summary>
        ///Method to delete Requirement Item and Field mapping
        /// </summary>
        /// <returns></returns>
        String ISharedRequirementPackageRepository.DeleteReqItemFieldMapping(Int32 requirementItemFieldID, String ItemHId, Int32 currentLoggedInUserId, Boolean isNewPackage = false, Int32 requirementCategoryID = 0)
        {
            RequirementItemField reqItemField = _sharedDataDBContext.RequirementItemFields.FirstOrDefault(cnd => cnd.RIF_ID == requirementItemFieldID
                                                                     && !cnd.RIF_IsDeleted);
            Boolean ifRequirementFieldMappedWithRule = false;
            if (!reqItemField.IsNullOrEmpty())
            {
                if (!isNewPackage)
                {
                    ifRequirementFieldMappedWithRule = IfRequirementFieldMappedWithRule(reqItemField.RIF_RequirementFieldID, reqItemField.RIF_RequirementItemID, ItemHId);
                }
                else
                {
                    //UAT-2514
                    String result = String.Empty;
                    EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
                    using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                    {
                        SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@ParentObjectID", "2-" + requirementCategoryID + "|3-" + reqItemField.RIF_RequirementItemID),
                              new SqlParameter("@PranetObjectTypeID", 3),
                              new SqlParameter("@ObjectID", "2-" + requirementCategoryID + "|3-" + reqItemField.RIF_RequirementItemID + "|4-" + reqItemField.RIF_RequirementFieldID),
                              new SqlParameter("@ObjectTypeID", 4),
                              new SqlParameter("@CurrentLoggedInUserID",currentLoggedInUserId)
                        };

                        base.OpenSQLDataReaderConnection(con);

                        using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_CheckChildRelatedParentRuleDetails", sqlParameterCollection))
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    result = dr["Result"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Result"]);
                                }
                            }
                        }
                    }
                    if (!result.IsNullOrEmpty() && result.Equals("Rule Exists"))
                    {
                        ifRequirementFieldMappedWithRule = true;
                    }
                }
            }
            if (ifRequirementFieldMappedWithRule)
            {
                return "Field can not be deleted as it is mapped with Item Expiration.";
            }
            else if (!reqItemField.IsNullOrEmpty())
            {
                reqItemField.RIF_IsDeleted = true;
                reqItemField.RIF_ModifiedByID = currentLoggedInUserId;
                reqItemField.RIF_ModifiedOn = DateTime.Now;
                Int32 reqCategoryItemID = Convert.ToInt32(_sharedDataDBContext.RequirementCategoryItems.Where(cond => cond.RCI_RequirementItemID == reqItemField.RIF_RequirementItemID && cond.RCI_RequirementCategoryID == requirementCategoryID && !cond.RCI_IsDeleted).Select(cond => cond.RCI_ID).FirstOrDefault());
                RequirementObjectProperty reqObjectProperty = _sharedDataDBContext.RequirementObjectProperties.Where(a => a.ROTP_ItemFieldID == reqItemField.RIF_ID && !a.ROTP_IsDeleted && a.ROTP_CategoryItemID == reqCategoryItemID && a.ROTP_CategoryID == requirementCategoryID).FirstOrDefault();
                if (!reqObjectProperty.IsNullOrEmpty())
                {
                    reqObjectProperty.ROTP_IsDeleted = true;
                    reqObjectProperty.ROTP_ModifiedBy = currentLoggedInUserId;
                    reqObjectProperty.ROTP_ModifiedOn = DateTime.Now;
                }
            }
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                if (isNewPackage && !reqItemField.IsNullOrEmpty() && !ifRequirementFieldMappedWithRule)
                {
                    Int32 objectId = 0;
                    #region Rot Pkg Object Sync Data
                    List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
                    RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                    objectData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.REMOVED.GetStringValue(); ;
                    objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_FIELD.GetStringValue();
                    objectData.NewObjectId = reqItemField.RIF_RequirementFieldID;
                    lstPackageObjectSynchingData.Add(objectData);
                    String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                    SaveRequirementPackageObjectForSync(requestXML, currentLoggedInUserId, out objectId);
                    #endregion
                }
                return String.Empty;
            }
            else
            {
                return "Some error while deleting field. Please try again.";
            }
        }

        private Boolean IfRequirementFieldMappedWithRule(int requirementFieldID, int requirementItemID, string ItemHId)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_CheckIfRequirementFieldMappedWithRule", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RequirementFieldID", requirementFieldID);
                command.Parameters.AddWithValue("@RequirementItemID", requirementItemID);
                command.Parameters.AddWithValue("@ItemHId", ItemHId);
                command.Parameters.Add("@resultValue", SqlDbType.Bit);
                command.Parameters["@resultValue"].Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
                return (Boolean)command.Parameters["@resultValue"].Value;
            }
        }

        /// <summary>
        /// Method to get all requirement fields of Items
        /// </summary>
        /// <returns></returns>
        List<RequirementFieldContract> ISharedRequirementPackageRepository.GetRequirementFieldsByItemID(Int32 requirementItemID)
        {
            List<RequirementFieldContract> lstReqItemFieldListContract = new List<RequirementFieldContract>();
            List<RequirementItemField> reqItemFieldList = _sharedDataDBContext.RequirementItemFields.Where(cnd => cnd.RIF_RequirementItemID == requirementItemID
                                                                 && !cnd.RIF_IsDeleted).ToList();
            if (!reqItemFieldList.IsNullOrEmpty())
            {
                reqItemFieldList.ForEach(catItem =>
                {
                    RequirementFieldContract reqFieldContract = new RequirementFieldContract();
                    reqFieldContract.RequirementFieldID = catItem.RIF_RequirementFieldID;
                    reqFieldContract.RequirementFieldName = catItem.RequirementField.RF_FieldName;
                    reqFieldContract.RequirementItemFieldID = catItem.RIF_ID;
                    reqFieldContract.RequirementItemID = catItem.RIF_RequirementItemID;
                    reqFieldContract.RequirementFieldDisplayOrder = catItem.RIF_DisplayOrder.HasValue ? catItem.RIF_DisplayOrder.Value : AppConsts.NONE;
                    reqFieldContract.RequirementFieldData = new RequirementFieldDataContract();
                    reqFieldContract.RequirementFieldData.RequirementFieldDataTypeCode = catItem.RequirementField.lkpRequirementFieldDataType.RFDT_Code;
                    reqFieldContract.RequirementFieldData.RequirementFieldDataTypeID = catItem.RequirementField.lkpRequirementFieldDataType.RFDT_ID;
                    reqFieldContract.RequirementFieldData.RequirementFieldDataTypeName = catItem.RequirementField.lkpRequirementFieldDataType.RFDT_Name;
                    lstReqItemFieldListContract.Add(reqFieldContract);
                    RequirementObjectProperty requirementObjProp = _sharedDataDBContext.RequirementObjectProperties
                                                            .Where(cond => cond.ROTP_ItemFieldID == reqFieldContract.RequirementFieldID && !cond.ROTP_IsDeleted)
                                                            .FirstOrDefault();

                    if (!requirementObjProp.IsNullOrEmpty())
                    {
                        Dictionary<String, Boolean> dicData = new Dictionary<String, Boolean>();

                        if (!requirementObjProp.ROTP_IsCustomSettings.IsNullOrEmpty() && Convert.ToBoolean(requirementObjProp.ROTP_IsCustomSettings))
                        {
                            dicData.Add(RotationDataEditableBy.ADBAdmin.GetStringValue(), requirementObjProp.ROTP_IsEditableByAdmin);
                            dicData.Add(RotationDataEditableBy.ClientAdmin.GetStringValue(), requirementObjProp.ROTP_IsEditableByClientAdmin);
                            dicData.Add(RotationDataEditableBy.Applicant.GetStringValue(), requirementObjProp.ROTP_IsEditableByApplicant);
                            reqFieldContract.IsCustomSetting = true;
                        }
                        else
                        {
                            reqFieldContract.IsCustomSetting = false;
                            dicData.Add(RotationDataEditableBy.ADBAdmin.GetStringValue(), false);
                            dicData.Add(RotationDataEditableBy.ClientAdmin.GetStringValue(), false);
                            dicData.Add(RotationDataEditableBy.Applicant.GetStringValue(), false);
                        }

                        reqFieldContract.SelectedEditableBy = dicData;
                    }
                    else
                    {
                        Dictionary<String, Boolean> dicData = new Dictionary<String, Boolean>();
                        reqFieldContract.IsCustomSetting = false;
                        dicData.Add(RotationDataEditableBy.ADBAdmin.GetStringValue(), false);
                        dicData.Add(RotationDataEditableBy.ClientAdmin.GetStringValue(), false);
                        dicData.Add(RotationDataEditableBy.Applicant.GetStringValue(), false);
                        reqFieldContract.SelectedEditableBy = dicData;
                    }
                });
            }
            return lstReqItemFieldListContract;
        }

        //UAT-3342
        List<RequirementFieldContract> ISharedRequirementPackageRepository.IsCalculatedAttribute(Int32 requirementFieldID)
        {
            List<RequirementFieldContract> lstReqFieldListContract = new List<RequirementFieldContract>();
            List<RequirementItemField> reqItemFieldList = _sharedDataDBContext.RequirementItemFields.Where(cnd => cnd.RIF_RequirementFieldID == requirementFieldID
                                                                 && !cnd.RIF_IsDeleted).ToList();
            if (!reqItemFieldList.IsNullOrEmpty())
            {
                reqItemFieldList.ForEach(catItem =>
                {
                    RequirementFieldContract reqFieldContract = new RequirementFieldContract();
                    reqFieldContract.RequirementFieldID = catItem.RIF_RequirementFieldID;
                    reqFieldContract.RequirementFieldName = catItem.RequirementField.RF_FieldName;
                    reqFieldContract.RequirementItemFieldID = catItem.RIF_ID;
                    reqFieldContract.RequirementItemID = catItem.RIF_RequirementItemID;
                    reqFieldContract.RequirementFieldDisplayOrder = catItem.RIF_DisplayOrder.HasValue ? catItem.RIF_DisplayOrder.Value : AppConsts.NONE;
                    reqFieldContract.RequirementFieldData = new RequirementFieldDataContract();
                    reqFieldContract.RequirementFieldData.RequirementFieldDataTypeCode = catItem.RequirementField.lkpRequirementFieldDataType.RFDT_Code;
                    reqFieldContract.RequirementFieldData.RequirementFieldDataTypeID = catItem.RequirementField.lkpRequirementFieldDataType.RFDT_ID;
                    reqFieldContract.RequirementFieldData.RequirementFieldDataTypeName = catItem.RequirementField.lkpRequirementFieldDataType.RFDT_Name;
                    reqFieldContract.AttributeTypeCode = catItem.RequirementField.lkpComplianceAttributeType.Code;
                    reqFieldContract.AttributeTypeID = catItem.RequirementField.lkpComplianceAttributeType.ComplianceAttributeTypeID;
                    lstReqFieldListContract.Add(reqFieldContract);
                });
            }
            return lstReqFieldListContract;
        }

        /// <summary>
        /// Method to get requirement field
        /// </summary>
        /// <returns></returns>
        RequirementField ISharedRequirementPackageRepository.GetRequirementFieldByFieldID(Int32 requirementFieldID)
        {
            return _sharedDataDBContext.RequirementFields.FirstOrDefault(cnd => cnd.RF_ID == requirementFieldID
                                                                 && !cnd.RF_IsDeleted);
        }

        RequirementObjectProperty ISharedRequirementPackageRepository.GetReqObjectProperty(int requirementFieldID, Int32 RequirementItemId, Int32 requirementCategoryID)
        {
            Int32 reqCategoryItemId = _sharedDataDBContext.RequirementCategoryItems.Where(cond => cond.RCI_RequirementCategoryID == requirementCategoryID && cond.RCI_RequirementItemID == RequirementItemId && !cond.RCI_IsDeleted).Select(cond => cond.RCI_ID).FirstOrDefault();
            return _sharedDataDBContext.RequirementObjectProperties.FirstOrDefault(cond => cond.ROTP_ItemFieldID == requirementFieldID && cond.ROTP_CategoryItemID == reqCategoryItemId && cond.ROTP_CategoryID == requirementCategoryID && !cond.ROTP_IsDeleted);
        }
        List<UniversalField> ISharedRequirementPackageRepository.GetUniversalFieldByAttributeDataTypeID(Int32 attributeDataTypeID)
        {
            List<UniversalField> universalFields = base.SharedDataDBContext.UniversalFields.Where(cond => !cond.UF_IsDeleted && cond.UF_AttributeDataTypeID == attributeDataTypeID).ToList();
            if (!universalFields.IsNullOrEmpty())
                return universalFields;
            return new List<UniversalField>();
        }
        /// <summary>
        /// Method to Save/Update requirement field
        /// </summary>
        /// <returns></returns>
        Boolean ISharedRequirementPackageRepository.SaveUpdateRequirementField(RequirementField reqField, Boolean isNewData, Boolean isNewPackage = false)
        {
            String ActionTypeCode = String.Empty;
            if (isNewData)
            {
                _sharedDataDBContext.RequirementFields.AddObject(reqField);
                ActionTypeCode = RequirementPackageObjectActionTypeEnum.ADDED.GetStringValue();
            }
            else
            {
                ActionTypeCode = RequirementPackageObjectActionTypeEnum.EDITED.GetStringValue();
            }
            if (_sharedDataDBContext.SaveChanges() > 0)
            {
                if (isNewPackage)
                {
                    Int32 objectId = 0;
                    #region Rot Pkg Object Sync Data
                    List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
                    RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                    objectData.ActionTypeCode = ActionTypeCode;
                    objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_FIELD.GetStringValue();
                    objectData.NewObjectId = reqField.RF_ID;
                    lstPackageObjectSynchingData.Add(objectData);
                    String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                    SaveRequirementPackageObjectForSync(requestXML, reqField.RF_CreatedByID, out objectId);
                    #endregion
                }
                return true;
            }
            return false;
        }
        void ISharedRequirementPackageRepository.SaveUpdateRequirementFieldEditable(RequirementFieldContract reqFieldContract, Int32 ReqItemField_Id, Int32 currentLoggedInUserId)
        {
            Int32 reqCategoryItemId = _sharedDataDBContext.RequirementCategoryItems.Where(cond => cond.RCI_RequirementCategoryID == reqFieldContract.RequirementCategoryID && cond.RCI_RequirementItemID == reqFieldContract.RequirementItemID && !cond.RCI_IsDeleted).Select(cond => cond.RCI_ID).FirstOrDefault();
            RequirementObjectProperty reqObjProp = _sharedDataDBContext.RequirementObjectProperties.Where(a => a.ROTP_CategoryID == reqFieldContract.RequirementCategoryID && a.ROTP_CategoryItemID == reqCategoryItemId && a.ROTP_ItemFieldID == ReqItemField_Id && !a.ROTP_IsDeleted).FirstOrDefault();

            if (!reqObjProp.IsNullOrEmpty())
            {
                if (reqFieldContract.IsCustomSetting)
                {
                    reqObjProp.ROTP_IsEditableByAdmin = Convert.ToBoolean(reqFieldContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.ADBAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                    reqObjProp.ROTP_IsEditableByClientAdmin = Convert.ToBoolean(reqFieldContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.ClientAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                    reqObjProp.ROTP_IsEditableByApplicant = Convert.ToBoolean(reqFieldContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.Applicant.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                    reqObjProp.ROTP_IsCustomSettings = reqFieldContract.IsCustomSetting;
                    if (reqFieldContract.IsCalculatedAttribute)
                        reqObjProp.ROTP_IsDeleted = true;
                }
                else
                {
                    reqObjProp.ROTP_IsDeleted = true;
                }
                reqObjProp.ROTP_ModifiedBy = currentLoggedInUserId;
                reqObjProp.ROTP_ModifiedOn = DateTime.Now;
            }
            else
            {
                if (!reqFieldContract.SelectedEditableBy.IsNullOrEmpty() && reqFieldContract.IsCustomSetting)
                {
                    RequirementObjectProperty reqObjPropData = new RequirementObjectProperty();
                    reqObjPropData.ROTP_CategoryID = reqFieldContract.RequirementCategoryID;
                    reqObjPropData.ROTP_CategoryItemID = reqCategoryItemId;
                    reqObjPropData.ROTP_ItemFieldID = ReqItemField_Id;
                    reqObjPropData.ROTP_IsDeleted = false;
                    reqObjPropData.ROTP_IsCustomSettings = reqFieldContract.IsCustomSetting;

                    reqObjPropData.ROTP_IsEditableByAdmin = Convert.ToBoolean(reqFieldContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.ADBAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                    reqObjPropData.ROTP_IsEditableByClientAdmin = Convert.ToBoolean(reqFieldContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.ClientAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                    reqObjPropData.ROTP_IsEditableByApplicant = Convert.ToBoolean(reqFieldContract.SelectedEditableBy.Where(x => x.Key == RotationDataEditableBy.Applicant.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                    reqObjPropData.ROTP_CreatedBy = currentLoggedInUserId;
                    reqObjPropData.ROTP_CreatedOn = DateTime.Now;
                    _sharedDataDBContext.RequirementObjectProperties.AddObject(reqObjPropData);
                }
            }
            _sharedDataDBContext.SaveChanges();
        }
        #endregion

        RequirementCategoryContract ISharedRequirementPackageRepository.GetRequirementCategoryDetailByCategoryID(Int32 rqrmntCtgryID)
        {
            RequirementCategoryContract reqCtgryContract = new RequirementCategoryContract();
            RequirementCategory reqCtgry = _sharedDataDBContext.RequirementCategories.FirstOrDefault(item => item.RC_ID == rqrmntCtgryID && !item.RC_IsDeleted);
            if (!reqCtgry.IsNullOrEmpty())
            {
                reqCtgryContract.RequirementCategoryID = reqCtgry.RC_ID;
                reqCtgryContract.RequirementCategoryName = reqCtgry.RC_CategoryName;
                reqCtgryContract.RequirementDocumentLink = reqCtgry.RC_SampleDocFormURL;
                reqCtgryContract.RequirementDocumentLinkLabel = reqCtgry.RC_SampleDocFormUrlLabel;//UAT-3161
                reqCtgryContract.RequirementCategoryLabel = reqCtgry.RC_CategoryLabel;
                reqCtgryContract.SendItemDoconApproval = Convert.ToBoolean(reqCtgry.RC_SendItemDocOnApproval); //UAT-3805
                String explanatoryNotesContentTypeCode = LCContentType.ExplanatoryNotes.GetStringValue();
                String reqrmnCategoryObjectTypeCode = LCObjectType.RequirementCategory.GetStringValue();
                LargeContent largeContent = _sharedDataDBContext.LargeContents
                                                            .Where(cond => cond.LC_ObjectID == reqCtgry.RC_ID && !cond.LC_IsDeleted
                                                            && cond.lkpLargeContentType.LCT_Code == explanatoryNotesContentTypeCode
                                                            && cond.lkpObjectType.OT_Code == reqrmnCategoryObjectTypeCode)
                                                            .FirstOrDefault();

                #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
                //Set ComplianceRequired Setting values in RequiredCategoryContract.
                RequirementPackageCategory reqPkgCatData = reqCtgry.RequirementPackageCategories.FirstOrDefault();
                reqCtgryContract.ComplianceReqEndDate = reqPkgCatData.RPC_ComplianceRqdEndDate;
                reqCtgryContract.ComplianceReqStartDate = reqPkgCatData.RPC_ComplianceRqdStartDate;
                reqCtgryContract.IsComplianceRequired = reqPkgCatData.RPC_ComplianceRequired;
                #endregion

                if (!largeContent.IsNullOrEmpty())
                {
                    reqCtgryContract.ExplanatoryNotes = largeContent.LC_Content;
                }
            }
            return reqCtgryContract;
        }

        List<RequirementCategoryContract> ISharedRequirementPackageRepository.GetRequirementCategoriesByPackageID(Int32 rqrmntPkgID)
        {
            List<RequirementCategoryContract> rqrmntCtgryList = new List<RequirementCategoryContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@RequirementPackageID", rqrmntPkgID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementCategoriesByPackageID", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementCategoryContract requirementCategoryContract = new RequirementCategoryContract();
                            requirementCategoryContract.RequirementCategoryID = dr["RequirementCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementCategoryID"]);
                            requirementCategoryContract.RequirementPackageCategoryID = dr["RequirementPackageCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageCategoryID"]);
                            requirementCategoryContract.RequirementCategoryName = dr["RequirementCategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementCategoryName"]);
                            requirementCategoryContract.RequirementCategoryCode = dr["RequirementCategoryCode"] != DBNull.Value ? (Guid)(dr["RequirementCategoryCode"]) : Guid.Empty;
                            requirementCategoryContract.RequirementDocumentLink = dr["RequirementDocumentLink"] != DBNull.Value ? Convert.ToString(dr["RequirementDocumentLink"]) : String.Empty;
                            requirementCategoryContract.ExplanatoryNotes = dr["CategoryExplanatoryNotes"] != DBNull.Value ? Convert.ToString(dr["CategoryExplanatoryNotes"]) : String.Empty;
                            requirementCategoryContract.RequirementDocumentLinkLabel = dr["RequirementDocumentLinkLabel"] != DBNull.Value ? Convert.ToString(dr["RequirementDocumentLinkLabel"]) : String.Empty;  //UAT-3161
                            requirementCategoryContract.SendItemDoconApproval = dr["SendItemDocOnApproval"] != DBNull.Value ? Convert.ToBoolean(dr["SendItemDocOnApproval"]) : false;
                            rqrmntCtgryList.Add(requirementCategoryContract);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return rqrmntCtgryList;
        }

        Boolean ISharedRequirementPackageRepository.DeleteReqPackageCategoryMapping(Int32 reqPkgCtgryID, Int32 currentLoggedInUserId)
        {
            RequirementPackageCategory reqPkgCtgry = _sharedDataDBContext.RequirementPackageCategories.FirstOrDefault(cnd => cnd.RPC_ID == reqPkgCtgryID
                                                                 && !cnd.RPC_IsDeleted);
            if (!reqPkgCtgry.IsNullOrEmpty())
            {
                reqPkgCtgry.RPC_IsDeleted = true;
                reqPkgCtgry.RPC_ModifiedByID = currentLoggedInUserId;
                reqPkgCtgry.RPC_ModifiedOn = DateTime.Now;
            }
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }

        Int32 ISharedRequirementPackageRepository.SaveRequirementCategoryDetails(RequirementCategoryContract requirementCategoryContract, Int32 currentLoggedInUserId)
        {
            RequirementCategory reqCtgryObject = new RequirementCategory();
            String explanatoryNotesContentTypeCode = LCContentType.ExplanatoryNotes.GetStringValue();
            String reqrmnCategoryObjectTypeCode = LCObjectType.RequirementCategory.GetStringValue();
            //Update Requirement Category Data 
            if (requirementCategoryContract.RequirementCategoryID > AppConsts.NONE)
            {
                reqCtgryObject = _sharedDataDBContext.RequirementPackageCategories
                                            .FirstOrDefault(cond => cond.RPC_RequirementCategoryID == requirementCategoryContract.RequirementCategoryID
                                            && cond.RPC_RequirementPackageID == requirementCategoryContract.RequirementPackageID
                                            && !cond.RPC_IsDeleted).RequirementCategory;
                reqCtgryObject.RC_CategoryName = requirementCategoryContract.RequirementCategoryName;
                reqCtgryObject.RC_SampleDocFormURL = requirementCategoryContract.RequirementDocumentLink;
                reqCtgryObject.RC_SampleDocFormUrlLabel = requirementCategoryContract.RequirementDocumentLinkLabel; //UAT-3161
                reqCtgryObject.RC_ModifiedByID = currentLoggedInUserId;
                reqCtgryObject.RC_ModifiedOn = DateTime.Now;
                reqCtgryObject.RC_SendItemDocOnApproval = requirementCategoryContract.SendItemDoconApproval; //UAT-3805

                LargeContent largeContent = _sharedDataDBContext.LargeContents
                                                            .Where(cond => cond.LC_ObjectID == requirementCategoryContract.RequirementCategoryID && !cond.LC_IsDeleted
                                                            && cond.lkpLargeContentType.LCT_Code == explanatoryNotesContentTypeCode
                                                            && cond.lkpObjectType.OT_Code == reqrmnCategoryObjectTypeCode)
                                                            .FirstOrDefault();
                if (!largeContent.IsNullOrEmpty())
                {
                    largeContent.LC_Content = requirementCategoryContract.ExplanatoryNotes;
                    largeContent.LC_ModifiedByID = currentLoggedInUserId;
                    largeContent.LC_ModifiedOn = DateTime.Now;
                }

                #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
                //Updating the RequirementPackageCategory on update of category.
                RequirementPackageCategory reqPkgCatData = reqCtgryObject.RequirementPackageCategories.FirstOrDefault();
                if (!reqPkgCatData.IsNullOrEmpty())
                {
                    reqPkgCatData.RPC_ComplianceRequired = requirementCategoryContract.IsComplianceRequired;
                    reqPkgCatData.RPC_ComplianceRqdStartDate = requirementCategoryContract.ComplianceReqStartDate;
                    reqPkgCatData.RPC_ComplianceRqdEndDate = requirementCategoryContract.ComplianceReqEndDate;
                    reqPkgCatData.RPC_ModifiedByID = currentLoggedInUserId;
                    reqPkgCatData.RPC_ModifiedOn = DateTime.Now;
                }
                #endregion

                _sharedDataDBContext.SaveChanges();
            }
            //Insert Requirement Category Data
            else
            {
                RequirementPackageCategory newRequirementPackageCategory = new RequirementPackageCategory();
                newRequirementPackageCategory.RPC_RequirementPackageID = requirementCategoryContract.RequirementPackageID;
                newRequirementPackageCategory.RPC_RequirementCategoryID = requirementCategoryContract.RequirementCategoryID;
                newRequirementPackageCategory.RPC_CreatedByID = currentLoggedInUserId;
                newRequirementPackageCategory.RPC_CreatedOn = DateTime.Now;

                #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
                //Added ComplianceRequired Setting in RequiredPackageCategory Table.
                newRequirementPackageCategory.RPC_ComplianceRequired = requirementCategoryContract.IsComplianceRequired;
                newRequirementPackageCategory.RPC_ComplianceRqdStartDate = requirementCategoryContract.ComplianceReqStartDate;
                newRequirementPackageCategory.RPC_ComplianceRqdEndDate = requirementCategoryContract.ComplianceReqEndDate;
                #endregion


                reqCtgryObject.RC_CategoryName = requirementCategoryContract.RequirementCategoryName;
                reqCtgryObject.RC_SampleDocFormURL = requirementCategoryContract.RequirementDocumentLink;
                reqCtgryObject.RC_SampleDocFormUrlLabel = requirementCategoryContract.RequirementDocumentLinkLabel; //UAT-3161
                reqCtgryObject.RC_IsDeleted = false;
                reqCtgryObject.RC_Code = Guid.NewGuid();
                reqCtgryObject.RC_CreatedByID = currentLoggedInUserId;
                reqCtgryObject.RC_CreatedOn = DateTime.Now;
                reqCtgryObject.RC_SendItemDocOnApproval = requirementCategoryContract.SendItemDoconApproval; //UAT-3805
                reqCtgryObject.RequirementPackageCategories.Add(newRequirementPackageCategory);
                _sharedDataDBContext.RequirementCategories.AddObject(reqCtgryObject);
                Boolean isSavedSuccessfully = _sharedDataDBContext.SaveChanges() > AppConsts.NONE;
                if (isSavedSuccessfully && !requirementCategoryContract.ExplanatoryNotes.IsNullOrEmpty())
                {
                    Int32 contentTypeId = _sharedDataDBContext.lkpLargeContentTypes
                                          .Where(obj => obj.LCT_Code == explanatoryNotesContentTypeCode && !obj.LCT_IsDeleted).FirstOrDefault().LCT_ID;
                    Int32 objectTypeId = _sharedDataDBContext.lkpObjectTypes
                                                    .Where(obj => obj.OT_Code == reqrmnCategoryObjectTypeCode && obj.OT_IsDeleted == false).FirstOrDefault().OT_ID;
                    LargeContent largeContent = new LargeContent();
                    largeContent.LC_Content = requirementCategoryContract.ExplanatoryNotes;
                    largeContent.LC_ObjectID = reqCtgryObject.RC_ID;
                    largeContent.LC_ObjectTypeID = objectTypeId;
                    largeContent.LC_LargeContentTypeID = contentTypeId;
                    largeContent.LC_CreatedByID = currentLoggedInUserId;
                    largeContent.LC_CreatedOn = DateTime.Now;
                    _sharedDataDBContext.LargeContents.AddObject(largeContent);
                    _sharedDataDBContext.SaveChanges();
                }
            }
            return reqCtgryObject.RC_ID;
        }

        #region UAT-1837.
        List<RequirementTreeContract> ISharedRequirementPackageRepository.GetRequirementTree(Int32 requirementPackageID)
        {
            var objectTreeData = SharedDataDBContext.usp_GetRequirementTree(requirementPackageID).ToList();

            List<RequirementTreeContract> lstRequirementTree = new List<RequirementTreeContract>();

            foreach (var objectTree in objectTreeData)
            {
                RequirementTreeContract requirementObjectTree = new RequirementTreeContract();
                requirementObjectTree.TreeNodeTypeID = objectTree.TreeNodeTypeID;
                requirementObjectTree.DataID = objectTree.DataID;
                requirementObjectTree.ParentDataID = objectTree.ParentDataID.HasValue ? objectTree.ParentDataID.Value : (Int32?)null;
                requirementObjectTree.HID = objectTree.HID;
                requirementObjectTree.NodeID = objectTree.NodeID;
                requirementObjectTree.ParentNodeID = objectTree.ParentNodeID.IsNull() ? null : objectTree.ParentNodeID;
                requirementObjectTree.Value = objectTree.Value;
                requirementObjectTree.UICode = objectTree.UICode;
                lstRequirementTree.Add(requirementObjectTree);
            }
            return lstRequirementTree;
        }


        List<RequirementRuleContract> ISharedRequirementPackageRepository.GetRequirementRuleDetail(Int32 requirementObjectTreeID)
        {
            List<RequirementRuleContract> requirementRuleDetailsList = new List<RequirementRuleContract>();

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@Rot_Id", requirementObjectTreeID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementRuleDetail", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementRuleContract requirementPackageDetail = new RequirementRuleContract();
                            requirementPackageDetail.ObjectTreeID = dr["RotID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RotID"]);
                            requirementPackageDetail.ObjectID = dr["ObjectID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ObjectID"]);
                            requirementPackageDetail.ObjectTypeCode = dr["ObjectTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ObjectTypeCode"]);
                            requirementPackageDetail.RequirementObjectRuleId = dr["ROR_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ROR_ID"]);
                            requirementPackageDetail.FixedRuleTypeCode = dr["FixedRuleTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["FixedRuleTypeCode"]);
                            requirementPackageDetail.RequirementFieldRuleTypeCode = dr["RequirementFieldRuleTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementFieldRuleTypeCode"]);
                            requirementPackageDetail.RequirementFieldRuleTypeValue = dr["RequirementFieldRuleValue"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementFieldRuleValue"]);
                            requirementPackageDetail.RequirementFieldDataTypeCode = dr["RequirementFieldDataTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementFieldDataTypeCode"]);
                            requirementPackageDetail.RequirementFieldDataTypeID = dr["RequirementFieldDataTypeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["RequirementFieldDataTypeID"]);
                            requirementPackageDetail.RequirementFieldDataTypeName = dr["RequirementFieldDataTypeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementFieldDataTypeName"]);
                            requirementPackageDetail.ExpirationValueTypeCode = dr["ExpirationValueTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ExpirationValueTypeCode"]);
                            requirementPackageDetail.ExpirationValueTypeID = dr["ExpirationValueTypeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ExpirationValueTypeID"]);
                            requirementPackageDetail.ExpirationValue = dr["ExpirationValue"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ExpirationValue"]);
                            requirementPackageDetail.SelectedDateTypeFieldId = dr["SelectedDateTypeFieldId"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["SelectedDateTypeFieldId"]);
                            requirementPackageDetail.RuleUIExpression = dr["RuleUIExpression"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RuleUIExpression"]);
                            requirementPackageDetail.RuleSqlExpression = dr["RuleSqlExpression"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RuleSqlExpression"]);
                            requirementPackageDetail.ExpirationCondStartDate = dr["ExpirationCondStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ExpirationCondStartDate"]);
                            requirementPackageDetail.ExpirationCondEndDate = dr["ExpirationCondEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ExpirationCondEndDate"]);
                            //2366
                            requirementPackageDetail.UiRequirementItemID = dr["UiRequirementItemID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["UiRequirementItemID"]);
                            requirementPackageDetail.UiRequirementFieldID = dr["UiRequirementFieldID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["UiRequirementFieldID"]);
                            requirementPackageDetail.UiRuleErrorMessage = dr["UiRuleErrorMessage"] == DBNull.Value ? String.Empty : Convert.ToString(dr["UiRuleErrorMessage"]);
                            requirementRuleDetailsList.Add(requirementPackageDetail);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return requirementRuleDetailsList;
        }


        RequirementObjectTree ISharedRequirementPackageRepository.GetExistingRequirementObjectTree(Int32 rotId)
        {
            return _sharedDataDBContext.RequirementObjectTrees.Include("RequirementObjectRules")
                                                  .Include("RequirementObjectRules.RequirementObjectRuleDetails")
                                                  .Include("RequirementObjectTreeProperties")
                                                  .Where(cond => cond.ROT_ID == rotId && !cond.ROT_IsDeleted).FirstOrDefault();
        }


        RequirementItem ISharedRequirementPackageRepository.GetRequirementItemDetail(Int32 requirementItemID)
        {
            return _sharedDataDBContext.RequirementItems.FirstOrDefault(item => item.RI_ID == requirementItemID && !item.RI_IsDeleted);
        }

        RequirementObjectTree ISharedRequirementPackageRepository.GetRequirementObjectTreeDetail(Int32 fieldId, Int32 parentObjectTreeId)
        {
            return SharedDataDBContext.RequirementObjectTrees.FirstOrDefault(cond => cond.ROT_ObjectID == fieldId && cond.ROT_ParentID == parentObjectTreeId);
        }

        RequirementObjectTree ISharedRequirementPackageRepository.GetRequirementObjectTreeForField(Int32 fieldId, Int32 itemId)
        {
            RequirementObjectTree itemRequirementObjectTree = SharedDataDBContext.RequirementObjectTrees.FirstOrDefault(cond => cond.ROT_ObjectID == itemId
                                                                                                                        && cond.ROT_ObjectTypeID == 3
                                                                                                                        && !cond.ROT_IsDeleted);
            return SharedDataDBContext.RequirementObjectTrees.FirstOrDefault(cond => cond.ROT_ObjectID == fieldId && cond.ROT_ParentID == itemRequirementObjectTree.ROT_ID);
        }

        #endregion

        RequirementObjectRule ISharedRequirementPackageRepository.GetRequirementObjectRuleForObjectTreeID(Int32 ROT_ID)
        {
            return _sharedDataDBContext.RequirementObjectRules.Where(cond => cond.ROR_ObjectTreeId == ROT_ID && !cond.ROR_IsDeleted).FirstOrDefault();
        }

        #region UAT-1828
        List<Int32> ISharedRequirementPackageRepository.GetRequirementPackageInstitution(Guid requirementPkgCode)
        {
            List<Int32> tenantIds = new List<Int32>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@RequirementPackageCode", requirementPkgCode)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementPackageInstitution", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Int32 tenantId = dr["TenantID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["TenantID"]);
                            tenantIds.Add(tenantId);
                        }
                    }
                }
            }
            return tenantIds;
        }
        #endregion


        #region UAT-2305
        List<RequirementPackage> ISharedRequirementPackageRepository.GetMasterRequirementPackages(Int32 rotPkgTypeID)
        {
            return _sharedDataDBContext.RequirementPackages.Where(x => x.RP_RequirementPackageTypeID == rotPkgTypeID && !x.RP_IsDeleted && !x.RP_IsArchived).ToList();
        }
        #endregion

        #region UAT-2213 Setup Tree Methods

        /// <summary>
        /// Fetch DataTable of Tree data by Requirement CategoryID
        /// </summary>
        /// <param name="reqCategoryID"></param>
        /// <returns></returns>
        DataTable ISharedRequirementPackageRepository.GetRotationMappingTreeData(Int32 reqCategoryID)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[dbo].[usp_GetRotationMappingTreeData]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CurrentReqCategoryID", reqCategoryID);
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

        List<RequirementPackageContract> ISharedRequirementPackageRepository.GetMasterRequirementPackageDetails(RequirementPackageContract requirementPackageDetailsContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<RequirementPackageContract> requirementPackageDetailList = new List<RequirementPackageContract>();
            String orderBy = "RequirementPackageID";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            //String AgencyIds = String.Join(",",requirementPackageDetailsContract.LstAgencyIDs);

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@PackageName", requirementPackageDetailsContract.RequirementPackageName),
                            new SqlParameter("@PackageLabel", requirementPackageDetailsContract.RequirementPackageLabel),
                            new SqlParameter("@AgencyIds", requirementPackageDetailsContract.lstSelectedAgencyIds),
                            new SqlParameter("@EffectiveStartDate", requirementPackageDetailsContract.EffectiveStartDate),
                            new SqlParameter("@EffectiveEndDate", requirementPackageDetailsContract.EffectiveEndDate),
                            new SqlParameter("@PackageCreatedDate", requirementPackageDetailsContract.PackageCreatedDate),
                            new SqlParameter("@RequirementPkgTypeID", requirementPackageDetailsContract.RequirementPkgTypeID),
                            new SqlParameter("@RotationEndDate", requirementPackageDetailsContract.RotationEndDate),
                            new SqlParameter("@PackageOptions", requirementPackageDetailsContract.PackageOptions),
                            new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            //new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                            //new SqlParameter("@PageSize", customPagingArgsContract.PageSize)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetMasterRequirementPackageDetails", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementPackageContract requirementPackageDetail = new RequirementPackageContract();
                            requirementPackageDetail.RequirementPackageID = dr["RequirementPackageID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageID"]);
                            requirementPackageDetail.RequirementPackageName = dr["RequirementPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageName"]);
                            requirementPackageDetail.RequirementPackageLabel = dr["RequirementPackageLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageLabel"]);
                            requirementPackageDetail.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                            requirementPackageDetail.AgencyNames = dr["AgencyNames"] != DBNull.Value ? Convert.ToString(dr["AgencyNames"]) : String.Empty;
                            requirementPackageDetail.EffectiveStartDate = dr["EffectiveStartDate"] != DBNull.Value ? Convert.ToDateTime(dr["EffectiveStartDate"]) : (DateTime?)null;
                            requirementPackageDetail.EffectiveEndDate = dr["EffectiveEndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EffectiveEndDate"]) : (DateTime?)null;
                            requirementPackageDetail.RotationEndDate = dr["RotationEndDate"] != DBNull.Value ? Convert.ToDateTime(dr["RotationEndDate"]) : (DateTime?)null;
                            requirementPackageDetail.RequirementPackageType = dr["RequirementPackageType"] != DBNull.Value ? Convert.ToString(dr["RequirementPackageType"]) : String.Empty;
                            requirementPackageDetail.PackageCreatedDate = dr["PackageCreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["PackageCreatedDate"]) : (DateTime?)null;
                            requirementPackageDetail.AgencyIDs = dr["AgencyIds"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyIds"]).ToString();
                            requirementPackageDetail.CategoryIDs = dr["CategoryIds"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CategoryIds"]).ToString();
                            requirementPackageDetail.RequirementPkgTypeID = dr["RequirementPackageTypeId"] != DBNull.Value ? Convert.ToInt32(dr["RequirementPackageTypeId"]) : AppConsts.NONE;
                            requirementPackageDetail.DefinedRequirementID = dr["DefinedRequirementID"] != DBNull.Value ? Convert.ToInt32(dr["DefinedRequirementID"]) : 0; requirementPackageDetailList.Add(requirementPackageDetail);
                            requirementPackageDetail.IsPkgArchived = dr["RP_IsArchived"] == DBNull.Value ? false : Convert.ToBoolean(dr["RP_IsArchived"]);
                            requirementPackageDetail.ReqReviewByID = dr["ReqReviewByID"] != DBNull.Value ? Convert.ToInt32(dr["ReqReviewByID"]) : AppConsts.NONE;
                            // requirementPackageDetail.HierarchyIds = dr["HierarchyIds"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyIds"]).ToString();//UAT-4657
                            // requirementPackageDetail.ParentPackageName = dr["ParentPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ParentPackageName"]).ToString();//UAT-4657 
                            requirementPackageDetail.ParentPackageCode = dr["ParentPackageCode"] == DBNull.Value ? (Guid?)null : (Guid)(dr["ParentPackageCode"]);//UAT-4657 
                        }
                    }
                }
            }
            return requirementPackageDetailList;
        }

        /// <summary>
        /// Add New Requirement Object Tree Data 
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <param name="pkgObjectTypeId"></param>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        List<RequirementObjectTreeContract> ISharedRequirementPackageRepository.AddNewRequirementObjectTree(Int32 requirementCategoryID, Int32 catObjectTypeId, Int32 currentUserID)
        {
            List<RequirementObjectTreeContract> requirementObjectTreeList = new List<RequirementObjectTreeContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
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

        #endregion

        #region [UAT-2213]

        RequirementCategoryContract ISharedRequirementPackageRepository.GetRequirementMasterCategoryDetailByCategoryID(Int32 ReqCatID)
        {
            RequirementCategoryContract reqCtgryContract = new RequirementCategoryContract();
            RequirementCategory reqCtgry = _sharedDataDBContext.RequirementCategories.FirstOrDefault(item => item.RC_ID == ReqCatID && !item.RC_IsDeleted);

            if (!reqCtgry.IsNullOrEmpty())
            {
                reqCtgryContract.RequirementCategoryID = reqCtgry.RC_ID;
                reqCtgryContract.RequirementCategoryName = reqCtgry.RC_CategoryName;
                reqCtgryContract.RequirementDocumentLink = reqCtgry.RC_SampleDocFormURL;
                reqCtgryContract.RequirementCategoryLabel = reqCtgry.RC_CategoryLabel;
                reqCtgryContract.IsComplianceRequired = reqCtgry.RC_ComplianceRequired;
                reqCtgryContract.ComplianceReqStartDate = reqCtgry.RC_ComplianceRqdStartDate;
                reqCtgryContract.ComplianceReqEndDate = reqCtgry.RC_ComplianceRqdEndDate;
                //UAT-2603//
                reqCtgryContract.AllowDataMovement = reqCtgry.RC_AllowDataMovement;
                //UAT-3792 : Ability to turn off applicant editibility on rotation items/categories
                // reqCtgryContract.AllowApplicantToEdit = reqCtgry.RC_AllowApplicantEdit;
                //UAT-3161
                reqCtgryContract.RequirementDocumentLinkLabel = reqCtgry.RC_SampleDocFormUrlLabel;
                reqCtgryContract.SendItemDoconApproval = Convert.ToBoolean(reqCtgry.RC_SendItemDocOnApproval); //UAT-3805
                reqCtgryContract.TriggerOtherCategoryRules = Convert.ToBoolean(reqCtgry.RC_TriggerOtherCategoryRules); //UAT-4259
                String explanatoryNotesContentTypeCode = LCContentType.ExplanatoryNotes.GetStringValue();
                String reqrmnCategoryObjectTypeCode = LCObjectType.RequirementCategory.GetStringValue();

                LargeContent largeContent = _sharedDataDBContext.LargeContents
                                                            .Where(cond => cond.LC_ObjectID == reqCtgry.RC_ID && !cond.LC_IsDeleted
                                                            && cond.lkpLargeContentType.LCT_Code == explanatoryNotesContentTypeCode
                                                            && cond.lkpObjectType.OT_Code == reqrmnCategoryObjectTypeCode)
                                                            .FirstOrDefault();
                if (!largeContent.IsNullOrEmpty())
                {
                    reqCtgryContract.ExplanatoryNotes = largeContent.LC_Content;
                }

                RequirementObjectProperty requirementObjProp = _sharedDataDBContext.RequirementObjectProperties
                                                            .Where(cond => cond.ROTP_CategoryID == reqCtgry.RC_ID && !cond.ROTP_IsDeleted
                                                            && cond.ROTP_CategoryItemID == null && cond.ROTP_ItemFieldID == null)
                                                            .FirstOrDefault();

                if (!requirementObjProp.IsNullOrEmpty())
                {
                    Dictionary<String, Boolean> dicData = new Dictionary<String, Boolean>();
                    dicData.Add(RotationDataEditableBy.ADBAdmin.GetStringValue(), requirementObjProp.ROTP_IsEditableByAdmin);
                    dicData.Add(RotationDataEditableBy.ClientAdmin.GetStringValue(), requirementObjProp.ROTP_IsEditableByClientAdmin);
                    dicData.Add(RotationDataEditableBy.Applicant.GetStringValue(), requirementObjProp.ROTP_IsEditableByApplicant);
                    reqCtgryContract.SelectedlstEditableBy = dicData;
                }
                else
                {
                    Dictionary<String, Boolean> dicData = new Dictionary<String, Boolean>();
                    dicData.Add(RotationDataEditableBy.ADBAdmin.GetStringValue(), true);
                    dicData.Add(RotationDataEditableBy.ClientAdmin.GetStringValue(), true);
                    dicData.Add(RotationDataEditableBy.Applicant.GetStringValue(), true);
                    reqCtgryContract.SelectedlstEditableBy = dicData;
                }

                // reqCtgryContract.UniversalCategoryID = _sharedDataDBContext.UniversalRequirementCategoryMappings.Where(a => a.URCM_RequirementCategoryID == ReqCatID && a.URCM_IsDeleted == false).Select(a => a.URCM_UniversalCategoryID).FirstOrDefault();

            }
            return reqCtgryContract;
        }


        Boolean ISharedRequirementPackageRepository.IsMasterCategoryNameExists(String newCategoryName)
        {
            RequirementCategory reqCtgry = _sharedDataDBContext.RequirementCategories
                                                .FirstOrDefault(item => !item.RC_IsDeleted
                                                                            && item.RC_CategoryName.Trim().ToLower().Equals(newCategoryName.Trim().ToLower()));

            if (!reqCtgry.IsNullOrEmpty())
                return true;
            else
                return false;

        }

        List<RequirementPackageContract> ISharedRequirementPackageRepository.GetAllMasterRequirementPackages()
        {

            return _sharedDataDBContext.RequirementPackages.Where(x => !x.RP_IsDeleted && x.RP_IsNewPackage)
                .Select(s => new RequirementPackageContract
                {
                    RequirementPackageName = s.RP_PackageName, //string.IsNullOrEmpty(s.RP_PackageLabel) ? s.RP_PackageName : s.RP_PackageLabel,
                    RequirementPackageID = s.RP_ID
                }).ToList();
        }

        Int32 ISharedRequirementPackageRepository.CreateCategoryCopy(CreateCategoryCopyContract createCategoryCopyContract)
        {
            Int32 newlyAddedReqCategoryID = 0;
            List<Int32> lstPkgIdsWithPrev = _sharedDataDBContext.RequirementPackageCategories
                    .Where(cond => cond.RPC_RequirementCategoryID == createCategoryCopyContract.OldRequirementCategoryID && !cond.RPC_IsDeleted)
                                                        .Select(sel => sel.RPC_RequirementPackageID).ToList(); //UAT-3436
            //UAT-4165
            var IsEditableByAdmin = Convert.ToBoolean(createCategoryCopyContract.SelectedlstEditableBy.Where(x => x.Key == RotationDataEditableBy.ADBAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
            var IsEditableByClientAdmin = Convert.ToBoolean(createCategoryCopyContract.SelectedlstEditableBy.Where(x => x.Key == RotationDataEditableBy.ClientAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
            var IsEditableByApplicant = Convert.ToBoolean(createCategoryCopyContract.SelectedlstEditableBy.Where(x => x.Key == RotationDataEditableBy.Applicant.GetStringValue()).Select(a => a.Value).FirstOrDefault());
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("[dbo].[usp_CreateCategoryCopy]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RequirementCategoryID", createCategoryCopyContract.OldRequirementCategoryID);
                command.Parameters.AddWithValue("@PackageIDs", createCategoryCopyContract.SelectedReqPackageIds);
                command.Parameters.AddWithValue("@CategoryName", createCategoryCopyContract.CategoryName);
                command.Parameters.AddWithValue("@CategoryLabel", createCategoryCopyContract.CategoryLabel);
                command.Parameters.AddWithValue("@ExpNotes", createCategoryCopyContract.ExplanatoryNotes);
                command.Parameters.AddWithValue("@CurrentUserID", createCategoryCopyContract.CurrentLoggedInUserId);
                command.Parameters.AddWithValue("@IsComplianceRequired", createCategoryCopyContract.IsComplianceRequired);
                command.Parameters.AddWithValue("@ComplianceReqStartDate", createCategoryCopyContract.ComplianceReqStartDate);
                command.Parameters.AddWithValue("@ComplianceReqEndDate", createCategoryCopyContract.ComplianceReqEndDate);
                command.Parameters.AddWithValue("@DocumentLink", createCategoryCopyContract.RequirementDocumentLink);
                command.Parameters.AddWithValue("@NewlyAddedReqCategoryID", newlyAddedReqCategoryID);
                command.Parameters.AddWithValue("@AllowDataMovement", createCategoryCopyContract.AllowDataMovement); //UAT-2603
                command.Parameters.AddWithValue("@DocumentLinkLabel", createCategoryCopyContract.RequirementDocumentLinkLabel);//UAT-3161
                command.Parameters.AddWithValue("@SendItemDoconApproval", createCategoryCopyContract.SendItemDoconApproval);//UAT-3805
                command.Parameters.AddWithValue("@IsEditableByAdmin", IsEditableByAdmin);//UAT-4165
                command.Parameters.AddWithValue("@IsEditableByClientAdmin", IsEditableByClientAdmin);//UAT-4165
                command.Parameters.AddWithValue("@IsEditableByApplicant", IsEditableByApplicant);//UAT-4165
                command.Parameters.AddWithValue("@TriggerOtherCategoryRules", IsEditableByApplicant);//UAT-4165
                //command.Parameters.AddWithValue("@AllowApplicantToEdit", createCategoryCopyContract.AllowApplicantToEdit); //UAT-3792
                command.Parameters["@NewlyAddedReqCategoryID"].Direction = ParameterDirection.Output;
                command.ExecuteScalar();
                newlyAddedReqCategoryID = Convert.ToInt32(command.Parameters["@NewlyAddedReqCategoryID"].Value);
                con.Close();


                List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>(); //UAT-3436

                //UAT-4254///
                String ActionTypeCodeAddForCatUrl = String.Empty;
                if (!createCategoryCopyContract.lstReqCatUrls.IsNullOrEmpty() && createCategoryCopyContract.lstReqCatUrls.Count > AppConsts.NONE)
                {
                    ActionTypeCodeAddForCatUrl = RequirementPackageObjectActionTypeEnum.ADDED.GetStringValue();

                    createCategoryCopyContract.lstReqCatUrls.ForEach(catUrl =>
                    {
                        RequirementCategoryDocLink requirementCatDocLink = new RequirementCategoryDocLink();
                        requirementCatDocLink.RCDL_RequirementCategoryID = newlyAddedReqCategoryID;
                        requirementCatDocLink.RCDL_SampleDocFormURL = catUrl.RequirementCatDocUrl;
                        requirementCatDocLink.RCDL_SampleDocFormUrlLabel = catUrl.RequirementCatDocUrlLabel;
                        requirementCatDocLink.RCDL_CreatedBy = createCategoryCopyContract.CurrentLoggedInUserId;
                        requirementCatDocLink.RCDL_CreatedOn = DateTime.Now;
                        SharedDataDBContext.RequirementCategoryDocLinks.AddObject(requirementCatDocLink);
                    });
                }
                _sharedDataDBContext.SaveChanges();
                //if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                //{
                //    if (!String.IsNullOrEmpty(ActionTypeCodeAddForCatUrl))
                //    {
                //        RequirementPackageObjectSynchingContract synchingData = new RequirementPackageObjectSynchingContract();
                //        synchingData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_CATEGORY_DOC_URL.GetStringValue();
                //        synchingData.NewObjectId = newlyAddedReqCategoryID;
                //        synchingData.ActionTypeCode = ActionTypeCodeAddForCatUrl;
                //        lstPackageObjectSynchingData.Add(synchingData);
                //    }
                //}
                //END


                #region Add Category in PAckage Object Synching

                List<Int32> lstCurrentAssinePkgIds = new List<Int32>();
                if (!createCategoryCopyContract.SelectedReqPackageIds.IsNullOrEmpty())
                    lstCurrentAssinePkgIds = createCategoryCopyContract.SelectedReqPackageIds.Split(',').Select(sel => Convert.ToInt32(sel)).ToList(); //UAT-3436


                RequirementPackageObjectSynchingContract objectOldData = null;
                if (lstCurrentAssinePkgIds.Any(cond => lstPkgIdsWithPrev.Contains(cond)))  //UAT-3436
                {
                    objectOldData = new RequirementPackageObjectSynchingContract();
                    objectOldData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.REPLACED.GetStringValue();
                    objectOldData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_CATEGORY.GetStringValue();
                    objectOldData.NewObjectId = newlyAddedReqCategoryID;
                    objectOldData.OldObjectId = createCategoryCopyContract.OldRequirementCategoryID;
                    lstPackageObjectSynchingData.Add(objectOldData);
                }

                objectOldData = new RequirementPackageObjectSynchingContract();
                objectOldData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.ADDED.GetStringValue(); //UAT-3436
                objectOldData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_CATEGORY.GetStringValue();
                objectOldData.NewObjectId = newlyAddedReqCategoryID;
                //objectOldData.OldObjectId = createCategoryCopyContract.OldRequirementCategoryID; //UAT-3436
                lstPackageObjectSynchingData.Add(objectOldData);
                //if (!createCategoryCopyContract.SelectedReqPackageIds.IsNullOrEmpty())
                //{
                //    RequirementPackageObjectSynchingContract objectnewData = new RequirementPackageObjectSynchingContract();
                //    objectnewData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.ADDED.GetStringValue();
                //    objectnewData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_CATEGORY.GetStringValue();
                //    objectnewData.NewObjectId = newlyAddedReqCategoryID;
                //    lstPackageObjectSynchingData.Add(objectnewData);
                //}
                Int32 objectId = 0;
                if (!lstPackageObjectSynchingData.IsNullOrEmpty())
                {
                    String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                    SaveRequirementPackageObjectForSync(requestXML, createCategoryCopyContract.CurrentLoggedInUserId, out objectId);
                }
                #endregion

            }
            return newlyAddedReqCategoryID;
        }


        // UAT-2213- Add and Update Category

        List<RequirementPackageContract> ISharedRequirementPackageRepository.GetCategoryPackageMapping(CategoryPackageMappingContract categoryPackageMappingContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<RequirementPackageContract> lstRequirementpackage = new List<RequirementPackageContract>();

            String orderBy = "RequirementPackageID";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";


            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@ReqCategoryID", categoryPackageMappingContract.ReqCategoryID),
                            new SqlParameter("@PkgName", categoryPackageMappingContract.ReqPackageName),
                            new SqlParameter("@ResultTypeId", categoryPackageMappingContract.ResultTypeID),
                            new SqlParameter("@EffectiveStartDate", categoryPackageMappingContract.EffectiveStartDate),
                            new SqlParameter("@EffectiveEndDate", categoryPackageMappingContract.EffectiveEndDate),
                            new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                            new SqlParameter("@PageSize", customPagingArgsContract.PageSize)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetCategoryPkgMapping", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementPackageContract requirementPackageDetail = new RequirementPackageContract();
                            requirementPackageDetail.RequirementPackageID = dr["RequirementPackageID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageID"]);
                            requirementPackageDetail.RequirementPackageName = dr["RequirementPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageName"]);
                            requirementPackageDetail.RequirementPackageLabel = dr["RequirementPackageLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageLabel"]);
                            requirementPackageDetail.EffectiveStartDate = dr["EffectiveStartDate"] != DBNull.Value ? Convert.ToDateTime(dr["EffectiveStartDate"]) : (DateTime?)null;
                            requirementPackageDetail.EffectiveEndDate = dr["EffectiveEndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EffectiveEndDate"]) : (DateTime?)null;
                            requirementPackageDetail.IsCategoryMappedWithPkg = dr["IsReqCatMappedWithPkg"] != DBNull.Value ? Convert.ToBoolean(dr["IsReqCatMappedWithPkg"]) : false;
                            requirementPackageDetail.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                            lstRequirementpackage.Add(requirementPackageDetail);
                        }
                    }
                }
            }
            return lstRequirementpackage;
        }

        Boolean ISharedRequirementPackageRepository.SaveCategoryPackageMapping(Int32 currentOrgUserID, Int32 requirementCategoryID, String requirementPackageIds)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            //UAT-3230
            List<Int32> lstPrevsRequirementPkgIds = _sharedDataDBContext.RequirementPackageCategories.Where(cond => cond.RPC_RequirementCategoryID == requirementCategoryID
                            && !cond.RPC_IsDeleted && !cond.RequirementCategory.RC_IsDeleted && !cond.RequirementPackage.RP_IsDeleted).Select(cond => cond.RPC_RequirementPackageID).Distinct().ToList();
            List<Int32> lstCurrentRequirementPkgIds = requirementPackageIds.IsNullOrEmpty() ? new List<Int32>() : requirementPackageIds.Split(',').Select(Int32.Parse).ToList();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                Boolean isCategoryMappedWithPackage = false;
                Boolean isCategoryRemovedFromPackage = false;
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_SaveCategoryPkgMapping", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReqCategoryID", requirementCategoryID);
                cmd.Parameters.AddWithValue("@ReqPkgIds", requirementPackageIds);
                cmd.Parameters.AddWithValue("@CurrentOrgUserID", currentOrgUserID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        isCategoryMappedWithPackage = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsCategoryAddedInPackage"]);
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        isCategoryRemovedFromPackage = Convert.ToBoolean(ds.Tables[1].Rows[0]["IsCategoryRemovedFromPackage"]);
                    }
                }


                List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
                if (!requirementPackageIds.IsNullOrEmpty() && isCategoryMappedWithPackage)
                {
                    RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                    objectData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.ADDED.GetStringValue();
                    objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_CATEGORY.GetStringValue();
                    objectData.NewObjectId = requirementCategoryID;
                    lstPackageObjectSynchingData.Add(objectData);
                }
                //if (isCategoryRemovedFromPackage)
                //{
                //    RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                //    objectData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.REMOVED.GetStringValue();
                //    objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_CATEGORY.GetStringValue();
                //    objectData.NewObjectId = requirementCategoryID;
                //    lstPackageObjectSynchingData.Add(objectData);
                //}
                //UAT-3230
                List<Int32> lstRequirementPkgIds = new List<Int32>();
                lstRequirementPkgIds.AddRange(lstPrevsRequirementPkgIds.Except(lstCurrentRequirementPkgIds).ToList());
                //lstRequirementPkgIds.AddRange(lstCurrentRequirementPkgIds.Except(lstPrevsRequirementPkgIds).ToList());
                //List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();                
                foreach (var requirementPackageID in lstRequirementPkgIds)
                {
                    RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                    objectData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.CATEGORY_REMOVE_FROM_PACKAGE.GetStringValue();
                    objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_PACKAGE.GetStringValue();
                    objectData.NewObjectId = Convert.ToInt32(requirementPackageID);
                    lstPackageObjectSynchingData.Add(objectData);
                }
                Int32 objectId = 0;
                if (!lstPackageObjectSynchingData.IsNullOrEmpty() && (isCategoryMappedWithPackage || isCategoryRemovedFromPackage))
                {
                    String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                    SaveRequirementPackageObjectForSync(requestXML, currentOrgUserID, out objectId);
                }
                return true;
            }
        }

        List<Int32> ISharedRequirementPackageRepository.GetMappedPackageIdsWithCategory(Int32 categoryID)
        {
            List<Int32> lstMappedPkgIds = new List<int>();

            lstMappedPkgIds = _sharedDataDBContext.RequirementPackageCategories
                                                    .Where(cond => cond.RPC_RequirementCategoryID == categoryID
                                                            && cond.RequirementPackage.RP_IsNewPackage
                                                            && !cond.RPC_IsDeleted).Select(n => n.RPC_RequirementPackageID).ToList();

            if (lstMappedPkgIds.IsNullOrEmpty())
                lstMappedPkgIds = new List<int>();

            return lstMappedPkgIds;
        }

        List<Int32> ISharedRequirementPackageRepository.GetMappedCategoriesWithPackage(Int32 packageID)
        {
            List<Int32> lstMappedCatIds = new List<int>();

            lstMappedCatIds = _sharedDataDBContext.RequirementPackageCategories
                                                    .Where(cond => cond.RPC_RequirementPackageID == packageID
                                                            && cond.RequirementPackage.RP_IsNewPackage
                                                            && cond.RequirementCategory.RC_IsNewCategory
                                                            && !cond.RPC_IsDeleted).Select(n => n.RPC_RequirementCategoryID).ToList();

            if (lstMappedCatIds.IsNullOrEmpty())
                lstMappedCatIds = new List<int>();

            return lstMappedCatIds;
        }

        // UAT-2213- Add and Update Category

        Int32 ISharedRequirementPackageRepository.SaveMasterRotationCategory(RequirementCategoryContract requirementCategoryContract, Int32 currentLoggedInUserId)
        {
            RequirementCategory reqCtgryObject = new RequirementCategory();
            try
            {

                String explanatoryNotesContentTypeCode = LCContentType.ExplanatoryNotes.GetStringValue();
                String reqrmnCategoryObjectTypeCode = LCObjectType.RequirementCategory.GetStringValue();
                //Update Requirement Category Data 
                if (requirementCategoryContract.RequirementCategoryID > AppConsts.NONE)
                {
                    reqCtgryObject = _sharedDataDBContext.RequirementCategories.Where(a => a.RC_ID == requirementCategoryContract.RequirementCategoryID).FirstOrDefault();
                    if (String.IsNullOrEmpty(Convert.ToString(reqCtgryObject.RC_ID)))
                    {
                        reqCtgryObject = _sharedDataDBContext.RequirementPackageCategories
                                              .FirstOrDefault(cond => cond.RPC_RequirementCategoryID == requirementCategoryContract.RequirementCategoryID
                                              // && cond.RPC_RequirementPackageID == requirementCategoryContract.RequirementPackageID
                                              && !cond.RPC_IsDeleted).RequirementCategory;
                    }

                    reqCtgryObject.RC_CategoryName = requirementCategoryContract.RequirementCategoryName;
                    //  reqCtgryObject.RC_SampleDocFormURL = requirementCategoryContract.RequirementDocumentLink;  //Commented in UAT-4254
                    reqCtgryObject.RC_ModifiedByID = currentLoggedInUserId;
                    reqCtgryObject.RC_ModifiedOn = DateTime.Now;
                    //UAT-2213 get category label also
                    reqCtgryObject.RC_CategoryLabel = requirementCategoryContract.RequirementCategoryLabel;
                    reqCtgryObject.RC_ComplianceRequired = requirementCategoryContract.IsComplianceRequired;
                    reqCtgryObject.RC_ComplianceRqdStartDate = requirementCategoryContract.ComplianceReqStartDate;
                    reqCtgryObject.RC_ComplianceRqdEndDate = requirementCategoryContract.ComplianceReqEndDate;
                    //UAT-2603//
                    reqCtgryObject.RC_AllowDataMovement = requirementCategoryContract.AllowDataMovement;
                    //UAT-3792 
                    // reqCtgryObject.RC_AllowApplicantEdit = requirementCategoryContract.AllowApplicantToEdit;
                    //UAT-3161
                    // reqCtgryObject.RC_SampleDocFormUrlLabel = requirementCategoryContract.RequirementDocumentLinkLabel; //Commented in UAT-4254
                    reqCtgryObject.RC_SendItemDocOnApproval = requirementCategoryContract.SendItemDoconApproval; //UAT-3805
                    reqCtgryObject.RC_TriggerOtherCategoryRules = requirementCategoryContract.TriggerOtherCategoryRules; // UAT-4259
                    LargeContent largeContent = _sharedDataDBContext.LargeContents
                                                                .Where(cond => cond.LC_ObjectID == requirementCategoryContract.RequirementCategoryID && !cond.LC_IsDeleted
                                                                && cond.lkpLargeContentType.LCT_Code == explanatoryNotesContentTypeCode
                                                                && cond.lkpObjectType.OT_Code == reqrmnCategoryObjectTypeCode)
                                                                .FirstOrDefault();
                    if (!largeContent.IsNullOrEmpty())
                    {
                        largeContent.LC_Content = requirementCategoryContract.ExplanatoryNotes;
                        largeContent.LC_ModifiedByID = currentLoggedInUserId;
                        largeContent.LC_ModifiedOn = DateTime.Now;
                    }
                    #region UAT-2213
                    //UAT-2213:New Rotation Package Process: Master Setup
                    else
                    {
                        if (!requirementCategoryContract.ExplanatoryNotes.IsNullOrEmpty())
                        {
                            Int32 contentTypeId = _sharedDataDBContext.lkpLargeContentTypes
                                             .Where(obj => obj.LCT_Code == explanatoryNotesContentTypeCode && !obj.LCT_IsDeleted).FirstOrDefault().LCT_ID;
                            Int32 objectTypeId = _sharedDataDBContext.lkpObjectTypes
                                                            .Where(obj => obj.OT_Code == reqrmnCategoryObjectTypeCode && obj.OT_IsDeleted == false).FirstOrDefault().OT_ID;
                            LargeContent largeContentNew = new LargeContent();
                            largeContentNew.LC_Content = requirementCategoryContract.ExplanatoryNotes;
                            largeContentNew.LC_ObjectID = reqCtgryObject.RC_ID;
                            largeContentNew.LC_ObjectTypeID = objectTypeId;
                            largeContentNew.LC_LargeContentTypeID = contentTypeId;
                            largeContentNew.LC_CreatedByID = currentLoggedInUserId;
                            largeContentNew.LC_CreatedOn = DateTime.Now;
                            _sharedDataDBContext.LargeContents.AddObject(largeContentNew);
                        }
                    }
                    #endregion

                    #region UAT-2165 : Rotation Requirements | Enhanced Rule Functionality (needed for Memorial's Flu Shots)
                    //Updating the RequirementPackageCategory on update of category.
                    List<RequirementPackageCategory> lstReqPkgCategory = reqCtgryObject.RequirementPackageCategories.ToList();
                    foreach (RequirementPackageCategory reqPkgCategory in lstReqPkgCategory)
                    {

                        if (!reqPkgCategory.IsNullOrEmpty())
                        {
                            reqPkgCategory.RPC_ComplianceRequired = requirementCategoryContract.IsComplianceRequired;
                            reqPkgCategory.RPC_ComplianceRqdStartDate = requirementCategoryContract.ComplianceReqStartDate;
                            reqPkgCategory.RPC_ComplianceRqdEndDate = requirementCategoryContract.ComplianceReqEndDate;
                            reqPkgCategory.RPC_ModifiedByID = currentLoggedInUserId;
                            reqPkgCategory.RPC_ModifiedOn = DateTime.Now;
                        }
                    }
                    #endregion

                    #region UAT-4165
                    RequirementObjectProperty reqObjProp = _sharedDataDBContext.RequirementObjectProperties.Where(a => a.ROTP_CategoryID == reqCtgryObject.RC_ID && !a.ROTP_IsDeleted && a.ROTP_CategoryItemID == null && a.ROTP_ItemFieldID == null).FirstOrDefault();

                    if (!reqObjProp.IsNullOrEmpty())
                    {
                        reqObjProp.ROTP_IsEditableByAdmin = Convert.ToBoolean(requirementCategoryContract.SelectedlstEditableBy.Where(x => x.Key == RotationDataEditableBy.ADBAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                        reqObjProp.ROTP_IsEditableByClientAdmin = Convert.ToBoolean(requirementCategoryContract.SelectedlstEditableBy.Where(x => x.Key == RotationDataEditableBy.ClientAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                        reqObjProp.ROTP_IsEditableByApplicant = Convert.ToBoolean(requirementCategoryContract.SelectedlstEditableBy.Where(x => x.Key == RotationDataEditableBy.Applicant.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                        reqObjProp.ROTP_ModifiedBy = currentLoggedInUserId;
                        reqObjProp.ROTP_ModifiedOn = DateTime.Now;
                    }
                    else
                    {
                        if (!requirementCategoryContract.SelectedlstEditableBy.IsNullOrEmpty())
                        {
                            RequirementObjectProperty reqObjPropData = new RequirementObjectProperty();
                            reqObjPropData.ROTP_CategoryID = reqCtgryObject.RC_ID;
                            reqObjPropData.ROTP_CategoryItemID = null;
                            reqObjPropData.ROTP_IsDeleted = false;
                            reqObjPropData.ROTP_IsCustomSettings = null;
                            reqObjPropData.ROTP_IsEditableByAdmin = Convert.ToBoolean(requirementCategoryContract.SelectedlstEditableBy.Where(x => x.Key == RotationDataEditableBy.ADBAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                            reqObjPropData.ROTP_IsEditableByClientAdmin = Convert.ToBoolean(requirementCategoryContract.SelectedlstEditableBy.Where(x => x.Key == RotationDataEditableBy.ClientAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                            reqObjPropData.ROTP_IsEditableByApplicant = Convert.ToBoolean(requirementCategoryContract.SelectedlstEditableBy.Where(x => x.Key == RotationDataEditableBy.Applicant.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                            reqObjPropData.ROTP_CreatedBy = currentLoggedInUserId;
                            reqObjPropData.ROTP_CreatedOn = DateTime.Now;
                            _sharedDataDBContext.RequirementObjectProperties.AddObject(reqObjPropData);
                        }
                    }
                    #endregion


                    #region UAT-4254

                    String ActionTypeCodeRemoveForCatUrl = String.Empty;
                    String ActionTypeCodeAddForCatUrl = String.Empty;
                    String ActionTypeCodeUpdateForCatUrl = String.Empty;

                    List<RequirementCategoryDocLink> lstExistingDocUrls = _sharedDataDBContext.RequirementCategoryDocLinks.Where(c => !c.RCDL_IsDeleted
                                                                                                && c.RCDL_RequirementCategoryID == reqCtgryObject.RC_ID).ToList();

                    //Delete removed urls from category
                    foreach (RequirementCategoryDocLink requirementCatDocURL in lstExistingDocUrls)
                    {
                        if ((!requirementCategoryContract.lstReqCatDocUrls.IsNullOrEmpty()
                                   && !requirementCategoryContract.lstReqCatDocUrls.Any(x => x.RequirementCatDocLinkID == requirementCatDocURL.RCDL_ID && x.RequirementCatDocLinkID > AppConsts.NONE))
                            || requirementCategoryContract.lstReqCatDocUrls.IsNullOrEmpty())
                        {
                            requirementCatDocURL.RCDL_IsDeleted = true;
                            requirementCatDocURL.RCDL_ModifiedBy = currentLoggedInUserId;
                            requirementCatDocURL.RCDL_ModifiedOn = DateTime.Now;
                            ActionTypeCodeRemoveForCatUrl = RequirementPackageObjectActionTypeEnum.REMOVED.GetStringValue();
                        }
                    }

                    if (!requirementCategoryContract.lstReqCatDocUrls.IsNullOrEmpty() && requirementCategoryContract.lstReqCatDocUrls.Count > AppConsts.NONE)
                    {
                        //New Records Add here.....
                        foreach (RequirementCategoryDocUrl reqCatDocUrl in requirementCategoryContract.lstReqCatDocUrls)
                        {
                            if (reqCatDocUrl.RequirementCatDocLinkID <= AppConsts.NONE)
                            {
                                RequirementCategoryDocLink newAddedReqCatDocLink = new RequirementCategoryDocLink();
                                newAddedReqCatDocLink.RCDL_SampleDocFormURL = reqCatDocUrl.RequirementCatDocUrl;
                                newAddedReqCatDocLink.RCDL_SampleDocFormUrlLabel = reqCatDocUrl.RequirementCatDocUrlLabel;
                                newAddedReqCatDocLink.RCDL_RequirementCategoryID = reqCtgryObject.RC_ID; //reqCatDocUrl.RequirementCatID;
                                newAddedReqCatDocLink.RCDL_CreatedBy = currentLoggedInUserId;
                                newAddedReqCatDocLink.RCDL_CreatedOn = DateTime.Now;
                                ActionTypeCodeAddForCatUrl = RequirementPackageObjectActionTypeEnum.ADDED.GetStringValue();
                                _sharedDataDBContext.RequirementCategoryDocLinks.AddObject(newAddedReqCatDocLink);
                            }
                        }
                    }

                    if (!lstExistingDocUrls.IsNullOrEmpty() && lstExistingDocUrls.Count > AppConsts.NONE)
                    {
                        //foreach (RequirementCategoryDocLink reqCatDocUrl in lstExistingDocUrls)
                        //{
                        //    //Previous Records Found and update
                        //    if (!requirementCategoryContract.lstReqCatDocUrls.IsNullOrEmpty() && requirementCategoryContract.lstReqCatDocUrls.Count > AppConsts.NONE
                        //        && requirementCategoryContract.lstReqCatDocUrls.Any(x => x.RequirementCatDocLinkID == reqCatDocUrl.RCDL_ID))
                        //    {
                        //        RequirementCategoryDocUrl reqCatDocUrlToUpdate = requirementCategoryContract.lstReqCatDocUrls.Where(x => x.RequirementCatDocLinkID == reqCatDocUrl.RCDL_ID).FirstOrDefault();

                        //        reqCatDocUrl.RCDL_SampleDocFormUrlLabel = reqCatDocUrlToUpdate.RequirementCatDocUrlLabel;
                        //        reqCatDocUrl.RCDL_SampleDocFormURL = reqCatDocUrlToUpdate.RequirementCatDocUrl;
                        //        reqCatDocUrl.RCDL_ModifiedBy = currentLoggedInUserId;
                        //        reqCatDocUrl.RCDL_ModifiedOn = DateTime.Now;
                        //        ActionTypeCodeUpdateForCatUrl = RequirementPackageObjectActionTypeEnum.EDITED.GetStringValue();
                        //    }
                        //}

                        foreach (RequirementCategoryDocUrl reqCatDocUrl in requirementCategoryContract.lstReqCatDocUrls.Where(c => c.RequirementCatDocLinkID > AppConsts.NONE).ToList())
                        {
                            RequirementCategoryDocLink reqCatDocUrlToUpdat = lstExistingDocUrls.Where(c => c.RCDL_ID == reqCatDocUrl.RequirementCatDocLinkID).FirstOrDefault();

                            reqCatDocUrlToUpdat.RCDL_SampleDocFormUrlLabel = reqCatDocUrl.RequirementCatDocUrlLabel;
                            reqCatDocUrlToUpdat.RCDL_SampleDocFormURL = reqCatDocUrl.RequirementCatDocUrl;
                            reqCatDocUrlToUpdat.RCDL_ModifiedBy = currentLoggedInUserId;
                            reqCatDocUrlToUpdat.RCDL_ModifiedOn = DateTime.Now;
                            ActionTypeCodeUpdateForCatUrl = RequirementPackageObjectActionTypeEnum.EDITED.GetStringValue();
                        }
                    }

                    #endregion


                    _sharedDataDBContext.SaveChanges();
                    #region Rot Pkg Object Sync Data
                    List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
                    RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                    objectData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.EDITED.GetStringValue(); ;
                    objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_CATEGORY.GetStringValue();
                    objectData.NewObjectId = requirementCategoryContract.RequirementCategoryID;
                    lstPackageObjectSynchingData.Add(objectData);

                    //Added in UAT-4254
                    //if (!String.IsNullOrEmpty(ActionTypeCodeUpdateForCatUrl))
                    //{
                    //    RequirementPackageObjectSynchingContract synchingData = new RequirementPackageObjectSynchingContract();
                    //    synchingData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_CATEGORY_DOC_URL.GetStringValue();
                    //    synchingData.NewObjectId = requirementCategoryContract.RequirementCategoryID;
                    //    synchingData.ActionTypeCode = ActionTypeCodeUpdateForCatUrl;
                    //    lstPackageObjectSynchingData.Add(synchingData);
                    //}
                    //if (!String.IsNullOrEmpty(ActionTypeCodeAddForCatUrl))
                    //{
                    //    RequirementPackageObjectSynchingContract synchingData = new RequirementPackageObjectSynchingContract();
                    //    synchingData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_CATEGORY_DOC_URL.GetStringValue();
                    //    synchingData.NewObjectId = requirementCategoryContract.RequirementCategoryID;
                    //    synchingData.ActionTypeCode = ActionTypeCodeAddForCatUrl;
                    //    lstPackageObjectSynchingData.Add(synchingData);
                    //}
                    //if (!String.IsNullOrEmpty(ActionTypeCodeRemoveForCatUrl))
                    //{
                    //    RequirementPackageObjectSynchingContract synchingData = new RequirementPackageObjectSynchingContract();
                    //    synchingData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_CATEGORY_DOC_URL.GetStringValue();
                    //    synchingData.NewObjectId = requirementCategoryContract.RequirementCategoryID;
                    //    synchingData.ActionTypeCode = ActionTypeCodeRemoveForCatUrl;
                    //    lstPackageObjectSynchingData.Add(synchingData);
                    //}
                    //END


                    String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                    Int32 objectId = 0;
                    SaveRequirementPackageObjectForSync(requestXML, currentLoggedInUserId, out objectId);
                    #endregion
                }
                //Insert Requirement Category Data
                else
                {

                    reqCtgryObject.RC_CategoryName = requirementCategoryContract.RequirementCategoryName;
                    reqCtgryObject.RC_CategoryLabel = requirementCategoryContract.RequirementCategoryLabel;
                    //reqCtgryObject.RC_SampleDocFormURL = requirementCategoryContract.RequirementDocumentLink; //Commented in UAT-4254
                    reqCtgryObject.RC_IsDeleted = false;
                    reqCtgryObject.RC_Code = Guid.NewGuid();
                    reqCtgryObject.RC_CreatedByID = currentLoggedInUserId;
                    reqCtgryObject.RC_CreatedOn = DateTime.Now;
                    reqCtgryObject.RC_ComplianceRequired = requirementCategoryContract.IsComplianceRequired;
                    reqCtgryObject.RC_ComplianceRqdStartDate = requirementCategoryContract.ComplianceReqStartDate;
                    reqCtgryObject.RC_ComplianceRqdEndDate = requirementCategoryContract.ComplianceReqEndDate;
                    reqCtgryObject.RC_IsNewCategory = true;
                    //UAT-2603//
                    reqCtgryObject.RC_AllowDataMovement = requirementCategoryContract.AllowDataMovement;
                    //UAT-3792 
                    // reqCtgryObject.RC_AllowApplicantEdit = requirementCategoryContract.AllowApplicantToEdit;
                    //UAT-3161
                    //reqCtgryObject.RC_SampleDocFormUrlLabel = requirementCategoryContract.RequirementDocumentLinkLabel; //Commented in UAT-4254
                    reqCtgryObject.RC_SendItemDocOnApproval = requirementCategoryContract.SendItemDoconApproval; //UAT-
                    reqCtgryObject.RC_TriggerOtherCategoryRules = requirementCategoryContract.TriggerOtherCategoryRules; // UAT-4259
                    _sharedDataDBContext.RequirementCategories.AddObject(reqCtgryObject);

                    Boolean isSavedSuccessfully = _sharedDataDBContext.SaveChanges() > AppConsts.NONE;
                    if (isSavedSuccessfully && !requirementCategoryContract.ExplanatoryNotes.IsNullOrEmpty())
                    {
                        Int32 contentTypeId = _sharedDataDBContext.lkpLargeContentTypes
                                              .Where(obj => obj.LCT_Code == explanatoryNotesContentTypeCode && !obj.LCT_IsDeleted).FirstOrDefault().LCT_ID;
                        Int32 objectTypeId = _sharedDataDBContext.lkpObjectTypes
                                                        .Where(obj => obj.OT_Code == reqrmnCategoryObjectTypeCode && obj.OT_IsDeleted == false).FirstOrDefault().OT_ID;
                        LargeContent largeContent = new LargeContent();
                        largeContent.LC_Content = requirementCategoryContract.ExplanatoryNotes;
                        largeContent.LC_ObjectID = reqCtgryObject.RC_ID;
                        largeContent.LC_ObjectTypeID = objectTypeId;
                        largeContent.LC_LargeContentTypeID = contentTypeId;
                        largeContent.LC_CreatedByID = currentLoggedInUserId;
                        largeContent.LC_CreatedOn = DateTime.Now;
                        _sharedDataDBContext.LargeContents.AddObject(largeContent);
                        _sharedDataDBContext.SaveChanges();
                    }

                    #region UAT-4165
                    if (isSavedSuccessfully && !requirementCategoryContract.SelectedlstEditableBy.IsNullOrEmpty())
                    {
                        RequirementObjectProperty reqObjProp = new RequirementObjectProperty();
                        reqObjProp.ROTP_CategoryID = reqCtgryObject.RC_ID;
                        reqObjProp.ROTP_CategoryItemID = null;
                        reqObjProp.ROTP_IsDeleted = false;
                        reqObjProp.ROTP_IsCustomSettings = null;
                        reqObjProp.ROTP_IsEditableByAdmin = Convert.ToBoolean(requirementCategoryContract.SelectedlstEditableBy.Where(x => x.Key == RotationDataEditableBy.ADBAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                        reqObjProp.ROTP_IsEditableByClientAdmin = Convert.ToBoolean(requirementCategoryContract.SelectedlstEditableBy.Where(x => x.Key == RotationDataEditableBy.ClientAdmin.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                        reqObjProp.ROTP_IsEditableByApplicant = Convert.ToBoolean(requirementCategoryContract.SelectedlstEditableBy.Where(x => x.Key == RotationDataEditableBy.Applicant.GetStringValue()).Select(a => a.Value).FirstOrDefault());
                        reqObjProp.ROTP_CreatedBy = currentLoggedInUserId;
                        reqObjProp.ROTP_CreatedOn = DateTime.Now;
                        _sharedDataDBContext.RequirementObjectProperties.AddObject(reqObjProp);
                        _sharedDataDBContext.SaveChanges();
                    }

                    #endregion

                    #region UAT-4254
                    if (!requirementCategoryContract.lstReqCatDocUrls.IsNullOrEmpty() && requirementCategoryContract.lstReqCatDocUrls.Count > AppConsts.NONE)
                    {
                        String ActionTypeCodeAddForCatUrl = String.Empty;
                        foreach (RequirementCategoryDocUrl reqCatDocUrl in requirementCategoryContract.lstReqCatDocUrls)
                        {
                            RequirementCategoryDocLink reqCatDocLink = new RequirementCategoryDocLink();
                            reqCatDocLink.RCDL_SampleDocFormURL = reqCatDocUrl.RequirementCatDocUrl;
                            reqCatDocLink.RCDL_SampleDocFormUrlLabel = reqCatDocUrl.RequirementCatDocUrlLabel;
                            reqCatDocLink.RCDL_RequirementCategoryID = reqCtgryObject.RC_ID;
                            reqCatDocLink.RCDL_CreatedBy = currentLoggedInUserId;
                            reqCatDocLink.RCDL_CreatedOn = DateTime.Now;
                            ActionTypeCodeAddForCatUrl = RequirementPackageObjectActionTypeEnum.ADDED.GetStringValue();
                            _sharedDataDBContext.RequirementCategoryDocLinks.AddObject(reqCatDocLink);

                        }
                        _sharedDataDBContext.SaveChanges();
                    }

                    #endregion
                }
                return reqCtgryObject.RC_ID;
            }
            catch (Exception exp)
            {
                return reqCtgryObject.RC_ID;
            }

        }

        public List<RequirementCategoryContract> GetMasterRequirementCategories(RequirementCategoryContract requirementCategoryDetailsContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<RequirementCategoryContract> requirementCategoriesDetailList = new List<RequirementCategoryContract>();
            String orderBy = "CategoryID";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@CategoryName", requirementCategoryDetailsContract.RequirementCategoryName),
                            new SqlParameter("@CategoryLabel", requirementCategoryDetailsContract.RequirementCategoryLabel),
                            new SqlParameter("@AgencyIds", requirementCategoryDetailsContract.lstSelectedAgencyIds),
                            new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                            new SqlParameter("@PageSize", customPagingArgsContract.PageSize)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetMasterRotationCategoriesDetails", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementCategoryContract requirementPackageDetail = new RequirementCategoryContract();
                            requirementPackageDetail.RequirementCategoryID = dr["CategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CategoryID"]);
                            requirementPackageDetail.RequirementCategoryName = dr["CategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CategoryName"]);
                            requirementPackageDetail.RequirementCategoryLabel = dr["CategoryLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CategoryLabel"]);
                            requirementPackageDetail.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                            requirementPackageDetail.AgencyNames = dr["AgencyNames"] != DBNull.Value ? Convert.ToString(dr["AgencyNames"]) : String.Empty;
                            //requirementPackageDetail.UniversalCategoryID = dr["UniversalCategoryID"] != DBNull.Value ? Convert.ToInt32(dr["UniversalCategoryID"]) : 0;
                            //requirementPackageDetail.ComplianceReqStartDate = dr["ComplianceRqdStartDate"] != DBNull.Value ? Convert.ToString(dr["ComplianceRqdStartDate"]) : String.Empty;
                            requirementCategoriesDetailList.Add(requirementPackageDetail);
                        }
                    }
                }
            }
            return requirementCategoriesDetailList;
        }

        public String DeleteRequirementCategory(int caregoryId)
        {
            String _retVal = string.Empty;
            RequirementPackageCategory objPackageCategory = _sharedDataDBContext.RequirementPackageCategories.Where(a => a.RPC_RequirementCategoryID == caregoryId && a.RPC_IsDeleted == false).FirstOrDefault();

            if (objPackageCategory == null)
            {
                RequirementObjectProperty objReqProp = _sharedDataDBContext.RequirementObjectProperties.Where(a => a.ROTP_CategoryID == caregoryId && a.ROTP_IsDeleted == false && a.ROTP_CategoryItemID == null && a.ROTP_ItemFieldID == null).FirstOrDefault();
                if (!objReqProp.IsNullOrEmpty())
                {
                    objReqProp.ROTP_IsDeleted = true;
                }
                RequirementCategory objCaregory = _sharedDataDBContext.RequirementCategories.Where(a => a.RC_ID == caregoryId).FirstOrDefault();
                if (objCaregory != null)
                {
                    objCaregory.RC_IsDeleted = true;
                }

                //UAT-4254
                List<RequirementCategoryDocLink> lstReqCatDocUrls = _sharedDataDBContext.RequirementCategoryDocLinks.Where(c => !c.RCDL_IsDeleted && c.RCDL_RequirementCategoryID == caregoryId).ToList();
                if (!lstReqCatDocUrls.IsNullOrEmpty() && lstReqCatDocUrls.Count > AppConsts.NONE)
                {
                    lstReqCatDocUrls.ForEach(docUrl =>
                    {
                        docUrl.RCDL_IsDeleted = true;
                    });
                }
                //END

                _sharedDataDBContext.SaveChanges();
                _retVal = "1";
            }
            else { _retVal = "2"; }
            return _retVal;

        }


        /// <summary>
        /// UAT: 4279
        /// Updates the Display order of the Category Mapping Display Order
        /// </summary>
        /// <param name="lstDPMIds"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="currentUserId"></param>
        public Boolean UpdatePackageCategoryMappingDisplayOrder(List<RequirementCategoryContract> CategoryId, Int32? destinationIndex, Int32 currentUserId, Int32 requirementPkgId)
        {
            DataTable dtNodes = new DataTable();
            dtNodes.Columns.Add("RPC_ID", typeof(Int32));
            dtNodes.Columns.Add("RPC_RequirementPackageID", typeof(Int32));
            dtNodes.Columns.Add("DestinationIndex", typeof(Int32));
            dtNodes.Columns.Add("CurrentLoggedInUserId", typeof(Int32));
            foreach (var dpmId in CategoryId)
            {
                dtNodes.Rows.Add(new object[] { Convert.ToInt32(dpmId.RequirementCategoryID), requirementPkgId, dpmId.CategoryDisplayOrder, currentUserId });

            }

            using (SqlConnection con = new SqlConnection())
            {
                EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
                List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
                List<RequirementPackageObjectSynchingContract> lstCategoryAddedRemoved = new List<RequirementPackageObjectSynchingContract>();

                con.ConnectionString = connection.StoreConnection.ConnectionString;
                SqlCommand _command = new SqlCommand("UpdateDisplaySequencePackageCategoryMapping", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@typeRequirementPackageCategory", dtNodes);
                con.Open();
                Int32 rowsAffected = _command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    con.Close();



                    RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                    objectData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.DisplayOrderChangeCategoryFromPackage.GetStringValue();
                    objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_PACKAGE.GetStringValue();
                    objectData.NewObjectId = Convert.ToInt32(dtNodes.Rows[0]["RPC_RequirementPackageID"]);
                    lstPackageObjectSynchingData.Add(objectData);
                    Int32 objectId = 0;
                    if (!lstPackageObjectSynchingData.IsNullOrEmpty())
                    {
                        String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                        SaveRequirementPackageObjectForSync(requestXML, currentUserId, out objectId);
                    }

                    return true;
                }
            }
            return false;
        }

        List<RequirementCategoryContract> ISharedRequirementPackageRepository.GetPackageCategoryMapping(PackageCategoryMappingContract packageCategoryMappingContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<RequirementCategoryContract> lstRequirementCategory = new List<RequirementCategoryContract>();

            String orderBy = "CategoryDisplayOrder";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";


            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@ReqPackageID", packageCategoryMappingContract.ReqPackageID),
                            new SqlParameter("@ReqCategoryName", packageCategoryMappingContract.ReqCategoryName),
                            new SqlParameter("@ResultTypeId", packageCategoryMappingContract.ResultTypeID),
                            new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                            new SqlParameter("@PageSize", customPagingArgsContract.PageSize)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetPackageCategoryMapping", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementCategoryContract requirementCategoryContract = new RequirementCategoryContract();
                            requirementCategoryContract.RequirementCategoryID = dr["RequirementCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementCategoryID"]);
                            requirementCategoryContract.RequirementCategoryName = dr["RequirementCategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementCategoryName"]);
                            requirementCategoryContract.RequirementCategoryLabel = dr["RequirementCategoryLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementCategoryLabel"]);
                            requirementCategoryContract.IsCategoryMappedWithPkg = dr["IsReqCatMappedWithPkg"] != DBNull.Value ? Convert.ToBoolean(dr["IsReqCatMappedWithPkg"]) : false;
                            requirementCategoryContract.CategoryDisplayOrder = dr["CategoryDisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CategoryDisplayOrder"]);
                            requirementCategoryContract.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                            lstRequirementCategory.Add(requirementCategoryContract);
                        }
                    }
                }
            }

            return lstRequirementCategory;

        }

        Boolean ISharedRequirementPackageRepository.SavePackageCategoryMapping(Int32 currentOrgUserID, Int32 requirementPackageID, String requirementCategoryIds, Boolean IsRotationPkgCopyFromAgencyHierarchy, Int32 ExistingPkgId = 0)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_SavePackageCategoryMapping", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReqpackageID", requirementPackageID);
                cmd.Parameters.AddWithValue("@ReqCatIds", requirementCategoryIds);
                cmd.Parameters.AddWithValue("@CurrentOrgUserID", currentOrgUserID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ExistingPkgId > AppConsts.NONE)
                {
                    var listOldRequirementPkgCat = _sharedDataDBContext.RequirementPackageCategories.Where(x => x.RPC_RequirementPackageID == ExistingPkgId && x.RPC_IsDeleted == false).ToList();
                    if (listOldRequirementPkgCat.IsNotNull() && listOldRequirementPkgCat.Count > AppConsts.NONE)
                    {
                        foreach (var item in listOldRequirementPkgCat)
                        {
                            var RequirementpkgCat = _sharedDataDBContext.RequirementPackageCategories.Where(Counter => Counter.RPC_RequirementCategoryID == item.RPC_RequirementCategoryID && Counter.RPC_RequirementPackageID == requirementPackageID && Counter.RPC_IsDeleted == false).FirstOrDefault();
                            RequirementpkgCat.RPC_CDisplayOrder = item.RPC_CDisplayOrder;
                            RequirementpkgCat.RPC_CreatedByID = currentOrgUserID;
                            RequirementpkgCat.RPC_CreatedOn = DateTime.Now;
                            _sharedDataDBContext.SaveChanges();
                        }


                    }
                }
                List<RequirementPackageObjectSynchingContract> lstCategoryAddedRemoved = new List<RequirementPackageObjectSynchingContract>();
                if (!(IsRotationPkgCopyFromAgencyHierarchy))
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dRow in ds.Tables[0].Rows)
                        {
                            RequirementPackageObjectSynchingContract objCatgeoryAddedRemoved = new RequirementPackageObjectSynchingContract();

                            objCatgeoryAddedRemoved.CategoryID = (Convert.ToInt32(dRow["CategoryID"]));
                            objCatgeoryAddedRemoved.IsAdded = (Convert.ToBoolean(dRow["IsAdded"]));
                            objCatgeoryAddedRemoved.IsRemoved = (Convert.ToBoolean(dRow["IsRemoved"]));
                            lstCategoryAddedRemoved.Add(objCatgeoryAddedRemoved);
                        }

                    }
                    //cmd.ExecuteScalar();
                    List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
                    foreach (RequirementPackageObjectSynchingContract item in lstCategoryAddedRemoved.Where(x => x.IsAdded))
                    {
                        RequirementPackageObjectSynchingContract objectDataAdded = new RequirementPackageObjectSynchingContract();
                        objectDataAdded.ActionTypeCode = RequirementPackageObjectActionTypeEnum.ADDED.GetStringValue();
                        objectDataAdded.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_CATEGORY.GetStringValue();
                        objectDataAdded.NewObjectId = Convert.ToInt32(item.CategoryID);
                        lstPackageObjectSynchingData.Add(objectDataAdded);
                    }

                    //foreach (RequirementPackageObjectSynchingContract item in lstCategoryAddedRemoved.Where(x => x.IsRemoved))
                    //{
                    //    RequirementPackageObjectSynchingContract objectDataRemoved = new RequirementPackageObjectSynchingContract();
                    //    objectDataRemoved.ActionTypeCode = RequirementPackageObjectActionTypeEnum.REMOVED.GetStringValue();
                    //    objectDataRemoved.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_CATEGORY.GetStringValue();
                    //    objectDataRemoved.NewObjectId = Convert.ToInt32(item.CategoryID);
                    //    lstPackageObjectSynchingData.Add(objectDataRemoved);
                    //}

                    //UAT-3230
                    //List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
                    if (lstCategoryAddedRemoved.Any(cond => cond.IsRemoved))
                    {
                        RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                        objectData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.CATEGORY_REMOVE_FROM_PACKAGE.GetStringValue();
                        objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_PACKAGE.GetStringValue();
                        objectData.NewObjectId = Convert.ToInt32(requirementPackageID);
                        lstPackageObjectSynchingData.Add(objectData);
                    }
                    Int32 objectId = 0;
                    if (!lstPackageObjectSynchingData.IsNullOrEmpty())
                    {
                        String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                        SaveRequirementPackageObjectForSync(requestXML, currentOrgUserID, out objectId);
                    }
                }

                return true;
            }
        }


        public List<RequirementCategoryContract> GetAllRequirmentCategories()
        {
            return base.SharedDataDBContext.RequirementCategories.Where(cond => !cond.RC_IsDeleted && cond.RC_IsNewCategory)
                 .Select(s => new RequirementCategoryContract
                 {
                     RequirementCategoryName = s.RC_CategoryName, //string.IsNullOrEmpty(s.RC_CategoryLabel) ? s.RC_CategoryName : s.RC_CategoryLabel,
                     RequirementCategoryID = s.RC_ID
                 }).ToList();
        }

        //Parameter requirementPkgVersioningStatus_DueId added for UAT-4657
        Int32 ISharedRequirementPackageRepository.SaveMasterRequirementPackage(RequirementPackageContract requirementPackageContract, Int32 currentLoggedInUserID, Boolean IsRotationPkgCopyFromAgencyHierarchy, Int32 requirementPkgVersioningStatus_DueId)
        {
            RequirementPackage rqmntPkg;
            List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
            if (requirementPackageContract.RequirementPackageID > AppConsts.NONE)
            {
                rqmntPkg = _sharedDataDBContext.RequirementPackages.FirstOrDefault(cond => cond.RP_ID == requirementPackageContract.RequirementPackageID);
                rqmntPkg.RP_PackageName = requirementPackageContract.RequirementPackageName;
                rqmntPkg.RP_PackageLabel = requirementPackageContract.RequirementPackageLabel;
                rqmntPkg.RP_ModifiedByID = currentLoggedInUserID;
                rqmntPkg.RP_ModifiedOn = DateTime.Now;
                rqmntPkg.RP_RequirementPackageTypeID = requirementPackageContract.RequirementPkgTypeID;
                rqmntPkg.RP_DefinedRequirementID = requirementPackageContract.DefinedRequirementID;
                rqmntPkg.RP_EffectiveStartDate = requirementPackageContract.EffectiveStartDate;
                rqmntPkg.RP_EffectiveEndDate = requirementPackageContract.EffectiveEndDate;
                rqmntPkg.RP_IsNewPackage = true;
                rqmntPkg.RP_ReqReviewByID = requirementPackageContract.ReqReviewByID;
                //if (!rqmntPkg.IsNullOrEmpty())
                //{
                //    IEnumerable<RequirementPackageAgency> existingRequirementPackageAgencies = rqmntPkg.RequirementPackageAgencies
                //                                                                                                 .Where(cond => !cond.RPA_IsDeleted);
                //    foreach (RequirementPackageAgency existingRequirementPackageAgency in existingRequirementPackageAgencies)
                //    {
                //        if (!requirementPackageContract.LstAgencyIDs.Contains(existingRequirementPackageAgency.RPA_AgencyID))
                //        {
                //            existingRequirementPackageAgency.RPA_IsDeleted = true;
                //            existingRequirementPackageAgency.RPA_ModifiedByID = currentLoggedInUserID;
                //            existingRequirementPackageAgency.RPA_ModifiedOn = DateTime.Now;
                //        }
                //        else
                //        {
                //            requirementPackageContract.LstAgencyIDs.Remove(existingRequirementPackageAgency.RPA_AgencyID);
                //        }
                //    }
                //    foreach (Int32 agencyID in requirementPackageContract.LstAgencyIDs)
                //    {
                //        RequirementPackageAgency newRequirementPackageAgency = new RequirementPackageAgency()
                //        {
                //            RPA_AgencyID = agencyID,
                //            RPA_IsDeleted = false,
                //            RPA_CreatedByID = currentLoggedInUserID,
                //            RPA_CreatedOn = DateTime.Now,
                //        };
                //        rqmntPkg.RequirementPackageAgencies.Add(newRequirementPackageAgency);
                //    }
                //}
                #region Rot Pkg Object Sync Data
                RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                objectData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.EDITED.GetStringValue();
                objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_PACKAGE.GetStringValue();
                objectData.NewObjectId = requirementPackageContract.RequirementPackageID;
                lstPackageObjectSynchingData.Add(objectData);
                #endregion
            }
            else
            {
                rqmntPkg = new RequirementPackage();
                rqmntPkg.RP_PackageName = requirementPackageContract.RequirementPackageName;
                rqmntPkg.RP_PackageLabel = requirementPackageContract.RequirementPackageLabel;
                rqmntPkg.RP_CreatedByID = currentLoggedInUserID;
                rqmntPkg.RP_CreatedOn = DateTime.Now;
                rqmntPkg.RP_IsDeleted = false;
                rqmntPkg.RP_IsActive = true;
                rqmntPkg.RP_IsUsed = false;
                rqmntPkg.RP_IsCopied = false;
                rqmntPkg.RP_Code = requirementPackageContract.RequirementPackageCode;
                rqmntPkg.RP_RequirementPackageTypeID = requirementPackageContract.RequirementPkgTypeID;
                rqmntPkg.RP_DefinedRequirementID = requirementPackageContract.DefinedRequirementID;
                rqmntPkg.RP_EffectiveStartDate = requirementPackageContract.EffectiveStartDate;
                rqmntPkg.RP_EffectiveEndDate = requirementPackageContract.EffectiveEndDate;
                rqmntPkg.RP_IsNewPackage = true;
                rqmntPkg.RP_ReqReviewByID = requirementPackageContract.ReqReviewByID;
                //UAT-4657 Starts//
                if (requirementPackageContract.existingPackageId > 0 && !requirementPackageContract.IsPackageCopy)
                {
                    rqmntPkg.RP_ParentPackageCode = _sharedDataDBContext.RequirementPackages
                        .FirstOrDefault(con => con.RP_ID == requirementPackageContract.existingPackageId).RP_Code;
                    rqmntPkg.RP_VersionStatusId = requirementPkgVersioningStatus_DueId;
                }
                //End UAT-4657//

                //foreach (Int32 agencyID in requirementPackageContract.LstAgencyIDs)
                //{
                //    RequirementPackageAgency newRequirementPackageAgency = new RequirementPackageAgency()
                //    {
                //        RPA_AgencyID = agencyID,
                //        RPA_IsDeleted = false,
                //        RPA_CreatedByID = currentLoggedInUserID,
                //        RPA_CreatedOn = DateTime.Now,
                //    };
                //    rqmntPkg.RequirementPackageAgencies.Add(newRequirementPackageAgency);
                //}
                _sharedDataDBContext.RequirementPackages.AddObject(rqmntPkg);
            }

            #region UAT-2650
            SaveUpdateAgencyHierarchyPackage(requirementPackageContract.LstAgencyHierarchyIDs, rqmntPkg, currentLoggedInUserID);
            #endregion

            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                Int32 objectId = 0;
                if (!lstPackageObjectSynchingData.IsNullOrEmpty())
                {
                    String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                    SaveRequirementPackageObjectForSync(requestXML, currentLoggedInUserID, out objectId);
                }

            }
            return rqmntPkg.RP_ID;
        }

        public Boolean ArchivePackage(Dictionary<Int32, Boolean> aryPackageIds, Int32 currentLoggedInUserId)
        {
            List<Int32> lstPackageIds = aryPackageIds.Select(x => x.Key).ToList();
            List<RequirementPackage> rqmntPkgList = _sharedDataDBContext.RequirementPackages.Where(cond => lstPackageIds.Contains(cond.RP_ID)).ToList();
            List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
            List<Int32> lstRequirementPackageIds = new List<int>();

            foreach (RequirementPackage packageData in rqmntPkgList)
            {
                //RequirementPackage rqmntPkg = _sharedDataDBContext.RequirementPackages.FirstOrDefault(cond => cond.RP_ID == packageId.Key);
                if (packageData != null)
                {
                    packageData.RP_IsArchived = true;
                }
                lstRequirementPackageIds.Add(packageData.RP_ID);
                #region Rot Pkg Object Sync Data
                RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                objectData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.PACKAGE_ARCHIVED.GetStringValue();
                objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_PACKAGE.GetStringValue();
                objectData.NewObjectId = packageData.RP_ID;
                lstPackageObjectSynchingData.Add(objectData);
                #endregion
            }
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                string RequirementPackageIds = string.Join(",", lstRequirementPackageIds.AsEnumerable());
                UpdateDataAfterArchivedData(RequirementPackageIds, currentLoggedInUserId);
                Int32 objectId = 0;
                #region Rot Pkg Object Sync Data
                String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                SaveRequirementPackageObjectForSync(requestXML, currentLoggedInUserId, out objectId);
                #endregion
                return true;
            }

            return false;
        }

        #region UAT: 4552
        bool UpdateDataAfterArchivedData(string requirementPackageID, Int32 currentLoggedInUserId)
        {

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@RequirementPackageID", requirementPackageID),
                    new SqlParameter("@CurrentUserId", currentLoggedInUserId),

                };

                base.OpenSQLDataReaderConnection(con);
                SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_NewDataSetAfterArchivedData", sqlParameterCollection);
                base.CloseSQLDataReaderConnection(con);
            }
            return true;
        }

        #endregion

        //UAT-4054
        public Boolean UnArchivePackage(Dictionary<Int32, Boolean> aryPackageIds, Int32 currentLoggedInUserId)
        {
            List<Int32> lstPackageIds = aryPackageIds.Select(x => x.Key).ToList();
            List<RequirementPackage> rqmntPkgList = _sharedDataDBContext.RequirementPackages.Where(cond => lstPackageIds.Contains(cond.RP_ID)).ToList();
            List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
            List<Int32> lstRequirementPackageIds = new List<int>();
            foreach (RequirementPackage packageData in rqmntPkgList)
            {
                if (packageData != null)
                {
                    packageData.RP_IsArchived = false;
                }
                lstRequirementPackageIds.Add(packageData.RP_ID);
                #region Rot Pkg Object Sync Data
                RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                objectData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.PACKAGE_UNARCHIVED.GetStringValue();
                objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_PACKAGE.GetStringValue();
                objectData.NewObjectId = packageData.RP_ID;
                lstPackageObjectSynchingData.Add(objectData);
                #endregion
            }
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                Int32 objectId = 0;
                string RequirementPackageIds = string.Join(",", lstRequirementPackageIds.AsEnumerable());
                UpdateDataAfterArchivedData(RequirementPackageIds, currentLoggedInUserId);
                #region Rot Pkg Object Sync Data
                String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                SaveRequirementPackageObjectForSync(requestXML, currentLoggedInUserId, out objectId);
                #endregion
                return true;
            }

            return false;
        }

        List<Int32> ISharedRequirementPackageRepository.GetMappedPackageDetails(Int32 reqCatID)
        {
            return _sharedDataDBContext.RequirementPackageCategories.Where(cond => !cond.RPC_IsDeleted
                                                && !cond.RequirementPackage.RP_IsDeleted && cond.RequirementPackage.RP_IsNewPackage
                                                && cond.RPC_RequirementCategoryID == reqCatID).Select(sel => sel.RequirementPackage.RP_ID).ToList();
        }

        #endregion



        #region
        //UAT: 4526

        List<RequirementAgencyData> ISharedRequirementPackageRepository.GetRequirementAgencyData(Int32 requirementPackageID)
        {
            List<RequirementAgencyData> listRequirementAgencyData = new List<RequirementAgencyData>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@RequirementPackageID", requirementPackageID)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "GetRequirementAgencyData", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementAgencyData requirementAgencyData = new RequirementAgencyData();
                            requirementAgencyData.RequirementPackageAgencyID = dr["RequirementPackageAgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageAgencyID"]);
                            requirementAgencyData.AgencyID = dr["AgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyID"]);
                            requirementAgencyData.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);

                            listRequirementAgencyData.Add(requirementAgencyData);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return listRequirementAgencyData;
        }

        #endregion
        #region UAT-2514 Copy Package
        /// <summary>
        /// New Method for Getting Package Details that is to be copied in Tenant
        /// </summary>
        /// <param name="requirementPackageID"></param>
        /// <returns></returns>
        List<RequirementPackageHierarchicalDetailsContract> ISharedRequirementPackageRepository.GetRequirementPackageHierarchalDetailsByPackageIDNew(Int32 requirementPackageID)
        {
            List<RequirementPackageHierarchicalDetailsContract> requirementPackageHierarchicalDetailList = new List<RequirementPackageHierarchicalDetailsContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@RequirementPackageID", requirementPackageID)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementPackageHierarchyDetailsNew", sqlParameterCollection))
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
                            //requirementPackageHierarchicalDetail.RequirementPackageRuleTypeCode = dr["RequirementPackageRuleTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageRuleTypeCode"]);
                            requirementPackageHierarchicalDetail.RequirementPackageCategoryID = dr["RequirementPackageCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageCategoryID"]);
                            requirementPackageHierarchicalDetail.CategoryDisplayOrder = dr["DisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DisplayOrder"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryID = dr["RequirementCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementCategoryID"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryCode = dr["RequirementCategoryCode"] == DBNull.Value ? Guid.Empty : (Guid)(dr["RequirementCategoryCode"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryName = dr["RequirementCategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementCategoryName"]);
                            //UAT-2514
                            requirementPackageHierarchicalDetail.RequirementPackageEffectiveStartDate = dr["RequirementPackageEffectiveStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RequirementPackageEffectiveStartDate"]);
                            requirementPackageHierarchicalDetail.RequirementPackageEffectiveEndDate = dr["RequirementPackageEffectiveEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RequirementPackageEffectiveEndDate"]);
                            requirementPackageHierarchicalDetail.IsNewRequirementPackage = dr["IsNewPackage"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsNewPackage"]);
                            requirementPackageHierarchicalDetail.IsPackageArchived = dr["IsPackageArchived"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsPackageArchived"]);

                            requirementPackageHierarchicalDetail.RequirementCategoryCompReqdStartDate = dr["CategoryComplianceReqStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["CategoryComplianceReqStartDate"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryCompReqdEndDate = dr["CategoryComplianceReqEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["CategoryComplianceReqEndDate"]);
                            requirementPackageHierarchicalDetail.IsNewRequirementCategory = dr["IsNewCategory"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsNewCategory"]);
                            requirementPackageHierarchicalDetail.IsCategoryComplianceRequired = dr["IsCategoryComplianceRequired"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsCategoryComplianceRequired"]);
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

                            //UAT-3077
                            requirementPackageHierarchicalDetail.IsPaymentTypeItem = dr["IsPaymentTypeItem"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsPaymentTypeItem"]);
                            requirementPackageHierarchicalDetail.ItemAmount = dr["ItemAmount"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(dr["ItemAmount"]);

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
                            requirementPackageHierarchicalDetail.RequirementFieldAttributeTypeID = dr["RequirementFieldAttributeTypeID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementFieldAttributeTypeID"]);
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
                            //requirementPackageHierarchicalDetail.RequirementPackageAgencyID = dr["RequirementPackageAgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageAgencyID"]);
                            //requirementPackageHierarchicalDetail.AgencyID = dr["AgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyID"]);
                            //requirementPackageHierarchicalDetail.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            //  requirementPackageHierarchicalDetail.PackageObjectTreeID = dr["PackageObjectTreeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PackageObjectTreeID"]);
                            requirementPackageHierarchicalDetail.CategoryObjectTreeID = dr["CategoryObjectTreeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CategoryObjectTreeID"]);
                            requirementPackageHierarchicalDetail.ItemObjectTreeID = dr["ItemObjectTreeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ItemObjectTreeID"]);
                            requirementPackageHierarchicalDetail.FieldObjectTreeID = dr["FieldObjectTreeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["FieldObjectTreeID"]);
                            requirementPackageHierarchicalDetail.CategoryExplanatoryNotes = dr["CategoryExplanatoryNotes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CategoryExplanatoryNotes"]);
                            //UAT 1352:As a rotation package creation admin, I should be able to create packages rotation for the instructor/preceptor to use
                            requirementPackageHierarchicalDetail.RequirementPkgTypeID = dr["RequirementPkgTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPkgTypeID"]);
                            requirementPackageHierarchicalDetail.RequirementPkgTypeCode = dr["RequirementPkgTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPkgTypeCode"]);
                            requirementPackageHierarchicalDetail.RequirementPackageInstitutionID = dr["RequirementPackageInstitutionID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageInstitutionID"]);
                            requirementPackageHierarchicalDetail.MappedTenantID = dr["MappedTenantID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["MappedTenantID"]);
                            requirementPackageHierarchicalDetail.MappedTenantName = dr["MappedTenantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["MappedTenantName"]);
                            requirementPackageHierarchicalDetail.IsUsed = dr["IsUsed"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsUsed"]);
                            requirementPackageHierarchicalDetail.RequirementCategorySQLExpression = dr["RequirementCategorySQLExpression"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementCategorySQLExpression"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryUIExpression = dr["RequirementCategoryUIExpression"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementCategoryUIExpression"]);

                            //UAT-2165
                            requirementPackageHierarchicalDetail.IsComplianceRequired = dr["IsComplianceRequired"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsComplianceRequired"]);
                            requirementPackageHierarchicalDetail.ComplianceReqStartDate = dr["ComplianceReqStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ComplianceReqStartDate"]);
                            requirementPackageHierarchicalDetail.ComplianceReqEndDate = dr["ComplianceReqEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ComplianceReqEndDate"]);
                            requirementPackageHierarchicalDetail.ExpirationCondEndDate = dr["ExpirationCondEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ExpirationCondEndDate"]);
                            requirementPackageHierarchicalDetail.ExpirationCondStartDate = dr["ExpirationCondStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["ExpirationCondStartDate"]);

                            #region UAT-2164 : Agency User - Granular Permissions
                            requirementPackageHierarchicalDetail.IsBackgroundDocument = dr["IsBackgroundDocument"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsBackgroundDocument"]);
                            #endregion

                            //UAT-2366
                            requirementPackageHierarchicalDetail.UiRequirementItemID = dr["UiRequirementItemID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["UiRequirementItemID"]);
                            requirementPackageHierarchicalDetail.UiRequirementFieldID = dr["UiRequirementFieldID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["UiRequirementFieldID"]);
                            requirementPackageHierarchicalDetail.RequirementFieldFixedRuleTypeCode = dr["RequirementFieldFixedRuleTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementFieldFixedRuleTypeCode"]);
                            requirementPackageHierarchicalDetail.UiRuleErrorMessage = dr["ErrorMessage"] == DBNull.Value ? String.Empty : Convert.ToString(dr["ErrorMessage"]);

                            //UAT-2332
                            requirementPackageHierarchicalDetail.DefinedRequirementDescription = dr["DefinedRequirementDescription"] != DBNull.Value ? Convert.ToString(dr["DefinedRequirementDescription"]) : String.Empty;
                            requirementPackageHierarchicalDetail.DefinedRequirementID = dr["DefinedRequirementID"] != DBNull.Value ? (Int32?)(dr["DefinedRequirementID"]) : null;

                            //UAT-2603
                            requirementPackageHierarchicalDetail.ReqCatAllowDataMovement = dr["RequirementCategoryAllowDataMovement"] == DBNull.Value ? false : Convert.ToBoolean(dr["RequirementCategoryAllowDataMovement"]);
                            requirementPackageHierarchicalDetail.ReqItmAllowDataMovement = dr["RequirementItemAllowDataMovement"] == DBNull.Value ? false : Convert.ToBoolean(dr["RequirementItemAllowDataMovement"]);
                            requirementPackageHierarchicalDetail.ReqFieldMaxLength = dr["ReqFieldMaxLength"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ReqFieldMaxLength"]);

                            #region UAT-3078
                            requirementPackageHierarchicalDetail.RequirementCategoryItemDisplayOrder = dr["RequirementCategoryItemDisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementCategoryItemDisplayOrder"]);
                            requirementPackageHierarchicalDetail.RequirementItemFieldDisplayOrder = dr["RequirementItemFieldDisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementItemFieldDisplayOrder"]);
                            #endregion

                            requirementPackageHierarchicalDetail.ReqReviewByDesc = dr["ReqReviewDesc"] != DBNull.Value ? Convert.ToString(dr["ReqReviewDesc"]) : String.Empty;
                            requirementPackageHierarchicalDetail.ReqReviewByID = dr["ReqReviewByID"] != DBNull.Value ? (Int32?)(dr["ReqReviewByID"]) : null;

                            //UAT-3805
                            requirementPackageHierarchicalDetail.SendItemDocOnApproval = dr["SendItemDocOnApproval"] == DBNull.Value ? false : Convert.ToBoolean(dr["SendItemDocOnApproval"]);
                            //UAT-3805
                            requirementPackageHierarchicalDetail.AllowItemDataEntry = dr["RequirementItemAllowDataEntry"] == DBNull.Value ? true : Convert.ToBoolean(dr["RequirementItemAllowDataEntry"]);

                            //UAT-3968
                            requirementPackageHierarchicalDetail.RequirementItemSampleDocumentFormURL = dr["ReqItemSampleDocURL"] != DBNull.Value ? Convert.ToString(dr["ReqItemSampleDocURL"]) : String.Empty;
                            requirementPackageHierarchicalDetail.RItemURLSampleDocURL = dr["RIU_SampleDocURL"] != DBNull.Value ? Convert.ToString(dr["RIU_SampleDocURL"]) : String.Empty;
                            requirementPackageHierarchicalDetail.RItemURLLabel = dr["RIU_Label"] != DBNull.Value ? Convert.ToString(dr["RIU_Label"]) : String.Empty;
                            #region UAT-4165
                            requirementPackageHierarchicalDetail.RequirementObjPropCategoryID = dr["RequirementItemPropCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementItemPropCategoryID"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryPropIsEditableByAdmin = dr["RequirementCategoryPropIsEditableByAdmin"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dr["RequirementCategoryPropIsEditableByAdmin"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryPropIsEditableByApplicant = dr["RequirementCategoryPropIsEditableByApplicant"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dr["RequirementCategoryPropIsEditableByApplicant"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryPropIsEditableByClientAdmin = dr["RequirementCategoryPropIsEditableByClientAdmin"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dr["RequirementCategoryPropIsEditableByClientAdmin"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryPropIsCustomSettings = dr["RequirementCategoryPropIsCustomSettings"] != DBNull.Value ? (Boolean?)(dr["RequirementCategoryPropIsCustomSettings"]) : null;
                            requirementPackageHierarchicalDetail.RequirementItemPropCategoryID = dr["RequirementItemPropCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementItemPropCategoryID"]);
                            requirementPackageHierarchicalDetail.RequirementItemPropCategoryItemID = dr["RequirementItemPropCategoryItemID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementItemPropCategoryItemID"]);
                            requirementPackageHierarchicalDetail.RequirementItemPropIsEditableByAdmin = dr["RequirementItemPropIsEditableByAdmin"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dr["RequirementItemPropIsEditableByAdmin"]);
                            requirementPackageHierarchicalDetail.RequirementCategoryPropIsEditableByApplicant = dr["RequirementCategoryPropIsEditableByApplicant"] == DBNull.Value ? false : Convert.ToBoolean(dr["RequirementCategoryPropIsEditableByApplicant"]);
                            requirementPackageHierarchicalDetail.RequirementItemPropIsEditableByApplicant = dr["RequirementItemPropIsEditableByApplicant"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dr["RequirementItemPropIsEditableByApplicant"]);
                            requirementPackageHierarchicalDetail.RequirementItemPropIsEditableByClientAdmin = dr["RequirementItemPropIsEditableByClientAdmin"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dr["RequirementItemPropIsEditableByClientAdmin"]);
                            requirementPackageHierarchicalDetail.RequirementItemPropIsCustomSettings = dr["RequirementItemPropIsCustomSettings"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dr["RequirementItemPropIsCustomSettings"]);
                            #endregion
                            #region UAT-4380
                            requirementPackageHierarchicalDetail.RequirementItemFieldPropCategoryID = dr["RequirementItemFieldPropCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementItemFieldPropCategoryID"]);
                            requirementPackageHierarchicalDetail.RequirementItemFieldPropCategoryItemID = dr["RequirementItemFieldPropCategoryItemID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementItemFieldPropCategoryItemID"]);
                            requirementPackageHierarchicalDetail.RequirementItemFieldPropItemFieldID = dr["RequirementItemFieldPropItemFieldID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementItemFieldPropItemFieldID"]);
                            requirementPackageHierarchicalDetail.RequirementItemFieldPropIsEditableByAdmin = dr["RequirementItemFieldPropIsEditableByAdmin"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dr["RequirementItemFieldPropIsEditableByAdmin"]);
                            requirementPackageHierarchicalDetail.RequirementItemFieldPropIsEditableByApplicant = dr["RequirementItemFieldPropIsEditableByApplicant"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dr["RequirementItemFieldPropIsEditableByApplicant"]);
                            requirementPackageHierarchicalDetail.RequirementItemFieldPropIsEditableByClientAdmin = dr["RequirementItemFieldPropIsEditableByClientAdmin"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dr["RequirementItemFieldPropIsEditableByClientAdmin"]);
                            requirementPackageHierarchicalDetail.RequirementItemFieldPropIsCustomSettings = dr["RequirementItemFieldPropIsCustomSettings"] == DBNull.Value ? (Boolean?)null : Convert.ToBoolean(dr["RequirementItemFieldPropIsCustomSettings"]);
                            #endregion

                            requirementPackageHierarchicalDetail.ParentPackageCode = dr["ParentPackageCode"] == DBNull.Value ? (Guid?)null : (Guid)(dr["ParentPackageCode"]);//UAT-4657
                            requirementPackageHierarchicalDetailList.Add(requirementPackageHierarchicalDetail);
                        }
                    }
                }


                base.CloseSQLDataReaderConnection(con);
            }
            return requirementPackageHierarchicalDetailList;
        }

        #endregion
        #region UAT-2514:
        String ISharedRequirementPackageRepository.GetRequirementPackageObjectIdsToSync(Int32 tenantId, Int32 ChunkSize, Int32 retryTimeLag)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetRotationPackageObjectIdsToSync", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TenantId", tenantId);
                command.Parameters.AddWithValue("@ChunckSize", ChunkSize);
                command.Parameters.AddWithValue("@retryTimeLag", retryTimeLag);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return Convert.ToString(ds.Tables[0].Rows[0]["RequirementPackageObjectIds"]);
                    }
                }
            }
            return String.Empty;
        }

        public Boolean SaveRequirementPackageObjectForSync(String requestDataXML, Int32 currentLoggedInUserId, out Int32 syncReqPkgObjID)
        {
            List<Int32> lstSyncReqPkgObjectIds = new List<Int32>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_AddRequirementPackageObjectsForSynching", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RequestXML", requestDataXML);
                cmd.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                //cmd.ExecuteScalar();
                //UAT-3230
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (var row in ds.Tables[0].AsEnumerable())
                        {
                            Int32 syncReqPkgObjectID = row["SyncRequirementPkgObjectId"] == DBNull.Value ? 0 : Convert.ToInt32(row["SyncRequirementPkgObjectId"]);
                            lstSyncReqPkgObjectIds.Add(syncReqPkgObjectID);
                        }
                    }
                }


                if (!lstSyncReqPkgObjectIds.IsNullOrEmpty())
                {
                    Dictionary<String, Object> dicData = new Dictionary<String, Object>();
                    dicData.Add("syncReqPkgObjectIds", String.Join(",", lstSyncReqPkgObjectIds));
                    dicData.Add("currentLoggedInUserId", currentLoggedInUserId);
                    dicData.Add("LoggerService", DALUtils.LoggerService);
                    var LoggerService = DALUtils.LoggerService;
                    INTSOF.ServiceUtil.ParallelTaskContext.PerformParallelTask(SaveUpdateRequirementPkgSyncDetails, dicData, LoggerService, null);
                }

                syncReqPkgObjID = Convert.ToInt32(String.Join(",", lstSyncReqPkgObjectIds.FirstOrDefault()));
                return true;
            }
        }


        public String ConvertPackageObjectTypeInXML(List<RequirementPackageObjectSynchingContract> lstObjects)
        {
            StringBuilder sb = new StringBuilder();
            foreach (RequirementPackageObjectSynchingContract itm in lstObjects)
            {
                sb.Append("<RequestData>");
                sb.Append("<ObjectId>" + itm.NewObjectId + "</ObjectId>");
                sb.Append("<ObjectTypeCode>" + itm.ObjectTypeCode + "</ObjectTypeCode>");
                sb.Append("<OldObjectId>" + itm.OldObjectId + "</OldObjectId>");
                sb.Append("<ActionTypeCode>" + itm.ActionTypeCode + "</ActionTypeCode>");
                sb.Append("</RequestData>");
            }
            return sb.ToString();
        }
        #endregion

        #region Field Rules
        Int32 ISharedRequirementPackageRepository.GetRequirementObjectTreeIDByprntID(String HID)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlCommand command = new SqlCommand("usp_GetRequirementObjectTreeID", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@HID", HID);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return Convert.ToInt32(ds.Tables[0].Rows[0]["RequirementObjectTreeID"]);
                    }
                }
            }
            return AppConsts.NONE;
        }

        Boolean ISharedRequirementPackageRepository.saveRequirmentFieldRuleDetails()
        {
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        List<RequirementObjectTreeProperty> ISharedRequirementPackageRepository.GetRequirementObjectTreePropertyByID(Int32 requirementObjectTreeID)
        {
            return _sharedDataDBContext.RequirementObjectTreeProperties.Where(cond => cond.ROTP_ObjectTreeID == requirementObjectTreeID && !cond.ROTP_IsDeleted).ToList();
        }
        List<Int32> ISharedRequirementPackageRepository.GetRequirementObjectTreeIDByReqFieldID(Int32 reqFieldID)
        {
            return _sharedDataDBContext.RequirementObjectTrees.Where(cond => cond.ROT_ObjectID == reqFieldID
                                                        && !cond.ROT_IsDeleted && cond.lkpObjectType.OT_Code == ComplainceObjectType.Attribute)
                                                        .Select(sel => sel.ROT_ID).ToList();
        }
        #endregion

        #region Check Rot. Eff. Start Date & EndDate
        RequirementPackageContract ISharedRequirementPackageRepository.CheckRotEffectiveDate(Int32 reqPackageID)
        {

            RequirementPackageContract reqPackageDetailContract = new RequirementPackageContract();
            var reqPkg = _sharedDataDBContext.RequirementPackages.FirstOrDefault(item => item.RP_ID == reqPackageID && !item.RP_IsDeleted);

            if (!reqPkg.IsNullOrEmpty())
            {
                reqPackageDetailContract.RequirementPackageID = reqPkg.RP_ID;
                reqPackageDetailContract.EffectiveStartDate = reqPkg.RP_EffectiveStartDate;
                reqPackageDetailContract.EffectiveEndDate = reqPkg.RP_EffectiveEndDate;
            }
            return reqPackageDetailContract;
        }
        #endregion

        List<Int32> ISharedRequirementPackageRepository.GetCategoryIdsForAssignedField(Int32 requrirementFieldID)
        {
            List<Int32> lstItemIds = _sharedDataDBContext.RequirementItemFields.Where(cond => cond.RIF_RequirementFieldID == requrirementFieldID && !cond.RIF_IsDeleted).Select(sel => sel.RIF_RequirementItemID).ToList();
            return _sharedDataDBContext.RequirementCategoryItems.Where(cond => lstItemIds.Contains(cond.RCI_RequirementItemID) && !cond.RCI_IsDeleted).Select(sel => sel.RCI_RequirementCategoryID).ToList();
        }

        //UAT-2533
        List<RequirementPackageContract> ISharedRequirementPackageRepository.GetRequirementPackageDetail(String RequiremntRotPackageIDs)
        {
            List<RequirementPackageContract> requirementPackageDetailList = new List<RequirementPackageContract>();

            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            new SqlParameter("@RotationPackagesIDs", RequiremntRotPackageIDs),
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementPackageDetailsForBulk", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementPackageContract requirementPackageDetail = new RequirementPackageContract();
                            requirementPackageDetail.RequirementPackageID = dr["RequirementPackageID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageID"]);
                            requirementPackageDetail.RequirementPackageName = dr["RequirementPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageName"]);
                            requirementPackageDetail.RequirementPackageLabel = dr["RequirementPackageLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageLabel"]);
                            requirementPackageDetail.AgencyIDs = dr["AgencyIds"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyIds"]).ToString();
                            requirementPackageDetailList.Add(requirementPackageDetail);
                        }
                    }
                }
            }
            return requirementPackageDetailList;
        }
        Boolean ISharedRequirementPackageRepository.BulkPackageCopy(String BulkCopyPackgXML, Int32 CurrentLoggedInUserId)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_BulkPackageCopy", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BulkPackageCopyContractXML", BulkCopyPackgXML);
                command.Parameters.AddWithValue("@CurrentLoggedInUserId", CurrentLoggedInUserId);
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
                //return (Boolean)command.Parameters["@resultValue"].Value;
                return true;
            }
        }

        Boolean ISharedRequirementPackageRepository.ScheduleAutoArchivalRequirementPackage(Int32 currentLoggedInUserId)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("usp_AutoPackageArchival", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                cmd.ExecuteScalar();
                return true;
            }
        }
        #region Save Update Agency Hierarchy Package
        public void SaveUpdateAgencyHierarchyPackage(List<Int32> agencyHierarchyIds, RequirementPackage reqPackage, Int32 currentLoggedInUserId)
        {
            AgencyHierarchyPackage agencyHierarchyPkgToBeAdd = null;
            List<AgencyHierarchyPackage> agencyHierarchyPkgDetails = reqPackage.AgencyHierarchyPackages.Where(cond => !cond.AHP_IsDeleted).ToList();

            agencyHierarchyPkgDetails.Where(cond => !agencyHierarchyIds.Contains(cond.AHP_AgencyHierarchyID)).ForEach(x =>
            {
                x.AHP_IsDeleted = true;
                x.AHP_ModifiedBy = currentLoggedInUserId;
                x.AHP_ModifiedOn = DateTime.Now;
            });

            foreach (Int32 agHierarchyId in agencyHierarchyIds)
            {
                if (!agencyHierarchyPkgDetails.Any(cond => cond.AHP_AgencyHierarchyID == agHierarchyId))
                {
                    agencyHierarchyPkgToBeAdd = new AgencyHierarchyPackage();
                    agencyHierarchyPkgToBeAdd.AHP_AgencyHierarchyID = agHierarchyId;
                    agencyHierarchyPkgToBeAdd.AHP_IsDeleted = false;
                    agencyHierarchyPkgToBeAdd.AHP_CreatedBy = currentLoggedInUserId;
                    agencyHierarchyPkgToBeAdd.AHP_CreatedOn = DateTime.Now;
                    reqPackage.AgencyHierarchyPackages.Add(agencyHierarchyPkgToBeAdd);
                }
            }
        }
        #endregion

        List<Int32> ISharedRequirementPackageRepository.GetAgencyHierarchyIdsByRequirementPackageID(Int32 requirementPackageID)
        {
            return _sharedDataDBContext.AgencyHierarchyPackages.Where(con => con.AHP_RequirementPackageID == requirementPackageID && !con.AHP_IsDeleted).Select(sel => sel.AHP_AgencyHierarchyID).ToList();
        }

        List<Int32> ISharedRequirementPackageRepository.GetAgencyHierarchyIdsWithPkgId(Int32 reqPackageID)
        {
            return _sharedDataDBContext.AgencyHierarchyPackages.Where(cond => !cond.AHP_IsDeleted && cond.AHP_RequirementPackageID == reqPackageID).Select(sel => sel.AHP_AgencyHierarchyID).ToList();
        }

        #region UAT-2706
        Dictionary<Int32, String> ISharedRequirementPackageRepository.GetRequirementCategoryDataBypackageId(Int32 ReqPackageId)
        {
            Dictionary<Int32, String> result = new Dictionary<Int32, String>();
            List<RequirementCategory> lstReqCategoryData = _sharedDataDBContext.RequirementPackageCategories.Where(cond => cond.RPC_RequirementPackageID == ReqPackageId
                                                                        && !cond.RPC_IsDeleted).Select(sel => sel.RequirementCategory).ToList();

            lstReqCategoryData.Where(cond => !cond.RC_IsDeleted).ForEach(x =>
            {
                result.Add(x.RC_ID, x.RC_CategoryLabel.IsNullOrEmpty() ? x.RC_CategoryName : x.RC_CategoryLabel);
            });

            return result;
        }
        List<RequirementItem> ISharedRequirementPackageRepository.GetRequirementItemByCategoryId(Int32 ReqCategoryId)
        {
            return _sharedDataDBContext.RequirementCategoryItems.Where(cond => cond.RCI_RequirementCategoryID == ReqCategoryId && !cond.RCI_IsDeleted)
                                                .Select(sel => sel.RequirementItem).ToList();
        }

        List<RequirementPackageContract> ISharedRequirementPackageRepository.GetRequirementPackagesByHierarcyIds(List<Int32> lstAgencyHierarchyIds, Int32 currentLoggedInUserId, CustomPagingArgsContract customPagingContract)
        {
            List<RequirementPackageContract> lstrequirementPackages = new List<RequirementPackageContract>();
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyHierarchyIds", String.Join(",",lstAgencyHierarchyIds)),
                    new SqlParameter("@CurrentUserId", currentLoggedInUserId),
                    new SqlParameter("@filteringSortingData", customPagingContract.XML)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetRequirementPackagesByAgencyHierarchy", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementPackageContract requirementPackageContract = new RequirementPackageContract();
                            requirementPackageContract.RequirementPackageID = dr["RequirementPackageID"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["RequirementPackageID"]);
                            requirementPackageContract.RequirementPackageName = dr["RequirementPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageName"]);
                            requirementPackageContract.RequirementPackageLabel = dr["RequirementPackageLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageLabel"]);
                            requirementPackageContract.RequirementPackageDescription = dr["RequirementPackageDescription"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageDescription"]);
                            requirementPackageContract.RequirementPackageStatus = dr["RequirementPackageStatus"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageStatus"]);
                            requirementPackageContract.RequirementPackageType = dr["RequirementPackageType"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageType"]);
                            requirementPackageContract.TotalCount = dr["VirtualCount"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["VirtualCount"]);
                            requirementPackageContract.PageIndex = dr["CurrentPageIndex"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["CurrentPageIndex"]);
                            requirementPackageContract.RequirementPackageAgencyHierarchyNode = dr["RequirementPackageAgencyHierarchyNode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageAgencyHierarchyNode"]);
                            lstrequirementPackages.Add(requirementPackageContract);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }

            return lstrequirementPackages;
        }

        //UAT-2795
        String ISharedRequirementPackageRepository.GetCategoryDocumentLink(Int32 reqCategoryId)
        {
            return SharedDataDBContext.RequirementCategories.Where(cond => cond.RC_ID == reqCategoryId && !cond.RC_IsDeleted).Select(sel => sel.RC_SampleDocFormURL).FirstOrDefault();
        }
        #endregion


        #region UAT-2973

        /// <summary>
        /// UAT-2973
        /// To get Requirement Packages which are assigned to agencyID
        /// </summary>
        /// <param name="agencyId"></param>
        /// <param name="currentLoggedInUserId"></param>
        /// <returns></returns>
        List<RequirementPackageContract> ISharedRequirementPackageRepository.GetRequirementPackagesFromAgencyIds(Int32 selectedTenantId, String agencyId, String reqPkgTypeCode = null)
        {
            List<RequirementPackageContract> requirementPackageList = new List<RequirementPackageContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyID", agencyId),
                    new SqlParameter("@SelectedTenantID", selectedTenantId),
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
                            //requirementPackageContract.RequirementPackageLabel = dr["RequirementPackageLabel"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["RequirementPackageLabel"]);
                            requirementPackageContract.RequirementPackageCode = (Guid)(dr["RequirementPackageCode"]);
                            requirementPackageContract.IsUsed = dr["IsUsed"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsUsed"]);
                            requirementPackageContract.IsSharedUserPackage = true;

                            //UAT-2514 Retrieved IsNewPackage Column                           
                            requirementPackageContract.IsNewPackage = dr["IsNewPackage"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsNewPackage"]);
                            requirementPackageContract.EffectiveStartDate = dr["RequirementEffectiveStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RequirementEffectiveStartDate"]);
                            requirementPackageContract.EffectiveEndDate = dr["RequirementEffectiveEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RequirementEffectiveEndDate"]);
                            #region UAT-4657
                            requirementPackageContract.RootParentCode = dr["RootParentCode"] == DBNull.Value ? Guid.Empty : (Guid)(dr["RootParentCode"]);
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
        #endregion

        #region Requirement Verification Assignment Queue AND User Work Queue
        //GetReqPkgSubscriptionIdList
        List<RequirementVerificationQueueContract> ISharedRequirementPackageRepository.GetAssignmentRotationVerificationQueueData(RequirementVerificationQueueContract requirementVerificationQueueContract, CustomPagingArgsContract customPagingArgsContract)
        {
            List<RequirementVerificationQueueContract> lstRequirementVerificationQueueContract = new List<RequirementVerificationQueueContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAssignmentRotationVerificationQueueSearch", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@xmlData", requirementVerificationQueueContract.XML);
                command.Parameters.AddWithValue("@xmlSortingAndFilteringData", customPagingArgsContract.XML);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.ONE && ds.Tables[0].Rows.Count > 0)
                {
                    Int32 CurrentPageIndex = ds.Tables[1].Rows[0][0] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    Int32 TotalCount = ds.Tables[1].Rows[0][1] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[1].Rows[0][1]);
                    lstRequirementVerificationQueueContract = ds.Tables[0].AsEnumerable().Select(x =>
                      new RequirementVerificationQueueContract
                      {
                          FlatVerificationDataID = x["FlatVerificationDataID"] == DBNull.Value ? 0 : Convert.ToInt32(x["FlatVerificationDataID"]),
                          TenantID = x["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(x["TenantID"]),
                          OrganizationUserID = x["OrganizationUserID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["OrganizationUserID"]),
                          ApplicantFirstName = x["ApplicantFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(x["ApplicantFirstName"]),
                          ApplicantLastName = x["ApplicantLastName"] == DBNull.Value ? String.Empty : Convert.ToString(x["ApplicantLastName"]),
                          ClinicalRotationID = x["ClinicalRotationID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ClinicalRotationID"]),
                          RequirementPackageName = x["RequirementPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(x["RequirementPackageName"]),
                          RequirementCategoryName = x["RequirementCategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(x["RequirementCategoryName"]),
                          RequirementCategoryID = x["RequirementCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(x["RequirementCategoryID"]),
                          RequirementItemName = x["RequirementItemName"] == DBNull.Value ? String.Empty : Convert.ToString(x["RequirementItemName"]),
                          RotationStartDate = x["RotationStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(x["RotationStartDate"]),
                          RotationEndDate = x["RotationEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(x["RotationEndDate"]),
                          SubmissionDate = x["SubmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(x["SubmissionDate"]),
                          RequirementPackageTypeID = x["RequirementPackageTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(x["RequirementPackageTypeID"]),
                          AgencyName = x["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(x["AgencyName"]),
                          RequirementPackageSubscriptionID = x["RequirementPackageSubscriptionID"] == DBNull.Value ? 0 : Convert.ToInt32(x["RequirementPackageSubscriptionID"]),
                          RequirementItemId = x["RequirementItemId"] == DBNull.Value ? 0 : Convert.ToInt32(x["RequirementItemId"]),
                          ApplicantRequirementItemId = x["ApplicantRequirementItemId"] == DBNull.Value ? 0 : Convert.ToInt32(x["ApplicantRequirementItemId"]),
                          TotalCount = TotalCount,
                          CurrentPageIndex = CurrentPageIndex,
                          AssignedUserName = x["AssignedUserName"] == DBNull.Value ? String.Empty : Convert.ToString(x["AssignedUserName"]),
                          ComplioID = x["ComplioID"] == DBNull.Value ? String.Empty : Convert.ToString(x["ComplioID"]),
                          IsCurrentRotation = x["IsCurrentRotation"] == DBNull.Value ? false : Convert.ToBoolean(x["IsCurrentRotation"]),
                          ReqReviewByDesc = x["ReqReviewByDesc"] == DBNull.Value ? String.Empty : Convert.ToString(x["ReqReviewByDesc"]),
                      }).ToList();
                }
            }
            return lstRequirementVerificationQueueContract;
        }


        List<ReqPkgSubscriptionIDList> ISharedRequirementPackageRepository.GetReqPkgSubscriptionIdList(RequirementVerificationQueueContract requirementVerificationQueueContract, Int32 CurrentReqPkgSubscriptionID, Int32 ApplicantRequirementItemID)
        {
            List<ReqPkgSubscriptionIDList> lstRequirementVerificationQueueContract = new List<ReqPkgSubscriptionIDList>();
            String sortExpression = requirementVerificationQueueContract.GridCustomPagingArguments.SortExpression;
            Boolean sortDirectionDescending = requirementVerificationQueueContract.GridCustomPagingArguments.SortDirectionDescending;
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetReqPkgSubscriptionIdList", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@xmlData", requirementVerificationQueueContract.XML);
                command.Parameters.AddWithValue("@CurrentReqPkgSubscriptionID", CurrentReqPkgSubscriptionID);
                command.Parameters.AddWithValue("@ApplicantRequirementItemID", ApplicantRequirementItemID);
                command.Parameters.AddWithValue("@SortDirectionDescending", sortDirectionDescending);
                command.Parameters.AddWithValue("@SortExpression", sortExpression);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > 0)
                {
                    lstRequirementVerificationQueueContract = ds.Tables[0].AsEnumerable().Select(x =>
                      new ReqPkgSubscriptionIDList
                      {
                          RequirementPackageSubscriptionID = x["RequirementPackageSubscriptionID"] == DBNull.Value ? 0 : Convert.ToInt32(x["RequirementPackageSubscriptionID"]),
                          ApplicantRequirementItemId = x["ApplicantRequirementItemId"] == DBNull.Value ? 0 : Convert.ToInt32(x["ApplicantRequirementItemId"]),
                          RequirementItemId = x["RequirementItemId"] == DBNull.Value ? 0 : Convert.ToInt32(x["RequirementItemId"]),
                          ApplicantRequirementCategoryId = x["ApplicantRequirementCategoryId"] == DBNull.Value ? 0 : Convert.ToInt32(x["ApplicantRequirementCategoryId"]),
                          RotationId = x["ClinicalRotationID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ClinicalRotationID"]),
                          ApplicantId = x["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(x["OrganizationUserID"]),
                          RequirementCategoryID = x["RequirementCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(x["RequirementCategoryID"]),
                          TenantID = x["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(x["TenantID"]),
                          AgencyId = x["AgencyId"] == DBNull.Value ? 0 : Convert.ToInt32(x["AgencyId"]),
                          NextSubscriptionID = x["NextSubscriptionID"] == DBNull.Value ? 0 : Convert.ToInt32(x["NextSubscriptionID"]),
                          NextApplicantRequirementItemId = x["NextApplicantRequirementItemId"] == DBNull.Value ? 0 : Convert.ToInt32(x["NextApplicantRequirementItemId"]),
                          NextApplicantRequirementCategoryId = x["NextApplicantRequirementCategoryId"] == DBNull.Value ? 0 : Convert.ToInt32(x["NextApplicantRequirementCategoryId"]),
                          NextRequirementItemId = x["NextRequirementItemId"] == DBNull.Value ? 0 : Convert.ToInt32(x["NextRequirementItemId"]),

                          NextClinicalRotationID = x["NextClinicalRotationID"] == DBNull.Value ? 0 : Convert.ToInt32(x["NextClinicalRotationID"]),
                          NextOrganizationUserID = x["NextOrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(x["NextOrganizationUserID"]),
                          NextRequirementCategoryID = x["NextRequirementCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(x["NextRequirementCategoryID"]),
                          NextTenantID = x["NextTenantID"] == DBNull.Value ? 0 : Convert.ToInt32(x["NextTenantID"]),
                          NextAgencyId = x["NextAgencyId"] == DBNull.Value ? 0 : Convert.ToInt32(x["NextAgencyId"]),
                          PrevSubscriptionID = x["PrevSubscriptionID"] == DBNull.Value ? 0 : Convert.ToInt32(x["PrevSubscriptionID"]),
                          PrevApplicantRequirementItemId = x["PrevApplicantRequirementItemId"] == DBNull.Value ? 0 : Convert.ToInt32(x["PrevApplicantRequirementItemId"]),
                          PrevApplicantRequirementCategoryId = x["PrevApplicantRequirementCategoryId"] == DBNull.Value ? 0 : Convert.ToInt32(x["PrevApplicantRequirementCategoryId"]),
                          PrevRequirementItemId = x["PrevRequirementItemId"] == DBNull.Value ? 0 : Convert.ToInt32(x["PrevRequirementItemId"]),
                          PrevClinicalRotationID = x["PrevClinicalRotationID"] == DBNull.Value ? 0 : Convert.ToInt32(x["PrevClinicalRotationID"]),
                          PrevOrganizationUserID = x["PrevOrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(x["PrevOrganizationUserID"]),
                          PrevRequirementCategoryID = x["PrevRequirementCategoryID"] == DBNull.Value ? 0 : Convert.ToInt32(x["PrevRequirementCategoryID"]),
                          PrevTenantID = x["PrevTenantID"] == DBNull.Value ? 0 : Convert.ToInt32(x["PrevTenantID"]),
                          PrevAgencyId = x["PrevAgencyId"] == DBNull.Value ? 0 : Convert.ToInt32(x["PrevAgencyId"]),

                      }).ToList();
                }
            }
            return lstRequirementVerificationQueueContract;
        }

        Boolean ISharedRequirementPackageRepository.AssignItemsToUser(List<Int32> lstSelectedVerificationItems, Int32 VerSelectedUserId, String verSelectedUserName)
        {
            List<FlatRequirementVerificationDetailData> data = _sharedDataDBContext.FlatRequirementVerificationDetailDatas.Where(con => lstSelectedVerificationItems.Contains(con.FRVDD_ID)).ToList();
            foreach (var item in data)
            {
                var GetRequirementVerificationData = _sharedDataDBContext.FlatRequirementVerificationDetailDatas.Where(con => con.FRVDD_RequirementItemId == item.FRVDD_RequirementItemId && con.FRVDD_PackageID == item.FRVDD_PackageID &&
                      con.FRVDD_ApplicantId == item.FRVDD_ApplicantId && con.FRVDD_PackageSubscriptionID == item.FRVDD_PackageSubscriptionID).ToList();
                foreach (FlatRequirementVerificationDetailData Childitem in GetRequirementVerificationData)
                {
                    Childitem.FRVDD_AssignedToUserID = VerSelectedUserId;
                    Childitem.FRVDD_AssignedUserName = verSelectedUserName;
                    Childitem.FRVDD_ModifiedOn = DateTime.Now;
                    Childitem.FRVDD_ModifiedBy = AppConsts.ONE;
                }
            }
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            else
                return false;
        }
        #endregion

        #region UAT-3052
        /// <summary>
        /// Used to get Agency Hierarchy only for reports
        /// </summary>
        /// <param name="loggedInEmailId"></param>
        /// <returns></returns>
        List<Agency> ISharedRequirementPackageRepository.GetAgencyHierarchy(String loggedInEmailId)
        {
            List<Agency> lstAgency = new List<Agency>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("[Report].[usp_Report_Filter_GetAgencyHierarchyForAgencyUser]", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LoggedInUserEmailId", loggedInEmailId);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lstAgency = ds.Tables[0].AsEnumerable().Select(x =>
                      new Agency
                      {
                          AG_ID = x["HIERARCHYID"] == DBNull.Value ? 0 : Convert.ToInt32(x["HIERARCHYID"]),
                          AG_Name = x["HierarchyName"] == DBNull.Value ? String.Empty : Convert.ToString(x["HierarchyName"]),
                      }).ToList();
                }
            }
            return lstAgency;
        }

        List<Agency> ISharedRequirementPackageRepository.GetAgencyUsers(String loggedInEmailId)
        {
            List<Agency> lstAgency = new List<Agency>();
            Int32 agencyUserId = _sharedDataDBContext.AgencyUsers.Where(cond => cond.AGU_Email.Equals(loggedInEmailId) && !cond.AGU_IsDeleted).Select(f => f.AGU_ID).FirstOrDefault();
            if (!agencyUserId.IsNullOrEmpty())
            {
                var agencyIds = _sharedDataDBContext.UserAgencyMappings.Where(cond => cond.UAM_AgencyUserID == agencyUserId && !cond.UAM_IsDeleted).Select(sel => sel.UAM_AgencyID).ToList();
                lstAgency = _sharedDataDBContext.Agencies.Where(con => agencyIds.Contains(con.AG_ID) && !con.AG_IsDeleted).ToList();
                return lstAgency;
            }
            return new List<Agency>();
        }
        #endregion

        #region UAT-3078
        Boolean ISharedRequirementPackageRepository.updateRequirementItemDisplayOrder(Int32 RequirementItemId, Int32 RequirementCategoryId, Int32 DisplayOrder, Int32 CurrentLoggedInUserID, Boolean isNewPackage = false)
        {
            String ActionTypeCode = String.Empty;
            RequirementCategoryItem requirementCatItem = _sharedDataDBContext.RequirementCategoryItems.Where(cond => cond.RCI_RequirementCategoryID == RequirementCategoryId
                                    && cond.RCI_RequirementItemID == RequirementItemId && !cond.RCI_IsDeleted).FirstOrDefault();

            if (!requirementCatItem.IsNullOrEmpty())
            {
                requirementCatItem.RCI_DisplayOrder = DisplayOrder;
                requirementCatItem.RCI_ModifiedByID = CurrentLoggedInUserID;
                requirementCatItem.RCI_ModifiedOn = DateTime.Now;
                ActionTypeCode = RequirementPackageObjectActionTypeEnum.EDITED.GetStringValue();
            }
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                Int32 objectId = 0;
                #region Rot Pkg Object Sync Data
                if (isNewPackage)
                {
                    List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
                    RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                    objectData.ActionTypeCode = ActionTypeCode;
                    objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_ITEM.GetStringValue();
                    objectData.NewObjectId = RequirementItemId;
                    lstPackageObjectSynchingData.Add(objectData);
                    String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                    SaveRequirementPackageObjectForSync(requestXML, CurrentLoggedInUserID, out objectId);
                }
                #endregion
                return true;
            }
            return false;
        }
        Boolean ISharedRequirementPackageRepository.updateRequirementFieldDisplayOrder(Int32 RequirementFieldId, Int32 RequirementItemId, Int32 DisplayOrder, Int32 CurrentLoggedInUserID, Boolean isNewPackage = false)
        {
            String ActionTypeCode = String.Empty;
            RequirementItemField requirementItemFld = _sharedDataDBContext.RequirementItemFields.Where(cond => cond.RIF_RequirementFieldID == RequirementFieldId
                                    && cond.RIF_RequirementItemID == RequirementItemId && !cond.RIF_IsDeleted).FirstOrDefault();

            if (!requirementItemFld.IsNullOrEmpty())
            {
                requirementItemFld.RIF_DisplayOrder = DisplayOrder;
                requirementItemFld.RIF_ModifiedByID = CurrentLoggedInUserID;
                requirementItemFld.RIF_ModifiedOn = DateTime.Now;
                ActionTypeCode = RequirementPackageObjectActionTypeEnum.EDITED.GetStringValue();
            }
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                Int32 objectId = 0;
                #region Rot Pkg Object Sync Data
                if (isNewPackage)
                {
                    List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
                    RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                    objectData.ActionTypeCode = ActionTypeCode;
                    objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_FIELD.GetStringValue();
                    objectData.NewObjectId = RequirementFieldId;
                    lstPackageObjectSynchingData.Add(objectData);
                    String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                    SaveRequirementPackageObjectForSync(requestXML, CurrentLoggedInUserID, out objectId);
                }
                #endregion
                return true;
            }
            return false;
        }
        #endregion


        RequirementPackage ISharedRequirementPackageRepository.GetRequirementPackageByCode(Guid rpCode)
        {
            return _sharedDataDBContext.RequirementPackages.FirstOrDefault(cond => cond.RP_Code == rpCode && !cond.RP_IsDeleted);
        }

        /// <summary>
        /// UAT -3146
        /// </summary>
        /// <param name="SelectedTenantIDs"></param>
        /// <param name="loggedInUserEmailId"></param>
        /// <returns></returns>
        Dictionary<String, String> ISharedRequirementPackageRepository.GetRotationListFilterForLoggedInAgencyUserReports(String SelectedTenantIDs, String loggedInUserEmailId)
        {
            Dictionary<String, String> dicRotationList = new Dictionary<String, String>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("Report.usp_Report_Filter_GetClinicalRotationList", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LoggedInUserEmailId", loggedInUserEmailId);
                command.Parameters.AddWithValue("@TenantIDs", SelectedTenantIDs);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    dicRotationList = ds.Tables[0].AsEnumerable()
                                   .ToDictionary<DataRow, String, String>(row => row.Field<String>("RotationID"),
                                       row => row.Field<String>("RotationName"));
                }
            }
            return dicRotationList;
        }


        #region UAT-3112
        List<Int32> ISharedRequirementPackageRepository.GetSystemDocumentsMapped(Int32 ItemID)
        {
            List<Int32> lstSystemDocIds = new List<Int32>();
            lstSystemDocIds = _sharedDataDBContext.RequirementItemSystemDocuments.Where(cond => !cond.RISD_IsDeleted && cond.RISD_RequirementItemID == ItemID).Select(sel => sel.RISD_SystemDocumentID).ToList();
            return lstSystemDocIds;
        }

        List<BadgeFormSystemDocField> ISharedRequirementPackageRepository.GetSystemDocFieldsMapped(Int32 SystemDocId)
        {
            return _sharedDataDBContext.BadgeFormSystemDocFields.Where(cond => !cond.BFSDF_IsDeleted && cond.BFSDF_SystemDocID == SystemDocId).ToList();
        }
        #endregion

        #region UAT-3176
        Boolean ISharedRequirementPackageRepository.SaveUpdateRotationAttributeGroup(RequirementAttributeGroupContract rotationAttributeGroupContract, Boolean IsAttributeGroupExists, Int32 currentLoggedInUserID)
        {
            Entity.SharedDataEntity.RequirementAttributeGroup objReqAttributeGroup = new Entity.SharedDataEntity.RequirementAttributeGroup();
            objReqAttributeGroup = _sharedDataDBContext.RequirementAttributeGroups.FirstOrDefault(x => x.RAG_ID == rotationAttributeGroupContract.RequirementAttributeGroupID && !x.RAG_IsDeleted);
            if (!rotationAttributeGroupContract.IsNullOrEmpty())
            {
                if (!IsAttributeGroupExists && objReqAttributeGroup.IsNullOrEmpty())
                {
                    Entity.SharedDataEntity.RequirementAttributeGroup objRequirementAttributeGroup = new Entity.SharedDataEntity.RequirementAttributeGroup();
                    objRequirementAttributeGroup.RAG_Code = rotationAttributeGroupContract.Code;
                    objRequirementAttributeGroup.RAG_Name = rotationAttributeGroupContract.Name;
                    objRequirementAttributeGroup.RAG_Label = rotationAttributeGroupContract.Label;
                    objRequirementAttributeGroup.RAG_CopiedFromCode = rotationAttributeGroupContract.CopiedFromCode;
                    objRequirementAttributeGroup.RAG_CreatedByID = currentLoggedInUserID;
                    objRequirementAttributeGroup.RAG_CreatedOn = DateTime.Now; ;
                    objRequirementAttributeGroup.RAG_IsCreatedByAdmin = rotationAttributeGroupContract.IsCreatedByAdmin;
                    objRequirementAttributeGroup.RAG_IsDeleted = rotationAttributeGroupContract.IsDeleted;

                    _sharedDataDBContext.RequirementAttributeGroups.AddObject(objRequirementAttributeGroup);
                }
                else
                {

                    objReqAttributeGroup.RAG_Code = rotationAttributeGroupContract.Code;
                    objReqAttributeGroup.RAG_Name = rotationAttributeGroupContract.Name;
                    objReqAttributeGroup.RAG_Label = rotationAttributeGroupContract.Label;
                    objReqAttributeGroup.RAG_CopiedFromCode = rotationAttributeGroupContract.CopiedFromCode;
                    objReqAttributeGroup.RAG_ModifiedByID = currentLoggedInUserID;
                    objReqAttributeGroup.RAG_ModifiedOn = DateTime.Now;
                    objReqAttributeGroup.RAG_IsCreatedByAdmin = rotationAttributeGroupContract.IsCreatedByAdmin;
                    objReqAttributeGroup.RAG_IsDeleted = rotationAttributeGroupContract.IsDeleted;
                }
            }

            if (_sharedDataDBContext.SaveChanges() > 0)
            {
                if (IsAttributeGroupExists)
                {
                    Int32 objectId = 0;
                    #region Rot Pkg Object Sync Data
                    List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();
                    RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
                    objectData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.EDITED.GetStringValue(); ;
                    objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_ATTRIBUTE_GROUP.GetStringValue();
                    objectData.NewObjectId = objReqAttributeGroup.RAG_ID;
                    lstPackageObjectSynchingData.Add(objectData);
                    String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                    SaveRequirementPackageObjectForSync(requestXML, currentLoggedInUserID, out objectId);
                    #endregion
                }
                return true;
            }
            return false;
        }

        List<RequirementAttributeGroupContract> ISharedRequirementPackageRepository.GetAllRotationAttributeGroup(String attributeName, String attributeLabel)
        {

            List<RequirementAttributeGroupContract> requirementAttributeGroupList = new List<RequirementAttributeGroupContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAllRotationAttributeGroup", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@attributeName", attributeName);
                command.Parameters.AddWithValue("@attributeLabel", attributeLabel);

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@attributeName", attributeName),
                             new SqlParameter("@attributeLabel",attributeLabel)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAllRotationAttributeGroup", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementAttributeGroupContract requirementAttributeGroup = new RequirementAttributeGroupContract();
                            requirementAttributeGroup.RequirementAttributeGroupID = dr["RequirementAttributeGroupID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RequirementAttributeGroupID"]);
                            requirementAttributeGroup.Code = dr["Code"].GetType().Name == "DBNull" ? new Guid() : (Guid)(dr["Code"]);
                            requirementAttributeGroup.Name = dr["Name"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["Name"]);
                            requirementAttributeGroup.Label = dr["Label"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["Label"]);
                            requirementAttributeGroup.IsCreatedByAdmin = dr["IsCreatedByAdmin"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsCreatedByAdmin"]);
                            requirementAttributeGroup.CopiedFromCode = dr["CopiedFromCode"].GetType().Name == "DBNull" ? new Guid() : (Guid)(dr["CopiedFromCode"]);
                            requirementAttributeGroup.IsDeleted = dr["IsDeleted"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["IsDeleted"]);
                            requirementAttributeGroupList.Add(requirementAttributeGroup);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);

            }
            return requirementAttributeGroupList;

        }


        /// <summary>
        /// Method to get Attribute Group based on attributeGroupId
        /// </summary>
        /// <param name="attributeGroupId"></param>
        /// <returns>ComplianceAttributeGroup</returns>
        RequirementAttributeGroupContract ISharedRequirementPackageRepository.GetAttributeGroupById(Int32 rotationAttributeGroupId)
        {
            return _sharedDataDBContext.RequirementAttributeGroups.Where(cond => cond.RAG_ID == rotationAttributeGroupId && !cond.RAG_IsDeleted)
                .Select(s => new RequirementAttributeGroupContract
                {
                    RequirementAttributeGroupID = s.RAG_ID,
                    Code = s.RAG_Code,
                    Name = s.RAG_Name,
                    Label = s.RAG_Label,
                    CopiedFromCode = s.RAG_CopiedFromCode,
                    IsCreatedByAdmin = s.RAG_IsCreatedByAdmin,
                    IsDeleted = s.RAG_IsDeleted,
                    CreatedByID = s.RAG_CreatedByID,
                    CreatedOn = s.RAG_CreatedOn,
                    ModifiedByID = s.RAG_ModifiedByID,
                    ModifiedOn = s.RAG_ModifiedOn
                }).FirstOrDefault();
        }


        Boolean ISharedRequirementPackageRepository.IsAttributeGroupMapped(Int32 requirementAttributeGroupId)
        {
            List<RequirementField> attributeMapping = _sharedDataDBContext.RequirementFields.Where(cond => cond.RF_RequirementAttributeGroupID == requirementAttributeGroupId && !cond.RF_IsDeleted).ToList();
            if (attributeMapping.Count > 0)
                return true;
            return false;
        }

        List<RequirementAttributeGroups> ISharedRequirementPackageRepository.GetRequirementAttributeGroups()
        {
            return _sharedDataDBContext.RequirementAttributeGroups.Where(cond => !cond.RAG_IsDeleted)
                .Select(s => new RequirementAttributeGroups
                {
                    RequirementAttributeGroupID = s.RAG_ID,
                    Code = (Guid)s.RAG_Code,
                    Name = s.RAG_Name,
                    Label = s.RAG_Label
                })
                .ToList();
        }
        #endregion

        #region UAT-3230
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="chunkSize"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        String ISharedRequirementPackageRepository.GetPendingRequirementPackageObjectIdsForTenant(Int32 tenantId, Int32 chunkSize, Int32 retryCount)
        {
            List<Int32> requirementPkgSyncObjectIds = _sharedDataDBContext.RequirementPkgSyncObjectDetails.Where(cond => cond.RPSD_TenantID == tenantId && !cond.RPSD_IsDeleted
                                               && cond.RPSD_IsActive && cond.RPSD_RetryCount < retryCount && cond.SyncRequirementPackageObject != null
                                               && cond.SyncRequirementPackageObject.SRPO_IsActive && !cond.SyncRequirementPackageObject.SRPO_IsDeleted)
                                               .Select(sel => sel.RPSD_SyncRequirementPkgObjectID).Take(chunkSize).ToList();
            if (!requirementPkgSyncObjectIds.IsNullOrEmpty())
            {
                return String.Join(",", requirementPkgSyncObjectIds);
            }
            return String.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUserId"></param>
        public void UpdateSyncRequirementPackageObjectsCount(Int32 currentUserId, String SyncReqPkgObjectIds)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_UpdateSyncRequirementPackageObjectsCount", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserId", currentUserId);
                command.Parameters.AddWithValue("@SyncReqPkgObjectIds", SyncReqPkgObjectIds);
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstSyncReqPkgObjectIds"></param>
        /// <param name="currentUserId"></param>
        private void SaveUpdateRequirementPkgSyncDetails(Dictionary<String, object> dicData)
        {
            ISysXLoggerService LoggerService = (dicData["LoggerService"] as ISysXLoggerService);
            Int32 currentUserId = Convert.ToInt32(dicData["currentLoggedInUserId"]);
            String syncReqPkgObjectIds = Convert.ToString(dicData["syncReqPkgObjectIds"]);
            try
            {
                EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    SqlCommand command = new SqlCommand("usp_InsertRequirementPkgSyncObjectDetails", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SyncReqPkgObjectIds", syncReqPkgObjectIds);
                    command.Parameters.AddWithValue("@UserId", currentUserId);
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
                UpdateSyncRequirementPackageObjectsCount(currentUserId, syncReqPkgObjectIds);
                RemoveSyncRequirementPackageObjects(currentUserId, syncReqPkgObjectIds);
            }
            catch (Exception ex)
            {
                LoggerService.GetLogger().Error("SyncReqPkgObjectIds : " + syncReqPkgObjectIds, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUserId"></param>
        public void RemoveSyncRequirementPackageObjects(Int32 currentUserId, String SyncReqPkgObjectIds)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_RemoveSyncRequirementPackageObjects", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SyncReqPkgObjectIds", SyncReqPkgObjectIds);
                command.Parameters.AddWithValue("@UserId", currentUserId);
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }
        #endregion

        #region UAT-3220
        Boolean ISharedRequirementPackageRepository.HideRequirementSharesDetailLink(Guid userID)
        {
            Boolean hideRequirementSharesDetailLink = false;
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                              new SqlParameter("@UserID", userID)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyPortalDetailLinkHidePermissionDetailByUserId", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            hideRequirementSharesDetailLink = dr["AUP_PermissionAccessTypeID"] == DBNull.Value ? false : Convert.ToInt32(dr["AUP_PermissionAccessTypeID"]) == AppConsts.ONE ? true : false;
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return hideRequirementSharesDetailLink;
        }
        #endregion

        Boolean ISharedRequirementPackageRepository.CloneRequirementItem(Int32 sourceReqItemID, Int32 currentLoggedInUserId, Int32 reqCatID)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_CloneRequirementItem", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SourceItemID", sourceReqItemID);
                command.Parameters.AddWithValue("@CurrentOrgUserID", currentLoggedInUserId);
                command.Parameters.AddWithValue("@ReqCatID", reqCatID);

                con.Open();
                command.ExecuteNonQuery();
                con.Close();
                return true;
            }
        }

        //UAT-3296
        String ISharedRequirementPackageRepository.GetCategoryExplanatoryNotes(Int32 reqCategoryId)
        {
            return SharedDataDBContext.LargeContents.Where(cond => cond.LC_ObjectID == reqCategoryId && !cond.LC_IsDeleted).Select(sel => sel.LC_Content).FirstOrDefault();
        }

        #region UAT-3295
        ProfileSharingInvitationDetailsContract ISharedRequirementPackageRepository.GetProfileShareDetailsById(Int32 invitationId)
        {

            ProfileSharingInvitationDetailsContract agencyProfileShareDetailContract = new ProfileSharingInvitationDetailsContract();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@InvitationId", invitationId)
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetProfileShareDetails", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            agencyProfileShareDetailContract.InvitationID = dr["InvitationID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["InvitationID"]);
                            agencyProfileShareDetailContract.ProfileSharingInvitationGroupID = dr["InvitationGroupId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["InvitationGroupId"]);
                            agencyProfileShareDetailContract.Department = Convert.ToString(dr["Department"]);
                            agencyProfileShareDetailContract.Program = Convert.ToString(dr["Program"]);
                            agencyProfileShareDetailContract.Course = Convert.ToString(dr["Course"]);
                            agencyProfileShareDetailContract.UnitFloorLoc = Convert.ToString(dr["UnitFloorLoc"]);
                            agencyProfileShareDetailContract.Shift = Convert.ToString(dr["RotationShift"]);
                            agencyProfileShareDetailContract.RotationName = Convert.ToString(dr["RotationName"]);
                            agencyProfileShareDetailContract.StartDate = dr["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["StartDate"]);
                            agencyProfileShareDetailContract.EndDate = dr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["EndDate"]);
                            agencyProfileShareDetailContract.StartTime = dr["StartTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(dr["StartTime"]);
                            agencyProfileShareDetailContract.EndTime = dr["EndTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)(dr["EndTime"]);
                            agencyProfileShareDetailContract.Time = dr["Times"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Times"]);
                            agencyProfileShareDetailContract.DaysName = dr["Days"].GetType().Name == "DBNull" ? null : Convert.ToString(dr["Days"]);
                            agencyProfileShareDetailContract.Term = Convert.ToString(dr["Term"]);
                            agencyProfileShareDetailContract.TypeSpecialty = Convert.ToString(dr["TypeSpecialty"]);
                            agencyProfileShareDetailContract.SchoolContactName = Convert.ToString(dr["SchoolContactName"]);
                            agencyProfileShareDetailContract.SchoolContactEmailId = Convert.ToString(dr["SchoolContactEmailId"]);

                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);

            }
            return agencyProfileShareDetailContract;

        }

        Boolean ISharedRequirementPackageRepository.UpdateProfileShareInvDetails(ProfileSharingInvitationDetailsContract profileShareDetails, Int32 currentLoggedInUserId)
        {
            ProfileSharingInvitationRotationDetail profileShareInvDetail = new ProfileSharingInvitationRotationDetail();

            ProfileSharingInvitation objProfileSharingInv = _sharedDataDBContext.ProfileSharingInvitations.Where(x => x.PSI_ID == profileShareDetails.InvitationID && !x.PSI_IsDeleted).FirstOrDefault();
            if (!objProfileSharingInv.IsNullOrEmpty())
                profileShareInvDetail = _sharedDataDBContext.ProfileSharingInvitationRotationDetails.Where(x => x.PSIRD_ProfileSharingInvitationGroupID == objProfileSharingInv.PSI_ProfileSharingInvitationGroupID && !x.PSIRD_IsDeleted).FirstOrDefault();

            if (profileShareInvDetail.IsNullOrEmpty())
            {
                Entity.SharedDataEntity.ProfileSharingInvitationRotationDetail objProfileShareInvDetails = new Entity.SharedDataEntity.ProfileSharingInvitationRotationDetail();
                objProfileShareInvDetails.PSIRD_ProfileSharingInvitationGroupID = objProfileSharingInv.PSI_ProfileSharingInvitationGroupID;
                objProfileShareInvDetails.PSIRD_StartDate = profileShareDetails.StartDate; // UAT-4324
                objProfileShareInvDetails.PSIRD_EndDate = profileShareDetails.EndDate; //UAT-4324
                objProfileShareInvDetails.PSIRD_RotationName = profileShareDetails.RotationName;
                objProfileShareInvDetails.PSIRD_Department = profileShareDetails.Department;
                objProfileShareInvDetails.PSIRD_Course = profileShareDetails.Course;
                objProfileShareInvDetails.PSIRD_UnitFloor = profileShareDetails.UnitFloorLoc;
                objProfileShareInvDetails.PSIRD_Term = profileShareDetails.Term;
                objProfileShareInvDetails.PSIRD_TypeSpecialty = profileShareDetails.TypeSpecialty;
                objProfileShareInvDetails.PSIRD_Shift = profileShareDetails.Shift;
                objProfileShareInvDetails.PSIRD_CreatedOn = DateTime.Now;
                objProfileShareInvDetails.PSIRD_CreatedByID = currentLoggedInUserId;
                objProfileShareInvDetails.PSIRD_SchoolContactEmailId = profileShareDetails.SchoolContactEmailId;
                objProfileShareInvDetails.PSIRD_SchoolContactName = profileShareDetails.SchoolContactName;
                objProfileShareInvDetails.PSIRD_Program = profileShareDetails.Program;
                _sharedDataDBContext.ProfileSharingInvitationRotationDetails.AddObject(objProfileShareInvDetails);
            }
            else
            {
                profileShareInvDetail.PSIRD_ProfileSharingInvitationGroupID = objProfileSharingInv.PSI_ProfileSharingInvitationGroupID;
                profileShareInvDetail.PSIRD_StartDate = profileShareDetails.StartDate; // UAT-4324
                profileShareInvDetail.PSIRD_EndDate = profileShareDetails.EndDate; //UAT-4324
                profileShareInvDetail.PSIRD_RotationName = profileShareDetails.RotationName;
                profileShareInvDetail.PSIRD_Department = profileShareDetails.Department;
                profileShareInvDetail.PSIRD_Course = profileShareDetails.Course;
                profileShareInvDetail.PSIRD_UnitFloor = profileShareDetails.UnitFloorLoc;
                profileShareInvDetail.PSIRD_Term = profileShareDetails.Term;
                profileShareInvDetail.PSIRD_TypeSpecialty = profileShareDetails.TypeSpecialty;
                profileShareInvDetail.PSIRD_Shift = profileShareDetails.Shift;
                profileShareInvDetail.PSIRD_ModifiedOn = DateTime.Now;
                profileShareInvDetail.PSIRD_ModifiedByID = currentLoggedInUserId;
                profileShareInvDetail.PSIRD_Program = profileShareDetails.Program;
                profileShareInvDetail.PSIRD_SchoolContactEmailId = profileShareDetails.SchoolContactEmailId;
                profileShareInvDetail.PSIRD_SchoolContactName = profileShareDetails.SchoolContactName;
            }

            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region UAT-3494
        Boolean ISharedRequirementPackageRepository.InsertRequirementPackageVersioningData(Int32 currentOrgUserID, Int32 OldPackageID, Int32 NewPackageID, String lstSelectedAgencyIds)
        {
            List<RequirementPackageObjectSynchingContract> lstPackageObjectSynchingData = new List<RequirementPackageObjectSynchingContract>();

            #region Rot Pkg Object Sync Data
            RequirementPackageObjectSynchingContract objectData = new RequirementPackageObjectSynchingContract();
            objectData.ActionTypeCode = RequirementPackageObjectActionTypeEnum.REPLACED.GetStringValue();
            objectData.ObjectTypeCode = RequirementPackageObjectTypeEnum.REQUIREMENT_PACKAGE.GetStringValue();
            objectData.NewObjectId = NewPackageID;
            objectData.OldObjectId = OldPackageID;

            lstPackageObjectSynchingData.Add(objectData);
            #endregion
            Int32 objectId = 0;
            if (!lstPackageObjectSynchingData.IsNullOrEmpty())
            {
                String requestXML = ConvertPackageObjectTypeInXML(lstPackageObjectSynchingData);
                SaveRequirementPackageObjectForSync(requestXML, currentOrgUserID, out objectId);
            }

            #region RotationPackageCopyFromAgencyHierarchy
            List<Int32> lstagencyIds = lstSelectedAgencyIds.Split(',').Select(Int32.Parse).ToList();
            foreach (var agencyId in lstagencyIds)
            {
                RequirementPackageVersioning objReqPkgVer = new RequirementPackageVersioning();
                objReqPkgVer.RPV_PrevPkgId = OldPackageID;
                objReqPkgVer.RPV_NewPkgId = NewPackageID;
                objReqPkgVer.RPV_IsDeleted = false;
                objReqPkgVer.RPV_AgencyId = (-1) * (agencyId);
                objReqPkgVer.RPV_CreatedBy = currentOrgUserID;
                objReqPkgVer.RPV_CreatedOn = DateTime.Now;
                objReqPkgVer.RPV_SyncRequirementPackageObjectsId = objectId;
                _sharedDataDBContext.RequirementPackageVersionings.AddObject(objReqPkgVer);
            }
            #endregion
            if (_sharedDataDBContext.SaveChanges() > 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region UAT-4254 || Release - 181

        List<RequirementCategoryDocLink> ISharedRequirementPackageRepository.GetRequirementCatDocUrls(Int32 reqCatId)
        {
            return SharedDataDBContext.RequirementCategoryDocLinks.Where(cond => !cond.RCDL_IsDeleted && cond.RCDL_RequirementCategoryID == reqCatId).ToList();
        }

        #endregion

        #region UAT-4657

        #region Requirement Package Versioning
        void ISharedRequirementPackageRepository.ManageRequirementVersionTenantMapping(Int32 currentOrgUserID)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_ManageRequirementPkgVersionRotationMappings", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CurrentUserId", currentOrgUserID);
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        List<RequirementPkgVersionTenantMapping> ISharedRequirementPackageRepository.GetRequirementPkgVersionTenantMapping(Int32 requirementPkgVersioningStatus_DueId)
        {
            return _sharedDataDBContext.RequirementPkgVersionTenantMappings.Where(con => con.RPVTM_StatusId == requirementPkgVersioningStatus_DueId && !con.RPVTM_IsDeleted
                                                                                 && con.RPVTM_RetryCount < AppConsts.THREE).ToList();
        }

        Boolean ISharedRequirementPackageRepository.UpdateRequirementPkgVersioningStatusInRequirementPackage(Int32 currentLoggedInUserId, Int32 requirementPkgVersioningStatus_DueId
                                                                                                        , Int32 requirementPkgVersioningStatus_InProgressId
                                                                                                        , Int32 requirementPkgVersioningStatus_CompletedId)
        {
            List<RequirementPackage> lstRequirementPackages = _sharedDataDBContext.RequirementPackages.Where(cond => cond.RP_IsDeleted == false
                                                                && (cond.RP_VersionStatusId == requirementPkgVersioningStatus_DueId
                                                                || cond.RP_VersionStatusId == requirementPkgVersioningStatus_InProgressId)).ToList();

            lstRequirementPackages.ForEach(reqPkg =>
            {
                List<RequirementPkgVersionTenantMapping> requirementPkgVersionTenantMappings = reqPkg.RequirementPkgVersionTenantMappings.Where(con => !con.RPVTM_IsDeleted).ToList();
                Boolean isNeedToMarkCompleted = false;

                if (!requirementPkgVersionTenantMappings.IsNullOrEmpty() && requirementPkgVersionTenantMappings.Any())
                {
                    if (requirementPkgVersionTenantMappings.Where(con => con.RPVTM_StatusId != requirementPkgVersioningStatus_CompletedId && con.RPVTM_RetryCount < AppConsts.THREE).Any())
                        isNeedToMarkCompleted = true;
                }
                if (isNeedToMarkCompleted)
                {
                    reqPkg.RP_VersionStatusId = requirementPkgVersioningStatus_CompletedId;
                    reqPkg.RP_ModifiedOn = DateTime.Now;
                    reqPkg.RP_ModifiedByID = currentLoggedInUserId;
                }
            });
            if (_sharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;


        }

        #endregion

        #region Requirement Category Disassociation
        Dictionary<Int32, String> ISharedRequirementPackageRepository.GetPackagesAssociatedWithCategory(Int32 categoryId)
        {
            Dictionary<Int32, String> pkgsLst = new Dictionary<int, string>();
            //pkgsLst = _sharedDataDBContext.RequirementPackages.Join(_sharedDataDBContext.RequirementPackageCategories,
            //           RP => RP.RP_ID
            //           , RPC => RPC.RPC_RequirementPackageID
            //           , (RP, RPC) => new { RP.RP_ID, RP.RP_PackageName, RPC.RPC_RequirementCategoryID })
            //           .Where(x => x.RPC_RequirementCategoryID == categoryId)
            //           .Distinct().ToDictionary(g => g.RP_ID, g => g.RP_PackageName);

            List<RequirementPackage> requirementPackages = _sharedDataDBContext.RequirementPackageCategories
                                                                                .Where(con => con.RPC_RequirementCategoryID == categoryId
                                                                                              && !con.RPC_IsDeleted
                                                                                              && con.RequirementPackage != null
                                                                                              && !con.RequirementPackage.RP_IsDeleted)
                                                                                .Select(sel => sel.RequirementPackage).ToList();

            if (!requirementPackages.IsNullOrEmpty())
            {
                requirementPackages.ForEach(x =>
                {
                    pkgsLst.Add(x.RP_ID, x.RP_PackageName);
                });
            }
            return pkgsLst;
        }

        public Boolean SaveCategoryDiassociationDetail(Int32 categoryId, String packageIds, Int32 currentLoggedInUserId)
        {
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_SaveCategoryDiassociationData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                command.Parameters.AddWithValue("@PackageIds", packageIds);
                command.Parameters.AddWithValue("@CurrentLoggedInUserId", currentLoggedInUserId);
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
                return true;
            }
        }

        List<RequirementCategoryDisassociationTenantMapping> ISharedRequirementPackageRepository.GetRequirementCategoryDisassociationTenantMappingForDisassociation()
        {
            return _sharedDataDBContext.RequirementCategoryDisassociationTenantMappings.Where(con => !con.RCDTM_IsDeleted && !con.RCDTM_IsCompleted && !con.RCDTM_IsDiscarded).ToList();
        }

        #endregion

        //List<Int32> ISharedRequirementPackageRepository.GetPendingTenantsWhichPkgVersioningOrCategoryDisassociationPending(Int32 requirementPkgVersioningStatus_NoRotationId, Int32 requirementPkgVersioningStatus_CompletedId)
        //{
        //    List<Int32> lstTenantIds = new List<Int32>();

        //    lstTenantIds.AddRange(_sharedDataDBContext.RequirementPkgVersionTenantMappings.Where(con => !con.RPVTM_IsDeleted && con.RPVTM_RetryCount < 3 &&
        //                                                                  con.RPVTM_StatusId != requirementPkgVersioningStatus_NoRotationId
        //                                                                  && con.RPVTM_StatusId != requirementPkgVersioningStatus_CompletedId).Select(sel => sel.RPVTM_TenantId).ToList());

        //    lstTenantIds.AddRange(_sharedDataDBContext.RequirementCategoryDisassociationTenantMappings.Where(con => !con.RCDTM_IsDeleted && !con.RCDTM_IsCompleted && !con.RCDTM_IsDiscarded)
        //                                              .Select(sel => sel.RCDTM_TenantId).ToList());
        //    return lstTenantIds;
        //}

        String ISharedRequirementPackageRepository.IsCategoryDisassociationInProgress(Int32 requirementCategoryId, List<Int32> selectedPkgIds
                                                                                        , Int32 requirementPkgVersioningStatus_DueId, Int32 requirementPkgVersioningStatus_InProgressId)
        {
            Boolean isCategoryDisassociationAlreadyInProgress = _sharedDataDBContext.RequirementCategoryDisassociations.Where(con => con.RCD_SourceCategoryId == requirementCategoryId && !con.RCD_IsDeleted && !con.RCD_IsMovementDiscarded
                                                                        && !con.RCD_IsMovementCompleted).Any();

            if (!isCategoryDisassociationAlreadyInProgress)
            {
                List<Guid> lstPackagesGuid =
                _sharedDataDBContext.RequirementPackages.Where(con => selectedPkgIds.Contains(con.RP_ID) && !con.RP_IsDeleted && con.RP_Code != null).Select(sel => sel.RP_Code.Value).ToList();

                if (!lstPackagesGuid.IsNullOrEmpty() && lstPackagesGuid.Any())
                {
                    Boolean isPkgVersioningPending = _sharedDataDBContext.RequirementPackages
                                                        .Where(con => ((con.RP_ParentPackageCode != null && lstPackagesGuid.Contains(con.RP_ParentPackageCode.Value))
                                                            || (con.RP_Code != null && lstPackagesGuid.Contains(con.RP_Code.Value)))
                                                            && (con.RP_VersionStatusId.Value == requirementPkgVersioningStatus_DueId
                                                            || con.RP_VersionStatusId.Value == requirementPkgVersioningStatus_InProgressId)
                                                             && !con.RP_IsDeleted).Any();

                    return isPkgVersioningPending ? "Package" : "";
                }
                else
                {
                    return String.Empty;
                }
            }
            return "Category";
        }

        List<RequirementPackageContract> ISharedRequirementPackageRepository.GetAllPkgVersionsByPkgId(Int32 PkgId)
        {
            List<RequirementPackageContract> requirementPackageList = new List<RequirementPackageContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {

                    new SqlParameter("@SelectedPkgId", PkgId)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAllPkgVersionsByPkgId", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementPackageContract requirementPackageContract = new RequirementPackageContract();
                            requirementPackageContract.RequirementPackageID = dr["RequirementPackageID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["RequirementPackageID"]);
                            requirementPackageContract.RequirementPackageName = Convert.ToString(dr["RequirementPackageName"]);
                            requirementPackageContract.RequirementPackageLabel = Convert.ToString(dr["RequirementPackageLabel"]);
                            requirementPackageContract.EffectiveStartDate = dr["RequirementEffectiveStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RequirementEffectiveStartDate"]);
                            requirementPackageContract.EffectiveEndDate = dr["RequirementEffectiveEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["RequirementEffectiveEndDate"]);
                            requirementPackageList.Add(requirementPackageContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return requirementPackageList.OrderByDescending(col => col.EffectiveStartDate).ToList();
        }

        void ISharedRequirementPackageRepository.UpdateRequirementCategoryDisassociationStatus(Int32 backgroundProcessUserId)
        {
            List<RequirementCategoryDisassociation> requirementCategoryDisassociations = _sharedDataDBContext.RequirementCategoryDisassociations.Where(con => !con.RCD_IsDeleted
                                                                                                                                                        && !con.RCD_IsMovementCompleted
                                                                                                                                                        && !con.RCD_IsMovementDiscarded).ToList();

            if (!requirementCategoryDisassociations.IsNullOrEmpty() && requirementCategoryDisassociations.Any())
            {
                foreach (RequirementCategoryDisassociation reqCatDisassociation in requirementCategoryDisassociations)
                {
                    Boolean isCompletedCategoryDissociationInAllTenants = true;
                    List<RequirementCategoryDisassociationTenantMapping> requirementCategoryDisassociationTenantMapping
                        = reqCatDisassociation.RequirementCategoryDisassociationTenantMappings.Where(con => !con.RCDTM_IsDeleted).ToList();

                    if (!requirementCategoryDisassociationTenantMapping.IsNullOrEmpty() && requirementCategoryDisassociationTenantMapping.Count > AppConsts.NONE)
                    {
                        if (requirementCategoryDisassociationTenantMapping.Where(con => !con.RCDTM_IsCompleted && !con.RCDTM_IsDiscarded).Any())
                            isCompletedCategoryDissociationInAllTenants = false;

                        if (isCompletedCategoryDissociationInAllTenants)
                        {
                            reqCatDisassociation.RCD_IsMovementCompleted = true;
                            reqCatDisassociation.RCD_ModifiedBy = backgroundProcessUserId;
                            reqCatDisassociation.RCD_ModifiedOn = DateTime.Now;
                        }
                    }
                }
                _sharedDataDBContext.SaveChanges();
            }
        }


        List<RequirementPackage> ISharedRequirementPackageRepository.GetMasterRequirementPackages()
        {
            return _sharedDataDBContext.RequirementPackages.Where(con => !con.RP_IsDeleted).ToList();
        }

        Boolean ISharedRequirementPackageRepository.IsSyncAlreadyInProgress(Int32 objectId, Boolean IsObjectTypePackage, List<lkpObjectType> lkpObjectTypes)
        {
            Int32 retryCount = AppConsts.RETRY_COUNT_FOR_REQUIREMENT_PACKAGE_SYNC;

            List<Guid> objGuids = new List<Guid>();
            List<RequirementObjectTree> requirementObjectTrees = _sharedDataDBContext.RequirementObjectTrees.Where(con => !con.ROT_IsDeleted && con.ROT_ObjectID != null).ToList();

            List<RequirementCategory> requirementCategories = new List<RequirementCategory>();

            //PackageTypeObject
            if (IsObjectTypePackage)
            {
                //RequirementPackage requirementPackage = _sharedDataDBContext.RequirementPackages.Where(con => con.RP_ID == objectId && !con.RP_IsDeleted).FirstOrDefault();

                //objGuids.Add(requirementPackage.RP_Code.Value);

                //String pkgObjectTypeCode = ObjectType.Compliance_Package.GetStringValue();

                //Int32 pkgObjectTypeId = lkpObjectTypes.Where(cond => cond.OT_Code == pkgObjectTypeCode && !cond.OT_IsDeleted).FirstOrDefault().OT_ID;

                //lstRequirementObjectTreeIds.AddRange(requirementObjectTrees.Where(con => con.ROT_ObjectTypeID == pkgObjectTypeId && con.ROT_ObjectID.Value == requirementPackage.RP_ID)
                //                                                            .Select(sel => sel.ROT_ID).ToList());


                requirementCategories.AddRange(_sharedDataDBContext.RequirementPackageCategories.Where(con => con.RPC_RequirementPackageID == objectId && !con.RPC_IsDeleted
                                                                                                                            && con.RequirementCategory != null && con.RequirementCategory.RC_Code != null)
                                                                                                                    .Select(sel => sel.RequirementCategory).ToList());
            }
            else
            {
                //CategoryTypeObject
                requirementCategories.AddRange(_sharedDataDBContext.RequirementCategories.Where(con => con.RC_ID == objectId && !con.RC_IsDeleted).ToList());
            }

            List<Int32> lstReqCatIds = new List<Int32>();
            if (!requirementCategories.IsNullOrEmpty())
            {
                List<Int32> lstRequirementObjectTreeIds = new List<Int32>();

                objGuids.AddRange(requirementCategories.Where(con => con.RC_Code != null).Select(sel => sel.RC_Code.Value).ToList());

                lstReqCatIds.AddRange(requirementCategories.Select(sel => sel.RC_ID).ToList());

                String catObjectTypeCode = ObjectType.Compliance_Category.GetStringValue();
                Int32 catObjectTypeId = lkpObjectTypes.Where(cond => cond.OT_Code == catObjectTypeCode && !cond.OT_IsDeleted).FirstOrDefault().OT_ID;


                lstRequirementObjectTreeIds.AddRange(requirementObjectTrees.Where(con => con.ROT_ObjectTypeID == catObjectTypeId && lstReqCatIds.Contains(con.ROT_ObjectID.Value)).Select(sel => sel.ROT_ID).ToList());

                List<RequirementItem> requirementItems = _sharedDataDBContext.RequirementCategoryItems.Where(con => lstReqCatIds.Contains(con.RCI_RequirementCategoryID) && !con.RCI_IsDeleted
                                                                                                                    && con.RequirementItem != null && con.RequirementItem.RI_Code != null)
                                                                                                      .Select(sel => sel.RequirementItem).ToList();
                List<Int32> lstReqItemIds = new List<Int32>();
                if (!requirementItems.IsNullOrEmpty())
                {
                    objGuids.AddRange(requirementItems.Where(con => con.RI_Code != null).Select(sel => sel.RI_Code.Value).ToList());

                    lstReqItemIds.AddRange(requirementItems.Select(sel => sel.RI_ID).ToList());

                    String itemObjectTypeCode = ObjectType.Compliance_Item.GetStringValue();
                    Int32 itemObjectTypeId = lkpObjectTypes.Where(cond => cond.OT_Code == itemObjectTypeCode && !cond.OT_IsDeleted).FirstOrDefault().OT_ID;

                    lstRequirementObjectTreeIds.AddRange(requirementObjectTrees.Where(con => con.ROT_ObjectTypeID == itemObjectTypeId && lstReqItemIds.Contains(con.ROT_ObjectID.Value)).Select(sel => sel.ROT_ID).ToList());


                    List<RequirementField> requirementFields = _sharedDataDBContext.RequirementItemFields.Where(con => lstReqItemIds.Contains(con.RIF_RequirementItemID) && !con.RIF_IsDeleted
                                                                                                                       && con.RequirementField != null && con.RequirementField.RF_Code != null)
                                                                                                            .Select(sel => sel.RequirementField).ToList();
                    List<Int32> lstReqFieldIds = new List<Int32>();
                    if (!requirementFields.IsNullOrEmpty())
                    {
                        lstReqFieldIds.AddRange(requirementFields.Select(sel => sel.RF_ID).ToList());


                        objGuids.AddRange(requirementFields.Where(con => con.RF_Code != null).Select(sel => sel.RF_Code.Value).ToList());

                        String fieldObjectTypeCode = ObjectType.Compliance_ATR.GetStringValue();
                        Int32 fieldObjectTypeId = lkpObjectTypes.Where(cond => cond.OT_Code == fieldObjectTypeCode && !cond.OT_IsDeleted).FirstOrDefault().OT_ID;

                        lstRequirementObjectTreeIds.AddRange(requirementObjectTrees.Where(con => con.ROT_ObjectTypeID == fieldObjectTypeId && lstReqFieldIds.Contains(con.ROT_ObjectID.Value)).Select(sel => sel.ROT_ID).ToList());

                    }
                }
                if (!lstRequirementObjectTreeIds.IsNullOrEmpty() && lstRequirementObjectTreeIds.Count > AppConsts.NONE)
                {
                    objGuids.AddRange(_sharedDataDBContext.RequirementObjectRules.Where(con => con.ROR_ObjectTreeId != null && lstRequirementObjectTreeIds.Contains(con.ROR_ObjectTreeId.Value))
                                                                                  .Select(sel => sel.ROR_Code).ToList());
                }
            }
            if (!objGuids.IsNullOrEmpty() && objGuids.Any())
            {
                List<Int32> requirementPkgSyncObjectIds = _sharedDataDBContext.RequirementPkgSyncObjectDetails.Where(cond => !cond.RPSD_IsDeleted
                                               && cond.RPSD_IsActive && cond.RPSD_RetryCount < retryCount && cond.SyncRequirementPackageObject != null
                                               && cond.SyncRequirementPackageObject.SRPO_IsActive && !cond.SyncRequirementPackageObject.SRPO_IsDeleted
                                               && objGuids.Contains(cond.SyncRequirementPackageObject.SRPO_NewObjectCode))
                                               .Select(sel => sel.RPSD_SyncRequirementPkgObjectID).ToList();

                if (requirementPkgSyncObjectIds.IsNullOrEmpty() && requirementPkgSyncObjectIds.Count == AppConsts.NONE)
                    return false;
                return true;
            }
            return false;
        }

        #endregion
    }
}
