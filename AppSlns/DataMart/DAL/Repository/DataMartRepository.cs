using DataMart.DAL.Interfaces;
using Entity;
using Entity.ClientEntity;
using Entity.SharedDataEntity;
using INTSOF.AppFabricCacheServer;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;

namespace DataMart.DAL.Repository
{
    public class DataMartRepository : ClientBaseRepository, IDataMartRepository
    {
        #region Variables
        private ADB_SharedDataEntities _sharedDataDBContext;
        private ADB_LibertyUniversity_ReviewEntities _clientDbContext;
        private SysXAppDBEntities _securityDBContext;
        #endregion

        #region Default Constructor to initilize DB Context
        ///// <summary>
        ///// Default constructor to initialize the context
        ///// </summary>
        public DataMartRepository(Int32 tenantID = 1) : base(tenantID)
        {
            _sharedDataDBContext = base.SharedDataDBContext;
            _securityDBContext = base.Context;
            _clientDbContext = base.ClientDBContext;
        }

        #endregion

        #region AgencyUsers

        IQueryable<AgencyUser> IDataMartRepository.GetAgencyUsers()
        {
            return _sharedDataDBContext.AgencyUsers.Include("ProfileSharingInvitations").Where(x => !x.AGU_IsDeleted && x.AGU_UserID.HasValue);
        }

        IQueryable<ProfileSharingInvitationGroup> IDataMartRepository.GetProfileSharingInvitationGroups()
        {
            List<Int32> _tenantIDs = _securityDBContext.Tenants.Where(x => !x.IsDeleted).Select(x => x.TenantID).ToList();
            return _sharedDataDBContext.ProfileSharingInvitationGroups.Where(x => !x.PSIG_IsDeleted && _tenantIDs.Contains(x.PSIG_TenantID.Value));
        }

        IEnumerable<DataRow> IDataMartRepository.GetAgenciesOfAgencyUser()
        {
            DataTable returnObject = new DataTable();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("Report.usp_DW_GetAgenciesOfAgencyUser", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 3600;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = sqlCommand;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    returnObject = ds.Tables[0];
                }
            }
            return returnObject.AsEnumerable();
        }

        IEnumerable<DataRow> IDataMartRepository.GetModifiedAgencyUsers(DateTime lastSyncDate)
        {
            DataTable returnObject = new DataTable();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("Report.usp_DW_GetModifiedAgencyUsers", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@LastSyncDate", lastSyncDate));
                sqlCommand.CommandTimeout = 3600;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = sqlCommand;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    returnObject = ds.Tables[0];
                }
            }
            return returnObject.AsEnumerable();
        }

        IEnumerable<DataRow> IDataMartRepository.GetModifiedInvitationGroups(DateTime lastSyncDate)
        {
            DataTable returnObject = new DataTable();
            EntityConnection connection = _clientDbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("Report.usp_DW_GetModifiedInvitationGroups", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@LastSyncDate", lastSyncDate));
                sqlCommand.CommandTimeout = 3600;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = sqlCommand;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    returnObject = ds.Tables[0];
                }
            }
            return returnObject.AsEnumerable();
        }

        IEnumerable<DataRow> IDataMartRepository.GetModifiedRotationDetails(DateTime lastSyncDate)
        {
            DataTable returnObject = new DataTable();
            EntityConnection connection = _clientDbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("Report.usp_DW_GetModifiedRotationDetails", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@LastSyncDate", lastSyncDate));
                sqlCommand.CommandTimeout = 3600;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = sqlCommand;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    returnObject = ds.Tables[0];
                }
            }
            return returnObject.AsEnumerable();
        }

        IEnumerable<DataRow> IDataMartRepository.GetSharedItemsOfInvitationGroup(String invitationGroupIDs)
        {
            DataTable returnObject = new DataTable();
            EntityConnection connection = _clientDbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("Report.usp_DW_SharedItemsOfInvitationGroup", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@InvitationGroupIDs", invitationGroupIDs));
                sqlCommand.CommandTimeout = 3600;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = sqlCommand;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    returnObject = ds.Tables[0];
                }
            }
            return returnObject.AsEnumerable();
        }

        IEnumerable<DataRow> IDataMartRepository.GetRotationDetailsOfInvitationGroup(String invitationGroupIDs)
        {
            DataTable returnObject = new DataTable();
            EntityConnection connection = _clientDbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand("Report.usp_DW_RotationDetailsOfInvitationGroup", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@InvitationGroupIDs", invitationGroupIDs));
                sqlCommand.CommandTimeout = 3600;
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = sqlCommand;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    returnObject = ds.Tables[0];
                }
            }
            return returnObject.AsEnumerable();
        }

        List<ClientDBConfiguration> IDataMartRepository.GetClientDBConfigurations()
        {
            String code = TenantType.Institution.GetStringValue();
            return _securityDBContext.ClientDBConfigurations.Where(cond => cond.Tenant.lkpTenantType.TenantTypeCode == code && cond.Tenant.IsDeleted == false).DistinctBy(cond => cond.CDB_ConnectionString).ToList();
        }
        #endregion

    }
}