#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageFeature.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;
using System.Linq;
using System.Web;
#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTERSOFT.WEB.UI.WebControls;
using Entity.ClientEntity;
using System.Web.UI.HtmlControls;
#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to managing features in security module.
    /// </summary>
    public partial class ManageFeature : BaseUserControl, IManageFeatureView
    {
        #region Internal Classes

        /// <summary>
        /// Has the methods declaration for Site data item
        /// </summary>
        internal class SiteDataItem
        {
            #region Properties

            /// <summary>
            /// Gets or sets the value for Text.
            /// </summary>
            /// <value>
            /// The text.
            /// </value>
            public String Text
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the value for Id.
            /// </summary>
            /// <value>
            /// The identifier.
            /// </value>
            public Int32 Id
            {

                get;
                set;
            }

            /// <summary>
            /// Gets or sets the value for Parent's id.
            /// </summary>
            /// <value>
            /// The identifier of the parent.
            /// </value>
            public Int32 ParentId
            {
                get;
                set;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Initializes a new instance of the SiteDataItem class.
            /// </summary>
            /// <param name="id">      The identifier.</param>
            /// <param name="parentId">Identifier for the parent.</param>
            /// <param name="text">    The text.</param>
            public SiteDataItem(Int32 id, Int32 parentId, String text)
            {
                Id = id;
                ParentId = parentId;
                Text = text;
            }

            #endregion
        }

        #endregion

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ManageFeaturePresenter _presenter = new ManageFeaturePresenter();
        private ManageFeatureContract _viewContract;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Presenter.
        /// </summary>
        /// <value>
        /// Represents Manage Tenant Presenter.
        /// </value>

        public ManageFeaturePresenter Presenter
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
        /// ErrorMessage.
        /// </summary>
        /// <value>
        /// Gets or sets the value for error message.
        /// </value>
        String IManageFeatureView.ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// CurrentUserID.
        /// </summary>
        /// <value>
        /// Gets the value for current user's id.
        /// </value>
        Int32 IManageFeatureView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// ProductFeatures.
        /// </summary>
        /// <value>
        /// Sets all product's feature.
        /// </value>
        IEnumerable<ProductFeature> IManageFeatureView.ProductFeatures
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
        ManageFeatureContract IManageFeatureView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManageFeatureContract();
                }

                return _viewContract;
            }
        }

        /// <summary>
        /// SuccessMessage</summary>
        /// <value>
        /// Gets or sets the value for Success message.</value>
        String IManageFeatureView.SuccessMessage
        {
            get;
            set;
        }

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IManageFeatureView CurrentViewContext
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

        ///<summary>
        ///Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// Calls WebClientApplication.BuildItemWithCurrentContext after the events are handled
        /// to fire the DI engine.
        ///</summary>
        ///<param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_FEATURES);
                lblManageFeature.Text = base.Title;
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }
                //Get page size from Session 
                if (Session["GRID_PAGE_SIZE"].IsNotNull())
                {
                    treeListFeature.PageSize = Convert.ToInt32(HttpContext.Current.Session["GRID_PAGE_SIZE"]);
                }
                Presenter.OnViewLoaded();
                base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_FEATURES));

                // Code to refresh menu bar.
                hfldUpdStat.Value = String.Empty;
                WclResourceManager rsm = WclResourceManager.GetCurrent(this.Page);

                if (!rsm.IsNull())
                {
                    rsm.CurrentScriptWriter.IDBag.AddItem("menu_trigger", hfldUpdStat.ClientID);
                }

                lblSuccess.Visible = false;
                lblSuccess.Text = String.Empty;

                //Call Presenter Method to check if selected business channel is AMS or Not//
                Presenter.IsBkgBusinessChannel();
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

        #region TreeList Feature Events

        /// <summary>
        /// Retrieves a list of all features.
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
        /// Performs an insert operation for feature.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListFeature_InsertCommand(object sender, TreeListCommandEventArgs e)
        {
            try
            {

                TreeListEditFormInsertItem treeListEditFormInsertItems = e.Item as TreeListEditFormInsertItem;
                CurrentViewContext.ErrorMessage = (treeListEditFormInsertItems.FindControl("lblErrorMessage") as Label).Text;
                CurrentViewContext.ViewContract.Name = (treeListEditFormInsertItems.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.IconImageName = (treeListEditFormInsertItems.FindControl("txtIconImageName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.UIControlId = String.Empty;
                CurrentViewContext.ViewContract.NavigationUrl = String.Empty;

                if (!(treeListEditFormInsertItems.FindControl("chkIsParent") as CheckBox).Checked)
                {
                    CurrentViewContext.ViewContract.UIControlId = Request.Params[(treeListEditFormInsertItems.FindControl("txtUIControlID") as WclTextBox).UniqueID];
                    CurrentViewContext.ViewContract.NavigationUrl = Request.Params[(treeListEditFormInsertItems.FindControl("txtNavigationURL") as WclTextBox).UniqueID];

                    String ucPath = CurrentViewContext.ViewContract.NavigationUrl.Substring(0, CurrentViewContext.ViewContract.NavigationUrl.LastIndexOf('/') + AppConsts.ONE) + CurrentViewContext.ViewContract.UIControlId;

                    // var userControl = (LoadControl(ucPath) as BaseUserControl);


                    //Commented- VV 08/05/2014
                    //if ((LoadControl(ucPath) as BaseUserControl).IsNotNull())
                    //{
                    //    var actionCollection = userControl.ActionCollection;
                    //    if (actionCollection.IsNotNull())
                    //    {
                    //        CurrentViewContext.ViewContract.ActionName = actionCollection.Select(cond => cond.Key).ToList();
                    //    }
                    //}

                    //GetFeatureAction(ucPath);
                    string spliter = string.Empty;
                    if (!(treeListEditFormInsertItems.FindControl("chkIsReportFeature") as CheckBox).Checked)
                    {
                        RadioButtonList rblFeatureAreaTypeCode = treeListEditFormInsertItems.FindControl("rblFeatureAreaType") as RadioButtonList;
                        if (rblFeatureAreaTypeCode.IsNullOrEmpty() 
                               || (!rblFeatureAreaTypeCode.IsNullOrEmpty() && !rblFeatureAreaTypeCode.SelectedValue.IsNullOrEmpty() 
                                   && String.Compare(rblFeatureAreaTypeCode.SelectedValue,FeatureAreaType.COMPLIO.GetStringValue()) == AppConsts.NONE))
                        {
                            GetFeatureAction(ucPath);
                        }
                        Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                     {
                                                                         AppConsts.UCID ,
                                                                         CurrentViewContext.ViewContract.UIControlId +
                                                                         Guid.NewGuid()
                                                                         }
                                                                 };
                        CurrentViewContext.ViewContract.NavigationUrl = CurrentViewContext.ViewContract.NavigationUrl.Replace(CurrentViewContext.ViewContract.UIControlId, queryString.ToEncryptedQueryString());
                    }
                    else
                    {
                        CurrentViewContext.ViewContract.IsReportFeature = true;
                        Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                     {
                                                                         AppConsts.UCID ,
                                                                         CurrentViewContext.ViewContract.UIControlId + "|" +
                                                                         Guid.NewGuid()
                                                                         }
                                                                 };
                        CurrentViewContext.ViewContract.NavigationUrl = CurrentViewContext.ViewContract.NavigationUrl.Replace(CurrentViewContext.ViewContract.UIControlId, queryString.ToEncryptedQueryString());
                    }


                    //End


                    // Added to capture the base physical folder path to be able to get the Dashboard templates
                    if ((treeListEditFormInsertItems.FindControl("chkIsDashboard") as CheckBox).Checked)
                    {
                        string navURL = CurrentViewContext.ViewContract.NavigationUrl;
                        CurrentViewContext.ViewContract.DashboardTemplatesPath =
                            Server.MapPath(navURL.Substring(0, navURL.LastIndexOf("/")));
                    }

                    //Dictionary<String, String> queryString = new Dictionary<String, String>
                    //                                             {
                    //                                                 {
                    //                                                     AppConsts.UCID ,
                    //                                                     CurrentViewContext.ViewContract.UIControlId +
                    //                                                     Guid.NewGuid()
                    //                                                     }
                    //                                             };

                    //CurrentViewContext.ViewContract.NavigationUrl = CurrentViewContext.ViewContract.NavigationUrl.Replace(CurrentViewContext.ViewContract.UIControlId, queryString.ToEncryptedQueryString());
                }

                CurrentViewContext.ViewContract.Description = (treeListEditFormInsertItems.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.DisplayOrder = (treeListEditFormInsertItems.FindControl("txtDisplayOrder") as WclNumericTextBox).Text.Trim().Equals(String.Empty) ? AppConsts.NONE :
                                                                                                                                                   Convert.ToInt32((treeListEditFormInsertItems.FindControl("txtDisplayOrder") as WclNumericTextBox).Text.Trim());
                //Onsite Changes for Dashboard - Start
                CurrentViewContext.ViewContract.IsDashboardFeature = (treeListEditFormInsertItems.FindControl("chkIsDashboard") as CheckBox).Checked;
                if (CurrentViewContext.ViewContract.IsDashboardFeature)
                    CurrentViewContext.ViewContract.ForExternalUser = (treeListEditFormInsertItems.FindControl("chkIsExternal") as CheckBox).Checked;
                //End

                if (!treeListEditFormInsertItems.ParentItem.IsNull())
                {
                    CurrentViewContext.ViewContract.ParentProductFeatureId = Convert.ToInt32(treeListEditFormInsertItems.ParentItem.GetDataKeyValue("ProductFeatureID"));
                }

                //Admin Entry Portal 
                RadioButtonList rblFeatureAreaType = treeListEditFormInsertItems.FindControl("rblFeatureAreaType") as RadioButtonList;
                if (!rblFeatureAreaType.IsNullOrEmpty() && !rblFeatureAreaType.SelectedValue.IsNullOrEmpty() && !CurrentViewContext.lstFeatureAreaType.IsNullOrEmpty() && CurrentViewContext.IsBkgBusinessChannel)
                {
                    CurrentViewContext.ViewContract.FeatureAreaTypeID = CurrentViewContext.lstFeatureAreaType.Where(con => con.FAT_Code == rblFeatureAreaType.SelectedValue).FirstOrDefault().FAT_ID;
                }

                Presenter.AddProductFeature();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    (treeListEditFormInsertItems.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                }
                else
                {
                    e.Canceled = false;
                }

                hfldUpdStat.Value = AppConsts.TRUE;
                treeListFeature.EditIndexes.Clear();

                if (treeListEditFormInsertItems.ParentItem.IsNull())
                {
                    // After creating a new feature, the page on which the new feature is added should get displayed.
                    treeListFeature.CurrentPageIndex = treeListFeature.PageCount;
                }

                lblSuccess.Visible = true;
                lblSuccess.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
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
        /// Performs an update operation for feature.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListFeature_UpdateCommand(object sender, TreeListCommandEventArgs e)
        {
            try
            {
                TreeListEditFormItem treeListEditFormInsertItems = e.Item as TreeListEditFormItem;
                CurrentViewContext.ErrorMessage = (treeListEditFormInsertItems.FindControl("lblErrorMessage") as Label).Text;
                CurrentViewContext.ViewContract.Name = (treeListEditFormInsertItems.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.IconImageName = (treeListEditFormInsertItems.FindControl("txtIconImageName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.UIControlId = String.Empty;
                CurrentViewContext.ViewContract.NavigationUrl = String.Empty;

                if (!(treeListEditFormInsertItems.FindControl("chkIsParent") as CheckBox).Checked)
                {
                    CurrentViewContext.ViewContract.UIControlId = Request.Params[(treeListEditFormInsertItems.FindControl("txtUIControlID") as WclTextBox).UniqueID];
                    CurrentViewContext.ViewContract.NavigationUrl = Request.Params[(treeListEditFormInsertItems.FindControl("txtNavigationURL") as WclTextBox).UniqueID];

                    // Added to capture the base physical folder path to be able to get the Dashboard templates
                    //if ((treeListEditFormInsertItems.FindControl("chkIsDashboard") as CheckBox).Checked)
                    //{
                    //    string navURL = CurrentViewContext.ViewContract.NavigationUrl;
                    //    CurrentViewContext.ViewContract.DashboardTemplatesPath =
                    //        Server.MapPath(navURL.Substring(0, navURL.LastIndexOf("/")));
                    //}

                    string spliter = string.Empty;
                    if (!(treeListEditFormInsertItems.FindControl("chkIsReportFeature") as CheckBox).Checked)
                    {
                        CurrentViewContext.ViewContract.IsReportFeature = true;
                        Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                     {
                                                                         AppConsts.UCID,
                                                                         CurrentViewContext.ViewContract.UIControlId +
                                                                         Guid.NewGuid()
                                                                         }
                                                                 };
                        CurrentViewContext.ViewContract.NavigationUrl = CurrentViewContext.ViewContract.NavigationUrl.Replace(CurrentViewContext.ViewContract.UIControlId, queryString.ToEncryptedQueryString());
                    }
                    else
                    {
                        CurrentViewContext.ViewContract.IsReportFeature = true;
                        Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                     {
                                                                         AppConsts.UCID,
                                                                         CurrentViewContext.ViewContract.UIControlId + "|" +
                                                                         Guid.NewGuid()
                                                                         }
                                                                 };
                        CurrentViewContext.ViewContract.NavigationUrl = CurrentViewContext.ViewContract.NavigationUrl.Replace(CurrentViewContext.ViewContract.UIControlId, queryString.ToEncryptedQueryString());
                    }
                }

                CurrentViewContext.ViewContract.Description = (treeListEditFormInsertItems.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.DisplayOrder = (treeListEditFormInsertItems.FindControl("txtDisplayOrder") as WclNumericTextBox).Text.Trim().Equals(String.Empty) ? AppConsts.NONE :
                                                                                                                                                   Convert.ToInt32((treeListEditFormInsertItems.FindControl("txtDisplayOrder") as WclNumericTextBox).Text.Trim());
                //Onsite Changes for Dashboard - Start
                CurrentViewContext.ViewContract.IsDashboardFeature = (treeListEditFormInsertItems.FindControl("chkIsDashboard") as CheckBox).Checked;
                if (CurrentViewContext.ViewContract.IsDashboardFeature)
                    CurrentViewContext.ViewContract.ForExternalUser = (treeListEditFormInsertItems.FindControl("chkIsExternal") as CheckBox).Checked;
                CurrentViewContext.ViewContract.ProductFeatureId = Convert.ToInt32(treeListEditFormInsertItems.ParentItem.GetDataKeyValue("ProductFeatureID"));
                //End

                RadioButtonList rblFeatureAreaType = treeListEditFormInsertItems.FindControl("rblFeatureAreaType") as RadioButtonList;
                if (!rblFeatureAreaType.IsNullOrEmpty() && !rblFeatureAreaType.SelectedValue.IsNullOrEmpty() && !CurrentViewContext.lstFeatureAreaType.IsNullOrEmpty() && CurrentViewContext.IsBkgBusinessChannel)
                {
                    CurrentViewContext.ViewContract.FeatureAreaTypeID = CurrentViewContext.lstFeatureAreaType.Where(con => con.FAT_Code == rblFeatureAreaType.SelectedValue).FirstOrDefault().FAT_ID;
                }

                Presenter.UpdateProductFeature();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    (treeListEditFormInsertItems.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                }
                else
                {
                    e.Canceled = false;
                }

                hfldUpdStat.Value = AppConsts.TRUE;
                lblSuccess.Visible = true;
                lblSuccess.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
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
        /// Performs a delete operation for feature.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListFeature_DeleteCommand(object sender, TreeListCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.ProductFeatureId = Convert.ToInt32(((TreeListDataItem)(e.Item)).GetDataKeyValue("ProductFeatureID"));
                Presenter.DeleteProductFeature();
                hfldUpdStat.Value = AppConsts.TRUE;
                lblSuccess.Visible = true;
                lblSuccess.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
                treeListFeature.InsertIndexes.Clear();
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
                if (e.Item.ItemType.Equals(TreeListItemType.AlternatingItem) || e.Item.ItemType.Equals(TreeListItemType.Item))
                {
                    (e.Item as TreeListDataItem)["UIControlID"].Text = FormatControlName((e.Item as TreeListDataItem)["UIControlID"].Text);
                }

                if (e.Item.ItemType.Equals(TreeListItemType.EditItem))
                {
                    (e.Item as TreeListDataItem)["UIControlID"].Text = FormatControlName((e.Item as TreeListDataItem)["UIControlID"].Text);
                    CurrentViewContext.ViewContract.CurrentWebPageName = (e.Item as TreeListDataItem)["UIControlID"].Text;
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
        /// Event handler. Called by treeListFeature for item created events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void treeListFeature_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        {
            try
            {
                if ((e.Item.ItemType.Equals(TreeListItemType.EditFormItem)))
                {
                    TreeListItem editform = (TreeListItem)e.Item;
                    WclButton selectButton = (WclButton)editform.FindControl("btnUIControlID");
                    WclTextBox uiControlId = (WclTextBox)editform.FindControl("txtUIControlID");
                    CheckBox chkIsReportFeature = (CheckBox)editform.FindControl("chkIsReportFeature");
                    uiControlId.ReadOnly = true;
                    WclTextBox txtNavigationUrl = (WclTextBox)editform.FindControl("txtNavigationURL");
                    RadioButtonList rblFeatureAreaType = (RadioButtonList)editform.FindControl("rblFeatureAreaType"); // Admin entry Portal
                    ProductFeature dataItem = ((Telerik.Web.UI.TreeListEditableItem)editform).DataItem as ProductFeature;
                    txtNavigationUrl.ReadOnly = true;

                    if (dataItem != null)
                    {
                        chkIsReportFeature.Checked = dataItem.IsReportFeature.HasValue ? dataItem.IsReportFeature.Value : false;
                        string[] viewandSheet = dataItem.UIControlID.Split(';');
                        if (dataItem.IsReportFeature.HasValue)
                            selectButton.Attributes.Add("onClick", "openWinReport('" + uiControlId.ClientID + "','" + txtNavigationUrl.ClientID + "','" + viewandSheet[0] + "','" + viewandSheet[1] + "'); return false;");
                        //else
                        //    selectButton.Attributes.Add("onClick", "openWin('" + uiControlId.ClientID + "','" + txtNavigationUrl.ClientID + "','" + "'); return false;");

                        //Above code commented in Admin Entry Portal// Below code is added//
                        else
                        {
                            if (!dataItem.lkpBusinessChannelType.IsNullOrEmpty() && !dataItem.BusinessChannelTypeID.IsNullOrEmpty() && dataItem.lkpBusinessChannelType.Code == BusinessChannelType.AMS.GetStringValue()
                                && !dataItem.lkpFeatureAreaType.IsNullOrEmpty() && !dataItem.FeatureAreaTypeID.IsNullOrEmpty() && dataItem.lkpFeatureAreaType.FAT_Code == FeatureAreaType.ADMINENTRYPORTAL.GetStringValue())
                            {
                                selectButton.Attributes.Add("onClick", "openComponentWin('" + uiControlId.ClientID + "','" + txtNavigationUrl.ClientID + "','" + "'); return false;");
                            }
                            else
                            {
                                selectButton.Attributes.Add("onClick", "openWin('" + uiControlId.ClientID + "','" + txtNavigationUrl.ClientID + "','" + "'); return false;");
                            }
                        }
                    }
                    else
                    {
                        //    selectButton.Attributes.Add("onClick", "openWin('" + uiControlId.ClientID + "','" + txtNavigationUrl.ClientID + "','" + "'); return false;");
                        //Above code commented in Admin Entry Portal// Below code is added//
                        if (CurrentViewContext.IsBkgBusinessChannel && !rblFeatureAreaType.SelectedValue.IsNullOrEmpty() && rblFeatureAreaType.SelectedValue == FeatureAreaType.ADMINENTRYPORTAL.GetStringValue())
                        {
                            selectButton.Attributes.Add("onClick", "openComponentWin('" + uiControlId.ClientID + "','" + txtNavigationUrl.ClientID + "','" + "'); return false;");
                        }
                        else
                        {
                            selectButton.Attributes.Add("onClick", "openWin('" + uiControlId.ClientID + "','" + txtNavigationUrl.ClientID + "','" + "'); return false;");
                        }
                    }

                    //Calling to Manage Feature Area type Div Visibility//
                    ManageFeatureAreaTypeDiv(editform);
                }

                if (e.Item.ItemType.Equals(TreeListItemType.Item) || e.Item.ItemType.Equals(TreeListItemType.AlternatingItem) || e.Item.ItemType.Equals(TreeListItemType.EditItem))
                {
                    TreeListDataItem item = (TreeListDataItem)e.Item;
                    Int32 itemIndex = item.DataItemIndex % treeListFeature.PageSize;

                    // To remove edit button from Security Feature.(Parent Menu).
                    if (item.OwnerTreeList.DataKeyValues[itemIndex]["ProductFeatureID"].Equals(AppConsts.ONE))
                    {
                        (e.Item as TreeListDataItem)["EditCommandColumn"].Controls[AppConsts.TWO].Visible = false;
                    }

                    // To remove delete icon for all the System Features based on IsSystem value.
                    if (Convert.ToInt32(((TreeListDataItem)(e.Item)).GetDataKeyValue("IsSystem")).Equals(AppConsts.ONE))
                    {
                        (e.Item as TreeListDataItem)["deleteButtonColumn"].Controls[AppConsts.NONE].Visible = false;
                        (e.Item as TreeListDataItem)["deleteButtonColumn"].Text = SysXUtils.GetMessage(ResourceConst.SPACE);
                    }
                    else
                    {
                        Button button = e.Item.Cells[e.Item.Cells.Count - AppConsts.ONE].Controls[AppConsts.NONE] as Button;

                        if (!button.IsNull())
                        {
                            button.Attributes.Add("onclick", "return confirm('This will delete feature association from Business Channel, Role and Product wherever applicable. Are you sure to continue?')");
                        }
                    }

                }
                //Customize Page Size of treelist. 
                TreeListPagerItem pagerItem = e.Item as TreeListPagerItem;
                if (pagerItem != null)
                {
                    RadComboBox combo = e.Item.FindControl("PageSizeComboBox") as RadComboBox;
                    combo.Items.Clear();
                    IList<Int32> defaultPageSizes = new List<Int32>();
                    defaultPageSizes.Add(10);
                    defaultPageSizes.Add(25);
                    defaultPageSizes.Add(50);
                    defaultPageSizes.Add(100);
                    foreach (Int32 size in defaultPageSizes)
                    {
                        // RadComboBoxItem item = new RadComboBoxItem(size.ToString(), size.ToString());
                        combo.Items.Add(new RadComboBoxItem(Convert.ToString(size), Convert.ToString(size)));
                    }
                    RadComboBoxItem comboBoxItem = combo.Items.FindItemByValue(pagerItem.OwnerTreeList.PageSize.ToString());
                    if (!comboBoxItem.IsNullOrEmpty())
                    {
                        comboBoxItem.Selected = true;
                    }
                    if (Session["GRID_PAGE_SIZE"].IsNull())
                    {
                        treeListFeature.PageSize = AppConsts.DEFAULT_PAZE_SIZE;
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


        protected void treeListFeature_PageSizeChanged(object sender, TreeListPageSizeChangedEventArgs e)
        {
            HttpContext.Current.Session["GRID_PAGE_SIZE"] = e.NewPageSize;
        }
        #endregion

        #endregion

        #region Method

        #region Protected Method

        /// <summary>
        /// Sets a checkbox.
        /// </summary>
        /// <param name="str"> Value of str.</param>
        /// <param name="name">Value for name.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        protected Boolean SetCheckbox(String str, String name)
        {
            Boolean flag = false;

            try
            {
                if (str.Length.Equals(AppConsts.NONE))
                {
                    flag = true;
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

            return flag;
        }

        #endregion

        #region Private Method

        protected void chkIsParent_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox sysXCheckBox = sender as CheckBox;
            RequiredFieldValidator rfvUIControlId = ((Control)sender).Parent.FindControl("rfvUIControlID") as RequiredFieldValidator;
            WclTextBox txtUIControlId = ((Control)sender).Parent.FindControl("txtUIControlID") as WclTextBox;
            WclTextBox txtNavigationURL = ((Control)sender).Parent.FindControl("txtNavigationURL") as WclTextBox;
            CheckBox chkIsReportFeature = ((Control)sender).Parent.FindControl("chkIsReportFeature") as CheckBox;

            if (sysXCheckBox.Checked)
            {
                txtUIControlId.Text = String.Empty;
                txtNavigationURL.Text = String.Empty;
                rfvUIControlId.Enabled = false;
                chkIsReportFeature.Checked = false;
            }
            else
            {
                rfvUIControlId.Enabled = true;
            }
        }
        protected void chkIsDashboard_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox sysXCheckBox = sender as CheckBox;
            CheckBox chkExternal = ((Control)sender).Parent.FindControl("chkIsExternal") as CheckBox;
            Control dvIsExternal = ((Control)sender).Parent.FindControl("dvIsExternal") as Control;
            if (sysXCheckBox.Checked)
            {
                dvIsExternal.Visible = true;
                chkExternal.Visible = true;
            }
            else
            {
                dvIsExternal.Visible = false;
                chkExternal.Visible = false;
            }
        }
        #endregion

        protected void cmbBusinessChannel_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            treeListFeature.Rebind();
        }

        #endregion

        #region AMS

        string IManageFeatureView.SelectedBusinessChannel
        {
            get
            {
                return cmbBusinessChannel.SelectedValue;
            }
            set
            {
                cmbBusinessChannel.SelectedValue = value;
            }
        }

        List<Entity.lkpBusinessChannelType> IManageFeatureView.BusinessChannels
        {
            set
            {
                cmbBusinessChannel.DataSource = value;
                cmbBusinessChannel.DataBind();
            }

        }


        private void GetFeatureAction(String parentUcPath)
        {
            var userControl = (LoadControl(parentUcPath) as BaseUserControl);

            if (!userControl.IsNull())
            {
                //First Step Read the List of Action from UserControl
                List<ClsFeatureAction> lstFeatureAction = new List<ClsFeatureAction>();

                if (!userControl.ActionCollection.IsNull())
                {
                    lstFeatureAction.AddRange(userControl.ActionCollection);
                }


                if (!userControl.ChildScreenPathCollection.IsNull())
                {
                    foreach (String childControlPath in userControl.ChildScreenPathCollection)
                    {
                        userControl = (LoadControl(childControlPath) as BaseUserControl);
                        if (!userControl.IsNull() && !userControl.ActionCollection.IsNull())
                        {
                            lstFeatureAction.AddRange(userControl.ActionCollection);
                        }
                    }


                }

                CurrentViewContext.ViewContract.ActionCollection = lstFeatureAction;
            }

        }

        #endregion

        protected void chkIsReportFeature_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox sysXCheckBox = sender as CheckBox;
            Label lblWebPageName = ((Control)sender).Parent.FindControl("lblUIControlID") as Label;

            RequiredFieldValidator rfvUIControlId = ((Control)sender).Parent.FindControl("rfvUIControlID") as RequiredFieldValidator;
            WclTextBox txtUIControlId = ((Control)sender).Parent.FindControl("txtUIControlID") as WclTextBox;
            WclTextBox txtNavigationURL = ((Control)sender).Parent.FindControl("txtNavigationURL") as WclTextBox;
            WclButton btnUIControlID = ((Control)sender).Parent.FindControl("btnUIControlID") as WclButton;
            CheckBox chkIsParent = ((Control)sender).Parent.FindControl("chkIsParent") as CheckBox;


            if (sysXCheckBox.Checked && sysXCheckBox != null)
            {
                txtUIControlId.Text = String.Empty;
                txtNavigationURL.Text = String.Empty;
                rfvUIControlId.Enabled = true;
                chkIsParent.Checked = false;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(System.Web.UI.Page), "chkIsParentScript", "showReqdSpan();", true);

                btnUIControlID.Attributes.Add("onClick", "openWinReport('" + txtUIControlId.ClientID + "','" + txtNavigationURL.ClientID + "','" + string.Empty + "','" + string.Empty + "'); return false;");
            }
            else
            {
                //chkIsParent.Checked = true;
                rfvUIControlId.Enabled = true;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(System.Web.UI.Page), "chkIsParentScript", "showReqdSpan();", true);
                btnUIControlID.Attributes.Add("onClick", "openWin('" + txtUIControlId.ClientID + "','" + txtNavigationURL.ClientID + "','" + "'); return false;");
            }
        }

        #region Admin Entry Portal

        #region Properties

        Boolean IManageFeatureView.IsBkgBusinessChannel
        {
            get
            {
                if (!ViewState["IsBkgBusinessChannel"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsBkgBusinessChannel"]);
                return false;
            }
            set
            {
                ViewState["IsBkgBusinessChannel"] = value;
            }
        }

        List<lkpFeatureAreaType> IManageFeatureView.lstFeatureAreaType
        {
            get
            {
                if (!ViewState["lstFeatureAreaType"].IsNullOrEmpty())
                    return (List<lkpFeatureAreaType>)(ViewState["lstFeatureAreaType"]);
                return new List<lkpFeatureAreaType>();
            }
            set
            {
                ViewState["lstFeatureAreaType"] = value;
            }
        }

        #endregion

        #region Method

        #region Private Method

        private void ManageFeatureAreaTypeDiv(TreeListItem formItem)
        {
            if (!formItem.IsNullOrEmpty())
            {
                HtmlGenericControl dvFeatureAreaType = formItem.FindControl("dvFeatureAreaType") as HtmlGenericControl;
                if (!dvFeatureAreaType.IsNullOrEmpty())
                {
                    if (CurrentViewContext.IsBkgBusinessChannel)
                    {
                        dvFeatureAreaType.Style.Add("display", "block");
                        BindFeatureAreaType(formItem);
                    }
                    else
                    {
                        dvFeatureAreaType.Style.Add("display", "none");
                    }
                }
            }
        }

        private void BindFeatureAreaType(TreeListItem formItem)
        {
            if (!formItem.IsNullOrEmpty())
            {
                ProductFeature dataItem = ((Telerik.Web.UI.TreeListEditableItem)formItem).DataItem as ProductFeature;

                RadioButtonList rblFeatureAreaType = formItem.FindControl("rblFeatureAreaType") as RadioButtonList;
                Presenter.GetFeatureAreaType();
                rblFeatureAreaType.DataSource = CurrentViewContext.lstFeatureAreaType;
                rblFeatureAreaType.DataBind();
                if (dataItem != null && !dataItem.FeatureAreaTypeID.IsNullOrEmpty() && dataItem.FeatureAreaTypeID > AppConsts.NONE)
                    rblFeatureAreaType.SelectedValue = CurrentViewContext.lstFeatureAreaType.Where(con => con.FAT_ID == dataItem.FeatureAreaTypeID).FirstOrDefault().FAT_Code;
                else
                    rblFeatureAreaType.SelectedValue = FeatureAreaType.COMPLIO.GetStringValue();
            }
        }

        #endregion

        #endregion

        #region Events

        protected void rblFeatureAreaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButtonList rblFeatureAreaType = sender as RadioButtonList;
                //TreeListItem editform = (TreeListItem)rblFeatureAreaType.Parent;
                Control editform = ((Control)sender).Parent;

                CheckBox chkIsParent = editform.FindControl("chkIsParent") as CheckBox;
                CheckBox chkIsReportFeature = editform.FindControl("chkIsReportFeature") as CheckBox;
                WclTextBox txtUIControlID = editform.FindControl("txtUIControlID") as WclTextBox;
                WclButton btnUIControlID = editform.FindControl("btnUIControlID") as WclButton;
                RequiredFieldValidator rfvUIControlId = ((Control)sender).Parent.FindControl("rfvUIControlID") as RequiredFieldValidator;
                WclTextBox txtNavigationURL = editform.FindControl("txtNavigationURL") as WclTextBox;
                CheckBox chkIsDashboard = editform.FindControl("chkIsDashboard") as CheckBox;
                Control dvIsExternal = editform.FindControl("dvIsExternal") as Control;
                CheckBox chkExternal = editform.FindControl("chkIsExternal") as CheckBox;

                chkIsParent.Checked = true;
                txtUIControlID.Text = String.Empty;
                rfvUIControlId.Enabled = false;
                txtNavigationURL.Text = String.Empty;
                chkIsDashboard.Checked = false;
                dvIsExternal.Visible = false;
                chkExternal.Checked = false;
                chkExternal.Visible = false;
                chkIsReportFeature.Checked = false;

                if (rblFeatureAreaType.SelectedValue == FeatureAreaType.ADMINENTRYPORTAL.GetStringValue())
                {
                    //set btnUIControlID CLICK event here as new method which will open new aspx page which will load react features xml.
                    chkIsReportFeature.Enabled = false;
                    btnUIControlID.Attributes.Add("onClick", "openComponentWin('" + txtUIControlID.ClientID + "','" + txtNavigationURL.ClientID + "','" + "'); return false;");
                }
                else
                {
                    //set btnUIControlID CLICK event here as openWin()
                    chkIsReportFeature.Enabled = true;
                    btnUIControlID.Attributes.Add("onClick", "openWin('" + txtUIControlID.ClientID + "','" + txtNavigationURL.ClientID + "','" + "'); return false;");
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

        #endregion
    }
}