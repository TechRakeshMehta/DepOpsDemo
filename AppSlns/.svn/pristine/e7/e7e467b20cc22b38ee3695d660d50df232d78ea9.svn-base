using CoreWeb.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.BkgSetup.Views
{
    public partial class RuleSetInfoBkg : BaseUserControl, IRulesetInfoBkgView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private RulesetInfoBkgPresenter _presenter = new RulesetInfoBkgPresenter();

        private Int32 _tenantid;
        private ComplianceRuleSetContract _viewContract;
        #endregion
        #endregion

        #region Properties


        public RulesetInfoBkgPresenter Presenter
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

        public IRulesetInfoBkgView CurrentViewContext
        {
            get { return this; }
        }


        public BkgRuleSet BkgRuleSet
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

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                //CurrentViewContext.CurrentRuleSetId = Convert.ToInt32(Request.QueryString["Id"]);
                //CurrentViewContext.SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                BindRuleSetInfo();
                ResetButtons(true);
                SetFormMode(false);
                ApplyActionLevelPermission(ActionCollection, "Rule set Info Bkg");
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
            CurrentViewContext.ViewContract.IsActive = chkActive.Checked;
            CurrentViewContext.ViewContract.Description = txtDescription.Text;
            CurrentViewContext.ViewContract.RuleSetId = CurrentRuleSetId;
            Presenter.UpdateRuleSetDetail();
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
            if (ErrorMessage == String.Empty || ErrorMessage == null)
            {
                (this.Page as BaseWebPage).ShowSuccessMessage("Ruleset updated successfully.");
            }
            else
            {
                (this.Page as BaseWebPage).ShowInfoMessage(ErrorMessage);
            }
            ResetButtons(true);
            SetFormMode(false);
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// To bind RuleSet Info controls
        /// </summary>
        public void BindRuleSetInfo()
        {
            Presenter.GetRuleSetInfoByID();
            txtName.Text = CurrentViewContext.BkgRuleSet.BRLS_Name;
            chkActive.Checked = CurrentViewContext.BkgRuleSet.BRLS_IsActive;
            txtDescription.Text = CurrentViewContext.BkgRuleSet.BRLS_Description;
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
            chkActive.IsActiveEnable = isEnable;
            txtDescription.Enabled = isEnable;
        }

        #endregion

        #region Apply Permissions

        protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            if (permission.IsNotNull())
            {
                permission.ForEach(x =>
                {
                    switch (x.PermissionID)
                    {
                        case AppConsts.ONE:
                            {
                                break;
                            }
                        case AppConsts.THREE:
                            {
                                if (x.FeatureAction.CustomActionId == "EditRuleSet")
                                {
                                    fsucCmdBarRuleInfo.SubmitButton.Enabled = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "EditRuleSet")
                                {
                                    fsucCmdBarRuleInfo.SubmitButton.Visible = false;
                                }
                                break;
                            }
                    }

                });
            }
        }

        #endregion

        #endregion
    }
}