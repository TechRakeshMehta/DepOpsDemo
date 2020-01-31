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



namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class RulesetList : BaseWebPage, IRulesetListView
    {

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private RulesetListPresenter _presenter=new RulesetListPresenter();
        private ComplianceRuleSetContract _viewContract;
        private String _viewType;
        private Int32 _tenantid;

        #endregion
        #endregion

        #region Properties

        
        public RulesetListPresenter Presenter
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

        public IRulesetListView CurrentViewContext
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

        public List<RuleSet> RuleSetList
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

        public List<lkpRuleType> RuleSetType
        {
            get;
            set;
        }

        public List<RuleSet> complianceRuleSets
        {
            get;
            set;
        }

        public Int32 selectedRuleSetId
        {
            get;
            set;
        }


        public Int32 parentPackageId
        {
            get
            {
                return Convert.ToInt32(ViewState["ParentPackageId"]);
            }
            set
            {
                ViewState["ParentPackageId"] = value;
            }
        }

        public Int32 parentCategoryId
        {
            get
            {
                return Convert.ToInt32(ViewState["ParentCategoryId"]);
            }
            set
            {
                ViewState["ParentCategoryId"] = value;
            }
        }

        public Int32 parentItemId
        {
            get
            {
                return Convert.ToInt32(ViewState["ParentItemId"]);
            }
            set
            {
                ViewState["ParentItemId"] = value;
            }
        }

        public String ObjectTypeCode
        {
            get
            {
                return Presenter.getObjectTypeCodeById(ObjectTypeId);
            }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                CurrentViewContext.ObjectId = Convert.ToInt32(Request.QueryString["Id"]);
                CurrentViewContext.ObjectTypeId = Convert.ToInt32(Request.QueryString["ObjectTypeId"]);
                //bindCategories();
                if (Request.QueryString["parentPackageID"] != null)
                    CurrentViewContext.parentPackageId = Convert.ToInt32(Request.QueryString["parentPackageID"]);
                if (Request.QueryString["parentCategoryID"] != null)
                    CurrentViewContext.parentCategoryId = Convert.ToInt32(Request.QueryString["parentCategoryID"]);
                if (Request.QueryString["parentItemID"] != null)
                    CurrentViewContext.parentItemId = Convert.ToInt32(Request.QueryString["parentItemID"]);

                if (Request.QueryString["SelectedTenantId"] != null)
                {
                    SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                }
            }
            Presenter.OnViewLoaded();
        }

        public void bindCategories()
        {
            //Presenter.GetMasterRuleSetsForObject();
            //cmbMaster.DataSource = CurrentViewContext.complianceRuleSets;
            //cmbMaster.DataBind();
            //Presenter.GetRuleSetTypeList();
            //cmbRuleSetType.DataSource = CurrentViewContext.RuleSetType;
            //cmbRuleSetType.DataBind();
            //cmbRuleSetType.Items.Insert(0, new RadComboBoxItem("--Select--", string.Empty));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            divAddForm.Visible = true;
            //divCreate.Visible = String.IsNullOrEmpty(cmbMaster.SelectedValue);
            ResetControls();
        }

        private void ResetControls()
        {
            txtName.Text = String.Empty;
            //cmbRuleSetType.SelectedIndex = 0;
            chkActive.Checked = true;
            txtDescription.Text = String.Empty;
        }

        protected void grdRuleSet_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            Presenter.GetRuleSetsForObject();
            if(CurrentViewContext.RuleSetList==null)
            {
                lblTitle.Visible = false;
                grdRuleSet.Visible = false;
            }
            else if (CurrentViewContext.RuleSetList.Count > 0)
            {
                lblTitle.Visible = true;
                grdRuleSet.Visible = true;
                grdRuleSet.DataSource = CurrentViewContext.RuleSetList;
            }

            else
            {
                lblTitle.Visible = false;
                grdRuleSet.Visible = false;
            }
        }

        protected void grdRuleSet_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            CurrentViewContext.ViewContract.RuleSetId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["RLS_ID"]);
            if (Presenter.DeleteRuleSetObjectMapping())
            {
                bindCategories();
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                base.ShowSuccessMessage("Ruleset Deleted successfully.");
            }
            else
            {
                base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (cmbMaster.SelectedValue == String.Empty)
            //{
            CurrentViewContext.ViewContract.Name = txtName.Text.Trim();
            //CurrentViewContext.ViewContract.RuleSetTypeID = Convert.ToInt32(cmbRuleSetType.SelectedValue);
            CurrentViewContext.ViewContract.IsActive = chkActive.Checked;
            CurrentViewContext.ViewContract.Description = txtDescription.Text;
            //}
            //else
            //{
            //    CurrentViewContext.selectedRuleSetId = Convert.ToInt32(cmbMaster.SelectedValue);
            //}
            Presenter.SaveNewRuleSet();
            if (ErrorMessage == String.Empty || ErrorMessage == null)
            {
                base.ShowSuccessMessage("Ruleset Added successfully.");
            }
            else
            {
                base.ShowInfoMessage(ErrorMessage);
            }
            divAddForm.Visible = false;
            ResetControls();
            bindCategories();
            grdRuleSet.Rebind();
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
        }

        protected void btnCancel_click(object sender, EventArgs e)
        {
            divAddForm.Visible = false;
            ResetControls();
        }

        //protected void cmbMaster_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    divCreate.Visible = String.IsNullOrEmpty(cmbMaster.SelectedValue);
        //    ResetControls();
        //}

        //protected void cmbMaster_DataBound(object sender, EventArgs e)
        //{
        //    cmbMaster.Items.Insert(0, new RadComboBoxItem("Create New", string.Empty));
        //}
    }
}

