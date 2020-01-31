using System;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using CoreWeb.Shell;
using Telerik.Web.UI;
using System.Collections.Generic;
using INTSOF.UI.Contract.ComplianceManagement;
using CoreWeb.IntsofSecurityModel;
using INTSOF.Utils;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class RulesetInfo : BaseWebPage, IRulesetInfoView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables
        
        private RulesetInfoPresenter _presenter=new RulesetInfoPresenter();
        private ComplianceRuleSetContract _viewContract;
        private String _viewType;
        private Int32 _tenantid;

        #endregion
        #endregion

        #region Properties

        
        public RulesetInfoPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        public IRulesetInfoView CurrentViewContext
        {
            get { return this; }
        }

        public ComplianceRuleSetContract ViewContract
        {
            get
            {
                if (_viewContract == null)
                {
                    _viewContract = new ComplianceRuleSetContract();
                }
                return _viewContract;
            }
        }

        public RuleSet ComplianceRuleSet
        {
            get;
            set;

        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set { _tenantid = value; }
        }

        public Int32 CurrentRuleSetId
        {
            get
            {
                return Convert.ToInt32(ViewState["currentRuleSetId"]);
            }
            set
            {
                ViewState["currentRuleSetId"] = value;
            }
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public List<lkpRuleType> RuleSetType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                CurrentRuleSetId = Convert.ToInt32(Request.QueryString["Id"]);
                SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                ViewContract.RuleSetId = CurrentRuleSetId;
                BindRuleSetInfo();
                ResetButtons(true);
                SetFormMode(false);
            }
            Presenter.OnViewLoaded();
        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarRuleInfo_CancelClick(object sender, EventArgs e)
        {
            BindRuleSetInfo();
            ResetButtons(true);
            SetFormMode(false);
        }

        /// <summary>
        /// Submit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarRuleInfo_SubmitClick(object sender, EventArgs e)
        {
            ResetButtons(false);
            SetFormMode(true);
        }

        /// <summary>
        /// Save button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarRuleInfo_SaveClick(object sender, EventArgs e)
        {
            CurrentViewContext.ViewContract.Name = txtName.Text.Trim();
            //CurrentViewContext.ViewContract.RuleSetTypeID = Convert.ToInt32(cmbRuleSetType.SelectedValue);
            CurrentViewContext.ViewContract.IsActive = chkActive.Checked;
            CurrentViewContext.ViewContract.Description = txtDescription.Text;
            CurrentViewContext.ViewContract.RuleSetId = CurrentRuleSetId;
            Presenter.UpdateRuleSetDetail();
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
            if (ErrorMessage == String.Empty || ErrorMessage ==null)
            {
                base.ShowSuccessMessage("Ruleset updated successfully.");
            }
            else
            {
                base.ShowInfoMessage(ErrorMessage);
            }
            ResetButtons(true);
            SetFormMode(false);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// To bind Package Info controls
        /// </summary>
        public void BindRuleSetInfo()
        {
            Presenter.GetRuleSetInfoByID();
            Presenter.GetRuleSetTypeList();
            txtName.Text = CurrentViewContext.ComplianceRuleSet.RLS_Name;
           // cmbRuleSetType.SelectedValue = CurrentViewContext.ComplianceRuleSet.RLS_RuleSetType.ToString();
            chkActive.Checked = CurrentViewContext.ComplianceRuleSet.RLS_IsActive;
            txtDescription.Text = CurrentViewContext.ComplianceRuleSet.RLS_Description;
            //cmbRuleSetType.DataSource = CurrentViewContext.RuleSetType;
            //cmbRuleSetType.DataBind();
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///  To show hide the buttons
        /// </summary>
        /// <param name="isReset"></param>
        private void ResetButtons(Boolean isReset)
        {
            fsucCmdBarRuleInfo.SaveButton.Visible = !isReset;
            fsucCmdBarRuleInfo.CancelButton.Visible = !isReset;
            fsucCmdBarRuleInfo.SubmitButton.Visible = isReset;
            fsucCmdBarRuleInfo.SubmitButton.Text = "Edit";
        }

        /// <summary>
        /// To set Form Mode (Readonly true or false)
        /// </summary>
        /// <param name="isReadOnly"></param>
        private void SetFormMode(Boolean isEnable)
        {
            txtName.Enabled = isEnable;
            //cmbRuleSetType.Enabled = isEnable;
            chkActive.IsActiveEnable = isEnable;
            txtDescription.Enabled = isEnable;
        }

        #endregion

        #endregion
    }
}

