using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace CoreWeb.BkgOperations.Views
{
   public interface INotificationMailDetailsView
   {
       Int32 SystemCommunicationID { get; }
       SystemCommunication SystemCommunicationDetail { get; set; }
    }
}
