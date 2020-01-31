#region Header Comment Block

//
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  SetupRuleTemplateBkg.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using System.Collections;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity.ClientEntity;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.CommonControls.Views;
using Business.RepoManagers;
using INTSOF.Utils.Consts;
using INTERSOFT.WEB.UI.WebControls;
using System.Threading;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public partial class SetupRuleTemplateBkg : BaseUserControl, ISetupRuleTemplateBkgView
    {
        #region Private Variables

        private SetupRuleTemplateBkgPresenter _presenter = new SetupRuleTemplateBkgPresenter();
        private String _viewType;
        private Int32 _tenantId;
        private Boolean? _isAdminLoggedIn = null;
        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #region Properties

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        ISetupRuleTemplateBkgView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion

        #region Public Properties


        public SetupRuleTemplateBkgPresenter Presenter
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

        public List<lkpBkgRuleResultType> RuleResultTypes
        {
            get;
            set;
        }

        public List<BkgRuleTemplate> RuleTemplates { get; set; }

        /// <summary>
        /// Returns the current logged-in user ID.
        /// </summary>
        public Int32 currentloggedinuserId
        {


            get
            {
                return base.CurrentUserId;
            }

        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    //_tenantId = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set
            {
                _tenantId = value;
            }
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        public Int32 DefaultTenantId
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

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        public List<Tenant> ListTenants
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                if (_selectedTenantId == AppConsts.NONE)
                {
                    if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                    {
                        _selectedTenantId = Convert.ToInt32(ddlTenant.SelectedValue);
                    }
                }
                return _selectedTenantId;
            }
            set
            {
                _selectedTenantId = value;
            }
        }

        public Boolean IsAdminLoggedIn
        {
            get
            {
                if (_isAdminLoggedIn.IsNull())
                {
                    Presenter.IsAdminLoggedIn();
                }
                return _isAdminLoggedIn.Value;
            }
            set
            {
                _isAdminLoggedIn = value;
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                BindTenant();
                ApplyActionLevelPermission(new List<Entity.ClientEntity.ClsFeatureAction>(), "Rule Template");
            }
            SetDefaultSelectedTenantId();
            base.SetPageTitle("Master Rule Templates");
        }

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Master Rule Templates";
                base.OnInit(e);

                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

                if (!IsPostBack)
                {
                    Dictionary<String, String> args = new Dictionary<String, String>();

                    if (!Request.QueryString["args"].IsNull())
                    {
                        args.ToDecryptedQueryString(Request.QueryString["args"]);

                        if (args.ContainsKey("tplname"))
                        {
                            string template = Convert.ToString(args["tplname"]);

                            if (!string.IsNullOrWhiteSpace(template))
                            {
                                if (args.ContainsKey("isSaveRequest"))
                                {
                                    Boolean isSaveRequest = Convert.ToBoolean(args["isSaveRequest"]);
                                    if (isSaveRequest)
                                    {
                                        ShowSuccessMessage("Rule template " + template + " saved successfully.");
                                    }
                                    else
                                    {
                                        ShowSuccessMessage("Rule template " + template + " updated successfully.");
                                    }
                                }
                            }
                        }

                        if (args.ContainsKey("SelectedTenantId"))
                        {
                            String selectedTenant = Convert.ToString(args["SelectedTenantId"]);

                            if (!String.IsNullOrWhiteSpace(selectedTenant))
                            {
                                Int32 selectedTenantId = 0;
                                Int32.TryParse(selectedTenant, out selectedTenantId);
                                CurrentViewContext.SelectedTenantId = selectedTenantId;
                                ddlTenant.SelectedValue = selectedTenant;
                            }
                        }
                    }
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

        #region Grid Events

        /// <summary>
        /// Sets the list of filters to be displayed in the grid. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdRuleTemplates_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdRuleTemplates.FilterMenu;
            int i = 0;
            while (i < menu.Items.Count)
            {
                if (menu.Items[i].Text == GridKnownFunction.Between.ToString() || menu.Items[i].Text == GridKnownFunction.NotBetween.ToString() ||
                    menu.Items[i].Text == GridKnownFunction.NotIsEmpty.ToString() || menu.Items[i].Text == GridKnownFunction.NotIsNull.ToString())
                {
                    menu.Items.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        protected void grdRuleTemplates_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedTenantId != DefaultTenantId)
                {
                    Presenter.GetRuleTemplates();
                    grdRuleTemplates.DataSource = CurrentViewContext.RuleTemplates;
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

        protected void grdRuleTemplates_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.InitInsertCommandName)
                {

                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                          {                    
                                              {"Child", @"UserControl\RuleTemplateFormBkg.ascx"},
                                              {"SelectedTenantId", Convert.ToString(SelectedTenantId)}
                                          };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    int ruleTemplateID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BRLT_ID"]);
                    Dictionary<String, String> queryString = new Dictionary<String, String>
                                          {
                                              {"RuleTemplateID", Convert.ToString(ruleTemplateID)},
                                              {"Child", @"UserControl\RuleTemplateFormBkg.ascx"},
                                              {"SelectedTenantId", Convert.ToString(SelectedTenantId)}
                                          };
                    string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
                }
                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    int ruleTemplateID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BRLT_ID"]);
                    if (Presenter.DeleteRuleTemplate(ruleTemplateID))
                    {
                        base.ShowSuccessMessage("Rule Template deleted successfully.");
                    }
                    else
                    {
                        base.ShowErrorInfoMessage(ErrorMessage);
                    }
                }
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdRuleTemplates);
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

        protected void grdRuleTemplates_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    BkgRuleTemplate ruleTemplate = (BkgRuleTemplate)e.Item.DataItem;
                    if (e.Item is GridDataItem)
                    {
                        GridDataItem item = e.Item as GridDataItem;
                        item["BRLT_IsActive"].Text = Convert.ToBoolean(item["BRLT_IsActive"].Text) == true ? Convert.ToString("Yes") : Convert.ToString("No");
                        //Restricts the user to edit and delete Rule Template if RuleTemplate is associated with any Rule.
                        if (ruleTemplate.BkgRuleMappings.IsNotNull() && ruleTemplate.BkgRuleMappings.Count(x => x.BRLM_IsDeleted == false) > 0)
                        {
                            RadButton btnEdit = item["EditCommandColumn"].FindControl("btnEdit") as RadButton;
                            btnEdit.Text = "View";
                            btnEdit.Icon.PrimaryIconCssClass = null;
                            btnEdit.Icon.PrimaryIconUrl = "~/App_Themes/Default/images/View.png";

                            ImageButton deleteColumn = item["DeleteColumn"].Controls[0] as ImageButton;
                            deleteColumn.Visible = false;
                        }
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

        #region DropDown Events

        /// <summary>
        /// Binds the rule templates as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                grdRuleTemplates.CurrentPageIndex = 0;
                grdRuleTemplates.MasterTableView.SortExpressions.Clear();
                grdRuleTemplates.MasterTableView.FilterExpression = null;

                foreach (GridColumn column in grdRuleTemplates.MasterTableView.OwnerGrid.Columns)
                {
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;
                }
                grdRuleTemplates.Rebind();
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

        #endregion

        #region Methods

        private void BindTenant()
        {
            if (IsAdminLoggedIn == true)
            {
                Presenter.GetTenants();
                ddlTenant.DataSource = CurrentViewContext.ListTenants;
                ddlTenant.DataBind();
            }
            else
            {
                pnlTenant.Visible = false;
            }
        }

        private void SetDefaultSelectedTenantId()
        {
            if (ddlTenant.SelectedValue.IsNullOrEmpty() )
            {
                if (IsAdminLoggedIn == true && !CurrentViewContext.ListTenants.IsNullOrEmpty())
                {
                    SelectedTenantId = CurrentViewContext.ListTenants.FirstOrDefault().TenantID;
                    ddlTenant.SelectedValue = Convert.ToString(SelectedTenantId);
                }
                else
                {
                    SelectedTenantId = TenantId;
                    if (ddlTenant.DataSource.IsNotNull())
                    {
                        ddlTenant.SelectedValue = Convert.ToString(TenantId);
                    }
                }
            }
        }

        #endregion

        #region Action Permission

        public override List<ClsFeatureAction> ActionCollection
        {
            get
            {
                List<ClsFeatureAction> actionCollection = new List<ClsFeatureAction>();
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "AddRuleTemplate",
                    CustomActionLabel = "Add New Rule Template",
                    ScreenName = "Rule Template"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "EditRuleTemplate",
                    CustomActionLabel = "Edit Rule Template",
                    ScreenName = "Rule Template"
                });

                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "DeleteRuleTemplate",
                    CustomActionLabel = "Delete Rule Template",
                    ScreenName = "Rule Template"
                });
                return actionCollection;
            }
        }

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
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "AddRuleTemplate")
                                {
                                    grdRuleTemplates.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "DeleteRuleTemplate")
                                {
                                    grdRuleTemplates.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "EditRuleTemplate")
                                {
                                    grdRuleTemplates.MasterTableView.GetColumn("EditCommandColumn").Display = false;
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

