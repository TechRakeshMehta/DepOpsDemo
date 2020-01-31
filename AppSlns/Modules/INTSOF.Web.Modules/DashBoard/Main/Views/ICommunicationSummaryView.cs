#region Namespaces

#region System Defined
using System;
using System.Collections.Generic;
using System.Text;
#endregion

#region Project Defined
using Entity;
#endregion
#endregion

namespace CoreWeb.Main.Views
{
    public interface ICommunicationSummaryView
    {
        Int32 QueueType { set; }
        Int32 CurrentUserId { get; }
        Guid MessageId { get; set; }
        List<MessageDetail> lstRecentMessages { get; set; }
        ICommunicationSummaryView CurrentViewContext { get; }
    }
}




