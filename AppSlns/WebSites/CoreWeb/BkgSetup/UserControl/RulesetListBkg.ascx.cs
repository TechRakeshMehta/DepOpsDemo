using System;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Collections.Generic;
using Entity.ClientEntity;
using System.Linq;
using Telerik.Web.UI;
using CoreWeb.Shell;
using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel;

namespace CoreWeb.BkgSetup.Views
{
    public partial class RulesetListBkg : BaseUserControl, IRulesetListBkgView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private RulesetListBkgPresenter _presenter = new RulesetListBkgPresenter();
        private ComplianceRuleSetContract _viewContract;
        private String _viewType;
        private Int32 _tenantid;

        #endregion
        #endregion

        #region Properties


        public RulesetListBkgPresenter Presenter
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

        public IRulesetListBkgView CurrentViewContext
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

        public BkgRuleSet ComplianceRuleSet
        {
            get;
            set;
        }

        public List<BkgRuleSet> RuleSetList
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

        public Int32 ObjectId
        {
            get
            {
                return Convert.ToInt32(ViewState["objectId"]);
            }
            set
            {
                ViewState["objectId"] = value;
            }
        }

        public Int32 ObjectTypeId
        {
            get
            {
                return Convert.ToInt32(ViewState["objectTypeId"]);
            }
            set
            {
                ViewState["objectTypeId"] = value;
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

        #endregion

        #region PageEvents

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    ApplyActionLevelPermission(ActionCollection, "Rule set List Bkg");
                    Presenter.OnViewInitialized();
                }
                Presenter.OnViewLoaded();
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

        #region GridEvents

        protected void grdRuleSet_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetRuleSetsForObject();
                grdRuleSet.DataSource = CurrentViewContext.RuleSetList;
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

        protected void grdRuleSet_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.RuleSetId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BRLS_ID"]);
                if (Presenter.DeleteRuleSetObjectMapping())
                {
                    //bindCategories();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    (this.Page as BaseWebPage).ShowSuccessMessage("Ruleset Deleted successfully.");
                }
                else
                {
                    (this.Page as BaseWebPage).ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
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

        #region Button events

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                divAddForm.Visible = true;
                ResetControls();
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


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.Name = txtName.Text.Trim();
                CurrentViewContext.ViewContract.IsActive = chkActive.Checked;
                CurrentViewContext.ViewContract.Description = txtDescription.Text;
                Presenter.SaveNewRuleSet();
                divAddForm.Visible = false;
                ResetControls();
                grdRuleSet.Rebind();
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                if (ErrorMessage == String.Empty || ErrorMessage == null)
                {
                    (this.Page as BaseWebPage).ShowSuccessMessage("Ruleset Added successfully.");
                }
                else
                {
                    (this.Page as BaseWebPage).ShowInfoMessage(ErrorMessage);
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

        protected void btnCancel_click(object sender, EventArgs e)
        {
            try
            {
                divAddForm.Visible = false;
                ResetControls();
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
        /// <summary>
        /// 
        /// </summary>
        private void ResetControls()
        {
            txtName.Text = String.Empty;
            chkActive.Checked = true;
            txtDescription.Text = String.Empty;
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
                                if (x.FeatureAction.CustomActionId == "AddNewRuleSet")
                                {
                                    btnAdd.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "DeleteRuleSet")
                                {
                                    grdRuleSet.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "AddNewRuleSet")
                                {
                                    btnAdd.Visible = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "DeleteRuleSet")
                                {
                                    grdRuleSet.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }

        #endregion
    }
}