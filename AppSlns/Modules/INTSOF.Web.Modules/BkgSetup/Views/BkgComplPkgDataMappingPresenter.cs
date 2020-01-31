using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System.Web;
using INTSOF.Contracts;
using INTSOF.ServiceUtil;

namespace CoreWeb.BkgSetup.Views
{
    public class BkgComplPkgDataMappingPresenter : Presenter<IBkgComplPkgDataMappingView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        private int SetClientID()
        {
            Int32 clientId = View.TenantId;
            //Checks if the logged in user is admin and some client is selected from the dropdown.
            if ((IsDefaultTenant || IsThirdPartyTenant) && View.SelectedTenantId != 0)
            {
                clientId = View.SelectedTenantId;
            }
            return clientId;
        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantId;
            }
        }

        public Boolean IsThirdPartyTenant
        {
            get
            {
                return SecurityManager.IsTenantThirdPartyType(View.TenantId, TenantType.Compliance_Reviewer.GetStringValue());
            }
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

        public void GetCompliancePackage()
        {
            Int32 clientId = SetClientID();
            try
            {
                View.lstCompliancePackage = ComplianceSetupManager.GetPermittedPackagesByUserID(clientId)
                                                                         .OrderBy(x => x.PackageName).ToList();//UAT sort dropdowns by Name;;
            }
            catch (Exception e)
            {
                View.lstCompliancePackage = new List<ComplaincePackageDetails>();
            }
        }

        public void GetBackgroundPackages()
        {
            Int32 clientId = SetClientID();
            try
            {
                View.lstBackgroundPackage = BackgroundSetupManager.GetPermittedBackgroundPackagesByUserID(clientId).Where(cond => cond.BPA_IsAvailableForApplicantOrder == true)
                                                                       .OrderBy(x => x.BPA_Name).ToList();//UAT sort dropdowns by Name;
            }
            catch (Exception e)
            {
                View.lstBackgroundPackage = new List<BackgroundPackage>();
            }
        }

        public List<Entity.ClientEntity.lkpBkgDataPointType> GetDataPoints()
        {
            Int32 clientId = SetClientID();
            try
            {
                return BackgroundSetupManager.GetDataPoints(clientId);
            }
            catch (Exception e)
            {
                return new List<Entity.ClientEntity.lkpBkgDataPointType>();
            }
        }

        public List<LookupContract> GetServiceGroups(Int32 selectedPackageId)
        {
            Int32 clientId = SetClientID();
            try
            {
                return BackgroundSetupManager.GetServiceGroupsForPackage(clientId, selectedPackageId);
            }
            catch (Exception e)
            {
                return new List<LookupContract>();
            }
        }

        public List<LookupContract> GetServices(int selectedServiceGrp, int selectedBkgPkgID)
        {
            Int32 clientId = SetClientID();
            try
            {
                return BackgroundSetupManager.GetServicesForSvcGroup(clientId, selectedServiceGrp, selectedBkgPkgID);
            }
            catch (Exception e)
            {
                return new List<LookupContract>();
            }
        }

        public List<LookupContract> GetServiceItems(int selectedService)
        {
            Int32 clientId = SetClientID();
            try
            {
                return BackgroundSetupManager.GetServiceItemsForSvc(clientId, selectedService);
            }
            catch (Exception e)
            {
                return new List<LookupContract>();
            }
        }

        public List<LookupContract> GetComplianceCatagories(int selectedComplPkgID)
        {
            Int32 clientId = SetClientID();
            try
            {
                return BackgroundSetupManager.GetComplianceCatagories(clientId, selectedComplPkgID);
            }
            catch (Exception e)
            {
                return new List<LookupContract>();
            }
        }

        public List<LookupContract> GetCatagoryItems(int selectedCatagoryID)
        {
            Int32 clientId = SetClientID();
            try
            {
                return BackgroundSetupManager.GetCatagoryItems(clientId, selectedCatagoryID);
            }
            catch (Exception e)
            {
                return new List<LookupContract>();
            }
        }

        public List<LookupContract> GetComplianceItemAttributes(int selectedItemID, string dataPointCode)
        {
            Int32 clientId = SetClientID();
            try
            {
                return BackgroundSetupManager.GetComplianceItemAttributes(clientId, selectedItemID, dataPointCode);
            }
            catch (Exception e)
            {
                return new List<LookupContract>();
            }
        }

        public List<BkgCompliancePackageMappingSearchData> FetchBkgCompliancePackageMapping()
        {
            Int32 clientId = SetClientID();
            List<BkgCompliancePackageMappingSearchData> tempMappingRecord = new List<BkgCompliancePackageMappingSearchData>();
            try
            {
                if (clientId != 1)
                {
                    View.GridCustomPaging.DefaultSortExpression = "BPA_Name";
                    View.GridCustomPaging.SecondarySortExpression = "BCPM_ID";
                    tempMappingRecord = BackgroundSetupManager.FetchBkgCompliancePackageMapping(clientId, View.BackgroundPackageId, View.CompliancePackageId,
                                                                                               View.GridCustomPaging);
                    if (tempMappingRecord.IsNotNull() && tempMappingRecord.Count > 0)
                    {
                        //if (tempMappingRecord[0].TotalCount > 0)
                        //{
                        //    View.VirtualRecordCount = tempMappingRecord[0].TotalCount;
                        //}
                        View.VirtualRecordCount = View.GridCustomPaging.VirtualPageCount;
                        View.CurrentPageIndex = View.GridCustomPaging.CurrentPageIndex;
                    }
                    else
                    {
                        View.VirtualRecordCount = 0;
                        View.CurrentPageIndex = 1;
                    }
                }
                return tempMappingRecord;
            }
            catch (Exception e)
            {
                return tempMappingRecord;
                throw;
            }
        }

        public List<Entity.ClientEntity.ComplianceAttributeOption> GetComplianceAttributeOption(Int32 attributeId)
        {
            Int32 clientId = SetClientID();
            try
            {
                if (attributeId > AppConsts.NONE)
                {
                    return BackgroundSetupManager.GetComplianceAttributeOption(clientId, attributeId);
                }
                return new List<Entity.ClientEntity.ComplianceAttributeOption>();
            }
            catch (Exception e)
            {
                return new List<Entity.ClientEntity.ComplianceAttributeOption>();
            }
        }

        public Boolean SaveBkgComplPkgDataMapping(BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract)
        {
            Int32 clientId = SetClientID();
            try
            {
                if (!IsBkgCompDataPointMappingExist(clientId, null, bkgComplPkgDataMappingContract))
                {
                    return BackgroundSetupManager.SaveBkgComplPkgDataMapping(clientId, bkgComplPkgDataMappingContract, View.CurrentLoggedInUserId);
                }
                else
                {
                    View.InfoMessage = "This Mapping already exist.";
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string DeleteBkgComplPkgDataMapping(int BCPM_ID)
        {
            Int32 clientId = SetClientID();
            try
            {
                return BackgroundSetupManager.DeleteBkgComplPkgDataMapping(clientId, BCPM_ID, View.CurrentLoggedInUserId);
            }
            catch (Exception e)
            {
                return "Mapping deletion failed.";
            }
        }


        public string UpdateBkgComplPkgDataMapping(int BCPM_ID, BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract)
        {
            Int32 clientId = SetClientID();
            try
            {
                if (!IsBkgCompDataPointMappingExist(clientId, BCPM_ID, bkgComplPkgDataMappingContract))
                {
                    return BackgroundSetupManager.UpdateBkgComplPkgDataMapping(clientId, BCPM_ID, View.CurrentLoggedInUserId, bkgComplPkgDataMappingContract);
                }
                else
                {
                    View.InfoMessage = "This Mapping already exist.";
                    return String.Empty;
                }
            }
            catch (Exception e)
            {
                return "Mapping updation failed.";
            }
        }

        public List<BkgCompliancePkgMappingAttrOption> FetchBkgCompliancePkgMappingAttrOptions(Int32 BCPM_ID)
        {
            Int32 clientId = SetClientID();
            try
            {
                return BackgroundSetupManager.FetchBkgCompliancePkgMappingAttrOptions(clientId, BCPM_ID);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #region UAT-1451:
        public Boolean IsBkgCompDataPointMappingExist(Int32 clientId, Int32? BCPM_ID, BkgComplPkgDataMappingContract bkgComplPkgDataMappingContract)
        {
            return BackgroundSetupManager.IsBkgCompDataPointMappingExist(clientId, BCPM_ID, bkgComplPkgDataMappingContract);
        }
        #endregion

        //UAT-2319
        public void SyncDataForNewMapping()
        {
            try
            {
                Int32 clientId = SetClientID();
                Dictionary<String, Object> dataDict = new Dictionary<String, Object>();
                dataDict.Add("tenantId", clientId);
                dataDict.Add("CurrentLoggedInUserId", View.CurrentLoggedInUserId);

                var LoggerService = (HttpContext.Current.ApplicationInstance as IWebApplication).LoggerService;
                var ExceptiomService = (HttpContext.Current.ApplicationInstance as IWebApplication).ExceptionService;
                ParallelTaskContext.PerformParallelTask(SyncData, dataDict, LoggerService, ExceptiomService);
            }
            catch(Exception ex)
            {
               //after sucessfully data sync the value of key is again set to zero in AppConfiguration table in security db.
                SecurityManager.UpdateAppConfiguration(AppConsts.Background_Data_Sync_In_Progress, AppConsts.ZERO);
            }
        }

        //UAT-2319
        private void SyncData(Dictionary<String, Object> data)
        {
            Int32 tenantId = Convert.ToInt32(data.GetValue("tenantId"));
            Int32 currentLoggedInUserId = Convert.ToInt32(data.GetValue("CurrentLoggedInUserId"));
            BackgroundProcessOrderManager.SyncDataForNewMapping(tenantId, currentLoggedInUserId);
        }

        //UAT-2319
        public Boolean GetAppConfiguration(String BkgDataSyncInProgress)
        {
            String AppConfigurationValue = SecurityManager.GetAppConfiguration(BkgDataSyncInProgress).AC_Value;
            if (AppConfigurationValue == AppConsts.ZERO)
            {
                return true;
            }
            return false;
        }
        public void UpdateAppConfiguration(String BkgDataSyncInProgress, String UpdatedValue)
        {
            SecurityManager.UpdateAppConfiguration(BkgDataSyncInProgress, UpdatedValue);
        }
    }
}
