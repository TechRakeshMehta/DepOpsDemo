using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.RepoManagers;
using Entity;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.Templates;
using INTSOF.Utils;

namespace CoreWeb.Templates.Views
{
    public class NodeNotificationManagementPresenter : Presenter<INodeNotificationManagement>
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

        public Int32 SaveUpdateNodeNotificationTemplates(SystemEventTemplatesContract communicationTemplateContract)
        {
            return TemplatesManager.SaveUpdateNodeNotificationTemplates(communicationTemplateContract);
        }

        //public SystemEventTemplatesContract GetTemplateDetails(Int32 templateId)
        //{
        //    return TemplatesManager.GetTemplateDetails(templateId);
        //}

        public SystemEventTemplatesContract GetNodeTemplateByNotificationMappingId(Int32 nodeNotificationMappingId, Int32 tenantId)
        {
            return TemplatesManager.GetNodeTemplateByNotificationMappingId(nodeNotificationMappingId, tenantId);
        }

        public Int32 GetSubEventTypeId(String subEventTypeCode, Int32 tenantId)
        {
            List<lkpCommunicationSubEvent> _lstSubEventTypes = LookupManager.GetMessagingLookUpData<lkpCommunicationSubEvent>();
            return _lstSubEventTypes.Where(nt => nt.Code == subEventTypeCode).FirstOrDefault().CommunicationSubEventID;
        }

        public Int32 GetNodeNotificationTypeId(String notifiicationTypeCode, Int32 tenantId)
        {
            List<lkpNodeNotificationType> _lstNotificationTypes = LookupManager.GetLookUpData<lkpNodeNotificationType>(tenantId);
            return _lstNotificationTypes.Where(nt => nt.NNT_Code == notifiicationTypeCode).FirstOrDefault().NNT_ID;
        }

        public String GetNodeNameById(Int32 dpmId, Int32 tenantId)
        {
            DeptProgramMapping _deptProgramMapping = ComplianceSetupManager.GetDepartmentProgMapping(tenantId, dpmId);
            if (!_deptProgramMapping.IsNullOrEmpty())
                return _deptProgramMapping.DPM_Label;
            else
                return String.Empty;
        }
    }
}
