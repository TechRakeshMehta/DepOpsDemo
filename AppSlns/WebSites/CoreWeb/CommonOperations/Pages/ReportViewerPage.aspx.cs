using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;

namespace CoreWeb.CommonOperations.Pages
{
    public partial class ReportViewerPage : BaseWebPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                //Decrypt the TenantId and SubscriptionIDs from Query String.
                if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
                {
                    queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                }

                ctlReportViewer.ReportCode = queryString["ReportCode"];

                if (!Session["SavedReportParameters"].IsNullOrEmpty())
                {
                    ctlReportViewer.Parameters = Convert.ToString(Session["SavedReportParameters"]);
                }
                ctlReportViewer.FromSavedReportFeature = true;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try { }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
    }
}