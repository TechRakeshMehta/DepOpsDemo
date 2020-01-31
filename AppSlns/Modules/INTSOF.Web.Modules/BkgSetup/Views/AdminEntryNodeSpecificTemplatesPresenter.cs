using Business.RepoManagers;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public class AdminEntryNodeSpecificTemplatesPresenter : Presenter<IAdminEntryNodeSpecificTemplatesView>
    {

        public void GetTemplate()
        {
            String subEventCode = CommunicationSubEvents.NOTIFICATION_FOR_ADMIN_ENTRY_APPLICANT_INVITE.GetStringValue();
            View.AdminEntryNodeTemplate = BackgroundSetupManager.GetTemplate(View.TenantId, View.DeptProgramMappingID, subEventCode);
        }

        public Boolean SaveUpdateAdminEntryNodeTemplate()
        {
            String subEventCode = CommunicationSubEvents.NOTIFICATION_FOR_ADMIN_ENTRY_APPLICANT_INVITE.GetStringValue();
            return BackgroundSetupManager.SaveUpdateAdminEntryNodeTemplate(View.TenantId, View.DeptProgramMappingID, subEventCode, View.CurrentLoggedInUserID, View.AdminEntryNodeTemplate);
        }
        public void BindTemplatePlaceHolders()
        {
            String subEventCode = CommunicationSubEvents.NOTIFICATION_FOR_ADMIN_ENTRY_APPLICANT_INVITE.GetStringValue();
            Int32 subEventId = TemplatesManager.GetSubEventIdByCode(subEventCode);
            if (subEventId > AppConsts.NONE)
                View.lstTemplatePlaceHolders = CommunicationManager.GetTemplatePlaceHolders(subEventId);
        }
    }
}
