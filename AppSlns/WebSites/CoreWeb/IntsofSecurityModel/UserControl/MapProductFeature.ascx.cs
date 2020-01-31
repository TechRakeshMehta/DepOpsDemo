#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MapProductFeature.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using Telerik.Web.UI;
using INTSOF.Utils;
using Entity;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.Shell;
using System.Threading;


#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to mapping of product with features in security module.
    /// </summary>
    public partial class MapProductFeature : BaseUserControl, IMapProductFeatureView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private List<TenantProductFeature> _clientProductFeature;
        private List<Permission> _permissions;
        private MapProductFeaturePresenter _presenter = new MapProductFeaturePresenter();
        private Dictionary<String, String> _queryStringParams;
        private String _viewType;
        private MapProductFeatureContract _viewContract;

        #endregion

        #endregion

        #region Properties

        Int32 IMapProductFeatureView.OrganizationUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        /// <summary>
        /// Presenter.
        /// </summary>
        /// <value>
        /// Represents Manage Tenant Presenter.
        /// </value>

        public MapProductFeaturePresenter Presenter
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
        /// SetErrorMessage.
        /// </summary>
        /// <value>
        /// Gets or sets the value for error message.
        /// </value>
        String IMapProductFeatureView.SetErrorMessage
        {
            get
            {
                return lblErrormessage.Text.Trim();
            }
            set
            {
                lblErrormessage.ShowMessage(value, MessageType.Error);
            }
        }

        /// <summary>
        /// Blocks.
        /// </summary>
        /// <value>
        /// Gets or sets all blocks.
        /// </value>
        IQueryable<lkpSysXBlock> IMapProductFeatureView.Blocks
        {
            set
            {
                value.ForEach(item =>
                {
                    item.BusinessChannelTypeName = item.lkpBusinessChannelType.Name;
                });
                grdBlockFeature.DataSource = value;
            }
        }

        /// <summary>
        /// ProductFeatures.
        /// </summary>
        /// <value>
        /// Gets or sets all product features.
        /// </value>
        IEnumerable<TenantProductFeature> IMapProductFeatureView.ProductFeatures
        {
            get
            {
                return _clientProductFeature.AsEnumerable();
            }
            set
            {
                _clientProductFeature = value.ToList();
            }
        }

        /// <summary>
        /// Features.
        /// </summary>
        /// <value>
        /// Gets or sets all features.
        /// </value>
        IEnumerable<SysXBlocksFeature> IMapProductFeatureView.Features
        {
            get;
            set;
        }

        /// <summary>
        /// Permissions.
        /// </summary>
        /// <value>
        /// Gets or sets the value for all permissions.
        /// </value>
        List<Permission> IMapProductFeatureView.Permissions
        {
            get;
            set;
        }

        /// <summary>
        /// FeatureMappings.
        /// </summary>
        /// <value>
        /// Gets or sets all mapping of features.
        /// </value>
        IEnumerable<BlockFeaturePermissionMapper> IMapProductFeatureView.FeatureMappings
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <value>
        /// The view contract.
        /// </value>
        MapProductFeatureContract IMapProductFeatureView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new MapProductFeatureContract();
                }

                return _viewContract;
            }
        }

        Int32 WarningDisplayedCounter
        {
            get
            {
                return Convert.ToInt32((!SysXWebSiteUtils.SessionService.GetCustomData("WarningDisplayedCounter").IsNull() ? SysXWebSiteUtils.SessionService.GetCustomData("WarningDisplayedCounter") : AppConsts.NONE));
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("WarningDisplayedCounter", value);
            }
        }

        #region ClientOnBoardingWizard

        /// <summary>
        /// Gets or sets a value indicating whether this instance is data load.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is data load; otherwise, <c>false</c>.
        /// </value>
        Boolean IMapProductFeatureView.IsDataLoad
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
        Boolean IMapProductFeatureView.IsClientOnBoardingWizard
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
                return Convert.ToBoolean(!SysXWebSiteUtils.SessionService.GetCustomData("IsPageMapProductFeature").IsNull() ? SysXWebSiteUtils.SessionService.GetCustomData("IsPageMapProductFeature") : false);
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("IsPageMapProductFeature", value);
            }
        }

        /// <summary>
        /// Gets the value of Validation Group.
        /// </summary>
        String IMapProductFeatureView.ValidationGroup
        {
            get
            {
                return CommandbarbtnSaveBottom.ValidationGroup;
            }
        }

        /// <summary>
        /// SetErrorMessage.
        /// </summary>
        /// <value>
        /// Gets or sets the value for error message.
        /// </value>
        String IMapProductFeatureView.SetSuccessMessage
        {
            get;
            set;
        }

        void IMapProductFeatureView.SaveFromWizard(object sender, EventArgs e)
        {
            btnSave_Click(sender, e);
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <value>
        /// The current view context.
        /// </value>
        IMapProductFeatureView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// Override this method and set IsPolicyEnable = false to disable policy settings. - TG
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MAPPING_PRODUCT_FEATURES);
                lblMapProductFeature.Text = base.Title;
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
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
        protected void Page_Load(Object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>();

                if (!Request.QueryString["args"].IsNull())
                {
                    encryptedQueryString.ToDecryptedQueryString(Request.QueryString["args"]);
                }

                //ClientOnBoardingWiz: Implementation 14/01/2012
                //CurrentViewContext.ViewContract.ProductId = encryptedQueryString.ContainsKey("TenantProductId") ? Int32.Parse(encryptedQueryString["TenantProductId"]) : AppConsts.NONE;
                if (!CurrentViewContext.IsClientOnBoardingWizard)
                {
                    CurrentViewContext.ViewContract.ProductId = encryptedQueryString.ContainsKey("TenantProductId") ? Int32.Parse(encryptedQueryString["TenantProductId"]) : AppConsts.NONE;
                }

                //ClientOnBoardingWiz: Implementation 14/01/2012
                if (CurrentViewContext.IsClientOnBoardingWizard)
                {
                    CommandbarbtnSaveBottom.Visible = false;
                }
                //ClientOnBoardingWiz: Implementation 14/01/2012
                if (CurrentViewContext.IsDataLoad && (!IsPageLoadByClientOnBoardingWizard))
                {
                    Presenter.OnViewInitialized();
                    IsPageLoadByClientOnBoardingWizard = true;
                    grdBlockFeature.Rebind();
                }

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }

                Presenter.OnViewLoaded();
                base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_PRODUCT_FEATURE));
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

        protected void cvIsPermissionAssigned_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CurrentViewContext.SetErrorMessage = String.Empty;
            args.IsValid = !ValidatePermission();
            if (!args.IsValid)
            {
                CurrentViewContext.SetErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_MAP_PRODUCT_FEATURE);
            }
        }

        /// <summary>
        /// Save Product Feature mapping information.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void btnSave_Click(Object sender, EventArgs e)
        {
            try
            {
                cvIsPermissionAssigned.Validate();
                CurrentViewContext.SetErrorMessage = String.Empty;

                if (!Page.IsValid)
                {
                    CurrentViewContext.SetErrorMessage = SysXUtils.GetMessage(ResourceConst.SECURITY_MAP_PRODUCT_FEATURE);
                    return;
                }

                CurrentViewContext.ViewContract.UpdatedSysXBlockIDs = new List<Int32>();
                List<BlockFeaturePermissionMapper> listMapper = new List<BlockFeaturePermissionMapper>();

                foreach (GridDataItem grdItem in grdBlockFeature.Items)
                {
                    GridNestedViewItem gridNestedViewItem = grdItem.ChildItem;
                    RadTreeList radTreeList = (RadTreeList)gridNestedViewItem.FindControl("treeListFeature");

                    if (radTreeList.Items.Count > AppConsts.NONE)
                    {
                        CurrentViewContext.ViewContract.UpdatedSysXBlockIDs.Add(Convert.ToInt32(grdItem.GetDataKeyValue("SysXBlockId")));
                    }

                    foreach (TreeListDataItem treeListItem in radTreeList.Items)
                    {
                        if ((treeListItem.FindControl("chkFeature") as CheckBox).Checked)
                        {
                            BlockFeaturePermissionMapper blockFeaturePermissionMapper =
                                new BlockFeaturePermissionMapper
                                {
                                    SysXBlockBlockId =
                                        Convert.ToInt32(treeListItem["SysXBlockFeatureID"].Text)
                                };

                            List<Int32> permissions = (_permissions.Where(
                                permission =>
                                ((CheckBox)treeListItem.FindControl("chkPermission" + permission.PermissionId)).Checked)
                                .Select(permission => permission.PermissionId)).ToList();
                            blockFeaturePermissionMapper.PermissionId = permissions;
                            listMapper.Add(blockFeaturePermissionMapper);
                        }
                    }

                    CurrentViewContext.FeatureMappings = listMapper.AsEnumerable();
                }

                Presenter.CheckFeatureInRole();

                // Below condition checks if the removed permission was being used by any role associated with current product.
                if (Presenter.IsFeaturePermissionUsedByRole())
                {
                    if (WarningDisplayedCounter.Equals(AppConsts.NONE))
                    {
                        WarningDisplayedCounter++;
                        CurrentViewContext.SetErrorMessage = "You have removed some permission(s) against feature(s), which are being currently used by role(s). " +
                                                             "If you are sure of it, click on Save Mapping to save the information.";
                        return;
                    }

                    Presenter.MappingFeature();
                    WarningDisplayedCounter = AppConsts.NONE;
                }
                else
                {
                    Presenter.MappingFeature();
                    WarningDisplayedCounter = AppConsts.NONE;
                }

                if (IsPageLoadByClientOnBoardingWizard)
                {
                    grdBlockFeature.Rebind();
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

        protected void btnCancel_Click(Object sender, EventArgs e)
        {
            try
            {
                RedirectToManageTenant();
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
        /// Mapping of products with features.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MapProductFeatureInit(Object sender, EventArgs e)
        {
            try
            {
                if (!Presenter.IsNull())
                {
                    Presenter.ViewInit();
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
        /// Return Features for each Block.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdMapProductFeature_NeedDataSource(Object sender, GridNeedDataSourceEventArgs e)
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
        /// Grd map product feature item data bound.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdMapProductFeature_ItemDataBound(Object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item)))
                {
                    GridDataItem gridDataItem = e.Item as GridDataItem;
                    lkpSysXBlock sysXBlock = (lkpSysXBlock)e.Item.DataItem;
                    CurrentViewContext.ViewContract.BlockId = sysXBlock.SysXBlockId;
                    _permissions = Presenter.GetPermissionList();
                    Presenter.RetrievingProductFeature();
                    gridDataItem["Count"].Text = Convert.ToString(GetCountOfBlock(CurrentViewContext.Features.ToList(), _clientProductFeature));
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
        /// Grd map product feature item command.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdMapProductFeature_ItemCommand(Object sender, GridCommandEventArgs e)
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
                    radTreeList.DataSource = CurrentViewContext.Features.ToList();
                    radTreeList.DataBind();
                    radTreeList.ExpandAllItems();
                }
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName)
                {
                    foreach (GridFilteringItem filterItem in grdBlockFeature.MasterTableView.GetItems(GridItemType.FilteringItem))
                    {
                        filterItem.Visible = false;
                    }
                    grdBlockFeature.ExportSettings.ExportOnlyData = true;
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

        #region TreeList Events

        /// <summary>
        /// Event handler. Called by treeListFeature for initialize events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:System.EventArgs"></see> object that contains the event
        ///  data.</param>
        protected void treeListFeature_Init(object sender, EventArgs e)
        {
            Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>();

            if (!Request.QueryString["args"].IsNull())
            {
                encryptedQueryString.ToDecryptedQueryString(Request.QueryString["args"]);
            }

            Int32 iCount = AppConsts.ONE;

            try
            {
                //ClientOnBoardingWiz: Implementation 14/01/2012
                //CurrentViewContext.ViewContract.ProductId = encryptedQueryString.ContainsKey("TenantProductId") ? Int32.Parse(encryptedQueryString["TenantProductId"]) : AppConsts.NONE;
                if (!CurrentViewContext.IsClientOnBoardingWizard)
                {
                    CurrentViewContext.ViewContract.ProductId = encryptedQueryString.ContainsKey("TenantProductId") ? Int32.Parse(encryptedQueryString["TenantProductId"]) : AppConsts.NONE;
                }

                Presenter = new MapProductFeaturePresenter();
                _permissions = Presenter.GetPermissionList();

                foreach (Permission permission in _permissions)
                {
                    TreeListTemplateColumn treeListTemplateColumn = new TreeListTemplateColumn { HeaderText = permission.Name };
                    treeListTemplateColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    treeListTemplateColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    treeListTemplateColumn.ItemTemplate = new ListViewTemplate("Column1", "chkPermission" + permission.PermissionId, Request.Params, TreeListViewTemplateColumnType.CheckBox);
                    iCount++;
                    (sender as RadTreeList).Columns.Add(treeListTemplateColumn);
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
        /// Return Block Feature.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListFeature_NeedDataSource(Object sender, TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.BlockId = Convert.ToInt32((sender as RadTreeList).Attributes["BlockId"]);
                Presenter.RetrievingProductFeature();
                (sender as RadTreeList).DataSource = CurrentViewContext.Features;
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
        /// Tree list feature item data bound.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void treeListFeature_ItemDataBound(Object sender, TreeListItemDataBoundEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(TreeListItemType.AlternatingItem) || e.Item.ItemType.Equals(TreeListItemType.Item))
                {
                    TreeListDataItem item = (TreeListDataItem)e.Item;
                    SysXBlocksFeature sysXBlocksFeature = (SysXBlocksFeature)item.DataItem;
                    (e.Item.FindControl("chkFeature") as CheckBox).Attributes.Add("OnClick", "ManageChild(this); ManageParent(this);");
                    (e.Item.FindControl("chkFeature") as CheckBox).CssClass = Convert.ToString(sysXBlocksFeature.ProductFeature.ParentProductFeatureID) + "_" + Convert.ToString(sysXBlocksFeature.SysXBlockID);
                    Boolean bFlag = _clientProductFeature.Exists(
                        clientFeature => clientFeature.SysXBlockFeatureID.Equals(sysXBlocksFeature.SysXBlockFeatureID));

                    if (bFlag)
                    {
                        (e.Item.FindControl("chkFeature") as CheckBox).Checked = true;

                        TenantProductFeature currentProductFeature = _clientProductFeature.FirstOrDefault(obj => obj.SysXBlockFeatureID.Equals(sysXBlocksFeature.SysXBlockFeatureID));

                        foreach (Permission permissions in currentProductFeature.FeaturePermissions.Select(featuePermission => _permissions.Find(
                            permission => permission.PermissionId.Equals(featuePermission.PermissionID))).Where(permissions => !permissions.IsNull()))
                        {
                            //TODO:
                            //IQueryable<RoleDetail> allRolesBasedOnProductId = Presenter.GetRoleDetailsByProductId(currentProductFeature.TenantProductID);

                            //foreach (var roleInfo in allRolesBasedOnProductId)
                            //{
                            //    if (Presenter.IsPermissionUsedByRole(roleInfo.RoleDetailID, currentProductFeature.SysXBlockFeatureID, permissions.PermissionId))
                            //    {
                            //        ((CheckBox)e.Item.FindControl("chkPermission" + permissions.PermissionId)).Checked = true;
                            //        ((CheckBox)e.Item.FindControl("chkPermission" + permissions.PermissionId)).Enabled = false;
                            //    }
                            //}

                            ((CheckBox)e.Item.FindControl("chkPermission" + permissions.PermissionId)).Checked = true;
                        }
                    }

                    _queryStringParams = ConstructQueryString(Request.Params);
                    Dictionary<String, String> stateValues = null;

                    foreach (Permission permission in _permissions)
                    {
                        CheckBox chkPermission = (CheckBox)e.Item.FindControl("chkPermission" + permission.PermissionId);
                        var queryStringParameters = _queryStringParams.Where(queryParameter => queryParameter.Key.Equals(chkPermission.UniqueID)).Select(queryParameter => queryParameter.Value).ToList();

                        if (queryStringParameters.Count > AppConsts.NONE)
                        {
                            foreach (var queryStringParameter in queryStringParameters)
                            {
                                if (ViewState["SaveState"].IsNull())
                                {
                                    stateValues = new Dictionary<String, String>();
                                }
                                else
                                {
                                    stateValues = (Dictionary<String, String>)ViewState["SaveState"];
                                }

                                chkPermission.Checked = queryStringParameter.ToLower().Equals("on");

                                if (stateValues.ContainsKey(chkPermission.UniqueID))
                                {
                                    stateValues[chkPermission.UniqueID] = queryStringParameter;
                                }
                                else
                                {
                                    stateValues.Add(chkPermission.UniqueID, queryStringParameter);
                                }

                                ViewState["SaveState"] = stateValues;
                            }
                        }
                        else
                        {
                            if (!ViewState["SaveState"].IsNull())
                            {
                                stateValues = (Dictionary<String, String>)ViewState["SaveState"];
                                queryStringParameters = stateValues.Where(condition => condition.Key.Equals(chkPermission.UniqueID)).Select(condition => condition.Value).ToList();
                                if (queryStringParameters.Count > AppConsts.NONE)
                                {
                                    foreach (var queryStringParameter in queryStringParameters)
                                    {
                                        chkPermission.Checked = queryStringParameter.ToLower().Equals("on");
                                    }
                                }
                            }
                        }

                        ViewState[chkPermission.ID] = Convert.ToString(chkPermission.Checked);
                    }

                    CheckBox checkBox = (CheckBox)item.FindControl("chkFeature");
                    var check = _queryStringParams.Where(queryStringParameter => queryStringParameter.Key.Equals(checkBox.UniqueID)).Select(queryStringParameter => queryStringParameter.Value).ToList();

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

        /// <summary>
        /// Renders the given writer.
        /// </summary>
        /// <param name="writer">.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            try
            {
                base.Render(writer);
                ScriptManager.RegisterStartupScript(this,
                                                    typeof(Page),
                                                    "typeAUniqueScriptNameHere1",
                                                    "FSObject.$('.rtlCollapse').hide();", true);
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

        #region Public Methods

        /// <summary>
        /// It handles the mapping of product with features.
        /// </summary>
        public MapProductFeature()
        {
            try
            {
                Init += new EventHandler(MapProductFeatureInit);
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
        /// Redirect to Manage Tenant page.
        /// </summary>
        public void RedirectToManageTenant()
        {
            try
            {
                //ClientOnBoardingWizard: 14/01/2012
                //Response.Redirect(String.Format("Default.aspx?ucid={0}", _viewType));
                if (!CurrentViewContext.IsClientOnBoardingWizard)
                {
                    Response.Redirect(String.Format("Default.aspx?ucid={0}", _viewType), false);
                }
                else
                {
                    CurrentViewContext.SetSuccessMessage = "Feature(s) has been mapped with product successfully.";
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
        /// Build the query string.
        /// </summary>
        /// <param name="parameters"> parameter value</param>
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

        #region Private Methods

        private Boolean ValidatePermission()
        {
            Boolean validate = false;
            Boolean treeListLoop = false;

            foreach (GridDataItem item in grdBlockFeature.Items)
            {
                GridNestedViewItem gridNestedViewItem = item.ChildItem;
                RadTreeList radTreeList = (RadTreeList)gridNestedViewItem.FindControl("treeListFeature");

                foreach (TreeListDataItem treeListItem in radTreeList.Items)
                {
                    if ((treeListItem.FindControl("chkFeature") as CheckBox).Checked)
                    {
                        BlockFeaturePermissionMapper blockFeaturePermissionMapper = new BlockFeaturePermissionMapper();
                        blockFeaturePermissionMapper.SysXBlockBlockId = Convert.ToInt32(treeListItem["SysXBlockFeatureID"].Text);
                        CheckBox[] chkPermission = new CheckBox[_permissions.Count];

                        for (Int32 permissionCount = AppConsts.NONE; permissionCount < _permissions.Count; permissionCount++)
                        {
                            chkPermission[permissionCount] = (CheckBox)treeListItem.FindControl("chkPermission" + _permissions[permissionCount].PermissionId);
                        }

                        if ((from permission in chkPermission where permission.Checked.Equals(false) select permission).ToList().Count().Equals(_permissions.Count))
                        {
                            validate = true;
                            treeListLoop = true;
                            break;
                        }
                    }
                }

                if (treeListLoop)
                {
                    break;
                }
            }

            return validate;
        }

        private static Int32 GetCountOfBlock(IEnumerable<SysXBlocksFeature> sysXBlocksFeatureList, List<TenantProductFeature> tenantProductFeatureList)
        {
            return sysXBlocksFeatureList.Select(sysXBlocksFeature => tenantProductFeatureList.Exists(
                clientFeature => clientFeature.SysXBlockFeatureID.Equals(sysXBlocksFeature.SysXBlockFeatureID))).Count(bFlag => bFlag);
        }

        #endregion

        #region protected Methods

        /// <summary>
        /// Set Parent for each Feature.
        /// </summary>
        /// <param name="name">Set the value for name.</param>
        /// <param name="parentProductFeatureId">value for parentProductFeature's id.</param>
        protected String SetParent(Object parentProductFeatureId, Object name)
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
}

/// <summary>
/// List view template.
/// </summary>
public class ListViewTemplate : ITemplate
{
    #region Variables

    #region Public Variables

    #endregion

    #region Private Variables

    private String _columnName;
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
    /// <param name="colname">column name value</param>
    /// <param name="controlId">Control Id value</param>
    /// <param name="requestParams">requested parameter value</param>
    /// <param name="columnType">Column name value</param>
    public ListViewTemplate(String colname, String controlId, NameValueCollection requestParams, TreeListViewTemplateColumnType columnType)
    {
        //Stores the column name.
        _columnName = colname;
        _controlId = controlId;
        _requestParams = requestParams;
        _columnType = columnType;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Handles InstantiateIn.
    /// </summary>
    /// <param name="container">set the value for container.</param>
    void ITemplate.InstantiateIn(Control container)
    {
        switch (_columnType)
        {
            case TreeListViewTemplateColumnType.CheckBox:
                CheckBox checkBox = new CheckBox { ID = _controlId };
                container.Controls.Add(checkBox); //Adds the newly created textbox to the container.
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
                container.Controls.Add(radioButton);  //Adds the newly created textbox to the container.
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