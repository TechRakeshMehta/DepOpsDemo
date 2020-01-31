using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public class ManageBkgServiceCustomFormPresenter : Presenter<IManageBkgServiceCustomFormView>
    {
        public void GetCustomFormsForService()
        {
            View.MappedCustomFormList = BackgroundSetupManager.GetCustomFormsForService(View.ServiceId).OrderBy(x => x.SequenceOrder).ToList();
        }
        //List<CustomForm> GetAllCustomForm(Int32 serviceId) 
        public void GetAllCustomForm()
        {
            View.CustomFormList = BackgroundSetupManager.GetAllCustomForm(View.ServiceId);
        }

        public void SaveSvcFormMapping(List<Int32> ChkFormIdsLst)
        {
            if (ChkFormIdsLst.IsNullOrEmpty() && ChkFormIdsLst.Count() < 0)
            {
                View.ErrorMessage = "Please Select atleast one Custom Form";
                return;
            }
            else
            {

                foreach (Int32 FormId in ChkFormIdsLst)
                {
                    Entity.BkgSvcFormMapping newSvcFormMapping = new Entity.BkgSvcFormMapping();
                    newSvcFormMapping.BSFM_BackgroundServiceID = View.ServiceId;
                    newSvcFormMapping.BSFM_CustomFormId = FormId;
                    newSvcFormMapping.BSFM_CreatedBy = View.CurrentLoggedInUserId;
                    newSvcFormMapping.BSFM_CreatedOn = DateTime.Now;
                    newSvcFormMapping.BSFM_IsDeleted = false;
                    BackgroundSetupManager.SaveSvcFormMapping(newSvcFormMapping);
                }

            }
        }

        //void DeleteSvcFormMApping(Int32 svcFormMappingID, Int32 currentLoggedInUserId)
        public void DeleteSvcFormMApping(Int32 svcFormMappingID)
        {
            if (svcFormMappingID.IsNotNull() && svcFormMappingID > 0)
            {
                BackgroundSetupManager.DeleteSvcFormMApping(svcFormMappingID, View.CurrentLoggedInUserId);
            }
            else 
            {
                View.ErrorMessage = "Some error occur. Please contact system administrator or try again later";
                //Document not found. Please contact system administrator or try again later.
            }
        }
    }
}
