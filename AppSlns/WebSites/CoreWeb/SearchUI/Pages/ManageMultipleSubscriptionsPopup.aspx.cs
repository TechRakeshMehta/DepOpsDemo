using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.SearchUI.Views;
using CoreWeb.Shell;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.SearchUI.Pages.Views
{
    public partial class ManageMultipleSubscriptions : BaseWebPage, IManageMultipleSubscriptionsPopup
    {
        #region VARIABLES

        #region PUBLIC VARIABLES
        #endregion

        #region PRIVATE VARIABLES
        private Dictionary<String, List<Int32>> _dicSubscriptions = new Dictionary<String, List<Int32>>();
        private ManageMultipleSubscriptionsPopupPresenter _presenter = new ManageMultipleSubscriptionsPopupPresenter();
        private CustomPagingArgsContract _gridCustomPaging = null;
        private String _viewType = "";
        #endregion

        #endregion

        #region PROPERTIES

        #region PUBLIC PROPERTIES

        public ManageMultipleSubscriptionsPopupPresenter Presenter
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

        //List of PackageSusbcriptionIDs
        public List<Int32> MultpleSubscriptionIDs
        {
            get
            {
                //ViewState["MultpleSubscriptionIDs"] = _dicSubscriptions.GetValue("lstMultipleSubscriptions");

                if (ViewState["MultpleSubscriptionIDs"] != null)
                {
                    return (List<Int32>)ViewState["MultpleSubscriptionIDs"];
                }
                else
                    return new List<Int32>();
            }

            set
            {
                ViewState["MultpleSubscriptionIDs"] = value;
            }
        }

        //List of PackageSusbcriptionIDs
        public List<Int32> SingleSubscriptionIDs
        {
            get
            {
                //ViewState["SingleSubscriptionIDs"] = _dicSubscriptions.GetValue("lstSingleSubscriptions");

                if (ViewState["SingleSubscriptionIDs"] != null)
                {
                    return (List<Int32>)ViewState["SingleSubscriptionIDs"];
                }
                else
                    return new List<Int32>();
            }
            set
            {
                ViewState["SingleSubscriptionIDs"] = value;
            }
        }

        //Get Selected TenantID
        public Int32 SelectedTenantID
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedTenantID"]);
            }
            set
            {
                ViewState["SelectedTenantID"] = value;
            }
        }

        //Get-set Current User ID
        public Int32 CurrentLoggedInUserID
        {
            get
            {
                return Convert.ToInt32(ViewState["CurrentLoggedInUserID"]);
            }
            set
            {
                ViewState["CurrentLoggedInUserID"] = value;
            }
        }

        //Get-Set data in lstMultipleSubscriptionsData
        public List<ManageMultipleSubscriptionContract> lstMultipleSubscriptionsData
        {
            get;
            set;
        }

        public IManageMultipleSubscriptionsPopup CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public String ErrorMessage
        {
            get
            {
                return ViewState["ErrorMessage"].ToString();
            }
            set
            {
                ViewState["ErrorMessage"] = value;
            }
        }

        public List<Int32> SelectedSubscriptions
        {
            get
            {
                List<Int32> lstSelectedSubscriptions = new List<Int32>();
                if (ViewState["SelectedSubscriptions"].IsNotNull())
                {
                    lstSelectedSubscriptions = (List<Int32>)ViewState["SelectedSubscriptions"];
                }
                return lstSelectedSubscriptions;
            }
            set
            {
                ViewState["SelectedSubscriptions"] = value;
            }
        }


        #endregion

        #region PRIVATE PROPERTIES
        #endregion

        #endregion

        #region EVENTS

        #region PAGE EVENTS

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!Request[AppConsts.SCREEN_NAME_QUERY_STRING].IsNullOrEmpty()
                            && (Request[AppConsts.SCREEN_NAME_QUERY_STRING].ToLower() == AppConsts.PORTFOLIO_SEARCH.ToLower()))
                    {
                        _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                        _dicSubscriptions = (Dictionary<String, List<Int32>>)Session["SubscriptionIDs"];
                        CurrentViewContext.SingleSubscriptionIDs = _dicSubscriptions.GetValue("lstSingleSubscriptions");
                        CurrentViewContext.MultpleSubscriptionIDs = _dicSubscriptions.GetValue("lstMultipleSubscriptions");
                        if (!Request["TenantID"].IsNullOrEmpty())
                        {
                            CurrentViewContext.SelectedTenantID = Convert.ToInt32(Request["TenantID"]);
                        }
                        if (!Request["CurrentUserID"].IsNullOrEmpty())
                        {
                            CurrentViewContext.CurrentLoggedInUserID = Convert.ToInt32(Request["CurrentUserID"]);
                        }
                    }
                }
            }
            catch (SysXException ex)
            {
                LogError(ex.Message, ex);
            }
            catch (System.Exception ex)
            {
                LogError(ex.Message, ex);
            }
        }

        #endregion

        #region GRID EVENTS

        #endregion

        #region BUTTON EVENTS
        protected void cmdBarMultipleSubscriptions_SubmitClick(object sender, EventArgs e)
        {
            if (CurrentViewContext.SelectedSubscriptions.IsNotNull() && CurrentViewContext.SelectedSubscriptions.Count > 0)
            {
                if (CurrentViewContext.SingleSubscriptionIDs.IsNotNull() && CurrentViewContext.SingleSubscriptionIDs.Count > 0)
                {
                    CurrentViewContext.SelectedSubscriptions.AddRange(CurrentViewContext.SingleSubscriptionIDs);
                }
                String result = Presenter.ArchieveSubscriptions();
                if(result == "false")
                {
                    base.ShowErrorMessage("Subscriptions are not archived sucessfully. Please try again.");
                }
                else
                {
                    //base.ShowSuccessMessage("Subscription(s) archived successfully.");
                    //CurrentViewContext.MultpleSubscriptionIDs = CurrentViewContext.MultpleSubscriptionIDs.
                    //                        Where(obj => (!(CurrentViewContext.SelectedSubscriptions.Contains(obj)))).ToList();
                    //CurrentViewContext.SelectedSubscriptions = null;
                    //grdArchiveMultipleSubscriptions.Rebind();
                    
                    //TO_DO 
                    
                    //code to close popup and rebind Parent grid.
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RedirectToParent();", true);
                    Session["SubscriptionIDs"] = null;
                }
            }
            else
            {
                if (CurrentViewContext.SelectedSubscriptions.IsNotNull() && !CurrentViewContext.SelectedSubscriptions.Any())
                {
                    base.ShowErrorInfoMessage("Please select subscription(s) for archiving.");
                }
            }
        }

        //protected void cmdBarMultipleSubscriptions_CancelClick(object sender, EventArgs e)
        //{
        //    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ClosePopup();", true);
        //}
        #endregion

        protected void grdArchiveMultipleSubscriptions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            Presenter.GetMultipleSubscriptionDataForPopup();
            grdArchiveMultipleSubscriptions.DataSource = CurrentViewContext.lstMultipleSubscriptionsData;
        }

        #endregion

        protected void grdArchiveMultipleSubscriptions_ItemDataBound(object sender, GridItemEventArgs e)
        {
            //To select checkboxes
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {

                Int32 itemDataId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PackageSubscriptionID"]);
                //Boolean isMapped = Convert.ToBoolean((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["IsUserGroupMatching"].ToString());
                if (Convert.ToInt32(itemDataId) != 0)
                {
                   List<Int32> selectedItems = CurrentViewContext.SelectedSubscriptions;
                    if (selectedItems.IsNotNull() && selectedItems.Count > 0)
                    {
                        if (selectedItems.Contains(itemDataId))
                        {
                            CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectSubscription"));
                            checkBox.Checked = true;
                        }
                    }
                }
                else
                {
                    CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectSubscription"));
                    checkBox.Enabled = false;
                }

                //To Add Tooltip to usergroup column
                GridDataItem dataItem = (GridDataItem)e.Item;
                if (Convert.ToString(dataItem["UserGroup"].Text).Length > 20)
                {
                    dataItem["UserGroup"].ToolTip = dataItem["UserGroup"].Text;
                    dataItem["UserGroup"].Text = (dataItem["UserGroup"].Text).ToString().Substring(0, 20) + "...";
                }
            }
            if (e.Item.ItemType.Equals(GridItemType.Footer))
            {
                Int32 rowCount = grdArchiveMultipleSubscriptions.Items.Count;
                if (rowCount > 0)
                {
                    Int32 checkCount = 0;
                    foreach (GridDataItem item in grdArchiveMultipleSubscriptions.Items)
                    {
                        CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectSubscription"));
                        if (checkBox.Checked)
                        {
                            checkCount++;
                        }
                    }
                    if (rowCount == checkCount)
                    {
                        GridHeaderItem item = grdArchiveMultipleSubscriptions.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                        CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectAll"));
                        checkBox.Checked = true;
                    }
                }
            }
        }

        protected void chkSelectSubscription_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;
                if (checkBox.IsNull())
                {
                    return;
                }
                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                List<Int32> selectedItems = CurrentViewContext.SelectedSubscriptions;
                Int32 subscriptionID = (Int32)dataItem.GetDataKeyValue("PackageSubscriptionID");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectSubscription")).Checked;

                if (isChecked)
                {
                    selectedItems.Add(subscriptionID);

                }
                else
                {
                    if (selectedItems != null)
                    {
                        selectedItems.Remove(subscriptionID);
                    }
                }
                CurrentViewContext.SelectedSubscriptions = selectedItems;
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



        #region METHODS

        #region PUBLIC METHODS
        #endregion

        #region PRIVATE METHODS
        #endregion

        #endregion

    }
}