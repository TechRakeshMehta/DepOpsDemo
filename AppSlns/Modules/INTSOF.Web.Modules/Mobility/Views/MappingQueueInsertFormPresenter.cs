using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.Mobility;
using INTSOF.Utils.Consts;
using System.Xml.Linq;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.Mobility.Views
{
    public class MappingQueueInsertFormPresenter : Presenter<IMappingQueueInsertFormView>
    {



        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }
        /// <summary>
        /// to get the list of Institution 
        /// </summary>
        public void GetTenantList()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            List<Entity.Tenant> lstTemp = SecurityManager.GetTenants(SortByName, false, clientCode);
            lstTemp.Insert(0, new Entity.Tenant { TenantName = "--SELECT--", TenantID = 0 });
            View.lstTenant = lstTemp;
        }
        /// <summary>
        /// to get Tenant id
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }
        /// <summary>
        /// To get Source package List
        /// </summary>
        /// <param name="tenantId"></param>
        public void GetPackagesList(Int32 tenantId)
        {
            //ComplianceSetupManager.GetProgramPackagesByProgramMapId(View.SourceNodeId, tenantId).ToList();

            List<AvailablePackageContarct> packageContarct = StoredProcedureManagers.GetAvailableCompAndBkgPackages(tenantId, View.SourceNodeId
                                                                                                                , OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue(),true);
            List<DeptProgramPackage> packageList = new List<DeptProgramPackage>();
            packageContarct.ForEach(pc =>
            {
                DeptProgramPackage pkgObj = new DeptProgramPackage();
                pkgObj.DPP_CompliancePackageID = pc.PackageId;
                pkgObj.CompliancePackage = new CompliancePackage();
                pkgObj.CompliancePackage.PackageName = pc.PackageName;
                packageList.Add(pkgObj);
            });
            View.lstPackage = packageList;
        }
        /// <summary>
        /// To get target package list
        /// </summary>
        /// <param name="tenantId"></param>
        public void GetTargetPackagesList(Int32 tenantId)
        {
            //List<DeptProgramPackage> packageList = ComplianceSetupManager.GetProgramPackagesByProgramMapId(View.TargetNodeId, tenantId).ToList();
            List<AvailablePackageContarct> packageContarct = StoredProcedureManagers.GetAvailableCompAndBkgPackages(tenantId, View.TargetNodeId
                                                                                                               , OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue(),true);
            List<DeptProgramPackage> packageList = new List<DeptProgramPackage>();
            packageContarct.ForEach(pc =>
            {
                DeptProgramPackage pkgObj = new DeptProgramPackage();
                pkgObj.DPP_CompliancePackageID = pc.PackageId;
                pkgObj.CompliancePackage = new CompliancePackage();
                pkgObj.CompliancePackage.PackageName = pc.PackageName;
                packageList.Add(pkgObj);
            });
            View.lstPackage = packageList;
        }
        /// <summary>
        /// to Save New mapping
        /// </summary>
        /// <returns></returns>
        public Boolean SaveMapping()
        {
            //check new Mapping already exist or not
            if (!MobilityManager.CheckIfMappingAlreadyExist(View.SelectedSourceNewMappingTenantId, View.SelectedSourcePackageId, View.SelectedTargetPackageId, View.SourceNodeId, View.TargetNodeId))
            {
                View.ErrorMessage = String.Empty;
                Entity.PkgMappingMaster pkgMappingMaster = new Entity.PkgMappingMaster();
                pkgMappingMaster.PMM_FromTenantID = View.SelectedSourceNewMappingTenantId;
                pkgMappingMaster.PMM_ToTenantID = View.SelectedSourceNewMappingTenantId;
                pkgMappingMaster.PMM_FromPackageID = View.SelectedSourcePackageId;
                pkgMappingMaster.PMM_ToPackageID = View.SelectedTargetPackageId;
                pkgMappingMaster.PMM_MappingStatusID = AppConsts.ONE;
                pkgMappingMaster.PMM_IsDeleted = false;
                pkgMappingMaster.PMM_CreatedByID = View.CurrentLoggedInUserId;
                pkgMappingMaster.PMM_CreatedOn = DateTime.Now;
                pkgMappingMaster.PMM_FromPackageName = View.SelectedSourceNewMappingPackage;
                pkgMappingMaster.PMM_ToPackageName = View.SelectedTargetNewMappingPackage;
                pkgMappingMaster.PMM_FromNodeID = View.SourceNodeId;
                pkgMappingMaster.PMM_ToNodeID = View.TargetNodeId;
                pkgMappingMaster.PMM_IsMappingSkipped = View.IsMappingSkip;
                pkgMappingMaster.PMM_Name = View.MappingName;
                //save the mapping
                if (MobilityManager.SaveMapping(View.SelectedSourceNewMappingTenantId, pkgMappingMaster))
                {
                    View.SuccessMessage = "Mapping saved successfully.";
                    return true;
                }
                else
                {
                    View.ErrorMessage = "Some error occurred.Please try again.";
                    return false;
                }
            }
            else
            {
                View.InfoMessage = "Mapping already exist.";
                return false;

            }

        }
    }
}




