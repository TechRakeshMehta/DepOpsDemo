using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class RuleListBkg : BaseUserControl, IRuleListBkgView
    {
        #region Variable

        #region Private Variables

        private RuleListBkgPresenter _presenter = new RuleListBkgPresenter();
        Int32 tenantid = 0;

        #endregion

        #region Public variables

        public RuleListBkgPresenter Presenter
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

        public IRuleListBkgView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 RuleSetId
        {
            get
            {
                if (ViewState["RuleSetId"] == null)
                    return 0;
                return Convert.ToInt32(ViewState["RuleSetId"]);
            }
            set
            {
                ViewState["RuleSetId"] = value;
            }
        }

        public List<BkgRuleMapping> lstRuleMapping
        {
            get;
            set;
        }

        public Int32 RuleMappingId
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

        public Int32 TenantId
        {
            get
            {
                if (tenantid == 0)
                {
                    //tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantid;
            }
            set { tenantid = value; }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public String ErrorMessage
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
        #endregion


        #region Events
        #region Page Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    ApplyActionLevelPermission(ActionCollection, "Rule List Bkg");
                    ucRuleDetail.SelectedTenantId = CurrentViewContext.SelectedTenantId;
                    ucRuleDetail.PackageId = PackageId;
                    ucRuleDetail.RuleSetId = CurrentViewContext.RuleSetId;
                }
                //ucRuleDetail.NotifyStatusChange -= ucRuleDetail_NotifyStatusChange;
                ucRuleDetail.NotifyStatusChange += ucRuleDetail_NotifyStatusChange;
                ucRuleDetail.NotifyCancelClick -= ucRuleDetail_NotifyCancelClick;
                ucRuleDetail.NotifyCancelClick += ucRuleDetail_NotifyCancelClick;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #region Gridevents
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRules_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetRuleMappings();
                grdRules.DataSource = lstRuleMapping;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRules_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    CurrentViewContext.RuleMappingId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BRLM_ID"]);
                    if (Presenter.DeleteRuleMapping())
                    {
                        grdRules.Rebind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                        (this.Page as BaseWebPage).ShowSuccessMessage("Rule deleted successfully.");
                    }
                    else
                    {
                        (this.Page as BaseWebPage).ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion
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
                                if (x.FeatureAction.CustomActionId == "DeleteRule")
                                {
                                    grdRules.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                if (x.FeatureAction.CustomActionId == "AddRule")
                                {
                                    btnAdd.Enabled = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "DeleteRule")
                                {
                                    grdRules.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                if (x.FeatureAction.CustomActionId == "AddRule")
                                {
                                    btnAdd.Visible = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }

        #endregion

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            divAddForm.Visible = true;
        }

        void ucRuleDetail_NotifyStatusChange(String message, Boolean isSuccess)
        {
            try
            {
                grdRules.Rebind();
                divAddForm.Visible = false;
                if (isSuccess)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    (this.Page as BaseWebPage).ShowSuccessMessage(message);
                }
                else
                    (this.Page as BaseWebPage).ShowErrorMessage(message);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        void ucRuleDetail_NotifyCancelClick()
        {
            try
            {
                divAddForm.Visible = false;
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
    }
}