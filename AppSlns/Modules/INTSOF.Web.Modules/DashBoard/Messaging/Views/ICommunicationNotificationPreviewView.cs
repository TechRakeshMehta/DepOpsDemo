using System;
using System.Collections.Generic;
using System.Text;

namespace CoreWeb.Messaging.Views
{
    public interface ICommunicationNotificationPreviewView
    {
        Int32 SystemCommunicationId { get; set; }

        String Subject { set; }

        String DetailedContent { set; }

        ICommunicationNotificationPreviewView CurrentContext { get; }
    }
}




