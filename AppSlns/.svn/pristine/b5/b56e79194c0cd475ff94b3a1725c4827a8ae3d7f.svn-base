using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.BkgOperations.Views;
using INTSOF.Utils;

namespace CoreWeb.BkgOperations.Views
{
    public partial class ManageInstitutionNode : BaseUserControl, IManageIntitutionNode
    {
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = AppConsts.TITLE_MANAGE_INSTITUTION_NODE;
                base.SetPageTitle(AppConsts.TITLE_MANAGE_INSTITUTION_NODE);
                base.OnInit(e);
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

        }
    }
}