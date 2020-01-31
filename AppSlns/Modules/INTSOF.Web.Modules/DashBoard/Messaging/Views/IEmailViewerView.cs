using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace CoreWeb.DashBoard.Messaging.Views
{
    public interface IEmailViewerView
    {
        Int32 SystemCommunicationId { get; set; }
        List<Int32> SystemCommunicationDeliveryIds { get; set; }
        Int32 CurrentUserId { get;}
       // String Subject { set; }
        String DetailedContent { set; }   
        List<SystemCommunicationDelivery> lstCommunicationDelivery {get; set;}
    }
}
