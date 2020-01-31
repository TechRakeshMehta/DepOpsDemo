using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using INTSOF.Complio.API.Core;

namespace INTSOF.Complio.API.Core
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IComplioAPI" in both code and config file together.
    [ServiceContract]
    public interface IComplioAPI
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetData", BodyStyle = WebMessageBodyStyle.Bare)]
        String GetData(RequestData requestData);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "AddData", ResponseFormat = WebMessageFormat.Json)]
        String AddData(RequestData requestData);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UpdateData", ResponseFormat = WebMessageFormat.Json)]
        String UpdateData(RequestData requestData);
    }
}
