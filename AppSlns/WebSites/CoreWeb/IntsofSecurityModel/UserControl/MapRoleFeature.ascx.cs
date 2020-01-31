#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MapRoleFeature.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using CoreWeb.Shell;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.IntsofSecurityModel.Providers;
using System.Threading;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.SysXSecurityModel;
using System.Web.UI.HtmlControls;


#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to mapping of roles with features in security module.
    /// </summary>
    public partial class MapRoleFeature : BaseUserControl, IMapRoleFeatureView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private List<Permission> _permissions;
        private MapRoleFeaturePresenter _presenter = new MapRoleFeaturePresenter();
        private Dictionary<String, String> _queryStringParams;
        private List<RolePermissionProductFeature> _roleFeatures;
        private String _viewType;
        private MapRoleFeatureContract _viewContract;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Sets a message describing the set error.
        /// </summary>
        /// <value>
        /// A message describing the set error.
        /// </value>
        public String SetErrorMessage
        {
            set
            {
                lblErrormessage.ShowMessage(value, MessageType.Error);
            }
        }

        #region ClientOnBoardingWizard

        public event EventHandler<EventArgs> SaveMappingClick;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is data load.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is data load; otherwise, <c>false</c>.
        /// </value>
        Boolean IMapRoleFeatureView.IsDataLoad
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is client on boarding wizard.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is client on boarding wizard; otherwise, <c>false</c>.
        /// </value>
        Boolean IMapRoleFeatureView.IsClientOnBoardingWizard
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is page load by client on boarding wizard.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is page load by client on boarding wizard; otherwise, <c>false</c>.
        /// </value>
        Boolean IsPageLoadByClientOnBoardingWizard
        {
            get
            {
                return Convert.ToBoolean(!SysXWebSiteUtils.SessionService.GetCustomData("IsPageMapRoleFeature").IsNull() ? SysXWebSiteUtils.SessionService.GetCustomData("IsPageMapRoleFeature") : false);
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("IsPageMapRoleFeature", value);
            }
        }

        /// <summary>
        /// Gets the value of Validation Group.
        /// </summary>
        String IMapRoleFeatureView.ValidationGroup
        {
            get
            {
                return CommandbarbtnSaveBottom.ValidationGroup;
            }
        }

        #endregion

        /// <summary>
        /// Permissions</summary>
        /// <value>
        /// Gets or sets the list of all permissions.</value>
        public List<Permission> Permissions
        {
            get;
            set;
        }

        /// <summary>
        /// Presenter</summary>
        /// <value>
        /// Represents Manage Tenant Presenter.</value>

        public MapRoleFeaturePresenter Presenter
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

        /// <summary>
        /// ProductID</summary>
        /// <value>
        /// Gets or sets the value for product's id.</value>
        Int32 IMapRoleFeatureView.ProductId
        {
            get
            {
                return Convert.ToInt32(ViewState["ProdcutID"]);
            }
            set
            {
                ViewState["ProdcutID"] = value;
            }
        }

        /// <summary>
        /// RoleFeatures</summary>
        /// <value>
        /// Gets or sets the list of all role features.</value>
        List<RolePermissionProductFeature> IMapRoleFeatureView.RoleFeatures
        {
            get
            {
                return _roleFeatures;
            }
            set
            {
                _roleFeatures = value;
            }
        }

        /// <summary>
        /// ProductFeatures.
        /// </summary>
        /// <value>
        /// Gets or sets the list of all product features.
        /// </value>
        List<TenantProductFeature> IMapRoleFeatureView.ProductFeatures
        {
            set;
            get;
        }

        /// <summary>
        /// Blocks.
        /// </summary>
        /// <value>
        /// Gets or sets the list of all blocks.
        /// </value>
        IQueryable<lkpSysXBlock> IMapRoleFeatureView.Blocks
        {
            set
            {
                value.ForEach(item =>
                {
                    item.BusinessChannelTypeName = item.lkpBusinessChannelType.Name;
                });
                grdMapRoleFeature.DataSource = value;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <value>
        /// The view contract.
        /// </value>
        MapRoleFeatureContract IMapRoleFeatureView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new MapRoleFeatureContract();
                }

                return _viewContract;
            }
        }

        //UAT-3228
        /// <summary>
        /// Current User Id.
        /// </summary>
        Int32 IMapRoleFeatureView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IMapRoleFeatureView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        private List<BreadCrumbNode> ReturnPath
        {
            get
            {
                if (Session["BreadCrumb"].IsNull())
                {
                    return new List<BreadCrumbNode>();
                }

                return (Session["BreadCrumb"] as List<BreadCrumbNode>);
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// Raises the initialize event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

                base.OnInit(e);
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MAPPING_ROLES_FEATURES);
                //lblMapRoleFeature.Text = base.Title;

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


        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>();

                if (!Request.QueryString["args"].IsNull())
                {
                    encryptedQueryString.ToDecryptedQueryString(Request.QueryString["args"]);
                }

                //ClientOnBoardingWiz: Implementation 14/01/2012
                //CurrentViewContext.ViewContract.RoleId = encryptedQueryString.ContainsKey("RoleDetailId") ? encryptedQueryString["RoleDetailId"] : String.Empty;
                //CurrentViewContext.ProductId = encryptedQueryString.ContainsKey("ProductID") ? Convert.ToInt32(encryptedQueryString["ProductID"]) : AppConsts.NONE;
                if (!CurrentViewContext.IsClientOnBoardingWizard)
                {
                    CurrentViewContext.ViewContract.RoleId = encryptedQueryString.ContainsKey("RoleDetailId") ? encryptedQueryString["RoleDetailId"] : String.Empty;
                    CurrentViewContext.ProductId = encryptedQueryString.ContainsKey("ProductID") ? Convert.ToInt32(encryptedQueryString["ProductID"]) : AppConsts.NONE;
                }

                //ClientOnBoardingWiz: Implementation 14/01/2012
                if (CurrentViewContext.IsDataLoad && (!IsPageLoadByClientOnBoardingWizard))
                {
                    grdMapRoleFeature.Rebind();
                    IsPageLoadByClientOnBoardingWizard = true;
                }

                if (CurrentViewContext.ProductId.Equals(AppConsts.NONE))
                {
                    throw new System.Exception(SysXUtils.GetMessage(ResourceConst.SECURITY_PRODUCT_NOT_ASSIGN));
                }

                if (!this.IsPostBack)
                {
                    Session["RoleFeatureActions"] = null;
                    Presenter.OnViewInitialized();
                }
                //lblMapRoleFeature.Text = Presenter.GetTanantName();
                //if (!string.IsNullOrWhiteSpace(lblMapRoleFeature.Text))
                //{
                String roleName = encryptedQueryString.ContainsKey("RoleDetailDescription") ? encryptedQueryString["RoleDetailDescription"] : String.Empty;
                lblMapRoleFeature.Text = roleName + "&nbsp;>&nbsp;" + base.Title;
                //}
                Presenter.OnViewLoaded();
                base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_ROLE_FEATURE));
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

        /// <summary>
        /// Performs save operation for the relations between roles with features.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SetErrorMessage = String.Empty;

            if (ValidatePermission())
            {
                SetErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_PERMISSION_FEATURE);
                return;
            }
            try
            {
                CurrentViewContext.ViewContract.UpdatedSysXBlockIDs = new List<Int32>();
                Dictionary<Int32, Int32> featurePermissions = new Dictionary<Int32, Int32>();

                foreach (GridDataItem item in grdMapRoleFeature.Items)
                {
                    GridNestedViewItem gridNestedViewItem = item.ChildItem;
                    RadTreeList radTreeList = (RadTreeList)gridNestedViewItem.FindControl("treeListFeature");

                    if (radTreeList.Items.Count > AppConsts.NONE)
                    {
                        CurrentViewContext.ViewContract.UpdatedSysXBlockIDs.Add(Convert.ToInt32(item.GetDataKeyValue("SysXBlockId")));

                        foreach (TreeListDataItem treeListItem in radTreeList.Items)
                        {
                            if ((treeListItem.FindControl("chkFeature") as CheckBox).Checked)
                            {
                                Int32 featureId = Convert.ToInt32(treeListItem["SysXBlockFeatureID"].Text);

                                foreach (Permission permission in _permissions.Where(permission => ((RadioButton)treeListItem.FindControl("chkPermission" + permission.PermissionId)).Checked))
                                {
                                    featurePermissions.Add(featureId, permission.PermissionId);
                                    break;
                                }
                            }
                        }
                    }

                }

                List<RoleFeatureActionContract> roleFeatureActions = null;
                if (Session["RoleFeatureActions"].IsNotNull())
                {
                    roleFeatureActions = Session["RoleFeatureActions"] as List<RoleFeatureActionContract>;
                }
                CurrentViewContext.ViewContract.FeaturePermissions = featurePermissions;
                Presenter.RoleFeatureMapping(roleFeatureActions);

                //ClientOnBoarding: Step back command
                if (CurrentViewContext.IsClientOnBoardingWizard)
                {
                    SaveMappingClick.Invoke(sender, e);
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

            //          Presenter.RoleFeatureMapping();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                RedirectToManageRole();
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
        /// <summary>
        /// Renders the given writer.
        /// </summary>
        /// <param name="writer">writer value</param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            ScriptManager.RegisterStartupScript(this,
                                                typeof(Page),
                                                "typeAUniqueScriptNameHere1",
                                                "FSObject.$('.rtlCollapse').hide();", true);
        }

        #endregion

        #region Grid Events

        /// <summary>
        /// Event handler. Called by grdBlockFeature for need data source events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdBlockFeature_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.RetrievingBlocks();
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

        /// <summary>
        /// Event handler. Called by grdBlockFeature for item command events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdBlockFeature_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem gridDataItem = (GridDataItem)e.Item;
                    CurrentViewContext.ViewContract.BlockId = Convert.ToInt32(gridDataItem.GetDataKeyValue("SysXBlockId"));
                    GridNestedViewItem gridNestedViewItem = gridDataItem.ChildItem;
                    RadTreeList radTreeList = (RadTreeList)gridNestedViewItem.FindControl("treeListFeature");
                    radTreeList.Attributes.Add("BlockId", Convert.ToString(CurrentViewContext.ViewContract.BlockId));
                    Presenter.RetrievingProductFeature();
                    radTreeList.DataSource = CurrentViewContext.ProductFeatures;
                    radTreeList.DataBind();
                    radTreeList.ExpandAllItems();
                }
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName)
                {
                    foreach (GridFilteringItem filterItem in grdMapRoleFeature.MasterTableView.GetItems(GridItemType.FilteringItem))
                    {
                        filterItem.Visible = false;
                    }
                    grdMapRoleFeature.ExportSettings.ExportOnlyData = true;
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

        /// <summary>
        /// Event handler. Called by grdBlockFeature for item data bound events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdBlockFeature_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (!e.Item.ItemType.Equals(GridItemType.AlternatingItem) && !e.Item.ItemType.Equals(GridItemType.Item))
            {
                return;
            }

            GridDataItem gridDataItem = e.Item as GridDataItem;
            lkpSysXBlock sysXBlock = (lkpSysXBlock)e.Item.DataItem;
            CurrentViewContext.ViewContract.BlockId = sysXBlock.SysXBlockId;
            Presenter.RetrievingBlockFeatureCount();
            gridDataItem["Count"].Text = Convert.ToString(CurrentViewContext.ViewContract.FeatureCount);
        }

        #endregion

        #region TreeList Events

        /// <summary>
        /// Event handler. Called by treeListFeature for initialize events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:System.EventArgs"></see> object that contains the event
        ///  data.</param>
        protected void treeListFeature_Init(object sender, EventArgs e)
        {
            Int32 iCount = AppConsts.ONE;

            try
            {
                Presenter = new MapRoleFeaturePresenter();
                _permissions = Presenter.GetPermissionList();

                foreach (Permission permission in _permissions)
                {
                    TreeListTemplateColumn treeListTemplateColumn = new TreeListTemplateColumn { HeaderText = permission.Name };
                    treeListTemplateColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    treeListTemplateColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

                    treeListTemplateColumn.ItemTemplate = new GridViewTemplate("Column1", "chkPermission" + permission.PermissionId, Request.Params, TreeListViewTemplateColumnType.RadioButton);
                    iCount++;
                    ((RadTreeList)(sender)).Columns.Add(treeListTemplateColumn);
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

        /// <summary>
        /// Retrieves the information for mapping between roles and features.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListFeature_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                RadTreeList treeList = (RadTreeList)sender;
                GridNestedViewItem item = (GridNestedViewItem)treeList.NamingContainer;
                CurrentViewContext.ViewContract.BlockId = Convert.ToInt32(item.ParentItem.GetDataKeyValue("SysXBlockId"));

                if (!CurrentViewContext.ProductId.Equals(AppConsts.NONE))
                {
                    Presenter.RetrievingProductFeature();
                    (sender as RadTreeList).DataSource = CurrentViewContext.ProductFeatures;
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

        /// <summary>
        /// Event handler. Called by treeListFeature for item data bound events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void treeListFeature_ItemDataBound(object sender, TreeListItemDataBoundEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(TreeListItemType.AlternatingItem) || e.Item.ItemType.Equals(TreeListItemType.Item) || e.Item.ItemType.Equals(TreeListItemType.SelectedItem))
                {
                    TreeListDataItem item = (TreeListDataItem)e.Item;
                    TenantProductFeature tenantProductFeature = (TenantProductFeature)item.DataItem;
                    RadButton btnFeatureActionList = (e.Item.FindControl("btnFeatureActionList") as RadButton);
                    (e.Item.FindControl("chkFeature") as CheckBox).Attributes.Add("OnClick", "ManageChild(this); ManageParent(this);");
                    (e.Item.FindControl("chkFeature") as CheckBox).CssClass = Convert.ToString(tenantProductFeature.SysXBlocksFeature.ProductFeature.ParentProductFeatureID) + "_" + Convert.ToString(tenantProductFeature.SysXBlocksFeature.SysXBlockID);

                    foreach (Permission permExist in _permissions)
                    {
                        ((RadioButton)e.Item.FindControl("chkPermission" + permExist.PermissionId)).Visible = false;
                    }

                    foreach (FeaturePermission permission in tenantProductFeature.FeaturePermissions)
                    {
                        ((RadioButton)e.Item.FindControl("chkPermission" + permission.PermissionID)).Visible = true;
                    }

                    //if (tenantProductFeature.SysXBlocksFeature.ProductFeature.UIControlID.IsNullOrEmpty())
                    //{
                    //    btnFeatureActionList.Style.Add("display","none");
                    //}

                    Boolean bFlag = _roleFeatures.Exists(
                        roleFeature =>
                        roleFeature.SysXBlocksFeature.ProductFeature.ProductFeatureID.Equals(tenantProductFeature.SysXBlocksFeature.ProductFeature.ProductFeatureID));

                    if (bFlag)
                    {
                        (e.Item.FindControl("chkFeature") as CheckBox).Checked = true;

                        RolePermissionProductFeature currentNode = _roleFeatures.Find(
                            roleFeature =>
                            roleFeature.SysXBlocksFeature.ProductFeature.ProductFeatureID.Equals(tenantProductFeature.SysXBlocksFeature.ProductFeature.ProductFeatureID));

                        List<RoleFeatureActionContract> roleFeatureActions;
                        if (Session["RoleFeatureActions"].IsNotNull())
                        {
                            roleFeatureActions = Session["RoleFeatureActions"] as List<RoleFeatureActionContract>;
                        }
                        else
                        {
                            Session["RoleFeatureActions"] = new List<RoleFeatureActionContract>();
                            roleFeatureActions = Session["RoleFeatureActions"] as List<RoleFeatureActionContract>;
                        }

                        if (!tenantProductFeature.SysXBlocksFeature.ProductFeature.UIControlID.IsNullOrEmpty())
                        {
                            btnFeatureActionList.Style.Add("display", "block");
                        }
                        if (currentNode.FeatureRoleActions.IsNotNull() && currentNode.FeatureRoleActions.Count > AppConsts.NONE)
                        {
                            currentNode.FeatureRoleActions.ForEach(featureRoleAction =>
                                {
                                    if (!roleFeatureActions.Any(cond => cond.FeatureActionID == featureRoleAction.FeatureActionID && cond.SysXBlockFeatureID == currentNode.SysXBlockFeatureId.Value))
                                    {
                                        roleFeatureActions.Add(new RoleFeatureActionContract
                                        {
                                            FeatureActionID = featureRoleAction.FeatureActionID.Value,
                                            PermissionID = featureRoleAction.PermissionID.Value,
                                            SysXBlockFeatureID = currentNode.SysXBlockFeatureId.Value
                                        });
                                    }
                                });
                        }

                        Permission permissions = _permissions.Find(
                            permission => permission.PermissionId.Equals(currentNode.PermissionId));

                        if (!permissions.IsNull())
                        {
                            ((RadioButton)e.Item.FindControl("chkPermission" + permissions.PermissionId)).Checked = true;
                        }
                    }

                    _queryStringParams = ConstructQueryString(Request.Params);
                    Dictionary<String, String> stateValues = null;
                    String radioGroupId = String.Empty;

                    foreach (Permission permission in _permissions)
                    {
                        RadioButton rbPermission = (RadioButton)e.Item.FindControl("chkPermission" + permission.PermissionId);
                        radioGroupId = rbPermission.ClientID.Replace(rbPermission.ID, rbPermission.GroupName);
                        break;
                    }

                    radioGroupId = radioGroupId.Replace("_", "$");
                    var queryStringParameters = _queryStringParams.Where(queryStringParams => queryStringParams.Key.Equals(radioGroupId)).Select(queryStringParams => queryStringParams.Value).ToList();

                    if (queryStringParameters.Count > AppConsts.NONE)
                    {
                        foreach (var val in queryStringParameters)
                        {
                            if (ViewState["SaveState"].IsNull())
                            {
                                stateValues = new Dictionary<String, String>();
                            }
                            else
                            {
                                stateValues = (Dictionary<String, String>)ViewState["SaveState"];
                            }

                            ((RadioButton)e.Item.FindControl(val)).Checked = true;

                            if (stateValues.ContainsKey(radioGroupId))
                            {
                                stateValues[radioGroupId] = val;
                            }
                            else
                            {
                                stateValues.Add(radioGroupId, val);
                            }

                            ViewState["SaveState"] = stateValues;
                        }
                    }
                    else
                    {
                        if (!ViewState["SaveState"].IsNull())
                        {
                            stateValues = (Dictionary<String, String>)ViewState["SaveState"];
                            queryStringParameters = stateValues.Where(stateValue => stateValue.Key.Equals(radioGroupId)).Select(stateValue => stateValue.Value).ToList();

                            if (queryStringParameters.Count > AppConsts.NONE)
                            {
                                foreach (var val in queryStringParameters)
                                {
                                    ((RadioButton)e.Item.FindControl(val)).Checked = true;
                                }
                            }

                        }

                    }

                    CheckBox checkBox = (CheckBox)item.FindControl("chkFeature");
                    var check = _queryStringParams.Where(queryStringParams => queryStringParams.Key.Equals(checkBox.UniqueID)).Select(queryStringParams => queryStringParams.Value).ToList();

                    if (check.Count > AppConsts.NONE)
                    {
                        foreach (var val in check)
                        {
                            checkBox.Checked = val.ToLower().Equals("on");

                            if (stateValues.IsNull())
                            {
                                stateValues = new Dictionary<String, String>();
                            }

                            if (stateValues.ContainsKey(checkBox.UniqueID))
                            {
                                stateValues[checkBox.UniqueID] = val;
                            }
                            else
                            {
                                stateValues.Add(checkBox.UniqueID, val);
                            }

                            ViewState["SaveState"] = stateValues;
                        }
                    }
                    else
                    {
                        if (!ViewState["SaveState"].IsNull())
                        {
                            stateValues = (Dictionary<String, String>)ViewState["SaveState"];
                            check = stateValues.Where(stateValue => stateValue.Key.Equals(checkBox.UniqueID)).Select(stateValue => stateValue.Value).ToList();

                            if (check.Count > AppConsts.NONE)
                            {
                                foreach (var val in check)
                                {
                                    checkBox.Checked = val.ToLower().Equals("on");
                                }
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


        protected void treeListFeature_ItemCommand(object sender, TreeListCommandEventArgs e)
        {
            try
            {
                //if (e.CommandName.Equals("ManageAction"))
                //{
                //    var item = (e.Item as TreeListDataItem);
                //    hdnProductFeatureID.Value = item.GetDataKeyValue("SysXBlocksFeature.ProductFeature.ProductFeatureID").ToString();
                //    hdnSysyXBlockFeatureID.Value = item["SysXBlockFeatureID"].Text;
                //    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ManageAction", "openPopUp();", true);
                //}
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

        #region public Methods

        /// <summary>
        /// Redirect to Manage Role page.
        /// </summary>
        public void RedirectToManageRole()
        {
            try
            {
                //ClientOnBoardingWizard: 14/01/2012
                if (!CurrentViewContext.IsClientOnBoardingWizard)
                {
                    Session["RoleFeatureActions"] = null;
                    if (ReturnPath.Count > AppConsts.NONE)
                    {
                        BreadCrumbNode node = ReturnPath.Where(condition => (condition.Level.Equals((ReturnPath.Count - AppConsts.ONE)))).FirstOrDefault();
                        Response.Redirect(!node.IsNull() ? node.NodeURL : String.Format("Default.aspx?ucid={0}", _viewType), false);
                    }
                    else
                    {
                        Response.Redirect(String.Format("Default.aspx?ucid={0}", _viewType), false);
                    }
                }
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
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

        /// <summary>
        /// Constructs the query string.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public Dictionary<String, String> ConstructQueryString(NameValueCollection parameters)
        {
            try
            {
                return parameters.Cast<String>().ToDictionary(name => name, name => parameters[name]);
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

            return null;
        }

        #endregion

        #region Private Method

        private Boolean ValidatePermission()
        {
            Boolean validate = false;
            Boolean treeListLoop = false;
            foreach (GridDataItem item in grdMapRoleFeature.Items)
            {
                GridNestedViewItem gridNestedViewItem = (GridNestedViewItem)item.ChildItem;
                RadTreeList radTreeList = (RadTreeList)gridNestedViewItem.FindControl("treeListFeature");
                foreach (TreeListDataItem treeListItem in radTreeList.Items)
                {
                    if ((treeListItem.FindControl("chkFeature") as CheckBox).Checked)
                    {
                        BlockFeaturePermissionMapper blockFeaturePermissionMapper = new BlockFeaturePermissionMapper();
                        blockFeaturePermissionMapper.SysXBlockBlockId = Convert.ToInt32(treeListItem["SysXBlockFeatureID"].Text);
                        CheckBox[] chkPermission = new CheckBox[_permissions.Count];

                        for (Int32 counter = AppConsts.NONE; counter < _permissions.Count; counter++)
                        {
                            chkPermission[counter] = (CheckBox)treeListItem.FindControl("chkPermission" + _permissions[counter].PermissionId);
                        }

                        if ((chkPermission.Where(permission => permission.Checked.Equals(false))).Count().Equals(_permissions.Count))
                        {
                            validate = true;
                            treeListLoop = true;
                            break;
                        }
                    }

                }
                if (treeListLoop)
                {
                    validate = true;
                    break;
                }

            }
            return validate;
        }

        #endregion

        #region Protected Method

        /// <summary>
        /// Set parent feature.
        /// </summary>
        /// <param name="name">value for name.</param>
        /// <param name="parentProductFeatureId">value for parentProductFeature's id.</param>
        protected String SetParent(object parentProductFeatureId, object name)
        {
            try
            {
                return Convert.ToString(parentProductFeatureId) + "_" + Convert.ToString(name);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
                return String.Empty;
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
                return String.Empty;
            }
        }

        #endregion


        #endregion
    }

    /// <summary>
    /// Grid view template.
    /// </summary>
    public class GridViewTemplate : ITemplate
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private TreeListViewTemplateColumnType _columnType;
        private String _controlId;
        private NameValueCollection _requestParams;

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Constructor where we define the template type and column name.
        /// </summary>
        /// <param name="colname">      Value for column name.</param>
        /// <param name="controlId">    Value for control's id.</param>
        /// <param name="requestParams">Value for requestParams.</param>
        /// <param name="columnType">   Value for column type.</param>
        public GridViewTemplate(String colname, String controlId, NameValueCollection requestParams, TreeListViewTemplateColumnType columnType)
        {
            _controlId = controlId;
            _requestParams = requestParams;
            _columnType = columnType;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handles InstantiateIn.
        /// </summary>
        /// <param name="container">container value</param>
        void ITemplate.InstantiateIn(Control container)
        {
            switch (_columnType)
            {
                case TreeListViewTemplateColumnType.CheckBox:
                    CheckBox checkBox = new CheckBox { ID = _controlId };
                    container.Controls.Add(checkBox);
                    String clientId = Convert.ToString(_requestParams[checkBox.ClientID]);

                    if (String.IsNullOrEmpty(clientId))
                    {
                        clientId = Convert.ToString(_requestParams[checkBox.ClientID.Replace("_", "$")]);
                        if (!String.IsNullOrEmpty(clientId))
                        {
                            checkBox.Checked = true;
                        }
                    }
                    else
                    {
                        checkBox.Checked = true;
                    }
                    break;

                case TreeListViewTemplateColumnType.RadioButton:
                    RadioButton radioButton = new RadioButton { ID = _controlId, GroupName = "PermissionGroup" };
                    container.Controls.Add(radioButton);
                    String rdPostValue = Convert.ToString(_requestParams[radioButton.ClientID]);

                    if (String.IsNullOrEmpty(rdPostValue))
                    {
                        rdPostValue = Convert.ToString(_requestParams[radioButton.ClientID.Replace("_", "$")]);

                        if (!String.IsNullOrEmpty(rdPostValue))
                        {
                            radioButton.Checked = true;
                        }
                    }
                    else
                    {
                        radioButton.Checked = true;
                    }
                    break;
            }
        }

        #endregion

        #endregion
    }
}