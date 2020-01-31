using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.PackageBundleManagement;
using System.Data;
using Entity.ClientEntity;
namespace DAL.Interfaces
{
  public  interface IPackageBundleRepository
    {
      List<ManagePackageBundleContract> lstPackageBundle(ManagePackageBundleContract objBundle);

      List<PackageBundlePackages> GetPackageBundlePackages();

      Boolean InsertPackageBundle(PackageBundle objPackageBundle);
      PackageBundle GetPackageBundleId(Int32 BundleId);
      Boolean UpdatePackageBundle();

      #region UAT-1200: WB: As a student I should be able to select one package which will order both a tracking package and a screening package.

      List<PackageBundle> GetPackageBundlesAvailableForOrder(Int32 orgUserId, Dictionary<Int32, Int32> selectedDpmIds);

      List<PackageBundleNodePackage> GetListOfPackageAvaiableUnderBundle(Int32 packageBundleId);

      /// <summary>
      /// Get the list of Packages under all the Bundles 
      /// </summary>
      /// <param name="lstBundleIds"></param>
      /// <returns></returns>
      List<PackageBundleNodePackage> GetBundlePackages(List<Int32> lstBundleIds);

      #endregion
    }
}
