using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Entity.ClientEntity;
using Business.RepoManagers;
using Entity;
using INTSOF.UI.Contract.Templates;

namespace CoreWeb.Templates.Views
{
    public class DeadlineTemplateMaintenanceFormPresenter : Presenter<IDeadlineTemplateMaintenanceForm>
    {
        public List<lkpNodeNotificationType> GetNodeNotificationTypes(Int32 tenantId)
        {
            return LookupManager.GetLookUpData<lkpNodeNotificationType>(tenantId);
        }

        public List<lkpCommunicationType> GetCommunicationTypes()
        {
            return CommunicationManager.GetCommunicationTypes();
        }

        public List<lkpCommunicationEvent> GetEvents(Int32 communicationTypeId)
        {
            return CommunicationManager.GetCommunicationEvents(Convert.ToInt32(communicationTypeId));
        }

        public List<lkpCommunicationSubEvent> GetSubEvents(Int32 communicationTypeId, Int32 subEventId)
        {
            return CommunicationManager.GetCommunicationTypeSubEvents(communicationTypeId, subEventId);
        }

        public List<NodeNotificationSpecificTemplates> GetNodeSpecific(String query, Int32 tenantId)
        {
            return StoredProcedureManagers.GetNodeTemplatesByQuery(query, tenantId);
        }

        /// <summary>
        /// Get the Placeholders for the SubEvent
        /// </summary>
        public List<CommunicationTemplatePlaceHolder> GetTemplatePlaceHolders(Int32 subEventId)
        {
            return CommunicationManager.GetTemplatePlaceHolders(subEventId);
        }

        public void SaveUpdateNodeNotificationTemplates(SystemEventTemplatesContract communicationTemplateContract)
        {
            TemplatesManager.SaveUpdateNodeNotificationTemplates(communicationTemplateContract);
        }

        public SystemEventTemplatesContract GetTemplateDetails(Int32 templateId)
        {
            return TemplatesManager.GetTemplateDetails(templateId);
        }
    }
}
