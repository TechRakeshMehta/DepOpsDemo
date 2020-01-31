using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class ManageServicePresenter : Presenter<IManageServiceView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }
        //Get the List Of tenants
        public void GetTenants()
        {
            // View.ListTenants = ComplianceDataManager.getClientTenant();
        }

        /// <summary>
        /// Gets the tanent id for the current logged-in user.
        /// </summary>
        /// <returns></returns>
        public Int32 GetTenantId()
        {
            return SecurityManager.GetOrganizationUser(View.CurrentLoggedInUserId).Organization.TenantID.Value;
        }

        /// <summary>
        /// Checked if logged user is admin or not.
        /// </summary>
        /// <returns>True if logged in user is admin.</returns>
        public void IsAdminLoggedIn()
        {
            //Checked if logged user is admin or not.
            if (View.TenantId == Business.RepoManagers.SecurityManager.DefaultTenantID)
            {
                View.IsAdminLoggedIn = true;
            }
            else
            {
                View.IsAdminLoggedIn = false;
            }
        }

        /// <summary>
        /// Gets all the service  form Master.
        /// </summary>
        public void GetMasterServices()
        {
            View.MasterServiceList = BackgroundSetupManager.GetMasterServices();
        }
        /// <summary>
        /// get all service type
        /// </summary>
        public void GetBkgServiceTypes()
        {
            View.BkgServiceTypeList = BackgroundSetupManager.GetBkgServiceType();
        }
        /// <summary>
        /// Save new service in Master
        /// </summary>
        public void SaveMasterService()
        {

            if (BackgroundSetupManager.CheckIfServiceNameAlreadyExist(View.ViewContract.ServiceName, View.ViewContract.ServiceID))
            {
                View.ErrorMessage = "Service Group Name can not be duplicate.";
            }
            else
            {
                EntityCollection<Entity.ApplicableServiceSetting> applicableServiceSetting = new EntityCollection<Entity.ApplicableServiceSetting>();
                applicableServiceSetting.Add(new ApplicableServiceSetting()
                {
                    //ASSE_ShowIgnoreResidentialHistory = View.ViewContract.ShowIgnoreResidentialHistory,
                    ASSE_ShowIsSupplemental = View.ViewContract.ShowIsSupplemental,
                    ASSE_ShowMaxOcuurence = View.ViewContract.ShowMaxOcuurence,
                    ASSE_ShowMinOcuurence = View.ViewContract.ShowMinOcuurence,
                    ASSE_ShowPackageCount = View.ViewContract.ShowPackageCount,
                    //ASSE_ShowResidenceYears = View.ViewContract.ShowResidenceYears,
                    ASSE_ShowSendDocument = View.ViewContract.ShowSendDocument,
                    ASSE_CreatedBy = View.CurrentLoggedInUserId,
                    ASSE_CreatedDate = DateTime.Now,
                    ASSE_IsDeleted = false
                });

                Entity.BackgroundService newBkgService = new Entity.BackgroundService
                {
                    BSE_Name = View.ViewContract.ServiceName,
                    BSE_Description = View.ViewContract.ServiceDesc,
                    BSE_IsEditable = true,
                    BSE_IsSystemPreConfigured = false,
                    BSE_SvcTypeID = View.ViewContract.ServiceTypeID,
                    BSE_ParentServiceID = View.ViewContract.ParentServiceID,
                    BSE_ConfigurableServiceText =View.ViewContract.ConfigurableServiceText,         //UAT-1728
                    ApplicableServiceSettings = applicableServiceSetting
                    //BSG_Active = View.ViewContract.Active,
                };
                //add new master Service
                BackgroundSetupManager.SaveNewServiceDetail(newBkgService, View.CurrentLoggedInUserId);
            }
        }

        public void UpdateServiceGroup()
        {
            if (BackgroundSetupManager.CheckIfServiceNameAlreadyExist(View.ViewContract.ServiceName, View.ViewContract.ServiceID))
            {
                View.ErrorMessage = "Service Group Name can not be duplicate.";
            }
            else
            {
                EntityCollection<Entity.ApplicableServiceSetting> applicableServiceSetting = new EntityCollection<Entity.ApplicableServiceSetting>();
                applicableServiceSetting.Add(new ApplicableServiceSetting()
                {
                    //ASSE_ShowIgnoreResidentialHistory = View.ViewContract.ShowIgnoreResidentialHistory,
                    ASSE_ShowIsSupplemental = View.ViewContract.ShowIsSupplemental,
                    ASSE_ShowMaxOcuurence = View.ViewContract.ShowMaxOcuurence,
                    ASSE_ShowMinOcuurence = View.ViewContract.ShowMinOcuurence,
                    ASSE_ShowPackageCount = View.ViewContract.ShowPackageCount,
                    //ASSE_ShowResidenceYears = View.ViewContract.ShowResidenceYears,
                    ASSE_ShowSendDocument = View.ViewContract.ShowSendDocument,
                    ASSE_ModifiedBy = View.CurrentLoggedInUserId,
                    ASSE_ModifiedDate = DateTime.Now
                });

                Entity.BackgroundService svcGroup = new Entity.BackgroundService
                {
                    BSE_Name = View.ViewContract.ServiceName,
                    BSE_Description = View.ViewContract.ServiceDesc,
                    BSE_SvcTypeID = View.ViewContract.ServiceTypeID,
                    BSE_ParentServiceID = View.ViewContract.ParentServiceID,
                    BSE_ConfigurableServiceText = View.ViewContract.ConfigurableServiceText, 
                    ApplicableServiceSettings=applicableServiceSetting
                    //BSG_Active = View.ViewContract.Active,
                };
                //Update the given Master service
                BackgroundSetupManager.UpdateServiceDetail(svcGroup, View.ViewContract.ServiceID, View.CurrentLoggedInUserId);
            }
        }

        public void DeletebackgroundService(Int32 bkgSvcMasterID)
        {
            if (!BackgroundSetupManager.IsChildServiceExist(bkgSvcMasterID))
            {
                BackgroundSetupManager.DeletebackgroundService(bkgSvcMasterID, View.CurrentLoggedInUserId);
            }
            else
            {
                View.InfoMessage = "Please delete its child services before deleting this service.";
            }

            //return true;
        }
        // Boolean IsServiceMAppedToClient(Int32 bkgSvcMasterID) 
        public Boolean IsServiceMAppedToClient(Int32 bkgSvcMasterID)
        {
            if (!(BackgroundServiceIntegrityManager.IsServiceMAppedToClient(bkgSvcMasterID)))
            {
                return true;
            }
            else
            {
                String masterSvcName = BackgroundSetupManager.BkgSrvName(bkgSvcMasterID);
                View.ErrorMessage = "You cannot delete Service " + masterSvcName + " as it is in use.";
                return false;
            }
        }

        #region Derived From Service
        public void GetDerivedFromServiceList(Int32? currentServiceId = null)
        {
            List<BackgroundService> tempDerivedServiceList = new List<BackgroundService>();
            tempDerivedServiceList = BackgroundSetupManager.GetDerivedFromServiceList(currentServiceId);
            tempDerivedServiceList.Insert(0, new BackgroundService { BSE_ID = 0, BSE_Name = "--Select--" });
            View.BkgDerivedFromServiceList = tempDerivedServiceList;
        }
        #endregion
    }
}
