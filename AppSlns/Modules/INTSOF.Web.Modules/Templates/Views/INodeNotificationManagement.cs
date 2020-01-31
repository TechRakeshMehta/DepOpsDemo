using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.Templates.Views
{
    public interface INodeNotificationManagement
    {
        Int32 SelectedTenantId { get; set; }
        Int32 SubEventId { get; set; }
        Int32 NodeNotificationTypeId { get; set; }
        Int32 CommunicationTemplateId { get; set; }
    }
}
