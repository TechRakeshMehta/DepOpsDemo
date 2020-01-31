using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.Templates
{
    [Serializable]
    public class CommunicationCopySettingsOverrideContract
    {
        public Int32 CommunicationNodeCopySettingID { get; set; }
        public Int32 OrganizationUserID { get; set; }
        public String Email { get; set; }
        public String UserName { get; set; }
        public String HierarchyLabel { get; set; }
        public Int32 HierarchyNodeID { get; set; }
        public Int32 NodeCopySettingID { get; set; }
        public String NodeCopySettingCode { get; set; }
        public String NodeCopySettingName { get; set; }
    }

    //UAT-3348
    [Serializable]
    public class CommunicationSettingsSubEventsContract
    {
        public Int32 CommunicationOverRideId { get; set; }
        public Int32 CommunicationTypeID { get; set; }
        public String CommunicationTypeName { get; set; }
        public Int32 CommunciationSubEventID { get; set; }
        public String CommunciationEventName { get; set; }
        public String CommunciationSubEventName { get; set; }
        public Int32 NodeCopySettingID { get; set; }
        public String NodeCopySettingName { get; set; }
        public Int32 CommunicationNodeSubeventsCopySettingID { get; set; }
    }
}
