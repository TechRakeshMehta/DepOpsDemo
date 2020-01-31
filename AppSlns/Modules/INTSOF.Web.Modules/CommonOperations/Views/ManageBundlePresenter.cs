using Business.RepoManagers;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceProxy.Modules.ClinicalRotation;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Entity.ClientEntity;
using INTSOF.UI.Contract.PackageBundleManagement;
namespace CoreWeb.CommonOperations.Views
{
    public class ManageBundlePresenter : Presenter<IManageBundle>
    {
        private ClinicalRotationProxy _clinicalRotationProxy
        {
            get
            {
                return new ClinicalRotationProxy();
            }
        }
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {

        }
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenant = SecurityManager.GetTenants(SortByName, false, clientCode);
        }
        public void IsAdminLoggedIn()
        {
            View.IsAdminLoggedIn = (SecurityManager.DefaultTenantID == View.TenantID);

        }

        public void GetBundleDetail()
        {
            List<ManagePackageBundleContract> packageBundleSearchData;
            if (View.SelectedTenantID == AppConsts.NONE)
            {
                packageBundleSearchData = new List<ManagePackageBundleContract>();
            }
            else
            {
                packageBundleSearchData = PackageBundleManager.GetBundleData(View.SearchContract);
            }
            View.BundleDataList = packageBundleSearchData;
        }

        public List<PackageBundlePackages> GetPackageBundlePackages()
        {
            if (View.SelectedTenantIDForAddForm == AppConsts.NONE)
            {
                return new List<PackageBundlePackages>();
            }

            return PackageBundleManager.GetPackageBundlePackages(View.SelectedTenantIDForAddForm);

        }
        public Boolean InsertPackageBundle()
        {
            Entity.ClientEntity.PackageBundle objPackagebundleEntity = new Entity.ClientEntity.PackageBundle();            
            objPackagebundleEntity.PBU_Name = View.BundleName;
            objPackagebundleEntity.PBU_Label = View.BundleLabel;
            objPackagebundleEntity.PBU_Description = View.BundleDescription;
            objPackagebundleEntity.PBU_ExplanatoryNotes = View.ExplanatoryNotes;
            objPackagebundleEntity.PBU_IsAvailableForOrder = View.IsAvailableForOrder;
            objPackagebundleEntity.PBU_CreatedOn = DateTime.Now;
            objPackagebundleEntity.PBU_CreatedByID = View.CurrentUserId;
            objPackagebundleEntity.PBU_IsDeleted = false;

            List<String> lstHirarchyNodeIds = View.HierarchyNode.Split(',').ToList();
            foreach (String hirarchyNodeId in lstHirarchyNodeIds)
            {
                PackageBundleNodeMapping objPBNM = new PackageBundleNodeMapping();
                objPBNM.PBNM_DeptProgramMappingID = Convert.ToInt32(hirarchyNodeId);
                objPBNM.PBNM_CreatedOn = DateTime.Now;
                objPBNM.PBNM_CreatedByID = View.CurrentUserId;
                objPBNM.PBNM_IsDeleted = false;
                objPackagebundleEntity.PackageBundleNodeMappings.Add(objPBNM);
            }

            //List<Int32> lstScreeningPackageIdsIds = View.ScreeningpackageIDs.Split(',').ToList();
            foreach (Int32 ScreeningPkgId in View.ScreeningpackageIDs)
            {
                PackageBundleNodePackage objPBNP = new PackageBundleNodePackage();
                objPBNP.PBNP_BkgPackageHierarchyMappingID = Convert.ToInt32(ScreeningPkgId);
                objPBNP.PBNP_CreatedOn = DateTime.Now;
                objPBNP.PBNP_CreatedByID = View.CurrentUserId;
                objPBNP.PBNP_IsDeleted = false;
                objPackagebundleEntity.PackageBundleNodePackages.Add(objPBNP);
            }

            //Get Compliance Package ID from Tracking Package type Code
            int TrackingPackageTypeId = LookupManager.GetLookUpData<lkpCompliancePackageType>(View.TenantID)
                                                           .FirstOrDefault(x => x.CPT_Code == "IMNZ"
                                                           && !x.CPT_IsDeleted).CPT_ID;


            //----------------------Tracking Package Insertion-------------------
            if (View.TrackingPackageID > AppConsts.NONE)
            {
                PackageBundleNodePackage objPBNPCP = new PackageBundleNodePackage();
                objPBNPCP.PBNP_DeptProgramPackageID = View.TrackingPackageID;
                objPBNPCP.PBNP_CreatedOn = DateTime.Now;
                objPBNPCP.PBNP_CreatedByID = View.CurrentUserId;
                objPBNPCP.PBNP_IsDeleted = false;
                objPBNPCP.PBNP_CompliancePackageTypeID = TrackingPackageTypeId;
                objPackagebundleEntity.PackageBundleNodePackages.Add(objPBNPCP);

            }


            //Get Compliance Package ID from Administrative Package type Code
            int AdministrativePackageTypeId = LookupManager.GetLookUpData<lkpCompliancePackageType>(View.TenantID)
                                                           .FirstOrDefault(x => x.CPT_Code == "ADMN"
                                                           && !x.CPT_IsDeleted).CPT_ID;

            //----------------------Administrative Package Insertion-------------------
            if (View.AdministrativePackageID > AppConsts.NONE)
            {
                PackageBundleNodePackage objPBNPAP = new PackageBundleNodePackage();
                objPBNPAP.PBNP_DeptProgramPackageID = View.AdministrativePackageID;
                objPBNPAP.PBNP_CreatedOn = DateTime.Now;
                objPBNPAP.PBNP_CreatedByID = View.CurrentUserId;
                objPBNPAP.PBNP_CompliancePackageTypeID = AdministrativePackageTypeId;
                objPackagebundleEntity.PackageBundleNodePackages.Add(objPBNPAP);
            }

            if (PackageBundleManager.InsertPackageBundleDetails(View.SelectedTenantID, objPackagebundleEntity))
            {
                View.SuccessMessage = "Package Bundle saved successfully.";
                return true;
            }
            else
            {
                View.ErrorMessage = "An error occured while adding Package Bundle. Please try again OR contact system administrator.";
                return false;
            }

        }

        public Boolean DeletePackageBundle()
        {
            if (PackageBundleManager.UpdatePackageBundleDetails(View.SelectedTenantID, View.BundleId, View.CurrentUserId))
            {
                View.SuccessMessage = "Package Bundle deleted successfully.";
                return true;
            }
            else
            {
                View.ErrorMessage = "An error occured while deleting Package Bundle. Please try again OR contact system administrator.";
                return false;
            }
        }

        public Boolean EditPackageBundle()
        {
            Entity.ClientEntity.PackageBundle PackageBundle = PackageBundleManager.GetPackageBundleDetailsId(View.SelectedTenantID, View.BundleId);
            PackageBundle.PBU_Name = View.BundleName;
            PackageBundle.PBU_Description = View.BundleDescription;
            PackageBundle.PBU_Label = View.BundleLabel;
            PackageBundle.PBU_IsAvailableForOrder = View.IsAvailableForOrder;
            PackageBundle.PBU_ExplanatoryNotes = View.ExplanatoryNotes;

            PackageBundle.PBU_ModifiedOn = DateTime.Now;
            PackageBundle.PBU_ModifiedByID = View.CurrentUserId;

            //Delete PackageBundleNodeMapping records related to selected Bundle ID
            List<PackageBundleNodeMapping> packageBundleNodeMappingList = PackageBundle.PackageBundleNodeMappings.Where(cond => !cond.PBNM_IsDeleted).ToList();

            //Insert new records in PackageBundleNodeMapping related to selected Bundle ID
            List<String> lstHierarchyNodeIds = View.HierarchyNode.Split(',').ToList();

            foreach (String item in lstHierarchyNodeIds)
            {
                Int32 hierarchyNodeID = Convert.ToInt32(item);
                if (!packageBundleNodeMappingList.Any(cond => cond.PBNM_DeptProgramMappingID == hierarchyNodeID))
                {
                    //Hierarchy Node do-not exists. Add PackageBundleNodeMapping
                    PackageBundleNodeMapping objPBNM = new PackageBundleNodeMapping();
                    objPBNM.PBNM_DeptProgramMappingID = hierarchyNodeID;
                    objPBNM.PBNM_CreatedOn = DateTime.Now;
                    objPBNM.PBNM_CreatedByID = View.CurrentUserId;
                    objPBNM.PBNM_IsDeleted = false;
                    PackageBundle.PackageBundleNodeMappings.Add(objPBNM);
                }
            }

            List<PackageBundleNodeMapping> packageBundleNodeMappingToBeRemoved = packageBundleNodeMappingList.Where(cond => !lstHierarchyNodeIds
                                                                                                             .Contains(cond.PBNM_DeptProgramMappingID.ToString()))
                                                                                                             .ToList();
            foreach (PackageBundleNodeMapping hirarchyNodeId in packageBundleNodeMappingToBeRemoved)
            {
                hirarchyNodeId.PBNM_IsDeleted = true;
                hirarchyNodeId.PBNM_ModifiedByID = View.CurrentUserId;
                hirarchyNodeId.PBNM_ModifiedOn = DateTime.Now;
            }

            //Delete records from PackageBundleNodePackage table related to selected bundle ID
            List<PackageBundleNodePackage> packageBundleNodePackageList = PackageBundle.PackageBundleNodePackages.Where(cond => !cond.PBNP_IsDeleted).ToList();

            #region Screening Packages

            foreach (Int32 ScreeningPkgId in View.ScreeningpackageIDs)
            {
                if (!packageBundleNodePackageList.Any(cond => cond.PBNP_BkgPackageHierarchyMappingID != null
                                                            && cond.PBNP_BkgPackageHierarchyMappingID == ScreeningPkgId
                                                            && cond.PBNP_DeptProgramPackageID == null))
                {
                    PackageBundleNodePackage objPBNP = new PackageBundleNodePackage();
                    objPBNP.PBNP_BkgPackageHierarchyMappingID = Convert.ToInt32(ScreeningPkgId);
                    objPBNP.PBNP_CreatedOn = DateTime.Now;
                    objPBNP.PBNP_CreatedByID = View.CurrentUserId;
                    objPBNP.PBNP_IsDeleted = false;
                    PackageBundle.PackageBundleNodePackages.Add(objPBNP);
                }
            }

            List<PackageBundleNodePackage> screeningPackagesToBeRemoved = packageBundleNodePackageList.Where(cond => cond.PBNP_BkgPackageHierarchyMappingID != null
                                            && cond.PBNP_DeptProgramPackageID == null &&
                                            !View.ScreeningpackageIDs.Contains(cond.PBNP_BkgPackageHierarchyMappingID.Value)).ToList();

            foreach (PackageBundleNodePackage PBNPId in screeningPackagesToBeRemoved)
            {
                PBNPId.PBNP_IsDeleted = true;
                PBNPId.PBNP_ModifiedByID = View.CurrentUserId;
                PBNPId.PBNP_ModifiedOn = DateTime.Now;
            }

            #endregion

            #region Trackng Package

            //Get Compliance Package ID from Tracking Package type Code
            int trackingPackageTypeId = LookupManager.GetLookUpData<lkpCompliancePackageType>(View.TenantID)
                                                           .FirstOrDefault(x => x.CPT_Code == "IMNZ"
                                                           && !x.CPT_IsDeleted).CPT_ID;

            PackageBundleNodePackage trackingPackageToBeUpdated = packageBundleNodePackageList.Where(cond => cond.PBNP_BkgPackageHierarchyMappingID == null
                                                                                            && cond.PBNP_DeptProgramPackageID != null
                                                                                            && cond.PBNP_CompliancePackageTypeID == trackingPackageTypeId)
                                                                                            .FirstOrDefault();
            if (View.TrackingPackageID > AppConsts.NONE)
            {
                if (trackingPackageToBeUpdated.IsNull())
                {
                    PackageBundleNodePackage objPBNPAP = new PackageBundleNodePackage();
                    objPBNPAP.PBNP_DeptProgramPackageID = View.TrackingPackageID;
                    objPBNPAP.PBNP_CreatedOn = DateTime.Now;
                    objPBNPAP.PBNP_CreatedByID = View.CurrentUserId;
                    objPBNPAP.PBNP_CompliancePackageTypeID = trackingPackageTypeId;
                    PackageBundle.PackageBundleNodePackages.Add(objPBNPAP);
                }
                else
                {
                    trackingPackageToBeUpdated.PBNP_DeptProgramPackageID = View.TrackingPackageID;
                    trackingPackageToBeUpdated.PBNP_ModifiedOn = DateTime.Now;
                    trackingPackageToBeUpdated.PBNP_ModifiedByID = View.CurrentUserId;
                }
            }
            else if (trackingPackageToBeUpdated.IsNotNull() && View.TrackingPackageID == AppConsts.NONE)
            {
                trackingPackageToBeUpdated.PBNP_IsDeleted = true;
                trackingPackageToBeUpdated.PBNP_ModifiedByID = View.CurrentUserId;
                trackingPackageToBeUpdated.PBNP_ModifiedOn = DateTime.Now;
            }

            #endregion
            
            #region Administrative Package
            
            int administrativePackageTypeId = LookupManager.GetLookUpData<lkpCompliancePackageType>(View.TenantID)
                                                           .FirstOrDefault(x => x.CPT_Code == "ADMN"
                                                           && !x.CPT_IsDeleted).CPT_ID;

            PackageBundleNodePackage admPackageToBeUpdated = packageBundleNodePackageList.Where(cond => cond.PBNP_BkgPackageHierarchyMappingID == null
                                                                                           && cond.PBNP_DeptProgramPackageID != null
                                                                                           && cond.PBNP_CompliancePackageTypeID == administrativePackageTypeId)
                                                                                           .FirstOrDefault();
            if (View.AdministrativePackageID > AppConsts.NONE)
            {
                if (admPackageToBeUpdated.IsNull())
                {
                    PackageBundleNodePackage objPBNPAP = new PackageBundleNodePackage();
                    objPBNPAP.PBNP_DeptProgramPackageID = View.AdministrativePackageID;
                    objPBNPAP.PBNP_CreatedOn = DateTime.Now;
                    objPBNPAP.PBNP_CreatedByID = View.CurrentUserId;
                    objPBNPAP.PBNP_CompliancePackageTypeID = administrativePackageTypeId;
                    PackageBundle.PackageBundleNodePackages.Add(objPBNPAP);
                }
                else
                {
                    admPackageToBeUpdated.PBNP_DeptProgramPackageID = View.AdministrativePackageID;
                    admPackageToBeUpdated.PBNP_ModifiedOn = DateTime.Now;
                    admPackageToBeUpdated.PBNP_ModifiedByID = View.CurrentUserId;
                }
            }
            else if (admPackageToBeUpdated.IsNotNull() && View.AdministrativePackageID == AppConsts.NONE)
            {
                admPackageToBeUpdated.PBNP_IsDeleted = true;
                admPackageToBeUpdated.PBNP_ModifiedByID = View.CurrentUserId;
                admPackageToBeUpdated.PBNP_ModifiedOn = DateTime.Now;
            }

            #endregion

            if (PackageBundleManager.EditPackageBundleDetails(View.SelectedTenantID))
            {
                View.SuccessMessage = "Bundle Package updated successfully.";
                return true;
            }
            else
            {
                View.ErrorMessage = "An error occured while updating Bundle Package. Please try again OR contact system administrator.";
                return false;
            }
        }

        public Entity.ClientEntity.PackageBundle GetPackageBundlebyId()
        {
            return PackageBundleManager.GetPackageBundleDetailsId(View.SelectedTenantID, View.BundleId);
        }
    }
}
