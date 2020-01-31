using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.Utils;

namespace CoreWeb.SystemSetUp.Views
{
    public class CreateAccountInvitationPresenter : Presenter<ICreateAccountInvitationView>
    {
        public override void OnViewInitialized()
        {
            //Bind tenant dropdown
            View.LstTenant = ComplianceDataManager.getClientTenant();
        }

        public override void OnViewLoaded()
        {

        }

        public Boolean IsDefaultTenant
        {
            get
            {
                return SecurityManager.DefaultTenantID == View.TenantID;
            }
        }

        /// <summary>
        /// Get SubEvent ID and bind the TemplatePlacehoders in the ContentEditor
        /// </summary>
        public void GetPlaceHolderList()
        {
            //Get SubEvent ID
            List<String> subEventCodes = new List<String>();
            subEventCodes.Add(AppConsts.CREATE_COMPL_ACC_SUBEVNT_CODE.ToLower());
            View.SubEventID = CommunicationManager.GetCommunicationSubEventIdsByCodes(subEventCodes).FirstOrDefault();
            //Fill Placeholders List for SubEvent
            View.TemplatePlaceHolders = CommunicationManager.GetTemplatePlaceHolders(View.SubEventID);
        }

        /// <summary>
        /// Get the template for template editor
        /// </summary>
        public void GetTemplateDetails()
        {
            View.TemplateId = CommunicationManager.GetCommunicationTemplateIDForSubEventID(View.SubEventID);
            View.SystemEventTemplate = TemplatesManager.GetTemplateDetails(View.TemplateId);
        }

        public Boolean SaveAccountCreationContact(List<Entity.AccountCreationContact> accountCreationContactList)
        {
            return SecurityManager.SaveAccountCreationContact(accountCreationContactList, View.SystemEventTemplate, View.SubEventID, View.CurrentLoggedInUserId, View.SelectedTenantId, View.TemplatePlaceHolders);
        }

        public bool IsEmailAlreadyExist(string email)
        {
            return ComplianceDataManager.IsOrganisationUserExistByEmail(email, View.SelectedTenantId);
        }

        /// <summary>
        /// Is Emails already exist
        /// </summary>
        /// <param name="lstEmail"></param>
        /// <returns></returns>
        public List<String> IsEmailsAlreadyExist(List<String> lstEmail)
        {
            List<Entity.ClientEntity.OrganizationUser> lstOrganizationUser = ComplianceDataManager.GetOrganisationUsersByEmail(lstEmail, View.SelectedTenantId);
            if (!lstOrganizationUser.IsNullOrEmpty())
                return lstOrganizationUser.Select(x => x.PrimaryEmailAddress.ToLower()).ToList();
            else
                return null;
        }
    }
}
