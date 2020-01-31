using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;

namespace CoreWeb.BkgSetup.Views
{
    public class ManageSvcItemCustomFormsPresenter : Presenter<IManageSvcItemCustomFormsView>
    {
        public override void OnViewInitialized()
        {
        }

        /// <summary>
        /// Get the List of Supplemental Custom Forms.
        /// </summary>       
        public void GetSupplCustomFrmsNotMappedToSvcItem()
        {
            List<Entity.CustomForm> tempLstCustomFormSupplementary = BackgroundSetupManager.GetSupplCustomFrmsNotMappedToSvcItem(View.TenantId, View.PackageServiceItemID);
            if (tempLstCustomFormSupplementary != null)
            {
                tempLstCustomFormSupplementary = tempLstCustomFormSupplementary.OrderBy(col => col.CF_Name).ToList();
            }
            View.lstCustomFormSupplementary = tempLstCustomFormSupplementary;
        }

        ///<summary>
        ///Save Custom Form Service Item Mappings
        ///</summary>
        public void SaveBkgSvcItemFormMapping()
        {
            if (View.SelectedCustomFormId.HasValue && View.SelectedCustomFormId.Value > 0 && View.PackageServiceItemID > 0)
            {
                BkgSvcItemFormMapping bkgSvcItemFormMappingObject = new BkgSvcItemFormMapping();
                bkgSvcItemFormMappingObject.BSIFM_CustomFormID = View.SelectedCustomFormId.Value;
                bkgSvcItemFormMappingObject.BSIFM_PackageServiceItemID = View.PackageServiceItemID;
                bkgSvcItemFormMappingObject.BSIFM_IsDeleted = false;
                bkgSvcItemFormMappingObject.BSIFM_CreatedByID = View.currentLoggedInUserId;
                bkgSvcItemFormMappingObject.BSIFM_CreatedOn = DateTime.Now;

                if (BackgroundSetupManager.SaveBkgSvcItemFormMapping(View.TenantId, bkgSvcItemFormMappingObject))
                {
                    View.SuccessMessage = "Custom Form mapped successfully.";
                }
                else
                {
                    View.ErrorMessage = "Some error has occurred.Please contact administrator.";
                }
            }
            else
                View.ErrorMessage = "Some error has occurred.Please contact administrator.";
        }

        /// <summary>
        /// Get the List of Supplemental Custom Forms Service Item Mappings.
        /// </summary>       
        public void GetPkgServiceItemCustomFormMappingDetails()
        {
            View.lstPkgServiceItemCustomFormMappingDetails = BackgroundSetupManager.GetPkgServiceItemCustomFormMappingDetails(View.TenantId, View.PackageServiceItemID);
        }

        public void DeleteBkgSvcItemFormMapping(Int32 bkgSvcItemFormMappingId)
        {
            BkgSvcItemFormMapping bkgSvcItemFormMappingObjectInDB = BackgroundSetupManager.GetBkgSvcItemFormMappingById(View.TenantId, bkgSvcItemFormMappingId);
            bkgSvcItemFormMappingObjectInDB.BSIFM_IsDeleted = true;
            bkgSvcItemFormMappingObjectInDB.BSIFM_ModifiedByID = View.currentLoggedInUserId;
            bkgSvcItemFormMappingObjectInDB.BSIFM_ModifiedOn = DateTime.Now;
            if (BackgroundSetupManager.UpdateChanges(View.TenantId))
            {
                View.SuccessMessage = "Custom Form unmapped successfully.";
            }
            else
            {
                View.ErrorMessage = "Some error has occurred.Please contact administrator.";
            }
        }
    }
}
