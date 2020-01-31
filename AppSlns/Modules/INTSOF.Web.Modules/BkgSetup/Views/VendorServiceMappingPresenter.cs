using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class VendorServiceMappingPresenter : Presenter<IVendorServiceMappingView>
    {
        public void GetVendorServiceMapping()
        {
            //var lstBkgSvcExtSvcMapping = BackgroundSetupManager.GetVendorServiceMapping().ToList();
            View.VendorServiceMappingList = BackgroundSetupManager.GetVendorServiceMapping()
                .Select(item => new VendorServiceMappingContract
                {
                    BSESM_ID = item.BSESM_ID,
                    BSESM_Code = item.BSESM_Code,
                    BSESM_CreatedBy = item.BSESM_CreatedBy,
                    BSESM_CreatedOn = item.BSESM_CreatedOn,
                    BSE_ID = item.BackgroundService.BSE_ID,
                    BSE_Name = item.BackgroundService.BSE_Name,
                    BSE_Description = item.BackgroundService.BSE_Description,
                    EBS_ID = item.ExternalBkgSvc.EBS_ID,
                    EBS_Name = String.IsNullOrEmpty(item.ExternalBkgSvc.EBS_Name) ? item.ExternalBkgSvc.EBS_Description : item.ExternalBkgSvc.EBS_Name,
                    EBS_Description = item.ExternalBkgSvc.EBS_Description,
                    EBS_ExternalCode = item.ExternalBkgSvc.EBS_ExternalCode,
                    EBS_Code = item.ExternalBkgSvc.EBS_Code,
                    EVE_ID = item.ExternalBkgSvc.ExternalVendor.EVE_ID,
                    EVE_Name = item.ExternalBkgSvc.ExternalVendor.EVE_Name,
                    EVE_Description = item.ExternalBkgSvc.ExternalVendor.EVE_Description,
                    EVE_Code = item.ExternalBkgSvc.ExternalVendor.EVE_Code,
                    IsEditable = item.ClientExtSvcVendorMappings.Any(x => !x.CESVM_IsDeleted) == true ? false : true
                }).ToList();

        }

        public Boolean DeleteVendorServiceMapping(Int32 currentUserId)
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.IfVendorServiceMappingCanBeDeleted(View.VendorServiceMappingID);
            if (response.CheckStatus == CheckStatus.True)
            {
                View.ErrorMessage = response.UIMessage;
                return false;
            }
            else
            {
                return BackgroundSetupManager.DeleteVendorServiceMapping(View.VendorServiceMappingID, currentUserId);
            }
        }

        public void BindDropDown()
        {
            View.BackgroundServiceList = BackgroundSetupManager.GetMasterServices().OrderBy(con => con.BSE_Name).ToList();
            View.ExternalVendorList = BackgroundSetupManager.GetVendors().OrderBy(con => con.EVE_Name).ToList();
        }

        public void GetExternalBkgSvcByVendorID()
        {
            View.ExternalBkgServiceList = BackgroundSetupManager.GetExternalBkgSvcByVendorID(View.VendorID).OrderBy(con => con.EBS_Name).ToList();
        }

        public String FetchExternalBkgServiceCodeByID(Int32 ExtSvcID)
        {
            return BackgroundSetupManager.FetchExternalBkgServiceCodeByID(SecurityManager.DefaultTenantID, ExtSvcID);
        }

        public Boolean AddVendorServiceMapping()
        {
            Boolean isAdded = false;
            if (BackgroundSetupManager.IfVendorServiceMappingExists(View.ViewContract.BSE_ID, View.ViewContract.EBS_ID, null))
            {
                View.ErrorMessage = "Vendor Service Mapping already exist.";
            }
            else
            {
                BkgSvcExtSvcMapping bkgSvcExtSvcMapping = new BkgSvcExtSvcMapping
                {
                    BSESM_BkgSvcId = View.ViewContract.BSE_ID,
                    BSESM_ExtSvcId = View.ViewContract.EBS_ID,
                    BSESM_Code = View.ViewContract.BSESM_Code,
                    BSESM_CopiedFromCode = null,
                    BSESM_IsDeleted = false,
                    BSESM_CreatedBy = View.ViewContract.BSESM_CreatedBy,
                    BSESM_CreatedOn = View.ViewContract.BSESM_CreatedOn
                };
                isAdded = BackgroundSetupManager.SaveVendorServiceMapping(bkgSvcExtSvcMapping);
            }

            return isAdded;
        }

        public Boolean UpdateVendorServiceMapping()
        {
            Boolean isUpdated = false;
            if (BackgroundSetupManager.IfVendorServiceMappingExists(View.ViewContract.BSE_ID, View.ViewContract.EBS_ID, View.ViewContract.BSESM_ID))
            {
                View.ErrorMessage = "Vendor Service Mapping already exist.";
            }
            else
            {
                BkgSvcExtSvcMapping bkgSvcExtSvcMapping = new BkgSvcExtSvcMapping
                {
                    BSESM_ID = View.ViewContract.BSESM_ID,
                    BSESM_BkgSvcId = View.ViewContract.BSE_ID,
                    BSESM_ExtSvcId = View.ViewContract.EBS_ID,
                    BSESM_ModifiedBy = View.ViewContract.BSESM_ModifiedBy,
                    BSESM_ModifiedOn = View.ViewContract.BSESM_ModifiedOn
                };
                isUpdated = BackgroundSetupManager.UpdateVendorServiceMapping(bkgSvcExtSvcMapping);
            }

            return isUpdated;
        }
    }
}
