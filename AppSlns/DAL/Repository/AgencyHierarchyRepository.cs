using DAL.Interfaces;
using Entity.ClientEntity;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;



namespace DAL.Repository
{
    public class AgencyHierarchyRepository : ClientBaseRepository, IAgencyHierarchyRepository
    {

        #region Variables
        private ADB_LibertyUniversity_ReviewEntities _dbContext;
        #endregion

        #region Default Constructor to initilize DB Context
        ///// <summary>
        ///// Default constructor to initialize the context
        ///// </summary>
        public AgencyHierarchyRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }



        #endregion

        #region Agency Hierarchy Mapping
        Int32 IAgencyHierarchyRepository.SaveAgencyHierarchyMapping(AgencyNodeMappingContract agencyNodeMappingContract)
        {
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;

            SqlParameter outputParam = new SqlParameter("@ReturnValue", SqlDbType.Int) { Direction = ParameterDirection.Output };
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_AgencyHierarchyAgencyMapping", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyHierarchyId", agencyNodeMappingContract.AgencyHierarchyId);
                command.Parameters.AddWithValue("@AgencyID", agencyNodeMappingContract.AgencyID);
                command.Parameters.AddWithValue("@loggedInUserID", agencyNodeMappingContract.CurrentLoggedInUserID);
                command.Parameters.Add(outputParam);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
            return Convert.ToInt32(outputParam.Value);
        }
        Boolean IAgencyHierarchyRepository.CheckForLeafNode(Int32 HierarchyNodeId)
        {
            Boolean _returnStatus = true;
            var data = SharedDataDBContext.AgencyHierarchies.Where(a => a.AH_ParentID == HierarchyNodeId && a.AH_IsDeleted == false).Count();
            if (data > AppConsts.NONE)
                _returnStatus = false;
            else _returnStatus = true;
            return _returnStatus;
        }
        Boolean IAgencyHierarchyRepository.DeleteAgencyNodeMapping(Int32 HierarchyNodeId, Int32 AgencyId)
        {
            Boolean returnStatus = false;
            AgencyHierarchyAgency agencyHierarchyAgency = SharedDataDBContext.AgencyHierarchyAgencies.Where(a => a.AHA_AgencyHierarchyID == HierarchyNodeId && a.AHA_AgencyID == AgencyId && a.AHA_IsDeleted == false).FirstOrDefault();
            if (agencyHierarchyAgency != null)
            {
                agencyHierarchyAgency.AHA_IsDeleted = true;
                returnStatus = true;
            }
            SharedDataDBContext.SaveChanges();
            return returnStatus;
        }
        AgencyNodeMappingContract IAgencyHierarchyRepository.GetAgencyHierarchyAgencyMapping(Int32 HierarchyNodeId)
        {
            AgencyNodeMappingContract agencyNodeMappingContract = new AgencyNodeMappingContract();
            AgencyHierarchyAgency agencyHierarchyAgency = SharedDataDBContext.AgencyHierarchyAgencies.Where(a => a.AHA_AgencyHierarchyID == HierarchyNodeId && a.AHA_IsDeleted == false).FirstOrDefault();
            if (agencyHierarchyAgency != null)
            {
                agencyNodeMappingContract.AgencyHierarchyId = agencyHierarchyAgency.AHA_AgencyHierarchyID;
                agencyNodeMappingContract.AgencyID = agencyHierarchyAgency.AHA_AgencyID;
                // agencyNodeMappingContract.CreatedBy = agencyHierarchyAgency.AHA_CreatedBy;
            }
            return agencyNodeMappingContract;
        }
        #endregion

        #region Public Method

        #endregion

        #region Common Control
        List<AgencyHierarchyContract> IAgencyHierarchyRepository.GetAgencyHierarchy(String agencyHierarchyNodeIds)
        {
            var _lstAgencyHierarchyContractt = new List<AgencyHierarchyContract>();
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                   {
                           new SqlParameter("@AgencyHierarchyNodeIds", agencyHierarchyNodeIds)

                   };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHierarchyRootNodes", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var agencyReviewData = new AgencyHierarchyContract();
                            agencyReviewData.NodeID = dr["NodeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["NodeID"]);
                            agencyReviewData.ParentNodeID = dr["ParentNodeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ParentNodeID"]);
                            agencyReviewData.Value = dr["Value"] == DBNull.Value ? String.Empty : Convert.ToString(dr["Value"]);
                            agencyReviewData.DisplayOrder = dr["DisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DisplayOrder"]); //UAT-3237
                            agencyReviewData.IsNodeAvailable = dr["IsNodeAvailable"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IsNodeAvailable"]); // UAT-4443
                            _lstAgencyHierarchyContractt.Add(agencyReviewData);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstAgencyHierarchyContractt;
        }
        List<AgencyHierarchyContract> IAgencyHierarchyRepository.GetTreeDataByRootNodeID(Int32 rootNodeID)
        {

            var _lstAgencyHierarchyContractt = new List<AgencyHierarchyContract>();
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                      {
                           new SqlParameter("@RootNodeID", rootNodeID)

                      };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHierarchyByRootNodeID", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var agencyReviewData = new AgencyHierarchyContract();
                            agencyReviewData.NodeID = dr["AgencyNodeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyNodeID"]);
                            agencyReviewData.ParentNodeID = dr["ParentNodeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ParentNodeID"]);
                            agencyReviewData.Value = dr["AgencyNodeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyNodeName"]);
                            agencyReviewData.DisplayOrder = dr["DisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DisplayOrder"]);
                            _lstAgencyHierarchyContractt.Add(agencyReviewData);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstAgencyHierarchyContractt;
        }

        AgencyHierarchyContract IAgencyHierarchyRepository.GetAgencyDetailByNodeId(Int32 nodeId)
        {

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            AgencyHierarchyContract agencyHierarchyContract = new AgencyHierarchyContract();
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetAgencyDetailByNodeID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NodeID", nodeId);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        agencyHierarchyContract.AgencyID = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["AgencyID"]));
                        agencyHierarchyContract.NodeID = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["AgencyNodeID"]));
                        agencyHierarchyContract.HierarchyLabel = Convert.ToString(ds.Tables[0].Rows[0]["AgencyHierarchyLabel"]);
                    }
                }
            }
            return agencyHierarchyContract;
        }

        List<AgencyHierarchyContract> IAgencyHierarchyRepository.GetAgencyHierarchyByRootNodeIds(String rootNodeIds, String agencyHierarchyIds)
        {
            var _lstAgencyHierarchyContractt = new List<AgencyHierarchyContract>();
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                     {
                           new SqlParameter("@RootNodeIds", rootNodeIds),
                             new SqlParameter("@AgencyHierarchyNodeIds", agencyHierarchyIds)
                     };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHierarchyByRootNodeIds", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var agencyReviewData = new AgencyHierarchyContract();
                            agencyReviewData.NodeID = dr["AgencyNodeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyNodeID"]);
                            agencyReviewData.ParentNodeID = dr["ParentNodeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ParentNodeID"]);
                            agencyReviewData.Value = dr["AgencyNodeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyNodeName"]);
                            agencyReviewData.NodeType = dr["NodeTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["NodeTypeCode"]);
                            agencyReviewData.AgencyID = dr["AgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyID"]);
                            agencyReviewData.HierarchyLabel = dr["AgencyHierarchyLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyHierarchyLabel"]);
                            agencyReviewData.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            agencyReviewData.IsDisabled = dr["IsDisabled"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsDisabled"]);
                            agencyReviewData.DisplayOrder = dr["DisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DisplayOrder"]);
                            agencyReviewData.IsNodeAvailable = dr["IsNodeAvailable"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IsNodeAvailable"]); // UAT-4443
                            _lstAgencyHierarchyContractt.Add(agencyReviewData);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstAgencyHierarchyContractt;

        }

        List<AgencyHierarchyContract> IAgencyHierarchyRepository.GetAgencyDetailByMultipleNodeIds(String nodeIds)
        {
            var _lstAgencyHierarchyContract = new List<AgencyHierarchyContract>();
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                     {
                           new SqlParameter("@NodeIDs", nodeIds)

                     };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyDetailByMultipleNodeIDs", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var agencyReviewData = new AgencyHierarchyContract();
                            agencyReviewData.NodeID = dr["AgencyNodeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyNodeID"]);
                            agencyReviewData.HierarchyLabel = dr["AgencyHierarchyLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyHierarchyLabel"]);
                            agencyReviewData.AgencyID = dr["AgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyID"]);
                            _lstAgencyHierarchyContract.Add(agencyReviewData);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstAgencyHierarchyContract;

        }
        String IAgencyHierarchyRepository.GetAgencyDetailByMultipleNodeID(AgencyHierarchMultiSelectParameter parm)
        {
            String _lstAgencyHierarchyContract = String.Empty;
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                     {
                           new SqlParameter("@NodeIDs", parm.NodeIds),
                             new SqlParameter("@AgencyIds", parm.AgencyIds),
                                      new SqlParameter("@HierarchySelectionType", parm.HierarchySelectionType)
                     };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyDetailByMultipleNodeIDs", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {

                            _lstAgencyHierarchyContract = dr["XMLResult"] == DBNull.Value ? String.Empty : Convert.ToString(dr["XMLResult"]);

                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstAgencyHierarchyContract;

        }

        List<String> IAgencyHierarchyRepository.GetAgencyHierarchyAgencyByMultipleNodeIds(AgencyHierarchMultiSelectParameter parm) // UAT-2926
        {
            List<String> _lstAgencyHierarchyContract = new List<String>();
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                     {
                           new SqlParameter("@NodeIDs", parm.NodeIds)
                     };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHierarchyAgencyCombination", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            _lstAgencyHierarchyContract.Add(dr["AgencyHierarchyAgency"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyHierarchyAgency"]));
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstAgencyHierarchyContract;

        }

        List<AgencyHierarchyContract> IAgencyHierarchyRepository.GetTreeDataByRootNodeIDForPopUp(AgencyHierarchPopUpParameter agencyHierarchPopUpParameter)
        {

            var _lstAgencyHierarchyContractt = new List<AgencyHierarchyContract>();
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                      {
                           new SqlParameter("@RootNodeID", agencyHierarchPopUpParameter.RootNodeId),
                           new SqlParameter("@AgencyHierarchyNodeIds", agencyHierarchPopUpParameter.AgencyHierarchyNodeIds)

                      };
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHierarchyByRootNodeIdForPopUp", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var agencyReviewData = new AgencyHierarchyContract();
                            agencyReviewData.NodeID = dr["AgencyNodeID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyNodeID"]);
                            agencyReviewData.ParentNodeID = dr["ParentNodeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(dr["ParentNodeID"]);
                            agencyReviewData.Value = dr["AgencyNodeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyNodeName"]);
                            agencyReviewData.NodeType = dr["NodeTypeCode"] == DBNull.Value ? String.Empty : Convert.ToString(dr["NodeTypeCode"]);
                            agencyReviewData.AgencyID = dr["AgencyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyID"]);
                            agencyReviewData.HierarchyLabel = dr["AgencyHierarchyLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyHierarchyLabel"]);
                            agencyReviewData.AgencyName = dr["AgencyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AgencyName"]);
                            agencyReviewData.DisplayOrder = dr["DisplayOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DisplayOrder"]); //UAT-3237
                            _lstAgencyHierarchyContractt.Add(agencyReviewData);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstAgencyHierarchyContractt;
        }
        String IAgencyHierarchyRepository.GetAgencyHierarchyLabel(Int32 agencyId, Int32 agencyHierarchyId)
        {
            AgencyHierarchy agencyHierarchy = SharedDataDBContext.AgencyHierarchies.Where(a => a.AH_ID == agencyHierarchyId && a.AH_IsDeleted == false).FirstOrDefault();
            Agency data = SharedDataDBContext.Agencies.Where(a => a.AG_ID == agencyId && a.AG_IsDeleted == false).FirstOrDefault();
            if (agencyHierarchy.IsNotNull())
            {
                return agencyHierarchy.AH_Label + " | " + (data.IsNotNull() && data.AG_Label.IsNullOrEmpty() ? data.AG_Name : data.AG_Label);
            }
            return String.Empty;
        }

        String IAgencyHierarchyRepository.GetAgencyHierarchyLabelForMultipleSelection(String agencyHierarchyNodeIds)
        {
            List<Int32> list = agencyHierarchyNodeIds.Split(',').Select(t => Int32.Parse(t)).ToList();

            List<String> agencyHierarchy = SharedDataDBContext.AgencyHierarchies.Where(a => list.Contains(a.AH_ID) && a.AH_IsDeleted == false).Select(x => x.AH_Label).ToList();

            if (agencyHierarchy.IsNotNull())
            {
                String label = String.Empty;
                label = String.Join(", ", agencyHierarchy.ToArray());
                return label;
            }
            return String.Empty;
        }
        String IAgencyHierarchyRepository.GetAgencyHierarchyParent(String agencyHierarchyIDs)
        {

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetAgencyHierarchyParent", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyHierarchyIDs", agencyHierarchyIDs);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["AgencyHierarchyParentID"].ToString();
                }
            }
            return String.Empty;

        }

        #endregion

        #region UAT-2630:Agency hierarchy mapping: Agency Hierarchies grid
        List<AgencyHierarchyDataContract> IAgencyHierarchyRepository.GetRootAgencyHierarchyData(CustomPagingArgsContract customPagingContract)
        {
            List<AgencyHierarchyDataContract> lstAgencyHierarchy = new List<AgencyHierarchyDataContract>();
            Int32 currentPageIndex = 0;
            Int32 virtualPageCount = 0;
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_GetDataForManageAgencyHierarchy", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@filteringSortingData", customPagingContract.XML);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        currentPageIndex = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["CurrentPageIndex"]));
                        virtualPageCount = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["VirtualCount"]));
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            var agencyHierarchyData = new AgencyHierarchyDataContract();
                            agencyHierarchyData.AgencyHierarchyID = ds.Tables[1].Rows[i]["AgencyHierarchyID"] == DBNull.Value ? 0 : Convert.ToInt32(ds.Tables[1].Rows[i]["AgencyHierarchyID"]);
                            agencyHierarchyData.AgencyHierarchyLabel = ds.Tables[1].Rows[i]["AgencyHierarchyLabel"] == DBNull.Value ? String.Empty : Convert.ToString(ds.Tables[1].Rows[i]["AgencyHierarchyLabel"]);
                            agencyHierarchyData.CurrentPageIndex = currentPageIndex;
                            agencyHierarchyData.VirtualPageCount = virtualPageCount;
                            lstAgencyHierarchy.Add(agencyHierarchyData);
                        }
                    }
                }
            }
            return lstAgencyHierarchy;
        }

        /// <summary>
        /// This method used to Delete the Root agency node and its child node
        /// </summary>
        /// <param name="agencyHierarchyId"></param>
        /// <returns></returns>
        Boolean IAgencyHierarchyRepository.DeleteRootAgencyHierarchy(Int32 agencyHierarchyId, Int32 currentLoggedInUserId)
        {
            Boolean outputResponse = false;

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("usp_DeleteRootAgencyHierarchyAndChild", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyHierarchyID", agencyHierarchyId);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", currentLoggedInUserId);
                command.ExecuteScalar();
                outputResponse = true;
            }
            return outputResponse;
        }
        #endregion

        #region UAT-2634 :- Agency Hierarchy Package Mapping
        List<RequirementPackageContract> IAgencyHierarchyRepository.GetRequirementPackages()
        {
            var _lstRequirementPackages = new List<RequirementPackageContract>();
            var requirementPackagesList = this.SharedDataDBContext.RequirementPackages.Where(cond => !cond.RP_IsDeleted && !cond.RP_IsArchived && cond.RP_IsActive).Select(sel => new { sel.RP_ID, sel.RP_PackageName, sel.RP_PackageLabel }).ToList();
            _lstRequirementPackages = requirementPackagesList.Select(sel => new RequirementPackageContract()
            {
                RequirementPackageID = sel.RP_ID,
                RequirementPackageName = sel.RP_PackageName,
            }).ToList();
            return _lstRequirementPackages;
        }
        List<RequirementPackageContract> IAgencyHierarchyRepository.GetAgencyHierarchyPackages(CustomPagingArgsContract customPagingArgsContract, Int32 agencyHierarchyID)
        {
            var _lstRequirementPackages = new List<RequirementPackageContract>();
            String orderBy = "RequirementPackageID";
            String ordDirection = null;

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = customPagingArgsContract.SortDirectionDescending == false ? "asc" : "desc";

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyHierarchyID", agencyHierarchyID),
                    new SqlParameter("@OrderBy", orderBy),
                    new SqlParameter("@OrderDirection", ordDirection),
                    new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                    new SqlParameter("@PageSize", customPagingArgsContract.PageSize),
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHierarchyPackages", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            RequirementPackageContract requirementPackage = new RequirementPackageContract();
                            requirementPackage.RequirementPackageID = dr["RequirementPackageID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["RequirementPackageID"]);
                            requirementPackage.AgencyHierarchyPackageID = dr["AgencyHierarchyPackageID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyHierarchyPackageID"]);
                            requirementPackage.RequirementPackageName = dr["RequirementPackageName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageName"]);
                            requirementPackage.RequirementPackageLabel = dr["RequirementPackageLabel"] == DBNull.Value ? String.Empty : Convert.ToString(dr["RequirementPackageLabel"]);
                            requirementPackage.TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0;
                            requirementPackage.PackageCategoryCount = dr["PackageCategoryCount"] != DBNull.Value ? Convert.ToInt32(dr["PackageCategoryCount"]) : 0;
                            requirementPackage.TempPackageCategoryCount = dr["PackageCategoryCount"] != DBNull.Value ? Convert.ToInt32(dr["PackageCategoryCount"]) : 0;
                            requirementPackage.IsNewPackage = dr["IsNewPackage"] != DBNull.Value ? Convert.ToBoolean(dr["IsNewPackage"]) : false;
                            requirementPackage.RequirementPackageType = dr["PackageType"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PackageType"]);
                            requirementPackage.RequirementPackageCodeType = dr["PackageCodeType"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PackageCodeType"]);
                            requirementPackage.PackageArchiveState = dr["PackageArchiveState"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PackageArchiveState"]);
                            requirementPackage.EffectiveStartDate = dr["PackageStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["PackageStartDate"]); //UAT-4657
                            _lstRequirementPackages.Add(requirementPackage);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return _lstRequirementPackages;
        }
        Boolean IAgencyHierarchyRepository.SaveAgencyHierarchyPackageMapping(AgencyHierarchyPackageContract agencyHierarchyPackageContract)
        {
            if (agencyHierarchyPackageContract.AgencyHierarchyPackageID > AppConsts.NONE)
            {
                //update
                var dbUpdate = this.SharedDataDBContext.AgencyHierarchyPackages.Where(cond => cond.AHP_ID == agencyHierarchyPackageContract.AgencyHierarchyPackageID).FirstOrDefault();
                if (!dbUpdate.IsNullOrEmpty())
                {
                    dbUpdate.AHP_AgencyHierarchyID = agencyHierarchyPackageContract.AgencyHierarchyID;
                    dbUpdate.AHP_RequirementPackageID = agencyHierarchyPackageContract.RequirementPackageID;
                    dbUpdate.AHP_ModifiedBy = agencyHierarchyPackageContract.CurrentLoggedInUser;
                    dbUpdate.AHP_ModifiedOn = DateTime.Now;
                }
            }
            else
            {
                //insert
                AgencyHierarchyPackage dbInsert = new AgencyHierarchyPackage();
                dbInsert.AHP_AgencyHierarchyID = agencyHierarchyPackageContract.AgencyHierarchyID;
                dbInsert.AHP_CreatedBy = agencyHierarchyPackageContract.CurrentLoggedInUser;
                dbInsert.AHP_CreatedOn = DateTime.Now;
                dbInsert.AHP_CreatedBy = agencyHierarchyPackageContract.CurrentLoggedInUser;
                dbInsert.AHP_IsDeleted = false;
                dbInsert.AHP_RequirementPackageID = agencyHierarchyPackageContract.RequirementPackageID;
                this.SharedDataDBContext.AgencyHierarchyPackages.AddObject(dbInsert);
            }

            if (this.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            else
                return false;
        }
        Boolean IAgencyHierarchyRepository.DeleteAgencyHierarchyPackageMapping(AgencyHierarchyPackageContract agencyHierarchyPackageContract)
        {
            var dbUpdate = this.SharedDataDBContext.AgencyHierarchyPackages.Where(cond => cond.AHP_ID == agencyHierarchyPackageContract.AgencyHierarchyPackageID).FirstOrDefault();
            if (!dbUpdate.IsNullOrEmpty())
            {
                dbUpdate.AHP_IsDeleted = true;
                dbUpdate.AHP_ModifiedBy = agencyHierarchyPackageContract.CurrentLoggedInUser;
                dbUpdate.AHP_ModifiedOn = DateTime.Now;
            }
            if (this.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            else
                return false;
        }
        #endregion

        #region UAT-2632:Agency hierarchy mapping: Map Node

        List<AgencyHierarchyDataContract> IAgencyHierarchyRepository.GetMappedNodesByNodeID(Int32 parentNodeID)
        {
            var lstMappedNodes = base.SharedDataDBContext.AgencyHierarchies.OrderBy(sd => sd.AH_DisplayOrder).Where(cnd => cnd.AH_ParentID == parentNodeID && !cnd.AH_IsDeleted).ToList();

            if (!lstMappedNodes.IsNullOrEmpty())
            {
                return lstMappedNodes.Select
                    (
                      slct => new AgencyHierarchyDataContract
                      {
                          AgencyHierarchyID = slct.AH_ID,
                          AgencyHierarchyLabel = slct.AgencyNode.AN_Name,
                          Description = slct.AgencyNode.AN_Description,
                          AgencyNodeID = slct.AH_AgencyNodeID,
                          DisplayOrder = slct.AH_DisplayOrder
                      }

                    ).ToList();
            }
            else
            {
                return new List<AgencyHierarchyDataContract>();
            }
        }

        /// <summary>
        /// This method used to Delete the agency node mapping and its child node
        /// </summary>
        /// <param name="agencyHierarchyId"></param>
        /// <returns></returns>
        Boolean IAgencyHierarchyRepository.DeleteNodeMapping(Int32 nodeId, Int32 currentLoggedInUserId)
        {
            var mappedNodeToDelete = base.SharedDataDBContext.AgencyHierarchies.FirstOrDefault(dlt => dlt.AH_ID == nodeId && !dlt.AH_IsDeleted);
            if (!mappedNodeToDelete.IsNullOrEmpty())
            {
                mappedNodeToDelete.AH_IsDeleted = true;
                mappedNodeToDelete.AH_ModifiedBy = currentLoggedInUserId;
                mappedNodeToDelete.AH_ModifiedOn = DateTime.Now;

                if (base.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                {
                    return true;
                }
            }
            return false;
        }

        Boolean IAgencyHierarchyRepository.IsAgencyMappedOnNode(Int32 nodeId)
        {
            return base.SharedDataDBContext.AgencyHierarchyAgencies.Any(cnd => cnd.AHA_AgencyHierarchyID == nodeId && !cnd.AHA_IsDeleted);
        }


        Tuple<Boolean, Int32> IAgencyHierarchyRepository.SaveNodeMapping(Int32 parentNodeId, Int32 agencyNodeId, String nodeLabel, Int32 currentLoggedInUserId)
        {
            String label = String.Empty;
            Int32 agencyHierarchyId = AppConsts.NONE;
            Boolean isDataSaved = false;
            if (parentNodeId > AppConsts.NONE)
            {
                var agencyHierarchyParent = base.SharedDataDBContext.AgencyHierarchies.FirstOrDefault(x => x.AH_ID == parentNodeId && !x.AH_IsDeleted);
                if (agencyHierarchyParent.IsNotNull())
                {
                    label = agencyHierarchyParent.AH_Label;
                    if (!nodeLabel.IsNullOrEmpty())
                    {
                        label += " > " + nodeLabel;
                    }

                }
            }
            else
            {
                label = nodeLabel;
            }
            if (agencyNodeId > AppConsts.NONE)
            {
                var lastDisplayOrder = base.SharedDataDBContext.AgencyHierarchies.OrderByDescending(p => p.AH_DisplayOrder).Where(x => x.AH_ParentID == parentNodeId && !x.AH_IsDeleted).FirstOrDefault(); //UAT-3237
                AgencyHierarchy agencyHierarchyObj = new AgencyHierarchy();
                agencyHierarchyObj.AH_ParentID = parentNodeId > AppConsts.NONE ? parentNodeId : (Int32?)null;
                agencyHierarchyObj.AH_AgencyNodeID = agencyNodeId;
                agencyHierarchyObj.AH_Label = label;
                agencyHierarchyObj.AH_CreatedBy = currentLoggedInUserId;
                agencyHierarchyObj.AH_CreatedOn = DateTime.Now;
                agencyHierarchyObj.AH_DisplayOrder = lastDisplayOrder == null ? 1 : (lastDisplayOrder.AH_DisplayOrder) + 1; //UAT-3237

                base.SharedDataDBContext.AgencyHierarchies.AddObject(agencyHierarchyObj);
                if (base.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                {
                    agencyHierarchyId = agencyHierarchyObj.AH_ID;
                    if (parentNodeId < AppConsts.ONE)
                    {
                        #region UAt-2712 Default Root Node Rotation Field Settings
                        AgencyHierarchyRotationFieldOption dbInsert = new AgencyHierarchyRotationFieldOption();
                        dbInsert.AHRFO_CheckParentSetting = false;
                        dbInsert.AHRFO_IsCourse_Required = true;
                        dbInsert.AHRFO_IsDepartment_Required = true;
                        dbInsert.AHRFO_IsProgram_Required = true;
                        dbInsert.AHRFO_CreatedOn = DateTime.Now;
                        dbInsert.AHRFO_CreatedByID = currentLoggedInUserId;
                        dbInsert.AHRFO_IsDeleted = false;
                        agencyHierarchyObj.AgencyHierarchyRotationFieldOptions.Add(dbInsert);
                        base.SharedDataDBContext.SaveChanges();
                        #endregion
                    }
                    isDataSaved = true;
                }
            }
            return new Tuple<Boolean, Int32>(isDataSaved, agencyHierarchyId);
        }

        Int32 IAgencyHierarchyRepository.GetAgencyNodeIDByAgencyHierarchyID(Int32 agencyHierarchyId)
        {
            return base.SharedDataDBContext.AgencyHierarchies.FirstOrDefault(x => x.AH_ID == agencyHierarchyId && !x.AH_IsDeleted).AH_AgencyNodeID;
        }
        #endregion

        #region UAT-2629

        #region Public Methods

        public Boolean SaveNodeDetail(AgencyNodeContract agencyNodeContract)
        {
            if (agencyNodeContract.NodeId > AppConsts.NONE)
            {
                AgencyNode updateNode = base.SharedDataDBContext.AgencyNodes.Where(cond => cond.AN_ID == agencyNodeContract.NodeId && !cond.AN_IsDeleted).FirstOrDefault();
                if (!updateNode.IsNullOrEmpty())
                {
                    if (!agencyNodeContract.IsDeleted)
                    { //update
                        updateNode.AN_Name = agencyNodeContract.NodeName;
                        updateNode.AN_Label = agencyNodeContract.NodeLabel;
                        updateNode.AN_Description = agencyNodeContract.NodeDescription;
                    }
                    else
                    { //delete
                        updateNode.AN_IsDeleted = true;
                    }
                    updateNode.AN_ModifiedBy = agencyNodeContract.CurrentLoggedInUser;
                    updateNode.AN_ModifiedOn = DateTime.Now;
                }
            }
            else
            { //add
                AgencyNode addNode = new AgencyNode();
                addNode.AN_Name = agencyNodeContract.NodeName;
                addNode.AN_Label = agencyNodeContract.NodeLabel;
                addNode.AN_Description = agencyNodeContract.NodeDescription;
                addNode.AN_IsDeleted = agencyNodeContract.IsDeleted;
                addNode.AN_CreatedOn = DateTime.Now;
                addNode.AN_CreatedBy = agencyNodeContract.CurrentLoggedInUser;

                base.SharedDataDBContext.AgencyNodes.AddObject(addNode);
            }
            if (base.SharedDataDBContext.SaveChanges() > 0)
            {
                return true;
            }

            return false;
        }

        public List<AgencyNodeContract> GetAgencyNodeList()
        {
            List<AgencyNodeContract> _lstNodeList = new List<AgencyNodeContract>();
            var nodeList = base.SharedDataDBContext.AgencyNodes.Where(cond => !cond.AN_IsDeleted).Select(sel => new { sel.AN_Name, sel.AN_Label, sel.AN_Description, sel.AN_ID }).ToList();
            _lstNodeList = nodeList.Select(sel => new AgencyNodeContract()
            {
                NodeId = sel.AN_ID
                ,
                NodeName = sel.AN_Name
                ,
                NodeLabel = sel.AN_Label
                ,
                NodeDescription = sel.AN_Description
            }).ToList();
            return _lstNodeList;

        }

        //UAT-3652
        public List<AgencyNodeContract> GetAgencyNodeRootList(CustomPagingArgsContract customPagingArgsContract, String agencyNodeName, String agencyNodeDesc)
        {
            List<AgencyNodeContract> _lstNodeList = new List<AgencyNodeContract>();

            String orderBy = "NodeId";
            String ordDirection = "desc";

            orderBy = String.IsNullOrEmpty(customPagingArgsContract.SortExpression) ? orderBy : customPagingArgsContract.SortExpression;
            ordDirection = !customPagingArgsContract.SortDirectionDescending ? "desc" : "asc";

            EntityConnection connection = base.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                     new SqlParameter("@OrderBy", orderBy),
                            new SqlParameter("@OrderDirection", ordDirection),
                            new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                            new SqlParameter("@PageSize", customPagingArgsContract.PageSize),
                            new SqlParameter("@AgencyNodeName", agencyNodeName),
                            new SqlParameter("@AgencyNodeDescription", agencyNodeDesc)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyNodes", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            AgencyNodeContract agencyNodeContract = new AgencyNodeContract();
                            agencyNodeContract.NodeId = dr["NodeId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["NodeId"]);
                            agencyNodeContract.NodeName = dr["NodeName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["NodeName"]);
                            agencyNodeContract.NodeLabel = dr["NodeLabel"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["NodeLabel"]);
                            agencyNodeContract.NodeDescription = dr["NodeDescription"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["NodeDescription"]);
                            agencyNodeContract.MappedRootHierachies = dr["MappedRootHierachies"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["MappedRootHierachies"]);
                            agencyNodeContract.TotalRecordCount = dr["TotalCount"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["TotalCount"]);
                            _lstNodeList.Add(agencyNodeContract);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return _lstNodeList;
        }


        #region UAT-3652
        /// <summary>
        /// To check whether the nodeID exists or not.
        /// </summary>
        /// <param name="nodeName"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public Boolean IsNodeExist(String nodeName, Int32? nodeId = null)
        {

            //ServiceResponse<List<AgencyNodeContract>> _response = _agencyHierarchyProxy.GetAgencyNodeList();
            //List<AgencyNodeContract> listNode = AgencyHierarchyManager.GetAgencyNodeList();
            if (nodeId != null)
            {
                if (base.SharedDataDBContext.AgencyNodes.Any(x => x.AN_Name.ToLower() == nodeName.ToLower() && x.AN_ID != nodeId))
                //if (listNode.Any(x => x.NodeName.ToLower() == nodeName.ToLower() && x.NodeId != nodeId))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (base.SharedDataDBContext.AgencyNodes.Any(x => x.AN_Name.ToLower() == nodeName.ToLower() && !x.AN_IsDeleted))
                //if (listNode.Any(x => x.NodeName.ToLower() == nodeName.ToLower() && !x.IsDeleted))
                {
                    return true;
                }
                return false;
            }
        }
        #endregion


        public Boolean IsNodeMapped(Int32 nodeID)
        {
            var agencyNode = base.SharedDataDBContext.AgencyHierarchies.Where(cond => (cond.AH_AgencyNodeID == nodeID) && !cond.AH_IsDeleted);
            if (!agencyNode.IsNullOrEmpty())
                return true;
            return false;
        }


        #endregion

        #endregion

        #region UAT-2636 : Agency hierarchy mapping: Map users with hierarchy node
        List<AgencyHierarchyUserContract> IAgencyHierarchyRepository.GetAgencyUsers(Int32 agencyHierarchyID)
        {
            var _lstAgencyHierarchyUserContract = new List<AgencyHierarchyUserContract>();

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyHierarchyID", agencyHierarchyID)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyUsers", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            AgencyHierarchyUserContract agencyUser = new AgencyHierarchyUserContract();
                            agencyUser.AGU_ID = dr["AGU_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGU_ID"]);
                            agencyUser.AGU_Name = dr["AGU_Name"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AGU_Name"]);
                            _lstAgencyHierarchyUserContract.Add(agencyUser);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return _lstAgencyHierarchyUserContract;
        }
        List<AgencyHierarchyUserContract> IAgencyHierarchyRepository.GetAgencyHierarchyUsers(Int32 agencyHierarchyID)
        {
            var _lstAgencyHierarchyUserContract = new List<AgencyHierarchyUserContract>();
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyHierarchyId", agencyHierarchyID)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHierarchyUsers", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            AgencyHierarchyUserContract agencyUser = new AgencyHierarchyUserContract();
                            agencyUser.AgencyHierarchyID = agencyHierarchyID;
                            agencyUser.AGU_ID = dr["AGU_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGU_ID"]);
                            agencyUser.AGU_Name = dr["AGU_Name"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AGU_Name"]);
                            agencyUser.LstAGU_AgencyID = dr["LstAGU_AgencyID"].GetType().Name == "DBNull" ? new List<Int32>() : Convert.ToString(dr["LstAGU_AgencyID"]).Split(',').ConvertIntoIntList();
                            agencyUser.AGU_ComplianceSharedInfoTypeID = dr["AGU_ComplianceSharedInfoTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGU_ComplianceSharedInfoTypeID"]);
                            agencyUser.AGU_ReqRotationSharedInfoTypeID = dr["AGU_ReqRotationSharedInfoTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGU_ReqRotationSharedInfoTypeID"]);
                            agencyUser.AGU_BkgSharedInfoTypeID = dr["AGU_BkgSharedInfoTypeID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGU_BkgSharedInfoTypeID"]);
                            agencyUser.AGU_ComplianceSharedInfoTypeName = dr["AGU_ComplianceSharedInfoTypeName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AGU_ComplianceSharedInfoTypeName"]);
                            agencyUser.AGU_ReqRotationSharedInfoTypeName = dr["AGU_ReqRotationSharedInfoTypeName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AGU_ReqRotationSharedInfoTypeName"]);
                            agencyUser.AGU_BkgSharedInfoTypeName = dr["AGU_BkgSharedInfoTypeName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AGU_BkgSharedInfoTypeName"]);
                            agencyUser.lstApplicationInvitationMetaDataID = dr["lstApplicationInvitationMetaDataID"].GetType().Name == "DBNull" ? new List<Int32>() : Convert.ToString(dr["lstApplicationInvitationMetaDataID"]).Split(',').ConvertIntoIntList();
                            agencyUser.AGU_AgencyUserPermission = Convert.ToBoolean(dr["AGU_AgencyUserPermission"]);
                            agencyUser.AGU_RotationPackagePermission = Convert.ToBoolean(dr["AGU_RotationPackagePermission"]);
                            agencyUser.lstInvitationSharedInfoTypeID = dr["lstInvitationSharedInfoTypeID"].GetType().Name == "DBNull" ? new List<Int32>() : Convert.ToString(dr["lstInvitationSharedInfoTypeID"]).Split(',').ConvertIntoIntList();
                            agencyUser.AttestationRptPermission = Convert.ToBoolean(dr["AttestationRptPermission"]);
                            agencyUser.SSN_Permission = Convert.ToBoolean(dr["SSN_Permission"]);
                            agencyUser.HideAgencyPortalDetailLink = Convert.ToBoolean(dr["HideAgencyPortalDetailLink_Permission"]);
                            //   agencyUser.IsEmailNeedToSend = Convert.ToBoolean(dr["IsEmailNeedToSend"]);//Code commented for UAT-2803
                            agencyUser.AGU_RotationPackageViewPermission = Convert.ToBoolean(dr["AGU_RotationPackageViewPermission"]);
                            agencyUser.AGU_AllowJobPosting = Convert.ToBoolean(dr["AGU_AllowJobPosting"]);
                            agencyUser.AGU_DoNotShowNonAgencyShares = Convert.ToBoolean(dr["AGU_DoNotShowNonAgencyShares"]);
                            agencyUser.IsRequirementSharingNonRotationNotification = Convert.ToBoolean(dr["IsRequirementSharingNonRotationNotification"]);//UAT-2803
                            agencyUser.IsRequirementSharingRotationNotification = Convert.ToBoolean(dr["IsRequirementSharingRotationNotification"]);//UAT-2803
                            agencyUser.IsRotationInvitationApprovalRejectionNotification = Convert.ToBoolean(dr["IsRotationInvitationApprovalRejectionNotification"]);//UAT-2803
                            agencyUser.IsIndividualProfileSharingWithEmailNotification = Convert.ToBoolean(dr["IsIndividualProfileSharingWithEmailNotification"]);//UAT-2803
                            agencyUser.IsProfileSharingWithEmailNotification = Convert.ToBoolean(dr["IsProfileSharingWithEmailNotification"]);//UAT-2942
                            agencyUser.SendOutOfComplianceNotification = Convert.ToBoolean(dr["SendOutOfComplianceNotification"]);//UAT-2977
                            agencyUser.SendUpdatedApplicantRequirementNotification = Convert.ToBoolean(dr["SendUpdatedApplicantRequirementNotification"]);
                            agencyUser.SendUpdatedRotationDetailsNotification = Convert.ToBoolean(dr["SendUpdatedRotationDetailsNotification"]);//UAT-3108
                            agencyUser.SendStudentDroppedFromRotationNotification = Convert.ToBoolean(dr["SendStudentDroppedFromRotationNotification"]);//UAT-3222
                            agencyUser.AGU_TemplateId = dr["AGU_TemplateId"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AGU_TemplateId"]);
                            agencyUser.SendItSystemAccessFormNotification = Convert.ToBoolean(dr["SendITSystemAccessFormNotification"]);//UAT-3998
                            agencyUser.SendRotationEndDateChangeNotification = dr["SendRotationEndDateChangeNotification"].GetType().Name == "DBNull" ? false : Convert.ToBoolean(dr["SendRotationEndDateChangeNotification"]);//UAT-4561
                            _lstAgencyHierarchyUserContract.Add(agencyUser);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return _lstAgencyHierarchyUserContract;
        }
        Boolean IAgencyHierarchyRepository.SaveAgencyHierarchyUserMapping(AgencyHierarchyUserContract agencyHierarchyUserContract)
        {
            //update
            var dbUpdate = this.SharedDataDBContext.AgencyHierarchyUsers.Where(cond => cond.AHU_AgencyUserID == agencyHierarchyUserContract.AGU_ID && cond.AHU_AgencyHierarchyID == agencyHierarchyUserContract.AgencyHierarchyID && !cond.AHU_IsDeleted).FirstOrDefault();
            if (!dbUpdate.IsNullOrEmpty())
            {
                dbUpdate.AHU_ModifiedBy = agencyHierarchyUserContract.CurrentLoggedInUser;
                dbUpdate.AHU_ModifiedOn = DateTime.Now;
            }
            else
            {
                //insert
                AgencyHierarchyUser dbInsert = new AgencyHierarchyUser();
                dbInsert.AHU_AgencyHierarchyID = agencyHierarchyUserContract.AgencyHierarchyID;
                dbInsert.AHU_CreatedBy = agencyHierarchyUserContract.CurrentLoggedInUser;
                dbInsert.AHU_CreatedOn = DateTime.Now;
                dbInsert.AHU_CreatedBy = agencyHierarchyUserContract.CurrentLoggedInUser;
                dbInsert.AHU_IsDeleted = false;
                dbInsert.AHU_AgencyUserID = agencyHierarchyUserContract.AGU_ID;
                this.SharedDataDBContext.AgencyHierarchyUsers.AddObject(dbInsert);
            }

            if (agencyHierarchyUserContract.IsUpdateFlag)
            {
                #region Agency User Permissions
                AgencyUser agencyUser = this.SharedDataDBContext.AgencyUsers.Where(x => x.AGU_ID == dbUpdate.AHU_AgencyUserID && !x.AGU_IsDeleted).FirstOrDefault();
                if (!agencyUser.IsNullOrEmpty())
                {
                    if (agencyHierarchyUserContract.AGU_TemplateId == AppConsts.NONE)
                    {
                        agencyUser.AGU_ComplianceSharedInfoTypeID = agencyHierarchyUserContract.AGU_ComplianceSharedInfoTypeID;
                        // agencyUser.AGU_AgencyUserPermission = agencyHierarchyUserContract.AGU_AgencyUserPermission;
                        // agencyUser.AGU_RotationPackagePermission = agencyHierarchyUserContract.AGU_RotationPackagePermission;
                        agencyUser.AGU_BkgSharedInfoTypeID = agencyHierarchyUserContract.AGU_BkgSharedInfoTypeID;
                        agencyUser.AGU_ReqRotationSharedInfoTypeID = agencyHierarchyUserContract.AGU_ReqRotationSharedInfoTypeID;
                    }

                    agencyUser.AGU_IsDeleted = false;
                    agencyUser.AGU_ModifiedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                    agencyUser.AGU_ModifiedOn = DateTime.Now;
                    //agencyUser.AGU_IsNeedToSendEmail = agencyHierarchyUserContract.IsEmailNeedToSend;//Code commented for UAT-2803
                    //agencyUser.AGU_RotationPackageViewPermission = agencyHierarchyUserContract.AGU_RotationPackageViewPermission;
                    //agencyUser.AGU_AllowJobPosting = agencyHierarchyUserContract.AGU_AllowJobPosting;
                    //agencyUser.AGU_DoNotShowNonAgencyShares = agencyHierarchyUserContract.AGU_DoNotShowNonAgencyShares;
                    agencyUser.AGU_TemplateId = agencyHierarchyUserContract.AGU_TemplateId; //UAT-3316


                    if (agencyHierarchyUserContract.AGU_TemplateId == AppConsts.NONE)
                    {
                        #region lstApplicationInvitationMetaDataID
                        List<AgencyUserSharedData> lstAgencyUserSharedData = this.SharedDataDBContext.AgencyUserSharedDatas.Where(x => x.AUSD_AgencyUserID == agencyUser.AGU_ID && !x.AUSD_IsDeleted).ToList();
                        if (lstAgencyUserSharedData.IsNotNull())
                        {


                            if (!agencyHierarchyUserContract.lstApplicationInvitationMetaDataID.IsNullOrEmpty() && agencyHierarchyUserContract.lstApplicationInvitationMetaDataID.Count > 0)
                            {
                                //Delete Old Data
                                foreach (var agencyUserSharedData in lstAgencyUserSharedData)
                                {
                                    agencyUserSharedData.AUSD_IsDeleted = true;
                                    agencyUserSharedData.AUSD_ModifiedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                                    agencyUserSharedData.AUSD_ModifiedOn = DateTime.Now;
                                }
                                //Add New Data
                                foreach (var applicationInvtMetaDataIds in agencyHierarchyUserContract.lstApplicationInvitationMetaDataID)
                                {
                                    AgencyUserSharedData agencyUserSharedData = new AgencyUserSharedData();
                                    agencyUserSharedData.AUSD_AgencyUserID = agencyUser.AGU_ID;
                                    agencyUserSharedData.AUSD_ApplicationInvitationMetaDataID = applicationInvtMetaDataIds;
                                    agencyUserSharedData.AUSD_IsDeleted = false;
                                    agencyUserSharedData.AUSD_CreatedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                                    agencyUserSharedData.AUSD_CreatedOn = DateTime.Now;

                                    this.SharedDataDBContext.AddToAgencyUserSharedDatas(agencyUserSharedData);
                                }
                            }
                        }
                        #endregion

                        #region Bkg Permissions
                        //UAT-1213: Updates to Agency User background check permissions.
                        List<Entity.SharedDataEntity.InvitationSharedInfoMapping> lstInvitationSharedInfoMapping = this.SharedDataDBContext.InvitationSharedInfoMappings.Where(x => x.ISIM_AgencyUserID == agencyUser.AGU_ID && x.ISIM_IsDeleted == false).ToList();
                        if (lstInvitationSharedInfoMapping.IsNotNull())
                        {
                            //Delete Old Data
                            foreach (var invitationSharedInfoMapping in lstInvitationSharedInfoMapping)
                            {
                                invitationSharedInfoMapping.ISIM_IsDeleted = true;
                                invitationSharedInfoMapping.ISIM_ModifiedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                                invitationSharedInfoMapping.ISIM_ModifiedOn = DateTime.Now;
                            }

                            //Add New Data
                            foreach (var invitationSharedInfoTypeID in agencyHierarchyUserContract.lstInvitationSharedInfoTypeID)
                            {
                                Entity.SharedDataEntity.InvitationSharedInfoMapping invitationSharedInfoMapping = new Entity.SharedDataEntity.InvitationSharedInfoMapping();
                                invitationSharedInfoMapping.ISIM_AgencyUserID = agencyUser.AGU_ID;
                                invitationSharedInfoMapping.ISIM_InvitationSharedInfoTypeID = invitationSharedInfoTypeID;
                                invitationSharedInfoMapping.ISIM_IsDeleted = false;
                                invitationSharedInfoMapping.ISIM_CreatedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                                invitationSharedInfoMapping.ISIM_CreatedOn = DateTime.Now;

                                this.SharedDataDBContext.AddToInvitationSharedInfoMappings(invitationSharedInfoMapping);
                            }
                        }
                        #endregion

                        #region Agency User Permissions

                        IEnumerable<AgencyUserPermission> existingAgencyUserAttestationPermissions = agencyUser.AgencyUserPermissions.Where(cond => !cond.AUP_IsDeleted && cond.lkpAgencyUserPermissionType.AUPT_Code == AgencyUserPermissionType.ATTESTATION_REPORT_TEXT_PERMISSION.GetStringValue());
                        if (!existingAgencyUserAttestationPermissions.IsNullOrEmpty())
                            existingAgencyUserAttestationPermissions.ForEach(s => s.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.IsManageAttestationPermission ? AppConsts.ONE : AppConsts.TWO);
                        else
                        {
                            AgencyUserPermission dbInsert = new AgencyUserPermission();
                            dbInsert.AUP_PermissionTypeID = AppConsts.ONE;
                            dbInsert.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.IsManageAttestationPermission ? AppConsts.ONE : AppConsts.TWO;
                            dbInsert.AUP_IsDeleted = false;
                            dbInsert.AUP_CreatedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                            dbInsert.AUP_CreatedOn = DateTime.Now;
                            agencyUser.AgencyUserPermissions.Add(dbInsert);
                        }
                        IEnumerable<AgencyUserPermission> existingAgencyUserSSN_Permissions = agencyUser.AgencyUserPermissions.Where(cond => !cond.AUP_IsDeleted && cond.lkpAgencyUserPermissionType.AUPT_Code == AgencyUserPermissionType.GRANULAR_SSN_PERMISSION.GetStringValue());

                        if (!existingAgencyUserSSN_Permissions.IsNullOrEmpty())
                            existingAgencyUserSSN_Permissions.ForEach(s => s.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.SSN_Permission ? AppConsts.ONE : AppConsts.TWO);
                        else
                        {
                            String SSNPermissionCode = AgencyUserPermissionType.GRANULAR_SSN_PERMISSION.GetStringValue();
                            Int32 SSNPermissionTypeID = this.SharedDataDBContext.lkpAgencyPermissionTypes.Where(g => g.APT_Code == SSNPermissionCode).FirstOrDefault().APT_ID;
                            AgencyUserPermission dbInsert = new AgencyUserPermission();
                            dbInsert.AUP_PermissionTypeID = SSNPermissionTypeID;
                            dbInsert.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.SSN_Permission ? AppConsts.ONE : AppConsts.TWO;
                            dbInsert.AUP_IsDeleted = false;
                            dbInsert.AUP_CreatedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                            dbInsert.AUP_CreatedOn = DateTime.Now;
                            agencyUser.AgencyUserPermissions.Add(dbInsert);
                        }

                        IEnumerable<AgencyUserPermission> existingAgencyUserDetailLink_Permissions = agencyUser.AgencyUserPermissions.Where(cond => !cond.AUP_IsDeleted && cond.lkpAgencyUserPermissionType.AUPT_Code == AgencyUserPermissionType.AGENCY_PORTAL_DETAIL_LINK_PERMISSION.GetStringValue());

                        if (!existingAgencyUserDetailLink_Permissions.IsNullOrEmpty())
                            existingAgencyUserDetailLink_Permissions.ForEach(s => s.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.HideAgencyPortalDetailLink ? AppConsts.ONE : AppConsts.TWO);
                        else
                        {
                            String detailLinkPermissionCode = AgencyUserPermissionType.AGENCY_PORTAL_DETAIL_LINK_PERMISSION.GetStringValue();
                            Int32 detailLinkPermissionTypeID = this.SharedDataDBContext.lkpAgencyPermissionTypes.Where(g => g.APT_Code == detailLinkPermissionCode).FirstOrDefault().APT_ID;
                            AgencyUserPermission dbInsert = new AgencyUserPermission();
                            dbInsert.AUP_PermissionTypeID = detailLinkPermissionTypeID;
                            dbInsert.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.HideAgencyPortalDetailLink ? AppConsts.ONE : AppConsts.TWO;
                            dbInsert.AUP_IsDeleted = false;
                            dbInsert.AUP_CreatedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                            dbInsert.AUP_CreatedOn = DateTime.Now;
                            agencyUser.AgencyUserPermissions.Add(dbInsert);
                        }

                        /////////////////////////////

                        IEnumerable<AgencyUserPermission> existingAgencyUserRotationPackageViewPermission = agencyUser.AgencyUserPermissions.Where(cond => !cond.AUP_IsDeleted && cond.lkpAgencyUserPermissionType.AUPT_Code == AgencyUserPermissionType.ROTATION_PACKAGE_VIEW_PERMISSION.GetStringValue());
                        if (!existingAgencyUserAttestationPermissions.IsNullOrEmpty())
                            existingAgencyUserAttestationPermissions.ForEach(s => s.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.AGU_RotationPackageViewPermission ? AppConsts.ONE : AppConsts.TWO);
                        else
                        {
                            AgencyUserPermission dbInsert = new AgencyUserPermission();
                            dbInsert.AUP_PermissionTypeID = AppConsts.ONE;
                            dbInsert.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.AGU_RotationPackageViewPermission ? AppConsts.ONE : AppConsts.TWO;
                            dbInsert.AUP_IsDeleted = false;
                            dbInsert.AUP_CreatedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                            dbInsert.AUP_CreatedOn = DateTime.Now;
                            agencyUser.AgencyUserPermissions.Add(dbInsert);
                        }


                        IEnumerable<AgencyUserPermission> existingAgencyAllowJobPostingPermission = agencyUser.AgencyUserPermissions.Where(cond => !cond.AUP_IsDeleted && cond.lkpAgencyUserPermissionType.AUPT_Code == AgencyUserPermissionType.ALLOW_JOB_POSTING_PERMISSION.GetStringValue());
                        if (!existingAgencyUserAttestationPermissions.IsNullOrEmpty())
                            existingAgencyUserAttestationPermissions.ForEach(s => s.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.AGU_AllowJobPosting ? AppConsts.ONE : AppConsts.TWO);
                        else
                        {
                            AgencyUserPermission dbInsert = new AgencyUserPermission();
                            dbInsert.AUP_PermissionTypeID = AppConsts.ONE;
                            dbInsert.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.AGU_AllowJobPosting ? AppConsts.ONE : AppConsts.TWO;
                            dbInsert.AUP_IsDeleted = false;
                            dbInsert.AUP_CreatedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                            dbInsert.AUP_CreatedOn = DateTime.Now;
                            agencyUser.AgencyUserPermissions.Add(dbInsert);
                        }

                        IEnumerable<AgencyUserPermission> existingAgencyDoNotShowNonAgencySharesPermission = agencyUser.AgencyUserPermissions.Where(cond => !cond.AUP_IsDeleted && cond.lkpAgencyUserPermissionType.AUPT_Code == AgencyUserPermissionType.DONOT_SHOW_NON_AGENCY_SHARES_PERMISSION.GetStringValue());
                        if (!existingAgencyUserAttestationPermissions.IsNullOrEmpty())
                            existingAgencyUserAttestationPermissions.ForEach(s => s.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.AGU_DoNotShowNonAgencyShares ? AppConsts.ONE : AppConsts.TWO);
                        else
                        {
                            AgencyUserPermission dbInsert = new AgencyUserPermission();
                            dbInsert.AUP_PermissionTypeID = AppConsts.ONE;
                            dbInsert.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.AGU_DoNotShowNonAgencyShares ? AppConsts.ONE : AppConsts.TWO;
                            dbInsert.AUP_IsDeleted = false;
                            dbInsert.AUP_CreatedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                            dbInsert.AUP_CreatedOn = DateTime.Now;
                            agencyUser.AgencyUserPermissions.Add(dbInsert);
                        }


                        IEnumerable<AgencyUserPermission> existingAgencyRotationPackagePermission = agencyUser.AgencyUserPermissions.Where(cond => !cond.AUP_IsDeleted && cond.lkpAgencyUserPermissionType.AUPT_Code == AgencyUserPermissionType.ROTATION_PACKAGE_PERMISSION.GetStringValue());
                        if (!existingAgencyUserAttestationPermissions.IsNullOrEmpty())
                            existingAgencyUserAttestationPermissions.ForEach(s => s.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.AGU_RotationPackagePermission ? AppConsts.ONE : AppConsts.TWO);
                        else
                        {
                            AgencyUserPermission dbInsert = new AgencyUserPermission();
                            dbInsert.AUP_PermissionTypeID = AppConsts.ONE;
                            dbInsert.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.AGU_RotationPackagePermission ? AppConsts.ONE : AppConsts.TWO;
                            dbInsert.AUP_IsDeleted = false;
                            dbInsert.AUP_CreatedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                            dbInsert.AUP_CreatedOn = DateTime.Now;
                            agencyUser.AgencyUserPermissions.Add(dbInsert);
                        }


                        IEnumerable<AgencyUserPermission> existingAgencyAgencyUserPermission = agencyUser.AgencyUserPermissions.Where(cond => !cond.AUP_IsDeleted && cond.lkpAgencyUserPermissionType.AUPT_Code == AgencyUserPermissionType.AGENCY_USER_PERMISSION.GetStringValue());
                        if (!existingAgencyUserAttestationPermissions.IsNullOrEmpty())
                            existingAgencyUserAttestationPermissions.ForEach(s => s.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.AGU_AgencyUserPermission ? AppConsts.ONE : AppConsts.TWO);
                        else
                        {
                            AgencyUserPermission dbInsert = new AgencyUserPermission();
                            dbInsert.AUP_PermissionTypeID = AppConsts.ONE;
                            dbInsert.AUP_PermissionAccessTypeID = agencyHierarchyUserContract.AGU_AgencyUserPermission ? AppConsts.ONE : AppConsts.TWO;
                            dbInsert.AUP_IsDeleted = false;
                            dbInsert.AUP_CreatedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                            dbInsert.AUP_CreatedOn = DateTime.Now;
                            agencyUser.AgencyUserPermissions.Add(dbInsert);
                        }

                        #endregion
                    }

                }
                if (agencyHierarchyUserContract.AGU_TemplateId == AppConsts.NONE)
                {
                    #region UAT-2803: Enhance the agency user settings so that we can individually choose what notifications an agency user receives
                    if (!agencyHierarchyUserContract.AGU_ID.IsNullOrEmpty())
                    {
                        List<AgencyUserNotificationMapping> lstAgencyUserNotificationMappings = this.SharedDataDBContext.AgencyUserNotificationMappings.Where(con => con.AUNM_AgencyUserID == agencyHierarchyUserContract.AGU_ID && !con.AUNM_IsDeleted).ToList();

                        if (!lstAgencyUserNotificationMappings.IsNullOrEmpty())
                        {
                            foreach (AgencyUserNotificationMapping agencyusernotMapping in lstAgencyUserNotificationMappings)
                            {
                                foreach (var item in agencyHierarchyUserContract.dicNotificationData)
                                {
                                    if (item.Key == agencyusernotMapping.AUNM_NotificationTypeID)
                                    {
                                        agencyusernotMapping.ANUM_IsMailToBeSend = item.Value;
                                        agencyusernotMapping.AUNM_ModifiedBy = agencyHierarchyUserContract.CurrentLoggedInUser;
                                        agencyusernotMapping.AUNM_ModifiedOn = DateTime.Now;
                                    }
                                }
                            }

                            foreach (var item in agencyHierarchyUserContract.dicNotificationData)
                            {
                                if (!(lstAgencyUserNotificationMappings.Exists(cond => cond.AUNM_NotificationTypeID == item.Key)))
                                {
                                    AgencyUserNotificationMapping agencyUserNotificationMapping = new AgencyUserNotificationMapping();
                                    agencyUserNotificationMapping.AUNM_AgencyUserID = agencyHierarchyUserContract.AGU_ID;
                                    agencyUserNotificationMapping.AUNM_NotificationTypeID = item.Key;
                                    agencyUserNotificationMapping.ANUM_IsMailToBeSend = item.Value;
                                    agencyUserNotificationMapping.AUNM_IsDeleted = false;
                                    agencyUserNotificationMapping.AUNM_CreatedOn = DateTime.Now;
                                    agencyUserNotificationMapping.AUNM_CreatedBy = agencyHierarchyUserContract.CurrentLoggedInUser;
                                    this.SharedDataDBContext.AgencyUserNotificationMappings.AddObject(agencyUserNotificationMapping);
                                }
                            }
                        }
                        else
                        {
                            if (!agencyHierarchyUserContract.dicNotificationData.IsNullOrEmpty())
                            {
                                foreach (var item in agencyHierarchyUserContract.dicNotificationData)
                                {
                                    AgencyUserNotificationMapping agencyUserNotificationMapping = new AgencyUserNotificationMapping();
                                    agencyUserNotificationMapping.AUNM_AgencyUserID = agencyHierarchyUserContract.AGU_ID;
                                    agencyUserNotificationMapping.AUNM_NotificationTypeID = item.Key;
                                    agencyUserNotificationMapping.ANUM_IsMailToBeSend = item.Value;
                                    agencyUserNotificationMapping.AUNM_IsDeleted = false;
                                    agencyUserNotificationMapping.AUNM_CreatedOn = DateTime.Now;
                                    agencyUserNotificationMapping.AUNM_CreatedBy = agencyHierarchyUserContract.CurrentLoggedInUser;
                                    this.SharedDataDBContext.AgencyUserNotificationMappings.AddObject(agencyUserNotificationMapping);
                                }
                            }
                        }
                    }
                    #endregion
                }

                #region UAT-3664, Agency user reports permissions

                if (agencyHierarchyUserContract.AGU_TemplateId == AppConsts.NONE)
                {
                    //Start UAT-3664 : Agency User Report Type Permissions.

                    String reportPermissionTypeCode = AgencyUserPermissionType.REPORTS_PERMISSION.GetStringValue();
                    Int32 reportPermissionTypeID = this.SharedDataDBContext.lkpAgencyUserPermissionTypes.Where(cond => !cond.AUPT_IsDeleted && cond.AUPT_Code == reportPermissionTypeCode).FirstOrDefault().AUPT_ID;
                    String noAccessTypeCode = AgencyUserPermissionAccessType.NO.GetStringValue();
                    Int32 noAccessPermissionID = this.SharedDataDBContext.lkpAgencyUserPermissionAccessTypes.Where(cnd => !cnd.AUPAT_IsDeleted && cnd.AUPAT_Code == noAccessTypeCode).FirstOrDefault().AUPAT_ID;
                    List<AgencyUserPermission> lstExistingAgencyUserReportsPermissions = agencyUser.AgencyUserPermissions.Where(cond => !cond.AUP_IsDeleted && cond.lkpAgencyUserPermissionType.AUPT_Code == reportPermissionTypeCode).ToList();



                    //List<AgencyUserPermission> existingAgencyUserReportsPermissions = existingAgencyUserPermissions.Where(cond => cond.AUP_PermissionTypeID == reportPermissionTypeID).ToList();
                    // List<AgencyUserPermission> lstAgencyUserReportsPermission = lstAgencyUserPermission.Where(cond => cond.AUP_PermissionTypeID == reportPermissionTypeID).ToList();

                    foreach (Int32 reportTypeID in agencyHierarchyUserContract.lstCheckedReportsTypeID)
                    {
                        if (!lstExistingAgencyUserReportsPermissions.IsNullOrEmpty() && lstExistingAgencyUserReportsPermissions.Where(cond => cond.AUP_RecordTypeID == reportTypeID).Any())
                        {
                            AgencyUserPermission aup = lstExistingAgencyUserReportsPermissions.Where(cond => cond.AUP_RecordTypeID == reportTypeID).FirstOrDefault();

                            //update
                            aup.AUP_PermissionAccessTypeID = noAccessPermissionID;
                            aup.AUP_PermissionTypeID = reportPermissionTypeID;
                            aup.AUP_IsDeleted = false;
                            aup.AUP_ModifiedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                            aup.AUP_ModifiedOn = DateTime.Now;
                            aup.AUP_RecordTypeID = reportTypeID;

                            lstExistingAgencyUserReportsPermissions = lstExistingAgencyUserReportsPermissions.Where(cond => cond.AUP_RecordTypeID != reportTypeID).ToList();
                        }
                        else
                        {
                            //Insert
                            AgencyUserPermission dbInsert = new AgencyUserPermission();
                            dbInsert.AUP_PermissionTypeID = reportPermissionTypeID;
                            dbInsert.AUP_PermissionAccessTypeID = noAccessPermissionID;
                            dbInsert.AUP_IsDeleted = false;
                            dbInsert.AUP_CreatedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                            dbInsert.AUP_CreatedOn = DateTime.Now;
                            dbInsert.AUP_RecordTypeID = reportTypeID;
                            agencyUser.AgencyUserPermissions.Add(dbInsert);
                        }

                    }

                    if (!lstExistingAgencyUserReportsPermissions.IsNullOrEmpty())
                    {
                        //These are unchecked record, i.e these reports have permission access "YES".
                        String yesPermissionAccessCode = AgencyUserPermissionAccessType.YES.GetStringValue();
                        Int32 yesPermissionAccessID = this.SharedDataDBContext.lkpAgencyUserPermissionAccessTypes.Where(con => !con.AUPAT_IsDeleted && con.AUPAT_Code == yesPermissionAccessCode).FirstOrDefault().AUPAT_ID;

                        if (yesPermissionAccessID > AppConsts.NONE)
                        {
                            foreach (AgencyUserPermission existingAgencyUserReportPermission in lstExistingAgencyUserReportsPermissions)
                            {
                                //AgencyUserPermission aup = lstExistingAgencyUserReportsPermissions.Where(cond => cond.AUP_RecordTypeID == existingAgencyUserReportPermission.AUP_RecordTypeID).FirstOrDefault();
                                AgencyUserPermission aup = existingAgencyUserReportPermission;
                                //update
                                aup.AUP_PermissionAccessTypeID = yesPermissionAccessID;
                                aup.AUP_IsDeleted = false;
                                aup.AUP_ModifiedByID = agencyHierarchyUserContract.CurrentLoggedInUser;
                                aup.AUP_ModifiedOn = DateTime.Now;
                            }
                        }
                    }


                    //END UAT-3664
                }
                #endregion


                #endregion
            }
            if (this.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                #region UAT-2637 : Agency hierarchy mapping: Automatic consolidation of user permissions

                EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
                using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    SqlCommand command = new SqlCommand("usp_ConsolidateAgencyUserPermissions", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AgencyHierarchyID", agencyHierarchyUserContract.AgencyHierarchyID);
                    command.Parameters.AddWithValue("@CurrentLoggedInUserID", agencyHierarchyUserContract.CurrentLoggedInUser);
                    command.Parameters.AddWithValue("@AgencyUserID", agencyHierarchyUserContract.AGU_ID);
                    command.ExecuteScalar();
                    con.Close();
                }

                #endregion

                #region UAT-3719
                if (agencyHierarchyUserContract.AGU_ID > AppConsts.NONE && agencyHierarchyUserContract.CurrentLoggedInUser > AppConsts.NONE)
                {
                    SaveAgencyUserPermissionAuditDetails(agencyHierarchyUserContract.AGU_ID, null, agencyHierarchyUserContract.CurrentLoggedInUser);
                }
                #endregion

                return true;
            }
            else
                return false;
        }
        Boolean IAgencyHierarchyRepository.DeleteAgencyHierarchyUserMapping(AgencyHierarchyUserContract agencyHierarchyUserContract)
        {
            var dbUpdate = this.SharedDataDBContext.AgencyHierarchyUsers.Where(cond => cond.AHU_AgencyUserID == agencyHierarchyUserContract.AGU_ID && cond.AHU_AgencyHierarchyID == agencyHierarchyUserContract.AgencyHierarchyID && !cond.AHU_IsDeleted).FirstOrDefault();
            if (!dbUpdate.IsNullOrEmpty())
            {
                dbUpdate.AHU_IsDeleted = true;
                dbUpdate.AHU_ModifiedBy = agencyHierarchyUserContract.CurrentLoggedInUser;
                dbUpdate.AHU_ModifiedOn = DateTime.Now;
            }

            if (this.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            else
                return false;
        }

        void IAgencyHierarchyRepository.CallDigestionStoreProcedureFunctionForAgencyHierarchy(Dictionary<String, Object> param)
        {
            //call digestion SP
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                // SqlCommand command = new SqlCommand("usp_spname", con);
                // command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@param1", param1);
                //  command.ExecuteScalar();
            }
        }
        #endregion

        #region [UAT-2635]

        List<SchoolNodeAssociationDataContract> IAgencyHierarchyRepository.GetSchoolNodeAssociationByAgencyHierarchyID(Int32 agencyHierarchyID)
        {

            List<SchoolNodeAssociationDataContract> lstSchoolNodeAssociationContract = new List<SchoolNodeAssociationDataContract>();

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                      {
                           new SqlParameter("@AgencyHierarchyID", agencyHierarchyID)

                      };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetSchoolNodeAssociationByAgencyHierarchyID", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            SchoolNodeAssociationDataContract schoolNodeAssociationContract = new SchoolNodeAssociationDataContract();
                            schoolNodeAssociationContract.TenantID = dr["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TenantID"]);
                            schoolNodeAssociationContract.TenantName = dr["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TenantName"]);
                            schoolNodeAssociationContract.DPM_ID = dr["DPM_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DPM_ID"]);
                            schoolNodeAssociationContract.DPM_Label = dr["DPM_Label"] == DBNull.Value ? String.Empty : Convert.ToString(dr["DPM_Label"]);
                            schoolNodeAssociationContract.AgencyHierarchyID = dr["AgencyHierarchyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyHierarchyID"]);
                            schoolNodeAssociationContract.IsAdminShare = dr["IsAdminShare"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsAdminShare"]);
                            schoolNodeAssociationContract.IsStudentShare = dr["IsStudentShare"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsStudentShare"]);
                            lstSchoolNodeAssociationContract.Add(schoolNodeAssociationContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return lstSchoolNodeAssociationContract;
        }

        Boolean IAgencyHierarchyRepository.IsSchoolNodeAssociationExists(Int32 agencyHierarchyInstitutionNodeID, Int32 agencyHierarchyID, Int32 DPM_ID)
        {
            Boolean isObjectExists = _dbContext.AgencyHierarchyInstitutionNodes
                                       .Where(cond => cond.AHIN_AgencyHierarchyID == agencyHierarchyID
                                                && cond.AHIN_DeptProgMappingID == DPM_ID
                                                && cond.AHIN_ID != agencyHierarchyInstitutionNodeID
                                                && cond.AHIN_IsDeleted == false).Any();

            return isObjectExists;
        }

        Boolean IAgencyHierarchyRepository.SaveUpdateSchoolNodeAssociation(SchoolNodeAssociationContract schoolNodeAssociationContract)
        {

            List<AgencyHierarchyInstitutionNode> agencyHierarchyInstitutionNodeList = _dbContext.AgencyHierarchyInstitutionNodes
                                    .Where(cond => cond.AHIN_AgencyHierarchyID == schoolNodeAssociationContract.AgencyHierarchyID && cond.AHIN_IsDeleted == false).ToList();

            if (schoolNodeAssociationContract.IsNotNull() && schoolNodeAssociationContract.DPM_IDs.Count > 0)
            {
                foreach (Int32 dpmId in schoolNodeAssociationContract.DPM_IDs)
                {
                    if (!agencyHierarchyInstitutionNodeList.Any(cond => cond.AHIN_DeptProgMappingID == dpmId && cond.AHIN_AgencyHierarchyID == schoolNodeAssociationContract.AgencyHierarchyID))
                    {
                        AgencyHierarchyInstitutionNode agencyHierarchyInstitutionNode = new AgencyHierarchyInstitutionNode();
                        agencyHierarchyInstitutionNode.AHIN_DeptProgMappingID = dpmId;
                        agencyHierarchyInstitutionNode.AHIN_AgencyHierarchyID = schoolNodeAssociationContract.AgencyHierarchyID;
                        agencyHierarchyInstitutionNode.AHIN_IsDeleted = false;
                        agencyHierarchyInstitutionNode.AHIN_CreatedBy = schoolNodeAssociationContract.CurrentLoggedInUserID;
                        agencyHierarchyInstitutionNode.AHIN_CreatedOn = DateTime.Now;
                        _dbContext.AgencyHierarchyInstitutionNodes.AddObject(agencyHierarchyInstitutionNode);
                    }
                }

                List<AgencyHierarchyInstitutionNode> agencyHierarchyInstitutionNodeToBeRemoved = agencyHierarchyInstitutionNodeList.Where(cond => !schoolNodeAssociationContract.DPM_IDs
                                                                                                                 .Contains(cond.AHIN_DeptProgMappingID) && cond.AHIN_AgencyHierarchyID == schoolNodeAssociationContract.AgencyHierarchyID).ToList();

                foreach (AgencyHierarchyInstitutionNode agencyHierarchyInstitutionNode in agencyHierarchyInstitutionNodeToBeRemoved)
                {
                    agencyHierarchyInstitutionNode.AHIN_IsDeleted = true;
                    agencyHierarchyInstitutionNode.AHIN_ModifiedBy = schoolNodeAssociationContract.CurrentLoggedInUserID;
                    agencyHierarchyInstitutionNode.AHIN_ModifiedOn = DateTime.Now;
                }
            }

            if (_dbContext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        Boolean IAgencyHierarchyRepository.RemoveSchoolNodeAssociation(SchoolNodeAssociationContract schoolNodeAssociationContract)
        {

            List<AgencyHierarchyInstitutionNode> existingObject = _dbContext.AgencyHierarchyInstitutionNodes
                           .Where(cond => cond.AHIN_AgencyHierarchyID == schoolNodeAssociationContract.AgencyHierarchyID && (schoolNodeAssociationContract.DPM_IDs.Contains(cond.AHIN_DeptProgMappingID))
                                   && cond.AHIN_IsDeleted == false).ToList();

            if (existingObject.IsNotNull() && existingObject.Count > 0)
            {
                foreach (AgencyHierarchyInstitutionNode agencyHierarchyInstitutionNodeobject in existingObject)
                {
                    agencyHierarchyInstitutionNodeobject.AHIN_ModifiedBy = schoolNodeAssociationContract.CurrentLoggedInUserID;
                    agencyHierarchyInstitutionNodeobject.AHIN_ModifiedOn = DateTime.Now;
                    agencyHierarchyInstitutionNodeobject.AHIN_IsDeleted = true;
                }

                if (_dbContext.SaveChanges() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        Boolean IAgencyHierarchyRepository.IsSchoolNodeAssociationExists(Int32 agencyHierarchyID)
        {
            if (_dbContext.AgencyHierarchyInstitutionNodes
                    .Count(cond => cond.AHIN_AgencyHierarchyID == agencyHierarchyID
                                && cond.AHIN_IsDeleted == false) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void IAgencyHierarchyRepository.AddRemoveAgencyHierarchyTenantMapping(Int32 tenantID, Int32 agencyHierarchyID, Int32 currentLoggedInUserID, Boolean needToRemove, Boolean IsAdminShare, Boolean IsStudentShare)
        {

            var agencyHierarchyTenantMappingExistingObject = this.SharedDataDBContext.AgencyHierarchyTenantMappings
                                                    .FirstOrDefault(cond => cond.AHTM_TenantID == tenantID
                                                            && cond.AHTM_AgencyHierarchyID == agencyHierarchyID
                                                            && !cond.AHTM_IsDeleted);

            if (agencyHierarchyTenantMappingExistingObject.IsNullOrEmpty())
            {
                AgencyHierarchyTenantMapping agencyHierarchyTenantMapping = new AgencyHierarchyTenantMapping();
                agencyHierarchyTenantMapping.AHTM_TenantID = tenantID;
                agencyHierarchyTenantMapping.AHTM_AgencyHierarchyID = agencyHierarchyID;
                agencyHierarchyTenantMapping.AHTM_IsDeleted = false;
                agencyHierarchyTenantMapping.AHTM_CreatedBy = currentLoggedInUserID;
                agencyHierarchyTenantMapping.AHTM_CreatedOn = DateTime.Now;
                agencyHierarchyTenantMapping.AHTM_IsAdminShare = IsAdminShare;
                agencyHierarchyTenantMapping.AHTM_IsStudentShare = IsStudentShare;
                this.SharedDataDBContext.AgencyHierarchyTenantMappings.AddObject(agencyHierarchyTenantMapping);
            }
            else
            {
                agencyHierarchyTenantMappingExistingObject.AHTM_IsStudentShare = IsStudentShare;
                agencyHierarchyTenantMappingExistingObject.AHTM_IsAdminShare = IsAdminShare;
                agencyHierarchyTenantMappingExistingObject.AHTM_ModifiedBy = currentLoggedInUserID;
                agencyHierarchyTenantMappingExistingObject.AHTM_ModifiedOn = DateTime.Now;

                if (needToRemove)
                    agencyHierarchyTenantMappingExistingObject.AHTM_IsDeleted = true;
            }

            var agencyHierarchyProfilePermissionToUpdate = base.SharedDataDBContext.AgencyHierarchyAgencyProfileSharePermissions.FirstOrDefault(cond => cond.AHAPSP_TenantID == tenantID
                                                            && cond.AHAPSP_AgencyHierarchyID == agencyHierarchyID
                                                            && !cond.AHAPSP_IsDeleted);
            if (!agencyHierarchyProfilePermissionToUpdate.IsNullOrEmpty())
            {
                agencyHierarchyProfilePermissionToUpdate.AHAPSP_IsAdminShare = IsAdminShare;
                agencyHierarchyProfilePermissionToUpdate.AHAPSP_IsStudentShare = IsStudentShare;
                agencyHierarchyProfilePermissionToUpdate.AHAPSP_ModifiedByID = currentLoggedInUserID;
                agencyHierarchyProfilePermissionToUpdate.AHAPSP_ModifiedOn = DateTime.Now;
                if (needToRemove)
                    agencyHierarchyProfilePermissionToUpdate.AHAPSP_IsDeleted = true;
            }
            else
            {
                AgencyHierarchyAgencyProfileSharePermission agHrProfileSharePermission = new AgencyHierarchyAgencyProfileSharePermission();
                agHrProfileSharePermission.AHAPSP_TenantID = tenantID;
                agHrProfileSharePermission.AHAPSP_AgencyHierarchyID = agencyHierarchyID;
                agHrProfileSharePermission.AHAPSP_IsDeleted = false;
                agHrProfileSharePermission.AHAPSP_IsStudentShare = IsStudentShare;
                agHrProfileSharePermission.AHAPSP_IsAdminShare = IsAdminShare;
                agHrProfileSharePermission.AHAPSP_CreatedByID = currentLoggedInUserID;
                agHrProfileSharePermission.AHAPSP_CreatedOn = DateTime.Now;
                this.SharedDataDBContext.AgencyHierarchyAgencyProfileSharePermissions.AddObject(agHrProfileSharePermission);
            }

            this.SharedDataDBContext.SaveChanges();

        }

        #endregion

        #region UAT-2641
        List<Int32> IAgencyHierarchyRepository.GetAgencyHierarchyIdsByOrgUserID(Int32 OrgUserID)
        {
            List<Int32> _lstAgencyHierarchyIds = new List<Int32>();
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                base.OpenSQLDataReaderConnection(con);
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                      {
                           new SqlParameter("@orgUserID", OrgUserID)

                      };
                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHierarchyIdsByOrgUserId", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Int32 agencyHierarchyId = dr["AgencyHierarchyId"] == DBNull.Value ? AppConsts.NONE : Convert.ToInt32(dr["AgencyHierarchyId"]);
                            _lstAgencyHierarchyIds.Add(agencyHierarchyId);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstAgencyHierarchyIds;
        }
        #endregion

        #region UAT-2633: Agency Hierarchy Agency Mapping
        Boolean IAgencyHierarchyRepository.SaveAgencyHierarchyAgencyMapping(AgencyNodeMappingContract agencyNodeMappingContract)
        {
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var item in agencyNodeMappingContract.SelectedAgencyIDs)
                    {
                        AgencyHierarchyAgency dbInsert = new AgencyHierarchyAgency();
                        dbInsert.AHA_AgencyHierarchyID = agencyNodeMappingContract.AgencyHierarchyId;
                        dbInsert.AHA_CreatedBy = agencyNodeMappingContract.CurrentLoggedInUserID;
                        dbInsert.AHA_CreatedOn = DateTime.Now;
                        dbInsert.AHA_IsDeleted = false;
                        dbInsert.AHA_AgencyID = item;
                        if (!this.SharedDataDBContext.AgencyHierarchyAgencies.Where(cond => cond.AHA_AgencyID == item && cond.AHA_AgencyHierarchyID == agencyNodeMappingContract.AgencyHierarchyId && !cond.AHA_IsDeleted).Any())
                        {
                            this.SharedDataDBContext.AgencyHierarchyAgencies.AddObject(dbInsert);
                            this.SharedDataDBContext.SaveChanges();
                        }
                    }

                    transaction.Commit();
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
        Boolean IAgencyHierarchyRepository.DeleteAgencyHierarchyAgencyMapping(AgencyNodeMappingContract agencyNodeMappingContract)
        {
            Boolean returnStatus = false;
            AgencyHierarchyAgency agencyHierarchyAgency = SharedDataDBContext.AgencyHierarchyAgencies.Where(cond => cond.AHA_ID == agencyNodeMappingContract.AgencyHierarchyAgencyID).FirstOrDefault();
            if (agencyHierarchyAgency != null)
            {
                agencyHierarchyAgency.AHA_IsDeleted = true;
                SharedDataDBContext.SaveChanges();
                returnStatus = true;
            }
            return returnStatus;
        }

        Boolean IAgencyHierarchyRepository.DeleteAgencyHierarchySetting(Int32 agencyID, Int32 currentLoggedInUserID, String SettingType)
        {
            AgencyHierarchySetting agencyHierarchySetting = SharedDataDBContext.AgencyHierarchySettings.Where(cond => cond.AHS_AgencyID == agencyID && !cond.AHS_IsDeleted && cond.lkpAgencyHierarchySetting.S_Code == SettingType).FirstOrDefault();

            if (agencyHierarchySetting != null)
            {
                agencyHierarchySetting.AHS_IsDeleted = true;
                agencyHierarchySetting.AHS_ModifiedOn = DateTime.Now;
                agencyHierarchySetting.AHS_ModifiedBy = currentLoggedInUserID;
                SharedDataDBContext.SaveChanges();
                return true;
            }

            return false;
        }


        Boolean IAgencyHierarchyRepository.IsAgencyHierarchyLeafNode(Int32 AgencyHierarchyID)
        {
            if (SharedDataDBContext.AgencyHierarchies.Where(a => a.AH_ParentID == AgencyHierarchyID && !a.AH_IsDeleted).Any())
                return false;
            else
                return true;
        }
        List<AgencyNodeMappingContract> IAgencyHierarchyRepository.GetAgencies(Int32 agencyHierarchyID)
        {
            var _lstAgencies = new List<AgencyNodeMappingContract>();

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyHierarchyID", agencyHierarchyID)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgenciesForAgencyHierarchy", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            AgencyNodeMappingContract agency = new AgencyNodeMappingContract();
                            agency.AgencyID = dr["AG_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AG_ID"]);
                            agency.AgencyName = dr["AG_Name"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AG_Name"]);
                            _lstAgencies.Add(agency);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }


            //var agencyList = this.SharedDataDBContext.Agencies.Where(cond => !cond.AG_IsDeleted).Select(sel => new { sel.AG_ID, sel.AG_Name, sel.AG_Description }).ToList();
            //_lstAgencies = agencyList.Select(sel => new AgencyNodeMappingContract()
            //{
            //    AgencyID = sel.AG_ID,
            //    AgencyName = sel.AG_Name
            //}).ToList();
            return _lstAgencies;
        }
        List<AgencyNodeMappingContract> IAgencyHierarchyRepository.GetAgencyHierarchyAgencies(Int32 agencyHierarchyID)
        {
            var _lstAgencies = new List<AgencyNodeMappingContract>();
            //var agencyList = this.SharedDataDBContext.AgencyHierarchyAgencies.Where(cond => !cond.AHA_IsDeleted && cond.AHA_AgencyHierarchyID == agencyHierarchyID).Select(sel => new { sel.AHA_ID, sel.Agency.AG_ID, sel.Agency.AG_Name }).ToList();
            //var result = this.SharedDataDBContext.AgencyHierarchies.Where(cond => !cond.AH_IsDeleted && cond.AH_ID == agencyHierarchyID).FirstOrDefault();
            //if (result.IsNotNull() && !result.IsNullOrEmpty())
            //{
            //    var lkpcode = "AAAB";
            //    var agencyhierarchyagency = result.AgencyHierarchyAgencies.Where(cond => !cond.AHA_IsDeleted).Select(sel => new { sel.AHA_ID, sel.Agency.AG_ID, sel.Agency.AG_Name }).ToList();
            //    var agencyhierarchySetting = result.AgencyHierarchySettings.Where(cond => !cond.AHS_IsDeleted && cond.lkpAgencyHierarchySetting.S_Code == lkpcode).Select(sel => new { sel.AHS_AgencyHierarchyID, sel.AHS_SettingValue }).ToList();

            //    _lstAgencies = agencyhierarchyagency.Select(sel => new AgencyNodeMappingContract()
            //    {
            //        AgencyHierarchyAgencyID = sel.AHA_ID,
            //        AgencyID = sel.AG_ID,
            //        AgencyName = sel.AG_Name,
            //        AttestationformSettingValue = agencyhierarchySetting.Where(cond => cond.AHS_AgencyHierarchyID == sel.AHA_ID).Select(s => s.AHS_SettingValue).FirstOrDefault()
            //    }).ToList();
            //}

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyHierarchyID", agencyHierarchyID)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyAttestationDocumentSettings", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            AgencyNodeMappingContract agency = new AgencyNodeMappingContract();
                            agency.AgencyID = dr["AG_ID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AG_ID"]);
                            agency.AgencyName = dr["AG_Name"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["AG_Name"]);
                            agency.AgencyHierarchyAgencyID = Convert.ToInt32(dr["AHA_ID"]);
                            agency.AttestationformSettingValue = dr["SettingValue"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["SettingValue"]);
                            agency.AttestationDocumentID = dr["DocumentID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["DocumentID"]);
                            agency.AttestationFileName = dr["FileName"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["FileName"]);
                            agency.AttestationDocumentPath = dr["DocumentPath"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["DocumentPath"]);
                            _lstAgencies.Add(agency);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);

                return _lstAgencies;
            }
        }
        #endregion

        #region [Agency Hierarchy Profile Share Permission]

        List<AgencyHierarchyProfileSharePermissionDataContract> IAgencyHierarchyRepository.GetProfileSharePermissionByAgencyHierarchyID(Int32 agencyHierarchyID)
        {

            List<AgencyHierarchyProfileSharePermissionDataContract> lstAgencyHierarchyProfileSharePermissionDataContract = new List<AgencyHierarchyProfileSharePermissionDataContract>();

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {

                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                      {
                           new SqlParameter("@AgencyHierarchyID", agencyHierarchyID)

                      };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetProfileSharePermissionByAgencyHierarchyID", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            AgencyHierarchyProfileSharePermissionDataContract agencyHierarchyProfileSharePermissionDataContract = new AgencyHierarchyProfileSharePermissionDataContract();
                            agencyHierarchyProfileSharePermissionDataContract.TenantID = dr["TenantID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TenantID"]);
                            agencyHierarchyProfileSharePermissionDataContract.TenantName = dr["TenantName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TenantName"]);
                            agencyHierarchyProfileSharePermissionDataContract.AgencyHierarchyAgencyProfileSharePermissionsID = dr["AgencyHierarchyAgencyProfileSharePermissionsID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyHierarchyAgencyProfileSharePermissionsID"]);
                            agencyHierarchyProfileSharePermissionDataContract.IsAdminShare = dr["IsAdminShare"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsAdminShare"]);
                            agencyHierarchyProfileSharePermissionDataContract.IsStudentShare = dr["IsStudentShare"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsStudentShare"]);
                            agencyHierarchyProfileSharePermissionDataContract.AgencyHierarchyID = dr["AgencyHierarchyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyHierarchyID"]);
                            lstAgencyHierarchyProfileSharePermissionDataContract.Add(agencyHierarchyProfileSharePermissionDataContract);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return lstAgencyHierarchyProfileSharePermissionDataContract;
        }

        Boolean IAgencyHierarchyRepository.SaveUpdateSchoolNodeAssociation(AgencyHierarchyProfileSharePermissionDataContract agencyHierarchyProfileSharePermissionDataContract)
        {
            if (agencyHierarchyProfileSharePermissionDataContract.AgencyHierarchyAgencyProfileSharePermissionsID == AppConsts.NONE)
            {
                AgencyHierarchyAgencyProfileSharePermission agencyHierarchyAgencyProfileSharePermission = new AgencyHierarchyAgencyProfileSharePermission();
                agencyHierarchyAgencyProfileSharePermission.AHAPSP_AgencyHierarchyID = agencyHierarchyProfileSharePermissionDataContract.AgencyHierarchyID;
                agencyHierarchyAgencyProfileSharePermission.AHAPSP_TenantID = agencyHierarchyProfileSharePermissionDataContract.TenantID;
                agencyHierarchyAgencyProfileSharePermission.AHAPSP_IsStudentShare = agencyHierarchyProfileSharePermissionDataContract.IsStudentShare;
                agencyHierarchyAgencyProfileSharePermission.AHAPSP_IsAdminShare = agencyHierarchyProfileSharePermissionDataContract.IsAdminShare;
                agencyHierarchyAgencyProfileSharePermission.AHAPSP_IsDeleted = false;
                agencyHierarchyAgencyProfileSharePermission.AHAPSP_CreatedByID = agencyHierarchyProfileSharePermissionDataContract.CurrentLoggedInUserID;
                agencyHierarchyAgencyProfileSharePermission.AHAPSP_CreatedOn = DateTime.Now;
                this.SharedDataDBContext.AgencyHierarchyAgencyProfileSharePermissions.AddObject(agencyHierarchyAgencyProfileSharePermission);
            }
            else
            {
                var existingObject = this.SharedDataDBContext.AgencyHierarchyAgencyProfileSharePermissions
                                        .Where(cond => cond.AHAPSP_ID == agencyHierarchyProfileSharePermissionDataContract.AgencyHierarchyAgencyProfileSharePermissionsID
                                            && !cond.AHAPSP_IsDeleted).FirstOrDefault();
                if (!existingObject.IsNullOrEmpty())
                {
                    existingObject.AHAPSP_AgencyHierarchyID = agencyHierarchyProfileSharePermissionDataContract.AgencyHierarchyID;
                    existingObject.AHAPSP_TenantID = agencyHierarchyProfileSharePermissionDataContract.TenantID;
                    existingObject.AHAPSP_IsStudentShare = agencyHierarchyProfileSharePermissionDataContract.IsStudentShare;
                    existingObject.AHAPSP_IsAdminShare = agencyHierarchyProfileSharePermissionDataContract.IsAdminShare;
                    existingObject.AHAPSP_IsDeleted = false;
                    existingObject.AHAPSP_ModifiedByID = agencyHierarchyProfileSharePermissionDataContract.CurrentLoggedInUserID;
                    existingObject.AHAPSP_ModifiedOn = DateTime.Now;
                }
            }
            if (this.SharedDataDBContext.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        Boolean IAgencyHierarchyRepository.RemoveProfileSharePermission(AgencyHierarchyProfileSharePermissionDataContract agencyHierarchyProfileSharePermissionDataContract)
        {
            var existingObject = this.SharedDataDBContext.AgencyHierarchyAgencyProfileSharePermissions
                                           .Where(cond => cond.AHAPSP_ID == agencyHierarchyProfileSharePermissionDataContract.AgencyHierarchyAgencyProfileSharePermissionsID
                                               && !cond.AHAPSP_IsDeleted).FirstOrDefault();

            if (!existingObject.IsNullOrEmpty())
            {
                existingObject.AHAPSP_ModifiedByID = agencyHierarchyProfileSharePermissionDataContract.CurrentLoggedInUserID;
                existingObject.AHAPSP_ModifiedOn = DateTime.Now;
                existingObject.AHAPSP_IsDeleted = true;

                if (this.SharedDataDBContext.SaveChanges() > 0)
                    return true;
                else
                    return false;
            }
            return false;
        }

        #endregion

        #region [UAT-2653]

        List<Int32> IAgencyHierarchyRepository.GetAgencyHiearchyIdsByDeptProgMappingID(String DPM_IDs)
        {
            var lstAgencyHieararchyIDs = new List<Int32>();

            EntityConnection connection = this._dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                   {
                           new SqlParameter("@DPM_Ids", DPM_IDs)

                   };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHiearchyIdsByDeptProgMappingID", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lstAgencyHieararchyIDs.Add(dr["AgencyHierarchyID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["AgencyHierarchyID"]));
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return lstAgencyHieararchyIDs;
        }

        #endregion

        #region UAT-2647
        List<Int32> IAgencyHierarchyRepository.GetAgencyHierarchyIdsByTenantID(Int32 TenantID)
        {
            return SharedDataDBContext.AgencyHierarchyTenantMappings.Where(cmd => cmd.AHTM_TenantID == TenantID && !cmd.AHTM_IsDeleted)
                .Select(sel => sel.AHTM_AgencyHierarchyID).Distinct().ToList();
        }
        #endregion

        #region UAT-3245
        List<Int32> IAgencyHierarchyRepository.GetAgencyHierarchyIdsByLstTenantIDs(List<Int32> lstTenantIds)
        {
            return SharedDataDBContext.AgencyHierarchyTenantMappings.Where(cmd => lstTenantIds.Contains(cmd.AHTM_TenantID) && !cmd.AHTM_IsDeleted)
                .Select(sel => sel.AHTM_AgencyHierarchyID).Distinct().ToList();
        }
        #endregion

        #region Digestion Process

        void IAgencyHierarchyRepository.CallDigestionProcedure(Dictionary<String, Object> param)
        {
            //call digestion SP
            String AgencyHierarchyId;
            String ChangeType;
            Int32 CurrentUserId;
            List<Int32> lstAgencyHierarchyID = new List<Int32>();

            param.TryGetValue("AgencyHierarchyId", out AgencyHierarchyId);
            param.TryGetValue("ChangeType", out ChangeType);
            param.TryGetValue("CurrentUserId", out CurrentUserId);

            lstAgencyHierarchyID = AgencyHierarchyId.Split(',').ConvertIntoIntList();

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                foreach (Int32 AgencyHierarchyID in lstAgencyHierarchyID.Distinct())
                {
                    SqlCommand command = new SqlCommand("usp_AgencyHierachyDigestion", con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AgencyHierarchyId", AgencyHierarchyID);
                    command.Parameters.AddWithValue("@ChangeType", ChangeType);
                    command.Parameters.AddWithValue("@CurrentUserId", CurrentUserId);
                    command.ExecuteScalar();
                }
                con.Close();
            }
        }

        #endregion

        String IAgencyHierarchyRepository.GetAgencyHiearchyIdsByTenantID(Int32 TenantID)
        {
            List<Int32> agencyHierarchyIds = SharedDataDBContext.AgencyHierarchyTenantMappings.Where(con => con.AHTM_TenantID == TenantID && !con.AHTM_IsDeleted).Select(sel => sel.AHTM_AgencyHierarchyID).ToList();
            return agencyHierarchyIds.IsNullOrEmpty() ? String.Empty : String.Join(",", agencyHierarchyIds);
        }

        #region UAT-2548
        Boolean IAgencyHierarchyRepository.SaveUpdateAgencyHierarchyTenantAccessMapping(Int32 AgencyHierarchyId, List<Int32> lstTenantIds, Int32 CurrentLoggedInUserId)
        {
            AgencyHierarchyTenantAccessMapping AddNewRecord = null;

            List<AgencyHierarchyTenantAccessMapping> lstAgencyTenantMapping = this.SharedDataDBContext.AgencyHierarchyTenantAccessMappings
                                                        .Where(cond => cond.AHTAM_AgencyHierarchyID == AgencyHierarchyId && !cond.AHTAM_IsDeleted).ToList();
            if (lstTenantIds.Count > AppConsts.NONE)
            {
                this.SharedDataDBContext.AgencyHierarchyTenantAccessMappings.Where(cond => cond.AHTAM_AgencyHierarchyID == AgencyHierarchyId && !cond.AHTAM_IsDeleted
                                                                && !lstTenantIds.Contains(cond.AHTAM_TenantID)).ForEach(x =>
                {
                    x.AHTAM_IsDeleted = true;
                    x.AHTAM_ModifiedBy = CurrentLoggedInUserId;
                    x.AHTAM_ModifiedOn = DateTime.Now;
                });

                foreach (Int32 tenantId in lstTenantIds)
                {
                    if (!lstAgencyTenantMapping.Where(cond => cond.AHTAM_TenantID == tenantId && !cond.AHTAM_IsDeleted).Any())
                    {
                        AddNewRecord = new AgencyHierarchyTenantAccessMapping();
                        AddNewRecord.AHTAM_TenantID = tenantId;
                        AddNewRecord.AHTAM_AgencyHierarchyID = AgencyHierarchyId;
                        AddNewRecord.AHTAM_IsDeleted = false;
                        AddNewRecord.AHTAM_CreatedBy = CurrentLoggedInUserId;
                        AddNewRecord.AHTAM_CreatedOn = DateTime.Now;

                        this.SharedDataDBContext.AgencyHierarchyTenantAccessMappings.AddObject(AddNewRecord);
                    }
                }
            }
            else
            {
                this.SharedDataDBContext.AgencyHierarchyTenantAccessMappings.Where(cond => cond.AHTAM_AgencyHierarchyID == AgencyHierarchyId && !cond.AHTAM_IsDeleted).ForEach(x =>
                            {
                                x.AHTAM_IsDeleted = true;
                                x.AHTAM_ModifiedBy = CurrentLoggedInUserId;
                                x.AHTAM_ModifiedOn = DateTime.Now;
                            });
            }

            if (this.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
                return true;
            return false;
        }
        List<Int32> IAgencyHierarchyRepository.GetAgencyHierarchyTenantAccessDetails(Int32 AgencyHierarchyId)
        {
            return this.SharedDataDBContext.AgencyHierarchyTenantAccessMappings.Where(cond => cond.AHTAM_AgencyHierarchyID == AgencyHierarchyId
                                        && !cond.AHTAM_IsDeleted).Select(sel => sel.AHTAM_TenantID).Distinct().ToList();
        }
        #endregion

        #region UAT-2712
        AgencyHierarchyRotationFieldOptionContract IAgencyHierarchyRepository.GetAgencyHierarchyRotationFieldOptionSetting(Int32 agencyHierarchyID)
        {
            AgencyHierarchyRotationFieldOptionContract agencyHierarchyRotationFieldOptionContract = new AgencyHierarchyRotationFieldOptionContract();
            Boolean DefaultSetting = false;

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyHierarchyID", agencyHierarchyID)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHierarchyRotationFieldOptionSettingById", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            agencyHierarchyRotationFieldOptionContract.AgencyHierarchyID = dr["AHRFO_AgencyHierarchyID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["AHRFO_AgencyHierarchyID"]);
                            agencyHierarchyRotationFieldOptionContract.IsRootNode = dr["AH_ParentID"].GetType().Name == "DBNull" ? true : false;
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsCourse_Required = dr["AHRFO_IsCourse_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsCourse_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsDaysBefore_Required = dr["AHRFO_IsDaysBefore_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsDaysBefore_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsDeadlineDate_Required = dr["AHRFO_IsDeadlineDate_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsDeadlineDate_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsDepartment_Required = dr["AHRFO_IsDepartment_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsDepartment_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsEndTime_Required = dr["AHRFO_IsEndTime_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsEndTime_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsFrequency_Required = dr["AHRFO_IsFrequency_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsFrequency_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsIP_Required = dr["AHRFO_IsIP_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsIP_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsNoOfHours_Required = dr["AHRFO_IsNoOfHours_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsNoOfHours_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsNoOfStudents_Required = dr["AHRFO_IsNoOfStudents_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsNoOfStudents_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsProgram_Required = dr["AHRFO_IsProgram_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsProgram_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsRotationName_Required = dr["AHRFO_IsRotationName_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsRotationName_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsRotationShift_Required = dr["AHRFO_IsRotationShift_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsRotationShift_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsRotDays_Required = dr["AHRFO_IsRotDays_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsRotDays_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsStartTime_Required = dr["AHRFO_IsStartTime_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsStartTime_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsSyllabusDocument_Required = dr["AHRFO_IsSyllabusDocument_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsSyllabusDocument_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsTerm_Required = dr["AHRFO_IsTerm_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsTerm_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsTypeSpecialty_Required = dr["AHRFO_IsTypeSpecialty_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsTypeSpecialty_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsUnitFloorLoc_Required = dr["AHRFO_IsUnitFloorLoc_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsUnitFloorLoc_Required"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_CheckParentSetting = dr["AHRFO_CheckParentSetting"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_CheckParentSetting"]);
                            agencyHierarchyRotationFieldOptionContract.AHRFO_IsAdditionalDocuments_Required = dr["AHRFO_IsAdditionalDocuments_Required"].GetType().Name == "DBNull" ? DefaultSetting : Convert.ToBoolean(dr["AHRFO_IsAdditionalDocuments_Required"]);
                        }
                    }

                }
                base.CloseSQLDataReaderConnection(con);
            }








            //var agencyHierarchyRotationFieldOptionSettingExistChk = this.SharedDataDBContext.AgencyHierarchyRotationFieldOptions.Where(cond => cond.AHRFO_AgencyHierarchyID == agencyHierarchyID && !cond.AHRFO_IsDeleted)
            //    .Select(sel => new {agencyHierarchyRotationFieldOptionSetting = sel, ParentId =sel.AgencyHierarchy.AH_ParentID }).FirstOrDefault();
            //if (!agencyHierarchyRotationFieldOptionSettingExistChk.IsNullOrEmpty())
            //{
            //    agencyHierarchyRotationFieldOptionContract.AgencyHierarchyID = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_AgencyHierarchyID;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsCourse_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsCourse_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsDaysBefore_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsDaysBefore_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsDeadlineDate_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsDeadlineDate_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsDepartment_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsDepartment_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsEndTime_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsEndTime_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsFrequency_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsFrequency_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsIP_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsIP_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsNoOfHours_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsNoOfHours_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsNoOfStudents_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsNoOfStudents_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsProgram_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsProgram_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsRotationName_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsRotationName_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsRotationShift_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsRotationShift_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsRotDays_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsRotDays_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsStartTime_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsStartTime_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsSyllabusDocument_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsSyllabusDocument_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsTerm_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsTerm_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsTypeSpecialty_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsTypeSpecialty_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_IsUnitFloorLoc_Required = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_IsUnitFloorLoc_Required;
            //    agencyHierarchyRotationFieldOptionContract.AHRFO_CheckParentSetting = agencyHierarchyRotationFieldOptionSettingExistChk.agencyHierarchyRotationFieldOptionSetting.AHRFO_CheckParentSetting;
            //    agencyHierarchyRotationFieldOptionContract.IsRootNode = agencyHierarchyRotationFieldOptionSettingExistChk.ParentId.IsNull() ? true : false;
            //}
            return agencyHierarchyRotationFieldOptionContract;
        }
        Boolean IAgencyHierarchyRepository.SaveAgencyHierarchyRotationFieldOptionSetting(AgencyHierarchyRotationFieldOptionContract agencyHierarchyRotationFieldOptionContract)
        {
            //update
            var dbUpdate = this.SharedDataDBContext.AgencyHierarchyRotationFieldOptions.Where(cond => !cond.AHRFO_IsDeleted && cond.AHRFO_AgencyHierarchyID == agencyHierarchyRotationFieldOptionContract.AgencyHierarchyID).FirstOrDefault();
            if (!dbUpdate.IsNullOrEmpty())
            {
                if (!agencyHierarchyRotationFieldOptionContract.AHRFO_CheckParentSetting)
                {
                    dbUpdate.AHRFO_AgencyHierarchyID = agencyHierarchyRotationFieldOptionContract.AgencyHierarchyID;
                    dbUpdate.AHRFO_IsCourse_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsCourse_Required;
                    dbUpdate.AHRFO_IsDaysBefore_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsDaysBefore_Required;
                    dbUpdate.AHRFO_IsDeadlineDate_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsDeadlineDate_Required;
                    dbUpdate.AHRFO_IsDepartment_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsDepartment_Required;
                    dbUpdate.AHRFO_IsEndTime_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsEndTime_Required;
                    dbUpdate.AHRFO_IsFrequency_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsFrequency_Required;
                    dbUpdate.AHRFO_IsIP_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsIP_Required;
                    dbUpdate.AHRFO_IsNoOfHours_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsNoOfHours_Required;
                    dbUpdate.AHRFO_IsNoOfStudents_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsNoOfStudents_Required;
                    dbUpdate.AHRFO_IsProgram_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsProgram_Required;
                    dbUpdate.AHRFO_IsRotationName_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsRotationName_Required;
                    dbUpdate.AHRFO_IsRotationShift_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsRotationShift_Required;
                    dbUpdate.AHRFO_IsRotDays_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsRotDays_Required;
                    dbUpdate.AHRFO_IsStartTime_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsStartTime_Required;
                    dbUpdate.AHRFO_IsSyllabusDocument_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsSyllabusDocument_Required;
                    dbUpdate.AHRFO_IsTerm_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsTerm_Required;
                    dbUpdate.AHRFO_IsTypeSpecialty_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsTypeSpecialty_Required;
                    dbUpdate.AHRFO_IsUnitFloorLoc_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsUnitFloorLoc_Required;
                    dbUpdate.AHRFO_CheckParentSetting = false;
                    dbUpdate.AHRFO_IsAdditionalDocuments_Required = Convert.ToInt32(agencyHierarchyRotationFieldOptionContract.AHRFO_IsAdditionalDocuments_Required);
                }
                else
                {
                    dbUpdate.AHRFO_CheckParentSetting = true;
                }
                dbUpdate.AHRFO_ModifiedByID = agencyHierarchyRotationFieldOptionContract.CurrentLoggedInUser;
                dbUpdate.AHRFO_ModifiedOn = DateTime.Now;
            }
            else
            {
                //insert
                AgencyHierarchyRotationFieldOption dbInsert = new AgencyHierarchyRotationFieldOption();
                dbInsert.AHRFO_AgencyHierarchyID = agencyHierarchyRotationFieldOptionContract.AgencyHierarchyID;
                dbInsert.AHRFO_CheckParentSetting = agencyHierarchyRotationFieldOptionContract.AHRFO_CheckParentSetting;
                dbInsert.AHRFO_IsCourse_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsCourse_Required;
                dbInsert.AHRFO_IsDaysBefore_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsDaysBefore_Required;
                dbInsert.AHRFO_IsDeadlineDate_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsDeadlineDate_Required;
                dbInsert.AHRFO_IsDepartment_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsDepartment_Required;
                dbInsert.AHRFO_IsEndTime_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsEndTime_Required;
                dbInsert.AHRFO_IsFrequency_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsFrequency_Required;
                dbInsert.AHRFO_IsIP_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsIP_Required;
                dbInsert.AHRFO_IsNoOfHours_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsNoOfHours_Required;
                dbInsert.AHRFO_IsNoOfStudents_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsNoOfStudents_Required;
                dbInsert.AHRFO_IsProgram_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsProgram_Required;
                dbInsert.AHRFO_IsRotationName_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsRotationName_Required;
                dbInsert.AHRFO_IsRotationShift_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsRotationShift_Required;
                dbInsert.AHRFO_IsRotDays_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsRotDays_Required;
                dbInsert.AHRFO_IsStartTime_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsStartTime_Required;
                dbInsert.AHRFO_IsSyllabusDocument_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsSyllabusDocument_Required;
                dbInsert.AHRFO_IsTerm_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsTerm_Required;
                dbInsert.AHRFO_IsTypeSpecialty_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsTypeSpecialty_Required;
                dbInsert.AHRFO_IsUnitFloorLoc_Required = agencyHierarchyRotationFieldOptionContract.AHRFO_IsUnitFloorLoc_Required;
                dbInsert.AHRFO_IsAdditionalDocuments_Required = Convert.ToInt32(agencyHierarchyRotationFieldOptionContract.AHRFO_IsAdditionalDocuments_Required);
                dbInsert.AHRFO_CreatedOn = DateTime.Now;
                dbInsert.AHRFO_CreatedByID = agencyHierarchyRotationFieldOptionContract.CurrentLoggedInUser;
                dbInsert.AHRFO_IsDeleted = false;
                this.SharedDataDBContext.AgencyHierarchyRotationFieldOptions.AddObject(dbInsert);
            }

            if (this.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            else
                return false;
        }
        //Boolean IAgencyHierarchyRepository.SaveDefaultAgencyHierarchyRotationFieldOptionSetting(Int32 agencyHierarchyID, Int32 currentLoggedInUser)
        //{
        //    AgencyHierarchyRotationFieldOption dbInsert = new AgencyHierarchyRotationFieldOption();
        //    dbInsert.AHRFO_AgencyHierarchyID = agencyHierarchyID;
        //    dbInsert.AHRFO_CheckParentSetting = false;
        //    dbInsert.AHRFO_IsCourse_Required = true;
        //    dbInsert.AHRFO_IsDepartment_Required = true;
        //    dbInsert.AHRFO_IsProgram_Required = true;
        //    dbInsert.AHRFO_CreatedOn = DateTime.Now;
        //    dbInsert.AHRFO_CreatedByID = currentLoggedInUser;
        //    dbInsert.AHRFO_IsDeleted = false;
        //    this.SharedDataDBContext.AgencyHierarchyRotationFieldOptions.AddObject(dbInsert);


        //    if (this.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
        //    {
        //        return true;
        //    }
        //    else
        //        return false;
        //}
        #endregion

        #region UAT-2784
        AgencyHierarchySettingContract IAgencyHierarchyRepository.GetAgencyHierarchySetting(Int32 agencyHierarchyID, String settingTypeCode)
        {
            AgencyHierarchySettingContract agencyHierarchySettingContract = new AgencyHierarchySettingContract();
            Boolean DefaultSetting = true;

            var agencyHierarchy = this.SharedDataDBContext.AgencyHierarchies.Where(cond => cond.AH_ID == agencyHierarchyID && !cond.AH_IsDeleted).FirstOrDefault();
            if (!agencyHierarchy.IsNullOrEmpty())
            {
                var agencyHierarchySetting = agencyHierarchy.AgencyHierarchySettings.Where(cond => !cond.AHS_IsDeleted).ToList();
                if (agencyHierarchySetting.Count > 0)
                {
                    var codeSpecificSetting = agencyHierarchySetting.Where(con => con.lkpAgencyHierarchySetting.S_Code == settingTypeCode && con.AHS_AgencyID.IsNullOrEmpty()).FirstOrDefault();
                    if (codeSpecificSetting.IsNotNull())
                    {
                        agencyHierarchySettingContract = new AgencyHierarchySettingContract
                        {
                            IsRootNode = agencyHierarchy.AH_ParentID.IsNullOrEmpty() ? true : false,
                            AgencyHierarchyID = agencyHierarchyID,
                            CheckParentSetting = codeSpecificSetting.AHS_CheckParentSetting,
                            IsExpirationCriteria = codeSpecificSetting.AHS_SettingValue == AppConsts.ZERO ? false : true,
                            IsRotationArchivedAutomatically = (settingTypeCode == AgencyHierarchySettingType.AUTOMATICALLY_ARCHIVED_ROTATION.GetStringValue() && codeSpecificSetting.AHS_SettingValue == AppConsts.STR_ONE) ? true : false,
                            SettingValue = codeSpecificSetting.AHS_SettingValue,
                            SettingTypeCode = codeSpecificSetting.lkpAgencyHierarchySetting.S_Code
                        };
                    }
                    return agencyHierarchySettingContract;
                }

            }

            return new AgencyHierarchySettingContract { AgencyHierarchyID = AppConsts.NONE, CheckParentSetting = DefaultSetting, IsExpirationCriteria = DefaultSetting, IsRootNode = agencyHierarchy.AH_ParentID.IsNullOrEmpty() ? true : false };
        }
        Boolean IAgencyHierarchyRepository.SaveAgencyHierarchySetting(AgencyHierarchySettingContract agencyHierarchySettingContract)
        {
            //update
            List<AgencyHierarchySetting> dbUpdateAgencyHierarchySettings = new List<AgencyHierarchySetting>();
            if (!agencyHierarchySettingContract.SettingTypeCode.IsNullOrEmpty())
            {
                Boolean isAgencyIDPassed = agencyHierarchySettingContract.AgencyID.HasValue && agencyHierarchySettingContract.AgencyID.Value > 0 ? true : false;

                dbUpdateAgencyHierarchySettings = this.SharedDataDBContext.AgencyHierarchySettings.Where(cond => !cond.AHS_IsDeleted && cond.AHS_AgencyHierarchyID == agencyHierarchySettingContract.AgencyHierarchyID
                                        && cond.lkpAgencyHierarchySetting.S_Code == agencyHierarchySettingContract.SettingTypeCode
                                        && ((isAgencyIDPassed == true && cond.AHS_AgencyID == agencyHierarchySettingContract.AgencyID.Value)
                                                || (isAgencyIDPassed == false && !cond.AHS_AgencyID.HasValue))).ToList();
            }
            //else
            //{
            //    dbUpdateAgencyHierarchySettings = this.SharedDataDBContext.AgencyHierarchySettings.Where(cond => !cond.AHS_IsDeleted && cond.AHS_AgencyHierarchyID == agencyHierarchySettingContract.AgencyHierarchyID).ToList();
            //}

            if (dbUpdateAgencyHierarchySettings.Count > 0)
            {
                dbUpdateAgencyHierarchySettings.ForEach(s =>
                {
                    if (!agencyHierarchySettingContract.CheckParentSetting)
                    {
                        s.AHS_CheckParentSetting = false;
                    }
                    else
                        s.AHS_CheckParentSetting = true;

                    s.AHS_ModifiedBy = agencyHierarchySettingContract.CurrentLoggedInUser;
                    s.AHS_ModifiedOn = DateTime.Now;
                    //Update setting based on code
                    if (s.lkpAgencyHierarchySetting.S_Code == AgencyHierarchySettingType.EXPIRATION_CRITERIA.GetStringValue())
                    {
                        s.AHS_SettingValue = agencyHierarchySettingContract.IsExpirationCriteria ? AppConsts.STR_ONE : AppConsts.ZERO;
                    }
                    else if (s.lkpAgencyHierarchySetting.S_Code == AgencyHierarchySettingType.SPECIFIC_ATTESTATION_FORM.GetStringValue())
                    {
                        s.AHS_SettingValue = agencyHierarchySettingContract.SettingValue;
                    }
                    else if (s.lkpAgencyHierarchySetting.S_Code == AgencyHierarchySettingType.AUTOMATICALLY_ARCHIVED_ROTATION.GetStringValue())
                    {
                        s.AHS_SettingValue = agencyHierarchySettingContract.IsRotationArchivedAutomatically ? AppConsts.STR_ONE : AppConsts.ZERO;
                    }
                    else if (s.lkpAgencyHierarchySetting.S_Code == AgencyHierarchySettingType.INSTPRECEPTOR_MANDATORY_FOR_INDIVIDUAL_SHARE.GetStringValue())
                    {
                        s.AHS_SettingValue = agencyHierarchySettingContract.SettingValue;
                    }
                    else if (s.lkpAgencyHierarchySetting.S_Code == AgencyHierarchySettingType.IS_NODE_AVAILABLE_FOR_ROTATION.GetStringValue()) // UAT-4443
                    {
                        s.AHS_SettingValue = agencyHierarchySettingContract.SettingValue;
                    }
                    //Start UAT-4673
                    else if (s.lkpAgencyHierarchySetting.S_Code == AgencyHierarchySettingType.UPDATE_REVIEW_STATUS.GetStringValue())
                    {
                        s.AHS_SettingValue = agencyHierarchySettingContract.SettingValue;
                    }
                    //End UAT-4673
                });
            }
            else
            {
                Int32 SettingID = this.SharedDataDBContext.lkpAgencyHierarchySettings.Where(cond => !cond.S_IsDeleted && cond.S_Code == agencyHierarchySettingContract.SettingTypeCode).Select(sel => sel.S_ID).FirstOrDefault();

                //insert Expiration Category entry
                if (agencyHierarchySettingContract.SettingValue.IsNullOrEmpty())
                {
                    AddAgencyHierarchySetting(agencyHierarchySettingContract.AgencyHierarchyID, agencyHierarchySettingContract.CheckParentSetting, SettingID, agencyHierarchySettingContract.IsExpirationCriteria ? AppConsts.STR_ONE : AppConsts.ZERO, agencyHierarchySettingContract.CurrentLoggedInUser);
                }
                else
                {
                    AddAgencyHierarchySetting(agencyHierarchySettingContract.AgencyHierarchyID, agencyHierarchySettingContract.CheckParentSetting, SettingID, agencyHierarchySettingContract.SettingValue, agencyHierarchySettingContract.CurrentLoggedInUser, agencyHierarchySettingContract.AgencyID);
                }

            }

            #region Call Digestion SP

            #endregion

            if (this.SharedDataDBContext.SaveChanges() > AppConsts.NONE)
            {
                return true;
            }
            else
                return false;
        }

        private void AddAgencyHierarchySetting(Int32 agencyHierarchyID, Boolean checkParentSetting, Int32 settingID, String settingValue, Int32 currentLoggedInUser, Int32? agencyID = null)
        {
            AgencyHierarchySetting dbInsert = new AgencyHierarchySetting();
            dbInsert.AHS_AgencyHierarchyID = agencyHierarchyID;
            dbInsert.AHS_CheckParentSetting = checkParentSetting;
            dbInsert.AHS_SettingID = settingID;
            dbInsert.AHS_SettingValue = settingValue;
            dbInsert.AHS_AgencyID = agencyID;
            dbInsert.AHS_CreatedOn = DateTime.Now;
            dbInsert.AHS_CreatedBy = currentLoggedInUser;
            dbInsert.AHS_IsDeleted = false;
            this.SharedDataDBContext.AgencyHierarchySettings.AddObject(dbInsert);
        }

        #endregion

        #region UAT-2982: Delete Mappings When Hierarchy is Deleted
        void IAgencyHierarchyRepository.DeletingAgencyHierarchyMappings(Int32 agencyHierarchyId, Int32 currentLoggedInUserID)
        {

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("usp_DeleteAgencyHierarchyMappings", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyHierarchyId", agencyHierarchyId);
                command.Parameters.AddWithValue("@CurrentUserId", currentLoggedInUserID);
                command.ExecuteScalar();
            }

        }
        #endregion

        #region UAT-3241
        List<String> IAgencyHierarchyRepository.GetAgencyNamesByIds(List<Int32> lstAgencyIds)
        {
            return this.SharedDataDBContext.Agencies.Where(con => lstAgencyIds.Contains(con.AG_ID) && !con.AG_IsDeleted).Select(sel => sel.AG_Name).ToList();
        }

        #endregion

        #region UAT-3237
        public Boolean UpdateNodeDisplayOrder(List<AgencyHierarchyDataContract> lstAgencyHierarchyIds, Int32? destinationIndex, Int32 currentLoggedInuser)
        {
            DataTable dtNodes = new DataTable();
            dtNodes.Columns.Add("AgencyHierarchyID", typeof(Int32));
            dtNodes.Columns.Add("DestinationIndex", typeof(Int32));
            dtNodes.Columns.Add("ModifiedBy", typeof(Int32));
            foreach (var dpmId in lstAgencyHierarchyIds)
            {
                dtNodes.Rows.Add(new object[] { Convert.ToInt32(dpmId.AgencyHierarchyID), destinationIndex, currentLoggedInuser });
                destinationIndex += 1;
            }
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand _command = new SqlCommand("UpdateAgencyHierarchyNodeSequence", con);
                _command.CommandType = CommandType.StoredProcedure;
                _command.Parameters.AddWithValue("@typeAgencyHierarchy", dtNodes);
                con.Open();
                Int32 rowsAffected = _command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    con.Close();
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region UAT-3719
        private void SaveAgencyUserPermissionAuditDetails(Int32? AgencyUserId, Int32? AgencyUserTemplateID, Int32 CurrentLoggedInUserID)
        {
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand("usp_SaveAgencyUserPermissionAuditData", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyUserID", AgencyUserId);
                command.Parameters.AddWithValue("@AgencyUserTemplateID", AgencyUserTemplateID);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", CurrentLoggedInUserID);
                command.ExecuteScalar();
                con.Close();
            }
        }
        #endregion

        #region UAT-3662

        AgencySetting IAgencyHierarchyRepository.GetInstPrecpReqdSetting(Int32 agencyId)
        {
            String instPrecepReqdSettingTypeCode = AgencyHierarchySettingType.INSTPRECEPTOR_MANDATORY_FOR_INDIVIDUAL_SHARE.GetStringValue();
            return this.SharedDataDBContext.AgencySettings.Where(cond => !cond.AS_IsDeleted && cond.AS_AgencyID == agencyId && cond.lkpAgencyHierarchySetting.S_Code == instPrecepReqdSettingTypeCode).FirstOrDefault();
        }

        #endregion

        #region UAT-3961
        public Boolean SaveAgencyHierarchyRootNodeSetting(Int32 agencyHierarchyRootNodeId, String agencyHierarchySettingTypeCode, String agencyHierarchySettingValue, Int32 CurrentLoggedInUserID)
        {
            Boolean outputResponse = false;
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand("usp_SaveAgencyHierarchyRootNodeSetting", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyHierarchyRootNodeId", agencyHierarchyRootNodeId);
                command.Parameters.AddWithValue("@AgencyHierarchySettingTypeCode", agencyHierarchySettingTypeCode);
                command.Parameters.AddWithValue("@AgencyHierarchySettingValue", agencyHierarchySettingValue);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", CurrentLoggedInUserID);
                command.ExecuteScalar();
                outputResponse = true;
                con.Close();
            }
            return outputResponse;
        }

        public List<AgencyHierarchyRootNodeSettingContract> GetAgencyHierarchyRootNodeMapping(Int32 agencyHierarchyRootNodeId, String agencyHierarchySettingTypeCode)
        {
            var _lstAgencyHierarchyUserContract = new List<AgencyHierarchyRootNodeSettingContract>();

            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                {
                    new SqlParameter("@AgencyHierarchyRootNodeId", agencyHierarchyRootNodeId),
                    new SqlParameter("@AgencyHierarchySettingTypeCode", agencyHierarchySettingTypeCode)
                };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyHierarchyRootNodeMapping", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            AgencyHierarchyRootNodeSettingContract agencyUser = new AgencyHierarchyRootNodeSettingContract();
                            agencyUser.MappingID = dr["MappingID"].GetType().Name == "DBNull" ? 0 : Convert.ToInt32(dr["MappingID"]);
                            agencyUser.MappingValue = dr["MappingValue"].GetType().Name == "DBNull" ? String.Empty : Convert.ToString(dr["MappingValue"]);
                            _lstAgencyHierarchyUserContract.Add(agencyUser);
                        }
                    }
                }
                base.CloseSQLDataReaderConnection(con);
            }
            return _lstAgencyHierarchyUserContract;
        }

        public Boolean SaveUpdateAgencyHierarchyRootNodeMapping(AgencyHierarchyRootNodeSettingContract agencyHierarchyRootNodeSettingContract)
        {
            Boolean outputResponse = false;
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand command = new SqlCommand("usp_SaveAgencyHierarchyRootNodeMapping", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyHierarchyRootNodeId", agencyHierarchyRootNodeSettingContract.AgencyHierarchyID);
                command.Parameters.AddWithValue("@AgencyHierarchySettingTypeCode", agencyHierarchyRootNodeSettingContract.SettingTypeCode);
                command.Parameters.AddWithValue("@AgencyHierarchyMappingValue", agencyHierarchyRootNodeSettingContract.MappingValue);
                command.Parameters.AddWithValue("@AgencyHierarchyMappingID", agencyHierarchyRootNodeSettingContract.MappingID);
                command.Parameters.AddWithValue("@IsRecordDeleted", agencyHierarchyRootNodeSettingContract.IsRecordDeleted);
                command.Parameters.AddWithValue("@CurrentLoggedInUserID", agencyHierarchyRootNodeSettingContract.CurrentLoggedInUser);
                command.ExecuteScalar();
                outputResponse = true;
                con.Close();
            }
            return outputResponse;
        }

        Boolean IAgencyHierarchyRepository.IsAgencyHierarchyRootNodeSettingExist(Int32 agencyHierarchyRootNodeId, String agencyHierarchySettingTypeCode)
        {
            EntityConnection connection = this.SharedDataDBContext.Connection as EntityConnection;

            SqlParameter outputParam = new SqlParameter("@ReturnValue", SqlDbType.Bit) { Direction = ParameterDirection.Output };

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_IsAgencyHierarchyRootNodeSettingExist", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AgencyHierarchyRootNodeId", agencyHierarchyRootNodeId);
                command.Parameters.AddWithValue("@AgencyHierarchySettingTypeCode", agencyHierarchySettingTypeCode);
                command.Parameters.Add(outputParam);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                command.ExecuteNonQuery();
                con.Close();
            }
            return Convert.ToBoolean(outputParam.Value);
        }
        #endregion


        #region UAT-3704
        List<Entity.SharedDataEntity.AgencyHierarchy> IAgencyHierarchyRepository.GetAgencyHierarchyRootNodes()
        {
            return this.SharedDataDBContext.AgencyHierarchies.Where(cond => !cond.AH_IsDeleted && cond.AH_ParentID == null).ToList();
        }
        #endregion

        #region UAT-4257
        List<AgencyHierarchyContract> IAgencyHierarchyRepository.GetAgencyHierarchyRootNodesByTenantIDs(List<Int32> tenantIds)
        {
            List<AgencyHierarchyContract> lstAgencyHierarchyRootNodes = SharedDataDBContext.AgencyHierarchyTenantMappings.Include("AgencyHierarchies").Where(cond => !cond.AHTM_IsDeleted && tenantIds.Contains(cond.AHTM_TenantID) && !cond.AgencyHierarchy.AH_IsDeleted && cond.AgencyHierarchy.AH_ParentID == null)
                                                                   .Select(x => new AgencyHierarchyContract
                                                                   {
                                                                       AgencyID = x.AgencyHierarchy.AH_ID,
                                                                       AgencyName = x.AgencyHierarchy.AH_Label
                                                                   }).ToList();

            return lstAgencyHierarchyRootNodes;
        }
        #endregion

        //UAT-4402
        List<RequirementCategoryContract> IAgencyHierarchyRepository.GetRequirementCategoryByPackageID(Int32 packageId)
        {
            List<RequirementCategoryContract> lstReqPkgCategory = SharedDataDBContext.RequirementPackageCategories.Include("RequirementCategories").Where(x => !x.RPC_IsDeleted && !x.RequirementCategory.RC_IsDeleted && x.RPC_RequirementPackageID == packageId)
                .Select(x => new RequirementCategoryContract
                {
                    RequirementPackageID = x.RPC_RequirementPackageID,
                    RequirementPackageCategoryID = x.RPC_ID,
                    RequirementCategoryName = x.RequirementCategory.RC_CategoryName,
                    RequirementCategoryLabel = x.RequirementCategory.RC_CategoryLabel
                }).ToList();
            return lstReqPkgCategory;
        }

        #region UAT-4657
        String IAgencyHierarchyRepository.IsPackageVersionInProgress(Int32 PkgId, Int32 requirementPkgVersioningStatus_DueId, Int32 requirementPkgVersioningStatus_InProgressId)
        {

            Guid pkgCode = SharedDataDBContext.RequirementPackages.FirstOrDefault(con => con.RP_ID == PkgId).RP_Code.Value;
            bool IsPackageDigestionPending = SharedDataDBContext.RequirementPackages.Any(con => 
                                                            (con.RP_ParentPackageCode == pkgCode || con.RP_Code == pkgCode) && 
                                                            (con.RP_VersionStatusId.Value == requirementPkgVersioningStatus_DueId || con.RP_VersionStatusId.Value == requirementPkgVersioningStatus_InProgressId) && 
                                                            !con.RP_IsDeleted);
           
                                                        //.Where(con => (con.RP_ParentPackageCode == pkgCode
                                                        //    || con.RP_Code == pkgCode)
                                                        //    && (con.RP_VersionStatusId.Value == requirementPkgVersioningStatus_DueId
                                                        //    || con.RP_VersionStatusId.Value == requirementPkgVersioningStatus_InProgressId)
                                                        //     && !con.RP_IsDeleted).Any();

            if (!IsPackageDigestionPending)
            {
                List<Int32> categoryIds = SharedDataDBContext.RequirementPackageCategories
                                         .Where(con => con.RPC_RequirementPackageID == PkgId && !con.RPC_IsDeleted)
                                         .Select(x => x.RPC_RequirementCategoryID).ToList();

                bool IsCategoryDissociationPending = SharedDataDBContext.RequirementCategoryDisassociations.Any(con =>
                                                    (categoryIds.Contains(con.RCD_SourceCategoryId) || categoryIds.Contains(con.RCD_TargetCategoryId.Value))
                                                     && !con.RCD_IsMovementCompleted && !con.RCD_IsMovementDiscarded && !con.RCD_IsDeleted);

                                              //.Where(con => (categoryIds.Contains(con.RCD_SourceCategoryId)
                                              //       || categoryIds.Contains(con.RCD_TargetCategoryId.Value))
                                              //       && !con.RCD_IsMovementCompleted && !con.RCD_IsDeleted).Any();

                return IsCategoryDissociationPending ? "Category" : "";
                //return "";
            }
            return "Package";
        }
        #endregion
    }
}

