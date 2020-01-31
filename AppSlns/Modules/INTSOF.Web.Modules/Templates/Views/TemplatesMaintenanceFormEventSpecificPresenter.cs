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
    public class TemplatesMaintenanceFormEventSpecificPresenter : Presenter<ITemplatesMaintenanceFormEventSpecificView>
    {
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
        }

        // TODO: Handle other view events and set state in the view

        public void BindTenants()
        {
            Boolean SortByName = true;
            String clientCode = TenantType.Institution.GetStringValue();
            View.lstTenants = SecurityManager.GetTenants(SortByName, false, clientCode);
        }

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
            View.CommunicationSubEvents = CommunicationManager.GetCommunicationTypeSubEventsSpecific(Convert.ToInt32(View.CommunicationTypeId), Convert.ToInt32(View.EventId), View.lstCommunicationSubEventCodes);//.OrderBy(x=>x.Name).ToList();
        }

        public void BindTemplatePlaceHolders()
        {
            View.TemplatePlaceHolders = CommunicationManager.GetTemplatePlaceHolders(Convert.ToInt32(View.SubEventId));
        }

        public void BindCategories()
        {
            // View.lstCategories = TemplatesManager.GetAvailableCategories(View.SelectedTenantId);
            View.lstCategories = ComplianceSetupManager.GetComplianceCategories(View.SelectedTenantId, false);
        }

        public void BindItems()
        {
            SystemEventSetting _systemEventSetting;
            List<Entity.ClientEntity.ComplianceItem> _lstItems = ComplianceSetupManager.GetComplianceItemsByCategory(View.SelectedCategoryId, View.SelectedTenantId, out _systemEventSetting, Convert.ToInt32(View.SubEventId));
            List<Int32?> _lstItemIdsInUse = CommunicationManager.GetItemIdsInUse(View.SelectedCategoryId, Convert.ToInt32(View.SubEventId), View.SelectedTenantId);

            if (_systemEventSetting.IsNotNull() && _systemEventSetting.SES_ID > 0)
            {
                //View.CategoryLevelTemplate = _communicationTemplate;
                View.TemplateName = _systemEventSetting.CommunicationTemplate.Name;
                View.TemplateSubject = _systemEventSetting.CommunicationTemplate.Subject;
                View.CommunicationTypeId = _systemEventSetting.CommunicationTemplate.lkpCommunicationSubEvent.CommunicationTypeID;
                View.SubEventId = _systemEventSetting.CommunicationTemplate.lkpCommunicationSubEvent.CommunicationSubEventID;
                View.EventId = _systemEventSetting.CommunicationTemplate.lkpCommunicationSubEvent.CommunicationEventID;
                View.NoOfDays = _systemEventSetting.NoOfDays;
                View.Frequency = _systemEventSetting.Frequency;
                View.TemplateContent = _systemEventSetting.CommunicationTemplate.Content;
                View.DaysBefore = _systemEventSetting.DaysBefore;
            }


            if (View.TemplateId > 0)
            {
                View.lstItems = _lstItems.Where(itm => !_lstItemIdsInUse.Contains(itm.ComplianceItemID) || itm.ComplianceItemID == View.ItemIdEdited).ToList();
            }
            else
            {
                View.lstItems = _lstItems.Where(itm => !_lstItemIdsInUse.Contains(itm.ComplianceItemID)).ToList();
            }
        }

        public Boolean SaveUpdateTemplate()
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

            if (View.SelectedTenantId > 0)
            {
                _systemEventTemplatesContract.EventSettings = new SystemEventSetting
                {
                    TenantID = View.SelectedTenantId,
                    ComplianceCategoryID = View.SelectedCategoryId,
                    ComplianceItemID = View.SelectedItemId,
                    CommunicationSubEventID = Convert.ToInt32(View.SubEventId),
                    Frequency = View.Frequency,
                    NoOfDays = View.NoOfDays,
                    SES_ID = View.SESId,
                    DaysBefore = View.DaysBefore,
                    IsNotificationBlocked = View.IsNotificationBlocked
                };
            }
            return TemplatesManager.SaveUpdateTemplate(_systemEventTemplatesContract);
        }

        public void GetTemplateDetails()
        {
            View.SystemEventTemplate = TemplatesManager.GetTemplateDetails(View.TemplateId);
        }

        public void SaveMailContent(Dictionary<String, object> content, Int32 subEventId, CommunicationTemplateContract communicationTemplateContract)
        {
            //  CommunicationManager.SaveMailContent(subEventId, content, communicationTemplateContract);
        }

        public Int32 GetSubEventTypeIDByCode(String code)
        {
            var commsubEvent = LookupManager.GetMessagingLookUpData<lkpCommunicationSubEvent>().Where(cond => cond.IsDeleted == false && cond.Code == code).FirstOrDefault();
            if (commsubEvent.IsNotNull())
            {
                return commsubEvent.CommunicationSubEventID;
            }
            return 0;
        }

        //UAT-1793 : Should not be able to create duplicate templates in the common template section of the System Template screen.
        public void GetSubEventsHavingTemplatesByTenant()
        {
            View.SubEventIdsWithTemplates = CommunicationManager.GetSubEventsHavingTemplatesByTenant(View.SelectedTenantId,(Int32?)null);
        }

        public void BindLanguages()
        {
            View.CommunicationLanguages = CommunicationManager.GetLanguages();
        }
    }
}




