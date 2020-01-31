using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace WidgetDataWebService
{
   
    [ServiceContract]
    public interface IDataFeed
    {
      
        [WebInvoke(UriTemplate = "GetXmlData", Method = "POST", ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Wrapped)]
        [OperationContract]
        Dictionary<String,String> GetXmlData(Int32 tenantID, Int32 settingID,Guid accessKey, String recordOriginStartDate, String recordOriginEndDate);
 
    }
}
