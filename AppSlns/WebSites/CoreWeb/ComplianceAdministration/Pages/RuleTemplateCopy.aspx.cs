using CoreWeb.Shell;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Web.UI;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class RuleTemplateCopy : BaseWebPage, IRuleTemplateCopyView
    {
        #region Private variable

        private RuleTemplateCopyPresenter _presenter = new RuleTemplateCopyPresenter();
        private String _viewType;
        #endregion

        #region Properties

        public RuleTemplateCopyPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        Int32 IRuleTemplateCopyView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public IRuleTemplateCopyView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String IRuleTemplateCopyView.RuleTemplateName
        {
            get;
            set;
        }

        Int32 IRuleTemplateCopyView.FromTenantID
        {
            get
            {
                return Convert.ToInt32(ViewState["FromTenantID"]);
            }
            set
            {
                ViewState["FromTenantID"] = value;
            }
        }

        Int32 IRuleTemplateCopyView.ToTenantID
        {
            get;
            set;
        }

        Int32 IRuleTemplateCopyView.SelectedRuleTemplateID
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedRuleTemplateID"]);
            }
            set
            {
                ViewState["SelectedRuleTemplateID"] = value;
            }
        }

        String IRuleTemplateCopyView.ErrorMessage
        {
            get;
            set;
        }

        List<Entity.Tenant> IRuleTemplateCopyView.ListTenants
        {
            get;
            set;
        }

        Int32 IRuleTemplateCopyView.DefaultTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["DefaultTenantId"]);
            }
            set
            {
                ViewState["DefaultTenantId"] = value;
            }
        }

        #endregion
        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                BindTenants();
                CurrentViewContext.FromTenantID = Convert.ToInt32(Request.QueryString["TenantID"]);
                CurrentViewContext.SelectedRuleTemplateID = Convert.ToInt32(Request.QueryString["RuleTemplateID"]);
            }
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

        }

        #endregion

        #region Methods

        public void BindTenants()
        {
            Presenter.GetTenants();
            ddlTenantName.DataSource = CurrentViewContext.ListTenants;
            ddlTenantName.DataBind();
        }

        #endregion

        #region Events

        protected void fsucCmdBarPrice_SaveClick(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.ToTenantID = Convert.ToInt32(ddlTenantName.SelectedValue);
                CurrentViewContext.RuleTemplateName = txtRuleTemplateName.Text;
                if (Presenter.CopyRuleTemplate())
                {
                    base.ShowSuccessMessage("Rule Template copy created successfully.");
                    txtRuleTemplateName.Text = String.Empty;
                    ddlTenantName.SelectedValue = String.Empty;
                }
                else
                {
                    CurrentViewContext.ErrorMessage = "Rule Template copy failed";
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "Page.hideProgress();", true);
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

        protected void cmbOrganization_DataBound(object sender, EventArgs e)
        {
            ddlTenantName.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        #endregion

    }
}