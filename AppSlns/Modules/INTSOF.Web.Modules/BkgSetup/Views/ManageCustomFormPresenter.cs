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
    public class ManageCustomFormPresenter : Presenter<IManageCustomFormView>
    {
        public override void OnViewInitialized()
        {
            View.DefaultTenantId = SecurityManager.DefaultTenantID;
        }

        /// <summary>
        /// Gets all the Custom Forms 
        /// </summary>
        public void GetAllCustomForms()
        {
            View.CustomForms = BackgroundSetupManager.GetAllCustomForms();
        }

        /// <summary>
        /// Saves the custom forms in the MasterDB.
        /// </summary>
        public void SaveCustomForm()
        {
            if (BackgroundSetupManager.CheckIfCustomFormNameAlreadyExist(View.ViewContract.CustomFormName))
            {
                View.ErrorMessage = "Custom Form Name can not be duplicate.";
            }
            else
            {
                CustomForm newcustomform = new CustomForm
                {
                    CF_Name = View.ViewContract.CustomFormName,
                    CF_Description = View.ViewContract.CustomFormDesc,
                    CF_IsEditable = true,
                    CF_IsSystemPreConfigured = false,
                    CF_Title = View.ViewContract.CustomFormTitle,
                    CF_CustomFormTypeID=View.ViewContract.SelectedCustomFormTypeID,
                };
                BackgroundSetupManager.SaveCustomFormDetail(newcustomform, View.CurrentLoggedInUserId);
            }
        }

        /// <summary>
        /// Updates the custom form.
        /// </summary>
        public void UpdateCustomForm()
        {
            if (BackgroundSetupManager.GetCurrentCustomFormInfo(View.ViewContract.CustomFormID) == null)
            {
                View.ErrorMessage = "Custom Form does not exist.";
            }
            else
            {
                CustomForm newcustomform = new CustomForm
                {
                    CF_Name = View.ViewContract.CustomFormName,
                    CF_Description = View.ViewContract.CustomFormDesc,
                    CF_Title = View.ViewContract.CustomFormTitle,
                    CF_CustomFormTypeID=View.ViewContract.SelectedCustomFormTypeID,
                };
                BackgroundSetupManager.UpdateCustomFormDetail(newcustomform, View.ViewContract.CustomFormID, View.CurrentLoggedInUserId);
            }
        }

        /// <summary>
        ///Updates Custom Form Sequence.
        /// </summary>
        public Boolean UpdateCustomFormSequence(IList<CustomForm> customFormsToMove, Int32 destinationIndex)
        {
            return BackgroundSetupManager.UpdateCustomFormSequence(customFormsToMove, destinationIndex, View.CurrentLoggedInUserId);
        }

        /// <summary>
        /// Deletes the custom forms.
        /// </summary>
        public Boolean DeleteCustomForm()
        {
            IntegrityCheckResponse response = BackgroundServiceIntegrityManager.CheckIfCustomFormCanBeDeleted(View.ViewContract.CustomFormID);
            if (response.CheckStatus == CheckStatus.True)
            {
                CustomForm customForm = getCurrentCustomFormInfo(View.ViewContract.CustomFormID);
                View.ErrorMessage = String.Format(response.UIMessage, customForm.CF_Name);
                return false;
            }
            else
            {
                Boolean result = BackgroundSetupManager.DeleteCustomForm(View.ViewContract.CustomFormID, View.CurrentLoggedInUserId);
                if (!result)
                    View.ErrorMessage = "Custom Form can not be deleted. Try again!";
                return result;
            }
        }

        /// <summary>
        /// Gets specific custom Form from master DB.
        /// </summary>
        /// <param name="customFormId">CustomFormID</param>
        public CustomForm getCurrentCustomFormInfo(Int32 customFormID)
        {
            return BackgroundSetupManager.GetCurrentCustomFormInfo(customFormID);
        }

        public void GetcustomFormType() 
        {
            View.LstCutomFormType = BackgroundSetupManager.GetcustomFormType();
        
        }

    }
}
