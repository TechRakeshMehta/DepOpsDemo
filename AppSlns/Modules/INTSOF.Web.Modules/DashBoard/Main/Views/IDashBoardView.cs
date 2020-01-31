using System;
using System.Collections.Generic;
using System.Text;
using Entity;

namespace CoreWeb.Main.Views
{
    public interface IDashBoardView
    {
        Int32 QueueType  { set; }
        Int32 CurrentUserId { get; }
        Guid MessageId { get; set; }
        List<MessageDetail> lstRecentMessages { get; set; }
        IDashBoardView CurrentViewContext { get; }
    }
}




