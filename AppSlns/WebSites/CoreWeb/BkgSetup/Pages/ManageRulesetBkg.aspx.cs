using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageRulesetBkg : BaseWebPage
    {
        #region Public Properties
        public Int32 BkgRuleSetId
        {
            get
            {
                return Convert.ToInt32(ViewState["BkgRuleSetId"]);
            }
            set
            {
                ViewState["BkgRuleSetId"] = value;
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
                        BkgRuleSetId = Convert.ToInt32(Request.QueryString["Id"]);
                    }
                    if (!Request.QueryString["PackageId"].IsNullOrEmpty())
                    {
                        PackageId = Convert.ToInt32(Request.QueryString["PackageId"]);
                    }
                    SetRuleSetdetailUCProperties();
                    SetRuleListUCProperties();
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
        private void SetRuleListUCProperties()
        {
            ucRuleList.SelectedTenantId = SelectedTenantId;
            ucRuleList.RuleSetId = BkgRuleSetId;
            ucRuleList.PackageId = PackageId;
        }

        private void SetRuleSetdetailUCProperties()
        {
            ucRuleSetDetail.SelectedTenantId = SelectedTenantId;
            ucRuleSetDetail.CurrentRuleSetId = BkgRuleSetId;
        }
        #endregion
        
    }
}