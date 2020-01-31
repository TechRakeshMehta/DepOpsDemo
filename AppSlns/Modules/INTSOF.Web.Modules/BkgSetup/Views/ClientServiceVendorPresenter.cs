
using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public class ClientServiceVendorPresenter : Presenter<IClientServiceVendorView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }
        //Get the List Of tenants
        public void GetTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            List<Entity.Tenant> lstTemp = SecurityManager.GetTenants(SortByName, false, clientCode);
            lstTemp.Insert(0, new Entity.Tenant { TenantName = "--SELECT--", TenantID = 0 });
            View.ListTenants = lstTemp;
        }
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


        public void GetMappedBkgSvcExtSvcToState()
        {
            View.GetMappedServiceStateList = BackgroundSetupManager.GetMappedBkgSvcExtSvcToState(View.SelectedTenantId);
        }

        public void GetBkgService()
        {
            View.BackgroundServicesLst = BackgroundSetupManager.GetBkgService(View.SelectedTenantId);
        }
        public void GetExtBkgSvcCorrespondsToBkgSvc(Int32 SelectedBkgSvcID, Boolean _isupdate)
        {
            View.ExtBkgServicesLst = BackgroundSetupManager.GetExtBkgSvcCorrespondsToBkgSvc(SelectedBkgSvcID, View.SelectedTenantId, _isupdate);
        }
        public void GetAllStates()
        {
            List<Entity.State> lstStateTemp = BackgroundSetupManager.GetAllStates().Where(x => !x.StateAbbreviation.IsNullOrEmpty()).ToList();
           // lstStateTemp.Insert(0, new Entity.State { StateName = "SELECT ALL", StateID = 0 });
            View.ListStates = lstStateTemp;
           
        }
        public List<Int32> GetMAppedStatesIdtoExtSvc(Int32 ExtSvcId)
        {
            return BackgroundSetupManager.GetMAppedStatesIdtoExtSvc(ExtSvcId ,View.SelectedTenantId);
        }

        public String FetchExternalBkgServiceCodeByID()
        {
            return BackgroundSetupManager.FetchExternalBkgServiceCodeByID(View.SelectedTenantId, View.SelectedExtbKGSvcid);
        }

        public void SaveClientServiceNewMapping(List<Int32> selectedNewMappedStateIds, Int32 selectedServiceID, Int32 selectedExternalServiceId)
        {
            //if (selectedNewMappedStateIds.IsNullOrEmpty() || selectedNewMappedStateIds.Count() == AppConsts.NONE)
            //{
            //    View.ErrorMessage = "Please Select atleast one State.";
            //    return;
            //}
            //else
            //{
                Int32 BkgSvcExtSvcMappedId = BackgroundSetupManager.GetBkgSvcExtSvcMappedId(selectedServiceID, selectedExternalServiceId);
                if (BkgSvcExtSvcMappedId > AppConsts.NONE)
                {
                    foreach (Int32 StateidtoMap in selectedNewMappedStateIds)
                    {
                        Entity.ClientExtSvcVendorMapping clientExtSvcVendorMapping = new Entity.ClientExtSvcVendorMapping();
                        if (StateidtoMap == AppConsts.NONE)
                        {
                            clientExtSvcVendorMapping.CESVM_StateID = null;
                        }
                        else {
                            clientExtSvcVendorMapping.CESVM_StateID = StateidtoMap;
                        }
                        clientExtSvcVendorMapping.CESVM_BkgSvcExtSvcMappingID = BkgSvcExtSvcMappedId;
                        clientExtSvcVendorMapping.CESVM_TenantID = View.SelectedTenantId;
                        clientExtSvcVendorMapping.CESVM_IsDeleted = false;
                        clientExtSvcVendorMapping.CESVM_CreatedBy = View.CurrentLoggedInUserId;
                        clientExtSvcVendorMapping.CESVM_CreatedOn = DateTime.Now;
                        BackgroundSetupManager.SaveClientSvcvendormapping(clientExtSvcVendorMapping);

                    }
                }
           // }
        }
        
        public void UpdateClientSvcVendorMapping(List<Int32> updatedMappedStateIds, Int32 selectedServiceID, Int32 selectedExternalServiceId)
        {
            if (updatedMappedStateIds.Count > 0)
            {
                BackgroundSetupManager.UpdateClientSvcVendorMapping(updatedMappedStateIds, selectedServiceID, selectedExternalServiceId, View.SelectedTenantId, View.CurrentLoggedInUserId);
            }
            else
            {
                View.ErrorMessage = "Please Select atleast one State to map with the Service.";
            }
        }
        public void DeleteClientVendorExtSvcMapping(Int32 BkgsvcID,Int32 ExtServiceID, String BkgServiceName)
        {

            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IsClientSvcVendorStateMappingIsAssociated(BkgsvcID, View.SelectedTenantId);
            if (response.CheckStatus == CheckStatus.True)
            {
                View.ErrorMessage = String.Format(response.UIMessage);
            }
            else
            {
                BackgroundSetupManager.DeleteClientSvcVendorMapping(BkgsvcID,ExtServiceID,View.SelectedTenantId, View.CurrentLoggedInUserId);
            }
        }
    }
}
