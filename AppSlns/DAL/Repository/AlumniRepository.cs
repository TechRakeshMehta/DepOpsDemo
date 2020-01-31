using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using System.Data;
using Entity.SharedDataEntity;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;



namespace DAL.Repository
{
    public class AlumniRepository : ClientBaseRepository, IAlumniRepository
    {

        #region Variables
        private ADB_LibertyUniversity_ReviewEntities _dbContext;
        #endregion

        #region Default Constructor to initilize DB Context
        ///// <summary>
        ///// Default constructor to initialize the context
        ///// </summary>
        public AlumniRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }

        #endregion

        Tuple<Int32, Int32> IAlumniRepository.CreateAlumniDefaultSubscription(Int32 currentLoggedInUserID, Int32 organizationUserProfileID, String machineIP)
        {
            EntityConnection connection = _dbContext.Connection as EntityConnection;

            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlCommand command = new SqlCommand("usp_CreateDefaultAlumniSubscription", con);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@OrganizationUserProfileID", organizationUserProfileID);
                command.Parameters.AddWithValue("@CurrentloggedInUserId", currentLoggedInUserID);
                command.Parameters.AddWithValue("@OrderMachineIP", machineIP);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.SelectCommand = command;
                DataSet ds = new DataSet();
                adp.Fill(ds);
                if (ds.Tables.Count > AppConsts.NONE && ds.Tables[0].Rows.Count > AppConsts.NONE)
                {
                    var result = ds.Tables[0].AsEnumerable().FirstOrDefault();
                    return new Tuple<Int32, Int32>(Convert.ToInt32(result.Field<Int32>("PkgSubscriptionID")), Convert.ToInt32(result.Field<Int32>("OrderID")));
                }
            }
            return new Tuple<Int32, Int32>(AppConsts.NONE, AppConsts.NONE);
        }

        Boolean IAlumniRepository.CheckAllSubscriptionsForApplicant(Int32 orgUserId)
        {
            Boolean isAllSubscriptionEnded = false;
            Int32 count = AppConsts.NONE;
            List<PackageSubscription> lstPackageSubscription = _dbContext.PackageSubscriptions.Where(con => con.OrganizationUserID == orgUserId && !con.IsDeleted).ToList();
            if (!lstPackageSubscription.IsNullOrEmpty())
            {
                foreach (PackageSubscription item in lstPackageSubscription)
                {
                    //ArchiveState.Package_Subscription_Cancelled
                    if (item.lkpArchiveState.AS_Code == ArchiveState.Archived.GetStringValue()
                        || item.lkpArchiveState.AS_Code == ArchiveState.Graduated.GetStringValue()
                        || item.lkpArchiveState.AS_Code == ArchiveState.Archived_and_Graduated.GetStringValue()
                        || item.ExpiryDate < DateTime.Now)
                    {
                        count = count + 1;
                    }
                }
                if (lstPackageSubscription.Count == count)
                {
                    isAllSubscriptionEnded = true;
                }
            }
            return isAllSubscriptionEnded;
        }
    }
}

