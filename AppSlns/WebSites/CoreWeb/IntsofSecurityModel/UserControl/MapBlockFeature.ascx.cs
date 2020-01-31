#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MapBlockFeature.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using System.Threading;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to mapping of block with features in security module.
    /// </summary>
    public partial class MapBlockFeature : BaseUserControl, IMapBlockFeatureView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private MapBlockFeaturePresenter _presenter = new MapBlockFeaturePresenter();
        private String _viewType;
        private MapBlockFeatureContract _viewContract;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Presenter.
        /// </summary>
        /// <value>
        /// Represents Manage Tenant Presenter.
        /// </value>

        public MapBlockFeaturePresenter Presenter
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
        /// SysXBlocksFeatures.
        /// </summary>
        /// <value>
        /// Gets or sets the value for SysXBlocksFeatures.
        /// </value>
        IEnumerable<SysXBlocksFeature> IMapBlockFeatureView.SysXBlocksFeatures
        {
            get;
            set;
        }

        /// <summary>
        /// ProductFeatures.
        /// </summary>
        /// <value>
        /// Gets or sets all ProductFeatures.
        /// </value>
        IQueryable<ProductFeature> IMapBlockFeatureView.ProductFeatures
        {
            set
            {
                treeListFeature.DataSource = value;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <value>
        /// The view contract.
        /// </value>
        MapBlockFeatureContract IMapBlockFeatureView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new MapBlockFeatureContract();
                }

                return _viewContract;
            }
        }

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <value>
        /// The current view context.
        /// </value>
        IMapBlockFeatureView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets the current user id.
        /// </summary>
        /// <remarks></remarks>
        Int32 IMapBlockFeatureView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
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
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MAPPING_LINE_OF_BUSINESS_FEATURES);
                lblMapLineOfBusiness.Text = base.Title;
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

                CurrentViewContext.ViewContract.SysXBloxkId = encryptedQueryString.ContainsKey("SysXBlockID") ? Int32.Parse(encryptedQueryString["SysXBlockID"]) : AppConsts.NONE;
                CurrentViewContext.BusinessChannelTypeID = encryptedQueryString.ContainsKey("BusinessChannelTypeID") ? Int16.Parse(encryptedQueryString["BusinessChannelTypeID"]) : (short)AppConsts.NONE;

                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }

                Presenter.OnViewLoaded();
                base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_BLOCK_FEATURE));
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
        /// Save Map Block Feature information.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.Features = (from treeListItem in treeListFeature.Items where (treeListItem.FindControl("chkFeature") as CheckBox).Checked select Convert.ToInt32(treeListItem.GetDataKeyValue("ProductFeatureID"))).ToList();
                Presenter.MappingBlockFeature();
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                RedirectToManageBlock();
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
        /// Retrieve Block's Feature.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListFeature_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.RetrievingProductFeatures();
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
        /// No Metadata Documentation available. 
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListFeature_ItemDataBound(object sender, TreeListItemDataBoundEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(TreeListItemType.Item) || e.Item.ItemType.Equals(TreeListItemType.AlternatingItem))
                {
                    Int32 productFeatureId = Convert.ToInt16((e.Item as TreeListDataItem).GetDataKeyValue("ProductFeatureID"));

                    if (CurrentViewContext.SysXBlocksFeatures.ToList().Exists(a => a.FeatureID.Equals(productFeatureId)))
                    {
                        (e.Item.FindControl("chkFeature") as CheckBox).Checked = true;
                    }

                    TreeListDataItem item = (TreeListDataItem)e.Item;
                    ProductFeature productFeature = (ProductFeature)item.DataItem;
                    (e.Item.FindControl("chkFeature") as CheckBox).Attributes.Add("OnClick", "ManageChild(this); ManageParent(this);");
                    (e.Item.FindControl("chkFeature") as CheckBox).CssClass = Convert.ToString(productFeature.ParentProductFeatureID);
                    (e.Item.FindControl("chkFeature") as CheckBox).InputAttributes.Add("alt", Convert.ToString(productFeature.ProductFeatureID));
                    (e.Item.FindControl("chkFeature") as CheckBox).InputAttributes.Add("parent", Convert.ToString(productFeature.ParentProductFeatureID));
                    (e.Item as TreeListDataItem)["UIControlID"].Text = FormatControlName((e.Item as TreeListDataItem)["UIControlID"].Text);
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
        /// No Metadata Documentation available. 
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void treeListFeature_PreRender(object sender, EventArgs e)
        {
            try
            {
                treeListFeature.ExpandAllItems();
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
        /// No Metadata Documentation available. 
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void treeListFeature_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        {
            // TODO: Below code will hide the expand button.
            //if (e.Item is TreeListDataItem)
            //{
            //    Control expandButton = e.Item.FindControl("ExpandCollapseButton");
            //    if (expandButton != null)
            //    {
            //        expandButton.Visible = false;
            //    }
            //}

            // Below code disables the expand button.
            if (e.Item is TreeListDataItem)
            {
                Control expandButton = e.Item.FindControl("ExpandCollapseButton");
                if (expandButton != null)
                {
                    ((Button)expandButton).Enabled = false;
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        #endregion

        #region Public Methods

        /// <summary>
        /// Redirect to Manage Block Page.
        /// </summary>
        public void RedirectToManageBlock()
        {
            try
            {
                Response.Redirect(String.Format("Default.aspx?ucid={0}", _viewType));
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

        #endregion

        #endregion


        Int16 IMapBlockFeatureView.BusinessChannelTypeID
        {
            get;
            set;
        }
    }
}