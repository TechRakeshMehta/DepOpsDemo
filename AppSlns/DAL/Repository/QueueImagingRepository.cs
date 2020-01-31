using DAL.Interfaces;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.Repository
{
    public class QueueImagingRepository : BaseRepository, IQueueImagingRepository
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private SysXAppDBEntities _dbNavigation;
        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public QueueImagingRepository()
        {
            _dbNavigation = base.Context;
        }

        #endregion

        #region Methods

        #region Public Methods

        public void SyncVerificationDataForTenant(Int32 tenantId)
        {
            EntityConnection connection = _dbNavigation.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_ExecuteVerificationDataSyncingProcedure", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tenantId", tenantId);
                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
            }
        }

        public List<Int32> GetTenantListDueForImaging()
        {
            return _dbNavigation.QueueImagings.Where(x => x.QI_IsImagingDue).Select(x => x.QI_TenantId).ToList();
        }

        Boolean IQueueImagingRepository.UpdateInsertQueueImagingDue(Int32 tenantId)
        {

            QueueImaging QueueImagingObj = _dbNavigation.QueueImagings.FirstOrDefault(x => x.QI_TenantId == tenantId);
            if (QueueImagingObj != null)
            {
                QueueImagingObj.QI_IsImagingDue = true;
            }
            else
            {
                QueueImagingObj = new QueueImaging();
                QueueImagingObj.QI_TenantId = tenantId;
                QueueImagingObj.QI_StartTime = null;
                QueueImagingObj.QI_EndTine = null;
                QueueImagingObj.QI_IsImagingDue = true;
                _dbNavigation.AddToQueueImagings(QueueImagingObj);
            }
            if (_dbNavigation.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}
