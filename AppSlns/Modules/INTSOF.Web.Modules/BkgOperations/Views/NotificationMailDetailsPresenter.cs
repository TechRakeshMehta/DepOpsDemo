using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.SharedObjects;
using Entity;
using Business.RepoManagers;
using INTSOF.UI.Contract.SysXSecurityModel;

namespace CoreWeb.BkgOperations.Views
{
  public class NotificationMailDetailsPresenter : Presenter<INotificationMailDetailsView>
    {
      public void GetSystemCommunicationForMailData()
      {
          View.SystemCommunicationDetail = MessageManager.GetSystemCommunicationForMailData(View.SystemCommunicationID);      
      }
    }
}
