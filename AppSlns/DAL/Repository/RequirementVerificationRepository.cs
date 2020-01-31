using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;

using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.Common;
using System.Data.Entity.Core.Objects.DataClasses;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using System.Xml;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using System.Text;

namespace DAL.Repository
{
    public class RequirementVerificationRepository : ClientBaseRepository, IRequirementVerificationRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public RequirementVerificationRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }

        #region Common Methods

        /// <summary>
        /// Get requirement Item StatusID by code
        /// </summary>
        /// <param name="lkpReqItemStatus"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        private Int32 GetItemStatusIdByCode(List<lkpRequirementItemStatu> lkpReqItemStatus, String statusCode)
        {
            return lkpReqItemStatus.Where(ris => ris.RIS_Code == statusCode).First().RIS_ID;
        }

        /// <summary>
        /// Get requirement Category StatusID by code
        /// </summary>
        /// <param name="lkpReqCatStatus"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        private Int32 GetRequirementCategoryStausIdByCode(List<lkpRequirementCategoryStatu> lkpReqCatStatus, String statusCode)
        {
            return lkpReqCatStatus.Where(rcs => rcs.RCS_Code == statusCode).First().RCS_ID;
        }


        #endregion

        #region Requirement Verification Queue

        /// <summary>
        /// Get requirement verification queue search data
        /// </summary>
        /// <param name="searchDataContract"></param>
        /// <param name="customPagingArgsContract"></param>
        /// <returns></returns>
        List<RequirementVerificationQueueContract> IRequirementVerificationRepository.GetRequirementVerificationQueueSearch(RequirementVerificationQueueContract searchDataContract, CustomPagingArgsContract customPagingArgsContract)
        {
            //string orderBy = QueueConstants.APPLICANT_SEARCH_DEFAULT_SORTING_FIELDS;

            List<RequirementVerificationQueueContract> lstRequirementVerificationQueueContract = new List<RequirementVerificationQueueContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetRequirementVerificationQueueSearch", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@xmlData", searchDataContract.XML);
                command.Parameters.AddWithValue("@xmlSortingAndFilteringData", customPagingArgsContract.XML);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lstRequirementVerificationQueueContract = ds.Tables[0].AsEnumerable().Select(x =>
                      new RequirementVerificationQueueContract
                      {
                          OrganizationUserID = x["OrganizationUserID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["OrganizationUserID"]),
                          ApplicantFirstName = x["ApplicantFirstName"] == DBNull.Value ? String.Empty : Convert.ToString(x["ApplicantFirstName"]),
                          ApplicantLastName = x["ApplicantLastName"] == DBNull.Value ? String.Empty : Convert.ToString(x["ApplicantLastName"]),
                          ClinicalRotationID = x["ClinicalRotationID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ClinicalRotationID"]),
                          RotationStartDate = x["RotationStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(x["RotationStartDate"]),
                          RotationEndDate = x["RotationEndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(x["RotationEndDate"]),
                          SubmissionDate = x["SubmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(x["SubmissionDate"]),
                          RequirementPackageTypeID = x["RequirementPackageTypeID"] == DBNull.Value ? 0 : Convert.ToInt32(x["RequirementPackageTypeID"]),
                          // AgencyID = x["AgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(x["AgencyID"]),
                          AgencyName = x["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(x["AgencyName"]),
                          RequirementPackageSubscriptionID = x["RequirementPackageSubscriptionID"] == DBNull.Value ? 0 : Convert.ToInt32(x["RequirementPackageSubscriptionID"]),
                          TotalCount = x["TotalCount"] == DBNull.Value ? 0 : Convert.ToInt32(x["TotalCount"]),
                          //RequirementPackageID = x["RequirementPackageID"] == DBNull.Value ? 0 : Convert.ToInt32(x["RequirementPackageID"]),
                          RequirementPackageName = x["RequirementPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(x["RequirementPackageName"]),
                          IsCurrentRotation = x["IsCurrentRotation"] == DBNull.Value ? false : Convert.ToBoolean(x["IsCurrentRotation"]),
                          ReqReviewByID = x["ReqReviewByID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(x["ReqReviewByID"]),
                          RequirementItemId = x["RequirementItemId"] == DBNull.Value ? 0 : Convert.ToInt32(x["RequirementItemId"]),
                          ApplicantRequirementItemId = x["ApplicantRequirementItemId"] == DBNull.Value ? 0 : Convert.ToInt32(x["ApplicantRequirementItemId"]),
                          ReqReviewByDesc = x["ReqReviewByDesc"] == DBNull.Value ? String.Empty : Convert.ToString(x["ReqReviewByDesc"]),
                          RequirementCategoryID = x["RequirementCategoryID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["RequirementCategoryID"]),
                          //UAT-3703
                          UserType = x["UserType"] == DBNull.Value ? String.Empty : Convert.ToString(x["UserType"])
                          ,ReqCategoryLabel = x["CategoryLabel"] == DBNull.Value ? String.Empty : Convert.ToString(x["CategoryLabel"])
                          ,ReqItemLabel = x["ItemLabel"] == DBNull.Value ? String.Empty : Convert.ToString(x["ItemLabel"])

                      }).ToList();
                }
            }
            return lstRequirementVerificationQueueContract;
        }

        List<ReqPkgSubscriptionIDList> IRequirementVerificationRepository.GetReqPkgSubscriptionIdListForRotationVerification(RequirementVerificationQueueContract requirementVerificationQueueContract, Int32 CurrentReqPkgSubscriptionID, Int32 ApplicantRequirementItemID)
        {
            List<ReqPkgSubscriptionIDList> lstRequirementVerificationQueueContract = new List<ReqPkgSubscriptionIDList>();
            String sortExpression = requirementVerificationQueueContract.GridCustomPagingArguments.SortExpression;
            Boolean sortDirectionDescending = requirementVerificationQueueContract.GridCustomPagingArguments.SortDirectionDescending;
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetReqPkgSubscriptionIdListForRotationVerificationSearch", con);
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
                          //ApplicantRequirementCategoryId = x["ApplicantRequirementCategoryId"] == DBNull.Value ? 0 : Convert.ToInt32(x["ApplicantRequirementCategoryId"]),
                          RotationId = x["ClinicalRotationID"] == DBNull.Value ? 0 : Convert.ToInt32(x["ClinicalRotationID"]),
                          RequirementCategoryID = x["RequirementCategoryId"] == DBNull.Value ? 0 : Convert.ToInt32(x["RequirementCategoryId"]),
                          ApplicantId = x["OrganizationUserID"] == DBNull.Value ? 0 : Convert.ToInt32(x["OrganizationUserID"])
                      }).ToList();
                }
            }
            return lstRequirementVerificationQueueContract;

        }
        #endregion

        #region Requirement Verification Details

        /// <summary>
        /// Get the Requirement Verification Details Screen data, including the data entered by Applicant.
        /// </summary>
        /// <param name="reqPkgSubId"></param>
        /// <returns></returns>
        List<RequirementVerificationDetailContract> IRequirementVerificationRepository.GetVerificationDetailData(Int32 reqPkgSubId)
        {
            var _lstVerficationDetailContract = new List<RequirementVerificationDetailContract>();

            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@ReqSubscriptionId", reqPkgSubId), 
                           
                        };

                base.OpenSQLDataReaderConnection(_sqlConnection);

                using (SqlDataReader _dr = base.ExecuteSQLDataReader(_sqlConnection, "usp_GetRequirementVerificationDetailData", sqlParameterCollection))
                {
                    if (_dr.HasRows)
                    {
                        while (_dr.Read())
                        {
                            _lstVerficationDetailContract.Add(new RequirementVerificationDetailContract
                            {
                                PkgId = Convert.ToInt32(_dr["PkgId"]),
                                PkgName = Convert.ToString(_dr["PkgName"]),
                                PkgStatusCode = Convert.ToString(_dr["PkgStatusCode"]),
                                PkgStatusName = Convert.ToString(_dr["PkgStatusName"]),
                                CatName = Convert.ToString(_dr["CatName"]),
                                CatId = Convert.ToInt32(_dr["CatId"]),
                                ApplReqCatDataId = Convert.ToInt32(_dr["ApplReqCatDataId"]),
                                CatStatusCode = Convert.ToString(_dr["CatStatusCode"]),
                                CatStatusName = Convert.ToString(_dr["CatStatusName"]),
                                ItemName = Convert.ToString(_dr["ItemName"]),
                                ItemId = Convert.ToInt32(_dr["ItemId"]),
                                ApplReqItemDataId = Convert.ToInt32(_dr["ApplReqItemDataId"]),
                                ItemStatusCode = Convert.ToString(_dr["ItemStatusCode"]),
                                ItemStatusName = Convert.ToString(_dr["ItemStatusName"]),
                                FieldName = Convert.ToString(_dr["FieldName"]),
                                FieldId = Convert.ToInt32(_dr["FieldId"]),
                                FieldDataTypeCode = Convert.ToString(_dr["FieldDataTypeCode"]),
                                ApplReqFieldDataId = Convert.ToInt32(_dr["ApplReqFieldDataId"]),
                                FieldDataValue = Convert.ToString(_dr["FieldDataValue"]),
                                OptionText = Convert.ToString(_dr["OptionText"]),
                                OptionValue = Convert.ToString(_dr["OptionValue"]),
                                ApplDocId = Convert.ToInt32(_dr["ApplDocId"]),
                                FieldDocName = Convert.ToString(_dr["FieldDocName"]),
                                FieldDocPath = Convert.ToString(_dr["FieldDocPath"]),
                                FieldRank = Convert.ToInt32(_dr["FieldRank"]),
                                IsFieldRequired = _dr["IsFieldRequired"] == DBNull.Value ? false : Convert.ToBoolean(_dr["IsFieldRequired"]),
                                RejectionReason = Convert.ToString(_dr["RejectionReason"]),

                                //UAT-1555Add Document link to Rotation package student notes (as it is on immunization package student notes).
                                //   RequirementDocumentLink = _dr["RequirementDocumentLink"] == DBNull.Value ? null : Convert.ToString(_dr["RequirementDocumentLink"]), //Commented IN UAT-4254
                                CategoryExplanatoryNotes = _dr["CategoryExplanatoryNotes"] == DBNull.Value ? String.Empty : Convert.ToString(_dr["CategoryExplanatoryNotes"]),
                                //  RequirementDocumentLinkLabel = _dr["RequirementDocumentLinkLabel"] == DBNull.Value ? null : Convert.ToString(_dr["RequirementDocumentLinkLabel"]), //UAT-3161 //Commented IN UAT-4254
                                RequirementDocumentLink = _dr["RequirementDocumentLink"] == DBNull.Value ? null : Convert.ToString(_dr["RequirementDocumentLink"]), //Added IN UAT-4254
                            });
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(_sqlConnection);
            }

            //Added  in UAT-4254
            foreach (var category in _lstVerficationDetailContract.DistinctBy(x => x.CatId))
            {
                category.lstReqCatDocUrls = new List<RequirementCategoryDocUrl>();

                if (!String.IsNullOrEmpty(category.RequirementDocumentLink))
                {
                    List<String> lstCatDocUrlsWithLabels = category.RequirementDocumentLink.Split(',').ToList();

                    lstCatDocUrlsWithLabels.ForEach(x =>
                    {
                        List<String> urlWithLabel = x.Split('#').ToList();
                        RequirementCategoryDocUrl requirementCategoryDocUrl = new RequirementCategoryDocUrl();
                        requirementCategoryDocUrl.RequirementCatDocUrlLabel = urlWithLabel[0];
                        requirementCategoryDocUrl.RequirementCatDocUrl = urlWithLabel[1];
                        requirementCategoryDocUrl.RequirementCatID = category.CatId;

                        category.lstReqCatDocUrls.Add(requirementCategoryDocUrl);
                    });
                }
            }
            //END

            return _lstVerficationDetailContract;
        }

        /// <summary>
        /// UAT 1626- Get the Requirement Verification Details by Category, including the data entered by Applicant.
        /// </summary>
        /// <param name="reqPkgSubId"></param>
        /// <returns></returns>
        List<RequirementVerificationDetailContract> IRequirementVerificationRepository.GetRequirementItemsByCategoryId(Int32 reqPkgSubId, List<Int32> reqCatId, Int32 rotationId)
        {
            var _lstVerficationDetailContract = new List<RequirementVerificationDetailContract>();

            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();

                SqlCommand sqlCmd = new SqlCommand("usp_GetRequirementItemsByCategoryId", _sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@ReqSubscriptionId", reqPkgSubId);
                sqlCmd.Parameters.AddWithValue("@RotationId", rotationId);

                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(Int32));

                foreach (var item in reqCatId)
                    dt.Rows.Add(item);

                SqlParameter integerLstParam = sqlCmd.Parameters.AddWithValue("@ReqCatId", dt);
                integerLstParam.SqlDbType = SqlDbType.Structured;

                base.OpenSQLDataReaderConnection(_sqlConnection);

                using (SqlDataReader _dr = sqlCmd.ExecuteReader())
                {
                    if (_dr.HasRows)
                    {
                        while (_dr.Read())
                        {
                            _lstVerficationDetailContract.Add(new RequirementVerificationDetailContract
                            {
                                PkgId = Convert.ToInt32(_dr["PkgId"]),
                                PkgName = Convert.ToString(_dr["PkgName"]),
                                PkgStatusCode = Convert.ToString(_dr["PkgStatusCode"]),
                                PkgStatusName = Convert.ToString(_dr["PkgStatusName"]),
                                CatName = Convert.ToString(_dr["CatName"]),
                                CatId = Convert.ToInt32(_dr["CatId"]),
                                ApplReqCatDataId = Convert.ToInt32(_dr["ApplReqCatDataId"]),
                                CatStatusCode = Convert.ToString(_dr["CatStatusCode"]),
                                CatStatusName = Convert.ToString(_dr["CatStatusName"]),
                                ItemName = Convert.ToString(_dr["ItemName"]),
                                ItemId = Convert.ToInt32(_dr["ItemId"]),
                                ApplReqItemDataId = Convert.ToInt32(_dr["ApplReqItemDataId"]),
                                ItemStatusCode = Convert.ToString(_dr["ItemStatusCode"]),
                                ItemStatusName = Convert.ToString(_dr["ItemStatusName"]),
                                FieldName = Convert.ToString(_dr["FieldName"]),
                                FieldId = Convert.ToInt32(_dr["FieldId"]),
                                FieldDataTypeCode = Convert.ToString(_dr["FieldDataTypeCode"]),
                                ApplReqFieldDataId = Convert.ToInt32(_dr["ApplReqFieldDataId"]),
                                FieldDataValue = Convert.ToString(_dr["FieldDataValue"]),
                                OptionText = Convert.ToString(_dr["OptionText"]),
                                OptionValue = Convert.ToString(_dr["OptionValue"]),
                                ApplDocId = Convert.ToInt32(_dr["ApplDocId"]),
                                FieldDocName = Convert.ToString(_dr["FieldDocName"]),
                                FieldDocPath = Convert.ToString(_dr["FieldDocPath"]),
                                FieldRank = Convert.ToInt32(_dr["FieldRank"]),
                                IsFieldRequired = _dr["IsFieldRequired"] == DBNull.Value ? false : Convert.ToBoolean(_dr["IsFieldRequired"]),
                                RejectionReason = Convert.ToString(_dr["RejectionReason"]),
                                FieldAttributeTypeCode = Convert.ToString(_dr["FieldAttributeTypeCode"]),
                                //UAT-1555Add Document link to Rotation package student notes (as it is on immunization package student notes).
                                //commented in UAT-4254
                                // RequirementDocumentLink = _dr["RequirementDocumentLink"] == DBNull.Value ? null : Convert.ToString(_dr["RequirementDocumentLink"]),
                                CategoryExplanatoryNotes = _dr["CategoryExplanatoryNotes"] == DBNull.Value ? String.Empty : Convert.ToString(_dr["CategoryExplanatoryNotes"]),
                                CategoryDescription = _dr["CategoryDescription"] == DBNull.Value ? String.Empty : Convert.ToString(_dr["CategoryDescription"]),
                                ItemDescription = _dr["ItemDescription"] == DBNull.Value ? String.Empty : Convert.ToString(_dr["ItemDescription"]),
                                ItemExplanatoryNotes = _dr["ItemExplanatoryNotes"] == DBNull.Value ? String.Empty : Convert.ToString(_dr["ItemExplanatoryNotes"]),
                                ItemSubmissionDate = _dr["SubmissionDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(_dr["SubmissionDate"]),
                                FieldMaxLength = _dr["FieldMaxLength"] == DBNull.Value ? AppConsts.FIFTY : Convert.ToInt32(_dr["FieldMaxLength"]),
                                RequirementItemDisplayOrder = _dr["RequirementCatItemDisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(_dr["RequirementCatItemDisplayOrder"]), //UAT-3078
                                RequirementItemFieldDisplayOrder = _dr["RequirementItemFieldDisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(_dr["RequirementItemFieldDisplayOrder"]),  //UAT-3078
                                //UAT-3077
                                IsPaymentTypeItem = _dr["IsPaymentTypeItem"] == DBNull.Value ? false : Convert.ToBoolean(_dr["IsPaymentTypeItem"]),
                                IsItemPaymentPaid = _dr["IsItemPaymentPaid"] == DBNull.Value ? false : Convert.ToBoolean(_dr["IsItemPaymentPaid"]),
                                ItemAmount = _dr["ItemAmount"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(_dr["ItemAmount"]),
                                ItemPaymentStatus = _dr["ItemPaymentStatus"] == DBNull.Value ? String.Empty : Convert.ToString(_dr["ItemPaymentStatus"]),
                                ItemPaymentStatusCode = _dr["ItemPaymentStatusCode"] == DBNull.Value ? String.Empty : Convert.ToString(_dr["ItemPaymentStatusCode"]),
                                PaidItemAmount = _dr["PaidItemAmount"] == DBNull.Value ? (Decimal?)null : Convert.ToDecimal(_dr["PaidItemAmount"]),
                                //UAT-3161
                                //commented in UAT-4254
                                // RequirementDocumentLinkLabel = _dr["RequirementDocumentLinkLabel"] == DBNull.Value ? null : Convert.ToString(_dr["RequirementDocumentLinkLabel"]),
                                RequirementAttributeGroupID = _dr["RequirementAttributeGroupID"] == DBNull.Value ? 0 : Convert.ToInt32(_dr["RequirementAttributeGroupID"]), //UAT-3176
                                ReqItemSampleDocFormURL = _dr["ReqItemSampleDocFormURL"] == DBNull.Value ? String.Empty : Convert.ToString(_dr["ReqItemSampleDocFormURL"]), //UAT-3309
                                AssignToUserID = _dr["AssignedToUserID"] == DBNull.Value ? 0 : Convert.ToInt32(_dr["AssignedToUserID"]), //UAT-3387
                                //ListRequirementItemURLContract = ClientDBContext.RequirementItemURLs.Where(item=>item.RIU_RequirementItemId==Convert.ToInt32(_dr["ItemId"]) && !item.RIU_IsDeleted);
                                //UAt-4165
                                IsEditableByAdmin = _dr["IsEditableByAdmin"] == DBNull.Value ? false : Convert.ToBoolean(_dr["IsEditableByAdmin"]),
                                IsEditableByApplicant = _dr["IsEditableByApplicant"] == DBNull.Value ? false : Convert.ToBoolean(_dr["IsEditableByApplicant"]),
                                IsEditableByClientAdmin = _dr["IsEditableByClientAdmin"] == DBNull.Value ? false : Convert.ToBoolean(_dr["IsEditableByClientAdmin"]),
                                //UAt-4380
                                IsFieldEditableByAdmin = _dr["IsFieldEditableByAdmin"] == DBNull.Value ? false : Convert.ToBoolean(_dr["IsFieldEditableByAdmin"]),
                                IsFieldEditableByApplicant = _dr["IsFieldEditableByApplicant"] == DBNull.Value ? false : Convert.ToBoolean(_dr["IsFieldEditableByApplicant"]),
                                IsFieldEditableByClientAdmin = _dr["IsFieldEditableByClientAdmin"] == DBNull.Value ? false : Convert.ToBoolean(_dr["IsFieldEditableByClientAdmin"]),
                                IsCategoryDataMovementAllowed = _dr["IsCategoryDataMovementAllowed"] == DBNull.Value ? false : Convert.ToBoolean(_dr["IsCategoryDataMovementAllowed"]), //UAT-4253
                                IsItemDataMovementAllowed = _dr["IsItemDataMovementAllowed"] == DBNull.Value ? false : Convert.ToBoolean(_dr["IsItemDataMovementAllowed"]), //UAT-4253
                                RequirementDocumentLink = _dr["RequirementDocumentLink"] == DBNull.Value ? null : Convert.ToString(_dr["RequirementDocumentLink"]) //Added in UAT-4254

                            });
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(_sqlConnection);
            }

            foreach (var item in _lstVerficationDetailContract.DistinctBy(x => x.ItemId))
            {

                var listRequirementItemURLs = ClientDBContext.RequirementItemURLs.Where(Iitem => Iitem.RIU_RequirementItemId == item.ItemId && !Iitem.RIU_IsDeleted).ToList();
                item.ListRequirementItemURLContract = new List<RequirementItemURLContract>();
                foreach (var items in listRequirementItemURLs)
                {

                    item.ListRequirementItemURLContract.Add(new RequirementItemURLContract { RItemURLID = items.RIU_ID, RItemURLLabel = items.RIU_Label, RItemURLSampleDocURL = items.RIU_SampleDocURL });
                }
            }

            //Added  in UAT-4254
            foreach (var category in _lstVerficationDetailContract.DistinctBy(x => x.CatId))
            {
                category.lstReqCatDocUrls = new List<RequirementCategoryDocUrl>();

                if (!String.IsNullOrEmpty(category.RequirementDocumentLink))
                {
                    List<String> lstCatDocUrlsWithLabels = category.RequirementDocumentLink.Split(',').ToList();

                    lstCatDocUrlsWithLabels.ForEach(x =>
                       {
                           List<String> urlWithLabel = x.Split('#').ToList();
                           RequirementCategoryDocUrl requirementCategoryDocUrl = new RequirementCategoryDocUrl();
                           requirementCategoryDocUrl.RequirementCatDocUrlLabel = urlWithLabel[0];
                           requirementCategoryDocUrl.RequirementCatDocUrl = urlWithLabel[1];
                           requirementCategoryDocUrl.RequirementCatID = category.CatId;

                           category.lstReqCatDocUrls.Add(requirementCategoryDocUrl);
                       });
                }
            }
            //END

            return _lstVerficationDetailContract;
        }


        /// <summary>
        /// Save/Update the data of the Verification Details screen.
        /// </summary>
        /// <param name="reqVerificationDataToSave"></param>
        /// <returns></returns>
        Dictionary<Int32, String> IRequirementVerificationRepository.SaveVerificationData(RequirementVerificationData reqVerificationDataToSave, List<lkpRequirementCategoryStatu> lkpReqCatStatus,
                                                                        List<lkpRequirementItemStatu> lkpReqItemStatus, Int32 currentUserId, List<lkpObjectType> lkpObjectType, ref Boolean isNewPackage)
        {
            var _currentDateTime = DateTime.Now;
            var _catIncompleteStatusCode = RequirementCategoryStatus.INCOMPLETE.GetStringValue();

            Boolean isSuccess = false;
            Boolean isFlatVerificationDataNeedsToUpdate = false; //UAT-4500
            Dictionary<Int32, String> dicResponse = new Dictionary<Int32, string>();
            foreach (var category in reqVerificationDataToSave.lstData)
            {
                if (category.ApplicantCatDataId == AppConsts.NONE)
                {
                    var _applCatData = new ApplicantRequirementCategoryData();

                    _applCatData.ARCD_RequirementPackageSubscriptionID = reqVerificationDataToSave.RPSId;
                    _applCatData.ARCD_RequirementCategoryID = category.CatId;
                    _applCatData.ARCD_CreatedOn = _currentDateTime;
                    _applCatData.ARCD_CreatedByID = currentUserId;
                    _applCatData.ARCD_IsDeleted = false;
                    _applCatData.ARCD_RequirementCategoryStatusID = GetRequirementCategoryStausIdByCode(lkpReqCatStatus, _catIncompleteStatusCode);

                    _applCatData.ApplicantRequirementItemDatas = new EntityCollection<ApplicantRequirementItemData>();

                    foreach (var item in category.lstItemData)
                    {
                        var _applItemDataInsert = GenerateApplicantRequirementItemFields(lkpReqItemStatus, currentUserId, _currentDateTime, item);
                        if (_applItemDataInsert.ARID_SubmissionDate.IsNullOrEmpty())
                        {
                            _applItemDataInsert.ARID_SubmissionDate = DateTime.Now;
                        }
                        _applItemDataInsert.ApplicantRequirementCategoryData = _applCatData;
                    }

                    RequirementPackage requirementPackage = ClientDBContext.RequirementPackageSubscriptions.FirstOrDefault(sel => sel.RPS_ID == reqVerificationDataToSave.RPSId).RequirementPackage;
                    Int32 reqPkgId = requirementPackage.RP_ID;
                    isNewPackage = requirementPackage.RP_IsNewPackage;
                    if (!requirementPackage.RP_IsNewPackage)
                    {
                        var dicResult = ValidateUiRules(_applCatData, lkpObjectType, reqPkgId, reqVerificationDataToSave.RPSId);
                        if (!dicResult.Keys.FirstOrDefault())
                        {
                            dicResponse.Add(-1, dicResult.Values.FirstOrDefault());
                        }
                        else
                        {
                            dicResponse.Add(0, dicResult.Values.FirstOrDefault());
                        }
                        isSuccess = dicResult.Keys.FirstOrDefault();
                    }
                    else
                    {

                        dicResponse = ValidateUiDynamicRules(_applCatData, category, lkpObjectType, reqPkgId, reqVerificationDataToSave.RPSId);
                        isSuccess = !dicResponse.Keys.Any();
                    }


                    if (isSuccess)
                    {
                        _dbContext.ApplicantRequirementCategoryDatas.AddObject(_applCatData);
                    }
                }
                else
                {
                    var _applCatDataUpdate = _dbContext.ApplicantRequirementCategoryDatas.Where(arcd => arcd.ARCD_ID == category.ApplicantCatDataId).First();
                    _applCatDataUpdate.ARCD_ModifiedOn = _currentDateTime;
                    _applCatDataUpdate.ARCD_ModifiedID = currentUserId;

                    RequirementPackage requirementPackage = ClientDBContext.RequirementPackageSubscriptions.FirstOrDefault(sel => sel.RPS_ID == reqVerificationDataToSave.RPSId).RequirementPackage;
                    isNewPackage = requirementPackage.RP_IsNewPackage;

                    foreach (var item in category.lstItemData)
                    {
                        if (item.ApplicantItemDataId == AppConsts.NONE)
                        {
                            var _applItemDataInsert = GenerateApplicantRequirementItemFields(lkpReqItemStatus, currentUserId, _currentDateTime, item);
                            _applItemDataInsert.ApplicantRequirementCategoryData = _applCatDataUpdate;
                            _dbContext.ApplicantRequirementItemDatas.AddObject(_applItemDataInsert);
                        }
                        else
                        {
                            var _applItemDataUpdate = _dbContext.ApplicantRequirementItemDatas.Where(arid => arid.ARID_ID == item.ApplicantItemDataId).First();

                            if (item.IsItemMarkedAsDeleted)
                            {
                                _applItemDataUpdate.ARID_IsDeleted = true;
                            }
                            else
                            {
                                _applItemDataUpdate.ARID_RequirementItemStatusID = GetItemStatusIdByCode(lkpReqItemStatus, item.ItemStatusCode);
                                _applItemDataUpdate.ARID_RejectionReason = item.RejectionReason;
                                //START UAT-4500
                                if (_applItemDataUpdate.lkpRequirementItemStatu.RIS_Code == RequirementItemStatus.APPROVED.GetStringValue() || _applItemDataUpdate.lkpRequirementItemStatu.RIS_Code == RequirementItemStatus.NOT_APPROVED.GetStringValue())
                                {
                                    var _flatRequirementVerificationData = SharedDataDBContext.FlatRequirementVerificationDetailDatas.Where(cond => cond.FRVDD_ApplicantRequirementItemId == _applItemDataUpdate.ARID_ID && cond.FRVDD_ApplicantRequirementCategoryId == _applItemDataUpdate.ARID_RequirementCategoryDataID && !cond.FRVDD_IsDeleted).OrderByDescending(odr => odr.FRVDD_ID).FirstOrDefault();
                                    if (!_flatRequirementVerificationData.IsNullOrEmpty() && !_flatRequirementVerificationData.FRVDD_AssignedToUserID.IsNullOrEmpty())
                                    {
                                        isFlatVerificationDataNeedsToUpdate = true;
                                        _flatRequirementVerificationData.FRVDD_AssignedToUserID = null;
                                        _flatRequirementVerificationData.FRVDD_AssignedUserName = string.Empty;
                                        _flatRequirementVerificationData.FRVDD_ModifiedBy = currentUserId;
                                        _flatRequirementVerificationData.FRVDD_ModifiedOn = _currentDateTime;
                                    }
                                }
                                //END UAT-4500
                            }

                            _applItemDataUpdate.ARID_ModifiedOn = _currentDateTime;
                            _applItemDataUpdate.ARID_ModifiedByID = currentUserId;

                            XmlDocument docApplFieldData = new XmlDocument();
                            XmlElement elApplFieldDataObject = (XmlElement)docApplFieldData.AppendChild(docApplFieldData.CreateElement("ApplFieldDataObject"));

                            foreach (var field in item.lstFieldData)
                            {
                                //XmlElement elApplFieldDataRec = (XmlElement)elApplFieldDataObject.AppendChild(docApplFieldData.CreateElement("ApplFieldDataRec"));

                                if (field.ApplicantFieldDataId == AppConsts.NONE && !item.IsItemMarkedAsDeleted)
                                {
                                    XmlElement elApplFieldDataRec = (XmlElement)elApplFieldDataObject.AppendChild(docApplFieldData.CreateElement("ApplFieldDataRec"));
                                    elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_ID")).InnerText = String.Empty;
                                    elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_RequirementItemDataID")).InnerText = Convert.ToString(item.ApplicantItemDataId);
                                    elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_RequirementFieldID")).InnerText = Convert.ToString(field.FieldId);
                                    elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_FieldValue")).InnerText = field.ApplicantFieldDataValue;
                                    elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_IsDeleted")).InnerText = Convert.ToString(0);
                                    elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_CreatedByID")).InnerText = Convert.ToString(currentUserId);
                                    elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_CreatedOn")).InnerText = Convert.ToString(_currentDateTime);
                                    elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_ModifiedByID")).InnerText = String.Empty;
                                    elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_ModifiedOn")).InnerText = String.Empty;

                                    //var _applFieldDataInsert = GenerateAppReqFieldDataInstance(currentUserId, _currentDateTime, field);
                                    //_applFieldDataInsert.ApplicantRequirementItemData = _applItemDataUpdate;
                                    //_dbContext.ApplicantRequirementFieldDatas.AddObject(_applFieldDataInsert);
                                }
                                else if (field.ApplicantFieldDataId.IsNotNull() && field.ApplicantFieldDataId != AppConsts.NONE)
                                {
                                    var _applFieldDataUpdate = _dbContext.ApplicantRequirementFieldDatas.Where(arfd => arfd.ARFD_ID == field.ApplicantFieldDataId).First();

                                    if (item.IsItemMarkedAsDeleted)
                                    {
                                        //_applFieldDataUpdate.ARFD_IsDeleted = true;
                                        //_applFieldDataUpdate.ARFD_ModifiedByID = currentUserId;
                                        //_applFieldDataUpdate.ARFD_ModifiedOn = _currentDateTime;

                                        XmlElement elApplFieldDataRec = (XmlElement)elApplFieldDataObject.AppendChild(docApplFieldData.CreateElement("ApplFieldDataRec"));
                                        elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_ID")).InnerText = Convert.ToString(field.ApplicantFieldDataId);
                                        elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_RequirementItemDataID")).InnerText = String.Empty;
                                        elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_RequirementFieldID")).InnerText = String.Empty;
                                        elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_FieldValue")).InnerText = String.Empty;
                                        elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_CreatedByID")).InnerText = String.Empty;
                                        elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_CreatedOn")).InnerText = String.Empty;
                                        elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_IsDeleted")).InnerText = Convert.ToString(1);
                                        elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_ModifiedByID")).InnerText = Convert.ToString(currentUserId);
                                        elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_ModifiedOn")).InnerText = Convert.ToString(_currentDateTime);


                                        string requirementFieldDataTypeCode = _applFieldDataUpdate.RequirementField.lkpRequirementFieldDataType.RFDT_Code.ToLower();

                                        if (requirementFieldDataTypeCode == RequirementFieldDataType.UPLOAD_DOCUMENT.GetStringValue().ToLower()
                                            || requirementFieldDataTypeCode == RequirementFieldDataType.VIEW_DOCUMENT.GetStringValue().ToLower())
                                        {
                                            // Delete Documents mapped 
                                            foreach (var document in _applFieldDataUpdate.ApplicantRequirementDocumentMaps)
                                            {
                                                document.ARDM_IsDeleted = true;
                                                document.ARDM_ModifiedByID = currentUserId;
                                                document.ARDM_ModifiedOn = _currentDateTime;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // Update data for Date and Combo type only
                                        if (_applFieldDataUpdate.RequirementField.lkpRequirementFieldDataType.RFDT_Code == RequirementFieldDataType.DATE.GetStringValue()
                                         || _applFieldDataUpdate.RequirementField.lkpRequirementFieldDataType.RFDT_Code == RequirementFieldDataType.OPTIONS.GetStringValue()
                                            || _applFieldDataUpdate.RequirementField.lkpRequirementFieldDataType.RFDT_Code == RequirementFieldDataType.TEXT.GetStringValue()
                                            )
                                        {
                                            //_applFieldDataUpdate.ARFD_FieldValue = field.ApplicantFieldDataValue;
                                            //_applFieldDataUpdate.ARFD_ModifiedOn = _currentDateTime;
                                            //_applFieldDataUpdate.ARFD_ModifiedByID = currentUserId;

                                            XmlElement elApplFieldDataRec = (XmlElement)elApplFieldDataObject.AppendChild(docApplFieldData.CreateElement("ApplFieldDataRec"));
                                            elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_ID")).InnerText = Convert.ToString(field.ApplicantFieldDataId);
                                            elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_RequirementItemDataID")).InnerText = String.Empty;
                                            elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_RequirementFieldID")).InnerText = String.Empty;
                                            elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_IsDeleted")).InnerText = Convert.ToString(0);
                                            elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_CreatedByID")).InnerText = String.Empty;
                                            elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_CreatedOn")).InnerText = String.Empty;
                                            elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_FieldValue")).InnerText = field.ApplicantFieldDataValue;
                                            elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_ModifiedByID")).InnerText = Convert.ToString(currentUserId);
                                            elApplFieldDataRec.AppendChild(docApplFieldData.CreateElement("ARFD_ModifiedOn")).InnerText = Convert.ToString(_currentDateTime);
                                        }
                                    }
                                }
                            }

                            // Call SP here
                            var xmlApplFieldData = docApplFieldData.OuterXml.ToString();
                            EntityConnection connection = _dbContext.Connection as EntityConnection;
                            Int32 rowsAffected;
                            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                            {
                                SqlCommand command = new SqlCommand("usp_SaveApplicantRequirementFieldData", con);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@xmlStringReqFieldData", xmlApplFieldData);

                                if (con.State == ConnectionState.Closed)
                                    con.Open();

                                rowsAffected = command.ExecuteNonQuery();
                            }
                        }
                    }

                    if (!requirementPackage.RP_IsNewPackage)
                    {
                        //dicResponse = ValidateUiRules(_applCatDataUpdate, lkpObjectType, _applCatDataUpdate.RequirementPackageSubscription.RPS_RequirementPackageID.Value, reqVerificationDataToSave.RPSId);
                        var dicResult = ValidateUiRules(_applCatDataUpdate, lkpObjectType, _applCatDataUpdate.RequirementPackageSubscription.RPS_RequirementPackageID.Value, reqVerificationDataToSave.RPSId);
                        if (!dicResult.Keys.FirstOrDefault())
                        {
                            dicResponse.Add(-1, dicResult.Values.FirstOrDefault());
                        }
                        else
                        {
                            dicResponse.Add(0, dicResult.Values.FirstOrDefault());
                        }
                        isSuccess = dicResult.Keys.FirstOrDefault();
                    }
                    else
                    {

                        //dicResponse = ValidateUiDynamicRules(_applCatDataUpdate, category, lkpObjectType, _applCatDataUpdate.RequirementPackageSubscription.RPS_RequirementPackageID.Value, reqVerificationDataToSave.RPSId);
                        dicResponse = ValidateUiDynamicRules(_applCatDataUpdate, category, lkpObjectType, _applCatDataUpdate.RequirementPackageSubscription.RPS_RequirementPackageID.Value, reqVerificationDataToSave.RPSId);
                        isSuccess = !dicResponse.Keys.Any();
                        //START UAT-4500
                        if (isSuccess && isFlatVerificationDataNeedsToUpdate)
                        {
                            SharedDataDBContext.SaveChanges();
                        }
                        //END UAT-4500
                    }
                    //isSuccess = !dicResponse.Keys.FirstOrDefault();
                }
            }
            if (isSuccess)
                _dbContext.SaveChanges();

            return dicResponse;
        }


        Boolean IRequirementVerificationRepository.IsNewRequirementPackage(Int32 reqPkgId)
        {
            return ClientDBContext.RequirementPackages.FirstOrDefault(cond => cond.RP_ID == reqPkgId && !cond.RP_IsDeleted).RP_IsNewPackage;
        }
        /// <summary>
        /// Get applicant documents by requirement package subscription id and category Id
        /// </summary>
        /// <param name="reqPkgSubId"></param>
        /// <param name="reqCatId"></param>
        /// <returns></returns>
        List<ApplicantFieldDocumentMappingContract> IRequirementVerificationRepository.GetRequirementApplicantDocumentsByCategoryId(Int32 reqPkgSubId, Int32 reqCatId)
        {
            var _lstApplicantFieldDocumentMappingContract = new List<ApplicantFieldDocumentMappingContract>();

            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();

                SqlCommand sqlCmd = new SqlCommand("usp_GetRequirementApplicantDocumentsByCategoryId", _sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@ReqSubscriptionId", reqPkgSubId);
                sqlCmd.Parameters.AddWithValue("@ReqCatId", reqCatId);

                base.OpenSQLDataReaderConnection(_sqlConnection);

                using (SqlDataReader _dr = sqlCmd.ExecuteReader())
                {
                    if (_dr.HasRows)
                    {
                        while (_dr.Read())
                        {
                            _lstApplicantFieldDocumentMappingContract.Add(new ApplicantFieldDocumentMappingContract
                            {
                                ApplicantDocumentId = Convert.ToInt32(_dr["ApplDocId"]),
                                FileName = Convert.ToString(_dr["FieldDocName"]),
                                DocumentPath = Convert.ToString(_dr["FieldDocPath"]),
                                DocumentType = Convert.ToString(_dr["FieldDocType"])
                            });
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(_sqlConnection);
            }
            return _lstApplicantFieldDocumentMappingContract;
        }

        #endregion

        #region UAT-1626 : update rotation package verification to mimic tracking verification
        /// <summary>
        /// Get Package and Category Details for Compliance Verification detail screen
        /// on basis of ReqPkgSubscriptionID
        /// </summary>
        /// <param name="ReqPkgSubscriptionID"></param>
        /// <returns></returns>
        List<RequirementVerificationDetailContract> IRequirementVerificationRepository.GetRequirementPackageCategoryData(Int32 ReqPkgSubscriptionID, Int32 rotationId)
        {
            var _lstVerficationDetailContract = new List<RequirementVerificationDetailContract>();

            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@ReqSubscriptionId", ReqPkgSubscriptionID),
                           new SqlParameter("@RotationId", rotationId),

                        };

                base.OpenSQLDataReaderConnection(_sqlConnection);

                using (SqlDataReader _dr = base.ExecuteSQLDataReader(_sqlConnection, "usp_GetRequirementPackageCategory", sqlParameterCollection))
                {
                    if (_dr.HasRows)
                    {
                        while (_dr.Read())
                        {
                            _lstVerficationDetailContract.Add(new RequirementVerificationDetailContract
                            {
                                PkgId = Convert.ToInt32(_dr["PkgId"]),
                                PkgName = Convert.ToString(_dr["PkgName"]),
                                PkgLabel = Convert.ToString(_dr["PkgLabel"]),
                                PkgStatusCode = Convert.ToString(_dr["PkgStatusCode"]),
                                PkgStatusName = Convert.ToString(_dr["PkgStatusName"]),
                                CatName = Convert.ToString(_dr["CatName"]),
                                CatId = Convert.ToInt32(_dr["CatId"]),
                                CatStatusCode = Convert.ToString(_dr["CatStatusCode"]),
                                CatStatusName = Convert.ToString(_dr["CatStatusName"]),
                                RotationMemberCount = Convert.ToString(_dr["RotationMemberCount"]),
                                CategoryRuleStatusID = Convert.ToString(_dr["RuleStatusID"]),//UAT 3106
                                ComplianceRqdStartDate = Convert.ToString(_dr["ComplianceRqdStartDate"]) != null && !Convert.ToString(_dr["ComplianceRqdStartDate"]).IsNullOrEmpty() ? Convert.ToDateTime(_dr["ComplianceRqdStartDate"]) : (DateTime?)null,
                                ComplianceRqdEndDate = Convert.ToString(_dr["ComplianceRqdEndDate"]) != null && !Convert.ToString(_dr["ComplianceRqdEndDate"]).IsNullOrEmpty() ? Convert.ToDateTime(_dr["ComplianceRqdEndDate"]) : (DateTime?)null,
                                isActualComplianceRequired = Convert.ToBoolean(_dr["IsComplianceRequired"]),
                                IsItemDataEntryExist = Convert.ToBoolean(_dr["IsItemDataEntryExist"])
                            });
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(_sqlConnection);
            }
            return _lstVerficationDetailContract;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Generate the RequirementItem and Field level data.
        /// </summary>
        /// <param name="lkpReqItemStatus"></param>
        /// <param name="currentUserId"></param>
        /// <param name="_currentDateTime"></param>
        /// <param name="_applCatDataUpdate"></param>
        /// <param name="item"></param>
        private ApplicantRequirementItemData GenerateApplicantRequirementItemFields(List<lkpRequirementItemStatu> lkpReqItemStatus, Int32 currentUserId, DateTime _currentDateTime, RequirementVerificationItemData item)
        {
            var _applItemDataInsert = GenerateApplReqItemDataInstance(currentUserId, _currentDateTime, item);
            _applItemDataInsert.ARID_RequirementItemStatusID = GetItemStatusIdByCode(lkpReqItemStatus, item.ItemStatusCode);

            foreach (var field in item.lstFieldData)
            {
                var _applFieldDataInsert = GenerateAppReqFieldDataInstance(currentUserId, _currentDateTime, field);
                _applFieldDataInsert.ApplicantRequirementItemData = _applItemDataInsert;
            }
            return _applItemDataInsert;
        }

        /// <summary>
        /// Generate the instance of the 'ApplicantRequirementFieldData'
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="_currentDateTime"></param>
        /// <param name="_applItemDataInsert"></param>
        /// <param name="field"></param>
        private ApplicantRequirementFieldData GenerateAppReqFieldDataInstance(Int32 currentUserId, DateTime _currentDateTime, RequirementVerificationFieldData field)
        {
            return new ApplicantRequirementFieldData
            {
                ARFD_RequirementFieldID = field.FieldId,
                ARFD_CreatedByID = currentUserId,
                ARFD_CreatedOn = _currentDateTime,
                ARFD_IsDeleted = false,
                ARFD_FieldValue = field.ApplicantFieldDataValue
            };
        }

        /// <summary>
        /// Generate the instance of the 'ApplicantRequirementItemData'
        /// </summary>
        /// <param name="lkpReqItemStatus"></param>
        /// <param name="currentUserId"></param>
        /// <param name="_currentDateTime"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private ApplicantRequirementItemData GenerateApplReqItemDataInstance(Int32 currentUserId, DateTime _currentDateTime, RequirementVerificationItemData item)
        {
            return new ApplicantRequirementItemData
            {
                ARID_RequirementItemID = item.ItemId,
                ARID_RejectionReason = item.RejectionReason,
                ARID_CreatedByID = currentUserId,
                ARID_CreatedOn = _currentDateTime,
                ARID_IsDeleted = false,
                ARID_SubmissionDate = _currentDateTime
            };
        }

        /// <summary>
        /// Get conenction string from the Context
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            return (_dbContext.Connection as System.Data.Entity.Core.EntityClient.EntityConnection).StoreConnection.ConnectionString;
        }

        #endregion


        Tuple<List<Int32>, String, Dictionary<Boolean, String>> IRequirementVerificationRepository.RotationSubscriptionApproveAllPendingItems(Int32 reqPkgSubsId, Int32 currentLoogedInUserId, Boolean isAdmin, ref Int32 affectedItemsCount)
        {
            String PendingApprovedItemNames = String.Empty; //UAT-4543
            List<Int32> lstCategory = new List<int>();
            Dictionary<Boolean, String> dicResponse = new Dictionary<bool, string>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection _sqlConnection = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                _sqlConnection.ConnectionString = GetConnectionString();

                SqlCommand cmd = new SqlCommand("usp_RotationSubscriptionApproveAllPendingItems", _sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ReqSubscriptionId", reqPkgSubsId);
                cmd.Parameters.AddWithValue("@CurrentLoggedInUserId", currentLoogedInUserId);
                cmd.Parameters.AddWithValue("@IsAdmin", isAdmin);
                cmd.Parameters.Add("@AffectedItemsCount", SqlDbType.Int);
                cmd.Parameters["@AffectedItemsCount"].Direction = ParameterDirection.Output;

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    String resultXml = Convert.ToString(ds.Tables[0].Rows[0]["ResultXml"]);
                    dicResponse = EvaluateResultXml(resultXml);
                    Boolean isSuccess = !dicResponse.Keys.FirstOrDefault();
                    if (isSuccess)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            lstCategory.Add(Convert.ToInt32(ds.Tables[1].Rows[i]["AppReqCategoryId"]));
                        }
                    }
                    PendingApprovedItemNames = Convert.ToString(ds.Tables[2].Rows[0]["PendingApprovedItem"]); //UAT-4543
                }

                if (!cmd.Parameters["@AffectedItemsCount"].Value.IsNullOrEmpty())
                    affectedItemsCount = Convert.ToInt32(cmd.Parameters["@AffectedItemsCount"].Value);
                else
                    affectedItemsCount = 0;

                base.CloseSQLDataReaderConnection(_sqlConnection);
            }
            return new Tuple<List<Int32>, String, Dictionary<Boolean, String>>(lstCategory, PendingApprovedItemNames, dicResponse);
        }

        private Dictionary<Boolean, String> ValidateUiRules(ApplicantRequirementCategoryData applicantReqCatData, List<lkpObjectType> lstObjectTypes
                                            , Int32 requirementPackageId, Int32 packageSubscriptionId)
        {
            #region GENERATE XML OF APPLICANT DATA ENTRY FORM OBJECTS AND GET OBJECT RULES BY EXECUTING STORED PROCEDURE usp_Rule_GetObjectsRules
            List<ApplicantRequirementFieldData> lstApplicantFieldData = new List<ApplicantRequirementFieldData>();
            XmlDocument xmlObjectsRules = new XmlDocument();
            XmlElement elementObjectRules = (XmlElement)xmlObjectsRules.AppendChild(xmlObjectsRules.CreateElement("RuleObjects"));
            foreach (var applicantReqItemData in applicantReqCatData.ApplicantRequirementItemDatas.Where(sel => !sel.ARID_IsDeleted))
            {
                foreach (var applicantField in applicantReqItemData.ApplicantRequirementFieldDatas.Where(sel => !sel.ARFD_IsDeleted))
                {
                    lstApplicantFieldData.Add(applicantField);
                    XmlNode exp = elementObjectRules.AppendChild(xmlObjectsRules.CreateElement("RuleObject"));
                    exp.AppendChild(xmlObjectsRules.CreateElement("TypeId")).InnerText = Convert.ToString(GetObjectTypeId(LCObjectType.ComplianceATR.GetStringValue(), lstObjectTypes));
                    exp.AppendChild(xmlObjectsRules.CreateElement("Id")).InnerText = Convert.ToString(applicantField.ARFD_RequirementFieldID);
                    exp.AppendChild(xmlObjectsRules.CreateElement("ParentId")).InnerText = Convert.ToString(applicantReqItemData.ARID_RequirementItemID);
                }
            }

            Int32 _ruleTypeId = GetRuleTypeId(ComplianceRuleType.UIRules.GetStringValue());
            List<usp_GetRequiremntUiRules_Result> lstObjectsRules = _dbContext.usp_GetRequiremntUiRules(packageSubscriptionId, requirementPackageId, applicantReqCatData.ARCD_RequirementCategoryID, Convert.ToString(xmlObjectsRules.OuterXml)).ToList();

            XmlDocument xmlRules = new XmlDocument();
            XmlElement elementRules = (XmlElement)xmlRules.AppendChild(xmlRules.CreateElement("Rules"));

            foreach (var objectRule in lstObjectsRules)
            {

                XmlElement elementRule = (XmlElement)elementRules.AppendChild(xmlRules.CreateElement("Rule"));
                elementRule.AppendChild(xmlRules.CreateElement("Id")).InnerText = Convert.ToString(objectRule.RuleId);
                XmlElement elementMappings = (XmlElement)elementRule.AppendChild(xmlRules.CreateElement("Mappings"));

                XmlNode nodeRules = elementMappings.AppendChild(xmlRules.CreateElement("Mapping"));
                nodeRules.AppendChild(xmlRules.CreateElement("Key")).InnerText = "[Object1]";
                ApplicantRequirementFieldData fieldData = lstApplicantFieldData.FirstOrDefault(sel => sel.ARFD_RequirementFieldID == objectRule.ObjectID);
                nodeRules.AppendChild(xmlRules.CreateElement("Value")).InnerText = fieldData.IsNullOrEmpty() ? String.Empty : fieldData.ARFD_FieldValue;

                XmlNode nodeRule2 = elementMappings.AppendChild(xmlRules.CreateElement("Mapping"));
                nodeRule2.AppendChild(xmlRules.CreateElement("Key")).InnerText = "[Object2]";

                ApplicantRequirementFieldData field2Data = lstApplicantFieldData.FirstOrDefault(sel => sel.ARFD_RequirementFieldID == objectRule.Object2ID);
                nodeRule2.AppendChild(xmlRules.CreateElement("Value")).InnerText = field2Data.IsNullOrEmpty() ? String.Empty : field2Data.ARFD_FieldValue;
            }

            String resultXML = _dbContext.usp_Rule_EvaluateFixedUiRules(xmlRules.OuterXml).FirstOrDefault();

            Dictionary<Boolean, String> dicResponse = EvaluateResultXml(resultXML);
            return dicResponse;
            #endregion
        }

        private static Dictionary<bool, string> EvaluateResultXml(String resultXML)
        {
            Dictionary<Boolean, String> dicResponse = new Dictionary<Boolean, String>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(resultXML);
            XmlNode nodeStatus = null;
            nodeStatus = xml.SelectSingleNode("Result/Status");
            XmlNodeList nodeMessages = xml.SelectNodes("Result/Messages/Message");
            Int32 statusCode = AppConsts.NONE;
            String message = String.Empty;
            if (nodeStatus.IsNullOrEmpty())
            {
                nodeStatus = xml.SelectSingleNode("Result");
            }
            List<String> lstMessage = new List<string>();
            if (!nodeMessages.IsNullOrEmpty())
            {
                foreach (XmlNode xmlNode in nodeMessages)
                {
                    if (xmlNode.IsNotNull())
                    {
                        lstMessage.Add(xmlNode.InnerText);
                    }
                }
            }
            message = String.Join("<br/>", lstMessage);
            statusCode = Convert.ToInt32(nodeStatus["Code"].InnerText);
            dicResponse.Add(Convert.ToBoolean(statusCode), message);
            return dicResponse;
        }

        private Dictionary<Int32, String> ValidateUiDynamicRules(ApplicantRequirementCategoryData applicantReqCatDataToUpdate, RequirementVerificationCategoryData applicantReqCatData
                                                                    , List<lkpObjectType> lstObjectTypes
                                                                    , Int32 requirementPackageId, Int32 packageSubscriptionId)
        {
            #region GENERATE XML OF APPLICANT DATA ENTRY FORM OBJECTS AND GET OBJECT RULES BY EXECUTING STORED PROCEDURE usp_Rule_GetObjectsRules
            List<RequirementVerificationFieldData> lstApplicantFieldData = new List<RequirementVerificationFieldData>();
            XmlDocument xmlObjectsRules = new XmlDocument();
            XmlElement elementObjectRules = (XmlElement)xmlObjectsRules.AppendChild(xmlObjectsRules.CreateElement("RuleObjects"));

            //added extra nodes for hierarchy for category
            XmlNode expCategoryNode = elementObjectRules.AppendChild(xmlObjectsRules.CreateElement("RuleObject"));
            expCategoryNode.AppendChild(xmlObjectsRules.CreateElement("TypeId")).InnerText = Convert.ToString(GetObjectTypeId(LCObjectType.ComplianceCategory.GetStringValue(), lstObjectTypes));
            expCategoryNode.AppendChild(xmlObjectsRules.CreateElement("Id")).InnerText = Convert.ToString(applicantReqCatData.CatId);
            expCategoryNode.AppendChild(xmlObjectsRules.CreateElement("ParentId")).InnerText = Convert.ToString(AppConsts.NONE);

            List<ApplicantRequirementItemData> lstApplicantRequirementItemData = applicantReqCatDataToUpdate.ApplicantRequirementItemDatas.Where(sel => !sel.ARID_IsDeleted).ToList();

            //UAT-3842
            List<RequirementVerificationItemData> lstRequirementVerificationItemData = applicantReqCatData.lstItemData.Where(con => !con.IsItemMarkedAsDeleted).ToList();

            foreach (RequirementVerificationItemData applicantReqItemData in lstRequirementVerificationItemData)
            {
                XmlNode expItemNode = elementObjectRules.AppendChild(xmlObjectsRules.CreateElement("RuleObject"));
                expItemNode.AppendChild(xmlObjectsRules.CreateElement("TypeId")).InnerText = Convert.ToString(GetObjectTypeId(LCObjectType.ComplianceItem.GetStringValue(), lstObjectTypes));
                expItemNode.AppendChild(xmlObjectsRules.CreateElement("Id")).InnerText = Convert.ToString(applicantReqItemData.ItemId);
                expItemNode.AppendChild(xmlObjectsRules.CreateElement("ParentId")).InnerText = Convert.ToString(applicantReqCatData.CatId);

                foreach (RequirementVerificationFieldData applicantField in applicantReqItemData.lstFieldData)
                {
                    lstApplicantFieldData.Add(applicantField);
                    XmlNode exp = elementObjectRules.AppendChild(xmlObjectsRules.CreateElement("RuleObject"));
                    exp.AppendChild(xmlObjectsRules.CreateElement("TypeId")).InnerText = Convert.ToString(GetObjectTypeId(LCObjectType.ComplianceATR.GetStringValue(), lstObjectTypes));
                    exp.AppendChild(xmlObjectsRules.CreateElement("Id")).InnerText = Convert.ToString(applicantField.FieldId);
                    exp.AppendChild(xmlObjectsRules.CreateElement("ParentId")).InnerText = Convert.ToString(applicantReqItemData.ItemId);
                }

                if (applicantReqItemData.IsFileUploadApplicable)
                {
                    XmlNode exp = elementObjectRules.AppendChild(xmlObjectsRules.CreateElement("RuleObject"));
                    exp.AppendChild(xmlObjectsRules.CreateElement("TypeId")).InnerText = Convert.ToString(GetObjectTypeId(LCObjectType.ComplianceATR.GetStringValue(), lstObjectTypes));
                    exp.AppendChild(xmlObjectsRules.CreateElement("Id")).InnerText = Convert.ToString(applicantReqItemData.FileUploadAttributeId);
                    exp.AppendChild(xmlObjectsRules.CreateElement("ParentId")).InnerText = Convert.ToString(applicantReqItemData.ItemId);
                }
            }

            Int32 _ruleTypeId = GetRuleTypeId(ComplianceRuleType.UIRules.GetStringValue());

            Int32 applicantId = ClientDBContext.RequirementPackageSubscriptions.FirstOrDefault(cond => cond.RPS_ID == packageSubscriptionId).RPS_ApplicantOrgUserID;
            List<GetRequirementObjectsRules> lstObjectsRules = _dbContext.usp_Rule_GetRequirementObjectsRules(packageSubscriptionId, _ruleTypeId, Convert.ToString(xmlObjectsRules.OuterXml)).ToList();

            XmlDocument xmlRules = new XmlDocument();
            XmlElement elementRules = (XmlElement)xmlRules.AppendChild(xmlRules.CreateElement("Rules"));

            Int32 initialRuleMappingId = 0;
            XmlElement elementMappings = null;

            foreach (var objectRule in lstObjectsRules)
            {
                if (initialRuleMappingId != objectRule.RuleMappingID)
                {
                    XmlElement elementRule = (XmlElement)elementRules.AppendChild(xmlRules.CreateElement("Rule"));
                    elementRule.AppendChild(xmlRules.CreateElement("Id")).InnerText = Convert.ToString(objectRule.RuleMappingID);
                    XmlElement elementObjectMapping = (XmlElement)elementRule.AppendChild(xmlRules.CreateElement("ObjectMapping"));
                    elementMappings = (XmlElement)elementObjectMapping.AppendChild(xmlRules.CreateElement("Mappings"));
                    initialRuleMappingId = objectRule.RuleMappingID;
                }

                XmlNode nodeRules = elementMappings.AppendChild(xmlRules.CreateElement("Mapping"));
                nodeRules.AppendChild(xmlRules.CreateElement("Key")).InnerText = objectRule.PlaceHolder;

                if (objectRule.RLMD_ConstantValue == AppConsts.SUBMISSION_DATE)
                {
                    var itemUsedInRules = lstApplicantRequirementItemData.FirstOrDefault(cond => cond.ARID_RequirementItemID == objectRule.ObjectId);
                    nodeRules.AppendChild(xmlRules.CreateElement("Value")).InnerText = itemUsedInRules.IsNullOrEmpty() ? String.Empty :
                                                    (itemUsedInRules.ARID_SubmissionDate.IsNullOrEmpty() ? DateTime.Now.ToString("MM/dd/yyyy") :
                                                    String.Format("{0:MM/dd/yyyy}", itemUsedInRules.ARID_SubmissionDate));
                }
                else
                {
                    RequirementVerificationFieldData fieldData = lstApplicantFieldData.FirstOrDefault(sel => sel.FieldId == objectRule.ObjectId);
                    if (!String.IsNullOrEmpty(Convert.ToString(objectRule.RLMD_ConstantValue)))
                        nodeRules.AppendChild(xmlRules.CreateElement("Value")).InnerText = Convert.ToString(objectRule.RLMD_ConstantValue);
                    else
                        nodeRules.AppendChild(xmlRules.CreateElement("Value")).InnerText = fieldData.IsNullOrEmpty() ? objectRule.FieldValue : fieldData.ApplicantFieldDataValue;
                }
            }

            String resultXML = _dbContext.usp_Rule_EvaluateRequirementPreSubmitRules(xmlRules.OuterXml).FirstOrDefault();

            Dictionary<Int32, String> dicResponse = new Dictionary<Int32, String>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(resultXML);
            XmlNodeList nodeList = xml.SelectNodes("Results/Result");
            StringBuilder sbErrors = new StringBuilder();

            Int32 _errorCount = 0;
            Int32 statusCode = AppConsts.NONE;

            #region UAT-4260
            foreach (XmlNode xmlNode in nodeList)
            {
                if (xmlNode.IsNotNull())
                {
                    Boolean result;
                    Boolean.TryParse(xmlNode["Result"].InnerText, out result);

                    String nodeResultText = xmlNode["Result"].InnerText;
                    UIValidationResultType enumType = nodeResultText.ParseEnumbyCode<UIValidationResultType>();

                    if (enumType == UIValidationResultType.False)
                    {
                        Int32 ruleMappingID;
                        var itemId = 0;
                        if (Int32.TryParse(xmlNode["RuleMappingID"].InnerText, out ruleMappingID)
                            && ruleMappingID > 0)
                        {
                            var objectsRule = lstObjectsRules.FirstOrDefault(oR => oR.RuleMappingID == ruleMappingID);
                            itemId = lstRequirementVerificationItemData.Where(rvid => rvid.lstFieldData
                            .Any(fd => fd.FieldId == objectsRule.ObjectId)
                            || rvid.FileUploadAttributeId == objectsRule.ObjectId)
                            .Select(rvid => rvid.ItemId)
                            .FirstOrDefault();
                        }

                        if (!dicResponse.ContainsKey(itemId))
                        {
                            dicResponse.Add(itemId, "");
                        }

                        dicResponse[itemId] += xmlNode["ErrorMessage"].InnerText + "<br />";
                        statusCode = AppConsts.ONE;
                    }
                    else if (enumType == UIValidationResultType.Error)
                    {
                        _errorCount++;
                        statusCode = AppConsts.ONE;
                    }
                }
            }
            #endregion

            //foreach (XmlNode xmlNode in nodeList)
            //{
            //    if (xmlNode.IsNotNull())
            //    {
            //        Boolean result;
            //        Boolean.TryParse(xmlNode["Result"].InnerText, out result);

            //        String nodeResultText = xmlNode["Result"].InnerText;
            //        UIValidationResultType enumType = nodeResultText.ParseEnumbyCode<UIValidationResultType>();

            //        if (enumType == UIValidationResultType.False)
            //        {
            //            sbErrors.Append(xmlNode["ErrorMessage"].InnerText + "<br />");
            //            statusCode = AppConsts.ONE;
            //        }
            //        else if (enumType == UIValidationResultType.Error)
            //        {
            //            _errorCount++;
            //            statusCode = AppConsts.ONE;
            //        }
            //    }
            //}

            if (_errorCount > 0)
            {
                if (!dicResponse.ContainsKey(0))
                {
                    dicResponse.Add(0, "");
                    dicResponse[0] = String.Format("<br /> Some error(s) has occured and {0} rule(s) could not be validated. Please make sure that you have entered valid data.", _errorCount);
                }
            }

            return dicResponse;
            #endregion
        }

        public Dictionary<Boolean, String> ValidateDocumentRules(RequirementVerificationCategoryData applicantReqCatData, List<lkpObjectType> lstObjectTypes
                                                                  , Int32 packageSubscriptionId)
        {
            #region GENERATE XML OF APPLICANT DATA ENTRY FORM OBJECTS AND GET OBJECT RULES BY EXECUTING STORED PROCEDURE usp_Rule_GetObjectsRules
            List<RequirementVerificationFieldData> lstApplicantFieldData = new List<RequirementVerificationFieldData>();
            XmlDocument xmlObjectsRules = new XmlDocument();
            XmlElement elementObjectRules = (XmlElement)xmlObjectsRules.AppendChild(xmlObjectsRules.CreateElement("RuleObjects"));

            //added extra nodes for hierarchy for category
            XmlNode expCategoryNode = elementObjectRules.AppendChild(xmlObjectsRules.CreateElement("RuleObject"));
            expCategoryNode.AppendChild(xmlObjectsRules.CreateElement("TypeId")).InnerText = Convert.ToString(GetObjectTypeId(LCObjectType.ComplianceCategory.GetStringValue(), lstObjectTypes));
            expCategoryNode.AppendChild(xmlObjectsRules.CreateElement("Id")).InnerText = Convert.ToString(applicantReqCatData.CatId);
            expCategoryNode.AppendChild(xmlObjectsRules.CreateElement("ParentId")).InnerText = Convert.ToString(AppConsts.NONE);


            foreach (RequirementVerificationItemData applicantReqItemData in applicantReqCatData.lstItemData)
            {
                XmlNode expItemNode = elementObjectRules.AppendChild(xmlObjectsRules.CreateElement("RuleObject"));
                expItemNode.AppendChild(xmlObjectsRules.CreateElement("TypeId")).InnerText = Convert.ToString(GetObjectTypeId(LCObjectType.ComplianceItem.GetStringValue(), lstObjectTypes));
                expItemNode.AppendChild(xmlObjectsRules.CreateElement("Id")).InnerText = Convert.ToString(applicantReqItemData.ItemId);
                expItemNode.AppendChild(xmlObjectsRules.CreateElement("ParentId")).InnerText = Convert.ToString(applicantReqCatData.CatId);

                foreach (RequirementVerificationFieldData applicantField in applicantReqItemData.lstFieldData)
                {
                    lstApplicantFieldData.Add(applicantField);
                    XmlNode exp = elementObjectRules.AppendChild(xmlObjectsRules.CreateElement("RuleObject"));
                    exp.AppendChild(xmlObjectsRules.CreateElement("TypeId")).InnerText = Convert.ToString(GetObjectTypeId(LCObjectType.ComplianceATR.GetStringValue(), lstObjectTypes));
                    exp.AppendChild(xmlObjectsRules.CreateElement("Id")).InnerText = Convert.ToString(applicantField.FieldId);
                    exp.AppendChild(xmlObjectsRules.CreateElement("ParentId")).InnerText = Convert.ToString(applicantReqItemData.ItemId);
                }
            }

            Int32 _ruleTypeId = GetRuleTypeId(ComplianceRuleType.UIRules.GetStringValue());

            Int32 applicantId = ClientDBContext.RequirementPackageSubscriptions.FirstOrDefault(cond => cond.RPS_ID == packageSubscriptionId).RPS_ApplicantOrgUserID;
            List<GetRequirementObjectsRules> lstObjectsRules = _dbContext.usp_Rule_GetRequirementObjectsRules(packageSubscriptionId, _ruleTypeId, Convert.ToString(xmlObjectsRules.OuterXml)).ToList();

            XmlDocument xmlRules = new XmlDocument();
            XmlElement elementRules = (XmlElement)xmlRules.AppendChild(xmlRules.CreateElement("Rules"));

            Int32 initialRuleMappingId = 0;
            XmlElement elementMappings = null;

            foreach (var objectRule in lstObjectsRules)
            {
                if (initialRuleMappingId != objectRule.RuleMappingID)
                {
                    XmlElement elementRule = (XmlElement)elementRules.AppendChild(xmlRules.CreateElement("Rule"));
                    elementRule.AppendChild(xmlRules.CreateElement("Id")).InnerText = Convert.ToString(objectRule.RuleMappingID);
                    XmlElement elementObjectMapping = (XmlElement)elementRule.AppendChild(xmlRules.CreateElement("ObjectMapping"));
                    elementMappings = (XmlElement)elementObjectMapping.AppendChild(xmlRules.CreateElement("Mappings"));
                    initialRuleMappingId = objectRule.RuleMappingID;
                }

                XmlNode nodeRules = elementMappings.AppendChild(xmlRules.CreateElement("Mapping"));
                nodeRules.AppendChild(xmlRules.CreateElement("Key")).InnerText = objectRule.PlaceHolder;

                RequirementVerificationFieldData fieldData = lstApplicantFieldData.FirstOrDefault(sel => sel.FieldId == objectRule.ObjectId);
                if (!String.IsNullOrEmpty(Convert.ToString(objectRule.RLMD_ConstantValue)))
                    nodeRules.AppendChild(xmlRules.CreateElement("Value")).InnerText = Convert.ToString(objectRule.RLMD_ConstantValue);
                else
                    nodeRules.AppendChild(xmlRules.CreateElement("Value")).InnerText = fieldData.IsNullOrEmpty() ? objectRule.FieldValue : fieldData.ApplicantFieldDataValue;
            }

            String resultXML = _dbContext.usp_Rule_EvaluateRequirementPreSubmitRules(xmlRules.OuterXml).FirstOrDefault();

            Dictionary<Boolean, String> dicResponse = new Dictionary<Boolean, String>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(resultXML);
            XmlNodeList nodeList = xml.SelectNodes("Results/Result");
            StringBuilder sbErrors = new StringBuilder();

            Int32 _errorCount = 0;
            Int32 statusCode = AppConsts.NONE;

            foreach (XmlNode xmlNode in nodeList)
            {
                if (xmlNode.IsNotNull())
                {
                    Boolean result;
                    Boolean.TryParse(xmlNode["Result"].InnerText, out result);

                    String nodeResultText = xmlNode["Result"].InnerText;
                    UIValidationResultType enumType = nodeResultText.ParseEnumbyCode<UIValidationResultType>();

                    if (enumType == UIValidationResultType.False)
                    {
                        sbErrors.Append(xmlNode["ErrorMessage"].InnerText + "<br />");
                        statusCode = AppConsts.ONE;
                    }
                    else if (enumType == UIValidationResultType.Error)
                    {
                        _errorCount++;
                        statusCode = AppConsts.ONE;
                    }
                }
            }

            if (_errorCount > 0)
                sbErrors.Append(String.Format("<br /> Some error(s) has occured and {0} rule(s) could not be validated. Please make sure that you have entered valid data.", _errorCount));

            dicResponse.Add(Convert.ToBoolean(statusCode), sbErrors.ToString());
            return dicResponse;
            #endregion
        }

        private Int32 GetObjectTypeId(String objectTypeCode, List<lkpObjectType> lstObjectTypes)
        {
            return lstObjectTypes.Where(oType => oType.OT_Code.ToLower() == objectTypeCode.ToLower()).FirstOrDefault().OT_ID;
        }

        private Int32 GetRuleTypeId(String ruleTypeCode)
        {
            return _dbContext.lkpRuleTypes.Where(rType => rType.RLT_Code.ToLower().Trim() == ruleTypeCode.ToLower().Trim()).FirstOrDefault().RLT_ID;
        }


        public SystemEntityUserPermission GetSystemEntityUserPermission(int clientOrganisationUserID)
        {
            var _systemEntityUserPermission = new SystemEntityUserPermission();

            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@OrganisationUserID", clientOrganisationUserID),
                        };

                base.OpenSQLDataReaderConnection(_sqlConnection);

                using (SqlDataReader _dr = base.ExecuteSQLDataReader(_sqlConnection, "usp_GetSystemEntityUserPermissionRequirementPackage", sqlParameterCollection))
                {
                    if (_dr.HasRows)
                    {
                        while (_dr.Read())
                        {
                            _systemEntityUserPermission = new SystemEntityUserPermission
                            {
                                SEP_OrganisationUserId = Convert.ToInt32(_dr["SEUP_OrganisationUserId"]),
                                SEP_EntityId = Convert.ToInt32(_dr["SEP_EntityId"]),
                                SEP_PermissionName = Convert.ToString(_dr["SEP_PermissionName"]),
                                SE_Name = Convert.ToString(_dr["SE_Name"]),
                                SE_CODE = Convert.ToString(_dr["SE_CODE"])
                            };
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(_sqlConnection);
            }
            return _systemEntityUserPermission;
        }

        #region UAT-2975: ADB Admin All Client Rotation Assignment and User work Queues.
        public Boolean SyncRequirementVerificationToFlatData(String packageSubscriptionsIDs, Int32 currentUserId)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_syncRequirementVerificationDataToFlatTable", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PackageSubscriptionIDs", packageSubscriptionsIDs);
                command.Parameters.AddWithValue("@CurrentLoggedInUSerID", currentUserId);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                command.ExecuteNonQuery();
                con.Close();
                return true;
            }
        }
        #endregion

        #region UAT-3957
        List<RequirementItemRejectionContract> IRequirementVerificationRepository.GetRequirementRejectedItemDetailsForMail(String rejectedItemIds)
        {
            List<RequirementItemRejectionContract> lstRequirementItemRejectionContract = new List<RequirementItemRejectionContract>();

            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetDataForRequirementItemRejectedMail", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RejectedItemDataIds", rejectedItemIds);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lstRequirementItemRejectionContract = ds.Tables[0].AsEnumerable().Select(x =>
                      new RequirementItemRejectionContract
                      {
                          ReqPkgSubscriptionID = x["ReqPkgSubscriptionID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["ReqPkgSubscriptionID"]),
                          ApplicantName = x["ApplicantName"] == DBNull.Value ? String.Empty : Convert.ToString(x["ApplicantName"]),
                          ApplicnatEmailAddress = x["ApplicantEmail"] == DBNull.Value ? String.Empty : Convert.ToString(x["ApplicantEmail"]),
                          ApplicantOrganizationUserId = x["OrganizationUserId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(x["OrganizationUserId"]),
                          RequirementItemName = x["RequirementItemName"] == DBNull.Value ? String.Empty : Convert.ToString(x["RequirementItemName"]),
                          RequirementCategoryName = x["RequirementCategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(x["RequirementCategoryName"]),
                          ItemRejectionReason = x["ItemRejectionReason"] == DBNull.Value ? String.Empty : Convert.ToString(x["ItemRejectionReason"]),
                          AgencyName = x["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(x["AgencyName"]),
                          RotationHierachyIds = x["RotationHierachyIds"] == DBNull.Value ? String.Empty : Convert.ToString(x["RotationHierachyIds"])
                      }).ToList();
                }
            }
            return lstRequirementItemRejectionContract;
        }

        #endregion


    }
}
