using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.BkgOperations.Pages
{
    public partial class ShowResultData : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                BindServiceResults();
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

        private void BindServiceResults()
        {

            SessionForSupplementServiceCustomForm supplementServiceCustomFormTemp = new SessionForSupplementServiceCustomForm();
            supplementServiceCustomFormTemp = (SessionForSupplementServiceCustomForm)SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.SUPPLEMENT_NATIONWIDE_SERVICE_RESULTS);
            if (!supplementServiceCustomFormTemp.IsNullOrEmpty() && !supplementServiceCustomFormTemp.lstFlaggedAndNotParshedResultData.IsNullOrEmpty())
            {
                rptrServiceResult.DataSource = supplementServiceCustomFormTemp.lstFlaggedAndNotParshedResultData;
                rptrServiceResult.DataBind();
            }
            else
            {
                base.ShowInfoMessage("No result found to display.");
            }
        }
    }
}