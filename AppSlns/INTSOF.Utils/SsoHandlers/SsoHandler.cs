using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace INTSOF.Utils.SsoHandlers
{
    public abstract class SsoHandler
    {
        public String HostName
        {
            get
            {
                return HttpContext.Current.Session["Session_Host"].ToString();
            }
        }
        public String TargetUrl
        {
            get
            {
                if (!HostName.IsNullOrEmpty())
                {
                    return HostName + "/secure/SsoPostHandler.aspx";
                }
                return String.Empty;
            }
        }

        abstract public String ProcessSessiondata(Boolean isSaveHistoryRequired);

        abstract public SsoHandlerContract GenerateWebApplcationData(Int32 integrationClientID);
    }
}
