using System;
using System.Web.Script.Services;
using System.Web.Services;


namespace WidgetDataWebService
{
    /// <summary>
    /// Summary description for WidgetData
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WidgetData : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Get(string tenantId, string widgetName, string parameters,Boolean useDefaultTenantId)
        {
            try
            {

                //JavaScriptSerializer s = new JavaScriptSerializer();
                string jsonRecordSets = Controller.WebServiceController.GetData(tenantId, widgetName, parameters, useDefaultTenantId);
                //this.Context.Response.ContentType = "application/json; charset=utf-8";
                //this.Context.Response.Write(s.Serialize(jsonRecordSets));
                return jsonRecordSets;
            }
            catch (Exception ex)
            {
                return "Error;" + ex.Message;
            }
        }

    }
}
