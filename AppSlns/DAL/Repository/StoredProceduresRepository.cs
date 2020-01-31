using DAL.Interfaces;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using System.Linq;

namespace DAL.Repository
{
    public class StoredProceduresRepository : ClientBaseRepository, IStoredProcedures
    {
        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public StoredProceduresRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }

        public DataSet GetDocumentMappings(String applicantComplianceAttributeIdsXML, String applicantComplianceItemIdsXML)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@ApplicantComplianceItemIds", applicantComplianceItemIdsXML),
                           new SqlParameter("@ApplicantComplianceAttributeIds", applicantComplianceAttributeIdsXML)
                        };
                DataSet _ds = new DataSet();

                return this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetApplicantVerificationDocuments", _ds, sqlParameterCollection);
            }
        }

        public DataSet GetUpdatedDocumentMappings(Int32 applicantComplianceItemId, Boolean isExceptionType)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@ApplicantComplianceItemId", applicantComplianceItemId),
                           new SqlParameter("@IsExceptionType", isExceptionType)
                        };
                DataSet _ds = new DataSet();

                return this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetApplicantVerificationDocumentsByItemId", _ds, sqlParameterCollection);
            }
        }

        /// <summary>
        /// Gets the RuleMappiung details like RuleSetId, AssignmentHierarchy, based on all the RuleMappingIds
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public DataSet GetRuleMappingDetailsByIds(List<Int32> lstRuleMappingIds)
        {
            XmlDocument xmlRules = new XmlDocument();
            XmlElement elementRuleMappingIds = (XmlElement)xmlRules.AppendChild(xmlRules.CreateElement("RuleMappingIds"));

            foreach (var mappingId in lstRuleMappingIds)
            {
                elementRuleMappingIds.AppendChild(xmlRules.CreateElement("RuleMappingId")).InnerText = Convert.ToString(mappingId);
            }

            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@RuleMappingIds", xmlRules.InnerXml)
                        };
                DataSet _ds = new DataSet();

                return this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetRuleMappingDetailsByIds", _ds, sqlParameterCollection);
            }
        }

        public DataTable GetNodeTemplatesByQuery(String query)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@Query", query), 
                        };
                DataSet _ds = new DataSet();
                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetDataByQuery", _ds, sqlParameterCollection);
                if (_ds.Tables.Count > 0)
                    return _ds.Tables[0];
                else
                    return new DataTable();
            }
        }

        #region Stored Procedures - Applicant Order Flow

        /// <summary>
        /// Common method, used to convert the 
        /// 1. OrganizationUserProfile 
        /// 2. List of Aliases 
        /// 3. List of Residential History 
        /// related XML's into DataTables
        /// </summary>
        /// <param name="inputXML"></param>
        /// <param name="organizationUserProfileId"></param>
        /// <param name="storedProcedureName"></param>
        /// <returns></returns>
        public DataTable ConvertXMLToAttribute(String inputXML, String storedProcedureName)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@OrganizationUserProfileID",  Convert.ToInt32(AppConsts.NONE)), // This conversion is required as compiler treats 0 as NULLABLE
                           new SqlParameter("@InputXML", inputXML)
                        };
                DataSet _ds = new DataSet();
                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, storedProcedureName, _ds, sqlParameterCollection);
                if (_ds.Tables.Count > 0)
                    return _ds.Tables[0];
                else
                    return new DataTable();
            }
        }

        /// <summary>
        /// Execute the Pricing stored procedure
        /// </summary>
        /// <param name="xml"></param>
        public String GetPricingData(String inputXML)
        {
            using ( SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@OrderInput", inputXML)
                            ,new SqlParameter("@ResponseXML",SqlDbType.Xml)
                            ,
                        };

                String _outputXML = String.Empty;
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = _sqlConnection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "ams.usp_GenerateOrderLineItems";

                    if (sqlParameterCollection != null)
                    {
                        sqlCommand.Parameters.AddRange(sqlParameterCollection);
                    }
                    sqlCommand.Parameters["@ResponseXML"].Direction = ParameterDirection.Output;
                    if (_sqlConnection.State == ConnectionState.Closed)
                        _sqlConnection.Open();
                    using (XmlReader reader = sqlCommand.ExecuteXmlReader())
                    {
                        while (reader.Read())
                        {
                            _outputXML = reader.ReadOuterXml();
                        }
                    }
                    if (_sqlConnection.State == ConnectionState.Open)
                        _sqlConnection.Close();
                }
                return _outputXML;
            }
        }

        /// <summary>
        /// To get the Order Line Items
        /// </summary>
        /// <param name="inputXM"></param>
        /// <returns></returns>

        public DataSet GetOrderLineItems(String inputXM)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@OrderInput", inputXM)
                        };
                DataSet _ds = new DataSet();

                return this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "ams.usp_GetOrderLineItems", _ds, sqlParameterCollection);
            }
        }

        public DataSet GetSavedOrderLineItems(Int32 OrderId)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@OrderId", OrderId)
                        };
                DataSet _ds = new DataSet();

                return this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "ams.usp_GetSavedOrderLineItems", _ds, sqlParameterCollection);
            }
        }

        /// <summary>
        /// Updates the External Service and Vendor Id for the linte items, after order is successfully placed.
        /// </summary>
        /// <param name="masterOrderId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public void UpdateExtServiceVendorforLineItems(Int32 masterOrderId, Int32 tenantId)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@TenantId",  tenantId), 
                           new SqlParameter("@OrderId", masterOrderId)
                        };

                using (SqlCommand _sqlCommand = new SqlCommand())
                {
                    _sqlCommand.Connection = _sqlConnection;
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandText = "ams.usp_UpdateBkgOrderPackageSvcLineItem";

                    if (sqlParameterCollection != null)
                    {
                        _sqlCommand.Parameters.AddRange(sqlParameterCollection);
                    }

                    if (_sqlConnection.State == ConnectionState.Closed)
                        _sqlConnection.Open();

                    _sqlCommand.ExecuteNonQuery();

                    if (_sqlConnection.State == ConnectionState.Open)
                        _sqlConnection.Close();
                }
            }
        }

        /// <summary>
        /// Get the Payment Options for the PAckages, at Package Level 
        /// and Also at the Node Level
        /// </summary>
        /// <param name="dppId"></param>
        /// <param name="dppsId"></param>
        /// <param name="bphmIds"></param>
        /// <param name="dpmId"></param>
        /// <returns></returns>
        /// 
        public DataSet GetPaymentOptions(int dppId, String bphmIds, Int32 dpmId)
        {
            return GetPaymentOptions(dppId.ToString(), bphmIds, dpmId);
        }
        public DataSet GetPaymentOptions(string dppIds, String bphmIds, Int32 dpmId)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@DPPId", dppIds), 
                           new SqlParameter("@BPHMIds", bphmIds),
                           new SqlParameter("@DPMId", dpmId)
                        };
                DataSet _ds = new DataSet();

                return this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetPkgPaymentOptions", _ds, sqlParameterCollection);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Generic method to call any stored procedure
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="resultDataSet"></param>
        /// <param name="sqlParameterCollection"></param>
        /// <returns></returns>
        private DataSet GetDataSet(SqlConnection connection, CommandType commandType, String commandText, DataSet resultDataSet, SqlParameter[] sqlParameterCollection = null)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = connection;
                sqlCommand.CommandType = commandType;
                sqlCommand.CommandText = commandText;

                if (sqlParameterCollection != null)
                {
                    sqlCommand.Parameters.AddRange(sqlParameterCollection);
                }
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(resultDataSet);
            } return resultDataSet;
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

        #region Order Processing

        public DataSet GetBackroundOrderSearchDetail(CustomPagingArgsContract gridCustomPaging, BkgOrderSearchContract bkgOrderSearchContract)
        {
            String storedProcedureName = "[ams].[usp_BackroundOrderSearch]";
            string orderBy = QueueConstants.ORDER_QUEUE_DEFAULT_SORTING_FIELDS;
            string ordDirection = null;

            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? "desc" : "asc" : "desc";

            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@OrderID",  bkgOrderSearchContract.OrderNumber),
                           new SqlParameter("@OrderFromDate",  bkgOrderSearchContract.OrderFromDate),
                           new SqlParameter("@OrderToDate",  bkgOrderSearchContract.OrderToDate),
                           new SqlParameter("@PaidFromDate",  bkgOrderSearchContract.PaidFromDate),
                           new SqlParameter("@PaidToDate",  bkgOrderSearchContract.PaidToDate),
                           new SqlParameter("@ApplicantFirstName",  bkgOrderSearchContract.ApplicantFirstName),
                           new SqlParameter("@ApplicantLastName",  bkgOrderSearchContract.ApplicantLastName),
                           new SqlParameter("@SSN",  bkgOrderSearchContract.ApplicantSSN),
                           new SqlParameter("@DOB",  bkgOrderSearchContract.DateOfBirth),
                           new SqlParameter("@OrderPaymentStatusID",  bkgOrderSearchContract.OrderPaymentStatusID),
                           new SqlParameter("@OrderStatusTypeID",  bkgOrderSearchContract.OrderStatusTypeID),
                           new SqlParameter("@ServiceID",  bkgOrderSearchContract.ServiceID),
                           new SqlParameter("@UserGroupID",  bkgOrderSearchContract.UserGroupID),
                           new SqlParameter("@InstitutionStatusColorID",  bkgOrderSearchContract.InstitutionStatusColorID),
                           new SqlParameter("@BkgServiceGroupID",  bkgOrderSearchContract.ServiceGroupId),
                           new SqlParameter("@IsArchived",  bkgOrderSearchContract.IsArchive),
                           new SqlParameter("@OrderClientStatusID",  bkgOrderSearchContract.OrderClientStatusID),
                           new SqlParameter("@IsServiceGroupRequired",  bkgOrderSearchContract.IsServiceGroupRequired),  
                           new SqlParameter("@IsFlagged",  bkgOrderSearchContract.IsFlagged), 
                           new SqlParameter("@OrderCompletedFromDate", bkgOrderSearchContract.OrderCompletedFromDate), 
                           new SqlParameter("@OrderCompletedToDate", bkgOrderSearchContract.OrderCompletedToDate), 
                           new SqlParameter("@ServiceFormStatusTypeID", bkgOrderSearchContract.ServiceFormStatusID), 
                           //new SqlParameter("@TargetHierarchyNodeId", bkgOrderSearchContract.DeptProgramMappingID), 
                           new SqlParameter("@TargetHierarchyNodeIds", bkgOrderSearchContract.DeptProgramMappingIDs), 
                           new SqlParameter("@CustomFields", bkgOrderSearchContract.CustomFields), 
                           new SqlParameter("@CurrentLoggedInUserId", bkgOrderSearchContract.LoggedInUserId), 
                           new SqlParameter("@ArchivedState",bkgOrderSearchContract.SelectedArchieveStateId),
                           new SqlParameter("@OrderBy",  orderBy),                                                                         
                           new SqlParameter("@OrderDirection",  ordDirection),
                           new SqlParameter("@PageIndex",  gridCustomPaging.CurrentPageIndex),
                           new SqlParameter("@PageSize",  gridCustomPaging.PageSize)                                                                         
                        };
                DataSet _ds = new DataSet();
                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, storedProcedureName, _ds, sqlParameterCollection);
                if (_ds.Tables.Count > 0)
                    return _ds;
                else
                    return new DataSet();
            }

        }

        #region UAT-1683

        String IStoredProcedures.ArchieveBkgOrder(short archieveStateID, Int32 currentUserId, List<BkgOrder> lstBkgOrderToBeArchieved, List<BkgOrderArchiveHistory> lstsubscriptionArchiveHistoryData)
        {
            //List<BkgOrder> lstBkgOrderToBeArchieved = GetBkgOrderToBeArchived(selectedOrderIds, archieveStateID);

            foreach (BkgOrder order in lstBkgOrderToBeArchieved)
            {
                order.BOR_ArchiveStateID = archieveStateID;
                order.BOR_LastArchivedDate = DateTime.Now;
                order.BOR_ModifiedByID = currentUserId;
                order.BOR_ModifiedOn = DateTime.Now;
            }

            foreach (BkgOrderArchiveHistory archiveHistoryItem in lstsubscriptionArchiveHistoryData)
            {
                //Get all CompliancePackageSubscriptionArchiveHistory based on the UnArchivalRequestId list
                List<BkgOrderArchiveHistory> lstOrderArchiveHistory = _dbContext.BkgOrderArchiveHistories.Where(x => x.BOAH_BkgOrderID == archiveHistoryItem.BOAH_BkgOrderID && !x.BOAH_IsDeleted).ToList();

                foreach (BkgOrderArchiveHistory archiveHistory in lstOrderArchiveHistory)
                {
                    //InActive row from CompliancePackageSubscriptionArchiveHistory
                    archiveHistory.BOAH_IsActive = false;
                    archiveHistory.BOAH_ModifiedBy = currentUserId;
                    archiveHistory.BOAH_ModifiedOn = DateTime.Now;
                }

                if (archiveHistoryItem.IsNotNull())
                {
                    _dbContext.BkgOrderArchiveHistories.AddObject(archiveHistoryItem);
                }
            }
            if (_dbContext.SaveChanges() > 0)
                return "true";
            return "false";
        }

        public List<BkgOrder> GetBkgOrderToBeArchived(List<Int32> orderIDs, List<Int32> archieveStatusIds)
        {
            //return _dbContext.BkgOrders.Where(x => orderIDs.Contains(x.BOR_MasterOrderID) && (x.BOR_ArchiveStateID ?? 0) != archieveStatusId && !x.BOR_IsDeleted).ToList();
            return _dbContext.BkgOrders.Where(x => orderIDs.Contains(x.BOR_MasterOrderID) && !archieveStatusIds.Contains(x.BOR_ArchiveStateID ?? 0) && !x.BOR_IsDeleted).ToList();
        }

        #region UAT-4085
        public List<BkgOrder> GetBkgOrderToBeUnArchived(List<Int32> orderIDs, List<Int32> archieveStatusIds)
        {
            //return _dbContext.BkgOrders.Where(x => orderIDs.Contains(x.BOR_MasterOrderID) && (x.BOR_ArchiveStateID ?? 0) != archieveStatusId && !x.BOR_IsDeleted).ToList();
            return _dbContext.BkgOrders.Where(x => orderIDs.Contains(x.BOR_MasterOrderID) && !archieveStatusIds.Contains(x.BOR_ArchiveStateID ?? 0) && !x.BOR_IsDeleted).ToList();
        }

        String IStoredProcedures.UnArchieveBkgOrder(short archieveStateID, Int32 currentUserId, List<BkgOrder> lstBkgOrderToBeUnArchived, List<BkgOrderArchiveHistory> lstsubscriptionArchiveHistoryData)
        {
            foreach (BkgOrder order in lstBkgOrderToBeUnArchived)
            {
                order.BOR_ArchiveStateID = archieveStateID;
                order.BOR_LastArchivedDate = DateTime.Now;
                order.BOR_ModifiedByID = currentUserId;
                order.BOR_ModifiedOn = DateTime.Now;
            }

            foreach (BkgOrderArchiveHistory archiveHistoryItem in lstsubscriptionArchiveHistoryData)
            {
                //Get all CompliancePackageSubscriptionArchiveHistory based on the UnArchivalRequestId list
                List<BkgOrderArchiveHistory> lstOrderArchiveHistory = _dbContext.BkgOrderArchiveHistories.Where(x => x.BOAH_BkgOrderID == archiveHistoryItem.BOAH_BkgOrderID && !x.BOAH_IsDeleted).ToList();

                foreach (BkgOrderArchiveHistory archiveHistory in lstOrderArchiveHistory)
                {
                    //InActive row from CompliancePackageSubscriptionArchiveHistory
                    archiveHistory.BOAH_IsActive = false;
                    archiveHistory.BOAH_ModifiedBy = currentUserId;
                    archiveHistory.BOAH_ModifiedOn = DateTime.Now;
                }

                if (archiveHistoryItem.IsNotNull())
                {
                    _dbContext.BkgOrderArchiveHistories.AddObject(archiveHistoryItem);
                }
            }
            if (_dbContext.SaveChanges() > 0)
                return "true";
            return "false";
        }


        #endregion
        #endregion
        #endregion

        #region Supplement Order

        /// <summary>
        /// Get the output XML from the Supplement order stored procedure
        /// </summary>
        /// <param name="inputXML"></param>
        /// <returns></returns>
        public String GetSupplementOrderPricingData(String inputXML)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                             new SqlParameter("@SupOrderXMl", inputXML)
                        };

                String _outputXML = String.Empty;
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = _sqlConnection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "ams.usp_GenerateSupplementOrderLineItems";

                    if (sqlParameterCollection != null)
                    {
                        sqlCommand.Parameters.AddRange(sqlParameterCollection);
                    }
                    if (_sqlConnection.State == ConnectionState.Closed)
                        _sqlConnection.Open();
                    using (XmlReader reader = sqlCommand.ExecuteXmlReader())
                    {
                        while (reader.Read())
                        {
                            _outputXML = reader.ReadOuterXml();
                        }
                    }
                    if (_sqlConnection.State == ConnectionState.Open)
                        _sqlConnection.Close();
                }
                return _outputXML;
            }
        }


        /// <summary>
        /// Gets Applicant details, related to the Current Background Order, for SupplementOrder flow
        /// </summary>
        public DataTable GetApplicantData(Int32 masterOrderId)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@MasterOrderId", masterOrderId), 
                        };
                DataSet _ds = new DataSet();
                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "ams.usp_GetApplicantInformation", _ds, sqlParameterCollection);
                if (_ds.Tables.Count > 0)
                    return _ds.Tables[0];
                else
                    return new DataTable();
            }
        }

        /// <summary>
        /// Gets Applicant Residential Histories & Personal Alias to display in Supplement Order, added during normal order
        /// </summary>
        public DataSet GetApplicantBkgOrderDeta(Int32 masterOrderId)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@MasterOrderId", masterOrderId), 
                        };
                DataSet _ds = new DataSet();
                return this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "ams.usp_GetApplicantBkgOrderData", _ds, sqlParameterCollection);
            }
        }

        #endregion

        #region Manual Service Forms

        public DataSet GetManualServiceFormsSearchDetail(CustomPagingArgsContract gridCustomPaging, ManualServiceFormsSearchContract manualServiceFormsSearchContract)
        {
            String storedProcedureName = "[ams].[usp_GetManualServiceForms]";
            string orderBy = QueueConstants.ORDER_QUEUE_DEFAULT_SORTING_FIELDS;
            string ordDirection = null;

            orderBy = String.IsNullOrEmpty(gridCustomPaging.SortExpression) ? orderBy : gridCustomPaging.SortExpression;
            ordDirection = gridCustomPaging.SortDirectionDescending == false ? "asc" : "desc";

            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@ApplicantFirstName",  manualServiceFormsSearchContract.ApplicantFirstName),
                           new SqlParameter("@ApplicantLastName",  manualServiceFormsSearchContract.ApplicantLastName),
                           new SqlParameter("@ServiceID",  manualServiceFormsSearchContract.ServiceID),
                           //new SqlParameter("@DeptProgramMappingID", manualServiceFormsSearchContract.DeptProgramMappingID),
                           new SqlParameter("@DelemittedDeptProgramMappingIDs", manualServiceFormsSearchContract.SelectedDeptProgramMappingID),
                           new SqlParameter("@ServiceFormStatusTypeID", manualServiceFormsSearchContract.ServiceFormStatusID),
                           new SqlParameter("@LoggedInUserId", manualServiceFormsSearchContract.LoggedInUserId),
                           new SqlParameter("@OrderBy",  orderBy),                                                                         
                           new SqlParameter("@OrderDirection",  ordDirection),
                           new SqlParameter("@PageIndex",  gridCustomPaging.CurrentPageIndex),
                           new SqlParameter("@PageSize",  gridCustomPaging.PageSize)                                                                          
                        };
                DataSet _ds = new DataSet();
                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, storedProcedureName, _ds, sqlParameterCollection);
                if (_ds.Tables.Count > 0)
                    return _ds;
                else
                    return new DataSet();
            }

        }
        #endregion

        #region  Applicant Hierarchy Mapping
        public List<ApplicantInstitutionHierarchyMapping> GetApplicantInstitutionHierarchyMapping(String organizationUserIDs)
        {
            String storedProcedureName = "[ams].[usp_GetApplicantHierarchyMapping]";

            List<ApplicantInstitutionHierarchyMapping> lstApplicantInstitutionHierarchyMapping = new List<ApplicantInstitutionHierarchyMapping>();
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@OrganizationUserIDs",  organizationUserIDs),                                                                                           
                        };
                DataSet _ds = new DataSet();
                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, storedProcedureName, _ds, sqlParameterCollection);
                if (_ds.Tables.Count > 0)
                {
                    foreach (DataRow dRow in _ds.Tables[0].Rows)
                    {
                        lstApplicantInstitutionHierarchyMapping.Add(new ApplicantInstitutionHierarchyMapping
                            {
                                DPM_ID = dRow["DPMID"] != DBNull.Value ? Convert.ToInt32(dRow["DPMID"]) : 0,
                                InstitutionNode_ID = dRow["InstitutionNodeID"] != DBNull.Value ? Convert.ToInt32(dRow["InstitutionNodeID"]) : 0,
                                InstitutionHierarchyLabel = dRow["InstitutionHierarchyLabel"] != DBNull.Value ? Convert.ToString(dRow["InstitutionHierarchyLabel"]) : String.Empty,
                                RecordID = dRow["RecordID"] != DBNull.Value ? Convert.ToInt32(dRow["RecordID"]) : (Int32?)null,
                                OrganisationUserId = Convert.ToInt32(dRow["OrganizationUserID"])
                            });
                    }
                }
                return lstApplicantInstitutionHierarchyMapping;
            }
        }
        #endregion

        #region Evaluate Post Submit Rules For Multi

        public void EvaluatePostSubmitRulesForMulti(String ruleXml, Int32 userID)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@InputXML", ruleXml),
                           new SqlParameter("@SystemUserID", userID)
                        };
                DataSet _ds = new DataSet();

                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "dbo.usp_Rule_EvaluatePostRuleMulti", _ds, sqlParameterCollection);
            }
        }
        #endregion

        #region Sales Force Service

        /// <summary>
        ///  Gets the list of Admins/Client admins and the Nodes on which they have permissions
        /// </summary>
        /// <param name="xml"></param>
        public DataTable GetPermissionsSubscriptionSettings(String subEventCode, String serviceType, Int32 tenantId)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@TenantId",  tenantId) ,
                           new SqlParameter("@SubEventCode",  subEventCode) ,
                           new SqlParameter("@ServiceType",  serviceType) 
                        };
                DataSet _ds = new DataSet();
                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "ams.usp_GetPermissionsSubscriptionSettings", _ds, sqlParameterCollection);
                if (_ds.Tables.Count > 0)
                    return _ds.Tables[0];
                else
                    return new DataTable();
            }
        }

        /// <summary>
        /// Gets the ComplianceData to be uploaded to Sales Force
        /// </summary>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public DataSet GetComplianceDataToUpload(Int32 chunkSize)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@ChunkSize",  chunkSize)
                        };

                DataSet _ds = new DataSet();
                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetComplianceDataToUpload", _ds, sqlParameterCollection);
                if (_ds.Tables.Count > 0)
                    return _ds;
                else
                    return new DataSet();
            }
        }

        /// <summary>
        /// Save the history for the execution of the service to uplaod the data to Sales Force
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public void SaveComplianceUploadServiceHistory(String request, String response)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@Request",  request), 
                           new SqlParameter("@Response", response)
                        };

                using (SqlCommand _sqlCommand = new SqlCommand())
                {
                    _sqlCommand.Connection = _sqlConnection;
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandText = "usp_InsertComplianceUploadServiceHistory";

                    if (sqlParameterCollection != null)
                    {
                        _sqlCommand.Parameters.AddRange(sqlParameterCollection);
                    }

                    if (_sqlConnection.State == ConnectionState.Closed)
                        _sqlConnection.Open();

                    _sqlCommand.ExecuteNonQuery();

                    if (_sqlConnection.State == ConnectionState.Open)
                        _sqlConnection.Close();
                }
            }
        }
        /// <summary>
        /// Update the status of the Records that have been either uploaded or Error occured in their upload,
        /// depending on the code passed
        /// </summary>
        /// <param name="tpcduIds">CSV of the TPCDU_ID's</param>
        /// <param name="statusCode"></param>
        public void UpdateThirdPartyComplianceDataUploadStatus(String tpcduIds, String statusCode, Int32 backgroundProcessUserId)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@TPCDUIds",  tpcduIds), 
                           new SqlParameter("@UpdateCode", statusCode),
                            new SqlParameter("@BackgroundProcessUserId", backgroundProcessUserId)
                        };

                using (SqlCommand _sqlCommand = new SqlCommand())
                {
                    _sqlCommand.Connection = _sqlConnection;
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandText = "usp_UpdateThirdPartyComplianceDataUploadStatus";

                    if (sqlParameterCollection != null)
                    {
                        _sqlCommand.Parameters.AddRange(sqlParameterCollection);
                    }

                    if (_sqlConnection.State == ConnectionState.Closed)
                        _sqlConnection.Open();

                    _sqlCommand.ExecuteNonQuery();

                    if (_sqlConnection.State == ConnectionState.Open)
                        _sqlConnection.Close();
                }
            }
        }

        public List<ThirdPartyDataUploadResponseTypeContract> GetThirdPartyDataUploadResponseRegex(Int32 clientDataUploadID)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                  SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@ClientDataUploadID",  clientDataUploadID),                        
                        };
                List<ThirdPartyDataUploadResponseTypeContract> listThirdPartyDataUploadResponse = new List<ThirdPartyDataUploadResponseTypeContract>();
                DataSet _ds = new DataSet();

                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetThirdPartyDataUploadResponse", _ds, sqlParameterCollection);
                if (_ds.Tables.Count > 0)
                {
                    foreach (DataRow dRow in _ds.Tables[0].Rows)
                    {
                        listThirdPartyDataUploadResponse.Add(new ThirdPartyDataUploadResponseTypeContract
                        {
                            ThirdPartyDataUploadResponseID = dRow["ThirdPartyDataUploadResponseID"] != DBNull.Value ? Convert.ToInt32(dRow["ThirdPartyDataUploadResponseID"]) : 0,
                            Regex = dRow["Regex"] != DBNull.Value ? Convert.ToString(dRow["Regex"]) : String.Empty,
                            ThirdPartyUploadOutputTypeCode = dRow["ThirdPartyUploadOutputTypeCode"] != DBNull.Value ? Convert.ToString(dRow["ThirdPartyUploadOutputTypeCode"]) : String.Empty,
                        });
                    }
                }
                return listThirdPartyDataUploadResponse;
            }
        }
        #endregion

        #region Stored Procedures - Compliance Administration

        /// <summary>
        /// Gets the Package level Payment Options, based on the package type i.e. Background or Compliance
        /// </summary>
        /// <param name="pkgNodeMappingId"></param>
        /// <param name="packageTypeCode"></param>
        /// <param name="offlineSettlementCode"></param>
        /// <returns></returns>
        public DataTable GetPackagePaymentOptions(Int32 pkgNodeMappingId, String packageTypeCode, String offlineSettlementCode)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@PackageNodeMappingId", pkgNodeMappingId ), 
                           new SqlParameter("@PackageTypeCode", packageTypeCode),
                           new SqlParameter("@OfflineSettlementCode", offlineSettlementCode )
                        };

                DataSet _ds = new DataSet();

                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetPackagePaymentOptions", _ds, sqlParameterCollection);
                if (_ds.Tables.Count > 0)
                    return _ds.Tables[0];
                else
                    return new DataTable();
            }
        }

        /// <summary>
        /// Updates the Display order of the Nodes of the Hierarchy Tree
        /// </summary>
        /// <param name="lstDPMIds"></param>
        /// <param name="destinationIndex"></param>
        /// <param name="currentUserId"></param>
        public Boolean UpdateNodeDisplayOrder(List<GetChildNodesWithPermission> lstDPMIds, Int32? destinationIndex, Int32 currentUserId)
        {
            DataTable dtNodes = new DataTable();
            dtNodes.Columns.Add("DPMID", typeof(Int32));
            dtNodes.Columns.Add("DestinationIndex", typeof(Int32));
            dtNodes.Columns.Add("CurrentLoggedInUserId", typeof(Int32));
            foreach (var dpmId in lstDPMIds)
            {
                dtNodes.Rows.Add(new object[] { Convert.ToInt32(dpmId.DPM_ID), destinationIndex, currentUserId });
                destinationIndex += 1;
            }

            using (SqlConnection con = new SqlConnection())
            {
                con.ConnectionString = GetConnectionString();
                SqlCommand _command = new SqlCommand("UpdateNodeSequence", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@typeDPM", dtNodes);
                con.Open();
                Int32 rowsAffected = _command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    con.Close();
                    //_dbContext.Refresh(RefreshMode.StoreWins, lstDPMIds);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get the Institution Nodes of the Selected Tenant, for the current user, based on the Permissions
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        DataTable IStoredProcedures.GetInstitutionNodes(Int32? userId)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@UserID", userId ) 
                        };

                DataSet _ds = new DataSet();

                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "Report.usp_Report_Filter_HierarchyList", _ds, sqlParameterCollection);
                if (_ds.Tables.Count > 0)
                    return _ds.Tables[0];
                else
                    return new DataTable();
            }
        }

        #endregion

        #region Stored Procedures - Admin Data Entry

        /// <summary>
        /// Gets the All the Package subscription related meta-data and applicant data 
        /// for the selected package subscription in Admin Data entry details screen
        /// </summary>
        /// <param name="pkgSubscriptionId"></param>
        /// <returns></returns>
        public DataSet GetAdminDataEntrySubscription(Int32 pkgSubscriptionId, Int32 documentId)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@PkgSubId", pkgSubscriptionId),
                           new SqlParameter("@CrntDocId", documentId)
                        };
                DataSet _ds = new DataSet();

                return this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetAdminDataEntrySubscription", _ds, sqlParameterCollection);
            }
        }

        /// <summary>
        /// Gets the Package details upto ApplicantComplianceItemData for the selected Package Subscription.
        /// </summary>
        /// <param name="pkgSubscriptionId"></param>
        /// <returns></returns>
        public DataSet GetPackageDetailsBySubscriptionId(Int32 pkgSubscriptionId)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@PkgSubId", pkgSubscriptionId)
                        };
                DataSet _ds = new DataSet();

                return this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetPackageDetailsBySubscriptionId", _ds, sqlParameterCollection);
            }
        }

        #endregion

        #region Get available compliance and background packages

        /// <summary>
        /// Method to get Compliance and background packages of selected node or its parent nodes.
        /// </summary>
        /// <param name="nodeId">Selected Node Id</param>
        /// <param name="organizationUserId">organizationUserId</param>
        /// <param name="orderPackageTypeCode">Package type for which packages want to get</param>
        /// <returns></returns>
        DataTable IStoredProcedures.GetAvailableCompAndBkgPackages(Int32 nodeId, String orderPackageTypeCode, Boolean isLoadParentPackages = false)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@NodeID", nodeId),
                           new SqlParameter("@PackageTypeCode", orderPackageTypeCode),
                           new SqlParameter("@IsLoadParentPackages", isLoadParentPackages),
                        };
                DataSet _ds = new DataSet();
                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetAvailableCompAndBkgPackages", _ds, sqlParameterCollection);
                if (_ds.Tables.Count > 0)
                    return _ds.Tables[0];
                else
                    return new DataTable();
            }
        }
        #endregion

        #region Profile Sharing

        /// <summary>
        /// Get the compliance and background packages that can be shared by the applicant
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="compliancePkgids">CSV List of Compliance packages selected by admin for sharing</param>
        /// <param name="bkgPkgIds">CSV List of Compliance packages selected by admin for sharing</param>
        /// <param name="IsApplicant"></param>
        /// <returns></returns>
        DataSet IStoredProcedures.GetSharingPackages(Int32 organizationUserId)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@OrgUserId", organizationUserId),   
                        };
                DataSet _ds = new DataSet();
                return this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetProfileSharingPackages", _ds, sqlParameterCollection);
            }
        }


        /// <summary>
        /// Get the Requirement packages that can be shared by the admins, for Rotation sharing
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <param name="rotationId"></param>
        /// <param name="reqPkgIds">CSV of the Requirement Packages selected for Sharing</param>
        /// <returns></returns>
        DataSet IStoredProcedures.GetSharingRequirementPackages(Int32 organizationUserId, Int32 rotationId)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@OrgUserId", organizationUserId), 
                           new SqlParameter("@RotationId", rotationId)
                        };
                DataSet _ds = new DataSet();
                return this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetSharingRequirementPackages", _ds, sqlParameterCollection);
            }
        }

        /// <summary>
        /// Stored procedure to get the address of a User, by Address HandleId
        /// </summary>
        /// <param name="addressHandleId"></param>
        /// <returns></returns>
        DataTable IStoredProcedures.GetAddressByAddressHandleId(Guid addressHandleId)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@AddressHandleId", addressHandleId), 
                        };
                DataSet _ds = new DataSet();
                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetAddressByAddressHandleId", _ds, sqlParameterCollection);
                if (_ds.Tables.Count > 0)
                    return _ds.Tables[0];
                else
                    return new DataTable();
            }
        }

        /// <summary>
        /// Get the Compliance & Background Packages (and their related data) for the
        /// selected applicants, out of which admin can select which category/service group etc can be shared - UAT 1324
        /// </summary>
        /// <param name="organizationUserId"></param>
        /// <returns></returns>
        DataSet IStoredProcedures.GetSharingPackageData(String organizationUserIds)
        {
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@ApplicantOrgUserIds", organizationUserIds)
                        };

                DataSet _ds = new DataSet();
                return this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_GetSharingPackageData", _ds, sqlParameterCollection);
            }
        }

        #endregion

        #region Generic 'Execute' Stored Procedure

        /// <summary>
        ///  Geeneric method to execute any Stored Procedure 
        /// </summary>
        /// <param name="dicParameters"></param>
        /// <param name="procedureName"></param>
        void IStoredProcedures.ExecuteProcedure(Dictionary<String, Object> dicParameters, String procedureName)
        {
            if (dicParameters.IsNull())
            {
                dicParameters = new Dictionary<String, Object>();
            }

            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();


                using (SqlCommand _sqlCommand = new SqlCommand())
                {
                    _sqlCommand.Connection = _sqlConnection;
                    _sqlCommand.CommandType = CommandType.StoredProcedure;
                    _sqlCommand.CommandText = procedureName;

                    foreach (var paremeter in dicParameters)
                    {
                        _sqlCommand.Parameters.Add(new SqlParameter(paremeter.Key, paremeter.Value));
                    }

                    if (_sqlConnection.State == ConnectionState.Closed)
                        _sqlConnection.Open();

                    _sqlCommand.ExecuteNonQuery();

                    if (_sqlConnection.State == ConnectionState.Open)
                        _sqlConnection.Close();
                }
            }
        }

        #endregion

        #region UAT-1843
        List<Int32> IStoredProcedures.GetBkgOrderIdByOrgUsers(List<Int32> orgUserIds,Int32 ArchiveId)
        {
            return _dbContext.BkgOrders.Where(cond => cond.BOR_ArchiveStateID == ArchiveId && !cond.BOR_IsDeleted && !cond.OrganizationUserProfile.IsDeleted
                && orgUserIds.Contains(cond.OrganizationUserProfile.OrganizationUserID)).Select(sel => sel.BOR_MasterOrderID).ToList();
        }
        #endregion

        #region UAT-3077
        public Tuple<Int32, Int32, Int32> ApprovedPaymentItem(Int32 orderId, Int32 currentLoggedInUserId)
        {
           
            using (SqlConnection _sqlConnection = new SqlConnection())
            {
                _sqlConnection.ConnectionString = GetConnectionString();
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@OrderID", orderId),
                           new SqlParameter("@CurrentLoggedInUserID", currentLoggedInUserId)
                        };
                DataSet _ds = new DataSet();

                this.GetDataSet(_sqlConnection, CommandType.StoredProcedure, "usp_ApprovePaymentItem", _ds, sqlParameterCollection);

                Int32 packageId=0;
                Int32 categoryID=0;
                Int32 itemID=0;

                if (!_ds.Tables.IsNullOrEmpty())
                {
                    packageId = Convert.ToInt32(_ds.Tables[0].Rows[0]["PackageID"]);
                    categoryID = Convert.ToInt32(_ds.Tables[0].Rows[0]["CategoryID"]);
                    itemID = Convert.ToInt32(_ds.Tables[0].Rows[0]["ItemID"]);
                }

                return new Tuple<Int32, Int32, Int32>(packageId, categoryID, itemID);
            }

        }
        #endregion

    }
}
