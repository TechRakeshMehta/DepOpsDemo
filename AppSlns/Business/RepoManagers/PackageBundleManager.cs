using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using Entity.SharedDataEntity;
using System.Data;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.UI.Contract.Templates;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using System.Xml.Linq;
using INTSOF.UI.Contract.PackageBundleManagement;
using System.Windows.Forms;
namespace Business.RepoManagers
{
    public class PackageBundleManager
    {
        #region PackageBundle Details
        public static List<ManagePackageBundleContract> GetBundleData(ManagePackageBundleContract objBundle)
        {
            try
            {
                return BALUtils.GetPackageBundleRepoInstance(objBundle.TenantId).lstPackageBundle(objBundle);
                // .GetClinicalRotationQueueData(clinicalRotationDetailContract, customPagingArgsContract);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        public static List<PackageBundlePackages> GetPackageBundlePackages(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetPackageBundleRepoInstance(tenantId).GetPackageBundlePackages();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean InsertPackageBundleDetails(Int32 tenantId, PackageBundle PackageBundle)
        {
            try
            {
                return BALUtils.GetPackageBundleRepoInstance(tenantId).InsertPackageBundle(PackageBundle);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion

        #region Update/Delete Package bundle

        public static PackageBundle GetPackageBundleDetailsId(Int32 tenantId, Int32 Bundle_Id)
        {
            try
            {
                return BALUtils.GetPackageBundleRepoInstance(tenantId).GetPackageBundleId(Bundle_Id);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean UpdatePackageBundleDetails(Int32 tenantId, Int32 bundleId, Int32 currentUserId)
        {
            try
            {
                Entity.ClientEntity.PackageBundle PackageBundle = BALUtils.GetPackageBundleRepoInstance(tenantId).GetPackageBundleId(bundleId);                
                PackageBundle.PBU_IsDeleted = true;
                PackageBundle.PBU_ModifiedByID = currentUserId;
                PackageBundle.PBU_ModifiedOn = DateTime.Now;

                //Delete records from PackageBundleNodeMapping table related to selected Bundle Id
                List<PackageBundleNodeMapping> packageBundleNodeMappingList = PackageBundle.PackageBundleNodeMappings.Where(cond => !cond.PBNM_IsDeleted).ToList();

                foreach (PackageBundleNodeMapping hirarchyNodeId in packageBundleNodeMappingList)
                {

                    hirarchyNodeId.PBNM_IsDeleted = true;
                    hirarchyNodeId.PBNM_ModifiedByID = currentUserId;
                    hirarchyNodeId.PBNM_ModifiedOn = DateTime.Now;
                }

                //Delete Records from PackageBundleNodePackage related to seleted BundleId
                List<PackageBundleNodePackage> packageBundleNodePackageList = PackageBundle.PackageBundleNodePackages.Where(cond => !cond.PBNP_IsDeleted).ToList();
                foreach (PackageBundleNodePackage PBNPId in packageBundleNodePackageList)
                {
                    PBNPId.PBNP_IsDeleted = true;
                    PBNPId.PBNP_ModifiedByID = currentUserId;
                    PBNPId.PBNP_ModifiedOn = DateTime.Now;
                }

                return BALUtils.GetPackageBundleRepoInstance(tenantId).UpdatePackageBundle();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static Boolean EditPackageBundleDetails(Int32 tenantId)
        {
            try
            {
                return BALUtils.GetPackageBundleRepoInstance(tenantId).UpdatePackageBundle();
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        #endregion

        #region UAT-1200: WB: As a student I should be able to select one package which will order both a tracking package and a screening package.
        public static List<PackageBundle> GetPackageBundlesAvailableForOrder(Int32 orgUserId, Dictionary<Int32, Int32> selectedDpmIds, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetPackageBundleRepoInstance(tenantId).GetPackageBundlesAvailableForOrder(orgUserId, selectedDpmIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        public static List<PackageBundleNodePackage> GetListOfPackageAvaiableUnderBundle(Int32 packageBundleId, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetPackageBundleRepoInstance(tenantId).GetListOfPackageAvaiableUnderBundle(packageBundleId);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }

        /// <summary>
        /// Get the list of Packages under all the Bundles 
        /// </summary>
        /// <param name="lstBundleIds"></param>
        /// <returns></returns>
        public static List<PackageBundleNodePackage> GetBundlePackages(List<Int32> lstBundleIds, Int32 tenantId)
        {
            try
            {
                return BALUtils.GetPackageBundleRepoInstance(tenantId).GetBundlePackages(lstBundleIds);
            }
            catch (SysXException ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw ex;
            }
            catch (Exception ex)
            {
                BALUtils.LogError(BALUtils.ClassModule + SysXException.ShowTrace() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace, ex);
                throw (new SysXException(ex.Message, ex));
            }
        }
        #endregion
    }
}
