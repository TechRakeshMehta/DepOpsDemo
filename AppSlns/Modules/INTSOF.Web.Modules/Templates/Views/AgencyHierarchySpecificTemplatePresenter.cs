using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace CoreWeb.Templates.Views
{
    public class AgencyHierarchySpecificTemplatePresenter : Presenter<IAgencyHierarchySpecificTemplateView>
    {
        public void BindLanguages()
        {
            View.lstCommunicationLanguages = new List<Entity.lkpLanguage>();
            View.lstCommunicationLanguages = CommunicationManager.GetLanguages();
        }

        public void BindAgencyHierarchyRootNodes()
        {
            View.lstAgencyHierarchyRootNodes = new List<Entity.SharedDataEntity.AgencyHierarchy>();
            View.lstAgencyHierarchyRootNodes = AgencyHierarchyManager.GetAgencyHierarchyRootNodes();

            List<Int32?> lstAgencyHierarchyIds = TemplatesManager.GetAlreadyMappedAgencyHierarchyWithSubEvnt(View.TemplateId, View.SubEventId);
            if (!lstAgencyHierarchyIds.IsNullOrEmpty())
                View.lstAgencyHierarchyRootNodes = View.lstAgencyHierarchyRootNodes.Where(cnd => !lstAgencyHierarchyIds.Contains(cnd.AH_ID)).ToList();
        }

        public void BindCommunicationTypes()
        {
            View.lstCommunicationTypes = new List<Entity.lkpCommunicationType>();
            View.lstCommunicationTypes = CommunicationManager.GetCommunicationTypes();
        }

        public void GetAgencyHierarchyTemplateDetail()
        {
            View.SystemEventTemplate = TemplatesManager.GetAgencyHierarchyTemplateDetail(View.TemplateId);
        }

        public void BindEvents()
        {
            View.lstCommunicationEvents = CommunicationManager.GetCommunicationEvents(Convert.ToInt32(View.CommunicationTypeId));
        }

        public void BindSubEvents()
        {
            View.lstCommunicationSubEvents = CommunicationManager.GetCommunicationTypeSubEventsSpecific(Convert.ToInt32(View.CommunicationTypeId), Convert.ToInt32(View.EventId), View.lstCommunicationSubEventCodes);
        }

        public void GetSubEventsHavingAgencySpecificTemplates()
        {
            View.SubEventIdsWithTemplates = CommunicationManager.GetSubEventsHavingAgencySpecificTemplates((Int32?)null);
        }

        public void BindTemplatePlaceHolders()
        {
            View.TemplatePlaceHolders = CommunicationManager.GetTemplatePlaceHolders(Convert.ToInt32(View.SubEventId));
        }

        public Boolean SaveUpdateAgencyHierarchyTemplate()
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

            if (!View.lstAgencyHierarchyRootNodeID.IsNullOrEmpty())
            {
                _systemEventTemplatesContract.lstAgencyHierarcEventSettings = new List<Entity.SystemEventSetting>();


                foreach (Int32 agencyHierarchyRootId in View.lstAgencyHierarchyRootNodeID)
                {
                    Entity.SystemEventSetting agencyHierarchyEventSetting = new Entity.SystemEventSetting();

                    agencyHierarchyEventSetting.TenantID = null;
                    agencyHierarchyEventSetting.ComplianceCategoryID = null;
                    agencyHierarchyEventSetting.ComplianceItemID = null;
                    agencyHierarchyEventSetting.CommunicationSubEventID = Convert.ToInt32(View.SubEventId);
                    agencyHierarchyEventSetting.Frequency = null;
                    agencyHierarchyEventSetting.NoOfDays = null;
                    //if (!View.lstSESId.IsNullOrEmpty() && View.lstSESId.Count() > AppConsts.NONE && View.lstSESId.Count() >= i + 1)
                    Entity.SystemEventSetting sysEventSetting = new Entity.SystemEventSetting();
                    if (!View.SystemEventTemplate.IsNullOrEmpty() && !View.SystemEventTemplate.lstAgencyHierarcEventSettings.IsNullOrEmpty())
                        sysEventSetting = View.SystemEventTemplate.lstAgencyHierarcEventSettings.Where(c => c.AgencyHierarchyID == agencyHierarchyRootId).FirstOrDefault();

                    if (!sysEventSetting.IsNullOrEmpty() && sysEventSetting.SES_ID > AppConsts.NONE)
                        agencyHierarchyEventSetting.SES_ID = sysEventSetting.SES_ID;//View.lstSESId[i];
                    else
                        agencyHierarchyEventSetting.SES_ID = AppConsts.NONE;

                    agencyHierarchyEventSetting.DaysBefore = null;
                    agencyHierarchyEventSetting.IsNotificationBlocked = false;
                    agencyHierarchyEventSetting.AgencyHierarchyID = agencyHierarchyRootId;// View.lstAgencyHierarchyRootNodeID[i];

                    _systemEventTemplatesContract.lstAgencyHierarcEventSettings.Add(agencyHierarchyEventSetting);

                }
            }
            return TemplatesManager.SaveUpdateAgencyHierarchyTemplate(_systemEventTemplatesContract);
        }
    }
}
