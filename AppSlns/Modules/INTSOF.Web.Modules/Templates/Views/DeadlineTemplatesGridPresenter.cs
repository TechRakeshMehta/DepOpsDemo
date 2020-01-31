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
    public class DeadlineTemplatesGridPresenter : Presenter<IDeadlineTemplatesGridView>
    {
        public List<NodeTemplatesContract> GetNodeTemplates(Int32 subEventId, Int32 tenantId)
        {
            return TemplatesManager.GetNodeTemplates(subEventId, tenantId);
        }

        public void DeleteTemplate(Int32 templateId, Int32 currentUserId)
        {
            TemplatesManager.DeleteTemplate(templateId, currentUserId, true);
        }

        public Int32 GetNodeNotificationTypeId(String notifiicationTypeCode, Int32 tenantId)
        {
            List<lkpNodeNotificationType> _lstNotificationTypes = LookupManager.GetLookUpData<lkpNodeNotificationType>(tenantId);
            return _lstNotificationTypes.Where(nt => nt.NNT_Code == notifiicationTypeCode).FirstOrDefault().NNT_ID;
        }
        public Int32 GetSubEventTypeId(String subEventTypeCode, Int32 tenantId)
        {
            List<lkpCommunicationSubEvent> _lstSubEventTypes = LookupManager.GetMessagingLookUpData<lkpCommunicationSubEvent>();
            return _lstSubEventTypes.Where(nt => nt.Code == subEventTypeCode).FirstOrDefault().CommunicationSubEventID;
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
