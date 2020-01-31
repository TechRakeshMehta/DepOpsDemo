using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Business.RepoManagers;
using Entity;
using INTSOF.UI.Contract.Templates;
using INTSOF.Utils;
using System.Linq;

namespace CoreWeb.Templates.Views
{
    public class TemplatesMaintenanceFormPresenter : Presenter<ITemplatesMaintenanceFormView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private ITemplatesController _controller;
        // public TemplatesMaintenanceFormPresenter([CreateNew] ITemplatesController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
           
        }

        // TODO: Handle other view events and set state in the view

        public void BindCommunicationTypes()
        {
            View.CommunicationTypes = CommunicationManager.GetCommunicationTypes();
        }

        public void BindEvents()
        {
            View.CommunicationEvents = CommunicationManager.GetCommunicationEvents(Convert.ToInt32(View.CommunicationTypeId));
        }

        public void BindSubEvents()
        {
            View.CommunicationSubEvents = CommunicationManager.GetCommunicationTypeSubEvents(Convert.ToInt32(View.CommunicationTypeId), Convert.ToInt32(View.EventId))
                                                                    .Where(cond => cond.Code != CommunicationSubEvents.EXTERNAL_EMAIL_NOTIFICATION.GetStringValue())
                                                                    .OrderBy(x => x.Name).ToList();//UAT sort dropdowns by Name;
        }

        public void BindLanguages()
        {
            View.CommunicationLanguages = CommunicationManager.GetLanguages();
        }

        public void BindTemplates()
        {
            View.TemplatePlaceHolders = CommunicationManager.GetTemplatePlaceHolders(Convert.ToInt32(View.SubEventId));
        }

        public void SaveUpdateTemplate()
        {
            SystemEventTemplatesContract _systemEventTemplatesContract = new SystemEventTemplatesContract();
            _systemEventTemplatesContract.SubEventId = Convert.ToInt32(View.SubEventId);
            _systemEventTemplatesContract.TemplateName = View.TemplateName;
            _systemEventTemplatesContract.TemplateDescription = View.TemplateDescription;
            _systemEventTemplatesContract.Subject = View.TemplateSubject;
            _systemEventTemplatesContract.TemplateContent = View.TemplateContent;
            _systemEventTemplatesContract.CurrentUserId = View.CurrentUserId;
            _systemEventTemplatesContract.CommunicationTemplateId = View.TemplateId;
            _systemEventTemplatesContract.CommunicationLanguageId = View.CommunicationLanguageId;

            TemplatesManager.SaveUpdateTemplate(_systemEventTemplatesContract);
        }

        public void GetTemplateDetails()
        {
            View.SystemEventTemplate = TemplatesManager.GetTemplateDetails(View.TemplateId);
        }

        public void SaveMailContent(Dictionary<String, object> content, Int32 subEventId, CommunicationTemplateContract communicationTemplateContract)
        {
            //  CommunicationManager.SaveMailContent(subEventId, content, communicationTemplateContract);
        }
        //UAT-1793: getting sub events whose template is already created-----
        public void GetSubEventsHavingTemplates()
        {
            View.SubEventIdsWithTemplates = CommunicationManager.GetSubEventsHavingTemplates(View.CommunicationLanguageId);
        }
    }
}




