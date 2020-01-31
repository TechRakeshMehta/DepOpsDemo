using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.PackageBundleManagement;
using DAL.Interfaces;
using System.Data.Entity.Core.EntityClient;
using Entity.ClientEntity;
using System.Data.SqlClient;
using System.Data;
using INTSOF.Utils;
namespace DAL.Repository
{
    public class PackageBundleRepository : ClientBaseRepository, IPackageBundleRepository
    {
        private ADB_LibertyUniversity_ReviewEntities _dbContext;

        /// <summary>
        /// Default constructor to initialize class level variables.
        /// </summary>
        public PackageBundleRepository(Int32 tenantId)
            : base(tenantId)
        {
            _dbContext = base.ClientDBContext;
        }

        #region BundleDetails

        List<ManagePackageBundleContract> IPackageBundleRepository.lstPackageBundle(ManagePackageBundleContract objBundle)
        {
            List<ManagePackageBundleContract> bundledata = new List<ManagePackageBundleContract>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                SqlParameter[] sqlParameterCollection = new SqlParameter[]
                        {
                            
                             new SqlParameter("@BundleName", objBundle.BundleName),  
                           new SqlParameter("@HierarchyNodes", objBundle.HierarchyNode), 
                          
                        };

                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetPackageBundleSearch", sqlParameterCollection))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ManagePackageBundleContract objdata = new ManagePackageBundleContract();
                            objdata.BundleId = dr["PackageBundleID"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(dr["PackageBundleID"]);
                            objdata.BundleName = Convert.ToString(dr["PackageBundleName"]);
                            objdata.TrackingPackage = Convert.ToString(dr["TrackingPackageName"]);
                            objdata.AdministrativePackage = Convert.ToString(dr["AdministrativePackageName"]);
                            objdata.ScreeningPackage = Convert.ToString(dr["ScreeningPackageName"]);
                            objdata.PackageBundleLabel = Convert.ToString(dr["PackageBundleLabel"]);
                            objdata.IsAvailableForOrder = dr["IsAvailableForOrder"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsAvailableForOrder"]);
                            objdata.HierarchyNodes = dr["HierarchyNodes"] == DBNull.Value ? String.Empty : Convert.ToString(dr["HierarchyNodes"]);
                            bundledata.Add(objdata);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);
            }
            return bundledata;
        }

        List<PackageBundlePackages> IPackageBundleRepository.GetPackageBundlePackages()
        {
            List<PackageBundlePackages> packageBundlePackagesList = new List<PackageBundlePackages>();
            EntityConnection connection = _dbContext.Connection as EntityConnection;
            using (SqlConnection con = new SqlConnection(connection.StoreConnection.ConnectionString))
            {
                base.OpenSQLDataReaderConnection(con);

                using (SqlDataReader dr = base.ExecuteSQLDataReader(con, "usp_GetPackagesDpmMap"))
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            PackageBundlePackages applicantData = new PackageBundlePackages();
                            applicantData.PackageID = dr["PackageID"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(dr["PackageID"]);
                            applicantData.PackageName = Convert.ToString(dr["PackageName"]);
                            applicantData.PackageNodeMappingID = dr["PackageNodeMappingID"].GetType().Name == "DBNull" ? AppConsts.NONE : Convert.ToInt32(dr["PackageNodeMappingID"]);
                            applicantData.PackageTypeCode = Convert.ToString(dr["PackageTypeCode"]);
                            applicantData.IsCompliancePackage = dr["IsCompliancePackage"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsCompliancePackage"]);
                            packageBundlePackagesList.Add(applicantData);
                        }
                    }
                }

                base.CloseSQLDataReaderConnection(con);

                //SqlCommand _command = new SqlCommand("usp_GetPackagesDpmMap", con);
                //_command.CommandType = CommandType.StoredProcedure;
                ////_command.Parameters.AddWithValue("@OrderId", bkgOrderId);
                ////  _command.Parameters.AddWithValue("@MasterOrderId", masterOrderId);
                //SqlDataAdapter _adp = new SqlDataAdapter();
                //_adp.SelectCommand = _command;
                //_adp.Fill(_ds);
                ////if (_ds.Tables.Count > 0)
                ////return _ds.Tables[0];
            }
            return packageBundlePackagesList;
        }
        public Boolean InsertPackageBundle(PackageBundle objPackageBundle)
        {
            _dbContext.PackageBundles.AddObject(objPackageBundle);
            //_ClientDBContext.InstitutionNodeTypes.AddObject(objRotation);
            if (_dbContext.SaveChanges() > 0)
                return true;
            return false;
        }
        #endregion

        public PackageBundle GetPackageBundleId(Int32 BundleId)
        {
            return _dbContext.PackageBundles.Where(cond => cond.PBU_ID == BundleId && !cond.PBU_IsDeleted).FirstOrDefault();
        }

        public Boolean UpdatePackageBundle()
        {
            if (_dbContext.SaveChanges() > 0)
                return true;
            return false;
        }

        #region UAT-1200: WB: As a student I should be able to select one package which will order both a tracking package and a screening package.
        List<PackageBundle> IPackageBundleRepository.GetPackageBundlesAvailableForOrder(Int32 orgUserId, Dictionary<Int32, Int32> selectedDpmIds)
        {
            List<PackageBundle> lstAvailablePackageBundles = new List<PackageBundle>();
            List<PackageBundle> lstPkageBundlesAvailbleforOrder = new List<PackageBundle>();
            selectedDpmIds = selectedDpmIds.OrderByDescending(dpmId => dpmId.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            List<Int32> alreadyPurchasedPkgIds = new List<Int32>();

            List<PackageSubscription> _activePackageSubscriptions = _dbContext.PackageSubscriptions.Include("Order")
                                                                                                   .Include("Order.DeptProgramPackage")
                                                                                                   .Include("Order.DeptProgramPackage.DeptProgramMapping")
                                                                                                   .Where(ps => !ps.IsDeleted && ps.OrganizationUserID == orgUserId 
                                                                                                   //&& (ps.ArchiveStateID == null || !ps.lkpArchiveState.AS_Code.Equals("AB")) //UAT-3422 - Removed check for Archived Subscriptions
                                                                                                   && ps.ExpiryDate > DateTime.Now).ToList();
            if (!_activePackageSubscriptions.IsNullOrEmpty())
                alreadyPurchasedPkgIds = _activePackageSubscriptions.Select(sel => sel.CompliancePackageID).ToList();

            String _pendingPaymentApprovalStatus = ApplicantOrderStatus.Pending_Payment_Approval.GetStringValue();
            String _cancellationRequestedStatus = ApplicantOrderStatus.Cancellation_Requested.GetStringValue();
            String compliancePackageTypeCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
            String bkgPartialOrderCancellationType = PartialOrderCancellationType.BACKGROUND_PACKAGE.GetStringValue();

            List<Int32> userProfileIds = _dbContext.OrganizationUserProfiles
                                    .Where(orgUserProfile => orgUserProfile.OrganizationUserID == orgUserId && !orgUserProfile.IsDeleted)
                                    .Select(orgUserProfile => orgUserProfile.OrganizationUserProfileID).ToList();

            List<Int32?> alreadyOrdereddppIDs = new List<Int32?>();
            if (!userProfileIds.IsNullOrEmpty())
            {
                alreadyOrdereddppIDs = _dbContext.OrderPaymentDetails.Where(x => userProfileIds.Contains(x.Order.OrganizationUserProfileID)
                        && !x.Order.IsDeleted && (x.Order.PartialOrderCancellationTypeID == null || x.Order.lkpPartialOrderCancellationType.Code == bkgPartialOrderCancellationType)
                        && !x.OPD_IsDeleted && (x.lkpOrderStatu.Code.Equals(_pendingPaymentApprovalStatus) ||
                        x.lkpOrderStatu.Code.Equals(_cancellationRequestedStatus))
                        && x.OrderPkgPaymentDetails.Any(cond => cond.lkpOrderPackageType.OPT_Code.Equals(compliancePackageTypeCode) && !cond.OPPD_IsDeleted)
                        ).Select(cnd => cnd.Order.DeptProgramPackageID).ToList();
            }

            foreach (Int32 dpmId in selectedDpmIds.Values)
            {
                lstAvailablePackageBundles = ClientDBContext.PackageBundles.Include("PackageBundleNodeMappings")
                                                                      .Where(cond => cond.PBU_IsDeleted == false
                                                                      && cond.PackageBundleNodeMappings.Any(sel => sel.PBNM_DeptProgramMappingID == dpmId && !sel.PBNM_IsDeleted)
                                                                      && cond.PBU_IsAvailableForOrder == true)
                                                                      .ToList();

                if (lstAvailablePackageBundles.Count > AppConsts.NONE)
                {
                    foreach (var pkgBundle in lstAvailablePackageBundles)
                    {
                        pkgBundle.PBU_Name = pkgBundle.PBU_Label.IsNullOrEmpty() ? pkgBundle.PBU_Name : pkgBundle.PBU_Label;
                        List<PackageBundleNodePackage> cmplncBundlePackages = pkgBundle.PackageBundleNodePackages.Where(cond => cond.PBNP_BkgPackageHierarchyMappingID == null && !cond.PBNP_IsDeleted).ToList();
                        if (cmplncBundlePackages.IsNullOrEmpty() ||
                            (!cmplncBundlePackages.Any(sel => alreadyPurchasedPkgIds.Contains(sel.DeptProgramPackage.DPP_CompliancePackageID))
                            && !cmplncBundlePackages.Any(cond => alreadyOrdereddppIDs.Contains(cond.PBNP_DeptProgramPackageID)))
                            )
                            lstPkageBundlesAvailbleforOrder.Add(pkgBundle);
                    }
                }
                if (lstPkageBundlesAvailbleforOrder.Count > AppConsts.NONE)
                    break;
            }

            return lstPkageBundlesAvailbleforOrder;
        }

        List<PackageBundleNodePackage> IPackageBundleRepository.GetListOfPackageAvaiableUnderBundle(Int32 packageBundleId)
        {
            return ClientDBContext.PackageBundleNodePackages.Where(pbp => pbp.PBNP_PackageBundleID == packageBundleId && !pbp.PBNP_IsDeleted).ToList();
        }

        /// <summary>
        /// Get the list of Packages under all the Bundles 
        /// </summary>
        /// <param name="lstBundleIds"></param>
        /// <returns></returns>
        List<PackageBundleNodePackage> IPackageBundleRepository.GetBundlePackages(List<Int32> lstBundleIds)
        {
            return ClientDBContext.PackageBundleNodePackages.Where(pbp => lstBundleIds.Contains(pbp.PBNP_PackageBundleID) && !pbp.PBNP_IsDeleted).ToList();
        }

        #endregion
        #region UAT-1648: As an applicant, I should be able to complete payment for an order that is in "sent for online payment"
        /// <summary>
        /// Get all package ids purchased by applicant those are "sent for online payment".
        /// </summary>
        /// <param name="organizationUserId">organizationUserId</param>
        /// <returns>List of Compliance Package IDs</returns>
        public List<Int32> GetAppCompPackageSentForOnlinePayment(Int32 organizationUserId)
        {
            List<Int32> lstCompPkgSentForOnlinePayment = new List<Int32>();
            List<OrderPaymentDetail> sentForOnlinePaymentDetail = new List<OrderPaymentDetail>();
            String sentForOnlinePaymentCode = ApplicantOrderStatus.Send_For_Online_Payment.GetStringValue();
            sentForOnlinePaymentDetail = _dbContext.OrderPaymentDetails.Where(cnd => cnd.Order.OrganizationUserProfile.OrganizationUserID == organizationUserId
                                                          && !cnd.Order.OrganizationUserProfile.IsDeleted && !cnd.Order.OrganizationUserProfile.OrganizationUser.IsDeleted
                                                          && !cnd.Order.IsDeleted && cnd.Order.PackageSubscription == null && cnd.lkpOrderStatu != null
                                                          && cnd.lkpOrderStatu.Code == sentForOnlinePaymentCode && !cnd.OPD_IsDeleted
                                                          ).ToList();

            if (!sentForOnlinePaymentDetail.IsNullOrEmpty())
            {
                String ordrPkgTypeComplianceRushOrderCode = OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue();
                String compliancePackageTypeCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
                foreach (var opd in sentForOnlinePaymentDetail)
                {
                    if (!opd.OrderPkgPaymentDetails.Any(OPPD => !OPPD.OPPD_IsDeleted && OPPD.lkpOrderPackageType.OPT_Code == ordrPkgTypeComplianceRushOrderCode)
                        && opd.OrderPkgPaymentDetails.Any(OPPD => !OPPD.OPPD_IsDeleted && OPPD.OPPD_BkgOrderPackageID == null
                                                          && OPPD.lkpOrderPackageType.OPT_Code == compliancePackageTypeCode)
                        )
                    {
                        lstCompPkgSentForOnlinePayment.Add(opd.Order.DeptProgramPackage.DPP_CompliancePackageID);
                    }
                }
            }
            return lstCompPkgSentForOnlinePayment;
        }
        #endregion
    }
}
