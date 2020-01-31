using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageRulesBkg : BaseWebPage
    {
        #region Public Properties
        public Int32 BkgRuleMappingId
        {
            get
            {
                return Convert.ToInt32(ViewState["BkgRuleMappingId"]);
            }
            set
            {
                ViewState["BkgRuleMappingId"] = value;
            }
        }

        public Int32 SelectedTenantId
        {
            get;
            set;
        }

        public Int32 PackageId
        {
            get;
            set;
        }

        #endregion

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if (!Request.QueryString["SelectedTenantId"].IsNullOrEmpty())
                    {
                        SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    }
                    if (!Request.QueryString["Id"].IsNullOrEmpty())
                    {
                        BkgRuleMappingId = Convert.ToInt32(Request.QueryString["Id"]);
                    }
                    if (!Request.QueryString["PackageId"].IsNullOrEmpty())
                    {
                        PackageId = Convert.ToInt32(Request.QueryString["PackageId"]);
                    }
                    SetRuledetailUCProperties();
                }
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
        #endregion

        #region Private Methods
        private void SetRuledetailUCProperties()
        {
            ucRuleInfo.SelectedTenantId = SelectedTenantId;
            ucRuleInfo.RuleMappingId = BkgRuleMappingId;
            ucRuleInfo.PackageId = PackageId;
        }

        
        #endregion
    }
}