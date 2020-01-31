using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Entity.SharedDataEntity;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;

namespace DAL.Repository
{
    public class AgencyReviewRepository : BaseRepository, IAgencyReviewRepository
    {
        #region Variables

        private ADB_SharedDataEntities _sharedDataDBContext;

        #endregion

        #region Default Constructor to initilize DB Context

        ///// <summary>
        ///// Default constructor to initialize the context
        ///// </summary>
        public AgencyReviewRepository()
        {
            _sharedDataDBContext = base.SharedDataDBContext;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the Agency Review Queue related data
        /// </summary>
        /// <param name="selectedStatusCodes"></param>
        /// <param name="selectedTenantIds"></param>
        /// <param name="sortingFilteringXML"></param>
        /// <returns></returns>
        List<AgencyReviewQueueContract> IAgencyReviewRepository.GetAgencyQueueData(String selectedStatusCodes, String selectedTenantIds, CustomPagingArgsContract customPagingArgsContract)
        {
            var _lstAgencyReviewQueueContract = new List<AgencyReviewQueueContract>();
            EntityConnection connection = _sharedDataDBContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                           new SqlParameter("@SelectedTenantIds", selectedTenantIds), 
                           new SqlParameter("@SelectedStatusCodes", selectedStatusCodes),
                           new SqlParameter("@OrderBy", customPagingArgsContract.SortExpression) ,
                           new SqlParameter("@OrderDirection", customPagingArgsContract.SortDirectionDescending  ? "DESC": "ASC") ,
                           new SqlParameter("@PageIndex", customPagingArgsContract.CurrentPageIndex),
                           new SqlParameter("@PageSize", customPagingArgsContract.PageSize) 
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetAgencyReviewQueueData", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var agencyReviewData = new AgencyReviewQueueContract();

                            agencyReviewData.AgencyId = Convert.ToInt32(dr["AgencyId"]);
                            agencyReviewData.AgencyName = Convert.ToString(dr["AgencyName"]);
                            agencyReviewData.ReviewStatus = Convert.ToString(dr["ReviewStatus"]);
                            agencyReviewData.TotalCount = Convert.ToInt32(dr["TotalCount"]);
                            agencyReviewData.FullAddress = Convert.ToString(dr["FullAddress"]);
                            agencyReviewData.NpiNumber = Convert.ToString(dr["NpiNumber"]);
                            agencyReviewData.InstitutionName = Convert.ToString(dr["InstitutionName"]);

                            _lstAgencyReviewQueueContract.Add(agencyReviewData);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return _lstAgencyReviewQueueContract;
        }

        /// <summary>
        /// Set Agency status to reviwed or available, based on the StatusID
        /// </summary>
        /// <param name="lstSelectedAgencyIds"></param>
        /// <param name="statusId"></param>
        void IAgencyReviewRepository.SetAgencySearchStatus(List<Int32> lstSelectedAgencyIds, Int32 statusId, Int32 currentUserId)
        {
            var _lstToUpdate = _sharedDataDBContext.Agencies.Where(ag => lstSelectedAgencyIds.Contains(ag.AG_ID)).ToList();

            foreach (var agencyToUpdate in _lstToUpdate)
            {
                agencyToUpdate.AG_SearchStatusID = statusId;
                agencyToUpdate.AG_ModifiedByID = currentUserId;
                agencyToUpdate.AG_ModifiedOn = DateTime.Now;
            }
            _sharedDataDBContext.SaveChanges();
        }

        #endregion
    }
}
